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
    public class ApiListagemCalendariosController : ApiController
    {
        /// <summary>
        /// Retorna os calendarios filtrados pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaCalendarioEntidadeEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaCalendariosSaidaDTO GetAll([FromUri] BuscaCalendarioEntidadeEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaCalendarios(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
