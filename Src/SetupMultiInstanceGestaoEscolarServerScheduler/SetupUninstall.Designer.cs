namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    partial class SetupUninstall
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
            this.lblListaServico = new System.Windows.Forms.Label();
            this.btnAvancar = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgresso = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.listService = new System.Windows.Forms.ListBox();
            this.chkDeleteFilesConfig = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblListaServico
            // 
            this.lblListaServico.AutoSize = true;
            this.lblListaServico.Location = new System.Drawing.Point(12, 9);
            this.lblListaServico.Name = "lblListaServico";
            this.lblListaServico.Size = new System.Drawing.Size(48, 13);
            this.lblListaServico.TabIndex = 4;
            this.lblListaServico.Text = "Serviços";
            // 
            // btnAvancar
            // 
            this.btnAvancar.Enabled = false;
            this.btnAvancar.Location = new System.Drawing.Point(216, 209);
            this.btnAvancar.Name = "btnAvancar";
            this.btnAvancar.Size = new System.Drawing.Size(75, 23);
            this.btnAvancar.TabIndex = 7;
            this.btnAvancar.Text = "Desinstalar";
            this.btnAvancar.UseVisualStyleBackColor = true;
            this.btnAvancar.Click += new System.EventHandler(this.btnAvancar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(297, 209);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 6;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(-1, 238);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(387, 25);
            this.progressBar.TabIndex = 8;
            // 
            // lblProgresso
            // 
            this.lblProgresso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.BackColor = System.Drawing.SystemColors.Control;
            this.lblProgresso.Location = new System.Drawing.Point(12, 222);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(0, 13);
            this.lblProgresso.TabIndex = 9;
            this.lblProgresso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listService
            // 
            this.listService.FormattingEnabled = true;
            this.listService.Location = new System.Drawing.Point(15, 25);
            this.listService.Name = "listService";
            this.listService.Size = new System.Drawing.Size(357, 173);
            this.listService.TabIndex = 11;
            this.listService.SelectedIndexChanged += new System.EventHandler(this.listService_SelectedIndexChanged);
            this.listService.Enter += new System.EventHandler(this.listService_Enter);
            // 
            // chkDeleteFilesConfig
            // 
            this.chkDeleteFilesConfig.AutoSize = true;
            this.chkDeleteFilesConfig.Location = new System.Drawing.Point(15, 202);
            this.chkDeleteFilesConfig.Name = "chkDeleteFilesConfig";
            this.chkDeleteFilesConfig.Size = new System.Drawing.Size(180, 17);
            this.chkDeleteFilesConfig.TabIndex = 12;
            this.chkDeleteFilesConfig.Text = "Excluir arquivos de configuração";
            this.chkDeleteFilesConfig.UseVisualStyleBackColor = true;
            // 
            // SetupUninstall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.chkDeleteFilesConfig);
            this.Controls.Add(this.listService);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnAvancar);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.lblListaServico);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupUninstall";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SetupUninstall";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupUninstall_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblListaServico;
        private System.Windows.Forms.Button btnAvancar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgresso;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ListBox listService;
        private System.Windows.Forms.CheckBox chkDeleteFilesConfig;
    }
}