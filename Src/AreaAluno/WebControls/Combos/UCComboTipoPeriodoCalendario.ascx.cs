using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno.WebControls.Combos
{
    public partial class UCComboTipoPeriodoCalendario : MotherUserControl
    {
        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo. tpc_id
        /// </summary>
        public Int32 Valor
        {
            get
            {
                return Convert.ToInt32(ddlTipoPeriodoCalendario.SelectedValue);
            }
            set
            {
                ddlTipoPeriodoCalendario.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Acesso ao dropdownlist do User Control
        /// </summary>
        public DropDownList _Combo
        {
            get { return ddlTipoPeriodoCalendario; }
            set { ddlTipoPeriodoCalendario = value; }
        }

        public CompareValidator _Validator
        {
            get { return cpvTipoPeriodoCalendario; }
            set { cpvTipoPeriodoCalendario = value; }
        }

        public Label _Label
        {
            get { return lblTitulo; }
            set { lblTitulo = value; }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string parametroPeriodo = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            cpvTipoPeriodoCalendario.ErrorMessage = parametroPeriodo + " é obrigatório.";
        }

        #endregion

        #region Métodos

        public void CarregarTipoPeriodoCalendario(long tur_id)
        {
            string parametroPeriodo = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlTipoPeriodoCalendario.Items.Clear();
            ddlTipoPeriodoCalendario.DataTextField = "cap_descricao";
            ddlTipoPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario_Tur(tur_id, ApplicationWEB.AppMinutosCacheLongo);
            ddlTipoPeriodoCalendario.Items.Insert(0, new ListItem("-- Selecione um " + parametroPeriodo + " --", "-1"));
            ddlTipoPeriodoCalendario.AppendDataBoundItems = true;
            ddlTipoPeriodoCalendario.DataBind();

        }

        #endregion
    }
}