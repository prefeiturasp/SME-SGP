using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboCursoPeriodo : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Configura validação do combo de curso período.
        /// </summary>
        public bool Obrigatorio
        {
            get
            {
                if (ViewState["Obrigatorio"] != null)
                    return Convert.ToBoolean(ViewState["Obrigatorio"]);
                return false;
            }
            set
            {
                ViewState["Obrigatorio"] = value;
            }

        }

        /// <summary>
        /// Configura título do combo de curso período.
        /// </summary>
        public string Titulo
        {
            get
            {
                return lblCursoPeriodo.Text;
            }
            set
            {
                lblCursoPeriodo.Text = value;
                cpvCursoPeriodo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Seta a visibilidade de combo
        /// </summary>
        public bool MostraCombo
        {
            get
            {
                return ddlCursoPeriodo.Visible;
            }
            set
            {
                ddlCursoPeriodo.Visible = value;
            }
        }

        /// <summary>
        /// Seta a visibilidade de label
        /// </summary>
        public bool MostraLabel
        {
            get
            {
                return lblCursoPeriodo.Visible;
            }
            set
            {
                lblCursoPeriodo.Visible = value;
            }
        }

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// valor[0] = cur_id
        /// valor[1] = crr_id
        /// valor[1] = crp_id
        /// </summary>
        public Int32[] Valor
        {
            get
            {
                string[] s = ddlCursoPeriodo.SelectedValue.Split(';');

                if (s.Length == 3)
                    return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]) };

                return new[] { -1, -1, -1 };
            }
            set
            {
                string s;
                if (value.Length == 3)
                    s = value[0] + ";" + value[1] + ";" + value[2];
                else
                    s = "-1";

                ddlCursoPeriodo.SelectedValue = s;
            }
        }

        /// <summary>
        /// Retorna o Cur_ID selecionado no combo.    
        /// </summary>
        public int Cur_ID
        {
            get
            {
                return Valor[0];
            }
        }

        /// <summary>
        /// Retorna o Crr_ID selecionado no combo.    
        /// </summary>
        public int Crr_ID
        {
            get
            {
                return Valor[1];
            }
        }

        /// <summary>
        /// Retorna o Crp_ID selecionado no combo.    
        /// </summary>
        public int Crp_ID
        {
            get
            {
                return Valor[2];
            }
        }

        /// <summary>
        /// Seta o validation group do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvCursoPeriodo.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Retorna o index do combo.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ddlCursoPeriodo.SelectedIndex;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado.
        /// </summary>
        public bool Enabled
        {
            set
            {
                ddlCursoPeriodo.Enabled = value;
            }
        }

        /// <summary>
        /// Retorna o combo.
        /// </summary>
        public DropDownList ddlCombo
        {
            get
            {
                return ddlCursoPeriodo;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCursoPeriodo.AutoPostBack = IndexChanged != null;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega todos os cursos/periodos ativos.
        /// </summary>
        public void Carregar()
        {

            ddlCursoPeriodo.Items.Clear();

            ddlCursoPeriodo.DataSource = ACA_CurriculoPeriodoBO.Select_Ativos(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            ddlCursoPeriodo.Items.Insert(0, new ListItem("-- Selecione o(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " / " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1", true));
            ddlCursoPeriodo.DataBind();

            cpvCursoPeriodo.Visible = Obrigatorio;
        }

        #endregion

        #region Eventos

        protected void ddlCursoPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion
    }
}
