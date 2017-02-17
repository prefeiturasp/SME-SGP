using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class RelAvaliacaoAtividadeAvaliativa : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propriedades

        /// <summary>
        /// Guarda se foram encontrados registros para a pesquisa selecionada
        /// </summary>
        private bool RegistrosEncontrados;

        #endregion

        #region Eventos ao Carregar

        /// <summary>
        /// Executado ao Logo ao passar os parametros para a pagina
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="tpc_id">Id do periodo</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="crp_id">Id do curriculo periodo</param>
        /// <param name="tnt_id">Id da avaliação</param>
        public RelAvaliacaoAtividadeAvaliativa(int esc_id, long tur_id, long tud_id, int tpc_id,
            int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, int tnt_id, string nomePeriodoCalendario, string nomeDisciplina,
            string nomeMunicipio, string nomeSecretaria, int arq_idLogo)
        {
            InitializeComponent();

            TURID.Value = tur_id;
            TUDID.Value = tud_id;
            TPCID.Value = tpc_id;

            UNIID.Value = uni_id;
            CALID.Value = cal_id;
            CURID.Value = cur_id;
            CRRID.Value = crr_id;
            CRPID.Value = crp_id;

            ESCID.Value = esc_id;
            TNTID.Value = tnt_id;

            NOMEPERIODOCALENDARIO.Value = nomePeriodoCalendario;
            NOMEDISCIPLINA.Value = nomeDisciplina;

            ARQID_LOGO.Value = arq_idLogo;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;

        }

        private void RelAvaliacaoAtividadeAvaliativa_DataSourceDemanded(object sender, EventArgs e)
        {

            ImgLogo.Image = SYS_ArquivoBO.SelecionaImagemPorArquivo(Convert.ToInt32(ARQID_LOGO.Value));

            this.nEW_RelatorioAvaliacao_CabecalhoTableAdapter.Fill(
                            dsGestaoEscolar1.NEW_RelatorioAvaliacao_Cabecalho,
                            Convert.ToInt32(ESCID.Value),
                            Convert.ToInt32(UNIID.Value),
                            Convert.ToInt32(CALID.Value),
                            Convert.ToInt32(CURID.Value),
                            Convert.ToInt32(CRRID.Value),
                            Convert.ToInt32(CRPID.Value),
                            Convert.ToInt32(TPCID.Value),
                            Convert.ToInt64(TURID.Value),
                            Convert.ToInt64(TUDID.Value)
                            );

            this.neW_RelatorioAvaliacao_DescricaoAtividadeTableAdapter1.Fill(
                dsGestaoEscolar1.NEW_RelatorioAvaliacao_DescricaoAtividade,
                Convert.ToInt32(TUDID.Value),
                Convert.ToInt32(TNTID.Value),
                Convert.ToInt32(TPCID.Value)
                );

            this.neW_RelatorioAvaliacao_AtAvaliativa_GraficoTableAdapter1.Fill(
                dsGestaoEscolar1.NEW_RelatorioAvaliacao_AtAvaliativa_Grafico,
                Convert.ToInt64(TURID.Value),
                Convert.ToInt64(TUDID.Value),
                Convert.ToInt32(TNTID.Value),
                Convert.ToInt32(TPCID.Value)
                );

            this.nEW_RelatorioAvaliacao_AtAvaliativaTableAdapter.Fill(
                dsGestaoEscolar1.NEW_RelatorioAvaliacao_AtAvaliativa,
                Convert.ToInt64(TURID.Value),
                Convert.ToInt64(TUDID.Value),
                Convert.ToInt32(TNTID.Value),
                Convert.ToInt32(TPCID.Value)
                );

            RegistrosEncontrados =
                (from DataRow dr in dsGestaoEscolar1.NEW_RelatorioAvaliacao_AtAvaliativa_Grafico.Rows
                 where Convert.ToInt32(dr["total"]) > 0
                 select dr).Count() > 0;
        }

        private void RelAvaliacaoAtividadeAvaliativa_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
            TUR_Turma tur = new TUR_Turma
            {
                tur_id = Convert.ToInt64(TURID.Value)
            };
            TUR_TurmaBO.GetEntity(tur);

            ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao
            {
                fav_id = tur.fav_id
            };
            ACA_FormatoAvaliacaoBO.GetEntity(fav);

            ACA_EscalaAvaliacao esa = new ACA_EscalaAvaliacao
            {
                esa_id = fav.esa_idPorDisciplina
            };
            ACA_EscalaAvaliacaoBO.GetEntity(esa);

            xrBarra.Visible = (esa.esa_tipo == Convert.ToByte(1));
            xrPizza.Visible = (esa.esa_tipo == Convert.ToByte(2));

            List<string> lstCoresRelatorio = new List<string>();

            //Carrega as faixas da turma com as cores
            var lstFaixas = (from dadosGeral in dsGestaoEscolar1.NEW_RelatorioAvaliacao_AtAvaliativa_Grafico.AsEnumerable()
                             group dadosGeral by new
                             {
                                 far_descricao = dadosGeral.Field<object>("far_descricao") != null
                                                 ? dadosGeral.Field<object>("far_descricao").ToString() : "",
                                 far_cor = dadosGeral.Field<object>("far_cor") != null
                                           ? dadosGeral.Field<object>("far_cor").ToString() : ""
                             } into faixas
                             orderby faixas.Key.far_descricao
                             select faixas.Key);

            //Se todas as faixas tiverem cores então vai usar as cores das faixas
            if (!lstFaixas.Any(p => string.IsNullOrEmpty(p.far_cor)))
                lstCoresRelatorio = lstFaixas.Select(p => p.far_cor).ToList();

            //Se não carregou nenhuma cor das faixas então usa as cores configuradas para o relatório
            if (lstCoresRelatorio.Count == 0)
                lstCoresRelatorio = RelatoriosDevUtil.SelecionaCoresRelatorio((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctGraficoAtividadeAvaliativa).Select(p => p.cor_corPaleta).ToList();

            Palette paletaCores = RelatoriosDevUtil.CarregarPaletaCoresRelatorio(lstCoresRelatorio);

            if (paletaCores.Count > 0)
            {
                if (xrPizza.Visible)
                {
                    xrPizza.PaletteRepository.Add("Gestao", paletaCores);
                    xrPizza.PaletteName = "Gestao";
                }

                if (xrBarra.Visible)
                {
                    xrBarra.PaletteRepository.Add("Gestao", paletaCores);
                    xrBarra.PaletteName = "Gestao";
                }
            }
        }

        #endregion

        #region Controla a exibição de elementos

        private void PageHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabelDescricaoAvaliacao.Visible = !string.IsNullOrEmpty(xrLabelDescricaoAvaliacao.Text);

            PageHeader.Visible = RegistrosEncontrados;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader1.Visible = RegistrosEncontrados;
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupHeader2.Visible = !RegistrosEncontrados;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Detail.Visible = RegistrosEncontrados;
        }

        #endregion

    }
}
