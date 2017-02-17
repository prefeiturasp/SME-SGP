namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    using System;
    using System.Data;
    using System.Linq;
    using System.ServiceProcess;
    using System.Windows.Forms;
    using SetupMultiInstanceGestaoEscolarServerScheduler.Infra;
    using System.Management;
    using System.Configuration.Install;
    using System.Collections.Specialized;
    using System.IO;
    using System.Collections.Generic;
    using System.Diagnostics;
    using GestaoEscolarServerScheduler;
    using System.Xml;

    public partial class SetupUpdate : Form
    {
        #region Propriedades

        private Form frmSetup;
        private ServiceInstaller serviceInstaller;
        private bool successAll = false;
        private bool successInstall = false;
        private bool successUninstall = false;
        private ManagementObject managementObj = null;
        public string serviceName;
        public string objPathName;

        #endregion Propriedades

        #region Construtor

        public SetupUpdate(Form form)
        {
            InitializeComponent();
            frmSetup = form;

            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            listService.Items.AddRange(servicos.Where(p => p.DisplayName.Contains(Constantes.NomeServico)).Select(n => n.DisplayName.ToString()).ToArray());

            serviceInstaller = new ServiceInstaller();

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        #endregion Construtor

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            string mensagem = successAll ?
                String.Format("Atualização do {0} realizada com sucesso. Clique em finalizar para fechar.", selectService().Trim()) :
                String.Format("Ocorreu um erro durante a atualização do {0}. Clique em finalizar para fechar.", selectService().Trim());

            FinishSetup finishFrm = new FinishSetup(mensagem);
            this.Hide();
            finishFrm.Show();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            try
            {
                PararServico(managementObj, out objPathName);

                int cAttempts = 0;

                while (cAttempts < 3)
                {
                    successUninstall = ExcluirDiretorio(objPathName, false);
                    if (successUninstall)
                    {
                        cAttempts = 3;
                    }
                }

                successUninstall &= true;
            }
            catch
            {
                successUninstall = false;
            }

            try
            {
                CopiarArquivos(objPathName);

                InstalarServico(objPathName, serviceName);

                successInstall = true;
            }
            catch
            {
                successInstall = false;
            }

            successAll = successInstall && successUninstall;

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmSetup.Show();
            this.Close();
        }

        private void btnAvancar_Click(object sender, EventArgs e)
        {
            lblProgresso.Text = "Atualizando o serviço...";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 10;

            serviceName = selectService().Trim();

            string mensagem;
            if (ValidarDados(out mensagem, out managementObj) && managementObj != null)
            {
                this.Enabled = false;

                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.MarqueeAnimationSpeed = 0;
                lblProgresso.Text = string.Empty;

                if (!string.IsNullOrEmpty(mensagem))
                {
                    MessageBox.Show(mensagem);
                }
                else
                {
                    FinishSetup finishFrm = new FinishSetup(String.Format("Ocorreu um erro durante a atualização do {0}. Clique em finalizar para fechar.", serviceName));
                    this.Hide();
                    finishFrm.Show();
                }
            }
        }

        private void listService_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectService()))
                btnAvancar.Enabled = false;
            else
                btnAvancar.Enabled = true;
        }

        private void listService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectService()))
                btnAvancar.Enabled = false;
            else
                btnAvancar.Enabled = true;
        }

        private void SetupUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSetup.Show();
        }

        #region Métodos

        private string selectService()
        {
            return listService.SelectedItem == null ? "" : listService.SelectedItem.ToString().Trim();
        }

        private bool StopService(ServiceInstaller si)
        {
            using (ServiceController sc = new ServiceController(si.ServiceName, Environment.MachineName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                return sc.Status == ServiceControllerStatus.Stopped;
            }
        }

        private bool StartService(ServiceInstaller si)
        {
            using (ServiceController sc = new ServiceController(si.ServiceName, Environment.MachineName))
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                }

                return sc.Status == ServiceControllerStatus.Running;
            }
        }

        /// <summary>
        /// Realiza o rollback das operações caso ocorrer algum erro.
        /// </summary>
        /// <param name="managementObj"></param>
        private void RollBackUninstall(ManagementObject managementObj)
        {
            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            if (servicos != null && !servicos.Any(s => s.ServiceName.Trim() == serviceName))
            {
                serviceInstaller.Install(new ListDictionary());
            }

            if (managementObj != null)
            {
                string objPathName = managementObj.GetPropertyValue("PathName").ToString().Replace("\"", "");

                if (!string.IsNullOrEmpty(objPathName))
                {
                    DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(objPathName));

                    if (!directory.Exists)
                    {
                        directory.Create();
                        string binPath = Path.GetDirectoryName(Application.ExecutablePath);

                        string[] extensions = new string[] { ".dll", ".exe", ".config" };

                        string[] files = Directory.GetFiles(binPath, "*.*", SearchOption.AllDirectories).Where(p => extensions.Any(q => q.Equals(Path.GetExtension(p)))).ToArray();

                        foreach (string file in files)
                        {
                            File.Copy(file, Path.Combine(directory.FullName, Path.GetFileName(file)), true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Realiza o rollback das operações caso ocorrer algum erro.
        /// </summary>
        private void RollBackInstall(string CaminhoInstalacao)
        {
            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            if (servicos != null && servicos.Any(s => s.ServiceName.Trim() == serviceName))
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

        /// <summary>
        /// para o serviço.
        /// </summary>
        /// <param name="managementObj">Objeto ManagementObject referente ao serviço.</param>
        /// <returns></returns>
        private void PararServico(ManagementObject managementObj, out string path)
        {
            path = managementObj.GetPropertyValue("PathName").ToString().Replace("\"", "");
            string caminhoAssembly = String.Format("/assemblypath={0}", path);
            String[] cmdline = { caminhoAssembly, serviceName };

            serviceInstaller.Context = new InstallContext("", cmdline);
            serviceInstaller.ServiceName = serviceName;
            StopService(serviceInstaller);
            serviceInstaller.Dispose();
        }

        /// <summary>
        /// Validar dados inseridos pelo usuário.
        /// </summary>
        /// <param name="mensagem">Mensagem de validação.</param>
        /// <param name="managementObj">Objeto ManagementObject referente ao serviço.</param>
        /// <returns></returns>
        private bool ValidarDados(out string mensagem, out ManagementObject managementObj)
        {
            mensagem = string.Empty;
            managementObj = new ManagementObject();

            if (string.IsNullOrEmpty(serviceName))
            {
                mensagem = "Nome do serviço é obrigatório.";
                return false;
            }

            WqlObjectQuery wqlObjectQuery = new WqlObjectQuery(string.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", serviceName));
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(wqlObjectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();

            foreach (ManagementObject obj in managementObjectCollection)
            {
                managementObj = obj;
                return true;
            }

            mensagem = "Não foi encontrado um serviço com este nome.";
            return false;
        }

        /// <summary>
        /// Exclui os diretórios e arquivos referente ao serviço.
        /// </summary>
        /// <param name="caminho">Caminho do executável do serviço.</param>
        private bool ExcluirDiretorio(string caminho, bool deleteConfig = true)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(caminho));
                if (directory.Exists)
                {
                    if (deleteConfig)
                    {
                        directory.Delete(true);
                    }
                    else
                    {
                        string[] configFiles = new string[] { "MSTech.Data.Common.dll.config", (Constantes.NomeExecutavel + ".config") };

                        FileInfo[] files = directory.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            if (!configFiles.Any(f => f == file.Name))
                                file.Delete();
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Realiza a cópia dos arquivos necessários para a execução do serviço.
        /// </summary>
        private void CopiarArquivos(string CaminhoInstalacao)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(CaminhoInstalacao));
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
            files.AddRange(Directory.GetFiles(binPath, "version.xml", SearchOption.AllDirectories));

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
        private void InstalarServico(string CaminhoInstalacao, string NomeServico)
        {
            string caminho = String.Format("/assemblypath={0}",
                Path.Combine(Path.GetDirectoryName(CaminhoInstalacao), Constantes.NomeExecutavel));
            String[] cmdline = { caminho };

            serviceInstaller.Context = new InstallContext("", cmdline);
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.Context.Parameters.Remove("versao");
            serviceInstaller.Context.Parameters.Add("versao", GetVersaoSistema());
            StartService(serviceInstaller);
            serviceInstaller.Dispose();
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
                          let teste = pi.Value.Split('=')
                          let dummyPi = (XmlElement)pi.OwnerDocument.ReadNode(XmlReader.Create(new StringReader("<" + teste[0] + " Valor=" + teste[1] + "/>")))
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
        /// Lê o arquivo version.xml e retorna a string com a versão do sistema.
        /// </summary>
        /// <returns></returns>
        #endregion Métodos

        private void listService_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectService()))
                btnAvancar.Enabled = false;
            else
                btnAvancar.Enabled = true;
        }

        private void listService_Enter_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectService()))
                btnAvancar.Enabled = false;
            else
                btnAvancar.Enabled = true;
        }
    }
}