using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GestaoAcademica.WebApi.Controllers
{
    public class conselhoClasseController : ApiController
    {
        /// <summary>
        /// Retorna dados de conselho de classe da matrícula do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "get")]
        public HttpResponseMessage GetPorMatricula(long alu_id, int mtu_id)
        {
            try
            {
                ConselhoClasseSaidaDTO retorno = ApiBO.BuscaDadosConselhoClasse(alu_id, mtu_id);

                if (retorno != null && retorno.dadosConselho.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ConselhoClasseSaidaDTO retorno = new ConselhoClasseSaidaDTO();
                retorno.Status = 1;
                retorno.StatusDescription = "Ocorreu um erro ao carregar dados.";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, retorno);
            }
        }
    }
}
