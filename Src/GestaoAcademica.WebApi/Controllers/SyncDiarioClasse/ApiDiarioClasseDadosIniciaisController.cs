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

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiDiarioClasseDadosIniciaisController : ApiController
    {
        /// <summary>
        /// Busca informações como endereço da APK e horário de sincronizações.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        
        public BuscaDadosIniciaisSaidaDTO GetAll([FromUri] BuscaDadosIniciaisEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaDadosIniciais(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }
    }
}
