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
    public partial class Avaliacao : MotherPageLogadoCompressedViewState
    {
        #region Structs

        /// <summary>
        /// Estrutura usada para guardar as notas de relatório.
        /// </summary>
        [Serializable]
        private struct NotasRelatorio
        {
            public string Id;
            public long alu_id;
            public int tnt_id;
            public int mtu_id;
            public string valor;
            public string arq_idRelatorio;
        }

        /// <summary>
        /// Estrutura que indica se uma atividade possui alunos com nota lançada.
        /// </summary>
        private struct AtividadeIndicacaoNota
        {
            public long tud_id { get; set; }

            public int tnt_id { get; set; }

            public bool PossuiNota { get; set; }
        }

        #endregion Structs

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
                    ViewState["VS_EntitiesControleTurma"] = 
                        TUR_TurmaDisciplinaBO.SelecionaEntidadesControleTurmas(UCControleTurma1.VS_tud_id, ApplicationWEB.AppMinutosCacheLongo);
                }
                return (ControleTurmas)(ViewState["VS_EntitiesControleTurma"]);
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
        /// Guarda as notas de relatório.
        /// </summary>
        private List<NotasRelatorio> VS_Nota_Relatorio
        {
            get
            {
                if (ViewState["VS_Nota_Relatorio"] == null)
                    ViewState["VS_Nota_Relatorio"] = new List<NotasRelatorio>();
                return (List<NotasRelatorio>)(ViewState["VS_Nota_Relatorio"]);
            }
            set
            {
                ViewState["VS_Nota_Relatorio"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        protected int VS_fav_id
        {
            get
            {
                if (VS_EntitiesControleTurma.turma != null)
                {
                    return VS_EntitiesControleTurma.turma.fav_id;
                }
                return -1;
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
        /// Armazena disciplina componentes da regência para docente em ViewState.
        /// </summary>
        private List<sComboTurmaDisciplina> VS_DisciplinaComponenteDocente
        {
            get
            {
                if (ViewState["VS_DisciplinaComponenteDocente"] != null)
                    return (List<sComboTurmaDisciplina>)(ViewState["VS_DisciplinaComponenteDocente"]);
                return new List<sComboTurmaDisciplina>();
            }
            set
            {
                ViewState["VS_DisciplinaComponenteDocente"] = value;
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
        /// Valor do parâmetro acadêmico PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS
        /// </summary>
        private bool ParametroPermitirAtividadesExclusivas
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Retorna o tud_id selecionado no combo ddlComponenteListao.
        /// Valores do combo:
        /// [0] - Tur_id
        /// [1] - Tud_id
        /// [2] - Permissão
        /// [3] - Tud_tipo
        /// </summary>
        private long ddlComponenteListao_Tud_Id_Selecionado
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlComponenteListao.SelectedValue))
                {
                    string[] valores = ddlComponenteListao.SelectedValue.Split(';');

                    if (valores.Length > 1)
                    {
                        return Convert.ToInt64(valores[1]);
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Calcula a quantidade de casas decimais da variação de notas.
        /// </summary>
        private int NumeroCasasDecimais
        {
            get
            {
                int numeroCasasDecimais = 1;
                if (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica != null)
                {
                    string variacao = Convert.ToDouble(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao).ToString();
                    if (variacao.IndexOf(",") >= 0)
                    {
                        numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                    }
                }

                return numeroCasasDecimais;
            }
        }

        /// <summary>
        /// Tabela com atividades da disciplina e período, junto com as notas dos alunos nas atividades.
        /// </summary>
        private DataTable DTAtividades;

        private List<ACA_EscalaAvaliacaoParecer> ltPareceres;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceres
        {
            get
            {
                return ltPareceres ??
                    (
                        ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_id)
                    );
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

        private long Alu_id;
        private Int32 Mtu_id;

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

        private List<AtividadeIndicacaoNota> ltAtividadeIndicacaoNota;

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

        private bool PermiteLancarNota
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarNota && VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoEdicao);
            }
        }

        private bool PermiteVisualizarNota
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarNota && VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoConsulta);
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
        /// ViewState que armazena a data e hora que carregou a TurmaNota
        /// </summary>
        private DateTime VS_Data_Listao_TurmaNota
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Listao_TurmaNota"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Listao_TurmaNota"] = value;
            }
        }

        /// <summary>
        /// Informa se irá recarregar a data da Nota após salvar os dados.
        /// </summary>
        private bool VS_recarregarDataNota
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_recarregarDataNota"] ?? false);
            }
            set
            {
                ViewState["VS_recarregarDataNota"] = value;
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
                if (EntTurmaDisciplina.tud_naoLancarNota)
                    lblMessage2.Text += UtilBO.GetErroMessage((string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " configurado(a) para não lançar nota nessa turma.",
                                                             UtilBO.TipoMensagem.Alerta);
                permiteEdicao = false;
                bool esconderSalvar = false;
                rptAlunosAvaliacao.Visible = pnlLancamentoAvaliacao.Visible = divListao.Visible = PermiteVisualizarNota;
                if (pnlLancamentoAvaliacao.Visible)
                {
                    CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true, ref esconderSalvar);
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
                                                                && PermiteLancarNota;
                }

                btnNovaAvaliacao.Visible = usuarioPermissao && PermiteLancarNota && VS_Periodo_Aberto;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairListao", "var exibeMensagemSair=" + btnSalvar.Visible.ToString().ToLower() + ";", true);

                if (!pnlLancamentoAvaliacao.Visible)
                    divListao.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString().ToLower()), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os dados do listão de avaliações na tela.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public void CarregarListaoAvaliacao(Guid ent_id, bool atualizaData, ref bool esconderSalvar)
        {
            try
            {
                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                       string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                       string.Empty;

                // Carrega os alunos matriculados
                List<AlunosTurmaDisciplina> ListaAlunos = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                     VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                    UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids);

                _lblMsgRepeater.Visible = false;
                pnlListTurmaDisciplina.Visible = pnlLancamentoAvaliacao.Visible = true;

                // Verifica se foram encontrado alunos
                if (ListaAlunos.Count <= 0)
                {
                    pnlListTurmaDisciplina.Visible = pnlLancamentoAvaliacao.Visible = false;
                    _lblMsgRepeater.Visible = true;
                    _lblMsgRepeater.Text = UtilBO.GetErroMessage("Não foram encontrados alunos na turma selecionada.", UtilBO.TipoMensagem.Alerta);
                    esconderSalvar = true;
                }
                else
                {
                    DataTable dt = CLS_TurmaNotaBO.GetSelectBy_TurmaDisciplina_PeriodoTodos(
                        VisibilidadeRegencia(ddlTurmaDisciplinaListao) ?
                                ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                            , UCNavegacaoTelaPeriodo.VS_tpc_id
                            , (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                            , PosicaoDocente
                            , VS_turmaDisciplinaRelacionada.tud_id
                            , __SessionWEB.__UsuarioWEB.Docente.doc_id == 0
                            , false
                            , true
                            , tur_ids);

                    // Carregar as atividades e notas dos alunos nas atividades.
                    var x = (from DataRow dr in dt.Rows
                             where !string.IsNullOrEmpty(dr["tnt_id"].ToString())
                             select dr);

                    if (x.Count() > 0)
                    {
                        DTAtividades = x.CopyToDataTable();
                    }
                    else
                    {
                        DTAtividades = new DataTable();
                    }

                    ltAtividadeIndicacaoNota = (from DataRow dr in DTAtividades.Rows
                                                group dr by new { tud_id = Convert.ToInt64(dr["tud_id"]), tnt_id = Convert.ToInt32(dr["tnt_id"]) }
                                                    into grupo
                                                    select new AtividadeIndicacaoNota
                                                    {
                                                        tud_id = grupo.Key.tud_id
                                                        ,
                                                        tnt_id = grupo.Key.tnt_id
                                                        ,
                                                        PossuiNota = grupo.Any(p => Convert.ToBoolean(p["PossuiNota"]))
                                                    }).ToList();

                    // Carregar repeter de alunos
                    rptAlunosAvaliacao.DataSource = ListaAlunos;
                    rptAlunosAvaliacao.DataBind();
                    // Limpa o hiddenfield do listão de avaliação pra zerar a ordenação.
                    hdnOrdenacaoAvaliacao.Value = "";

                    RepeaterItem header = (RepeaterItem)rptAlunosAvaliacao.Controls[0];
                    Repeater rptAtividades = (Repeater)header.FindControl("rptAtividadesAvaliacao");

                    _lblMsgRepeaterAvaliacao.Visible = false;

                    if (rptAtividades.Items.Count == 0)
                    {
                        _lblMsgRepeaterAvaliacao.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.NenhumaAvaliacaoEncontrada").ToString(), UtilBO.TipoMensagem.Alerta);
                        _lblMsgRepeaterAvaliacao.Visible = true;
                        UCComboOrdenacaoAvaliacao.Visible = true;
                        esconderSalvar = true;
                    }
                    else
                    {
                        UCComboOrdenacaoAvaliacao.Visible = true;
                    }

                    // localizo o nome do usuário que realizou a última alteração nos dados
                    var usuarioAlteracao = (from DataRow dr in dt.Rows
                                            where !string.IsNullOrEmpty(dr["usuarioAltMedia"].ToString())
                                            select new
                                            {
                                                usuarioAltMedia = dr["usuarioAltMedia"].ToString(),
                                                dataAltMedia = Convert.ToDateTime(dr["dataAltMedia"])
                                            });

                    if ((usuarioAlteracao != null) && usuarioAlteracao.Any())
                    {
                        var usuario = usuarioAlteracao.First();

                        lblAlteracaoMedia.Text = CarregarUsuarioAlteracao(usuario.usuarioAltMedia, usuario.dataAltMedia);
                        if (!string.IsNullOrEmpty(lblAlteracaoMedia.Text))
                            divUsuarioAlteracaoMedia.Visible = true;
                    }
                }

                if (atualizaData)
                    VS_Data_Listao_TurmaNota = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString().ToLower()), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o nome do usuário que alterou os dados
        /// </summary>
        private String CarregarUsuarioAlteracao(string nomeUsuario, DateTime dataAlteracao)
        {
            if (string.IsNullOrEmpty(nomeUsuario))
            {
                return string.Empty;
            }

            return "</br>" + GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString() +
                   " alteradas por: " + nomeUsuario.Trim() + " em " + dataAlteracao.ToString("G");
        }

        /// <summary>
        /// Carrega o combo de disciplinas, verificando se usuário logado é docente.
        /// </summary>
        private bool CarregarDisciplinasComboListao(long tur_id, long tud_idSelecionada)
        {
            try
            {
                ddlTurmaDisciplinaListao.Items.Clear();
                ddlComponenteListao.Items.Clear();

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia
                                                                    (tur_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);
                    List<sComboTurmaDisciplina> disciplinaComponenteDocente = (from dr in dtTurmaDisciplina
                                                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia
                                                                               select new sComboTurmaDisciplina
                                                                               {
                                                                                   tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                                   ,
                                                                                   tur_tud_id = dr.tur_tud_id.ToString()
                                                                               }).ToList();

                    if ((!VisibilidadeRegencia(ddlTurmaDisciplinaListao)) || (disciplinaComponenteDocente.Count() > 0))
                    {
                        VS_DisciplinaComponenteDocente = disciplinaComponenteDocente;

                        List<sComboTurmaDisciplina> turmaDisciplina = (from dr in dtTurmaDisciplina
                                                                       where (Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                             && (Convert.ToString(dr.tur_tud_id.Split(';')[1]).Equals(VS_EntitiesControleTurma.turmaDisciplina.tud_id.ToString()))
                                                                       select new sComboTurmaDisciplina
                                                                       {
                                                                           tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                           ,
                                                                           tur_tud_id = dr.tur_tud_id.ToString()
                                                                       }).ToList();

                        ddlTurmaDisciplinaListao.DataSource = turmaDisciplina;
                        ddlTurmaDisciplinaListao.DataBind();

                        if (turmaDisciplina.Any(p => Convert.ToByte(p.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia))
                        {
                            CarregaComponenteRegenciaDocente(
                                Convert.ToInt64(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[0]),
                                ddlComponenteListao);
                            UCCadastroAvaliacao.CarregaComponenteRegencia(tur_id, disciplinaComponenteDocente.Where(p => Convert.ToInt64(p.tur_tud_id.Split(';')[0]) == tur_id).ToList());
                        }
                    }
                    else
                    {
                        lblMessage.Text = UtilBO.GetErroMessage("Docente não possui permissão em disciplinas para lançamento de notas.", UtilBO.TipoMensagem.Alerta);
                        return false;
                    }
                }
                else
                {
                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                    List<sComboTurmaDisciplina> turmaDisciplina = (from dr in dtTurmaDisciplina
                                                                   where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                        && (Convert.ToString(dr.tur_tud_id.Split(';')[1]).Equals(VS_EntitiesControleTurma.turmaDisciplina.tud_id.ToString()))
                                                                   select new sComboTurmaDisciplina
                                                                   {
                                                                       tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                       ,
                                                                       tur_tud_id = dr.tur_tud_id.ToString()
                                                                   }).ToList();

                    //Carrega as disciplinas da turma caso a visão do usuário não seja individual, exceto as componentes de regência.
                    ddlTurmaDisciplinaListao.DataSource = turmaDisciplina;
                    ddlTurmaDisciplinaListao.DataBind();

                    // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                    if (turmaDisciplina.Any(p => Convert.ToByte(p.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia))
                    {
                        //Carrega componentes da regência.
                        var turmaDisciplinaComponenteRegencia = (from dr in dtTurmaDisciplina
                                                                 where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                 select dr);
                        ddlComponenteListao.DataSource = turmaDisciplinaComponenteRegencia;
                        ddlComponenteListao.DataBind();
                        UCCadastroAvaliacao.CarregaComponenteRegencia(tur_id, turmaDisciplinaComponenteRegencia.ToList());
                    }
                }

                if (ddlTurmaDisciplinaListao.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlTurmaDisciplinaListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlTurmaDisciplinaListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlTurmaDisciplinaListao.SelectedValue = x.First();
                }

                if (ddlComponenteListao.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlComponenteListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlComponenteListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlComponenteListao.SelectedValue = x.First();
                }

                ddlComponenteListao.Visible = lblComponenteListao.Visible = ddlComponenteListao.Items.Count > 0 && Convert.ToByte(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
                return true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                return false;
            }
        }

        /// <summary>
        /// Carrega disciplina(s componente(s) da regencia somente da turma selecionada.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        private void CarregaComponenteRegenciaDocente(long tur_id, DropDownList ddlDisciplinaComponentes)
        {
            var turmaDisciplinaComponenteRegencia = (from dr in VS_DisciplinaComponenteDocente
                                                     where (Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == tur_id)
                                                     select new sComboTurmaDisciplina
                                                     {
                                                         tur_tud_nome = dr.tur_tud_nome.ToString()
                                                         ,
                                                         tur_tud_id = dr.tur_tud_id.ToString()
                                                     }).ToList();

            ddlDisciplinaComponentes.DataSource = turmaDisciplinaComponenteRegencia;
            ddlDisciplinaComponentes.DataBind();
        }

        /// <summary>
        /// Indica se a visibilidade é do tipo regência-componente regência.
        /// </summary>
        /// <returns></returns>
        private bool VisibilidadeRegencia(DropDownList ddlTurmas)
        {
            return ddlTurmas.Items.Count > 0 && Convert.ToByte(ddlTurmas.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do repeater.
        /// </summary>
        private static string RetornaAvaliacao(RepeaterItem item)
        {
            TextBox txtNota = (TextBox)item.FindControl("txtNota");

            if (txtNota.Visible)
                return txtNota.Text;

            DropDownList ddlPareceres = (DropDownList)item.FindControl("ddlPareceres");

            if (ddlPareceres.Visible)
            {
                if (ddlPareceres.SelectedValue == "-1")
                    return string.Empty;

                return ddlPareceres.SelectedValue;
            }

            return string.Empty;
        }

        /// <summary>
        /// Verifica e cria uma avaliacao unica para o tipo de atividade, disciplina e periodo.
        /// </summary>
        /// <param name="tudId">Id da turma disciplina</param>
        /// <param name="tpcId">Id do periodo do calendario</param>
        /// <param name="tavId">Id do tipo de atividade</param>
        /// <param name="tntNome">Nome da avaliacao</param>
        /// <param name="tdtPosicao">Posicao do docente</param>
        /// <returns></returns>
        private CLS_TurmaNota CriarAvaliacaoUnicaTipoAtividade(long tudId, int tpcId, int tavId, byte tdtPosicao)
        {
            // Confirmo se ja existe uma avaliacao cadastrada para o tipo de atividade
            CLS_TurmaNota avaliacaoLicaoDeCasa = CLS_TurmaNotaBO.GetSelectByTipoAtividade(tudId, UCNavegacaoTelaPeriodo.VS_tpc_id, tavId);
            if (avaliacaoLicaoDeCasa.tnt_id <= 0)
            {
                avaliacaoLicaoDeCasa.IsNew = true;
                avaliacaoLicaoDeCasa.tud_id = tudId;
                avaliacaoLicaoDeCasa.tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id;
                avaliacaoLicaoDeCasa.tnt_nome = string.Empty;
                avaliacaoLicaoDeCasa.tnt_situacao = 1; //Ativo
                avaliacaoLicaoDeCasa.tav_id = tavId;
                avaliacaoLicaoDeCasa.tdt_posicao = tdtPosicao;
                avaliacaoLicaoDeCasa.tnt_exclusiva = false;
                avaliacaoLicaoDeCasa.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                if (CLS_TurmaNotaBO.Save(avaliacaoLicaoDeCasa))
                {
                    avaliacaoLicaoDeCasa.IsNew = true;
                    return avaliacaoLicaoDeCasa;
                }
            }
            return avaliacaoLicaoDeCasa;
        }

        /// <summary>
        /// Salva listão de avaliações.
        /// </summary>
        /// <param name="PermaneceTela"></param>
        /// <returns></returns>
        private bool SalvarAvaliacao(out string msg)
        {
            msg = "";
            if (!VS_Periodo_Aberto)
                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.MensagemApenasConsulta").ToString());

            List<CLS_TurmaNotaAluno> listTurmaNotaAluno = new List<CLS_TurmaNotaAluno>();
            List<CLS_TurmaNota> listTurmaNota = new List<CLS_TurmaNota>();
            List<CLS_TurmaAula> listaTurmaAula = new List<CLS_TurmaAula>();

            RepeaterItem header = (RepeaterItem)rptAlunosAvaliacao.Controls[0];
            Repeater rptAtividades = (Repeater)header.FindControl("rptAtividadesAvaliacao");
            Guid UsuIdLogado = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
            long tudId = VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id;
            int tntIdLicaoCasa = -1;

            // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
            foreach (RepeaterItem itemAtividade in rptAtividades.Items)
            {
                byte tdt_posicao = Convert.ToByte(((Label)itemAtividade.FindControl("lblPosicao")).Text);
                Guid usu_id_ativ = (!string.IsNullOrEmpty(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) ? new Guid(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty);
                if (usu_id_ativ == UsuIdLogado || !(PosicaoDocente > 0 && !VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao)))
                {
                    int qatId = Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnQatId")).Value);
                    int tnt_id = Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnTntId")).Value);

                    if (qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa)
                    {
                        if (tnt_id <= 0)
                        {
                            // Verifico se ja existe uma avaliacao de cadastrada
                            CLS_TurmaNota ent = CriarAvaliacaoUnicaTipoAtividade(tudId
                                                                                , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                                                , Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnTavId")).Value)
                                                                                , tdt_posicao);

                            // Se nao precisou cadastrar uma nova avaliacao, entao os dados da tela nao estao atualizados.
                            if (!ent.IsNew)
                            {
                                VS_recarregarDataNota = false;
                                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaNota").ToString());
                            }
                            tntIdLicaoCasa = ent.tnt_id;
                        }
                        else
                        {
                            CLS_TurmaNota ent = new CLS_TurmaNota
                            {
                                tud_id = tudId,
                                tnt_id = tnt_id
                            };
                            CLS_TurmaNotaBO.GetEntity(ent);
                            if (!ent.IsNew && ent.tnt_dataAlteracao > VS_Data_Listao_TurmaNota)
                            {
                                VS_recarregarDataNota = false;
                                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaNota").ToString());
                            }
                            ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                            listTurmaNota.Add(ent);
                        }
                    }
                    else
                    {
                        string strDataAula = ((Label)itemAtividade.FindControl("lblDataAula")).Text;
                        DateTime dataAula = String.IsNullOrEmpty(strDataAula) ? new DateTime() : Convert.ToDateTime(strDataAula);

                        if (!String.IsNullOrEmpty(strDataAula)
                            && (VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                            && UCControleTurma1.VS_tur_dataEncerramento != new DateTime()
                            && dataAula > UCControleTurma1.VS_tur_dataEncerramento)
                        {
                            throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ValidacaoDataEncerramentoTurma").ToString());
                        }

                        int tau_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltau_id")).Text);
                        bool tnt_exclusiva = Convert.ToBoolean(((Label)itemAtividade.FindControl("lblAtividadeExclusiva")).Text);

                        CLS_TurmaNota ent = new CLS_TurmaNota
                        {
                            tud_id = tudId,
                            tnt_id = tnt_id
                        };
                        CLS_TurmaNotaBO.GetEntity(ent);
                        if (!ent.IsNew && ent.tnt_dataAlteracao > VS_Data_Listao_TurmaNota)
                        {
                            VS_recarregarDataNota = false;
                            throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaNota").ToString());
                        }

                        ent.tdt_posicao = PosicaoDocente;
                        ent.tnt_exclusiva = tnt_exclusiva;
                        ent.tnt_data = dataAula;
                        ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

                        listTurmaNota.Add(ent);

                        if (tau_id > 0)
                        {
                            listaTurmaAula.Add(new CLS_TurmaAula
                            {
                                tud_id = ent.tud_id,
                                tau_id = tau_id,
                                tau_data = dataAula
                            });
                        }
                    }
                }
            }

            listaTurmaAula = (
                    from aula in listaTurmaAula
                    group aula by new { aula.tud_id, aula.tau_id, aula.tau_data } into gAula
                    select new CLS_TurmaAula
                    {
                        tud_id = EntTurmaDisciplina.tud_id,
                        tau_id = gAula.Key.tau_id,
                        tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(
                            listTurmaNota.Where(p => p.tud_id == gAula.Key.tud_id && p.tnt_data == gAula.Key.tau_data).ToList())
                    }
                ).Distinct().ToList();

            foreach (RepeaterItem itemAluno in rptAlunosAvaliacao.Items)
            {
                rptAtividades = (Repeater)itemAluno.FindControl("rptAtividadesAvaliacao");
                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);
                string pes_nome = Convert.ToString(((Label)itemAluno.FindControl("lblNomeOficial")).Text);

                // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                foreach (RepeaterItem itemAtividadeAluno in rptAtividades.Items)
                {
                    Guid usu_id_ativ = (!string.IsNullOrEmpty(((Label)itemAtividadeAluno.FindControl("lblUsuIdAtiv")).Text) ? new Guid(((Label)itemAtividadeAluno.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty);

                    HtmlGenericControl divAtividades = (HtmlGenericControl)itemAtividadeAluno.FindControl("divAtividades");
                    if (divAtividades != null)
                    {
                        byte tdt_posicao = Convert.ToByte(((Label)divAtividades.FindControl("lblPosicao")).Text);
                        CheckBox chkParticipante = (CheckBox)itemAtividadeAluno.FindControl("chkParticipante");
                        CheckBox chkFalta = (CheckBox)itemAtividadeAluno.FindControl("chkFalta");

                        if (usu_id_ativ == UsuIdLogado || !(PosicaoDocente > 0 && PosicaoDocente != tdt_posicao))
                        {
                            if (CLS_TurmaNotaAlunoBO.VerificaValoresNotas(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica, RetornaAvaliacao(itemAtividadeAluno), pes_nome))
                            {
                                HiddenField hdnTntId = (HiddenField)itemAtividadeAluno.FindControl("hdnTntId");
                                int tnt_id = 0;
                                if (hdnTntId != null && !string.IsNullOrEmpty(hdnTntId.Value))
                                {
                                    tnt_id = Convert.ToInt32(hdnTntId.Value);
                                }

                                if (tnt_id <= 0)
                                {
                                    HiddenField hdnQatId = (HiddenField)itemAtividadeAluno.FindControl("hdnQatId");
                                    int qatId = 0;
                                    if (hdnQatId != null && !string.IsNullOrEmpty(hdnQatId.Value))
                                    {
                                        qatId = Convert.ToInt32(hdnQatId.Value);
                                    }

                                    if (qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa)
                                    {
                                        tnt_id = tntIdLicaoCasa;
                                    }
                                }

                                if (tnt_id > 0)
                                {
                                    bool ausente = (chkFalta != null && chkFalta.Visible) ? chkFalta.Checked : false;

                                    // Busca relatório lançado.
                                    NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                                        p.alu_id == alu_id
                                        && p.tnt_id == tnt_id
                                        && p.mtu_id == mtu_id);

                                    CLS_TurmaNotaAluno ent = new CLS_TurmaNotaAluno
                                    {
                                        tud_id = tudId,
                                        tnt_id = tnt_id,
                                        alu_id = alu_id,
                                        mtu_id = mtu_id,
                                        mtd_id = mtd_id,
                                        tna_avaliacao = ausente ? string.Empty : RetornaAvaliacao(itemAtividadeAluno),
                                        tna_relatorio = ausente ? string.Empty : rel.valor,
                                        tna_naoCompareceu = ausente,
                                        tna_situacao = 1,
                                        tna_participante = (chkParticipante != null && chkParticipante.Visible) ? chkParticipante.Checked : true
                                    };

                                    listTurmaNotaAluno.Add(ent);
                                }
                            }

                            HabilitaControles(divAtividades.Controls, true);
                        }
                        else
                            HabilitaControles(divAtividades.Controls, false);
                    }
                }
            }

            CLS_TurmaNotaAlunoBO.Save
                (
                    listTurmaNotaAluno
                    , listTurmaNota
                    , new List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia>()
                    , UCControleTurma1.VS_tur_id
                    , tudId
                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                    , VS_fav_id
                    , PosicaoDocente
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    , listaTurmaAula
                    , VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    , (byte)LOG_AvaliacaoMedia_Alteracao_Origem.Web
                    , (byte)LOG_TurmaNota_Alteracao_Origem.WebListao
                    , (byte)LOG_TurmaNota_Alteracao_Tipo.LancamentoNotas
                );

            try
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Listão de nota | " +
                                                                        "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                        " | tur_id: " + UCControleTurma1.VS_tur_id + "; tud_id: " + tudId);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }

            bool esconderSalvar = false;
            CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, false, ref esconderSalvar);
            btnSalvar.Visible = btnSalvarCima.Visible = !esconderSalvar;
            btnNovaAvaliacao.Visible = usuarioPermissao && PermiteLancarNota && VS_Periodo_Aberto;

            msg = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.SucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);

            return true;
        }

        /// <summary>
        /// Adiciona um item na lista de relatórios do ViewState.
        /// </summary>
        /// <param name="tnt_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="valor"></param>
        private void AdicionaItemRelatorio(int tnt_id, long alu_id, int mtu_id, string valor)
        {
            VS_Nota_Relatorio.Add(new NotasRelatorio
            {
                tnt_id = tnt_id
                ,
                alu_id = alu_id
                ,
                mtu_id = mtu_id
                ,
                valor = valor
            });
        }

        /// <summary>
        /// Seta imagem de relatório lançado para o item.
        /// </summary>
        /// <param name="itemAtividade">Item do repeater de atividades</param>
        private void SetaImgRelatorio(RepeaterItem itemAtividade)
        {
            ImageButton btnRelatorio = (ImageButton)itemAtividade.FindControl("btnRelatorio");
            Image imgSituacao = (Image)itemAtividade.FindControl("imgSituacao");

            Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
            RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

            long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
            int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
            int tnt_id = Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnTntId")).Value);

            NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                                    p.alu_id == alu_id
                                    && p.tnt_id == tnt_id
                                    && p.mtu_id == mtu_id);

            //Verifica se o relatório já foi lançado e seta a visibilidade do imgSituacao
            if (!string.IsNullOrEmpty(rel.valor))
            {
                imgSituacao.Visible = true;
                btnRelatorio.ToolTip = "Alterar lançamento do relatório";
            }
            else
                imgSituacao.Visible = false;
        }

        /// <summary>
        /// Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
        /// </summary>
        /// <param name="e">Item do repeater</param>
        private void SetaNomeAluno(RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header)
            {
                string pes_nome = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "pes_nome"));
                Label lblNome = (Label)e.Item.FindControl("lblNome");
                if (lblNome != null)
                    lblNome.Text = pes_nome;

                // Recupera a data de matrícula do aluno na turma/disciplina
                string sDataMatricula = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mtd_dataMatricula"));
                if (!string.IsNullOrEmpty(sDataMatricula))
                {
                    DateTime dataMatricula = Convert.ToDateTime(sDataMatricula);
                    if (dataMatricula.Date > UCNavegacaoTelaPeriodo.cap_dataInicio.Date)
                    {
                        if (lblNome != null)
                            lblNome.Text += "<br/>" + "<b>Data de matrícula:</b> " + dataMatricula.ToString("dd/MM/yyyy");
                    }
                }

                // Recupera a data de saída do aluno na turma/disciplina
                string sDataSaida = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida"));
                if (!string.IsNullOrEmpty(sDataSaida))
                {
                    DateTime dataSaida = Convert.ToDateTime(sDataSaida);
                    if (dataSaida.Date < UCNavegacaoTelaPeriodo.cap_dataFim)
                    {
                        if (lblNome != null)
                            lblNome.Text += "<br/>" + "<b>Data de saída:</b> " + dataSaida.ToString("dd/MM/yyyy");
                    }
                }
            }
        }

        /// <summary>
        /// Carrega repeater de atividades e avaliações da secretaria com dataSources para o listao.
        /// </summary>
        /// <param name="e">Item do repeater de alunos</param>
        /// <param name="alu_id">ID do aluno</param>
        private void CarregaRepeateresInternos(RepeaterItemEventArgs e, long alu_id)
        {
            Repeater rptAtividades = (Repeater)e.Item.FindControl("rptAtividadesAvaliacao");
            DataTable dtAtividades = new DataTable();
            List<DataRow> ltAtividades;
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Busca todas as atividades para o cabeçalho.
                ltAtividades = (from DataRow dr in DTAtividades.Rows
                                where !string.IsNullOrEmpty(Convert.ToString(dr["tnt_id"]))
                                group dr by dr["tnt_id"] into g
                                let tdt_posicao = Convert.ToByte(g.FirstOrDefault()["tdt_posicao"])
                                where VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoConsulta)
                                select g.FirstOrDefault()).ToList();

                if (ltAtividades.Count > 0)
                    dtAtividades = ltAtividades.CopyToDataTable();
            }
            else
            {
                int mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                int mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));

                // Busca as notas das atividades para o aluno.
                ltAtividades = (from DataRow dr in DTAtividades.Rows
                                where
                                    !string.IsNullOrEmpty(Convert.ToString(dr["tnt_id"]))
                                    && Convert.ToInt64(dr["alu_id"]) == alu_id
                                    && Convert.ToInt32(dr["mtu_id"]) == mtu_id
                                    && Convert.ToInt32(dr["mtd_id"]) == mtd_id
                                group dr by dr["tnt_id"] into g
                                let tdt_posicao = Convert.ToByte(g.FirstOrDefault()["tdt_posicao"])
                                where VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoConsulta)
                                select g.FirstOrDefault()).ToList();

                if (ltAtividades.Count > 0)
                    dtAtividades = ltAtividades.CopyToDataTable();
            }

            if (rptAtividades != null)
            {
                rptAtividades.DataSource = dtAtividades;
                rptAtividades.DataBind();
            }
        }

        /// <summary>
        /// Retorna o script que adiciona variáveis necessárias para o javscript da tela.
        /// </summary>
        /// <returns></returns>
        private string GeraScriptVariaveisTurma()
        {
            string arredondamento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ARREDONDAMENTO_NOTA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
            string destacarCampoNota = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.DESTACAR_CAMPO_NOTA_AVALIACAO_ACIMA_PERMITIDO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();

            string escala_variacao = "0";
            string escala_maior_valor = "0";

            if (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica != null)
            {
                escala_variacao = VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao.ToString();
                escala_maior_valor = VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_maiorValor.ToString().Replace(',', '.');
            }

            string script = "var calcularMediaAutomatica = false;" +
                      "var arredondamento = " + arredondamento.ToLower() + ";" +
                      "var qtdCasasDecimais = parseInt('" + NumeroCasasDecimais + "');" +
                      "var variacaoEscala = '" + escala_variacao.Replace(',', '.') + "';" +
                      "var destacarCampoNota = " + destacarCampoNota.ToLower() + ";" +
                      "var maiorValor = " + escala_maior_valor + ";";

            return script;
        }

        /// <summary>
        /// Recarregar o grid de avaliacoes.
        /// </summary>
        private void RecarregarListaoAvaliacao()
        {
            if (pnlLancamentoAvaliacao.Visible)
            {
                rptAlunosAvaliacao.Controls.Clear();
                bool esconderSalvar = false;
                CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true, ref esconderSalvar);
                btnSalvar.Visible = btnSalvarCima.Visible = !esconderSalvar;
                btnNovaAvaliacao.Visible = usuarioPermissao && PermiteLancarNota && VS_Periodo_Aberto;
                updListao.Update();
            }
        }

        /// <summary>
        /// Recarrega as habilidades relacionadas quando o componente da regência é alterado.
        /// </summary>
        private void UCCadastroAvaliacao_RecarregarHabilidadesRelacionadas()
        {
            UCCadastroAvaliacao.CarregarHabilidadesAvaliacao(UCControleTurma1.VS_tur_id
                                                            , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                            , UCNavegacaoTelaPeriodo.VS_cal_id
                                                            , UCControleTurma1.VS_tdt_posicao
                                                            , VS_EntitiesControleTurma.curriculoPeriodo.cur_id
                                                            , VS_EntitiesControleTurma.curriculoPeriodo.crr_id
                                                            , VS_EntitiesControleTurma.curriculoPeriodo.crp_id);
        }

        /// <summary>
        /// Salva a avaliacao.
        /// </summary>
        private void UCCadastroAvaliacao_SalvarAvaliacao()
        {
            bool sucessoSalvarNotas = SalvarAvaliacaoMensagem(false);
            if (UCCadastroAvaliacao.SalvarNovaAtividade(VS_EntitiesControleTurma.turma
                                                        , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                        , UCControleTurma1.VS_tdt_posicao
                                                        , (VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                                                            ? UCControleTurma1.VS_tur_dataEncerramento : new DateTime()
                                                        , UCNavegacaoTelaPeriodo.cal_dataInicio
                                                        , UCNavegacaoTelaPeriodo.cal_dataFim
                                                        , UCNavegacaoTelaPeriodo.cap_dataInicio
                                                        , UCNavegacaoTelaPeriodo.cap_dataFim
                                                        , VS_EntitiesControleTurma.formatoAvaliacao.fav_permiteRecuperacaoForaPeriodo
                                                        , (byte)LOG_TurmaNota_Alteracao_Origem.WebListao
                                                        , sucessoSalvarNotas))
            {
                // Fecha o pop-up e mostra a mensagem de sucesso
                ScriptManager.RegisterStartupScript(Page, GetType(), "CadastroAvaliacao", "$(document).ready(function() { var exibirMensagemConfirmacao=false; scrollToTop(); $('#divAtividadeAvaliativa').dialog('close'); });", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.SucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);
                RecarregarListaoAvaliacao();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "CadastroAvaliacao", "$(document).ready(function() { $('#divAtividadeAvaliativa').animate({ scrollTop: 0 }, 'fast'); });", true);
            }
        }

        /// <summary>
        /// Cancelar avaliacao.
        /// </summary>
        private void UCCadastroAvaliacao_CancelarAvaliacao()
        {
            // Fecha o pop-up
            ScriptManager.RegisterStartupScript(Page, GetType(), "CadastroAvaliacao", "$(document).ready(function() { var exibirMensagemConfirmacao=false; $('#divAtividadeAvaliativa').dialog('close'); });", true);
        }

        /// <summary>
        /// Confirmacao de operacao.
        /// </summary>
        private void UCConfirmacaoOperacao1_ConfimaOperacao()
        {
            if (!string.IsNullOrEmpty(hdnConfirmArguments.Value))
            {
                string[] commandArgs = hdnConfirmArguments.Value.Split(';');
                string command = commandArgs[0];
                hdnConfirmArguments.Value = "";

                if (command == "ExcluirAvaliacao")
                {
                    try
                    {
                        bool sucessoSalvarNotas = SalvarAvaliacaoMensagem(false);
                        if (sucessoSalvarNotas)
                        {
                            CLS_TurmaNota entity = new CLS_TurmaNota { tnt_id = Convert.ToInt32(commandArgs[1]), tud_id = Convert.ToInt64(commandArgs[2]) };
                            CLS_TurmaNotaBO.GetEntity(entity);
                            if (!entity.IsNew)
                            {
                                if (CLS_TurmaNotaBO.Delete(entity))
                                {
                                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.SucessoExcluirAvaliacao").ToString(), UtilBO.TipoMensagem.Sucesso);
                                    RecarregarListaoAvaliacao();
                                }
                                else
                                {
                                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroExcluirAvaliacao").ToString(), UtilBO.TipoMensagem.Erro);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroExcluirAvaliacao").ToString(), UtilBO.TipoMensagem.Erro);
                    }
                }
            }
        }

        /// <summary>
        /// Abre o dialog para adicionar uma nova avaliação.
        /// </summary>
        private void AbrirAdicionarAvaliacao()
        {
            UCCadastroAvaliacao.CarregarAvaliacao(UCControleTurma1.VS_tur_id
                                                    , UCControleTurma1.VS_tud_id
                                                    , -1
                                                    , VS_EntitiesControleTurma.turma.tur_tipo
                                                    , EntTurmaDisciplina.tud_tipo
                                                    , usuarioPermissao && !VS_PeriodoEfetivado && PermiteLancarNota
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , UCNavegacaoTelaPeriodo.VS_cal_id
                                                    , UCControleTurma1.VS_tdt_posicao
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.cur_id
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.crr_id
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.crp_id);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroAvaliacao",
                                                "$(document).ready(function() { $('#divAtividadeAvaliativa').dialog('option', 'title', '"
                                                + GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.pnlAtividadeAvaliativa.GroupingText.Novo").ToString()
                                                + "'); $('#divAtividadeAvaliativa').dialog('open'); });", true);
        }

        /// <summary>
        /// Abre o dialog para editar uma avaliação.
        /// </summary>
        /// <param name="tntId"></param>
        /// <param name="permiteEditar"></param>
        private void AbrirEditarAvaliacao(string tntId, string permiteEditar)
        {
            UCCadastroAvaliacao.CarregarAvaliacao(UCControleTurma1.VS_tur_id
                                                    , UCControleTurma1.VS_tud_id
                                                    , Convert.ToInt32(tntId)
                                                    , VS_EntitiesControleTurma.turma.tur_tipo
                                                    , EntTurmaDisciplina.tud_tipo
                                                    , Convert.ToBoolean(permiteEditar)
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , UCNavegacaoTelaPeriodo.VS_cal_id
                                                    , UCControleTurma1.VS_tdt_posicao
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.cur_id
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.crr_id
                                                    , VS_EntitiesControleTurma.curriculoPeriodo.crp_id);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroAvaliacao",
                                                "$(document).ready(function() { scrollToTop(); $('#divAtividadeAvaliativa').dialog('option', 'title', '"
                                                + GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.pnlAtividadeAvaliativa.GroupingText.Edicao").ToString()
                                                + "'); $('#divAtividadeAvaliativa').dialog('open'); });", true);
        }

        /// <summary>
        /// Abre o dialog para confirmar a exclusão de uma avaliação.
        /// </summary>
        /// <param name="tntId"></param>
        /// <param name="tudId"></param>
        private void AbrirExcluirAvaliacao(string tntId, long tudId, string nomeAvaliacao)
        {
            UCConfirmacaoOperacao1.Mensagem = string.Format(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.UCConfirmacaoOperacao1.Mensagem.ConfirmacaoExclusaoAvaliacao").ToString(), "'" + nomeAvaliacao + "'");
            UCConfirmacaoOperacao1.EventBtnNao = false;
            UCConfirmacaoOperacao1.Update();
            hdnConfirmArguments.Value = string.Format("{0};{1};{2}", "ExcluirAvaliacao", tntId, tudId);
            updListao.Update();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaMovimentacao", "$(document).ready(function(){ scrollToTop(); $('#divConfirmacao').dialog('open'); });", true);
        }

        /// <summary>
        /// Salva as notas das avaliações e mostra a mensagem de sucesso.
        /// </summary>
        /// <returns></returns>
        private bool SalvarAvaliacaoMensagem(bool exibeMensagemSucesso = true)
        {
            bool sucesso = true;
            string msg = "";
            VS_recarregarDataNota = true;

            //Salva avaliação
            try
            {
                if (Page.IsValid && pnlLancamentoAvaliacao.Visible && PermiteLancarNota)
                    sucesso = SalvarAvaliacao(out msg);
            }
            catch (ValidationException ex)
            {
                sucesso = false;
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                sucesso = false;
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                sucesso = false;
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                sucesso = false;
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage(string.Format("Erro ao tentar salvar {0}.", GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString().ToLower()), UtilBO.TipoMensagem.Erro);
            }

            if (VS_recarregarDataNota)
                VS_Data_Listao_TurmaNota = DateTime.Now.AddSeconds(1);

            if (!string.IsNullOrEmpty(msg) && (exibeMensagemSucesso || !sucesso))
            {
                lblMessage.Text += msg;
                ScriptManager.RegisterStartupScript(Page, GetType(), "ValidacaoSalvarNotas", "$(document).ready(scrollToTop);", true);
            }
            return sucesso;
        }

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMessage.Text = message;

                    if (PreviousPage == null && Session["DadosPaginaRetorno"] == null && Session["tud_id"] == null)
                    {
                        // Se não carregou nenhuma turma, redireciona pra busca.
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("É necessário selecionar uma turma.", UtilBO.TipoMensagem.Alerta);
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao ||
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                            RedirecionarPagina("~/Academico/ControleTurma/MinhaEscolaGestor.aspx");
                        else
                            RedirecionarPagina("~/Academico/ControleTurma/Busca.aspx");

                        return;
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
                            UCControleTurma1.VS_tur_tipo = Convert.ToByte(listaDados["Edit_tur_tipo"]);
                            UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(listaDados["Edit_tud_idAluno"]);
                            UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(listaDados["Edit_tur_idNormal"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(listaDados["Edit_tpc_id"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(listaDados["Edit_tpc_ordem"]);
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

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.Avaliacao;

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
                                return;
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
                            return;
                        }

                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);
                        CarregarDisciplinasComboListao(UCControleTurma1.VS_tur_id, VS_EntitiesControleTurma.turmaDisciplina.tud_id);
                        CarregarTela();
                    }

                    bool mudaCorTitulo = VS_cal_ano < DateTime.Now.Year && VS_turmasAnoAtual && VS_EntitiesControleTurma.turma.tur_situacao == 1;

                    UCControleTurma1.CorTituloTurma = mudaCorTitulo ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black;
                    divMessageTurmaAnterior.Visible = mudaCorTitulo;

                    Dictionary<int, string> dicFiltro = CLS_QualificadorAtividadeBO.GetTiposBy_TurmaDisciplina(VS_EntitiesControleTurma.turmaDisciplina.tud_id);
                    if (dicFiltro.Count > 0)
                    {
                        cblQualificadorAtividade.DataSource = dicFiltro;
                        cblQualificadorAtividade.DataBind();
                        foreach (ListItem item in cblQualificadorAtividade.Items)
                        {
                            item.Selected = item.Value != ((int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDaAtividadeDiversificada).ToString()
                                            && item.Value != ((int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDoInstrumentoDeAvaliacao).ToString();
                            item.Attributes.Add("valor", item.Value);
                        }
                    }
                    else
                    {
                        fdsPesquisa.Visible = false;
                    }

                    hdnPermiteRecuperacaoQualquerNota.Value = VS_EntitiesControleTurma.formatoAvaliacao.fav_permiteRecuperacaoQualquerNota.ToString();

                    EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;
                    if (tipo == EscalaAvaliacaoTipo.Numerica)
                    {
                        hdnMinimoAprovacaoDocente.Value = VS_EntitiesControleTurma.formatoAvaliacao.valorMinimoAprovacaoDocente;
                    }
                    else if (tipo == EscalaAvaliacaoTipo.Pareceres && !String.IsNullOrEmpty(VS_EntitiesControleTurma.formatoAvaliacao.valorMinimoAprovacaoDocente))
                    {
                        for (int i = 0; i < ltPareceres.Count; i++)
                        {
                            if (ltPareceres[i].eap_valor.ToUpper() == VS_EntitiesControleTurma.formatoAvaliacao.valorMinimoAprovacaoDocente.ToUpper())
                            {
                                hdnMinimoAprovacaoDocente.Value = i.ToString();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (ListItem item in cblQualificadorAtividade.Items)
                    {
                        item.Attributes.Add("valor", item.Value);
                    }
                }

                UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
                UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
                UCControleTurma1.DisciplinaCompartilhadaIndexChanged = uccDisciplinaCompartilhada_IndexChanged;
                UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
                UCCadastroAvaliacao.RecarregarHabilidadesRelacionadas += UCCadastroAvaliacao_RecarregarHabilidadesRelacionadas;
                UCCadastroAvaliacao.SalvarAvaliacao += UCCadastroAvaliacao_SalvarAvaliacao;
                UCCadastroAvaliacao.CancelarAvaliacao += UCCadastroAvaliacao_CancelarAvaliacao;
                UCConfirmacaoOperacao1.ConfimaOperacao += UCConfirmacaoOperacao1_ConfimaOperacao;
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
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));

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

                    sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tablesorter.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jquery.metadata.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/FixedLeftColumn/jsFixedLeftColumn.js"));
                    Page.Header.Controls.Add(UtilBO.SetStyleHeader("~/Includes/FixedLeftColumn/", "cssFixedLeftColumn.css", false));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsLancamentoAvaliacoesGeral.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsControleTurma_Listao.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));

                    script = GeraScriptVariaveisTurma();
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "VariaveisScript", script, true);

                    // Exibe mensagem de erro, quando causado por um controle em uma chamada assincrona dentro de um UpdatePanel.
                    if (!Page.ClientScript.IsClientScriptBlockRegistered("EndRequestHandler"))
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type=\"text/javascript\">");
                        sb.Append("function EndRequestHandler(sender, args) {");
                        sb.Append("if (args.get_error() != undefined) {");
                        sb.Append("var Error = '" + UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroInesperado").ToString(), UtilBO.TipoMensagem.Erro) + "';");
                        //Show your custom popup or...
                        //sb.Append("alert(Error);");
                        //Hide default ajax error popup
                        sb.Append("args.set_errorHandled(true);");
                        //...redirect error to your Error Panel on page
                        sb.Append("$('#" + lblMessage.ClientID + "').replaceWith(Error);");
                        sb.Append("}");
                        sb.Append("}");
                        sb.Append("</script>");
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "EndRequestHandler", sb.ToString(), false);
                    }
                    if (!Page.ClientScript.IsStartupScriptRegistered("AddEndRequestHandler"))
                    {
                        System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                        sb2.Append("<script type=\"text/javascript\">");
                        sb2.Append("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);");
                        sb2.Append("</script>");
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddEndRequestHandler", sb2.ToString(), false);
                    }
                    //
                }

                trExibirAlunoDispensadoListao.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (this.IsPostBack && Request.Params != null && Request.Params.Count > 0)
                {
                    var controlName = Request.Params.Get("__EVENTTARGET");
                    var argument = Request.Params.Get("__EVENTARGUMENT");
                    if (controlName == "pnlAvaliacao")
                    {
                        string[] arguments = argument.Split(',');
                        Page.Validate("NotasAvaliacao");
                        AbrirEditarAvaliacao(arguments[0], arguments[1]);
                    }
                }
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (VS_PeriodoEfetivado)
            {
                try
                {
                    lblPeriodoEfetivado.Visible = true;
                    lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.MensagemEfetivado").ToString(),
                                                                     UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Legenda Listão de Avaliação
                    if (tbLegendaListao != null)
                    {
                        HtmlTableCell cell = tbLegendaListao.Rows[0].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.AlunoDispensado;
                        cell = tbLegendaListao.Rows[1].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.AlunoInativo;
                    }
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
                        Response.Redirect("~/Academico/ControleTurma/Avaliacao.aspx", false);
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
                    Response.Redirect("~/Academico/ControleTurma/Avaliacao.aspx", false);
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

                    Response.Redirect("~/Academico/ControleTurma/Avaliacao.aspx", false);
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

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarAvaliacaoMensagem();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnRelatorio_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Recuperando item que chamou.
                ImageButton btnRelatorio = (ImageButton)sender;
                RepeaterItem itemAtividade = (RepeaterItem)btnRelatorio.NamingContainer;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int tnt_id = Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnTntId")).Value);

                NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                    p.alu_id == alu_id
                    && p.tnt_id == tnt_id
                    && p.mtu_id == mtu_id);

                // Guarda o tipo de alteração, o alu_id, o mtu_id e o tnt_id da linha que está sendo editada.
                hdnIds.Value = 1 + ";" + alu_id + ";" + tnt_id + ";" + mtu_id;

                lblDadosRelatorio.Text = "<b>Nome do aluno:</b> " + ((Label)itemAluno.FindControl("lblNome")).Text;
                txtRelatorio.Text = rel.valor;

                // Abrir relatório.
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                string[] s = hdnIds.Value.Split(';');

                int tipoAlteracao = Convert.ToInt32(s[0]);
                long alu_id = Convert.ToInt32(s[1]);
                int id = Convert.ToInt32(s[2]);
                int mtu_id = Convert.ToInt32(s[3]);

                if (tipoAlteracao == 1)
                {
                    if (VS_Nota_Relatorio.Exists(p =>
                        p.tnt_id == id
                        && p.alu_id == alu_id
                        && p.mtu_id == mtu_id))
                    {
                        int alterar = VS_Nota_Relatorio.FindIndex(p =>
                            p.tnt_id == id
                            && p.alu_id == alu_id
                            && p.mtu_id == mtu_id);

                        VS_Nota_Relatorio[alterar] = new NotasRelatorio
                        {
                            valor = txtRelatorio.Text
                            ,
                            tnt_id = id
                            ,
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                        };
                    }
                    else
                        AdicionaItemRelatorio(id, alu_id, mtu_id, txtRelatorio.Text);

                    // Percorre os itens do repeater para atualizar os botões de relatório.
                    foreach (RepeaterItem item in rptAlunosAvaliacao.Items)
                    {
                        Repeater rptAtividades = (Repeater)item.FindControl("rptAtividadesAvaliacao");
                        foreach (RepeaterItem itemAtividade in rptAtividades.Items)
                            SetaImgRelatorio(itemAtividade);
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('close'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptAlunosAvaliacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                // Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
                SetaNomeAluno(e);

                Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                //Parametros dispensa disciplina - pinta linha
                Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;

                // Aluno inativo - pinta a linha
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));

                // Carrega repeater de atividades e avaliações da secretaria.
                CarregaRepeateresInternos(e, Alu_id);

                if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                    Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                    if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        HtmlControl tdNumChamadaAvaliacao = (HtmlControl)e.Item.FindControl("tdNumChamadaAvaliacao");
                        HtmlControl tdNomeAvaliacao = (HtmlControl)e.Item.FindControl("tdNomeAvaliacao");
                        tdNumChamadaAvaliacao.Style["background-color"] = tdNomeAvaliacao.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                }
            }
        }

        protected void ddlComponenteListao_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esconderSalvar = false;
            CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true, ref esconderSalvar);
            btnSalvar.Visible = btnSalvarCima.Visible = !esconderSalvar;
            btnNovaAvaliacao.Visible = usuarioPermissao && PermiteLancarNota && VS_Periodo_Aberto;
        }

        protected void rptAtividadesHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ImageButton btnAtualizarAvaliacaoAutomatica = (ImageButton)e.Item.FindControl("btnAtualizarAvaliacaoAutomatica");
                Panel pnlAvaliacao = (Panel)e.Item.FindControl("pnlAvaliacao");
                ImageButton btnExcluirAvaliacao = (ImageButton)e.Item.FindControl("btnExcluirAvaliacao");
                int qatId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "qat_id"));

                bool permissaoAlteracao = PermiteLancarNota && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= !VS_PeriodoEfetivado;

                if (btnExcluirAvaliacao != null)
                {
                    if (qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa
                        || qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDaAtividadeDiversificada
                        || qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDoInstrumentoDeAvaliacao)
                    {
                        btnExcluirAvaliacao.Visible = false;
                    }

                    btnExcluirAvaliacao.Visible &= permissaoAlteracao && !VS_PeriodoEfetivado;

                    if (btnExcluirAvaliacao.Visible)
                    {
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        sm.RegisterPostBackControl(btnExcluirAvaliacao);
                    }
                }

                if (qatId == (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa)
                {
                    Label lbltnt_data = (Label)e.Item.FindControl("lbltnt_data");
                    if (lbltnt_data != null)
                    {
                        lbltnt_data.Visible = false;
                    }

                    if (btnAtualizarAvaliacaoAutomatica != null)
                    {
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        sm.RegisterPostBackControl(btnAtualizarAvaliacaoAutomatica);
                    }
                }
                else
                {
                    if (btnAtualizarAvaliacaoAutomatica != null)
                    {
                        btnAtualizarAvaliacaoAutomatica.Visible = false;
                    }

                    if (pnlAvaliacao != null)
                    {
                        pnlAvaliacao.Style.Add("cursor", "pointer");
                        pnlAvaliacao.Attributes.Add("onclick", "ClickAvaliacao('" + DataBinder.Eval(e.Item.DataItem, "tnt_id").ToString() + "," + permissaoAlteracao.ToString() + "');");
                    }
                }
            }
        }

        protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                // Seta o campo de nota de acordo com o tipo de escala de avaliação.
                TextBox txtNota = (TextBox)e.Item.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)e.Item.FindControl("ddlPareceres");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                CheckBox chkParticipante = (CheckBox)e.Item.FindControl("chkParticipante");
                ImageButton btnHabilidade = (ImageButton)e.Item.FindControl("btnHabilidade");
                CheckBox chkFalta = (CheckBox)e.Item.FindControl("chkFalta");
                int qatId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "qat_id"));

                // Habilita os controles de acordo com a posição do docente.
                HtmlGenericControl divAtividades = (HtmlGenericControl)e.Item.FindControl("divAtividades");
                bool permissaoAlteracao = PermiteLancarNota
                                            && Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0
                                            && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "qat_id")) != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa;
                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= !VS_PeriodoEfetivado;

                string avaliacao = DataBinder.Eval(e.Item.DataItem, "avaliacao").ToString();

                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id").ToString());
                int tnt_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tnt_id").ToString());

                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                txtNota.Visible = tipo == EscalaAvaliacaoTipo.Numerica;
                ddlPareceres.Visible = tipo == EscalaAvaliacaoTipo.Pareceres;
                btnRelatorio.Visible = tipo == EscalaAvaliacaoTipo.Relatorios;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    ddlPareceres.Items.Insert(0, new ListItem("-- Selecione um conceito --", "-1", true));
                    ddlPareceres.AppendDataBoundItems = true;
                    ddlPareceres.DataSource = LtPareceres;
                    ddlPareceres.DataBind();
                }

                if (divAtividades != null)
                {
                    HabilitaControles(divAtividades.Controls, usuarioPermissao && permissaoAlteracao && VS_Periodo_Aberto);
                }

                // Setar valores.
                Double tnaAvaliacao;
                txtNota.Text = Double.TryParse(avaliacao, out tnaAvaliacao) ? String.Format("{0:F" + NumeroCasasDecimais + "}", tnaAvaliacao) : avaliacao;

                ddlPareceres.SelectedValue = avaliacao;

                bool tnt_exclusiva = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tnt_exclusiva").ToString());

                // Setar visibilidade de controles para avaliações exclusivas.
                if (ParametroPermitirAtividadesExclusivas && tnt_exclusiva)
                {
                    chkParticipante.Visible = true;
                    chkParticipante.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tna_participante").ToString());

                    switch (tipo)
                    {
                        case EscalaAvaliacaoTipo.Numerica:
                            txtNota.Enabled = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;

                        case EscalaAvaliacaoTipo.Pareceres:
                            ddlPareceres.Enabled = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;

                        case EscalaAvaliacaoTipo.Relatorios:
                            btnRelatorio.Visible = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;
                    }
                }
                else
                    chkParticipante.Visible = false;

                // Setar relatórios.
                RepeaterItem itemAtividade = e.Item;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);

                if (tipo == EscalaAvaliacaoTipo.Relatorios)
                {
                    string tna_relatorio = DataBinder.Eval(itemAtividade.DataItem, "relatorio").ToString();
                    AdicionaItemRelatorio(tnt_id, alu_id, mtu_id, tna_relatorio);

                    SetaImgRelatorio(itemAtividade);
                }

                txtNota.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                ddlPareceres.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);

                bool AlunoDispensado = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "AlunoDispensado") ?? false));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui aluno dispensado.
                    if (divAtividades != null)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                // Aluno Inativo
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                HtmlTableCell tdAtividadesAtivAva = (HtmlTableCell)e.Item.FindControl("tdAtividadesAtivAva");

                if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    // Pinta célula que possui aluno inativo.
                    if (tdAtividadesAtivAva != null)
                    {
                        tdAtividadesAtivAva.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                }

                if (btnHabilidade != null)
                {
                    btnHabilidade.Visible = qatId != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa
                                            && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.RELACIONAR_HABILIDADES_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                if (chkFalta != null)
                {
                    chkFalta.Visible = qatId != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa;
                    chkFalta.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ausente") ?? false);
                }
            }
        }

        protected void btnSalvarHabilidadesRelacionadas_Click(object sender, EventArgs e)
        {
            try
            {
                if (CLS_TurmaNotaAlunoOrientacaoCurricularBO.SalvarEmLote(UCHabilidades.RetornaListaHabilidadesAluno()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Atividade Aluno Orientação curricular"
                                                                            + " | tud_id: " + UCControleTurma1.VS_tud_id
                                                                            + " | tnt_id: " + UCHabilidades.VS_tnt_id
                                                                            + " | alu_id;mtu_id;mtd_id : " + UCHabilidades.VS_idAluno);

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SalvarHabilidadesRelacionadas"
                                                        , "$(document).ready(function() { $('#divHabilidadesRelacionadas').dialog('close'); scrollToTop(); });", true);

                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.SucessoSalvarHabilidades").ToString(), UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    lblMensagemHabilidade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroSalvarHabilidades").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemHabilidade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroSalvarHabilidades").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptAtividadesAvaliacao_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AtualizarNotas")
            {
                try
                {
                    int tnt_id = Convert.ToInt32(e.CommandArgument.ToString());

                    DataTable dtAlunos = new DataTable();
                    dtAlunos.Columns.Add("alu_id");
                    dtAlunos.Columns.Add("mtu_id");
                    dtAlunos.Columns.Add("mtd_id");

                    foreach (RepeaterItem itemAluno in rptAlunosAvaliacao.Items)
                    {
                        DataRow aluno = dtAlunos.NewRow();
                        aluno["alu_id"] = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                        aluno["mtu_id"] = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                        aluno["mtd_id"] = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);
                        dtAlunos.Rows.Add(aluno);
                    }

                    DataTable dtNotas = CLS_TurmaNotaBO.CalculaNotaAlunos(VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                                                                            , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                                            , VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_id
                                                                            , dtAlunos);

                    foreach (RepeaterItem itemAluno in rptAlunosAvaliacao.Items)
                    {
                        Repeater rptAtividadesAvaliacao = (Repeater)itemAluno.FindControl("rptAtividadesAvaliacao");
                        long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                        int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                        int mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);

                        foreach (RepeaterItem itemAvaliacao in rptAtividadesAvaliacao.Items)
                        {
                            HiddenField hdnTntId = (HiddenField)itemAvaliacao.FindControl("hdnTntId");
                            if (Convert.ToInt32(hdnTntId.Value) == tnt_id)
                            {
                                TextBox txtNota = (TextBox)itemAvaliacao.FindControl("txtNota");
                                string aas_avaliacao = dtNotas.Select(String.Format("alu_id={0} AND mtu_id={1} AND mtd_id={2}", alu_id, mtu_id, mtd_id))[0]["avaliacao"].ToString();

                                // Setar valores.
                                Double aasAvaliacao;
                                txtNota.Text = Double.TryParse(aas_avaliacao, out aasAvaliacao) ? String.Format("{0:F" + NumeroCasasDecimais + "}", aasAvaliacao) : aas_avaliacao;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar notas.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void ddlTurmaDisciplinaListao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarTela();
                ddlComponenteListao.Visible = lblComponenteListao.Visible = ddlComponenteListao.Items.Count > 0 && Convert.ToByte(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovaAvaliacao_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirAdicionarAvaliacao();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnHabilidade_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Recuperando item que chamou.
                ImageButton btnHabilidade = (ImageButton)sender;
                RepeaterItem itemAtividade = (RepeaterItem)btnHabilidade.NamingContainer;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);
                int tnt_id = Convert.ToInt32(((HiddenField)itemAtividade.FindControl("hdnTntId")).Value);

                Repeater rptAtividadesAvaliacao = (Repeater)rptAlunosAvaliacao.Controls[0].Controls[0].FindControl("rptAtividadesAvaliacao");
                RepeaterItem nomeAtividade = rptAtividadesAvaliacao.Items[itemAtividade.ItemIndex];
                litHabilidades.Text = string.Format(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.litHabilidades.Text").ToString(), ((Label)nomeAtividade.FindControl("lblAtividade")).Text, ((Label)itemAluno.FindControl("lblNome")).Text);

                UCHabilidades.CarregarHabilidadesAluno(
                        VS_EntitiesControleTurma.curriculoPeriodo.cur_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crr_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                        UCControleTurma1.VS_tur_id,
                        UCControleTurma1.VS_tud_id,
                        UCNavegacaoTelaPeriodo.VS_cal_id,
                        PosicaoDocente,
                        tnt_id,
                        UCNavegacaoTelaPeriodo.VS_tpc_id,
                        alu_id,
                        mtu_id,
                        mtd_id
                    );

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConsultaHabilidadesAluno",
                                                        "$(document).ready(function() { $('#divHabilidadesRelacionadas').dialog('option', 'title', '"
                                                        + GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.btnHabilidade.ToolTip").ToString()
                                                        + "'); $('#divHabilidadesRelacionadas').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Avaliacao.lblMessage.ErroCarregarHabilidades").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnExcluirAvaliacao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnExcluirAvaliacao = (ImageButton)sender;
                RepeaterItem itemAtividade = (RepeaterItem)btnExcluirAvaliacao.NamingContainer;
                AbrirExcluirAvaliacao(((HiddenField)itemAtividade.FindControl("hdnTntId")).Value, UCControleTurma1.VS_tud_id, ((Label)itemAtividade.FindControl("lblAtividade")).Text);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}