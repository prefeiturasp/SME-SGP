namespace SetupMultiInstanceGestaoEscolarServerScheduler
{
    partial class SetupGestaoEscolarServerScheduler
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
            this.lnkInstall = new System.Windows.Forms.LinkLabel();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.lnkUninstall = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lnkInstall
            // 
            this.lnkInstall.AutoSize = true;
            this.lnkInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkInstall.Location = new System.Drawing.Point(12, 18);
            this.lnkInstall.Name = "lnkInstall";
            this.lnkInstall.Size = new System.Drawing.Size(141, 17);
            this.lnkInstall.TabIndex = 3;
            this.lnkInstall.TabStop = true;
            this.lnkInstall.Text = "New service instance";
            this.lnkInstall.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkInstall_LinkClicked);
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpdate.Location = new System.Drawing.Point(12, 93);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(160, 17);
            this.lnkUpdate.TabIndex = 4;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update service instance";
            this.lnkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdate_LinkClicked);
            // 
            // lnkUninstall
            // 
            this.lnkUninstall.AutoSize = true;
            this.lnkUninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUninstall.Location = new System.Drawing.Point(12, 168);
            this.lnkUninstall.Name = "lnkUninstall";
            this.lnkUninstall.Size = new System.Drawing.Size(172, 17);
            this.lnkUninstall.TabIndex = 5;
            this.lnkUninstall.TabStop = true;
            this.lnkUninstall.Text = "Uninstall service instance ";
            this.lnkUninstall.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUninstall_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Location = new System.Drawing.Point(27, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 42);
            this.label1.TabIndex = 6;
            this.label1.Text = "Instala de uma nova instância do serviço de processamento de dados do Gestão Esco" +
    "lar.";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Location = new System.Drawing.Point(27, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 42);
            this.label2.TabIndex = 7;
            this.label2.Text = "Atualiza uma instância instalada do serviço de processamento de dados do Gestão E" +
    "scolar.";
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Location = new System.Drawing.Point(27, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(345, 42);
            this.label3.TabIndex = 8;
            this.label3.Text = "Desinstala uma instância instalada do serviço de processamento de dados do Gestão" +
    " Escolar.";
            // 
            // SetupGestaoEscolarServerScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 245);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkUninstall);
            this.Controls.Add(this.lnkUpdate);
            this.Controls.Add(this.lnkInstall);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupGestaoEscolarServerScheduler";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SetupGestaoEscolarServerScheduler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel lnkInstall;
        private System.Windows.Forms.LinkLabel lnkUpdate;
        private System.Windows.Forms.LinkLabel lnkUninstall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}