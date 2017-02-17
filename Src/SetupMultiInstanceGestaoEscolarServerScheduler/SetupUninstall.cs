namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    using SetupMultiInstanceGestaoEscolarServerScheduler.Infra;
    using System;
    using System.Configuration.Install;
    using System.IO;
    using System.Management;
    using System.ServiceProcess;
    using System.Windows.Forms;
    using System.Linq;
    using System.Collections.Specialized;

    public partial class SetupUninstall : Form
    {
        #region Propriedades

        private Form frmSetup;
        private ServiceInstaller serviceInstaller;
        private bool sucesso = false;
        private ManagementObject managementObj = null;
        public string serviceName;
        private bool deleteConfigFiles = true;

        #endregion Propriedades

        #region Construtor

        public SetupUninstall(Form form)
        {
            InitializeComponent();
            frmSetup = form;

            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            listService.Items.AddRange(servicos.Where(p => p.DisplayName.Contains(Constantes.NomeServico)).Select(n => n.DisplayName.ToString()).ToArray());

            serviceInstaller = new ServiceInstaller();
            serviceInstaller.BeforeUninstall += serviceInstaller_BeforeUninstall;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        #endregion Construtor

        #region Eventos

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            string mensagem = sucesso ?
                String.Format("Desinstalação do {0} realizada com sucesso. Clique em finalizar para fechar.", selectService().Trim()) :
                String.Format("Ocorreu um erro durante a desinstalação do {0}. Clique em finalizar para fechar.", selectService().Trim());

            FinishSetup finishFrm = new FinishSetup(mensagem);
            this.Hide();
            finishFrm.Show();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string objPathName = DesinstalarServico(managementObj);

                ExcluirDiretorio(objPathName, deleteConfigFiles);

                sucesso = true;
            }
            catch
            {
                sucesso = false;

                RollBack(managementObj);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmSetup.Show();
            this.Close();
        }

        private void btnAvancar_Click(object sender, EventArgs e)
        {

            lblProgresso.Text = "Desisntalando o serviço...";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 10;

            serviceName = selectService().Trim();
            deleteConfigFiles = chkDeleteFilesConfig.Checked;

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
                    FinishSetup finishFrm = new FinishSetup(String.Format("Ocorreu um erro durante a desinstalação do {0}. Clique em finalizar para fechar.", serviceName));
                    this.Hide();
                    finishFrm.Show();
                }
            }
        }

        private void serviceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
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

        private void SetupUninstall_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSetup.Show();
        }

        #endregion Eventos

        #region Métodos


        private string selectService()
        {
            return listService.SelectedItem == null ? "" : listService.SelectedItem.ToString().Trim();
        }

        /// <summary>
        /// Realiza o rollback das operações caso ocorrer algum erro.
        /// </summary>
        /// <param name="managementObj"></param>
        private void RollBack(ManagementObject managementObj)
        {
            ServiceController[] servicos = ServiceController.GetServices(Environment.MachineName);
            if (servicos != null && !servicos.Any(s => s.ServiceName.Trim() == selectService().Trim()))
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
        /// Exclui os diretórios e arquivos referente ao serviço.
        /// </summary>
        /// <param name="caminho">Caminho do executável do serviço.</param>
        private void ExcluirDiretorio(string caminho, bool deleteConfig = true)
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
                    string[] configFiles = new string[] { "MSTech.Data.Common.dll.config" };

                    FileInfo[] files = directory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (!configFiles.Any(f => f == file.Name))
                            file.Delete();
                    }
                }


            }
        }

        /// <summary>
        /// Desinstala o serviço.
        /// </summary>
        /// <param name="managementObj">Objeto ManagementObject referente ao serviço.</param>
        /// <returns></returns>
        private string DesinstalarServico(ManagementObject managementObj)
        {
            string caminho = managementObj.GetPropertyValue("PathName").ToString().Replace("\"", "");
            string caminhoAssembly = String.Format("/assemblypath={0}", caminho);
            String[] cmdline = { caminhoAssembly, serviceName };
            serviceInstaller.Context = new InstallContext("", cmdline);
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.Uninstall(null);

            return caminho;
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

        #endregion Métodos
    }
}
