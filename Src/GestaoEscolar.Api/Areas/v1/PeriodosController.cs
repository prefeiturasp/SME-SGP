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
    [RoutePrefix("api/v1/Periodos")]
    public class PeriodosController : BaseApiController
    {
        /// <summary>
        /// Busca uma lista de períodos de acordo com o id do curso, do currículo e do ciclo 
        /// </summary>
        /// <param name="cursoId">(Obrigarório) Id do curso</param>
        /// <param name="curriculoId">(Obrigarório) Id do currículo</param>
        /// <param name="cicloId">(Opcional) Id do ciclo</param>
        /// <returns>Retorna uma lista de períodos</returns>
        [Route("")]
        [ResponseType(typeof(List<jsonObject>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetPeriodos(int cursoId, int curriculoId, int cicloId = 0)
        {
            try
            {
                List<sComboPeriodo> lst = new List<sComboPeriodo>();
                if (cicloId > 0)
                    lst = ACA_CurriculoPeriodoBO.Select_Por_TipoCiclo(cursoId, curriculoId, cicloId, __userLogged.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);
                else
                    lst = ACA_CurriculoPeriodoBO.GetSelect(cursoId, curriculoId, __userLogged.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

                return Request.CreateResponse(HttpStatusCode.OK,
                        lst.Select(p => new jsonObject
                        {
                            id = p.cur_id_crr_id_crp_id.Split(';')[2],
                            text = p.crp_descricao
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
