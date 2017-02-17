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

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiQuantidadeTurmasFechamentoController : ApiController
    {
        public DataTable GetAll()
        {
            return ApiBO.SelecionaTurmasFechamento();
        }
    }
}
