using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;

public partial class Configuracao_TipoMacroCampoDisciplina_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvTipoMacroCampoDisciplina.DataKeys[grvTipoMacroCampoDisciplina.EditIndex].Value);
        }
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = grvTipoMacroCampoDisciplina;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                lblMessage.Text = message;

            // Atualiza o grid
            grvTipoMacroCampoDisciplina.PageSize = UCComboQtdePaginacao1.Valor;
            grvTipoMacroCampoDisciplina.DataBind();

            grvTipoMacroCampoDisciplina.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        grvTipoMacroCampoDisciplina.PageSize = UCComboQtdePaginacao1.Valor;
        grvTipoMacroCampoDisciplina.PageIndex = 0;
        // atualiza o grid
        grvTipoMacroCampoDisciplina.DataBind();
    }

    #endregion

    #region Eventos

    protected void grvTipoMacroCampoDisciplina_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TipoMacroCampoEletivaAlunoBO.GetTotalRecords();
    }

    protected void grvTipoMacroCampoDisciplina_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int tea_id = Convert.ToInt32(grvTipoMacroCampoDisciplina.DataKeys[index].Value.ToString());

                ACA_TipoMacroCampoEletivaAluno entity = new ACA_TipoMacroCampoEletivaAluno { tea_id = tea_id };
                ACA_TipoMacroCampoEletivaAlunoBO.GetEntity(entity);

                if (ACA_TipoMacroCampoEletivaAlunoBO.Delete(entity))
                {
                    grvTipoMacroCampoDisciplina.PageIndex = 0;
                    grvTipoMacroCampoDisciplina.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tea_id: " + tea_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " eletivo(a) excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " eletivo(a).", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void grvTipoMacroCampoDisciplina_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
            if (btnAlterar != null)
            {
                btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
            if (btnExcluir != null)
            {
                btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void btnNovoTipoMacroCampo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoMacroCampoDisciplina/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}

