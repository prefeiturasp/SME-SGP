namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    partial class SetupInstall
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnAvancar = new System.Windows.Forms.Button();
            this.lblNomeServico = new System.Windows.Forms.Label();
            this.txtNomeServico = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lblCaminho = new System.Windows.Forms.Label();
            this.txtCaminho = new System.Windows.Forms.TextBox();
            this.btnProcurarCaminho = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgresso = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrefixoNome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAvaliablePorts = new System.Windows.Forms.ComboBox();
            this.txtPorta = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnVoltar
            // 
            this.btnVoltar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnVoltar.Location = new System.Drawing.Point(297, 209);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 4;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // btnAvancar
            // 
            this.btnAvancar.Location = new System.Drawing.Point(216, 209);
            this.btnAvancar.Name = "btnAvancar";
            this.btnAvancar.Size = new System.Drawing.Size(75, 23);
            this.btnAvancar.TabIndex = 5;
            this.btnAvancar.Text = "Instalar";
            this.btnAvancar.UseVisualStyleBackColor = true;
            this.btnAvancar.Click += new System.EventHandler(this.btnAvancar_Click);
            // 
            // lblNomeServico
            // 
            this.lblNomeServico.AutoSize = true;
            this.lblNomeServico.Location = new System.Drawing.Point(12, 26);
            this.lblNomeServico.Name = "lblNomeServico";
            this.lblNomeServico.Size = new System.Drawing.Size(87, 13);
            this.lblNomeServico.TabIndex = 2;
            this.lblNomeServico.Text = "Nome do serviço";
            // 
            // txtNomeServico
            // 
            this.txtNomeServico.Location = new System.Drawing.Point(200, 45);
            this.txtNomeServico.Name = "txtNomeServico";
            this.txtNomeServico.Size = new System.Drawing.Size(126, 20);
            this.txtNomeServico.TabIndex = 1;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // lblCaminho
            // 
            this.lblCaminho.AutoSize = true;
            this.lblCaminho.Location = new System.Drawing.Point(12, 90);
            this.lblCaminho.Name = "lblCaminho";
            this.lblCaminho.Size = new System.Drawing.Size(185, 13);
            this.lblCaminho.TabIndex = 4;
            this.lblCaminho.Text = "Caminho raiz de instalação do serviço";
            // 
            // txtCaminho
            // 
            this.txtCaminho.Location = new System.Drawing.Point(15, 106);
            this.txtCaminho.Name = "txtCaminho";
            this.txtCaminho.Size = new System.Drawing.Size(357, 20);
            this.txtCaminho.TabIndex = 2;
            // 
            // btnProcurarCaminho
            // 
            this.btnProcurarCaminho.Location = new System.Drawing.Point(297, 132);
            this.btnProcurarCaminho.Name = "btnProcurarCaminho";
            this.btnProcurarCaminho.Size = new System.Drawing.Size(75, 23);
            this.btnProcurarCaminho.TabIndex = 6;
            this.btnProcurarCaminho.Text = "Procurar";
            this.btnProcurarCaminho.UseVisualStyleBackColor = true;
            this.btnProcurarCaminho.Click += new System.EventHandler(this.btnProcurarCaminho_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(-1, 238);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(386, 23);
            this.progressBar.TabIndex = 7;
            // 
            // lblProgresso
            // 
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.Location = new System.Drawing.Point(12, 222);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(0, 13);
            this.lblProgresso.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Porta do scheduler (portas disponíveis entre 555 e 600)";
            // 
            // txtPrefixoNome
            // 
            this.txtPrefixoNome.Enabled = false;
            this.txtPrefixoNome.Location = new System.Drawing.Point(15, 45);
            this.txtPrefixoNome.Name = "txtPrefixoNome";
            this.txtPrefixoNome.ReadOnly = true;
            this.txtPrefixoNome.Size = new System.Drawing.Size(163, 20);
            this.txtPrefixoNome.TabIndex = 11;
            this.txtPrefixoNome.Text = "Gestão escolar server scheduler";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "-";
            // 
            // cmbAvaliablePorts
            // 
            this.cmbAvaliablePorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAvaliablePorts.FormattingEnabled = true;
            this.cmbAvaliablePorts.Location = new System.Drawing.Point(15, 174);
            this.cmbAvaliablePorts.MaxDropDownItems = 5;
            this.cmbAvaliablePorts.Name = "cmbAvaliablePorts";
            this.cmbAvaliablePorts.Size = new System.Drawing.Size(84, 21);
            this.cmbAvaliablePorts.TabIndex = 14;
            this.cmbAvaliablePorts.SelectedIndexChanged += new System.EventHandler(this.cmbAvaliablePorts_SelectedIndexChanged);
            // 
            // txtPorta
            // 
            this.txtPorta.Enabled = false;
            this.txtPorta.Location = new System.Drawing.Point(332, 45);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.ReadOnly = true;
            this.txtPorta.Size = new System.Drawing.Size(40, 20);
            this.txtPorta.TabIndex = 15;
            // 
            // SetupInstall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.txtPorta);
            this.Controls.Add(this.cmbAvaliablePorts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPrefixoNome);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnProcurarCaminho);
            this.Controls.Add(this.txtCaminho);
            this.Controls.Add(this.lblCaminho);
            this.Controls.Add(this.txtNomeServico);
            this.Controls.Add(this.lblNomeServico);
            this.Controls.Add(this.btnAvancar);
            this.Controls.Add(this.btnVoltar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupInstall";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SetupInstall";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupInstall_FormClosed);
            this.Load += new System.EventHandler(this.SetupInstall_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnAvancar;
        private System.Windows.Forms.Label lblNomeServico;
        private System.Windows.Forms.TextBox txtNomeServico;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label lblCaminho;
        private System.Windows.Forms.TextBox txtCaminho;
        private System.Windows.Forms.Button btnProcurarCaminho;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgresso;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrefixoNome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAvaliablePorts;
        private System.Windows.Forms.TextBox txtPorta;
    }
}