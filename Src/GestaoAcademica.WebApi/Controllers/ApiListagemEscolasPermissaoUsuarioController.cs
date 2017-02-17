using System;
using System.Collections.Generic;
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
    public class ApiListagemEscolasPermissaoUsuarioController : ApiController
    {
        /// <summary>
        /// Retorna os cursos filtrados pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="BuscaCursosEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaEscolasPorUsuarioSaidaDTO GetAll([FromUri] BuscaEscolasPorUsuarioEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaEscolasPorPermissaoUsuario(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
