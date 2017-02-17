namespace GestaoEscolarServerScheduler
{
    partial class InstallerServerScheduler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SvcProcessInstallerServerScheduler = new System.ServiceProcess.ServiceProcessInstaller();
            this.SvcInstallerServerScheduler = new System.ServiceProcess.ServiceInstaller();
            // 
            // SvcProcessInstallerServerScheduler
            // 
            this.SvcProcessInstallerServerScheduler.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SvcProcessInstallerServerScheduler.Password = null;
            this.SvcProcessInstallerServerScheduler.Username = null;
            // 
            // SvcInstallerServerScheduler
            // 
            this.SvcInstallerServerScheduler.Description = "Gestão Escolar Server Scheduler ";
            this.SvcInstallerServerScheduler.DisplayName = "Gestão Escolar Server Scheduler ";
            this.SvcInstallerServerScheduler.ServiceName = "Gestão Escolar Server Scheduler";
            this.SvcInstallerServerScheduler.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.SvcInstallerServerScheduler.Committed += new System.Configuration.Install.InstallEventHandler(this.SvcInstallerServerScheduler_Committed);
            // 
            // InstallerServerScheduler
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SvcProcessInstallerServerScheduler,
            this.SvcInstallerServerScheduler});

        }


        #endregion


        private System.ServiceProcess.ServiceProcessInstaller SvcProcessInstallerServerScheduler;
        private System.ServiceProcess.ServiceInstaller SvcInstallerServerScheduler;
    }
}