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
    public class ApiListagemEscalaAvaliacaoController : ApiController
    {
        

        /// <summary>
        /// Retorna a listagem de todas as escalas de avaliação de acordo com a entidade.
        /// </summary>
        /// <param name="filtroEntrada"></param>
        /// <returns></returns>
        public BuscaEscalaAvaliacaoSaidaDTO GetAll([FromUri] BuscaEscalaAvaliacaoEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.ListagemEscalaAvaliacao(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
