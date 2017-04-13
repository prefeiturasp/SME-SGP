using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.Combos.Novos;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class MinhaEscolaGestor : MotherPageLogadoCompressedViewState
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

        #region Constantes

        private const int grvTurma_ColunaListao = 4;
        private const int grvTurma_ColunaFrequencia = 5;
        private const int grvTurma_ColunaAvaliacao = 6;
        private const int grvTurma_ColunaEfetivacao = 7;
        private const int grvTurma_ColunaAlunos = 8;

        #endregion Constantes

        #region Propriedades

        private string periodosEfetivados;
        private int totalPrevistas = 0, totalDadas = 0, totalRepostas = 0;

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

        private long tur_idAula;
        private int cal_idAula;
        private bool mostraSalvar;

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
        /// ViewState que armazena a lista de pendências das disciplina.
        /// </summary>
        private Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>> VS_listaPendencias
        {
            get
            {
                return (Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>>)(ViewState["VS_listaPendencias"] ?? (ViewState["VS_listaPendencias"] = new Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>>()));
            }

            set
            {
                ViewState["VS_listaPendencias"] = value;
            }
        }

        private int indiceRptTurmas = -1;

        #endregion

        #region Delegates

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }
                else
                {
                    rptTurmas.DataSource = null;
                    rptTurmas.DataBind();


                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                    foreach (RepeaterItem itemTurma in rptTurmas.Items)
                    {
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

                        int uni_id = 0;
                        HiddenField hdnUnidadeEscola = itemTurma.FindControl("hdnUnidadeEscola") as HiddenField;
                        if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                        {
                            uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                        }

                        // Verifico se existe evento de fechamento vigente para a escola e calendario,
                        // para mostrar o botao de atualizar pendencias.
                        string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
                        string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
                        List<ACA_Evento> lstEventosEscola = ACA_EventoBO.GetEntity_Efetivacao_ListPorPeriodo(cal_id, -1, Guid.Empty, esc_id, uni_id, ent_id);

                        if (lstEventosEscola.Any(p => Convert.ToString(p.tev_id) == valor || Convert.ToString(p.tev_id) == valorFinal))
                        {
                            CarregarPendencias(itemTurma, true);
                        }
                    }
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de curso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCCursoCurriculo1_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCurso = (DropDownList)sender;
                PesquisaTurmasFiltros(ddlCurso.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de período
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCCurriculoPeriodo1_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlPeriodo = (DropDownList)sender;
                PesquisaTurmasFiltros(ddlPeriodo.ClientID);

                //UCCCurriculoPeriodo combo = (UCCCurriculoPeriodo)sender;
                //DropDownList combo = (DropDownList)sender;
                //RepeaterItem item = (RepeaterItem)combo.NamingContainer;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de disciplina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCComboTipoDisciplina1_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlDisciplina = (DropDownList)sender;
                PesquisaTurmasFiltros(ddlDisciplina.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void uccTurmaDisciplina_IndexChanged()
        {
            try
            {
                string[] valor = uccTurmaDisciplina.Valor.Split(';');

                if (valor.Length > 2)
                {
                    long tur_id = Convert.ToInt64(valor[0]);
                    long tud_id = Convert.ToInt64(valor[1]);
                    int cal_id = Convert.ToInt32(valor[2]);
                    byte tdt_posicaoLocal = 1;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        tdt_posicaoLocal = Convert.ToByte(valor[3]);
                    }

                    if (tdt_posicaoLocal > 0)
                    {
                        tur_idAula = tur_id;
                        cal_idAula = cal_id;

                        totalPrevistas = 0;
                        mostraSalvar = false;
                        periodosEfetivados = "";
                        grvPeriodosAulas.DataSource = ACA_CalendarioPeriodoBO.Seleciona_QtdeAulas_TurmaDiscplina(tur_id, tud_id, cal_id, tdt_posicaoLocal, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                        //tdt_posicao = tdt_posicao == 0 ? tdt_posicaoLocal : tdt_posicao;
                        grvPeriodosAulas.DataBind();
                        btnSalvarAulasPrevistas.Visible = mostraSalvar;

                        if (!string.IsNullOrEmpty(periodosEfetivados))
                        {
                            lblPeriodoEfetivado.Visible = true;
                            lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.MinhaEscolaGestor.AulasPrevistas.MensagemEfetivado").ToString(),
                                                                             UtilBO.TipoMensagem.Informacao);
                        }

                        //if (VS_ChavesRedirecionaDiario.Length > 0 && VS_ChavesRedirecionaDiario[0] > 0)
                        //{
                        // Se está alimentada a propriedade de chaves para redirecionar pro Diário de classe, altera
                        // quando o usuário muda o combo.
                        //VS_ChavesRedirecionaDiario = new long[] { tud_id, tdt_posicaoLocal };
                        //}
                    }

                    if (grvPeriodosAulas.Rows.Count <= 0)
                    {
                        lblMensagemIndicador.Text = UtilBO.GetErroMessage("Nenhum aluno matriculado para os períodos do calendário.", UtilBO.TipoMensagem.Alerta);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemIndicador.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnIndicadores.Update();
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
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaMinhaEscolaGestor.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;

                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    UCComboUAEscola1.Inicializar();
                    if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola1_IndexChangedUA();
                    }

                    if (UCComboUAEscola1.Esc_ID > 0 && UCComboUAEscola1.QuantidadeItemsComboEscolas == 2)
                    {
                        divFiltros.Visible = false;
                    }
                    else
                    {
                        if (UCComboUAEscola1.QuantidadeItemsComboEscolas <= 1 && UCComboUAEscola1.QuantidadeItemsComboUAs <= 1)
                        {
                            lblMensagem.Text = UtilBO.GetErroMessage("Usuário não possui nenhuma escola vinculada.", UtilBO.TipoMensagem.Alerta);
                        }

                        divFiltros.Visible = true;
                    }
                    VerificarBusca();

                    CarregaRepeaterEscolas();

                    hdnProcessarFilaFechamentoTela.Value = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ? "true" : "false";
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            #region Associando Delegates

            UCComboUAEscola1.IndexChangedUA += UCComboUAEscola1_IndexChangedUA;
            uccTurmaDisciplina.IndexChanged += uccTurmaDisciplina_IndexChanged;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            AssociaDelegatesRepeater();

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

        #region Eventos

        protected void rptTurmas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
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

                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
                List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

                dados.All(p =>
                {
                    lista.AddRange(p.Turmas.Where(t => t.esc_id == esc_id && t.cal_id == cal_id));
                    return true;
                });

                lista = lista.GroupBy(x => x.tud_id).Select(g => g.First()).ToList();

                var ciclos = lista.Where(x => x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo).GroupBy(x => x.tci_id).Select(g => g.First()).OrderBy(y => y.tci_ordem);
                indiceRptTurmas = e.Item.ItemIndex;

                if (ciclos.Count<Struct_MinhasTurmas.Struct_Turmas>() > 0)
                {
                    Repeater rptCiclos = e.Item.FindControl("rptCiclos") as Repeater;
                    if (rptCiclos != null)
                    {
                        rptCiclos.DataSource = ciclos;
                        rptCiclos.DataBind();
                    }

                    Repeater rptCiclosAbas = e.Item.FindControl("rptCiclosAbas") as Repeater;
                    if (rptCiclosAbas != null)
                    {
                        rptCiclosAbas.DataSource = ciclos;
                        rptCiclosAbas.DataBind();
                    }
                }

                // Carrega turmas extintas
                GridView grvTurmasExtintas = e.Item.FindControl("grvTurmasExtintas") as GridView;
                if (grvTurmasExtintas != null)
                {
                    HiddenField hdnIndiceRptTurmas = (HiddenField)e.Item.FindControl("hdnIndiceRptTurmas");
                    if (hdnIndiceRptTurmas != null)
                    {
                        hdnIndiceRptTurmas.Value = indiceRptTurmas.ToString();
                    }

                    var turmasExtintas = lista.Where(x => x.tur_situacao == (byte)TUR_TurmaSituacao.Extinta).ToList();
                    grvTurmasExtintas.DataSource = turmasExtintas;
                    grvTurmasExtintas.DataBind();

                    Panel fdsTurmasExtintas = (Panel)e.Item.FindControl("fdsTurmasExtintas");
                    HtmlGenericControl liTurmasEx = (HtmlGenericControl)e.Item.FindControl("liTurmasEx");
                    if (fdsTurmasExtintas != null && liTurmasEx != null)
                    {
                        fdsTurmasExtintas.Visible = liTurmasEx.Visible = (grvTurmasExtintas.Rows.Count > 0);
                    }
                }

                // Carrega Projetos / Atividades Complementares
                GridView grvProjetosRecParalela = e.Item.FindControl("grvProjetosRecParalela") as GridView;
                if (grvProjetosRecParalela != null)
                {
                    HiddenField hdnIndiceRptTurmas = (HiddenField)e.Item.FindControl("hdnIndiceRptTurmasRec");
                    if (hdnIndiceRptTurmas != null)
                    {
                        hdnIndiceRptTurmas.Value = indiceRptTurmas.ToString();
                    }

                    var turmasRecuperacao = lista.Where(x => x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                    grvProjetosRecParalela.DataSource = turmasRecuperacao;
                    grvProjetosRecParalela.DataBind();

                    Panel fdsProjetos = (Panel)e.Item.FindControl("fdsProjetos");
                    HtmlGenericControl liProjetos = (HtmlGenericControl)e.Item.FindControl("liProjetos");
                    if (fdsProjetos != null && liProjetos != null)
                    {
                        fdsProjetos.Visible = liProjetos.Visible = (grvProjetosRecParalela.Rows.Count > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptCiclosAbas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                int ciclo = 0;
                int esc_id = 0;
                int uni_id = 0;
                int cur_id = 0;
                int crr_id = 0;
                bool carregarGridTurmas = false;
                int cal_id = 0;

                // Id Escola
                HiddenField hdnId = e.Item.Parent.Parent.FindControl("hdnEscola") as HiddenField;
                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                {
                    esc_id = Convert.ToInt32(hdnId.Value);
                }

                // Id Escola Unidade
                HiddenField hdnUnidadeEscola = e.Item.Parent.Parent.FindControl("hdnUnidadeEscola") as HiddenField;
                if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                {
                    uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                }

                // Calendario
                HiddenField hdnCalendario = e.Item.Parent.Parent.FindControl("hdnCalendario") as HiddenField;
                if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                {
                    cal_id = Convert.ToInt32(hdnCalendario.Value);
                }

                HiddenField hdnCiclo = e.Item.FindControl("hdnCiclo") as HiddenField;
                if (hdnCiclo != null && !string.IsNullOrEmpty(hdnCiclo.Value))
                {
                    ciclo = Convert.ToInt32(hdnCiclo.Value);
                }

                UCCCursoCurriculo UCCurso = e.Item.FindControl("UCCCursoCurriculo1") as UCCCursoCurriculo;
                if (UCCurso != null)
                {
                    UCCurso.CarregarPorEscolaCalendarioTipoCiclo(esc_id, uni_id, cal_id, ciclo);

                    cur_id = UCCurso.Valor[0];
                    crr_id = UCCurso.Valor[1];

                    if (cur_id > 0 && crr_id > 0)
                    {
                        UCCurso.Visible = false;
                        carregarGridTurmas = true;
                    }
                }

                UCCCurriculoPeriodo UCPeriodo = e.Item.FindControl("UCCCurriculoPeriodo1") as UCCCurriculoPeriodo;
                if (UCPeriodo != null)
                {
                    if (cur_id > 0 && crr_id > 0)
                    {
                        UCPeriodo.CarregarPorCursoCurriculoTipoCiclo(cur_id, crr_id, ciclo);
                    }
                    else
                    {
                        UCPeriodo.PermiteEditar = false;
                    }
                    //UCPeriodo.IndexChanged += UCCCurriculoPeriodo1_IndexChanged;
                    //UCPeriodo.IndexChanged_Sender += UCCCurriculoPeriodo1_IndexChanged;
                }

                WebControls_Combos_UCComboTipoDisciplina UCTipoDisciplina = e.Item.FindControl("UCComboTipoDisciplina1") as WebControls_Combos_UCComboTipoDisciplina;
                if (UCTipoDisciplina != null)
                {
                    UCTipoDisciplina.Titulo = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString();
                    if (cur_id > 0)
                    {
                        UCTipoDisciplina.CarregarTipoDisciplinaPorCursoTipoCiclo(cur_id, ciclo, esc_id);
                    }
                    else
                    {
                        UCTipoDisciplina.CarregarTipoDisciplina();
                        UCTipoDisciplina.PermiteEditar = false;
                        UCTipoDisciplina.Valor = -1;
                    }
                }

                HiddenField hdnIndiceRptTurmas = (HiddenField)e.Item.FindControl("hdnIndiceRptTurmas");
                if (hdnIndiceRptTurmas != null)
                {
                    hdnIndiceRptTurmas.Value = indiceRptTurmas.ToString();
                }

                #region Carrega Grids Turmas

                if (carregarGridTurmas)
                {
                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
                    List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

                    dados.All(p =>
                    {
                        lista.AddRange(p.Turmas.Where(t => t.esc_id == esc_id && t.cal_id == cal_id));
                        return true;
                    });

                    lista = lista.GroupBy(x => x.tud_id).Select(g => g.First()).ToList();

                    GridView grdVw = e.Item.FindControl("grvTurma") as GridView;
                    if (grdVw != null)
                    {
                        var turmas = lista.Where(x => x.tci_id == ciclo && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                        grdVw.Visible = true;
                        grdVw.DataSource = turmas;
                        grdVw.DataBind();
                    }
                }
                else
                {
                    GridView grdVw = e.Item.FindControl("grvTurma") as GridView;
                    if (grdVw != null)
                    {
                        grdVw.Visible = false;
                    }
                }

                #endregion Carrega Grids Turmas
            }
        }

        /// <summary>
        /// Evento generico utilizando no grvTurma (docente) e grvTurmas (admin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMinhasTurmas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            switch (e.CommandName)
            {
                case "Indicadores":
                    {
                        try
                        {
                            //tdt_posicao = 0;
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int esc_id = Convert.ToInt32(args[0]);
                                long tur_id = Convert.ToInt64(args[1]);
                                long tud_id = Convert.ToInt64(args[2]);
                                int cal_id = Convert.ToInt32(args[3]);
                                //tdt_posicao = Convert.ToByte(args[4]);
                                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                                //lblMensagemBloqueio.Visible = false;
                                CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id);
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }
                case "Planejamento":
                    {
                        RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                case "DiarioClasse":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_DiarioClasse").ToString(), "DiarioClasse", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                case "Listao":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                case "Fechamento":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Efetivacao", grid, e.CommandArgument.ToString(), false);
                        break;
                    }
                case "FechamentoAutomatico":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Fechamento", grid, e.CommandArgument.ToString(), false);
                        break;
                    }
                case "Alunos":
                    {
                        RedirecionaTelaMinhasTurmas("Alunos", "Alunos", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                case "Frequencia":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString(), "Frequencia", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                case "Avaliacao":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString(), "Avaliacao", grid, e.CommandArgument.ToString(), true);
                        break;
                    }
                
                #region Pendência no fechamento

                case "PendenciaFechamento":
                case "PendenciaFechamentoAutomatico":
                    {
                        HiddenField hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmas");
                        string chavePendencia = string.Empty;
                        if (hdnIndiceRptTurmas != null)
                        {
                            chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                        }
                        else
                        {
                            hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmasRec");
                            if (hdnIndiceRptTurmas != null)
                            {
                                chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                            }
                        }

                        long tud_id = 0;
                        byte tud_tipo = 0;
                        int index = Convert.ToInt32(e.CommandArgument.ToString());

                        Int64.TryParse(grid.DataKeys[index].Values["tud_id"].ToString(), out tud_id);
                        tud_tipo = Convert.ToByte(grid.DataKeys[index].Values["tud_tipo"]);

                        REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia;
                        if (tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            pendencia = VS_listaPendencias[chavePendencia].FindAll
                            (
                                p =>
                                (
                                    p.tud_id == tud_id
                                    &&
                                    (
                                        p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                        || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                        || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                        || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                        || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                    )
                                )
                            )
                            .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).FirstOrDefault();
                        }
                        else
                        {
                            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                                __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                            if (controleOrdemDisciplinas)
                            {
                                pendencia = VS_listaPendencias[chavePendencia].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_idRegencia == tud_id
                                        &&
                                        (
                                            p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                        )
                                    )
                                )
                                .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).ThenBy(p => p.tds_ordem).FirstOrDefault();
                            }
                            else
                            {
                                pendencia = VS_listaPendencias[chavePendencia].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_idRegencia == tud_id
                                        &&
                                        (
                                            p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                        )
                                    )
                                )
                                .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).ThenBy(p => p.tud_nome).FirstOrDefault();
                            }
                        }

                        if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota)
                        {
                            // Redireciona para o Listão de Avaliação
                            RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);

                        }
                        else if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula)
                        {
                            // Redireciona para o Diário de Classe
                            RedirecionaTelaMinhasTurmas("Diário de Classe", "DiarioClasse", grid, e.CommandArgument.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id);

                        }
                        else if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                            || pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                            || pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer)
                        {
                            // Redireciona para o Fechamento final
                            RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), e.CommandName == "PendenciaFechamentoAutomatico" ? "Fechamento" : "Efetivacao", grid, e.CommandArgument.ToString(), false, pendencia.tipoPendencia);
                        }

                        break;
                    }

                #endregion Pendência no fechamento

                #region Pendência no planejamento

                case "PendenciaPlanejamento":
                    {
                        HiddenField hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmas");
                        string chavePendencia = string.Empty;
                        if (hdnIndiceRptTurmas != null)
                        {
                            chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                        }
                        else
                        {
                            hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmasRec");
                            if (hdnIndiceRptTurmas != null)
                            {
                                chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                            }
                        }

                        long tud_id = 0;
                        byte tud_tipo = 0;
                        int index = Convert.ToInt32(e.CommandArgument.ToString());

                        Int64.TryParse(grid.DataKeys[index].Values["tud_id"].ToString(), out tud_id);
                        tud_tipo = Convert.ToByte(grid.DataKeys[index].Values["tud_tipo"]);

                        REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia;
                        if (tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            pendencia = VS_listaPendencias[chavePendencia].FindAll
                            (
                                p =>
                                (
                                    p.tud_id == tud_id
                                    && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                )
                            )
                            .OrderBy(p => p.tpc_ordem).FirstOrDefault();
                        }
                        else
                        {
                            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                                __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                            if (controleOrdemDisciplinas)
                            {
                                pendencia = VS_listaPendencias[chavePendencia].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_idRegencia == tud_id
                                        && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                    )
                                )
                                .OrderBy(p => p.tpc_ordem).ThenBy(p => p.tds_ordem).FirstOrDefault();
                            }
                            else
                            {
                                pendencia = VS_listaPendencias[chavePendencia].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_idRegencia == tud_id
                                        && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                    )
                                )
                                .OrderBy(p => p.tpc_ordem).ThenBy(p => p.tud_nome).FirstOrDefault();
                            }
                        }

                        if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento)
                        {
                            // Redireciona para o Planejamento anual
                            RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);
                        }

                        break;
                    }

                #endregion Pendência no planejamento

                #region Pendência no plano de aula

                case "PendenciaPlanoAula":
                    {
                        HiddenField hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmas");
                        string chavePendencia = string.Empty;
                        if (hdnIndiceRptTurmas != null)
                        {
                            chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                        }
                        else
                        {
                            hdnIndiceRptTurmas = (HiddenField)grid.Parent.FindControl("hdnIndiceRptTurmasRec");
                            if (hdnIndiceRptTurmas != null)
                            {
                                chavePendencia = rptTurmas.Items[Convert.ToInt32(hdnIndiceRptTurmas.Value)].ClientID;
                            }
                        }

                        long tud_id = 0;
                        int index = Convert.ToInt32(e.CommandArgument.ToString());

                        Int64.TryParse(grid.DataKeys[index].Values["tud_id"].ToString(), out tud_id);

                        REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia;
                        pendencia = VS_listaPendencias[chavePendencia].FindAll
                        (
                            p =>
                            (
                                p.tud_id == tud_id
                                && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula
                            )
                        )
                        .OrderBy(p => p.tpc_ordem).FirstOrDefault();

                        if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula)
                        {
                            // Redireciona para o Listão de plano de aula
                            RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id);
                        }

                        break;
                    }

                #endregion Pendência no plano de aula

                default:
                    {
                        break;
                    }
            }
        }

        protected void grvPeriodosAulas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox text = (TextBox)e.Row.FindControl("txtPrevistas");
                if (text != null)
                {
                    text.CssClass += " txtPrevistas";
                    totalPrevistas += int.Parse(string.IsNullOrEmpty(text.Text) ? "0" : text.Text);

                    int tpc_id = int.Parse(grvPeriodosAulas.DataKeys[e.Row.RowIndex]["tpc_id"].ToString());
                    DateTime cap_dataFim = DateTime.Parse(grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_dataFim"].ToString());
                    bool periodoEfetivado = VS_PeriodoEfetivado(tpc_id, cal_idAula, tur_idAula, cap_dataFim);

                    if (periodoEfetivado)
                        periodosEfetivados += string.IsNullOrEmpty(periodosEfetivados) ? grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_descricao"].ToString() :
                                              ", " + grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_descricao"].ToString();

                    if (periodoEfetivado)
                        text.Enabled = false;
                    else
                        mostraSalvar = true;
                }

                Label label = (Label)e.Row.FindControl("lblDadas");
                if (label != null)
                    totalDadas += int.Parse(label.Text);

                label = (Label)e.Row.FindControl("lblReposicoes");
                if (label != null)
                    totalRepostas += int.Parse(label.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label label = (Label)e.Row.FindControl("lblTotalPrevistas");
                if (label != null)
                    label.Text = totalPrevistas.ToString();
                label = (Label)e.Row.FindControl("lblTotalDadas");
                if (label != null)
                    label.Text = totalDadas.ToString();
                label = (Label)e.Row.FindControl("lblTotalReposicoes");
                if (label != null)
                    label.Text = totalRepostas.ToString();

                e.Row.CssClass = "gridRow";
            }

            // Acertar mensagem de obrigatoriedade dos validators.
            RequiredFieldValidator rvPrevistas = (RequiredFieldValidator)e.Row.FindControl("rvPrevistas");
            CompareValidator cvPrevistas = (CompareValidator)e.Row.FindControl("cvPrevistas");

            if (rvPrevistas != null)
            {
                rvPrevistas.ErrorMessage = string.Format("Quantidade de aulas previstas do {0} é obrigatório.", DataBinder.Eval(e.Row.DataItem, "cap_descricao").ToString());
            }

            if (cvPrevistas != null)
            {
                cvPrevistas.ErrorMessage = string.Format("Quantidade de aulas previstas do {0} deve ser maior que 0 (zero).", DataBinder.Eval(e.Row.DataItem, "cap_descricao").ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvTurmasExtintas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.CommandName == "DiarioClasse")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_DiarioClasse").ToString(), "DiarioClasse", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Indicadores")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(',');
                    if (args.Length > 4)
                    {
                        int esc_id = Convert.ToInt32(args[0]);
                        long tur_id = Convert.ToInt64(args[1]);
                        long tud_id = Convert.ToInt64(args[2]);
                        int cal_id = Convert.ToInt32(args[3]);
                        Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                        CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Planejamento")
            {
                RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Listao")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Fechamento")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Efetivacao", grid, e.CommandArgument.ToString(), false);
            }
            else if (e.CommandName == "FechamentoAutomatico")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Fechamento", grid, e.CommandArgument.ToString(), false);
            }
            else if (e.CommandName == "Alunos")
            {
                RedirecionaTelaMinhasTurmas("Alunos", "Alunos", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Frequencia")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString(), "Frequencia", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Avaliacao")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString(), "Avaliacao", grid, e.CommandArgument.ToString(), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvProjetosRecParalela_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.CommandName == "DiarioClasse")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_DiarioClasse").ToString(), "DiarioClasse", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Indicadores")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(',');
                    if (args.Length > 4)
                    {
                        int esc_id = Convert.ToInt32(args[0]);
                        long tur_id = Convert.ToInt64(args[1]);
                        long tud_id = Convert.ToInt64(args[2]);
                        int cal_id = Convert.ToInt32(args[3]);
                        Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                        CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Planejamento")
            {
                RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Listao")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Fechamento")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Efetivacao", grid, e.CommandArgument.ToString(), false);
            }
            else if (e.CommandName == "FechamentoAutomatico")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Fechamento", grid, e.CommandArgument.ToString(), false);
            }
            else if (e.CommandName == "Alunos")
            {
                RedirecionaTelaMinhasTurmas("Alunos", "Alunos", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Frequencia")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString(), "Frequencia", grid, e.CommandArgument.ToString(), true);
            }
            else if (e.CommandName == "Avaliacao")
            {
                RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString(), "Avaliacao", grid, e.CommandArgument.ToString(), true);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregaRepeaterEscolas();
                AssociaDelegatesRepeater();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("MinhaEscolaGestor.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnSalvarAulasPrevistas_Click(object sender, EventArgs e)
        {
            try
            {
                long tud_id = Int64.Parse(grvPeriodosAulas.DataKeys[0]["tud_id"].ToString());
                byte tud_tipo = Byte.Parse(grvPeriodosAulas.DataKeys[0]["tud_tipo"].ToString());
                bool fav_fechamentoAutomatico = bool.Parse(grvPeriodosAulas.DataKeys[0]["fav_fechamentoAutomatico"].ToString());
                List<TUR_TurmaDisciplinaAulaPrevista> aulasPrevistas = TUR_TurmaDisciplinaAulaPrevistaBO.SelecionaPorDisciplina(tud_id);

                totalPrevistas = 0;

                List<TUR_TurmaDisciplinaAulaPrevista> listaSalvar = new List<TUR_TurmaDisciplinaAulaPrevista>();
                List<TUR_TurmaDisciplinaAulaPrevista> listaProcessarPend = new List<TUR_TurmaDisciplinaAulaPrevista>();

                foreach (GridViewRow row in grvPeriodosAulas.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        int tpc_id = int.Parse(grvPeriodosAulas.DataKeys[row.RowIndex]["tpc_id"].ToString());

                        TextBox text = (TextBox)row.FindControl("txtPrevistas");
                        if (text != null)
                        {
                        if (string.IsNullOrEmpty(text.Text))
                            throw new ValidationException("Quantidade de aulas previstas é obrigatório.");

                        int qtAulasPrevistas = int.Parse(text.Text);
                        if (qtAulasPrevistas < 1)
                            throw new ValidationException("Quantidade de aulas previstas deve ser maior que 0.");

                        TUR_TurmaDisciplinaAulaPrevista aulaPrevista = aulasPrevistas.Find(p => p.tpc_id == tpc_id);
                        if (aulaPrevista == null)
                        {
                            aulaPrevista = new TUR_TurmaDisciplinaAulaPrevista();

                            // Seta os dados para uma nova insercao.
                            aulaPrevista.tud_id = tud_id;
                            aulaPrevista.tpc_id = tpc_id;
                                aulaPrevista.tap_registrosCorrigidos = false;
                        }
                            totalPrevistas += qtAulasPrevistas;

                            if ((aulaPrevista.tap_registrosCorrigidos && aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas) || !text.Enabled)
                                continue;

                        aulaPrevista.tap_registrosCorrigidos = aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas;

                            if (!(aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas || !text.Enabled))
                                listaProcessarPend.Add(aulaPrevista);

                        // Atualiza ou seta as aulas previstas.
                        aulaPrevista.tap_aulasPrevitas = qtAulasPrevistas;

                            aulaPrevista.tud_tipo = tud_tipo;

                        listaSalvar.Add(aulaPrevista);
                    }
                }
                }

                if (TUR_TurmaDisciplinaAulaPrevistaBO.SalvarAulasPrevistas(listaSalvar, listaProcessarPend, __SessionWEB.__UsuarioWEB.Usuario.ent_id, int.Parse(hdnEscId.Value), __SessionWEB.__UsuarioWEB.Docente.doc_id, fav_fechamentoAutomatico))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Aulas previstas | tud_id: " + tud_id);

                    if (grvPeriodosAulas.FooterRow != null)
                    {
                        Label label = (Label)grvPeriodosAulas.FooterRow.FindControl("lblTotalPrevistas");
                        if (label != null)
                            label.Text = totalPrevistas.ToString();
                    }

                    lblMensagemIndicador.Text = UtilBO.GetErroMessage("Dados salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    //// Recarrega o grid.
                    CarregaRepeaterEscolas();
                    AssociaDelegatesRepeater();
                }
                else
                {
                    lblMensagemIndicador.Text = UtilBO.GetErroMessage("Não foi possível salvar as aulas previstas.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemIndicador.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemIndicador.Text = UtilBO.GetErroMessage("Erro ao tentar salvar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnResultado.Update();
                lblMensagemIndicador.Focus();
            }
        }

        private void UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina(long tud_id)
        {
            // Atualiza a sessao com a disciplina compartilhada selecionada
            Session["TudIdCompartilhada"] = tud_id.ToString();
            //
            RedirecionaTela(VS_TelaRedirecionar);
        }

        protected void grvTurmas_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.TELA_UNICA_LANCAMENTO_FREQUENCIA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                grid.Columns[grvTurma_ColunaListao].Visible = true;
                grid.Columns[grvTurma_ColunaFrequencia].Visible = grid.Columns[grvTurma_ColunaAvaliacao].Visible = false;
            }
            else
            {
                grid.Columns[grvTurma_ColunaListao].Visible = false;
                grid.Columns[grvTurma_ColunaFrequencia].Visible = grid.Columns[grvTurma_ColunaAvaliacao].Visible = true;
            }

            grid.Columns[grvTurma_ColunaAlunos].Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grid.Columns[grvTurma_ColunaEfetivacao].Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        protected void lkbMensagemPendenciaFechamentoTurma_Click(object sender, EventArgs e)
        {        
            sGridTurmaEscolaCalendario grid = new sGridTurmaEscolaCalendario();
            try
            {
                RepeaterItem rptItemTurma = null;
                LinkButton lkb = (LinkButton)sender;
                if (lkb != null)
                {
                    RepeaterItem rptItemCiclo = (RepeaterItem)lkb.NamingContainer;
                    if (rptItemCiclo != null)
                    {
                        Repeater rptCiclo = (Repeater)rptItemCiclo.NamingContainer;
                        if (rptCiclo != null)
                        {
                            rptItemTurma = (RepeaterItem)rptCiclo.NamingContainer;
                            if (rptItemTurma != null)
                            {
                                HiddenField hdnUadSuperior = (HiddenField)rptItemTurma.FindControl("hdnUadSuperior");
                                HiddenField hdnEscola = (HiddenField)rptItemTurma.FindControl("hdnEscola");
                                HiddenField hdnUnidadeEscola = (HiddenField)rptItemTurma.FindControl("hdnUnidadeEscola");
                                HiddenField hdnCalendario = (HiddenField)rptItemTurma.FindControl("hdnCalendario");
                                HiddenField hdnCalendarioAno = (HiddenField)rptItemTurma.FindControl("hdnCalendarioAno");
                                GridView grv = (GridView)rptItemCiclo.FindControl("grvTurma");

                                if (hdnUadSuperior != null && hdnEscola != null && hdnUnidadeEscola != null && hdnCalendario != null && hdnCalendarioAno != null && grv != null)
                                {
                                    grid = new sGridTurmaEscolaCalendario
                                    {
                                        gridTurma = grv
                                        ,
                                        uad_idSuperior = new Guid(string.IsNullOrEmpty(hdnUadSuperior.Value) ? Guid.Empty.ToString() : hdnUadSuperior.Value)
                                        ,
                                        esc_id = Convert.ToInt32(string.IsNullOrEmpty(hdnEscola.Value) ? "-1" : hdnEscola.Value)
                                        ,
                                        uni_id = Convert.ToInt32(string.IsNullOrEmpty(hdnUnidadeEscola.Value) ? "-1" : hdnUnidadeEscola.Value)
                                        ,
                                        cal_id = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendario.Value) ? "-1" : hdnCalendario.Value)
                                        ,
                                        cal_ano = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendarioAno.Value) ? "-1" : hdnCalendarioAno.Value)
                                    };
                                }
                            }
                        }
                    }
                }

                string report, parametros;

                if (grid.gridTurma != null && grid.esc_id > 0 && grid.uni_id > 0 && grid.cal_id > 0 && grid.cal_ano > 0 && rptItemTurma != null)
                {

                    var turmadisciplina = ((from GridViewRow row in grid.gridTurma.Rows
                                            join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendComp in VS_listaPendencias[rptItemTurma.ClientID]
                                            on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pendComp.tud_idRegencia
                                            join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendReg in VS_listaPendencias[rptItemTurma.ClientID]
                                            on pendComp.tud_id equals pendReg.tud_id
                                            where
                                            (
                                                pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                            )
                                            select pendReg.tud_id)
                                          .Union(from GridViewRow row in grid.gridTurma.Rows
                                                 join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                          on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                                 where
                                                 (
                                                    pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                                 )
                                                 select pend.tud_id));

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_id in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                    }

                    string tud_ids = string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                    turmadisciplina = from GridViewRow row in grid.gridTurma.Rows
                                      join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                      on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                      where
                                      (
                                            pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                      )
                                      && pend.tud_idRegencia > 0
                                      select pend.tud_idRegencia;

                    if (turmadisciplina.Any())
                    {
                        tud_ids += "," + string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                        //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                        foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                        {
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        }
                    }

                    report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
                    parametros = "uad_idSuperiorGestao=" + grid.uad_idSuperior +
                                 "&esc_id=" + grid.esc_id +
                                 "&uni_id=" + grid.uni_id +
                                 "&cal_id=" + grid.cal_id +
                                 "&cal_ano=" + grid.cal_ano +
                                 "&cur_id=-1" +
                                 "&crr_id=-1" +
                                 "&crp_id=-1" +
                                 "&tur_id=-1" +
                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                 "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                                 "&tud_ids=" + tud_ids +
                                 "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                                 "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                 "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                 "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                 "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                         , ApplicationWEB.LogoRelatorioSSRS) +
                                 "&DataProcessamento=" + VS_listaPendencias[rptItemTurma.ClientID].Max(p => p.DataProcessamento);

                    CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void lkbMensagemPendenciaFechamentoExtintas_Click(object sender, EventArgs e)
        {
            sGridTurmaEscolaCalendario grid = new sGridTurmaEscolaCalendario();
            try
            {
                RepeaterItem rptItemTurma = null;
                LinkButton lkb = (LinkButton)sender;
                if (lkb != null)
                {
                    rptItemTurma = (RepeaterItem)lkb.NamingContainer;
                    if (rptItemTurma != null)
                    {
                        HiddenField hdnUadSuperior = (HiddenField)rptItemTurma.FindControl("hdnUadSuperior");
                        HiddenField hdnEscola = (HiddenField)rptItemTurma.FindControl("hdnEscola");
                        HiddenField hdnUnidadeEscola = (HiddenField)rptItemTurma.FindControl("hdnUnidadeEscola");
                        HiddenField hdnCalendario = (HiddenField)rptItemTurma.FindControl("hdnCalendario");
                        HiddenField hdnCalendarioAno = (HiddenField)rptItemTurma.FindControl("hdnCalendarioAno");
                        GridView grv = (GridView)rptItemTurma.FindControl("grvTurmasExtintas");

                        if (hdnUadSuperior != null && hdnEscola != null && hdnUnidadeEscola != null && hdnCalendario != null && hdnCalendarioAno != null && grv != null)
                        {
                            grid = new sGridTurmaEscolaCalendario
                            {
                                gridTurma = grv
                                ,
                                uad_idSuperior = new Guid(string.IsNullOrEmpty(hdnUadSuperior.Value) ? Guid.Empty.ToString() : hdnUadSuperior.Value)
                                ,
                                esc_id = Convert.ToInt32(string.IsNullOrEmpty(hdnEscola.Value) ? "-1" : hdnEscola.Value)
                                ,
                                uni_id = Convert.ToInt32(string.IsNullOrEmpty(hdnUnidadeEscola.Value) ? "-1" : hdnUnidadeEscola.Value)
                                ,
                                cal_id = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendario.Value) ? "-1" : hdnCalendario.Value)
                                ,
                                cal_ano = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendarioAno.Value) ? "-1" : hdnCalendarioAno.Value)
                            };
                        }
                    }
                }

                string report, parametros;

                if (grid.gridTurma != null && grid.esc_id > 0 && grid.uni_id > 0 && grid.cal_id > 0 && grid.cal_ano > 0 && rptItemTurma != null)
                {
                    var turmadisciplina = from GridViewRow row in grid.gridTurma.Rows
                                          join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                          on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                          where
                                          (
                                                pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                          )
                                          select pend.tud_id;

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_id in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                    }

                    string tud_ids = string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                    turmadisciplina = from GridViewRow row in grid.gridTurma.Rows
                                      join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                      on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                      where
                                      (
                                            pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                      )
                                      select pend.tud_idRegencia;

                    if (turmadisciplina.Any())
                    {
                        tud_ids += "," + string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                        //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                        foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                        {
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        }
                    }

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                    }

                    report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
                    parametros = "uad_idSuperiorGestao=" + grid.uad_idSuperior +
                                 "&esc_id=" + grid.esc_id +
                                 "&uni_id=" + grid.uni_id +
                                 "&cal_id=" + grid.cal_id +
                                 "&cal_ano=" + grid.cal_ano +
                                 "&cur_id=-1" +
                                 "&crr_id=-1" +
                                 "&crp_id=-1" +
                                 "&tur_id=-1" +
                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                 "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                                 "&tud_ids=" + tud_ids +
                                 "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                                 "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                 "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                 "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                 "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                         , ApplicationWEB.LogoRelatorioSSRS) +
                                 "&DataProcessamento=" + VS_listaPendencias[rptItemTurma.ClientID].Max(p => p.DataProcessamento);

                    CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void lkbMensagemPendenciaFechamentoProjeto_Click(object sender, EventArgs e)
        {
            sGridTurmaEscolaCalendario grid = new sGridTurmaEscolaCalendario();
            try
            {
                RepeaterItem rptItemTurma = null;
                LinkButton lkb = (LinkButton)sender;
                if (lkb != null)
                {
                    rptItemTurma = (RepeaterItem)lkb.NamingContainer;
                    if (rptItemTurma != null)
                    {
                        HiddenField hdnUadSuperior = (HiddenField)rptItemTurma.FindControl("hdnUadSuperior");
                        HiddenField hdnEscola = (HiddenField)rptItemTurma.FindControl("hdnEscola");
                        HiddenField hdnUnidadeEscola = (HiddenField)rptItemTurma.FindControl("hdnUnidadeEscola");
                        HiddenField hdnCalendario = (HiddenField)rptItemTurma.FindControl("hdnCalendario");
                        HiddenField hdnCalendarioAno = (HiddenField)rptItemTurma.FindControl("hdnCalendarioAno");
                        GridView grv = (GridView)rptItemTurma.FindControl("grvProjetosRecParalela");

                        if (hdnUadSuperior != null && hdnEscola != null && hdnUnidadeEscola != null && hdnCalendario != null && hdnCalendarioAno != null && grv != null)
                        {
                            grid = new sGridTurmaEscolaCalendario
                            {
                                gridTurma = grv
                                ,
                                uad_idSuperior = new Guid(string.IsNullOrEmpty(hdnUadSuperior.Value) ? Guid.Empty.ToString() : hdnUadSuperior.Value)
                                ,
                                esc_id = Convert.ToInt32(string.IsNullOrEmpty(hdnEscola.Value) ? "-1" : hdnEscola.Value)
                                ,
                                uni_id = Convert.ToInt32(string.IsNullOrEmpty(hdnUnidadeEscola.Value) ? "-1" : hdnUnidadeEscola.Value)
                                ,
                                cal_id = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendario.Value) ? "-1" : hdnCalendario.Value)
                                ,
                                cal_ano = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendarioAno.Value) ? "-1" : hdnCalendarioAno.Value)
                            };
                        }                        
                    }
                }

                string report, parametros;

                if (grid.gridTurma != null && grid.esc_id > 0 && grid.uni_id > 0 && grid.cal_id > 0 && grid.cal_ano > 0 && rptItemTurma != null)
                {
                    var turmadisciplina = from GridViewRow row in grid.gridTurma.Rows
                                          join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                          on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                          where
                                          (
                                                pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                          )
                                          select pend.tud_id;

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_id in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                    }

                    string tud_ids = string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                    turmadisciplina = from GridViewRow row in grid.gridTurma.Rows
                                      join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[rptItemTurma.ClientID]
                                      on Convert.ToInt64(grid.gridTurma.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                      where
                                      (
                                            pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                      )
                                      select pend.tud_idRegencia;

                    if (turmadisciplina.Any())
                    {
                        tud_ids += "," + string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                        //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                        foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                        {
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                            CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        }
                    }

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                    }

                    report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
                    parametros = "uad_idSuperiorGestao=" + grid.uad_idSuperior +
                                 "&esc_id=" + grid.esc_id +
                                 "&uni_id=" + grid.uni_id +
                                 "&cal_id=" + grid.cal_id +
                                 "&cal_ano=" + grid.cal_ano +
                                 "&cur_id=-1" +
                                 "&crr_id=-1" +
                                 "&crp_id=-1" +
                                 "&tur_id=-1" +
                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                 "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                                 "&tud_ids=" + tud_ids +
                                 "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                                 "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                 "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                 "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                 "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                         , ApplicationWEB.LogoRelatorioSSRS) +
                                 "&DataProcessamento=" + VS_listaPendencias[rptItemTurma.ClientID].Max(p => p.DataProcessamento);

                    CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvTurma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && PreCarregarFechamentoCache)
            {
                long tur_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tur_id"));
                int fav_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "fav_id"));
                int esc_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "esc_id"));
                int cal_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cal_id"));
                byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tur_tipo"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));

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

                ImageButton imgPendenciaFechamento = (ImageButton)e.Row.FindControl("imgPendenciaFechamento");
                if (imgPendenciaFechamento != null)
                {
                    imgPendenciaFechamento.CommandName = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "PendenciaFechamentoAutomatico" : "PendenciaFechamento";
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

                HiddenField hdn = (HiddenField)e.Row.FindControl("hdnDadosFechamento");
                hdn.Value = tud_id.ToString()
                            
                            + ";" + tur_id.ToString()
                            
                            + ";" + tpc_id.ToString()                        
                            
                            + ";" + ava_id.ToString()
                            
                            + ";" + entityFormatoAvaliacao.fav_id.ToString()
                            
                            + ";" + ava_tipo.ToString()
                            
                            + ";" + entityEscalaAvaliacao.esa_id.ToString()

                            + ";" + entityEscalaAvaliacao.esa_tipo.ToString()

                            + ";" + entityEscalaAvaliacaoDocente.esa_tipo.ToString()

                            + ";" + notaMinimaAprovacao.ToString()

                            + ";" + ordemParecerMinimo.ToString()

                            + ";" + entityFormatoAvaliacao.fav_tipoLancamentoFrequencia.ToString()

                            + ";" + entityFormatoAvaliacao.fav_calculoQtdeAulasDadas.ToString()

                            + ";" + tur_tipo.ToString()

                            + ";" + cal_id.ToString()

                            + ";" + DataBinder.Eval(e.Row.DataItem, "tud_tipo").ToString()

                            + ";" + tpc_ordem.ToString()

                            + ";" + entityFormatoAvaliacao.fav_variacao.ToString()

                            + ";" + "0"

                            + ";" + (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "tud_disciplinaEspecial")) ? "true" : "false")

                            + ";" + (entityFormatoAvaliacao.fav_fechamentoAutomatico ? "true" : "false");
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhaEscola)
            {
                string valor1;
                string valor2;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                if (!string.IsNullOrEmpty(valor1) && UCComboUAEscola1.Uad_ID != new Guid(valor1))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor1);
                    UCComboUAEscola1_IndexChangedUA();
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2);

                if (!string.IsNullOrEmpty(valor1) && !string.IsNullOrEmpty(valor2) &&
                    UCComboUAEscola1.Esc_ID != Convert.ToInt32(valor1) && UCComboUAEscola1.Uni_ID != Convert.ToInt32(valor2))
                {
                    UCComboUAEscola1.SelectedValueEscolas = new int[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                }
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
        /// Carrega o repeater de escolas
        /// </summary>
        private void CarregaRepeaterEscolas()
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                if (UCComboUAEscola1.Esc_ID > 0)
                {
                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);

                    if (dados.Count == 0)
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage("Nenhuma turma encontrada para o Gestor.", UtilBO.TipoMensagem.Alerta);
                        return;
                    }
                    rptTurmas.DataSource = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo));

                    #region Salvar busca realizada com os parâmetros do ODS.

                    Dictionary<string, string> filtros = new Dictionary<string, string>();

                    filtros.Add("ua_superior", UCComboUAEscola1.Uad_ID.ToString());
                    filtros.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                    filtros.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());

                    __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.MinhaEscola, Filtros = filtros };

                    #endregion Salvar busca realizada com os parâmetros do ODS.
                }
                else
                {
                    rptTurmas.DataSource = null;
                }

                rptTurmas.DataBind();

                foreach (RepeaterItem itemTurma in rptTurmas.Items)
                {
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

                    int uni_id = 0;
                    HiddenField hdnUnidadeEscola = itemTurma.FindControl("hdnUnidadeEscola") as HiddenField;
                    if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                    {
                        uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                    }

                    // Verifico se existe evento de fechamento vigente para a escola e calendario,
                    // para mostrar o botao de atualizar pendencias.
                    string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
                    string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
                    List<ACA_Evento> lstEventosEscola = ACA_EventoBO.GetEntity_Efetivacao_ListPorPeriodo(cal_id, -1, Guid.Empty, esc_id, uni_id, ent_id);

                    if (lstEventosEscola.Any(p => Convert.ToString(p.tev_id) == valor || Convert.ToString(p.tev_id) == valorFinal))
                    {
                        CarregarPendencias(itemTurma, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pesquisa as turmas pelo ciclo
        /// </summary>
        /// <param name="ClientIDCombo"></param>
        private void PesquisaTurmasFiltros(string ClientIDCombo)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                int esc_id = 0;
                int uni_id = 0;
                byte ciclo_id = 0;
                int cur_id = 0;
                int crr_id = 0;
                int crp_id = 0;
                int tds_id = 0;
                int cal_id = 0;

                foreach (RepeaterItem rpTurma in rptTurmas.Items)
                {
                    if (ClientIDCombo.IndexOf(string.Format("{0}_", rpTurma.ClientID)) >= 0)
                    {
                        // Id Escola
                        HiddenField hdnId = rpTurma.FindControl("hdnEscola") as HiddenField;
                        if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                        {
                            esc_id = Convert.ToInt32(hdnId.Value);
                        }

                        // Id Unidade
                        HiddenField hdnIdUnid = rpTurma.FindControl("hdnUnidadeEscola") as HiddenField;
                        if (hdnIdUnid != null && !string.IsNullOrEmpty(hdnIdUnid.Value))
                        {
                            uni_id = Convert.ToInt32(hdnIdUnid.Value);
                        }

                        // Calendario
                        HiddenField hdnCalendario = rpTurma.FindControl("hdnCalendario") as HiddenField;
                        if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                        {
                            cal_id = Convert.ToInt32(hdnCalendario.Value);
                        }

                        // txtSelectedTab
                        HtmlInputHidden txtSelectedTab = rpTurma.FindControl("txtSelectedTab") as HtmlInputHidden;

                        Repeater rptCiclo = rpTurma.FindControl("rptCiclosAbas") as Repeater;
                        if (rptCiclo != null)
                        {
                            foreach (RepeaterItem rptItemCiclo in rptCiclo.Items)
                            {
                                if (ClientIDCombo.IndexOf(string.Format("{0}_", rptItemCiclo.ClientID)) >= 0)
                                {
                                    bool carregaPeriodoDisciplina = false;
                                    bool carregaGridTurmas = false;

                                    // Id do Ciclo
                                    HiddenField hdnCiclo = rptItemCiclo.FindControl("hdnCiclo") as HiddenField;
                                    if (hdnCiclo != null && !string.IsNullOrEmpty(hdnCiclo.Value))
                                    {
                                        ciclo_id = Convert.ToByte(hdnCiclo.Value);
                                    }

                                    UCCCursoCurriculo UCCurso = rptItemCiclo.FindControl("UCCCursoCurriculo1") as UCCCursoCurriculo;
                                    if (UCCurso != null)
                                    {
                                        if (UCCurso.ClientID_Combo == ClientIDCombo)
                                        {
                                            carregaPeriodoDisciplina = true;
                                        }
                                        cur_id = UCCurso.Valor[0];
                                        crr_id = UCCurso.Valor[1];

                                        if (cur_id > 0 && crr_id > 0)
                                        {
                                            carregaGridTurmas = true;
                                        }
                                    }

                                    UCCCurriculoPeriodo UCPeriodo = rptItemCiclo.FindControl("UCCCurriculoPeriodo1") as UCCCurriculoPeriodo;
                                    if (UCPeriodo != null)
                                    {
                                        if (UCPeriodo.ClientID_Combo == ClientIDCombo)
                                        {
                                            txtSelectedTab.Value = rptItemCiclo.ItemIndex.ToString();
                                        }

                                        if (carregaPeriodoDisciplina)
                                        {
                                            UCPeriodo.PermiteEditar = true;
                                            //UCPeriodo.CarregarPorCursoCurriculoTipoCiclo(cur_id, crr_id, ciclo_id);
                                            UCPeriodo.CarregarPorCursoCurriculoEscolaCiclo(cur_id, crr_id, esc_id, uni_id, ciclo_id);
                                        }

                                        crp_id = UCPeriodo.Valor[2];
                                    }

                                    WebControls_Combos_UCComboTipoDisciplina UCTipoDisciplina = rptItemCiclo.FindControl("UCComboTipoDisciplina1") as WebControls_Combos_UCComboTipoDisciplina;
                                    if (UCTipoDisciplina != null)
                                    {
                                        if (UCTipoDisciplina._Combo.ClientID == ClientIDCombo)
                                        {
                                            txtSelectedTab.Value = rptItemCiclo.ItemIndex.ToString();
                                        }

                                        tds_id = UCTipoDisciplina.Valor;

                                        if (carregaPeriodoDisciplina)
                                        {
                                            UCTipoDisciplina.PermiteEditar = true;
                                            UCTipoDisciplina.CarregarTipoDisciplinaPorCursoTipoCiclo(cur_id, ciclo_id, esc_id);
                                        }
                                    }

                                    // Carrega grid de turmas pelo periodo selecionado
                                    GridView grvTurma = rptItemCiclo.FindControl("grvTurma") as GridView;
                                    if (grvTurma != null)
                                    {
                                        grvTurma.Visible = true;

                                        List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
                                        List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

                                        dados.All(p =>
                                        {
                                            lista.AddRange(p.Turmas.Where(t => t.esc_id == esc_id && t.cal_id == cal_id));
                                            return true;
                                        });

                                        lista = lista.GroupBy(x => x.tud_id).Select(g => g.First()).ToList();

                                        if (carregaGridTurmas && cur_id > 0 && crr_id > 0 && crp_id > 0 && tds_id > 0)
                                        {
                                            var turmas = lista.Where(x => x.tci_id == ciclo_id && x.cur_id == cur_id && x.crr_id == crr_id && x.crp_id == crp_id && x.tds_id == tds_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                            grvTurma.DataSource = turmas;
                                            grvTurma.DataBind();
                                        }
                                        else if (carregaGridTurmas && cur_id > 0 && crr_id > 0)
                                        {
                                            if (crp_id > 0)
                                            {
                                                var turmas = lista.Where(x => x.tci_id == ciclo_id && x.cur_id == cur_id && x.crr_id == crr_id && x.crp_id == crp_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                                grvTurma.DataSource = turmas;
                                                grvTurma.DataBind();
                                            }
                                            else if (tds_id > 0)
                                            {
                                                var turmas = lista.Where(x => x.tci_id == ciclo_id && x.cur_id == cur_id && x.crr_id == crr_id && x.tds_id == tds_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                                grvTurma.DataSource = turmas;
                                                grvTurma.DataBind();
                                            }
                                            else
                                            {
                                                var turmas = lista.Where(x => x.tci_id == ciclo_id && x.cur_id == cur_id && x.crr_id == crr_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                                grvTurma.DataSource = turmas;
                                                grvTurma.DataBind();
                                            }
                                        }
                                        else if (carregaGridTurmas && tds_id > 0)
                                        {
                                            var turmas = lista.Where(x => x.tci_id == ciclo_id && x.tds_id == tds_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                            grvTurma.DataSource = turmas;
                                            grvTurma.DataBind();
                                        }
                                        else if (carregaGridTurmas)
                                        {
                                            var turmas = lista.Where(x => x.tci_id == ciclo_id && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && x.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno).ToList();

                                            grvTurma.DataSource = turmas;
                                            grvTurma.DataBind();
                                        }

                                        // Verifico se existe evento de fechamento vigente para a escola e calendario,
                                        // para mostrar o botao de atualizar pendencias.
                                        string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
                                        string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
                                        List<ACA_Evento> lstEventosEscola = ACA_EventoBO.GetEntity_Efetivacao_ListPorPeriodo(cal_id, -1, Guid.Empty, esc_id, uni_id, ent_id);

                                        if (lstEventosEscola.Any(p => Convert.ToString(p.tev_id) == valor || Convert.ToString(p.tev_id) == valorFinal))
                                        {
                                            CarregarPendencias(rpTurma, true);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void CarregaAulasPrevistas(int esc_id, long tur_id, long tud_id, int cal_id, Guid ent_id)
        {
            List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
            List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

            var dadosTurmas = dados.Find(p => p.esc_id == esc_id && p.cal_id == cal_id).Turmas;
            var turmaSelecionada = dadosTurmas.Where(p => p.tud_id == tud_id).ToList().FirstOrDefault();

            pnlIndicadores.GroupingText = turmaSelecionada.tur_escolaUnidade + "<br />" + turmaSelecionada.tur_calendario;

            dados.All(p =>
            {
                lista.AddRange(p.Turmas.Where(t => t.esc_id == esc_id && p.cal_id == cal_id));
                return true;
            });

            lista = lista.GroupBy(x => x.tud_id).Select(g => g.First()).ToList();
            uccTurmaDisciplina.CarregarCombo(lista, "EscolaTurmaDisciplina", "DataValueFieldCombo");

            uccTurmaDisciplina.Valor = String.Format(
                    "{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                    turmaSelecionada.tur_id,
                    turmaSelecionada.tud_id,
                    turmaSelecionada.cal_id,
                    turmaSelecionada.tdt_posicao,
                    turmaSelecionada.tud_tipo,
                    turmaSelecionada.tur_tipo,
                    turmaSelecionada.tur_idNormal,
                    turmaSelecionada.tud_idAluno,
                    (turmaSelecionada.fav_fechamentoAutomatico ? "true" : "false")
                );
                
            // Carregar os dados da turma.
            uccTurmaDisciplina_IndexChanged();

            hdnEscId.Value = esc_id.ToString();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "TrocarTurma", "$(document).ready(function() { $('.divIndicadores').dialog('open'); });", true);
        }

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno(long tud_id, int esc_id, int uni_id, long tur_id, bool tud_naoLancarNota, bool tud_naoLancarFrequencia, int cal_id, string EscolaTurmaDisciplina, DateTime tur_dataEncerramento, string tciIds, byte tur_tipo, long tud_idAluno, long tur_idNormal, byte tipoPendencia, int tpcIdPendencia, long tudIdPendencia)
        {
            Session.Remove("TudIdCompartilhada");
            byte opcaoAba = Convert.ToByte(eOpcaoAbaMinhasTurmas.DiarioClasse);
            List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
            Struct_CalendarioPeriodos periodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();

            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Tud_idRetorno_ControleTurma", tud_id.ToString());
            listaDados.Add("Edit_tdt_posicao", "1");
            listaDados.Add("Edit_esc_id", esc_id.ToString());
            listaDados.Add("Edit_uni_id", uni_id.ToString());
            listaDados.Add("Edit_tur_id", tur_id.ToString());
            listaDados.Add("Edit_tud_naoLancarNota", tud_naoLancarNota.ToString());
            listaDados.Add("Edit_tud_naoLancarFrequencia", tud_naoLancarFrequencia.ToString());
            listaDados.Add("Edit_tpc_id", periodo.tpc_id.ToString());
            listaDados.Add("Edit_tpc_ordem", periodo.tpc_ordem.ToString());
            listaDados.Add("Edit_cal_id", cal_id.ToString());
            listaDados.Add("TextoTurmas", EscolaTurmaDisciplina);
            listaDados.Add("OpcaoAbaAtual", opcaoAba.ToString());
            listaDados.Add("Edit_tur_dataEncerramento", tur_dataEncerramento.ToString());
            listaDados.Add("Edit_tciIds", tciIds);
            listaDados.Add("Edit_tur_tipo", tur_tipo.ToString());
            listaDados.Add("Edit_tud_idAluno", tud_idAluno.ToString());
            listaDados.Add("Edit_tur_idNormal", tur_idNormal.ToString());
            listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/MinhaEscolaGestor.aspx");

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(__SessionWEB.__UsuarioWEB.Usuario.ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);

            if (tipoPendencia > 0)
            {
                Session["tipoPendencia"] = tipoPendencia;
                Session["tpcIdPendencia"] = tpcIdPendencia;
                Session["tudIdPendencia"] = tudIdPendencia;
            }
            else
            {
                Session.Remove("tipoPendencia");
                Session.Remove("tpcIdPendencia");
                Session.Remove("tudIdPendencia");
            }
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
        /// Método para associar os delegates do repeater
        /// </summary>
        private void AssociaDelegatesRepeater()
        {
            foreach (RepeaterItem rpTurma in rptTurmas.Items)
            {
                Repeater rptCiclo = rpTurma.FindControl("rptCiclosAbas") as Repeater;
                if (rptCiclo != null)
                {
                    foreach (RepeaterItem rptItemCiclo in rptCiclo.Items)
                    {
                        UCCCursoCurriculo UCCurso = rptItemCiclo.FindControl("UCCCursoCurriculo1") as UCCCursoCurriculo;
                        if (UCCurso != null)
                        {
                            UCCurso.IndexChanged_Sender += UCCCursoCurriculo1_IndexChanged;
                        }

                        UCCCurriculoPeriodo UCPeriodo = rptItemCiclo.FindControl("UCCCurriculoPeriodo1") as UCCCurriculoPeriodo;
                        if (UCPeriodo != null)
                        {
                            UCPeriodo.IndexChanged_Sender += UCCCurriculoPeriodo1_IndexChanged;
                        }

                        WebControls_Combos_UCComboTipoDisciplina UCTipoDisciplina = rptItemCiclo.FindControl("UCComboTipoDisciplina1") as WebControls_Combos_UCComboTipoDisciplina;
                        if (UCTipoDisciplina != null)
                        {
                            UCTipoDisciplina.IndexChanged_Sender += UCComboTipoDisciplina1_IndexChanged;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se a disciplina esta sendo compartilhada com mais de uma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_codigo">Codigo da turma</param>
        /// <param name="tud_nome">Nome da disciplina</param>
        /// <returns></returns>
        private bool VerificaDisciplinasCompartilhadas(long tud_id, string turma, string disciplina)
        {
            List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo);
            if (lstDisciplinaCompartilhada.Count == 0)
            {
                if (String.IsNullOrEmpty(disciplina))
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(String.Format("{0} {1}.",
                                                                            GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                            , turma)
                                                                        , UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(String.Format("{0} {1} - {2}.",
                                                                            GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                            , turma
                                                                            , disciplina)
                                                                        , UtilBO.TipoMensagem.Alerta);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "VerificaDisciplinasCompartilhadas", "$(document).ready(scrollToTop);", true);
                return false;
            }
            else if (lstDisciplinaCompartilhada.Count == 1)
            {
                // Atualiza a sessao com a disciplina compartilhada
                Session["TudIdCompartilhada"] = lstDisciplinaCompartilhada[0].tud_id.ToString();
            }
            else if (lstDisciplinaCompartilhada.Count > 1)
            {
                UCSelecaoDisciplinaCompartilhada1.AbrirDialog(tud_id, 0, (String.IsNullOrEmpty(disciplina) ? turma : turma + " - " + disciplina));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Redireciona para uma das telas do Minhas Turmas, de acordo com o evento do grid.
        /// </summary>
        /// <param name="nomeTela"></param>
        /// <param name="nomePagina"></param>
        /// <param name="grid"></param>
        /// <param name="indice"></param>
        /// <param name="validarDisciplinaCompartilhada"></param>
        private void RedirecionaTelaMinhasTurmas(string nomeTela, string nomePagina, GridView grid, string indice, bool validarDisciplinaCompartilhada, byte tipoPendencia = 0, int tpcIdPendencia = -1, long tudIdPendencia = -1)
        {
            try
            {
                int index = Convert.ToInt32(indice);
                if (grid != null)
                {
                    long tud_id = 0;
                    int esc_id = 0;
                    int uni_id = 0;                 
                    long tur_id = 0;
                    bool tud_naoLancarNota = false;
                    bool tud_naoLancarFrequencia = false;
                    int cal_id = 0;
                    string EscolaTurmaDisciplina = string.Empty;
                    string tciIds = string.Empty;
                    DateTime tur_dataEncerramento = new DateTime();
                    byte tud_tipo = 0;
                    string nomeTurmaDisciplina = string.Empty;
                    long tud_idAluno = 0;
                    long tur_idNormal = 0;
                    byte tur_tipo = 0;
                    
                    tud_id = Convert.ToInt64(grid.DataKeys[index].Values["tud_id"].ToString());
                    esc_id = Convert.ToInt32(grid.DataKeys[index].Values["esc_id"].ToString());
                    uni_id = Convert.ToInt32(grid.DataKeys[index].Values["uni_id"].ToString());
                    tur_id = Convert.ToInt64(grid.DataKeys[index].Values["tur_id"].ToString());
                    tud_naoLancarNota = Convert.ToBoolean(grid.DataKeys[index].Values["tud_naoLancarNota"].ToString());
                    tud_naoLancarFrequencia = Convert.ToBoolean(grid.DataKeys[index].Values["tud_naoLancarFrequencia"].ToString());
                    cal_id = Convert.ToInt32(grid.DataKeys[index].Values["cal_id"].ToString());
                    EscolaTurmaDisciplina = grid.DataKeys[index].Values["EscolaTurmaDisciplina"].ToString();
                    tciIds = grid.DataKeys[index].Values["tciIds"].ToString();
                    tur_dataEncerramento = Convert.ToDateTime(grid.DataKeys[index].Values["tur_dataEncerramento"].ToString());
                    tud_tipo = Convert.ToByte(grid.DataKeys[index].Values["tud_tipo"].ToString());
                    nomeTurmaDisciplina = ((Label)grid.Rows[index].FindControl("lblTurma")).Text;
                    Byte.TryParse(grid.DataKeys[index].Values["tur_tipo"].ToString(), out tur_tipo);
                    Int64.TryParse(grid.DataKeys[index].Values["tud_idAluno"].ToString(), out tud_idAluno);
                    Int64.TryParse(grid.DataKeys[index].Values["tur_idNormal"].ToString(), out tur_idNormal);

                    CarregaSessionPaginaRetorno(tud_id, esc_id, uni_id, tur_id, tud_naoLancarNota, tud_naoLancarFrequencia, cal_id, EscolaTurmaDisciplina, tur_dataEncerramento, tciIds, tur_tipo, tud_idAluno, tur_idNormal, tipoPendencia, tpcIdPendencia, tudIdPendencia);
                    if (!validarDisciplinaCompartilhada
                        || tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                        || VerificaDisciplinasCompartilhadas(tud_id, nomeTurmaDisciplina, ""))
                    {
                        RedirecionaTela(nomePagina);
                    }
                    else
                    {
                        VS_TelaRedirecionar = nomePagina;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", nomeTela), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// O método verifica as pendências de fechamento das disciplinas dos grids de turmas.
        /// </summary>
        /// <param name="listaGrid">Lista de grids.</param>
        private void VerificaPendenciasFechamento(RepeaterItem itemTurma, List<GridView> listaGrid, List<sTurmaDisciplinaEscolaCalendario> lstCarregarPendencias, bool mostrarPendencia)
        {
            if (lstCarregarPendencias != null)
            {
                List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> lst = REL_TurmaDisciplinaSituacaoFechamentoBO.SelecionaPendencias(lstCarregarPendencias, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);
                if (!VS_listaPendencias.ContainsKey(itemTurma.ClientID))
                    VS_listaPendencias.Add(itemTurma.ClientID, lst);
                else
                    VS_listaPendencias[itemTurma.ClientID] = lst;   
            }

            if (mostrarPendencia)
            {
                List<string> abasPendenciaFechamento = new List<string>();
                foreach (var grv in listaGrid)
                {
                    bool possuiPendencia = false;

                    foreach (GridViewRow row in grv.Rows)
                    {
                        long tud_id = Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]);
                        byte tud_tipo = Convert.ToByte(grv.DataKeys[row.RowIndex].Values["tud_tipo"]);
                        Image imgPendenciaFechamento = (Image)row.FindControl("imgPendenciaFechamento");
                        Image imgPendenciaPlanejamento = (Image)row.FindControl("imgPendenciaPlanejamento");
                        Image imgPendenciaPlanoAula = (Image)row.FindControl("imgPendenciaPlanoAula");
                        if (imgPendenciaFechamento != null)
                        {
                            imgPendenciaFechamento.Visible = false;
                        }
                        if (imgPendenciaPlanejamento != null)
                        {
                            imgPendenciaPlanejamento.Visible = false;
                        }
                        if (imgPendenciaPlanoAula != null)
                        {
                            imgPendenciaPlanoAula.Visible = false;
                        }

                        if (tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            if (VS_listaPendencias[itemTurma.ClientID].Any(item =>
                                (
                                    item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                )
                                && item.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia 
                                && item.tud_idRegencia == tud_id))
                            {
                                if (imgPendenciaFechamento != null)
                                    imgPendenciaFechamento.Visible = possuiPendencia = true;
                            }

                            if (VS_listaPendencias[itemTurma.ClientID].Any(item =>
                                item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                && item.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia 
                                && item.tud_idRegencia == tud_id))
                            {
                                if (imgPendenciaPlanejamento != null)
                                    imgPendenciaPlanejamento.Visible = true;
                            }
                        }
                        else
                        {
                            if (VS_listaPendencias[itemTurma.ClientID].Any(item =>
                                (
                                    item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                    || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                )
                                && item.tud_id == tud_id))
                            {
                                if (imgPendenciaFechamento != null)
                                    imgPendenciaFechamento.Visible = possuiPendencia = true;
                            }

                            if (VS_listaPendencias[itemTurma.ClientID].Any(item =>
                                item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                && item.tud_id == tud_id))
                            {
                                if (imgPendenciaPlanejamento != null)
                                    imgPendenciaPlanejamento.Visible = true;
                            }
                        }

                        if (VS_listaPendencias[itemTurma.ClientID].Any(item =>
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula
                            && item.tud_id == tud_id)
                        &&
                        (
                            // Mesma regra para exibir o ícone no Listão e no Diário de Classe.
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                            || Convert.ToInt32(grv.DataKeys[row.RowIndex].Values["tne_id"]) != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        ))
                        {
                            if (imgPendenciaPlanoAula != null)
                                imgPendenciaPlanoAula.Visible = true;
                        }
                    }

                    if (possuiPendencia && lstCarregarPendencias != null)
                    {
                        if (grv.ID == "grvTurmasExtintas")
                        {
                            abasPendenciaFechamento.Add(GetGlobalResourceObject("Academico", "ControleTurma.BuscaMinhaEscola.lblTabsTurmasEx.text").ToString());
                        }
                        else if (grv.ID == "grvProjetosRecParalela")
                        {
                            abasPendenciaFechamento.Add(GetGlobalResourceObject("Academico", "ControleTurma.MinhaEscolaGestor.litProjetos.Text").ToString());
                        }
                        else
                        {
                            RepeaterItem rptItemCiclo = (RepeaterItem)grv.NamingContainer;
                            if (rptItemCiclo != null)
                            {
                                HiddenField hdnNomeCiclo = (HiddenField)rptItemCiclo.FindControl("hdnNomeCiclo");
                                if (hdnNomeCiclo != null)
                                {
                                    abasPendenciaFechamento.Add(hdnNomeCiclo.Value);
                                }
                            }
                        }
                    }

                    RepeaterItem rptItem = (RepeaterItem)grv.NamingContainer;
                    if (rptItem != null)
                    {
                        HtmlGenericControl divMensagemFechamentoPendencia = grv.ID == "grvTurmasExtintas" ?
                            (HtmlGenericControl)rptItem.FindControl("mensagemPendenciaFechamentoMinhaEscolaGestorExtintas") :
                                                                            grv.ID == "grvProjetosRecParalela" ?
                            (HtmlGenericControl)rptItem.FindControl("mensagemPendenciaFechamentoMinhaEscolaGestorProjeto") :
                            (HtmlGenericControl)rptItem.FindControl("mensagemPendenciaFechamentoMinhaEscolaGestor");

                        HtmlGenericControl mensagemSemPendenciaFechamento = grv.ID == "grvTurmasExtintas" ?
                            (HtmlGenericControl)rptItem.FindControl("mensagemSemPendenciaFechamentoExtintas") :
                                                                            grv.ID == "grvProjetosRecParalela" ?
                            (HtmlGenericControl)rptItem.FindControl("mensagemSemPendenciaFechamentoProjeto") :
                            (HtmlGenericControl)rptItem.FindControl("mensagemSemPendenciaFechamento");

                        Label lblDataProcessamento = grv.ID == "grvTurmasExtintas" ?
                            (Label)rptItem.FindControl("lblDataProcessamentoExtintas") :
                                                        grv.ID == "grvProjetosRecParalela" ?
                            (Label)rptItem.FindControl("lblDataProcessamentoProjeto") :
                            (Label)rptItem.FindControl("lblDataProcessamento");
                        if (lblDataProcessamento != null)
                        {
                            lblDataProcessamento.Text = string.Empty;
                            if (possuiPendencia)
                            {
                                var pendencias = (from GridViewRow row in grv.Rows
                                                  join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[itemTurma.ClientID]
                                                  on Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                                  where pend.DataProcessamento != new DateTime()
                                                  select pend).Union(
                                                    from GridViewRow row in grv.Rows
                                                    join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[itemTurma.ClientID]
                                                    on Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_idRegencia
                                                    where pend.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia &&
                                                          Convert.ToByte(grv.DataKeys[row.RowIndex].Values["tud_tipo"]) == (byte)TurmaDisciplinaTipo.Regencia &&
                                                          pend.DataProcessamento != new DateTime()
                                                    select pend
                                                    );
                                if (pendencias.Any())
                                {
                                    DateTime dataProcessamento = pendencias.Max(p => p.DataProcessamento);
                                    lblDataProcessamento.Text = pendencias.Any() ?
                                        String.Format(GetGlobalResourceObject("Academico", "ControleTurma.MinhaEscolaGestor.lblDataProcessamento.Text").ToString(), dataProcessamento.ToString("dd'/'MM'/'yyyy HH:mm:ss")) :
                                        string.Empty;
                                }

                                possuiPendencia = false;

                                var turmadisciplina = ((from GridViewRow row in grv.Rows
                                                        join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendComp in VS_listaPendencias[itemTurma.ClientID]
                                                        on Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]) equals pendComp.tud_idRegencia
                                                        join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendReg in VS_listaPendencias[itemTurma.ClientID]
                                                        on pendComp.tud_id equals pendReg.tud_id
                                                        where
                                                        (
                                                            pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                            || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                            || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                            || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                            || pendReg.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                                        )
                                                        select pendReg.tud_id)
                                          .Union(from GridViewRow row in grv.Rows
                                                 join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[itemTurma.ClientID]
                                                 on Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                                 where
                                                 (
                                                    pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                                 )
                                                 select pend.tud_id));

                                if (turmadisciplina.Any())
                                {
                                    possuiPendencia = true;
                                }

                                turmadisciplina = from GridViewRow row in grv.Rows
                                                  join REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in VS_listaPendencias[itemTurma.ClientID]
                                                  on Convert.ToInt64(grv.DataKeys[row.RowIndex].Values["tud_id"]) equals pend.tud_id
                                                  where
                                                  (
                                                    pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                                    || pend.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                                  )
                                                  && pend.tud_idRegencia > 0
                                                  select pend.tud_idRegencia;

                                if (turmadisciplina.Any())
                                {
                                    possuiPendencia = true;
                                }

                                if (mensagemSemPendenciaFechamento != null)
                                {
                                    mensagemSemPendenciaFechamento.Visible = !possuiPendencia;
                                }

                                if (divMensagemFechamentoPendencia != null)
                                {
                                    divMensagemFechamentoPendencia.Visible = possuiPendencia;
                                }
                            }
                        }
                    }
                }

                Control mensagemPendenciaFechamentoAbas = itemTurma.FindControl("mensagemPendenciaFechamentoAbas");
                Literal lblMensagemPendenciaFechamento = (Literal)itemTurma.FindControl("lblMensagemPendenciaFechamento");
                if (abasPendenciaFechamento.Count > 0)
                {
                    string strAbasPendenciaFechamento = string.Empty;
                    for (int i = 0; i < abasPendenciaFechamento.Count; i++)
                    {
                        if (i == 0)
                        {
                            strAbasPendenciaFechamento += abasPendenciaFechamento[i];
                        }
                        else if (i == abasPendenciaFechamento.Count - 1)
                        {
                            strAbasPendenciaFechamento += " e " + abasPendenciaFechamento[i];
                        }
                        else
                        {
                            strAbasPendenciaFechamento += ", " + abasPendenciaFechamento[i];
                        }
                    }
                    mensagemPendenciaFechamentoAbas.Visible = true;
                    lblMensagemPendenciaFechamento.Text = string.Format(GetGlobalResourceObject("Academico", "ControleTurma.MinhaEscolaGestor.lblMensagemPendenciaFechamento.Text").ToString(), strAbasPendenciaFechamento);
                }
                else
                {
                    mensagemPendenciaFechamentoAbas.Visible = false;
                }
                upnResultado.Update();
            }
        }

        /// <summary>
        /// Atualiza as indicacoes de pendencias no fechamento por turma/disciplina e a mensagem de pendencia geral.
        /// </summary>
        private void CarregarPendencias(RepeaterItem itemTurma, bool mostrarPendencia)
        {
            Repeater rptCiclosAbas = (Repeater)itemTurma.FindControl("rptCiclosAbas");
            GridView grvTurmasExtintas = (GridView)itemTurma.FindControl("grvTurmasExtintas");
            GridView grvProjetosRecParalela = (GridView)itemTurma.FindControl("grvProjetosRecParalela");
            
            List<GridView> listaGridsTurmas = new List<GridView>();
            if (rptCiclosAbas != null)
            {
                listaGridsTurmas = (from RepeaterItem itemCiclo in rptCiclosAbas.Items
                                    let grid = (GridView)itemCiclo.FindControl("grvTurma")
                                    where grid != null
                                    select grid).ToList();
            }

            if (grvTurmasExtintas != null)
            {
                listaGridsTurmas.Add(grvTurmasExtintas);
            }

            if (grvProjetosRecParalela != null)
            {
                listaGridsTurmas.Add(grvProjetosRecParalela);
            }

            if (listaGridsTurmas.Any())
            {
                HiddenField hdnEscola = (HiddenField)itemTurma.FindControl("hdnEscola");
                HiddenField hdnCalendario = (HiddenField)itemTurma.FindControl("hdnCalendario");
                int esc_id = Convert.ToInt32(hdnEscola.Value);

                List<sTurmaDisciplinaEscolaCalendario> lstCarregarPendencias = new List<sTurmaDisciplinaEscolaCalendario>();
                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(__SessionWEB.__UsuarioWEB.Usuario.ent_id, esc_id, ApplicationWEB.AppMinutosCacheCurto);     
                List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();
                dados.All(p =>
                {
                    lista.AddRange(p.Turmas.Where(t => t.esc_id == esc_id
                                                        && t.cal_id == Convert.ToInt32(hdnCalendario.Value)
                                                        && (t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo
                                                            || t.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)));
                    return true;
                });

                lstCarregarPendencias.AddRange(lista.Select(p =>
                                                            new sTurmaDisciplinaEscolaCalendario
                                                            {
                                                                tur_id = p.tur_id
                                                                ,
                                                                tud_id = p.tud_id
                                                                ,
                                                                tud_tipo = p.tud_tipo
                                                                ,
                                                                esc_id = p.esc_id
                                                                ,
                                                                uni_id = p.uni_id
                                                                ,
                                                                cal_id = p.cal_id
                                                            }
                                                            ).Distinct().ToList());

                lstCarregarPendencias = lstCarregarPendencias.Select(p => p).Distinct().ToList();
                VerificaPendenciasFechamento(itemTurma, listaGridsTurmas, lstCarregarPendencias, mostrarPendencia);
            }
        }

        #endregion Métodos 
    }
}