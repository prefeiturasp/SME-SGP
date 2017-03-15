using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GestaoEscolar.Api.App_Start;

namespace GestaoEscolar.Api.Controllers.Base
{
    [AuthenticationFilter()]
    public class BaseApiController : ApiController 
    {
        public UsuarioWEB __userLogged { get; set; }

    }
}