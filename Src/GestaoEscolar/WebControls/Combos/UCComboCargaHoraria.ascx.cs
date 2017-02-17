using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboCargaHoraria : MotherUserControl
    {
        #region DELEGATES

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
                if (ddlCombo.Items.FindByValue(value.ToString()) != null)
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
            set
            {
                ddlCombo.Enabled = value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione uma carga horária" do dropdownlist.  
        /// Por padrão é false e a mensagem "Selecione uma carga horária" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma carga horária -- ", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="crg_especialista">Indica se o cargo selecionado é de docente especialista</param>
        public void CarregarCargaHoraria(int crg_id, bool crg_especialista)
        {
            ddlCombo.Items.Clear();

            ddlCombo.DataSource = RHU_CargaHorariaBO.SelecionaPorPadraoEspecialistaCargo(crg_especialista, crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma carga horária -- ", "-1", true));
            ddlCombo.AppendDataBoundItems = true;

            ddlCombo.DataBind();

        }

        #endregion

        #region EVENTOS

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