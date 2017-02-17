using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboMeioTransporte : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion Delegates

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // TODO
                }
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #region Propriedades
        /// <summary>
        /// Retorna o valor que foi selecionado no combo
        /// </summary>
        public int Value
        {
            get
            {
                return Convert.ToByte(ddlMeioTransporte.SelectedValue == "-1" ? "0" : ddlMeioTransporte.SelectedValue);
            }
            set
            {
                ddlMeioTransporte.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Seta um titulo diferente do padrão para o combo
        /// </summary>
        public string Titulo
        {
            set
            {
                lblMeioTransporte.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
            get
            {
                return lblMeioTransporte.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
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
                    AdicionaAsteriscoObrigatorio(lblMeioTransporte);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblMeioTransporte);
                }

                cpvCombo.Visible = value;
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return ddlMeioTransporte.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvCombo.ValidationGroup = value;
            }
        }

        /// <summary>
        /// ClientID do combo
        /// </summary>
        public string Combo_ClientID
        {
            get
            {
                return ddlMeioTransporte.ClientID;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            get
            {
                return ddlMeioTransporte.Enabled;
            }
            set
            {
                ddlMeioTransporte.Enabled = value;
            }
        }

        /// <summary>
        /// Propriedade que verifica quantos items existem no combo
        /// </summary>
        public int QuantidadeItensCombo
        {
            get
            {
                return ddlMeioTransporte.Items.Count;
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedIndex do Combo.
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlMeioTransporte.SelectedValue = ddlMeioTransporte.Items[value].Value;
            }
            get
            {
                return ddlMeioTransporte.SelectedIndex;
            }
        }
        #endregion Propriedades

        #region Método
        /// <summary>
        /// Seta o foco no combo
        /// </summary>
        public void SetarFoco()
        {
            ddlMeioTransporte.Focus();
        }
        #endregion Método

        #region Eventos
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlMeioTransporte.AutoPostBack = IndexChanged != null;
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }
        #endregion Eventos

    }
}