using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubHistoricoEscolarAssinatura : DevExpress.XtraReports.UI.XtraReport
    {
        public SubHistoricoEscolarAssinatura()
        {
            InitializeComponent();
        }

        private void SubHistoricoEscolarAssinatura_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(NOMEDIRETOR.Value.ToString()))
            {
                lblNomeDiretor.Visible = true;
                lblNomeDiretor.Text = NOMEDIRETOR.Value.ToString();
            }
            if (!string.IsNullOrEmpty(RGDIRETOR.Value.ToString()))
            {
                lblRGDiretor.Visible = true;
                lblRGDiretor.Text = RGDIRETOR.Value.ToString();
            }
        }

    }
}
