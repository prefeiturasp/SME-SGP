using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class protocolosController : ApiController
    {

        public struct ProtocoloFiltro
        {
            public Int32 esc_id { get; set; }
            public String dataBase { get; set; }
            public int pro_tipo { get; set; }
        }

        struct ProtocolResponse
        {
            public long pro_protocolo { get; set; }
        }

        /// <summary>
        /// Descrição: retorna um registro de protocolo pelo id (Guid)
        /// -- Utilização: URL_API/protocolos/00000000-0000-0000-0000-000000000000
        /// -- Parâmetros: id= id do protocolo (pro_id)
        /// </summary>
        /// <param name="id">pro_id=id do protocolo</param>
        /// <returns></returns>
        [HttpGet]
        public DCL_ProtocoloDTO Get(string id)
        {
            try
            {
                DCL_ProtocoloDTO dto = ApiBO.SelecionarProtocoloPorId(id);

                if (dto != null)
                    return dto;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Descrição: retorna uma lista de protocolos por escola e data base, podendo filtrar pelo tipo
        /// --Utilização:URL_API/protocolos?esc_id=1&dataBase=9999-99-99T99:99:99.999&pro_tipo=1
        /// --Paramêtros:esc_id/dataBase/pro_tipo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<DCL_ProtocoloDTO> GetByEscola([FromUri] ProtocoloFiltro filtro)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(filtro.dataBase) ? new DateTime() : Convert.ToDateTime(filtro.dataBase);
                List<DCL_ProtocoloDTO> protocolos = ApiBO.SelecionarProtocoloPorEscola(filtro.esc_id, data, filtro.pro_tipo);
                if (protocolos != null && protocolos.Count > 0) return protocolos;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Descrição: recebe e persiste um protocolo no SGP para ser processado.
        /// -- Utilização: URL_API/protocolos
        /// </summary>
        /// <param name="protocolo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] DCL_Protocolo protocolo)
        {
            try
            {
                DCL_Protocolo protocoloBD = ApiBO.SalvarProtocolo(protocolo);
                return Request.CreateResponse(HttpStatusCode.Created, protocoloBD);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro: " + e.Message);
            }
        }

        ///// <summary>
        ///// Descrição: recebe e persiste uma lista de protocolos no SGP para ser processado.
        ///// -- Utilização: URL_API/protocolos
        ///// </summary>
        ///// <param name="protocolo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage Post([FromBody] DCL_Protocolo[] protocolos)
        //{
        //    try
        //    {
        //        List<DCL_Protocolo> list = new List<DCL_Protocolo>();
        //        foreach (DCL_Protocolo protocolo in protocolos) {
        //            list.Add(ApiBO.SalvarProtocolo(protocolo));
        //        }

        //        return Request.CreateResponse(HttpStatusCode.Created, list);
        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro: " + e.Message);
        //    }
        //}
    }
}
