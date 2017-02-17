using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemPermissoesTipoDocenteController : ApiController
    {
        public ListagemPermissoesTipoDocenteDTO GetAll()
        {
            return ApiBO.BuscarPermissoesTipoDocente();
        }

    }
}
