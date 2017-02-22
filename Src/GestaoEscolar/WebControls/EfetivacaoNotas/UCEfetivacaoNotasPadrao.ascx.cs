using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.EfetivacaoNotas
{
    public partial class UCEfetivacaoNotasPadrao : MotherUserControl
    {
        #region Constantes

        // constantes criadas para a ordem das colunas do grid de efetivação de notas
        private const int colunaIdade = 2;
        private const int colunaAvaliacaoEja = 3;
        private const int colunaNotaAdicional = 4;
        private const int colunaNota = 5;
        private const int colunaNotaPosConselho = 6;
        private const int colunaFaltoso = 7;
        private const int colunaNotaRegencia = 8;
        private const int colunaAulas = 9;
        private const int colunaFaltas = 10;
        private const int colunaAusenciasCompensadas = 11;
        private const int colunaFrequencia = 12;
        private const int colunaFrequenciaAcumulada = 13;
        private const int colunaFrequenciaFinal = 14;
        private const int colunaFrequenciaAjustada = 15;
        private const int colunaObservacaoDisciplina = 16;
        private const int colunaObservacaoConselho = 17;
        private const int colunaAtualizaFrequencia = 18;
        private const int colunaResultado = 19;
        private const int colunaBoletim = 20;

        // constantes criadas para a ordem das colunas da tabela de efetivação de notas
        // para os componentes da Regencia
        private const int colunaComponenteRegenciaNota = 1;

        private const int colunaComponenteRegenciaNotaPosConselho = 2;
        private bool[] visibilidadeColunasComponenteRegencia = new bool[3] { true, false, false };

        #endregion Constantes

        #region Propriedades

        #region Armazenadas no ViewState

        /// <summary>
        /// Retorna se os txts de nota e frequência devem ser habilitados para lançamentos de dados de anos anteriores.
        /// </summary>
        private bool HabilitarLancamentosAnoLetivoAnterior
        {
            get
            {
                int anoHabilitar = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade
                   (eChaveAcademico.ANO_LETIVO_ABRIR_FECHAMENTO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (anoHabilitar > 0)
                {
                    return (VS_CalendarioAnual.cal_ano == anoHabilitar);
                }

                return false;
            }
        }

        /// <summary>
        /// Guarda o valor do parametro
        /// </summary>
        private bool DesabilitarLancamentoNotaEfetivacao
        {
            get
            {
                return
                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade
                    (eChaveAcademico.DESABILITAR_LANCAMENTO_NOTA_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                    && !HabilitarLancamentosAnoLetivoAnterior;
            }
        }

        /// <summary>
        /// Retorna o valor mínimo para aprovação, caso:
        /// - A efetivação esteja sendo no conceito global (sem VS_Tud_id);
        /// - A escala de avaliação do conceito global seja do tipo numérica;
        /// - Tenha sido informado um valor mínimo para aprovação do conceito global do tipo numérico (float) válido.
        /// </summary>
        private double VS_NotaMinima
        {
            get
            {
                if (ViewState["VS_NotaMinima"] == null)
                {
                    if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    {
                        string valorMinimo = (Tud_id <= 0
                                                  ? VS_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal
                                                  : VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina);
                        double ret;
                        if (double.TryParse(valorMinimo, out ret))
                        {
                            ViewState["VS_NotaMinima"] = ret;
                        }
                        else
                        {
                            ViewState["VS_NotaMinima"] = -1;
                        }
                    }
                    else
                    {
                        ViewState["VS_NotaMinima"] = -1;
                    }
                }

                return double.Parse(ViewState["VS_NotaMinima"].ToString(), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Retorna a ordem do parecer mínimo para aprovação, caso:
        /// - A efetivação esteja sendo no conceito global (sem VS_Tud_id);
        /// - A escala de avaliação do conceito global seja do tipo pareceres;
        /// - Tenha sido informado um parecer mínimo para aprovação do conceito global no formato de avaliação.
        /// </summary>
        private int VS_ParecerMinimo
        {
            get
            {
                if (ViewState["VS_ParecerMinimo"] == null)
                {
                    if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    {
                        string valorMinimo = (Tud_id <= 0
                              ? VS_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal
                              : VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina);

                        // Retorna a ordem do parecer da nota normal.
                        int i = RetornaOrdemParecer(valorMinimo, false);

                        ViewState["VS_ParecerMinimo"] = i;
                    }
                    else
                    {
                        ViewState["VS_ParecerMinimo"] = -1;
                    }
                }

                return Convert.ToInt32(ViewState["VS_ParecerMinimo"].ToString());
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
        /// </summary>
        private long _VS_tur_id
        {
            get
            {
                return _UCEfetivacaoNotas._VS_tur_id;
            }
        }

        /// <summary>
        /// ViewState que armazena o ID do tipo de nível de ensino da turma.
        /// </summary>
        private int VS_tne_id
        {
            get
            {
                if (ViewState["VS_tne_id"] == null)
                {
                    if (_VS_tur_id > 0)
                    {
                        List<TUR_TurmaCurriculo> ltTurmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(_VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);
                        ACA_Curso curso = ACA_CursoBO.GetEntity(new ACA_Curso
                        {
                            cur_id = ltTurmaCurriculo.First().cur_id
                        });
                        ViewState["VS_tne_id"] = curso.tne_id;
                    }
                }

                return Convert.ToInt32(ViewState["VS_tne_id"] ?? "-1");
            }
        }

        /// <summary>
        /// Retorna o Tud_ID da disciplina principal
        /// </summary>
        private long Tud_idPrincipal
        {
            get
            {
                if (gvAlunos.DataKeys.Count > 0)
                {
                    // Recupera o valor salvo no DataKeys do grid.
                    return Convert.ToInt64(gvAlunos.DataKeys[0].Values["tud_idPrincipal"]);
                }

                if (gvAlunos.DataSource != null && gvAlunos.DataSource is DataTable)
                {
                    // Caso o DataKeys ainda não tenha sido carregado (acessando do header do grid).
                    // Busca o DataSource do grid, a primeira linha da tabela.
                    DataTable dt = (DataTable)gvAlunos.DataSource;
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt64(dt.Rows[0]["tud_idPrincipal"]);
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        private long Tud_id
        {
            get
            {
                string[] ids = _UCEfetivacaoNotas.TurmaDisciplina_Ids;

                if (ids.Length > 1)
                {
                    return Convert.ToInt64(ids[1]);
                }

                return -1;
            }
        }

        private int VS_tds_id
        {
            get
            {
                if (ViewState["VS_tud_id"] == null)
                    ViewState["VS_tud_id"] = Tud_id;

                if (Convert.ToInt64(ViewState["VS_tud_id"]) != Tud_id)
                {
                    ViewState["VS_tds_id"] = null;
                    ViewState["VS_tud_id"] = Tud_id;
                }

                if (ViewState["VS_tds_id"] == null)
                    ViewState["VS_tds_id"] = TUR_TurmaDisciplinaRelDisciplinaBO.GetSelectTdsBy_tud_id(Tud_id);

                return Convert.ToInt32(ViewState["VS_tds_id"]);
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        private int _VS_fav_id
        {
            get
            {
                return _UCEfetivacaoNotas._VS_fav_id;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cap_dataInicio
        /// </summary>
        private DateTime _VS_cap_dataInicio
        {
            get
            {
                if (ViewState["_VS_cap_dataInicio"] != null)
                {
                    return Convert.ToDateTime(ViewState["_VS_cap_dataInicio"]);
                }

                return new DateTime();
            }

            set
            {
                ViewState["_VS_cap_dataInicio"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cap_dataFim
        /// </summary>
        private DateTime _VS_cap_dataFim
        {
            get
            {
                if (ViewState["_VS_cap_dataFim"] != null)
                {
                    return Convert.ToDateTime(ViewState["_VS_cap_dataFim"]);
                }

                return new DateTime();
            }

            set
            {
                ViewState["_VS_cap_dataFim"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_id
        /// </summary>
        private int _VS_ava_id
        {
            get
            {
                return _UCEfetivacaoNotas._VS_ava_id;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tpc_id
        /// </summary>
        private int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_tipo
        /// </summary>
        private byte VS_ava_tipo
        {
            get
            {
                return Convert.ToByte(ViewState["VS_ava_tipo"] ?? "0");
            }

            set
            {
                ViewState["VS_ava_tipo"] = value;
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
        /// Propriedade que identifica qual botão de atualização foi usado (-1 = AtualizaTodos / Qualquer outro numero = Botão de Atualização de cada linha de aluno)
        /// </summary>
        private int _VS_IndiceBotaoAtualizaFrequencia
        {
            get
            {
                if (string.IsNullOrEmpty(hdnIndiceBtnAtualizar.Value))
                {
                    return -1;
                }

                return Convert.ToInt32(hdnIndiceBtnAtualizar.Value);
            }
            set
            {
                hdnIndiceBtnAtualizar.Value = value.ToString();
            }
        }

        /// <summary>
        /// Guarda as notas de relatório.
        /// </summary>
        private List<UCEfetivacaoNotas.NotasRelatorio> _VS_Nota_Relatorio
        {
            get
            {
                if (ViewState["_VS_Nota_Relatorio"] != null)
                {
                    return (List<UCEfetivacaoNotas.NotasRelatorio>)ViewState["_VS_Nota_Relatorio"];
                }

                return new List<UCEfetivacaoNotas.NotasRelatorio>();
            }

            set
            {
                ViewState["_VS_Nota_Relatorio"] = value;
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
                            ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(VS_Turma.cal_id, VS_Turma.tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                        )
                    );
            }
        }

        /// <summary>
        /// Guarda as justificativas de nota pós-conselho
        /// </summary>
        private List<UCEfetivacaoNotas.Justificativa> VS_JustificativaPosConselho
        {
            get
            {
                if (ViewState["VS_JustificativaPosConselho"] != null)
                {
                    return (List<UCEfetivacaoNotas.Justificativa>)ViewState["VS_JustificativaPosConselho"];
                }

                return new List<UCEfetivacaoNotas.Justificativa>();
            }

            set
            {
                ViewState["VS_JustificativaPosConselho"] = value;
            }
        }

        private bool ExibeCompensacaoAusencia
        {
            get
            {
                if (ViewState["_VS_Exibe_Compensacao_Ausencia"] == null)
                {
                    ViewState["_VS_Exibe_Compensacao_Ausencia"] = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                        (
                            (VS_turmaDisciplinaCompartilhada != null && !EntTurmaDisciplina.tud_naoLancarFrequencia)
                            || (VS_turmaDisciplinaCompartilhada == null && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta))
                            || (__SessionWEB.__UsuarioWEB.Docente.doc_id <= 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                        );
                }

                return Convert.ToBoolean(ViewState["_VS_Exibe_Compensacao_Ausencia"]);
            }
        }

        /// <summary>
        /// Entidade da turma selecionada no combo.
        /// </summary>
        private TUR_Turma VS_Turma
        {
            get
            {
                return _UCEfetivacaoNotas.VS_Turma;
            }
        }

        /// <summary>
        /// Entidade do calendário da turma selecionada no combo.
        /// </summary>
        private ACA_CalendarioAnual VS_CalendarioAnual
        {
            get
            {
                return _UCEfetivacaoNotas.VS_CalendarioAnual;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_FormatoAvaliacao VS_FormatoAvaliacao
        {
            get
            {
                return _UCEfetivacaoNotas.VS_FormatoAvaliacao;
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do formato. Se for lançamento na disciplina,
        /// retorna de acordo com o esa_idPorDisciplina, se for global, retorna
        /// o esa_idConceitoGlobal.
        /// </summary>
        private ACA_EscalaAvaliacao VS_EscalaAvaliacao
        {
            get
            {
                return _UCEfetivacaoNotas.VS_EscalaAvaliacao;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_EscalaAvaliacaoNumerica VS_EscalaNumerica
        {
            get
            {
                return _UCEfetivacaoNotas.VS_EscalaNumerica;
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do docente que está configurado no formato (esa_idDocente).
        /// </summary>
        private ACA_EscalaAvaliacao VS_EscalaAvaliacaoDocente
        {
            get
            {
                return _UCEfetivacaoNotas.VS_EscalaAvaliacaoDocente;
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação da avaliação adicional do Conceito Global.
        /// </summary>
        private ACA_EscalaAvaliacao VS_EscalaAvaliacaoAdicional
        {
            get
            {
                return _UCEfetivacaoNotas.VS_EscalaAvaliacaoAdicional;
            }
        }

        /// <summary>
        /// Retorna a avaliação selecionada na tela de busca.
        /// </summary>
        private ACA_Avaliacao VS_Avaliacao
        {
            get
            {
                return _UCEfetivacaoNotas.VS_Avaliacao;
            }
        }

        /// <summary>
        /// Retorna uma flag, caso o formato de avaliação possua o conceito global e esteja selecionado
        /// o conceito global na tela, se está marcado para ter avaliação adicional no conceito global.
        /// </summary>
        public bool PossuiAvaliacaoAdicional
        {
            get
            {
                return _UCEfetivacaoNotas.PossuiAvaliacaoAdicional;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_EscalaAvaliacaoNumerica VS_EscalaNumericaAdicional
        {
            get
            {
                return _UCEfetivacaoNotas.VS_EscalaNumericaAdicional;
            }
        }

        /// <summary>
        /// Lista de IDs das turmas normais dos alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        public List<long> VS_listaTur_ids
        {
            get
            {
                return _UCEfetivacaoNotas.VS_listaTur_ids;
            }
        }

        #endregion Armazenadas no ViewState

        #region Usadas no DataBound

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

        private List<ACA_EscalaAvaliacaoParecer> _ltPareceres;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceres
        {
            get
            {
                return _ltPareceres ??
                       (_ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(VS_EscalaAvaliacao.esa_id));
            }
        }

        private List<ACA_EscalaAvaliacaoParecer> _ltPareceresAdicional;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação da avaliação adicional.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceresAdicional
        {
            get
            {
                return _ltPareceresAdicional ??
                       (_ltPareceresAdicional =
                        ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(VS_EscalaAvaliacaoAdicional.esa_id));
            }
        }

        /// <summary>
        /// Flag que retorna se é a disciplina "Resultado final" = quando formato de
        /// avaliação for "Por disciplina" e a avaliação é Final ou Per+Final.
        /// </summary>
        private bool resultadoFinal
        {
            get
            {
                AvaliacaoTipo tipo = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
                ACA_FormatoAvaliacaoTipo tipoFormato = (ACA_FormatoAvaliacaoTipo)VS_FormatoAvaliacao.fav_tipo;

                return (Tud_id <= 0) &&
                        (tipoFormato == ACA_FormatoAvaliacaoTipo.Disciplina) &&
                        ((tipo == AvaliacaoTipo.PeriodicaFinal) ||
                         (tipo == AvaliacaoTipo.Final));
            }
        }

        /// <summary>
        /// Lista referente aos alunos nas disciplinas componentes da Regencia
        /// </summary>
        private List<AlunosEfetivacaoPadraoComponenteRegencia> listaAlunosComponentesRegencia;

        /// <summary>
        /// Guarda se eh avaliacao do ultimo periodo, usada no dataBound do grid de alunos.
        /// </summary>
        private bool avaliacaoUltimoPerido;

        private List<CLS_AlunoFrequenciaExterna> listaFrequenciaExterna;

        #endregion Usadas no DataBound

        #region Parâmetros acadêmicos

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

        #endregion Parâmetros acadêmicos

        /// <summary>
        /// Retorna uma flag informando se é para calcular o resultado automaticamente.
        /// </summary>
        public bool CalcularResultadoAutomatico
        {
            get
            {
                return VS_Avaliacao.ava_id > 0 && gvAlunos.Columns[colunaResultado].Visible
                       && !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                       && !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITAR_APROVACAO_MANUAL_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool periodoFechado
        {
            get
            {
                DateTime dataAtual = DateTime.Now;

                bool fechado = dataAtual > _VS_cap_dataFim && dataAtual < _VS_cap_dataInicio;

                switch (VS_ava_tipo)
                {
                    case (byte)AvaliacaoTipo.Periodica:
                    case (byte)AvaliacaoTipo.PeriodicaFinal:
                        fechado |= !VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoNotas);
                        break;

                    case (byte)AvaliacaoTipo.Recuperacao:
                        if (VS_tpc_id > 0)
                        {
                            fechado |= !VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoRecuperacao);
                        }
                        else
                        {
                            if (_VS_fav_id > 0 && _VS_ava_id > 0)
                            {
                                List<int> tpc_ids = ACA_AvaliacaoRelacionadaBO.RetornaPeriodoCalendarioRelacionadosPorAvaliacao(_VS_fav_id, _VS_ava_id).Split(',').Select(p => Convert.ToInt32(p)).ToList();
                                fechado |= !VS_ListaEventos.Exists(p => tpc_ids.Contains(p.tpc_id) && p.tev_id == tev_EfetivacaoRecuperacao);
                            }
                        }
                        break;

                    case (byte)AvaliacaoTipo.Final:
                        fechado |= !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal);
                        break;

                    case (byte)AvaliacaoTipo.RecuperacaoFinal:
                        fechado |= !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoRecuperacaoFinal);
                        break;
                }

                return fechado;
            }
        }

        private bool? existeDispensaDisciplina;

        /// <summary>
        /// Informa se há aluno(s) com dispensa de disciplina.
        /// </summary>
        private bool ExisteDispensaDisciplina
        {
            get
            {
                return (bool)(existeDispensaDisciplina ?? (existeDispensaDisciplina = false));
            }
        }

        /// <summary>
        /// Informa se havera tratamento de regencia
        /// </summary>
        private bool ExibirItensRegencia
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ITENS_REGENCIA_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
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
                                ViewState["VS_ltPermissaoCompensacao"] =
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(_UCEfetivacaoNotas.VS_posicao, (byte)EnumModuloPermissao.Compensacoes)
                            )
                        );
            }
        }

        /// <summary>
        /// DataTable com os tipos de resultados do curso/currículo/período da turma
        /// </summary>
        private List<Struct_TipoResultado> VS_dtTiposResultados
        {
            get
            {
                if (ViewState["VS_dtTiposResultados"] != null)
                    return (List<Struct_TipoResultado>)ViewState["VS_dtTiposResultados"];
                VS_dtTiposResultados = new List<Struct_TipoResultado>();
                return VS_dtTiposResultados;
            }
            set
            {
                ViewState["VS_dtTiposResultados"] = value;
            }
        }

        /// <summary>
        /// Tipo do colaborador selecionado no gridview para edição
        /// </summary>
        private bool VS_BuscouTiposResultados
        {
            get
            {
                if (ViewState["VS_BuscouTiposResultados"] != null)
                    return Convert.ToBoolean(ViewState["VS_BuscouTiposResultados"]);
                return false;
            }
            set
            {
                ViewState["VS_BuscouTiposResultados"] = value;
            }
        }

        /// <summary>
        /// Flag que indica se a disciplina é oferecia para alunos de libras.
        /// </summary>
        private bool VS_DisciplinaEspecial
        {
            get
            {
                return EntTurmaDisciplina.tud_disciplinaEspecial;
            }
        }

        public UpdatePanel UppAlunos
        {
            get
            {
                return uppGridAlunos;
            }
        }

        private Label _lblMessage
        {
            get
            {
                return _UCEfetivacaoNotas.LblMessage;
            }
        }

        private Label _lblMessage2
        {
            get
            {
                return _UCEfetivacaoNotas.LblMessage2;
            }
        }

        private Label _lblMessage3
        {
            get
            {
                return _UCEfetivacaoNotas.LblMessage3;
            }
        }

        private Panel pnlAlunos
        {
            get
            {
                return _UCEfetivacaoNotas.PnlAlunos;
            }
        }

        private TUR_TurmaDisciplina VS_turmaDisciplinaCompartilhada
        {
            get
            {
                return (TUR_TurmaDisciplina)_UCEfetivacaoNotas.VS_turmaDisciplinaCompartilhada;
            }
        }

        private UCEfetivacaoNotas _UCEfetivacaoNotas
        {
            get
            {
                return (UCEfetivacaoNotas)this.Parent.Parent;
            }
        }

        /// <summary>
        /// Guarda o numero de casas para arredondademento da frequencia do aluno
        /// </summary>
        private int VS_NumeroCasasDecimaisFrequencia
        {
            get
            {
                return (int)(ViewState["VS_NumeroCasasDecimaisFrequencia"] ?? 0);
            }

            set
            {
                ViewState["VS_NumeroCasasDecimaisFrequencia"] = value;
            }
        }

        /// <summary>
        /// Retorna a string de formatacao para a frequencia com base no numero de casas decimais
        /// </summary>
        public string VS_FormatacaoDecimaisFrequencia
        {
            get
            {
                return ViewState["VS_FormatacaoDecimaisFrequencia"].ToString() ?? "{0:0.00}";
            }

            set
            {
                ViewState["VS_FormatacaoDecimaisFrequencia"] = value;
            }
        }

        public bool HabilitaBoletimAluno
        {
            get
            {
                if (ViewState["HabilitaBoletimAluno"] != null)
                    return Convert.ToBoolean(ViewState["HabilitaBoletimAluno"]);
                return false;
            }
            set
            {
                ViewState["HabilitaBoletimAluno"] = value;
            }
        }

        /// <summary>
        /// Guarda a quantidade de aulas dadas exibida no label e usa para verificar pendência em disciplinas
        /// que não lançam nota (a pendência é se tem alguma aula criada no bimestre também).
        /// </summary>
        public int VS_QtdeAulasDadas
        {
            get
            {
                if (ViewState["VS_QtdeAulasDadas"] == null)
                    return 0;

                return Convert.ToInt32(ViewState["VS_QtdeAulasDadas"]);
            }
            set
            {
                ViewState["VS_QtdeAulasDadas"] = value;
            }
        }

        private string AlunoInativo;

        private string CorAlunoNaoAvaliado;

        private string AlunoDispensado;

        private string AlunoFrequenciaLimite;

        private string CorAlunoProximoBaixaFrequencia;

        #endregion Propriedades 

        #region DELEGATES

        public delegate void commandObservacaoDisciplina(Int32 indiceAluno, Int64 tud_id, Int64 alu_id, Int32 mtu_id, Int32 mtd_id, string dadosAluno, string titulo, bool periodoFechado);

        public event commandObservacaoDisciplina AbrirObservacaoDisciplina;

        public delegate void commandObservacaoConselho(Int32 indiceAluno, Int64 tur_id, Int64 alu_id, Int32 mtu_id, string dadosAluno, string titulo, bool fav_avaliacaoFinalAnalitica, AvaliacaoTipo ava_tipo, int cal_id, int tpc_id, bool efetivacaoSemestral, bool periodoFechado, int ava_idUltimoBimestre = 0);

        public event commandObservacaoConselho AbrirObservacaoConselho;

        public delegate void commandBoletim(Int64 alu_id, Int32 tpc_id, Int32 mtu_id);

        public event commandBoletim AbrirBoletim;

        public delegate void commandAbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno);

        public event commandAbrirRelatorio AbrirRelatorio;

        public delegate void commandAbrirJustificativa(string id, string textoJustificativa, string pes_nome, string dis_nome);

        public event commandAbrirJustificativa AbrirJustificativa;

        #endregion DELEGATES

        #region Métodos

        /// <summary>
        /// Seta a disciplina e turma selecionados em tela que chamou o user control.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        public void SetaTurmaDisciplina()
        {
            _UCComboOrdenacao1.CarregaComboParametro();

            if (_VS_tur_id > 0)
            {
                //atualização de parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
                Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA();
            }
        }

        ///// <summary>
        ///// Atualiza parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
        ///// </summary>
        private void Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA()
        {
            divAvaliacao.InnerText = "Avaliação do aluno na turma de " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
        }

        #region Carregar grid de alunos

        /// <summary>
        /// Retorna se existe alunos com fechamento pendente a partir da lista de alunos do grid.
        /// </summary>
        /// <param name="dt">Lista de alunos do grid</param>
        /// <returns></returns>
        private bool RetornaAlunosFechamentoPendente<T>(List<T> lista, bool tud_naoLancarNota)
        {
            if (!tud_naoLancarNota && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
            {
                var listaVerificacao = from T item in lista
                                       let propInfoAvaliacaoID = item.GetType().GetProperty("AvaliacaoID")
                                       let propInfoAvaliacao = item.GetType().GetProperty("Avaliacao")
                                       let propInfoAvaliacaoPosConselho = item.GetType().GetProperty("avaliacaoPosConselho")
                                       select new
                                       {
                                           AvaliacaoID = Convert.ToInt64(propInfoAvaliacaoID.GetValue(item, null) ?? "0")
                                           ,
                                           Avaliacao = (propInfoAvaliacao.GetValue(item, null) ?? string.Empty).ToString()
                                           ,
                                           AvaliacaoPosConselho = (propInfoAvaliacaoPosConselho.GetValue(item, null) ?? string.Empty).ToString()
                                       };

                object lockObject = new object();
                bool pendente = false;

                Parallel.ForEach
                (
                    listaVerificacao,
                    () => false,
                    (dadosGeral, loopState, alunoPendente) =>
                    {
                        return
                            alunoPendente
                            ||
                            dadosGeral.AvaliacaoID <= 0
                            // ou a nota do aluno no fechamento esta vazia
                            || (
                                String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                && String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                            )
                            // ou nota incompativel com o tipo de escala de avaliacao
                            || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica
                                &&
                                (
                                    (!String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                    && String.IsNullOrEmpty(NotaFormatada(dadosGeral.Avaliacao)))
                                    ||
                                    (!String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                                    && String.IsNullOrEmpty(NotaFormatada(dadosGeral.AvaliacaoPosConselho)))
                                )
                            )
                            || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Pareceres
                                &&
                                (
                                    (!String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                    && !LtPareceres.Any(p => p.eap_valor.Equals(dadosGeral.Avaliacao)))
                                    ||
                                    (!String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                                    && !LtPareceres.Any(p => p.eap_valor.Equals(dadosGeral.AvaliacaoPosConselho)))
                                )
                            );
                    },
                    (alunoPendente) =>
                    {
                        lock (lockObject)
                        {
                            pendente |= alunoPendente;
                        }
                    }
                );

                return pendente;
            }
            else
            {
                var listaVerificacao = from T item in lista
                                       let propInfoAvaliacaoID = item.GetType().GetProperty("AvaliacaoID")
                                       let propInfoQtAulasEfetivado = item.GetType().GetProperty("QtAulasEfetivado")
                                       select new
                                       {
                                           AvaliacaoID = Convert.ToInt64(propInfoAvaliacaoID.GetValue(item, null) ?? "0")
                                           ,
                                           QtAulasEfetivado = Convert.ToInt32(propInfoQtAulasEfetivado.GetValue(item, null) ?? "0")
                                       };

                if (VS_QtdeAulasDadas <= 0 && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                {
                    // Quando é tud_naoLancarNota ou recuperação paralela, é necessário verificar se tem alguma
                    // aula criada no bimestre.
                    return true;
                }

                object lockObject = new object();
                bool pendente = false;

                Parallel.ForEach
                (
                    listaVerificacao,
                    () => false,
                    (dadosGeral, loopState, alunoPendente) =>
                    {
                        return
                            alunoPendente
                            || dadosGeral.AvaliacaoID <= 0
                            // ou o numero de aulas é menor que 1
                            || (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia && dadosGeral.QtAulasEfetivado < 1);
                    },
                    (alunoPendente) =>
                    {
                        lock (lockObject)
                        {
                            pendente |= alunoPendente;
                        }
                    }
                );

                return pendente;
            }
        }

        /// <summary>
        /// Carrega o grid com os alunos por turma ou disciplina.
        /// </summary>
        private void CarregarGridAlunos()
        {
            VS_tpc_id = VS_Avaliacao.tpc_id;
            List<AlunosEfetivacaoDisciplinaPadrao> listaDisciplina = new List<AlunosEfetivacaoDisciplinaPadrao>();
            List<AlunosEfetivacaoTurmaPadrao> listaTurma = new List<AlunosEfetivacaoTurmaPadrao>();
            // Escala do conceito global.
            int esa_id = VS_EscalaAvaliacao.esa_id;

            ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoLancamento = (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

            // Valor do conceito global ou por disciplina.
            string valorMinimo = Tud_id > 0 ?
                VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina :
                VS_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

            AvaliacaoTipo tipoAvaliacao = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
            VS_ava_tipo = (byte)tipoAvaliacao;

            EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            string avaliacaoesRelacionadas = string.Empty;

            double notaMinimaAprovacao = 0;
            int ordemParecerMinimo = 0;
            int cacheLongo = ApplicationWEB.AppMinutosCacheLongo;
            int cacheMedio = ApplicationWEB.AppMinutosCacheMedio;

            Parallel.Invoke
            (
                () =>
                {
                    #region Calcula numero Casas decimais da frequencia

                    VS_NumeroCasasDecimaisFrequencia = GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(VS_FormatoAvaliacao.fav_variacao);
                    VS_FormatacaoDecimaisFrequencia =
                        "{" + GestaoEscolarUtilBO.CriaFormatacaoDecimal((VS_FormatoAvaliacao.fav_variacao > 0 ? VS_NumeroCasasDecimaisFrequencia : 2), "0:{0}") + "}";

                    #endregion Calcula numero Casas decimais da frequencia
                },
                () =>
                {
                    if (tipoEscala == EscalaAvaliacaoTipo.Numerica)
                    {
                        notaMinimaAprovacao = double.Parse(valorMinimo.Replace(',', '.'), CultureInfo.InvariantCulture);
                    }
                    else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                    {
                        ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(esa_id, valorMinimo, cacheLongo);
                    }
                },
                () =>
                {
                    // Retorna uma string com os ava_idRelacionada separados por ",".
                    avaliacaoesRelacionadas = ACA_AvaliacaoRelacionadaBO.RetornaRelacionadasPor_Avaliacao(_VS_fav_id, _VS_ava_id, cacheMedio);
                }
            );

            DateTime cap_dataInicio, cap_dataFim;

            if (Tud_id > 0)
            {
                ACA_CalendarioPeriodoBO.RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina(VS_tpc_id, avaliacaoesRelacionadas, Tud_id, _VS_fav_id, tipoAvaliacao, VS_Turma.cal_id, out cap_dataInicio, out cap_dataFim);

                _VS_cap_dataInicio = cap_dataInicio;
                _VS_cap_dataFim = cap_dataFim;

                // Se a avaliação for do tipo "Recuperação Final" carrega os alunos de forma diferente
                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                {
                    // Busca os alunos por disciplina na turma.
                    listaDisciplina = VS_DisciplinaEspecial ?
                        MTR_MatriculaTurmaDisciplinaBO.GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
                        (
                            Tud_id,
                            _VS_tur_id,
                            VS_tpc_id,
                            _VS_ava_id,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            avaliacaoesRelacionadas,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            _UCEfetivacaoNotas.VS_tipoDocente,
                            false,
                            ApplicationWEB.AppMinutosCacheFechamento
                        ) :
                        MTR_MatriculaTurmaDisciplinaBO.GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
                        (
                            Tud_id,
                            _VS_tur_id,
                            VS_tpc_id,
                            _VS_ava_id,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            avaliacaoesRelacionadas,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            false,
                            ApplicationWEB.AppMinutosCacheFechamento
                        );
                }
                else
                {
                    // Busca os alunos por disciplina na turma.
                    listaDisciplina = VS_DisciplinaEspecial ?
                        MTR_MatriculaTurmaDisciplinaBO.GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
                        (
                            Tud_id,
                            _VS_tur_id,
                            VS_tpc_id,
                            _VS_ava_id,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            (byte)tipoAvaliacao,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            avaliacaoesRelacionadas,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            , VS_Turma.tur_tipo
                            , VS_Turma.cal_id
                            , _UCEfetivacaoNotas.VS_tipoDocente
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , false
                            , ApplicationWEB.AppMinutosCacheFechamento
                            , VS_listaTur_ids
                        ) :
                        MTR_MatriculaTurmaDisciplinaBO.GetSelectBy_TurmaDisciplinaPeriodo
                        (
                            Tud_id,
                            _VS_tur_id,
                            VS_tpc_id,
                            _VS_ava_id,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            (byte)tipoAvaliacao,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            avaliacaoesRelacionadas,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            , VS_Turma.tur_tipo
                            , VS_Turma.cal_id
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , EntTurmaDisciplina.tud_tipo
                            , VS_Avaliacao.tpc_ordem
                            , VS_FormatoAvaliacao.fav_variacao
                            , false
                            , ApplicationWEB.AppMinutosCacheFechamento
                            , VS_listaTur_ids
                        );

                    // Se for disciplina de regencia, carrego os dados referentes aos componentes da regencia
                    if (listaDisciplina.Any() && EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                    {
                        DataTable dtAlunos = new DataTable();
                        dtAlunos.Columns.Add("alu_id");
                        dtAlunos.Columns.Add("mtu_id");

                        object lockObject = new Object();

                        Parallel.ForEach
                        (
                            listaDisciplina,
                            aluno =>
                            {
                                lock (lockObject)
                                {
                                    DataRow drAluno = dtAlunos.NewRow();
                                    drAluno["alu_id"] = aluno.alu_id;
                                    drAluno["mtu_id"] = aluno.mtu_id;
                                    dtAlunos.Rows.Add(drAluno);
                                }
                            }
                        );

                        listaAlunosComponentesRegencia = MTR_MatriculaTurmaDisciplinaBO.GetSelect_ComponentesRegencia_By_TurmaFormato
                                                        (
                                                            _VS_tur_id,
                                                            VS_tpc_id,
                                                            _VS_ava_id,
                                                            _VS_fav_id,
                                                            (byte)tipoAvaliacao,
                                                            esa_id,
                                                            (byte)tipoEscala,
                                                            VS_EscalaAvaliacaoDocente.esa_tipo,
                                                            avaliacaoesRelacionadas,
                                                            notaMinimaAprovacao,
                                                            ordemParecerMinimo,
                                                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                            , VS_Turma.tur_tipo
                                                            , dtAlunos
                                                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                            , ApplicationWEB.AppMinutosCacheFechamento
                                                        );
                    }
                }
            }
            else
            {
                ACA_CalendarioPeriodoBO.RetornaDatasPeriodoPor_FormatoAvaliacaoTurma(VS_tpc_id, avaliacaoesRelacionadas, _VS_tur_id, _VS_fav_id, tipoAvaliacao, VS_Turma.cal_id, out cap_dataInicio, out cap_dataFim);

                _VS_cap_dataInicio = cap_dataInicio;
                _VS_cap_dataFim = cap_dataFim;

                // Se a avaliação for do tipo "Recuperação Final" carrega os alunos de forma diferente
                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                {
                    // Busca os alunos pela turma
                    listaTurma = MTR_MatriculaTurmaBO.GetSelectBy_Alunos_RecuperacaoFinal_By_Turma(
                        _VS_tur_id,
                        VS_tpc_id,
                        _VS_ava_id,
                        Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                        _VS_fav_id,
                        esa_id,
                        (byte)tipoEscala,
                        avaliacaoesRelacionadas,
                        notaMinimaAprovacao,
                        ordemParecerMinimo,
                        (byte)tipoLancamento,
                        VS_EscalaAvaliacaoAdicional.esa_tipo,
                        false,
                        ApplicationWEB.AppMinutosCacheFechamento);
                }
                else
                {
                    // Busca os alunos pela turma
                    listaTurma = MTR_MatriculaTurmaBO.GetSelectBy_Turma_Periodo(
                        _VS_tur_id,
                        VS_tpc_id,
                        _VS_ava_id,
                        Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                        _VS_fav_id,
                        (byte)tipoAvaliacao,
                        esa_id,
                        (byte)tipoEscala,
                        avaliacaoesRelacionadas,
                        notaMinimaAprovacao,
                        ordemParecerMinimo,
                        (byte)tipoLancamento,
                        VS_EscalaAvaliacaoAdicional.esa_tipo,
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                        false,
                        ApplicationWEB.AppMinutosCacheFechamento
                        );
                }
            }

            // Ordenação dos alunos.
            int numeroChamada;
            listaDisciplina = Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue) == 0 ?
                                    listaDisciplina.OrderBy(p => Int32.TryParse(p.mtd_numeroChamada, out numeroChamada) ? numeroChamada : -1).ThenBy(p => p.pes_nome).ToList()
                                    : listaDisciplina.OrderBy(p => p.pes_nome).ToList();


            if (Tud_id > 0 && VS_tpc_id > 0 && listaDisciplina != null && listaDisciplina.Count > 0)
            {
                List<MTR_MatriculaTurmaDisciplina> listaMtds = 
                    listaDisciplina.Select(p => new MTR_MatriculaTurmaDisciplina { alu_id = p.alu_id, mtu_id = p.mtu_id, mtd_id = p.mtd_id }).ToList();

                // Buscar frequências externas para exibir na tela.
                listaFrequenciaExterna = CLS_AlunoFrequenciaExternaBO.SelecionaPor_MatriculasDisciplinaPeriodo(listaMtds, VS_tpc_id);
            }

            // Mostra total de aulas cadastradas no período.
            SetaQuantidadeAulas(VS_tpc_id);

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_STATUS_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)                
                && (EntTurmaDisciplina.tud_tipo != (int)TurmaDisciplinaTipo.TerritorioSaber))
            {
                bool tud_naoLancarNota = false;
                if (Tud_id > 0)
                {
                    tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
                }
                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && !tud_naoLancarNota)
                {
                    if (listaAlunosComponentesRegencia != null && listaAlunosComponentesRegencia.Any() && listaDisciplina != null && listaDisciplina.Any())
                    {
                        this._UCEfetivacaoNotas.AtualizarStatusEfetivacao(RetornaAlunosFechamentoPendente(listaAlunosComponentesRegencia, tud_naoLancarNota));
                    }
                }
                else if (Tud_id > 0)
                {
                    if (listaDisciplina != null && listaDisciplina.Any())
                    {
                        this._UCEfetivacaoNotas.AtualizarStatusEfetivacao(RetornaAlunosFechamentoPendente(listaDisciplina, tud_naoLancarNota));
                    }
                }
                else if (listaTurma != null && listaTurma.Any())
                {
                    this._UCEfetivacaoNotas.AtualizarStatusEfetivacao(RetornaAlunosFechamentoPendente(listaTurma, tud_naoLancarNota));
                }
            }

            if (Tud_id > 0)
            {
                Parallel.Invoke
                (
                    () => SetaDadosRelatorio(listaDisciplina, "atd_relatorio", "atd_semProfessor"),
                    () =>
                    {
                        // Se for disciplina de regencia, carrego os dados referentes aos componentes da regencia
                        if (listaDisciplina.Any() && EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            Parallel.Invoke
                            (
                                () => SetaDadosRelatorio(listaAlunosComponentesRegencia, "atd_relatorio", "atd_semProfessor"),
                                () => SetaDadosJustificativaPosConselho(listaAlunosComponentesRegencia)
                            );
                        }
                    },
                    () => SetaDadosJustificativaPosConselho(listaDisciplina)
                );
            }
            else
            {
                Parallel.Invoke
                (
                    () => SetaDadosRelatorio(listaTurma, "aat_relatorio", "aat_semProfessor"),
                    () => SetaDadosJustificativaPosConselho(listaTurma)
                );

            }

            // Seta a visibilidade das colunas do grid de acordo com o tipo de avaliação.
            SetaColunasVisiveisGrid(VS_Avaliacao);

            // Seta nome dos headers das colunas de nota.
            SetaNomesColunas(tipoEscala, VS_FormatoAvaliacao);

            // Mostra total de aulas cadastradas no período.
            SetaQuantidadeAulas(VS_tpc_id);

            List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);
            int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
            avaliacaoUltimoPerido = ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.Periodica
                                        || (AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.PeriodicaFinal)
                                        && VS_Avaliacao.tpc_id > 0 && VS_Avaliacao.tpc_id == tpc_idUltimoPerido;

            if (Tud_id > 0)
            {
                gvAlunos.DataSource = listaDisciplina;
            }
            else
            {
                gvAlunos.DataSource = listaTurma;
            }

            gvAlunos.DataBind();


            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            VS_dtTiposResultados.Clear();

            object sync = new object();

            AlunoInativo = ApplicationWEB.AlunoInativo;
            CorAlunoNaoAvaliado = ApplicationWEB.CorAlunoNaoAvaliado;
            AlunoDispensado = ApplicationWEB.AlunoDispensado;
            AlunoFrequenciaLimite = ApplicationWEB.AlunoFrequenciaLimite;
            CorAlunoProximoBaixaFrequencia = ApplicationWEB.CorAlunoProximoBaixaFrequencia;
            bool habilitarAnoAnterior = HabilitarLancamentosAnoLetivoAnterior;

            Parallel.ForEach
                     (
                         (from GridViewRow row in gvAlunos.Rows
                          select row)
                         ,
                         row =>
                         {
                             if (!habilitarAnoAnterior)
                             {
                                 // Seta eventos dos botões de atualizar frequência.
                                 SetaEventosBtnAtualizar(row);
                             }

                             if (row.RowType == DataControlRowType.DataRow)
                             {
                                 ConfiguraDadosAlunoLinhaGrid(row);

                                 long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
                                 int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_id"]);

                                 if (gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                                 {
                                     TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");

                                     if (txtFrequenciaAcumulada != null && string.IsNullOrEmpty(txtFrequenciaAcumulada.Text))
                                     {
                                         lock (sync)
                                         {
                                             CalculaFrequenciaAcumulada(row, alu_id, mtu_id);
                                         }
                                     }
                                 }

                                 DropDownList ddlResultado = (DropDownList)row.FindControl("ddlResultado");

                                 lock (sync)
                                 {
                                     AdicionaItemsResultado(ddlResultado, alu_id, mtu_id);
                                 }

                                 if (ddlResultado != null)
                                 {
                                     HiddenField hdnResultado = (HiddenField)row.FindControl("hdnResultado");
                                     ddlResultado.SelectedValue = hdnResultado.Value;
                                 }

                                 TextBox txtNota = (TextBox)row.FindControl("txtNota");
                                 HiddenField hdnNota = (HiddenField)row.FindControl("hdnNota");
                                 DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                                 HiddenField hdnRecuperacaoPorNota = (HiddenField)row.FindControl("hdnRecuperacaoPorNota");

                                 TextBox txtNotaPosConselho = (TextBox)row.FindControl("txtNotaPosConselho");
                                 DropDownList ddlPareceresPosConselho = (DropDownList)row.FindControl("ddlPareceresPosConselho");
                                 HiddenField hdnNotaPosConselho = (HiddenField)row.FindControl("hdnNotaPosConselho");

                                 TextBox txtNotaAdicional = (TextBox)row.FindControl("txtNotaAdicional");
                                 DropDownList ddlPareceresAdicional = (DropDownList)row.FindControl("ddlPareceresAdicional");
                                 HiddenField hdnAvaliacaoAdicional = (HiddenField)row.FindControl("hdnAvaliacaoAdicional");

                                 bool exibeCampoNotaAluno = true;

                                 if ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.RecuperacaoFinal)
                                 {
                                     exibeCampoNotaAluno = Convert.ToBoolean(hdnRecuperacaoPorNota.Value);
                                 }

                                 lock (sync)
                                 {
                                     // Seta campos da avaliação principal.
                                     SetaCamposAvaliacao(tipo, txtNota, hdnNota.Value, ddlPareceres, false, exibeCampoNotaAluno, row);

                                     // Seta campos da avaliação pós-conselho.
                                     SetaCamposAvaliacaoPosConselho(tipo, txtNotaPosConselho, hdnNotaPosConselho.Value, ddlPareceresPosConselho, VS_Avaliacao.ava_exibeNotaPosConselho, row);

                                     // Seta campos da avaliação adicional.
                                     SetaCamposAvaliacao(
                                         (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo,
                                         txtNotaAdicional,
                                         hdnAvaliacaoAdicional.Value,
                                         ddlPareceresAdicional,
                                         true,
                                         exibeCampoNotaAluno,
                                         row);

                                     SetaComponentesRelatorioLinhaGrid(row, exibeCampoNotaAluno);

                                     SetaComponentesRelatorioPosConselhoLinhaGrid(row, VS_Avaliacao.ava_exibeNotaPosConselho);

                                 }

                                 if (ExibirItensRegencia && listaAlunosComponentesRegencia != null && listaAlunosComponentesRegencia.Any())
                                 {
                                     Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");
                                     LinkButton btnExpandir = (LinkButton)row.FindControl("btnExpandir");
                                     btnExpandir.ToolTip = gvAlunos.Columns[colunaNota].HeaderText;

                                     rptComponenteRegencia.DataSource = (from aluno in listaAlunosComponentesRegencia
                                                                         where aluno.alu_id == alu_id
                                                                         select aluno);
                                     lock (sync)
                                     {
                                         rptComponenteRegencia.DataBind();
                                     }
                                 }
                             }
                         }
                     );

            if (ExibirItensRegencia && listaAlunosComponentesRegencia != null && listaAlunosComponentesRegencia.Any())
            {
                Parallel.ForEach
                (
                    (from GridViewRow row in gvAlunos.Rows
                     let rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia")
                     from RepeaterItem item in rptComponenteRegencia.Items
                     where item.ItemType == ListItemType.Item ||
                           item.ItemType == ListItemType.AlternatingItem
                     select item)
                     ,
                     item =>
                     {
                         TextBox txtNota = (TextBox)item.FindControl("txtNota");
                         DropDownList ddlPareceres = (DropDownList)item.FindControl("ddlPareceres");
                         HiddenField hdnAvaliacao = (HiddenField)item.FindControl("hdnAvaliacao");
                         SetaCamposAvaliacao(
                               (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo,
                               txtNota,
                               hdnAvaliacao.Value,
                               ddlPareceres,
                               false,
                               true,
                               null,
                               item);

                         //
                         // CARREGA CAMPO NOTA POS-CONSELHO
                         //
                         TextBox txtNotaPosConselho = (TextBox)item.FindControl("txtNotaPosConselho");
                         DropDownList ddlPareceresPosConselho = (DropDownList)item.FindControl("ddlPareceresPosConselho");
                         HiddenField hdnAvaliacaoPosConselho = (HiddenField)item.FindControl("hdnAvaliacaoPosConselho");
                         // Seta campos da avaliação pós-conselho.
                         SetaCamposAvaliacaoPosConselho(
                             (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo,
                             txtNotaPosConselho,
                             hdnAvaliacaoPosConselho.Value,
                             ddlPareceresPosConselho,
                             VS_Avaliacao.ava_exibeNotaPosConselho,
                             null,
                             item);

                     }
                );
            }

            divLegenda.Visible = gvAlunos.Rows.Count > 0;
            HabilitarControlesTela((_UCEfetivacaoNotas.usuarioPermissao && _UCEfetivacaoNotas.DocentePodeEditar) && pnlAlunos.Visible && !periodoFechado && _UCEfetivacaoNotas.usuarioPermissao);

            if (ExisteDispensaDisciplina)
                lblMessageInfo.Text = UtilBO.GetErroMessage("Alunos com dispensa de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                                            " terão suas notas e frequência desconsideradas.", UtilBO.TipoMensagem.Informacao);
        }

        /// <summary>
        /// Verifica se é necessário abrir a popup de confirmação pra atualizar notas/frequências, se
        /// for pra abrir a popup seta o evento OnClientClick nos botões para evitar postback no servidor,
        /// está muito lento.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        private void SetaEventosBtnAtualizar(GridViewRow row)
        {
            if (row.RowType == DataControlRowType.Header ||
                row.RowType == DataControlRowType.DataRow)
            {
                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
                EscalaAvaliacaoTipo tipoAdicional = (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo;

                // Permite atualização de nota se a nota (da disciplina) for numérica ou por permitir salvar
                // nota final no lançamento de avaliações.
                bool atualizaNota = gvAlunos.Columns[colunaNota].Visible && Tud_id > 0
                       && (tipo == EscalaAvaliacaoTipo.Numerica ||
                       ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                // Permite atualização de nota se tiver nota adicional numérica e tiver a disciplina principal.
                bool atualizaNotaAdicional = gvAlunos.Columns[colunaNotaAdicional].Visible && Tud_idPrincipal > 0
                    && tipoAdicional == EscalaAvaliacaoTipo.Numerica;

                // Permite atualização da frequência caso a coluna de faltas esteja visível.
                bool atualizaFrequencia = gvAlunos.Columns[colunaFaltas].Visible;

                // Verifico se é conceito global
                bool atualizaNotaConceitoGlobal = gvAlunos.Columns[colunaNota].Visible && Tud_id <= 0;

                // Exibe a popup de atualizar a frequência, vai exibir se tiver nota e frequência disponível para
                // atualização.
                if ((atualizaNota || atualizaNotaAdicional) && atualizaFrequencia
                    &&
                    VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.Final
                    &&
                    // teste abaixo foi incluído para evitar que o popup seja aberto qdo existir para atualizar nota ou falta,
                    // ou seja para abrir deverá ter os dois.
                    (!((atualizaNotaConceitoGlobal && (atualizaNota || atualizaNotaAdicional) && atualizaFrequencia)
                    ||
                    ((atualizaNota || atualizaNotaAdicional) && !atualizaFrequencia)
                    ||
                    (atualizaFrequencia && !atualizaNota && !atualizaNotaAdicional))))
                {
                    ImageButton btnFrequencia = (ImageButton)row.FindControl(
                        (row.RowType == DataControlRowType.Header ? "btnTodasFrequencias" : "btnFrequencia"));
                    if (btnFrequencia != null)
                    {
                        int indice = row.RowType == DataControlRowType.Header ? -1 : row.RowIndex;

                        // Registra no evento do botão atualizar apenas o javascript para abrir a popup e setar
                        // o valor no hdnIndiceBtnAtualizar.
                        btnFrequencia.OnClientClick = "$('#divConfirmaAtualizacao').dialog('open');" +
                                "$('#" + hdnIndiceBtnAtualizar.ClientID + "').val('" + indice + "');" +
                                // Evita o evento que causa postback.
                                "return false;";
                    }
                }
            }
        }

        /// <summary>
        /// Seta nomes das colunas de nota e nota adicional.
        /// </summary>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="fav">Entidade de formato de avaliação</param>
        private void SetaNomesColunas(EscalaAvaliacaoTipo tipoEscala, ACA_FormatoAvaliacao fav)
        {
            string nomeNota = "Nota";

            if ((_UCEfetivacaoNotas.VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
            {
                nomeNota = "Média";
            }
            else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
            {
                nomeNota = "Conceito";
            }
            else if (tipoEscala == EscalaAvaliacaoTipo.Relatorios)
            {
                nomeNota = "Relatório";
            }

            gvAlunos.Columns[colunaNota].HeaderText = nomeNota;
            gvAlunos.Columns[colunaNotaPosConselho].HeaderText = nomeNota + " pós-conselho";
            //gvAlunos.Columns[colunaNotaRegencia].HeaderText = nomeNota;
            _UCEfetivacaoNotas.AlterarTituloJustificativa("Justificativa " + nomeNota.ToLower() + " pós-conselho");

            if (gvAlunos.Columns[colunaNotaAdicional].Visible)
            {
                // Nome da coluna de Avaliação Adicional.
                string nomeNotaAdicional = "Nota";
                EscalaAvaliacaoTipo tipoEscalaAdicional = (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo;

                if ((_UCEfetivacaoNotas.VS_EfetivacaoSemestral) && (tipoEscalaAdicional == EscalaAvaliacaoTipo.Numerica))
                {
                    nomeNotaAdicional = "Média";
                }
                else if (tipoEscalaAdicional == EscalaAvaliacaoTipo.Pareceres)
                {
                    nomeNotaAdicional = "Conceito";
                }
                else if (tipoEscalaAdicional == EscalaAvaliacaoTipo.Relatorios)
                {
                    nomeNotaAdicional = "Relatório";
                }

                gvAlunos.Columns[colunaNotaAdicional].HeaderText = nomeNotaAdicional;
            }

            switch (fav.fav_tipoApuracaoFrequencia)
            {
                case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula:
                    gvAlunos.Columns[colunaAulas].HeaderText = "Qtde. tempos de aula";
                    break;

                case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia:
                    gvAlunos.Columns[colunaAulas].HeaderText = "Qtde. dias de aulas";
                    break;
            }

            if (fav.fav_tipoLancamentoFrequencia == (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPrevistasDocente)
            {
                gvAlunos.Columns[colunaAulas].HeaderText = "Aulas previstas";
            }
        }

        /// <summary>
        /// Formata a nota de acordo com o número de casas decimais.
        /// </summary>
        /// <param name="nota">Nota.</param>
        /// <returns>Nota formatada.</returns>
        private string NotaFormatada(string nota)
        {
            if (!string.IsNullOrEmpty(nota))
            {
                decimal notaNumerica;
                decimal.TryParse(nota, out notaNumerica);

                return NotaFormatada(notaNumerica);
            }

            return nota;
        }

        /// <summary>
        /// Formata a nota de acordo com o número de casas decimais.
        /// </summary>
        /// <param name="nota">Nota.</param>
        /// <returns>Nota formatada.</returns>
        private string NotaFormatada(decimal nota)
        {
            return nota >= 0 ?
                Math.Round(nota, RetornaNumeroCasasDecimais()).ToString() :
                string.Empty;
        }

        /// <summary>
        /// Seta os campos relacionados à avaliação como visíveis, e seta os valores de acordo com
        /// o tipo de escala.
        /// Se escala numérica, configura o txt. Se escala for por pareceres, configura o ddl.
        /// </summary>
        /// <param name="tipo">Tipo de escala de avaliação</param>
        /// <param name="txtNota">Textbox de nota</param>
        /// <param name="aat_avaliacao">Nota do aluno</param>
        /// <param name="adicional">Indica se serão carregados os pareceres da avaliação adicional</param>
        /// <param name="ddlPareceres">Combo de pareceres</param>
        /// <param name="exibeCampoNotaAluno">Indica se vai exibir os campos de notas</param>
        /// <param name="row"></param>
        /// <param name="rptItem"></param>
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres, bool adicional, bool exibeCampoNotaAluno, GridViewRow row, RepeaterItem rptItem = null)
        {
            if (txtNota != null)
            {
                if (exibeCampoNotaAluno && (tipo == EscalaAvaliacaoTipo.Numerica))
                {
                    txtNota.Visible = true;
                    txtNota.Text = NotaFormatada(aat_avaliacao);
                    if (row != null)
                    {
                        CheckBox chkFaltoso = (CheckBox)row.FindControl("chkFaltoso");

                        if (!string.IsNullOrEmpty(txtNota.Text))
                        {
                            double nota = double.Parse(txtNota.Text, CultureInfo.InvariantCulture);

                            if (VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao &&
                                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                chkFaltoso != null && !chkFaltoso.Checked &&
                                nota < VS_NotaMinima)
                            {
                                row.Cells[colunaNota].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
                            }
                        }
                    }
                }
                else
                {
                    txtNota.Visible = false;
                }
            }

            if (ddlPareceres != null)
            {
                ddlPareceres.Visible = exibeCampoNotaAluno && tipo == EscalaAvaliacaoTipo.Pareceres;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    CarregarPareceres(ddlPareceres, adicional);

                    int ordem = RetornaOrdemParecer(aat_avaliacao, adicional);

                    // Concatena eap_valor + eap_ordem.
                    ddlPareceres.SelectedValue = aat_avaliacao + ";" + ordem;

                    if (row != null)
                    {
                        CheckBox chkFaltoso = (CheckBox)row.FindControl("chkFaltoso");

                        if (VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao &&
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                            chkFaltoso != null && !chkFaltoso.Checked &&
                            ordem != -1 && ordem < VS_ParecerMinimo)
                        {
                            row.Cells[colunaNota].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
                        }
                    }
                }
            }

            if (VS_Avaliacao.ava_tipo == (byte)AvaliacaoTipo.RecuperacaoFinal
                && VS_Avaliacao.ava_recFinalConceitoMaximoAprovacao == (byte)ACA_Avaliacao_RecFinalConceitoMaximoAprovacao.Conceito_Nota_Minima_Aprovacao
                && tipo != EscalaAvaliacaoTipo.Relatorios

                // Apenas para a efetivação global
                && Tud_id <= 0

                // Apenas para a nota principal
                && !adicional)
            {
                string idValidatorNotaMaxima = (tipo == EscalaAvaliacaoTipo.Numerica
                                                    ? "cvNotaMaxima"
                                                    : "cvParecerMaximo");

                // Se for recuperação final e a nota máxima for a nota mínima para aprovação, seta nota máxima para os alunos.
                CustomValidator cv = row != null ? (CustomValidator)row.FindControl(idValidatorNotaMaxima) : (CustomValidator)rptItem.FindControl(idValidatorNotaMaxima);
                if (cv != null)
                {
                    cv.Visible = true;
                    if (tipo == EscalaAvaliacaoTipo.Numerica)
                    {
                        cv.ErrorMessage = "A nota máxima do aluno deve ser menor ou igual a " + VS_NotaMinima + ".";
                    }

                    if (tipo == EscalaAvaliacaoTipo.Pareceres)
                    {
                        string conceitoMaximo = (from ListItem item in ddlPareceres.Items
                                                 where item.Value.Split(';')[1] == VS_ParecerMinimo.ToString()
                                                 select item.Text
                                                ).FirstOrDefault();

                        cv.ErrorMessage = "O conceito máximo do aluno deve ser menor ou igual a " + conceitoMaximo + ".";
                    }
                }
            }
        }

        /// <summary>
        /// Seta os campos relacionados à avaliação pós-conselho como visíveis, e seta os valores de acordo com
        /// o tipo de escala.
        /// Se escala numérica, configura o txt. Se escala for por pareceres, configura o ddl.
        /// </summary>
        /// <param name="tipo">Tipo de escala de avaliação</param>
        /// <param name="txtNota">Textbox de nota</param>
        /// <param name="aat_avaliacao">Nota do aluno</param>
        /// <param name="adicional">Indica se serão carregados os pareceres da avaliação adicional</param>
        /// <param name="ddlPareceres">Combo de pareceres</param>
        /// <param name="exibeCampoNotaAluno">Indica se vai exibir os campos de notas</param>
        /// <param name="row"></param>
        private void SetaCamposAvaliacaoPosConselho(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres, bool exibeCampoNotaAluno, GridViewRow row, RepeaterItem rptItem = null)
        {
            if (txtNota != null)
            {
                txtNota.Visible = exibeCampoNotaAluno && tipo == EscalaAvaliacaoTipo.Numerica;
                if (tipo == EscalaAvaliacaoTipo.Numerica)
                    txtNota.Text = VS_Avaliacao.ava_exibeNotaPosConselho ? NotaFormatada(aat_avaliacao) : string.Empty;

                if (txtNota.Visible)
                {
                    if (row != null)
                    {
                        CheckBox chkFaltoso = (CheckBox)row.FindControl("chkFaltoso");

                        if (!string.IsNullOrEmpty(txtNota.Text))
                        {
                            double nota = double.Parse(txtNota.Text, CultureInfo.InvariantCulture);

                            if (VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao &&
                                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                chkFaltoso != null && !chkFaltoso.Checked &&
                                nota < VS_NotaMinima)
                            {
                                row.Cells[colunaNotaPosConselho].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
                            }
                        }
                    }

                    if (txtNota.Enabled)
                    {
                        ImageButton btnJustificativaPosConselho = row != null ?
                                (ImageButton)row.FindControl("btnJustificativaPosConselho") :
                                (ImageButton)rptItem.FindControl("btnJustificativaPosConselho");

                        Image imgJustificativaPosConselhoSituacao = row != null ?
                            (Image)row.FindControl("imgJustificativaPosConselhoSituacao") :
                            (Image)rptItem.FindControl("imgJustificativaPosConselhoSituacao");

                        if (string.IsNullOrEmpty(txtNota.Text))
                        {
                            if (btnJustificativaPosConselho != null)
                            {
                                btnJustificativaPosConselho.Enabled = false;
                            }

                            if (imgJustificativaPosConselhoSituacao != null)
                            {
                                imgJustificativaPosConselhoSituacao.Style["visibility"] = "hidden";
                            }
                        }
                        else
                        {
                            if (btnJustificativaPosConselho != null)
                            {
                                btnJustificativaPosConselho.Enabled = (_UCEfetivacaoNotas.usuarioPermissao && _UCEfetivacaoNotas.DocentePodeEditar) && pnlAlunos.Visible && !periodoFechado && _UCEfetivacaoNotas.usuarioPermissao;

                            }

                            if (imgJustificativaPosConselhoSituacao != null)
                            {
                                imgJustificativaPosConselhoSituacao.Style["visibility"] = "visible";
                            }
                        }
                    }
                }
            }

            if (ddlPareceres != null)
            {
                ddlPareceres.Visible = exibeCampoNotaAluno && tipo == EscalaAvaliacaoTipo.Pareceres;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    CarregarPareceres(ddlPareceres, false);

                    int ordem = RetornaOrdemParecer(aat_avaliacao, false);

                    // Concatena eap_valor + eap_ordem.
                    ddlPareceres.SelectedValue = VS_Avaliacao.ava_exibeNotaPosConselho ?
                        aat_avaliacao + ";" + ordem :
                        "-1;-1";

                    if (row != null)
                    {
                        CheckBox chkFaltoso = (CheckBox)row.FindControl("chkFaltoso");

                        if (VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao &&
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                            chkFaltoso != null && !chkFaltoso.Checked &&
                            ordem != -1 && ordem < VS_ParecerMinimo)
                        {
                            row.Cells[colunaNotaPosConselho].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
                        }
                    }

                    if (ddlPareceres.Enabled)
                    {
                        ImageButton btnJustificativaPosConselho = row != null ?
                                (ImageButton)row.FindControl("btnJustificativaPosConselho") :
                                (ImageButton)rptItem.FindControl("btnJustificativaPosConselho");

                        Image imgJustificativaPosConselhoSituacao = row != null ?
                            (Image)row.FindControl("imgJustificativaPosConselhoSituacao") :
                            (Image)rptItem.FindControl("imgJustificativaPosConselhoSituacao");

                        if (ddlPareceres.SelectedIndex <= 0)
                        {
                            if (btnJustificativaPosConselho != null)
                            {
                                btnJustificativaPosConselho.Enabled = false;
                            }

                            if (imgJustificativaPosConselhoSituacao != null)
                            {
                                imgJustificativaPosConselhoSituacao.Style["visibility"] = "hidden";
                            }
                        }
                        else
                        {
                            if (btnJustificativaPosConselho != null)
                            {
                                btnJustificativaPosConselho.Enabled = (_UCEfetivacaoNotas.usuarioPermissao && _UCEfetivacaoNotas.DocentePodeEditar) && pnlAlunos.Visible && !periodoFechado && _UCEfetivacaoNotas.usuarioPermissao;

                            }

                            if (imgJustificativaPosConselhoSituacao != null)
                            {
                                imgJustificativaPosConselhoSituacao.Style["visibility"] = "visible";
                            }
                        }
                    }
                }
            }

            if (VS_Avaliacao.ava_tipo == (byte)AvaliacaoTipo.RecuperacaoFinal
                && VS_Avaliacao.ava_recFinalConceitoMaximoAprovacao == (byte)ACA_Avaliacao_RecFinalConceitoMaximoAprovacao.Conceito_Nota_Minima_Aprovacao
                && tipo != EscalaAvaliacaoTipo.Relatorios

                // Apenas para a efetivação global
                && Tud_id <= 0)
            {
                string idValidatorNotaMaxima = (tipo == EscalaAvaliacaoTipo.Numerica
                                                    ? "cvNotaPosConselhoMaxima"
                                                    : "cvParecerPosConselhoMaximo");

                // Se for recuperação final e a nota máxima for a nota mínima para aprovação, seta nota máxima para os alunos.
                CustomValidator cv = row != null ? (CustomValidator)row.FindControl(idValidatorNotaMaxima) : (CustomValidator)rptItem.FindControl(idValidatorNotaMaxima);
                if (cv != null)
                {
                    cv.Visible = true;
                    if (tipo == EscalaAvaliacaoTipo.Numerica)
                    {
                        cv.ErrorMessage = "A nota pós-conselho máxima do aluno deve ser menor ou igual a " + VS_NotaMinima + ".";
                    }

                    if (tipo == EscalaAvaliacaoTipo.Pareceres)
                    {
                        string conceitoMaximo = (from ListItem item in ddlPareceres.Items
                                                 where item.Value.Split(';')[1] == VS_ParecerMinimo.ToString()
                                                 select item.Text
                                                ).FirstOrDefault();

                        cv.ErrorMessage = "O conceito pós-conselho máximo do aluno deve ser menor ou igual a " + conceitoMaximo + ".";
                    }
                }
            }
        }

        /// <summary>
        /// Mostra total de aulas dadas no período, se a coluna de faltas estiver visível
        /// na tela.
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        private void SetaQuantidadeAulas(int tpc_id)
        {
            if (lblQtdeAulas.Visible)
            {
                byte tipoLancamento = VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

                byte calculoQtdeAulasDadas = VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas;

                string qtAulas = CLS_AlunoAvaliacaoTurmaBO.CalculaQtdeTemposAula(_VS_tur_id, tpc_id, Tud_id, tipoLancamento, calculoQtdeAulasDadas).ToString();

                lblQtdeAulas.Text = qtAulas;
            }

            if (lblQtdeAulasDadas.Visible)
            {
                int qtdeAulasReposicao = 0;
                int qtdeAulasReposicaoPeriodo = 0;
                int qtdAulasDatasPeriodo = 0;

                byte posicaoEspecial = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Especial, ApplicationWEB.AppMinutosCacheLongo);

                string tdc_ids = VS_DisciplinaEspecial && _UCEfetivacaoNotas.VS_posicao == posicaoEspecial ? ((byte)EnumTipoDocente.Especial).ToString() :
                                                         string.Format("{0};{1};{2}",
                                                                        ((byte)EnumTipoDocente.Titular).ToString(),
                                                                        ((byte)EnumTipoDocente.Substituto).ToString(),
                                                                        ((byte)EnumTipoDocente.SegundoTitular).ToString());

                VS_QtdeAulasDadas = CLS_TurmaAulaBO.SelecionaQuantidadeAulasDadas(Tud_id, tpc_id, tdc_ids, out qtdeAulasReposicao, out qtdAulasDatasPeriodo, out qtdeAulasReposicaoPeriodo, null);

                lblQtdeAulasDadas.Text = UtilBO.GetErroMessage(
                    qtdeAulasReposicao > 0 ?
                    string.Format(GetGlobalResourceObject("WebControls", "EfetvacaoNotas.UCEfetivacaoNotasPadrao.QuantAulasDadasReposicao").ToString(), VS_QtdeAulasDadas.ToString(), qtdeAulasReposicao.ToString()) :
                    string.Format(GetGlobalResourceObject("WebControls", "EfetvacaoNotas.UCEfetivacaoNotasPadrao.QuantAulasDadas").ToString(), VS_QtdeAulasDadas.ToString()), UtilBO.TipoMensagem.Informacao);

                VS_QtdeAulasDadas = qtdAulasDatasPeriodo;

                // Caso seja uma disciplina do tipo experiência, exibe a quantidade de total de aulas do períodos
                if (lblTotalAulasExperiencia.Visible)
                {
                    lblTotalAulasExperiencia.Text = UtilBO.GetErroMessage(String.Format(
                        GetGlobalResourceObject("UserControl", "EfetvacaoNotas.UCEfetivacaoNotasPadrao.QuantTotalAulasExperiencia").ToString(),
                        qtdAulasDatasPeriodo.ToString()), UtilBO.TipoMensagem.Informacao);
                }
            }
        }

        /// <summary>
        /// Seta dados da opção de nota por relatório e dados da tela de acordo com os dados do
        /// DataTable informado.
        /// </summary>
        /// <param name="dt">DataTable para pegar os dados</param>
        /// <param name="nomeColunaRelatorio">Nome da coluna onde tem a nota por relatório</param>
        /// <param name="nomeColunaSemProfessor">Nome da coluna onde tem a informação se é sem professor, para setar o checkbox</param>
        private void SetaDadosRelatorio<T>(List<T> lista, string nomeColunaRelatorio, string nomeColunaSemProfessor) where T : struct
        {
            bool semProfessor = false;
            bool naoAvaliado = false;

            if (lista.Any())
            {
                PropertyInfo propInfo = lista.First().GetType().GetProperty(nomeColunaSemProfessor);
                semProfessor = Convert.ToBoolean(propInfo.GetValue(lista.First(), null));

                propInfo = lista.First().GetType().GetProperty("naoAvaliado");
                naoAvaliado = Convert.ToBoolean(propInfo.GetValue(lista.First(), null));

                _UCComboOrdenacao1.Visible = false;

                object lockObject = new object();

                Parallel.ForEach
                (
                    lista
                    ,
                    item =>
                    {
                        lock (lockObject)
                        {
                            propInfo = item.GetType().GetProperty("tur_id");
                            string id = propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("tud_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("alu_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("mtu_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("mtd_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("AvaliacaoID");
                            id += propInfo.GetValue(item, null) != null && !propInfo.GetValue(item, null).ToString().Equals("0") ?
                                propInfo.GetValue(item, null).ToString() :
                                "-1";

                            propInfo = item.GetType().GetProperty(nomeColunaRelatorio);
                            string valorRelatorio = (propInfo.GetValue(item, null) ?? string.Empty).ToString();

                            propInfo = item.GetType().GetProperty("arq_idRelatorio");
                            string arq_idRelatorio = (propInfo.GetValue(item, null) ?? string.Empty).ToString();

                            UCEfetivacaoNotas.NotasRelatorio rel = new UCEfetivacaoNotas.NotasRelatorio
                            {
                                Id = id,
                                Valor = valorRelatorio,
                                arq_idRelatorio = arq_idRelatorio
                            };

                            _VS_Nota_Relatorio.Add(rel);
                        }
                    }
                );

                _UCComboOrdenacao1.Visible = true;
            }

            chkSemProfessor.Checked = semProfessor && VS_Avaliacao.ava_exibeSemProfessor && chkSemProfessor.Visible;
            chkNaoAvaliado.Checked = naoAvaliado && VS_Avaliacao.ava_exibeNaoAvaliados;
        }

        /// <summary>
        /// Seta dados da justificativa de nota pós-conselho e dados da tela de acordo com os dados do
        /// DataTable informado.
        /// </summary>
        /// <param name="dt">DataTable para pegar os dados</param>
        private void SetaDadosJustificativaPosConselho<T>(List<T> lista) where T : struct
        {
            _UCComboOrdenacao1.Visible = false;

            if (lista.Any())
            {
                object lockObject = new object();

                Parallel.ForEach
                (
                    lista,
                    item =>
                    {
                        lock (lockObject)
                        {
                            PropertyInfo propInfo = item.GetType().GetProperty("tur_id");
                            string id = propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("tud_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("alu_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("mtu_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("mtd_id");
                            id += propInfo.GetValue(item, null).ToString() + ";";

                            propInfo = item.GetType().GetProperty("AvaliacaoID");
                            id += propInfo.GetValue(item, null) != null && !propInfo.GetValue(item, null).ToString().Equals("0") ?
                                propInfo.GetValue(item, null).ToString() :
                                "-1";

                            propInfo = item.GetType().GetProperty("justificativaPosConselho");
                            string justificativaPosConselho = (propInfo.GetValue(item, null) ?? string.Empty).ToString();

                            UCEfetivacaoNotas.Justificativa justificativa = new UCEfetivacaoNotas.Justificativa
                            {
                                Id = id,
                                Valor = justificativaPosConselho
                            };

                            VS_JustificativaPosConselho.Add(justificativa);
                        }
                    }
                );

                _UCComboOrdenacao1.Visible = true;
            }
        }

        /// <summary>
        /// Seta visibilidade das colunas do grid de acordo com as regras da tela.
        /// </summary>
        /// <param name="entAvaliacao">Entidade de avaliação selecionada</param>
        private void SetaColunasVisiveisGrid(ACA_Avaliacao entAvaliacao)
        {
            bool tud_naoLancarFrequencia = false;
            bool tud_naoLancarNota = false;
            if (Tud_id > 0)
            {
                tud_naoLancarFrequencia = EntTurmaDisciplina.tud_naoLancarFrequencia;
                tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
            }

            AvaliacaoTipo tipo = (AvaliacaoTipo)entAvaliacao.ava_tipo;
            int tpc_id = entAvaliacao.tpc_id;

            bool visibleQtTemposAula = (VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas !=
                                    (byte)ACA_FormatoAvaliacaoCalculoQtdeAulasDadas.Automatico) ||
                                   (Tud_id <= 0);

            TurmaDisciplinaTipo tipoDisciplina = (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo;

            bool permissaoConpensacao = VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta);
            bool permissaoBoletim = _UCEfetivacaoNotas.VS_ltPermissaoBoletim.Any(p => p.pdc_permissaoConsulta);

            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            gvAlunos.Columns[colunaIdade].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBE_IDADE_EFETIVACAO, ent_id);

            // Frequência: quando a avaliação for Periodica ou PeriodicaFinal ou Final.
            //             ou RecuperacaoFinal e existir período do calendário ligado
            gvAlunos.Columns[colunaFrequencia].Visible = !resultadoFinal &&
                                                          ((tipo == AvaliacaoTipo.Periodica) ||
                                                           (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                                           (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0));

            gvAlunos.Columns[colunaFrequencia].Visible &= visibleQtTemposAula;

            gvAlunos.Columns[colunaFrequencia].Visible |= entAvaliacao.ava_exibeFrequencia;

            gvAlunos.Columns[colunaFrequencia].Visible &= (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFrequencia].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFrequencia].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;
            gvAlunos.Columns[colunaFrequenciaAcumulada].Visible = !resultadoFinal &&
                                                                   (Tud_id <= 0) &&
                                                                   ((tipo == AvaliacaoTipo.Periodica) ||
                                                                    (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                                                    (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)) &&
                                                                   (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFrequenciaAcumulada].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFrequenciaAcumulada].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

            gvAlunos.Columns[colunaFrequenciaFinal].Visible = false;

            if (entAvaliacao.ava_ocultarAtualizacao || (tud_naoLancarNota && tud_naoLancarFrequencia))
            {
                gvAlunos.Columns[colunaAtualizaFrequencia].Visible = false;
            }
            else
            {
                // Atualizar frequência: quando a avaliação for Periodica ou PeriodicaFinal ou Final ou Recuperação Final e existir período do calendário ligado
                gvAlunos.Columns[colunaAtualizaFrequencia].Visible =
                    (
                        !resultadoFinal &&
                        (
                            (tipo == AvaliacaoTipo.Periodica)
                            || (tipo == AvaliacaoTipo.PeriodicaFinal)
                            || (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)
                            // [Task 29873] Caso esteja marcado para calcular a média da avaliação final e seja numérica a escala.
                            || (tipo == AvaliacaoTipo.Final && VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal
                                && VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                        )
                    )
                    || gvAlunos.Columns[colunaFrequenciaFinal].Visible
                    || entAvaliacao.ava_exibeFrequencia &&
                    (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));
            }

            //Coluna frequencia ajustada com as ausências cadastradas
            gvAlunos.Columns[colunaFrequenciaAjustada].Visible = ExibeCompensacaoAusencia && (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFrequenciaAjustada].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFrequenciaAjustada].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

            // Coluna com qt de faltas = coluna de frequencia.
            gvAlunos.Columns[colunaFaltas].Visible = !resultadoFinal &&
                                                      ((tipo == AvaliacaoTipo.Periodica) ||
                                                       (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                                       (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)) &&
                                                      (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFaltas].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFaltas].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

            // Coluna com qt de aulas = coluna de frequencia.
            gvAlunos.Columns[colunaAulas].Visible = !resultadoFinal &&
                                                     ((tipo == AvaliacaoTipo.Periodica) ||
                                                      (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                                      (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)) &&
                                                     (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaAulas].Visible &= visibleQtTemposAula;

            lblQtdeAulasCaption.Visible = !resultadoFinal &&
                                          ((tipo == AvaliacaoTipo.Periodica) ||
                                           (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                           (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)) &&
                                          (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia))
                                          && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_CAMPO_QTDE_DIAS_AULA, ent_id);
            lblQtdeAulasCaption.Visible &= visibleQtTemposAula;
            lblQtdeAulas.Visible = lblQtdeAulasCaption.Visible;

            gvAlunos.Columns[colunaAusenciasCompensadas].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_AUSENCIA_COMPENSADA_EFETIVACAO, ent_id)
                && (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaAusenciasCompensadas].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaAusenciasCompensadas].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

            // Faz a verificação de permissão de acesso a compensação de ausência, apenas se for docente
            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                gvAlunos.Columns[colunaAusenciasCompensadas].Visible &= ((VS_turmaDisciplinaCompartilhada != null && !EntTurmaDisciplina.tud_naoLancarFrequencia)
                                                                        || (VS_turmaDisciplinaCompartilhada == null && permissaoConpensacao));
            }

            // Resultado: quando a avaliação for Final ou PeriodicaFinal ou RecuperacaoFinal
            // Se a turma for de PEJA - mostra o resultado para as avaliações periódicas também.
            gvAlunos.Columns[colunaResultado].Visible =
                ((tipo == AvaliacaoTipo.PeriodicaFinal || tipo == AvaliacaoTipo.Final || tipo == AvaliacaoTipo.RecuperacaoFinal)
                || (_UCEfetivacaoNotas.VS_turma_Peja && tipo == AvaliacaoTipo.Periodica)) && (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.Regencia && ExibirItensRegencia));

            // A coluna resultado deve aparecer conforme a flag no formato de avaliação.
            // Se definida por "Disciplina", só aparece no lançamento da disciplina, senão só aparece
            // no conceito global.
            bool visibleResultado = Tud_id > 0
                                   ? VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == (byte)ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.NotaDisciplina
                                        || VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == (byte)ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.TodosAprovados
                                   : VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal !=
                                     (byte)ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.NotaDisciplina;

            gvAlunos.Columns[colunaResultado].Visible &= visibleResultado;

            gvAlunos.Columns[colunaBoletim].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_COLUNA_BOLETIM_MANUTENCAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            // Faz a verificação de permissão de acesso ao boletim, apenas se for docente
            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                gvAlunos.Columns[colunaBoletim].Visible &= (VS_turmaDisciplinaCompartilhada == null && permissaoBoletim);
            }

            gvAlunos.Columns[colunaObservacaoDisciplina].Visible = ((tipo == AvaliacaoTipo.Periodica) ||
                                              (tipo == AvaliacaoTipo.PeriodicaFinal)) &&
                                              VS_Avaliacao.ava_exibeObservacaoDisciplina && Tud_id > 0;

            gvAlunos.Columns[colunaObservacaoConselho].Visible = ((tipo == AvaliacaoTipo.Periodica) ||
                                                                  (tipo == AvaliacaoTipo.PeriodicaFinal)) &&
                                                                  VS_Avaliacao.ava_exibeObservacaoConselhoPedagogico;


            if (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.Regencia && ExibirItensRegencia))
            {
                gvAlunos.Columns[colunaNotaRegencia].Visible = false;
                if ((tipo == AvaliacaoTipo.Recuperacao || tipo == AvaliacaoTipo.RecuperacaoFinal))
                {
                    // Nota: esconde se for do tipo recuperação ou recuperação final
                    //       e não for baseada em conceito global e/ou nota por disciplina
                    gvAlunos.Columns[colunaNota].Visible = entAvaliacao.ava_baseadaConceitoGlobal || entAvaliacao.ava_baseadaNotaDisciplina;

                    gvAlunos.Columns[colunaNota].Visible &= !tud_naoLancarNota;

                    // Nota da avaliação adicional: esconde se for do tipo recuperação ou recuperação final
                    //                              e não for baseada em avaliação adicional
                    gvAlunos.Columns[colunaNotaAdicional].Visible = entAvaliacao.ava_baseadaAvaliacaoAdicional && PossuiAvaliacaoAdicional;

                    gvAlunos.Columns[colunaNotaAdicional].Visible &= !tud_naoLancarNota;
                }
                else
                {
                    // Nota: esconde se for "Resultado final" - formato por disciplina e avaliação final ou per + final.
                    gvAlunos.Columns[colunaNota].Visible = !resultadoFinal;

                    gvAlunos.Columns[colunaNota].Visible &= !tud_naoLancarNota;

                    // Nota da avaliação adicional.
                    gvAlunos.Columns[colunaNotaAdicional].Visible = !resultadoFinal && PossuiAvaliacaoAdicional;

                    gvAlunos.Columns[colunaNotaAdicional].Visible &= !tud_naoLancarNota;
                }

                // "Faltoso" - aparece quando é lançamento no Conceito Global e o tipo de avaliação é periódica.
                gvAlunos.Columns[colunaFaltoso].Visible = (tipo == AvaliacaoTipo.Periodica) &&
                                                           (VS_FormatoAvaliacao.fav_tipo !=
                                                            (byte)ACA_FormatoAvaliacaoTipo.Disciplina) &&
                                                           (Tud_id <= 0);

                // Verifica se a turma possui efetivação e se é disciplina
                // Verifica se COC da disciplina está marcado para ser avaliado ou lançar frequência;
                if (_UCEfetivacaoNotas.VS_EfetivacaoSemestral && Tud_id > 0)
                {
                    ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular entity = null;

                    // Verfica se é recuperação e busca o id do período por avaliação relacionada.
                    if (tipo == AvaliacaoTipo.Recuperacao ||
                        tipo == AvaliacaoTipo.RecuperacaoFinal)
                    {
                        DataTable dtAvaliacaoRec = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(tipo, entAvaliacao.fav_id);

                        if (dtAvaliacaoRec.Rows.Count > 0)
                        {
                            DataRow drRec = dtAvaliacaoRec.Rows.Cast<DataRow>().ToList().Find(p => Convert.ToInt32(p["ava_id"]) == entAvaliacao.ava_id);
                            if (drRec != null)
                            {
                                entity = _UCEfetivacaoNotas.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == Convert.ToInt32(drRec["tpc_id"]));
                            }
                        }
                    }
                    else
                    {
                        entity = _UCEfetivacaoNotas.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == entAvaliacao.tpc_id);
                    }

                    if (entity != null)
                    {
                        gvAlunos.Columns[colunaNota].Visible &= entity.csp_nota;
                    }
                    else
                    {
                        gvAlunos.Columns[colunaNota].Visible = false;
                    }
                }

                // Verifica o tud_id, porque não é possível efetivar a avaliação (PEJA) para a disciplina.
                gvAlunos.Columns[colunaAvaliacaoEja].Visible = VS_CursoSeriadoAvaliacoes && Tud_id <= 0;

                bool semAvaliacao = (Tud_id > 0 && chkSemProfessor.Checked) ||
                                    (Tud_id <= 0 && (chkSemProfessor.Checked || chkNaoAvaliado.Checked));

                // Mais uma condição nas colunas de nota = checkbox de professor checada.
                gvAlunos.Columns[colunaNota].Visible &= !semAvaliacao;
                gvAlunos.Columns[colunaNotaAdicional].Visible &= !semAvaliacao;
                gvAlunos.Columns[colunaResultado].Visible &= !semAvaliacao;
                gvAlunos.Columns[colunaFaltoso].Visible &= !semAvaliacao;
                gvAlunos.Columns[colunaAvaliacaoEja].Visible &= !semAvaliacao;

                gvAlunos.Columns[colunaNotaPosConselho].Visible = entAvaliacao.ava_exibeNotaPosConselho &&
                                                                  VS_EscalaAvaliacao.esa_tipo != (byte)EscalaAvaliacaoTipo.Relatorios;

                gvAlunos.Columns[colunaNotaPosConselho].Visible &= !tud_naoLancarNota;
            }
            else
            {
                gvAlunos.Columns[colunaNota].Visible =
                gvAlunos.Columns[colunaNotaAdicional].Visible =
                gvAlunos.Columns[colunaNotaPosConselho].Visible =
                gvAlunos.Columns[colunaAvaliacaoEja].Visible =
                gvAlunos.Columns[colunaFaltoso].Visible =
                gvAlunos.Columns[colunaResultado].Visible = false;
                gvAlunos.Columns[colunaNotaRegencia].Visible = tipoDisciplina == TurmaDisciplinaTipo.Regencia && ExibirItensRegencia;

                //
                // VISIBILIDADE DAS COLUNAS DOS COMPONENTES DA REGENCIA
                //
                if (gvAlunos.Columns[colunaNotaRegencia].Visible)
                {
                    if ((tipo == AvaliacaoTipo.Recuperacao || tipo == AvaliacaoTipo.RecuperacaoFinal))
                    {
                        // Nota: esconde se for do tipo recuperação ou recuperação final
                        //       e não for baseada em conceito global e/ou nota por disciplina
                        visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] = entAvaliacao.ava_baseadaConceitoGlobal || entAvaliacao.ava_baseadaNotaDisciplina;
                    }
                    else
                    {
                        // Nota: esconde se for "Resultado final" - formato por disciplina e avaliação final ou per + final.
                        visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] = !resultadoFinal;
                    }
                    // Verifica se a turma possui efetivação e se é disciplina
                    // Verifica se COC da disciplina está marcado para ser avaliado ou lançar frequência;
                    if (_UCEfetivacaoNotas.VS_EfetivacaoSemestral)
                    {
                        ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular entity = null;

                        // Verifica se é recuperação e busca o id do período por avaliação relacionada.
                        if (tipo == AvaliacaoTipo.Recuperacao || tipo == AvaliacaoTipo.RecuperacaoFinal)
                        {
                            DataTable dtAvaliacaoRec = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(tipo, entAvaliacao.fav_id);
                            if (dtAvaliacaoRec.Rows.Count > 0)
                            {
                                DataRow drRec = dtAvaliacaoRec.Rows.Cast<DataRow>().ToList().Find(p => Convert.ToInt32(p["ava_id"]) == entAvaliacao.ava_id);
                                if (drRec != null)
                                {
                                    entity = _UCEfetivacaoNotas.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == Convert.ToInt32(drRec["tpc_id"]));
                                }
                            }
                        }
                        else
                        {
                            entity = _UCEfetivacaoNotas.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == entAvaliacao.tpc_id);
                        }

                        if (entity != null)
                        {
                            visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] &= entity.csp_nota;
                        }
                        else
                        {
                            visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] = false;
                        }
                    }
                    // Mais uma condição nas colunas de nota = checkbox de professor checada.
                    visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] &= !chkSemProfessor.Checked;
                    visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] &= !tud_naoLancarNota;

                    visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaPosConselho] = entAvaliacao.ava_exibeNotaPosConselho && VS_EscalaAvaliacao.esa_tipo != (byte)EscalaAvaliacaoTipo.Relatorios;
                    visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaPosConselho] &= !tud_naoLancarNota;

                    if (!visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota]
                        && !visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaPosConselho])
                    {
                        gvAlunos.Columns[colunaNotaRegencia].Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Recarrega o repeater de alunos.
        /// </summary>
        private void ReCarregarGridAlunos()
        {
            CarregarGridAlunos();
        }

        /// <summary>
        /// Mostra data de matrícula/saída do aluno quando necessário.
        /// Coloca também cor para a linha de acordo com a situação da matrícula do aluno.
        /// </summary>
        /// <param name="e">Linha atual do grid</param>
        private void ConfiguraDadosAlunoLinhaGrid(GridViewRow row)
        {
            HiddenField hdnSituacao = (HiddenField)row.FindControl("hdnSituacao");
            int situacao = Convert.ToInt32(hdnSituacao.Value);

            Label lblAluno = (Label)row.FindControl("lblAluno");

            // Recupera a data de matrícula do aluno na turma/disciplina
            HiddenField hdnDataMatricula = (HiddenField)row.FindControl("hdnDataMatricula");
            if (!string.IsNullOrEmpty(hdnDataMatricula.Value))
            {
                DateTime dataMatricula = Convert.ToDateTime(hdnDataMatricula.Value);
                if (dataMatricula != new DateTime() && dataMatricula.Date > _VS_cap_dataInicio.Date)
                {
                    if (lblAluno != null)
                    {
                        lblAluno.Text += "<br/>" + "<b>Data de matrícula:</b> " + dataMatricula.ToString("dd/MM/yyyy");
                    }
                }
            }

            // Recupera a data de saída do aluno na turma/disciplina
            HiddenField hdnDataSaida = (HiddenField)row.FindControl("hdnDataSaida");
            if (!string.IsNullOrEmpty(hdnDataSaida.Value))
            {
                DateTime dataSaida = Convert.ToDateTime(hdnDataSaida.Value);
                if (dataSaida != new DateTime() && dataSaida.Date < _VS_cap_dataFim)
                {
                    if (lblAluno != null)
                    {
                        lblAluno.Text += "<br/>" + "<b>Data de saída:</b> " + dataSaida.ToString("dd/MM/yyyy");
                    }
                }
            }

            // Pinta a linha se o aluno estiver inativo.
            if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
            {
                row.Style["background-color"] = AlunoInativo;
            }
            else if (VS_CursoSeriadoAvaliacoes)
            {
                HiddenField hdnAvaliado = (HiddenField)row.FindControl("hdnAvaliado");
                bool ala_avaliado = hdnAvaliado.Value == "1";

                if (!ala_avaliado)
                {
                    row.Style["background-color"] = CorAlunoNaoAvaliado;
                }
            }

            HiddenField hdnDispensaDisciplina = (HiddenField)row.FindControl("hdnDispensaDisciplina");
            bool dispensadisciplina = hdnDispensaDisciplina.Value == "1";
            // Pinta a linha se há dispensa de disciplina para o aluno.
            if (dispensadisciplina)
            {
                row.Style["background-color"] = AlunoDispensado;
                if (!ExisteDispensaDisciplina)
                    existeDispensaDisciplina = true;
            }

            if (ExibeCompensacaoAusencia)
            {
                if (gvAlunos.Columns[colunaFrequencia].Visible)
                {
                    HiddenField hdnFrequenciaAjustada = (HiddenField)row.FindControl("hdnFrequenciaAjustada");
                    string txtFrequenciaFinalAjustada = hdnFrequenciaAjustada.Value;

                    HiddenField hdnQtAulas = (HiddenField)row.FindControl("hdnQtAulas");
                    int qtAulas = 0;
                    if (!string.IsNullOrEmpty(hdnQtAulas.Value))
                    {
                        qtAulas = Convert.ToInt32(hdnQtAulas.Value);
                    }

                    decimal frequencia;
                    if (Decimal.TryParse(txtFrequenciaFinalAjustada, out frequencia) && qtAulas > 0)
                    {
                        // se o formato de avaliacao tiver o percentual minimo de frequencia da disciplina cadastrado, devo utilizar esse valor,
                        // senao devo utilizar o percentual minimo de frequencia geral cadastrado para o formato de avaliacao
                        if ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0 && frequencia < VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                            || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0 && frequencia < VS_FormatoAvaliacao.percentualMinimoFrequencia))
                        {
                            row.Style["background-color"] = AlunoFrequenciaLimite;
                        }
                        // alunos proximos de atingir o percentual minimo de frequencia
                        else if (VS_FormatoAvaliacao.percentualBaixaFrequencia > 0
                                && ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0
                                        && frequencia >= VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
                                        && frequencia < VS_FormatoAvaliacao.percentualBaixaFrequencia)
                                    || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0
                                        && frequencia >= VS_FormatoAvaliacao.percentualMinimoFrequencia
                                        && frequencia < VS_FormatoAvaliacao.percentualBaixaFrequencia)))
                        {
                            row.Style["background-color"] = CorAlunoProximoBaixaFrequencia;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Carrega a mensagem das disciplinas divergentes por alunos, após o retorno do save do conceito global
        /// </summary>
        /// <param name="listDisciplinasDivergentesPorAluno">Lista de disciplinas divergentes por aluno</param>
        private void CarregaGridAlunosComDisciplinasDivergentes(List<sDisciplinasDivergentesPorAluno> listDisciplinasDivergentesPorAluno)
        {
            CarregarGridAlunos();
            List<string> disciplinasDivergentes;

            foreach (GridViewRow row in gvAlunos.Rows)
            {
                Label lblAluno = (Label)row.FindControl("lblAluno");
                long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);

                disciplinasDivergentes = listDisciplinasDivergentesPorAluno.FirstOrDefault(p => p.alu_id == alu_id).disciplinasDivergentes;

                if (lblAluno != null && disciplinasDivergentes != null && disciplinasDivergentes.Count > 0)
                {
                    lblAluno.Text = lblAluno.Text + "<br/>Divergência " +
                        (disciplinasDivergentes.Count == 1 ? "no(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " : " : "nos(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " : ") +
                        string.Join(",", disciplinasDivergentes.ToArray());
                    lblAluno.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        /// <summary>
        /// O método mostra os alunos com divergência na frequência.
        /// </summary>
        /// <param name="listAlunosComDivergencia"></param>
        private void CarregaGridAlunosDivergentes(List<long> listAlunosComDivergencia)
        {
            List<long> alunosDivergentes = (from GridViewRow row in gvAlunos.Rows
                                            let alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"])
                                            where listAlunosComDivergencia.Exists(p => p == alu_id)
                                            select alu_id).ToList();

            foreach (GridViewRow row in gvAlunos.Rows)
            {
                Label lblAluno = (Label)row.FindControl("lblAluno");
                Label lblNomeAluno = (Label)row.FindControl("lblNomeAluno");
                long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);

                if (lblAluno != null && lblNomeAluno != null)
                {
                    lblAluno.Text = lblNomeAluno.Text + (alunosDivergentes.Exists(p => p == alu_id) ? "<br/>Aluno com divergência na frequência." : string.Empty);
                    lblAluno.ForeColor = alunosDivergentes.Exists(p => p == alu_id) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                }
            }
        }

        /// <summary>
        /// Trata o evento do botao relatorio dentro do grid
        /// </summary>
        /// <param name="id"></param>
        private void TrataEventoCommandRelatorio(string id, string pes_nome, string dis_nome = null)
        {
            if (AbrirRelatorio != null)
            {
                string dadosAluno = "<b>Nome do aluno:</b> " + pes_nome;
                if (!String.IsNullOrEmpty(dis_nome))
                    dadosAluno += "<br /><b>" + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text") + ":</b> " + dis_nome;

                UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == id);
                AbrirRelatorio(id, nota.Valor, nota.arq_idRelatorio, dadosAluno);
            }
        }

        #endregion Carregar grid de alunos

        /// <summary>
        /// Esconde botões de salvar e grid de alunos - utilizado quando não existe avaliação ou disciplina
        /// nos combos.
        /// </summary>
        public void EscondeTelaAlunos(string mensagem)
        {
            pnlAlunos.Visible = false;
            _UCEfetivacaoNotas.VisibleBotaoSalvar = false;
            if (!String.IsNullOrEmpty(mensagem))
            {
                _lblMessage.Text = UtilBO.GetErroMessage(mensagem, UtilBO.TipoMensagem.Alerta);
            }
            gvAlunos.DataSource = new DataTable();
            gvAlunos.DataBind();
            VS_JustificativaPosConselho = new List<UCEfetivacaoNotas.Justificativa>();
        }

        /// <summary>
        /// Mostra o grid de alunos e botões de salvar.
        /// </summary>
        private void MostraTelaAlunos()
        {
            pnlAlunos.Visible = true;
            _UCEfetivacaoNotas.VisibleBotaoSalvar = !periodoFechado && _UCEfetivacaoNotas.usuarioPermissao;
        }

        /// <summary>
        /// Limpa valores de variáveis locais que são usadas a cada postback.
        /// </summary>
        private void LimpaVariaveisLocais()
        {
            _ltPareceres = null;
            _ltPareceresAdicional = null;
            ViewState["VS_ParecerMinimo"] = null;
            ViewState["VS_NotaMinima"] = null;
        }

        /// <summary>
        /// Seta eventos nos textbox que precisam recalcular a frequência acumulada.
        /// </summary>
        private void SetaEventosTxtFrequenciaAcumulada()
        {
            if (Tud_id > 0)
            {
                if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
                {
                    // Seta eventos nos txts para atualizar a frequência acumulada.
                    Parallel.ForEach
                    (
                        gvAlunos.Rows.Cast<GridViewRow>()
                        ,
                        row =>
                        {
                            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
                            if (txtQtdeAula != null)
                            {
                                txtQtdeAula.TextChanged += AtualizarFrequenciaAjustada;
                                txtQtdeAula.AutoPostBack = true;
                            }
                            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
                            if (txtQtdeFalta != null)
                            {
                                txtQtdeFalta.TextChanged += AtualizarFrequenciaAjustada;
                                txtQtdeFalta.AutoPostBack = true;
                            }

                            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
                            if (txtAusenciasCompensadas != null)
                            {
                                txtAusenciasCompensadas.TextChanged += AtualizarFrequenciaAjustada;
                                txtAusenciasCompensadas.AutoPostBack = true;
                            }
                        }
                    );
                }
            }
            else
            {
                if (gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                {
                    // Seta eventos nos txts para atualizar a frequência acumulada.
                    Parallel.ForEach
                    (
                        gvAlunos.Rows.Cast<GridViewRow>()
                        ,
                        row =>
                        {
                            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
                            if (txtQtdeAula != null)
                            {
                                txtQtdeAula.TextChanged += AtualizarFrequenciaAcumulada;
                                txtQtdeAula.AutoPostBack = true;
                            }
                            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
                            if (txtQtdeFalta != null)
                            {
                                txtQtdeFalta.TextChanged += AtualizarFrequenciaAcumulada;
                                txtQtdeFalta.AutoPostBack = true;
                            }
                        }
                    );
                }
            }
        }

        /// <summary>
        /// Seta enabled nos controles da tela, de acordo com o parâmetro.
        /// </summary>
        /// <param name="value">Indica se campo será habilitado/desabilitado</param>
        private void HabilitarControlesTela(bool value)
        {
            Parallel.Invoke
            (
                () =>
                {
                    HabilitaControles(pnlAlunos.Controls, value);
                },
                () =>
                {
                    HabilitaControles(_UCEfetivacaoNotas.FdsRelatorio.Controls, value);
                }
            );

            chkNaoAvaliado.Enabled = !chkSemProfessor.Checked || !chkSemProfessor.Visible;
            chkSemProfessor.Enabled = !chkNaoAvaliado.Checked || !chkNaoAvaliado.Visible;

            _UCEfetivacaoNotas.VisibleBotaoSalvar = value;

            if (value)
            {
                bool atualizaFrequencia = gvAlunos.Columns[colunaFaltas].Visible;

                _UCEfetivacaoNotas.TextBotaoCancelar = "Cancelar";

                if (gvAlunos.HeaderRow != null && HabilitarLancamentosAnoLetivoAnterior)
                {
                    ImageButton btnTodasFrequencias = (ImageButton)gvAlunos.HeaderRow.FindControl("btnTodasFrequencias");
                    btnTodasFrequencias.Enabled = atualizaFrequencia;
                }

                bool habilitarAnoAnterior = HabilitarLancamentosAnoLetivoAnterior;
                bool desabilitarNota = DesabilitarLancamentoNotaEfetivacao;


                //foreach (GridViewRow Row in gvAlunos.Rows)
                Parallel.ForEach
                (
                    gvAlunos.Rows.Cast<GridViewRow>()
                    ,
                    Row =>
                    {
                        TextBox txtQtdeAula = (TextBox)Row.FindControl("txtQtdeAula");
                        TextBox txtQtdeFalta = (TextBox)Row.FindControl("txtQtdeFalta");
                        TextBox txtAusenciasCompensadas = (TextBox)Row.FindControl("txtAusenciasCompensadas");
                        TextBox txtFrequencia = (TextBox)Row.FindControl("txtFrequencia");
                        TextBox txtFrequenciaAcumulada = (TextBox)Row.FindControl("txtFrequenciaAcumulada");
                        TextBox txtFrequenciaFinal = (TextBox)Row.FindControl("txtFrequenciaFinal");
                        Repeater rptComponenteRegencia = (Repeater)Row.FindControl("rptComponenteRegencia");

                        ImageButton btnFrequencia = (ImageButton)Row.FindControl("btnFrequencia");
                        if (btnFrequencia != null && habilitarAnoAnterior)
                        {

                            btnFrequencia.Enabled = atualizaFrequencia;
                        }

                        if (txtQtdeAula != null)
                        {
                            if (Tud_id > 0)
                            {
                                txtQtdeAula.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                    || habilitarAnoAnterior;
                            }
                            else
                            {
                                txtQtdeAula.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacao
                                    || habilitarAnoAnterior;
                            }
                        }

                        if (txtQtdeFalta != null)
                        {
                            if (Tud_id > 0)
                            {
                                txtQtdeFalta.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                        || habilitarAnoAnterior;
                            }
                            else
                            {
                                txtQtdeFalta.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacao
                                                        || habilitarAnoAnterior;
                            }
                        }

                        if (txtAusenciasCompensadas != null)
                        {
                            if (Tud_id > 0)
                            {
                                txtAusenciasCompensadas.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                                   || habilitarAnoAnterior;
                            }
                            else
                            {
                                txtAusenciasCompensadas.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacao
                                                                    || habilitarAnoAnterior;
                            }
                        }

                        if (txtFrequencia != null)
                        {
                            txtFrequencia.Enabled = !(gvAlunos.Columns[colunaAulas].Visible && gvAlunos.Columns[colunaFaltas].Visible);
                        }

                        if (txtFrequenciaAcumulada != null)
                        {
                            // Nunca habilita o txt de frequência acumulada, sempre será calculado.
                            txtFrequenciaAcumulada.Enabled = false;
                        }

                        if (txtFrequenciaFinal != null)
                        {
                            txtFrequenciaFinal.Enabled = false;
                        }

                        TextBox txtFrequenciaFinalAjustada = (TextBox)Row.FindControl("txtFrequenciaFinalAjustada");
                        if (txtFrequenciaFinalAjustada != null)
                        {
                            txtFrequenciaFinalAjustada.Enabled = false;
                        }

                        TextBox txtNota = (TextBox)Row.FindControl("txtNota");
                        if ((txtNota != null) && desabilitarNota)
                        {
                            txtNota.Enabled = false;
                        }

                        DropDownList ddlPareceres = (DropDownList)Row.FindControl("ddlPareceres");
                        if ((ddlPareceres != null) && desabilitarNota)
                        {
                            ddlPareceres.Enabled = false;
                        }

                        CheckBox chkFaltoso = (CheckBox)Row.FindControl("chkFaltoso");
                        bool faltoso = chkFaltoso.Checked;

                        if (!gvAlunos.Columns[colunaFaltoso].Visible)
                        {
                            Label lblFaltoso = (Label)Row.FindControl("lblFaltoso");
                            faltoso = Convert.ToBoolean(lblFaltoso.Text);
                        }

                        if (faltoso)
                        {
                            txtNota = (TextBox)Row.FindControl("txtNota");
                            ddlPareceres = (DropDownList)Row.FindControl("ddlPareceres");
                            TextBox txtNotaAdicional = (TextBox)Row.FindControl("txtNotaAdicional");
                            DropDownList ddlPareceresAdicional = (DropDownList)Row.FindControl("ddlPareceresAdicional");

                            txtNota.Text = string.Empty;
                            txtNotaAdicional.Text = string.Empty;
                            if (ddlPareceres.Items.Count > 0)
                            {
                                ddlPareceres.SelectedIndex = 0;
                            }

                            if (ddlPareceresAdicional.Items.Count > 0)
                            {
                                ddlPareceresAdicional.SelectedIndex = 0;
                            }

                            txtNota.Enabled = false;
                            ddlPareceres.Enabled = false;
                            txtNotaAdicional.Enabled = false;
                            ddlPareceresAdicional.Enabled = false;
                        }

                        DropDownList ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
                        ddlResultado.Enabled = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                        {
                            txtNota = (TextBox)rptItem.FindControl("txtNota");
                            if ((txtNota != null) && desabilitarNota)
                            {
                                txtNota.Enabled = false;
                            }

                            ddlPareceres = (DropDownList)rptItem.FindControl("ddlPareceres");
                            if ((ddlPareceres != null) && desabilitarNota)
                            {
                                ddlPareceres.Enabled = false;
                            }

                            Label lblFaltoso = (Label)rptItem.FindControl("lblFaltoso");
                            if (Convert.ToBoolean(lblFaltoso.Text))
                            {
                                txtNota.Text = string.Empty;
                                if (ddlPareceres.Items.Count > 0)
                                {
                                    ddlPareceres.SelectedIndex = 0;
                                }
                                txtNota.Enabled = false;
                                ddlPareceres.Enabled = false;
                            }
                        }
                    }
                );
            }
            else
            {
                _UCEfetivacaoNotas.TextBotaoCancelar = "Voltar";

                if (HabilitaBoletimAluno)
                {
                    HabilitaAcessoBoletimAluno();
                }
            }
            _UCComboOrdenacao1._Combo.Enabled = true;
        }

        /// <summary>
        /// Habilita o ícone para consulta do boletim no gridview da aluno
        /// </summary>
        /// <returns></returns>
        public void HabilitaAcessoBoletimAluno()
        {
            pnlAlunos.Enabled = true;
            gvAlunos.Enabled = true;
            ((System.Web.UI.WebControls.WebControl)gvAlunos.Controls[0]).Enabled = true;

            foreach (GridViewRow Row in gvAlunos.Rows)
            {
                Row.Enabled = true;
                Row.Cells[colunaBoletim].Enabled = true;
                ImageButton btnBoletim = (ImageButton)Row.FindControl("btnBoletim");
                if (btnBoletim != null)
                {
                    btnBoletim.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Retorna o número de casas decimais de acordo com a variação da escala de avaliação
        /// (só se for do tipo numérica.
        /// </summary>
        /// <returns></returns>
        private int RetornaNumeroCasasDecimais()
        {
            int numeroCasasDecimais = 1;
            if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
            {
                // Calcula a quantidade de casas decimais da variação de notas
                string variacao = Convert.ToDouble(VS_EscalaNumerica.ean_variacao).ToString();
                int notainteira;
                if (Int32.TryParse(variacao, out notainteira))
                {
                    numeroCasasDecimais = 0;
                }
                else if (variacao.IndexOf(",") >= 0)
                {
                    numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                }
            }

            return numeroCasasDecimais;
        }

        /// <summary>
        /// Carrega os dados na tela.
        /// </summary>
        public void LoadFromEntity()
        {
            try
            {
                LimpaVariaveisLocais();

                // Limpa o viewState de relatórios.
                _VS_Nota_Relatorio = new List<UCEfetivacaoNotas.NotasRelatorio>();

                VS_JustificativaPosConselho = new List<UCEfetivacaoNotas.Justificativa>();

                TUR_Turma tur = VS_Turma;
                ACA_FormatoAvaliacao fav = VS_FormatoAvaliacao;

                AvaliacaoTipo tipo = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                if (Tud_id <= 0 && tipoEscala != EscalaAvaliacaoTipo.Relatorios && fav.fav_obrigatorioRelatorioReprovacao)
                {
                    lnObrigatorio.Visible = true;
                    divLegenda.Style.Add(HtmlTextWriterStyle.Width, "450px");
                }
                else
                {
                    lnObrigatorio.Visible = false;
                    divLegenda.Style.Add(HtmlTextWriterStyle.Width, "260px");
                }

                // Exibe o botão imprimir e importar anotações do aluno no cadastro de relatorios
                // apenas quando não for avaliação do tipo relatório.
                if (tipoEscala == EscalaAvaliacaoTipo.Relatorios)
                {
                    _UCEfetivacaoNotas.VisibleImprimirEImportarAnotacoesRelatorio = false;
                }
                else
                {
                    _UCEfetivacaoNotas.VisibleImprimirEImportarAnotacoesRelatorio = true;
                }

                switch (fav.fav_tipoApuracaoFrequencia)
                {
                    case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula:
                        lblQtdeAulasCaption.Text = "Qtde. tempos de aula:";
                        break;

                    case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia:
                        lblQtdeAulasCaption.Text = "Qtde. dias de aulas:";
                        break;
                }

                // Opção de "Sem professor":
                // Somente nas avaliações periódicas;
                // Se for só conceito global - exibe.
                // Se for conceito global + disciplina, ou só disciplina - exibe na disciplina.
                chkSemProfessor.Visible = divchkSemProfessor.Visible = (tipo == AvaliacaoTipo.Periodica) &&
                                           (fav.fav_tipo == (byte)ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
                                            Tud_id > 0) && VS_Avaliacao.ava_exibeSemProfessor;

                // Oção de "Não avaliados":
                // Somente nas avaliações periódicas e no conceito global;
                // Se for conceito global + disciplina, ou só conceito global e marcado para exibir no formato - exibe.
                chkNaoAvaliado.Visible = divchkNaoAvaliado.Visible = tipo == AvaliacaoTipo.Periodica &&
                                         fav.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.Disciplina &&
                                         VS_Avaliacao.ava_exibeNaoAvaliados &&
                                         Tud_id <= 0;

                lblQtdeAulasDadas.Visible = divlblQtdeAulas.Visible = fav.fav_tipoLancamentoFrequencia == (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPrevistasDocente;

                //Exibe o campo total de aulas apenas quando o tipo da disciplina é "Experiência"
                lblTotalAulasExperiencia.Visible = lblQtdeAulasDadas.Visible && (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo == TurmaDisciplinaTipo.Experiencia;

                VerificaRegrasCurso();
                // Carregar grid.
                CarregarGridAlunos();
                lnAlunoNaoAvaliado.Visible = VS_CursoSeriadoAvaliacoes;
                lnAlunoFrequencia.Visible = ExibeCompensacaoAusencia;
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a efetivação.", UtilBO.TipoMensagem.Erro);
                EscondeTelaAlunos(String.Empty);
            }
        }

        /// <summary>
        /// Verifica se as regras do curso estão sendo cumpridas.
        /// Quando o regime de matrícula é Seriado por avaliações, o formato tem que
        /// ser do tipo Conceito Global e a avaliação selecionada tem que ser do tipo
        /// Periódica ou Periódica + Final.
        /// </summary>
        private void VerificaRegrasCurso()
        {
            ACA_CurriculoPeriodo entCurPeriodo;
            bool Seriado;

            if (CLS_AlunoAvaliacaoTurmaBO.ValidaRegrasCurso(VS_Turma, VS_FormatoAvaliacao, _VS_ava_id, out entCurPeriodo, out Seriado) && Seriado)
            {
                gvAlunos.Columns[colunaAvaliacaoEja].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodoAvaliacao(entCurPeriodo.crp_nomeAvaliacao);
            }

            VS_CursoSeriadoAvaliacoes = Seriado && entCurPeriodo.crp_turmaAvaliacao;
        }

        /// <summary>
        /// Salva os dados no banco.
        /// </summary>
        public void Salvar()
        {
            List<sDisciplinasDivergentesPorAluno> listDisciplinasDivergentesPorAluno = new List<sDisciplinasDivergentesPorAluno>();
            List<long> listAlunosComDivergenciaEmDisciplina = new List<long>();
            List<long> listAlunosComDivergencia = new List<long>();
            List<string> listDisciplinasNaoLancadas = new List<string>();

            try
            {
                if (periodoFechado)
                    throw new ValidationException(String.Format("{0} disponível apenas para consulta.", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

                string msgValidacao = string.Empty;
                bool permaneceTela = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina;
                List<CLS_AvaliacaoTurma_Cadastro> listaTurma;
                ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia;

                ValidaGeraDados(out listaDisciplina, out listaTurma, out tipoFrequencia);

                if (Tud_id > 0)
                {
                    List<NotaFinalAlunoTurmaDisciplina> listaNotaFinalAlunoTurmaDisciplina = retornaListaNotaFinalAlunoTurmaDisciplina();
                    CLS_AlunoAvaliacaoTurmaDisciplinaBO.Save(
                        VS_Turma,
                        Tud_id,
                        VS_FormatoAvaliacao,
                        listaDisciplina,
                        ApplicationWEB.TamanhoMaximoArquivo,
                        ApplicationWEB.TiposArquivosPermitidos,
                        tipoFrequencia,
                        (ACA_FormatoAvaliacaoCalculoQtdeAulasDadas)VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                        out msgValidacao,
                        VS_Avaliacao.tpc_id,
                        (AvaliacaoTipo)VS_Avaliacao.ava_tipo,
                        out listAlunosComDivergenciaEmDisciplina,
                        VS_EscalaAvaliacao,
                        listaNotaFinalAlunoTurmaDisciplina,
                        _UCEfetivacaoNotas.VS_EfetivacaoSemestral,
                        _VS_ava_id,
                        VS_Avaliacao.ava_exibeNotaPosConselho,
                        (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo,
                        gvAlunos.Columns[colunaNotaRegencia].Visible,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }
                else
                {
                    List<NotaFinalAlunoTurma> listaNotaFinalAlunoTurma = retornaListaNotaFinalAlunoTurma();
                    CLS_AlunoAvaliacaoTurmaBO.Save(
                        listaTurma,
                        resultadoFinal,
                        ApplicationWEB.TamanhoMaximoArquivo,
                        ApplicationWEB.TiposArquivosPermitidos,
                        VS_Turma,
                        VS_FormatoAvaliacao,
                        (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                        (ACA_FormatoAvaliacaoCalculoQtdeAulasDadas)VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                        out msgValidacao,
                        VS_Avaliacao.tpc_id,
                        (AvaliacaoTipo)VS_Avaliacao.ava_tipo,
                        true,
                        out listDisciplinasDivergentesPorAluno,
                        out listDisciplinasNaoLancadas,
                        out listAlunosComDivergencia,
                        VS_EscalaAvaliacao,
                        listaNotaFinalAlunoTurma
                        , _UCEfetivacaoNotas.VS_turma_Peja, _VS_ava_id, VS_Avaliacao.ava_exibeNotaPosConselho
                        , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, _UCEfetivacaoNotas.VS_MensagemLogEfetivacao + "tur_id: " + _VS_tur_id + "; tud_id: " + Tud_id + "; ava_id: " + _VS_ava_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }

                // Verifica se tem mensagem de validação (que não impede a gravação dos dados).
                UtilBO.TipoMensagem tipoMsg = UtilBO.TipoMensagem.Sucesso;
                string msg;

                if (listDisciplinasDivergentesPorAluno.Count > 0)
                {
                    msg = "Efetivação salva com sucesso apenas para os alunos sem divergência.";
                    if (listDisciplinasDivergentesPorAluno.Count > 1)
                    {
                        msgValidacao = "Efetivação não salva para os alunos listados abaixo em vermelho," +
                                    "\npois existem informações divergentes no lançamento de frequência mensal e do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " dos mesmos. ";
                    }
                    else
                    {
                        msgValidacao = "Efetivação não salva para o aluno listado abaixo em vermelho," +
                                    "\npois existem informações divergentes no lançamento de frequência mensal e do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " do mesmo. ";
                    }
                }
                else
                {
                    msg = "Efetivação salva com sucesso.";
                }

                if (listAlunosComDivergencia.Count > 0)
                {
                    msg = "Efetivação salva com sucesso apenas para os alunos sem divergência.";

                    if (listAlunosComDivergencia.Count > 1)
                    {
                        msgValidacao = "Efetivação não salva para os alunos listados abaixo em vermelho," +
                                    "\npois existem divergências entre a frequência global e a frequência apurada no lançamento mensal. ";
                    }
                    else
                    {
                        msgValidacao = "Efetivação não salva para o aluno listado abaixo em vermelho," +
                                    "\npois existem divergências entre a frequência global e a frequência apurada no lançamento mensal. ";
                    }
                }
                else
                {
                    msg = "Efetivação salva com sucesso.";
                }

                _lblMessage.Text = UtilBO.GetErroMessage(msg, tipoMsg);

                if (!string.IsNullOrEmpty(msgValidacao))
                {
                    tipoMsg = UtilBO.TipoMensagem.Alerta;
                }

                if (permaneceTela)
                {
                    if (listAlunosComDivergenciaEmDisciplina.Count == 0)
                    {
                        LoadFromEntity();
                    }

                    if (listDisciplinasDivergentesPorAluno.Count > 0)
                    {
                        CarregaGridAlunosComDisciplinasDivergentes(listDisciplinasDivergentesPorAluno);
                    }

                    if (listAlunosComDivergencia.Count > 0)
                    {
                        CarregaGridAlunosDivergentes(listAlunosComDivergencia);
                    }

                    if (listAlunosComDivergenciaEmDisciplina.Count > 0)
                    {
                        CarregaGridAlunosDivergentes(listAlunosComDivergenciaEmDisciplina);
                    }

                    if (tipoMsg == UtilBO.TipoMensagem.Alerta)
                    {
                        _lblMessage2.Text = UtilBO.GetErroMessage(TrocaParametroMensagem(msgValidacao), tipoMsg);
                    }
                }
                else
                {
                    RedirecionaBusca(UtilBO.GetErroMessage(TrocaParametroMensagem(msg + msgValidacao), tipoMsg));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (ValidationException ex)
            {
                if (listDisciplinasNaoLancadas.Count > 0)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(String.Format("O " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        " não poderá ser fechado pois não foi lançada a frequência mensal ou o " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        (listDisciplinasNaoLancadas.Count == 1 ? " não foi fechado para o(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " a seguir: "
                            : " não foi fechado para os(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " a seguir: ") +
                               string.Join(",", listDisciplinasNaoLancadas.ToArray()) + "."), UtilBO.TipoMensagem.Alerta);
                }
                else if (listDisciplinasDivergentesPorAluno.Count > 0)
                {
                    CarregaGridAlunosComDisciplinasDivergentes(listDisciplinasDivergentesPorAluno);
                    _lblMessage.Text = UtilBO.GetErroMessage(String.Format("O " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        " não poderá ser fechado pois as informações estão divergentes no lançamento de frequência mensal e do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " de todos os alunos. ")
                        , UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a efetivação.", UtilBO.TipoMensagem.Erro);
            }
        }

        private List<NotaFinalAlunoTurma> retornaListaNotaFinalAlunoTurma()
        {
            List<NotaFinalAlunoTurma> listaNotaFinalAluno = new List<NotaFinalAlunoTurma>();
            AvaliacaoTipo tipo = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
            if (tipo == AvaliacaoTipo.RecuperacaoFinal)
            {
                // Escala do conceito global.
                int esa_id = VS_EscalaAvaliacao.esa_id;

                ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoLancamento = (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

                // Valor do conceito global ou por disciplina.
                string valorMinimo = Tud_id > 0 ?
                    VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina :
                    VS_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                if (tipoEscala == EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = double.Parse(valorMinimo.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                DataTable dtAvaliacaoFinal = ACA_AvaliacaoBO.SelectAvaliacaoFinal_PorFormato(_VS_fav_id);
                if (dtAvaliacaoFinal.Rows.Count > 0)
                {
                    int ava_idFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["ava_id"].ToString());
                    int tpc_idFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["tpc_id"].ToString());
                    int ava_tipoFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["ava_tipo"].ToString());

                    // Busca os alunos pela turma
                    listaNotaFinalAluno = MTR_MatriculaTurmaBO.GetSelect_NotaFinalAluno_By_Turma_Periodo(
                        _VS_tur_id,
                        tpc_idFinal,
                        ava_idFinal,
                        Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                        _VS_fav_id,
                        (byte)ava_tipoFinal,
                        esa_id,
                        (byte)tipoEscala,
                        string.Empty,
                        notaMinimaAprovacao,
                        ordemParecerMinimo,
                        (byte)tipoLancamento,
                        VS_EscalaAvaliacaoAdicional.esa_tipo,
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                        false);
                }
            }

            return listaNotaFinalAluno;
        }

        private List<NotaFinalAlunoTurmaDisciplina> retornaListaNotaFinalAlunoTurmaDisciplina()
        {
            List<NotaFinalAlunoTurmaDisciplina> listaNotaFinalAlunoDisciplina = new List<NotaFinalAlunoTurmaDisciplina>();
            AvaliacaoTipo tipo = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
            if (tipo == AvaliacaoTipo.RecuperacaoFinal)
            {
                // Escala do conceito global.
                int esa_id = VS_EscalaAvaliacao.esa_id;

                ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoLancamento =
                    (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

                // Valor do conceito global ou por disciplina.
                string valorMinimo = Tud_id > 0
                                         ? VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina
                                         : VS_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                if (tipoEscala == EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = double.Parse(valorMinimo.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                DataTable dtAvaliacaoFinal = ACA_AvaliacaoBO.SelectAvaliacaoFinal_PorFormato(_VS_fav_id);
                if (dtAvaliacaoFinal.Rows.Count > 0)
                {
                    int ava_idFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["ava_id"].ToString());
                    int tpc_idFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["tpc_id"].ToString());
                    int ava_tipoFinal = Convert.ToInt32(dtAvaliacaoFinal.Rows[0]["ava_tipo"].ToString());

                    listaNotaFinalAlunoDisciplina = VS_DisciplinaEspecial && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                        MTR_MatriculaTurmaDisciplinaBO.GetSelect_NotaFinalAluno_By_Turma_Disciplina_PeriodoFiltroDeficiencia
                        (
                            Tud_id,
                            _VS_tur_id,
                            tpc_idFinal,
                            ava_idFinal,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            (byte)ava_tipoFinal,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            string.Empty,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            , VS_Turma.tur_tipo
                            , VS_Turma.cal_id
                            , _UCEfetivacaoNotas.VS_tipoDocente
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , false
                            , VS_listaTur_ids
                        ) :
                        MTR_MatriculaTurmaDisciplinaBO.GetSelect_NotaFinalAluno_By_Turma_Disciplina_Periodo
                        (
                            Tud_id,
                            _VS_tur_id,
                            tpc_idFinal,
                            ava_idFinal,
                            Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                            _VS_fav_id,
                            (byte)ava_tipoFinal,
                            esa_id,
                            (byte)tipoEscala,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            string.Empty,
                            notaMinimaAprovacao,
                            ordemParecerMinimo,
                            (byte)tipoLancamento,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            , VS_Turma.tur_tipo
                            , VS_Turma.cal_id
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , EntTurmaDisciplina.tud_tipo
                            , VS_Avaliacao.tpc_ordem
                            , VS_FormatoAvaliacao.fav_variacao
                            , false
                            , VS_listaTur_ids
                        );
                }
            }

            return listaNotaFinalAlunoDisciplina;
        }

        /// <summary>
        /// Faz a validação dos dados na tela e gera as listas necessárias para salvar.
        /// </summary>
        /// <param name="listaDisciplina">Lista de disciplinas para usar para salvar</param>
        /// <param name="listaTurma">Lista de turmas para usar para salvar</param>
        /// <param name="tipoFrequencia">Tipo de lançamento de frequência</param>
        private void ValidaGeraDados(out List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina, out List<CLS_AvaliacaoTurma_Cadastro> listaTurma, out ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia)
        {
            listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();
            listaTurma = new List<CLS_AvaliacaoTurma_Cadastro>();

            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinaLocal = new List<CLS_AvaliacaoTurDisc_Cadastro>();
            List<CLS_AvaliacaoTurma_Cadastro> listaTurmaLocal = new List<CLS_AvaliacaoTurma_Cadastro>();

            List<string> alunosErroIntervalo = new List<string>(), alunosErroIntervaloNotaAdicional = new List<string>(),
                         alunosErroConversao = new List<string>(), alunosErroConversaoNotaAdicional = new List<string>(),
                         alunosErroIntervaloNotaPosConselho = new List<string>(), alunosErroConversaoNotaPosConselho = new List<string>();
            string stringErro = string.Empty, stringErroAulas = string.Empty, stringErroFaltas = string.Empty,
                   stringErroAusencias = string.Empty;

            // Variável utilizada na efetivação de recuperação
            AvaliacaoTipo tipo = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
            tipoFrequencia = (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

            // Se a escala de avaliação é numérica.
            bool tipoEscalaNumerica = false;
            if ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica)
            {
                // Traz os valores limite para a validação da nota.
                tipoEscalaNumerica = true;
            }

            // Se o formato de avaliação possui conceito adicional.
            bool notaAdicionalTipoEscalaNumerica = false;
            ACA_EscalaAvaliacaoNumerica notaAdicionalEscalaNumerica = new ACA_EscalaAvaliacaoNumerica();
            if (VS_FormatoAvaliacao.fav_conceitoGlobalAdicional)
            {
                // Se a escala de avaliação é numérica.
                if ((EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo == EscalaAvaliacaoTipo.Numerica)
                {
                    // Traz os valores limite para a validação da nota.
                    notaAdicionalTipoEscalaNumerica = true;
                    notaAdicionalEscalaNumerica = VS_EscalaNumericaAdicional;
                }
            }

            bool existeAlunoComNotaPosConselho = false;
            bool existeAlunoSemNotaPosConselho = false;

            object lockObject = new object();

            //foreach (GridViewRow row in gvAlunos.Rows)
            Parallel.ForEach
            (
                gvAlunos.Rows.Cast<GridViewRow>()
                ,
                row =>
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string aulas = ((TextBox)row.FindControl("txtQtdeAula")).Text;
                        string faltas = ((TextBox)row.FindControl("txtQtdeFalta")).Text;
                        string ausencias = ((TextBox)row.FindControl("txtAusenciasCompensadas")).Text;
                        int aula, falta, ausencia;

                        if (int.TryParse(aulas, out aula) && (aula > 99999))
                        {
                            stringErroAulas = "A quantidade de aulas deve ser menor que 99999.";
                        }

                        if (int.TryParse(faltas, out falta) && (falta > 99999))
                        {
                            stringErroFaltas = "A quantidade de faltas deve ser menor que 99999.";
                        }

                        if (int.TryParse(ausencias, out ausencia) && (ausencia > 99999))
                        {
                            stringErroAusencias = "A quantidade de ausências compensadas deve ser menor que 99999.";
                        }
                    }

                    if (tipoEscalaNumerica)
                    {
                        if (!gvAlunos.Columns[colunaNotaRegencia].Visible)
                        {
                            if (gvAlunos.Columns[colunaNota].Visible)
                            {
                                // Recupera o valor da avaliação normal.
                                TextBox txtNota = (TextBox)row.FindControl("txtNota");
                                if ((txtNota != null) && !string.IsNullOrEmpty(txtNota.Text))
                                {
                                    decimal nota;
                                    if (decimal.TryParse(txtNota.Text, out nota))
                                    {
                                        // Valida se os valores da nota estão dentro dos limites da escala.
                                        if (tipo != AvaliacaoTipo.Recuperacao)
                                        {
                                            if ((nota < VS_EscalaNumerica.ean_menorValor) || (nota > VS_EscalaNumerica.ean_maiorValor))
                                            {
                                                lock (lockObject)
                                                {
                                                    alunosErroIntervalo.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Se é recuperação, possui apenas limite inferior.
                                            if (nota < VS_EscalaNumerica.ean_menorValor)
                                            {
                                                lock (lockObject)
                                                {
                                                    alunosErroIntervalo.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lock (lockObject)
                                        {
                                            alunosErroConversaoNotaAdicional.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                        }
                                    }
                                }
                            }

                            if (gvAlunos.Columns[colunaNotaPosConselho].Visible)
                            {
                                TextBox txtNotaPosConselho = (TextBox)row.FindControl("txtNotaPosConselho");
                                if ((txtNotaPosConselho != null) && !string.IsNullOrEmpty(txtNotaPosConselho.Text))
                                {
                                    existeAlunoComNotaPosConselho = true;
                                    decimal notaPosConselho;
                                    if (decimal.TryParse(txtNotaPosConselho.Text, out notaPosConselho))
                                    {
                                        // Valida se os valores da nota estão dentro dos limites da escala.
                                        if (tipo != AvaliacaoTipo.Recuperacao)
                                        {
                                            if ((notaPosConselho < VS_EscalaNumerica.ean_menorValor) || (notaPosConselho > VS_EscalaNumerica.ean_maiorValor))
                                            {
                                                lock (lockObject)
                                                {
                                                    alunosErroIntervaloNotaPosConselho.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Se é recuperação, possui apenas limite inferior.
                                            if (notaPosConselho < VS_EscalaNumerica.ean_menorValor)
                                            {
                                                lock (lockObject)
                                                {
                                                    alunosErroIntervaloNotaPosConselho.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lock (lockObject)
                                        {
                                            alunosErroConversaoNotaPosConselho.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                        }
                                    }
                                }
                                else
                                {
                                    existeAlunoSemNotaPosConselho = true;
                                }
                            }
                        }
                        else
                        {
                            Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");
                            // Guarda se possui algum erro de validacao na nota de algum dos componentes da regencia
                            bool erroIntervaloNota = false;
                            bool erroConversaoNota = false;
                            bool erroIntervaloNotaPosConselho = false;
                            bool erroConversaoNotaPosConselho = false;
                            //
                            //foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                            Parallel.ForEach
                            (
                                rptComponenteRegencia.Items.Cast<RepeaterItem>()
                                ,
                                rptItem =>
                                {
                                    // Recupera o valor da avaliação normal.
                                    TextBox txtNota = (TextBox)rptItem.FindControl("txtNota");
                                    if ((txtNota != null) && !string.IsNullOrEmpty(txtNota.Text))
                                    {
                                        decimal nota;
                                        if (decimal.TryParse(txtNota.Text, out nota))
                                        {
                                            // Valida se os valores da nota estão dentro dos limites da escala.
                                            if (tipo != AvaliacaoTipo.Recuperacao)
                                            {
                                                if ((nota < VS_EscalaNumerica.ean_menorValor) || (nota > VS_EscalaNumerica.ean_maiorValor))
                                                {
                                                    lock (lockObject)
                                                    {
                                                        erroIntervaloNota |= true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Se é recuperação, possui apenas limite inferior.
                                                if (nota < VS_EscalaNumerica.ean_menorValor)
                                                {
                                                    lock (lockObject)
                                                    {
                                                        erroIntervaloNota |= true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lock (lockObject)
                                            {
                                                erroConversaoNota |= true;
                                            }
                                        }
                                    }

                                    HtmlTableRow trComponente = (HtmlTableRow)rptItem.FindControl("tr1");
                                    if (trComponente.Cells[colunaComponenteRegenciaNotaPosConselho].Visible)
                                    {
                                        TextBox txtNotaPosConselho = (TextBox)rptItem.FindControl("txtNotaPosConselho");
                                        if ((txtNotaPosConselho != null) && !string.IsNullOrEmpty(txtNotaPosConselho.Text))
                                        {
                                            existeAlunoComNotaPosConselho = true;
                                            decimal notaPosConselho;
                                            if (decimal.TryParse(txtNotaPosConselho.Text, out notaPosConselho))
                                            {
                                                // Valida se os valores da nota estão dentro dos limites da escala.
                                                if (tipo != AvaliacaoTipo.Recuperacao)
                                                {
                                                    if ((notaPosConselho < VS_EscalaNumerica.ean_menorValor) || (notaPosConselho > VS_EscalaNumerica.ean_maiorValor))
                                                    {
                                                        lock (lockObject)
                                                        {
                                                            erroIntervaloNotaPosConselho |= true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // Se é recuperação, possui apenas limite inferior.
                                                    if (notaPosConselho < VS_EscalaNumerica.ean_menorValor)
                                                    {
                                                        lock (lockObject)
                                                        {
                                                            erroIntervaloNotaPosConselho |= true;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lock (lockObject)
                                                {
                                                    erroConversaoNotaPosConselho |= true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lock (lockObject)
                                            {
                                                existeAlunoSemNotaPosConselho = true;
                                            }
                                        }
                                    }
                                }
                            );

                            if (erroIntervaloNota)
                            {
                                lock (lockObject)
                                {
                                    alunosErroIntervalo.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                }
                            }
                            if (erroConversaoNota)
                            {
                                lock (lockObject)
                                {
                                    alunosErroConversaoNotaAdicional.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                }
                            }
                            if (erroIntervaloNotaPosConselho)
                            {
                                lock (lockObject)
                                {
                                    alunosErroIntervaloNotaPosConselho.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                }
                            }
                            if (erroConversaoNotaPosConselho)
                            {
                                lock (lockObject)
                                {
                                    alunosErroConversaoNotaPosConselho.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                }
                            }
                        }
                    }
                    else if ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Pareceres)
                    {
                        if (!gvAlunos.Columns[colunaNotaRegencia].Visible)
                        {
                            if (gvAlunos.Columns[colunaNotaPosConselho].Visible)
                            {
                                DropDownList ddlPareceresPosConselho = (DropDownList)row.FindControl("ddlPareceresPosConselho");
                                if (ddlPareceresPosConselho != null && ddlPareceresPosConselho.SelectedIndex > 0)
                                {
                                    existeAlunoComNotaPosConselho = true;
                                }
                                else
                                {
                                    existeAlunoSemNotaPosConselho = true;
                                }
                            }
                        }
                        else
                        {
                            Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");
                            //foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                            Parallel.ForEach
                            (
                                rptComponenteRegencia.Items.Cast<RepeaterItem>()
                                ,
                                rptItem =>
                                {
                                    HtmlTableRow trComponente = (HtmlTableRow)rptItem.FindControl("tr1");
                                    if (trComponente.Cells[colunaComponenteRegenciaNotaPosConselho].Visible)
                                    {
                                        DropDownList ddlPareceresPosConselho = (DropDownList)rptItem.FindControl("ddlPareceresPosConselho");
                                        if (ddlPareceresPosConselho != null && ddlPareceresPosConselho.SelectedIndex > 0)
                                        {
                                            existeAlunoComNotaPosConselho = true;
                                        }
                                        else
                                        {
                                            existeAlunoSemNotaPosConselho = true;
                                        }
                                    }
                                }
                            );
                        }
                    }

                    if (notaAdicionalTipoEscalaNumerica)
                    {
                        // Recupera o valor da avaliação adicional.
                        TextBox txtNotaAdicional = (TextBox)row.FindControl("txtNotaAdicional");
                        if ((txtNotaAdicional != null) && !string.IsNullOrEmpty(txtNotaAdicional.Text))
                        {
                            decimal nota;
                            if (decimal.TryParse(txtNotaAdicional.Text, out nota))
                            {
                                // Valida se os valores da nota estão dentro dos limites da escala.
                                if (tipo != AvaliacaoTipo.Recuperacao)
                                {
                                    if ((nota < notaAdicionalEscalaNumerica.ean_menorValor) || (nota > notaAdicionalEscalaNumerica.ean_maiorValor))
                                    {
                                        lock (lockObject)
                                        {
                                            alunosErroIntervaloNotaAdicional.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                        }
                                    }
                                }
                                else
                                {
                                    // Se é recuperação, possui apenas limite inferior.
                                    if (nota < notaAdicionalEscalaNumerica.ean_menorValor)
                                    {
                                        lock (lockObject)
                                        {
                                            alunosErroIntervaloNotaAdicional.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lock (lockObject)
                                {
                                    alunosErroConversao.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                }
                            }
                        }
                    }

                    if (Tud_id > 0)
                    {
                        lock (lockObject)
                        {
                            listaDisciplinaLocal.AddRange(AdicionaLinhaDisciplina(row));
                        }
                    }
                    else
                    {
                        lock (lockObject)
                        {
                            listaTurmaLocal.AddRange(AdicionaLinhaTurma(row));
                        }
                    }

                    //if (Tud_id > 0)
                    //{
                    //    AdicionaLinhaDisciplina(row, ref listaDisciplina);
                    //}
                    //else
                    //{
                    //    AdicionaLinhaTurma(row, ref listaTurma);
                    //}
                }
            );

            listaDisciplina = listaDisciplinaLocal;
            listaTurma = listaTurmaLocal;

            int numeroCasasDecimais = RetornaNumeroCasasDecimais();

            if (tipo != AvaliacaoTipo.Recuperacao)
            {
                if (alunosErroIntervalo.Count == 1)
                {
                    stringErro += string.Format(
                                    "Nota do aluno {0} está fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervalo.ToArray()),
                                    Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(VS_EscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }
                else if (alunosErroIntervalo.Count > 1)
                {
                    stringErro += string.Format(
                                    "Nota dos alunos {0} estão fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervalo.ToArray()),
                                    Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(VS_EscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }

                if (alunosErroIntervaloNotaAdicional.Count == 1)
                {
                    stringErro += string.Format(
                                    "Nota adicional do aluno {0} está fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervaloNotaAdicional.ToArray()),
                                    Math.Round(notaAdicionalEscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(notaAdicionalEscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }
                else if (alunosErroIntervaloNotaAdicional.Count > 1)
                {
                    stringErro += string.Format(
                                    "Nota adicional dos alunos {0} estão fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervaloNotaAdicional.ToArray()),
                                    Math.Round(notaAdicionalEscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(notaAdicionalEscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }

                if (alunosErroIntervaloNotaPosConselho.Count == 1)
                {
                    stringErro += string.Format(
                                    "Nota pós-conselho do aluno {0} está fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervaloNotaPosConselho.ToArray()),
                                    Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(VS_EscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }
                else if (alunosErroIntervaloNotaPosConselho.Count > 1)
                {
                    stringErro += string.Format(
                                    "Nota pós-conselho dos alunos {0} estão fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                    string.Join(", ", alunosErroIntervaloNotaPosConselho.ToArray()),
                                    Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais),
                                    Math.Round(VS_EscalaNumerica.ean_maiorValor, numeroCasasDecimais));
                }
            }
            else
            {
                if (tipoEscalaNumerica)
                {
                    if (alunosErroIntervalo.Count == 1)
                    {
                        stringErro += string.Format(
                                        "Nota do aluno {0} está fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervalo.ToArray()),
                                        Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }
                    else if (alunosErroIntervalo.Count > 1)
                    {
                        stringErro += string.Format(
                                        "Nota dos alunos {0} estão fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervalo.ToArray()),
                                        Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }

                    if (alunosErroIntervaloNotaPosConselho.Count == 1)
                    {
                        stringErro += string.Format(
                                        "Nota pós-conselho do aluno {0} está fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervaloNotaPosConselho.ToArray()),
                                        Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }
                    else if (alunosErroIntervaloNotaPosConselho.Count > 1)
                    {
                        stringErro += string.Format(
                                        "Nota pós-conselho dos alunos {0} estão fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervaloNotaPosConselho.ToArray()),
                                        Math.Round(VS_EscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }
                }

                if (notaAdicionalTipoEscalaNumerica)
                {
                    if (alunosErroIntervaloNotaAdicional.Count == 1)
                    {
                        stringErro += string.Format(
                                        "Nota do aluno {0} está fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervaloNotaAdicional.ToArray()),
                                        Math.Round(notaAdicionalEscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }
                    else if (alunosErroIntervaloNotaAdicional.Count > 1)
                    {
                        stringErro += string.Format(
                                        "Nota dos alunos {0} estão fora do intervalo estipulado para recuperação (nota mínima de {1}).<br />",
                                        string.Join(", ", alunosErroIntervaloNotaAdicional.ToArray()),
                                        Math.Round(notaAdicionalEscalaNumerica.ean_menorValor, numeroCasasDecimais));
                    }
                }
            }

            if (alunosErroConversao.Count == 1)
            {
                stringErro += string.Format("Nota para o aluno {0} é inválida.", string.Join(", ", alunosErroConversao.ToArray()));
            }
            else if (alunosErroConversao.Count > 1)
            {
                stringErro += string.Format("Nota para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversao.ToArray()));
            }

            if (alunosErroConversaoNotaAdicional.Count == 1)
            {
                stringErro += string.Format("Nota adicional para o aluno {0} é inválida.", string.Join(", ", alunosErroConversaoNotaAdicional.ToArray()));
            }
            else if (alunosErroConversaoNotaAdicional.Count > 1)
            {
                stringErro += string.Format("Nota adicional para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversaoNotaAdicional.ToArray()));
            }

            if (alunosErroConversaoNotaPosConselho.Count == 1)
            {
                stringErro += string.Format("Nota pós-conselho para o aluno {0} é inválida.", string.Join(", ", alunosErroConversaoNotaPosConselho.ToArray()));
            }
            else if (alunosErroConversaoNotaPosConselho.Count > 1)
            {
                stringErro += string.Format("Nota pós-conselho para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversaoNotaPosConselho.ToArray()));
            }

            if (!string.IsNullOrEmpty(stringErroAulas))
            {
                stringErro += !string.IsNullOrEmpty(stringErro) ? "<br />" + stringErroAulas : stringErroAulas;
            }

            if (!string.IsNullOrEmpty(stringErroFaltas))
            {
                stringErro += !string.IsNullOrEmpty(stringErro) ? "<br />" + stringErroFaltas : stringErroFaltas;
            }

            if (!string.IsNullOrEmpty(stringErroAusencias))
            {
                stringErro += !string.IsNullOrEmpty(stringErro) ? "<br />" + stringErroAusencias : stringErroAusencias;
            }

            if (!string.IsNullOrEmpty(stringErro))
            {
                throw new ValidationException(stringErro);
            }

            // Mostrar mensagem de alerta se houver alunos sem preenchimento de notas pós-conselho,
            // e se tiver pelo menos um aluno com nota preenchida
            if (existeAlunoSemNotaPosConselho && existeAlunoComNotaPosConselho &&
                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_MENSAGEM_SEM_LANCAMENTO_POS_CONSELHO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                _lblMessage3.Text = UtilBO.GetErroMessage("Existem alunos sem preenchimento de " + gvAlunos.Columns[colunaNotaPosConselho].HeaderText.ToLower() + ".", UtilBO.TipoMensagem.Alerta);
            }
        }

        /// <summary>
        /// Adiciona uma linha na lista com os dados da linha do grid.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        /// <param name="listaTurma"></param>
        private List<CLS_AvaliacaoTurma_Cadastro> AdicionaLinhaTurma(GridViewRow row)
        {
            List<CLS_AvaliacaoTurma_Cadastro> listaTurma = new List<CLS_AvaliacaoTurma_Cadastro>();

            long tur_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["tur_id"]);
            long tud_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["tud_id"]);
            long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_id"]);
            int mtu_idAnterior = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_idAnterior"]);
            int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtd_id"]);
            bool dispensadisciplina = Convert.ToBoolean(gvAlunos.DataKeys[row.RowIndex].Values["dispensadisciplina"]);
            int aat_id = -1;
            MtrTurmaResultado resultado = 0;

            if (!String.IsNullOrEmpty(gvAlunos.DataKeys[row.RowIndex].Values["AvaliacaoID"].ToString()))
            {
                aat_id = Convert.ToInt32(Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["AvaliacaoID"]));
            }

            CLS_AlunoAvaliacaoTurma ent = new CLS_AlunoAvaliacaoTurma
            {
                tur_id = tur_id,
                alu_id = alu_id,
                mtu_id = mtu_id,
                aat_id = aat_id,
                IsNew = aat_id <= 0
            };

            // [Carla 24/05] Removi GetEntity que existia, pois estava dando timeout na produção, e conferi os
            // dados que são alimentados pelo método são todos os que existem na entidade.
            //if (!ent.IsNew)
            //{
            //    CLS_AlunoAvaliacaoTurmaBO.GetEntity(ent);
            //}
            //else
            //{
            ent.fav_id = _VS_fav_id;
            ent.ava_id = _VS_ava_id;
            ent.aat_situacao = dispensadisciplina ? (byte)CLS_AlunoAvaliacaoTurmaSituacao.Excluido : (byte)CLS_AlunoAvaliacaoTurmaSituacao.Ativo;
            //}

            // Setar o registroExterno para false.
            ent.aat_registroexterno = false;

            #region Dados das aulas / frequências

            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
            ent.aat_numeroFaltas = gvAlunos.Columns[colunaFaltas].Visible ?
                String.IsNullOrEmpty(txtQtdeFalta.Text) ? -1 : Convert.ToInt32(txtQtdeFalta.Text) :
                0;

            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
            ent.aat_numeroAulas = gvAlunos.Columns[colunaAulas].Visible ?
                String.IsNullOrEmpty(txtQtdeAula.Text) ? -1 : Convert.ToInt32(txtQtdeAula.Text) :
                0;

            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
            ent.aat_ausenciasCompensadas = gvAlunos.Columns[colunaAusenciasCompensadas].Visible ?
                String.IsNullOrEmpty(txtAusenciasCompensadas.Text) ? -1 : Convert.ToInt32(txtAusenciasCompensadas.Text) :
                0;

            TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
            TextBox txtFrequenciaFinal = (TextBox)row.FindControl("txtFrequenciaFinal");

            if (gvAlunos.Columns[colunaFrequenciaFinal].Visible)
            {
                ent.aat_frequencia = String.IsNullOrEmpty(txtFrequenciaFinal.Text) ? 0 : Convert.ToDecimal(txtFrequenciaFinal.Text);
            }
            else if ((ent.aat_numeroFaltas > -1) && (ent.aat_numeroAulas > 0))
            {
                ent.aat_frequencia = gvAlunos.Columns[colunaFrequencia].Visible ?
                    CalculaFrequencia(ent.aat_numeroAulas, ent.aat_numeroFaltas) :
                    0;
            }
            else
            {
                ent.aat_frequencia = gvAlunos.Columns[colunaFrequencia].Visible ?
                   String.IsNullOrEmpty(txtFrequencia.Text) ? -1 : Convert.ToDecimal(txtFrequencia.Text) :
                   0;
            }

            #endregion Dados das aulas / frequências

            if ((chkSemProfessor.Checked && VS_Avaliacao.ava_exibeSemProfessor) || (chkNaoAvaliado.Checked && VS_Avaliacao.ava_exibeNaoAvaliados))
            {
                ent.aat_avaliacao = string.Empty;
                ent.aat_relatorio = string.Empty;
                ent.aat_avaliacaoAdicional = string.Empty;
                ent.aat_avaliacaoPosConselho = string.Empty;
                ent.aat_justificativaPosConselho = string.Empty;
                ent.aat_faltoso = false;
                ent.aat_semProfessor = chkSemProfessor.Checked;
                ent.aat_naoAvaliado = chkNaoAvaliado.Checked;
            }
            else
            {
                bool salvarRelatorio;
                ent.aat_avaliacao = RetornaAvaliacao(row, out salvarRelatorio);
                ent.aat_avaliacaoAdicional = RetornaAvaliacaoAdicional(row);

                ent.aat_avaliacaoPosConselho = VS_Avaliacao.ava_exibeNotaPosConselho ?
                                               RetornaAvaliacaoPosConselho(row) :
                                               string.Empty;

                if (!string.IsNullOrEmpty(ent.aat_avaliacaoPosConselho))
                {
                    if (VS_JustificativaPosConselho.Exists
                                                (p => (p.Id == (tur_id.ToString() + ";"
                                                              + tud_id.ToString() + ";"
                                                              + alu_id.ToString() + ";"
                                                              + mtu_id.ToString() + ";"
                                                              + mtd_id.ToString() + ";"
                                                              + aat_id.ToString()))))
                    {
                        ent.aat_justificativaPosConselho = VS_JustificativaPosConselho.Find
                                                                                        (p => (p.Id == (tur_id.ToString() + ";"
                                                                                                      + tud_id.ToString() + ";"
                                                                                                      + alu_id.ToString() + ";"
                                                                                                      + mtu_id.ToString() + ";"
                                                                                                      + mtd_id.ToString() + ";"
                                                                                                      + aat_id.ToString()))).Valor;
                    }
                }
                else
                {
                    ent.aat_justificativaPosConselho = string.Empty;
                }

                // Buscar valor do checkbox faltoso.
                if (gvAlunos.Columns[colunaFrequencia].Visible)
                {
                    CheckBox chkFaltoso = (CheckBox)row.FindControl("chkFaltoso");
                    if (chkFaltoso != null)
                    {
                        ent.aat_faltoso = chkFaltoso.Checked;
                    }
                }
                else
                {
                    ent.aat_faltoso = false;
                }

                ent.aat_semProfessor = false;
                ent.aat_naoAvaliado = false;

                TextBox txtFrequenciaFinalAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
                if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
                {
                    ent.aat_frequenciaAcumulada = String.IsNullOrEmpty(txtFrequenciaFinalAjustada.Text) ? 0 : Convert.ToDecimal(txtFrequenciaFinalAjustada.Text);
                }

                resultado = RetornaResultado(row, ent);

                if (salvarRelatorio)
                {
                    ent.aat_relatorio = (_VS_Nota_Relatorio.Find
                                          (p => (p.Id == (tur_id.ToString() + ";"
                                                          + tud_id.ToString() + ";"
                                                          + alu_id.ToString() + ";"
                                                          + mtu_id.ToString() + ";"
                                                          + mtd_id.ToString() + ";"
                                                          + aat_id.ToString())))).Valor;

                    string arq_idRelatorio = (_VS_Nota_Relatorio.Find
                                              (p => (p.Id == (tur_id.ToString() + ";"
                                                              + tud_id.ToString() + ";"
                                                              + alu_id.ToString() + ";"
                                                              + mtu_id.ToString() + ";"
                                                              + mtd_id.ToString() + ";"
                                                              + aat_id.ToString())))).arq_idRelatorio;

                    ent.arq_idRelatorio = string.IsNullOrEmpty(arq_idRelatorio) ? 0 : Convert.ToInt64(arq_idRelatorio);
                }
                else
                {
                    ent.aat_relatorio = string.Empty;
                    ent.arq_idRelatorio = 0;
                }
            }

            listaTurma.Add(new CLS_AvaliacaoTurma_Cadastro
            {
                entity = ent,
                resultado = resultado,
                mtu_idAnterior = mtu_idAnterior
            });

            return listaTurma;
        }

        /// <summary>
        /// Retorna o enum de resultado, verificando se é necessário calcular o resultado automaticamente, ou se será
        /// utilizado o valor do combo da tela.
        /// </summary>
        /// <param name="row">Linha do grid</param>
        /// <param name="ent">Entidade da avaliação na turma</param>
        /// <returns></returns>
        private MtrTurmaResultado RetornaResultado(GridViewRow row, CLS_AlunoAvaliacaoTurma ent)
        {
            MtrTurmaResultado resultado = 0;
            if (gvAlunos.Columns[colunaResultado].Visible)
            {
                if (CalcularResultadoAutomatico)
                {
                    // Calcular o resultado de acordo com as regras.
                    ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal criterio =
                        (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)
                        VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal;

                    bool notaValida = false;
                    bool frequenciaValida = true;

                    if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    {
                        TextBox txtNota = (TextBox)row.FindControl("txtNota");
                        double nota;
                        Double.TryParse(ent.aat_avaliacao.Replace(",", "."), out nota);

                        // Se o txt estiver escondido, é porque não vai lançar a nota.
                        notaValida = nota >= VS_NotaMinima || !txtNota.Visible;
                    }
                    else if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    {
                        DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                        int ordem = Convert.ToInt32(ddlPareceres.SelectedValue.Split(';')[1]);

                        // Se o ddl estiver escondido, é porque não vai lançar a nota.
                        notaValida = ordem >= VS_ParecerMinimo || !ddlPareceres.Visible;
                    }

                    TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");
                    if (txtFrequenciaAcumulada != null && gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                    {
                        decimal frequencia;
                        Decimal.TryParse(txtFrequenciaAcumulada.Text, out frequencia);
                        frequenciaValida = frequencia >= VS_FormatoAvaliacao.percentualMinimoFrequencia;
                    }

                    if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia)
                    {
                        if (!notaValida)
                        {
                            resultado = MtrTurmaResultado.Reprovado;
                        }
                        else if (frequenciaValida)
                        {
                            resultado = MtrTurmaResultado.Aprovado;
                        }
                        else
                        {
                            resultado = MtrTurmaResultado.ReprovadoFrequencia;
                        }
                    }
                    else if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ApenasFrequencia)
                    {
                        // Critério = ApenasFrequencia,  Valida apenas a frequência
                        resultado = frequenciaValida ? MtrTurmaResultado.Aprovado : MtrTurmaResultado.ReprovadoFrequencia;
                    }
                    else if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.TodosAprovados)
                    {
                        // Critério = TodosAprovados Sempre aprovado
                        resultado = MtrTurmaResultado.Aprovado;
                    }
                    else
                    {
                        // Critério = 2-ConceitoGlobal ou 3-NotaDisicplina. Só valida a nota.
                        resultado = notaValida ? MtrTurmaResultado.Aprovado : MtrTurmaResultado.Reprovado;
                    }

                    if (_UCEfetivacaoNotas.VS_NomeAvaliacaoRecuperacaoFinal != "" && resultado != MtrTurmaResultado.Aprovado
                        && VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.RecuperacaoFinal)
                    {
                        // Se tem avaliação de recuperação final, e o resultado é "não aprovado", ele vai pra recuperação.
                        resultado = MtrTurmaResultado.RecuperacaoFinal;
                    }
                }
                else
                {
                    DropDownList ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                    byte valor = Convert.ToByte(ddlResultado.SelectedValue == "-1" ? "0" : ddlResultado.SelectedValue);
                    resultado = (MtrTurmaResultado)Convert.ToByte(valor);
                }
            }
            return resultado;
        }

        /// <summary>
        /// Retorna o enum de resultado, verificando se é necessário calcular o resultado automaticamente, ou se será
        /// utilizado o valor do combo da tela.
        /// </summary>
        /// <param name="row">Linha do grid</param>
        /// <param name="ent">Entidade da avaliação na turma</param>
        /// <returns></returns>
        private MtrTurmaDisciplinaResultado RetornaResultadoDisciplina(GridViewRow row, CLS_AlunoAvaliacaoTurmaDisciplina ent)
        {
            MtrTurmaDisciplinaResultado resultado = 0;
            if (gvAlunos.Columns[colunaResultado].Visible)
            {
                if (CalcularResultadoAutomatico)
                {
                    // Calcular o resultado de acordo com as regras.
                    ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal criterio =
                        (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)
                        VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal;

                    bool notaValida = false;
                    bool frequenciaValida = true;

                    if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    {
                        TextBox txtNota = (TextBox)row.FindControl("txtNota");
                        double nota;
                        Double.TryParse(ent.atd_avaliacao.Replace(",", "."), out nota);

                        // Se o txt estiver escondido, é porque não vai lançar a nota.
                        notaValida = nota >= VS_NotaMinima || !txtNota.Visible;
                    }
                    else if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    {
                        DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                        int ordem = Convert.ToInt32(ddlPareceres.SelectedValue.Split(';')[1]);

                        // Se o ddl estiver escondido, é porque não vai lançar a nota.
                        notaValida = ordem >= VS_ParecerMinimo || !ddlPareceres.Visible;
                    }

                    TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");
                    if (txtFrequenciaAcumulada != null && gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                    {
                        decimal frequencia;
                        Decimal.TryParse(txtFrequenciaAcumulada.Text, out frequencia);
                        frequenciaValida = frequencia >= VS_FormatoAvaliacao.percentualMinimoFrequencia;
                    }

                    if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia)
                    {
                        if (!notaValida)
                        {
                            resultado = MtrTurmaDisciplinaResultado.Reprovado;
                        }
                        else if (frequenciaValida)
                        {
                            resultado = MtrTurmaDisciplinaResultado.Aprovado;
                        }
                        else
                        {
                            resultado = MtrTurmaDisciplinaResultado.ReprovadoFrequencia;
                        }
                    }
                    else if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ApenasFrequencia)
                    {
                        // Critério = ApenasFrequencia,  Valida apenas a frequência
                        resultado = frequenciaValida ? MtrTurmaDisciplinaResultado.Aprovado : MtrTurmaDisciplinaResultado.ReprovadoFrequencia;
                    }
                    else if (criterio == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.TodosAprovados)
                    {
                        // Critério = TodosAprovados Sempre aprovado
                        resultado = MtrTurmaDisciplinaResultado.Aprovado;
                    }
                    else
                    {
                        // Critério = 2-ConceitoGlobal ou 3-NotaDisicplina. Só valida a nota.
                        resultado = notaValida ? MtrTurmaDisciplinaResultado.Aprovado : MtrTurmaDisciplinaResultado.Reprovado;
                    }

                    if (_UCEfetivacaoNotas.VS_NomeAvaliacaoRecuperacaoFinal != "" && resultado != MtrTurmaDisciplinaResultado.Aprovado
                        && VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.RecuperacaoFinal)
                    {
                        // Se tem avaliação de recuperação final, e o resultado é "não aprovado", ele vai pra recuperação.
                        resultado = MtrTurmaDisciplinaResultado.RecuperacaoFinal;
                    }
                }
                else
                {
                    DropDownList ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                    byte valor = Convert.ToByte(ddlResultado.SelectedValue == "-1" ? "0" : ddlResultado.SelectedValue);
                    resultado = (MtrTurmaDisciplinaResultado)Convert.ToByte(valor);
                }
            }
            return resultado;
        }

        /// <summary>
        /// Adiciona uma linha na lista com os dados da linha do grid.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        /// <param name="listaDisciplina"></param>
        private List<CLS_AvaliacaoTurDisc_Cadastro> AdicionaLinhaDisciplina(GridViewRow row)
        {
            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();

            long tur_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["tur_id"]);
            long tud_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["tud_id"]);
            long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_id"]);
            int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtd_id"]);
            int mtu_idAnterior = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_idAnterior"]);
            int mtd_idAnterior = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtd_idAnterior"]);
            bool dispensadisciplina = Convert.ToBoolean(gvAlunos.DataKeys[row.RowIndex].Values["dispensadisciplina"]);

            int atd_id = -1;
            MtrTurmaDisciplinaResultado resultado = 0;

            if (!String.IsNullOrEmpty(gvAlunos.DataKeys[row.RowIndex].Values["AvaliacaoID"].ToString()))
            {
                atd_id = Convert.ToInt32(Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["AvaliacaoID"]));
            }

            CLS_AlunoAvaliacaoTurmaDisciplina ent = new CLS_AlunoAvaliacaoTurmaDisciplina
            {
                tud_id = tud_id,
                alu_id = alu_id,
                mtu_id = mtu_id,
                mtd_id = mtd_id,
                atd_id = atd_id,
                IsNew = atd_id <= 0
            };

            // [Carla 24/05] Removi GetEntity que existia, pois estava dando timeout na produção, e conferi os
            // dados que são alimentados pelo método são todos os que existem na entidade.
            //if (!ent.IsNew)
            //{
            //    CLS_AlunoAvaliacaoTurmaDisciplinaBO.GetEntity(ent);
            //}
            //else
            //{
            ent.fav_id = _VS_fav_id;
            ent.ava_id = _VS_ava_id;
            ent.atd_situacao = dispensadisciplina ? (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Excluido : (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;
            //}

            // Setar o registroExterno para false.
            ent.atd_registroexterno = false;

            #region Campos das aulas / frequências

            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
            ent.atd_numeroFaltas = gvAlunos.Columns[colunaFaltas].Visible ?
                (String.IsNullOrEmpty(txtQtdeFalta.Text) ? 0 : Convert.ToInt32(txtQtdeFalta.Text)) :
                0;

            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
            ent.atd_numeroAulas = gvAlunos.Columns[colunaAulas].Visible ?
                (String.IsNullOrEmpty(txtQtdeAula.Text) ? 0 : Convert.ToInt32(txtQtdeAula.Text)) :
                0;

            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
            ent.atd_ausenciasCompensadas = gvAlunos.Columns[colunaAusenciasCompensadas].Visible ?
                (String.IsNullOrEmpty(txtAusenciasCompensadas.Text) ? 0 : Convert.ToInt32(txtAusenciasCompensadas.Text)) :
                0;

            TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
            TextBox txtFrequenciaFinal = (TextBox)row.FindControl("txtFrequenciaFinal");

            if (gvAlunos.Columns[colunaFrequenciaFinal].Visible)
            {
                ent.atd_frequencia = String.IsNullOrEmpty(txtFrequenciaFinal.Text) ? 0 : Convert.ToDecimal(txtFrequenciaFinal.Text);
            }
            else if ((ent.atd_numeroFaltas > -1) && (ent.atd_numeroAulas > 0))
            {
                ent.atd_frequencia = gvAlunos.Columns[colunaFrequencia].Visible ?
                    CalculaFrequencia(ent.atd_numeroAulas, ent.atd_numeroFaltas) :
                    0;
            }
            else
            {
                ent.atd_frequencia = gvAlunos.Columns[colunaFrequencia].Visible ?
                   (String.IsNullOrEmpty(txtFrequencia.Text) ? 0 : Convert.ToDecimal(txtFrequencia.Text)) :
                   0;
            }

            TextBox txtFrequenciaFinalAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
            if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
            {
                ent.atd_frequenciaFinalAjustada = String.IsNullOrEmpty(txtFrequenciaFinalAjustada.Text) ? 0 : Convert.ToDecimal(txtFrequenciaFinalAjustada.Text);
            }

            #endregion Campos das aulas / frequências

            if (chkSemProfessor.Checked && VS_Avaliacao.ava_exibeSemProfessor)
            {
                ent.atd_avaliacao = string.Empty;
                ent.atd_relatorio = string.Empty;
                ent.atd_semProfessor = true;
                ent.atd_avaliacaoPosConselho = string.Empty;
                ent.atd_justificativaPosConselho = string.Empty;
            }
            else
            {
                bool salvarRelatorio;
                ent.atd_avaliacao = RetornaAvaliacao(row, out salvarRelatorio);

                ent.atd_avaliacaoPosConselho = VS_Avaliacao.ava_exibeNotaPosConselho ?
                                               RetornaAvaliacaoPosConselho(row) :
                                               string.Empty;

                if (!string.IsNullOrEmpty(ent.atd_avaliacaoPosConselho))
                {
                    if (VS_JustificativaPosConselho.Exists
                                               (p => (p.Id == (tur_id.ToString() + ";"
                                                             + tud_id.ToString() + ";"
                                                             + alu_id.ToString() + ";"
                                                             + mtu_id.ToString() + ";"
                                                             + mtd_id.ToString() + ";"
                                                             + atd_id.ToString()))))
                    {
                        ent.atd_justificativaPosConselho = VS_JustificativaPosConselho.Find
                                                                                        (p => (p.Id == (tur_id.ToString() + ";"
                                                                                                      + tud_id.ToString() + ";"
                                                                                                      + alu_id.ToString() + ";"
                                                                                                      + mtu_id.ToString() + ";"
                                                                                                      + mtd_id.ToString() + ";"
                                                                                                      + atd_id.ToString()))).Valor;
                    }
                }
                else
                {
                    ent.atd_justificativaPosConselho = string.Empty;
                }

                ent.atd_semProfessor = false;

                resultado = RetornaResultadoDisciplina(row, ent);

                if (salvarRelatorio)
                {
                    ent.atd_relatorio = (_VS_Nota_Relatorio.Find
                                          (p => (p.Id == (tur_id.ToString() + ";"
                                                          + tud_id.ToString() + ";"
                                                          + alu_id.ToString() + ";"
                                                          + mtu_id.ToString() + ";"
                                                          + mtd_id.ToString() + ";"
                                                          + atd_id.ToString())))).Valor;

                    string arq_idRelatorio = (_VS_Nota_Relatorio.Find
                                              (p => (p.Id == (tur_id.ToString() + ";"
                                                              + tud_id.ToString() + ";"
                                                              + alu_id.ToString() + ";"
                                                              + mtu_id.ToString() + ";"
                                                              + mtd_id.ToString() + ";"
                                                              + atd_id.ToString())))).arq_idRelatorio;

                    ent.arq_idRelatorio = string.IsNullOrEmpty(arq_idRelatorio) ? 0 : Convert.ToInt64(arq_idRelatorio);
                }
                else
                {
                    ent.atd_relatorio = string.Empty;
                    ent.arq_idRelatorio = 0;
                }
            }

            listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
            {
                entity = ent,
                resultado = resultado,
                mtu_idAnterior = mtu_idAnterior,
                mtd_idAnterior = mtd_idAnterior
            });

            //
            // Adiciona as disciplinas componentes da regencia
            //
            if (gvAlunos.Columns[colunaNotaRegencia].Visible)
            {
                Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");
                resultado = 0;

                foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                {
                    HiddenField hfDataKeys = (HiddenField)rptItem.FindControl("hfDataKeys");
                    string[] dataKeys = hfDataKeys.Value.Split(';');
                    Int64.TryParse(dataKeys[0], out tud_id);
                    mtd_id = Convert.ToInt32(dataKeys[1]);
                    mtu_idAnterior = Convert.ToInt32(dataKeys[2]);
                    mtd_idAnterior = Convert.ToInt32(dataKeys[3]);
                    dispensadisciplina = Convert.ToBoolean(Convert.ToByte(dataKeys[4]));
                    atd_id = Convert.ToInt32(dataKeys[5]);

                    ImageButton btnRelatorio = (ImageButton)rptItem.FindControl("btnRelatorio");
                    string commandArgument = btnRelatorio.CommandArgument;

                    ent = new CLS_AlunoAvaliacaoTurmaDisciplina
                    {
                        tud_id = tud_id,
                        alu_id = alu_id,
                        mtu_id = mtu_id,
                        mtd_id = mtd_id,
                        atd_id = atd_id,
                        IsNew = atd_id <= 0
                    };

                    ent.fav_id = _VS_fav_id;
                    ent.ava_id = _VS_ava_id;
                    ent.atd_situacao = dispensadisciplina ? (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Excluido : (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                    // Setar o registroExterno para false.
                    ent.atd_registroexterno = false;

                    ent.atd_numeroFaltas = 0;
                    ent.atd_numeroAulas = 0;
                    ent.atd_ausenciasCompensadas = 0;
                    ent.atd_frequencia = 0;

                    if (chkSemProfessor.Checked && VS_Avaliacao.ava_exibeSemProfessor)
                    {
                        ent.atd_avaliacao = string.Empty;
                        ent.atd_relatorio = string.Empty;
                        ent.atd_semProfessor = true;
                        ent.atd_avaliacaoPosConselho = string.Empty;
                        ent.atd_justificativaPosConselho = string.Empty;
                    }
                    else
                    {
                        bool salvarRelatorio;
                        ent.atd_avaliacao = RetornaAvaliacao(rptItem, out salvarRelatorio);
                        ent.atd_avaliacaoPosConselho = VS_Avaliacao.ava_exibeNotaPosConselho ? RetornaAvaliacaoPosConselho(rptItem) : string.Empty;
                        if (!string.IsNullOrEmpty(ent.atd_avaliacaoPosConselho))
                        {
                            if (VS_JustificativaPosConselho.Exists(p => (p.Id == commandArgument)))
                            {
                                ent.atd_justificativaPosConselho = VS_JustificativaPosConselho.Find(p => (p.Id == commandArgument)).Valor;
                            }
                        }
                        else
                        {
                            ent.atd_justificativaPosConselho = string.Empty;
                        }

                        ent.atd_semProfessor = false;

                        if (salvarRelatorio)
                        {
                            ent.atd_relatorio = (_VS_Nota_Relatorio.Find(p => (p.Id == commandArgument))).Valor;
                            string arq_idRelatorio = (_VS_Nota_Relatorio.Find(p => (p.Id == commandArgument))).arq_idRelatorio;
                            ent.arq_idRelatorio = string.IsNullOrEmpty(arq_idRelatorio) ? 0 : Convert.ToInt64(arq_idRelatorio);
                        }
                        else
                        {
                            ent.atd_relatorio = string.Empty;
                            ent.arq_idRelatorio = 0;
                        }
                    }

                    listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                    {
                        entity = ent,
                        resultado = resultado,
                        mtu_idAnterior = mtu_idAnterior,
                        mtd_idAnterior = mtd_idAnterior
                    });
                }
            }

            return listaDisciplina;
        }

        /// <summary>
        /// Retorna a nota / parecer informado para a avliação adicional na linha do grid, caso esteja
        /// configurado para alimentar a nota da avaliação adicional.
        /// </summary>
        private string RetornaAvaliacaoAdicional(GridViewRow row)
        {
            if (gvAlunos.Columns[colunaNotaAdicional].Visible)
            {
                TextBox txtNota = (TextBox)row.FindControl("txtNotaAdicional");
                DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceresAdicional");

                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo;

                if (tipo == EscalaAvaliacaoTipo.Numerica && txtNota != null)
                {
                    if (txtNota.Visible)
                    {
                        return txtNota.Text;
                    }
                }
                else if (tipo == EscalaAvaliacaoTipo.Pareceres && ddlPareceres != null)
                {
                    if (ddlPareceres.Visible)
                    {
                        if (ddlPareceres.SelectedValue.Split(';')[0] == "-1")
                        {
                            return string.Empty;
                        }

                        return ddlPareceres.SelectedValue.Split(';')[0];
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacao(Control row, out bool salvarRelatorio)
        {
            TextBox txtNota = (TextBox)row.FindControl("txtNota");
            DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            // Verifica se o lançamento é por relatório ou se é conceito global
            salvarRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            // Se o formato de avaliação for por conceito global
            salvarRelatorio = salvarRelatorio || ((ACA_FormatoAvaliacaoTipo)VS_FormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.ConceitoGlobal);

            // Se a efetivação for do conceito global
            salvarRelatorio = salvarRelatorio || (Tud_id <= 0);

            if (txtNota != null)
            {
                if (txtNota.Visible)
                {
                    return txtNota.Text;
                }
            }

            if (ddlPareceres != null)
            {
                if (ddlPareceres.Visible)
                {
                    if (ddlPareceres.SelectedValue.Split(';')[0] == "-1")
                    {
                        return string.Empty;
                    }

                    return ddlPareceres.SelectedValue.Split(';')[0];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Retorna a nota / parecer pós-conselho informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacaoPosConselho(Control row)
        {
            TextBox txtNota = (TextBox)row.FindControl("txtNotaPosConselho");
            DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceresPosConselho");

            if (txtNota != null)
            {
                if (txtNota.Visible)
                {
                    return txtNota.Text;
                }
            }

            if (ddlPareceres != null)
            {
                if (ddlPareceres.Visible)
                {
                    if (ddlPareceres.SelectedValue.Split(';')[0] == "-1")
                    {
                        return string.Empty;
                    }

                    return ddlPareceres.SelectedValue.Split(';')[0];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Redireciona para a página de busca.
        /// </summary>
        /// <param name="msg"></param>
        private void RedirecionaBusca(string msg)
        {
            _UCEfetivacaoNotas.RedirecionaBusca(msg);
        }

        /// <summary>
        /// Adiciona os itens de resultado no dropDownList.
        /// </summary>
        private void AdicionaItemsResultado(DropDownList ddl, Int64 alu_id, int mtu_id)
        {
            // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
            ListItem item = new ListItem("-- Selecione um " + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") + " --", "-1");
            ddl.Items.Add(item);

            if (!VS_BuscouTiposResultados)
            {
                VS_BuscouTiposResultados = true;

                MTR_MatriculaTurma matriculaTurma = new MTR_MatriculaTurma
                {
                    alu_id = alu_id,
                    mtu_id = mtu_id
                };
                MTR_MatriculaTurmaBO.GetEntity(matriculaTurma);

                List<Struct_TipoResultado> listaTiposResultados;
                if (Tud_id > 0)
                {
                    // Verifica se existe resultados para esse curso/curriculo/periodo
                    listaTiposResultados = ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(matriculaTurma.cur_id, matriculaTurma.crr_id, matriculaTurma.crp_id, EnumTipoLancamento.Disciplinas, VS_tds_id);
                }
                else
                {
                    // Verifica se existe resultados para esse curso/curriculo/periodo
                    listaTiposResultados = ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(matriculaTurma.cur_id, matriculaTurma.crr_id, matriculaTurma.crp_id, EnumTipoLancamento.ConceitoGlobal);
                }
                VS_dtTiposResultados = listaTiposResultados;

                //if (ddl.Items.Count > 1)
                //return;
            }

            if (VS_dtTiposResultados.Count > 0)
            {
                foreach (Struct_TipoResultado tipoResultado in VS_dtTiposResultados)
                {
                    item = new ListItem(tipoResultado.tpr_nomenclatura, tipoResultado.tpr_resultado.ToString());
                    ddl.Items.Add(item);
                }
                return;
            }

            // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
            item = new ListItem("Aprovado", "1");
            ddl.Items.Add(item);

            if (VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.RecuperacaoFinal
                && _UCEfetivacaoNotas.VS_NomeAvaliacaoRecuperacaoFinal != "")
            {
                // Só mostra a opção "Reprovado", caso o critério de avaliação seja
                // Conceito Global + Frequência ou Conceito Global ou  Nota por Disciplina
                if ((ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia ||
                    (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobal ||
                    (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.NotaDisciplina ||
                    (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ApenasFrequencia)
                {
                    // Se o formato possuir uma avaliação do tipo Recuperação final, mostra ao invés das opções de reprovado,
                    // só uma opção "[Nome da avaliação de recuperação final]", indicando que ele precisa dessa avaliação.
                    item = new ListItem(_UCEfetivacaoNotas.VS_NomeAvaliacaoRecuperacaoFinal, "9");
                    ddl.Items.Add(item);
                }
            }
            else
            {
                // Só mostra a opção "Reprovado", caso o critério de avaliação seja
                // Conceito Global + Frequência ou Conceito Global ou  Nota por Disciplina
                if ((ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia ||
                    (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobal ||
                    (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.NotaDisciplina)
                {
                    item = new ListItem("Reprovado", "2");
                    ddl.Items.Add(item);
                }

                // Só mostra a opção "Reprovado por frequência", no lançamento global
                if (Tud_id <= 0 || gvAlunos.Columns[colunaFrequenciaFinal].Visible)
                {
                    // Só mostra a opção "Reprovado por frequência", caso o critério de avaliação seja
                    // Conceito Global + Frequência ou Apenas frequência
                    if ((ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia ||
                        (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ApenasFrequencia ||
                        gvAlunos.Columns[colunaFrequenciaFinal].Visible)
                    {
                        // Adiciona os itens da tabela MTR_MatriculaTurma.
                        item = new ListItem("Reprovado por frequência", "8");
                        ddl.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Aplica a variacao de frequencia
        /// </summary>
        /// <param name="frequencia">Valor da frequencia</param>
        /// <returns></returns>
        public decimal AplicaVariacaoFrequencia(decimal frequencia)
        {
            decimal result;
            decimal NotaLimiteArredondamento = (decimal)0.5;
            decimal variacaoFrequencia = VS_FormatoAvaliacao.fav_variacao == 0 ? (decimal)0.01 : VS_FormatoAvaliacao.fav_variacao;
            int numeroCasasDecimaisl = VS_FormatoAvaliacao.fav_variacao == 0 ? 2 : VS_NumeroCasasDecimaisFrequencia;

            decimal inteiro = (int)frequencia;
            decimal casasDecimais = frequencia - inteiro;

            decimal variacaoVerificar = casasDecimais / variacaoFrequencia;
            decimal variacaoVerificarInteiro = (int)variacaoVerificar;
            decimal variacaoVerificarDecimal = variacaoVerificar - variacaoVerificarInteiro;

            decimal acessimo = NotaLimiteArredondamento <= variacaoVerificarDecimal ? variacaoFrequencia : 0;

            result = inteiro + (variacaoFrequencia * variacaoVerificarInteiro) + acessimo;

            return decimal.Round(result, numeroCasasDecimaisl);
        }

        /// <summary>
        /// Método para cálculo da frequência do aluno apartir da
        /// qtde. de aulas e faltas do aluno.
        /// </summary>
        /// <param name="qtdAula">Quantidade de aulas</param>
        /// <param name="qtdFalta">Quantidade de faltas</param>
        /// <returns></returns>
        private decimal CalculaFrequencia(int qtdAula, int qtdFalta)
        {
            decimal frequencia = 0;

            // Calcula frequência apartir da qtde. de aulas e qtde. de faltas.
            if (qtdAula > 0)
            {
                frequencia = (((Convert.ToDecimal(qtdAula) - Convert.ToDecimal(qtdFalta)) / Convert.ToDecimal(qtdAula)) * 100);
            }

            return AplicaVariacaoFrequencia(frequencia);
        }

        /// <summary>
        /// Seta componentes relacionados ao relatório na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        /// <param name="Row">Linha do grid</param>
        /// <param name="exibeCampoNotaAluno">Indica se vai exibir o campo de notas</param>
        private void SetaComponentesRelatorioLinhaGrid(GridViewRow Row, bool exibeCampoNotaAluno)
        {
            ImageButton btnRelatorio = (ImageButton)Row.FindControl("btnRelatorio");
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            bool mostraBotaoRelatorio = ((tipo == EscalaAvaliacaoTipo.Relatorios) || (tipo != EscalaAvaliacaoTipo.Relatorios && Tud_id <= 0) && exibeCampoNotaAluno);

            CustomValidator cvRelatorioDesempenho = (CustomValidator)Row.FindControl("cvRelatorioDesempenho");
            if (cvRelatorioDesempenho != null)
            {
                // Se for turma do PEJA, e estiver configurado pra exibir o relatório, ele deve ser validado na hora de salvar.
                cvRelatorioDesempenho.Visible = _UCEfetivacaoNotas.VS_turma_Peja && mostraBotaoRelatorio;
            }

            // Deixar com display na tela para poder acessar por javascript.
            if (btnRelatorio != null)
            {
                btnRelatorio.Visible = mostraBotaoRelatorio;

                btnRelatorio.CommandArgument = gvAlunos.DataKeys[Row.RowIndex]["tur_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["tud_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["alu_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["mtu_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["mtd_id"] + ";" +
                                               (gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] != DBNull.Value ?
                                                gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] : "-1");

                btnRelatorio.ToolTip = tipo == EscalaAvaliacaoTipo.Relatorios ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                // Pesquisa o item pelo id.
                UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
                AtualizaIconesStatusPreenchimentoRelatorio(Row, false, mostraBotaoRelatorio, nota.Valor, nota.arq_idRelatorio);
            }

            ImageButton btnAvaliacao = (ImageButton)Row.FindControl("btnAvaliacao");

            string id = gvAlunos.DataKeys[Row.RowIndex]["alu_id"].ToString();
            if (btnAvaliacao != null)
            {
                btnAvaliacao.Visible = false;
            }
        }

        /// <summary>
        /// Seta componentes relacionados ao relatório na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        /// <param name="Row">Linha do grid</param>
        /// <param name="exibeCampoNotaAluno">Indica se vai exibir o campo de notas</param>
        private void SetaComponentesRelatorioPosConselhoLinhaGrid(GridViewRow Row, bool exibeCampoNotaAluno)
        {
            ImageButton btnRelatorioPosConselho = (ImageButton)Row.FindControl("btnRelatorioPosConselho");
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            bool mostraBotaoRelatorio = ((tipo == EscalaAvaliacaoTipo.Relatorios) || (tipo != EscalaAvaliacaoTipo.Relatorios && Tud_id <= 0) && exibeCampoNotaAluno);

            CustomValidator cvRelatorioDesempenhoPosConselho = (CustomValidator)Row.FindControl("cvRelatorioDesempenhoPosConselho");
            if (cvRelatorioDesempenhoPosConselho != null)
            {
                // Se for turma do PEJA, e estiver configurado pra exibir o relatório, ele deve ser validado na hora de salvar.
                cvRelatorioDesempenhoPosConselho.Visible = _UCEfetivacaoNotas.VS_turma_Peja && mostraBotaoRelatorio;
            }

            // Deixar com display na tela para poder acessar por javascript.
            if (btnRelatorioPosConselho != null)
            {
                btnRelatorioPosConselho.Visible = mostraBotaoRelatorio;

                btnRelatorioPosConselho.CommandArgument = gvAlunos.DataKeys[Row.RowIndex]["tur_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["tud_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["alu_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["mtu_id"] + ";" +
                                               gvAlunos.DataKeys[Row.RowIndex]["mtd_id"] + ";" +
                                               (gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] != DBNull.Value ?
                                                gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] : "-1");

                btnRelatorioPosConselho.ToolTip = tipo == EscalaAvaliacaoTipo.Relatorios ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                // Pesquisa o item pelo id.
                UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == btnRelatorioPosConselho.CommandArgument);
                AtualizaIconesStatusPreenchimentoRelatorio(Row, true, mostraBotaoRelatorio, nota.Valor, nota.arq_idRelatorio);
            }
        }

        /// <summary>
        /// Atualiza os ícones na linha do grid que indicam o status do preenchimento do relatório
        /// </summary>
        /// <param name="ctlLinha">Controle linha</param>
        /// <param name="posConselho">Indica se é nota pos-conselho</param>
        /// <param name="mostraBotaoRelatorio">Indica se o botão de relatório é mostrado</param>
        /// <param name="valorNota">Valor da nota do relatório</param>
        /// <param name="arqIdRelatorio">Id do arquivo anexado ao relatório</param>
        private void AtualizaIconesStatusPreenchimentoRelatorio(Control ctlLinha, bool posConselho, bool mostraBotaoRelatorio, string valorNota, string arqIdRelatorio)
        {
            Image imgSituacao = (Image)ctlLinha.FindControl(posConselho ? "imgSituacaoPosConselho" : "imgSituacao");
            HyperLink hplAnexo = (HyperLink)ctlLinha.FindControl(posConselho ? "hplAnexoPosConselho" : "hplAnexo");

            if (mostraBotaoRelatorio)
            {
                // Seta imagem de relatório lançado para o item.
                imgSituacao.Visible = !string.IsNullOrEmpty(valorNota);

                // Seta imagem de anexo para o item
                if (string.IsNullOrEmpty(arqIdRelatorio) || Convert.ToInt64(arqIdRelatorio) <= 0)
                {
                    hplAnexo.Visible = false;
                }
                else
                {
                    hplAnexo.Visible = true;
                    hplAnexo.NavigateUrl = "~/FileHandler.ashx?file=" + arqIdRelatorio;
                    hplAnexo.Target = "_blank";
                }
            }
            else
            {
                imgSituacao.Visible = false;
                hplAnexo.Visible = false;
            }
        }

        /// <summary>
        /// Seta componentes relacionados à observação do aluno na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        private void SetaComponenteObservacaoLinhaGrid(GridViewRow Row, CLS_AlunoAvaliacaoTurDis_Observacao observacao)
        {
            int index = Row.RowIndex;
            long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"].ToString());
            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"].ToString());
            int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtd_id"] != DBNull.Value ? gvAlunos.DataKeys[index].Values["mtd_id"] : "-1");

            Image imgObservacaoSituacao = (Image)Row.FindControl("imgObservacaoSituacao");

            if (imgObservacaoSituacao != null)
            {
                string qualidade = string.Empty;
                string desempenho = string.Empty;
                string recomendacaoAluno = string.Empty;
                string recomendacaoResp = string.Empty;

                if (observacao.entityObservacao != null && observacao.entityObservacao != new CLS_AlunoAvaliacaoTurmaDisciplinaObservacao())
                {
                    qualidade = observacao.entityObservacao.ado_qualidade;
                    desempenho = observacao.entityObservacao.ado_desempenhoAprendizado;
                    recomendacaoAluno = observacao.entityObservacao.ado_recomendacaoAluno;
                    recomendacaoResp = observacao.entityObservacao.ado_recomendacaoResponsavel;
                }

                imgObservacaoSituacao.Visible = !(string.IsNullOrEmpty(qualidade) &&
                                                  string.IsNullOrEmpty(desempenho) &&
                                                  string.IsNullOrEmpty(recomendacaoAluno) &&
                                                  string.IsNullOrEmpty(recomendacaoResp) &&
                                          !observacao.ltQualidade.Any() &&
                                          !observacao.ltDesempenho.Any() &&
                                          !observacao.ltRecomendacao.Any());
            }
        }

        // <summary>
        /// Seta componentes relacionados à observação do aluno na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        private void SetaComponenteObservacaoConselhoLinhaGrid(GridViewRow Row, CLS_AlunoAvaliacaoTur_Observacao observacao, byte resultado)
        {
            int index = Row.RowIndex;
            long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"].ToString());
            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"].ToString());

            Image imgObservacaoConselhoSituacao = (Image)Row.FindControl("imgObservacaoConselhoSituacao");

            if (imgObservacaoConselhoSituacao != null)
            {
                string qualidade = string.Empty;
                string desempenho = string.Empty;
                string recomendacaoAluno = string.Empty;
                string recomendacaoResp = string.Empty;

                if (observacao.entityObservacao != null && observacao.entityObservacao != new CLS_AlunoAvaliacaoTurmaObservacao())
                {
                    qualidade = observacao.entityObservacao.ato_qualidade;
                    desempenho = observacao.entityObservacao.ato_desempenhoAprendizado;
                    recomendacaoAluno = observacao.entityObservacao.ato_recomendacaoAluno;
                    recomendacaoResp = observacao.entityObservacao.ato_recomendacaoResponsavel;
                }

                imgObservacaoConselhoSituacao.Visible = !(string.IsNullOrEmpty(qualidade) &&
                                                  string.IsNullOrEmpty(desempenho) &&
                                                  string.IsNullOrEmpty(recomendacaoAluno) &&
                                                  string.IsNullOrEmpty(recomendacaoResp) &&
                                          !observacao.ltQualidade.Any() &&
                                          !observacao.ltDesempenho.Any() &&
                                          !observacao.ltRecomendacao.Any()) || resultado > 0;
            }
        }

        /// <summary>
        /// Calcular a frequência acumulada.
        /// </summary>
        /// <param name="row">Linha a ser calculada a frequência acumulada.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        private void CalculaFrequenciaAcumulada(GridViewRow row, long alu_id, int mtu_id)
        {
            int aat_numeroAulas = 0;
            int aat_numeroFaltas = 0;

            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
            if (txtQtdeAula != null)
            {
                int.TryParse(txtQtdeAula.Text, out aat_numeroAulas);
            }

            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
            if (txtQtdeFalta != null)
            {
                int.TryParse(txtQtdeFalta.Text, out aat_numeroFaltas);
            }

            TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");
            if (txtFrequenciaAcumulada != null)
            {
                txtFrequenciaAcumulada.Text = string.Format(
                        VS_FormatacaoDecimaisFrequencia
                        , CLS_AlunoAvaliacaoTurmaBO.RetornaFrequenciaAcumuladaCalculada(
                    _VS_tur_id,
                    alu_id,
                    mtu_id,
                    _VS_fav_id,
                    _VS_ava_id,
                    aat_numeroAulas,
                            aat_numeroFaltas)
                    );
            }
        }

        /// <summary>
        /// Calcular a frequência ajustada.
        /// </summary>
        /// <param name="row">Linha a ser calculada a frequência ajustada.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        private void CalculaFrequenciaAjustada(GridViewRow row, long alu_id, int mtu_id)
        {
            int aat_numeroAulas = 0;
            int aat_numeroFaltas = 0;
            int aat_numeroAusenciasCompensadas = 0;

            TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
            if (txtQtdeAula != null)
            {
                int.TryParse(txtQtdeAula.Text, out aat_numeroAulas);
            }

            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
            if (txtQtdeFalta != null)
            {
                int.TryParse(txtQtdeFalta.Text, out aat_numeroFaltas);
            }

            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
            if (txtAusenciasCompensadas != null)
            {
                int.TryParse(txtAusenciasCompensadas.Text, out aat_numeroAusenciasCompensadas);
            }

            if (VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia == (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPrevistasDocente)
            {
                Label lblQtdeFaltaReposicao = (Label)row.FindControl("lblQtdeFaltaReposicao");
                if (lblQtdeFaltaReposicao != null)
                {
                    int faltasReposicao = 0;
                    if (int.TryParse(lblQtdeFaltaReposicao.Text, out faltasReposicao))
                    {
                        aat_numeroFaltas += faltasReposicao;
                    }
                }
            }

            TextBox txtFrequenciaAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
            if (txtFrequenciaAjustada != null)
            {
                txtFrequenciaAjustada.Text =
                    string.Format(
                        VS_FormatacaoDecimaisFrequencia
                        , CLS_AlunoAvaliacaoTurmaBO.RetornaFrequenciaAjustadaCalculada(
                            Tud_id,
                            _VS_tur_id,
                            alu_id,
                            mtu_id,
                            _VS_fav_id,
                            _VS_ava_id,
                            VS_tpc_id,
                            VS_EscalaAvaliacao.esa_tipo,
                            VS_EscalaAvaliacaoDocente.esa_tipo,
                            VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                            VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                            aat_numeroAulas,
                            aat_numeroFaltas,
                            aat_numeroAusenciasCompensadas
                        )
                    );

                decimal frequencia;
                HiddenField hdnSituacao = (HiddenField)row.FindControl("hdnSituacao");
                int situacao = Convert.ToInt32(hdnSituacao.Value);

                if (Decimal.TryParse(txtFrequenciaAjustada.Text, out frequencia) && aat_numeroAulas > 0)
                {
                    // se o formato de avaliacao tiver o percentual minimo de frequencia da disciplina cadastrado, devo utilizar esse valor,
                    // senao devo utilizar o percentual minimo de frequencia geral cadastrado para o formato de avaliacao
                    if ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0 && frequencia < VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                        || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0 && frequencia < VS_FormatoAvaliacao.percentualMinimoFrequencia))
                    {
                        row.Style["background-color"] = ApplicationWEB.AlunoFrequenciaLimite;
                    }
                    // alunos proximos de atingir o percentual minimo de frequencia
                    else if (VS_FormatoAvaliacao.percentualBaixaFrequencia > 0
                            && ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0
                                    && frequencia >= VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
                                    && frequencia < VS_FormatoAvaliacao.percentualBaixaFrequencia)
                                || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0
                                    && frequencia >= VS_FormatoAvaliacao.percentualMinimoFrequencia
                                    && frequencia < VS_FormatoAvaliacao.percentualBaixaFrequencia)))
                    {
                        row.Style["background-color"] = ApplicationWEB.CorAlunoProximoBaixaFrequencia;
                    }
                    else if ((row.Style["background-color"] == ApplicationWEB.AlunoFrequenciaLimite ||
                              row.Style["background-color"] == ApplicationWEB.CorAlunoProximoBaixaFrequencia) &&
                             situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        row.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                    else if (row.Style["background-color"] == ApplicationWEB.AlunoFrequenciaLimite ||
                             row.Style["background-color"] == ApplicationWEB.CorAlunoProximoBaixaFrequencia)
                    {
                        row.Style.Remove("background-color");
                    }
                }
            }
        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        /// <param name="adicional">Indica se serão carregados os pareceres da avaliação adicional</param>
        private void CarregarPareceres(DropDownList ddlPareceres, bool adicional)
        {
            ListItem li = new ListItem("-- Selecione um conceito --", "-1;-1", true);
            ddlPareceres.Items.Add(li);

            List<ACA_EscalaAvaliacaoParecer> dt = adicional ? LtPareceresAdicional : LtPareceres;

            foreach (ACA_EscalaAvaliacaoParecer eap in dt)
            {
                li = new ListItem(eap.descricao, eap.eap_valor + ";" + eap.eap_ordem.ToString());
                ddlPareceres.Items.Add(li);
            }
        }

        /// <summary>
        /// Busca na tabela de pareceres o campo eap_ordem de acordo com o eap_valor.
        /// </summary>
        /// <param name="eap_valor">Valor a ser buscado</param>
        /// <param name="adicional">Indica se serão carregados os pareceres da avaliação adicional</param>
        /// <returns>A ordem do parecer (-1 caso não encontrado)</returns>
        private int RetornaOrdemParecer(string eap_valor, bool adicional)
        {
            List<ACA_EscalaAvaliacaoParecer> dt = adicional ? LtPareceresAdicional : LtPareceres;

            // Busca o campo Ordem de acordo com o valor do parecer.
            var x = from ACA_EscalaAvaliacaoParecer eap in dt
                    where eap.eap_valor.Equals(eap_valor)
                    select eap.eap_ordem;

            if (x.Count() > 0)
            {
                return x.First();
            }

            return -1;
        }

        /// <summary>
        /// Método para salvar o relatorio antes de imprimir
        /// </summary>
        public string ImprimirRelatorio(long alu_idSalvar, string idNotaRelatorio, string notaRelatorio, HttpPostedFile arquivoRelatorio, bool visivelAnexo, out EscalaAvaliacaoTipo esa_tipo, out int tpc_id)
        {
            esa_tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
            tpc_id = VS_Avaliacao.tpc_id;

            // Salva o relatório da nota em view state
            SalvarRelatorio(idNotaRelatorio, notaRelatorio, arquivoRelatorio, visivelAnexo);

            // Salva os dados da efetivação do aluno no banco
            List<CLS_AvaliacaoTurma_Cadastro> listaTurma = new List<CLS_AvaliacaoTurma_Cadastro>();

            listaTurma = gvAlunos.Rows.Cast<GridViewRow>().AsParallel().SelectMany(p => AdicionaLinhaTurma(p)).ToList();
            //foreach (GridViewRow row in gvAlunos.Rows)
            //{
            //    long alu_idGrid = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
            //    if (alu_idSalvar == alu_idGrid)
            //    {
            //        AdicionaLinhaTurma(row, ref listaTurma);
            //    }
            //}

            List<sDisciplinasDivergentesPorAluno> list;
            List<string> list2;

            List<NotaFinalAlunoTurma> listaNotaFinalAluno = retornaListaNotaFinalAlunoTurma();
            List<long> listAlunosComDivergencia;
            string msgValidacaoFrequencia;

            CLS_AlunoAvaliacaoTurmaBO.Save(
                listaTurma,
                resultadoFinal,
                ApplicationWEB.TamanhoMaximoArquivo,
                ApplicationWEB.TiposArquivosPermitidos,
                VS_Turma,
                VS_FormatoAvaliacao,
                (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                (ACA_FormatoAvaliacaoCalculoQtdeAulasDadas)VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                out msgValidacaoFrequencia,
                VS_Avaliacao.tpc_id,
                (AvaliacaoTipo)VS_Avaliacao.ava_tipo,
                false,
                out list,
                out list2,
                out listAlunosComDivergencia,
                VS_EscalaAvaliacao,
                listaNotaFinalAluno
                , _UCEfetivacaoNotas.VS_turma_Peja, _VS_ava_id, VS_Avaliacao.ava_exibeNotaPosConselho
                , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            try
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, _UCEfetivacaoNotas.VS_MensagemLogEfetivacao + "tur_id: " + _VS_tur_id + "; tud_id: " + Tud_id + "; ava_id: " + _VS_ava_id + "; alu_id: " + alu_idSalvar);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }

            // Carrega o relatorio caso ele tenha sido salvo
            UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == idNotaRelatorio);
            return nota.arq_idRelatorio;
        }

        /// <summary>
        /// Salva os dados do relatório que está sendo lançado.
        /// </summary>
        public void SalvarRelatorio(string idRelatorio, string nota, HttpPostedFile arquivoRelatorio, bool visivelAnexo)
        {
            // Cria anexo para salvar temporariamente no banco
            SYS_Arquivo arq = SYS_ArquivoBO.CriarAnexo(arquivoRelatorio);
            if (arq != null)
            {
                // Salva o anexo com a situação temporário
                arq.arq_situacao = (byte)SYS_ArquivoSituacao.Temporario;
                SYS_ArquivoBO.Save(arq, ApplicationWEB.TamanhoMaximoArquivo, ApplicationWEB.TiposArquivosPermitidos);
            }
            string arq_idRelatorioSalvo = string.Empty;

            if (_VS_Nota_Relatorio.Exists(p => p.Id == idRelatorio))
            {
                int alterar = _VS_Nota_Relatorio.FindIndex(p => p.Id == idRelatorio);
                string arq_idRelatorio = visivelAnexo ? _VS_Nota_Relatorio.Find(p => p.Id == idRelatorio).arq_idRelatorio : string.Empty;
                arq_idRelatorioSalvo = arq == null ? arq_idRelatorio : arq.arq_id.ToString();

                _VS_Nota_Relatorio[alterar] = new UCEfetivacaoNotas.NotasRelatorio
                {
                    Id = idRelatorio,
                    Valor = nota,
                    arq_idRelatorio = arq_idRelatorioSalvo
                };
            }
            else
            {
                arq_idRelatorioSalvo = arq == null ? string.Empty : arq.arq_id.ToString();
                UCEfetivacaoNotas.NotasRelatorio rel = new UCEfetivacaoNotas.NotasRelatorio
                {
                    Id = idRelatorio,
                    Valor = nota,
                    arq_idRelatorio = arq_idRelatorioSalvo
                };
                _VS_Nota_Relatorio.Add(rel);
            }

            // Atualiza o gvAlunos.
            uppGridAlunos.Update();

            if (!String.IsNullOrEmpty(hdnLocalImgCheckSituacao.Value))
            {
                string[] localizacaoImgCheck = hdnLocalImgCheckSituacao.Value.Split(',');
                Control controlPrincipal;
                if (localizacaoImgCheck.Count() == 1)
                {
                    controlPrincipal = gvAlunos.Rows[Convert.ToInt32(localizacaoImgCheck[0])];
                }
                else
                {
                    controlPrincipal = ((Repeater)(gvAlunos.Rows[Convert.ToInt32(localizacaoImgCheck[0])]
                                            .FindControl("rptComponenteRegencia"))).Items[Convert.ToInt32(localizacaoImgCheck[1])];
                }

                AtualizaIconesStatusPreenchimentoRelatorio(controlPrincipal, false, true, nota, arq_idRelatorioSalvo);
                hdnLocalImgCheckSituacao.Value = String.Empty;
            }
        }

        /// <summary>
        /// Recalcula os campos da frequência e o campo da média para a linha do grid de alunos.
        /// </summary>
        /// <param name="index">Linha que será recarregada</param>
        /// <param name="CamposAtualizar">Quais campos serão atualizados: Notas/Faltas/Notas_e_Faltas</param>
        private void AtualizaFrequenciaMedia_LinhaAluno(int index, string CamposAtualizar)
        {
            GridViewRow row = gvAlunos.Rows[index];

            decimal frequencia, media, frequenciaAcumulada, FrequenciaFinalAjustada;
            int qtdeAulas, qtdeFaltas, ausenciasCompensadas;
            string mediaConceito;

            long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index]["alu_id"]);
            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index]["mtu_id"]);

            if (Tud_id > 0)
            {
                // Frequência acumulada - não existe para a disciplina.
                frequenciaAcumulada = 0;
                int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[index]["mtd_id"]);
                MTR_MatriculaTurmaDisciplinaBO.CalculaFrequencia_Media_Aluno(
                    alu_id,
                    mtu_id,
                    mtd_id,
                    Tud_id,
                    VS_FormatoAvaliacao.fav_id,
                    VS_Avaliacao.tpc_id,
                    VS_EscalaAvaliacao.esa_tipo,
                    VS_EscalaAvaliacaoDocente.esa_tipo,
                    VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                    VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                    out frequencia,
                    out qtdeAulas,
                    out qtdeFaltas,
                    out media,
                    out mediaConceito,
                    out frequenciaAcumulada,
                    out ausenciasCompensadas,
                    out FrequenciaFinalAjustada
                    , (byte)VS_Avaliacao.ava_tipo
                    , VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal
                    , __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                    (byte)_UCEfetivacaoNotas.VS_tipoDocente : (byte)0);

                // Atualiza o campo de nota com a média da disciplina.
                if (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas")
                {
                    if (gvAlunos.Columns[colunaNotaRegencia].Visible)
                    {
                        DataTable dbNotasComponentesRegencia = MTR_MatriculaTurmaDisciplinaBO.Calcula_MediaComponentesRegencia_Aluno(
                            alu_id,
                            mtu_id,
                            _VS_tur_id,
                            VS_FormatoAvaliacao.fav_id,
                            VS_Avaliacao.tpc_id,
                            VS_EscalaAvaliacao.esa_tipo,
                            VS_EscalaAvaliacaoDocente.esa_tipo);

                        Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");
                        foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                        {
                            HiddenField hfDataKeys = (HiddenField)rptItem.FindControl("hfDataKeys");
                            string[] dataKeys = hfDataKeys.Value.Split(';');
                            mtd_id = Convert.ToInt32(dataKeys[1]);
                            List<DataRow> rowComponenteRegencia = (from tRow in dbNotasComponentesRegencia.AsEnumerable()
                                                                   where Convert.ToInt64(tRow.Field<object>("alu_id")) == alu_id
                                                                    && Convert.ToInt32(tRow.Field<object>("mtu_id")) == mtu_id
                                                                    && Convert.ToInt32(tRow.Field<object>("mtd_id")) == mtd_id
                                                                   select tRow).ToList();

                            if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                            {
                                TextBox txtNota = (TextBox)rptItem.FindControl("txtNota");
                                if (txtNota != null)
                                {
                                    txtNota.Text = NotaFormatada(rowComponenteRegencia.Count > 0 ? Convert.ToDecimal(rowComponenteRegencia[0]["Avaliacao"]) : Convert.ToDecimal("-1,00"));
                                }
                            }
                            else if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                            {
                                DropDownList ddlPareceres = (DropDownList)rptItem.FindControl("ddlPareceres");
                                mediaConceito = rowComponenteRegencia.Count > 0 ? rowComponenteRegencia[0]["AvaliacaoPareceres"].ToString() : "";
                                if (ddlPareceres != null)
                                {
                                    ddlPareceres.SelectedValue = string.IsNullOrEmpty(mediaConceito) ? "-1;-1" : mediaConceito.ToString() + ";" + RetornaOrdemParecer(mediaConceito.ToString(), false);
                                }
                            }
                        }
                    }
                    else
                    {
                        SetaNotaAtualizada(row, NotaFormatada(media), mediaConceito);
                    }
                }
            }
            else
            {
                string Avaliacao;

                MTR_MatriculaTurmaBO.CalculaFrequencia_Media_Aluno(
                    alu_id,
                    mtu_id,
                    _VS_tur_id,
                    VS_FormatoAvaliacao.fav_id,
                    VS_Avaliacao.tpc_id,
                    VS_Avaliacao.ava_id,
                    VS_EscalaAvaliacao.esa_tipo,
                    VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                    VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                    out frequencia,
                    out qtdeAulas,
                    out qtdeFaltas,
                    out frequenciaAcumulada,
                    out ausenciasCompensadas,
                    out FrequenciaFinalAjustada
                    , out Avaliacao);

                // Atualiza o campo de nota com a média do conceito global.
                if (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas")
                {
                    SetaNotaAtualizada(row, Avaliacao, "");
                }
            }

            if (CamposAtualizar == "Faltas" || CamposAtualizar == "Notas_e_Faltas")
            {
                // Configura os campos Qtde. Aulas, Qtde. Fatas e Frequência.
                TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
                if (txtQtdeFalta != null)
                {
                    txtQtdeFalta.Text = qtdeFaltas.ToString();
                }

                TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
                if (txtQtdeAula != null)
                {
                    txtQtdeAula.Text = qtdeAulas.ToString();
                }

                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                if (txtFrequencia != null)
                {
                    txtFrequencia.Text = string.Format(VS_FormatacaoDecimaisFrequencia, frequencia);
                }

                TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");
                if (txtFrequenciaAcumulada != null)
                {
                    txtFrequenciaAcumulada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, frequenciaAcumulada);
                }

                TextBox txtFrequenciaFinal = (TextBox)row.FindControl("txtFrequenciaFinal");
                if (txtFrequenciaFinal != null)
                {
                    txtFrequenciaFinal.Text = string.Format(VS_FormatacaoDecimaisFrequencia, frequenciaAcumulada);

                    if (gvAlunos.Columns[colunaFrequenciaFinal].Visible)
                    {
                        DropDownList ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                        if (ddlResultado != null)
                        {
                            if (Convert.ToDecimal(txtFrequenciaFinal.Text) < VS_FormatoAvaliacao.percentualMinimoFrequencia)
                            {
                                ddlResultado.SelectedValue = "8";
                            }
                        }
                    }
                }

                TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
                if ((txtAusenciasCompensadas != null) && ExibeCompensacaoAusencia)
                {
                    txtAusenciasCompensadas.Text = ausenciasCompensadas.ToString();
                }

                TextBox txtFrequenciaFinalAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
                if (txtFrequenciaFinalAjustada != null)
                {
                    txtFrequenciaFinalAjustada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, FrequenciaFinalAjustada);

                    decimal frequencia1;
                    if (Decimal.TryParse(txtFrequenciaFinalAjustada.Text, out frequencia1) && gvAlunos.Columns[colunaFrequenciaAjustada].Visible
                        // se o formato de avaliacao tiver o percentual minimo de frequencia da disciplina cadastrado, devo utilizar esse valor,
                        // senao devo utilizar o percentual minimo de frequencia geral cadastrado para o formato de avaliacao
                        && ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0 && frequencia1 < VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                            || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0 && frequencia1 < VS_FormatoAvaliacao.percentualMinimoFrequencia)))
                    {
                        row.Style["background-color"] = ApplicationWEB.AlunoFrequenciaLimite;
                    }
                    else if (row.Style["background-color"] == ApplicationWEB.AlunoFrequenciaLimite)
                    {
                        row.Style.Remove("background-color");
                    }
                }
            }

            if (Tud_idPrincipal > 0 && (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas"))
            {
                int mtd_idPrincipal = Convert.ToInt32(gvAlunos.DataKeys[index]["mtd_idPrincipal"]);

                TextBox txtNotaAdicional = (TextBox)row.FindControl("txtNotaAdicional");

                if (txtNotaAdicional != null)
                {
                    decimal dMedia = MTR_MatriculaTurmaDisciplinaBO.CalculaMediaAvaliacaoAdicionalPorAluno(
                        alu_id,
                        mtu_id,
                        mtd_idPrincipal,
                        Tud_idPrincipal,
                        VS_FormatoAvaliacao.fav_id,
                        VS_Avaliacao.tpc_id,
                        VS_EscalaAvaliacaoAdicional.esa_tipo,
                        VS_EscalaAvaliacaoDocente.esa_tipo);

                    txtNotaAdicional.Text = dMedia.ToString();
                }
            }
        }

        /// <summary>
        /// Seta a nota no txt de nota ou no combo de pareceres de acordo com o tipo de escala.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="media"></param>
        /// <param name="mediaConceito"></param>
        private void SetaNotaAtualizada(GridViewRow row, string media, string mediaConceito)
        {
            if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
            {
                TextBox txtNota = (TextBox)row.FindControl("txtNota");

                if (txtNota != null)
                {
                    txtNota.Text = media;
                }
            }
            if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
            {
                DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                if (ddlPareceres != null)
                {
                    ddlPareceres.SelectedValue = string.IsNullOrEmpty(mediaConceito) ? "-1;-1" : mediaConceito.ToString() + ";" + RetornaOrdemParecer(mediaConceito.ToString(), false);
                }
            }
        }

        /// <summary>
        /// Recalcula os campos da frequência e o campo da média para a linha do grid de alunos.
        /// </summary>
        /// <param name="CamposAtualizar">Quais campos serão atualizados: Notas/Faltas/Notas_e_Faltas</param>
        private void AtualizaFrequenciaMedia_TodosAlunos(string CamposAtualizar)
        {
            // DataTable para calcular notas/frequência de disciplinas
            DataTable dtDisciplinas;

            // DataTable para calcular notas/frequência do conceito global
            DataTable dtConceitoGlobal = new DataTable();

            // DataTable para calcular notas dos componentes da regencia
            DataTable dbNotasComponentesRegencia = new DataTable();

            if (Tud_id > 0)
            {
                dtDisciplinas = MTR_MatriculaTurmaDisciplinaBO.CalculaFrequencia_Media_TodosAlunos(
                    Tud_id,
                    VS_FormatoAvaliacao.fav_id,
                    VS_Avaliacao.tpc_id,
                    VS_EscalaAvaliacao.esa_tipo,
                    VS_EscalaAvaliacaoDocente.esa_tipo,
                    VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                    VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                    (byte)VS_Avaliacao.ava_tipo,
                    VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal,
                    __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                    (byte)_UCEfetivacaoNotas.VS_tipoDocente : (byte)0, VS_listaTur_ids);

                // Atualiza o campo de nota com a média da disciplina, para os componentes da regencia.
                if (gvAlunos.Columns[colunaNotaRegencia].Visible && (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas"))
                {
                    dbNotasComponentesRegencia = MTR_MatriculaTurmaDisciplinaBO.Calcula_MediaComponentesRegencia_TodosAlunos(
                            _VS_tur_id,
                            VS_FormatoAvaliacao.fav_id,
                            VS_Avaliacao.tpc_id,
                            VS_EscalaAvaliacao.esa_tipo,
                            VS_EscalaAvaliacaoDocente.esa_tipo);
                }
            }
            else
            {
                dtDisciplinas = MTR_MatriculaTurmaBO.CalculaFrequencia_Media_TodosAlunos(
                    _VS_tur_id,
                    VS_FormatoAvaliacao.fav_id,
                    VS_Avaliacao.tpc_id,
                    VS_EscalaAvaliacao.esa_tipo,
                    VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
                    VS_FormatoAvaliacao.fav_calculoQtdeAulasDadas,
                    _VS_ava_id);
            }

            if (Tud_idPrincipal > 0 && (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas"))
            {
                dtConceitoGlobal = MTR_MatriculaTurmaDisciplinaBO.CalculaMediaAvaliacaoAdicionalTodosAlunos(
                    Tud_idPrincipal,
                    VS_FormatoAvaliacao.fav_id,
                    VS_Avaliacao.tpc_id,
                    VS_EscalaAvaliacaoAdicional.esa_tipo,
                    VS_EscalaAvaliacaoDocente.esa_tipo);
            }

            long alu_id;

            string corAlunoFrequenciaLimite = ApplicationWEB.AlunoFrequenciaLimite;

            foreach (GridViewRow row in gvAlunos.Rows)
            {
                alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
                var x = from DataRow dr in dtDisciplinas.Rows
                        where Convert.ToInt64(dr["alu_id"]) == alu_id
                        select dr;

                DataRow drDadoAluno = x.Count() > 0 ? x.First() : dtDisciplinas.NewRow();

                if (Tud_id > 0 || VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal)
                {
                    // Atualiza a nota se for lançamento na disciplina e se a coluna de nota estiver visível.
                    if (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas")
                    {
                        if (gvAlunos.Columns[colunaNotaRegencia].Visible)
                        {
                            int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex]["mtu_id"]);
                            int mtd_id;
                            Repeater rptComponenteRegencia = (Repeater)row.FindControl("rptComponenteRegencia");

                            foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                            {
                                HiddenField hfDataKeys = (HiddenField)rptItem.FindControl("hfDataKeys");
                                string[] dataKeys = hfDataKeys.Value.Split(';');
                                mtd_id = Convert.ToInt32(dataKeys[1]);
                                List<DataRow> rowComponenteRegencia = (from tRow in dbNotasComponentesRegencia.AsEnumerable()
                                                                       where Convert.ToInt64(tRow.Field<object>("alu_id")) == alu_id
                                                                        && Convert.ToInt32(tRow.Field<object>("mtu_id")) == mtu_id
                                                                        && Convert.ToInt32(tRow.Field<object>("mtd_id")) == mtd_id
                                                                       select tRow).ToList();

                                if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                                {
                                    TextBox txtNota = (TextBox)rptItem.FindControl("txtNota");
                                    if (txtNota != null)
                                    {
                                        txtNota.Text = NotaFormatada(rowComponenteRegencia.Count > 0 ? Convert.ToDecimal(rowComponenteRegencia[0]["Avaliacao"]) : Convert.ToDecimal("-1,00"));
                                    }
                                }
                                else if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                                {
                                    DropDownList ddlPareceres = (DropDownList)rptItem.FindControl("ddlPareceres");
                                    string mediaConceito = rowComponenteRegencia.Count > 0 ? rowComponenteRegencia[0]["AvaliacaoPareceres"].ToString() : "";
                                    if (ddlPareceres != null)
                                    {
                                        ddlPareceres.SelectedValue = string.IsNullOrEmpty(mediaConceito) ? "-1;-1" : mediaConceito.ToString() + ";" + RetornaOrdemParecer(mediaConceito.ToString(), false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                            {
                                TextBox txtNota = (TextBox)row.FindControl("txtNota");

                                if (txtNota != null)
                                {
                                    txtNota.Text = NotaFormatada(x.Count() > 0 && !string.IsNullOrEmpty(drDadoAluno["Avaliacao"].ToString()) ?
                                        Convert.ToDecimal(drDadoAluno["Avaliacao"]) : Convert.ToDecimal("-1,00"));
                                }
                            }
                            if (VS_EscalaAvaliacaoDocente.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                            {
                                DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");

                                string mediaConceito = x.Count() > 0 ? drDadoAluno["AvaliacaoPareceres"].ToString() : "";
                                if (ddlPareceres != null)
                                {
                                    ddlPareceres.SelectedValue = string.IsNullOrEmpty(mediaConceito) ? "-1;-1" : mediaConceito.ToString() + ";" + RetornaOrdemParecer(mediaConceito.ToString(), false);
                                }
                            }
                        }
                    }
                }

                if (Tud_idPrincipal > 0)
                {
                    // Atualiza a nota quando é conceito global e se a coluna de nota adicional estiver visível.
                    if (CamposAtualizar == "Notas" || CamposAtualizar == "Notas_e_Faltas")
                    {
                        var xAdicional = from DataRow dr in dtConceitoGlobal.Rows
                                         where Convert.ToInt64(dr["alu_id"]) == Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"])
                                         && Convert.ToInt32(dr["mtu_id"]) == Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["mtu_id"])
                                         && Convert.ToInt64(dr["mtd_id"]) == Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["mtd_idPrincipal"])
                                         select new
                                         {
                                             avaliacao = Convert.ToDecimal(dr["Avaliacao"])
                                         };

                        TextBox txtNotaAdicional = (TextBox)row.FindControl("txtNotaAdicional");

                        if (txtNotaAdicional != null)
                        {
                            txtNotaAdicional.Text = xAdicional.Count() > 0 ? xAdicional.First().avaliacao.ToString() : "0,00";
                        }
                    }
                }

                if (CamposAtualizar == "Faltas" || CamposAtualizar == "Notas_e_Faltas")
                {
                    // Configura os campos Qtde. Aulas, Qtde. Fatas e Frequência.
                    TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
                    if (txtQtdeFalta != null)
                    {
                        txtQtdeFalta.Text = x.Count() > 0 ? Convert.ToString(Convert.ToInt32(drDadoAluno["QtFaltasAluno"])) : "0";
                    }

                    TextBox txtQtdeAula = (TextBox)row.FindControl("txtQtdeAula");
                    if (txtQtdeAula != null)
                    {
                        txtQtdeAula.Text = x.Count() > 0 ? Convert.ToString(Convert.ToInt32(drDadoAluno["QtAulasAluno"])) : "0";
                    }

                    TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                    if (txtFrequencia != null)
                    {
                        txtFrequencia.Text = string.Format(VS_FormatacaoDecimaisFrequencia, (x.Count() > 0 ? Convert.ToDecimal(drDadoAluno["Frequencia"]) : 0));
                    }

                    TextBox txtFrequenciaAcumulada = (TextBox)row.FindControl("txtFrequenciaAcumulada");
                    if (txtFrequenciaAcumulada != null)
                    {
                        txtFrequenciaAcumulada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, (x.Count() > 0 ? Convert.ToDecimal(drDadoAluno["FrequenciaAcumulada"]) : 0));
                    }

                    TextBox txtFrequenciaFinal = (TextBox)row.FindControl("txtFrequenciaFinal");
                    if (txtFrequenciaFinal != null)
                    {
                        txtFrequenciaFinal.Text = string.Format(VS_FormatacaoDecimaisFrequencia, (x.Count() > 0 ? Convert.ToDecimal(drDadoAluno["FrequenciaAcumulada"]) : 0));

                        if (gvAlunos.Columns[colunaFrequenciaFinal].Visible)
                        {
                            DropDownList ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                            if (ddlResultado != null)
                            {
                                if (Convert.ToDecimal(txtFrequenciaFinal.Text) < VS_FormatoAvaliacao.percentualMinimoFrequencia)
                                {
                                    ddlResultado.SelectedValue = "8";
                                }
                            }
                        }
                    }

                    TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
                    if ((txtAusenciasCompensadas != null) && ExibeCompensacaoAusencia)
                    {
                        txtAusenciasCompensadas.Text = x.Count() > 0 ? Convert.ToString(Convert.ToInt32(drDadoAluno["ausenciasCompensadas"])) : "0";
                    }

                    TextBox txtFrequenciaFinalAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
                    if (txtFrequenciaFinalAjustada != null)
                    {
                        txtFrequenciaFinalAjustada.Text = string.Format(
                                VS_FormatacaoDecimaisFrequencia
                                , (x.Count() > 0 ? Convert.ToDecimal(drDadoAluno["FrequenciaFinalAjustada"]) : 0)
                            );

                        decimal frequencia;
                        if (Decimal.TryParse(txtFrequenciaFinalAjustada.Text, out frequencia) && gvAlunos.Columns[colunaFrequenciaAjustada].Visible
                            // se o formato de avaliacao tiver o percentual minimo de frequencia da disciplina cadastrado, devo utilizar esse valor,
                            // senao devo utilizar o percentual minimo de frequencia geral cadastrado para o formato de avaliacao
                            && ((VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina > 0 && frequencia < VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                                || (VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina == 0 && frequencia < VS_FormatoAvaliacao.percentualMinimoFrequencia)))
                        {
                            row.Style["background-color"] = corAlunoFrequenciaLimite;
                        }
                        else if (row.Style["background-color"] == corAlunoFrequenciaLimite)
                        {
                            row.Style.Remove("background-color");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Atualiza a frequência ajustada recalculando ela de acordo com os valores nos campos de faltas e aulas.
        /// </summary>
        /// <param name="sender">Txt de frequência da linha do grid que será atualizada</param>
        /// <param name="e">EventArgs</param>
        private void AtualizarFrequenciaAjustada(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;

                // Calcular a frequência acumulada para o aluno.
                long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);
                int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex]["mtu_id"]);

                CalculaFrequenciaAjustada(row, alu_id, mtu_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar a frequência acumulada.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Atualiza a frequência acumulada recalculando ela de acordo com os valores nos campos de faltas e aulas.
        /// </summary>
        /// <param name="sender">Txt de frequência da linha do grid que será atualizada</param>
        /// <param name="e">EventArgs</param>
        private void AtualizarFrequenciaAcumulada(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;

                // Calcular a frequência acumulada para o aluno.
                long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);
                int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex]["mtu_id"]);

                CalculaFrequenciaAcumulada(row, alu_id, mtu_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar a frequência acumulada.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva os dados da justificativa de nota pós-conselho que está sendo lançado.
        /// </summary>
        public void SalvarJustificativaPosConselho(string id, string textoJustificativa)
        {
            if (VS_JustificativaPosConselho.Exists(p => p.Id == id))
            {
                int alterar = VS_JustificativaPosConselho.FindIndex(p => p.Id == id);
                VS_JustificativaPosConselho[alterar] = new UCEfetivacaoNotas.Justificativa
                {
                    Id = id,
                    Valor = textoJustificativa
                };
            }
            else
            {
                UCEfetivacaoNotas.Justificativa justificativa = new UCEfetivacaoNotas.Justificativa
                {
                    Id = id,
                    Valor = textoJustificativa
                };
                VS_JustificativaPosConselho.Add(justificativa);
            }

            // Atualiza o gvAlunos.
            uppGridAlunos.Update();

            if (!String.IsNullOrEmpty(hdnLocalImgCheckSituacao.Value))
            {
                string[] localizacaoImgCheck = hdnLocalImgCheckSituacao.Value.Split(',');
                Image imgCheckSituacao;
                if (localizacaoImgCheck.Count() == 1)
                {
                    imgCheckSituacao = (Image)gvAlunos.Rows[Convert.ToInt32(localizacaoImgCheck[0])].FindControl("imgJustificativaPosConselhoSituacao");
                }
                else
                {
                    imgCheckSituacao = (Image)((Repeater)(gvAlunos.Rows[Convert.ToInt32(localizacaoImgCheck[0])].FindControl("rptComponenteRegencia")))
                                            .Items[Convert.ToInt32(localizacaoImgCheck[1])].FindControl("imgJustificativaPosConselhoSituacao");
                }
                imgCheckSituacao.Visible = !String.IsNullOrEmpty(textoJustificativa);
                hdnLocalImgCheckSituacao.Value = String.Empty;
            }
        }

        #endregion Métodos

        #region Eventos Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
                if (cell != null)
                {
                    cell.BgColor = ApplicationWEB.AlunoInativo;
                }

                HtmlTableCell cellObrigatorio = tbLegenda.Rows[1].Cells[0];
                if (cellObrigatorio != null)
                {
                    cellObrigatorio.BgColor = "FF3030";
                }

                HtmlTableCell cellNaoAvaliado = lnAlunoNaoAvaliado.Cells[0];
                if (cellNaoAvaliado != null)
                {
                    cellNaoAvaliado.BgColor = ApplicationWEB.CorAlunoNaoAvaliado;
                }

                lnAlunoDispensado.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                HtmlTableCell cellDispensado = lnAlunoDispensado.Cells[0];
                if (cellDispensado != null)
                {
                    cellDispensado.BgColor = ApplicationWEB.AlunoDispensado;
                }

                HtmlTableCell cellAlunoFrequencia = lnAlunoFrequencia.Cells[0];
                if (cellAlunoFrequencia != null)
                {
                    cellAlunoFrequencia.BgColor = ApplicationWEB.AlunoFrequenciaLimite;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pnlAlunos.GroupingText = _UCEfetivacaoNotas.NomeModulo;
                if (IsPostBack)
                {
                    SetaEventosTxtFrequenciaAcumulada();
                }

                _UCComboOrdenacao1._OnSelectedIndexChange += ReCarregarGridAlunos;
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ACA_FormatoAvaliacao fav = VS_FormatoAvaliacao;

            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                string arredondamento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ARREDONDAMENTO_NOTA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
                string exibeCorMedia = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
                ACA_EscalaAvaliacaoNumerica escalaNum = new ACA_EscalaAvaliacaoNumerica { esa_id = fav.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoNumericaBO.GetEntity(escalaNum);

                string script = "var parametro = " + fav.fav_obrigatorioRelatorioReprovacao.ToString().ToLower() + ";" +
                    "var numeroCasasDecimais = " + RetornaNumeroCasasDecimais() + ";" +
                    "var periodoFechado = " + periodoFechado.ToString().ToLower() + ";" +
                    "var arredondamento = " + arredondamento.ToString().ToLower() + ";" +
                    "var variacaoEscala = '" + escalaNum.ean_variacao.ToString().Replace(',', '.') + "';" +
                    "var exibeCorMedia = " + (string.IsNullOrEmpty(exibeCorMedia) ? "false" : exibeCorMedia) + ";";

                if (Tud_id <= 0)
                {
                    // Seta variáveis de nota mínima por javascript, caso seja lançamento global, para
                    // ficar vermelha a linha caso a nota esteja baixa (obrigatório relatório).
                    script += "var notaMinima = parseFloat('" +
                                    VS_NotaMinima.ToString().Replace(',', '.') + "');" +
                                    "var parecerMinimo = " + VS_ParecerMinimo + ";";
                }

                if (sm.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "notaMinima", script, true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "notaMinima", script, true);
                }

                if (CalcularResultadoAutomatico)
                {
                    // Se tem avaliação selecionada, se a coluna resultado está visível,
                    // e se não é permitido alterar o resultado (a coluna fica desabilitada, e seleciona o resultado
                    // automaticamente).
                    string resultadoAutomatico =
                        String.Format(
                            @"var criterioAvaliacao = {0};
                        var notaMinimaAprovacao = {1};
                        var parecerMinimoAprovacao = {2};
                        var possuiAvaliacaoRecuperacaoFinal = {3};
                        var percentualMinimoFrequencia = {4};"
                            , VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal
                            , VS_NotaMinima
                            , VS_ParecerMinimo
                            , (_UCEfetivacaoNotas.VS_NomeAvaliacaoRecuperacaoFinal != "" && VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.RecuperacaoFinal).ToString().ToLower()
                            , VS_FormatoAvaliacao.percentualMinimoFrequencia.ToString().Replace(",", "."));

                    if (sm.IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "resultadoAutomatico", resultadoAutomatico, true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "resultadoAutomatico", resultadoAutomatico, true);
                    }
                }
            }

            SetaEventosTxtFrequenciaAcumulada();
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void gvAlunos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblTituloResultadoEfetivacao = (Label)e.Row.FindControl("lblTituloResultadoEfetivacao");

                if (lblTituloResultadoEfetivacao != null)
                    lblTituloResultadoEfetivacao.Text = GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO").ToString();

                ImageButton btnTodasFrequencias = (ImageButton)e.Row.FindControl("btnTodasFrequencias");
                if ((btnTodasFrequencias != null) && (tipo == EscalaAvaliacaoTipo.Numerica) && !HabilitarLancamentosAnoLetivoAnterior)
                {
                    btnTodasFrequencias.ToolTip = "Atualizar frequência e nota de todos os alunos";
                }

                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
                string nomeNota = "Nota";

                if ((_UCEfetivacaoNotas.VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
                {
                    nomeNota = "Média";
                }
                else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                {
                    nomeNota = "Conceito";
                }
                else if (tipoEscala == EscalaAvaliacaoTipo.Relatorios)
                {
                    nomeNota = "Relatório";
                }

                ((Literal)e.Row.FindControl("litNotaRegencia")).Text = nomeNota;
                ((LinkButton)e.Row.FindControl("btnExpandir")).ToolTip = "Expandir para todos os alunos";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBE_IDADE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                    !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "pes_dataNascimento").ToString()))
                {
                    Label lblIdade = (Label)e.Row.FindControl("lblIdade");
                    if (lblIdade != null)
                        lblIdade.Text = GestaoEscolarUtilBO.DiferencaDataExtenso(Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "pes_dataNascimento").ToString()), DateTime.Today);
                }

                if (gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                {
                    decimal frequenciaAcumulada;
                    TextBox txtFrequenciaAcumulada = (TextBox)e.Row.FindControl("txtFrequenciaAcumulada");

                    if (Decimal.TryParse((DataBinder.Eval(e.Row.DataItem, "frequenciaAcumulada") ?? string.Empty).ToString(), out frequenciaAcumulada) &&
                        frequenciaAcumulada > 0)
                    {
                        // Seta o valor que veio do banco de dados (a efetivação já foi salva uma vez).
                        txtFrequenciaAcumulada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, frequenciaAcumulada);
                    }
                }

                ImageButton btnFrequencia = (ImageButton)e.Row.FindControl("btnFrequencia");
                if ((btnFrequencia != null) && (tipo == EscalaAvaliacaoTipo.Numerica) && !HabilitarLancamentosAnoLetivoAnterior)
                {
                    btnFrequencia.ToolTip = "Atualizar frequência e nota do aluno";
                }

                bool observacaoPreenchida = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "observacaoPreenchida"));
                bool observacaoConselhoPreenchida = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "observacaoConselhoPreenchida"));

                TextBox txtQtdeAula = (TextBox)e.Row.FindControl("txtQtdeAula");

                ImageButton btnObservacao = (ImageButton)e.Row.FindControl("btnObservacao");
                Image imgObservacaoSituacao = (Image)e.Row.FindControl("imgObservacaoSituacao");

                ImageButton btnObservacaoConselho = (ImageButton)e.Row.FindControl("btnObservacaoConselho");
                Image imgObservacaoConselhoSituacao = (Image)e.Row.FindControl("imgObservacaoConselhoSituacao");

                ImageButton btnJustificativaPosConselho = (ImageButton)e.Row.FindControl("btnJustificativaPosConselho");

                ImageButton btnBoletim = (ImageButton)e.Row.FindControl("btnBoletim");
                if (btnBoletim != null)
                {
                    btnBoletim.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (btnJustificativaPosConselho != null)
                {
                    btnJustificativaPosConselho.CommandArgument = gvAlunos.DataKeys[e.Row.RowIndex]["tur_id"] + ";" +
                                                                  gvAlunos.DataKeys[e.Row.RowIndex]["tud_id"] + ";" +
                                                                  gvAlunos.DataKeys[e.Row.RowIndex]["alu_id"] + ";" +
                                                                  gvAlunos.DataKeys[e.Row.RowIndex]["mtu_id"] + ";" +
                                                                  gvAlunos.DataKeys[e.Row.RowIndex]["mtd_id"] + ";" +
                                                                  (gvAlunos.DataKeys[e.Row.RowIndex]["AvaliacaoID"] != DBNull.Value ?
                                                                   gvAlunos.DataKeys[e.Row.RowIndex]["AvaliacaoID"] : "-1");

                    btnJustificativaPosConselho.ToolTip = "Infomar justificativa de " + gvAlunos.Columns[colunaNotaPosConselho].HeaderText.ToLower();

                    Image imgJustificativaPosConselhoSituacao = (Image)e.Row.FindControl("imgJustificativaPosConselhoSituacao");
                    if (imgJustificativaPosConselhoSituacao != null)
                    {
                        imgJustificativaPosConselhoSituacao.Visible = !String.IsNullOrEmpty(VS_JustificativaPosConselho.FirstOrDefault(p => p.Id == btnJustificativaPosConselho.CommandArgument).Valor);
                    }
                }

                if (btnObservacao != null)
                {
                    btnObservacao.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (imgObservacaoSituacao != null)
                {
                    imgObservacaoSituacao.Visible = observacaoPreenchida;
                }

                if (btnObservacaoConselho != null)
                {
                    btnObservacaoConselho.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (imgObservacaoConselhoSituacao != null && (AvaliacaoTipo)VS_Avaliacao.ava_tipo != AvaliacaoTipo.RecuperacaoFinal)
                {
                    byte resultado = DataBinder.Eval(e.Row.DataItem, "mtu_resultado") == DBNull.Value ? (byte)0 : Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "mtu_resultado").ToString());
                    imgObservacaoConselhoSituacao.Visible = observacaoConselhoPreenchida || (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && avaliacaoUltimoPerido && resultado > 0);
                }

                bool exibeCampoNotaAluno = true;
                bool exibeCampoFrequencia = true;

                // Se a avaliação for do tipo "Recuperação Final"
                // esconde os campos de notas, caso o aluno não esteja de recuperação por nota
                if ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.RecuperacaoFinal)
                {
                    exibeCampoNotaAluno = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "recuperacaoPorNota"));
                }

                if (txtQtdeAula != null)
                {
                    txtQtdeAula.Visible = exibeCampoFrequencia;
                }

                CustomValidator cvQtFaltas = (CustomValidator)e.Row.FindControl("cvQtFaltas");

                if (cvQtFaltas != null)
                {
                    switch (VS_FormatoAvaliacao.fav_tipoApuracaoFrequencia)
                    {
                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula:
                            cvQtFaltas.ErrorMessage = "A quantidade de faltas deve ser menor ou igual à quantidade de tempos de aula.";
                            break;

                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia:
                            cvQtFaltas.ErrorMessage = "A quantidade de faltas deve ser menor ou igual à quantidade de dias de aulas.";
                            break;
                    }
                }

                ImageButton btnFaltasExternas = (ImageButton)e.Row.FindControl("btnFaltasExternas");
                if (btnFaltasExternas != null && listaFrequenciaExterna != null && listaFrequenciaExterna.Count > 0)
                {
                    long alu_id = Convert.ToInt64(gvAlunos.DataKeys[e.Row.RowIndex]["alu_id"]);
                    int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[e.Row.RowIndex]["mtu_id"]);
                    int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[e.Row.RowIndex]["mtd_id"]);

                    CLS_AlunoFrequenciaExterna ext = listaFrequenciaExterna.Find(p => p.alu_id == alu_id && p.mtu_id == mtu_id && p.mtd_id == mtd_id);
                    if (ext != null)
                    {

                    }
                }
            }
        }

        protected void gvAlunos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Relatorio")
            {
                Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                string pes_nome = ((Label)(rowControl.FindControl("lblNomeAluno"))).Text;
                hdnLocalImgCheckSituacao.Value = ((GridViewRow)rowControl).RowIndex.ToString();
                TrataEventoCommandRelatorio(e.CommandArgument.ToString(), pes_nome);
            }
            else if (e.CommandName == "ObservacaoDisciplina")
            {
                try
                {
                    if (this.AbrirObservacaoDisciplina != null)
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        long tud_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["tud_id"].ToString());
                        long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"].ToString());
                        int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"].ToString());
                        int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtd_id"] != DBNull.Value ? gvAlunos.DataKeys[index].Values["mtd_id"] : "-1");

                        string dadosAluno = "<b>Nome do aluno:</b> " + gvAlunos.DataKeys[index].Values["pes_nome"].ToString();
                        if (gvAlunos.DataKeys[index].Values["alc_matricula"] != null)
                        {
                            dadosAluno += "<br /><b>" + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ":</b>" + gvAlunos.DataKeys[index].Values["alc_matricula"];
                        }
                        if (gvAlunos.DataKeys[index].Values["tur_codigo"] != null)
                        {
                            dadosAluno += "<br /><b>Turma:</b> " + gvAlunos.DataKeys[index].Values["tur_codigo"];
                        }
                        if (gvAlunos.DataKeys[index].Values["mtd_numeroChamada"] != null)
                        {
                            dadosAluno += "<br /><b>Número de chamada:</b> " + gvAlunos.DataKeys[index].Values["mtd_numeroChamada"];
                        }

                        AbrirObservacaoDisciplina(index, tud_id, alu_id, mtu_id, mtd_id, dadosAluno, "Registro do professor - " + VS_Avaliacao.ava_nome, periodoFechado);
                    }
                }
                catch (Exception ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a observação do aluno.", UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }
            else if (e.CommandName == "ObservacaoConselho")
            {
                try
                {
                    if (this.AbrirObservacaoConselho != null)
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"].ToString());
                        int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"].ToString());
                        long tur_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["tur_id"].ToString());

                        string dadosAluno = "<b>Nome do aluno:</b> " + gvAlunos.DataKeys[index].Values["pes_nome"].ToString();
                        if (gvAlunos.DataKeys[index].Values["alc_matricula"] != null)
                        {
                            dadosAluno += "<br /><b>" + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ":</b>" + gvAlunos.DataKeys[index].Values["alc_matricula"];
                        }
                        if (gvAlunos.DataKeys[index].Values["tur_codigo"] != null)
                        {
                            dadosAluno += "<br /><b>Turma:</b> " + gvAlunos.DataKeys[index].Values["tur_codigo"];
                        }
                        if (gvAlunos.DataKeys[index].Values["mtd_numeroChamada"] != null)
                        {
                            dadosAluno += "<br /><b>Número de chamada:</b> " + gvAlunos.DataKeys[index].Values["mtd_numeroChamada"];
                        }

                        AbrirObservacaoConselho(index, tur_id, alu_id, mtu_id, dadosAluno
                                                , GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.RegistroConselho")
                                                    + " - " + VS_Avaliacao.ava_nome
                                                    , VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica
                                                    , (AvaliacaoTipo)VS_Avaliacao.ava_tipo
                                                    , VS_Turma.cal_id, VS_Avaliacao.tpc_id, _UCEfetivacaoNotas.VS_EfetivacaoSemestral, periodoFechado);
                    }
                }
                catch (Exception ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a observação do aluno.", UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }
            else if (e.CommandName == "JustificativaPosConselho")
            {
                try
                {
                    Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                    string pes_nome = ((Label)(rowControl.FindControl("lblNomeAluno"))).Text;
                    hdnLocalImgCheckSituacao.Value = ((GridViewRow)rowControl).RowIndex.ToString();
                    string id = e.CommandArgument.ToString();
                    UCEfetivacaoNotas.Justificativa justificativa = VS_JustificativaPosConselho.Find(p => p.Id == id);
                    AbrirJustificativa(id, justificativa.Valor, pes_nome, null);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a justificativa de nota pós-conselho do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Boletim")
            {
                try
                {
                    if (AbrirBoletim != null)
                    {
                        int index = int.Parse(e.CommandArgument.ToString());
                        long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"]);
                        int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"]);
                        AbrirBoletim(alu_id, VS_tpc_id, mtu_id);
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o boletim completo do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void chkSemProfessor_CheckedChanged(object sender, EventArgs e)
        {
            chkNaoAvaliado.Enabled = !chkSemProfessor.Checked;
            SetaColunasVisiveisGrid(VS_Avaliacao);
        }

        protected void chkNaoAvaliado_CheckedChanged(object sender, EventArgs e)
        {
            chkSemProfessor.Enabled = !chkNaoAvaliado.Checked;
            SetaColunasVisiveisGrid(VS_Avaliacao);
        }

        public string btnImportarAnotacoesAluno_Click(long alu_id)
        {
            string tpc_ids = VS_Avaliacao.tpc_id.ToString();

            if (!(VS_Avaliacao.tpc_id > 0) && (VS_Avaliacao.ava_tipo == (byte)AvaliacaoTipo.Recuperacao))
            {
                tpc_ids = ACA_AvaliacaoRelacionadaBO.RetornaPeriodoCalendarioRelacionadosPorAvaliacao(_VS_fav_id, _VS_ava_id);
            }

            string anotacao = string.Empty;
            //string anotacaotxt = string.Empty;

            DataTable dt = CLS_TurmaAulaBO.SelecionaAnotacaoPorAlunoPeriodoCalendario(alu_id, tpc_ids);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                anotacao = anotacao + dt.Rows[i]["taa_disciplinaData"] + " " + dt.Rows[i]["taa_anotacao"] + "\r\n";
            }

            return anotacao;
        }

        protected void btnFrequencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Recupera o index da linha do grid que disparou o evento.
                ImageButton btnFrequencia = (ImageButton)sender;
                GridViewRow row = (GridViewRow)btnFrequencia.NamingContainer;

                if (HabilitarLancamentosAnoLetivoAnterior)
                {
                    long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);
                    int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex]["mtu_id"]);

                    if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
                    {
                        CalculaFrequenciaAjustada(row, alu_id, mtu_id);
                    }

                    if (gvAlunos.Columns[colunaFrequenciaAcumulada].Visible)
                    {
                        CalculaFrequenciaAcumulada(row, alu_id, mtu_id);
                    }
                }
                else
                {
                    _VS_IndiceBotaoAtualizaFrequencia = row.RowIndex;

                    EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
                    EscalaAvaliacaoTipo tipoAdicional = (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo;

                    // Permite atualização de nota se a nota (da disciplina) for numérica ou por permitir salvar
                    // nota final no lançamento de avaliações.
                    bool atualizaNota = (gvAlunos.Columns[colunaNota].Visible || (gvAlunos.Columns[colunaNotaRegencia].Visible
                                                                                    && ((HtmlTableRow)row.FindControl("rptComponenteRegencia").Controls[0].FindControl("tr0")).Cells[colunaComponenteRegenciaNota].Visible)
                                        )
                                        && (Tud_id > 0 || VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal)
                                        && (tipo == EscalaAvaliacaoTipo.Numerica
                                                || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                    // Permite atualização de nota se tiver nota adicional numérica e tiver a disciplina principal.
                    bool atualizaNotaAdicional = gvAlunos.Columns[colunaNotaAdicional].Visible && Tud_idPrincipal > 0
                        && tipoAdicional == EscalaAvaliacaoTipo.Numerica;

                    // Permite atualização da frequência caso a coluna de faltas esteja visível.
                    bool atualizaFrequencia = gvAlunos.Columns[colunaFaltas].Visible;

                    // Permite atualização a quantidade de auldas caso a coluna esteja visível.
                    bool atualizaQtdAulas = gvAlunos.Columns[colunaAulas].Visible;

                    // verifico se é conceito global
                    bool atualizaNotaConceitoGlobal = gvAlunos.Columns[colunaNota].Visible && Tud_id <= 0;

                    // Exibe a popup de atualizar a frequência, vai exibir se tiver nota e frequência disponível para
                    // atualização.
                    if ((atualizaNota || atualizaNotaAdicional) && atualizaFrequencia
                        &&
                        VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.Final
                        &&
                        // teste abaixo foi incluído para evitar que o popup seja aberto qdo existir para atualizar nota ou falta,
                        // ou seja para abrir deverá ter os dois.
                        (!((atualizaNotaConceitoGlobal && (atualizaNota || atualizaNotaAdicional) && atualizaFrequencia))
                        ||
                        ((atualizaNota || atualizaNotaAdicional) && !atualizaFrequencia)
                        ||
                        (atualizaFrequencia && !atualizaNota && !atualizaNotaAdicional)))
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaAtualizacao", "$(document).ready(function(){ $('#divConfirmaAtualizacao').dialog('open'); });", true);
                    }
                    else
                    {
                        string sAtualizar = "";

                        if (atualizaFrequencia || atualizaQtdAulas)
                            sAtualizar = "Faltas";

                        if ((atualizaNota || atualizaNotaAdicional) && (atualizaFrequencia || atualizaQtdAulas))
                        {
                            sAtualizar = "Notas_e_Faltas";
                        }
                        else if (atualizaNota || atualizaNotaAdicional)
                        {
                            sAtualizar = "Notas";
                        }

                        AtualizaFrequenciaMedia_LinhaAluno(row.RowIndex, sAtualizar);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar os dados do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnTodasFrequencias_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (HabilitarLancamentosAnoLetivoAnterior)
                {
                    bool calculaFrequenciaAcumulada = gvAlunos.Columns[colunaFrequenciaAcumulada].Visible;
                    bool calculaFrequenciaAjustada = gvAlunos.Columns[colunaFrequenciaAjustada].Visible;

                    foreach (GridViewRow row in gvAlunos.Rows)
                    {
                        long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex]["alu_id"]);
                        int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex]["mtu_id"]);

                        if (calculaFrequenciaAcumulada)
                        {
                            CalculaFrequenciaAcumulada(row, alu_id, mtu_id);
                        }

                        if (calculaFrequenciaAjustada)
                        {
                            CalculaFrequenciaAjustada(row, alu_id, mtu_id);
                        }
                    }
                }
                else
                {
                    _VS_IndiceBotaoAtualizaFrequencia = -1;
                    EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
                    EscalaAvaliacaoTipo tipoAdicional = (EscalaAvaliacaoTipo)VS_EscalaAvaliacaoAdicional.esa_tipo;

                    // Permite atualização de nota se a nota (da disciplina) for numérica ou por permitir salvar
                    // nota final no lançamento de avaliações.
                    bool atualizaNota = (gvAlunos.Columns[colunaNota].Visible || (gvAlunos.Columns[colunaNotaRegencia].Visible
                                                                                    && gvAlunos.Rows.Count > 0
                                                                                    && ((HtmlTableRow)gvAlunos.Rows[0].FindControl("rptComponenteRegencia").Controls[0].FindControl("tr0")).Cells[colunaComponenteRegenciaNota].Visible)
                                        )
                                        && (Tud_id > 0 || VS_FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal)
                                        && (tipo == EscalaAvaliacaoTipo.Numerica
                                                || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                    // Permite atualização de nota se tiver nota adicional numérica e tiver a disciplina principal.
                    bool atualizaNotaAdicional = gvAlunos.Columns[colunaNotaAdicional].Visible && Tud_idPrincipal > 0
                        && tipoAdicional == EscalaAvaliacaoTipo.Numerica;

                    // Permite atualização da frequência caso a coluna de faltas esteja visível.
                    bool atualizaFrequencia = gvAlunos.Columns[colunaFaltas].Visible;

                    // Permite atualização a quantidade de auldas caso a coluna esteja visível.
                    bool atualizaQtdAulas = gvAlunos.Columns[colunaAulas].Visible;

                    // Exibe a popup de atualizar a frequência, vai exibir se tiver nota e frequência disponível para
                    // atualização.
                    if ((atualizaNota || atualizaNotaAdicional) && atualizaFrequencia
                        && VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.Final)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaAtualizacao", "$(document).ready(function(){ $('#divConfirmaAtualizacao').dialog('open'); });", true);
                    }
                    else
                    {
                        string sAtualizar = "";

                        if (atualizaFrequencia || atualizaQtdAulas)
                            sAtualizar = "Faltas";

                        if ((atualizaNota || atualizaNotaAdicional) && (atualizaFrequencia || atualizaQtdAulas))
                        {
                            sAtualizar = "Notas_e_Faltas";
                        }
                        else if (atualizaNota || atualizaNotaAdicional)
                        {
                            sAtualizar = "Notas";
                        }

                        AtualizaFrequenciaMedia_TodosAlunos(sAtualizar);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar os dados do aluno.", UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void btnAtNotas_Click(object sender, EventArgs e)
        {
            try
            {
                if (_VS_IndiceBotaoAtualizaFrequencia == -1)
                {
                    AtualizaFrequenciaMedia_TodosAlunos("Notas");
                }
                else
                {
                    AtualizaFrequenciaMedia_LinhaAluno(_VS_IndiceBotaoAtualizaFrequencia, "Notas");
                }

                uppGridAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atualizar a nota do aluno.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ConfirmaAtualizacao", "$(document).ready(function(){ $('#divConfirmaAtualizacao').dialog('close'); });", true);
            }
        }

        protected void btnAtFaltas_Click(object sender, EventArgs e)
        {
            try
            {
                if (_VS_IndiceBotaoAtualizaFrequencia == -1)
                {
                    AtualizaFrequenciaMedia_TodosAlunos("Faltas");
                }
                else
                {
                    AtualizaFrequenciaMedia_LinhaAluno(_VS_IndiceBotaoAtualizaFrequencia, "Faltas");
                }

                uppGridAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atualizar a frequência do aluno.", UtilBO.TipoMensagem.Alerta);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ConfirmaAtualizacao", "$(document).ready(function(){ $('#divConfirmaAtualizacao').dialog('close'); });", true);
            }
        }

        protected void btnAtNotaseFaltas_Click(object sender, EventArgs e)
        {
            try
            {
                if (_VS_IndiceBotaoAtualizaFrequencia == -1)
                {
                    AtualizaFrequenciaMedia_TodosAlunos("Notas_e_Faltas");
                }
                else
                {
                    AtualizaFrequenciaMedia_LinhaAluno(_VS_IndiceBotaoAtualizaFrequencia, "Notas_e_Faltas");
                }

                uppGridAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Não foi possível atualizar frequência e nota do aluno.", UtilBO.TipoMensagem.Alerta);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ConfirmaAtualizacao", "$(document).ready(function(){ $('#divConfirmaAtualizacao').dialog('close'); });", true);
            }
        }

        public void ddlTurmaDisciplina_SelectedIndexChanged(int countAvaliacoes)
        {
            VS_BuscouTiposResultados = false;

            bool permiteConsultar = true;
            if (!_UCEfetivacaoNotas.TurmaDisciplina_Ids[0].Equals("-1") && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                permiteConsultar = VS_turmaDisciplinaCompartilhada != null || _UCEfetivacaoNotas.VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoConsulta);
            }

            // ReCarrega o combo de avaliações.
            if (countAvaliacoes > 1)
            {
                if (permiteConsultar)
                    MostraTelaAlunos();
                else
                    EscondeTelaAlunos("Docente não possui permissão para consultar notas do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionado(a).");
            }
            else
            {
                EscondeTelaAlunos("Não foram encontradas avaliações para o(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionado(a) ou está fora do período de lançamento.");
            }
        }

        public void UCComboAvaliacao1_IndexChanged()
        {
            try
            {
                if (_VS_ava_id > 0)
                {
                    LoadFromEntity();

                    bool permiteConsultar = true;
                    if (!_UCEfetivacaoNotas.TurmaDisciplina_Ids[0].Equals("-1") && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        permiteConsultar = VS_turmaDisciplinaCompartilhada != null || _UCEfetivacaoNotas.VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoConsulta);
                    }

                    if (permiteConsultar)
                        MostraTelaAlunos();
                    else
                        EscondeTelaAlunos("Docente não possui permissão para consultar notas do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionado(a).");
                }
                else
                {
                    EscondeTelaAlunos("É necessário selecionar uma avaliação.");
                }
            }
            catch (Exception ex)
            {
                TrataErro(ex, _lblMessage, "carregar os dados");
            }
        }

        protected void cvQtFaltas_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomValidator cvValidar = (CustomValidator)source;

                GridViewRow row = (GridViewRow)cvValidar.NamingContainer;

                TextBox txtComparar = (TextBox)row.FindControl("txtQtdeAula");

                int qtAulas = 0;
                int qtFaltas;

                string s = string.IsNullOrEmpty(args.Value) ? "0" : args.Value;

                if (Int32.TryParse(s, out qtFaltas))
                {
                    if (!string.IsNullOrEmpty(txtComparar.Text))
                    {
                        qtAulas = Convert.ToInt32(txtComparar.Text);
                    }

                    args.IsValid = !gvAlunos.Columns[colunaFaltas].Visible || !gvAlunos.Columns[colunaAulas].Visible || (qtFaltas <= qtAulas);
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvRelatorioDesempenho_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = true;

                if (_UCEfetivacaoNotas.VS_turma_Peja)
                {
                    CustomValidator cvRelatorioDesempenho = (CustomValidator)source;
                    cvRelatorioDesempenho.Style.Remove(HtmlTextWriterStyle.Color);
                    cvRelatorioDesempenho.Style.Add(HtmlTextWriterStyle.Color, "white");

                    Control row = (Control)cvRelatorioDesempenho.NamingContainer;

                    TextBox txtNota = (TextBox)row.FindControl("txtNota");
                    DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                    ImageButton btnRelatorio = (ImageButton)row.FindControl("btnRelatorio");

                    bool notaBaixa = false;

                    if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    {
                        // Verificar se a nota está abaixo da mínima e é obrigatório lançar relatório.
                        double nota;
                        if (double.TryParse(txtNota.Text, out nota))
                        {
                            notaBaixa = nota < VS_NotaMinima && !string.IsNullOrEmpty(txtNota.Text);
                        }
                    }
                    else if (VS_EscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    {
                        // Verificar se o parecer está abaixo do mínimo e é obrigatório lançar relatório.
                        int ordem = Convert.ToInt32(ddlPareceres.SelectedValue.Split(';')[1]);
                        notaBaixa = ordem < VS_ParecerMinimo && ordem != -1;
                    }

                    // Se for obrigatório lançar relatório.
                    if (btnRelatorio != null && btnRelatorio.Visible && notaBaixa)
                    {
                        string id = btnRelatorio.CommandArgument;
                        UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == id);

                        if (string.IsNullOrEmpty(nota.Valor))
                        {
                            args.IsValid = false;
                            Label lblNome = (Label)row.FindControl("lblNomeAluno");
                            cvRelatorioDesempenho.ErrorMessage =
                                "O relatório de desempenho é obrigatório para o aluno " + lblNome.Text + ".";
                        }
                    }
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvNotaMaxima_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomValidator cvValidar = (CustomValidator)source;
                TextBox txtValidar = (TextBox)cvValidar.NamingContainer.FindControl(cvValidar.ControlToValidate);

                double nota;
                if (Double.TryParse(txtValidar.Text, out nota))
                {
                    args.IsValid = nota <= VS_NotaMinima;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvParecerMaximo_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomValidator cvValidar = (CustomValidator)source;
                DropDownList ddlPareceresValidar = (DropDownList)cvValidar.NamingContainer.FindControl(cvValidar.ControlToValidate);

                int ordemSelecionada = Convert.ToInt32(ddlPareceresValidar.SelectedItem.Value.Split(';')[1]);

                args.IsValid = ordemSelecionada <= VS_ParecerMinimo;
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvFrequencia_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                double frequencia;

                string s = string.IsNullOrEmpty(args.Value) ? "0" : args.Value;

                if (Double.TryParse(s, out frequencia))
                {
                    args.IsValid = frequencia >= 0;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvQtAusenciasCompensadas_Validar(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomValidator cvValidar = (CustomValidator)source;

                GridViewRow row = (GridViewRow)cvValidar.NamingContainer;

                TextBox txtComparar = (TextBox)row.FindControl("txtQtdeFalta");
                HiddenField hdnQtFaltasAnteriores = (HiddenField)row.FindControl("hdnQtFaltasAnteriores");
                HiddenField hdnQtcompensadasAnteriores = (HiddenField)row.FindControl("hdnQtcompensadasAnteriores");

                int qtFaltas = 0;
                int qtAusenciasCompensadas;
                int qtFaltasAnteriores = 0;
                int qtcompensadasAnteriores = 0;

                string s = string.IsNullOrEmpty(args.Value) ? "0" : args.Value;

                if (Int32.TryParse(s, out qtAusenciasCompensadas))
                {
                    if (!string.IsNullOrEmpty(txtComparar.Text))
                    {
                        qtFaltas = Convert.ToInt32(txtComparar.Text);
                    }

                    s = string.IsNullOrEmpty(hdnQtFaltasAnteriores.Value) ? "0" : hdnQtFaltasAnteriores.Value;

                    if (Int32.TryParse(s, out qtFaltasAnteriores))
                    {
                        qtFaltas += qtFaltasAnteriores;
                    }

                    s = string.IsNullOrEmpty(hdnQtcompensadasAnteriores.Value) ? "0" : hdnQtcompensadasAnteriores.Value;

                    if (Int32.TryParse(s, out qtcompensadasAnteriores))
                    {
                        qtAusenciasCompensadas += qtcompensadasAnteriores;
                    }

                    args.IsValid = !gvAlunos.Columns[colunaAusenciasCompensadas].Visible || (qtAusenciasCompensadas <= qtFaltas);
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        public void UCAlunoEfetivacaoObservacao_ReturnValues(Int32 indiceAluno, object observacao, eTipoObservacao tipoObservacao, byte resultado)
        {
            try
            {
                GridViewRow row = gvAlunos.Rows[indiceAluno];

                if (tipoObservacao == eTipoObservacao.Disciplina)
                {
                    CLS_AlunoAvaliacaoTurDis_Observacao ucObservacaoRetorno = (CLS_AlunoAvaliacaoTurDis_Observacao)observacao;
                    SetaComponenteObservacaoLinhaGrid(row, ucObservacaoRetorno);
                }
                else
                {
                    CLS_AlunoAvaliacaoTur_Observacao ucObservacaoRetorno = (CLS_AlunoAvaliacaoTur_Observacao)observacao;
                    SetaComponenteObservacaoConselhoLinhaGrid(row, ucObservacaoRetorno, resultado);
                }

                uppGridAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptComponenteRegencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                //
                // CABECALHO COLUNAS
                //
                Literal litHead = (Literal)e.Item.FindControl("litHeadNota");
                litHead.Text = gvAlunos.Columns[colunaNota].HeaderText;
                litHead = (Literal)e.Item.FindControl("litHeadNotaPosConselho");
                litHead.Text = gvAlunos.Columns[colunaNotaPosConselho].HeaderText;

                ((Literal)e.Item.FindControl("litHeadNomeDisciplina")).Text =
                    GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text").ToString();


                //
                // VISIBILIDADE COLUNAS
                //
                HtmlTableRow trHeaderComponente = (HtmlTableRow)e.Item.FindControl("tr0");
                //bool tud_naoLancarNota = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tud_naoLancarNota"));
                trHeaderComponente.Cells[colunaComponenteRegenciaNota].Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota];
                trHeaderComponente.Cells[colunaComponenteRegenciaNotaPosConselho].Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaPosConselho];
            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem)
                        || (e.Item.ItemType == ListItemType.Item))
            {
                //
                // VISIBILIDADE COLUNAS
                //
                HtmlTableRow trComponente = (HtmlTableRow)e.Item.FindControl("tr1");
                trComponente.Cells[colunaComponenteRegenciaNota].Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota];
                trComponente.Cells[colunaComponenteRegenciaNotaPosConselho].Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaPosConselho];

                //
                // BOTAO RELATORIO NOTA
                //
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                bool mostraBotaoRelatorio = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Relatorios;
                string alunoId = DataBinder.Eval(e.Item.DataItem, "alu_id").ToString();
                string matriculaTurmaId = DataBinder.Eval(e.Item.DataItem, "mtu_id").ToString();
                string commandArgument = DataBinder.Eval(e.Item.DataItem, "tur_id").ToString() + ";" +
                                            DataBinder.Eval(e.Item.DataItem, "tud_id").ToString() + ";" +
                                            alunoId + ";" +
                                            matriculaTurmaId + ";" +
                                            DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString() + ";" +
                                            (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString()) ?
                                                DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString() : "-1");
                HiddenField hfDataKeys = (HiddenField)e.Item.FindControl("hfDataKeys");
                hfDataKeys.Value = DataBinder.Eval(e.Item.DataItem, "tud_id").ToString() + ";" +
                                DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString() + ";" +
                                DataBinder.Eval(e.Item.DataItem, "mtu_idAnterior").ToString() + ";" +
                                DataBinder.Eval(e.Item.DataItem, "mtd_idAnterior").ToString() + ";" +
                                DataBinder.Eval(e.Item.DataItem, "dispensadisciplina").ToString() + ";" +
                                (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString()) ?
                                    DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString() : "-1");

                CustomValidator cvRelatorioDesempenho = (CustomValidator)e.Item.FindControl("cvRelatorioDesempenho");
                if (cvRelatorioDesempenho != null)
                {
                    // Se for turma do PEJA, e estiver configurado pra exibir o relatório, ele deve ser validado na hora de salvar.
                    cvRelatorioDesempenho.Visible = _UCEfetivacaoNotas.VS_turma_Peja && mostraBotaoRelatorio;
                }

                // Deixar com display na tela para poder acessar por javascript.
                if (btnRelatorio != null)
                {
                    btnRelatorio.Visible = mostraBotaoRelatorio;
                    btnRelatorio.CommandArgument = commandArgument;
                    btnRelatorio.ToolTip = mostraBotaoRelatorio ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                    // Pesquisa o item pelo id.
                    UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
                    AtualizaIconesStatusPreenchimentoRelatorio(e.Item, false, mostraBotaoRelatorio, nota.Valor, nota.arq_idRelatorio);
                }

                //
                // BOTAO JUSTIFICATIVA NOTA POS-CONSELHO
                //
                ImageButton btnJustificativaPosConselho = (ImageButton)e.Item.FindControl("btnJustificativaPosConselho");
                if (btnJustificativaPosConselho != null)
                {
                    btnJustificativaPosConselho.CommandArgument = commandArgument;
                    btnJustificativaPosConselho.ToolTip = "Infomar justificativa de " + gvAlunos.Columns[colunaNotaPosConselho].HeaderText.ToLower();

                    Image imgJustificativaPosConselhoSituacao = (Image)e.Item.FindControl("imgJustificativaPosConselhoSituacao");
                    if (imgJustificativaPosConselhoSituacao != null)
                    {
                        imgJustificativaPosConselhoSituacao.Visible = !String.IsNullOrEmpty(VS_JustificativaPosConselho.FirstOrDefault(p => p.Id == btnJustificativaPosConselho.CommandArgument).Valor);
                    }
                }

                //
                // BOTAO RELATORIO NOTA POS-CONSELHO
                //
                ImageButton btnRelatorioPosConselho = (ImageButton)e.Item.FindControl("btnRelatorioPosConselho");
                CustomValidator cvRelatorioDesempenhoPosConselho = (CustomValidator)e.Item.FindControl("cvRelatorioDesempenhoPosConselho");
                if (cvRelatorioDesempenhoPosConselho != null)
                {
                    // Se for turma do PEJA, e estiver configurado pra exibir o relatório, ele deve ser validado na hora de salvar.
                    cvRelatorioDesempenhoPosConselho.Visible = _UCEfetivacaoNotas.VS_turma_Peja && mostraBotaoRelatorio;
                }

                // Deixar com display na tela para poder acessar por javascript.
                if (btnRelatorioPosConselho != null)
                {
                    btnRelatorioPosConselho.Visible = mostraBotaoRelatorio;
                    btnRelatorioPosConselho.CommandArgument = commandArgument;
                    btnRelatorioPosConselho.ToolTip = mostraBotaoRelatorio ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                    // Pesquisa o item pelo id.
                    UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == btnRelatorioPosConselho.CommandArgument);
                    AtualizaIconesStatusPreenchimentoRelatorio(e.Item, true, mostraBotaoRelatorio, nota.Valor, nota.arq_idRelatorio);
                }
            }
        }

        protected void rptComponenteRegencia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Control rptItemControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
            Control rowControl = rptItemControl.Parent.Parent.Parent;
            string pes_nome = ((Label)(rowControl.FindControl("lblNomeAluno"))).Text;
            string dis_nome = ((Label)(rptItemControl.FindControl("lblNomeDisciplina"))).Text;
            hdnLocalImgCheckSituacao.Value = ((GridViewRow)rowControl).RowIndex + "," + e.Item.ItemIndex;

            if (e.CommandName == "Relatorio")
            {
                TrataEventoCommandRelatorio(e.CommandArgument.ToString(), pes_nome, dis_nome);
            }
            else if (e.CommandName == "JustificativaPosConselho")
            {
                try
                {
                    string id = e.CommandArgument.ToString();
                    UCEfetivacaoNotas.Justificativa justificativa = VS_JustificativaPosConselho.Find(p => p.Id == id);
                    AbrirJustificativa(id, justificativa.Valor, pes_nome, dis_nome);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a justificativa de nota pós-conselho do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos
    }
}