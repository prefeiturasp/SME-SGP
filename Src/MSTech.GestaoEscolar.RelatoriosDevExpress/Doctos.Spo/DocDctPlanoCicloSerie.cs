using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class DocDctPlanoCicloSerie : DevExpress.XtraReports.UI.XtraReport
    {
        public DocDctPlanoCicloSerie(string cal_ano, int cal_id, int esc_id, int uni_id, Guid ent_id, bool mostraCodigoEscola, string nomeMunicipio, string nomeSecretaria, int arqid_logo)
        {
            InitializeComponent();

            ARQID_LOGO.Value = arqid_logo;
            CALID.Value = cal_id;
            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            ENTID.Value = ent_id;
            CODIGOESCOLA.Value = mostraCodigoEscola;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
            lblAnoLetivo.Text = "Ano letivo: " + cal_ano;
        }

        private void DocDctPlanoCicloSerie_DataSourceDemanded(object sender, EventArgs e)
        {
            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            int RowsCount = this.neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1.Fill(
                        dsGestaoEscolar1.NEW_Relatorio_0005_DocDctPlanoCicloSerie,
                        Convert.ToInt32(CALID.Value),
                        Convert.ToInt32(ESCID.Value),
                        Convert.ToInt32(UNIID.Value),
                        (Guid)ENTID.Value,
                        (Boolean)CODIGOESCOLA.Value);

            if (RowsCount <= 0)
            {
                Detail.Visible = false;
                grpMensagem.Visible = true;
            }
        }

    }
}
