using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboAvisoTextoGeral : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged IndexChanged;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo
        /// </summary>
        public long Valor
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
        /// Adciona e remove a mensagem "Selecione um aviso e texto" do dropdownlist.
        /// Por padrão é false e a mensagem "Selecione um aviso e texo" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
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
        /// Seta e retorna um titulo diferente do padrão para o combo
        /// </summary>
        public string Titulo
        {
            get
            {
                return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty).Replace("*", string.Empty).Trim();
            }
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
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
        /// Seta o foco no combo    
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
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

        #endregion Propriedades

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

        #endregion Eventos
    }
}