using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboCurriculoPeriodoAvaliacao1 : MotherUserControl
    {
        #region DELEGATES

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        public delegate void SelectedIndexChanged_Sender(object sender, EventArgs e);
        public event SelectedIndexChanged_Sender IndexChanged_Sender;

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
                ddlCombo.SelectedValue = value.ToString();
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
                cpvCombo.ErrorMessage = value.EndsWith("*") ? value.Remove(value.Length - 2) + " é obrigatório." : value + " é obrigatório.";
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
        /// Propriedade que seta o SelectedIndex do Combo.       
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um Cruso" do dropdownlist.  
        /// Por padrão é false e a mensagem "Selecione um Curso" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "-1", true));
                ddlCombo.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo.   
        /// </summary>
        public Int32 WidthCombo
        {
            set
            {
                ddlCombo.Width = value;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Mostra as avaliações do período não excluídas logicamente no dropdownlist  
        /// </summary>
        /// <param name="crr_qtdeAvaliacaoProgressao">Quantidade de avaliações do período do currículo</param>
        /// <param name="crp_nomeAvaliacao">Nome das avaliações do período do currículo</param>        
        public void CarregarAvaliacaoPorCurriculoPeriodo
        (
            int crr_qtdeAvaliacaoProgressao
            , string crp_nomeAvaliacao
        )
        {
            try
            {
                ddlCombo.Items.Clear();

                DataTable dt = new DataTable();
                dt.Columns.Add("numeroAvaliacao");
                dt.Columns.Add("crp_nomeAvaliacao");

                for (int i = 1; i <= crr_qtdeAvaliacaoProgressao; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["numeroAvaliacao"] = i;
                    dr["crp_nomeAvaliacao"] = crp_nomeAvaliacao + " " + i;
                    dt.Rows.Add(dr);
                }

                ddlCombo.DataSource = dt;
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "-1", true));
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
        /// Seleciona o primeiro registro do combo
        /// </summary>
        /// <returns></returns>
        public void SelecionaPrimeiroItem()
        {
            // Seleciona o primeiro item.
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

            if (IndexChanged != null)
                IndexChanged();
        }

        /// <summary>
        /// Mostra as avaliações da turma não excluídas logicamente no dropdownlist  
        /// </summary>
        /// <param name="tur_id">Quantidade de avaliações do período do currículo</param>
        public void CarregarAvaliacaoPorTurma
        (
            long tur_id
        )
        {
            try
            {
                ddlCombo.Items.Clear();

                List<TUR_TurmaCurriculoAvaliacao> listaAvaliacaoBanco = TUR_TurmaCurriculoAvaliacaoBO.SelecionaAvaliacaoPorTurma(tur_id);

                DataTable dt = new DataTable();
                dt.Columns.Add("numeroAvaliacao");
                dt.Columns.Add("crp_nomeAvaliacao");

                foreach (TUR_TurmaCurriculoAvaliacao entity in listaAvaliacaoBanco)
                {
                    DataRow dr = dt.NewRow();
                    dr["numeroAvaliacao"] = entity.tca_numeroAvaliacao;
                    dr["crp_nomeAvaliacao"] = entity.crp_nomeAvaliacao;
                    dt.Rows.Add(dr);
                }

                ddlCombo.DataSource = dt;
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "-1", true));
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

        #region EVENTOS

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (IndexChanged != null) || (IndexChanged_Sender != null);
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();

            if (IndexChanged_Sender != null)
                IndexChanged_Sender(sender, e);
        }

        #endregion
    }
}