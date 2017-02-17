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
    public class ApiListagemAulasController : ApiController
    {
        /// <summary>
        /// Retornas as últimas aulas da turma e disciplina passadas como parametros de entrada.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaAulaSaidaDTO GetAll([FromUri] BuscaAulaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaAulas(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
