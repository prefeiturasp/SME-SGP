using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoCurriculoPeriodo : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public SelectedIndexChanged OnSelectedIndexChanged;

        #endregion Delegates

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
                cpvCombo.ErrorMessage = RemoveAsteriscoObrigatorio(value) + " é obrigatório.";
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um ano" do dropdownlist.  
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if ((value) && (ddlCombo.Items.FindByValue("-1") == null))
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoTipoCurrPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Indica se deve trazer o primeiro item selecinado caso seja o único
        /// (Sem contar a MensagemSelecione)
        /// </summary>
        public bool TrazerComboCarregado
        {
            get
            {
                if (ViewState["TrazerComboCarregado"] != null)
                    return Convert.ToBoolean(ViewState["TrazerComboCarregado"]);
                return false;
            }
            set
            {
                ViewState["TrazerComboCarregado"] = value;
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Altera o Label para o nome padrão de tipo curriculo período no sistema
                Titulo = GestaoEscolarUtilBO.nomePadraoTipoCurrPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (cpvCombo.Visible)
                    AdicionaAsteriscoObrigatorio(lblTitulo);
            }
        }

        #endregion Page Life Cycle

        #region METODOS

        /// <summary>
        /// Traz o primeiro item selecinado caso seja o único
        /// </summary>
        private void SelecionaPrimeiroItem()
        {
            if (TrazerComboCarregado && ddlCombo.Items.Count == 2 && ddlCombo.Items[0].Value == "-1")
            {
                // Seleciona o primeiro item.
                ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

                if (OnSelectedIndexChanged != null)
                    OnSelectedIndexChanged();
            }
        }

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
        public void CarregarPorAnoNivelEnsino(int chp_anoLetivo, int tne_id)
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = ACA_TipoCurriculoPeriodoBO.SelecionaPorAnoLetivoNivelEnsino(chp_anoLetivo, tne_id);
            MostrarMessageSelecione = true;
            ddlCombo.DataBind();
        }

        /// <summary>
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// </summary>
        public void CarregarPorNivelEnsinoModalidade(int tne_id, int tme_id)
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = ACA_TipoCurriculoPeriodoBO.SelectByPesquisa(tne_id, tme_id);
            MostrarMessageSelecione = true;
            ddlCombo.DataBind();
            SelecionaPrimeiroItem();
        }

        /// <summary>
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// de acordo com as atribuições do docente.
        /// </summary>
        public void CarregarPorNivelEnsinoModalidadeDocente(int tne_id, int tme_id, long doc_id)
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = ACA_TipoCurriculoPeriodoBO.SelecionaTipoCurriculoPeriodoDocente(tne_id, tme_id, doc_id);
            MostrarMessageSelecione = true;
            ddlCombo.DataBind();
            SelecionaPrimeiroItem();
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged();
        }

        #endregion
    }
}