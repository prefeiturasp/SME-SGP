using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AreaAluno.WebControls.Combos
{
    public partial class UCComboAnosLetivos : MotherUserControl
    {

        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion Delegates


        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Referente ao campo mtu_id.
        /// </summary>
        public string Valor
        {
            get
            {
                return ddlAnosLetivos.SelectedValue;
            }
            set
            {
                ddlAnosLetivos.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Referente ao campo Ano.
        /// </summary>
        public int Ano
        {
            get
            {
                return Convert.ToInt32(ddlAnosLetivos.SelectedItem.Text);
            }
            set
            {
                if (ddlAnosLetivos.Items.Cast<ListItem>().Any(p => p.Text == value.ToString()))
                {
                    ddlAnosLetivos.SelectedValue = ddlAnosLetivos.Items.FindByText(value.ToString()).Value;
                }
                else
                {
                    ddlAnosLetivos.SelectedValue = ddlAnosLetivos.Items.Cast<ListItem>().Last().Value;
                }
            }
        }

        public DropDownList Combo
        {
            get
            {
                return ddlAnosLetivos;
            }
            set
            {
                ddlAnosLetivos = value;
            }
        }

        #endregion Propriedades


        #region Métodos

        /// <summary>
        /// Carrega o combo de anos letivos com todas as matrículas do aluno.
        /// </summary>
        public void CarregarComboAnosLetivos(long alu_id, int mtu_id)
        {
            DataTable dt = ACA_AlunoBO.AreaAluno_DadosTurmaAtualBoletim(alu_id, 0);
            ddlAnosLetivos.DataSource = dt;
            ddlAnosLetivos.DataBind();

            // Exibe o combo quando tem mais de 1 item.
            divAnosLetivos.Visible = ddlAnosLetivos.Items.Count > 0;

            // Selecionar o mtu_id que está na sessão do aluno.
            if (ddlAnosLetivos.Items.FindByValue(mtu_id.ToString()) != null)
            {
                ddlAnosLetivos.SelectedValue = mtu_id.ToString();
            }
        }
        #endregion Métodos

        #region Eventos

        protected void ddlAnosLetivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion Eventos
    }
}