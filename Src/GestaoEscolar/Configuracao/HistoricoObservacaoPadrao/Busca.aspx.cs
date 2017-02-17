using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Web;

public partial class Configuracao_HistoricoObservacaoPadrao_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvHistoricoEscolar.DataKeys[_dgvHistoricoEscolar.EditIndex].Value);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvHistoricoEscolar;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _lblMessage.Visible = !(string.IsNullOrEmpty(message));

            // atribui nova quantidade de itens por página para o grid
            _dgvHistoricoEscolar.PageSize = UCComboQtdePaginacao1.Valor;
            _dgvHistoricoEscolar.PageIndex = 0;

            Page.Form.DefaultButton = _btnNovo.UniqueID;

            _dgvHistoricoEscolar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    protected void _odsHistoricoEscolar_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _dgvHistoricoEscolar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int hop_id = Convert.ToInt32(_dgvHistoricoEscolar.DataKeys[index].Value.ToString());

                ACA_HistoricoObservacaoPadrao entity = new ACA_HistoricoObservacaoPadrao { hop_id = hop_id };
                ACA_HistoricoObservacaoPadraoBO.GetEntity(entity);

                if (ACA_HistoricoObservacaoPadraoBO.Delete(entity))
                {
                    _dgvHistoricoEscolar.PageIndex = 0;
                    _dgvHistoricoEscolar.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "hop_id: " + hop_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Observação do histórico escolar excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a observação do histórico escolar.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _dgvHistoricoEscolar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracao/HistoricoObservacaoPadrao/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade de itens por página para o grid
        _dgvHistoricoEscolar.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvHistoricoEscolar.PageIndex = 0;
        // atualiza o grid
        _dgvHistoricoEscolar.DataBind();
    }

    #endregion
}
