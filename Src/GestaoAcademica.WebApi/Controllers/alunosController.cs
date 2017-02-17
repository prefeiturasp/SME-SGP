using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class alunosController : ApiController
    {

        /// <summary>
        /// Descrição: retorna registro de aluno pelo id
        /// -- Utilização: URL_API/aluno/1
        /// -- Parâmetros: id: id do aluno
        /// </summary>
        /// <param name="id">id do aluno</param>
        /// <returns>registro de aluno</returns>
        [HttpGet]
        public ACA_AlunoDTO Get(long id)
        {
            try
            {
                List<ACA_AlunoDTO> aluno = ApiBO.SelecionaDadosAluno(id.ToString(), 0, new DateTime());

                if (aluno != null && aluno.Count > 0)
                    return aluno.FirstOrDefault();
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
        /// Descrição: retorna registros de alunos pelos ids concatenados com fotos compactadas
        /// -- Utilização: URL_API/alunos?ids=1;2;3
        /// -- Parâmetros: ids: ids dos alunos concatenados
        /// </summary>
        /// <param name="ids">ids de alunos concatenados</param>
        /// <returns>lista de alunos</returns>
        [HttpGet]
        public List<ACA_AlunoDTO> GetAlunosPorIds(string ids)
        {
            try
            {
                List<ACA_AlunoDTO> alunos = ApiBO.SelecionaDadosAluno(ids, 0, new DateTime());

                if (alunos != null && alunos.Count > 0)
                    return alunos;
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
        /// Descrição: retorna registros de usuário dos alunos pelos ids concatenados
        /// -- Utilização: URL_API/alunos?alu_ids=1;2;3
        /// -- Parâmetros: ids: ids dos alunos concatenados
        /// </summary>
        /// <param name="ids">ids de alunos concatenados</param>
        /// <returns>lista de alunos</returns>
        [HttpGet]
        public List<object> GetAlunosFotosPorIds(string alu_ids)
        {
            try
            {
                List<object> alunos = ApiBO.SelecionaDadosFotoAlunos(alu_ids, 0, 0);

                if (alunos != null && alunos.Count > 0)
                    return alunos;
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
        /// Descrição: retorna registros de usuário dos alunos pelos ids concatenados com fotos redimensionadas e compactadas
        /// -- Utilização: URL_API/alunos?alu_ids=1;2;3&largura=100&altura=100
        /// -- Parâmetros: ids: ids dos alunos concatenados
        /// </summary>
        /// <param name="ids">ids de alunos concatenados</param>
        /// <param name="largura">largura da foto</param>
        /// <param name="altura">altura da foto</param>
        /// <returns>lista de alunos</returns>
        [HttpGet]
        public List<object> GetAlunosFotosPorIds(string alu_ids, int largura, int altura)
        {
            try
            {
                List<object> alunos = ApiBO.SelecionaDadosFotoAlunos(alu_ids, largura, altura);

                if (alunos != null && alunos.Count > 0)
                    return alunos;
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
        /// Descrição: Retorna lista de alunos por escola levando em consideração a data base 
        /// para selecionar apenas alunos criados ou alterados apos esta data. Caso não se passe a
        /// data como parametro serão retornados apenas alunos ativos.
        /// -- Utilização:  URL_API/alunos?esc_id=1
        ///                 URL_API/alunos?esc_id=1&dataBase=0000-00-00T00:00:00.000
        /// -- Parametros: esc_id = id da escola / dataBase = data base para seleção dos registros.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base</param>
        /// <returns>Lista de alunos</returns>
        [HttpGet]
        public List<ACA_AlunoDTO> GetAlunosPorEscola(int esc_id)
        {
            return GetAlunosPorEscolaDataBase(esc_id, new DateTime());
        }

        [HttpGet][ApiExplorerSettings(IgnoreApi = true)]
        public List<ACA_AlunoDTO> GetAlunosPorEscolaDataBase(int esc_id, DateTime dataBase)
        {
            try
            {
                List<ACA_AlunoDTO> alunos = ApiBO.SelecionaDadosAluno(null, esc_id, dataBase);

                if (alunos != null && alunos.Count > 0)
                    return alunos;
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
        /// OBSOLETO - Entre em contato com a equipe técnica. Quando não passar
        /// a data base apenas registros ativos serão retornados, caso contrario apenas registros
        /// criados ou alterados apos esta data serão retornados.
        /// Descrição: Retorna os alunos vinculados a determinada turma
        /// -- Utilização: URL_API/alunos?tur_id=1 ou URL_API/alunos?tur_id=1&dataBase=9999-99-99T99:99:99.999
        /// -- Parâmetros: tur_id=id da turma
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        [HttpGet]
        public List<AlunoTurma> GetAlunosPorTurma(int tur_id)
        {
            return GetAlunosPorTurma(tur_id, null);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<AlunoTurma> GetAlunosPorTurma(int tur_id, string dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<AlunoTurma> alunos = ApiBO.SelecionarAlunosPorTurma(tur_id, data);

                if (alunos != null && alunos.Count > 0)
                    return alunos;
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
