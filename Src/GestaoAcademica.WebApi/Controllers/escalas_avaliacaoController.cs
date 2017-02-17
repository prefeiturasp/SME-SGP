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
    public class escalas_avaliacaoController : ApiController
    {

        /// <summary>
        /// Descrição: Retorna os dados da escala de avaliação numerica, de parecer ou de relatorio pelo id.
        /// -- Utilização: URL_API/escalas_avaliacao/1
        /// -- Paramêtros: id da escala de avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ACA_EscalaAvaliacaoDTO Get(int id)
        {
            try
            {
                ACA_EscalaAvaliacaoDTO dto = new ACA_EscalaAvaliacaoDTO();
                dto = ApiBO.BuscarEscalaAvaliacaoPorId(id);

                if (dto != null) return dto;
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

    }
}
