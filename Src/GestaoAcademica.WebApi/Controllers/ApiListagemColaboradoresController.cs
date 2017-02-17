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
    public class FiltrosColaboradores
    {
        public Guid ent_id { get; set; }
        public Guid uad_id { get; set; }
        public int crg_id { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemColaboradoresController : ApiController
    {
        /// <summary>
        /// Retorna alunos ativos de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtros">Entidade de filtros informados</param>
        /// <returns></returns>
        public DataTable GetAll([FromUri] FiltrosColaboradores filtros)
        {
            if (filtros.ent_id == null || filtros.ent_id == Guid.Empty)
            {
                throw new Exception("Entidade é obrigatório.");
            }

            // Se informou a Unidade ou o Cargo, faz a consulta
            if ((filtros.uad_id != null && filtros.uad_id != Guid.Empty)
                || (filtros.crg_id > 0))
            {
                return ApiBO.SelecionaColaboradoresPorUnidade_Cargo(filtros.ent_id, filtros.uad_id, filtros.crg_id);
            }

            return null;
        }
    }
}
