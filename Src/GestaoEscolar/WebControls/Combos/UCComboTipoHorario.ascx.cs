namespace GestaoEscolar.WebControls.Combos
{
    using System;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;    

    public partial class UCComboTipoHorario : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// ClientID do combo
        /// </summary>
        public string ComboClientID
        {
            get
            {
                return ddlTipoHorario.ClientID;
            }
        }

        /// <summary>
        /// Propriedade que seta a obrigatoriedade do combo
        /// </summary>
        public bool Obrigatorio
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblTipoHorario);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTipoHorario);
                }

                cvTipoHorario.Visible = value;
            }
        }

        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cvTipoHorario.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Propriedade que seta se o combo é editável
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                ddlTipoHorario.Enabled = value;
            }
        }

        /// <summary>
        /// Retorna e seta o valor selecionado no combo
        /// </summary>
        public byte Valor
        {
            get
            {
                return Convert.ToByte(string.IsNullOrEmpty(ddlTipoHorario.SelectedValue) ? "0" : ddlTipoHorario.SelectedValue);
            }

            set
            {
                ddlTipoHorario.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Propriedade que indica se o combo atualizará a tela.
        /// </summary>
        public bool AutoPostBack
        {
            get
            {
                return ddlTipoHorario.AutoPostBack;
            }

            set
            {
                ddlTipoHorario.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Propriedade que indica se o combo estara ativo na tela.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return ddlTipoHorario.Enabled;
            }

            set
            {
                ddlTipoHorario.Enabled = value;
            }
        }

        /// <summary>
        /// Propriedade que indica se o combo estara ativo na tela.
        /// </summary>
        public string CssClass
        {
            get
            {
                return ddlTipoHorario.CssClass;
            }

            set
            {
                ddlTipoHorario.CssClass = "text30C " + (string.IsNullOrEmpty(value) ? "ddlTipoHorario" : value);
            }
        }

        #endregion Propriedades

        #region Métodos

        public void Carregar()
        {
            GestaoEscolarUtilBO.CarregarComboEnum<ACA_TurnoHorarioTipo>(ddlTipoHorario.Items, true);
        }

        /// <summary>
        /// Seta o foco no combo    
        /// </summary>
        public void SetarFoco()
        {
            ddlTipoHorario.Focus();
        }

        #endregion Métodos

        #region Eventos

        protected void ddlTipoHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
            {
                IndexChanged();
            }
        }

        #endregion Eventos
    }
}