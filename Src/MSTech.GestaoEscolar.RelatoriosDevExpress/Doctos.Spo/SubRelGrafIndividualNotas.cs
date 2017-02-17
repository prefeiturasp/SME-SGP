using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Net.Mime;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubRelGrafIndividualNotas : DevExpress.XtraReports.UI.XtraReport
    {
        bool addRange = false;
        public SubRelGrafIndividualNotas()
        {
            InitializeComponent();
        }

        private void SubRelGrafIndividualNotas_DataSourceDemanded(object sender, EventArgs e)
        {
            this.neW_Relatorio_GrafIndividualNotasTableAdapter.Fill(
                        dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas,
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
                        Convert.ToInt64(TURID.Value),
                        ALUID.Value.ToString());
        }

        private void SubRelGrafIndividualNotas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Palette paletaCores = RelatoriosDevUtil.CarregarPaletaCoresRelaorio((int)ReportNameGestaoAcademica.GraficoIndividualNotas);

            if (paletaCores.Count > 0)
            {
                xrChart1.PaletteRepository.Add("Gestao", paletaCores);
                xrChart1.PaletteName = "Gestao";
            }

            lblAluno.Visible = xrChart1.Visible = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.Count > 0;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.GetCurrentColumnValue("alu_id") != null)
            {
                int esa_tipo = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.AsEnumerable()
                                        .Where(p => Convert.ToInt64(p.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()))
                                            .FirstOrDefault().esa_tipo;
                int esa_id = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.AsEnumerable()
                                    .Where(p => Convert.ToInt64(p.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()))
                                        .FirstOrDefault().esa_id;

                List<int> lstTpc = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.AsEnumerable()
                                    where Convert.ToInt64(dadosGeral.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString())
                                    group dadosGeral by dadosGeral.Field<object>("tpc_ordem") into dadosGeralTpc
                                    select Convert.ToInt32(dadosGeralTpc.Key)).ToList();

                xrChart1.Series.Clear();

                int series = 0;
                foreach (int tpc_ordem in lstTpc)
                {
                    string tpc_nome = dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.AsEnumerable()
                                        .Where(p => Convert.ToInt32(p.Field<object>("tpc_ordem")) == tpc_ordem)
                                            .FirstOrDefault().tpc_nome;
                    Series serie = new Series(tpc_nome, ViewType.Line);
                    serie.ArgumentScaleType = ScaleType.Auto;
                    xrChart1.Series.Add(serie);
                    xrChart1.Series[series].DataSource = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafIndividualNotas.AsEnumerable()
                                                          where Convert.ToInt64(dadosGeral.Field<object>("alu_id")) == Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()) &&
                                                                Convert.ToInt32(dadosGeral.Field<object>("tpc_ordem")) == tpc_ordem
                                                          select dadosGeral).CopyToDataTable();
                    xrChart1.Series[series].ArgumentDataMember = "dis_nome";
                    xrChart1.Series[series].LegendText = tpc_nome;
                    xrChart1.Series[series].ValueDataMembers[0] = "valor";
                    xrChart1.Series[series].ShowInLegend = true;

                    if (esa_tipo == 2)
                        xrChart1.Series[series].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    series++;
                }

                if (!addRange)
                {
                    addRange = true;
                    (xrChart1.Diagram as XYDiagram).AxisY.Range.Auto = false;
                    if (esa_tipo == 2)
                    {
                        List<ACA_EscalaAvaliacaoParecer> lstEscala = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id);

                        //Calcula valor minimo, maximo e a sobra de 5% para vizualisação dos pontos
                        var MinValue = lstEscala.Min(p => p.eap_ordem);
                        var MaxValue = lstEscala.Max(p => p.eap_ordem);
                        decimal sobra = (MaxValue * (decimal)0.05);

                        //Adiciona range da escala de avaliação.
                        (xrChart1.Diagram as XYDiagram).AxisY.Range.MinValue = MinValue - sobra;
                        (xrChart1.Diagram as XYDiagram).AxisY.Range.MaxValue = MaxValue + sobra;

                        //Adiciona custom labels para as notas de conceito
                        foreach (ACA_EscalaAvaliacaoParecer eap in lstEscala)
                        {
                            CustomAxisLabel label = new CustomAxisLabel();

                            label.AxisValue = eap.eap_ordem;
                            label.Name = eap.eap_valor;
                            (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Add(label);
                        }
                    }
                    else
                    {
                        List<ACA_EscalaAvaliacaoNumerica> lstEscala = ACA_EscalaAvaliacaoNumericaBO.GetSelectBy_Escala(esa_id);
                        
                        //Adiciona range da escala de avaliação.
                        (xrChart1.Diagram as XYDiagram).AxisY.Range.MinValue = 0;
                        (xrChart1.Diagram as XYDiagram).AxisY.Range.MaxValue = 10;

                        (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Clear();

                        int index = 0;
                        for (int i = 0; i <= 10; i++)
                        {
                            (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Add(new CustomAxisLabel(i.ToString()));
                            (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels[index].AxisValue = i;
                            index++;
                        }
                    }

                    (xrChart1.Diagram as XYDiagram).AxisY.Label.Angle = 0;
                }
            }
        }
    }
}
