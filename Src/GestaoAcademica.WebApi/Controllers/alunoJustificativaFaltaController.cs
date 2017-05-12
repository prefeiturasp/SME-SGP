using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GestaoAcademica.WebApi.Controllers
{
    public class alunoJustificativaFaltaController : ApiController
    {
        /// <summary>
        /// Descrição: retorna os dados das justificativas do aluno.
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada: ano e id do aluno.</param>
        /// <returns>Objeto com os dados das justificativas.</returns>
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "get")]
        public AlunoJustificativaFaltaSaidaDTO Get([FromUri] AnoAlunoEntradaDTO filtros)
        {
            try
            {
                return ApiBO.BuscaJustificativasAluno(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                AlunoJustificativaFaltaSaidaDTO saidaDTO = new AlunoJustificativaFaltaSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                saidaDTO.justificativas = new List<JustificativaDTO>();
                return saidaDTO;
            }
        }
    }
}