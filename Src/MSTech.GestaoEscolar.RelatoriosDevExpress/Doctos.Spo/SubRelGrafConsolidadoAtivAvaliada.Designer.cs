namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    partial class SubRelGrafConsolidadoAtivAvaliada
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.lblpes_nome = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblfar_descricao = new DevExpress.XtraReports.UI.XRLabel();
            this.dsGestaoEscolar1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolar();
            this.nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter();
            this.ESCID = new DevExpress.XtraReports.Parameters.Parameter();
            this.UNIID = new DevExpress.XtraReports.Parameters.Parameter();
            this.TURID = new DevExpress.XtraReports.Parameters.Parameter();
            this.TDSID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblAlunos = new DevExpress.XtraReports.UI.XRLabel();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TPCID = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblpes_nome});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 15.83335F;
            this.Detail.MultiColumn.ColumnCount = 3;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblpes_nome
            // 
            this.lblpes_nome.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblpes_nome.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.pes_nome")});
            this.lblpes_nome.Dpi = 100F;
            this.lblpes_nome.Font = new System.Drawing.Font("Arial", 7F);
            this.lblpes_nome.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblpes_nome.Name = "lblpes_nome";
            this.lblpes_nome.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblpes_nome.SizeF = new System.Drawing.SizeF(232.2917F, 15.83335F);
            this.lblpes_nome.StylePriority.UseBorders = false;
            this.lblpes_nome.StylePriority.UseFont = false;
            this.lblpes_nome.StylePriority.UseTextAlignment = false;
            this.lblpes_nome.Text = "lblpes_nome";
            this.lblpes_nome.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblfar_descricao});
            this.GroupHeader1.Dpi = 100F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("far_valor", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("far_descricao", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 16.875F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // lblfar_descricao
            // 
            this.lblfar_descricao.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblfar_descricao.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblfar_descricao.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma.far_descricao")});
            this.lblfar_descricao.Dpi = 100F;
            this.lblfar_descricao.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblfar_descricao.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblfar_descricao.Name = "lblfar_descricao";
            this.lblfar_descricao.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblfar_descricao.SizeF = new System.Drawing.SizeF(232.2917F, 16.875F);
            this.lblfar_descricao.StylePriority.UseBackColor = false;
            this.lblfar_descricao.StylePriority.UseBorders = false;
            this.lblfar_descricao.StylePriority.UseFont = false;
            this.lblfar_descricao.StylePriority.UseTextAlignment = false;
            this.lblfar_descricao.Text = "[far_descricao]";
            this.lblfar_descricao.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // dsGestaoEscolar1
            // 
            this.dsGestaoEscolar1.DataSetName = "DSGestaoEscolar";
            this.dsGestaoEscolar1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter
            // 
            this.nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter.ClearBeforeFill = true;
            // 
            // ESCID
            // 
            this.ESCID.Name = "ESCID";
            // 
            // UNIID
            // 
            this.UNIID.Name = "UNIID";
            // 
            // TURID
            // 
            this.TURID.Name = "TURID";
            // 
            // TDSID
            // 
            this.TDSID.Name = "TDSID";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblAlunos});
            this.ReportHeader.Dpi = 100F;
            this.ReportHeader.HeightF = 20F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lblAlunos
            // 
            this.lblAlunos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblAlunos.Dpi = 100F;
            this.lblAlunos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAlunos.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblAlunos.Name = "lblAlunos";
            this.lblAlunos.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAlunos.SizeF = new System.Drawing.SizeF(700F, 20F);
            this.lblAlunos.StylePriority.UseBorders = false;
            this.lblAlunos.StylePriority.UseFont = false;
            this.lblAlunos.StylePriority.UseTextAlignment = false;
            this.lblAlunos.Text = "Alunos";
            this.lblAlunos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // TPCID
            // 
            this.TPCID.Name = "TPCID";
            // 
            // SubRelGrafConsolidadoAtivAvaliada
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.ReportHeader});
            this.DataAdapter = this.nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter;
            this.DataMember = "NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurma";
            this.DataSource = this.dsGestaoEscolar1;
            this.Margins = new System.Drawing.Printing.Margins(75, 75, 0, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ESCID,
            this.UNIID,
            this.TURID,
            this.TDSID,
            this.TPCID});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "16.1";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.SubRelGrafConsolidadoAtivAvaliada_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DSGestaoEscolar dsGestaoEscolar1;
        public DevExpress.XtraReports.Parameters.Parameter ESCID;
        public DevExpress.XtraReports.Parameters.Parameter UNIID;
        public DevExpress.XtraReports.Parameters.Parameter TURID;
        public DevExpress.XtraReports.Parameters.Parameter TDSID;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel lblAlunos;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRLabel lblpes_nome;
        private DevExpress.XtraReports.UI.XRLabel lblfar_descricao;
        private DSGestaoEscolarTableAdapters.NEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter nEW_Relatorio_GrafConsAtivAvaliada_AlunosTurmaTableAdapter;
        public DevExpress.XtraReports.Parameters.Parameter TPCID;
    }
}
