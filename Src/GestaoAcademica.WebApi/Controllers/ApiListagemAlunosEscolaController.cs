using System;
using System.Collections.Generic;
using System.Data;
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
    public class ApiListagemAlunosEscolaController : ApiController
    {
        /// <summary>
        /// Retorna alunos ativos de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtros">Entidade de filtros informados</param>
        /// <returns></returns>
        public BuscaAlunosDetalhadoEscolaSaidaDTO GetAll([FromUri] BuscaAlunosDetalhadoEscolaEntradaDTO filtros)
        {
            try
            {
                return ApiBO.SelecionaAlunosDetalhadoPorUnidadeAdministrativaEscola(filtros);
            }
            catch
            {
                return null;
            }
        }
    }
}
