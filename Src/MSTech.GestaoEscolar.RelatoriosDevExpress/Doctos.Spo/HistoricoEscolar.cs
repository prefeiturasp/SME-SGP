using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class HistoricoEscolar : DevExpress.XtraReports.UI.XtraReport
    {
        #region Eventos ao Carregar

        public HistoricoEscolar(string alu_id, string mtu_id, Guid ent_id, string nomeMunicipio, string nomeSecretaria)
        {
            InitializeComponent();

            ALUID.Value = alu_id;
            MTUID.Value = mtu_id;
            ENTID.Value = ent_id;

            lblNomeMunicipio.Text = nomeMunicipio;
            lblNomeSecretaria.Text = nomeSecretaria;
        }

        private void HistoricoEscolar_DataSourceDemanded(object sender, EventArgs e)
        {
            this.neW_Relatorio_0005_HistoricoCabecalhoTableAdapter1.Fill(
                    dsGestaoEscolar1.NEW_Relatorio_0005_HistoricoCabecalho,
                    ALUID.Value.ToString(),
                    MTUID.Value.ToString(),
                    new Guid(ENTID.Value.ToString()));
        }

        private void subReportGrade_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string grade = ACA_AlunoHistoricoBO.GerarGradeHistPedagogico(Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()), new Guid());
            if (string.IsNullOrEmpty(grade))
                ((SubHistoricoEscolarGrade)((XRSubreport)sender).ReportSource).Visible = false;
            ((SubHistoricoEscolarGrade)((XRSubreport)sender).ReportSource).GRADE.Value = grade;
        }

        private void subReportEstudos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string estudos = ACA_AlunoHistoricoBO.GerarEsturosHistPedagogico(Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()), new Guid());
            if (string.IsNullOrEmpty(estudos))
                ((SubHistoricoEscolarEstudos)((XRSubreport)sender).ReportSource).Visible = false;
            ((SubHistoricoEscolarEstudos)((XRSubreport)sender).ReportSource).ESTUDOS.Value = estudos;
        }

        private void subReportTransferencia_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string dataTransferencia = "";
            string transferencia = ACA_AlunoHistoricoBO.GerarTransferenciaHistPedagogico(Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString()),
                                                                                         Convert.ToInt32(this.GetCurrentColumnValue("mtu_id").ToString()), 
                                                                                         out dataTransferencia);
            if (string.IsNullOrEmpty(transferencia))
                ((SubHistoricoEscolarTransferencia)((XRSubreport)sender).ReportSource).Visible = false;
            ((SubHistoricoEscolarTransferencia)((XRSubreport)sender).ReportSource).TRANSFERENCIA.Value = transferencia;
            ((SubHistoricoEscolarTransferencia)((XRSubreport)sender).ReportSource).DATATRANSF.Value = dataTransferencia;
        }

        private void subReportObservacao_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

        private void subReportComplementar_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubHistoricoEscolarComplementar)((XRSubreport)sender).ReportSource).ALUID.Value = Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString());
        }

        private void subReportCertificado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubHistoricoEscolarCertificado)((XRSubreport)sender).ReportSource).ALUID.Value = Convert.ToInt64(this.GetCurrentColumnValue("alu_id").ToString());
            ((SubHistoricoEscolarCertificado)((XRSubreport)sender).ReportSource).NOMEALUNO.Value = this.GetCurrentColumnValue("pes_nome").ToString();
            ((SubHistoricoEscolarCertificado)((XRSubreport)sender).ReportSource).NOMEDIRETOR.Value = this.GetCurrentColumnValue("nomeDiretor").ToString();
            ((SubHistoricoEscolarCertificado)((XRSubreport)sender).ReportSource).NOMEESCOLA.Value = this.GetCurrentColumnValue("Escola").ToString();
        }

        private void subReportAssinatura_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubHistoricoEscolarAssinatura)((XRSubreport)sender).ReportSource).NOMEDIRETOR.Value = this.GetCurrentColumnValue("nomeDiretor").ToString();
            ((SubHistoricoEscolarAssinatura)((XRSubreport)sender).ReportSource).RGDIRETOR.Value = this.GetCurrentColumnValue("rgDiretor").ToString();
        }

        #endregion
    }
}
