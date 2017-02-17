using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiBuscaPlanejamentoAnualController : ApiController
    {

        /// <summary>
        /// Retorna os calendários de acordo com a escola passada
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public PlanejamentoAnualSaidaDTO GetAll([FromUri] PlanejamentoAnualEntradaDTO filtroEntrada)
        {
            try
            {
                // Configuração que indica se irá retornar dados por escola (se estiver false, só sincroniza
                // com o filtro tud_id alimentado).
                filtroEntrada.sincronizarPorEscola = ApplicationWEB.SincronizarPlanejamentoTablet;
                return ApiBO.BuscaPlanejamentoAnual(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
