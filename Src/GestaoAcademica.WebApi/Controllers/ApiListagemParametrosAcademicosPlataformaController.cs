using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemParametrosAcademicosPlataformaController : ApiController
    {
        /// <summary>
        /// Retorna os dados do docente filtrado pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaDadosDocenteEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaParametrosAcademicosSaidaDTO GetAll([FromUri] BuscaParametrosAcademicosEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.ListagemParametrosAcademicosPlataforma(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
