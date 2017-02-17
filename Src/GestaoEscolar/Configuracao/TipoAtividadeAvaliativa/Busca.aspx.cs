using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Web.UI.HtmlControls;

public partial class Configuracao_TipoAtividadeAvaliativa_Busca : MotherPageLogado
{
    #region Eventos Page Life Cycle
    
    protected void Page_Init(object sender, EventArgs e)
    {
        HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
        if (cell != null)
            cell.BgColor = ApplicationWEB.AlunoInativo;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = grvTipoAtividadeAvaliativa;

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
            grvTipoAtividadeAvaliativa.PageSize = itensPagina;
            // atualiza o grid
            grvTipoAtividadeAvaliativa.DataBind();

            Page.Form.DefaultButton = btnNovo.UniqueID;

            grvTipoAtividadeAvaliativa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            divLegenda.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar && (grvTipoAtividadeAvaliativa.Rows.Count > 0);
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        grvTipoAtividadeAvaliativa.PageSize = UCComboQtdePaginacao1.Valor;
        grvTipoAtividadeAvaliativa.PageIndex = 0;
        // atualiza o grid
        grvTipoAtividadeAvaliativa.DataBind();
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna o id tipo de atividade avaliativa para editar
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvTipoAtividadeAvaliativa.DataKeys[grvTipoAtividadeAvaliativa.EditIndex].Value);
        }
    }

    #endregion

    #region Eventos

    protected void odsTipoAtividadeAvaliativa_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void grvTipoAtividadeAvaliativa_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "InativarAtividade")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int tav_id = Convert.ToInt32(grvTipoAtividadeAvaliativa.DataKeys[index].Value.ToString());

            CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa { tav_id = tav_id };
            CLS_TipoAtividadeAvaliativaBO.GetEntity(entity);

            entity.tav_situacao = (byte)CLS_TipoAtividadeAvaliativaSituacao.Inativo;

            if (CLS_TipoAtividadeAvaliativaBO.Save(entity))
            {
                grvTipoAtividadeAvaliativa.PageIndex = 0;
                grvTipoAtividadeAvaliativa.DataBind();
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tav_id: " + tav_id);
                lblMessage.Text = UtilBO.GetErroMessage("Tipo de atividade avaliativa inativado com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }
        }

        if (e.CommandName == "AtivarAtividade")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int tav_id = Convert.ToInt32(grvTipoAtividadeAvaliativa.DataKeys[index].Value.ToString());

            CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa { tav_id = tav_id };
            CLS_TipoAtividadeAvaliativaBO.GetEntity(entity);

            entity.tav_situacao = (byte)CLS_TipoAtividadeAvaliativaSituacao.Ativo;

            if (CLS_TipoAtividadeAvaliativaBO.Save(entity))
            {
                grvTipoAtividadeAvaliativa.PageIndex = 0;
                grvTipoAtividadeAvaliativa.DataBind();
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tav_id: " + tav_id);
                lblMessage.Text = UtilBO.GetErroMessage("Tipo de atividade avaliativa ativado com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }
        }

        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int tav_id = Convert.ToInt32(grvTipoAtividadeAvaliativa.DataKeys[index].Value.ToString());

                CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa { tav_id = tav_id };
                CLS_TipoAtividadeAvaliativaBO.GetEntity(entity);

                if (CLS_TipoAtividadeAvaliativaBO.Delete(entity))
                {
                    grvTipoAtividadeAvaliativa.PageIndex = 0;
                    grvTipoAtividadeAvaliativa.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tav_id: " + tav_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Tipo de atividade avaliativa excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo de atividade avaliativa.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void grvTipoAtividadeAvaliativa_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            byte situacao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tav_situacao"));

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

            ImageButton btnInativarAtividade = (ImageButton)e.Row.FindControl("btnInativarAtividade");
            if (btnInativarAtividade != null)
            {
                btnInativarAtividade.CommandArgument = e.Row.RowIndex.ToString();

                string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnInativarAtividade.ClientID), string.Format("Confirmar a operação?"));

                Page.ClientScript.RegisterStartupScript(GetType(), btnInativarAtividade.ClientID, script, true);
            }

            ImageButton btnAtivarAtividade = (ImageButton)e.Row.FindControl("btnAtivarAtividade");
            if (btnInativarAtividade != null)
            {
                btnAtivarAtividade.CommandArgument = e.Row.RowIndex.ToString();

                string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnAtivarAtividade.ClientID), string.Format("Confirmar a operação?"));

                Page.ClientScript.RegisterStartupScript(GetType(), btnAtivarAtividade.ClientID, script, true);
            }

            ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
            if (btnExcluir != null)
            {
                btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }

            if ((CLS_TipoAtividadeAvaliativaSituacao)situacao == CLS_TipoAtividadeAvaliativaSituacao.Inativo)
            {
                e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;
                btnInativarAtividade.Visible = false;
                btnAtivarAtividade.Visible = true;
            }
            else
            {
                btnInativarAtividade.Visible = true;
                btnAtivarAtividade.Visible = false;
            }
        }
    }

    protected void btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracao/TipoAtividadeAvaliativa/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
