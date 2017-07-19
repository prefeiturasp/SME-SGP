using System;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using System.Web;
using System.Web.UI;

namespace GestaoEscolar.Academico.Sondagem
{
    public partial class Agendamento : MotherPageLogado
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        UCAgendamento1.VS_paginaBusca = "Academico/Sondagem/Busca.aspx";
                        UCAgendamento1.bntSalvarVisible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        UCAgendamento1.btnCancelarText = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                                         GetGlobalResourceObject("Academico", "Sondagem.Agendamento.btnCancelar.Text").ToString() :
                                                         GetGlobalResourceObject("Academico", "Sondagem.Agendamento.btnVoltar.Text").ToString();

                        UCAgendamento1._LoadFromEntity(PreviousPage.SelectedItem);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.SelecioneSondagem").ToString(), UtilBO.TipoMensagem.Alerta);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    UCAgendamento1.lblMessageText = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }
        
        #endregion
    }
}