using MSTech.GestaoEscolar.Jobs.GestaoAcademica;
using Quartz.Impl;

namespace MSTech.GestaoEscolar.Jobs.Schedulers
{
    /// <summary>
    /// Classe para Criar o scheduler do Quartz com as configurações necessárias para o Gestão Escolar.
    /// </summary>
    public class GestaoEscolarScheduler : ServerScheduler
    {
        public GestaoEscolarScheduler()
        {
            this.InstanceName = "GestaoEscolarScheduler";
            this.ConnectionStringName = "GestaoEscolar";
            this.BindName = "GestaoEscolarScheduler";
            this.Port = "555";
            this.Sis_ID = (int)CoreSSO.Entities.Enums.Sistemas.GestaoAcademica;
        }

        public GestaoEscolarScheduler(string port)
        {
            this.InstanceName = "GestaoEscolarScheduler";
            this.ConnectionStringName = "GestaoEscolar";
            this.BindName = "GestaoEscolarScheduler";
            this.Port = port;
            this.Sis_ID = (int)CoreSSO.Entities.Enums.Sistemas.GestaoAcademica;
        }

        /// <summary>
        /// Método para Incializar os Jobs no scheduler do Quartz.
        /// </summary>
        protected override void InicializaJobs()
        {
            JobDetailImpl jobDetail = new JobDetailImpl(typeof(MS_JOB_ArquivosExclusao).Name, typeof(MS_JOB_ArquivosExclusao));
            jobDetail.Durable = true;
            AddJob(jobDetail);
            
            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaAulas_DiarioClasse).Name, typeof(MS_JOB_AtualizaAulas_DiarioClasse));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaPlanejamento_DiarioClasse).Name, typeof(MS_JOB_AtualizaPlanejamento_DiarioClasse));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaLogs_DiarioClasse).Name, typeof(MS_JOB_AtualizaLogs_DiarioClasse));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaFoto_DiarioClasse).Name, typeof(MS_JOB_AtualizaFoto_DiarioClasse));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaCompensacao_DiarioClasse).Name, typeof(MS_JOB_AtualizaCompensacao_DiarioClasse));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_FechamentoRecalcularFrequenciaAulasPrevistas).Name, typeof(MS_JOB_FechamentoRecalcularFrequenciaAulasPrevistas));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_GeracaoHistoricoPedagogico).Name, typeof(MS_JOB_GeracaoHistoricoPedagogico));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_MatriculasBoletim_Atualizar).Name, typeof(MS_JOB_MatriculasBoletim_Atualizar));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaFrequenciaAjustadaFinal).Name, typeof(MS_JOB_AtualizaFrequenciaAjustadaFinal));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias).Name, typeof(MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoNotaFrequenciaFechamento).Name, typeof(MS_JOB_ProcessamentoNotaFrequenciaFechamento));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaFechamento_AberturaEvento).Name, typeof(MS_JOB_AtualizaFechamento_AberturaEvento));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoPendenciasAberturaEvento).Name, typeof(MS_JOB_ProcessamentoPendenciasAberturaEvento));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaAtribuicoesEsporadicas).Name, typeof(MS_JOB_AtualizaAtribuicoesEsporadicas));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_AtualizaIndicadorFrequencia).Name, typeof(MS_JOB_AtualizaIndicadorFrequencia));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoNotaFrequenciaFechamentoParalelo).Name, typeof(MS_JOB_ProcessamentoNotaFrequenciaFechamentoParalelo));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoPendenciaAulas).Name, typeof(MS_JOB_ProcessamentoPendenciaAulas));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoAbonoFalta).Name, typeof(MS_JOB_ProcessamentoAbonoFalta));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoAberturaTurmaAnosAnteriores).Name, typeof(MS_JOB_ProcessamentoAberturaTurmaAnosAnteriores));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoDivergenciasRematriculas).Name, typeof(MS_JOB_ProcessamentoDivergenciasRematriculas));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoSugestaoAulasPrevistas).Name, typeof(MS_JOB_ProcessamentoSugestaoAulasPrevistas));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoSugestaoAulasPrevistas_TodaRede).Name, typeof(MS_JOB_ProcessamentoSugestaoAulasPrevistas_TodaRede));
            jobDetail.Durable = true;
            AddJob(jobDetail);

            jobDetail = new JobDetailImpl(typeof(MS_JOB_ProcessamentoDivergenciasAulasPrevistas).Name, typeof(MS_JOB_ProcessamentoDivergenciasAulasPrevistas));
            jobDetail.Durable = true;
            AddJob(jobDetail);
        }
    }
}