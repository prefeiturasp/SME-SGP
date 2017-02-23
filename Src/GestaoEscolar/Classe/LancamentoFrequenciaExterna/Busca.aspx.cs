namespace GestaoEscolar.Classe.LancamentoFrequenciaExterna
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// ID do aluno para edição
        /// </summary>
        public long EditAluId
        {
            get
            {
                long alu_id = -1;

                if (grvResultado.DataKeys != null && grvResultado.DataKeys.Count > grvResultado.EditIndex)
                {
                    alu_id = Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values["alu_id"]);
                }

                return alu_id;
            }
        }

        /// <summary>
        /// ID da matricula turma do aluno para edição
        /// </summary>
        public int EditMtuId
        {
            get
            {
                int mtu_id = -1;

                if (grvResultado.DataKeys != null && grvResultado.DataKeys.Count > grvResultado.EditIndex)
                {
                    mtu_id = Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values["mtu_id"]);
                }

                return mtu_id;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (ViewState["VS_Ordenacao"] == null)
                {
                    ViewState["VS_Ordenacao"] = "pes_nome";
                }

                return ViewState["VS_Ordenacao"].ToString();
            }

            set
            {
                ViewState["VS_Ordenacao"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (ViewState["VS_SortDirection"] == null)
                {
                    ViewState["VS_SortDirection"] = SortDirection.Ascending;
                }

                return (SortDirection)(ViewState["VS_SortDirection"]);
            }

            set
            {
                ViewState["VS_SortDirection"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        private void UCFiltroEscolas__Selecionar()
        {
            try
            {
                if (UCFiltroEscolas._VS_FiltroEscola)
                    UCFiltroEscolas._UnidadeEscola_LoadBy_uad_idSuperior(UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID, true, false);

                UCFiltroEscolas._ComboUnidadeEscola.Enabled = UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID != Guid.Empty;

                if (UCFiltroEscolas._ComboUnidadeEscola.Enabled)
                    UCFiltroEscolas._ComboUnidadeEscola.Focus();

                updPesquisa.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        private void UCFiltroEscolas__SelecionarEscola()
        {
            try
            {
                UCCCalendario.Valor = -1;
                UCCCalendario.PermiteEditar = false;
                UCCTurma.Valor = new long[] { -1, -1, -1 };
                UCCTurma.PermiteEditar = false;

                if (UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID > 0 && UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID > 0)
                {
                    UCCCalendario.PermiteEditar = true;
                    UCCCalendario.SetarFoco();
                }             

                updPesquisa.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        private void UCCCalendario_IndexChanged()
        {
            try
            {
                CarregarComboTurma();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        #endregion Delegates

        #region Page Life Cicle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string mensagem = __SessionWEB.PostMessages;
                    if (!string.IsNullOrEmpty(mensagem))
                    {
                        lblMensagem.Text = mensagem;
                    }

                    InicializarTela();
                    VerificarBusca();
                }

                UCFiltroEscolas._Selecionar += UCFiltroEscolas__Selecionar;
                UCFiltroEscolas._SelecionarEscola += UCFiltroEscolas__SelecionarEscola;
                UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Page Life Cicle

        #region Métodos

        /// <summary>
        /// Inicializa componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            UCCCalendario.CarregarCalendarioAnual();
            UCCCalendario.Valor = -1;
            UCCCalendario.PermiteEditar = false;
            UCCTurma.Valor = new long[] { -1, -1, -1 };
            UCCTurma.PermiteEditar = false;
            UCFiltroEscolas.SelecionaCombosAutomatico = false;
            UCFiltroEscolas._LoadInicial(false, true);
            if (UCFiltroEscolas._VS_FiltroEscola)
            {
                UCFiltroEscolas._cvUnidadeAdministrativa.ValidationGroup = "Busca";
            }
            UCFiltroEscolas._cvUnidadeEscola.ValidationGroup = "Busca";
            updPesquisa.Update();
        }

        /// <summary>
        /// Realiza a pesquisa de alunos para lançamento de frequência externa
        /// </summary>
        private void Pesquisar()
        {
            using (DataTable dt = MTR_MatriculaTurmaBO.SelecionaAlunosEntradaOutrasRedes(UCCTurma.Valor[0]))
            {
                using (DataView dv = dt.DefaultView)
                {
                    dv.Sort = string.Format("{0} {1}", VS_Ordenacao, VS_SortDirection == SortDirection.Ascending ? "ASC" : "DESC");
                    grvResultado.DataSource = dv;
                    grvResultado.DataBind();
                }
            }

            pnlResultados.Visible = true;
            updResultado.Update();

            #region Salvar busca realizada

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            filtros.Add("turmaExtinta", chkTurmaExtinta.Checked.ToString());

            if (UCFiltroEscolas._VS_FiltroEscola)
            {
                filtros.Add("uad_idSuperior", UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID.ToString());
            }

            filtros.Add("esc_id", UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID.ToString());
            filtros.Add("uni_id", UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID.ToString());
            filtros.Add("cal_id", UCCCalendario.Valor.ToString());
            filtros.Add("tur_id", UCCTurma.Valor[0].ToString());
            filtros.Add("crp_id", UCCTurma.Valor[1].ToString());
            filtros.Add("ttn_id", UCCTurma.Valor[2].ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.LancamentoFrequenciaExterna
                    ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada
        }

        /// <summary>
        /// Verifica busca já realizada
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LancamentoFrequenciaExterna)
            {
                string valor1, valor2, valor3;

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("turmaExtinta", out valor1))
                {
                    chkTurmaExtinta.Checked = Convert.ToBoolean(valor1);
                    valor1 = string.Empty;
                }

                if (UCFiltroEscolas._VS_FiltroEscola)
                {
                    if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor1))
                    {
                        UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue = valor1;
                        UCFiltroEscolas__Selecionar();
                        valor1 = string.Empty;
                    }
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor1) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2))
                {
                    UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = string.Format("{0};{1}", valor1, valor2);
                    UCFiltroEscolas__SelecionarEscola();
                    valor1 = string.Empty;
                    valor2 = string.Empty;
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor1))
                {
                    UCCCalendario.Valor = Convert.ToInt32(valor1);
                    UCCCalendario_IndexChanged();
                    valor1 = string.Empty;
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor1) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor2) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor3))
                {
                    UCCTurma.Valor = new long[] { Convert.ToInt64(valor1), Convert.ToInt64(valor2), Convert.ToInt64(valor3) };
                    updPesquisa.Update();
                }

                if (UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID > 0 && UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID > 0 &&
                    UCCCalendario.Valor > 0 && UCCTurma.Valor[0] > 0)
                {
                    Pesquisar();
                }
            }
        }

        /// <summary>
        /// Carrega o combo de turmas conforme os filtros
        /// </summary>
        private void CarregarComboTurma()
        {
            UCCTurma.Valor = new long[] { -1, -1, -1 };
            UCCTurma.PermiteEditar = false;

            if (UCCCalendario.Valor > 0)
            {
                TUR_TurmaSituacao tur_situacao = TUR_TurmaSituacao.Ativo;
                if (chkTurmaExtinta.Checked)
                {
                    tur_situacao = TUR_TurmaSituacao.Extinta;
                }
                UCCTurma.CarregarPorEscolaCalendarioSituacao_TurmasNormais(UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID, UCCCalendario.Valor, tur_situacao);
                UCCTurma.SetarFoco();
                UCCTurma.PermiteEditar = true;
            }

            updPesquisa.Update();
        }

        #endregion Métodos

        #region Eventos

        protected void chkTurmaExtinta_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarComboTurma();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox  
                UCComboQtdePaginacao.Valor = itensPagina;

                grvResultado.PageIndex = 0;
                grvResultado.PageSize = UCComboQtdePaginacao.Valor;

                VS_Ordenacao = "pes_nome";
                VS_SortDirection = SortDirection.Ascending;

                Pesquisar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        protected void grvResultado_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = MTR_MatriculaTurmaBO.GetTotalRecords();
            ConfiguraColunasOrdenacao((GridView)sender, VS_Ordenacao, VS_SortDirection);
        }

        protected void grvResultado_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvResultado.EditIndex = e.NewEditIndex;
        }

        protected void grvResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grvResultado.PageIndex = e.NewPageIndex;
                Pesquisar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        protected void grvResultado_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                VS_Ordenacao = e.SortExpression;
                VS_SortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                Pesquisar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // Atribui nova quantidade itens por página para o grid.
            grvResultado.PageSize = UCComboQtdePaginacao.Valor;
            grvResultado.PageIndex = 0;
            Pesquisar();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            RedirecionarPagina("~/Classe/LancamentoFrequenciaExterna/Busca.aspx");
        }

        #endregion Eventos
    }
}