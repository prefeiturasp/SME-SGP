using GestaoEscolar.Api.App_Start;
using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/Escolas")]
    public class EscolasController : BaseApiController
    {
        /// <summary>
        /// Busca escolas de acordo com a permissão do grupo do usuário do token e o id da diretoria
        /// </summary>
        /// <param name="diretoriaId">(Opcional) Id da diretoria</param>
        /// <returns>Retorna uma lista de escolas</returns>
        [Route("")]
        [ResponseType(typeof(List<Escola>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetEscolas(Guid? diretoriaId = null)
        {
            try
            {
                List<sComboUAEscola> lst = new List<sComboUAEscola>();
                if ((__userLogged.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) ||
                    !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__userLogged.Usuario.ent_id))
                {
                    lst = ESC_UnidadeEscolaBO.SelecionaEscolasControladas(
                            __userLogged.Usuario.ent_id,
                            __userLogged.Grupo.gru_id,
                            __userLogged.Usuario.usu_id,
                            true,
                            ApplicationWEB.AppMinutosCacheLongo);
                }
                else
                {
                    lst = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperior(
                        diretoriaId == null ? Guid.Empty : diretoriaId.Value,
                        __userLogged.Usuario.ent_id,
                        __userLogged.Grupo.gru_id,
                        __userLogged.Usuario.usu_id,
                        (byte)1,
                        true,
                        ApplicationWEB.AppMinutosCacheLongo);
                }

                return Request.CreateResponse(HttpStatusCode.OK,
                        lst.Select(p => new Escola
                        {
                            id = p.esc_id.ToString(),
                            unidadeId = p.uni_id.ToString(),
                            text = p.uni_escolaNome
                        })
                        );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Busca escolas de acordo com a permissão do grupo do usuário do token e o id da diretoria
        /// </summary>
        /// <param name="escolasControladas">(Opcional) True para retornar apenas as escolas controladas. False para retornar as escolas não controlas e Não passar nada retorna todas controladas e não controladas, que o grupo do usuário tem permissão</param>
        /// <returns>Retorna uma lista de escolas</returns>
        [Route("MinhasEscolas")]
        [ResponseType(typeof(List<Escola>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetEscolasByFilter(bool? escolasControladas)
        {
            try
            {
                List<sComboUAEscola> lst = new List<sComboUAEscola>();

                lst = ESC_UnidadeEscolaBO.SelecionaEscolasControladas(__userLogged.Usuario.ent_id
                                                                    , __userLogged.Grupo.gru_id
                                                                    , __userLogged.Usuario.usu_id
                                                                    , esc_controleSistema: escolasControladas
                                                                    , appMinutosCacheLongo: ApplicationWEB.AppMinutosCacheLongo);
                
                return Request.CreateResponse(HttpStatusCode.OK, lst.Select(p => new Escola
                                                                                     {
                                                                                         id = p.esc_id.ToString(),
                                                                                         unidadeId = p.uni_id.ToString(),
                                                                                         text = p.uni_escolaNome
                                                                                     }));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
