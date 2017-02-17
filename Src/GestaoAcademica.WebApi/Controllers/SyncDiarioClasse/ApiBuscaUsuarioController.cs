using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiBuscaUsuarioController : ApiController
    {

        /// <summary>
        /// Busca informações do usuário atravéz do login.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaDadosUsuarioSaidaDTO GetAll([FromUri] BuscaDadosUsuarioEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDadosUsuario(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }


    }
}
