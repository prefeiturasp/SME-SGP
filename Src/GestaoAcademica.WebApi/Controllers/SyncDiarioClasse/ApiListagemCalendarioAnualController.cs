using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiListagemCalendarioAnualController : ApiController
    {
        /// <summary>
        /// Retorna os calendários de acordo com a escola passada
        /// 
        /// Obsoleto: utilize a API /calendarios_anuais
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        public CalendarioAnualSaidaDTO GetAll([FromUri] CalendarioAnualEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaCalendarioAnual(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
