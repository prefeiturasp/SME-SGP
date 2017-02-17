using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemTipoNivelEnsinoController : ApiController
    {
        /// <summary>
        /// Retorna os tipos de nivel de ensino
        /// </summary>
        /// <returns></returns>
        public BuscaTipoNivelEnsinoSaidaDTO GetAll()
        {
            try
            {
                return ApiBO.BuscarTiposNivelEnsino();
            }
            catch
            {
                return null;
            }
        }
    }
}
