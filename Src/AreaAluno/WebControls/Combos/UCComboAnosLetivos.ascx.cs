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

        public int mtu_id
        {
            get
            {
                string[] mtu_id_cal_id = ddlAnosLetivos.SelectedValue.Split(',');

                int valor = -1;
                if (mtu_id_cal_id.Length > 0)
                {
                    int.TryParse(mtu_id_cal_id[0], out valor);
                }

                return valor;
            }

            set
            {
                var valores = (from ListItem item in ddlAnosLetivos.Items
                               let mtu_id_cal_id = item.Value.Split(',')
                               where mtu_id_cal_id.Length > 0
                                     && mtu_id_cal_id[0] == value.ToString()
                               select item.Value);

                if (valores.Any())
                {
                    ddlAnosLetivos.SelectedValue = valores.First();
                }
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
                return Convert.ToInt32(ddlAnosLetivos.SelectedItem.Text.Split('-').First().Trim());
            }
            set
            {
                var valores = (from ListItem item in ddlAnosLetivos.Items
                               let texto = item.Text.Split(',')
                               where texto.Length > 0
                                     && texto[0] == value.ToString()
                               select item.Text);

                if (valores.Any())
                {
                    if (ddlAnosLetivos.Items.Cast<ListItem>().Any(p => p.Text == valores.First()))
                    {
                        ddlAnosLetivos.SelectedValue = ddlAnosLetivos.Items.FindByText(valores.First()).Value;
                    }
                    else
                    {
                        ddlAnosLetivos.SelectedValue = ddlAnosLetivos.Items.Cast<ListItem>().Last().Value;
                    }
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

            var valores = (from ListItem item in ddlAnosLetivos.Items
                           let mtu_id_cal_id = item.Value.Split(',')
                           where mtu_id_cal_id.Length > 0
                                 && mtu_id_cal_id[0] == mtu_id.ToString()
                           select item.Value);

            // Selecionar o mtu_id que está na sessão do aluno.
            if (valores.Any())
            {
                ddlAnosLetivos.SelectedValue = valores.First();
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