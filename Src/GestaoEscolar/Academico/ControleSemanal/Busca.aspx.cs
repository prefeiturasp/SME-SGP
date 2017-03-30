using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;

namespace GestaoEscolar.Academico.ControleSemanal
{
    public partial class Busca : MotherPageLogadoCompressedViewState
    {
        #region Estrutura

        /// <summary>
        /// Estrutura que armazena e agrupa os grids de turma por escola e calendário.
        /// </summary>
        public struct sGridTurmaEscolaCalendario
        {
            public GridView gridTurma { get; set; }
            public Guid uad_idSuperior { get; set; }
            public int esc_id { get; set; }
            public int uni_id { get; set; }
            public int cal_id { get; set; }
            public int cal_ano { get; set; }
        }

        #endregion Estrutura

        #region Propriedades

        private int VS_rptTurmasIndice
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_rptTurmasIndice"] ?? -1);
            }

            set
            {
                ViewState["VS_rptTurmasIndice"] = value;
            }
        }

        private bool VS_titular
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_titular"] ?? false);
            }

            set
            {
                ViewState["VS_titular"] = value;
            }
        }

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        public string PaginaRetorno
        {
            get
            {
                return "~/Academico/ControleTurma/Busca.aspx";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                    }
                }

                return SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Lista com o histórico do docente.
        /// </summary>
        private List<sHistoricoDocente> VS_ltHistoricoDocente
        {
            get
            {
                return (List<sHistoricoDocente>)(ViewState["VS_ltHistoricoDocente"]);
            }
            set
            {
                ViewState["VS_ltHistoricoDocente"] = value;
            }
        }

        /// <summary>
        /// Indica os ids na disciplina selecionada, e foi bloqueado o redirecionamento à
        /// tela do diário, por faltar o lançamento das aulas previstas.
        /// </summary>
        private long[] VS_ChavesRedirecionaDiario
        {
            get
            {
                if (ViewState["VS_ChavesRedirecionaDiario"] == null)
                {
                    return new long[] { -1, -1 };
                }

                return (long[])ViewState["VS_ChavesRedirecionaDiario"];
            }
            set
            {
                ViewState["VS_ChavesRedirecionaDiario"] = value;
            }
        }

        /// <summary>
        /// Verifica se o usuário logado pode salvar os dados das aulas previstas
        /// </summary>
        private bool VS_permiteSalvarAulasPrevistas
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_permiteSalvarAulasPrevistas"] ?? true);
            }

            set
            {
                ViewState["VS_permiteSalvarAulasPrevistas"] = value;
            }
        }

        /// <summary>
        /// Guarda a tela que precisa redirecionar.
        /// </summary>
        private string VS_TelaRedirecionar
        {
            get
            {
                if (ViewState["VS_TelaRedirecionar"] == null)
                    return "";

                return ViewState["VS_TelaRedirecionar"].ToString();
            }
            set
            {
                ViewState["VS_TelaRedirecionar"] = value;
            }
        }

        /// <summary>
        /// Parâmetro que indica se o fechamento será pré carregado no cache.
        /// </summary>
        private bool PreCarregarFechamentoCache
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }
        
        /// <summary>
        /// Informa se o período já foi fechado (evento de fechamento já acabou) e não há nenhum evento de fechamento por vir.
        /// Se o período ainda estiver ativo então não verifica o evento de fechamento
        /// </summary>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cap_dataFim">Data fim do período</param>
        /// <returns></returns>
        private bool VS_PeriodoEfetivado(int tpc_id, int cal_id, long tur_id, DateTime cap_dataFim)
        {
            bool efetivado = false;

            //Se o bimestre está ativo ou nem começou então não bloqueia pelo evento de fechamento
            if (DateTime.Today <= cap_dataFim)
                return false;

            List<ACA_Evento> lstEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false);

            //Só permite editar o bimestre se tiver evento ativo
            efetivado = !lstEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                                DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

            return efetivado;
        }

        /// <summary>
        /// ViewState que armazena a lista de pendências do fechamento.
        /// </summary>
        private Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento>> VS_listaPendenciaFechamento
        {
            get
            {
                return (Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento>>)(ViewState["VS_listaPendenciaFechamento"] ?? (ViewState["VS_listaPendenciaFechamento"] = new Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento>>()));
            }

            set
            {
                ViewState["VS_listaPendenciaFechamento"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                UCComboUAEscola1.FiltroEscolasControladas = true;
                UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCurriculoPeriodo1.PermiteEditar = false;
                UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCCCalendario1.PermiteEditar = false;
                UCCCalendario1.Valor = -1;

                if (UCComboUAEscola1.Esc_ID > 0)
                {
                    UCCCalendario1.CarregarCalendarioAnual();
                    UCCCalendario1.PermiteEditar = true;
                    UCCCalendario1.SetarFoco();
                }

                UCCCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo calendario
        /// </summary>
        private void UCCCalendario1_IndexChanged()
        {
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCComboCursoCurriculo1.PermiteEditar = false;
            UCComboCursoCurriculo1.Valor = new[] { -1, -1 };

            if (UCComboUAEscola1.Esc_ID > 0 && UCCCalendario1.Valor > 0)
            {
                UCComboCursoCurriculo1.PermiteEditar = true;
                // Carregar todos os cursos, não só ativos, para exibir turmas encerradas na busca.
                UCComboCursoCurriculo1.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCCCalendario1.Valor, 0);
                UCComboCursoCurriculo1.SetarFoco();
            }

            UCComboCursoCurriculo1_IndexChanged();
        }

        /// <summary>
        /// Evento change do combo de curso curriculo
        /// </summary>
        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                if (UCComboCursoCurriculo1.Valor[0] > 0)
                {
                    // carrego o ciclo
                    UCComboTipoCiclo.CarregarCicloPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    if (UCComboTipoCiclo.ddlCombo.Items.Count > 0)
                    {
                        UCComboTipoCiclo.Visible = true;
                        UCComboTipoCiclo.Enabled = true;
                        UCComboTipoCiclo.ddlCombo.Focus();

                        UCComboTipoCiclo_IndexChanged();
                    }
                    else
                    {
                        UCCCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                        UCCCurriculoPeriodo1.PermiteEditar = true;
                        UCCCurriculoPeriodo1.SetarFoco();
                    }
                }
                else
                {
                    UCComboTipoCiclo.Visible = false;
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de tipo de ciclo
        /// </summary>
        private void UCComboTipoCiclo_IndexChanged()
        {
            try
            {
                if (UCComboTipoCiclo.Valor > 0)
                {
                    UCCCurriculoPeriodo1.CarregarPorCursoCurriculoTipoCiclo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], UCComboTipoCiclo.Valor);
                    UCCCurriculoPeriodo1.PermiteEditar = true;
                    UCCCurriculoPeriodo1.SetarFoco();
                }
                else
                {
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
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
                UCComboUAEscola1_IndexChangedUA();

            string esc_id, uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Atualiza o grid de acordo com a quantidade de paginação
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // Atribui nova quantidade de itens por página para o grid.
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                grvTurmas.PageIndex = 0;
                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Page life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;

                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;

                    if (VS_visaoDocente)
                    {
                        List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);

                        // Guarda em uma variável as escolas que possuem alguma turma ativa
                        var dadosEscolasAtivas = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo)).ToList();

                        if (dadosEscolasAtivas.Count == 0)
                        {  // se o docente não possuir nenhuma turma - exibir a mensagem informativa
                            lblMensagem.Text = UtilBO.GetErroMessage((String)GetGlobalResourceObject("Mensagens", "MSG_ATRIBUICAODOCENTES"), UtilBO.TipoMensagem.Informacao);
                            lblMensagem1.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemTurma").ToString(),
                                                                      UtilBO.TipoMensagem.Alerta);
                        }

                        VS_titular = dados.Exists(p => p.Turmas.Any(t => t.tdc_id == (int)EnumTipoDocente.Titular));

                        divFiltros.Visible = false;
                    }
                    else
                    {
                        VS_titular = false;
                        divFiltros.Visible = true;
                        grvTurmas.PageSize = ApplicationWEB._Paginacao;

                        #region Inicializar

                        UCComboUAEscola1.FocusUA();
                        UCComboUAEscola1.Inicializar();

                        UCComboTipoCiclo.Carregar();
                        UCComboTipoCiclo.SelectedValue = "-1";

                        this.VerificarBusca();

                        #endregion Inicializar

                        Page.Form.DefaultButton = btnPesquisar.UniqueID;
                        Page.Form.DefaultFocus = UCComboUAEscola1.VisibleUA ? UCComboUAEscola1.ComboUA_ClientID : UCComboUAEscola1.ComboEscola_ClientID;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            #region Associando Delegates

            UCComboUAEscola1.IndexChangedUA += UCComboUAEscola1_IndexChangedUA;
            UCComboUAEscola1.IndexChangedUnidadeEscola += UCComboUAEscola1_IndexChangedUnidadeEscola;
            UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboTipoCiclo.IndexChanged += UCComboTipoCiclo_IndexChanged;

            #endregion Associando Delegates
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                string script = "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
                    "exibirNotaFinal=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id) ? "1" : "0") + ";" +
                    "ExibeCompensacao=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id) ? "1" : "0") + ";" +
                    "MinutosCacheFechamento=" + ApplicationWEB.AppMinutosCacheFechamento + ";";

                if (sm.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Parametros", script, true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Parametros", script, true);
                }
            }
        }

        #endregion Page life Cycle

        #region Métodos

        /// <summary>
        /// Adiciona uma classe css ao um controle da página.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void AddClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Add(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Remove uma classe css ao um controle da página.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void RemoveClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Remove(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Verifica se um controle possui uma classe css.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private bool HasClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return classes.Exists(p => p.Equals(cssClass));
        }

        /// <summary>
        /// Formata string para retornar dados para o cabeçalho por escola e calendário.
        /// </summary>
        /// <param name="escola">Dados escola.</param>
        /// <param name="calendario">Dados calendário.</param>
        /// <returns></returns>
        public string RetornaCabecalho(string escola, string calendario)
        {
            return String.Format("{0}<br />{1}", escola, calendario);
        }

        /// <summary>
        /// Retorna lista de períodos de um calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário anual.</param>
        /// <returns></returns>
        public List<ACA_CalendarioPeriodo> RetornaPeriodo(int cal_id)
        {
            return ACA_CalendarioPeriodoBO.SelecionaPeriodoPorCalendarioEntidade(cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToList();
        }

        /// <summary>
        /// Realiza a pesquisa mediante aos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                odsplanejamentoSemanal.SelectParameters.Clear();
                odsplanejamentoSemanal.SelectParameters.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("cal_id", UCCCalendario1.Valor.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("crp_id", UCCCurriculoPeriodo1.Valor[2].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("ent_id", ent_id.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("tur_codigo", txtCodigoTurma.Text);
                odsplanejamentoSemanal.SelectParameters.Add("tud_tipo", Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia).ToString());
                odsplanejamentoSemanal.SelectParameters.Add("appMinutosCacheCurto", ApplicationWEB.AppMinutosCacheCurto.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("tci_id", UCComboTipoCiclo.Tci_id.ToString());

                grvTurmas.PageIndex = 0;
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                divResultadoVisaoSuperior.Visible = true;

                // Limpar a ordenação realizada.
                grvTurmas.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsplanejamentoSemanal.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                if (UCComboUAEscola1.FiltroEscola)
                    filtros.Add("ua_superior", UCComboUAEscola1.Uad_ID.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.MinhasTurmas, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.PlanejamentoSemanal)
            {
                string valor1;
                string valor2;
                string valor3;
                string esc_id;
                string uni_id;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                if (!string.IsNullOrEmpty(valor1))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor1);
                    SelecionarEscola(UCComboUAEscola1.FiltroEscola);
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }
                else if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                {
                    UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor1);
                UCCCalendario1.Valor = Convert.ToInt32(valor1);
                UCCCalendario1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                UCComboCursoCurriculo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                UCComboCursoCurriculo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tci_id", out valor1);
                UCComboTipoCiclo.Tci_id = Convert.ToInt32(valor1);
                UCComboTipoCiclo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                if (Convert.ToInt32(valor3) > 0)
                    UCCCurriculoPeriodo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2), Convert.ToInt32(valor3) };

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor1);
                txtCodigoTurma.Text = valor1;
                txtCodigoTurma.Focus();

                Pesquisar();
            }
            else
            {
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Retorna o id da div que possui a aba.
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public string RetornaTabID(int tpc_id)
        {
            return "divTabs-" + tpc_id.ToString();
        }

        /// <summary>
        /// Redireciona pro Diário de classe, criando as variáveis de sessão necessárias.
        /// </summary>
        /// <param name="grid"></param>
        private void RedirecionaDiarioClasse(GridView grid)
        {
            // Cria variáveis na sessão
            Session["tud_id"] = grid.DataKeys[grid.EditIndex].Values["tud_id"].ToString();
            Session["tdt_posicao"] = grid.DataKeys[grid.EditIndex].Values["tdt_posicao"].ToString();
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

            bool disciplinaEspecial = Convert.ToBoolean(grid.DataKeys[grid.EditIndex].Values["tud_disciplinaEspecial"].ToString());

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ||
                (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 && !disciplinaEspecial))
            {
                Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocencia", "$(document).ready(function() { $('#divSelecionaTipoDocencia').dialog('open'); });", true);
            }
        }

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno(long tud_id, int esc_id, int uni_id, long tur_id, bool tud_naoLancarNota, bool tud_naoLancarFrequencia, int cal_id, string EscolaTurmaDisciplina, int tdt_posicao, DateTime tur_dataEncerramento, string tciIds, byte tur_tipo, long tud_idAluno, long tur_idNormal)
        {
            Session.Remove("tud_id");
            Session.Remove("tdt_posicao");
            Session.Remove("PaginaRetorno");
            Session.Remove("TudIdCompartilhada");

            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            byte opcaoAba = Convert.ToByte(eOpcaoAbaMinhasTurmas.DiarioClasse);

            List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
            Struct_CalendarioPeriodos periodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();

            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Tud_idRetorno_ControleTurma", tud_id.ToString());
            listaDados.Add("Edit_tdt_posicao", tdt_posicao.ToString());
            listaDados.Add("Edit_esc_id", esc_id.ToString());
            listaDados.Add("Edit_uni_id", uni_id.ToString());
            listaDados.Add("Edit_tur_id", tur_id.ToString());
            listaDados.Add("Edit_tud_naoLancarNota", tud_naoLancarNota.ToString());
            listaDados.Add("Edit_tud_naoLancarFrequencia", tud_naoLancarFrequencia.ToString());
            listaDados.Add("Edit_tur_dataEncerramento", tur_dataEncerramento.ToString());
            listaDados.Add("Edit_tpc_id", periodo.tpc_id.ToString());
            listaDados.Add("Edit_tpc_ordem", periodo.tpc_ordem.ToString());
            listaDados.Add("Edit_cal_id", cal_id.ToString());
            listaDados.Add("TextoTurmas", EscolaTurmaDisciplina);
            listaDados.Add("OpcaoAbaAtual", opcaoAba.ToString());
            listaDados.Add("Edit_tciIds", tciIds);
            listaDados.Add("Edit_tur_tipo", tur_tipo.ToString());
            listaDados.Add("Edit_tud_idAluno", tud_idAluno.ToString());
            listaDados.Add("Edit_tur_idNormal", tur_idNormal.ToString());
            listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/Busca.aspx");

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
            Session["Historico"] = false;
        }

        /// <summary>
        /// Colocar a turma/disciplina selecionada no historico na sessão.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdt_posicao"></param>
        private void CarregaSessionHistorico(string tud_id, string tdt_posicao)
        {
            Session.Remove("DadosPaginaRetorno");
            Session.Remove("VS_DadosTurmas");
            Session.Remove("TudIdCompartilhada");

            // Cria variáveis na sessão
            Session["tud_id"] = tud_id;
            Session["tdt_posicao"] = tdt_posicao;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";
            Session["Historico"] = true;
        }

        /// <summary>
        /// Redireciona para a página informada dentro da pasta Controle de Turmas.
        /// </summary>
        /// <param name="pagina">Página</param>
        private void RedirecionaTela(string pagina)
        {
            Response.Redirect("~/Academico/ControleTurma/" + pagina + ".aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redireciona para a tela de atribuição de docentes.
        /// </summary>
        private void RedirecionaTelaAtribuicaoDocente()
        {
            Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoDocentes/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redireciona para uma das telas do Minhas Turmas, de acordo com o evento do grid.
        /// </summary>
        /// <param name="nomeTela"></param>
        /// <param name="nomePagina"></param>
        /// <param name="grid"></param>
        /// <param name="indice"></param>
        /// <param name="validarDisciplinaCompartilhada"></param>
        private void RedirecionaTelaMinhasTurmas(string nomeTela, string nomePagina, GridView grid, string indice, bool validarDisciplinaCompartilhada)
        {
            try
            {
                int index = 0;
                if (int.TryParse(indice, out index))
                {
                    index = index % grid.PageSize;
                    grid.EditIndex = index;
                    if (grid != null)
                    {
                        long tud_id = 0;
                        long tur_id = 0;
                        int esc_id = 0;
                        int uni_id = 0;
                        int cal_id = 0;
                        bool tud_naoLancarNota = false;
                        bool tud_naoLancarFrequencia = false;
                        string EscolaTurmaDisciplina = string.Empty;
                        byte posicao;
                        byte tud_tipo = 0;
                        byte tur_tipo = 0;
                        DateTime tur_dataEncerramento = new DateTime();
                        string tciIds = string.Empty;
                        bool disciplinaAtiva = true;
                        long tud_idAluno = 0;
                        long tur_idNormal = 0;

                        Int64.TryParse(grid.DataKeys[index].Values["tud_id"].ToString(), out tud_id);
                        Int64.TryParse(grid.DataKeys[index].Values["tur_id"].ToString(), out tur_id);
                        Int32.TryParse(grid.DataKeys[index].Values["esc_id"].ToString(), out esc_id);
                        Int32.TryParse(grid.DataKeys[index].Values["uni_id"].ToString(), out uni_id);
                        Int32.TryParse(grid.DataKeys[index].Values["cal_id"].ToString(), out cal_id);
                        Boolean.TryParse(grid.DataKeys[index].Values["tud_naoLancarNota"].ToString(), out tud_naoLancarNota);
                        Boolean.TryParse(grid.DataKeys[index].Values["tud_naoLancarFrequencia"].ToString(), out tud_naoLancarFrequencia);
                        EscolaTurmaDisciplina = grid.DataKeys[index].Values["EscolaTurmaDisciplina"].ToString();
                        byte.TryParse(grid.DataKeys[index].Values["tdt_posicao"].ToString(), out posicao);
                        tud_tipo = Convert.ToByte(grid.DataKeys[index].Values["tud_tipo"]);
                        DateTime.TryParse(grid.DataKeys[index].Values["tur_dataEncerramento"].ToString(), out tur_dataEncerramento);
                        tciIds = grid.DataKeys[index].Values["tciIds"].ToString();
                        Byte.TryParse(grid.DataKeys[index].Values["tur_tipo"].ToString(), out tur_tipo);
                        Int64.TryParse(grid.DataKeys[index].Values["tud_idAluno"].ToString(), out tud_idAluno);
                        Int64.TryParse(grid.DataKeys[index].Values["tur_idNormal"].ToString(), out tur_idNormal);

                        CarregaSessionPaginaRetorno(tud_id, esc_id, uni_id, tur_id, tud_naoLancarNota, tud_naoLancarFrequencia, cal_id, EscolaTurmaDisciplina, posicao, tur_dataEncerramento, tciIds, tur_tipo, tud_idAluno, tur_idNormal);

                        Boolean.TryParse(grid.DataKeys[index].Values["disciplinaAtiva"].ToString(), out disciplinaAtiva);
                        if (!validarDisciplinaCompartilhada || disciplinaAtiva || !VS_visaoDocente)
                        {
                            if (!validarDisciplinaCompartilhada || tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                            {
                                if (VS_visaoDocente)
                                {
                                    RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                                    VS_rptTurmasIndice = itemTurma.ItemIndex;
                                }

                                RedirecionaTela(nomePagina);
                            }
                            else
                            {
                                VS_TelaRedirecionar = nomePagina;
                                grid.EditIndex = -1;
                            }
                        }
                        else
                        {
                            RedirecionaTelaAtribuicaoDocente();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", nomeTela), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void grvTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && PreCarregarFechamentoCache)
            {
                long tur_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tur_id"));
                int fav_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "fav_id"));
                int esc_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "esc_id"));
                int cal_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cal_id"));
                byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tur_tipo"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tud_tipo"));
                bool tud_disciplinaEspecial = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "tud_disciplinaEspecial"));
                byte tdt_posicao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tdt_posicao"));

                ACA_FormatoAvaliacao entityFormatoAvaliacao = new ACA_FormatoAvaliacao { fav_id = fav_id };
                ACA_FormatoAvaliacaoBO.GetEntity(entityFormatoAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacao = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacaoDocente = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idDocente };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacaoDocente);

                ImageButton btnFechamento = (ImageButton)e.Row.FindControl("btnFechamento");
                if (btnFechamento != null)
                {
                    btnFechamento.OnClientClick = "CarregarCacheEfetivacao(this);";
                    btnFechamento.CommandName = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "FechamentoAutomatico" : "Fechamento";
                }

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                // Valor do conceito global ou por disciplina.
                string valorMinimo = tud_id > 0 ?
                    entityFormatoAvaliacao.valorMinimoAprovacaoPorDisciplina :
                    entityFormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = Convert.ToDouble(valorMinimo.Replace(',', '.'));
                }
                else if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(entityEscalaAvaliacao.esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                bool incluirFinal = entityFormatoAvaliacao.fav_avaliacaoFinalAnalitica;

                List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);

                List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(cal_id, ApplicationWEB.AppMinutosCacheCurto);

                List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = tabelaPeriodos.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();

                int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
                int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

                //Busca o bimestre corrente
                Struct_CalendarioPeriodos periodoCorrente = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();
                int tpc_id = periodoCorrente.tpc_id;
                int tpc_ordem = periodoCorrente.tpc_ordem;

                if (tpc_id <= 0 && !incluirFinal)
                {
                    //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                    Struct_CalendarioPeriodos proximoPeriodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                    tpc_id = proximoPeriodo.tpc_id;
                    tpc_ordem = proximoPeriodo.tpc_ordem;

                    if (tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado então seleciona o ultimo
                        tpc_id = tpc_idUltimoPerido;
                        tpc_ordem = tpc_ordemUltimoPeriodo;
                    }
                }

                if (tpc_id >= 0 && incluirFinal)
                {
                    if (tpc_id == tpc_idUltimoPerido)
                    {
                        // Se for o ultimo periodo e a avaliacao final estiver aberta,
                        // selecionar a avaliacao final
                        List<ACA_Evento> listaEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);
                        if (listaEventos.Exists(p => p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                        {
                            tpc_id = tpc_ordem - 1;
                        }
                    }

                    if (tpc_id == 0)
                    {
                        //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                        Struct_CalendarioPeriodos proximoPeriodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                        tpc_id = proximoPeriodo.tpc_id;
                        tpc_ordem = proximoPeriodo.tpc_ordem;

                        if (tpc_id <= 0)
                        {
                            //Se não tem bimestre selecionado então seleciona o final
                            tpc_id = tpc_ordem = -1;
                        }
                    }
                }

                int ava_id = -1;
                byte ava_tipo = 0;

                if (tpc_id > 0)
                {
                    List<ACA_Avaliacao> listaAvaliacao = ACA_AvaliacaoBO.ConsultaPor_Periodo_Relacionadas(fav_id, tpc_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (listaAvaliacao.Any())
                    {
                        ava_id = listaAvaliacao.First().ava_id;
                        ava_tipo = (byte)listaAvaliacao.First().ava_tipo;
                    }
                }
                else
                {
                    List<ACA_Avaliacao> listaAvaliacao = ACA_AvaliacaoBO.SelectAvaliacaoFinal_PorFormato(fav_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (listaAvaliacao.Any(p => p.ava_tipo == (byte)AvaliacaoTipo.Final))
                    {
                        ava_id = listaAvaliacao.Find(p => p.ava_tipo == (byte)AvaliacaoTipo.Final).ava_id;
                        ava_tipo = (byte)AvaliacaoTipo.Final;
                    }
                }

                HiddenField hdn = (HiddenField)e.Row.FindControl("hdnTudId");
                hdn.Value = tud_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTurId");
                hdn.Value = tur_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTpcId");
                hdn.Value = tpc_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnAvaId");
                hdn.Value = ava_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnFavId");
                hdn.Value = fav_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoAvaliacao");
                hdn.Value = ava_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnEsaId");
                hdn.Value = entityEscalaAvaliacao.esa_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoEscala");
                hdn.Value = entityEscalaAvaliacao.esa_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoEscalaDocente");
                hdn.Value = entityEscalaAvaliacaoDocente.esa_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnNotaMinima");
                hdn.Value = notaMinimaAprovacao.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnParecerMinimo");
                hdn.Value = ordemParecerMinimo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoLancamento");
                hdn.Value = entityFormatoAvaliacao.fav_tipoLancamentoFrequencia.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnCalculoQtAulasDadas");
                hdn.Value = entityFormatoAvaliacao.fav_calculoQtdeAulasDadas.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTurTipo");
                hdn.Value = tur_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnCalId");
                hdn.Value = cal_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTudTipo");
                hdn.Value = tud_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTpcOrdem");
                hdn.Value = tpc_ordem.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnVariacao");
                hdn.Value = entityFormatoAvaliacao.fav_variacao.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoDocente");
                hdn.Value = (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            (byte)ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo) : (byte)0).ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnDisciplinaEspecial");
                hdn.Value = tud_disciplinaEspecial ? "true" : "false";

                hdn = (HiddenField)e.Row.FindControl("hdnFechamentoAutomatico");
                hdn.Value = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "true" : "false";
            }
        }

        /// <summary>
        /// Evento generico utilizando no grvTurma (docente) e grvTurmas (admin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMinhasTurmas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            VS_ChavesRedirecionaDiario = new long[] { -1, -1 };

            GridView grid = (GridView)sender;
            switch (e.CommandName)
            {
                #region Planejamento

                case "Planejamento":
                    {
                        RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Planejamento

                default:
                    {
                        break;
                    }
            }
        }

        protected void grvTurmas_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            
            if (!VS_visaoDocente)
            {
                UCTotalRegistros.Total = TUR_TurmaBO.GetTotalRecords();

                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(grvTurmas);

                if ((!string.IsNullOrEmpty(grvTurmas.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas))
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                    if (filtros.ContainsKey("VS_Ordenacao"))
                    {
                        filtros["VS_Ordenacao"] = grvTurmas.SortExpression;
                    }
                    else
                    {
                        filtros.Add("VS_Ordenacao", grvTurmas.SortExpression);
                    }

                    if (filtros.ContainsKey("VS_SortDirection"))
                    {
                        filtros["VS_SortDirection"] = grvTurmas.SortDirection.ToString();
                    }
                    else
                    {
                        filtros.Add("VS_SortDirection", grvTurmas.SortDirection.ToString());
                    }

                    __SessionWEB.BuscaRealizada = new BuscaGestao
                    {
                        PaginaBusca = PaginaGestao.MinhasTurmas
                        ,
                        Filtros = filtros
                    };
                }
            }
            
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void lkEspecial_Click(object sender, EventArgs e)
        {
            Session["tdt_posicao"] = (byte)EnumTipoDocente.Especial;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

            Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void rptTurmas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int indice = e.Item.ItemIndex;
                int esc_id = 0;
                int cal_id = 0;

                // Id Escola
                HiddenField hdnId = e.Item.FindControl("hdnEscola") as HiddenField;
                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                {
                    esc_id = Convert.ToInt32(hdnId.Value);
                }

                // Calendario
                HiddenField hdnCalendario = e.Item.FindControl("hdnCalendario") as HiddenField;
                if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                {
                    cal_id = Convert.ToInt32(hdnCalendario.Value);
                }

                int uni_id = 0;
                HiddenField hdnUnidadeEscola = e.Item.FindControl("hdnUnidadeEscola") as HiddenField;
                if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                {
                    uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                }

                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);

                GridView grdVw = e.Item.FindControl("grvTurma") as GridView;
                grdVw.DataSource = dadosTurmasAtivas;
                grdVw.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView grid = ((GridView)(sender));
                if (grid != null)
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                    RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                    int indice = itemTurma.ItemIndex;
                    int esc_id = 0;
                    int cal_id = 0;

                    // Id Escola
                    HiddenField hdnId = itemTurma.FindControl("hdnEscola") as HiddenField;
                    if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                    {
                        esc_id = Convert.ToInt32(hdnId.Value);
                    }

                    // Calendario
                    HiddenField hdnCalendario = itemTurma.FindControl("hdnCalendario") as HiddenField;
                    if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                    {
                        cal_id = Convert.ToInt32(hdnCalendario.Value);
                    }

                    grid.PageIndex = e.NewPageIndex;

                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                    List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);

                    grid.DataSource = dadosTurmasAtivas;
                    grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}