using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCTipoJustificativa : MotherUserControl
    {
        #region Constantes

        private const string valorSelecione = "-1";

        private const string valorMsgSelecione = "-- Selecione um tipo de justificativa --";

        #endregion

        #region Proriedades

        /// <summary>ClientID do combo</summary>
        public string ClientID_Combo { get { return ddlCombo.ClientID; } }

        /// <summary>ClientID do validator</summary>
        public string ClientID_Validator { get { return cpvCombo.ClientID; } }

        /// <summary>ClientID do label</summary>
        public string ClientID_Label { get { return lblTitulo.ClientID; } }

        /// <summary>Coloca na primeira linha a mensagem de selecione um item.</summary>
        public bool MostrarMensagemSelecione
        {
            get
            {
                if (ViewState["MostrarMensagemSelecione"] != null)
                    return Convert.ToBoolean(ViewState["MostrarMensagemSelecione"]);
                return true;
            }
            set
            {
                ViewState["MostrarMensagemSelecione"] = value;
            }
        }

        /// <summary>Propriedade que seta a label e a validação do combo</summary>
        public bool Obrigatorio
        {
            get
            {
                return cpvCombo.Visible;
            }
            set
            {
                if (value)
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                else
                    RemoveAsteriscoObrigatorio(lblTitulo);

                cpvCombo.Visible = value;
            }
        }

        /// <summary>Deixa o combo habilitado de acordo com o valor passado</summary>
        public bool PermiteEditar
        {
            get { return ddlCombo.Enabled; }
            set { ddlCombo.Enabled = value; }
        }

        /// <summary>Propriedade que verifica quantos items existem no combo</summary>
        public int QuantidadeItensCombo { get { return ddlCombo.Items.Count; } }

        /// <summary>Retorna o texto selecionado no combo</summary>
        public string Texto { get { return ddlCombo.SelectedItem.ToString(); } }

        /// <summary>Seta um titulo diferente do padrão para o combo</summary>
        public string Titulo
        {
            get
            {
                return lblTitulo.Text;
            }
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ').Trim() + " é obrigatório.";
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

        /// <summary>Seta o validationGroup do combo.</summary>
        public string ValidationGroup
        {
            get { return cpvCombo.ValidationGroup; }
            set { cpvCombo.ValidationGroup = value; }
        }

        /// <summary>Retorna e seta o valor selecionado no combo</summary>
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

        #endregion

        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);
        public event SelectedIndexChange_Sender IndexChanged_Sender;

        #endregion

        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            cpvCombo.ValueToCompare = valorSelecione;

            Obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) 
                          || lblTitulo.Text.EndsWith(" *");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (IndexChanged != null) || (IndexChanged_Sender != null);
            CarregarMensagemSelecione();
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();

            if (IndexChanged_Sender != null)
                IndexChanged_Sender(sender, e);
        }

        #endregion

        #region Metodos

        /// <summary>Carrega a mensagem de selecione de acordo com a propriedade <see cref="MostrarMensagemSelecione"/></summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
                ddlCombo.Items.Insert(0, new ListItem(valorMsgSelecione, valorSelecione, true));

            ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
        }

        /// <summary>Carrega o combo</summary>
        /// <param name="dataSource">Dados a serem inseridos no combo.</param>
        public void CarregarCombo(object dataSource)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = dataSource;

                CarregarMensagemSelecione();
                ddlCombo.DataBind();
            }
            catch (Exception)
            {
                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>Carrega todos os tipos de evento não excluídos logicamente</summary>
        public void Carregar()
        {
            CarregarCombo(ACA_TipoJustificativaFaltaBO.TiposJustificativaFalta());
        }

        /// <summary>Carrega os tipos de evento não excluídos logicamente que podem ter liberação de eventos</summary>
        public void CarregarLiberacao()
        {
            CarregarCombo(ACA_TipoJustificativaFaltaBO.TiposJustificativaFalta());
        }

        /// <summary>Seta o foco no combo</summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        #endregion
    }
}