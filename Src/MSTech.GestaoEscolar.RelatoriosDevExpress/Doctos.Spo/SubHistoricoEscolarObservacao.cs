using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubHistoricoEscolarObservacao : DevExpress.XtraReports.UI.XtraReport
    {
        public SubHistoricoEscolarObservacao()
        {
            InitializeComponent();
        }

        private void SubHistoricoEscolarObservacao_DataSourceDemanded(object sender, EventArgs e)
        {
            this.neW_Relatorio_0005_HistoricoEscolarObservacoesTableAdapter1.Fill(
                dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoEscolarObservacoes);
        }

    }
}
