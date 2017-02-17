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
    public class recursos_aulaController : ApiController
    {
        /// <summary>
        /// Descrição: retorna um registro do recurso (ACA_RecursosAula) pelo id
        /// -- Utilização: URL_API/recursos_aula/1
        /// -- Parâmetros: id= id do recurso (rsa_id)
        /// </summary>
        /// <param name="id">rsa_id</param>
        /// <returns></returns>
        [HttpGet]
        public ACA_RecursosAulaDTO Get(int id)
        {
            try
            {
                ACA_RecursosAulaDTO dto = ApiBO.SelecionarRecursoPorId(id);
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
        /// Descrição: retorna uma lista com todos os recursos ativos que podem ser utilizados na aula.
        /// -- Utilização: URL_API/recursos_aula
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_RecursosAulaDTO> GetAll()
        {
            try
            {
                List<ACA_RecursosAulaDTO> recursos = ApiBO.SelecionarRecursosAtivos();
                if (recursos != null && recursos.Count > 0) return recursos;
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
