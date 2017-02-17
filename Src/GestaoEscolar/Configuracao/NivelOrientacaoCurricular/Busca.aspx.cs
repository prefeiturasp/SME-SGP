using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.Configuracao.NivelOrientacaoCurricular
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        public int EditItem_cal_id
        {
            get
            {
                return UCComboCalendario1.Valor;
            }
        }

        public int EditItem_cur_id
        {
            get
            {
                return UCComboCursoCurriculo1.Cur_ID;
            }
        }

        public int EditItem_crr_id
        {
            get
            {
                return UCComboCursoCurriculo1.Crr_ID;
            }
        }

        public int EditItem_crp_id
        {
            get
            {
                return UCComboCurriculoPeriodo1.Valor[2];
            }
        }

        public int EditItem_mat_id
        {
            get
            {
                return UCComboMatrizHabilidades.Valor;
            }
        }

        public string EditItem_calendario
        {
            get
            {
                return UCComboCalendario1.Texto;
            }
        }

        public string EditItem_curso
        {
            get
            {
                return UCComboCursoCurriculo1.Texto;
            }
        }

        public string EditItem_grupamento
        {
            get
            {
                return UCComboCurriculoPeriodo1.Texto;
            }
        }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        public int EditItem_tds_id
        {
            get
            {
                return UCComboTipoDisciplina.Valor;
            }
        }

        /// <summary>
        /// Nome do tipo de disciplina.
        /// </summary>
        public string EditItem_tipoDisciplina
        {
            get
            {
                return UCComboTipoDisciplina.Texto;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    // Carrega os combos.
                    UCComboCalendario1.CarregarCalendarioAnual();
                    UCComboMatrizHabilidades.Carregar();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboTipoDisciplina.Valor = -1;
                UCComboTipoDisciplina.PermiteEditar = false;
                UCComboCurriculoPeriodo1._Combo.SelectedIndex = 0;
                UCComboCurriculoPeriodo1.PermiteEditar = true;

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1.CancelSelect = false;
                UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], -1, -1);

                UCComboCurriculoPeriodo1._Combo.Enabled = UCComboCursoCurriculo1.Valor[0] > 0;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboCalendario1_IndexChanged()
        {
            try
            {
                if (UCComboCalendario1.Valor > 0)
                {
                    UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscolaCalendario(-1, -1, 0, UCComboCalendario1.Valor);
                    UCComboCursoCurriculo1.PermiteEditar = true;
                }
                else
                {
                    UCComboCursoCurriculo1.Valor = new int[] { -1, -1 };
                    UCComboCursoCurriculo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboCurriculoPeriodo1_OnSelectedIndexChange()
        {
            try
            {
                UCComboTipoDisciplina.Valor = -1;
                if (UCComboCurriculoPeriodo1.Valor[0] > 0)
                {
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodoTipoDisciplina(UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], (byte)ACA_CurriculoDisciplinaTipo.Regencia);
                }

                UCComboTipoDisciplina.PermiteEditar = (UCComboCursoCurriculo1.Valor[0] > 0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}