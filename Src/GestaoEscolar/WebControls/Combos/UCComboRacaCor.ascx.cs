using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboRacaCor : MotherUserControl
{
    #region PROPRIEDADADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlRacaCor;
        }
        set
        {
            _ddlRacaCor = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelRacaCor;
        }
        set
        {
            LabelRacaCor = value;
        }
    }

    #endregion
}
