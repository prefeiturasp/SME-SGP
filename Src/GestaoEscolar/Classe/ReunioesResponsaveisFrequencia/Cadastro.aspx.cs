using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;

public partial class Classe_ReunioesResponsaveisFrequencia_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna a tur_id da busca
    /// </summary>
    private int VS_alu_id
    {
        get
        {
            if (ViewState["VS_alu_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_alu_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_alu_id"] = value;
        }
    }

    /// <summary>
    /// Retorna a tur_id da busca
    /// </summary>
    private int VS_tur_id
    {
        get
        {
            if (ViewState["VS_tur_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tur_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_tur_id"] = value;
        }
    }

    /// <summary>
    /// Retorna a cal_id da busca
    /// </summary>
    private int VS_cal_id
    {
        get
        {
            if (ViewState["VS_cal_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_cal_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_cal_id"] = value;
        }
    }

    /// <summary>
    /// Retorna a cap_id da busca
    /// </summary>
    private int VS_cap_id
    {
        get
        {
            return cadastroReunioesPorPeriodo ? Convert.ToInt32(ViewState["VS_cap_id"] ?? "-1") : UCComboPeriodoCalendario1.Cap_ID;
        }

        set
        {
            ViewState["VS_cap_id"] = value;
        }
    }

    /// <summary>
    /// DataTable de efetivacoes
    /// </summary>
    private DataTable dtEfetivacoes;

    /// <summary>
    /// Retorna a qtd_reunioes
    /// </summary>
    private int VS_qtd_reunioes
    {
        get
        {
            if (ViewState["VS_qtd_reunioes"] != null)
                return Convert.ToInt32(ViewState["VS_qtd_reunioes"]);
            return -1;
        }
        set
        {
            ViewState["VS_qtd_reunioes"] = value;
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
                        ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                    )
                );
        }
    }

    /// <summary>
    /// Guarda a lista com os calendario periodo do calendario da turma.
    /// </summary>
    private List<sCalendarioPeriodo> VS_ListCalendarioPeriodo
    {
        get
        {
            if (ViewState["VS_ListCalendarioPeriodo"] == null || !((List<sCalendarioPeriodo>)ViewState["VS_ListCalendarioPeriodo"]).Any())
            {
                List<Struct_CalendarioPeriodos> dt = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);

                ViewState["VS_ListCalendarioPeriodo"] = (from dr in dt.AsEnumerable()
                                                         select new sCalendarioPeriodo
                                                         {
                                                             cap_id = dr.cap_id
                                                             ,
                                                             tpc_id = dr.tpc_id
                                                             ,
                                                             cap_dataInicio = dr.cap_dataInicio
                                                             ,
                                                             cap_dataFim = dr.cap_dataFim
                                                         }).Distinct().ToList();
            }

            return (List<sCalendarioPeriodo>)ViewState["VS_ListCalendarioPeriodo"];
        }
    }

    /// <summary>
    /// Propriedade que indica o COC está fechado para lançamento.
    /// </summary>
    private bool periodoAberto
    {
        get
        {
            sCalendarioPeriodo entCap = new sCalendarioPeriodo { };

            if (cadastroReunioesPorPeriodo)
                entCap = VS_ListCalendarioPeriodo.Find(p => p.cap_id == VS_cap_id);
            else
                entCap = VS_ListCalendarioPeriodo.Find(p => p.tpc_id == UCComboPeriodoCalendario1.Tpc_ID);

            if (entCap.cap_id > 0)
            {
                return (DateTime.Now <= entCap.cap_dataFim && DateTime.Now >= entCap.cap_dataInicio) ||
                                VS_ListaEventos.Exists(p => p.tpc_id == entCap.tpc_id &&
                                                             p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            }

            return false;
        }
    }

    /// <summary>
    /// Flag que indica se o curso da turma selecionada tem regime de matrícula
    /// "Seriado por avaliações" (EJA).
    /// </summary>
    private bool VS_CursoSeriadoAvaliacoes
    {
        get
        {
            if (ViewState["VS_CursoSeriadoAvaliacoes"] == null)
            {
                return false;
            }

            return Convert.ToBoolean(ViewState["VS_CursoSeriadoAvaliacoes"]);
        }

        set
        {
            ViewState["VS_CursoSeriadoAvaliacoes"] = value;
        }
    }

    /// <summary>
    /// Nome da avaliação (UP)
    /// </summary>
    private string VS_crp_nomeAvaliacao
    {
        get
        {
            return (ViewState["VS_crp_nomeAvaliacao"] ?? string.Empty).ToString();
        }

        set
        {
            ViewState["VS_crp_nomeAvaliacao"] = value;
        }
    }

    /// <summary>
    /// Parâmetro acadêmico que indica se o cadastro de reunião de responáveis é por período do calendário.
    /// </summary>
    private bool cadastroReunioesPorPeriodo
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CADASTRAR_REUNIOES_POR_PERIODO_CALENDARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    #endregion Propriedades

    #region Estruturas

    /// <summary>
    /// Struct para guardar os dados utilizados do Calendario Periodo na tela
    /// </summary>
    [Serializable]
    public struct sCalendarioPeriodo
    {
        public int cap_id;
        public int tpc_id;
        public DateTime cap_dataInicio;
        public DateTime cap_dataFim;
    }

    #endregion Estruturas

    #region Delegates

    private void UCComboPeriodoCalendario1_IndexChanged()
    {
        CarregarTelaLancametoFrequencia();
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// Habilita os controles da tela
    /// </summary>
    /// <param name="value">Flag que indica se o usuário possui permissão de alteração</param>
    private void HabilitarControlesTela(bool value)
    {
        HabilitaControles(pnlLancamentoFrequencias.Controls, value);

        _btnSalvar.Visible = value;
        _btnSalvar2.Visible = value;

        if (value)
        {
            _btnCancelar.Text = "Cancelar";
            _btnCancelar2.Text = "Cancelar";
        }
        else
        {
            _btnCancelar.Text = "Voltar";
            _btnCancelar2.Text = "Voltar";
        }

        _UCComboOrdenacao1._Combo.Enabled = true;
    }

    /// <summary>
    /// Carrega dados na tela.
    /// </summary>
    private void CarregarTelaLancametoFrequencia()
    {
        try
        {
            if (VerificaPeriodoSelecionado())
            {
                // Carregar repeater de alunos.
                dtEfetivacoes = CLS_FrequenciaReuniaoBO.SelecionaEfetivacaoDeReuniaoDeResponsaveis(VS_tur_id, VS_cal_id, VS_cap_id);
                rptAlunos.DataSource = MTR_MatriculaTurmaBO.SelecionaAlunosPorTurmaPeriodoCalendario(
                    VS_tur_id,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    VS_cal_id,
                    VS_cap_id,
                    1);
                rptAlunos.DataBind();

                //Fazendo as validações após carregar os dados.
                if (rptAlunos.Items.Count == 0)
                {
                    EscondeGridAlunos("Não foram encontrados alunos na turma selecionada.");
                    chkTodas.Visible = false;
                }
                else
                {
                    pnlLancamentoFrequencias.Visible = true;

                    RepeaterItem header = (RepeaterItem)rptAlunos.Controls[0];
                    Repeater rptReunioes = (Repeater)header.FindControl("rptReunioes");

                    lblMsgParecer.Visible = rptReunioes.Items.Count > 0;

                    _lblMsgRepeater.Visible = rptReunioes.Items.Count == 0;

                    rptAlunos.Visible = true;

                    if (rptReunioes.Items.Count == 0)
                    {
                        EscondeGridAlunos("Não foram encontradas reuniões para a turma.");
                        chkTodas.Visible = false;
                    }
                }

                HabilitarControlesTela(__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && periodoAberto);
                ChecharFrequencias();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as frequências.", UtilBO.TipoMensagem.Erro);
            _uppMessage.Update();
        }
    }

    /// <summary>
    /// Verifica se tem período selecionado e esconde a tela caso não haja.
    /// </summary>
    /// <returns>Se existe um período selecionado</returns>
    private bool VerificaPeriodoSelecionado()
    {
        bool temPeriodo = cadastroReunioesPorPeriodo ? VS_cap_id > 0 : UCComboPeriodoCalendario1.Tpc_ID > 0;

        pnlLancamentoFrequencias.Visible = temPeriodo;
        _btnSalvar.Visible = _btnSalvar2.Visible = _btnImprimir.Visible = temPeriodo;
        _btnCancelar.Text = _btnCancelar2.Text = (temPeriodo ? "Cancelar" : "Voltar");

        // Caso o combo Tipo Periodo Calendario estiver vazio.
        if (!temPeriodo)
        {
            if (UCComboPeriodoCalendario1.ExisteAlgumItem())
            {
                _lblMessage.Text =
                    UtilBO.GetErroMessage(
                        "É necessário selecionar um " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ".",
                        UtilBO.TipoMensagem.Alerta);
            }
            else
            {
                // Caso o combo Tipo Periodo Calendario estiver vazio.
                _lblMessage.Text =
                    UtilBO.GetErroMessage("Não existe " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                          " cadastrado para a turma e " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionados(as).",
                                          UtilBO.TipoMensagem.Alerta);
            }
        }

        return temPeriodo;
    }

    /// <summary>
    /// Esconde o grid de alunos, e mostra a mensagem do parâmetro no lugar dele.
    /// </summary>
    /// <param name="msg">Mensagem a ser mostrada ao usuário</param>
    private void EscondeGridAlunos(string msg)
    {
        bool mostra = !string.IsNullOrEmpty(msg);

        pnlLancamentoFrequencias.Visible = mostra;
        _UCComboOrdenacao1.Visible = false;
        rptAlunos.Visible = false;
        lblMsgParecer.Visible = false;
        _lblMsgRepeater.Visible = mostra;
        _lblMsgRepeater.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
    }

    /// <summary>
    /// Checa as frequencias dos alunos
    /// </summary>
    private void ChecharFrequencias()
    {
        foreach (RepeaterItem itemAluno in rptAlunos.Items)
        {
            Repeater rptReunioes = (Repeater)itemAluno.FindControl("rptReunioes");
            Int64 alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
            CheckBox checkAll = (CheckBox)itemAluno.FindControl("checkAll");
            int count = 0;

            // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
            foreach (RepeaterItem alunos in rptReunioes.Items)
            {
                int reu_id = Convert.ToInt32(((Label)alunos.FindControl("lblreu_id")).Text);
                CheckBox chkFrequencia = (CheckBox)alunos.FindControl("chkFrequencia");

                CLS_FrequenciaReuniaoResponsaveis ent = new CLS_FrequenciaReuniaoResponsaveis
                {
                    alu_id = alu_id
                        ,
                    cal_id = VS_cal_id
                        ,
                    cap_id = VS_cap_id
                        ,
                    frp_id = reu_id
                };
                CLS_FrequenciaReuniaoResponsaveisBO.GetEntity(ent);

                if (ent.frp_frequencia)
                {
                    chkFrequencia.Checked = true;
                    count++;
                }
                checkAll.Checked = (count == rptReunioes.Items.Count);
            }
        }
    }

    /// <summary>
    /// Salva no banco as frequências.
    /// </summary>
    public bool Salvar(bool PermaneceTela)
    {
        try
        {
            if (!periodoAberto)
                throw new ValidationException(String.Format("Lançamento de frequência em reunião de pais e responsáveis {0} disponível apenas para consulta.", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

            List<CLS_FrequenciaReuniao> listfr = new List<CLS_FrequenciaReuniao>();
            List<CLS_FrequenciaReuniaoResponsaveis> listfrp = new List<CLS_FrequenciaReuniaoResponsaveis>();

            RepeaterItem header = (RepeaterItem)rptAlunos.Controls[0];
            Repeater rptReunioes = (Repeater)header.FindControl("rptReunioes");

            bool pCadastroPorPeriodo = cadastroReunioesPorPeriodo;

            // Adiciona itens na lista de TurmaAula - só pra alterar o tau_efetivado.
            foreach (RepeaterItem itemAtividade in rptReunioes.Items)
            {
                CheckBox chkEfetivado = (CheckBox)itemAtividade.FindControl("chkEfetivado");
                int reu_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lblreu_id")).Text);

                CLS_FrequenciaReuniao ent = new CLS_FrequenciaReuniao
                {
                    tur_id = VS_tur_id,
                    cal_id = VS_cal_id,
                    cap_id = VS_cap_id,
                    frp_id = reu_id
                };
                CLS_FrequenciaReuniaoBO.GetEntity(ent);

                ent.frr_efetivado = chkEfetivado.Checked;

                listfr.Add(ent);
            }

            foreach (RepeaterItem itemAluno in rptAlunos.Items)
            {
                rptReunioes = (Repeater)itemAluno.FindControl("rptReunioes");
                Int64 alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);

                // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                foreach (RepeaterItem alunos in rptReunioes.Items)
                {
                    int reu_id = Convert.ToInt32(((Label)alunos.FindControl("lblreu_id")).Text);
                    CheckBox chkFrequencia = (CheckBox)alunos.FindControl("chkFrequencia");
                    CheckBox chkEfetivado = (CheckBox)alunos.FindControl("chkEfetivado");
                    bool frequencia = chkFrequencia.Checked;

                    if (reu_id > 0)
                    {
                        CLS_FrequenciaReuniaoResponsaveis ent = new CLS_FrequenciaReuniaoResponsaveis
                        {
                            alu_id = alu_id
                                ,
                            cal_id = VS_cal_id
                                ,
                            cap_id = pCadastroPorPeriodo ? VS_cap_id : UCComboPeriodoCalendario1.Cap_ID
                                ,
                            frp_id = reu_id
                                ,
                            frp_frequencia = frequencia
                        };

                        listfrp.Add(ent);
                    }
                }
            }

            if (CLS_FrequenciaReuniaoResponsaveisBO.Salvar(listfrp, listfr))
            {
                if (PermaneceTela)
                {
                    CarregarTelaLancametoFrequencia();
                    _lblMessage.Text = UtilBO.GetErroMessage("Lançamento de frequências salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Lançamento de frequências salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    Response.Redirect("~/Classe/ReunioesResponsaveisFrequencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tur_id: " + VS_tur_id + "; cal_id: " + VS_cal_id + "; cap_id: " + VS_cap_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }

                return true;
            }

            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar as frequências.", UtilBO.TipoMensagem.Erro);
            return false;
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            return false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar as frequências.", UtilBO.TipoMensagem.Erro);
            _uppMessage.Update();
            return false;
        }
    }

    /// <summary>
    /// Verifica se as regras do curso estão sendo cumpridas.
    /// Quando o regime de matrícula é Seriado por avaliações, o formato tem que
    /// ser do tipo Conceito Global e a avaliação selecionada tem que ser do tipo
    /// Periódica ou Periódica + Final.
    /// </summary>
    private void VerificaRegrasCurso(TUR_Turma entityTurma, ACA_FormatoAvaliacao entityFormatoAvaliacao)
    {
        ACA_CurriculoPeriodo entCurPeriodo;
        bool Seriado;

        if (TUR_TurmaCurriculoBO.ValidaCursoSeriadoAvaliacao(entityTurma, entityFormatoAvaliacao, out entCurPeriodo, out Seriado) && Seriado)
        {
            VS_crp_nomeAvaliacao = GestaoEscolarUtilBO.nomePadraoPeriodoAvaliacao(entCurPeriodo.crp_nomeAvaliacao);
        }

        VS_CursoSeriadoAvaliacoes = Seriado && entCurPeriodo.crp_turmaAvaliacao;
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsgParecer.Text = "Marque apenas as reuniões em que o responsável esteve presente.<br>";

        //"Marque a opção Efetivado para indicar que o lançamento de frequência do " +
        //"dia foi finalizado e todas as ausências foram apontadas.";
        lblMsgParecer.Text = UtilBO.GetErroMessage(lblMsgParecer.Text, UtilBO.TipoMensagem.Informacao);
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryFixer));
            RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));

            // A ordem dos 2 scripts abaixo não deve ser alterada - se for, as máscaras
            // dos campos não vai funcionar, pois no primeiro script ele "refaz" as tabelas
            // com o JQuery.Fixer, e por isso não adianta setar as máscaras antes.
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroFrequenciaReuniaoResponsaveis.js"));

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                if (!Convert.ToString(_btnCancelar.CssClass).Contains("btnMensagemUnload"))
                {
                    _btnCancelar.CssClass += " btnMensagemUnload";
                }

                if (!Convert.ToString(_btnCancelar2.CssClass).Contains("btnMensagemUnload"))
                {
                    _btnCancelar2.CssClass += " btnMensagemUnload";
                }
            }
        }

        HabilitarControlesTela(__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!string.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _carregaComboHora();
            _carregaComboMinuto();
            if (Session["tur_idFrequencia"] != null)
            {
                VS_tur_id = Convert.ToInt32(Session["tur_idFrequencia"]);

                Session.Remove("tur_idFrequencia");

                TUR_Turma tur = new TUR_Turma
                {
                    tur_id = VS_tur_id
                };
                TUR_TurmaBO.GetEntity(tur);

                ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao
                {
                    fav_id = tur.fav_id
                };
                ACA_FormatoAvaliacaoBO.GetEntity(fav);

                ESC_Escola entEscola = ESC_EscolaBO.GetEntity(
                    new ESC_Escola { esc_id = tur.esc_id });

                List<TUR_TurmaCurriculo> crrTur = TUR_TurmaCurriculoBO.GetSelectBy_Turma(VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);

                ACA_Curso cur = new ACA_Curso
                {
                    cur_id = crrTur[0].cur_id
                };
                ACA_CursoBO.GetEntity(cur);

                VS_cal_id = tur.cal_id;

                if (Session["cap_idFrequencia"] != null && cadastroReunioesPorPeriodo)
                {
                    VS_cap_id = Convert.ToInt32(Session["cap_idFrequencia"]);
                    Session.Remove("tur_idFrequencia");
                }

                ACA_CursoReunioes crn = ACA_CursoReunioesBO.SelecionaPorCursoCalendarioPeriodo
                (
                    cur.cur_id
                    ,
                    crrTur[0].crr_id
                    ,
                    VS_cal_id
                    ,
                    cadastroReunioesPorPeriodo ? VS_cap_id : -1
                );

                VS_qtd_reunioes = crn.crn_qtde;

                string esc_nome = entEscola.esc_nome;

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    esc_nome = entEscola.esc_codigo + " - " + esc_nome;
                }

                lblTurma.Text = "Escola: <b>" + esc_nome + "</b><br />";
                lblTurma.Text += "Turma: <b>" + tur.tur_codigo + "</b>";
                lblCurso.Text = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": <b>" + cur.cur_nome + "</b>";

                if (cadastroReunioesPorPeriodo)
                {
                    ACA_CalendarioPeriodo cap = new ACA_CalendarioPeriodo { cal_id = VS_cal_id, cap_id = VS_cap_id };
                    ACA_CalendarioPeriodoBO.GetEntity(cap);
                    lblPeriodoCalendario.Text = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": <b>" + cap.cap_descricao + (periodoAberto ? " (aberto)" : string.Empty) + "</b>";

                    UCComboPeriodoCalendario1.Visible = false;
                    lblPeriodoCalendario.Visible = true;
                }
                else
                {
                    lblPeriodoCalendario.Visible = false;
                    UCComboPeriodoCalendario1.Visible = true;

                    // Carregar combo de período do calendário.
                    UCComboPeriodoCalendario1.CarregarTodosPor_EventoEfetivacao(VS_cal_id, -1, VS_tur_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                }

                VerificaRegrasCurso(tur, fav);
                CarregarTelaLancametoFrequencia();
            }
            else
            {
                Response.Redirect("~/Classe/ReunioesResponsaveisFrequencia/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        _UCComboOrdenacao1._OnSelectedIndexChange += CarregarTelaLancametoFrequencia;
        UCComboPeriodoCalendario1.IndexChanged += UCComboPeriodoCalendario1_IndexChanged;
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Classe/ReunioesResponsaveisFrequencia/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
            Salvar(ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    protected void rptAlunos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) ||
            (e.Item.ItemType == ListItemType.AlternatingItem) ||
            (e.Item.ItemType == ListItemType.Header))
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlControl thAvaliacaoAluno = (HtmlControl)e.Item.FindControl("thAvaliacaoAluno");
                Label lblAvaliacaoAluno = (Label)e.Item.FindControl("lblAvaliacaoAluno");
                lblAvaliacaoAluno.Text = VS_crp_nomeAvaliacao;
                thAvaliacaoAluno.Visible = VS_CursoSeriadoAvaliacoes;
            }
            else
            {
                HtmlControl tdAvaliacaoAluno = (HtmlControl)e.Item.FindControl("tdAvaliacaoAluno");
                tdAvaliacaoAluno.Visible = VS_CursoSeriadoAvaliacoes;
            }

            Repeater rptReunioes = (Repeater)e.Item.FindControl("rptReunioes");
            DataTable dtReunioes = new DataTable();

            // Carregar o cabeçalho com a quantidade de reuniões

            dtReunioes.Columns.Add("reu_id");
            dtReunioes.Columns.Add("reu_titulo");
            dtReunioes.Columns.Add("reu_efetivado");
            for (int i = 1; i <= VS_qtd_reunioes; i++)
            {
                var x = (from p in dtEfetivacoes.AsEnumerable()
                         where p.Field<int>("frp_id") == i
                         select p).AsEnumerable();

                DataRow row = dtReunioes.NewRow();
                row["reu_id"] = i;
                row["reu_titulo"] = i + "ª Reunião";
                row["reu_efetivado"] = (x.Count() > 0 ? Convert.ToBoolean(x.CopyToDataTable().Rows[0]["frr_efetivado"]) : false);
                dtReunioes.Rows.Add(row);
            }
            /*if (VS_qtd_reunioes > 0)
            {
                DataRow rowTodas = dtReunioes.NewRow();
                rowTodas["reu_id"] = 0;
                rowTodas["reu_titulo"] = "Todas";
                dtReunioes.Rows.Add(rowTodas);
            }*/

            _btnSalvar.TabIndex = Convert.ToInt16(dtReunioes.Rows.Count);
            _btnCancelar.TabIndex = Convert.ToInt16(dtReunioes.Rows.Count + 1);
            rptReunioes.DataSource = dtReunioes;
            rptReunioes.DataBind();

            //Button btn = (Button)e.Item.FindControl("btnImprimir");
            //if (btn != null)
            //    btn.Attributes.Add("onclick","javascript: return Funcao();");
            //btn.Attributes.Add("onclick", String.Format("javascript: return Funcao('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
        }
    }

    #endregion Eventos

    #region Impressão declaração de comparecimento

    #region Métodos

    /// <summary>
    /// Carrega combo de hora
    /// </summary>
    private void _carregaComboHora()
    {
        ddlHoraInicial.Items.Clear();
        ddlHoraInicial.Items.Insert(0, new ListItem("--", "-1", true));
        ddlHoraFinal.Items.Clear();
        ddlHoraFinal.Items.Insert(0, new ListItem("--", "-1", true));
        for (int i = 0; i <= 23; i++)
        {
            string horaInicial = i < 10 ? string.Concat("0", i.ToString()) : i.ToString();
            ddlHoraInicial.Items.Insert(i + 1, new ListItem(horaInicial, i.ToString(), true));

            string horaFinal = i < 10 ? string.Concat("0", i.ToString()) : i.ToString();
            ddlHoraFinal.Items.Insert(i + 1, new ListItem(horaFinal, i.ToString(), true));
        }
    }

    /// <summary>
    /// carrega combo de minuto
    /// </summary>
    ///
    private void _carregaComboMinuto()
    {
        ddlMinutosInicial.Items.Clear();
        ddlMinutosInicial.Items.Insert(0, new ListItem("--", "-1", true));
        ddlMinutosFinal.Items.Clear();
        ddlMinutosFinal.Items.Insert(0, new ListItem("--", "-1", true));

        int x = 0;
        int index = 1;
        while (x <= 55)
        {
            string minutoInicial = x < 10 ? string.Concat("0", x.ToString()) : x.ToString();
            ddlMinutosInicial.Items.Insert(index, new ListItem(minutoInicial, x.ToString(), true));

            string minutoFinal = x < 10 ? string.Concat("0", x.ToString()) : x.ToString();
            ddlMinutosFinal.Items.Insert(index, new ListItem(minutoFinal, x.ToString(), true));

            index++;
            x = x + 5;
        }
    }

    #endregion Métodos

    #region Eventos

    protected void rptAlunos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        ImageButton btn = (ImageButton)e.Item.FindControl("btnimprimir");

        //VS_BuscaPessoa = ((ImageButton)sender).ID;

        VS_alu_id = Convert.ToInt32(btn.CommandArgument);
        Session["tur_idFrequencia"] = VS_tur_id;
        lblComComparecimento.Text = string.Empty;
        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Declaracao Comparecimento", "$('#divCompComparecimento').dialog('open');", true);
    }

    protected void _btnImprimir_Click(object sender, EventArgs e)
    {
        VS_alu_id = -1;
        Session["tur_idFrequencia"] = VS_tur_id;
        lblComComparecimento.Text = string.Empty;
        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Declaracao Comparecimento", "$('#divCompComparecimento').dialog('open');", true);
    }

    protected void _btnGerarRelatorio_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "divCompComparecimento", "$('#divCompComparecimento').dialog('close'); });", true);

                string parametros =
                    "ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                    "&data=" + txtData.Text +
                    "&horaInicial=" + ddlHoraInicial.SelectedItem.Text + "h" + ddlMinutosInicial.SelectedItem.Text +
                    "&horaFinal=" + ddlHoraFinal.SelectedItem.Text + "h" + ddlMinutosFinal.SelectedItem.Text +
                    "&tur_id=" + VS_tur_id +
                    "&alu_id=" + VS_alu_id +
                    "&tipoResponsavel=" + rblParticipante.SelectedValue +
                    "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                    "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();

                string report = ((int)ReportNameGestaoAcademica.ProgSocial_DeclaracaoComparecimentoReuniao).ToString();
                
                CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
            }
            catch (ValidationException ex)
            {
                lblComComparecimento.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
        }
    }

    protected void cvVadationTime_ServerValidate(object source, ServerValidateEventArgs args)
    {
        StringBuilder builderBegin = new StringBuilder();
        builderBegin
            .Append(txtData.Text)
            .Append(" ")
            .Append(ddlHoraInicial.SelectedItem.Text)
            .Append(":")
            .Append(ddlMinutosInicial.SelectedItem.Text)
            .Append(":00");
        DateTime dateBegin = DateTime.Parse(builderBegin.ToString());

        StringBuilder builderEnd = new StringBuilder();
        builderEnd
            .Append(txtData.Text)
            .Append(" ")
            .Append(ddlHoraFinal.SelectedItem.Text)
            .Append(":")
            .Append(ddlMinutosFinal.SelectedItem.Text)
            .Append(":00");
        DateTime dateEnd = DateTime.Parse(builderEnd.ToString());

        args.IsValid = DateTime.Compare(dateBegin, dateEnd) < 0;//date > 0 = inicio > fim, date < 0 = inicio < fim
    }

    protected void cvParticipante_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (VS_alu_id > 0)
        {
            List<ACA_AlunoResponsavel> lista = ACA_AlunoResponsavelBO.SelecionaResponsaveisPorAluno(VS_alu_id);
            bool existeTipoResponsavel = false;
            switch (Convert.ToInt32(rblParticipante.SelectedValue))
            {
                case 1:
                    existeTipoResponsavel = (from alr in lista
                                             where alr.tra_id == TipoResponsavelAlunoParametro.tra_idPai(__SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                             select alr).Count() > 0;
                    break;

                case 2:
                    existeTipoResponsavel = (from alr in lista
                                             where alr.tra_id == TipoResponsavelAlunoParametro.tra_idMae(__SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                             select alr).Count() > 0;
                    break;

                case 3:
                    existeTipoResponsavel = (from alr in lista
                                             where alr.alr_principal
                                             select alr).Count() > 0;
                    break;
            }

            args.IsValid = existeTipoResponsavel;
        }
        else
            args.IsValid = true;
    }

    #endregion Eventos

    #endregion Impressão declaração de comparecimento
}