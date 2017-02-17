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
    public class disciplinasController : ApiController
    {


        /// <summary>
        /// Descrição: retorna um registro da disciplina da turma por id.
        /// -- Utilização: URL_API/disciplinas/1
        /// -- Parâmetros: id=corresponde ao id da turma disciplina.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public TUR_TurmaDisciplinaDTO Get(long id)
        {
            try
            {
                List<TUR_TurmaDisciplinaDTO> disciplinas = ApiBO.SelecionarTurmaDisciplinaPorId(id);

                if (disciplinas != null && disciplinas.Count > 0) return disciplinas.FirstOrDefault();
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
        /// Descrição: retorna as disciplinas que o docente esta vinculado.
        /// -- utilização : URL_API/disciplinas?doc_id=1
        /// -- parâmetros : doc_id: id do docente
        /// </summary>
        /// <param name="doc_id">id do docente</param>
        /// <returns></returns>
        [HttpGet]
        public List<TUR_TurmaDisciplinaDTO> GetDisciplinasPorDocente(long doc_id)
        {
            return GetDisciplinasPorDocenteTurma(0, doc_id);
        }

        /// <summary>
        /// Descrição: retorna as disciplinas da turma.
        /// -- utilização : URL_API/disciplinas?tur_id=1
        /// -- parâmetros : tur_id: id da turma
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        [HttpGet]
        public List<TUR_TurmaDisciplinaDTO> GetDisciplinasPorTurma(long tur_id)
        {
            return GetDisciplinasPorDocenteTurma(tur_id, 0);
        }

        /// <summary>
        /// Descrição: retorna as disciplinas do docente na turma.
        /// -- utilização : URL_API/disciplinas?tur_id=1&doc_id=1
        /// -- parâmetros : tur_id: id da turma; doc_id: id do docente
        /// </summary>
        /// <param name="doc_id">id do docente</param>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        [HttpGet]
        public List<TUR_TurmaDisciplinaDTO> GetDisciplinasPorDocenteTurma(long tur_id, long doc_id)
        {
            try
            {
                List<TUR_TurmaDisciplinaDTO> disciplinas = ApiBO.SelecionarTurmaDisciplinaPorTurmaDocente(tur_id, doc_id);

                if (disciplinas != null && disciplinas.Count > 0) return disciplinas;
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
        /// Descrição: Retorna uma listagem de disciplinas ativas por escola
        /// --Utilização: URL_API/disciplinas?esc_id=1
        /// --Paramêtros: esc_id id da escola
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns>dataTable com as disciplinas</returns>
        [HttpGet]
        public List<TUR_TurmaDisciplinaDTO> GetDisciplinasPorEscola(int esc_id)
        {
            return GetDisciplinasPorEscolaDataBase(esc_id, null);
        }


        /// <summary>
        /// Descrição: Retorna uma listagem de disciplinas por escola, serão retornados
        /// apenas os registros criados ou alterados apos a data base.
        /// --Utilização: URL_API/disciplinas?esc_id=1&dataBase=999-99-99T99:99:99.999
        /// --Paramêtros: esc_id id da escola / dataBase = data base para seleção dos registros
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns>dataTable com as disciplinas</returns>
        [HttpGet]
        public List<TUR_TurmaDisciplinaDTO> GetDisciplinasPorEscolaDataBase(int esc_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<TUR_TurmaDisciplinaDTO> disciplinas = ApiBO.SelecionarTurmaDisciplinaPorEscola(esc_id, data);

                if (disciplinas != null && disciplinas.Count > 0)
                    return disciplinas;
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
