using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboMatrizHabilidades : MotherUserControl
    {
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
            get { return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty); }
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        #endregion PROPRIEDADES

        #region METODOS

        /// <summary>
        /// Seta o foco no combo
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Carrega o combo
        /// </summary>
        public void Carregar()
        {
            odsDados.SelectParameters.Clear();
            odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            ddlCombo.Items.Clear();
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma matriz de habilidades --", "-1", true));
            ddlCombo.DataBind();
        }

        #endregion METODOS

        #region EVENTOS

        protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                // Grava o erro e mostra pro usuário.
                ApplicationWEB._GravaErro(e.Exception.InnerException);

                e.ExceptionHandled = true;

                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        #endregion EVENTOS
    }
}