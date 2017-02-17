using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.Jobs.Schedulers
{
    /// <summary>
    /// Classe abstrata para manipulação do scheduler do Quartz, seguindo configurações 
    /// pré-estabelecida para os sistemas do Gestão Escolar.
    /// </summary>
    public abstract class ServerScheduler
    {
        /// <summary>
        /// Interface para acesso ao scheduler do Quartz.
        /// </summary>
        private IScheduler scheduler;

        /// <summary>
        /// Nome da Instância do Quartz
        /// </summary>
        protected string InstanceName { get; set; }

        /// <summary>
        /// Nome da connectionString que será utilizada pelo Quartz
        /// </summary>
        protected string ConnectionStringName { get; set; }

        /// <summary>
        /// Nome para ligar a instância do Quartz protocolo tcp e a porta
        /// </summary>
        protected string BindName { get; set; }

        /// <summary>
        /// Porta para publicar a instância do Quartz para acesso via tcp.
        /// </summary>
        protected string Port { get; set; }

        /// <summary>
        /// Armazena o sis_id do sistema.
        /// </summary>
        public int Sis_ID { get; set; }

        /// <summary>
        /// Inicializa a instância do Quartz.
        /// </summary>
        public void Start()
        {

            var properties = new NameValueCollection();

            //instance
            properties["quartz.scheduler.instanceName"] = this.InstanceName;
            properties["quartz.scheduler.instanceId"] = "AUTO";
            //thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "30";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            // job store
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.useProperties"] = "false";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "QTZ_";
            properties["quartz.jobStore.clustered"] = "true";
            properties["quartz.jobStore.selectWithLockSQL"] = "SELECT * FROM {0}LOCKS UPDLOCK WHERE LOCK_NAME = @lockName";
            //dataSource default
            properties["quartz.dataSource.default.connectionString"] = GetConnectionString();
            properties["quartz.dataSource.default.provider"] = "SqlServer-20";
            //remoting expoter
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = Port;
            properties["quartz.scheduler.exporter.bindName"] = BindName;
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            var schedulerFactory = new StdSchedulerFactory(properties);
            scheduler = schedulerFactory.GetScheduler();
                        
            InicializaJobs();

            scheduler.Start();

            scheduler.Context.Add("sis_id", this.Sis_ID);

            //Para não finalizar a execução
            System.Threading.Thread.Sleep(0);

        }

        /// <summary>
        /// Método para Incializar os Jobs no scheduler do Quartz.
        /// </summary>
        protected abstract void InicializaJobs();

        /// <summary>
        /// Para o scheduler do Quartz.
        /// </summary>
        public void Stop()
        {
            scheduler.Shutdown();
        }

        /// <summary>
        /// Recupera a connectionString da TalkDBTransactionCollection
        /// </summary>
        /// <returns>ConnectionString</returns>
        private string GetConnectionString()
        {
            TalkDBTransactionCollection tkCollection = new TalkDBTransactionCollection();
            return tkCollection[ConnectionStringName].GetConnection.ConnectionString;
        }

        /// <summary>
        /// Adiciona um Job no scheduler do Quartz.
        /// </summary>
        /// <param name="jobDetail">Job implementado</param>
        protected void AddJob(JobDetailImpl jobDetail)
        {
            scheduler.AddJob(jobDetail, true);
        }
    }

}
