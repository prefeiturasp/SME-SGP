using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemUsuariosController : ApiController
    {
        /// <summary>
        /// Retorna os usuários filtrados pelo objeto com parâmetros de entrada.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public BuscaUsuariosSaidaDTO GetAll([FromUri] BuscaUsuariosEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaUsuarios(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
