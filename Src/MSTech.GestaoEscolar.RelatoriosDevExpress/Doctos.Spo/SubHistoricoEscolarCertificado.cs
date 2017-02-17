using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubHistoricoEscolarCertificado : DevExpress.XtraReports.UI.XtraReport
    {
        public SubHistoricoEscolarCertificado()
        {
            InitializeComponent();
        }

        private void SubHistoricoEscolarCertificado_DataSourceDemanded(object sender, EventArgs e)
        {
            this.neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1.Fill(
                dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarCertificado,
                Convert.ToInt64(ALUID.Value),
                NOMEALUNO.Value.ToString(),
                NOMEDIRETOR.Value.ToString(),
                NOMEESCOLA.Value.ToString());

            this.ReportHeader.Visible = dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarCertificado.Rows.Count > 0 &&
                                        !string.IsNullOrEmpty(dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarCertificado[0]["certificado"].ToString());
        }

    }
}
