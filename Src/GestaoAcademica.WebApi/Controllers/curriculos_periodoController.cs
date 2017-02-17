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
    public class curriculos_periodoController : ApiController
    {

        /// <summary>
        /// Descrição: Retorna uma lista de curriculos do periodo por escola. Quando
        /// recebe a data base apenas os registros criados ou alterados apos esta data serão
        /// retornados, caso contrario apenas registros ativos serão retornados.
        /// -- Utilização: API_URL/curriculos_periodo?esc_id=1 ou API_URL/curriculos_periodo?esc_id=1&dataBase=9999-99-99T99:99:99.999
        /// -- Parametros: esc_id = id da escola / dataBase = data base para retorno dos registros.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_CurriculoPeriodoDTO> GetCurriculoPeriodoPorEscola(int esc_id)
        {
            return GetCurriculoPeriodoPorEscolaDataBase(esc_id, null);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ACA_CurriculoPeriodoDTO> GetCurriculoPeriodoPorEscolaDataBase(int esc_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ACA_CurriculoPeriodoDTO> curriculos = ApiBO.SelecionarCurriculosPorEscola(esc_id, data);

                if (curriculos != null && curriculos.Count > 0)
                    return curriculos;
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
