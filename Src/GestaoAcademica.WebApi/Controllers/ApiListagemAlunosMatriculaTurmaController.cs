using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemAlunosMatriculaTurmaController : ApiController
    {
        /// <summary>
        /// Retorna os alunos matriculados na turma
        /// </summary>
        /// <param name="BuscaAlunosMatriculadosTurmaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaAlunosMatriculadosTurmaSaidaDTO GetAll([FromUri] BuscaAlunosMatriculadosTurmaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaAlunosMatriculadosTurma(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
