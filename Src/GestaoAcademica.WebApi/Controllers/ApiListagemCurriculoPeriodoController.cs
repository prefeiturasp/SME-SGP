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
    public class ApiListagemCurriculoPeriodoController : ApiController
    {
        /// <summary>
        /// Retorna os periodos(serie) filtrados pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaCurriculoPeriodoEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaCurriculoPeriodoSaidaDTO GetAll([FromUri] BuscaCurriculoPeriodoEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaCurriculoPeriodoPorEntidadeCursoCurriculo(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
