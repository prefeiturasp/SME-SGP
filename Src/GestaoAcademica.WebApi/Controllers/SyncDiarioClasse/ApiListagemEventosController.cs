using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemEventosController : ApiController
    {
        /// <summary>
        /// Retornas todos os eventos do tipo fechamento para a escola
        /// </summary>
        /// <param name="buscaEventosEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaEventosSaidaDTO GetByEscola([FromUri] BuscaEventosEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaEventos(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
