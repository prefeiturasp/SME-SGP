using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.UI.HtmlControls;

namespace GestaoEscolar.WebControls.Etiqueta
{
    public partial class UCPosicaoEtiqueta : MotherUserControl
    {

        #region Delegates

        /// <summary>
        /// Guarda o returns
        /// </summary>
        private string returns
        {
            get
            {

                if ((string)ViewState["returns"] == null)
                    return "0";
                else
                    return (string)ViewState["returns"];
            }
            set
            {
                ViewState["returns"] = value;
            }
        }

        public delegate void OnReturnValues(String posicaoEtiqueta);
        public event OnReturnValues ReturnValues;

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Button btnSelecionadoOld = (Button)FindControl("btnEtiqueta" + returns);
                if (btnSelecionadoOld != null)
                    btnSelecionadoOld.BackColor = System.Drawing.Color.Green;
            }
                      
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Handles the Click event of the btnEtiqueta control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnEtiqueta_Click(object sender, EventArgs e)
        {
            try
            {

                Button btnSelecionadoOld = (Button)FindControl("btnEtiqueta" + returns);
                if ((btnSelecionadoOld != null) && (btnSelecionadoOld.BackColor == System.Drawing.Color.Green))
                    btnSelecionadoOld.BackColor = btnSelecionarPosicao.BackColor;

                Button btnSelecionado = (Button)sender;

                if (returns == btnSelecionado.TabIndex.ToString())
                {
                    btnSelecionado.BackColor = btnSelecionarPosicao.BackColor;
                    returns = "0";
                }
                else
                {
                    btnSelecionado.BackColor = System.Drawing.Color.Green;
                    returns = btnSelecionado.TabIndex.ToString();
                }



            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void btnSelecionarPosicao_Click(object sender, EventArgs e)
        {
            ReturnValues(returns);
        }

        #endregion

    }
}
