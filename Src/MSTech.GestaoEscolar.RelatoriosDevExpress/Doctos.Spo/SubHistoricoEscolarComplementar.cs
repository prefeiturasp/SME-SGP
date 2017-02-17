using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubHistoricoEscolarComplementar : DevExpress.XtraReports.UI.XtraReport
    {
        public SubHistoricoEscolarComplementar()
        {
            InitializeComponent();
        }

        private void SubHistoricoEscolarComplementar_DataSourceDemanded(object sender, EventArgs e)
        {
            this.neW_Relatorio_0005_HistoricoEscolarObsComplementarTableAdapter1.Fill(
                dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarObsComplementar,
                Convert.ToInt64(ALUID.Value));

            this.ReportHeader.Visible = dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarObsComplementar.Rows.Count > 0 &&
                                        !string.IsNullOrEmpty(dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarObsComplementar[0]["obsComplementar"].ToString());
        }

    }
}
