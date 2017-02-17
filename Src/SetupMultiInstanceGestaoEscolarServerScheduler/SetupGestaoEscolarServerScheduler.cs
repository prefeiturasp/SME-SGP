namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    using System;
    using System.Windows.Forms;

    public partial class SetupGestaoEscolarServerScheduler : Form
    {
        public SetupGestaoEscolarServerScheduler()
        {
            InitializeComponent();
        }

        private void lnkInstall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetupInstall installFrm = new SetupInstall(this);
            installFrm.Show();
            this.Hide();
        }

        private void lnkUninstall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetupUninstall uninstallFrm = new SetupUninstall(this);
            uninstallFrm.Show();
            this.Hide();
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetupUpdate updateFrm = new SetupUpdate(this);
            updateFrm.Show();
            this.Hide();
        }
    }
}
