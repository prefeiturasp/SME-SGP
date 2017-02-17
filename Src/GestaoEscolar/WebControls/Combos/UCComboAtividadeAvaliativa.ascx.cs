using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboAtividadeAvaliativa : MotherUserControl
    {
        #region Delegates

        public delegate void IndexChanged();

        public event IndexChanged _IndexChanged;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Valor = tnt_id
        /// </summary>
        public int Valor
        {
            get
            {
                return Convert.ToInt32(ddlAtividade.SelectedValue);
            }
            set
            {
                if (ddlAtividade.Items.FindByValue(value.ToString()) != null)
                    ddlAtividade.SelectedValue = value.ToString();
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
                    AdicionaAsteriscoObrigatorio(lblAtividade);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblAtividade);
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
                ddlAtividade.Enabled = value;
            }
        }

        /// <summary>
        /// Texto do título ao combo.
        /// </summary>
        public string Texto
        {
            set
            {
                lblAtividade.Text = value;
                cpvCombo.ErrorMessage = value + " é obrigatório.";
            }
        }

        /// <summary>
        /// Nome da atividade selecionada no combo.
        /// </summary>
        public string TextoSelecionado
        {
            get
            {
                return ddlAtividade.SelectedItem.Text;
            }
        }

        /// <summary>
        /// Adiciona e remove a mensagem "Nota/Conceito final" do dropdownlist.
        /// Por padrão é false e a mensagem "Nota/Conceito final" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if (value)
                    ddlAtividade.Items.Insert(0, new ListItem("Nota/Conceito final", "-1", true));
                ddlAtividade.AppendDataBoundItems = value;
            }
        }

        public int Count
        {
            get
            {
                return ddlAtividade.Items.Count;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método que carrega as atividades avaliativas por turmadisciplina.
        /// </summary>
        /// <param name="tud_id">Id da TurmaDisciplina</param>
        public void CarregarAtividadePorTurma(long tud_id)
        {
            ddlAtividade.Items.Clear();
            MostrarMessageSelecione = true;

            DataTable dt = CLS_TurmaNotaBO.SelecionaPorTurmaDisciplina(tud_id);

            ddlAtividade.DataSource = dt;
            ddlAtividade.DataBind();
        }

        public void CarregarPorTurmaDisciplinaPeriodoCalendario(long tud_id, int tpc_id)
        {
            ddlAtividade.Items.Clear();
            MostrarMessageSelecione = true;

            DataTable dt = CLS_TurmaNotaBO.SelecionaPorTurmaDisciplina(tud_id, tpc_id);

            ddlAtividade.DataSource = dt;
            ddlAtividade.DataBind();

        }

        #endregion Métodos

        #region Eventos

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlAtividade.AutoPostBack = (_IndexChanged != null);
        }

        protected void ddlAtividade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_IndexChanged != null)
            {
                _IndexChanged();
            }
        }

        #endregion Eventos


    }
}