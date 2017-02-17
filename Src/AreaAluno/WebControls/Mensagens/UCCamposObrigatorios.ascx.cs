using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AreaAluno.WebControls.Mensagens
{
    public partial class UCCamposObrigatorios : System.Web.UI.UserControl
    {
        public bool HabilitaLabel
        {
            get
            {
                return lblCampoObrigatorio.Enabled;
            }
        }
    }
}