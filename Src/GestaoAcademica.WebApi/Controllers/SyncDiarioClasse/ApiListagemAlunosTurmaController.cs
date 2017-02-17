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
    public class ApiListagemAlunosTurmaController : ApiController
    {
        /// <summary>
        /// Retorna os alunos das turmas filtradas pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        
        public BuscaAlunosTurmaSaidaDTO GetAll([FromUri] BuscaAlunosTurmaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaAlunosTurma(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
