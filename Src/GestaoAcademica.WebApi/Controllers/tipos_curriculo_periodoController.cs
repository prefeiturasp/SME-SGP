using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;



namespace GestaoAcademica.WebApi.Controllers
{
    public class tipos_curriculo_periodoController : ApiController
    {
        /// <summary>
        /// Descrição: Retorna uma lista de descrição da série.
        /// -- Utilização: 
        /// -- Parametros: tme_id = modalidade de ensino / tne_id = nível de ensino
        /// </summary>
        /// <returns></returns>
        /// <param name="tme_id, tne_id"></param>
        [HttpGet]
        public List<ACA_TipoCurriculoPeriodoDTO> GetTipoCurriculoPeriodoDTO(int tme_id, int tne_id) 
        {
            try
            {
               
                List<ACA_TipoCurriculoPeriodoDTO> tipoCurriculoPeriodo = ApiBO.SelecionarTipoCurriculoPeriodo(tme_id, tne_id); 

                if (tipoCurriculoPeriodo != null && tipoCurriculoPeriodo.Count > 0)
                    return tipoCurriculoPeriodo;
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
