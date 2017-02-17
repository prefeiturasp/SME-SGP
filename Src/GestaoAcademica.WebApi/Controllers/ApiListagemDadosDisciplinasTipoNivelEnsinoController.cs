using System.Web.Http;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemDadosDisciplinasTipoNivelEnsinoController : ApiController
    {
        /// <summary>
        /// Retorna os dados das disciplinas filtrados pelo objeto com parâmetros de entrada [tne_id].
        /// </summary>
        /// <param name="BuscaDadosDisciplinasEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaDadosDisciplinasSaidaDTO GetAll([FromUri] BuscaDadosDisciplinasEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDadosDisciplinas(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}