using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class DocDctPlanoAnual : DevExpress.XtraReports.UI.XtraReport
    {
        public DocDctPlanoAnual(string cal_ano, int cal_id, int esc_id, int uni_id, Guid ent_id, long tur_id, long tud_id, bool mostraCodigoEscola, string nomeMunicipio, string nomeSecretaria, int arqid_logo)
        {
            InitializeComponent();

            ARQID_LOGO.Value = arqid_logo;
            CALID.Value = cal_id;
            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            ENTID.Value = ent_id;
            TURID.Value = tur_id;
            TUDID.Value = tud_id;
            CODIGOESCOLA.Value = mostraCodigoEscola;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
            lblAnoLetivo.Text = "Ano letivo: " + cal_ano;
                        
        }

        private void DocDctPlanoAnual_DataSourceDemanded(object sender, EventArgs e)
        {
            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            int RowsCount = this.neW_Relatorio_0005_DocDctPlanoAnualTableAdapter1.Fill(
                        dsGestaoEscolar1.NEW_Relatorio_0005_DocDctPlanoAnual,
                        Convert.ToInt32(CALID.Value),
                        Convert.ToInt32(ESCID.Value),
                        Convert.ToInt32(UNIID.Value),
                        (Guid)ENTID.Value,
                        Convert.ToInt64(TURID.Value),
                        Convert.ToInt64(TUDID.Value),
                        (Boolean)CODIGOESCOLA.Value);

            if (RowsCount <= 0)
            {
                Detail.Visible = false;
                grpMensagem.Visible = true;
                lblSemRegistro.Text = "Não existe resultado para a pesquisa selecionada.";
            }
            
            if (dsGestaoEscolar1.NEW_Relatorio_0005_DocDctPlanoAnual.Any(p => string.IsNullOrEmpty(p.titulo)))
            {
                Detail.Visible = false;
                grpMensagem.Visible = true;
                lblSemRegistro.Text = "Não há registro de Plano Anual para este componente curricular na turma solicitada.";
            }
        }

    }
}
