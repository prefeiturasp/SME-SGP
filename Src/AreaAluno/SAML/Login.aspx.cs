using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.Security;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Validation.Exceptions;

public partial class SAML_Login : MotherPageLogado
{
    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //#region Trace
                //// ***** TRACE *****
                //// Write a trace message
                //if (Trace.IsEnabled)
                //{
                //    // Forms
                //    if (HttpContext.Current.User != null)
                //    {
                //        Trace.Write("HttpContext.Current.User", HttpContext.Current.User.ToString());
                //        Trace.Write("HttpContext.Current.User.Identity", HttpContext.Current.User.Identity.ToString());
                //        if (HttpContext.Current.User.Identity is FormsIdentity)
                //        {
                //            Trace.Write("HttpContext.Current.User.Identity.IsAuthenticated", HttpContext.Current.User.Identity.IsAuthenticated.ToString());
                //            if (HttpContext.Current.User.Identity.IsAuthenticated)
                //            {
                //                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                //                Trace.Write("FormsIdentity.Ticket.Name", id.Ticket.Name);
                //                Trace.Write("FormsIdentity.Ticket.IssueDate", id.Ticket.IssueDate.ToString());
                //            }
                //        }
                //    }
                //    else
                //    {
                //        Trace.Write("HttpContext.Current.User", "NULL");
                //    }

                //    // Session
                //    if(__SessionWEB.__UsuarioWEB.Usuario != null)
                //    {
                //        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario.usu_login", __SessionWEB.__UsuarioWEB.Usuario.usu_login);
                //        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario.usu_login", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                //    }
                //    else
                //    {
                //        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario", "NULL");
                //    }
                //}
                //// ***** FIM *****
                //#endregion

                // Verifica se usuário está autenticado
                if (UserIsAuthenticated())
                {
                    __SessionWEB.__UsuarioWEB.responsavel = false;

                    // Carrega grupos do usuário
                    IList<SYS_Grupo> list = SYS_GrupoBO.GetSelectBySis_idAndUsu_id(
                       __SessionWEB.__UsuarioWEB.Usuario.usu_id
                       , ApplicationWEB.AreaAlunoSistemaID);

                    // Verifica se foi carregado os grupos do usuário
                    if (list.Count > 0)
                    {
                        // Verifica se usuário logado possui um único grupo para carregar na Session, 
                        // caso possua vários grupos será necessário selecionar apenas um grupo
                        if (list.Count == 1)
                        {
                            __SessionWEB.__UsuarioWEB.Grupo = list[0];

                            HttpContext.Current.User.Identity.AddGrupoId(Request, __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                            //// Realiza autenticação do usuário no Sistema Gestão Acadêmica
                            //SYS_UsuarioBO.AutenticarUsuario(__SessionWEB.__UsuarioWEB.Usuario, __SessionWEB.__UsuarioWEB.Grupo);

                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, String.Format("Autenticação do usuário ( {0} ) com grupo ( {1} ) no sistema ( {2} ).", __SessionWEB.__UsuarioWEB.Usuario.usu_login, __SessionWEB.__UsuarioWEB.Grupo.gru_nome, __SessionWEB.TituloSistema));

                            //Response.Redirect("~/Index.aspx", false);
                            Response.Redirect("~/Login.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            rptGrupos.DataSource = list;
                            rptGrupos.DataBind();
                            divGrupos.Visible = true;
                        }
                    }
                    else
                        throw new ValidationException("Não foi possível atender a solicitação, nenhum grupo de usuário encontrado.");
                }
                else
                    throw new ValidationException("O usuário não tem permissão de acesso ao sistema.");
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message + "<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Alerta);
                btnVoltar.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atender a solicitação.<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Erro);
                btnVoltar.Visible = true;
            }
        }
    }

    protected void _rptGrupos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                //Limpa a busca realizada com outra visão
                __SessionWEB.BuscaRealizada = new MSTech.GestaoEscolar.BLL.BuscaGestao();

                // Carrega grupo selecionado na Session
                SYS_Grupo grupo = new SYS_Grupo { gru_id = new Guid(e.CommandArgument.ToString()) };
                __SessionWEB.__UsuarioWEB.Grupo = SYS_GrupoBO.GetEntity(grupo);

                // Realiza autenticação do usuário no Sistema Gestão Acadêmica
                HttpContext.Current.User.Identity.AddGrupoId(Request, __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, String.Format("Autenticação do usuário ( {0} ) com grupo ( {1} ) no sistema ( {2} ).", __SessionWEB.__UsuarioWEB.Usuario.usu_login, __SessionWEB.__UsuarioWEB.Grupo.gru_nome, __SessionWEB.TituloSistema));

                //Response.Redirect("~/Index.aspx", false);
                Response.Redirect("~/Login.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atender a solicitação.<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Erro);
            btnVoltar.Visible = true;
        }
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB.UrlCoreSSO + "/Sistema.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
