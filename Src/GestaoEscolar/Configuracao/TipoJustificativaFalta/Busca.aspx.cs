using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoJustificativaFalta_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvTipoJustificativaFalta.DataKeys[grvTipoJustificativaFalta.EditIndex].Value);
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        grvTipoJustificativaFalta.PageSize = UCComboQtdePaginacao1.Valor;
        grvTipoJustificativaFalta.PageIndex = 0;
        // atualiza o grid
        grvTipoJustificativaFalta.DataBind();
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

        UCComboQtdePaginacao1.GridViewRelacionado = grvTipoJustificativaFalta;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox            
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            grvTipoJustificativaFalta.PageSize = itensPagina;

            Page.Form.DefaultButton = btnNovo.UniqueID;

            grvTipoJustificativaFalta.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Eventos

    protected void odsTipoJustificativaFalta_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["situacao"] = 0;

        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void grvTipoJustificativaFalta_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int tjf_id = Convert.ToInt32(grvTipoJustificativaFalta.DataKeys[index].Value.ToString());

                ACA_TipoJustificativaFalta entity = new ACA_TipoJustificativaFalta { tjf_id = tjf_id };
                ACA_TipoJustificativaFaltaBO.GetEntity(entity);

                if (ACA_TipoJustificativaFaltaBO.Delete(entity))
                {
                    grvTipoJustificativaFalta.PageIndex = 0;
                    grvTipoJustificativaFalta.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tjf_id: " + tjf_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Tipo de justificativa de falta excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o Tipo de justificativa de falta.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void grvTipoJustificativaFalta_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void grvTipoJustificativaFalta_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TipoJustificativaFaltaBO.GetTotalRecords();
    }

    protected void btnNovoTipoJustificativaFalta_Click(object sender, EventArgs e)
    {
        Response.Redirect("Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
