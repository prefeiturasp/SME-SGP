using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class areas_conhecimentoController : ApiController
    {
        /// <summary>
        /// Seleciona todas as áreas de conhecimento ativas.
        /// -- Utilização: URL_API/area_conhecimento
        /// -- Parâmetros: Sem parâmetros.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<ACA_AreaConhecimentoDTO> GetAll()
        {
            try
            {
                List<ACA_AreaConhecimentoDTO> dto = ApiBO.SelecionarAreasConhecimento();

                if (dto != null && dto.Count > 0)
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
        /// Seleciona os dados da área do conhecimento por id.
        /// -- Utilização: URL_API/area_conhecimento/1
        /// -- Parâmetros: id da area do conhecimento
        /// </summary>
        /// <param name="id">Id da área de conhecimento.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public ACA_AreaConhecimentoDTO Get(int id)
        {
            try
            {
                ACA_AreaConhecimentoDTO dto = ApiBO.SelecionarAreaConhecimentoPorId(id);

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
        /// Persiste os dados do Json.
        /// -- Utilização: URL_API/area_conhecimento/
        ///                - Deve ser informado json contendo um array com as áreas do conhecimento a serem persistidos;
        ///                - O formato do json deve seguir o modelo do método que seleciona todas as áreas do conhecimento;
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {
                string json = Request.Content.ReadAsStringAsync().Result;

                List<ACA_AreaConhecimentoDTO> dto = ApiBO.SalvarAreaConhecimento(json);

                return (dto == null || dto.Count.Equals(0)) ?
                    Request.CreateResponse(HttpStatusCode.NoContent) :
                    Request.CreateResponse(HttpStatusCode.Created, dto);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
            }
        }
    }
}
