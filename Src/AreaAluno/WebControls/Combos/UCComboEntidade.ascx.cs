using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno.WebControls.Combos
{
    public partial class UCComboEntidade : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Referente ao campo ent_id.
        /// </summary>
        public Guid Valor
        {
            get
            {
                //if (new Guid(ddlCombo.SelectedValue) == new Guid())
                //    return new Guid();

                return new Guid(ddlCombo.SelectedValue);
            }
            set
            {
                ddlCombo.SelectedValue = value.ToString();
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
        /// Mostra ou não o label do combo
        /// </summary>
        public bool ExibeTitulo
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um Formato" do dropdownlist.  
        /// Por padrão é false e a mensagem "Selecione um Formato" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma entidade --", Guid.Empty.ToString(), true));
                ddlCombo.AppendDataBoundItems = value;
            }
        }


        public string Cliente_ID
        {
            get
            {
                return ddlCombo.ClientID;
            }
        }

        public bool MostrarCombo;

        #endregion

        #region Métodos

        /// <summary>
        /// Seta o foco no combo.
        /// </summary>
        public new void Focus()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Mostra as entidades não excluídas logicamente no dropdownlist    
        /// </summary>
        public void Carregar()
        {
            try
            {
                ddlCombo.Items.Clear();

                ddlCombo.DataSource = SYS_EntidadeBO.GetSelect(Guid.Empty, Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, false, 0, 0);

                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma entidade --", Guid.Empty.ToString(), true));
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

        /// <summary>
        /// Carrega o dropdownlist por entidade e situacao
        /// </summary>
        public void CarregarPorEntidadeSituacao(Guid ent_id, byte ent_situacao)
        {
            try
            {
                ddlCombo.Items.Clear();

                ddlCombo.DataSource = SYS_EntidadeBO.GetSelectBy_SistemaEntidade(ent_id, Guid.Empty, string.Empty, string.Empty, string.Empty, ent_situacao, false, 0, 0);

                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma entidade --", Guid.Empty.ToString(), true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();

                VerificaEntidades();
            }
            catch (Exception e)
            {
                // Grava o erro e mostra pro usuário.
                ApplicationWEB._GravaErro(e.InnerException);

                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// Verifica se possui mais de uma entidade e habilita
        /// ou nao a propriedade para visualisar o combo
        /// </summary>
        private void VerificaEntidades()
        {
            MostrarCombo = true;

            if (ddlCombo.Items.Count <= 2)
            {
                ddlCombo.Items.RemoveAt(0);

                MostrarCombo = false;
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