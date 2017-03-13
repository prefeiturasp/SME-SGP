using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class Frequencia : MotherPageLogadoCompressedViewState
    {
        #region Propriedades

        /// <summary>
        /// Guarda todas as entities utilizadas pela pagina
        /// </summary>
        /// <author>juliano.real</author>
        /// <datetime>05/05/2014-15:29</datetime>
        private ControleTurmas VS_EntitiesControleTurma
        {
            get
            {
                if (ViewState["VS_EntitiesControleTurma"] == null)
                {
                    ViewState["VS_EntitiesControleTurma"] = TUR_TurmaDisciplinaBO.SelecionaEntidadesControleTurmas(UCControleTurma1.VS_tud_id, ApplicationWEB.AppMinutosCacheLongo);
                }
                return (ControleTurmas)(ViewState["VS_EntitiesControleTurma"]);
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro compensações.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoCompensacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoCompensacao"] ??
                            (
                                ViewState["VS_ltPermissaoCompensacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Compensacoes)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de frequencia.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoFrequencia
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoFrequencia"] ??
                            (
                                ViewState["VS_ltPermissaoFrequencia"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Frequencia)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de avaliações.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoAvaliacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoAvaliacao"] ??
                            (
                                ViewState["VS_ltPermissaoAvaliacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Avaliacoes)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de efetivacap.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoEfetivacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoEfetivacao"] ??
                            (
                                ViewState["VS_ltPermissaoEfetivacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Efetivacao)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de planejamento anual.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoPlanejamentoAnual
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoPlanejamentoAnual"] ??
                            (
                                ViewState["VS_ltPermissaoPlanejamentoAnual"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.PlanejamentoAnual)
                            )
                        );
            }
        }

        /// <summary>
        /// Carrega a turma disciplina relacionada (para as disciplinas de docencia compartilhada).
        /// </summary>
        private sTurmaDisciplinaRelacionada VS_turmaDisciplinaRelacionada
        {
            get
            {
                if (ViewState["VS_turmaDisciplinaRelacionada"] != null)
                    return (sTurmaDisciplinaRelacionada)ViewState["VS_turmaDisciplinaRelacionada"];
                return new sTurmaDisciplinaRelacionada();
            }

            set
            {
                ViewState["VS_turmaDisciplinaRelacionada"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
        /// </summary>
        protected int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] == null)
                {
                    ViewState["VS_cal_ano"] = ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_ano;
                }
                return Convert.ToInt32(ViewState["VS_cal_ano"]);
            }
        }

        /// <summary>
        /// ViewState que armazena o tipo de docente logado.
        /// </summary>
        private EnumTipoDocente VS_tipoDocente
        {
            get
            {
                return (EnumTipoDocente)(ViewState["VS_tipoDocente"] ?? 0);
            }

            set
            {
                ViewState["VS_tipoDocente"] = value;
            }
        }

        /// <summary>
        /// Guarda os eventos cadastrados para a turma e calendário.
        /// </summary>
        private List<ACA_Evento> VS_ListaEventos
        {
            get
            {
                return
                    (List<ACA_Evento>)
                    (
                        ViewState["VS_ListaEventos"] ??
                        (
                            ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(UCNavegacaoTelaPeriodo.VS_cal_id
                                                                                                  , UCControleTurma1.VS_tur_id
                                                                                                  , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                                                                                                  , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                                                                  , ApplicationWEB.AppMinutosCacheLongo
                                                                                                  , false)
                        )
                    );
            }
        }

        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool VS_Periodo_Aberto
        {
            get
            {
                DateTime dataAtual = DateTime.Now.Date;

                bool aberto = (dataAtual.Date <= UCNavegacaoTelaPeriodo.cap_dataFim.Date
                                && dataAtual.Date >= UCNavegacaoTelaPeriodo.cap_dataInicio.Date) ||
                                VS_ListaEventos.Exists(p => p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id &&
                                                            p.evt_dataInicio <= dataAtual.Date && p.evt_dataFim >= dataAtual.Date &&
                                                            p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS,
                                                                                                                                 __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                return aberto;
            }
        }

        /// <summary>
        /// Informa se o período já foi fechado (evento de fechamento já acabou) e não há nenhum evento de fechamento por vir.
        /// Se o período ainda estiver ativo então não verifica o evento de fechamento
        /// </summary>
        private bool VS_PeriodoEfetivado
        {
            get
            {
                bool efetivado = false;

                //Se o bimestre está ativo ou nem começou então não bloqueia pelo evento de fechamento
                if (DateTime.Today <= UCNavegacaoTelaPeriodo.cap_dataFim)
                    return false;

                //Só permite editar o bimestre se tiver evento ativo
                efetivado = !VS_ListaEventos.Exists(p => p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                                    DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

                return efetivado;
            }
        }

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        private bool usuarioPermissao
        {
            get
            {
                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                {
                    return false;
                }

                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// ViewState que armazena a situação da turma disciplina.
        /// </summary>
        private int VS_situacaoTurmaDisciplina
        {
            get
            {
                if (ViewState["VS_situacaoTurmaDisciplina"] != null)
                    return Convert.ToInt32(ViewState["VS_situacaoTurmaDisciplina"]);
                return 1;
            }

            set
            {
                ViewState["VS_situacaoTurmaDisciplina"] = value;
            }
        }

        private TUR_TurmaDisciplina EntTurmaDisciplina
        {
            get
            {
                return VS_EntitiesControleTurma.turmaDisciplina;
            }
        }

        private byte PosicaoDocente
        {
            get
            {
                return UCControleTurma1.VS_tdt_posicao;
            }
        }

        private bool PermiteLancarFrequencia
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoEdicao);
            }
        }

        private bool PermiteVisualizarFrequencia
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoConsulta);
            }
        }

        private bool PermiteLancarCompensacao
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoEdicao);
            }
        }

        private bool PermiteVisualizarCompensacao
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta);
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de turmasAnoAtual que indica se ha turmas ativas turma no ano atual
        /// </summary>
        protected bool VS_turmasAnoAtual
        {
            get
            {
                if (ViewState["VS_turmasAnoAtual"] == null)
                    return false;

                return Convert.ToBoolean(ViewState["VS_turmasAnoAtual"]);
            }
            set
            {
                ViewState["VS_turmasAnoAtual"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a data e hora que carregou a TurmaAula
        /// </summary>
        private DateTime VS_Data_Listao_TurmaAula
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Listao_TurmaAula"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Listao_TurmaAula"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena as aulas que foram salvas.
        /// </summary>
        private List<int> VS_lstTauSalvas
        {
            get
            {
                return (List<int>)(ViewState["VS_lstTauSalvas"] ?? new List<int>());
            }

            set
            {
                ViewState["VS_lstTauSalvas"] = value;
            }
        }

        /// <summary>
        /// Informa se irá recarregar a data da Aula após salvar os dados.
        /// </summary>
        private bool VS_recarregarDataAula
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_recarregarDataAula"] ?? false);
            }
            set
            {
                ViewState["VS_recarregarDataAula"] = value;
            }
        }

        private bool permiteEdicao;

        /// <summary>
        /// Armazena se a tela foi carregada pelo Historico de turmas.
        /// </summary>
        private bool VS_historico
        {
            get
            {
                if (ViewState["VS_historico"] != null)
                    return (bool)ViewState["VS_historico"];
                return false;
            }
            set
            {
                ViewState["VS_historico"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a página da frequência do listão.
        /// </summary>
        public int VS_paginaFreq
        {
            get
            {
                if (ViewState["VS_paginaFreq"] == null)
                    ViewState["VS_paginaFreq"] = 1;
                return Convert.ToInt32(ViewState["VS_paginaFreq"]);
            }

            set
            {
                ViewState["VS_paginaFreq"] = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno()
        {
            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Tud_idRetorno_ControleTurma", UCControleTurma1.VS_tud_id.ToString());
            listaDados.Add("Edit_tdt_posicao", UCControleTurma1.VS_tdt_posicao.ToString());
            listaDados.Add("Edit_esc_id", UCControleTurma1.VS_esc_id.ToString());
            listaDados.Add("Edit_uni_id", UCControleTurma1.VS_uni_id.ToString());
            listaDados.Add("Edit_tur_id", UCControleTurma1.VS_tur_id.ToString());
            listaDados.Add("Edit_tud_naoLancarNota", UCControleTurma1.VS_tud_naoLancarNota.ToString());
            listaDados.Add("Edit_tud_naoLancarFrequencia", UCControleTurma1.VS_tud_naoLancarFrequencia.ToString());
            listaDados.Add("Edit_tur_dataEncerramento", UCControleTurma1.VS_tur_dataEncerramento.ToString());
            listaDados.Add("Edit_tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
            listaDados.Add("Edit_tpc_ordem", UCNavegacaoTelaPeriodo.VS_tpc_ordem.ToString());
            listaDados.Add("Edit_cal_id", UCNavegacaoTelaPeriodo.VS_cal_id.ToString());
            listaDados.Add("TextoTurmas", UCControleTurma1.LabelTurmas);
            listaDados.Add("OpcaoAbaAtual", Convert.ToByte(UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual).ToString());
            listaDados.Add("Edit_tciIds", UCControleTurma1.VS_tciIds);
            listaDados.Add("Edit_tur_tipo", UCControleTurma1.VS_tur_tipo.ToString());
            listaDados.Add("Edit_tud_idAluno", UCControleTurma1.VS_tud_idAluno.ToString());
            listaDados.Add("Edit_tur_idNormal", UCControleTurma1.VS_tur_idNormal.ToString());
            listaDados.Add("PaginaRetorno", UCNavegacaoTelaPeriodo.VS_paginaRetorno);

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);
            if (VS_turmaDisciplinaRelacionada.tud_id > 0)
            {
                Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
            }
            Session["Historico"] = VS_historico;

            Session["tur_tud_ids"] = UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => String.Format("{0};{1}", p.tur_id, p.tud_id)).ToList();
        }

        /// <summary>
        /// Carrega dados da tela.
        /// </summary>
        private void CarregarTela()
        {
            try
            {
                VS_paginaFreq = 1;

                if (EntTurmaDisciplina.tud_naoLancarFrequencia)
                    lblMessage2.Text += UtilBO.GetErroMessage((string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " configurado(a) para não lançar frequência nessa turma.",
                                                             UtilBO.TipoMensagem.Alerta);

                hdbtnCompensacaoAusenciaVisible.Value = PermiteLancarCompensacao.ToString();
                UCLancamentoFrequencia.TextoMsgParecer = UtilBO.GetErroMessage(UCLancamentoFrequencia.TextoMsgParecer, UtilBO.TipoMensagem.Informacao);

                if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                {
                    switch (VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia)
                    {
                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia:
                            UCLancamentoFrequencia.TextoMsgParecer = UtilBO.GetErroMessage("Marque apenas os dias de aula que o aluno não assistiu.<br>" +
                                     "Marque a opção Efetivado para indicar que o lançamento de frequência do " +
                                     "dia foi finalizado e todas as ausências foram apontadas.", UtilBO.TipoMensagem.Informacao);
                            break;

                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula:
                            UCLancamentoFrequencia.TextoMsgParecer = UtilBO.GetErroMessage("Cada caixa de marcação corresponde a um tempo de aula.<br>" +
                                     "Marque apenas os tempos de aula que o aluno não assistiu em cada dia.<br/>" +
                                     "Marque a opção Efetivado para indicar que o lançamento de frequência do " +
                                     "dia foi finalizado e todas as ausências foram apontadas.", UtilBO.TipoMensagem.Informacao);
                            break;
                    }
                }

                btnCompensacaoAusencia.Visible = PermiteLancarCompensacao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0;
                permiteEdicao = false;
                bool esconderSalvar = false;
                pnlListaoLancamentoFrequencias.Visible = PermiteVisualizarFrequencia;
                if (pnlListaoLancamentoFrequencias.Visible)
                {
                    CarregarListaoFrequencia(true, false, false, false, ref esconderSalvar);
                }

                //mostra o botao salvar -> se o docente possuir essa turma ou se for turma extinta
                //e se o usuario possuir permissao para editar a frequencia ou avaliacao
                if (esconderSalvar)
                {
                    btnSalvar.Visible = btnSalvarCima.Visible = false;
                }
                else
                {
                    btnSalvar.Visible = btnSalvarCima.Visible = usuarioPermissao && !VS_PeriodoEfetivado
                                                                && (VS_situacaoTurmaDisciplina == 1
                                                                    || (VS_situacaoTurmaDisciplina != 1 && permiteEdicao)
                                                                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                                                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                                                                && PermiteLancarFrequencia;
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairListao", "var exibeMensagemSair=" + btnSalvar.Visible.ToString().ToLower() + ";", true);

                if (!pnlListaoLancamentoFrequencias.Visible)
                    divListao.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString()), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega dados de lançamento de frequência na tela.
        /// Só carrega caso a disciplina não seja do tipo
        /// complementação da regência.
        /// </summary>
        private void CarregarListaoFrequencia(bool atualizaData, bool proximo, bool anterior, bool inalterado, ref bool esconderSalvar)
        {
            try
            {
                // Só carrega a frequência caso a disciplina não seja do tipo
                // complementação da regência.
                if (EntTurmaDisciplina.tud_tipo !=
                    (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                {
                    int countAulas;
                    int pagina = VS_paginaFreq;

                    string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                                        string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                                        string.Empty;

                    UCLancamentoFrequencia.Carregar(proximo
                                                    , anterior
                                                    , inalterado
                                                    , VS_EntitiesControleTurma
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , UCNavegacaoTelaPeriodo.cap_dataInicio.Date
                                                    , UCNavegacaoTelaPeriodo.cap_dataFim.Date
                                                    , PosicaoDocente
                                                    , VS_tipoDocente
                                                    , VS_turmaDisciplinaRelacionada.tud_id
                                                    , PermiteVisualizarCompensacao
                                                    , VS_ltPermissaoFrequencia
                                                    , PermiteLancarFrequencia
                                                    , out countAulas
                                                    , VS_situacaoTurmaDisciplina
                                                    , ref permiteEdicao
                                                    , usuarioPermissao
                                                    , VS_PeriodoEfetivado
                                                    , VS_Periodo_Aberto
                                                    , ref esconderSalvar
                                                    , ref pagina
                                                    , VS_EntitiesControleTurma.curso.tne_id
                                                    , tur_ids);

                    VS_paginaFreq = pagina;
                    btnSalvar.TabIndex = Convert.ToInt16(countAulas);
                    hdbtnCompensacaoAusenciaVisible.Value = PermiteLancarCompensacao.ToString();

                    //Verifica se a aba que está aparecendo é a de frequência para mostrar o botão de compensação de ausência
                    //não usar btnCompensacaoAusencia.Visible = false; senão não funciona na troca de abas
                    if (PermiteLancarCompensacao &&
                        (string.IsNullOrEmpty(hdnListaoSelecionado.Value) || hdnListaoSelecionado.Value.Equals("0")))
                        btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "inline-block"); //mostra o botão assim para funcionar na troca de abas.
                    else
                        btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "none"); //esconde o botão assim para funcionar na troca de abas.
                }
                else
                {
                    // Caso a disciplina seja do tipo Complemento de regência, não lança frequência.
                    pnlListaoLancamentoFrequencias.Visible = btnCompensacaoAusencia.Visible = false;
                    UCLancamentoFrequencia.ValorHdnOrdenacao = "";
                }

                if (atualizaData)
                    VS_Data_Listao_TurmaAula = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString()), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                try
                {
                    if (PreviousPage == null && Session["DadosPaginaRetorno"] == null && Session["tud_id"] == null)
                    {
                        // Se não carregou nenhuma turma, redireciona pra busca.
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("É necessário selecionar uma turma.", UtilBO.TipoMensagem.Alerta);
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao ||
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                            RedirecionarPagina("~/Academico/ControleTurma/MinhaEscolaGestor.aspx");
                        else
                            RedirecionarPagina("~/Academico/ControleTurma/Busca.aspx");
                    }
                    else
                    {
                        List<Struct_MinhasTurmas> dadosTodasTurmas = new List<Struct_MinhasTurmas>();
                        long tud_idCompartilhada = -1;
                        if (Session["Historico"] != null)
                        {
                            VS_historico = Convert.ToBoolean(Session["Historico"]) && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0;
                            Session.Remove("Historico");
                        }
                        if (Session["TudIdCompartilhada"] != null)
                        {
                            tud_idCompartilhada = Convert.ToInt64(Session["TudIdCompartilhada"]);
                            Session.Remove("TudIdCompartilhada");
                        }
                        if (Session["tud_id"] != null && Session["tdt_posicao"] != null && Session["PaginaRetorno"] != null)
                        {
                            UCControleTurma1.VS_tud_id = Convert.ToInt64(Session["tud_id"]);
                            UCControleTurma1.VS_tdt_posicao = Convert.ToByte(Session["tdt_posicao"]);
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = Session["PaginaRetorno"].ToString();
                            if (Session["tur_tipo"] != null && Session["tur_idNormal"] != null && Session["tud_idAluno"] != null)
                            {
                                UCControleTurma1.VS_tur_tipo = Convert.ToByte(Session["tur_tipo"]);
                                UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(Session["tur_idNormal"]);
                                UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(Session["tud_idAluno"]);
                            }
                            if (VS_EntitiesControleTurma.escola == null)
                            {
                                ViewState["VS_EntitiesControleTurma"] = null;
                            }
                            UCControleTurma1.VS_esc_id = VS_EntitiesControleTurma.escola.esc_id;
                            UCControleTurma1.VS_uni_id = VS_EntitiesControleTurma.turma.uni_id;
                            UCControleTurma1.VS_tur_id = VS_EntitiesControleTurma.turma.tur_id;
                            UCControleTurma1.VS_tud_naoLancarNota = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarNota;
                            UCControleTurma1.VS_tud_naoLancarFrequencia = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarFrequencia;
                            UCControleTurma1.VS_tur_dataEncerramento = VS_EntitiesControleTurma.turma.tur_dataEncerramento;
                            UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;
                            UCControleTurma1.VS_tciIds = VS_EntitiesControleTurma.tciIds;
                            if (Session["VS_TpcId"] != null)
                                UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(Session["VS_TpcId"]);
                            if (Session["VS_TpcOrdem"] != null)
                                UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(Session["VS_TpcOrdem"]);
                        }
                        else if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                        {
                            UCControleTurma1.VS_tud_id = PreviousPage.Edit_tud_id;
                            UCControleTurma1.VS_tdt_posicao = PreviousPage.Edit_tdt_posicao;
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = PreviousPage.PaginaRetorno;
                            if (VS_EntitiesControleTurma.escola == null)
                            {
                                ViewState["VS_EntitiesControleTurma"] = null;
                            }
                            UCControleTurma1.VS_esc_id = VS_EntitiesControleTurma.escola.esc_id;
                            UCControleTurma1.VS_uni_id = VS_EntitiesControleTurma.turma.uni_id;
                            UCControleTurma1.VS_tur_id = VS_EntitiesControleTurma.turma.tur_id;
                            UCControleTurma1.VS_tud_naoLancarNota = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarNota;
                            UCControleTurma1.VS_tud_naoLancarFrequencia = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarFrequencia;
                            UCControleTurma1.VS_tur_dataEncerramento = VS_EntitiesControleTurma.turma.tur_dataEncerramento;
                            UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;
                            UCControleTurma1.VS_tciIds = VS_EntitiesControleTurma.tciIds;
                            UCControleTurma1.VS_tur_tipo = VS_EntitiesControleTurma.turma.tur_tipo;
                        }
                        else if (Session["DadosPaginaRetorno"] != null)
                        {
                            Dictionary<string, string> listaDados = (Dictionary<string, string>)Session["DadosPaginaRetorno"];
                            UCControleTurma1.VS_tud_id = Convert.ToInt64(listaDados["Tud_idRetorno_ControleTurma"]);
                            UCControleTurma1.VS_tdt_posicao = Convert.ToByte(listaDados["Edit_tdt_posicao"]);
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = listaDados["PaginaRetorno"].ToString();
                            UCControleTurma1.VS_esc_id = Convert.ToInt32(listaDados["Edit_esc_id"]);
                            UCControleTurma1.VS_uni_id = Convert.ToInt32(listaDados["Edit_uni_id"]);
                            UCControleTurma1.VS_tur_id = Convert.ToInt64(listaDados["Edit_tur_id"]);
                            UCControleTurma1.VS_tud_naoLancarNota = Convert.ToBoolean(listaDados["Edit_tud_naoLancarNota"]);
                            UCControleTurma1.VS_tud_naoLancarFrequencia = Convert.ToBoolean(listaDados["Edit_tud_naoLancarFrequencia"]);
                            UCControleTurma1.VS_tur_dataEncerramento = Convert.ToDateTime(listaDados["Edit_tur_dataEncerramento"]);
                            UCNavegacaoTelaPeriodo.VS_cal_id = Convert.ToInt32(listaDados["Edit_cal_id"]);
                            UCControleTurma1.VS_tciIds = listaDados["Edit_tciIds"];
                            UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(listaDados["Edit_tpc_id"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(listaDados["Edit_tpc_ordem"]);
                            UCControleTurma1.VS_tur_tipo = Convert.ToByte(listaDados["Edit_tur_tipo"]);
                            UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(listaDados["Edit_tud_idAluno"]);
                            UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(listaDados["Edit_tur_idNormal"]);
                            UCControleTurma1.VS_tur_tud_ids = (List<string>)(Session["tur_tud_ids"] ?? new List<string>());
                            UCControleTurma1.LabelTurmas = listaDados["TextoTurmas"];
                        }

                        // Remove os dados que possam estar na sessao
                        Session.Remove("tud_id");
                        Session.Remove("tdt_posicao");
                        Session.Remove("PaginaRetorno");
                        Session.Remove("DadosPaginaRetorno");
                        Session.Remove("VS_DadosTurmas");
                        Session.Remove("VS_TpcId");
                        Session.Remove("tur_tipo");
                        Session.Remove("tur_idNormal");
                        Session.Remove("tud_idAluno");
                        Session.Remove("tur_tud_ids");
                        //

                        List<Struct_MinhasTurmas.Struct_Turmas> dadosTurma = new List<Struct_MinhasTurmas.Struct_Turmas>();

                        // Se for perfil Administrador
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                        {
                            dadosTodasTurmas.Add
                            (
                                new Struct_MinhasTurmas
                                {
                                    Turmas = TUR_TurmaBO.SelecionaMinhasTurmasComboPorTurId
                                                             (
                                                                 VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.MultisseriadaDocente
                                                                 ? UCControleTurma1.VS_tur_idNormal : UCControleTurma1.VS_tur_id,
                                                                 ApplicationWEB.AppMinutosCacheCurto
                                                             )
                                }
                            );

                            // Não busca pela posição
                            dadosTodasTurmas.All(p =>
                            {
                                dadosTurma.AddRange(p.Turmas.Where(t => t.tud_id == UCControleTurma1.VS_tud_id));
                                return true;
                            });

                            UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                        }
                        else
                        {
                            dadosTodasTurmas = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto, false);

                            dadosTodasTurmas.All(p =>
                            {
                                dadosTurma.AddRange(p.Turmas.Where(t => t.tud_id == UCControleTurma1.VS_tud_id && t.tdt_posicao == UCControleTurma1.VS_tdt_posicao));
                                return true;
                            });

                            VS_situacaoTurmaDisciplina = dadosTurma.FirstOrDefault().tdt_situacao;

                            UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                        }

                        VS_turmasAnoAtual = dadosTurma.FirstOrDefault().turmasAnoAtual;

                        hdnListaoSelecionado.Value = "0";

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.Frequencia;

                        // Carrega o combo de disciplinas e seta o valor selecionado.
                        List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmas = new List<Struct_MinhasTurmas.Struct_Turmas>();

                        dadosTodasTurmas.All(p =>
                        {
                            dadosTurmas.AddRange(p.Turmas);
                            return true;
                        });

                        // Carrega combo de turmas
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                        {
                            List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasCombo = TUR_TurmaBO.SelecionaMinhasTurmasComboPorTurId
                            (
                                VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.MultisseriadaDocente
                                ? UCControleTurma1.VS_tur_idNormal : UCControleTurma1.VS_tur_id,
                                ApplicationWEB.AppMinutosCacheCurto
                            );

                            UCControleTurma1.CarregaTurmas(dadosTurmasCombo, UCNavegacaoTelaPeriodo.VS_cal_id, VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico);
                        }
                        else
                        {
                            List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasCombo = new List<Struct_MinhasTurmas.Struct_Turmas>();

                            if (VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && VS_situacaoTurmaDisciplina == 1)
                            {
                                // dadosTurmasAtivas
                                dadosTurmasCombo = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dadosTodasTurmas, 0);
                            }
                            else
                            {
                                dadosTurmasCombo = dadosTurmas;
                            }

                            UCControleTurma1.CarregaTurmas(dadosTurmasCombo, UCNavegacaoTelaPeriodo.VS_cal_id, VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico);
                        }

                        TUR_TurmaDisciplina entDisciplinaRelacionada = null;
                        if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                        {
                            List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                        : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo);
                            bool docenciaCompartilhadaOk = false;
                            if (lstDisciplinaCompartilhada.Count > 0)
                            {
                                if (tud_idCompartilhada <= 0 || !lstDisciplinaCompartilhada.Any(p => p.tud_id == tud_idCompartilhada))
                                {
                                    tud_idCompartilhada = lstDisciplinaCompartilhada[0].tud_id;
                                }

                                if (tud_idCompartilhada > 0)
                                {
                                    docenciaCompartilhadaOk = true;
                                    entDisciplinaRelacionada = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_idCompartilhada });
                                    VS_turmaDisciplinaRelacionada = lstDisciplinaCompartilhada.Find(p => p.tud_id == tud_idCompartilhada);
                                    UCControleTurma1.CarregarDisciplinaCompartilhada(lstDisciplinaCompartilhada, VS_turmaDisciplinaRelacionada.tud_id, VS_turmaDisciplinaRelacionada.tdr_id);
                                }
                            }

                            if (!docenciaCompartilhadaOk)
                            {
                                __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("{0} {1} - {2}.",
                                                                                    GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                                    , VS_EntitiesControleTurma.turma.tur_codigo
                                                                                    , VS_EntitiesControleTurma.turmaDisciplina.tud_nome)
                                                                                , UtilBO.TipoMensagem.Alerta);
                                RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                            }
                        }

                        UCNavegacaoTelaPeriodo.CarregarPeriodos(VS_ltPermissaoFrequencia, VS_ltPermissaoEfetivacao,
                                        VS_ltPermissaoPlanejamentoAnual, VS_ltPermissaoAvaliacao,
                                        entDisciplinaRelacionada, UCControleTurma1.VS_esc_id,
                                        VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, UCControleTurma1.VS_tdt_posicao, UCControleTurma1.VS_tur_id, VS_EntitiesControleTurma.turmaDisciplina.tud_id);

                        if (UCNavegacaoTelaPeriodo.VS_tpc_id <= 0)
                        {
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Escola não permite lançar dados.", UtilBO.TipoMensagem.Alerta);
                            RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                        }

                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);
                        CarregarTela();

                        //bloquear botoes -> se o docente nao possuir mais essa turma e se nao for turma extinta
                        if (VS_situacaoTurmaDisciplina != 1
                            && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Encerrada
                            && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Extinta)
                        {
                            //UCNavegacaoTelaPeriodo.VisiblePlanejamentoAnual = false;
                            //UCNavegacaoTelaPeriodo.VisibleEfetivacao = false;
                            btnCompensacaoAusencia.Visible = false;
                        }
                    }

                    bool mudaCorTitulo = VS_cal_ano < DateTime.Now.Year && VS_turmasAnoAtual && VS_EntitiesControleTurma.turma.tur_situacao == 1;

                    UCControleTurma1.CorTituloTurma = mudaCorTitulo ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black;
                    divMessageTurmaAnterior.Visible = mudaCorTitulo;
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }

            UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
            UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
            UCControleTurma1.DisciplinaCompartilhadaIndexChanged = uccDisciplinaCompartilhada_IndexChanged;
            UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
            UCLancamentoFrequencia.Recarregar += UCLancamentoFrequencia_Recarregar;
            UCLancamentoFrequencia.CarregarAusencias += UCLancamentoFrequencia_CarregarAusencias;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            UCControleTurma1.chkTurmasNormaisMultisseriadasIndexChanged += UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged;


            // Configura javascripts da tela.
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));

                string script;
                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                    script = "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
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

                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmerEfetivacao.js"));
                }
                else
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                }

                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tablesorter.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.metadata.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsControleTurma_Listao.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));
            }

            UCLancamentoFrequencia.VisivelAlunoDispensado = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (VS_PeriodoEfetivado)
            {
                lblPeriodoEfetivado.Visible = true;
                lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Frequencia.MensagemEfetivado").ToString(),
                                                                 UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    UCLancamentoFrequencia.CarregarLegenda();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        private void uccTurmaDisciplina_IndexChanged()
        {
            try
            {
                string[] valor = UCControleTurma1.ValorTurmas.Split(';');
                if (valor.Length > 4)
                {
                    byte tud_tipo = Convert.ToByte(valor[4]);
                    bool dialogDocCompartilhada = false;
                    if (tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                    {
                        long tud_id = Convert.ToInt64(valor[1]);
                        List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                    : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo);
                        if (lstDisciplinaCompartilhada.Count > 1)
                        {
                            UCSelecaoDisciplinaCompartilhada1.AbrirDialog(tud_id, VS_historico ? __SessionWEB.__UsuarioWEB.Docente.doc_id : 0, UCControleTurma1.TextoSelecionadoTurmas);
                            dialogDocCompartilhada = true;
                            hdnValorTurmas.Value = UCControleTurma1.ValorTurmas;
                        }
                    }
                    if (!dialogDocCompartilhada)
                    {
                        Session["tud_id"] = valor[1].ToString();
                        Session["tdt_posicao"] = valor[3].ToString();
                        Session["PaginaRetorno"] = UCNavegacaoTelaPeriodo.VS_paginaRetorno;
                        Session["VS_TpcId"] = UCNavegacaoTelaPeriodo.VS_tpc_id;
                        Session["VS_TpcOrdem"] = UCNavegacaoTelaPeriodo.VS_tpc_ordem;

                        if (valor.Length > 7)
                        {
                            Session["tur_tipo"] = valor[5].ToString();
                            Session["tur_idNormal"] = valor[6].ToString();
                            Session["tud_idAluno"] = valor[7].ToString();
                        }

                        if (VS_turmaDisciplinaRelacionada.tud_id > 0)
                        {
                            Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
                        }
                        Session["Historico"] = VS_historico;
                        Response.Redirect("~/Academico/ControleTurma/Frequencia.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void uccDisciplinaCompartilhada_IndexChanged()
        {
            try
            {
                string[] valor = UCControleTurma1.ValorDisciplinaCompartilhada.Split(';');
                if (valor.Length > 0)
                {
                    VS_turmaDisciplinaRelacionada = new sTurmaDisciplinaRelacionada { tud_id = Convert.ToInt64(valor[0]) };
                    CarregaSessionPaginaRetorno();
                    Response.Redirect("~/Academico/ControleTurma/Frequencia.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina(long tud_id)
        {
            try
            {
                string[] valor = hdnValorTurmas.Value.Split(';');
                if (valor.Length > 4)
                {
                    Session["tud_id"] = valor[1].ToString();
                    Session["tdt_posicao"] = valor[3].ToString();
                    Session["PaginaRetorno"] = UCNavegacaoTelaPeriodo.VS_paginaRetorno;
                    Session["VS_TpcId"] = UCNavegacaoTelaPeriodo.VS_tpc_id;
                    Session["VS_TpcOrdem"] = UCNavegacaoTelaPeriodo.VS_tpc_ordem;
                    Session["TudIdCompartilhada"] = tud_id.ToString();
                    Session["Historico"] = VS_historico;

                    Session["tur_tipo"] = valor[5].ToString();
                    Session["tur_idNormal"] = valor[6].ToString();
                    Session["tud_idAluno"] = valor[7].ToString();

                    Response.Redirect("~/Academico/ControleTurma/Frequencia.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged()
        {
            try
            {
                CarregarTela();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Recarrega o grid do listao de frequencia.
        /// </summary>
        /// <param name="atualizaData"></param>
        /// <param name="proximo"></param>
        /// <param name="anterior"></param>
        /// <param name="inalterado"></param>
        private void UCLancamentoFrequencia_Recarregar(bool atualizaData, bool proximo, bool anterior, bool inalterado)
        {
            bool esconderSalvar = false;
            CarregarListaoFrequencia(atualizaData, proximo, anterior, inalterado, ref esconderSalvar);
            if (esconderSalvar)
                btnSalvar.Visible = btnSalvarCima.Visible = false;
        }

        /// <summary>
        /// Carrega as ausências pelos filtros informados.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="mtd_id"></param>
        void UCLancamentoFrequencia_CarregarAusencias(long alu_id, int mtu_id, int mtd_id)
        {
            try
            {
                gvCompAusencia.PageIndex = 0;
                gvCompAusencia.DataSourceID = odsCompAusencia.ID;
                odsCompAusencia.SelectParameters.Clear();
                odsCompAusencia.SelectParameters.Add("tud_id", EntTurmaDisciplina.tud_id.ToString());
                odsCompAusencia.SelectParameters.Add("tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
                odsCompAusencia.SelectParameters.Add("alu_id", alu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtu_id", mtu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtd_id", mtd_id.ToString());

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                gvCompAusencia.PageSize = itensPagina;
                // Seta nome da coluna periodos
                gvCompAusencia.Columns[2].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                // atualiza o grid
                gvCompAusencia.DataBind();
                fdsResultados.Visible = true;
                UCComboQtdePaginacao1.Visible = gvCompAusencia.Rows.Count > 0;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConsultaCompensacao", "$(document).ready(function() { $('#divCompensacao').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações de ausências.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnCompensacao.Update();
            }
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // atribui nova quantidade itens por página para o grid
                gvCompAusencia.PageSize = UCComboQtdePaginacao1.Valor;
                gvCompAusencia.PageIndex = 0;
                // atualiza o grid
                gvCompAusencia.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string msg = "";
            VS_recarregarDataAula = true;

            //Salva frequência
            try
            {
                if (Page.IsValid && pnlListaoLancamentoFrequencias.Visible && PermiteLancarFrequencia)
                {
                    List<int> lstTauSalvas = VS_lstTauSalvas;
                    bool recarregarDataAula = VS_recarregarDataAula;
                    UCLancamentoFrequencia.Salvar(out msg
                                                    , VS_PeriodoEfetivado
                                                    , VS_Periodo_Aberto
                                                    , ref lstTauSalvas
                                                    , VS_EntitiesControleTurma
                                                    , VS_Data_Listao_TurmaAula
                                                    , ref recarregarDataAula
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , PosicaoDocente
                                                    , PermiteLancarFrequencia
                                                    , VS_situacaoTurmaDisciplina);
                    VS_lstTauSalvas = lstTauSalvas;
                    VS_recarregarDataAula = recarregarDataAula;
                }
            }
            catch (ValidationException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage(string.Format("Erro ao tentar salvar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString().ToLower()), UtilBO.TipoMensagem.Erro);
            }

            if (VS_recarregarDataAula)
                VS_Data_Listao_TurmaAula = DateTime.Now.AddSeconds(1);

            if (!string.IsNullOrEmpty(msg))
                lblMessage.Text += msg;
        }

        protected void btnCompensacaoAusencia_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Add("PaginaRetorno_CompensacaoAusencia", "~/Academico/ControleTurma/Frequencia.aspx");
                CarregaSessionPaginaRetorno();

                Response.Redirect("~/Classe/CompensacaoAusencia/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void gvCompAusencia_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CLS_CompensacaoAusenciaBO.GetTotalRecords();
        }

        #endregion Eventos
    }
}