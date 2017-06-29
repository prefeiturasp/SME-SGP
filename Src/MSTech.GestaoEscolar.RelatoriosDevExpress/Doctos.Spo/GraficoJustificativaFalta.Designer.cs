namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    partial class GraficoJustificativaFalta
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
            DevExpress.XtraCharts.TextAnnotation textAnnotation1 = new DevExpress.XtraCharts.TextAnnotation();
            DevExpress.XtraCharts.ChartAnchorPoint chartAnchorPoint1 = new DevExpress.XtraCharts.ChartAnchorPoint();
            DevExpress.XtraCharts.FreePosition freePosition1 = new DevExpress.XtraCharts.FreePosition();
            DevExpress.XtraCharts.FullStackedBarSeriesView fullStackedBarSeriesView1 = new DevExpress.XtraCharts.FullStackedBarSeriesView();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblSemRegistroGrafico = new DevExpress.XtraReports.UI.XRLabel();
            this.ENTID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CALID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ESCID = new DevExpress.XtraReports.Parameters.Parameter();
            this.UNIID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CURID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CRRID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CRPID = new DevExpress.XtraReports.Parameters.Parameter();
            this.TURID = new DevExpress.XtraReports.Parameters.Parameter();
            this.UADSUPERIOR = new DevExpress.XtraReports.Parameters.Parameter();
            this.ADM = new DevExpress.XtraReports.Parameters.Parameter();
            this.USUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.GRUID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ARQID_LOGO = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblCalendario = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNomeMunicipio = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNomeSecretaria = new DevExpress.XtraReports.UI.XRLabel();
            this.lblEscola = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblFiltros = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTitulo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDRE = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.ImgLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.dsGestaoEscolar1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolar();
            this.nEW_Relatorio_GraficoJustificativaFaltaTableAdapter = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_Relatorio_GraficoJustificativaFaltaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(fullStackedBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 75F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 75F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.GroupHeader1.Dpi = 100F;
            this.GroupHeader1.HeightF = 337.9167F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // xrPanel1
            // 
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrChart1});
            this.xrPanel1.Dpi = 100F;
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(699.7068F, 337.9167F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.ForeColor = System.Drawing.Color.Black;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(439.8839F, 21.83336F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(164.5833F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.Text = "Justificativas de falta";
            // 
            // xrChart1
            // 
            chartAnchorPoint1.Y = 43;
            textAnnotation1.AnchorPoint = chartAnchorPoint1;
            textAnnotation1.AutoSize = false;
            textAnnotation1.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            textAnnotation1.ConnectorStyle = DevExpress.XtraCharts.AnnotationConnectorStyle.None;
            textAnnotation1.Height = 20;
            textAnnotation1.Name = "annLegenda";
            textAnnotation1.ShapeKind = DevExpress.XtraCharts.ShapeKind.Rectangle;
            freePosition1.DockCorner = DevExpress.XtraCharts.DockCorner.RightTop;
            freePosition1.InnerIndents.Bottom = 229;
            freePosition1.InnerIndents.Left = 0;
            freePosition1.InnerIndents.Right = 29;
            freePosition1.InnerIndents.Top = 14;
            textAnnotation1.ShapePosition = freePosition1;
            textAnnotation1.Text = "Justificativas de falta";
            textAnnotation1.TextAlignment = System.Drawing.StringAlignment.Far;
            textAnnotation1.Width = 45;
            this.xrChart1.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation1});
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart1.Dpi = 100F;
            this.xrChart1.Legend.Name = "Default Legend";
            this.xrChart1.Legend.Padding.Right = 20;
            this.xrChart1.Legend.Padding.Top = 20;
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrChart1.Name = "xrChart1";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart1.SeriesTemplate.View = fullStackedBarSeriesView1;
            this.xrChart1.SizeF = new System.Drawing.SizeF(680F, 318.0417F);
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSemRegistroGrafico});
            this.GroupHeader2.Dpi = 100F;
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
            this.lblSemRegistroGrafico.Dpi = 100F;
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
            // ENTID
            // 
            this.ENTID.Name = "ENTID";
            // 
            // CALID
            // 
            this.CALID.Name = "CALID";
            // 
            // ESCID
            // 
            this.ESCID.Name = "ESCID";
            // 
            // UNIID
            // 
            this.UNIID.Name = "UNIID";
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
            // TURID
            // 
            this.TURID.Name = "TURID";
            // 
            // UADSUPERIOR
            // 
            this.UADSUPERIOR.Name = "UADSUPERIOR";
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
            // ARQID_LOGO
            // 
            this.ARQID_LOGO.Name = "ARQID_LOGO";
            // 
            // GroupHeader3
            // 
            this.GroupHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblCalendario,
            this.lblNomeMunicipio,
            this.lblNomeSecretaria,
            this.lblEscola,
            this.xrPageInfo1,
            this.lblFiltros,
            this.lblTitulo,
            this.lblDRE,
            this.xrLine2,
            this.ImgLogo});
            this.GroupHeader3.Dpi = 100F;
            this.GroupHeader3.HeightF = 161.67F;
            this.GroupHeader3.Level = 2;
            this.GroupHeader3.Name = "GroupHeader3";
            this.GroupHeader3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader3_BeforePrint);
            // 
            // lblCalendario
            // 
            this.lblCalendario.Dpi = 100F;
            this.lblCalendario.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblCalendario.LocationFloat = new DevExpress.Utils.PointFloat(0.5074987F, 115F);
            this.lblCalendario.Name = "lblCalendario";
            this.lblCalendario.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCalendario.SizeF = new System.Drawing.SizeF(698.7068F, 18F);
            this.lblCalendario.StylePriority.UseFont = false;
            this.lblCalendario.StylePriority.UseTextAlignment = false;
            this.lblCalendario.Text = "lblCalendario";
            this.lblCalendario.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblNomeMunicipio
            // 
            this.lblNomeMunicipio.Dpi = 100F;
            this.lblNomeMunicipio.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblNomeMunicipio.LocationFloat = new DevExpress.Utils.PointFloat(95.25801F, 0F);
            this.lblNomeMunicipio.Name = "lblNomeMunicipio";
            this.lblNomeMunicipio.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNomeMunicipio.SizeF = new System.Drawing.SizeF(509.2094F, 20F);
            this.lblNomeMunicipio.StylePriority.UseFont = false;
            this.lblNomeMunicipio.StylePriority.UseTextAlignment = false;
            this.lblNomeMunicipio.Text = "lblNomeMunicipio";
            this.lblNomeMunicipio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblNomeSecretaria
            // 
            this.lblNomeSecretaria.Dpi = 100F;
            this.lblNomeSecretaria.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblNomeSecretaria.LocationFloat = new DevExpress.Utils.PointFloat(95.25801F, 20.00001F);
            this.lblNomeSecretaria.Name = "lblNomeSecretaria";
            this.lblNomeSecretaria.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNomeSecretaria.SizeF = new System.Drawing.SizeF(509.2093F, 20F);
            this.lblNomeSecretaria.StylePriority.UseFont = false;
            this.lblNomeSecretaria.StylePriority.UseTextAlignment = false;
            this.lblNomeSecretaria.Text = "lblNomeSecretaria";
            this.lblNomeSecretaria.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblEscola
            // 
            this.lblEscola.Dpi = 100F;
            this.lblEscola.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblEscola.LocationFloat = new DevExpress.Utils.PointFloat(95.25801F, 60.00004F);
            this.lblEscola.Name = "lblEscola";
            this.lblEscola.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblEscola.SizeF = new System.Drawing.SizeF(509.21F, 20F);
            this.lblEscola.StylePriority.UseFont = false;
            this.lblEscola.StylePriority.UseTextAlignment = false;
            this.lblEscola.Text = "lblEscola";
            this.lblEscola.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 100F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 8F);
            this.xrPageInfo1.Format = "Data de emissão: {0:dd/MM/yyyy}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(544.0092F, 135.67F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(155.2084F, 18F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblFiltros
            // 
            this.lblFiltros.Dpi = 100F;
            this.lblFiltros.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblFiltros.LocationFloat = new DevExpress.Utils.PointFloat(0.2174988F, 135.67F);
            this.lblFiltros.Name = "lblFiltros";
            this.lblFiltros.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFiltros.SizeF = new System.Drawing.SizeF(543.7916F, 18F);
            this.lblFiltros.StylePriority.UseFont = false;
            this.lblFiltros.StylePriority.UseTextAlignment = false;
            this.lblFiltros.Text = "lblFiltros";
            this.lblFiltros.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dpi = 100F;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0.2174988F, 90F);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitulo.SizeF = new System.Drawing.SizeF(699.71F, 23F);
            this.lblTitulo.StylePriority.UseFont = false;
            this.lblTitulo.StylePriority.UseTextAlignment = false;
            this.lblTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblDRE
            // 
            this.lblDRE.Dpi = 100F;
            this.lblDRE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDRE.LocationFloat = new DevExpress.Utils.PointFloat(95.25801F, 40.00003F);
            this.lblDRE.Name = "lblDRE";
            this.lblDRE.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDRE.SizeF = new System.Drawing.SizeF(509.21F, 20F);
            this.lblDRE.StylePriority.UseFont = false;
            this.lblDRE.StylePriority.UseTextAlignment = false;
            this.lblDRE.Text = "lblDRE";
            this.lblDRE.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 100F;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0.07250977F, 153.67F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(699.4134F, 8F);
            // 
            // ImgLogo
            // 
            this.ImgLogo.Dpi = 100F;
            this.ImgLogo.LocationFloat = new DevExpress.Utils.PointFloat(0.5074965F, 0F);
            this.ImgLogo.Name = "ImgLogo";
            this.ImgLogo.SizeF = new System.Drawing.SizeF(94.7505F, 80.00001F);
            this.ImgLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // dsGestaoEscolar1
            // 
            this.dsGestaoEscolar1.DataSetName = "DSGestaoEscolar";
            this.dsGestaoEscolar1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // nEW_Relatorio_GraficoJustificativaFaltaTableAdapter
            // 
            this.nEW_Relatorio_GraficoJustificativaFaltaTableAdapter.ClearBeforeFill = true;
            // 
            // GraficoJustificativaFalta
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupHeader3});
            this.DataAdapter = this.nEW_Relatorio_GraficoJustificativaFaltaTableAdapter;
            this.DataMember = "NEW_Relatorio_GraficoJustificativaFalta";
            this.DataSource = this.dsGestaoEscolar1;
            this.Margins = new System.Drawing.Printing.Margins(75, 75, 75, 75);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ENTID,
            this.CALID,
            this.ESCID,
            this.UNIID,
            this.CURID,
            this.CRRID,
            this.CRPID,
            this.TURID,
            this.UADSUPERIOR,
            this.ADM,
            this.USUID,
            this.GRUID,
            this.ARQID_LOGO});
            this.Version = "16.1";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.GraficoJustificativaFalta_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(fullStackedBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel lblSemRegistroGrafico;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DevExpress.XtraReports.UI.XRChart xrChart1;
        public DevExpress.XtraReports.Parameters.Parameter ENTID;
        public DevExpress.XtraReports.Parameters.Parameter CALID;
        public DevExpress.XtraReports.Parameters.Parameter ESCID;
        public DevExpress.XtraReports.Parameters.Parameter UNIID;
        public DevExpress.XtraReports.Parameters.Parameter CURID;
        public DevExpress.XtraReports.Parameters.Parameter CRRID;
        public DevExpress.XtraReports.Parameters.Parameter CRPID;
        public DevExpress.XtraReports.Parameters.Parameter TURID;
        public DevExpress.XtraReports.Parameters.Parameter UADSUPERIOR;
        public DevExpress.XtraReports.Parameters.Parameter ADM;
        public DevExpress.XtraReports.Parameters.Parameter USUID;
        public DevExpress.XtraReports.Parameters.Parameter GRUID;
        public DevExpress.XtraReports.Parameters.Parameter ARQID_LOGO;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        private DevExpress.XtraReports.UI.XRLabel lblCalendario;
        private DevExpress.XtraReports.UI.XRLabel lblNomeMunicipio;
        private DevExpress.XtraReports.UI.XRLabel lblNomeSecretaria;
        private DevExpress.XtraReports.UI.XRLabel lblEscola;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel lblFiltros;
        private DevExpress.XtraReports.UI.XRLabel lblTitulo;
        private DevExpress.XtraReports.UI.XRLabel lblDRE;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRPictureBox ImgLogo;
        private DSGestaoEscolar dsGestaoEscolar1;
        private DSGestaoEscolarTableAdapters.NEW_Relatorio_GraficoJustificativaFaltaTableAdapter nEW_Relatorio_GraficoJustificativaFaltaTableAdapter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
