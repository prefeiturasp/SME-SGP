using GestaoEscolar.Api.App_Start;
using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/Usuarios")]
    public class UsuariosController : BaseApiController
    {
        /// <summary>
        /// Loga o usuário e retorna um token para acessos posteriores
        /// </summary>
        /// <param name="usuario">Objeto com dados do usuário</param>
        /// <returns>Sucesso: token - Erro: Menssagem de validação</returns>
        [Route("Login")]
        [AuthenticationFilter(false)]
        [ResponseType(typeof(Usuario))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        [HttpPost]
        public HttpResponseMessage PostLogin(Usuario usuario)
        {
            try
            {
                var user = new SYS_Usuario { usu_login = usuario.login, usu_senha = usuario.senha, ent_id = usuario.entidade };
                LoginStatus status = SYS_UsuarioBO.ValidarLogin(user);

                if (status == LoginStatus.Sucesso)
                {
                    if (SYS_UsuarioBO.GetSelectBy_ent_id_usu_login(user))
                    {
                        var grupos = SYS_GrupoBO.GetSelectBySis_idAndUsu_id(user.usu_id, ApplicationWEB.SistemaID);
                        if (grupos.Count > 0)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("login", user.usu_login);
                            dic.Add("entity", user.ent_id);
                            dic.Add("group", grupos.First().gru_id);

                            var jwtKey = System.Configuration.ConfigurationManager.AppSettings["jwtKey"];
                            SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                            PES_Pessoa entityPessoa = new PES_Pessoa { pes_id = user.pes_id };
                            PES_PessoaBO.GetEntity(entityPessoa);

                            UsuarioLogado usuarioLogado = new UsuarioLogado {
                                grupo = grupos.First().gru_nome,
                                nome = (string.IsNullOrEmpty(entityPessoa.pes_nome) ? user.usu_login : entityPessoa.pes_nome),
                                token = JWT.JsonWebToken.Encode(dic, sa.Decrypt(jwtKey), JWT.JwtHashAlgorithm.HS256)
                            };

                            return Request.CreateResponse(HttpStatusCode.OK, usuarioLogado);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Usuário não está vinculado a um grupo");
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Usuário não encontrado");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Usuário ou senha inválidos");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
