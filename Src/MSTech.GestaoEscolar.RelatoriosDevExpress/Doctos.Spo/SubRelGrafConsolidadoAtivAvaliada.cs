using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    public partial class SubRelGrafConsolidadoAtivAvaliada : DevExpress.XtraReports.UI.XtraReport
    {
        public SubRelGrafConsolidadoAtivAvaliada()
        {
            InitializeComponent();
        }

        private void SubRelGrafConsolidadoAtivAvaliada_DataSourceDemanded(object sender, EventArgs e)
        {
            
            DataTable tabelaAlunos;
            try
            {
                tabelaAlunos = (from dadosGeral in dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.AsEnumerable()
                                where Convert.ToInt32(dadosGeral.Field<object>("esc_id")) == Convert.ToInt32(ESCID.Value) &&
                                      Convert.ToInt32(dadosGeral.Field<object>("uni_id")) == Convert.ToInt32(UNIID.Value) &&
                                      Convert.ToInt64(dadosGeral.Field<object>("tur_id")) == Convert.ToInt64(TURID.Value) &&
                                      Convert.ToInt32(dadosGeral.Field<object>("dis_id")) == Convert.ToInt32(DISID.Value)
                                select dadosGeral).CopyToDataTable();
            }
            catch
            {
                tabelaAlunos = new DataTable();
            }
            if (tabelaAlunos.Rows.Count > 0)
            {
                this.DataSource = tabelaAlunos;
                this.DataMember = dsGestaoEscolar1.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.TableName;
            }
            lblAlunos.Visible = lblfar_descricao.Visible = lblpes_nome.Visible = tabelaAlunos.Rows.Count > 0;
        }

        public void SubRelGrafConsolidadoAtivAvaliada_SetDataSet(DSGestaoEscolar dsRelGrafConsolidadoAtivAvaliada)
        {
            this.dsGestaoEscolar1 = dsRelGrafConsolidadoAtivAvaliada;
        }
    }
}
