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
    public class AlunosAprovadosFiltros
    {
        public string anosLetivos { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiQuantidadeAlunosAprovadosController : ApiController
    {

        [HttpGet]
        public DataTable GetAll([FromUri] AlunosAprovadosFiltros filtro)
        {
            return ApiBO.SelecionaQuantidadeAlunosAprovados(filtro.anosLetivos);
        }
    }
}
