using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class RelGrafTurmaMatrizCurricular : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propriedades

        /// <summary>
        /// Guarda se foram encontrados registros para a pesquisa selecionada
        /// </summary>
        private bool RegistrosEncontrados;
        private bool conceito;
        Int64 tur_id;
        int tpc_id;

        #endregion

        #region Eventos ao Carregar

        public RelGrafTurmaMatrizCurricular(int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id,
                                            long tur_id, int tpc_id, int cap_id, Guid usu_id, Guid gru_id, bool adm, 
                                            Guid ent_id, int NivelEnsinoEducacaoInfantil, string MensagemAviso,
                                            string nomeMunicipio, string nomeSecretaria, int arq_idLogo)
        {
            InitializeComponent();

            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            CALID.Value = cal_id;
            CAPID.Value = cap_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;
            TURID.Value = tur_id;
            TPCID.Value = tpc_id;
            USUID.Value = usu_id;
            GRUID.Value = gru_id;
            ENTID.Value = ent_id;
            ADM.Value = adm;
            NIVELENSINOEDUCACAOINFANTIL.Value = NivelEnsinoEducacaoInfantil;

            ARQID_LOGO.Value = arq_idLogo;

            lblMensagem.Text = MensagemAviso;
            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
           
        }

        private void RelGrafTurmaMatrizCurricular_DataSourceDemanded(object sender, EventArgs e)
        {
            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.nEW_Relatorio_GrafTurmaMatrizCurricularTableAdapter.Fill(
                            dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular,
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
                            Convert.ToInt32(NIVELENSINOEDUCACAOINFANTIL.Value));

            RegistrosEncontrados = dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.Rows.Count > 0 &&
                                   dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                        .Any(p => p.Field<object>("totalTurma") != null &&
                                                  Convert.ToInt32(p.Field<object>("totalTurma")) > 0);
            
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader1.Visible = RegistrosEncontrados;
            lblTitulo.Text = "Apresentação síntese dos resultados de avaliação turma/matriz curricular";
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader2.Visible = !RegistrosEncontrados;
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader3.Visible = RegistrosEncontrados;
            xrPanel1.Visible = true;
            if (this.GetCurrentColumnValue("tur_id") != null)
                tur_id = Convert.ToInt64(this.GetCurrentColumnValue("tur_id").ToString());
            else
                tur_id = 0;
            if (this.GetCurrentColumnValue("tpc_id") != null)
                tpc_id = Convert.ToInt32(this.GetCurrentColumnValue("tpc_id").ToString());
            else
                tpc_id = 0;
            if (tur_id > 0 && tpc_id > 0)
            {
                int totalTurma = dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                        .Any(p => Convert.ToInt64(p.Field<object>("tur_id")) == tur_id &&
                                                  Convert.ToInt32(p.Field<object>("tpc_id")) == tpc_id &&
                                                    p.Field<object>("totalTurma") != null &&
                                                    Convert.ToInt32(p.Field<object>("totalTurma")) > 0) ?
                                 dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                        .Where(p => Convert.ToInt64(p.Field<object>("tur_id")) == tur_id &&
                                                    Convert.ToInt32(p.Field<object>("tpc_id")) == tpc_id &&
                                                    p.Field<object>("totalTurma") != null &&
                                                    Convert.ToInt32(p.Field<object>("totalTurma")) > 0)
                                                    .FirstOrDefault().totalTurma : 0;

                xrChart1.Series.Clear();
                if (totalTurma > 0)
                {
                    List<string> lstFaixa = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                             where Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == tur_id &&
                                                   Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == tpc_id &&
                                                   dadosGeral.Field<object>("far_descricao") != null &&
                                                   !string.IsNullOrEmpty(dadosGeral.Field<object>("far_descricao").ToString())
                                             group dadosGeral by new
                                             {
                                                 descricao = dadosGeral.Field<object>("far_descricao") != null ?
                                                             dadosGeral.Field<object>("far_descricao").ToString() : "",
                                                 ordenar = dadosGeral.Field<object>("far_descricao") != null ?
                                                           dadosGeral.Field<object>("far_descricao").ToString() : ""
                                             }
                                             into dadosGeralFaixa
                                             orderby dadosGeralFaixa.Key.ordenar.ToString()
                                             select dadosGeralFaixa.Key.descricao.ToString()).ToList();

                    foreach (string far_descricao in lstFaixa)
                    {

                        Series serie = new Series(far_descricao, ViewType.FullStackedBar);
                        serie.ArgumentScaleType = ScaleType.Auto;
                        ((FullStackedBarPointOptions)serie.Label.PointOptions).PercentOptions.ValueAsPercent = true;
                        serie.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                        serie.Label.PointOptions.ValueNumericOptions.Precision = 0;
                        serie.Label.TextColor = Color.Black;
                        
                        serie.DataSource = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                                              where Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == tur_id &&
                                                                    Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == tpc_id &&
                                                                    dadosGeral.Field<object>("far_descricao") != null &&
                                                                    dadosGeral.Field<object>("far_descricao").ToString() == far_descricao
                                                              select dadosGeral).CopyToDataTable();
                        serie.ArgumentDataMember = "dis_nome";
                        serie.LegendText = far_descricao;
                        serie.ValueDataMembers[0] = "qtdAlunos";
                        serie.ShowInLegend = true;
                        
                        xrChart1.Series.Add(serie);

                    }

                    if (lstFaixa.Count > 0)
                    {
                        xrChart1.Legend.Direction = LegendDirection.BottomToTop;
                        (xrChart1.Diagram as XYDiagram).AxisY.Label.Angle = 0;
                        (xrChart1.Diagram as XYDiagram).AxisX.Label.Angle = 0;
                        (xrChart1.Diagram as XYDiagram).AxisY.VisualRange.SetMinMaxValues(0, 1);
                        (xrChart1.Diagram as XYDiagram).AxisY.Label.NumericOptions.Format = NumericFormat.Percent;
                        (xrChart1.Diagram as XYDiagram).AxisY.Label.NumericOptions.Precision = 0;
                        (xrChart1.Diagram as XYDiagram).AxisY.WholeRange.SetMinMaxValues(0, 1);
                        (xrChart1.Diagram as XYDiagram).AxisY.WholeRange.AlwaysShowZeroLevel = true;
                        (xrChart1.Diagram as XYDiagram).Rotated = true;

                        int index = 0;
                        (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Add(new CustomAxisLabel(""));
                        (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels[index].AxisValue = -10 * 0.01;
                        index++;
                        for (int i = 0; i <= 100; i += 10)
                        {
                            (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Add(new CustomAxisLabel(i.ToString() + "%"));
                            (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels[index].AxisValue = i * 0.01;
                            index++;
                        }
                        (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels.Add(new CustomAxisLabel(""));
                        (xrChart1.Diagram as XYDiagram).AxisY.CustomLabels[index].AxisValue = 110 * 0.01;
                        index++;
                    }


                    List<string> lstCoresRelatorio = new List<string>();

                    //Carrega as faixas da turma com as cores
                    var lstFaixas = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                                     where Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == tur_id &&
                                           Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == tpc_id
                                     group dadosGeral by new
                                     {
                                         far_ordenar = dadosGeral.Field<object>("far_ordenar") != null
                                                       ? dadosGeral.Field<object>("far_ordenar").ToString() : "",
                                         far_cor = dadosGeral.Field<object>("far_cor") != null
                                                   ? dadosGeral.Field<object>("far_cor").ToString() : ""
                                     } into faixas
                                     orderby faixas.Key.far_ordenar
                                     select faixas.Key);

                    //Se todas as faixas tiverem cores então vai usar as cores das faixas
                    if (!lstFaixas.Any(p => string.IsNullOrEmpty(p.far_cor)))
                        lstCoresRelatorio = lstFaixas.Select(p => p.far_cor).ToList();

                    //Se não carregou nenhuma cor das faixas então usa as cores configuradas para o relatório
                    if (lstCoresRelatorio.Count == 0)
                        lstCoresRelatorio = RelatoriosDevUtil.SelecionaCoresRelatorio((int)ReportNameGestaoAcademica.GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular).Select(p => p.cor_corPaleta).ToList();
                    
                    Palette paletaCores = RelatoriosDevUtil.CarregarPaletaCoresRelatorio(lstCoresRelatorio);

                    if (paletaCores.Count > 0)
                    {
                        xrChart1.PaletteRepository.Add("Gestao", paletaCores);
                        xrChart1.PaletteName = "Gestao";
                    }
                    if(dsGestaoEscolar1.NEW_Relatorio_GrafTurmaMatrizCurricular.AsEnumerable()
                        .Any(p => Convert.ToInt64(p.Field<object>("tur_id")) == tur_id && Convert.ToInt32(p.Field<object>("tpc_id")) == tpc_id && Convert.ToInt64(p.Field<object>("qtdAlunos")) > 0 && p.esa_tipo != 1))
                    {
                        ((TextAnnotation)xrChart1.AnnotationRepository[0]).Text = "Conceitos";
                        xrChart1.Legend.Padding.Right = 3;
                    }
                    else
                    {
                        ((TextAnnotation)xrChart1.AnnotationRepository[0]).Text = "Faixas de notas";
                        xrChart1.Legend.Padding.Right = 20;
                    }
                    
                }
                else
                {
                    xrPanel1.Visible = false;
                }
            }
            else
            {
                xrPanel1.Visible = false;
            }
        }

        #endregion

        }
        
    }
