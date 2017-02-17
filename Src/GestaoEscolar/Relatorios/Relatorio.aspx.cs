using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class Relatorios_Relatorio : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Guarda a página que deve ser retornado o site em caso de erros
    /// ou quando clicar no botão voltar.
    /// </summary>
    private string _VS_CaminhoPagina
    {
        get
        {
            if (ViewState["_VS_CaminhoPagina"] != null)
                return (string)ViewState["_VS_CaminhoPagina"];
            return string.Empty;
        }
        set
        {
            ViewState["_VS_CaminhoPagina"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda dados necessários para a página de retorno.
    /// </summary>
    private object VS_DadosPaginaRetorno
    {
        get
        {
            return ViewState["VS_DadosPaginaRetorno"];
        }
        set
        {
            ViewState["VS_DadosPaginaRetorno"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda dados necessários para a página de retorno Minhas turmas.
    /// </summary>
    private object VS_DadosPaginaRetorno_MinhasTurmas
    {
        get
        {
            return ViewState["VS_DadosPaginaRetorno_MinhasTurmas"];
        }
        set
        {
            ViewState["VS_DadosPaginaRetorno_MinhasTurmas"] = value;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this._lblTitulo.Text = ((MotherMasterPage)Page.Master).GetSiteMapTitle;
            _lblMessageLayout.Text = MSTech.CoreSSO.BLL.UtilBO.GetErroMessage(GetGlobalResourceObject("WebControls", "Relatorios.UCRelatorios.lblMessageLayout.MsgAvisoCompleto").ToString(), MSTech.CoreSSO.BLL.UtilBO.TipoMensagem.Informacao);

            //Se a url for igual entao redireciona para o index
            if (Uri.Equals(Request.Url, Request.UrlReferrer))
                this._VS_CaminhoPagina = "~/Index.aspx";
            else
                this._VS_CaminhoPagina = Convert.ToString(Request.UrlReferrer);

            if (Session["DadosPaginaRetorno"] != null)
            {
                VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                Session.Remove("DadosPaginaRetorno");

                VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                Session.Remove("VS_DadosTurmas");
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        //if (CFG_RelatorioBO.ExternalUrlReport == null)
            CFG_RelatorioBO.ExternalUrlReport = ApplicationWEB.ExternalUrlReport;

        if (String.IsNullOrEmpty(CFG_RelatorioBO.ExternalUrlReport))
            CreateUserControlReportViewer();
        else
            CreateIFrameReportViewer();
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        if (VS_DadosPaginaRetorno != null)
        {
            Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
            Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
        }
        Response.Redirect(_VS_CaminhoPagina, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();

    }

    #endregion

    #region Métodos

    protected void CreateUserControlReportViewer()
    {
        try
        {
            Placeholder1.Controls.Clear();
            WebControls_Relatorio_UCReportView ucSimpleControl = LoadControl("~/WebControls/Relatorio/UCReportView.ascx") as WebControls_Relatorio_UCReportView;

            ucSimpleControl._VS_TipoRelatorio = tipoRelatorio.Relatorio;
            //ucSimpleControl.SetTitlePage = ((MotherMasterPage)Page.Master).GetSiteMapTitle;

            Placeholder1.Controls.Add(ucSimpleControl);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = MSTech.CoreSSO.BLL.UtilBO.GetErroMessage("Não foi possível carregar o componente(UC) do documento.", MSTech.CoreSSO.BLL.UtilBO.TipoMensagem.Erro);
        }
    }

    protected void CreateIFrameReportViewer()
    {
        try
        {
            Placeholder1.Controls.Clear();

            string report = CFG_RelatorioBO.qsCurrentReportID_Encrypted;
            string tipRel = CFG_RelatorioBO.qsCurrentReportID_Encrypted;
            string paramsRel = CFG_RelatorioBO.qsCurrentReportParameters_Encrypted;

            string extUrl = String.Format("{0}/Relatorios/Relatorio.aspx?dummy={1}&tipRel={2}&params={3}"
                                            , CFG_RelatorioBO.ExternalUrlReport                            
                                            , HttpUtility.UrlEncode(report)
                                            , HttpUtility.UrlEncode(tipRel)
                                            , HttpUtility.UrlEncode(paramsRel));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<iframe id=\"ifUrlReport\" runat=\"server\" width=\"100%\" height=\"100%\" align=\"middle\" src=\"" + extUrl + "\" frameborder=\"0\"></iframe>");
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine(" loadingProgressIframe();"); //Função localizada em jsUtilGestao.js
            sb.AppendLine("</script>");

            LiteralControl iframeControl = new LiteralControl(sb.ToString());
            Placeholder1.Controls.Add(iframeControl);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = MSTech.CoreSSO.BLL.UtilBO.GetErroMessage("Não foi possível carregar o componente(IFrame) do documento.", MSTech.CoreSSO.BLL.UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

}
