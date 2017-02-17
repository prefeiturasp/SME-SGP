using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoEvento_Busca : MotherPageLogado
{
    #region Constantes

    private const int colunaPermissoes = 4;

    #endregion 

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTipoEvento.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTipoEvento.PageIndex = 0;
        // atualiza o grid
        _dgvTipoEvento.DataBind();
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna o id tipo de evento para editar
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvTipoEvento.DataKeys[_dgvTipoEvento.EditIndex].Value);
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

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTipoEvento;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                _lblMessage.Text = message;
            }

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTipoEvento.PageSize = itensPagina;
            // atualiza o grid
            _dgvTipoEvento.DataBind();

            Page.Form.DefaultButton = _btnNovo.UniqueID;

            _dgvTipoEvento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }

        _dgvTipoEvento.Columns[colunaPermissoes].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_PERMISSAO_TIPO_EVENTO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
    }

    protected void _odsTipoEvento_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _dgvTipoEvento_RowDataBound(object sender, GridViewRowEventArgs e)
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

            ImageButton btnPermissoes = (ImageButton)e.Row.FindControl("btnPermissoes");
            if (_btnExcluir != null)
            {
                btnPermissoes.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
        }
    }

    protected void _dgvTipoEvento_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int tev_id = Convert.ToInt32(_dgvTipoEvento.DataKeys[index].Value.ToString());

                ACA_TipoEvento entity = new ACA_TipoEvento { tev_id = tev_id };
                ACA_TipoEventoBO.GetEntity(entity);

                if (ACA_TipoEventoBO.Delete(entity))
                {
                    _dgvTipoEvento.PageIndex = 0;
                    _dgvTipoEvento.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tev_id: " + tev_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Tipo de evento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir o tipo de evento.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoEvento/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
