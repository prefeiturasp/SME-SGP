using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCComboGenerico : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades obrigatórias para utilizar o combo

        /// <summary>
        /// Valor que é configurado para o primeiro item do combo (item "Selecione").
        /// </summary>
        public string ValorItemVazio
        {
            get;
            set;
        }

        /// <summary>
        /// Seta um titulo diferente do padrão para o combo
        /// </summary>
        public string TituloCombo
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
            get
            {
                return lblTitulo.Text;
            }
        }

        /// <summary>
        /// Seta se será inserido um item acima dos dados do combo.
        /// </summary>
        public bool MostrarMensagemSelecione { get; set; }

        /// <summary>
        /// Seta a mensagem de selecione.
        /// </summary>
        public string MensagemSelecione { get; set; }

        #endregion

        #region Proriedades

        /// <summary>
        /// ClientID do combo
        /// </summary>
        public string ClientID_Combo
        {
            get
            {
                return ddlCombo.ClientID;
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
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                else
                    RemoveAsteriscoObrigatorio(lblTitulo);

                cpvCombo.Visible = value;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            get
            {
                return ddlCombo.Enabled;
            }
            set
            {
                ddlCombo.Enabled = value;
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
        /// Propriedade que seta o SelectedIndex do Combo.       
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
            }
        }
        
        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string TextoSelecionado
        {
            get
            {
                return ddlCombo.SelectedItem.ToString();
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
        /// Retorna e seta o valor selecionado no combo
        /// </summary>
        public string Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                    return ValorItemVazio;
                
                return ddlCombo.SelectedValue;
            }
            set
            {
                if (ddlCombo.Items.FindByValue(value) != null)
                {
                    ddlCombo.SelectedValue = value;
                }
            }
        }

        /// <summary>
        /// Propriedade visible da label do nome do combo
        /// </summary>
        public bool Visible_Label
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo.   
        /// </summary>
        public Int32 Width_Combo
        {
            set
            {
                ddlCombo.Width = value;
            }
        }

        /// <summary>
        /// Se propriedade for verdadeira, adiciona classe de mensagem de saída da página ao combo 
        /// </summary>
        public bool ConfirmExit { get; set; }

        /// <summary>
        /// Propriedade que seta o skin do combo.   
        /// </summary>
        public string SkinID_Combo
        {
            set
            {
                ddlCombo.SkinID = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Traz o primeiro item selecinado caso seja o único
        /// </summary>
        private void SelecionaPrimeiroItem()
        {
            if ((QuantidadeItensCombo == 2 && MostrarMensagemSelecione) || 
                (QuantidadeItensCombo == 1 && !MostrarMensagemSelecione))
            {
                int indicePrimeiroItem = QuantidadeItensCombo - 1;

                // Seleciona o primeiro item.
                ddlCombo.SelectedValue = ddlCombo.Items[indicePrimeiroItem].Value;

                if (IndexChanged != null)
                    IndexChanged();
            }
        }

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(ValorItemVazio) == null))
                ddlCombo.Items.Insert(0, new ListItem(string.IsNullOrEmpty(MensagemSelecione) ?
                    string.Format("-- Selecione um(a) {0} --", RetornaTituloCombo()) : MensagemSelecione
                    , ValorItemVazio, true));

            ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
        }

        /// <summary>
        /// Retorna o título do combo sem o "*" de obrigatório.
        /// </summary>
        /// <returns></returns>
        protected string RetornaTituloCombo()
        {
            return lblTitulo.Text.Replace('*', ' ').ToLower();
        }

        /// <summary>
        /// Carrega o combo com o datasource informado.
        /// </summary>
        /// <param name="dataSource">Dados a serem inseridos no combo</param>
        /// <param name="ddlTextField">DataTextField do combo</param>
        /// <param name="ddlValueField">DataValueField do combo</param>
        public void CarregarCombo(object dataSource, string ddlTextField, string ddlValueField)
        {
            try
            {
                ddlCombo.DataTextField = ddlTextField;
                ddlCombo.DataValueField = ddlValueField;

                ddlCombo.Items.Clear();
                ddlCombo.DataSource = dataSource;

                CarregarMensagemSelecione();
                ddlCombo.DataBind();
                SelecionaPrimeiroItem();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = "Erro ao tentar carregar " + RetornaTituloCombo() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// Seta o foco no combo    
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        #endregion

        #region Eventos

        #region Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                               lblTitulo.Text.EndsWith(" *");

            cpvCombo.ValueToCompare = ValorItemVazio;

            Obrigatorio = obrigatorio;

            // Adiciona classe de mensagem de saída da página ao combo
            if (ConfirmExit && !Convert.ToString(ddlCombo.CssClass).Contains("btnMensagemUnload"))
            {
                ddlCombo.CssClass += " btnMensagemUnload";
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (IndexChanged != null);
            CarregarMensagemSelecione();
        }

        #endregion

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion
    }
}