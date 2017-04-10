using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class turma_horariosController : ApiController
    {
        /// <summary>
        /// Persiste os dados do Json.
        /// -- Utilização: URL_API/turma_horarios/
        ///                - Deve ser informado json contendo um array com os turma horários a serem persistidos;
        ///                - O formato do json deve seguir o modelo do método que seleciona todas os turma horários;
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {
                string json = Request.Content.ReadAsStringAsync().Result;

                List<TUR_TurmaHorarioDTO> dto = ApiBO.SalvarTurmaHorario(json);

                return (dto == null || dto.Count.Equals(0)) ?
                    Request.CreateResponse(HttpStatusCode.NoContent) :
                    Request.CreateResponse(HttpStatusCode.Created, dto);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<TUR_TurmaHorarioDTO> GetAll()
        {
            try
            {
                List<TUR_TurmaHorarioDTO> dto = TUR_TurmaHorarioBO.SelecionarTurmaHorario();

                if (dto != null && dto.Count > 0)
                    return dto;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trn_id">Id</param>
        /// <param name="trh_id">Id</param>
        /// <param name="thr_id">Id</param>
        /// <param name="tud_id">Id</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public TUR_TurmaHorarioDTO Get(int trn_id, int trh_id, int thr_id, long tud_id)
        {
            try
            {
                TUR_TurmaHorarioDTO dto = TUR_TurmaHorarioBO.SelecionarTurmaHorarioPorId(trn_id, trh_id, thr_id, tud_id);

                if (dto != null)
                    return dto;

            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

    }
}
