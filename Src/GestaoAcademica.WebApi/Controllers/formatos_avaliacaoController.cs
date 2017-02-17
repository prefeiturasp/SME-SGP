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
    public class formatos_avaliacaoController : ApiController
    {

        /// <summary>
        /// Descrição: Retorna o registro de formato de avaliação pelo id.
        /// -- Utilização: URL_API/formatos_avaliacao/1
        /// -- Paramêtros: id - id do formato de avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ACA_FormatoAvaliacaoDTO Get(int id)
        {
            try
            {
                ACA_FormatoAvaliacaoDTO dto = ApiBO.BuscarFormatoAvaliacaoPorId(id);

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
