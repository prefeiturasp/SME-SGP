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
    public class ApiBuscaDadosDocenteController : ApiController
    {
        /// <summary>
        /// Retorna os dados do docente filtrado pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaDadosDocenteEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaDadosDocenteSaidaDTO GetAll([FromUri] BuscaDadosDocenteEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDadosDocente(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
