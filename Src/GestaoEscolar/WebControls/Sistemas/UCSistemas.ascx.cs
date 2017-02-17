using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

/// <summary>
/// PartialCaching - cache do conteúdo do UserControl.
/// O parâmetro "UsuarioLogado" define a variação desse cache (implementado na Global.asax).
/// </summary>
[PartialCaching(10, null, null, "UsuarioLogado", true)]
public partial class WebControls_Sistemas_UCSistemas : MotherUserControl
{
    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        // Altera o tempo do cache do UserControl para usar a configuração de cache curto.
        this.CachePolicy.Duration = new TimeSpan(0, 0, ApplicationWEB.AppMinutosCacheCurto, 0);
    }

    protected void odsSistemas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!e.ExecutingSelectCount)
            e.InputParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id);
    }

    protected void rptSistemas_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            string sis_nome = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "sis_nome"));
            string sis_urlImagem = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "sis_urlImagem"));
            string imagePath = String.Concat(__SessionWEB.UrlCoreSSO, "/App_Themes/", __SessionWEB.TemaPadrao, "/images/logos/");

            Image img = (Image)e.Item.FindControl("imgSistema");
            img.AlternateText = sis_nome;
            if (!string.IsNullOrEmpty(sis_urlImagem))
            {
                img.ImageUrl = imagePath + sis_urlImagem;
            }
        }
    }

    #endregion
}
