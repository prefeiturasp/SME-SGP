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
using System.Web.UI.WebControls;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/TiposDocente")]
    public class TiposDocenteController : BaseApiController
    {
        /// <summary>
        /// Busca uma lista de tipos de docente
        /// </summary>
        /// <returns>Retorna uma lista de tipos de docente</returns>
        [Route("")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetTiposDocente()
        {
            try
            {
                var ret = EnumToJsonObject<EnumTipoDocente>();
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
