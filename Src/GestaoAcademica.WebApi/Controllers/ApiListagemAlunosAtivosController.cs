using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class FiltrosAlunos
    {
        public Guid ent_id { get; set; }
        public Guid uad_id { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemAlunosAtivosController : ApiController
    {
        /// <summary>
        /// Retorna alunos ativos de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtros">Entidade de filtros informados</param>
        /// <returns></returns>
        public DataTable GetAll([FromUri] FiltrosAlunos filtros)
        {
            if (filtros.ent_id == null || filtros.ent_id == Guid.Empty)
            {
                throw new Exception("Entidade é obrigatório.");
            }

            if (filtros.uad_id != null && filtros.uad_id != Guid.Empty)
            {
                return ApiBO.SelecionaAlunosAtivosPorUnidadeAdministrativaEscola(filtros.ent_id, filtros.uad_id);
            }

            return null;
        }
    }
}
