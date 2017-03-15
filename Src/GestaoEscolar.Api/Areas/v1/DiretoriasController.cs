using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
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
        [ResponseType(typeof(List<htmlSelect>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetDiretorias()
        {
            try
            {
                Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__userLogged.Usuario.ent_id);
                var lst = ESC_UnidadeEscolaBO.GetSelectBy_PesquisaTodos_Cache(
                        tua_id,
                        __userLogged.Usuario.ent_id,
                        ApplicationWEB.AppMinutosCacheLongo);

                if (lst.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK,
                        lst.Select(p => new htmlSelect { id = p.uad_id.ToString(), parentId = p.uad_idSuperior.ToString(), text = p.uad_nome })
                        );
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

    }
}
