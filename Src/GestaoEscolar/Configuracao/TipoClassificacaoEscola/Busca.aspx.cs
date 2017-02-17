using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoClassificacaoEscola_Busca : MotherPageLogado
{
    #region Propriedade

    /// <summary>
    /// Gets EditItem.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvTipoClassificacaoEscola.DataKeys[grvTipoClassificacaoEscola.EditIndex].Value);
        }
    }

    /// <summary>
    /// Armazena o ID do tipo de classificação de escola.
    /// </summary>
    public int VS_tce_id
    {
        get
        {
            if (ViewState["VS_tce_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tce_id"]);
            }

            return -1;
        }
        set
        {
            ViewState["VS_tce_id"] = value;
        }
    }


    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // Atribui nova quantidade itens por página para o grid
        grvTipoClassificacaoEscola.PageSize = UCComboQtdePaginacao1.Valor;
        grvTipoClassificacaoEscola.PageIndex = 0;

        // Atualiza o grid
        grvTipoClassificacaoEscola.DataBind();

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
            if (!string.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }

            UCComboQtdePaginacao1.GridViewRelacionado = grvTipoClassificacaoEscola;

            // Atribui quantidade itens por página para o grid
            grvTipoClassificacaoEscola.PageSize = UCComboQtdePaginacao1.Valor;
        }
    }

    #endregion

    #region Evento

    protected void odsTipoClassificacaoEscola_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
        {
            e.InputParameters.Clear();
        }
    }
    
    protected void grvTipoClassificacaoEscola_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ESC_TipoClassificacaoEscolaBO.GetTotalRecords();
    }
    
    protected void grvTipoClassificacaoEscola_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Cargos")
        {
            try
            {
                VS_tce_id = Convert.ToInt32(grvTipoClassificacaoEscola.DataKeys[index].Values["tce_id"]);

                Response.Redirect("~/Configuracao/TipoClassificacaoEscola/Cargos.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a página.", UtilBO.TipoMensagem.Erro);
                lblMessage.Visible = true;
            }
        }
    }

    #endregion
}
