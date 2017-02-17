namespace MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo
{
    partial class DocDctPlanoCicloSerie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocDctPlanoCicloSerie));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.grpCabecalho = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblAnoLetivo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTitulo = new DevExpress.XtraReports.UI.XRLabel();
            this.ImgLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblNomeMunicipio = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNomeSecretaria = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDRE = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.dsGestaoEscolar1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolar();
            this.neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1 = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.DSGestaoEscolarTableAdapters.NEW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter();
            this.ARQID_LOGO = new DevExpress.XtraReports.Parameters.Parameter();
            this.CALID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ESCID = new DevExpress.XtraReports.Parameters.Parameter();
            this.UNIID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ENTID = new DevExpress.XtraReports.Parameters.Parameter();
            this.CODIGOESCOLA = new DevExpress.XtraReports.Parameters.Parameter();
            this.grpRodape = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.grpMensagem = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblSemRegistro = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText1,
            this.xrLabel1});
            this.Detail.HeightF = 57.08332F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 0, 100F);
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBandExceptFirstEntry;
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrRichText1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "NEW_Relatorio_0005_DocDctPlanoCicloSerie.plc_planoCiclo")});
            this.xrRichText1.Font = new System.Drawing.Font("Arial", 10F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 31.99997F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(680F, 23F);
            this.xrRichText1.StylePriority.UseBorders = false;
            this.xrRichText1.StylePriority.UsePadding = false;
            this.xrRichText1.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.LightGray;
            this.xrLabel1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_0005_DocDctPlanoCicloSerie.tci_nome")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 9.000015F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(680F, 22.99999F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            // grpCabecalho
            // 
            this.grpCabecalho.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2,
            this.xrPageInfo1,
            this.lblAnoLetivo,
            this.lblTitulo,
            this.ImgLogo,
            this.lblNomeMunicipio,
            this.lblNomeSecretaria,
            this.lblDRE,
            this.xrLabel2});
            this.grpCabecalho.HeightF = 154.9984F;
            this.grpCabecalho.Level = 1;
            this.grpCabecalho.Name = "grpCabecalho";
            this.grpCabecalho.RepeatEveryPage = true;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 146.9984F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(699.9999F, 8F);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 8F);
            this.xrPageInfo1.Format = "Data de emissão: {0:dd/MM/yyyy}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(543.75F, 128.9984F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(156.25F, 18F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblAnoLetivo
            // 
            this.lblAnoLetivo.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblAnoLetivo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 109.9983F);
            this.lblAnoLetivo.Name = "lblAnoLetivo";
            this.lblAnoLetivo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAnoLetivo.SizeF = new System.Drawing.SizeF(700F, 18.00002F);
            this.lblAnoLetivo.StylePriority.UseFont = false;
            this.lblAnoLetivo.StylePriority.UseTextAlignment = false;
            this.lblAnoLetivo.Text = "lblAnoLetivo";
            this.lblAnoLetivo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 84.99832F);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitulo.SizeF = new System.Drawing.SizeF(699.9999F, 23F);
            this.lblTitulo.StylePriority.UseFont = false;
            this.lblTitulo.StylePriority.UseTextAlignment = false;
            this.lblTitulo.Text = "Plano do ciclo/série";
            this.lblTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ImgLogo
            // 
            this.ImgLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.178914E-05F);
            this.ImgLogo.Name = "ImgLogo";
            this.ImgLogo.SizeF = new System.Drawing.SizeF(94.7505F, 80.00001F);
            this.ImgLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblNomeMunicipio
            // 
            this.lblNomeMunicipio.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblNomeMunicipio.LocationFloat = new DevExpress.Utils.PointFloat(94.7505F, 0F);
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
            this.lblNomeSecretaria.LocationFloat = new DevExpress.Utils.PointFloat(94.7505F, 20.00001F);
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_0005_DocDctPlanoCicloSerie.DRE")});
            this.lblDRE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDRE.LocationFloat = new DevExpress.Utils.PointFloat(94.7505F, 40.00003F);
            this.lblDRE.Name = "lblDRE";
            this.lblDRE.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDRE.SizeF = new System.Drawing.SizeF(509.4993F, 20F);
            this.lblDRE.StylePriority.UseFont = false;
            this.lblDRE.StylePriority.UseTextAlignment = false;
            this.lblDRE.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NEW_Relatorio_0005_DocDctPlanoCicloSerie.Escola")});
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(94.7505F, 60.00004F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(509.4993F, 20F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // dsGestaoEscolar1
            // 
            this.dsGestaoEscolar1.DataSetName = "DSGestaoEscolar";
            this.dsGestaoEscolar1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1
            // 
            this.neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1.ClearBeforeFill = true;
            // 
            // ARQID_LOGO
            // 
            this.ARQID_LOGO.Name = "ARQID_LOGO";
            // 
            // CALID
            // 
            this.CALID.Name = "CALID";
            this.CALID.Type = typeof(int);
            this.CALID.ValueInfo = "0";
            // 
            // ESCID
            // 
            this.ESCID.Name = "ESCID";
            this.ESCID.Type = typeof(long);
            this.ESCID.ValueInfo = "0";
            // 
            // UNIID
            // 
            this.UNIID.Name = "UNIID";
            this.UNIID.Type = typeof(short);
            this.UNIID.ValueInfo = "0";
            // 
            // ENTID
            // 
            this.ENTID.Name = "ENTID";
            this.ENTID.Type = typeof(System.Guid);
            // 
            // CODIGOESCOLA
            // 
            this.CODIGOESCOLA.Name = "CODIGOESCOLA";
            this.CODIGOESCOLA.Type = typeof(bool);
            this.CODIGOESCOLA.ValueInfo = "False";
            // 
            // grpRodape
            // 
            this.grpRodape.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
            this.grpRodape.HeightF = 20.29165F;
            this.grpRodape.Level = 1;
            this.grpRodape.Name = "grpRodape";
            this.grpRodape.PrintAtBottom = true;
            this.grpRodape.RepeatEveryPage = true;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Arial", 8F);
            this.xrPageInfo2.Format = "Pag. {0} / {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(599.9999F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(100F, 20.29165F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // grpMensagem
            // 
            this.grpMensagem.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSemRegistro});
            this.grpMensagem.HeightF = 45F;
            this.grpMensagem.Name = "grpMensagem";
            this.grpMensagem.Visible = false;
            // 
            // lblSemRegistro
            // 
            this.lblSemRegistro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(223)))));
            this.lblSemRegistro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(9)))));
            this.lblSemRegistro.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblSemRegistro.CanShrink = true;
            this.lblSemRegistro.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemRegistro.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblSemRegistro.Name = "lblSemRegistro";
            this.lblSemRegistro.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 10, 10, 100F);
            this.lblSemRegistro.SizeF = new System.Drawing.SizeF(699.9997F, 45F);
            this.lblSemRegistro.StylePriority.UseBackColor = false;
            this.lblSemRegistro.StylePriority.UseBorderColor = false;
            this.lblSemRegistro.StylePriority.UseBorders = false;
            this.lblSemRegistro.StylePriority.UseFont = false;
            this.lblSemRegistro.StylePriority.UsePadding = false;
            this.lblSemRegistro.StylePriority.UseTextAlignment = false;
            this.lblSemRegistro.Text = "Não há registro de Plano do Ciclo/Série nesta Unidade Escolar.";
            this.lblSemRegistro.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DocDctPlanoCicloSerie
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.grpCabecalho,
            this.grpRodape,
            this.grpMensagem});
            this.DataAdapter = this.neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1;
            this.DataMember = "NEW_Relatorio_0005_DocDctPlanoCicloSerie";
            this.DataSource = this.dsGestaoEscolar1;
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.Margins = new System.Drawing.Printing.Margins(100, 50, 100, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ARQID_LOGO,
            this.CALID,
            this.ESCID,
            this.UNIID,
            this.ENTID,
            this.CODIGOESCOLA});
            this.Version = "14.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.DocDctPlanoCicloSerie_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGestaoEscolar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand grpCabecalho;
        private DevExpress.XtraReports.UI.XRPictureBox ImgLogo;
        private DevExpress.XtraReports.UI.XRLabel lblNomeMunicipio;
        private DevExpress.XtraReports.UI.XRLabel lblNomeSecretaria;
        private DevExpress.XtraReports.UI.XRLabel lblDRE;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblTitulo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DSGestaoEscolar dsGestaoEscolar1;
        private DSGestaoEscolarTableAdapters.NEW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter neW_Relatorio_0005_DocDctPlanoCicloSerieTableAdapter1;
        public DevExpress.XtraReports.Parameters.Parameter ARQID_LOGO;
        public DevExpress.XtraReports.Parameters.Parameter CALID;
        public DevExpress.XtraReports.Parameters.Parameter ESCID;
        public DevExpress.XtraReports.Parameters.Parameter UNIID;
        public DevExpress.XtraReports.Parameters.Parameter ENTID;
        public DevExpress.XtraReports.Parameters.Parameter CODIGOESCOLA;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRRichText xrRichText1;
        private DevExpress.XtraReports.UI.GroupFooterBand grpRodape;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRLabel lblAnoLetivo;
        private DevExpress.XtraReports.UI.GroupHeaderBand grpMensagem;
        private DevExpress.XtraReports.UI.XRLabel lblSemRegistro;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
    }
}
