using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class colaboradoresController : ApiController
    {
        [HttpGet]
        public List<RHU_ColaboradorDTO> GetColaboradoresPorEscolaMatricula(int esc_id, string matricula)
        {
            try
            {
                List<RHU_ColaboradorDTO> colaboradores = ApiBO.SelecionarColaboradoresPorEscolaMatricula(esc_id, matricula);

                if (colaboradores != null && colaboradores.Count > 0)
                    return colaboradores;
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

        [HttpGet]
        public List<RHU_ColaboradorDTO> GetColaboradoresPorMatricula(string matricula)
        {
            return GetColaboradoresPorEscolaMatricula(0, matricula);
        }

        [HttpGet]
        public List<RHU_ColaboradorDTO> GetColaboradoresPorEscola(int esc_id)
        {
            return GetColaboradoresPorEscolaMatricula(esc_id, "");
        }

        /// <summary>
        /// Persiste os dados do Json.
        /// -- Utilização: URL_API/colaboradore/
        ///                - Deve ser informado json contendo um array com os cargos/funções a serem persistidos;
        ///                - O formato do json deve seguir o modelo do método que seleciona todos os colaboradores;
        ///                - Para o método POST é necessário informar todos os atributos dos itens da lista colaboradorCargo 
        ///                  e/ou colaboradorFuncao, pois é realizado uma série de validações na camada de negócios.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {
                string json = Request.Content.ReadAsStringAsync().Result;

                List<RHU_ColaboradorDTO> dto = ApiBO.SalvarColaboradores(json);

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
