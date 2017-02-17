using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class RelGrafIndividualNotas : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propriedades

        /// <summary>
        /// Guarda se foram encontrados registros para a pesquisa selecionada
        /// </summary>
        private bool RegistrosEncontrados;
        private bool conceito;
        private long tur_id;

        #endregion 
        
        #region Eventos ao Carregar

        public RelGrafIndividualNotas(int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, 
                                      int tpc_id, int cap_id, Guid usu_id, Guid gru_id, int NivelEnsinoEducaocaInfantil, 
                                      string alu_id, bool adm, Guid ent_id, string nomeMunicipio, string nomeSecretaria,
                                      int arq_idLogo)
        {
            InitializeComponent();

            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            CALID.Value = cal_id;
            CAPID.Value = cap_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;
            TPCID.Value = tpc_id;
            USUID.Value = usu_id;
            GRUID.Value = gru_id;
            ENTID.Value = ent_id;
            ALUID.Value = alu_id;
            NIVELENSINOEDUCACAOINFANTIL.Value = NivelEnsinoEducaocaInfantil;
            ADM.Value = adm;

            ARQID_LOGO.Value = arq_idLogo;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
        }

        private void RelGrafIndividualNotas_DataSourceDemanded(object sender, EventArgs e)
        {

            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.neW_Relatorio_GrafIndividualNotas_TurmasTableAdapter.Fill(
                            dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas_Turmas,
                            ENTID.Value.ToString(),
                            Convert.ToBoolean(ADM.Value),
                            USUID.Value.ToString(),
                            GRUID.Value.ToString(),
                            Convert.ToInt32(ESCID.Value),
                            Convert.ToInt32(UNIID.Value),
                            Convert.ToInt32(CALID.Value),
                            Convert.ToInt32(TPCID.Value),
                            Convert.ToInt32(CURID.Value),
                            Convert.ToInt32(CRPID.Value),
                            Convert.ToInt32(CRRID.Value),
                            ALUID.Value.ToString());

            RegistrosEncontrados = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas_Turmas.Rows.Count > 0;
            conceito = !dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas_Turmas.AsEnumerable().Any(p => p.esa_tipo == 1);
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader3.Visible = RegistrosEncontrados;
            lblTitulo.Text = "Acompanhamento individual de " + (conceito ? "conceitos" : "notas");
        }
        
        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader2.Visible = !RegistrosEncontrados;
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).Visible = RegistrosEncontrados;
            if (RegistrosEncontrados)
            {
                tur_id = Convert.ToInt64(this.GetCurrentColumnValue("tur_id").ToString());
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).TURID.Value = tur_id;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).ENTID.Value = ENTID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).CALID.Value = CALID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).ESCID.Value = ESCID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).UNIID.Value = UNIID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).CURID.Value = CURID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).CRRID.Value = CRRID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).CRPID.Value = CRPID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).TPCID.Value = TPCID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).ALUID.Value = ALUID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).ADM.Value = ADM.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).USUID.Value = USUID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).GRUID.Value = GRUID.Value;
                ((SubRelGrafIndividualNotas)((XRSubreport)sender).ReportSource).NIVELENSINOEDUCACAOINFANTIL.Value = NIVELENSINOEDUCACAOINFANTIL.Value;
            }
        }  
        
        #endregion
    }
}
