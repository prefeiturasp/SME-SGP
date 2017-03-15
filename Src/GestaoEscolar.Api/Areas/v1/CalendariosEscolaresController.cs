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
    [RoutePrefix("api/v1/CalendariosEscolares")]
    public class CalendariosEscolaresController : BaseApiController
    {
        /// <summary>
        /// Busca calendários escolares do período anual de acordo com a entidade do usuário
        /// </summary>
        /// <returns>Retorna uma lista de calendários escolares</returns>
        [Route("Anual")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetCalendarioAnual()
        {
            try
            {
               var lst = ACA_CalendarioAnualBO.SelecionaCalendarioAnual(__userLogged.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

                if (lst.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                            lst.Select(p => new jsonObject { id = p.cal_id.ToString(), text = p.cal_ano_desc })
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
