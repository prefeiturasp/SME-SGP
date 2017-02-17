using System.Web.Http;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemDadosCursosEntidadeController : ApiController
    {
        /// <summary>
        /// Retorna todos os níveis de ensino, cursos e currículos mediante a entidade informada.
        /// </summary>
        /// <param name="BuscaDadosCursosEntidadeEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaDadosCursosEntidadeSaidaDTO GetAll([FromUri] BuscaDadosCursosEntidadeEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDadosCursosEntidade(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}