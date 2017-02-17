using DevExpress.XtraPrinting;
namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
	partial class RelGrafIndividualNotas
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dsGestaoEscolar1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolar();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblSemRegistroGrafico = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.ImgLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblNomeMunicipio = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNomeSecretaria = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDRE = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTitulo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblTurma = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.neW_Relatorio_GrafIndividualNotas_TurmasTableAdapter = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_Relatorio_GrafIndividualNotas_TurmasTableAdapter();
            this.nEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter();
            this.ENTID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ADM = new DevExpress.XtraReports.Parameters.Parameter();
            this.USUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.GRUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ESCID = new DevExpress.XtraReports.Parameters.Parameter();
            this.UNIID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CALID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CAPID = new DevExpress.XtraReports.Parameters.Parameter();
            this.TPCID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CURID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CRRID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CRPID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ALUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.NIVELENSINOEDUCACAOINFANTIL = new DevExpress.XtraReports.Parameters.Parameter();
            this.ARQID_LOGO = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dsGestaoEscolar1
            // 
            this.dsGestaoEscolar1.DataSetName = "DSGestaoEscolar";
            this.dsGestaoEscolar1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSemRegistroGrafico});
            this.GroupHeader2.HeightF = 45F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader2_BeforePrint);
            // 
            // lblSemRegistroGrafico
            // 
            this.lblSemRegistroGrafico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(223)))));
            this.lblSemRegistroGrafico.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(9)))));
            this.lblSemRegistroGrafico.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblSemRegistroGrafico.CanShrink = true;
            this.lblSemRegistroGrafico.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemRegistroGrafico.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblSemRegistroGrafico.Name = "lblSemRegistroGrafico";
            this.lblSemRegistroGrafico.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 10, 10, 100F);
            this.lblSemRegistroGrafico.SizeF = new System.Drawing.SizeF(699.9997F, 45F);
            this.lblSemRegistroGrafico.StylePriority.UseBackColor = false;
            this.lblSemRegistroGrafico.StylePriority.UseBorderColor = false;
            this.lblSemRegistroGrafico.StylePriority.UseBorders = false;
            this.lblSemRegistroGrafico.StylePriority.UseFont = false;
            this.lblSemRegistroGrafico.StylePriority.UsePadding = false;
            this.lblSemRegistroGrafico.StylePriority.UseTextAlignment = false;
            this.lblSemRegistroGrafico.Text = "Não existe resultado para a pesquisa selecionada.";
            this.lblSemRegistroGrafico.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader3
            // 
            this.GroupHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ImgLogo,
            this.lblNomeMunicipio,
            this.lblNomeSecretaria,
            this.lblDRE,
            this.xrLabel2,
            this.lblTitulo,
            this.xrPageInfo1,
            this.xrLine2,
            this.lblTurma,
            this.xrSubreport1});
            this.GroupHeader3.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("tur_codigo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("tur_id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader3.HeightF = 182.5416F;
            this.GroupHeader3.Name = "GroupHeader3";
            this.GroupHeader3.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.GroupHeader3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader3_BeforePrint);
            // 
            // ImgLogo
            // 
            this.ImgLogo.LocationFloat = new DevExpress.Utils.PointFloat(0.1168887F, 0.583299F);
            this.ImgLogo.Name = "ImgLogo";
            this.ImgLogo.SizeF = new System.Drawing.SizeF(94.7505F, 80.00001F);
            this.ImgLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblNomeMunicipio
            // 
            this.lblNomeMunicipio.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblNomeMunicipio.LocationFloat = new DevExpress.Utils.PointFloat(94.86739F, 0.583299F);
            this.lblNomeMunicipio.Name = "lblNomeMunicipio";
            this.lblNomeMunicipio.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNomeMunicipio.SizeF = new System.Drawing.SizeF(509.4994F, 20F);
            this.lblNomeMunicipio.StylePriority.UseFont = false;
            this.lblNomeMunicipio.StylePriority.UseTextAlignment = false;
            this.lblNomeMunicipio.Text = "lblNomeMunicipio";
            this.lblNomeMunicipio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblNomeSecretaria
            // 
            this.lblNomeSecretaria.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblNomeSecretaria.LocationFloat = new DevExpress.Utils.PointFloat(94.86752F, 20.58331F);
            this.lblNomeSecretaria.Name = "lblNomeSecretaria";
            this.lblNomeSecretaria.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNomeSecretaria.SizeF = new System.Drawing.SizeF(509.4993F, 20F);
            this.lblNomeSecretaria.StylePriority.UseFont = false;
            this.lblNomeSecretaria.StylePriority.UseTextAlignment = false;
            this.lblNomeSecretaria.Text = "lblNomeSecretaria";
            this.lblNomeSecretaria.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDRE
            // 
            this.lblDRE.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_GrafIndividualNotas_Turmas.uad_nomeSuperior")});
            this.lblDRE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDRE.LocationFloat = new DevExpress.Utils.PointFloat(94.86755F, 40.58332F);
            this.lblDRE.Name = "lblDRE";
            this.lblDRE.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDRE.SizeF = new System.Drawing.SizeF(509.4991F, 20F);
            this.lblDRE.StylePriority.UseFont = false;
            this.lblDRE.StylePriority.UseTextAlignment = false;
            this.lblDRE.Text = "lblDRE";
            this.lblDRE.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_GrafIndividualNotas_Turmas.esc_nome")});
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(94.86755F, 60.58331F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(509.4991F, 20F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 90.58329F);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitulo.SizeF = new System.Drawing.SizeF(699.71F, 32F);
            this.lblTitulo.StylePriority.UseFont = false;
            this.lblTitulo.StylePriority.UseTextAlignment = false;
            this.lblTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 8F);
            this.xrPageInfo1.Format = "Data de emissão: {0:dd/MM/yyyy}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(543.7499F, 126.5416F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(156.25F, 22.99998F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 151.5416F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(699.4134F, 8F);
            // 
            // lblTurma
            // 
            this.lblTurma.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_GrafIndividualNotas_Turmas.CicloTurBim", "{0}")});
            this.lblTurma.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblTurma.LocationFloat = new DevExpress.Utils.PointFloat(0F, 126.5416F);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTurma.SizeF = new System.Drawing.SizeF(532.2917F, 22.99998F);
            this.lblTurma.StylePriority.UseFont = false;
            this.lblTurma.StylePriority.UseTextAlignment = false;
            this.lblTurma.Text = "lblTurma";
            this.lblTurma.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 159.5416F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.SubRelGrafIndividualNotas();
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(699.7101F, 23F);
            this.xrSubreport1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport1_BeforePrint);
            // 
            // neW_Relatorio_GrafIndividualNotas_TurmasTableAdapter
            // 
            this.neW_Relatorio_GrafIndividualNotas_TurmasTableAdapter.ClearBeforeFill = true;
            // 
            // nEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter
            // 
            this.nEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter.ClearBeforeFill = true;
            // 
            // ENTID
            // 
            this.ENTID.Name = "ENTID";
            // 
            // ADM
            // 
            this.ADM.Name = "ADM";
            // 
            // USUID
            // 
            this.USUID.Name = "USUID";
            // 
            // GRUID
            // 
            this.GRUID.Name = "GRUID";
            // 
            // ESCID
            // 
            this.ESCID.Name = "ESCID";
            // 
            // UNIID
            // 
            this.UNIID.Name = "UNIID";
            // 
            // CALID
            // 
            this.CALID.Name = "CALID";
            // 
            // CAPID
            // 
            this.CAPID.Name = "CAPID";
            // 
            // TPCID
            // 
            this.TPCID.Name = "TPCID";
            // 
            // CURID
            // 
            this.CURID.Name = "CURID";
            // 
            // CRRID
            // 
            this.CRRID.Name = "CRRID";
            // 
            // CRPID
            // 
            this.CRPID.Name = "CRPID";
            // 
            // ALUID
            // 
            this.ALUID.Name = "ALUID";
            // 
            // NIVELENSINOEDUCACAOINFANTIL
            // 
            this.NIVELENSINOEDUCACAOINFANTIL.Name = "NIVELENSINOEDUCACAOINFANTIL";
            // 
            // ARQID_LOGO
            // 
            this.ARQID_LOGO.Name = "ARQID_LOGO";
            this.ARQID_LOGO.Type = typeof(int);
            this.ARQID_LOGO.ValueInfo = "-1";
            // 
            // RelGrafIndividualNotas
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader2,
            this.GroupHeader3});
            this.DataMember = "NEW_Relatorio_GrafIndividualNotas_Turmas";
            this.DataSource = this.dsGestaoEscolar1;
            this.Margins = new System.Drawing.Printing.Margins(100, 50, 100, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ENTID,
            this.ADM,
            this.USUID,
            this.GRUID,
            this.ESCID,
            this.UNIID,
            this.CALID,
            this.CAPID,
            this.TPCID,
            this.CURID,
            this.CRRID,
            this.CRPID,
            this.ALUID,
            this.NIVELENSINOEDUCACAOINFANTIL,
            this.ARQID_LOGO});
            this.RequestParameters = false;
            this.Version = "14.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.RelGrafIndividualNotas_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DSGestaoEscolar dsGestaoEscolar1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel lblSemRegistroGrafico;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        private DevExpress.XtraReports.UI.XRLabel lblTurma;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DSGestaoEscolarTableAdapters.NEW_Relatorio_GrafIndividualNotas_TurmasTableAdapter neW_Relatorio_GrafIndividualNotas_TurmasTableAdapter;
        private DSGestaoEscolarTableAdapters.NEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter nEW_RelatorioGrafConsAtivAvaliada_CabecalhoTableAdapter;
        private DevExpress.XtraReports.Parameters.Parameter ENTID;
        private DevExpress.XtraReports.Parameters.Parameter ADM;
        private DevExpress.XtraReports.Parameters.Parameter USUID;
        private DevExpress.XtraReports.Parameters.Parameter GRUID;
        private DevExpress.XtraReports.Parameters.Parameter ESCID;
        private DevExpress.XtraReports.Parameters.Parameter UNIID;
        private DevExpress.XtraReports.Parameters.Parameter CALID;
        private DevExpress.XtraReports.Parameters.Parameter CAPID;
        private DevExpress.XtraReports.Parameters.Parameter TPCID;
        private DevExpress.XtraReports.Parameters.Parameter CURID;
        private DevExpress.XtraReports.Parameters.Parameter CRRID;
        private DevExpress.XtraReports.Parameters.Parameter CRPID;
        private DevExpress.XtraReports.Parameters.Parameter ALUID;
        private DevExpress.XtraReports.Parameters.Parameter NIVELENSINOEDUCACAOINFANTIL;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel lblNomeMunicipio;
        private DevExpress.XtraReports.UI.XRLabel lblNomeSecretaria;
        private DevExpress.XtraReports.UI.XRLabel lblDRE;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblTitulo;
        private DevExpress.XtraReports.UI.XRPictureBox ImgLogo;
        public DevExpress.XtraReports.Parameters.Parameter ARQID_LOGO;
	}
}
