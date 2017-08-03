using System;
using MSTech.GestaoEscolar.BLL;
using Quartz;
using Quartz.Impl;

namespace MSTech.GestaoEscolar.Jobs.GestaoAcademica
{
    //[DisallowConcurrentExecution]
    public class MS_JOB_ArquivosExclusao : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobArquivoExclusao();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }
            
    public class MS_JOB_AtualizaAulas_DiarioClasse : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaAulasDiarioClasse();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaPlanejamento_DiarioClasse : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaPlanejamentoDiarioClasse();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaLogs_DiarioClasse : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaLogsDiarioClasse();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }
    
    public class MS_JOB_AtualizaFoto_DiarioClasse : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaFotoDiarioClasse();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaCompensacao_DiarioClasse : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaCompensacaoDiarioClasse();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }
    
    public class MS_JOB_FechamentoRecalcularFrequenciaAulasPrevistas : IJob
    {
        #region IJob Member

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobFechamentoRecalcularFrequenciaAulasPrevistas();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Member
    }
    
    public class MS_JOB_GeracaoHistoricoPedagogico : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobGeracaoHistoricoPedagogico();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_MatriculasBoletim_Atualizar : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobMatriculasBoletimAtualizar();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }
        #endregion IJob Members
    }

    public class MS_JOB_AtualizaFrequenciaAjustadaFinal : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobAtualizaFrequenciaAjustadaFinal();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }
        #endregion IJob Members
    }    

    public class MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoNotaFrequenciaFechamento : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaFechamento_AberturaEvento : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AtualizaFechamento_AberturaEventoAsync();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }
    
    public class MS_JOB_ProcessamentoPendenciasAberturaEvento : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJobProcessamentoPendenciasAberturaEvento();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaAtribuicoesEsporadicas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AtualizaAtribuicoesEsporadicas();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AtualizaIndicadorFrequencia : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AtualizaIndicadorFrequencia();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoNotaFrequenciaFechamentoParalelo : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                string groupProcess = string.Format("GroupProcess{0}", DateTime.Now.ToBinary());

                int threads = GestaoEscolarServicosBO.GetQuantidadeMaximaFilaFechamento();
                threads = threads <= 0 ? 1 : threads;

                for (int i = 0; i < threads; i++)
                {
                    var jobKey = new JobKey(string.Format("UniqueJobID{0}", i), groupProcess);

                    IJobDetail jobDetail = JobBuilder.Create<MS_JOB_ProcessamentoNotaFrequenciaFechamento>()
                        .WithIdentity(jobKey)
                        .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(string.Format("UniqueTriggerID{0}", i), groupProcess)
                        .StartNow()
                        .ForJob(jobDetail)
                        .Build();

                    context.Scheduler.ScheduleJob(jobDetail, trigger);
                }

            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoPendenciaAulas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoPendenciaAulas();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoAbonoFalta : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoAbonoFalta();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoAberturaTurmaAnosAnteriores : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoAberturaTurmaAnosAnteriores();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoDivergenciasRematriculas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Guid sle_id = SYS_ServicosLogExecucaoBO.IniciarServico(eChaveServicos.ProcessamentoDivergenciasRematriculas);
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoDivergenciasRematriculas(sle_id);
                SYS_ServicosLogExecucaoBO.FinalizarServio(sle_id);
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoSugestaoAulasPrevistas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Guid sle_id = SYS_ServicosLogExecucaoBO.IniciarServico(eChaveServicos.ProcessamentoSugestaoAulasPrevistas);
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoSugestaoAulasPrevistas();
                SYS_ServicosLogExecucaoBO.FinalizarServio(sle_id);
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoSugestaoAulasPrevistas_TodaRede : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Guid sle_id = SYS_ServicosLogExecucaoBO.IniciarServico(eChaveServicos.ProcessamentoSugestaoAulasPrevistasTodaRede);
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoSugestaoAulasPrevistas_TodaRede();
                SYS_ServicosLogExecucaoBO.FinalizarServio(sle_id);
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoDivergenciasAulasPrevistas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Guid sle_id = SYS_ServicosLogExecucaoBO.IniciarServico(eChaveServicos.ProcessamentoDivergenciasAulasPrevistas);
                GestaoEscolarServicosBO.MS_JOB_ProcessamentoDivergenciasAulasPrevistas();
                SYS_ServicosLogExecucaoBO.FinalizarServio(sle_id);
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AlertaPreenchimentoFrequencia : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AlertaPreenchimentoFrequencias();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AlertaInicioFechamento : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AlertaInicioFechamento();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AlertaFimFechamento : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AlertaFimFechamento();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AlertaAlunosBaixaFrequencia : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AlertaAlunosBaixaFrequencia();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_AlertaAlunosFaltasConsecutivas : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_AlertaAlunosFaltasConsecutivas();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoPreenchimentoFrequencia : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoPreenchimentoFrequencia();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoAlunosFrequencia : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecJOB_ProcessamentoAlunosFrequencia();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }

    public class MS_JOB_ProcessamentoAnaliseSondagemConsolidada : IJob
    {
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GestaoEscolarServicosBO.ExecMS_JOB_ProcessamentoAnaliseSondagemConsolidada();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, context.Scheduler.Context);
            }
        }

        #endregion IJob Members
    }
}