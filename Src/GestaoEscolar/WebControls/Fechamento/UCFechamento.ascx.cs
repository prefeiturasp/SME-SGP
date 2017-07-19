using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;

namespace GestaoEscolar.WebControls.Fechamento
{
    public partial class UCFechamento : MotherUserControl
    {
        #region DELEGATES

        public delegate void commandAbrirRelatorioRP(long alu_id, string tds_idRP);
        public event commandAbrirRelatorioRP AbrirRelatorioRP;

        public delegate void commandAbrirRelatorioAEE(long alu_id);
        public event commandAbrirRelatorioAEE AbrirRelatorioAEE;

        #endregion DELEGATES

        #region Estruturas

        /// <summary>
        /// Estrutura usada para guardar as notas de relatório.
        /// </summary>
        [Serializable]
        public struct NotasRelatorio
        {
            public string Id;
            public string Valor;
            public string arq_idRelatorio;
        }

        #endregion Estruturas

        #region Classes

        public class AlunoDisciplina
        {
            public Int64 aluId;
            public String nomeDisciplina;
        }

        #endregion Classes

        #region Propriedades

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
        /// </summary>
        public long VS_tur_id
        {
            get
            {
                if (ViewState["VS_tur_id"] != null)
                {
                    return Convert.ToInt64(ViewState["VS_tur_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_Turma"] = null;
                ViewState["VS_FormatoAvaliacao"] = null;

                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tud_id
        /// </summary>
        public string[] TurmaDisciplina_Ids
        {
            get
            {
                return hdnTurmaDisciplina.Value.Split(';');
            }
        }

        /// <summary>
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        public long Tud_id
        {
            get
            {
                string[] ids = TurmaDisciplina_Ids;

                if (ids.Length > 1)
                {
                    return Convert.ToInt64(ids[1]);
                }

                return -1;
            }
        }

        /// <summary>
        /// ID do período selecionado na tela que chamou.
        /// </summary>
        private int VS_Tpc_IdSelecionado
        {
            get
            {
                if (ViewState["VS_Tpc_IdSelecionado"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Tpc_IdSelecionado"]);
            }
            set
            {
                ViewState["VS_Tpc_IdSelecionado"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        public int VS_fav_id
        {
            get
            {
                if (ViewState["VS_fav_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_fav_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_fav_id"] = value;
                ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = null;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_id
        /// </summary>
        public int VS_ava_id
        {
            get
            {
                if (ViewState["VS_ava_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_ava_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_Avaliacao"] = null;
                ViewState["VS_EscalaAvaliacao"] = null;
                ViewState["VS_EscalaNumerica"] = null;
                ViewState["VS_EscalaAvaliacaoDocente"] = null;
                ViewState["VS_EscalaAvaliacaoAdicional"] = null;
                ViewState["VS_EscalaNumericaAdicional"] = null;

                ViewState["VS_ava_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda se a janela para o cadastro de observação do aluno está aberta
        /// </summary>
        private bool VS_JanelaObservacaoAberta
        {
            get
            {
                if (ViewState["VS_JanelaObservacaoAberta"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_JanelaObservacaoAberta"]);
                }

                return false;
            }
            set
            {
                ViewState["VS_JanelaObservacaoAberta"] = value;
            }
        }

        /// <summary>
        /// Retorna uma flag informando se existe uma avaliação de recuperação final para o formato da turma.
        /// </summary>
        public string VS_NomeAvaliacaoRecuperacaoFinal
        {
            get
            {
                if (ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] == null)
                {
                    DataTable dt = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(AvaliacaoTipo.RecuperacaoFinal, VS_fav_id);
                    if (dt.Rows.Count > 0)
                        ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = dt.Rows[0]["ava_nome"].ToString();
                    else
                        ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = "";
                }

                return ViewState["VS_NomeAvaliacaoRecuperacaoFinal"].ToString();
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando a matriz curricular da turma
        /// </summary>
        public List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular> VS_MatrizCurricular
        {
            get
            {
                if (ViewState["VS_MatrizCurricular"] != null)
                {
                    return (List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular>)ViewState["VS_MatrizCurricular"];
                }

                return new List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular>();
            }

            set
            {
                ViewState["VS_MatrizCurricular"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando se a turma possui efetivação semestral
        /// </summary>
        public bool VS_EfetivacaoSemestral
        {
            get
            {
                if (ViewState["VS_EfetivacaoSemestral"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_EfetivacaoSemestral"]);
                }

                return false;
            }

            set
            {
                ViewState["VS_EfetivacaoSemestral"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando a url de retorno
        /// </summary>
        public byte _VS_URL_Retorno_Efetivacao
        {
            get
            {
                if (ViewState["_VS_URL_Retorno_Efetivacao"] != null)
                {
                    return Convert.ToByte(ViewState["_VS_URL_Retorno_Efetivacao"]);
                }

                return 0;
            }

            set
            {
                ViewState["_VS_URL_Retorno_Efetivacao"] = value;
            }
        }

        /// <summary>
        /// Guarda a mensagem que deve aparecer antes de salvar o log
        /// </summary>
        public string VS_MensagemLogEfetivacao
        {
            get
            {
                if (ViewState["VS_MensagemLogEfetivacao"] == null)
                {
                    return "";
                }

                return ViewState["VS_MensagemLogEfetivacao"].ToString();
            }
            set
            {
                ViewState["VS_MensagemLogEfetivacao"] = value;
            }
        }

        /// <summary>
        /// Guarda a posição do docente.
        /// </summary>
        public byte VS_posicao
        {
            get
            {
                if (ViewState["VS_posicao"] == null)
                {
                    string posicaoDocente = hiddenPosicaoDocente.Value;
                    string situacaoTud = hiddenTudSituacao.Value;
                    if ((__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || situacaoTud.Equals("4")) && !string.IsNullOrEmpty(posicaoDocente))
                    {
                        ViewState["VS_posicao"] = Convert.ToByte(posicaoDocente);
                    }
                    else
                    {
                        ViewState["VS_posicao"] = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id,
                                                    Tud_id, ApplicationWEB.AppMinutosCacheLongo);
                    }
                }

                return Convert.ToByte(ViewState["VS_posicao"]);
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de planejamento anual.
        /// </summary>
        public List<sPermissaoDocente> VS_ltPermissaoEfetivacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoEfetivacao"] ??
                            (
                                ViewState["VS_ltPermissaoEfetivacao"] =
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(VS_posicao, (byte)EnumModuloPermissao.Efetivacao)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para consultar boletim.
        /// </summary>
        public List<sPermissaoDocente> VS_ltPermissaoBoletim
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoBoletim"] ??
                            (
                                ViewState["VS_ltPermissaoBoletim"] =
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(VS_posicao, (byte)EnumModuloPermissao.Boletim)
                            )
                        );
            }
        }

        /// <summary>
        /// Indica se o docente logado tem permissão para editar a disciplina selecionada.
        /// no combo.
        /// </summary>
        public bool DocentePodeEditar
        {
            get
            {
                string[] ids = TurmaDisciplina_Ids;

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && !VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoEdicao))
                    return false;

                if (ids.Length > 2)
                {
                    return Convert.ToBoolean(Convert.ToByte(ids[2]));
                }

                return true;
            }
        }

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        public bool usuarioPermissao
        {
            get
            {
                string situacaoTud = hiddenTudSituacao.Value;

                //bloquear botoes -> se for disciplina de docencia compartilhada ou 
                // se o docente nao possuir mais essa turma e se nao for turma extinta
                if (VS_turmaDisciplinaCompartilhada != null
                    || (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0
                        && (!VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoEdicao))
                        || (situacaoTud == ((byte)TUR_TurmaDocenteSituacao.Inativo).ToString()
                                && VS_Turma.tur_situacao != (byte)TUR_TurmaSituacao.Encerrada
                                && VS_Turma.tur_situacao != (byte)TUR_TurmaSituacao.Extinta)))
                    return false;

                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
        }

        /// <summary>
        /// Carrega as configurações para a disciplina compartilhada, relacionada com a disciplina atual.
        /// </summary>
        public TUR_TurmaDisciplina VS_turmaDisciplinaCompartilhada
        {
            get
            {
                if (ViewState["VS_turmaDisciplinaCompartilhada"] != null)
                    return (TUR_TurmaDisciplina)ViewState["VS_turmaDisciplinaCompartilhada"];
                return null;
            }

            set
            {
                ViewState["VS_turmaDisciplinaCompartilhada"] = value;
            }
        }

        /// <summary>
        /// Carrega a turma disciplina relacionada (para as disciplinas de docencia compartilhada).
        /// </summary>
        public sTurmaDisciplinaRelacionada VS_turmaDisciplinaRelacionada
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
        /// ViewState que armazena o tipo de docente logado.
        /// </summary>
        public EnumTipoDocente VS_tipoDocente
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
        /// ViewState que armazena se deve ser adicionado o botao Final no final dos períodos.
        /// </summary>
        private bool VS_IncluirPeriodoFinal
        {
            get
            {
                return (bool)(ViewState["VS_IncluirPeriodoFinal"] ?? false);
            }

            set
            {
                ViewState["VS_IncluirPeriodoFinal"] = value;
            }
        }

        /// <summary>
        /// Seta a propriedade Visible do botao salvar.
        /// </summary>
        public bool VisibleBotaoSalvar
        {
            set
            {
                hdnVisibleBotaoSalvar.Value = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// Seta a mensagem no label principal da tela.
        /// </summary>
        public string MensagemTela
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        /// <summary>
        /// Entidade da turma selecionada no combo.
        /// </summary>
        public TUR_Turma VS_Turma
        {
            get
            {
                return (TUR_Turma)(ViewState["VS_Turma"] ?? (ViewState["VS_Turma"] = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = VS_tur_id })));
            }
        }

        /// <summary>
        /// Entidade do calendário da turma selecionada no combo.
        /// </summary>
        public ACA_CalendarioAnual VS_CalendarioAnual
        {
            get
            {
                if (ViewState["VS_CalendarioAnual"] == null)
                {
                    ACA_CalendarioAnual calendarioAnual = new ACA_CalendarioAnual { cal_id = VS_Turma.cal_id };
                    ACA_CalendarioAnualBO.GetEntity(calendarioAnual);
                    ViewState["VS_CalendarioAnual"] = calendarioAnual;
                }
                return (ACA_CalendarioAnual)ViewState["VS_CalendarioAnual"];
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_FormatoAvaliacao VS_FormatoAvaliacao
        {
            get
            {
                if (ViewState["VS_FormatoAvaliacao"] == null)
                {
                    ACA_FormatoAvaliacao formatoAvaliacao = new ACA_FormatoAvaliacao
                    {
                        fav_id = VS_fav_id
                    };
                    ACA_FormatoAvaliacaoBO.GetEntity(formatoAvaliacao);

                    ViewState["VS_FormatoAvaliacao"] = formatoAvaliacao;
                }

                return (ACA_FormatoAvaliacao)ViewState["VS_FormatoAvaliacao"];
            }
        }

        /// <summary>
        /// Retorna a avaliação selecionada na tela de busca.
        /// </summary>
        public ACA_Avaliacao VS_Avaliacao
        {
            get
            {
                return (ACA_Avaliacao)(ViewState["VS_Avaliacao"] ?? (ViewState["VS_Avaliacao"] = ACA_AvaliacaoBO.GetEntity(new ACA_Avaliacao { fav_id = VS_fav_id, ava_id = VS_ava_id })));
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do formato. Se for lançamento na disciplina,
        /// retorna de acordo com o esa_idPorDisciplina, se for global, retorna
        /// o esa_idConceitoGlobal.
        /// </summary>
        public ACA_EscalaAvaliacao VS_EscalaAvaliacao
        {
            get
            {
                if (ViewState["VS_EscalaAvaliacao"] == null)
                {
                    ACA_EscalaAvaliacao escala = new ACA_EscalaAvaliacao
                    {
                        esa_id = VS_FormatoAvaliacao.esa_idPorDisciplina
                    };

                    if (escala.esa_id > 0)
                    {
                        // Só chama o método de carregar se o id da escala for > 0.
                        ACA_EscalaAvaliacaoBO.GetEntity(escala);
                    }

                    ViewState["VS_EscalaAvaliacao"] = escala;
                }

                return (ACA_EscalaAvaliacao)ViewState["VS_EscalaAvaliacao"];
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_EscalaAvaliacaoNumerica VS_EscalaNumerica
        {
            get
            {
                if (ViewState["VS_EscalaNumerica"] == null)
                {
                    ACA_EscalaAvaliacaoNumerica entEscalaNumerica = new ACA_EscalaAvaliacaoNumerica
                    {
                        esa_id = VS_EscalaAvaliacao.esa_id
                    };
                    ACA_EscalaAvaliacaoNumericaBO.GetEntity(entEscalaNumerica);

                    ViewState["VS_EscalaNumerica"] = entEscalaNumerica;
                }

                return (ACA_EscalaAvaliacaoNumerica)ViewState["VS_EscalaNumerica"];
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do docente que está configurado no formato (esa_idDocente).
        /// </summary>
        public ACA_EscalaAvaliacao VS_EscalaAvaliacaoDocente
        {
            get
            {
                if (ViewState["VS_EscalaAvaliacaoDocente"] == null)
                {
                    ACA_EscalaAvaliacao escalaDocente = new ACA_EscalaAvaliacao
                    {
                        esa_id = VS_FormatoAvaliacao.esa_idDocente
                    };

                    if (escalaDocente.esa_id > 0)
                    {
                        // Só chama o método de carregar se o id da escala for > 0.
                        ACA_EscalaAvaliacaoBO.GetEntity(escalaDocente);
                    }

                    ViewState["VS_EscalaAvaliacaoDocente"] = escalaDocente;
                }

                return (ACA_EscalaAvaliacao)ViewState["VS_EscalaAvaliacaoDocente"];
            }
        }

        private TUR_TurmaDisciplina _entTurmaDisciplina;

        /// <summary>
        /// Entidade da disciplina selecionada no combo.
        /// </summary>
        private TUR_TurmaDisciplina EntTurmaDisciplina
        {
            get
            {
                return _entTurmaDisciplina ??
                    (_entTurmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = Tud_id }));
            }
        }

        public Control FdsRelatorio
        {
            get
            {
                return fdsRelatorio;
            }
        }

        public Label LblMessage
        {
            get
            {
                return lblMessage;
            }
        }

        public Label LblMessage2
        {
            get
            {
                return lblMessage2;
            }
        }

        public Label LblMessage3
        {
            get
            {
                return lblMessage3;
            }
        }

        public Label LblFixMessage
        {
            get
            {
                return lblFixMessage;
            }
        }

        public Button BtnSalvar
        {
            get
            {
                return btnSalvar;
            }
        }

        public Panel PnlAlunos
        {
            get
            {
                return pnlAlunos;
            }
        }

        /// <summary>
        /// Retorna/Guarda a posição do docente.
        /// </summary>
        public HiddenField hiddenPosicaoDocente
        {
            get
            {
                return hdnPosicaoDocente;
            }
            set
            {
                hdnPosicaoDocente = value;
                ViewState["VS_posicao"] = null;
                ViewState["VS_ltPermissaoEfetivacao"] = null;
                ViewState["VS_ltPermissaoBoletim"] = null;
            }
        }

        /// <summary>
        /// Retorna/Guarda a situação da turma disciplina do docente.
        /// </summary>
        public HiddenField hiddenTudSituacao
        {
            get
            {
                return hdnTudSituacao;
            }
            set
            {
                hdnTudSituacao = value;
                ViewState["VS_posicao"] = null;
                ViewState["VS_ltPermissaoEfetivacao"] = null;
                ViewState["VS_ltPermissaoBoletim"] = null;
            }
        }

        /// <summary>
        /// Lista de IDs das turmas normais dos alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        public List<long> VS_listaTur_ids
        {
            get
            {
                return (List<long>)(ViewState["VS_listaTur_ids"] ?? new List<long>());
            }

            set
            {
                ViewState["VS_listaTur_ids"] = value;
            }
        }

        /// <summary>
        /// Guarda os eventos cadastrados para a turma e calendário.
        /// </summary>
        public List<ACA_Evento> VS_ListaEventos
        {
            get
            {
                return
                    (List<ACA_Evento>)
                    (
                        ViewState["VS_ListaEventos"] ??
                        (
                            ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(VS_Turma.cal_id, VS_Turma.tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false)
                        )
                    );
            }
        }

        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool periodoFechado
        {
            get
            {
                switch (VS_Avaliacao.ava_tipo)
                {
                    case (byte)AvaliacaoTipo.Periodica:
                    case (byte)AvaliacaoTipo.PeriodicaFinal:
                        return !VS_ListaEventos.Exists(p => p.tpc_id == VS_Avaliacao.tpc_id && p.tev_id == tev_EfetivacaoNotas && p.vigente);

                    case (byte)AvaliacaoTipo.Recuperacao:
                        if (VS_Avaliacao.tpc_id > 0)
                        {
                            return !VS_ListaEventos.Exists(p => p.tpc_id == VS_Avaliacao.tpc_id && p.tev_id == tev_EfetivacaoRecuperacao && p.vigente);
                        }
                        else
                        {
                            if (VS_fav_id > 0 && VS_ava_id > 0)
                            {
                                List<int> tpc_ids = ACA_AvaliacaoRelacionadaBO.RetornaPeriodoCalendarioRelacionadosPorAvaliacao(VS_fav_id, VS_Avaliacao.tpc_id).Split(',').Select(p => Convert.ToInt32(p)).ToList();
                                return !VS_ListaEventos.Exists(p => tpc_ids.Contains(p.tpc_id) && p.tev_id == tev_EfetivacaoRecuperacao && p.vigente);
                            }
                        }
                        break;

                    case (byte)AvaliacaoTipo.Final:
                        List<sTipoPeriodoCalendario> ltPeriodo = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);

                        int tpc_id = ltPeriodo.OrderBy(p => p.tpc_ordem).Last().tpc_id;

                        return !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal && p.vigente) &&
                               !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoNotas && p.tpc_id == tpc_id && p.vigente);

                    case (byte)AvaliacaoTipo.RecuperacaoFinal:
                        return !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoRecuperacaoFinal && p.vigente);
                }

                return false;
            }
        }

        /// <summary>
        /// Parâmetro acadêmico com o ID do tipo de evento de efetivação de notas.
        /// </summary>
        private int tev_EfetivacaoNotas
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Parâmetro acadêmico com o ID do tipo de evento de efetivação de recuperação.
        /// </summary>
        private int tev_EfetivacaoRecuperacao
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Parâmetro acadêmico com o ID do tipo de evento de efetivação final.
        /// </summary>
        private int tev_EfetivacaoFinal
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Parâmetro acadêmico com o ID do tipo de evento de efetivação de recuperação final.
        /// </summary>
        private int tev_EfetivacaoRecuperacaoFinal
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Seta a disciplina e turma selecionados em tela que chamou o user control.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        public void SetaTurmaDisciplina(long tud_id, long tur_id, int tpc_id, bool incluirPeriodoFinal = false, List<long> listaTur_ids = null)
        {
            this.VS_tur_id = tur_id;
            this.VS_Tpc_IdSelecionado = tpc_id;
            this.VS_IncluirPeriodoFinal = incluirPeriodoFinal;
            this.VS_listaTur_ids = listaTur_ids ?? new List<long>();
            this.lblFixMessage.Text = String.Empty;

            if (VS_tur_id > 0)
            {
                // Efetivação semestral
                bool efetivacaoSemestral;
                VS_MatrizCurricular = ACA_CurriculoControleSemestralDisciplinaPeriodoBO.SelecionaMatrizCurricularTurma(VS_tur_id, out efetivacaoSemestral);
                VS_EfetivacaoSemestral = efetivacaoSemestral;
            }

            UCFechamentoPadrao.SetaTurmaDisciplina();
            UCFechamentoFinal.SetaTurmaDisciplina();

            if (tur_id > 0)
            {
                // Carrega o combo de disciplinas.
                CarregarDisciplinas(tud_id, tpc_id);
            }
        }

        /// <summary>
        /// Verifica o formato de avaliação e carrega as disciplinas de acordo
        /// com o conceito de avaliação.
        /// </summary>
        /// <param name="tud_idSelecionado">Id da disciplina selecionada</param>
        private void CarregarDisciplinas(long tud_idSelecionado, int tpc_idSelecionado)
        {
            VS_fav_id = VS_Turma.fav_id;

            long doc_id = -1;
            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && VS_turmaDisciplinaCompartilhada == null)
            {
                doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
            }

            hdnVariacaoFrequencia.Value = VS_FormatoAvaliacao.fav_variacao.ToString();

            DataTable dt = new DataTable();
            switch ((ACA_FormatoAvaliacaoTipo)VS_FormatoAvaliacao.fav_tipo)
            {
                case ACA_FormatoAvaliacaoTipo.Disciplina:
                    {
                        ACA_Avaliacao avaliacao = new ACA_Avaliacao
                        {
                            fav_id = VS_fav_id,
                            ava_id = VS_ava_id
                        };
                        ACA_AvaliacaoBO.GetEntity(avaliacao);

                        AvaliacaoTipo tipoAvaliacao = (AvaliacaoTipo)avaliacao.ava_tipo;

                        string disciplinaAdicional = (tipoAvaliacao == AvaliacaoTipo.Final ||
                                                       tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal)
                                                       && !VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica
                                                          ? "Resultado final"
                                                          : string.Empty;

                        // Se avaliação for Final ou Per+Final, mostra linha a mais "Res.Final".
                        dt = TUR_TurmaDisciplinaBO.GetSelect_Efetivacao_By_TurmaDisciplinaAdicional(VS_tur_id, doc_id, VS_Turma.tur_codigo, disciplinaAdicional, VS_Turma, 0, 0, 0);

                        break;
                    }

                case ACA_FormatoAvaliacaoTipo.GlobalDisciplina:
                    {
                        // Mostra disciplinas + "Conceito global".
                        // Só exibe o conceito global se a visão for diferente de docente
                        // ou se a visão for igual a docente e o formato estiver configurado
                        // para o docente lançar conceito global.
                        string disciplinaAdicional = string.Empty;
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                        {
                            disciplinaAdicional = "Conceito global";
                        }
                        else
                        {
                            if (VS_FormatoAvaliacao.fav_conceitoGlobalDocente)
                            {
                                disciplinaAdicional = "Conceito global";
                            }
                        }

                        dt = TUR_TurmaDisciplinaBO.GetSelect_Efetivacao_By_TurmaDisciplinaAdicional(VS_tur_id, doc_id, VS_Turma.tur_codigo, disciplinaAdicional, VS_Turma, 0, 0);

                        break;
                    }
            }
            var turmaDisciplina = VS_turmaDisciplinaCompartilhada != null ?
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                         && VS_turmaDisciplinaRelacionada.tud_id.ToString() == Convert.ToString(dr["tur_tud_id"].ToString().Split(';')[1])
                                         // a disciplina de docência compartilhada nao terá fechamento
                                         && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr)
                                     :
                                        tud_idSelecionado > 0 &&
                                    dt.Rows.Cast<DataRow>().Any(p => (Convert.ToString(p["tur_tud_id"].ToString().Split(';')[1]).Equals(tud_idSelecionado.ToString()))) &&
                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_CARREGAR_APENAS_DISCIPLINA_SELECIONADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ?
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                         && (Convert.ToString(dr["tur_tud_id"].ToString().Split(';')[1]).Equals(tud_idSelecionado.ToString()))
                                         // a disciplina de docência compartilhada nao terá fechamento
                                             && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr)
                                    :
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                         // a disciplina de docência compartilhada nao terá fechamento
                                            && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr);

            if (tud_idSelecionado > 0 && turmaDisciplina.Count<DataRow>() > 0)
            {
                DataTable dtTud = turmaDisciplina.CopyToDataTable();

                // Busca o tud_id nos itens do combo.
                var x = from DataRow item in dtTud.Rows
                        where item["tur_tud_id"].ToString().Split(';')[1].Equals(tud_idSelecionado.ToString())
                        select item["tur_tud_id"].ToString();

                hdnTurmaDisciplina.Value =
                    x.Count() > 0
                    ? x.First()
                    : dtTud.Rows[0]["tur_tud_id"].ToString();
            }

            ddlTurmaDisciplina_SelectedIndexChanged();
        }

        public void AtualizarStatusEfetivacao(bool pendente, bool tudNaoLancarNota)
        {
            divStatusFechamento.Visible = true;
            if (pendente)
            {
                imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "statusAlertaPendencia.png";
                if (UCFechamentoPadrao.Visible)
                {
                    lblStatusFechamento.Text = tudNaoLancarNota ?
                                                GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.PendenteNaoLancaNota").ToString()
                                                : GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.PendenteLancaNota").ToString();
                }
                else if (UCFechamentoFinal.Visible)
                {
                    lblStatusFechamento.Text = tudNaoLancarNota ?
                                                GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.PendenteFinalNaoLancaNota").ToString()
                                                : GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.PendenteFinalLancaNota").ToString();
                }
            }
            else
            {
                imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "statusSucessoPendencia.png";
                lblStatusFechamento.Text = tudNaoLancarNota ?
                                            GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.ConcluidoNaoLancaNota").ToString()
                                            : GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblStatusFechamento.Text.ConcluidoLancaNota").ToString();
            }
            upnStatusFechamento.Update();
        }

        /// <summary>
        /// Carrega a avaliação de acordo com os eventos do calendário,
        /// e com os períodos ligados à disciplina.
        /// </summary>
        /// <param name="entityTurma">Entidade da turma</param>
        /// <param name="tud_id">Id da disciplina</param>
        private void CarregarAvaliacao()
        {
            try
            { 
                if (VS_Turma.fav_id <= 0)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("É necessário selecionar uma turma que possua um formato de avaliação.", UtilBO.TipoMensagem.Erro);
                }
                else
                {
                    // Busca o evento ligado ao calendário, que seja do tipo definido
                    // no parâmetro como de efetivação.
                    List<ACA_Evento> listEvento = ACA_EventoBO.GetEntity_Efetivacao_List(VS_Turma.cal_id, VS_Turma.tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, VS_Tpc_IdSelecionado, ApplicationWEB.AppMinutosCacheLongo);

                    int valor = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    
                    // verifica se existe evento do tipo Efetivação Nota
                    string listaTpcIdPeriodicaPeriodicaFinal = string.Empty;
                    IEnumerable<ACA_Evento> dadoNota =
                        (from ACA_Evento item in listEvento
                         where item.tev_id == valor
                         select item);
                    // se existir, pega os tpc_id's
                    List<ACA_Evento> lt = dadoNota.ToList();
                    if (lt.Count > 0)
                    {
                        var x = from ACA_Evento evt in listEvento
                                where evt.tev_id == valor
                                select evt.tpc_id;

                        foreach (int tpc_id in x.ToList())
                        {
                            if (string.IsNullOrEmpty(listaTpcIdPeriodicaPeriodicaFinal))
                                listaTpcIdPeriodicaPeriodicaFinal += Convert.ToString(tpc_id);
                            else
                                listaTpcIdPeriodicaPeriodicaFinal += "," + Convert.ToString(tpc_id);
                        }
                    }

                    // verifica se existe evento do tipo efetivação final
                    bool existeFinal = false;
                    IEnumerable<ACA_Evento> dadoFinal =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorFinal
                         select item);
                    List<ACA_Evento> ltFinal = dadoFinal.ToList();
                    // se existir, marca para trazer as avaliações do tipo final
                    if (ltFinal.Count > 0)
                    {
                        existeFinal = true;
                    }

                    DataTable dtAvaliacoes;

                    // Se for turma eletiva do aluno, carrega apenas os períodos do calendário em que
                    // a turma é oferecida
                    if ((TUR_TurmaTipo)VS_Turma.tur_tipo == TUR_TurmaTipo.EletivaAluno)
                    {
                        List<CadastroTurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectCadastradosBy_Turma(VS_Turma.tur_id);
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao_TurmaDisciplinaCalendario(VS_Turma.tur_id, listaDisciplinas[0].entTurmaDisciplina.tud_id, VS_Turma.fav_id, listaTpcIdPeriodicaPeriodicaFinal, string.Empty, existeFinal, true, true);

                        if (VS_Tpc_IdSelecionado > 0)
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where (VS_Tpc_IdSelecionado == Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"]))
                                                   select dr);
                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                    }
                    else
                    {
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao(VS_Turma.tur_id, VS_Turma.fav_id, Tud_id, listaTpcIdPeriodicaPeriodicaFinal, string.Empty, existeFinal, false, true, true, VS_Tpc_IdSelecionado, ApplicationWEB.AppMinutosCacheLongo);
                    }

                    if (VS_IncluirPeriodoFinal)
                    {
                        if (VS_Tpc_IdSelecionado > 0)
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where (VS_Tpc_IdSelecionado == Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"]))
                                                   select dr);

                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                        else
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where ((byte)dr["ava_tipo"] == (byte)AvaliacaoTipo.Final)
                                                   select dr);

                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                    }

                    var avaliacoes = (from DataRow dr in dtAvaliacoes.Rows
                                      let tpc_id = Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"])
                                      where VS_Tpc_IdSelecionado <= 0 || tpc_id == VS_Tpc_IdSelecionado
                                      select dr);

                    dtAvaliacoes = avaliacoes.Any() ? avaliacoes.CopyToDataTable() : new DataTable();

                    if (dtAvaliacoes.Rows.Count > 0)
                    {
                        if (VS_ava_id > 0)
                        {
                            DataRow dr = dtAvaliacoes.Rows.Cast<DataRow>().ToList().Find(p => Convert.ToInt32(p["ava_id"]) == VS_ava_id);
                            if (dr != null)
                            {
                                VS_ava_id = Convert.ToInt32(dr["ava_id"]);
                            }
                            else
                            {
                                VS_ava_id = Convert.ToInt32(dtAvaliacoes.Rows[0]["ava_id"]);
                            }
                        }
                        else
                        {
                            VS_ava_id = Convert.ToInt32(dtAvaliacoes.Rows[0]["ava_id"]);
                        }
                    }
                    else
                    {
                        VS_ava_id = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar avaliação.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jquery.PrintBoletim.js"));
                    //recarrega sempre pare pegar os campos que antes estavam invisiveis na tela
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroEfetivacao.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsPopupFrequenciaExterna.js"));
                }

                UCAlunoEfetivacaoObservacaoGeral.ReturnValues += UCAlunoEfetivacaoObservacaoGeral_ReturnValues;

                UCFechamentoPadrao.AbrirObservacaoConselho += UCFechamento_AbrirObservacaoConselho;
                UCFechamentoFinal.AbrirObservacaoConselho += UCFechamento_AbrirObservacaoConselho;

                UCFechamentoPadrao.AbrirBoletim += UCFechamento_AbrirBoletim;
                UCFechamentoFinal.AbrirBoletim += UCFechamento_AbrirBoletim;

                UCFechamentoPadrao.AbrirRelatorio += UCFechamento_AbrirRelatorio;
                UCFechamentoFinal.AbrirRelatorio += UCFechamento_AbrirRelatorio;

                UCFechamentoPadrao.AbrirRelatorioRP += UCFechamento_AbrirRelatorioRP;
                UCFechamentoFinal.AbrirRelatorioRP += UCFechamento_AbrirRelatorioRP;

                UCFechamentoPadrao.AbrirRelatorioAEE += UCFechamento_AbrirRelatorioAEE;
                UCFechamentoFinal.AbrirRelatorioAEE += UCFechamento_AbrirRelatorioAEE;
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                // Adiciona JavaScripts da tela.
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
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

                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmerEfetivacao.js"));
                }
                else
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                }
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsAlunoEfetivacaoObservacao.js"));
            }

            //tmrLoad_Tick(this, null);
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (UCFechamentoPadrao.Visible)
                {
                    UCFechamentoPadrao.Salvar();
                }
                else if (UCFechamentoFinal.Visible)
                {
                    UCFechamentoFinal.Salvar();
                }
            }
        }

        protected void btnSalvarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = UtilBO.GetErroMessage("Relatório salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                if (UCFechamentoPadrao.Visible)
                {
                    UCFechamentoPadrao.SalvarRelatorio(hdnIndices.Value, txtArea.Text, fupAnexoRelatorio.PostedFile, hplAnexoRelatorio.Visible);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('close'); });", true);
            }
            catch (ValidationException err)
            {
                lblMessageRelatorio.Text = UtilBO.GetErroMessage(err.Message, UtilBO.TipoMensagem.Alerta);
                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o relatório.", UtilBO.TipoMensagem.Alerta);
            }
        }

        private void ddlTurmaDisciplina_SelectedIndexChanged()
        {
            VS_tur_id = Convert.ToInt64(hdnTurmaDisciplina.Value.Split(';')[0]);
            CarregarAvaliacao();

            VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(VS_posicao, ApplicationWEB.AppMinutosCacheLongo);
            UCFechamentoPadrao.ddlTurmaDisciplina_SelectedIndexChanged();
            UCFechamentoFinal.ddlTurmaDisciplina_SelectedIndexChanged();
            UCComboAvaliacao1_IndexChanged();
        }

        private void UCComboAvaliacao1_IndexChanged()
        {
            try
            {
                divStatusFechamento.Visible = false;
                if (VS_ava_id > 0)
                {
                    ACA_Avaliacao avaliacao = new ACA_Avaliacao
                    {
                        fav_id = VS_fav_id,
                        ava_id = VS_ava_id
                    };
                    ACA_AvaliacaoBO.GetEntity(avaliacao);

                    UCFechamentoPadrao.MostrarLoading += UCFechamento_MostrarLoading;
                    UCFechamentoFinal.MostrarLoading += UCFechamento_MostrarLoading;
                    UCFechamento_EsconderLoading();
                    hdnTentativas.Value = "0";

                    if (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && (AvaliacaoTipo)avaliacao.ava_tipo == AvaliacaoTipo.Final)
                    {
                        UCFechamentoPadrao.Visible = false;
                        UCFechamentoFinal.Visible = true;
                        UCFechamentoPadrao.EscondeTelaAlunos(String.Empty);
                        PnlAlunos.Visible = true;
                        UCFechamentoFinal.UCComboAvaliacao1_IndexChanged();
                    }
                    else
                    {
                        UCFechamentoPadrao.Visible = true;
                        UCFechamentoFinal.Visible = false;
                        UCFechamentoFinal.EscondeTelaAlunos(String.Empty);
                        PnlAlunos.Visible = true;
                        UCFechamentoPadrao.UCComboAvaliacao1_IndexChanged();
                    }
                }
                else
                {
                    UCFechamentoPadrao.Visible = true;
                    UCFechamentoFinal.Visible = false;
                    UCFechamentoFinal.EscondeTelaAlunos(String.Empty);
                    PnlAlunos.Visible = true;
                    UCFechamentoPadrao.UCComboAvaliacao1_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                TrataErro(ex, lblMessage, "carregar os dados");
            }
        }

        protected void btnExcluirAnexoRelatorio_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnExcluirAnexoRelatorio.Visible = false;
                hplAnexoRelatorio.Visible = false;
                if (!VS_JanelaObservacaoAberta)
                {
                    // Atualiza o gvAlunos.
                    if (UCFechamentoPadrao.Visible)
                    {
                        UCFechamentoPadrao.UppAlunos.Update();
                    }
                    else if (UCFechamentoFinal.Visible)
                    {
                        UCFechamentoFinal.UppAlunos.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o anexo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCAlunoEfetivacaoObservacaoGeral_ReturnValues(CLS_AlunoAvaliacaoTurmaObservacao entityObservacaoSelecionada, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina)
        {
            try
            {
                if (UCFechamentoPadrao.Visible)
                {
                    UCFechamentoPadrao.UCAlunoEfetivacaoObservacao_ReturnValues(Convert.ToInt32(hdnIndiceObservacaoGeral.Value), entityObservacaoSelecionada, resultado);
                }
                else if (UCFechamentoFinal.Visible)
                {
                    byte parecerFinal = 0;
                    if (listaMatriculaTurmaDisciplina.Count > 0 && listaMatriculaTurmaDisciplina.Any(p => p.tud_id == Tud_id))
                    {
                        parecerFinal = listaMatriculaTurmaDisciplina.Find(p => p.tud_id == Tud_id).mtd_resultado;
                    }
                    UCFechamentoFinal.UCAlunoEfetivacaoObservacao_ReturnValues(Convert.ToInt32(hdnIndiceObservacaoGeral.Value), entityObservacaoSelecionada, listaAtualizacaoEfetivacao, resultado, parecerFinal);
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharObservacao", "var exibirMensagemConfirmacao=false;$('#divCadastroObservacaoGeral').dialog('close'); scrollToTop();", true);
                VS_JanelaObservacaoAberta = false;
                updObservacaoGeral.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCFechamento_AbrirObservacaoConselho(int indiceAluno, long alu_id, int mtu_id, string titulo, int tpc_id)
        {
            hdnIndiceObservacaoGeral.Value = indiceAluno.ToString();
            UCAlunoEfetivacaoObservacaoGeral.CarregarDadosAluno(alu_id, mtu_id, VS_tur_id, VS_Turma.esc_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), false, tpc_id);
            UCAlunoEfetivacaoObservacaoGeral.VS_MensagemLogEfetivacaoObservacao = titulo + " | ";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "divCadastroObservacaoGeral",
                "$('#divCadastroObservacaoGeral').dialog('option', 'title', '" + titulo + "');" +
                "var exibirMensagemConfirmacao=false;$('#divCadastroObservacaoGeral').dialog('open'); ", true);
            VS_JanelaObservacaoAberta = true;
            updObservacaoGeral.Update();
        }

        private void UCFechamento_AbrirBoletim(long alu_id, int tpc_id, int mtu_id)
        {
            UCBoletim.Carregar(tpc_id, alu_id, mtu_id);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BoletimCompletoAluno", "$(document).ready(function(){ $('#divBoletimCompleto').dialog('open'); });", true);
        }

        private void UCAlunoEfetivacaoObservacao_AbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno)
        {
            lblDadosRelatorio.Text = dadosAluno;
            hdnIndices.Value = idRelatorio;
            txtArea.Text = nota;

            if (!string.IsNullOrEmpty(arq_idRelatorio) && Convert.ToInt64(arq_idRelatorio) > 0)
            {
                SYS_Arquivo arq = new SYS_Arquivo { arq_id = Convert.ToInt64(arq_idRelatorio) };
                SYS_ArquivoBO.GetEntity(arq);
                if (!arq.IsNew)
                {
                    btnExcluirAnexoRelatorio.Visible = true;
                    hplAnexoRelatorio.NavigateUrl = "~/FileHandler.ashx?file=" + arq_idRelatorio;
                    hplAnexoRelatorio.Target = "_blank";
                    hplAnexoRelatorio.Text = arq.arq_nome;
                    hplAnexoRelatorio.Visible = true;
                }
                else
                {
                    btnExcluirAnexoRelatorio.Visible = false;
                    hplAnexoRelatorio.Visible = false;
                }
            }
            else
            {
                btnExcluirAnexoRelatorio.Visible = false;
                hplAnexoRelatorio.Visible = false;
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
        }

        private void UCFechamento_AbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno)
        {
            VS_JanelaObservacaoAberta = false;
            UCAlunoEfetivacaoObservacao_AbrirRelatorio(idRelatorio, nota, arq_idRelatorio, dadosAluno);
        }

        private void UCFechamento_AbrirRelatorioRP(long alu_id, string tds_idRP)
        {
            if (AbrirRelatorioRP != null)
            {
                AbrirRelatorioRP(alu_id, tds_idRP);
            }
        }

        private void UCFechamento_AbrirRelatorioAEE(long alu_id)
        {
            if (AbrirRelatorioAEE != null)
            {
                AbrirRelatorioAEE(alu_id);
            }
        }

        #endregion Eventos

        #region Fila de processamento

        private void UCFechamento_MostrarLoading(int tempoProcessar)
        {

            if (periodoFechado)
            {
                SetaProcessamentoConcluido();
            }
            else
            {
                var pendencias = CLS_AlunoFechamentoPendenciaBO.SelecionarAguardandoProcessamento(VS_tur_id, Tud_id, EntTurmaDisciplina.tud_tipo, VS_Avaliacao.tpc_id);

                if (pendencias == null || pendencias.Rows.Count == 0)
                {
                    SetaProcessamentoConcluido();
                }
                else
                {
                    // limpa cache desta turma
                    string pattern;
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_PATTERN_KEY, Tud_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY, Tud_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY, VS_tur_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_PATTERN_KEY, Tud_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY, Tud_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_PATTERN_KEY, VS_tur_id);
                    CacheManager.Factory.RemoveByPattern(pattern);
                    pattern = String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, VS_Turma.esc_id, VS_Turma.uni_id, VS_CalendarioAnual.cal_ano , Tud_id);
                    CacheManager.Factory.Remove(pattern);
                    AsyncProcessarFilaPendente = new CLS_AlunoFechamentoPendenciaBO.ProcessarFilaPendente(CLS_AlunoFechamentoPendenciaBO.Processar);

                    callback = RetornoProcessarFila;

                    // Inicia assincronamente a execução da fila de processamento caso haja necessidade ("Fura-fila").
                    AsyncProcessarFilaPendente.BeginInvoke(Tud_id, (byte)VS_Avaliacao.ava_tipo, pendencias, callback, null);

            imgLoading.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "ajax-loader.gif";
            lblLoading.Text = string.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.lblLoading.Text").ToString(), (tempoProcessar / 1000).ToString());
                    divLoading.Visible = true;
            tmrLoad.Interval = tempoProcessar;
            tmrLoad.Enabled = true;
                }
            }
        }

        CLS_AlunoFechamentoPendenciaBO.ProcessarFilaPendente AsyncProcessarFilaPendente;
        AsyncCallback callback;

        /// <summary>
        /// Joga o retorno do processamento em Sessão para ser utilizado no timer.
        /// </summary>
        /// <param name="ar"></param>
        private void RetornoProcessarFila(IAsyncResult ar)
        {
            Session["resultFila"] = ar;
        }

        private void UCFechamento_EsconderLoading()
        {
            divLoading.Visible = false;
            tmrLoad.Enabled = false;
            tmrLoad.Tick -= tmrLoad_Tick;
        }

        protected void tmrLoad_Tick(object sender, EventArgs e)
        {
            if (Session["resultFila"] == null
                || !((IAsyncResult)Session["resultFila"]).IsCompleted)
            {
                int numeroTentativas = Convert.ToInt32(hdnTentativas.Value);
                numeroTentativas++;

                int maxTentativas = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA_TENTATIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                // se ainda nao atingiu o numero maximo de tentativas para verificacao da fila
                if (numeroTentativas < maxTentativas)
                {
                    // atualizo o numero de tentativas
                    // e continuo aguardando ate a proxima verificacao
                    hdnTentativas.Value = numeroTentativas.ToString();
                }
                // se nao foi possivel processar
                else
                {
                    // mostra uma mensagem de que nao foi possivel processar
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.MensagemErroProcessamento").ToString(), UtilBO.TipoMensagem.Alerta);

                    // esconde o loading
                    UCFechamento_EsconderLoading();
                }
            }
            // se foi possivel processar
            else
            {
                // Limpa o resultado guardado em sessão.
                Session["resultFila"] = null;
                // esconde o loading e ajustar as telas.
                SetaProcessamentoConcluido();
            }
        }

        private void SetaProcessamentoConcluido()
        {
                UCFechamento_EsconderLoading();

            if (UCFechamentoPadrao.Visible)
                UCFechamentoPadrao.SetaProcessamentoConcluido();

            if (UCFechamentoFinal.Visible)
                UCFechamentoFinal.SetaProcessamentoConcluido();
        }

        #endregion
    }
}