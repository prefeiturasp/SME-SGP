using System;
using System.Collections.Generic;
using System.Data;
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
    public class ApiListagemEscolasController : ApiController
    {
        /// <summary>
        /// Retorna as escolas filtradas pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaEscolaSaidaDTO GetAll([FromUri] BuscaEscolaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.ListagemEscolas(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
