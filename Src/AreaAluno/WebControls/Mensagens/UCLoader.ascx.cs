using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AreaAluno.WebControls.Mensagens
{
    public partial class UCLoader : MotherUserControl
    {
        #region Propriedades

        /// <summary>
        /// ID do updatePanel associado ao Progress.
        /// </summary>
        public string AssociatedUpdatePanelID
        {
            set
            {
                upgProgress.AssociatedUpdatePanelID = value;
            }
        }

        #endregion
    }
}