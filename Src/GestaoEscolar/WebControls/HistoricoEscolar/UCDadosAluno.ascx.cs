using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.HistoricoEscolar
{
    public partial class UCDadosAluno : MotherUserControl
    {
        #region Delegate

        public delegate void onClickVisualizar();
        public event onClickVisualizar clickVisualizar;

        public delegate void onClickVoltar();
        public event onClickVoltar clickVoltar;

        #endregion

        #region Propiedades

        public Int64 VS_alu_id
        {
            get
            {
                return ViewState["VS_alu_id"] == null ? 0 : Convert.ToInt64(ViewState["VS_alu_id"]);
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        public int VS_esc_id
        {
            get
            {
                return ViewState["VS_esc_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_esc_id"]);
            }
            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        public int VS_uni_id
        {
            get
            {
                return ViewState["VS_uni_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_uni_id"]);
            }
            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        public string message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Carrega os dados do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        public void Carregar(Int64 alu_id)
        {
            try
            {
                DataTable dtDados = ACA_AlunoBO.BuscaDadosAluno(alu_id, true);

                if (dtDados.Rows.Count > 0)
                {
                    VS_alu_id = Convert.ToInt64(dtDados.Rows[0]["alu_id"]);
                    VS_esc_id = Convert.ToInt32(dtDados.Rows[0]["esc_id"]);
                    VS_uni_id = Convert.ToInt32(dtDados.Rows[0]["uni_id"]);

                    txtNome.Text = dtDados.Rows[0]["pes_nome"].ToString();
                    txtMatricula.Text = dtDados.Rows[0]["alc_registroGeral"].ToString();

                    txtCidade.Text = dtDados.Rows[0]["cid_nome"].ToString();
                    txtEstado.Text = dtDados.Rows[0]["unf_nome"].ToString();
                    txtNacionalidade.Text = dtDados.Rows[0]["naturalidade"].ToString();
                    txtDataNascimento.Text = dtDados.Rows[0]["pes_dataNascimento"].ToString();

                    txtRG.Text = dtDados.Rows[0]["psd_numero"].ToString();
                    txtDataExpedicao.Text = dtDados.Rows[0]["psd_dataEmissao"].ToString();
                    txtOrgao.Text = dtDados.Rows[0]["psd_orgaoEmissao"].ToString();
                    txtEstadoRG.Text = dtDados.Rows[0]["unf_nomeRG"].ToString();

                    txtNumeroCertidao.Text = dtDados.Rows[0]["ctc_numeroTermo"].ToString();
                    txtFolha.Text = dtDados.Rows[0]["ctc_folha"].ToString();
                    txtLivro.Text = dtDados.Rows[0]["ctc_livro"].ToString();
                    txtEstadoCN.Text = dtDados.Rows[0]["unf_nomeCN"].ToString();
                }

            }
            catch
            {
            }
        }

        #endregion

        #region Eventos

        protected void btnVisualizarHistorico_Click(object sender, EventArgs e)
        {
            if (clickVisualizar != null)
            {
                clickVisualizar();
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (clickVoltar != null)
            {
                clickVoltar();
            }
        }

        #endregion

    }
}