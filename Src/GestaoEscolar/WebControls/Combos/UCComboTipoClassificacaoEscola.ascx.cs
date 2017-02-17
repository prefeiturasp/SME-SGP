using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoClassificacaoEscola : MotherUserControl
    {

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// </summary>
        public Int32 Valor
        {
            get
            {
                return Convert.ToInt32(ddlTipoClassificacao.SelectedValue);
            }
            set
            {
                ddlTipoClassificacao.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Retorna visibilidade do usercontrol.
        /// </summary>
        public bool uccTipoClassificacaoVisible
        {
            get
            {
                return divUCCTipoClassificacao.Visible;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega tipos de classificação de escola.
        /// </summary>
        public void Carregar()
        {
            try
            {
                ddlTipoClassificacao.DataSource = ESC_TipoClassificacaoEscolaBO.SelecionaTipoClassificacaoEscola();
                ddlTipoClassificacao.DataBind();
                if (ddlTipoClassificacao.Items.Count > 0)
                {
                    ddlTipoClassificacao.Items.Insert(0, new ListItem("-- Selecione um tipo de classificação --", "-1", true));
                    divUCCTipoClassificacao.Visible = true;
                }
                else divUCCTipoClassificacao.Visible = false;
            }
            catch (Exception)
            {
                lblMessage.Text = "Erro ao tentar carregar tipo de classificação.";
                lblMessage.Visible = true;
            }
        }

        #endregion
    }
}