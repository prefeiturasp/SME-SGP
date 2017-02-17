using MSTech.GestaoEscolar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoCiclo : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);
        public event SelectedIndexChange_Sender IndexChanged_Sender;

        #endregion

        #region Propriedades

        /// <summary>
        /// Configura validação do combo
        /// </summary>
        public bool Obrigatorio
        {
            get
            {
                if (ViewState["Obrigatorio"] != null)
                    return Convert.ToBoolean(ViewState["Obrigatorio"]);
                return false;
            }
            set
            {
                ViewState["Obrigatorio"] = value;
            }

        }

        /// <summary>
        /// Configura título do combo de tipo de ciclo
        /// </summary>
        public string Titulo
        {
            get
            {
                return lblTipoCiclo.Text;
            }
            set
            {
                lblTipoCiclo.Text = value;
                cpvTipoCiclo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Seta a visibilidade de combo
        /// </summary>
        public bool MostraCombo
        {
            get
            {
                return ddlTipoCiclo.Visible;
            }
            set
            {
                ddlTipoCiclo.Visible = value;
            }
        }

        /// <summary>
        /// Seta a visibilidade de label
        /// </summary>
        public bool MostraLabel
        {
            get
            {
                return lblTipoCiclo.Visible;
            }
            set
            {
                lblTipoCiclo.Visible = value;
            }
        }

        /// <summary>
        /// Retorna o Tci_id selecionado no combo.    
        /// </summary>
        public int Tci_id
        {
            get
            {
                if (string.IsNullOrEmpty(ddlTipoCiclo.SelectedValue))
                    return -1;
                return Convert.ToInt32(ddlTipoCiclo.SelectedValue);
            }
            set
            {
                ddlTipoCiclo.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Seta o validation group do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvTipoCiclo.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Retorna o index do combo.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ddlTipoCiclo.SelectedIndex;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado.
        /// </summary>
        public bool Enabled
        {
            set
            {
                ddlTipoCiclo.Enabled = value;
            }
        }

        /// <summary>
        /// Retorna o combo.
        /// </summary>
        public DropDownList ddlCombo
        {
            get
            {
                return ddlTipoCiclo;
            }
        }

        /// <summary>
        /// Propriedade "SelectedValue" do combo
        /// </summary>
        public string SelectedValue
        {
            get { return ddlTipoCiclo.SelectedValue; }
            set { ddlTipoCiclo.SelectedValue = value; }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlTipoCiclo.AutoPostBack = IndexChanged != null;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega todos os tipos de ciclo ativos.
        /// </summary>
        public void Carregar()
        {
            ddlTipoCiclo.Items.Clear();

            ddlTipoCiclo.DataSource = ACA_TipoCicloBO.SelecionaTipoCicloAtivos(ApplicationWEB.AppMinutosCacheLongo);
            ddlTipoCiclo.Items.Insert(0, new ListItem("-- Selecione o(a) " + Regex.Replace(Titulo.ToLower(), "<.*?>", string.Empty) + " --", "-1", true));
            ddlTipoCiclo.DataBind();

            cpvTipoCiclo.Visible = Obrigatorio;
        }

        #endregion

        #region Eventos

        protected void ddlTipoCiclo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();

            if (IndexChanged_Sender != null)
                IndexChanged_Sender(sender, e);
        }

        /// <summary>
        /// Carrega todos os tipos de ciclo ativos vinculados ao curso/curriculo período.
        /// </summary>
        public void CarregarCicloPorCursoCurriculo(int cur_id, int crr_id)
        {
            ddlTipoCiclo.Items.Clear();

            ddlTipoCiclo.DataSource = ACA_TipoCicloBO.SelecionaCicloPorCursoCurriculo(cur_id, crr_id, ApplicationWEB.AppMinutosCacheLongo);
            ddlTipoCiclo.Items.Insert(0, new ListItem("-- Selecione o(a) " + Regex.Replace(Titulo.ToLower(), "<.*?>", string.Empty) + " --", "-1", true));
            ddlTipoCiclo.DataBind();

            cpvTipoCiclo.Visible = Obrigatorio;
        }

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// valor[0] = tci_id
        /// </summary>
        public int Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlTipoCiclo.SelectedValue))
                {
                    return -1;
                }
                return Convert.ToInt32(ddlTipoCiclo.SelectedValue);
            }
            set
            {
                ddlTipoCiclo.SelectedValue = value.ToString(); 
            }
        }


        #endregion
    }
}