using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Mensagens_UCCamposObrigatorios : MotherUserControl
{
    public bool HabilitaLabel
    {
        get
        {
            return lblCampoObrigatorio.Enabled;
        }
    }
}
