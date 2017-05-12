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
    public class alunoAnotacaoController : ApiController
    {
        /// <summary>
        /// Descrição: retorna os dados das anotações do aluno, tanto do docente como da equipe gestora.
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada: ano, id do aluno e id da matrícula na turma.</param>
        /// <returns>Objeto com os dados das anotações.</returns>
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "get")]
        public AlunoAnotacaoSaidaDTO Get([FromUri] AnoAlunoTurmaEntradaDTO filtros)
        {
            try
            {
                return ApiBO.BuscaAnotacoesAluno(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                AlunoAnotacaoSaidaDTO saidaDTO = new AlunoAnotacaoSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                saidaDTO.anotacoesDocente = new List<AnotacaoDocenteDTO>();
                saidaDTO.anotacoesGestor = new List<AnotacaoGestorDTO>();
                return saidaDTO;
            }
        }
    }
}