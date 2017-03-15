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
        /// <param name="diretoriaId">Id da diretoria(Guid)</param>
        /// <returns>Retorna uma lista de escolas</returns>
        [Route("Diretoria/{diretoriaId:guid}")]
        [ResponseType(typeof(List<htmlSelect>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetEscolas(string diretoriaId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

    }
}
