using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using System.Collections.Generic;
using DevExpress.XtraCharts;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class GraficoJustificativaFalta : XtraReport
    {
        #region Propriedades

        /// <summary>
        /// Guarda se foram encontrados registros para a pesquisa selecionada
        /// </summary>
        private bool RegistrosEncontrados;
        
        #endregion

        public GraficoJustificativaFalta(Guid uad_idSuperior, int esc_id, int uni_id, int cal_id, 
                                         int cur_id, int crr_id, int crp_id, long tur_id, 
                                         Guid usu_id, Guid gru_id, bool adm, Guid ent_id,
                                         string nomeMunicipio, string nomeSecretaria, string dre, string escola, 
                                         string calendario, string filtros, int arq_idLogo)
        {
            InitializeComponent();

            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            CALID.Value = cal_id;
            TURID.Value = tur_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;
            USUID.Value = usu_id;
            GRUID.Value = gru_id;
            ENTID.Value = ent_id;
            UADSUPERIOR.Value = uad_idSuperior;
            ADM.Value = adm;

            ARQID_LOGO.Value = arq_idLogo;
            
            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
            lblDRE.Text = string.IsNullOrEmpty(dre) ? "Todas as unidades" : dre;
            lblEscola.Text = string.IsNullOrEmpty(dre) ? "" : (string.IsNullOrEmpty(escola) ? "Todas as escolas" : escola);
            lblCalendario.Text = calendario;
            lblFiltros.Text = filtros;
        }

        private void GraficoJustificativaFalta_DataSourceDemanded(object sender, EventArgs e)
        {
            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.nEW_Relatorio_GraficoJustificativaFaltaTableAdapter.Fill(
                            dsGestaoEscolar1.NEW_Relatorio_GraficoJustificativaFalta,
                            ENTID.Value.ToString(),
                            Convert.ToInt32(CALID.Value),
                            Convert.ToInt32(ESCID.Value),
                            Convert.ToInt32(UNIID.Value),
                            Convert.ToInt64(TURID.Value),
                            Convert.ToInt32(CURID.Value),
                            Convert.ToInt32(CRRID.Value),
                            Convert.ToInt32(CRPID.Value),
                            UADSUPERIOR.Value.ToString(),
                            Convert.ToBoolean(ADM.Value),
                            USUID.Value.ToString(),
                            GRUID.Value.ToString());

            RegistrosEncontrados = dsGestaoEscolar1.NEW_Relatorio_GraficoJustificativaFalta.Rows.Count > 0 &&
                                   dsGestaoEscolar1.NEW_Relatorio_GraficoJustificativaFalta.AsEnumerable()
                                        .Any(p => p.Field<object>("totalAlunos") != null &&
                                                  Convert.ToInt32(p.Field<object>("totalAlunos")) > 0);
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader2.Visible = !RegistrosEncontrados;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader1.Visible = RegistrosEncontrados;
            xrPanel1.Visible = true;

            xrChart1.Series.Clear();
            List<string> lstTipoJustificativa = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GraficoJustificativaFalta.AsEnumerable()
                                                 group dadosGeral by new
                                                 {
                                                     descricao = dadosGeral.Field<object>("tjf_nome") != null ?
                                                             dadosGeral.Field<object>("tjf_nome").ToString() : ""
                                                 }
                                                 into dadosGeralTipo
                                                 orderby dadosGeralTipo.Key.descricao.ToString()
                                                 select dadosGeralTipo.Key.descricao.ToString()).ToList();
            foreach (string tjf_nome in lstTipoJustificativa)
            {
                Series serie = new Series(tjf_nome, ViewType.StackedBar);
                serie.ArgumentScaleType = ScaleType.Auto;
                serie.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                serie.Label.PointOptions.ValueNumericOptions.Precision = 0;
                serie.Label.TextColor = Color.Black;

                serie.DataSource = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GraficoJustificativaFalta.AsEnumerable()
                                    where dadosGeral.Field<object>("tjf_nome") != null &&
                                          dadosGeral.Field<object>("tjf_nome").ToString() == tjf_nome
                                    select dadosGeral).CopyToDataTable();
                serie.ArgumentDataMember = "tjf_nome";
                serie.LegendText = tjf_nome;
                serie.ValueDataMembers[0] = "totalAlunos";
                serie.ShowInLegend = true;

                xrChart1.Series.Add(serie);
            }
            
            if (lstTipoJustificativa.Count > 0)
            {
                xrChart1.Legend.Direction = LegendDirection.BottomToTop;
                ((TextAnnotation)xrChart1.AnnotationRepository[0]).Text = "Justificativas de falta";
                (xrChart1.Diagram as XYDiagram).AxisY.Label.Angle = 0;
                (xrChart1.Diagram as XYDiagram).AxisY.Label.Visible = false;
                (xrChart1.Diagram as XYDiagram).AxisX.Label.Angle = 0;
                (xrChart1.Diagram as XYDiagram).AxisY.Range.Auto = true;
                (xrChart1.Diagram as XYDiagram).AxisY.NumericOptions.Format = NumericFormat.Number;
                (xrChart1.Diagram as XYDiagram).AxisY.NumericOptions.Precision = 0;
                (xrChart1.Diagram as XYDiagram).AxisY.Range.AlwaysShowZeroLevel = true;
            }
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader3.Visible = RegistrosEncontrados;
            lblTitulo.Text = "Gráfico de justificativa de faltas";
        }
    }
}
