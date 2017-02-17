namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    partial class SetupUpdate
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
            this.listService = new System.Windows.Forms.ListBox();
            this.btnAvancar = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.lblListaServico = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lblProgresso = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listService
            // 
            this.listService.FormattingEnabled = true;
            this.listService.Location = new System.Drawing.Point(12, 25);
            this.listService.Name = "listService";
            this.listService.Size = new System.Drawing.Size(357, 173);
            this.listService.TabIndex = 15;
            this.listService.TabStop = false;
            this.listService.SelectedIndexChanged += new System.EventHandler(this.listService_SelectedIndexChanged_1);
            this.listService.Enter += new System.EventHandler(this.listService_Enter_1);
            // 
            // btnAvancar
            // 
            this.btnAvancar.Enabled = false;
            this.btnAvancar.Location = new System.Drawing.Point(216, 209);
            this.btnAvancar.Name = "btnAvancar";
            this.btnAvancar.Size = new System.Drawing.Size(75, 23);
            this.btnAvancar.TabIndex = 14;
            this.btnAvancar.Text = "Atualizar";
            this.btnAvancar.UseVisualStyleBackColor = true;
            this.btnAvancar.Click += new System.EventHandler(this.btnAvancar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(297, 209);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(75, 23);
            this.btnVoltar.TabIndex = 13;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // lblListaServico
            // 
            this.lblListaServico.AutoSize = true;
            this.lblListaServico.Location = new System.Drawing.Point(12, 9);
            this.lblListaServico.Name = "lblListaServico";
            this.lblListaServico.Size = new System.Drawing.Size(48, 13);
            this.lblListaServico.TabIndex = 12;
            this.lblListaServico.Text = "Serviços";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(-5, 238);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(387, 25);
            this.progressBar.TabIndex = 16;
            // 
            // lblProgresso
            // 
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.Location = new System.Drawing.Point(12, 214);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(0, 13);
            this.lblProgresso.TabIndex = 17;
            // 
            // SetupUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.listService);
            this.Controls.Add(this.btnAvancar);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.lblListaServico);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupUpdate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SetupUpdate";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupUpdate_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listService;
        private System.Windows.Forms.Button btnAvancar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label lblListaServico;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lblProgresso;
    }
}