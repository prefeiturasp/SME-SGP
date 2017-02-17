namespace GestaoEscolar.WebControls.BuscaLancamentoClasse
{
    using System;
    using System.Web;
    using System.Web.UI;

    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;

    /// <summary>
    /// UserControl da pesquisa das telas do menu Classe
    /// </summary>
    public partial class UCBuscaLancamentoClasse : MotherUserControl
    {
        #region Delegates

        public delegate void Pesquisar();

        public event Pesquisar OnPesquisar;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Título do panel.
        /// </summary>
        public string GroupingText
        {
            set
            {
                pnlPesquisa.GroupingText = value;
            }
        }

        /// <summary>
        /// Id da unidade administrativa superior.
        /// </summary>
        public Guid UadIdSuperior
        {
            get
            {
                return UCComboUAEscola1.Uad_ID;
            }
        }

        /// <summary>
        /// UniqueId do botão pesquisar.
        /// </summary>
        public string Pesquisar_UniqueID
        {
            get
            {
                return btnPesquisarTurmas.UniqueID;
            }
        }

        private PaginaGestao _paginaBusca;

        /// <summary>
        /// Página de busca.
        /// </summary>
        public PaginaGestao PaginaBusca
        {
            get
            {
                return _paginaBusca;
            }
            set
            {
                _paginaBusca = value;
            }
        }

        /// <summary>
        /// Id da escola.
        /// </summary>
        public int EscId
        {
            get
            {
                return UCComboUAEscola1.Esc_ID;
            }
        }

        /// <summary>
        /// Id da unidade escola.
        /// </summary>
        public int UniId
        {
            get
            {
                return UCComboUAEscola1.Uni_ID;
            }
        }

        /// <summary>
        /// Id do curso.
        /// </summary>
        public int CurId
        {
            get
            {
                return UCComboCursoCurriculo1.Cur_ID;
            }
        }

        /// <summary>
        /// Id do currículo.
        /// </summary>
        public int CrrId
        {
            get
            {
                return UCComboCursoCurriculo1.Crr_ID;
            }
        }

        /// <summary>
        /// Id do calendário.
        /// </summary>
        public int CalId
        {
            get
            {
                return UCComboCalendario1.Valor;
            }
        }

        /// <summary>
        /// Id do turno.
        /// </summary>
        public int TrnId
        {
            get
            {
                return UCComboTurno1.Valor;
            }
        }

        /// <summary>
        /// Código da turma.
        /// </summary>
        public string TurCodigo
        {
            get
            {
                return txtCodigoTurma.Text;
            }
        }

        /// <summary>
        /// Indica se a visualização do filtro é de Gestor:
        /// apenas pela escola e calendario.
        /// </summary>
        public bool VisualizacaoGestor
        {
            get
            {
                if (ViewState["VisualizacaoGestor"] != null)
                    return Convert.ToBoolean(ViewState["VisualizacaoGestor"]);
                return false;
            }
            set
            {
                ViewState["VisualizacaoGestor"] = value;
            }
        }

        #endregion Propriedades

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(upnBusca, typeof(UpdatePanel), pnlPesquisa.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", pnlPesquisa.ClientID)), true);

            if (!IsPostBack)
            {
                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisarTurmas.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }

            UCComboUAEscola1.IndexChangedUA += UCFiltroEscolas1__Selecionar;
            UCComboUAEscola1.IndexChangedUnidadeEscola += UCFiltroEscolas1__SelecionarEscola;
            UCComboCursoCurriculo1.IndexChanged += _UCComboCursoCurriculo_IndexChanged;

            if ( !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_CAMPO_TURNO_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                 &&
                (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
               )
            {   // quando o parâmetro estiver marcado para não exibir(valor falso) o Turno, e o usuário tiver visão de Gestao, Administracao ou UnidadeAdministrativa
                // o Turno não deve aparece na tela de Busca (Filtro).
                UCComboTurno1.Visible = false;
            }

            if (VisualizacaoGestor)
            {
                UCComboCursoCurriculo1.Visible =
                UCComboTurno1.Visible =
                lblCodigoTurma.Visible =
                txtCodigoTurma.Visible = false;
            }
        }

        /// <summary>
        /// Evento do botão limpar busca.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Evento do botão limpar pesquisar.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        protected void btnPesquisarTurmas_Click(object sender, EventArgs e)
        {
            PesquisarDados();
        }

        /// <summary>
        /// Inicializa os combos.
        /// </summary>
        public void Inicializar()
        {
            try
            {
                UCComboUAEscola1.FocusUA();
                UCComboUAEscola1.Inicializar();
                UCComboTurno1.CarregarTurno();

                this.VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Realiza a busca dos dados de acordo com o delegate.
        /// </summary>
        private void PesquisarDados()
        {
            if (OnPesquisar != null)
            {
                OnPesquisar();
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaBusca)
            {
                string valor;
                string valor2;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                if (!string.IsNullOrEmpty(valor))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor);
                    SelecionarEscola(UCComboUAEscola1.FiltroEscola);
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                _UCComboCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCComboCalendario1.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("trn_id", out valor);
                UCComboTurno1.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor);
                txtCodigoTurma.Text = valor;
                txtCodigoTurma.Focus();

                PesquisarDados();
            }
            else
            {
                UCFiltroEscolas1__SelecionarEscola();

                UCComboCalendario1.CarregarCalendarioAnual();
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
        /// realizada.
        /// </summary>
        /// <param name="filtroEscolas"></param>
        private void SelecionarEscola(bool filtroEscolas)
        {
            if (filtroEscolas)
            {
                UCFiltroEscolas1__Selecionar();
            }

            string esc_id;
            string uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                UCFiltroEscolas1__SelecionarEscola();
            }
        }

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCFiltroEscolas1__Selecionar()
        {
            try
            {
                UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }
                else
                {
                    UCComboCursoCurriculo1.Valor = new[] { -1, -1, -1 };
                    UCComboCursoCurriculo1.CarregarCursoCurriculo();
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void UCFiltroEscolas1__SelecionarEscola()
        {
            try
            {
                if (PaginaBusca == PaginaGestao.ProgressaoPEJA)
                {
                    if (UCComboUAEscola1.Esc_ID > 0)
                    {
                        UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscolaPEJA(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, 0);
                        UCComboCursoCurriculo1.SetarFoco();
                    }
                    else
                    {
                        UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscolaPEJA(-1, -1, 0);
                    }    
                }
                else
                {
                    if (UCComboUAEscola1.Esc_ID > 0)
                    {
                        UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, 0);
                        UCComboCursoCurriculo1.SetarFoco();
                    }
                    else
                    {
                        UCComboCursoCurriculo1.CarregarCursoCurriculo();
                    }    
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) curso(s) da unidade escolar.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de curso.
        /// </summary>
        private void _UCComboCursoCurriculo_IndexChanged()
        {
            try
            {
                if (UCComboCursoCurriculo1.Cur_ID > 0)
                {
                    UCComboCalendario1.CarregarCalendarioAnualPorCurso(UCComboCursoCurriculo1.Cur_ID);
                    UCComboCalendario1.SetarFoco();
                }
                else
                {
                    UCComboCalendario1.CarregarCalendarioAnual();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) período(s) do curso.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}