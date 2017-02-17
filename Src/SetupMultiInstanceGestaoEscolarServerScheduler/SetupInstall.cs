namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    using GestaoEscolarServerScheduler;
    using SetupMultiInstanceGestaoEscolarServerScheduler.Infra;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.ServiceProcess;
    using System.Windows.Forms;
    using System.Xml;

    public partial class SetupInstall : Form
    {
        #region Propriedades

        private Form frmSetup;
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;
        private bool sucesso = false;
        private int servicePort;

        private string CaminhoInstalacao
        {
            get
            {
                return Path.Combine(txtCaminho.Text.Trim(), NomeServico);
            }
        }

        /// <summary>
        /// Retorna o nome completo do serviço.
        /// </summary>
        private string NomeServico
        {
            get
            {
                return txtPrefixoNome.Text +
                    (string.IsNullOrEmpty(txtNomeServico.Text.Trim()) ? "" : " - " + txtNomeServico.Text.Trim()) +
                    (string.IsNullOrEmpty(txtPorta.Text.Trim()) ? "" : "(" + txtPorta.Text.Trim() + ")");

            }
        }

        #endregion Propriedades

        #region Construtor

        public SetupInstall(Form form)
        {
            InitializeComponent();
            frmSetup = form;

            txtPrefixoNome.Text = Constantes.NomeServico;
            txtCaminho.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), Constantes.NomeEmpresa);

            cmbAvaliablePorts.Items.AddRange(ServiceUtilities.GetAvailablePorts().Cast<object>().ToArray());
            cmbAvaliablePorts.DropDownHeight = 106;
            cmbAvaliablePorts.SelectedIndex = 0;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            serviceInstaller.AfterInstall += serviceInstaller_AfterInstall;
            serviceInstaller.Committed += serviceInstaller_Committed;
            serviceInstaller.BeforeRollback += serviceInstaller_BeforeRollback;
        }

        #endregion Construtor

        #region Eventos

        private void SetupInstall_Load(object sender, EventArgs e)
        {
            txtNomeServico.Focus();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            string mensagem = sucesso ?
                String.Format("Instalação do {0} realizada com sucesso. Clique em finalizar para fechar.", NomeServico) :
                String.Format("Ocorreu um erro durante a instalação do {0}. Clique em finalizar para fechar.", NomeServico);

            FinishSetup finishFrm = new FinishSetup(mensagem);
            this.Hide();
            finishFrm.Show();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                CopiarArquivos();

                ConfigurarBancoConfig();

                InstalarServico();

                sucesso = true;
            }
            catch
            {
                sucesso = false;

                RollBack();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmSetup.Show();
            this.Close();
        }

        private void SetupInstall_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSetup.Show();
        }

        private void btnProcurarCaminho_Click(object sender, EventArgs e)
        {
            DialogResult resultado = folderBrowserDialog.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                txtCaminho.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnAvancar_Click(object sender, EventArgs e)
        {
            lblProgresso.Text = "Instalando o serviço...";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 10;
            
            string mensagem;
            if (ValidarDados(out mensagem))
            {
                this.Enabled = false;

                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.MarqueeAnimationSpeed = 0;
                lblProgresso.Text = string.Empty;

                MessageBox.Show(mensagem);
            }
        }

        private void serviceInstaller_BeforeRollback(object sender, InstallEventArgs e)
        {
            ServiceInstaller si = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(si.ServiceName, Environment.MachineName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }

        private void serviceInstaller_Committed(object sender, InstallEventArgs e)
        {
            ServiceInstaller si = (ServiceInstaller)sender;
            string serviceName = si.ServiceName;
            string command = " D:(A;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCDCLCSWRPWPDTLOCRSDRCWDWO;;;BA)(A;;CCLCSWLOCRRC;;;IU)(A;;CCLCSWLOCRRC;;;SU)(A;;LCSWRPWP;;;AU)(A;;CCLCSWRPWPDTLOCRRC;;;PU)S:(AU;FA;CCDCLCSWRPWPDTLOCRSDRCWDWO;;;WD)";

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

        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller si = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(si.ServiceName, Environment.MachineName))
            {
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    sc.Start();
                }
            }
        }

        private void cmbAvaliablePorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPorta.Text = cmbAvaliablePorts.Text;
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Valida os dados digitados.
        /// </summary>
        /// <param name="mensagem">Mensagem de retorno da validação.</param>
        /// <returns></returns>
        private bool ValidarDados(out string mensagem)
        {
            mensagem = string.Empty;

            if (string.IsNullOrEmpty(NomeServico))
            {
                mensagem = "Nome do serviço é obrigatório.";
                return false;
            }

            if (string.IsNullOrEmpty(txtCaminho.Text))
            {
                mensagem = "Caminho de instalação raiz do serviço é obrigatório.";
                return false;
            }

            servicePort = (int)cmbAvaliablePorts.SelectedItem;

            if (servicePort < 555 || servicePort > 600)
            {
                mensagem = "Porta inválida.";
                return false;
            }

            if (ServiceUtilities.PortInUse(servicePort))
            {
                mensagem = "A porta já esta em uso.";
                return false;
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
            ManagementObjectCollection servicos = searcher.Get();


            foreach (ManagementObject obj in servicos)
            {
                if (obj["Name"].ToString().Trim().Equals(NomeServico))
                {
                    mensagem = "Já existe um serviço com este nome.\r\n";
                }

                if (string.IsNullOrEmpty(obj["PathName"].ToString()) ? false : obj["PathName"].ToString().Trim().Equals(CaminhoInstalacao.Trim()))
                {
                    mensagem += "Já existe um serviço instalado neste caminho.";
                }

                if (!string.IsNullOrEmpty(mensagem))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Realiza a chamada do executável que configura a conexão do serviço com o banco de dados e espera o retorno do mesmo.
        /// </summary>
        private void ConfigurarBancoConfig()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Path.Combine(CaminhoInstalacao, Constantes.NomeConfigExecutavel);
            start.WindowStyle = ProcessWindowStyle.Normal;

            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();
            }
        }

        /// <summary>
        /// Realiza a cópia dos arquivos necessários para a execução do serviço.
        /// </summary>
        private void CopiarArquivos()
        {
            DirectoryInfo directory = new DirectoryInfo(CaminhoInstalacao);
            if (!directory.Exists)
            {
                directory.Create();
            }

            string binPath = Path.GetDirectoryName(Application.ExecutablePath);

            string[] extensions = new string[] { ".dll", ".exe", ".config" };

            List<string> files =
                Directory.GetFiles(binPath, "*.*", SearchOption.AllDirectories)
                .Where(p => extensions.Any(e => e.Equals(Path.GetExtension(p)))).ToList();

            binPath = Path.Combine(binPath, "Arquivos");
            files.AddRange(Directory.GetFiles(binPath, "*.*", SearchOption.AllDirectories)
                .Where(p => extensions.Any(e => e.Equals(Path.GetExtension(p)))).ToList());

            // Busca o arquivo de versão também.
            // files.AddRange(Directory.GetFiles(binPath, "version.xml", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(binPath, "Versao.wxi", SearchOption.AllDirectories));


            foreach (string file in files)
            {
                string destFile = Path.Combine(directory.FullName, Path.GetFileName(file));

                if (file.EndsWith(".config"))
                {
                    // Só copia o arquivo de config se ele não existir.
                    if (!File.Exists(destFile))
                    {
                        File.Copy(file, destFile, false);
                    }
                }
                else
                {
                    File.Copy(file, destFile, true);
                }
            }


        }

        /// <summary>
        /// Instala o serviço.
        /// </summary>
        private void InstalarServico()
        {
            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Username = null;
            processInstaller.Password = null;

            string caminho = String.Format("/assemblypath={0}",
                Path.Combine(CaminhoInstalacao, Constantes.NomeExecutavel));
            String[] cmdline = { caminho };

            InstallerServerScheduler installer = new InstallerServerScheduler();
            installer.Context = new InstallContext("", cmdline);
            installer.Context.Parameters.Add("nomeServico", NomeServico);
            installer.Context.Parameters.Add("porta", servicePort.ToString());
            installer.Context.Parameters.Add("versao", GetVersaoSistema());
            installer.Install(new ListDictionary());
            installer.Dispose();
        }

        /// <summary>
        /// Lê o arquivo version.xml e retorna a string com a versão do sistema.
        /// </summary>
        /// <returns></returns>
        private string GetVersaoSistema()
        {
            XmlDocument xmlDoc = new XmlDocument();
            String strRet = String.Empty;

            string binPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Arquivos");

            xmlDoc.Load(binPath + "\\Versao.wxi");

            XmlNode xmlNd = xmlDoc.SelectSingleNode("//Include");

            var ltNodes = from XmlNode nd in xmlNd.ChildNodes
                          where nd is XmlProcessingInstruction
                          let pi = (XmlProcessingInstruction)nd
                          let noXml = pi.Value.Split('=')
                          let dummyPi = (XmlElement)pi.OwnerDocument.ReadNode(XmlReader.Create(new StringReader("<" + noXml[0] + " Valor=" + noXml[1] + "/>")))
                          select dummyPi;

            if (ltNodes != null && ltNodes.Any())
            {
                string major = ltNodes.Where(p => p.Name == "Major").FirstOrDefault().Attributes["Valor"].Value;
                string minor = ltNodes.Where(p => p.Name == "Minor").FirstOrDefault().Attributes["Valor"].Value;
                string revision = ltNodes.Where(p => p.Name == "Revision").FirstOrDefault().Attributes["Valor"].Value;
                string build = ltNodes.Where(p => p.Name == "Build").FirstOrDefault().Attributes["Valor"].Value;

                strRet = String.Format("Versão: {0}.{1}.{2}.{3}", major, minor, revision, build);
            }


            return strRet;
        }

        /// <summary>
        /// Realiza o rollback das operações caso ocorrer algum erro.
        /// </summary>
        private void RollBack()
        {

            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            if (servicos != null && servicos.Any(s => s.ServiceName.Trim() == NomeServico))
            {
                serviceInstaller.Uninstall(null);
            }

            if (!string.IsNullOrEmpty(CaminhoInstalacao))
            {
                DirectoryInfo directory = new DirectoryInfo(CaminhoInstalacao);
                if (directory.Exists)
                {
                    directory.Delete(true);
                }
            }
        }

        #endregion Métodos


    }
}
