using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/Diretorias")]
    public class DiretoriasController : BaseApiController
    {
        /// <summary>
        /// Busca diretorias de acordo com a permissão do grupo do usuário do token
        /// </summary>
        /// <returns>Lista de diretorias</returns>
        [Route("")]
        [ResponseType(typeof(Diretoria))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetDiretorias()
        {
            try
            {
                Diretoria diretoria = new Diretoria();
                if ((__userLogged.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) ||
                    !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__userLogged.Usuario.ent_id))
                {
                    diretoria.visible = false;
                }
                else
                {
                    Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__userLogged.Usuario.ent_id);
                    var lst = ESC_UnidadeEscolaBO.GetSelectBy_PesquisaTodos_Cache(
                            tua_id,
                            __userLogged.Usuario.ent_id,
                            ApplicationWEB.AppMinutosCacheLongo);
                    diretoria.visible = true;
                    diretoria.diretorias = lst.Select(p => new jsonObject { id = p.uad_id.ToString(), text = p.uad_nome }).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, diretoria);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
