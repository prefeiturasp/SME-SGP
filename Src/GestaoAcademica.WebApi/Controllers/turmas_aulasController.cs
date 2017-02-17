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
    public class turmas_aulasController : ApiController
    {

        /// <summary>
        /// Retorna a aula pelo id
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <returns></returns>
        [HttpGet]
        public CLS_TurmaAulaDTO Get(Int64 tud_id, Int32 tau_id)
        {
            try
            {
                List<CLS_TurmaAulaDTO> aula = ApiBO.BuscarAula(tud_id, tau_id);

                if (aula != null && aula.Count > 0)
                    return aula.First();
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
        /// Retorna as aulas pela disciplina dentro do periodo.
        /// </summary>
        /// <param name="tud_id">id da disciplina do professor na turma</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <returns></returns>
        [HttpGet]
        public List<CLS_TurmaAulaDTO> GetAulasDaDisciplina(Int64 tud_id, DateTime dataInicio, DateTime dataFim)
        {
            return GetAulasDaDisciplinaPorPosicao(tud_id, 1, dataInicio, dataFim);
        }

        /// <summary>
        /// Retorna as aulas pela disciplina e posição do professor dentro do periodo.
        /// </summary>
        /// <param name="tud_id">id da disciplina do professor na turma</param>
        /// <param name="tdt_posicao">posição do professor na turma</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <returns></returns>
        [HttpGet]
        public List<CLS_TurmaAulaDTO> GetAulasDaDisciplinaPorPosicao(Int64 tud_id, byte tdt_posicao, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                List<CLS_TurmaAulaDTO> aulas = ApiBO.BuscarAulasPorPeriodo(tud_id, tdt_posicao, dataInicio, dataFim, Guid.Empty);

                if (aulas != null && aulas.Count > 0)
                    return aulas;
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
        /// Retorna as aulas pela disciplina e posição do professor dentro do periodo.
        /// </summary>
        /// <param name="tud_id">id da disciplina do professor na turma</param>
        /// <param name="tdt_posicao">posição do professor na turma</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <param name="usu_id">ID do usuário que criou a aula</param>
        /// <returns></returns>
        [HttpGet]
        public List<CLS_TurmaAulaDTO> GetAulasDaDisciplinaPorPosicao(Int64 tud_id, byte tdt_posicao, DateTime dataInicio, DateTime dataFim, Guid usu_id)
        {
            try
            {
                List<CLS_TurmaAulaDTO> aulas = ApiBO.BuscarAulasPorPeriodo(tud_id, tdt_posicao, dataInicio, dataFim, usu_id);

                if (aulas != null && aulas.Count > 0)
                    return aulas;
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
        /// retorna as aulas ativas e excluidas da turma a partir da data base.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para a consulta (yyyy-MM-ddTHH:mm:ss.SSS)</param>
        /// <returns></returns>
        [HttpGet]
        public List<CLS_TurmaAulaDTO> GetAulasDaTurma(Int64 tur_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<CLS_TurmaAulaDTO> aulas = ApiBO.BuscarAulasPorTurmaDataBase(tur_id, data);

                if (aulas != null && aulas.Count > 0)
                    return aulas;
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
        /// retorna as aulas ativas e excluidas da escola a partir da data base.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para a consulta (yyyy-MM-ddTHH:mm:ss.SSS)</param>
        /// <returns></returns>
        [HttpGet]
        public object GetAulasDaEscola(Int32 esc_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                object aulas = ApiBO.BuscarAulasPorEscolaDataBase(esc_id, data);

                if (aulas != null)
                    return aulas;
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
