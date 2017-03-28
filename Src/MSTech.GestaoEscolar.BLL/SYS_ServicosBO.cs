/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    public enum eChaveServicos : byte
    {
        Arquivos = 1
        ,

        AulasAtividadesAvaliativas = 2
        ,

        Colaborador = 3
        ,

        Trasnferencia = 4
        ,

        Docente = 5
        ,

        Curso = 6
        ,

        PrevisaoSeries = 7
        ,

        TurmaDependenciasEscolares = 8
        ,

        Rel_SituacaoPlanejamentoAulasNotas = 11
        ,

        ProcessamentoAlertasDocente = 12
        ,

        HistoricoEscolarObservacaoTemporarioExclusao = 13
        ,

        EnviarEmailQuantidadeErrosDiaAnterior = 14
        ,

        TurmaDocente_ControleVigencia = 15
        ,

        Rel_EstatisticaAlunoIdadeSexo = 16
        ,

        ImportacaoMD = 17
        ,
        
        AtualizaAulas_DiarioClasse = 21
        ,

        AtualizaPlanejamento_DiarioClasse = 22
        ,

        AtualizaLogs_DiarioClasse = 23
        ,

        AtualizaJustificativa_DiarioClasse = 24
        ,

        AtualizaFoto_DiarioClasse = 25
        ,

        AtualizaCompensacao_DiarioClasse = 26
        ,

        ConsolidadoAtividadeAvaliativa = 27
        ,

        AlocacaoDocente = 28
        ,

        FechamentoRecalcularFrequenciaAulasPrevistas = 29
		,
		
		InativarAlunosRemanejadosSCA = 30
        ,

        GeracaoHistoricoPedagogico = 31
        ,
		
        MatriculasBoletimAtualizar = 33
        ,
		
        AtualizaFrequenciaAjustadaFinal = 34
		,
		
		RelatorioVisaoGeralEscola = 37
        ,

        RelatorioPendenciaAlunos = 38
        ,

        ProcessamentoNotaFrequenciaFechamento = 39
        ,

        AtualizaFechamentoAberturaEvento = 40
        ,

        ProcessamentoDadosFechamento = 41
        ,

        ProcessamentoPendenciasAberturaEvento = 42
        ,

        UnificacaoResponsaveis = 43
        ,

        UnificacaoAlunos = 44
        ,

        AtualizaAtribuicoesEsporadicas = 45
        ,

        AtualizaIndicadorFrequencia = 46
        ,

        ProcessamentoNotaFrequenciaFechamentoParalelo = 47
        ,

        ProcessamentoPendenciaAulas = 48
        ,

        ProcessamentoAbonoFalta = 49
        ,

        ProcessamentoAberturaTurmaAnosAnteriores = 50
        ,

        ProcessamentoDivergenciasRematriculas = 51
        ,

        ProcessamentoSugestaoAulasPrevistas = 52
        ,

        ProcessamentoSugestaoAulasPrevistasTodaRede = 53
        ,

        ProcessamentoDivergenciasAulasPrevistas = 54
    }

    #endregion Enumerador

    /// <summary>
    /// SYS_Servicos Business Object
    /// </summary>
    public class SYS_ServicosBO : BusinessBase<SYS_ServicosDAO, SYS_Servicos>
    {
        /// <summary>
        /// Retorna os serviços cadastrados filtraodor por ids.
        /// </summary>
        /// <param name="ser_ids"></param>
        /// <returns></returns>
        public static List<SYS_Servicos> SelecionaServicosIds(string ser_ids)
        {
            return new SYS_ServicosDAO().SelecionaServicosIds(ser_ids);
        }

        /// <summary>
        /// Retorna um DataTable com todos os serviços cadastrados
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaServicos()
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            return dao.SelectBy_Pesquisa();
        }

        /// <summary>
        /// Retorna uma Lista de entidade com todos os serviços cadastrados
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<SYS_Servicos> SelecionaListaServicos()
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            DataTable dt = dao.SelectBy_Pesquisa();

            List<SYS_Servicos> lt = new List<SYS_Servicos>();

            foreach (DataRow dr in dt.Rows)
            {
                SYS_Servicos entity = new SYS_Servicos();
                lt.Add(dao.DataRowToEntity(dr, entity));
            }

            return lt;
        }

        /// <summary>
        /// Seleciona nome do job pelo nome do serviço
        /// </summary>
        /// <param name="ser_nome">Nome do serviço</param>
        /// <returns></returns>
        public static string SelecionaProcedimentoPorNome(string ser_nome)
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            return dao.SelectProcedimentoPorNome(ser_nome);
        }

        /// <summary>
        /// Verifica se já existe um serviço cadastrado com o mesmo nome
        /// </summary>
        /// <param name="ser_id">ID do serviço</param>
        /// <param name="ser_nome">Nome do serviço</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaServicoExistente
        (
            Int16 ser_id
            , string ser_nome
        )
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            return dao.SelectBy_Nome(ser_id, ser_nome);
        }

        /// <summary>
        /// Verifica se já existe um serviço cadastrado com o mesmo nome de procedimento
        /// </summary>
        /// <param name="ser_id">ID do serviço</param>
        /// <param name="ser_nomeProcedimento">Nome do procedimento do serviço</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaProcedimentoExistente
        (
            Int16 ser_id
            , string ser_nomeProcedimento
        )
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            return dao.SelectBy_NomeProcedimento(ser_id, ser_nomeProcedimento);
        }

        /// <summary>
        /// Inclui ou altera o serviço
        /// </summary>
        /// <param name="entity">Entidade SYS_Servicos</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            SYS_Servicos entity
        )
        {
            if (entity.Validate())
            {
                if (VerificaServicoExistente(entity.ser_id, entity.ser_nome))
                    throw new ValidationException("Já existe um serviço cadastrado com este nome.");

                if (VerificaProcedimentoExistente(entity.ser_id, entity.ser_nomeProcedimento))
                    throw new ValidationException("Já existe um procedimento cadastrado com este nome.");

                SYS_ServicosDAO dao = new SYS_ServicosDAO();
                dao.Salvar(entity);

                return true;
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Deleta logicamente o serviço
        /// </summary>
        /// <param name="entity">Entidade SYS_Servicos</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            SYS_Servicos entity
        )
        {
            SYS_ServicosDAO dao = new SYS_ServicosDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o serviço pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("ser_id", entity.ser_id.ToString(), "SYS_Servicos", dao._Banco))
                    throw new ValidationException("Não é possível excluir o serviço pois possui outros registros ligados a ele.");

                //Deleta logicamente o serviço
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Seleciona o status dos serviços configurados
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectStatus()
        {
            return new SYS_ServicosDAO().SelectStatus();
        }

        /// <summary>
        /// Seleciona a expressão de agendamento do serviço
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public static string GeraCronExpression(string jobName)
        {
            return new SYS_ServicosDAO().GeraCronExpression(jobName);
        }

        /// <summary>
        /// Seleciona o status do serviço
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public static byte SelecionaStatusServico(string jobName)
        {
            return new SYS_ServicosDAO().SelecionaStatusServico(jobName);
        }
    }
}