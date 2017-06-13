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
    [RoutePrefix("api/v1/Ciclos")]
    public class CiclosController : BaseApiController
    {
        /// <summary>
        /// Busca uma lista de ciclos de acordo com o id do curso e do currículo
        /// </summary>
        /// <param name="cursoId">(Obrigarório) Id do curso</param>
        /// <param name="curriculoId">(Obrigarório) Id do currículo</param>
        /// <returns>Retorna uma lista de ciclos</returns>
        [Route("Anual")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetCiclos(int cursoId, int curriculoId)
        {
            try
            {
                var lst = ACA_TipoCicloBO.SelecionaCicloPorCursoCurriculo(cursoId, curriculoId, ApplicationWEB.AppMinutosCacheLongo);
                return Request.CreateResponse(HttpStatusCode.OK,
                        lst.Select(p => new Curso
                        {
                            id = p.tci_id,
                            text = p.tci_nome
                        })
                        );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
