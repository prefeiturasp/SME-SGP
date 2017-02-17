using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{

    /// <summary>
    /// Retorna listagem de compensações de falta por escola
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemCompensacaoFaltaController : ApiController
    {

        public ListagemCompensacaoFaltaSaidaDTO GetAll([FromUri] ListagemCompensacaoFaltaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaCompensacaoFalta(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
