using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.UI.HtmlControls;
using MSTech.GestaoEscolar.BLL;

namespace AreaAluno
{
    public partial class MasterPagePaginaInicial : MotherMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Exibe o título no navegador
            Page.Title = __SessionWEB.TituloGeral;

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

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LISTA_SISTEMAS_AREA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                  && !ApplicationWEB.LoginProprioDoSistema)
            {
                //Carrega logo geral do sistema
                imgGeral.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoGeral);
                imgGeral.ToolTip = __SessionWEB.TituloGeral;
                imgGeral.AlternateText = __SessionWEB.TituloGeral;
                ImgLogoGeral.ToolTip = __SessionWEB.TituloGeral;
                ImgLogoGeral.NavigateUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";
            }
            else
            {
                h1Logo.Visible = ImgLogoGeral.Visible = false;
                if (UCSistemas1 != null)
                {
                    UCSistemas1.Visible = false;
                }
            }

            //Atribui o caminho do logo do sistema atual, caso ele exista no Sistema Administrativo
            if (string.IsNullOrEmpty(__SessionWEB.UrlLogoSistema))
                ImgLogoSistemaAtual.Visible = false;
            else
            {
                //Carrega logo do sistema atual
                imgSistemaAtual.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoSistema);
                imgSistemaAtual.AlternateText = __SessionWEB.TituloSistema;
                imgSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
                ImgLogoSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
                ImgLogoSistemaAtual.NavigateUrl = "~/Index.aspx";
            }

            // Se ja tiver logado mostra o linkbutton pra selecionar o aluno, senão esconde
            if (Session["Pes_Id_Responsavel"] != null)
            {
                if (Convert.ToInt32(Session["Qtde_Filhos_Responsavel"]) > 1)
                {
                    lbSelecaoAlunos.Visible = true;
                }
                else
                {
                    lbSelecaoAlunos.Visible = false;
                }

                imgLogoAreaAluno.Visible = false;
                imgLogoAreaResp.Visible = true;
            }
            else
            {
                lbSelecaoAlunos.Visible = false;
                imgLogoAreaAluno.Visible = true;
                imgLogoAreaResp.Visible = false;
            }
        }
    }
}