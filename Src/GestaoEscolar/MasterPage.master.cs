using System;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class MasterPage : MotherMasterPage
{
    #region Métodos

    /// <summary>
    /// Altera o texto dos labels que tem o "*" indicando que é obrigatório, criando uma tag
    /// <span> em volta do "*". Aplica a classe de css "asteriscoObrigatorio".
    /// </summary>
    private void SetaLabelsObrigatorios()
    {
        SetaLabelsObrigatorios(form1.Controls);
    }

    /// <summary>
    /// Percorre os controles procurando os labels. Altera o texto dos labels que terminem 
    /// com "*", para aplicar a classe de css "asteriscoObrigatorio", em volta do "*".
    /// </summary>
    /// <param name="controls">Coleção de controles</param>
    private void SetaLabelsObrigatorios(ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            if (control.HasControls())
            {
                SetaLabelsObrigatorios(control.Controls);
            }

            if (control.GetType() == typeof(Label))
            {
                Label lbl = control as Label;
                if (lbl != null)
                    lbl.Text = SubstituiAsterisco(lbl.Text);
            }
        }
    }

    /// <summary>
    /// Substitui o asterisco do texto por uma tag <span> com a classe de css "asteriscoObrigatorio".
    /// </summary>
    /// <param name="texto">Texto de origem</param>
    /// <returns>Texto de retorno com a tag</returns>
    private string SubstituiAsterisco(string texto)
    {
        if (texto.Trim().EndsWith("*"))
        {
            texto = texto.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
            // Altera o "*" no texto do label para adicionar a classe de css "obrigatorio".
            texto = texto.Replace("*", ApplicationWEB.TextoAsteriscoObrigatorio);
        }

        return texto;
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        //Exibe o título no navegador
        Page.Title = __SessionWEB.TituloGeral + " - " + __SessionWEB.TituloSistema;

        #region Adiciona links de favicon

        HtmlLink link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon.ico");
        link.Attributes["rel"] = "shortcut icon";
        link.Attributes["sizes"] = "";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-57x57.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "57x57";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-114x114.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "114x114";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-72x72.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "72x72";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-144x144.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "144x144";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-60x60.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "60x60";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-120x120.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "120x120";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-76x76.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "76x76";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-152x152.png");
        link.Attributes["rel"] = "apple-touch-icon";
        link.Attributes["sizes"] = "152x152";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-196x196.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "196x196";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-160x160.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "160x160";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-96x96.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "96x96";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-16x16.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "16x16";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-32x32.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "32x32";
        Page.Header.Controls.Add(link);

        link = new HtmlLink();
        link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-32x32.png");
        link.Attributes["rel"] = "icon";
        link.Attributes["sizes"] = "32x32";
        Page.Header.Controls.Add(link);

        HtmlMeta meta = new HtmlMeta();
        meta.Name = "msapplication-TileImage";
        meta.Content = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/mstile-144x144.png");
        Page.Header.Controls.Add(meta);

        meta = new HtmlMeta();
        meta.Name = "msapplication-config";
        meta.Content = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/browserconfig.xml");
        Page.Header.Controls.Add(meta);

        #endregion

        if (TemaAtual == "IntranetSME")
        {
            divBarraSP.Visible = true;
        }

        ImgLogoGeral.ToolTip = __SessionWEB.TituloGeral;
        ImgLogoGeral.NavigateUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";

        ImgLogoSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
        ImgLogoSistemaAtual.NavigateUrl = "~/Index.aspx";
        
        UCPluginNotificacao.Visible = ApplicationWEB.LigarPluginNotificacoes;
        
        if (!IsPostBack)
        {
            // Esconde o link "Meus dados" caso o sistema esteja utilizando a integração com AD (a alteração dos dados
            // será feita pelo Core).
            divMeusDados.Visible = !ApplicationWEB.UtilizarIntegracaoADUsuario;

            try
            {

                //Exibe o contato do help desk do cliente
                spnHelpDesk.InnerHtml = __SessionWEB.HelpDeskContato;

                if (__SessionWEB.__UsuarioWEB.Grupo != null)
                {
                    string menuXml = GestaoEscolarUtilBO.CarregarMenu(
                        __SessionWEB.__UsuarioWEB.Grupo.sis_id
                        , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                        , __SessionWEB.__UsuarioWEB.Grupo.vis_id
                        , 30);
                    if (String.IsNullOrEmpty(menuXml))
                        menuXml = "<menus/>";
                    XmlDataSource1.Data = menuXml;
                    XmlDataSource1.DataBind();

                    //Carrrega nome do usuario logado no sistema e exibe na pagina na mensagem de Bem-vindo.
                    lblUsuario.Text = RetornaLoginFormatado(__SessionWEB.UsuarioLogado);

                    //Exibe a mensagem de copyright no rodapé.
                    lblCopyright.Text = "<span class='tituloGeral'>" + __SessionWEB.TituloGeral + " - " + __SessionWEB.TituloSistema + "</span><span class='sep'> - </span><span class='versao'>" + _VS_versao + "</span><span class='sep'> - </span><span class='mensagem'>" + __SessionWEB.MensagemCopyright + "</span>";

                    //Atribui o caminho do logo geral do sistema, caso ele exista no Sistema Administrativo
                    if (string.IsNullOrEmpty(__SessionWEB.UrlLogoGeral))
                        ImgLogoGeral.Visible = false;
                    else if (imgGeral != null)
                    {
                        //Carrega logo geral do sistema
                        imgGeral.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoGeral);
                        imgGeral.ToolTip = __SessionWEB.TituloGeral;
                        imgGeral.AlternateText = __SessionWEB.TituloGeral;
                    }

                    //Atribui o caminho do logo do sistema atual, caso ele exista no Sistema Administrativo
                    if (string.IsNullOrEmpty(__SessionWEB.UrlLogoSistema))
                        ImgLogoSistemaAtual.Visible = false;
                    else if (imgSistemaAtual != null)
                    {
                        //Carrega logo do sistema atual
                        imgSistemaAtual.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoSistema);
                        imgSistemaAtual.AlternateText = __SessionWEB.TituloSistema;
                        imgSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
                    }

                    //TODO: Descomentar codigo abaixo.
                    imgInstituicao.Visible = false;
                    ImgLogoInstitiuicao.Visible = false;

                    ////Atribui o caminho do logo cliente, caso ele exista no Sistema Administrativo
                    //if (string.IsNullOrEmpty(__SessionWEB.UrlInstituicao.Trim()))
                    //    ImgLogoInstitiuicao.Visible = false;
                    //else
                    //{
                    //    //Carrega logo do cliente
                    //    ImgLogoInstitiuicao.ImageUrl = UtilBO.UrlImagem(__SessionWEB.UrlLogoInstituicao);
                    //    ImgLogoInstitiuicao.ToolTip = string.Empty;
                    //    ImgLogoInstitiuicao.NavigateUrl = __SessionWEB.UrlInstituicao;
                    //}

                    //imgImageInstituicao.Visible = !ImgLogoInstitiuicao.Visible;
                    //imgImageInstituicao.ImageUrl = UtilBO.UrlImagem(__SessionWEB.UrlLogoInstituicao);

                    // Carrega a url do Help, do cache do sistema.
                    string urlHelp =
                       SYS_ModuloSiteMapBO.SelecionaUrlHelpByUrl_Cache(Request.AppRelativeCurrentExecutionFilePath,
                                                                       ApplicationWEB.SistemaID);
                    if (!string.IsNullOrEmpty(urlHelp))
                    {
                        if (!urlHelp.StartsWith("~") && !urlHelp.StartsWith("http://") && !urlHelp.StartsWith("https://"))
                            urlHelp = "http://" + urlHelp;
                        hplHelp.Visible = true;
                        hplHelp.NavigateUrl = urlHelp;
                        hplHelp.ToolTip = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.MENSAGEM_ICONE_HELP);
                    }
                    else
                        hplHelp.Visible = false;
                }
                else
                {
                    Response.Redirect("~/logout.ashx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                lblUsuario.Text = __SessionWEB.__UsuarioWEB.Usuario.usu_login;
                ApplicationWEB._GravaErro(ex);
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        SetaLabelsObrigatorios();
    }

    protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        ApplicationWEB._GravaErro(e.Exception);
    }

    #endregion
}
