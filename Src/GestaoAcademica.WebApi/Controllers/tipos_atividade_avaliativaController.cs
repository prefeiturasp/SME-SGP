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
    public class tipos_atividade_avaliativaController : ApiController
    {
        /// <summary>
        /// Descrição: retorna o registro do tipo de atividade avaliativa pelo id
        /// -- Utilização: URL_API/tipos_atividade_avaliativa/1
        /// -- Parâmetros: id - id do tipo de atividade avaliativa (tav_id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public CLS_TipoAtividadeAvaliativaDTO Get(int id)
        {
            try
            {
                CLS_TipoAtividadeAvaliativaDTO dto = ApiBO.SelecionarTipoAtividadeAvaliativaPorId(id);
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

        /// <summary>
        /// Descrição: retorna uma lista com os tipos de atividade avaliativas ativos.
        /// -- Utilização: URL_API/tipos_atividade_avaliativa
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CLS_TipoAtividadeAvaliativaDTO> GetAll()
        {
            try
            {
                List<CLS_TipoAtividadeAvaliativaDTO> dto = ApiBO.SelecionarTipoAtividadeAvaliativaAtivos();

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
