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
    public class ApiListagemTiposAtividadeAvaliativaController : ApiController
    {
        /// <summary>
        /// Retorna os tipos de atividades avaliativas.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        
        public BuscaTiposAtividadeAvaliativaSaidaDTO GetAll([FromUri] BuscaTiposAtividadeAvaliativaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaTiposAtividadeAvaliativa(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
