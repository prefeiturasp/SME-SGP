using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.Academico.OrientacaoCurricular
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// ID do curso.
        /// </summary>
        public int Edit_Cur_id
        {
            get
            {
                return UCComboCursoCurriculo1.Valor[0];
            }
        }

        /// <summary>
        /// ID do currículo.
        /// </summary>
        public int Edit_Crr_id
        {
            get
            {
                return UCComboCursoCurriculo1.Valor[1];
            }
        }

        /// <summary>
        /// ID do período.
        /// </summary>
        public int Edit_Crp_id
        {
            get
            {
                return UCComboCurriculoPeriodo1.Valor[2];
            }
        }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        public int Edit_Tds_id
        {
            get
            {
                return UCComboTipoDisciplina1.Valor;
            }
        }

        /// <summary>
        /// ID da matriz de habilidades.
        /// </summary>
        public int Edit_Mat_id
        {
            get
            {
                return UCComboMatrizHabilidades.Valor;
            }
        }

        /// <summary>
        /// ID do calendário.
        /// </summary>
        public int Edit_Cal_id
        {
            get
            {
                return UCComboCalendario1.Valor;
            }
        }

        public string Edit_Curso
        {
            get
            {
                return UCComboCursoCurriculo1.Texto;
            }
        }

        public string Edit_Grupamento
        {
            get
            {
                return UCComboCurriculoPeriodo1._Combo.SelectedItem.ToString();
            }
        }

        public string Edit_Disciplina
        {
            get
            {
                return UCComboTipoDisciplina1.Texto;
            }
        }

        public string Edit_Calendario
        {
            get
            {
                return UCComboCalendario1.Texto;
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                {
                    lblMensagem.Text = message;
                    updMessage.Update();
                }

                if (!IsPostBack)
                {
                    VerificaPermissaoUsuario();

                    // Carrega os combos.
                    UCComboCursoCurriculo1.CarregarCursoCurriculoSituacao(1);
                    UCComboCalendario1.CarregarCalendarioAnual();

                    UCComboTipoDisciplina1.CarregarTipoDisciplinaPorCursoCurriculoPeriodo(1,1,1);
                    UCComboTipoDisciplina1.PermiteEditar = false;                    
                    
                    UCComboCurriculoPeriodo1._Combo.Enabled = false;
                    UCComboCurriculoPeriodo1.CancelSelect = true;

                    UCComboMatrizHabilidades.Carregar();

                    bool orientacoesCurricularesAula =
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    string url = orientacoesCurricularesAula ? "~/Academico/OrientacaoCurricular/CadastroHabilidade.aspx" :
                                                               "~/Academico/OrientacaoCurricular/Cadastro.aspx";
                    UCComboMatrizHabilidades.Visible = orientacoesCurricularesAula;

                    btnPesquisar.PostBackUrl = url;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }

            Page.Form.DefaultButton = btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = UCComboCursoCurriculo1.Combo_ClientID;

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboCurriculoPeriodo1._OnSelectedIndexChange += UCComboCurriculoPeriodo1_IndexChanged;
        }

        #endregion

        #region Delegates

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboTipoDisciplina1._Combo.SelectedValue = "-1";

                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
                UCComboCurriculoPeriodo1.PermiteEditar = true;

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1.CancelSelect = false;
                UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], -1, -1);

                UCComboCurriculoPeriodo1._Combo.Enabled = UCComboCursoCurriculo1.Valor[0] > 0;
                UCComboTipoDisciplina1.PermiteEditar = (UCComboCurriculoPeriodo1.Valor[0] > 0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCurriculoPeriodo1_IndexChanged()
        {
            try
            {
                UCComboTipoDisciplina1._Combo.SelectedValue = "-1";
                if (UCComboCurriculoPeriodo1.Valor[0] > 0)
                {
                    UCComboTipoDisciplina1.CarregarTipoDisciplinaPorCursoCurriculoPeriodoTipoDisciplina(UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], (byte)ACA_CurriculoDisciplinaTipo.Regencia);
                }

                UCComboTipoDisciplina1.Valor = -1;
                UCComboTipoDisciplina1.PermiteEditar = (UCComboCursoCurriculo1.Valor[0] > 0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            pnlPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }

        #endregion

        #region Eventos

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/OrientacaoCurricular/Busca.aspx");
        }

        #endregion
    }
}