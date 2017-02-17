using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboSexo : MotherUserControl
{
    #region PROPRIEDADADES

    /// <summary>
    /// Atribui estados para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlSexo;
        }
        set
        {
            _ddlSexo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelSexo;
        }
        set
        {
            LabelSexo = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(LabelSexo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(LabelSexo);

            }

            cpvSexo.Visible = value;
        }
    }


    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvSexo.ValidationGroup = value;
        }
    }

    #endregion
}
