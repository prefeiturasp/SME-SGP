using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class aluno_fechamento_pendenciaController : ApiController
    {
        
        /// <summary>
        /// Seleciona a fila de alunoFechamentoPendencia do serviço
        /// -- Utilização: URL_API/servicos/GetFila/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<FilaAlunoFechamentoPendenciaDTO> GetFila()
        {
            try
            {
                List<FilaAlunoFechamentoPendenciaDTO> fila = ApiBO.SelecionarFilaAlunoFechamentoPendenciaAPI();
                return fila;
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