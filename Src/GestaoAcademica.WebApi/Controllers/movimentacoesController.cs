using GestaoAcademica.WebApi.Authentication;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GestaoAcademica.WebApi.Controllers
{
    public class movimentacoesController : ApiController
    {
        /// <summary>
        /// Seleciona todas as movimentações(Struct) posterior a data informada.
        ///     Struct {
        ///         alu_id: Id do Aluno
        ///         , tmo_id: Id do tipo de movimentacao
        ///         , escolaSaida: Id da escola de saida
        ///         , turmaSaida: Id da turma de saida
        ///         , escolaEntrada: Id da escola de entrada
        ///         , turmaEntrada: Id da turma de entrada
        ///     }        
        /// -- Utilização: URL_API/movimentacoes/?dataAlteracao=0000-00-00T00:00:00
        /// -- Parâmetros: dataAlteracao: Data em que a movimentação foi realizada.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<Movimentacao> GetAll(String dataAlteracao)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataAlteracao) ? new DateTime() : Convert.ToDateTime(dataAlteracao);
                List<Movimentacao> dto = ApiBO.SelecionarMovimentacoesPorDataAlteracao(data);
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
        /// Seleciona todas as movimentações(Entidade) posterior a data informada.
        /// -- Utilização: URL_API/movimentacoes/?mov_dataAlteracao=0000-00-00T00:00:00
        /// -- Parâmetros: mov_dataAlteracao: Data em que a movimentação foi realizada.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public List<MTR_MovimentacaoDTO> Get(String mov_dataAlteracao)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(mov_dataAlteracao) ? new DateTime() : Convert.ToDateTime(mov_dataAlteracao);
                List<MTR_MovimentacaoDTO> dto = ApiBO.SelecionarMovimentacoesDTOPorDataAlteracao(data);
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
        /// Seleciona a movimentacao para o alu_id e mov_id informado.
        /// -- Utilização: URL_API/movimentacoes/?alu_id=1&mov_id=2
        /// -- Parâmetros: alu_id: Id do aluno
        ///                mov_id: Id da movimentacao
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public MTR_MovimentacaoDTO Get(long alu_id, int mov_id)
        {
            try
            {
                MTR_MovimentacaoDTO movimentacao = ApiBO.SelecionarMovimentacaoPorid(alu_id, mov_id);
                if (movimentacao != null) return movimentacao;
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
        /// Descrição: retorna os dados de algumas movimentações específicas do aluno.
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada: ano, id do aluno e id da matrícula na turma..</param>
        /// <returns>Objeto com os dados das movimentações.</returns>
        [HttpGet]
        [BasicAuthentication(false)]
        [JWTAuthenticationFilter()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public AlunoMovimentacaoSaidaDTO Get([FromUri] AnoAlunoTurmaEntradaDTO filtros)
        {
            try
            {
                return ApiBO.BuscaMovimentacoesEspecificasAluno(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                AlunoMovimentacaoSaidaDTO saidaDTO = new AlunoMovimentacaoSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                saidaDTO.movimentacoes = new List<MovimentacaoDTO>();
                return saidaDTO;
            }
        }
    }
}