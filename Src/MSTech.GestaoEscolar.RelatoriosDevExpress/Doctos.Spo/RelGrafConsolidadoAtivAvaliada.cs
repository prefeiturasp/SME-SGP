using System;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class RelGrafConsolidadoAtivAvaliada : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propriedades

        /// <summary>
        /// Guarda se foram encontrados registros para a pesquisa selecionada
        /// </summary>
        private bool RegistrosEncontrados;
        private bool conceito;

        #endregion 

        #region Eventos ao Carregar

        public RelGrafConsolidadoAtivAvaliada(int esc_id, int uni_id, Guid uad_idSuperior, int cal_id, int cur_id, 
                                              int crr_id, int crp_id, long tur_id, int tpc_id, int cap_id, int tds_id, 
                                              Guid usu_id, Guid gru_id, string nomePadraoCalendario, 
                                              string MatriculaEstadual, string NomePadraoCurso, string NomePadraoBimestre,
                                              int NivelEnsinoEducaocaInfantil, bool adm, Guid ent_id, string nomeMunicipio,
                                              string nomeSecretaria, int arq_idLogo)
        {
            InitializeComponent();

            ESCID.Value = esc_id;
            UNIID.Value = uni_id;
            UADIDSUPERIOR.Value = uad_idSuperior;
            CALID.Value = cal_id;
            CAPID.Value = cap_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;
            TURID.Value = tur_id;
            TPCID.Value = tpc_id;
            TDSID.Value = tds_id;
            USUID.Value = usu_id;
            GRUID.Value = gru_id;
            ENTID.Value = ent_id;
            NOMEPERIODOCALENDARIO.Value = nomePadraoCalendario;
            MATRICULAESTADUAL.Value = MatriculaEstadual;
            NOMEPADRAOCURSO.Value = NomePadraoCurso;
            NOMEPADRAOBIMESTRE.Value = NomePadraoBimestre;
            NIVELENSINOEDUCACAOINFANTIL.Value = NivelEnsinoEducaocaInfantil;
            ADM.Value = adm;

            ARQID_LOGO.Value = arq_idLogo;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria; 
        }

        private void RelGrafConsolidadoAtivAvaliada_DataSourceDemanded(object sender, EventArgs e)
        {
            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter.Fill(
                            dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma,
                            ENTID.Value.ToString(),
                            Convert.ToInt32(CALID.Value),
                            Convert.ToInt32(ESCID.Value),
                            Convert.ToInt32(UNIID.Value),
                            Convert.ToInt64(TURID.Value),
                            Convert.ToInt32(CURID.Value),
                            Convert.ToInt32(CRRID.Value),
                            Convert.ToInt32(CRPID.Value),
                            Convert.ToInt32(TPCID.Value),
                            Convert.ToInt32(TDSID.Value),
                            -1,
                            UADIDSUPERIOR.Value.ToString(),
                            Convert.ToBoolean(ADM.Value),
                            USUID.Value.ToString(),
                            GRUID.Value.ToString(),
                            MATRICULAESTADUAL.ToString(),
                            Convert.ToInt32(NIVELENSINOEDUCACAOINFANTIL.Value),
                            false);

            RegistrosEncontrados = dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.Rows.Count > 0;
            conceito = dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.AsEnumerable().All(p => p.esa_tipo != 1);

            xrSubreport1.ReportSource.DataAdapter = this.nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter;
            ((SubRelGrafConsolidadoAtivAvaliada)xrSubreport1.ReportSource).SubRelGrafConsolidadoAtivAvaliada_SetDataSet(dsGestaoEscolar1);
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader3.Visible = RegistrosEncontrados;

            if (RegistrosEncontrados && 
                !string.IsNullOrEmpty(this.GetCurrentColumnValue("esc_id").ToString()) && 
                !string.IsNullOrEmpty(this.GetCurrentColumnValue("uni_id").ToString()) &&
                !string.IsNullOrEmpty(this.GetCurrentColumnValue("tur_id").ToString()) && 
                !string.IsNullOrEmpty(this.GetCurrentColumnValue("dis_id").ToString()))
            {
                xrChart1.Series[0].DataSource = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.AsEnumerable()
                                                 where Convert.ToInt32(dadosGeral.Field<object>("esc_id")) == Convert.ToInt32(this.GetCurrentColumnValue("esc_id").ToString()) &&
                                                       Convert.ToInt32(dadosGeral.Field<object>("uni_id")) == Convert.ToInt32(this.GetCurrentColumnValue("uni_id").ToString()) &&
                                                       Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == Convert.ToInt64(this.GetCurrentColumnValue("tur_id").ToString()) &&
                                                       Convert.ToInt32(dadosGeral.Field<object>("dis_id")) == Convert.ToInt32(this.GetCurrentColumnValue("dis_id").ToString())
                                                 group dadosGeral by new { dis_id = dadosGeral.tds_id
                                                                            , dis_nome = dadosGeral.tds_nome
                                                                            , far_ordenar = dadosGeral.far_ordenar
                                                                            , far_valor = dadosGeral.far_valor
                                                                            , far_descricao = dadosGeral.far_descricao
                                                                            , esa_tipo = dadosGeral.esa_tipo
                                                 } into d
                                                 orderby d.Key.dis_nome, d.Key.far_ordenar, d.Key.far_descricao
                                                 select new {far_descricao = d.Key.far_descricao, qtdAlunos = d.Count()}).ToList();
                
                xrChart1.Series[0].ArgumentDataMember = "far_descricao";
                xrChart1.Series[0].ValueDataMembers[0] = "qtdAlunos";

                List<string> lstCoresRelatorio = new List<string>();

                //Carrega as faixas da turma com as cores
                var lstFaixas = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.AsEnumerable()
                                 where Convert.ToInt32(dadosGeral.Field<object>("esc_id")) == Convert.ToInt32(this.GetCurrentColumnValue("esc_id").ToString()) &&
                                       Convert.ToInt32(dadosGeral.Field<object>("uni_id")) == Convert.ToInt32(this.GetCurrentColumnValue("uni_id").ToString()) &&
                                       Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == Convert.ToInt64(this.GetCurrentColumnValue("tur_id").ToString()) &&
                                       Convert.ToInt32(dadosGeral.Field<object>("dis_id")) == Convert.ToInt32(this.GetCurrentColumnValue("dis_id").ToString())
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
                    lstCoresRelatorio = RelatoriosDevUtil.SelecionaCoresRelatorio((int)ReportNameGestaoAcademica.GraficoConsolidadoAtividadeAvaliativa).Select(p => p.cor_corPaleta).ToList();

                Palette paletaCores = RelatoriosDevUtil.CarregarPaletaCoresRelatorio(lstCoresRelatorio);

                if (paletaCores.Count > 0)
                {
                    xrChart1.PaletteRepository.Add("Gestao", paletaCores);
                    xrChart1.PaletteName = "Gestao";
                }

                xrChart1.Legend.Padding.Right = conceito ? 3 : 20;
                ((TextAnnotation)xrChart1.AnnotationRepository[0]).Text = conceito ? "Conceitos" :"Faixas de notas";
            }
        }

        #endregion

        #region Controla a exibição de elementos

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader1.Visible = RegistrosEncontrados;
            lblTitulo.Text = "Gráfico consolidado " + (conceito ? "dos conceitos" : "das notas") + " dos alunos";
        }

        private void GroupHeader4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader4.Visible = ((!RegistrosEncontrados) && (Convert.ToInt64(TURID.Value) <= 0 || Convert.ToInt32(TDSID.Value) <= 0));
        }

        private void GroupHeader5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader5.Visible = (!RegistrosEncontrados && (Convert.ToInt64(TURID.Value) > 0 && Convert.ToInt32(TDSID.Value) > 0));
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).Visible = Convert.ToInt64(TURID.Value) > 0;
            if (RegistrosEncontrados && Convert.ToInt64(TURID.Value) > 0)
            {
                SubRelGrafConsolidadoAtivAvaliada subReport = ((SubRelGrafConsolidadoAtivAvaliada)((XRSubreport)sender).ReportSource);
                subReport.ESCID.Value = this.GetCurrentColumnValue("esc_id").ToString();
                subReport.UNIID.Value = this.GetCurrentColumnValue("uni_id").ToString();
                subReport.TURID.Value = this.GetCurrentColumnValue("tur_id").ToString();
                subReport.DISID.Value = this.GetCurrentColumnValue("dis_id").ToString();
            }
        }

        #endregion
    }
}
