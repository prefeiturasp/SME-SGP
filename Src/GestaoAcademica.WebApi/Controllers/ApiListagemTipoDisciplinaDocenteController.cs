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

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemTipoDisciplinaDocenteController : ApiController
    {
        /// <summary>
        /// Retorna as turmas filtradas pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaTurmaDisciplinaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaTurmaDisciplinaSaidaDTO GetAll([FromUri] BuscaTurmaDisciplinaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDocenteDisciplinas(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
