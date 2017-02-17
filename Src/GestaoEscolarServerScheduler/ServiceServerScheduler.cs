using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using MSTech.GestaoEscolar.Jobs.Schedulers;
using MSTech.GestaoEscolar.Jobs;
using System.Configuration;
using System.IO;

namespace GestaoEscolarServerScheduler
{
    partial class ServiceServerScheduler : ServiceBase
    {
        public ServiceServerScheduler()
        {
            InitializeComponent();
        }

        private ServerScheduler scheduler;

        protected override void OnStart(string[] args)
        {
            try
            {
                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "Gestão escolar";

                if (args.Length > 0)
                {
                    try
                    {
                        System.Configuration.Configuration config = ConfigurationManager.
                            OpenExeConfiguration(ConfigurationUserLevel.None);

                        if (config == null)
                            appLog.WriteEntry("config null");

                        config.AppSettings.Settings.Remove("porta");

                        // Primeiro argumento é a porta.
                        config.AppSettings.Settings.Add("porta", args[0]);

                        config.Save(ConfigurationSaveMode.Modified);
                    }
                    catch (Exception ex)
                    {
                        appLog.WriteEntry("Erro ao alterar o config: " + ex.Message);
                    }
                }

                string porta = ConfigurationManager.AppSettings.Get("porta");

                if (!string.IsNullOrEmpty(porta))
                {
                   
                    appLog.WriteEntry("Iniciando serviço na porta: " + porta);
                    scheduler = new GestaoEscolarScheduler(porta);
                }
                else
                {
                    scheduler = new GestaoEscolarScheduler();
                }
                
                scheduler.Start();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, scheduler.Sis_ID);
            }
        }

        protected override void OnStop()
        {
            try
            {
                scheduler.Stop();
            }
            catch (Exception ex)
            {
                Util.GravarErro(ex, scheduler.Sis_ID);
            }
        }
    }
}

