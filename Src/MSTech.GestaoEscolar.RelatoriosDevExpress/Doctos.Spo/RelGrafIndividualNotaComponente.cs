using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Collections.Generic;
using MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class RelGrafIndividualNotaComponente : DevExpress.XtraReports.UI.XtraReport
    {

        public RelGrafIndividualNotaComponente(int esc_id, int uni_id, int cal_id, int tpc_id, int cur_id, int crr_id, int crp_id,
                                               string alu_id, Guid ent_id, string nomeMunicipio, string nomeSecretaria, int arq_idLogo)
        {
            InitializeComponent();

            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            CALID.Value = cal_id;
            TPCID.Value = tpc_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;
            ENTID.Value = ent_id;
            ALUID.Value = alu_id;
            ARQID_LOGO.Value = arq_idLogo;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria; 

        }

        private void xrPanel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()) > 0)
            {

                xrChartNotaBim.Series[0].DataSource = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente.AsEnumerable()
                                                       where
                                                           Convert.ToInt64(dadosGeral.Field<object>("alu_id")) ==
                                                           Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()) &&
                                                           Convert.ToInt64(dadosGeral.Field<object>("tds_id")) ==
                                                           Convert.ToInt64(this.GetCurrentColumnValue("tds_id").ToString())
                                                       select dadosGeral).CopyToDataTable();
                xrChartNotaBim.Series[0].ArgumentDataMember = "cap_descricao";
                xrChartNotaBim.Series[0].ValueDataMembers[0] = "valor";
            }
        }

        private void RelGrafIndividualNotaComponente_DataSourceDemanded(object sender, EventArgs e)
        {

            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.nEW_Relatorio_GrafIndividualNotaComponente_AlunosTableAdapter.Fill(
                        dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente_Alunos,
                        ENTID.Value.ToString(),
                        Convert.ToInt32(ESCID.Value),
                        Convert.ToInt32(UNIID.Value),
                        Convert.ToInt32(CALID.Value),
                        Convert.ToInt32(TPCID.Value),
                        Convert.ToInt32(CURID.Value),
                        Convert.ToInt32(CRPID.Value),
                        Convert.ToInt32(CRRID.Value),
                        ALUID.Value.ToString());
            
            this.nEW_Relatorio_GrafIndividualNotaComponenteTableAdapter.Fill(
                        dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente,
                        Convert.ToInt32(CALID.Value),
                        Convert.ToInt32(TPCID.Value),
                        Convert.ToInt32(CURID.Value),
                        Convert.ToInt32(CRPID.Value),
                        Convert.ToInt32(CRRID.Value),
                        ALUID.Value.ToString());

            grpNaoExiste.Visible = !(Detail.Visible = grpCabecalho.Visible = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente_Alunos.Rows.Count > 0);
            
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int esa_tipo = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente.AsEnumerable()
                                        .Where(p => Convert.ToInt64(p.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()))
                                            .FirstOrDefault().esa_tipo;

            int esa_id = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotaComponente.AsEnumerable()
                                    .Where(p => Convert.ToInt64(p.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()))
                                        .FirstOrDefault().esa_id;

            ((XYDiagram)xrChartNotaBim.Diagram).AxisY.Range.Auto = false;
            if (esa_tipo == 2)
            {
                List<ACA_EscalaAvaliacaoParecer> escalaParecerList = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id);

                xrChartNotaBim.Series[0].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;

                //Calcula valor minimo, maximo e a sobra de 5% para vizualisação dos pontos
                var MinValue = escalaParecerList.Min(p => p.eap_ordem);
                var MaxValue = escalaParecerList.Max(p => p.eap_ordem);
                decimal sobra = (MaxValue * (decimal)0.05);

                //Adiciona range da escala de avaliação.
                ((XYDiagram)xrChartNotaBim.Diagram).AxisY.Range.MinValue = MinValue - sobra;
                ((XYDiagram)xrChartNotaBim.Diagram).AxisY.Range.MaxValue = MaxValue + sobra;

                //Adiciona custom labels para as notas de conceito
                foreach (ACA_EscalaAvaliacaoParecer eap in escalaParecerList)
                {
                    ((XYDiagram)xrChartNotaBim.Diagram).AxisY.CustomLabels.Add(new CustomAxisLabel {AxisValue = eap.eap_ordem, Name = eap.eap_valor});
                }

                lblTitulo.Text = "Acompanhamento individual de conceitos";
            }
            else
            {
                List<ACA_EscalaAvaliacaoNumerica> escalaNumericaList = ACA_EscalaAvaliacaoNumericaBO.GetSelectBy_Escala(esa_id);    

                //Adiciona range da escala de avaliação.
                ((XYDiagram)xrChartNotaBim.Diagram).AxisY.Range.MinValue = 0;
                ((XYDiagram)xrChartNotaBim.Diagram).AxisY.Range.MaxValue = 10;

                (xrChartNotaBim.Diagram as XYDiagram).AxisY.CustomLabels.Clear();

                int index = 0;
                for (int i = 0; i <= 10; i++)
                {
                    (xrChartNotaBim.Diagram as XYDiagram).AxisY.CustomLabels.Add(new CustomAxisLabel(i.ToString()));
                    (xrChartNotaBim.Diagram as XYDiagram).AxisY.CustomLabels[index].AxisValue = i;
                    index++;
                }

                lblTitulo.Text = "Acompanhamento individual de notas";
            }

        }

        private void RelGrafIndividualNotaComponente_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Palette paletaCores = RelatoriosDevUtil.CarregarPaletaCoresRelaorio((int)ReportNameGestaoAcademica.GraficoIndividualNotaComponente);

            if (paletaCores.Count > 0)
            {
                xrChartNotaBim.PaletteRepository.Add("Gestao", paletaCores);
                xrChartNotaBim.PaletteName = "Gestao";
            }
        }

    }
}
