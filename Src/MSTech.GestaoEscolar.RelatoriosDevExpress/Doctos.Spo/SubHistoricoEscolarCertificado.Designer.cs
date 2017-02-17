namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
	partial class SubHistoricoEscolarCertificado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubHistoricoEscolarCertificado));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.dsGestaoEscolar1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolar();
            this.neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter();
            this.ALUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.NOMEALUNO = new DevExpress.XtraReports.Parameters.Parameter();
            this.NOMEDIRETOR = new DevExpress.XtraReports.Parameters.Parameter();
            this.CERTIFICADO = new DevExpress.XtraReports.Parameters.Parameter();
            this.NOMEESCOLA = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2,
            this.xrPanel1});
            this.ReportHeader.HeightF = 21.08167F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText1});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 13.54167F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(727F, 7.54F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrRichText1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "NEW_Relatorio_0005_HistoricoEscolarCertificado.certificado")});
            this.xrRichText1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(3F, 4F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(721F, 2F);
            this.xrRichText1.StylePriority.UseBorders = false;
            this.xrRichText1.StylePriority.UseFont = false;
            // 
            // xrPanel1
            // 
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(727F, 13.54167F);
            // 
            // dsGestaoEscolar1
            // 
            this.dsGestaoEscolar1.DataSetName = "DSGestaoEscolar";
            this.dsGestaoEscolar1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1
            // 
            this.neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1.ClearBeforeFill = true;
            // 
            // ALUID
            // 
            this.ALUID.Name = "ALUID";
            // 
            // NOMEALUNO
            // 
            this.NOMEALUNO.Name = "NOMEALUNO";
            // 
            // NOMEDIRETOR
            // 
            this.NOMEDIRETOR.Name = "NOMEDIRETOR";
            // 
            // CERTIFICADO
            // 
            this.CERTIFICADO.Name = "CERTIFICADO";
            // 
            // NOMEESCOLA
            // 
            this.NOMEESCOLA.Name = "NOMEESCOLA";
            // 
            // SubHistoricoEscolarCertificado
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataAdapter = this.neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1;
            this.DataMember = "NEW_Relatorio_0005_HistoricoEscolarCertificado";
            this.DataSource = this.dsGestaoEscolar1;
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 0, 0);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ALUID,
            this.NOMEALUNO,
            this.NOMEESCOLA,
            this.NOMEDIRETOR,
            this.CERTIFICADO});
            this.Version = "12.1";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.SubHistoricoEscolarCertificado_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DSGestaoEscolar dsGestaoEscolar1;
        private DSGestaoEscolarTableAdapters.NEW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter neW_Relatorio_0005_HistoricoEscolarCertificadoTableAdapter1;
        public DevExpress.XtraReports.Parameters.Parameter ALUID;
        public DevExpress.XtraReports.Parameters.Parameter NOMEALUNO;
        public DevExpress.XtraReports.Parameters.Parameter NOMEDIRETOR;
        private DevExpress.XtraReports.Parameters.Parameter CERTIFICADO;
        private DevExpress.XtraReports.UI.XRRichText xrRichText1;
        private DevExpress.XtraReports.UI.XRPanel xrPanel2;
        public DevExpress.XtraReports.Parameters.Parameter NOMEESCOLA;
	}
}
