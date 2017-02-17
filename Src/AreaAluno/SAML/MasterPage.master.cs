using System;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class SAML_MasterPage : MotherMasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // Exibe o título no navegador
                Page.Title = __SessionWEB.TituloGeral;

                //Exibe a mensagem de copyright no rodapé.
                lblCopyright.Text = "<span class='tituloGeral'>" + __SessionWEB.TituloGeral + " - " + __SessionWEB.TituloSistema + "</span><span class='sep'> - </span><span class='versao'>" + _VS_versao + "</span><span class='sep'> - </span><span class='mensagem'>" + __SessionWEB.MensagemCopyright + "</span>";

                //Atribui o caminho do logo geral do sistema, caso ele exista no Sistema Administrativo
                if (string.IsNullOrEmpty(__SessionWEB.UrlLogoGeral))
                    ImgLogoGeral.Visible = false;
                else
                {
                    //Carrega logo geral do sistema
                    imgGeral.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoGeral);
                    ImgLogoGeral.ToolTip = __SessionWEB.TituloGeral;
                    ImgLogoGeral.NavigateUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";
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
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }
    }
}
