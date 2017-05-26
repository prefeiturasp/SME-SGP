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
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/CalendariosAnuais")]
    public class CalendariosAnuaisController : BaseApiController
    {
        /// <summary>
        /// Busca os calendários anuais
        /// </summary>
        /// <returns>Lista de todos os anos letivos distintos</returns>
        [Route("")]
        [ResponseType(typeof(List<int>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetCalendariosAnuais()
        {
            try
            {
                var lst = ACA_CalendarioAnualBO.SelecionarAnosLetivos();

                return Request.CreateResponse(HttpStatusCode.OK,
                        (from DataRow dr in lst.Rows
                         select Convert.ToInt32(dr["cal_ano"]))
                        );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
