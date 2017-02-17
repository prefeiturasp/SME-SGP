using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboQtdePaginacao : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedade

    /// <summary>
    /// GridView Relacionado ao user control.
    /// </summary>
    public GridView GridViewRelacionado
    {
        set
        {
            try
            {
                value.DataBound += Grid_DataBound;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }
    }

    private void Grid_DataBound(object sender, EventArgs e)
    {
        try
        {
            GridView gv = (GridView)sender;
            this.Visible = gv.Rows.Count > 0;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Configura valor para paginação.
    /// </summary>
    public int Valor
    {
        get
        {
            return Convert.ToInt32(ddlQtPaginado.SelectedValue);
        }
        set
        {
            ddlQtPaginado.SelectedValue = value.ToString();
        }
    }

    /// <summary>
    /// Configura valor inicial para paginação
    /// apartir de parâmetros e configurações.
    /// </summary>
    public bool ComboDefaultValue
    {
        set
        {
            if (value)
            {
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);
                ddlQtPaginado.SelectedValue = Convert.ToString(itensPagina);
            }
        }
    }

    #endregion

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlQtPaginado.AutoPostBack = IndexChanged != null;
    }

    protected void ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion
}

