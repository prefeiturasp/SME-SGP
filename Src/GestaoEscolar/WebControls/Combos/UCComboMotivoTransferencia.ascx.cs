using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboMotivoTransferencia : MotherUserControl
    {
        #region DELEGATES

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Retorna e seta o valor selecionado no combo
        /// </summary>
        public int Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                    return -1;

                return Convert.ToInt32(ddlCombo.SelectedValue);
            }
            set
            {
                ddlCombo.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return ddlCombo.SelectedItem.ToString();
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
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTitulo);

                }

                cpvCombo.Visible = value;
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
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                ddlCombo.Enabled = value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Outros motivos de transferência..." do dropdownlist.  
        /// Por padrão é false e a mensagem "Outros motivos de transferência..." não é exibida.
        /// </summary>
        public bool MostrarMessageOutros
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(ddlCombo.Items.Count, new ListItem("Outros motivos de transferência...", "0", true));
            }
        }

        /// <summary>
        /// Evento Click javascript do combo.
        /// </summary>
        public string Combo_OnChange
        {
            set
            {
                ddlCombo.Attributes["onchange"] = value;
            }
        }

        /// <summary>
        /// Propriedade ClientID do combo.
        /// </summary>
        public string Combo_ClientID
        {
            get
            {
                return ddlCombo.ClientID;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Seta o foco no combo    
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Mostra os motivos de transferências não excluídos logicamente no dropdownlist    
        /// </summary>
        public void CarregarMotivosTransferencia()
        {
            ddlCombo.Items.Clear();

            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um motivo de transferência --", "-1", true));
            ddlCombo.DataSource = MTR_MotivoTransferenciaBO.SelecionaMotivoTransferencia(); 
            ddlCombo.DataBind();
        }

        #endregion

        #region EVENTOS

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = IndexChanged != null;
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion
    }
}