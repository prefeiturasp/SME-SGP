using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboParametroGrupoPerfil : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Referente ao campo pgs_chave.
        /// </summary>
        public string Valor
        {
            get
            {
                return ddlCombo.SelectedValue;
            }
            set
            {
                ddlCombo.SelectedValue = value;
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
        /// Texto do título ao combo.
        /// </summary>
        public string Texto
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value + " é obrigatório.";
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um grupo padrão" do dropdownlist.  
        /// Por padrão é false e a mensagem "Selecione um grupo padrão" não é exibida.
        /// </summary>
        public bool _MostrarMessageSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um grupo padrão --", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica se existe o valor no dropdownlist    
        /// </summary>
        /// <param name="pgs_chave">Chave do grupo padrão</param>
        /// <returns>True:Item existe / False:Item não existe</returns>
        public bool ExisteItem(string pgs_chave)
        {
            return (ddlCombo.Items.FindByValue(pgs_chave) != null);
        }

        /// <summary>
        /// Carrega o combo com os grupos padrão
        /// </summary>        
        public void CarregarGrupoPadrao()
        {
            try
            {
                ddlCombo.Items.Clear();

                ddlCombo.DataSource = SYS_ParametroGrupoPerfilBO.GetSelect2(Guid.Empty, false, 1, 1);
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um grupo padrão --", "-1", true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();
            }
            catch (Exception e)
            {
                // Grava o erro e mostra pro usuário.
                ApplicationWEB._GravaErro(e.InnerException);

                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }
       
        #endregion

        #region Eventos

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