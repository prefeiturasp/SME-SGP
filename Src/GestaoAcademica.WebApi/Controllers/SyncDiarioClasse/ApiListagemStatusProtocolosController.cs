using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemStatusProtocolosController : ApiController
    {
        /// <summary>
        /// Retorna os status dos protocolos que estão pendentes.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public ProtocoloSaidaDTO GetAll([FromUri] ProtocoloEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaStatusProtocolos(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
