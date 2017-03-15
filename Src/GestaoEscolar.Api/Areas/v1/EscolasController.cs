using GestaoEscolar.Api.App_Start;
using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
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
    [RoutePrefix("api/v1/Escolas")]
    public class EscolasController : BaseApiController
    {
        /// <summary>
        /// Busca escolas de acordo com a permissão do grupo do usuário do token e o id da diretoria
        /// </summary>
        /// <param name="diretoriaId">(Opcional) Id da diretoria(Guid)</param>
        /// <returns>Retorna uma lista de escolas</returns>
        [Route("Diretoria/{diretoriaId:guid}")]
        [ResponseType(typeof(List<Escola>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetEscolas(Guid diretoriaId)
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
                else {
                    lst = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperior(
                        diretoriaId,
                        __userLogged.Usuario.ent_id,
                        __userLogged.Grupo.gru_id,
                        __userLogged.Usuario.usu_id,
                        (byte)1,
                        true,
                        ApplicationWEB.AppMinutosCacheLongo);
                }

                if (lst.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                            lst.Select(p => new Escola {
                                id = p.esc_id.ToString(),
                                unidadeId = p.uni_id.ToString(),
                                text = p.uni_escolaNome })
                            );
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

    }
}
