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
    [RoutePrefix("api/v1/ClassificacoesEscolares")]
    public class ClassificacoesEscolaresController : BaseApiController
    {
        /// <summary>
        /// Busca todos tipos de classificações escolares
        /// </summary>
        /// <returns>Lista de todas o tipos de classificações escolares ativas distintas</returns>
        [Route("")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetTiposClassificacoesEscolares()
        {
            try
            {
                var lst = ESC_TipoClassificacaoEscolaBO.SelecionaTipoClassificacaoEscola();

                return Request.CreateResponse(HttpStatusCode.OK,
                        (from DataRow dr in lst.Rows
                         select new TipoClassificacao {
                             id = Convert.ToInt32(dr["tce_id"]),
                             descricao = dr["tce_nome"].ToString()
                         } )
                        );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
