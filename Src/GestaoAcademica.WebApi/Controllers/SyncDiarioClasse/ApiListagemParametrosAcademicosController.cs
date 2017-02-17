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
    public class ApiListagemParametrosAcademicosController : ApiController
    {
        /// <summary>
        /// Retorna os valores dos parâmetros acadêmicos informador pelo filtro.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaParametrosAcademicosSaidaDTO GetAll([FromUri] BuscaParametrosAcademicosEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.ListagemParametrosAcademicos(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
