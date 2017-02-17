using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace GestaoEscolarServerScheduler
{
    [RunInstaller(true)]
    public partial class InstallerServerScheduler : Installer
    {
        public InstallerServerScheduler()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            string nomeServico = this.Context.Parameters["nomeServico"];
            string port = this.Context.Parameters["porta"];

            if (!String.IsNullOrEmpty(nomeServico))
            {
                this.SvcInstallerServerScheduler.ServiceName = nomeServico;
                this.SvcInstallerServerScheduler.DisplayName = nomeServico;
            }

            if (!string.IsNullOrEmpty(port))
            {
                this.SvcInstallerServerScheduler.Description += ".\n Porta do scheduler: " + port;
            }

            string versao = this.Context.Parameters["versao"];
            this.SvcInstallerServerScheduler.Description += ".\n " + versao;

            base.Install(stateSaver);
        }

        protected override void OnCommitting(IDictionary savedState)
        {
            string nomeServico = this.Context.Parameters["nomeServico"];

            if (!String.IsNullOrEmpty(nomeServico))
            {
                this.SvcInstallerServerScheduler.ServiceName = nomeServico;
                this.SvcInstallerServerScheduler.DisplayName = nomeServico;
            }

            base.OnCommitting(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            string nomeServico = this.Context.Parameters["nomeServico"];

            if (!String.IsNullOrEmpty(nomeServico))
            {
                this.SvcInstallerServerScheduler.ServiceName = nomeServico;
                this.SvcInstallerServerScheduler.DisplayName = nomeServico;
            }

            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            string nomeServico = this.Context.Parameters["nomeServico"];

            if (!String.IsNullOrEmpty(nomeServico))
            {
                this.SvcInstallerServerScheduler.ServiceName = nomeServico;
                this.SvcInstallerServerScheduler.DisplayName = nomeServico;
            }

            base.Uninstall(savedState);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            try
            {
                //        //Verifica o caminho da instalação
                //        String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //        path = Path.GetDirectoryName(path);
                //        Directory.SetCurrentDirectory(path);
                //        string diretorio = Environment.CurrentDirectory;

                //        //Cria o arquivo config
                //        BaseBll.CriaArquivoConfig(diretorio);
                
                string nomeServico = this.Context.Parameters["nomeServico"];
                string port = this.Context.Parameters["porta"];
                
                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "Gestão escolar";
                appLog.WriteEntry("port: " + port);
                appLog.WriteEntry("nomeServico: " + nomeServico);

                //Inicia o serviço
                ServiceController sc = new ServiceController(SvcInstallerServerScheduler.ServiceName, Environment.MachineName);
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    if (!string.IsNullOrEmpty(port))
                    {
                        // Se passou a porta para iniciar o serviço, passa no args essa porta.
                        sc.Start(new[] { port });
                    }
                    else
                    {
                        sc.Start();
                    }
                }
            }
            catch
            {

            }

            base.OnAfterInstall(savedState);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            try
            {
                ////Verifica o caminho da instalação
                //String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //path = Path.GetDirectoryName(path);
                //Directory.SetCurrentDirectory(path);
                //string diretorio = Environment.CurrentDirectory;

                ////Cria o arquivo temporário
                //BaseBll.CriaArquivoTemp(diretorio);

                //Para o serviço
                ServiceController sc = new ServiceController(SvcInstallerServerScheduler.ServiceName, Environment.MachineName);

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch
            {

            }

            base.OnBeforeUninstall(savedState);
        }

        private void SvcInstallerServerScheduler_Committed(object sender, InstallEventArgs e)
        {
            string serviceName = this.SvcInstallerServerScheduler.ServiceName;
            string command = " D:(A;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCDCLCSWRPWPDTLOCRSDRCWDWO;;;BA)(A;;CCLCSWLOCRRC;;;IU)(A;;CCLCSWLOCRRC;;;SU)(A;;LCSWRPWP;;;AU)(A;;CCLCSWRPWPDTLOCRRC;;;PU)S:(AU;FA;CCDCLCSWRPWPDTLOCRSDRCWDWO;;;WD)";

            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("sc", " sdset " + serviceName + command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                proc.WaitForExit();

            }
            catch
            {
            }

            //try
            //{
            //    string displayName = SvcInstallerServerScheduler.DisplayName;
            //    System.Diagnostics.ProcessStartInfo procStartInfo =
            //         new System.Diagnostics.ProcessStartInfo("netsh", "firewall set portopening TCP 555 \"" + displayName + " - 555\"");

            //    procStartInfo.RedirectStandardOutput = true;
            //    procStartInfo.UseShellExecute = false;
            //    procStartInfo.CreateNoWindow = true;
            //    procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //    System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //    proc.StartInfo = procStartInfo;

            //    proc.Start();
            //    proc.WaitForExit();

            //}
            //catch 
            //{
            //}


        }
    }
}
