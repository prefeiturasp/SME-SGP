using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Mensagens_UCTotalRegistros : MotherUserControl
{
    #region Propriedades

    private string gridViewID;

    /// <summary>
    /// ID do GridView associado aos registros.
    /// </summary>
    public string AssociatedGridViewID
    {
        get
        {
            return gridViewID;
        }
        set
        {
            gridViewID = value;
        }
    }

    /// <summary>
    /// Total de registros.
    /// </summary>
    public int Total
    {
        get
        {
            return VS_TotalRegistros;
        }
        set
        {
            try
            {
                if (value > 0)
                {
                    GridView grv = (GridView)this.Parent.FindControl(gridViewID);
                    if (grv != null)
                    {
                        bool PageLast = (((grv.PageIndex + 1) == grv.PageCount));

                        int NumberStart = (grv.PageIndex * grv.PageSize) + 1;
                        int NumberEnd = (PageLast ? value : ((grv.PageIndex * grv.PageSize) + grv.PageSize));

                        lblTotalRegistros.Visible = true;
                        lblTotalRegistros.Text = String.Format("Mostrando {0} - {1} registro(s) do total de {2}", NumberStart, NumberEnd, value);
                        lblTotalRegistros.Attributes.Add("valor", value.ToString());
                        VS_TotalRegistros = value;
                    }
                }
                else
                {
                    lblTotalRegistros.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }
    }

    /// <summary>
    /// Armazena o total de registro calculado.
    /// </summary>
    private int VS_TotalRegistros
    {
        get
        {
            if (ViewState["TotalRegistros"] == null)
                return 0;
            return (int)ViewState["TotalRegistros"];
        }
        set
        {
            ViewState["TotalRegistros"] = value;
        }
    }

    #endregion

}
