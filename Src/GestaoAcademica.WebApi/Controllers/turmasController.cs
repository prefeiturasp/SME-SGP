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
    public class turmasController : ApiController
    {
        public struct TurmaFiltros
        {
            public int esc_id { get; set; }
            public int doc_id { get; set; }
            public Int64 tud_id { get; set; }
            public String dataBase { get; set; }
			public Int64 tur_id { get; set; }
        }

        /// <summary>
        /// Descrição: retorna o registro de turma pelo id
        /// -- Utilização: URL_API/turmas/1
        /// -- Parâmetros: id=corresponde ao id da turma.
        /// </summary>
        /// <param name="id">tur_id</param>
        /// <returns></returns>
        [HttpGet]
        public TUR_TurmaDTO Get(Int64 id)
        {
            try
            {
                List<TUR_TurmaDTO> turmas = ApiBO.SelecionarTurmasAPI(id, 0, 0, 0, new DateTime());
                if (turmas != null && turmas.Count > 0) return turmas.FirstOrDefault();
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
        /// Descrição: lista de turmas com as disciplinas e matriculas por escola, docente e/ou turma disciplina.
        /// -- Utilização:URL_API/turmas?esc_id=1&doc_id=1&tud_id=1&dataBase=2014-12-25T11:10:55.444
        /// -- Paramêtros: esc_id/doc_id/tud_id/dataBase
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpGet]
        public List<TUR_TurmaDTO> GetTurmaPorEscolaDocente([FromUri] TurmaFiltros filtros)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(filtros.dataBase) ? new DateTime() : Convert.ToDateTime(filtros.dataBase);
                List<TUR_TurmaDTO> turma = ApiBO.SelecionarTurmasAPI(filtros.tur_id, filtros.esc_id, filtros.doc_id, filtros.tud_id, data);
                if (turma != null && turma.Count > 0) return turma;
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
        /// Recebe a turma pra salvar 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromUri] TurmaFiltros filter)
        {
            try
            {
                var json = Request.Content.ReadAsStringAsync().Result;
                var turma = ApiBO.SalvarTurma(json);

                return Request.CreateResponse(HttpStatusCode.Created, turma);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro: " + e.Message);
            }
        }
    }
}
