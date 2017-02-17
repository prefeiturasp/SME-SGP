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
    public class ApiListagemEscolasProfessorController : ApiController
    {
        
        /// <summary>
        /// Busca as escolas de acordo com o usuário do professor passado.
        /// </summary>
        /// <param name="filtroEntrada"></param>
        /// <returns></returns>
        public BuscaEscolasProfessorSaidaDTO GetAll([FromUri] BuscaEscolasProfessorEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.ListagemEscolasProfessor(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
