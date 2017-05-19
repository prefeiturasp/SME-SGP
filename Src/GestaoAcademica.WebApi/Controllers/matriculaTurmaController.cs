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
    public class matriculaTurmaController : ApiController
    {
        /// <summary>
        /// Seleciona dados da matrícula do aluno
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada: id da matricula e id do aluno.</param>
        /// <returns>Objeto com os dados da matrícula do aluno.</returns>
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "get")]
        public HttpResponseMessage Get(long alu_id, int mtu_id)
        {
            try
            {
                MatriculaTurmaSaidaDTO retorno = ApiBO.BuscaMatriculaTurma(alu_id, mtu_id);

                if (retorno != null && retorno.mtu_id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                MatriculaTurmaSaidaDTO saidaDTO = new MatriculaTurmaSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, saidaDTO);
            }
        }
    }
}
