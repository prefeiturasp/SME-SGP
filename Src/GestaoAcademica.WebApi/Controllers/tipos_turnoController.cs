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
    public class tipos_turnoController : ApiController
    {
        /// <summary>
        /// Seleciona todos os tipos de turno ativas.
        /// -- Utilização: URL_API/tipos_turno       
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<ACA_TipoTurnoDTO> GetAll()
        {
            try
            {
                List<ACA_TipoTurnoDTO> dto = ApiBO.SelecionarTiposTurno();
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
        /// Seleciona os dados do tipo de turnos por id.
        /// -- Utilização: URL_API/tipos_turno/1
        /// -- Parâmetros: Id do tipo de turno
        /// </summary>
        /// <param name="id">Id do tipo de turno.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public ACA_TipoTurnoDTO Get(int id)
        {
            try
            {
                ACA_TipoTurnoDTO dto = ApiBO.SelecionaTiposTurnoPorID(id);
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
