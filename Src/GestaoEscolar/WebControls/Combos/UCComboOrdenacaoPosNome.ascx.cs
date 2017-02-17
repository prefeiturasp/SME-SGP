using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboOrdenacaoPosNome : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChange();

        public event SelectedIndexChange _OnSelectedIndexChange;

        #endregion

        #region Propriedades

        public DropDownList _Combo
        {
            get
            {
                return _ddlOrdenacao;
            }
            set
            {
                _ddlOrdenacao = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void _ddlOrdenacao_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this._OnSelectedIndexChange != null)
                this._OnSelectedIndexChange();
        }

        #endregion
    }
}