using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;

public partial class Configuracao_TipoEquipamentoDeficiente_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna o id do tipo de equipamento para deficiente na edição
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvTipoEquipamentoDeficiente.DataKeys[grvTipoEquipamentoDeficiente.EditIndex].Value);
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

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                lblMensagem.Text = message;
            }

            UCComboQtdePaginacao1.GridViewRelacionado = grvTipoEquipamentoDeficiente;

            // Atualiza o grid
            grvTipoEquipamentoDeficiente.PageSize = UCComboQtdePaginacao1.Valor;
            grvTipoEquipamentoDeficiente.DataBind();

            grvTipoEquipamentoDeficiente.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnTipoEquipamentoDeficiente.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        grvTipoEquipamentoDeficiente.PageSize = UCComboQtdePaginacao1.Valor;
        grvTipoEquipamentoDeficiente.PageIndex = 0;
        // atualiza o grid
        grvTipoEquipamentoDeficiente.DataBind();
    }

    #endregion

    #region Evento

    protected void grvTipoEquipamentoDeficiente_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TipoEquipamentoDeficienteBO.GetTotalRecords();
    }

    protected void grvTipoEquipamentoDeficiente_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int ted_id = Convert.ToInt32(grvTipoEquipamentoDeficiente.DataKeys[index].Value.ToString());

                ACA_TipoEquipamentoDeficiente entity = new ACA_TipoEquipamentoDeficiente { ted_id = ted_id };
                ACA_TipoEquipamentoDeficienteBO.GetEntity(entity);

                if (ACA_TipoEquipamentoDeficienteBO.Delete(entity))
                {
                    grvTipoEquipamentoDeficiente.PageIndex = 0;
                    grvTipoEquipamentoDeficiente.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "ted_id: " + ted_id);
                    lblMensagem.Text = UtilBO.GetErroMessage(
                        GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Busca.SucessoAoExcluir").ToString()
                        , UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(
                    GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Busca.ErroAoTentarExluir").ToString()
                    , UtilBO.TipoMensagem.Erro);
            }
        }
    }


    protected void grvTipoEquipamentoDeficiente_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnTipoEquipamentoDeficiente_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracao/TipoEquipamentoDeficiente/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}