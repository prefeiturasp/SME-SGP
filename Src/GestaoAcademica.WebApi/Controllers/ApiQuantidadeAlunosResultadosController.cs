using MSTech.GestaoEscolar.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class AlunosResultadosFiltros
    {
        public string cal_ano { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiQuantidadeAlunosResultadosController : ApiController
    {
        [HttpGet]
        public DataTable GetAll([FromUri] AlunosResultadosFiltros filtro)
        {
            return ApiBO.SelecionaQuantidadeAlunosResultados(filtro.cal_ano);
        }
    }
}
