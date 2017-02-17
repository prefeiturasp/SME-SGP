using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoDocente_Busca : MotherPageLogado
{

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = gdvTipoDocente;

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
            gdvTipoDocente.PageSize = itensPagina;

            // atualiza o grid
            gdvTipoDocente.DataBind();

            gdvTipoDocente.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        gdvTipoDocente.PageSize = UCComboQtdePaginacao1.Valor;
        gdvTipoDocente.PageIndex = 0;
        // atualiza o grid
        gdvTipoDocente.DataBind();
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna o id do tipo de docente para editar
    /// </summary>
    public byte EditItem
    {
        get
        {
            return Convert.ToByte(gdvTipoDocente.DataKeys[gdvTipoDocente.EditIndex].Value);
        }
    }

    #endregion

    #region Eventos

    protected void odsTipoDocente_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void gdvTipoDocente_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                byte tdc_id = Convert.ToByte(gdvTipoDocente.DataKeys[index].Value.ToString());

                ACA_TipoDocente entity = new ACA_TipoDocente { tdc_id = tdc_id };
                ACA_TipoDocenteBO.GetEntity(entity);

                if (ACA_TipoDocenteBO.Delete(entity))
                {
                    gdvTipoDocente.PageIndex = 0;
                    gdvTipoDocente.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tdc_id: " + tdc_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Tipo de docente excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo de docente.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void gdvTipoDocente_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDocente/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

}
