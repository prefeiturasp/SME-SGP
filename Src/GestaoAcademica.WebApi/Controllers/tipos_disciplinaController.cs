using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class tipos_disciplinaController : ApiController
    {
        /// <summary>
        /// Seleciona todos os tipos de disciplina ativas.
        /// -- Utilização: URL_API/tipos_disciplina/?tne_id={1}&aco_id={2}
        /// -- Parâmetros: tne_id (Id do Tipo de nível de ensino) 
        ///                aco_id (Id da Area do conhecimento)
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<ACA_TipoDisciplinaDTO> GetAll([FromUri] TipoDisciplinaEntradaDTO filtro)
        {
            try
            {
                List<ACA_TipoDisciplinaDTO> dto = ApiBO.SelecionarTiposDisciplina(filtro);
                if (dto != null && dto.Count > 0) return dto;
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
        /// Seleciona os dados do tipo de disciplina por id.
        /// -- Utilização: URL_API/tipos_disciplina/1
        /// -- Parâmetros: Id do tipo de disciplina
        /// </summary>
        /// <param name="id">Id do tipo de disciplina.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public ACA_TipoDisciplinaDTO Get(int id)
        {
            try
            {
                ACA_TipoDisciplinaDTO dto = ApiBO.SelecionarTipoDisciplinaPorId(id);

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
        /// Seleciona os dados do tipo de disciplina por Tipo Nivel Ensino.
        /// -- Utilização: URL_API/tipos_disciplina?tne_id=1
        /// -- Parâmetros: Id do tipo de nivel ensino
        /// </summary>
        /// <param name="id">Id do tipo de nivel ensino.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<ACA_TipoDisciplinaDTO> GetPorNivelEnsino(int tne_id)
        {
            try
            {
                List<ACA_TipoDisciplinaDTO> dto = ApiBO.SelecionarTipoDisciplinaPorNivelEnsino(tne_id);

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