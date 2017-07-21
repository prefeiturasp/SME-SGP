using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class SAML_Login : MotherPageLogadoCompressedViewState
{
    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                #region Trace

                // ***** TRACE *****
                // Write a trace message
                if (Trace.IsEnabled)
                {
                    // Forms
                    if (HttpContext.Current.User != null)
                    {
                        Trace.Write("HttpContext.Current.User", HttpContext.Current.User.ToString());
                        Trace.Write("HttpContext.Current.User.Identity", HttpContext.Current.User.Identity.ToString());
                        if (HttpContext.Current.User.Identity is FormsIdentity)
                        {
                            Trace.Write("HttpContext.Current.User.Identity.IsAuthenticated", HttpContext.Current.User.Identity.IsAuthenticated.ToString());
                            if (HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                                Trace.Write("FormsIdentity.Ticket.Name", id.Ticket.Name);
                                Trace.Write("FormsIdentity.Ticket.IssueDate", id.Ticket.IssueDate.ToString());
                            }
                        }
                    }
                    else
                    {
                        Trace.Write("HttpContext.Current.User", "NULL");
                    }

                    // Session
                    if (__SessionWEB.__UsuarioWEB.Usuario != null)
                    {
                        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario.usu_login", __SessionWEB.__UsuarioWEB.Usuario.usu_login);
                        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario.usu_login", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                    }
                    else
                    {
                        Trace.Write("__SessionWEB.__UsuarioWEB.Usuario", "NULL");
                    }
                }
                // ***** FIM *****

                #endregion Trace

                // Verifica se usuário está autenticado
                if (UserIsAuthenticated())
                {
                    // Carrega grupos do usuário
                    IList<SYS_Grupo> list = SYS_GrupoBO.GetSelectBySis_idAndUsu_id(
                       __SessionWEB.__UsuarioWEB.Usuario.usu_id
                       , ApplicationWEB.SistemaID);

                    // Verifica se foi carregado os grupos do usuário
                    if (list.Count > 0)
                    {
                        // Verifica se usuário logado possui um único grupo para carregar na Session,
                        // caso possua vários grupos será necessário selecionar apenas um grupo
                        if (list.Count == 1)
                        {
                            __SessionWEB.__UsuarioWEB.Grupo = list[0];

                            //// Realiza autenticação do usuário no Sistema Gestão Acadêmica
                            HttpContext.Current.User.Identity.AddGrupoId(Request, __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                            //SYS_UsuarioBO.AutenticarUsuario(__SessionWEB.__UsuarioWEB.Usuario, __SessionWEB.__UsuarioWEB.Grupo);

                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, String.Format("Autenticação do usuário ( {0} ) com grupo ( {1} ) no sistema ( {2} ).", __SessionWEB.__UsuarioWEB.Usuario.usu_login, __SessionWEB.__UsuarioWEB.Grupo.gru_nome, __SessionWEB.TituloSistema));

                            RedirecionaPaginaInicial();
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
                //SYS_UsuarioBO.AutenticarUsuario(__SessionWEB.__UsuarioWEB.Usuario, __SessionWEB.__UsuarioWEB.Grupo);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, String.Format("Autenticação do usuário ( {0} ) com grupo ( {1} ) no sistema ( {2} ).", __SessionWEB.__UsuarioWEB.Usuario.usu_login, __SessionWEB.__UsuarioWEB.Grupo.gru_nome, __SessionWEB.TituloSistema));

                RedirecionaPaginaInicial();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atender a solicitação.<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Erro);
            btnVoltar.Visible = true;
        }
    }

    /// <summary>
    /// Verifica qual a página inicial que deve ser redirecionada.
    /// </summary>
    private void RedirecionaPaginaInicial()
    {
        long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
        int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
        SYS_Modulo modulo;

        if (visao == SysVisaoID.Individual && doc_id > 0 && !string.IsNullOrEmpty(ApplicationWEB.HomeDocente))
        {
            // Se tem tela inicial configurada para o docente, redirecionar.
            Response.Redirect(ApplicationWEB.HomeDocente, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else if ((visao == SysVisaoID.Gestao || visao == SysVisaoID.UnidadeAdministrativa) &&
            !string.IsNullOrEmpty(ApplicationWEB.HomeGestor) &&
            GestaoEscolarUtilBO.GetGrupoPermissao_Grupo_By_Url(ApplicationWEB.SistemaID
                                                               , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                                                               , VirtualPathUtility.ToAbsolute(ApplicationWEB.HomeGestor)
                                                               , out modulo
                                                               , ApplicationWEB.AppMinutosCacheLongoGeral).grp_consultar && modulo.mod_id > 0)
        {
            // Se tem tela inicial configurada para o gestor, redirecionar.
            Response.Redirect(ApplicationWEB.HomeGestor, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            Response.Redirect("~/Index.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB.UrlCoreSSO + "/Sistema.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion Eventos
}