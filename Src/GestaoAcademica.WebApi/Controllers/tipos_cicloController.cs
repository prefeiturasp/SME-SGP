using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace GestaoAcademica.WebApi.Controllers
{
    public class tipos_cicloController : ApiController
    {
        
        //Retorno da API no padrão novo, comentado pois o diario de classe ainda precisa se ajustar a ele, entao esta usando do modo antigo.

        ///// <summary>
        ///// Seleciona todos os tipos de ciclo ativos.
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    try
        //    {               
        //        List<ACA_TipoCicloDTO> dto = ApiBO.SelecionarTipoCiclo();

        //        return (dto == null || dto.Count.Equals(0)) ?
        //            Request.CreateResponse(HttpStatusCode.NotFound) :
        //            Request.CreateResponse(HttpStatusCode.OK, dto);

        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
        //    }
        //}

        /// <summary>
        /// Seleciona todos os tipos de ciclo ativos.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaTipoCicloSaidaDTO GetAll()
        {
            try
            {
                return ApiBO.SelecionarTipoCiclo();
            }
            catch
            {
                return null;
            }
        }


    }
}
