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
    [RoutePrefix("api/v1/Cursos")]
    public class CursosController : BaseApiController
    {
        /// <summary>
        /// Busca cursos de acordo com a escola, unidade escolar e o calendário
        /// </summary>
        /// <param name="escolaId">(Obrigarório) Id da escola</param>
        /// <param name="unidadeId">(Obrigarório) Id da unidade escolar</param>
        /// <param name="calendarioId">(Obrigarório) Id do calendário escolar</param>
        /// <returns>Retorna uma lista de cursos</returns>
        [Route("")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetCursos(int escolaId, int unidadeId, int calendarioId)
        {
            try
            {
                var lst = ACA_CursoBO.SelecionaCursoCurriculoCalendarioEscola(escolaId, unidadeId, 0,
                    __userLogged.Usuario.ent_id, calendarioId, ApplicationWEB.AppMinutosCacheLongo);

                if (lst.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                            lst.Select(p => new Curso {
                                    id = p.cur_crr_id.Split(';')[0],
                                    curriculoId = p.cur_crr_id.Split(';')[1],
                                    text = p.cur_crr_nome })
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
