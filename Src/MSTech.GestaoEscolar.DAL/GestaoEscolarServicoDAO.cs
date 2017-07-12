using System.Data;
using System.Data.Common;
using MSTech.Data.Common;
using System;

namespace MSTech.GestaoEscolar.DAL
{
    public class GestaoEscolarServicoDAO : Persistent
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        public void ExecJobArquivoExclusaoAsync()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ArquivosExclusao", _Banco);
            qs.Execute();
        }
                
        /// <summary>
        /// Retorna a expressão de configuração de acordo com o nome do trigger.
        /// </summary>
        /// <param name="trigger">Nome do trigger.</param>
        /// <returns>Expressão de configuração</returns>
        public DataTable SelecionaExpressaoPorTrigger(string trigger)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_QTZ_Cron_Triggers_SelecionaExpressaoPorTrigger", _Banco);

            #region Parâmetros

            DbParameter Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@trigger";
            Param.Value = trigger;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }
                        
        /// <summary>
        /// Atualiza as situaçoes das atribuiçoes esporádicas e das TurmaDocente geradas de acordo com a vigencia.
        /// </summary>
        public void ExecJobAtualizaAtribuicoesEsporadicasAsync()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_AtualizaAtribuicoesEsporadicas", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Atualiza as informações pré-calculadas da tabela CLS_IndicadorFrequencia.
        /// </summary>
        public void ExecJobAtualizaIndicadorFrequenciaAsync()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_AtualizaIndicadorFrequencia", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Executa o JOB de recálculo da frequência no fechamento baseado em aulas previstas.
        /// </summary>
        public void ExecJobFechamentoRecalcularFrequenciaAulasPrevistasAsync()
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("MS_JOB_FechamentoRecalcularFrequenciaAulasPrevistas", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Executa o JOB de geracao de historico pedagogico para os alunos.
        /// </summary>
        public void ExecJobGeracaoHistoricoPedagogicoAsync()
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("MS_JOB_GeracaoHistoricoPedagogico", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Executa o JOB que realiza a atualização dos registros da tabela MTR_MatriculasBoletim.
        /// </summary>
        public void ExecJobJobMatriculasBoletimAtualizarAsync()
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("MS_JOB_MatriculasBoletim_Atualizar", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Atualiza a frequência ajustada no fechamento do último bimestre, dos registros afetados.
        /// </summary>
        public void ExecJobJobAtualizaFrequenciaAjustadaFinalAsync()
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("MS_JOB_AtualizaFrequenciaAjustadaFinal", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Faz o pré procesamento do relatório de pendências por disciplinas e alunos
        /// </summary>
        public DataTable ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
            return qs.Return;
        }

        /// <summary>
        /// Faz o pré procesamento de notas e frequeências que estão na fila para o novo fechamento
        /// </summary>
        public DataTable ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AtualizaFaltasFechamento", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
            return qs.Return;
        }

        /// <summary>
        /// Joga os registros da tabela pré-processada para a tabela de fechamento no dia da abertura do evento de fechamento
        /// </summary>
        public void ExecJOB_AtualizaFechamento_AberturaEventoAsync()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_AtualizaFechamento_AberturaEvento", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Executa o job que processa as pendências da escola no bimestre de acordo com a abertura do evento.
        /// </summary>
        public void ExecJobProcessamentoPendenciasAberturaEventoAsync()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ProcessamentoPendenciasAberturaEvento", _Banco);
            qs.Execute();
        }
                
        /// <summary>
        /// Processa as pendencias de aulas sem plano.
        /// </summary>
        public void ExecJOB_ProcessamentoPendenciaAulas()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ProcessamentoPendenciaAulas", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Processa a remoção das faltas com justificativa de abono.
        /// </summary>
        public void ExecJOB_ProcessamentoAbonoFalta()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ProcessamentoAbonoFalta", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Processa a abertura/fechamento das turmas dos anos anteriores.
        /// </summary>
        public void ExecJOB_ProcessamentoAberturaTurmaAnosAnteriores()
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ProcessamentoAberturaTurmaAnosAnteriores", _Banco);
            qs.Execute();
        }

        /// <summary>
        /// Processa as divergências nas rematrículas conforme o resultado do ano anterior
        /// </summary>
        public void ExecJOB_ProcessamentoDivergenciasRematriculas(Guid sle_id)
        {
            QueryStoredProcedureAsync qs = new QueryStoredProcedureAsync("MS_JOB_ProcessamentoDivergenciasRematriculas", _Banco);

            qs.Execute();
        }

        /// <summary>
        /// Processa os dados para a sugestão das aulas previstas.
        /// </summary>
        public void ExecJOB_ProcessamentoSugestaoAulasPrevistas(bool todaRede)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoSugestaoAulasPrevistas", _Banco);

            #region Parâmetros

            DbParameter Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@todaRede";
            Param.Size = 1;
            Param.Value = todaRede;
            qs.Parameters.Add(Param);

            #endregion

            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa as divergências entre registros de aulas dadas e aulas previstas.
        /// </summary>
        public void ExecJOB_ProcessamentoDivergenciasAulasPrevistas()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoDivergenciasAulasPrevistas", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa o alerta de preenchimento de frequência.
        /// </summary>
        public void ExecJOB_AlertaPreenchimentoFrequencias()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AlertaPreenchimentoFrequencia", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa o alerta de aviso de início de fechamento.
        /// </summary>
        public DataTable ExecJOB_AlertaInicioFechamento(int cfa_periodoAnalise)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AlertaInicioFechamento", _Banco);

            #region Parâmetros

            DbParameter Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cfa_periodoAnalise";
            Param.Size = 4;
            Param.Value = cfa_periodoAnalise;
            qs.Parameters.Add(Param);

            #endregion

            qs.TimeOut = 0;
            qs.Execute();
            return qs.Return;
        }

        /// <summary>
        /// Processa o alerta de aviso de final de fechamento.
        /// </summary>
        public DataTable ExecJOB_AlertaFimFechamento(int cfa_periodoAnalise)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AlertaFimFechamento", _Banco);

            #region Parâmetros

            DbParameter Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cfa_periodoAnalise";
            Param.Size = 4;
            Param.Value = cfa_periodoAnalise;
            qs.Parameters.Add(Param);

            #endregion

            qs.TimeOut = 0;
            qs.Execute();
            return qs.Return;
        }

        /// <summary>
        /// Processa o alerta de alunos com baixa frequência.
        /// </summary>
        public void ExecJOB_AlertaAlunosBaixaFrequencia()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AlertaAlunosBaixaFrequencia", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa o alerta de alunos com faltas consecutivas.
        /// </summary>
        public void ExecJOB_AlertaAlunosFaltasConsecutivas()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_AlertaAlunosFaltasConsecutivas", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa o preenchimento de frequência, conta as aulas sem a flag efetivado.
        /// Utiliza a fila do fechamento.
        /// </summary>
        public void ExecJOB_ProcessamentoPreenchimentoFrequencia()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoPreenchimentoFrequencia", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }

        /// <summary>
        /// Processa os alunos com baixa frequência e com faltas consecutivas.
        /// Utiliza a fila do fechamento.
        /// </summary>
        public void ExecJOB_ProcessamentoAlunosFrequencia()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoAlunosFrequencia", _Banco);
            qs.TimeOut = 0;
            qs.Execute();
        }
    }
}
