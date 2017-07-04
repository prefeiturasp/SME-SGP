using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboQuestionario : MotherUserControl
    {
        #region Delegates

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
        /// Propriedade que seta o SelectedIndex do Combo.       
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlCombo.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Propriedade que verifica quantos items existem no combo
        /// </summary>
        public int QuantidadeItensCombo
        {
            get
            {
                return ddlCombo.Items.Count;
            }
        }

        /// <summary>
        /// Atribui valores para o combo
        /// </summary>
        public DropDownList Combo
        {
            get
            {
                return ddlCombo;
            }
            set
            {
                ddlCombo = value;
            }
        }

        /// <summary>
        /// ClientID do combo
        /// </summary>
        public string Combo_ClientID
        {
            get
            {
                return ddlCombo.ClientID;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo.   
        /// </summary>
        public Int32 WidthCombo
        {
            set
            {
                ddlCombo.Width = value;
            }
        }

        /// <summary>
        /// Propriedade visible da label do nome do combo
        /// </summary>
        public bool LabelVisible
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
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
        /// Seta um titulo diferente do padrão para o combo
        /// </summary>
        public string Titulo
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um questionário" do dropdownlist.  
        /// Por padrão é false e a mensagem "Selecione um questionário" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if ((value) && (ddlCombo.Items.FindByValue("-1") == null))
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de questionário --", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
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
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// </summary>
        public void CarregarQuestionario()
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = CLS_QuestionarioBO.GetQuestionarioBy_qst_titulo("");
            MostrarMessageSelecione = true;       
            ddlCombo.DataBind();
        }
        
        #endregion
    }
}