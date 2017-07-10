using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Fechamento
{
    public partial class UCFechamentoPadrao : MotherUserControl
    {
        #region Constantes

        private const int colunaStatus = 0;
        private const int colunaNota = 3;
        private const int colunaNotaPosConselho = 4;
        private const int colunaNotaRegencia = 5;
        private const int colunaFaltas = 6;
        private const int colunaAusenciasCompensadas = 7;
        private const int colunaFrequenciaAjustada = 8;
        private const int colunaObservacaoConselho = 9;
        private const int colunaResultado = 10;
        private const int colunaBoletim = 11;

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
                        string valorMinimo = (VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina);
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
                        string valorMinimo = VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina;

                        // Retorna a ordem do parecer da nota normal.
                        int i = RetornaOrdemParecer(valorMinimo);

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
        private long VS_tur_id
        {
            get
            {
                return _UCFechamento.VS_tur_id;
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
                    if (VS_tur_id > 0)
                    {
                        List<TUR_TurmaCurriculo> ltTurmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);
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
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        private long Tud_id
        {
            get
            {
                string[] ids = _UCFechamento.TurmaDisciplina_Ids;

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
        private int VS_fav_id
        {
            get
            {
                return _UCFechamento.VS_fav_id;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cap_dataInicio
        /// </summary>
        private DateTime VS_cap_dataInicio
        {
            get
            {
                if (ViewState["VS_cap_dataInicio"] != null)
                {
                    return Convert.ToDateTime(ViewState["VS_cap_dataInicio"]);
                }

                return new DateTime();
            }

            set
            {
                ViewState["VS_cap_dataInicio"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cap_dataFim
        /// </summary>
        private DateTime VS_cap_dataFim
        {
            get
            {
                if (ViewState["VS_cap_dataFim"] != null)
                {
                    return Convert.ToDateTime(ViewState["VS_cap_dataFim"]);
                }

                return new DateTime();
            }

            set
            {
                ViewState["VS_cap_dataFim"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_id
        /// </summary>
        private int VS_ava_id
        {
            get
            {
                return _UCFechamento.VS_ava_id;
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
        /// Guarda as notas de relatório.
        /// </summary>
        private List<UCFechamento.NotasRelatorio> VS_Nota_Relatorio
        {
            get
            {
                if (ViewState["VS_Nota_Relatorio"] != null)
                {
                    return (List<UCFechamento.NotasRelatorio>)ViewState["VS_Nota_Relatorio"];
                }

                return new List<UCFechamento.NotasRelatorio>();
            }

            set
            {
                ViewState["VS_Nota_Relatorio"] = value;
            }
        }

        /// <summary>
        /// Guarda os eventos cadastrados para a turma e calendário.
        /// </summary>
        private List<ACA_Evento> VS_ListaEventos
        {
            get
            {
                return _UCFechamento.VS_ListaEventos;
            }
        }

        private bool ExibeCompensacaoAusencia
        {
            get
            {
                if (ViewState["VS_Exibe_Compensacao_Ausencia"] == null)
                {
                    ViewState["VS_Exibe_Compensacao_Ausencia"] = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                        (
                            (VS_turmaDisciplinaCompartilhada != null && !EntTurmaDisciplina.tud_naoLancarFrequencia)
                            || (VS_turmaDisciplinaCompartilhada == null && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta))
                            || (__SessionWEB.__UsuarioWEB.Docente.doc_id <= 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                        );
                }

                return Convert.ToBoolean(ViewState["VS_Exibe_Compensacao_Ausencia"]);
            }
        }

        /// <summary>
        /// Entidade da turma selecionada no combo.
        /// </summary>
        private TUR_Turma VS_Turma
        {
            get
            {
                return _UCFechamento.VS_Turma;
            }
        }

        /// <summary>
        /// Entidade do calendário da turma selecionada no combo.
        /// </summary>
        private ACA_CalendarioAnual VS_CalendarioAnual
        {
            get
            {
                return _UCFechamento.VS_CalendarioAnual;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_FormatoAvaliacao VS_FormatoAvaliacao
        {
            get
            {
                return _UCFechamento.VS_FormatoAvaliacao;
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
                return _UCFechamento.VS_EscalaAvaliacao;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_EscalaAvaliacaoNumerica VS_EscalaNumerica
        {
            get
            {
                return _UCFechamento.VS_EscalaNumerica;
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do docente que está configurado no formato (esa_idDocente).
        /// </summary>
        private ACA_EscalaAvaliacao VS_EscalaAvaliacaoDocente
        {
            get
            {
                return _UCFechamento.VS_EscalaAvaliacaoDocente;
            }
        }

        /// <summary>
        /// Retorna a avaliação selecionada na tela de busca.
        /// </summary>
        private ACA_Avaliacao VS_Avaliacao
        {
            get
            {
                return _UCFechamento.VS_Avaliacao;
            }
        }

        /// <summary>
        /// Lista de IDs das turmas normais dos alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        public List<long> VS_listaTur_ids
        {
            get
            {
                return _UCFechamento.VS_listaTur_ids;
            }
        }

        /// <summary>
        /// Guarda as notas de relatório.
        /// </summary>
        private List<ACA_ConfiguracaoServicoPendencia> VS_ListConfiguracaoServicoPendencia
        {
            get
            {
                if (ViewState["VS_ListConfiguracaoServicoPendencia"] != null)
                {
                    return (List<ACA_ConfiguracaoServicoPendencia>)ViewState["VS_ListConfiguracaoServicoPendencia"];
                }

                return new List<ACA_ConfiguracaoServicoPendencia>();
            }

            set
            {
                ViewState["VS_ListConfiguracaoServicoPendencia"] = value;
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

        /// <summary>
        /// Lista referente aos alunos nas disciplinas componentes da Regencia
        /// </summary>
        private List<AlunosFechamentoPadraoComponenteRegencia> listaAlunosComponentesRegencia;

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
                switch (VS_ava_tipo)
                {
                    case (byte)AvaliacaoTipo.Periodica:
                    case (byte)AvaliacaoTipo.PeriodicaFinal:
                        return !VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoNotas && p.vigente);

                    case (byte)AvaliacaoTipo.Recuperacao:
                        if (VS_tpc_id > 0)
                        {
                            return !VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoRecuperacao && p.vigente);
                        }
                        else
                        {
                            if (VS_fav_id > 0 && VS_ava_id > 0)
                            {
                                List<int> tpc_ids = ACA_AvaliacaoRelacionadaBO.RetornaPeriodoCalendarioRelacionadosPorAvaliacao(VS_fav_id, VS_ava_id).Split(',').Select(p => Convert.ToInt32(p)).ToList();
                                return !VS_ListaEventos.Exists(p => tpc_ids.Contains(p.tpc_id) && p.tev_id == tev_EfetivacaoRecuperacao && p.vigente);
                            }
                        }
                        break;

                    case (byte)AvaliacaoTipo.Final:
                        return !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal && p.vigente);

                    case (byte)AvaliacaoTipo.RecuperacaoFinal:
                        return !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoRecuperacaoFinal && p.vigente);
                }

                return false;
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
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(_UCFechamento.VS_posicao, (byte)EnumModuloPermissao.Compensacoes)
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

        private Label lblMessage
        {
            get
            {
                return _UCFechamento.LblMessage;
            }
        }

        private Label lblMessage2
        {
            get
            {
                return _UCFechamento.LblMessage2;
            }
        }

        private Label lblMessage3
        {
            get
            {
                return _UCFechamento.LblMessage3;
            }
        }

        private Panel pnlAlunos
        {
            get
            {
                return _UCFechamento.PnlAlunos;
            }
        }

        private TUR_TurmaDisciplina VS_turmaDisciplinaCompartilhada
        {
            get
            {
                return (TUR_TurmaDisciplina)_UCFechamento.VS_turmaDisciplinaCompartilhada;
            }
        }

        private UCFechamento _UCFechamento
        {
            get
            {
                return (UCFechamento)this.NamingContainer;
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

        private string AlunoDispensado;

        private string AlunoFrequenciaLimite;

        private string CorAlunoProximoBaixaFrequencia;

        private List<UCFechamento.AlunoDisciplina> lstAlunosPendentes = new List<UCFechamento.AlunoDisciplina>();

        private List<Struct_PreenchimentoAluno> lstAlunosRelatorioRP = new List<Struct_PreenchimentoAluno>();

        #endregion Propriedades

        #region DELEGATES

        public delegate void commandObservacaoConselho(Int32 indiceAluno, Int64 alu_id, Int32 mtu_id, string titulo, int tpc_id);

        public event commandObservacaoConselho AbrirObservacaoConselho;

        public delegate void commandBoletim(Int64 alu_id, Int32 tpc_id, Int32 mtu_id);

        public event commandBoletim AbrirBoletim;

        public delegate void commandAbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno);

        public event commandAbrirRelatorio AbrirRelatorio;

        public delegate void commandMostrarLoading(int tempoProcessar);

        public event commandMostrarLoading MostrarLoading;

        public delegate void commandAbrirRelatorioRP(long alu_id, string tds_idRP);

        public event commandAbrirRelatorioRP AbrirRelatorioRP;

        public delegate void commandAbrirRelatorioAEE(long alu_id);

        public event commandAbrirRelatorioAEE AbrirRelatorioAEE;

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
        }

        #region Carregar grid de alunos

        /// <summary>
        /// Retorna se existe alunos com fechamento pendente a partir da lista de alunos do grid.
        /// </summary>
        /// <param name="dt">Lista de alunos do grid</param>
        /// <returns></returns>
        private bool RetornaAlunosFechamentoPendente<T>(List<T> lista, bool tud_naoLancarNota, bool componenteRegencia)
        {
            if (!tud_naoLancarNota && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
            {
                var listaVerificacao = from T item in lista
                                       let propInfoAvaliacaoID = item.GetType().GetProperty("AvaliacaoID")
                                       let propInfoAvaliacao = item.GetType().GetProperty("Avaliacao")
                                       let propInfoAvaliacaoPosConselho = item.GetType().GetProperty("avaliacaoPosConselho")
                                       let propAluId = item.GetType().GetProperty("alu_id")
                                       select new
                                       {
                                           AvaliacaoID = Convert.ToInt64(propInfoAvaliacaoID.GetValue(item, null) ?? "0")
                                           ,
                                           Avaliacao = (propInfoAvaliacao.GetValue(item, null) ?? string.Empty).ToString()
                                           ,
                                           AvaliacaoPosConselho = (propInfoAvaliacaoPosConselho.GetValue(item, null) ?? string.Empty).ToString()
                                           ,
                                           aluId = Convert.ToInt64(propAluId.GetValue(item, null) ?? "-1")
                                           ,
                                           disNome = componenteRegencia ? (item.GetType().GetProperty("dis_nome").GetValue(item, null) ?? string.Empty).ToString() : string.Empty
                                       };

                object lockObject = new object();
                bool pendente = false;

                Parallel.ForEach
                (
                    listaVerificacao,
                    () => false,
                    (dadosGeral, loopState, alunoPendente) =>
                    {
                        bool retorno =
                            dadosGeral.AvaliacaoID <= 0
                            // ou a nota do aluno no fechamento esta vazia
                            || (
                                String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                && String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                                && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semSintese)
                            )
                            // ou nota incompativel com o tipo de escala de avaliacao
                            || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica
                                &&
                                (
                                    (!String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                    && String.IsNullOrEmpty(NotaFormatada(dadosGeral.Avaliacao))
                                    && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semSintese))
                                    ||
                                    (!String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                                    && String.IsNullOrEmpty(NotaFormatada(dadosGeral.AvaliacaoPosConselho))
                                    && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semSintese))
                                )
                            )
                            || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Pareceres
                                &&
                                (
                                    (!String.IsNullOrEmpty(dadosGeral.Avaliacao)
                                    && !LtPareceres.Any(p => p.eap_valor.Equals(dadosGeral.Avaliacao))
                                    && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semSintese))
                                    ||
                                    (!String.IsNullOrEmpty(dadosGeral.AvaliacaoPosConselho)
                                    && !LtPareceres.Any(p => p.eap_valor.Equals(dadosGeral.AvaliacaoPosConselho))
                                    && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semSintese))
                                )
                            );

                        if (retorno)
                        {
                            UCFechamento.AlunoDisciplina alunoDisciplina = new UCFechamento.AlunoDisciplina();
                            alunoDisciplina.aluId = dadosGeral.aluId;
                            alunoDisciplina.nomeDisciplina = dadosGeral.disNome;
                            lock (lockObject)
                            {
                                lstAlunosPendentes.Add(alunoDisciplina);
                            }
                        }
                        return alunoPendente || retorno;
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
                // Se for uma turma de recuperacao paralela com justificativa para a pendência no fechamento
                // considero como finalizada.
                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
                {
                    CLS_FechamentoJustificativaPendencia justificativaPendencia = CLS_FechamentoJustificativaPendenciaBO.GetSelectBy_TurmaDisciplinaPeriodo(Tud_id, VS_Turma.cal_id, VS_tpc_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (justificativaPendencia.fjp_id > 0)
                    {
                        return false;
                    }
                }

                var listaVerificacao = from T item in lista
                                       let propInfoAvaliacaoID = item.GetType().GetProperty("AvaliacaoID")
                                       let propInfoQtAulasEfetivado = item.GetType().GetProperty("QtAulasEfetivado")
                                       select new
                                       {
                                           AvaliacaoID = Convert.ToInt64(propInfoAvaliacaoID.GetValue(item, null) ?? "0")
                                           ,
                                           QtAulasEfetivado = Convert.ToInt32(propInfoQtAulasEfetivado.GetValue(item, null) ?? "0")
                                       };

                // Se não for Experiência (Território do Saber) mantem a verificação atual.
                if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Experiencia)
                {
                    if (VS_QtdeAulasDadas <= 0 && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia
                        && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_disciplinaSemAula))
                    {
                        // Quando é tud_naoLancarNota ou recuperação paralela, é necessário verificar se tem alguma
                        // aula criada no bimestre.
                        return true;
                    }
                }
                // Se for Experiência (Território do Saber), verifica se existe aula criada de acordo com a vigência da Experiência
                else
                {
                    // Quando é tud_naoLancarNota e Experiência (Território do Saber), verifica se existe aula criada de acordo com a vigência da Experiência
                    if (CLS_TurmaAulaBO.VerificaPendenciaCadastroAulaExperiencia(Tud_id, VS_tpc_id) && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_disciplinaSemAula))
                    {
                        return true;
                    }
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
                            || dadosGeral.AvaliacaoID <= 0;
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
            List<AlunosFechamentoPadrao> listaDisciplina = new List<AlunosFechamentoPadrao>();
            // Escala do conceito global.
            int esa_id = VS_EscalaAvaliacao.esa_id;

            ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoLancamento = (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia;

            // Valor do conceito global ou por disciplina.
            string valorMinimo = VS_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina;

            AvaliacaoTipo tipoAvaliacao = (AvaliacaoTipo)VS_Avaliacao.ava_tipo;
            VS_ava_tipo = (byte)tipoAvaliacao;

            EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            int cacheMedio = ApplicationWEB.AppMinutosCacheMedio;

            string avaliacaoesRelacionadas = string.Empty;
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
                    // Retorna uma string com os ava_idRelacionada separados por ",".
                    avaliacaoesRelacionadas = ACA_AvaliacaoRelacionadaBO.RetornaRelacionadasPor_Avaliacao(VS_fav_id, VS_ava_id, cacheMedio);
                }
            );

            DateTime cap_dataInicio, cap_dataFim;

            if (Tud_id > 0)
            {
                ACA_CalendarioPeriodoBO.RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina(VS_tpc_id, avaliacaoesRelacionadas, Tud_id, VS_fav_id, tipoAvaliacao, VS_Turma.cal_id, out cap_dataInicio, out cap_dataFim);

                VS_cap_dataInicio = cap_dataInicio;
                VS_cap_dataFim = cap_dataFim;

                // Busca os alunos por disciplina na turma.
                listaDisciplina = VS_DisciplinaEspecial ?
                    MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamentoFiltroDeficiencia
                    (
                        Tud_id,
                        VS_tur_id,
                        VS_tpc_id,
                        VS_ava_id,
                        Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                        VS_fav_id,
                        (byte)tipoAvaliacao,
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        , VS_Turma.tur_tipo
                        , VS_Turma.cal_id
                        , _UCFechamento.VS_tipoDocente
                        , false
                        , ApplicationWEB.AppMinutosCacheFechamento
                        , VS_listaTur_ids
                    ) :
                    MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamento
                    (
                        Tud_id,
                        VS_tur_id,
                        VS_tpc_id,
                        VS_ava_id,
                        Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue),
                        VS_fav_id,
                        (byte)tipoAvaliacao,
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        , VS_Turma.tur_tipo
                        , VS_Turma.cal_id
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
                                if (!dtAlunos.AsEnumerable().Any(a => Convert.ToInt64(a["alu_id"]) == aluno.alu_id && Convert.ToInt32(a["mtu_id"]) == aluno.mtu_id))
                                {
                                    DataRow drAluno = dtAlunos.NewRow();
                                    drAluno["alu_id"] = aluno.alu_id;
                                    drAluno["mtu_id"] = aluno.mtu_id;
                                    dtAlunos.Rows.Add(drAluno);
                                }
                            }
                        }
                    );

                    listaAlunosComponentesRegencia = MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamentoComponentesRegencia
                                                    (
                                                        VS_tur_id,
                                                        VS_tpc_id,
                                                        VS_ava_id,
                                                        VS_fav_id,
                                                        (byte)tipoAvaliacao,
                                                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                        , VS_Turma.tur_tipo
                                                        , dtAlunos
                                                        , ApplicationWEB.AppMinutosCacheFechamento
                                                    );
                }
            }

            // Ordenação dos alunos.
            int numeroChamada;
            listaDisciplina = Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue) == 0 ?
                                    listaDisciplina.OrderBy(p => Int32.TryParse(p.mtd_numeroChamada, out numeroChamada) ? numeroChamada : -1).ThenBy(p => p.pes_nome).ToList()
                                    : listaDisciplina.OrderBy(p => p.pes_nome).ToList();

            // Mostra total de aulas cadastradas no período.
            SetaQuantidadeAulas(VS_tpc_id);

            if (Tud_id > 0 && VS_tpc_id > 0 && listaDisciplina != null && listaDisciplina.Count > 0)
            {
                List<MTR_MatriculaTurmaDisciplina> listaMtds =
                    listaDisciplina.Select(p => new MTR_MatriculaTurmaDisciplina { alu_id = p.alu_id, mtu_id = p.mtu_id, mtd_id = p.mtd_id }).ToList();

                // Buscar frequências externas para exibir na tela.
                listaFrequenciaExterna = CLS_AlunoFrequenciaExternaBO.SelecionaPor_MatriculasDisciplinaPeriodo(listaMtds, VS_tpc_id);
            }

            bool pendenciaFechamento = false;
            lstAlunosPendentes = new List<UCFechamento.AlunoDisciplina>();
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_STATUS_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                && (EntTurmaDisciplina.tud_tipo != (int)TurmaDisciplinaTipo.TerritorioSaber)
                // Exibe a mensagem de pendencia na efetivacao apos o inicio do fechamento do bimestre,
                // ou apos o termino do periodo do fechamento. 
                &&
                (VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoNotas && p.vigente)
                    || VS_ListaEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoNotas && !p.vigente && p.evt_dataFim < DateTime.Now)))
            {
                bool tud_naoLancarNota = false;
                if (Tud_id > 0)
                {
                    tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
                }

                VS_ListConfiguracaoServicoPendencia = new List<ACA_ConfiguracaoServicoPendencia>();

                foreach (int cur_id in TUR_TurmaCurriculoBO.GetSelectBy_Turma(VS_tur_id, ApplicationWEB.AppMinutosCacheLongo)
                                                           .GroupBy(t => t.cur_id).Select(t => t.Key))
                {
                    ACA_Curso cur = new ACA_Curso { cur_id = cur_id };
                    ACA_CursoBO.GetEntity(cur);
                    VS_ListConfiguracaoServicoPendencia.AddRange(ACA_ConfiguracaoServicoPendenciaBO.SelectTodasBy_tne_id_tme_id_tur_tipo(cur.tne_id, cur.tme_id, VS_Turma.tur_tipo));
                }

                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && !tud_naoLancarNota)
                {
                    if (listaAlunosComponentesRegencia != null
                        && listaAlunosComponentesRegencia.Any()
                        && listaDisciplina != null
                        && listaDisciplina.Any()
                        && VS_cap_dataInicio <= DateTime.Today)
                    {
                        pendenciaFechamento = RetornaAlunosFechamentoPendente(listaAlunosComponentesRegencia, tud_naoLancarNota, true);
                        this._UCFechamento.AtualizarStatusEfetivacao(pendenciaFechamento, tud_naoLancarNota);
                    }
                }
                else if (Tud_id > 0
                        && listaDisciplina != null
                        && listaDisciplina.Any()
                        && VS_cap_dataInicio <= DateTime.Today)
                {
                    pendenciaFechamento = RetornaAlunosFechamentoPendente(listaDisciplina, tud_naoLancarNota, false);
                    this._UCFechamento.AtualizarStatusEfetivacao(pendenciaFechamento, tud_naoLancarNota);
                }
            }

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
            if (Tud_id > 0 && tipo == EscalaAvaliacaoTipo.Relatorios)
            {
                // Se for disciplina de regencia, carrego os dados referentes aos componentes da regencia
                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    SetaDadosRelatorio(listaAlunosComponentesRegencia, "atd_relatorio");
                }
                else
                {
                    SetaDadosRelatorio(listaDisciplina, "atd_relatorio");
                }
            }

            // Seta a visibilidade das colunas do grid de acordo com o tipo de avaliação.
            SetaColunasVisiveisGrid(VS_Avaliacao, pendenciaFechamento);

            // Seta nome dos headers das colunas de nota.
            SetaNomesColunas(tipoEscala, VS_FormatoAvaliacao);

            List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);
            int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
            avaliacaoUltimoPerido = ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.Periodica
                                        || (AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.PeriodicaFinal)
                                        && VS_Avaliacao.tpc_id > 0 && VS_Avaliacao.tpc_id == tpc_idUltimoPerido;

            if (Tud_id > 0)
            {
                if (VS_Turma.tur_tipo == (byte)TUR_TurmaTipo.Normal)
                {
                    lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(VS_tpc_id, VS_Turma.tur_id, Tud_id, ApplicationWEB.AppMinutosCacheMedio);
                }

                gvAlunos.DataSource = listaDisciplina;
            }
            gvAlunos.DataBind();

            VS_dtTiposResultados.Clear();

            object sync = new object();

            AlunoInativo = ApplicationWEB.AlunoInativo;
            AlunoDispensado = ApplicationWEB.AlunoDispensado;
            AlunoFrequenciaLimite = ApplicationWEB.AlunoFrequenciaLimite;
            CorAlunoProximoBaixaFrequencia = ApplicationWEB.CorAlunoProximoBaixaFrequencia;

            Parallel.ForEach
                     (
                         (from GridViewRow row in gvAlunos.Rows
                          select row)
                         ,
                         row =>
                         {
                             if (row.RowType == DataControlRowType.DataRow)
                             {
                                 ConfiguraDadosAlunoLinhaGrid(row);

                                 long alu_id = Convert.ToInt64(gvAlunos.DataKeys[row.RowIndex].Values["alu_id"]);
                                 int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[row.RowIndex].Values["mtu_id"]);

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

                                 bool exibeCampoNotaAluno = true;

                                 if ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.RecuperacaoFinal)
                                 {
                                     exibeCampoNotaAluno = Convert.ToBoolean(hdnRecuperacaoPorNota.Value);
                                 }

                                 if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                                 {
                                     lock (sync)
                                     {
                                         // Seta campos da avaliação principal.
                                         SetaCamposAvaliacao(tipo, txtNota, hdnNota.Value, ddlPareceres, exibeCampoNotaAluno, row, false);

                                         // Seta campos da avaliação pós-conselho.
                                         SetaCamposAvaliacao(tipo, txtNotaPosConselho, hdnNotaPosConselho.Value, ddlPareceresPosConselho, VS_Avaliacao.ava_exibeNotaPosConselho, row, true);

                                         SetaComponentesRelatorioLinhaGrid(row, exibeCampoNotaAluno, false);

                                         SetaComponentesRelatorioLinhaGrid(row, VS_Avaliacao.ava_exibeNotaPosConselho, true);

                                     }
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
                               true,
                               null,
                               false,
                               item);

                         //
                         // CARREGA CAMPO NOTA POS-CONSELHO
                         //
                         TextBox txtNotaPosConselho = (TextBox)item.FindControl("txtNotaPosConselho");
                         DropDownList ddlPareceresPosConselho = (DropDownList)item.FindControl("ddlPareceresPosConselho");
                         HiddenField hdnAvaliacaoPosConselho = (HiddenField)item.FindControl("hdnAvaliacaoPosConselho");
                         // Seta campos da avaliação pós-conselho.
                         SetaCamposAvaliacao(
                             (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo,
                             txtNotaPosConselho,
                             hdnAvaliacaoPosConselho.Value,
                             ddlPareceresPosConselho,
                             VS_Avaliacao.ava_exibeNotaPosConselho,
                             null,
                             true,
                             item);
                     }
                );
            }

            divLegenda.Visible = gvAlunos.Rows.Count > 0;
            HabilitarControlesTela((_UCFechamento.usuarioPermissao && _UCFechamento.DocentePodeEditar) && pnlAlunos.Visible && !periodoFechado && _UCFechamento.usuarioPermissao);

            if (ExisteDispensaDisciplina)
                lblMessageInfo.Text = UtilBO.GetErroMessage("Alunos com dispensa de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                                            " terão suas notas e frequência desconsideradas.", UtilBO.TipoMensagem.Informacao);

            lblMessageInfo2.Visible = false;
            string informacaoFechamento;
            if (EntTurmaDisciplina.tud_naoLancarNota)
            {
                informacaoFechamento = GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.lblMessageInfo2.Text.MsgNaoLancaNota").ToString();
            }
            else
            {
                informacaoFechamento = string.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.lblMessageInfo2.Text.MsgLancaNota").ToString(), gvAlunos.Columns[colunaNota].HeaderText.ToLower());
            }
            if (!String.IsNullOrEmpty(informacaoFechamento))
            {
                lblMessageInfo2.Visible = true;
                lblMessageInfo2.Text = UtilBO.GetErroMessage(informacaoFechamento, UtilBO.TipoMensagem.Informacao);
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

            if ((_UCFechamento.VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
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
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres, bool exibeCampoNotaAluno, GridViewRow row, bool posConselho, RepeaterItem rptItem = null)
        {
            if (txtNota != null)
            {
                if (exibeCampoNotaAluno && (tipo == EscalaAvaliacaoTipo.Numerica))
                {
                    txtNota.Visible = true;
                    txtNota.Text = !posConselho || VS_Avaliacao.ava_exibeNotaPosConselho ? NotaFormatada(aat_avaliacao) : string.Empty;
                    if (row != null && !string.IsNullOrEmpty(txtNota.Text))
                    {
                        double nota = double.Parse(txtNota.Text, CultureInfo.InvariantCulture);
                        if (VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao
                            && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            && nota < VS_NotaMinima)
                        {
                            row.Cells[posConselho ? colunaNotaPosConselho : colunaNota].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
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
                    CarregarPareceres(ddlPareceres);

                    int ordem = RetornaOrdemParecer(aat_avaliacao);

                    // Concatena eap_valor + eap_ordem.
                    ddlPareceres.SelectedValue = !posConselho || VS_Avaliacao.ava_exibeNotaPosConselho ? aat_avaliacao + ";" + ordem : "-1;-1";

                    if (row != null
                        && VS_FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao
                        && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        && ordem != -1 && ordem < VS_ParecerMinimo)
                    {
                        row.Cells[posConselho ? colunaNotaPosConselho : colunaNota].BackColor = System.Drawing.Color.FromArgb(255, 30, 30);
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
            if (lblQtdeAulasPrevistas.Visible)
            {
                List<TUR_TurmaDisciplinaAulaPrevista> aulasPrevistas = TUR_TurmaDisciplinaAulaPrevistaBO.SelecionaPorDisciplina(Tud_id, ApplicationWEB.AppMinutosCacheLongo);
                lblQtdeAulasPrevistas.Text = string.Empty;
                if (aulasPrevistas.Count > 0)
                {
                    TUR_TurmaDisciplinaAulaPrevista aulaPrevista = aulasPrevistas.Find(p => p.tpc_id == tpc_id);
                    if (aulaPrevista != null)
                    {
                        lblQtdeAulasPrevistas.Text = UtilBO.GetErroMessage(String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.QuantAulasPrevistas").ToString(), aulaPrevista.tap_aulasPrevitas.ToString()), UtilBO.TipoMensagem.Informacao);
                    }
                }
                if (string.IsNullOrEmpty(lblQtdeAulasPrevistas.Text))
                {
                    lblQtdeAulasPrevistas.Visible = false;
                }
            }

            if (lblQtdeAulasDadas.Visible)
            {
                int qtdeAulasReposicao = 0;
                int qtdeAulasReposicaoPeriodo = 0;
                int qtdAulasDatasPeriodo = 0;

                byte posicaoEspecial = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Especial, ApplicationWEB.AppMinutosCacheLongo);

                string tdc_ids = VS_DisciplinaEspecial && _UCFechamento.VS_posicao == posicaoEspecial ? ((byte)EnumTipoDocente.Especial).ToString() :
                                                         string.Format("{0};{1};{2}",
                                                                        ((byte)EnumTipoDocente.Titular).ToString(),
                                                                        ((byte)EnumTipoDocente.Substituto).ToString(),
                                                                        ((byte)EnumTipoDocente.SegundoTitular).ToString());

                VS_QtdeAulasDadas = CLS_TurmaAulaBO.SelecionaQuantidadeAulasDadas(Tud_id, tpc_id, tdc_ids, out qtdeAulasReposicao, out qtdAulasDatasPeriodo, out qtdeAulasReposicaoPeriodo, null);

                lblQtdeAulasDadas.Text = UtilBO.GetErroMessage(
                    qtdeAulasReposicao > 0 ?
                    string.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.QuantAulasDadasReposicao").ToString(), VS_QtdeAulasDadas.ToString(), qtdeAulasReposicao.ToString()) :
                    string.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.QuantAulasDadas").ToString(), VS_QtdeAulasDadas.ToString()), UtilBO.TipoMensagem.Informacao);

                VS_QtdeAulasDadas = qtdAulasDatasPeriodo;

                // Caso seja uma disciplina do tipo experiência, exibe a quantidade de total de aulas do períodos
                if (lblTotalAulasExperiencia.Visible)
                {
                    lblTotalAulasExperiencia.Text = UtilBO.GetErroMessage(String.Format(
                        GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.QuantTotalAulasExperiencia").ToString(), 
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
        private void SetaDadosRelatorio<T>(List<T> lista, string nomeColunaRelatorio) where T : struct
        {
            if (lista.Any())
            {
                PropertyInfo propInfo;

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

                            UCFechamento.NotasRelatorio rel = new UCFechamento.NotasRelatorio
                            {
                                Id = id,
                                Valor = valorRelatorio,
                                arq_idRelatorio = arq_idRelatorio
                            };

                            VS_Nota_Relatorio.Add(rel);
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
        private void SetaColunasVisiveisGrid(ACA_Avaliacao entAvaliacao, bool pendenciaFechamento)
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
            TurmaDisciplinaTipo tipoDisciplina = (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo;

            bool permissaoConpensacao = VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta);
            bool permissaoBoletim = _UCFechamento.VS_ltPermissaoBoletim.Any(p => p.pdc_permissaoConsulta);

            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            //Coluna frequencia ajustada com as ausências cadastradas
            gvAlunos.Columns[colunaFrequenciaAjustada].Visible = ExibeCompensacaoAusencia && (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFrequenciaAjustada].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFrequenciaAjustada].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

            // Coluna com qt de faltas = coluna de frequencia.
            gvAlunos.Columns[colunaFaltas].Visible = ((tipo == AvaliacaoTipo.Periodica) ||
                                                       (tipo == AvaliacaoTipo.PeriodicaFinal) ||
                                                       (tipo == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)) &&
                                                      (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia && ExibirItensRegencia));

            gvAlunos.Columns[colunaFaltas].Visible &= !tud_naoLancarFrequencia;

            gvAlunos.Columns[colunaFaltas].Visible &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;

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
            gvAlunos.Columns[colunaResultado].Visible = (tipo == AvaliacaoTipo.PeriodicaFinal || tipo == AvaliacaoTipo.Final || tipo == AvaliacaoTipo.RecuperacaoFinal)
                                                        && (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.Regencia && ExibirItensRegencia));

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

            CFG_PermissaoModuloOperacao permissaoModuloOperacao = new CFG_PermissaoModuloOperacao()
            {
                gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                sis_id = ApplicationWEB.SistemaID,
                mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoVisualizacaoObservacoes)
            };
            CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloOperacao);

            bool possuiPermissaoVisualizacao = true;

            if (!permissaoModuloOperacao.IsNew && !permissaoModuloOperacao.pmo_permissaoConsulta)
            {
                possuiPermissaoVisualizacao = false;
            }

            gvAlunos.Columns[colunaObservacaoConselho].Visible = ((tipo == AvaliacaoTipo.Periodica) || (tipo == AvaliacaoTipo.PeriodicaFinal))
                                                                  && VS_Avaliacao.ava_exibeObservacaoConselhoPedagogico && possuiPermissaoVisualizacao;


            if (!ExibirItensRegencia || (tipoDisciplina != TurmaDisciplinaTipo.Regencia && ExibirItensRegencia))
            {
                gvAlunos.Columns[colunaNotaRegencia].Visible = false;
                gvAlunos.Columns[colunaNota].Visible = !tud_naoLancarNota;

                // Verifica se a turma possui efetivação e se é disciplina
                // Verifica se COC da disciplina está marcado para ser avaliado ou lançar frequência;
                if (_UCFechamento.VS_EfetivacaoSemestral && Tud_id > 0)
                {
                    ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular entity = null;

                    // Verfica se é recuperação e busca o id do período por avaliação relacionada.
                    if (tipo == AvaliacaoTipo.Recuperacao || tipo == AvaliacaoTipo.RecuperacaoFinal)
                    {
                        DataTable dtAvaliacaoRec = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(tipo, entAvaliacao.fav_id);

                        if (dtAvaliacaoRec.Rows.Count > 0)
                        {
                            DataRow drRec = dtAvaliacaoRec.Rows.Cast<DataRow>().ToList().Find(p => Convert.ToInt32(p["ava_id"]) == entAvaliacao.ava_id);
                            if (drRec != null)
                            {
                                entity = _UCFechamento.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == Convert.ToInt32(drRec["tpc_id"]));
                            }
                        }
                    }
                    else
                    {
                        entity = _UCFechamento.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == entAvaliacao.tpc_id);
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

                gvAlunos.Columns[colunaNotaPosConselho].Visible = entAvaliacao.ava_exibeNotaPosConselho &&
                                                                  VS_EscalaAvaliacao.esa_tipo != (byte)EscalaAvaliacaoTipo.Relatorios;

                gvAlunos.Columns[colunaNotaPosConselho].Visible &= !tud_naoLancarNota;
            }
            else
            {
                gvAlunos.Columns[colunaNota].Visible =
                gvAlunos.Columns[colunaNotaPosConselho].Visible =
                gvAlunos.Columns[colunaResultado].Visible = false;
                gvAlunos.Columns[colunaNotaRegencia].Visible = tipoDisciplina == TurmaDisciplinaTipo.Regencia && ExibirItensRegencia;

                //
                // VISIBILIDADE DAS COLUNAS DOS COMPONENTES DA REGENCIA
                //
                if (gvAlunos.Columns[colunaNotaRegencia].Visible)
                {
                    visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNota] = true;

                    // Verifica se a turma possui efetivação e se é disciplina
                    // Verifica se COC da disciplina está marcado para ser avaliado ou lançar frequência;
                    if (_UCFechamento.VS_EfetivacaoSemestral)
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
                                    entity = _UCFechamento.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == Convert.ToInt32(drRec["tpc_id"]));
                                }
                            }
                        }
                        else
                        {
                            entity = _UCFechamento.VS_MatrizCurricular.Find(p => p.tud_id == Tud_id && p.tpc_id == entAvaliacao.tpc_id);
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
            gvAlunos.Columns[colunaStatus].Visible = pendenciaFechamento && !tud_naoLancarNota;
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
                if (dataMatricula != new DateTime() && dataMatricula.Date > VS_cap_dataInicio.Date)
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
                if (dataSaida != new DateTime() && dataSaida.Date < VS_cap_dataFim)
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
                if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
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

                UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == id);
                AbrirRelatorio(id, nota.Valor, nota.arq_idRelatorio, dadosAluno);
            }
        }

        #endregion Carregar grid de alunos

        /// <summary>
        /// Esconde botões de salvar e grid de alunos - utilizado quando não existe avaliação ou disciplina
        /// nos combos.
        /// </summary>
        public void EscondeTelaAlunos(string mensagem, UtilBO.TipoMensagem tipoMsg = UtilBO.TipoMensagem.Alerta)
        {
            pnlAlunos.Visible = false;
            _UCFechamento.VisibleBotaoSalvar = false;
            if (!String.IsNullOrEmpty(mensagem))
            {
                lblMessage.Text = UtilBO.GetErroMessage(mensagem, tipoMsg);
            }
            gvAlunos.DataSource = new DataTable();
            gvAlunos.DataBind();
        }

        /// <summary>
        /// Esconde botões de salvar e grid de alunos - utilizado quando não existe avaliação ou disciplina
        /// nos combos.
        /// </summary>
        public void VerificaPendenciaFilaProcessamento()
        {

            var pendencias = CLS_AlunoFechamentoPendenciaBO.SelecionarAguardandoProcessamento(VS_tur_id, Tud_id, EntTurmaDisciplina.tud_tipo, VS_Avaliacao.tpc_id);

            if (pendencias == null || pendencias.Rows.Count == 0)
            {
                SetaProcessamentoConcluido();
            }
            else
            {
                EscondeTelaAlunos((string)GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.lblMessage.Text.MsgPendenciaProcessamento"), UtilBO.TipoMensagem.Informacao);
            }
        }

        /// <summary>
        /// Mostra o grid de alunos e botões de salvar.
        /// </summary>
        private void MostraTelaAlunos()
        {
            pnlAlunos.Visible = true;
            _UCFechamento.VisibleBotaoSalvar = !periodoFechado && _UCFechamento.usuarioPermissao
                                                && (!EntTurmaDisciplina.tud_naoLancarNota
                                                    || (!VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                        || HabilitarLancamentosAnoLetivoAnterior));
        }

        /// <summary>
        /// Limpa valores de variáveis locais que são usadas a cada postback.
        /// </summary>
        private void LimpaVariaveisLocais()
        {
            _ltPareceres = null;
            ViewState["VS_ParecerMinimo"] = null;
            ViewState["VS_NotaMinima"] = null;
        }

        /// <summary>
        /// Seta eventos nos textbox que precisam recalcular a frequência acumulada.
        /// </summary>
        private void SetaEventosTxtFrequenciaAcumulada()
        {
            if (Tud_id > 0 && gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
            {
                // Seta eventos nos txts para atualizar a frequência acumulada.
                Parallel.ForEach
                (
                    gvAlunos.Rows.Cast<GridViewRow>()
                    ,
                    row =>
                    {
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
                    HabilitaControles(_UCFechamento.FdsRelatorio.Controls, value);
                }
            );

            bool habilitarAnoLetivoAnterior = HabilitarLancamentosAnoLetivoAnterior;
            bool desabilitarNotaEfetivacao = DesabilitarLancamentoNotaEfetivacao;
            _UCFechamento.VisibleBotaoSalvar = value &&
                                            (!EntTurmaDisciplina.tud_naoLancarNota
                                                || (!VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                    || habilitarAnoLetivoAnterior));

            if (value)
            {
                Parallel.ForEach
                (
                    gvAlunos.Rows.Cast<GridViewRow>()
                    ,
                    Row =>
                    {
                        TextBox txtQtdeFalta = (TextBox)Row.FindControl("txtQtdeFalta");
                        TextBox txtAusenciasCompensadas = (TextBox)Row.FindControl("txtAusenciasCompensadas");
                        Repeater rptComponenteRegencia = (Repeater)Row.FindControl("rptComponenteRegencia");

                        if (txtQtdeFalta != null)
                        {
                            if (Tud_id > 0)
                            {
                                txtQtdeFalta.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                        || habilitarAnoLetivoAnterior;
                            }
                            else
                            {
                                txtQtdeFalta.Enabled = false;
                            }
                        }

                        if (txtAusenciasCompensadas != null)
                        {
                            if (Tud_id > 0)
                            {
                                txtAusenciasCompensadas.Enabled = !VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                                                   || habilitarAnoLetivoAnterior;
                            }
                            else
                            {
                                txtAusenciasCompensadas.Enabled = false;
                            }
                        }

                        TextBox txtFrequenciaFinalAjustada = (TextBox)Row.FindControl("txtFrequenciaFinalAjustada");
                        if (txtFrequenciaFinalAjustada != null)
                        {
                            txtFrequenciaFinalAjustada.Enabled = false;
                        }

                        TextBox txtNota = (TextBox)Row.FindControl("txtNota");
                        if (txtNota != null && desabilitarNotaEfetivacao)
                        {
                            txtNota.Enabled = false;
                        }

                        DropDownList ddlPareceres = (DropDownList)Row.FindControl("ddlPareceres");
                        if (ddlPareceres != null && desabilitarNotaEfetivacao)
                        {
                            ddlPareceres.Enabled = false;
                        }

                        DropDownList ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
                        ddlResultado.Enabled = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        foreach (RepeaterItem rptItem in rptComponenteRegencia.Items)
                        {
                            txtNota = (TextBox)rptItem.FindControl("txtNota");
                            if (txtNota != null && desabilitarNotaEfetivacao)
                            {
                                txtNota.Enabled = false;
                            }

                            ddlPareceres = (DropDownList)rptItem.FindControl("ddlPareceres");
                            if (ddlPareceres != null && desabilitarNotaEfetivacao)
                            {
                                ddlPareceres.Enabled = false;
                            }
                        }
                    }
                );
            }
            else
            {
                HabilitaAcessoPopUps();
            }
            _UCComboOrdenacao1._Combo.Enabled = true;
        }

        /// <summary>
        /// Habilita os botoes que abrem pop-up no gridview do aluno
        /// </summary>
        /// <returns></returns>
        private void HabilitaAcessoPopUps()
        {
            pnlAlunos.Enabled = true;
            gvAlunos.Enabled = true;
            ((System.Web.UI.WebControls.WebControl)gvAlunos.Controls[0]).Enabled = true;

            foreach (GridViewRow Row in gvAlunos.Rows)
            {
                Row.Enabled = true;
                Row.Cells[colunaBoletim].Enabled = true;
                Row.Cells[colunaObservacaoConselho].Enabled = true;
                Row.Cells[colunaFaltas].Enabled = true;

                ImageButton btnBoletim = (ImageButton)Row.FindControl("btnBoletim");
                if (btnBoletim != null)
                {
                    btnBoletim.Enabled = true;
                }

                ImageButton btnObservacaoConselho = (ImageButton)Row.FindControl("btnObservacaoConselho");
                if (btnObservacaoConselho != null)
                {
                    btnObservacaoConselho.Enabled = true;
                }

                ImageButton btnFaltasExternas = (ImageButton)Row.FindControl("btnFaltasExternas");
                if (btnFaltasExternas != null)
                {
                    btnFaltasExternas.Enabled = true;
                }
                btnFaltasExternas.Enabled = true;
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
                VS_Nota_Relatorio = new List<UCFechamento.NotasRelatorio>();

                divLegenda.Style.Add(HtmlTextWriterStyle.Width, "300px");

                lblQtdeAulasDadas.Visible = lblQtdeAulasPrevistas.Visible = VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia == (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPrevistasDocente;

                //Exibe o campo total de aulas apenas quando o tipo da disciplina é "Experiência"
                lblTotalAulasExperiencia.Visible = lblQtdeAulasDadas.Visible && (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo == TurmaDisciplinaTipo.Experiencia;

                // Carregar grid.
                CarregarGridAlunos();
                lnAlunoFrequencia.Visible = ExibeCompensacaoAusencia;
                lnAlunoProximoBaixaFrequencia.Visible = ExibeCompensacaoAusencia && VS_FormatoAvaliacao.percentualBaixaFrequencia > 0;
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a efetivação.", UtilBO.TipoMensagem.Erro);
                EscondeTelaAlunos(String.Empty);
            }
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
                List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina;
                ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia;

                ValidaGeraDados(out listaDisciplina, out tipoFrequencia);

                if (Tud_id > 0)
                {
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
                        new List<NotaFinalAlunoTurmaDisciplina>(),
                        _UCFechamento.VS_EfetivacaoSemestral,
                        VS_ava_id,
                        VS_Avaliacao.ava_exibeNotaPosConselho,
                        (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo,
                        gvAlunos.Columns[colunaNotaRegencia].Visible,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, _UCFechamento.VS_MensagemLogEfetivacao + "tur_id: " + VS_tur_id + "; tud_id: " + Tud_id + "; ava_id: " + VS_ava_id);
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

                lblMessage.Text = UtilBO.GetErroMessage(msg, tipoMsg);

                if (!string.IsNullOrEmpty(msgValidacao))
                {
                    tipoMsg = UtilBO.TipoMensagem.Alerta;
                }

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
                    lblMessage2.Text = UtilBO.GetErroMessage(TrocaParametroMensagem(msgValidacao), tipoMsg);
                }
            }
            catch (ThreadAbortException) { }
            catch (ValidationException ex)
            {
                if (listDisciplinasNaoLancadas.Count > 0)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(String.Format("O " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        " não poderá ser fechado pois não foi lançada a frequência mensal ou o " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        (listDisciplinasNaoLancadas.Count == 1 ? " não foi fechado para o(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " a seguir: "
                            : " não foi fechado para os(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " a seguir: ") +
                               string.Join(",", listDisciplinasNaoLancadas.ToArray()) + "."), UtilBO.TipoMensagem.Alerta);
                }
                else if (listDisciplinasDivergentesPorAluno.Count > 0)
                {
                    CarregaGridAlunosComDisciplinasDivergentes(listDisciplinasDivergentesPorAluno);
                    lblMessage.Text = UtilBO.GetErroMessage(String.Format("O " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToUpper() +
                        " não poderá ser fechado pois as informações estão divergentes no lançamento de frequência mensal e do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " de todos os alunos. ")
                        , UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a efetivação.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Faz a validação dos dados na tela e gera as listas necessárias para salvar.
        /// </summary>
        /// <param name="listaDisciplina">Lista de disciplinas para usar para salvar</param>
        /// <param name="listaTurma">Lista de turmas para usar para salvar</param>
        /// <param name="tipoFrequencia">Tipo de lançamento de frequência</param>
        private void ValidaGeraDados(out List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina, out ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia)
        {
            listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();

            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinaLocal = new List<CLS_AvaliacaoTurDisc_Cadastro>();

            List<string> alunosErroIntervalo = new List<string>(),
                         alunosErroConversao = new List<string>(),
                         alunosErroIntervaloNotaPosConselho = new List<string>(), alunosErroConversaoNotaPosConselho = new List<string>();
            string stringErro = string.Empty, stringErroFaltas = string.Empty, stringErroAusencias = string.Empty;

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

            bool existeAlunoComNotaPosConselho = false;
            bool existeAlunoSemNotaPosConselho = false;
            bool habilitarAnoLetivoAnterior = HabilitarLancamentosAnoLetivoAnterior;
            bool desabilitarNotaEfetivacao = DesabilitarLancamentoNotaEfetivacao;

            object lockObject = new object();
            Parallel.ForEach
            (
                gvAlunos.Rows.Cast<GridViewRow>()
                ,
                row =>
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string strQtd;
                        int intQtd;

                        if (!VS_FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina
                                || habilitarAnoLetivoAnterior)
                        {
                            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
                            if (txtQtdeFalta.Enabled)
                            {
                                strQtd = txtQtdeFalta.Text;
                                if (int.TryParse(strQtd, out intQtd) && (intQtd > 99999))
                                {
                                    stringErroFaltas = "A quantidade de faltas deve ser menor que 99999.";
                                }
                            }

                            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
                            if (txtAusenciasCompensadas.Enabled)
                            {
                                strQtd = txtAusenciasCompensadas.Text;
                                if (int.TryParse(strQtd, out intQtd) && (intQtd > 99999))
                                {
                                    stringErroAusencias = "A quantidade de ausências compensadas deve ser menor que 99999.";
                                }
                            }
                        }
                    }

                    if (tipoEscalaNumerica)
                    {
                        if (!gvAlunos.Columns[colunaNotaRegencia].Visible)
                        {
                            if (gvAlunos.Columns[colunaNota].Visible && !desabilitarNotaEfetivacao)
                            {
                                // Recupera o valor da avaliação normal.
                                TextBox txtNota = (TextBox)row.FindControl("txtNota");
                                if (txtNota != null
                                    && txtNota.Enabled
                                    && !string.IsNullOrEmpty(txtNota.Text))
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
                                            alunosErroConversao.Add(((Label)row.FindControl("lblNomeAluno")).Text);
                                        }
                                    }
                                }
                            }

                            if (gvAlunos.Columns[colunaNotaPosConselho].Visible)
                            {
                                TextBox txtNotaPosConselho = (TextBox)row.FindControl("txtNotaPosConselho");
                                if (txtNotaPosConselho != null
                                    && !string.IsNullOrEmpty(txtNotaPosConselho.Text))
                                {
                                    existeAlunoComNotaPosConselho = true;
                                    if (txtNotaPosConselho.Enabled)
                                    {
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
                            Parallel.ForEach
                        (
                            rptComponenteRegencia.Items.Cast<RepeaterItem>()
                            ,
                            rptItem =>
                            {
                            // Recupera o valor da avaliação normal.
                            if (!desabilitarNotaEfetivacao)
                                {
                                    TextBox txtNota = (TextBox)rptItem.FindControl("txtNota");
                                    if (txtNota != null
                                    && txtNota.Enabled
                                    && !string.IsNullOrEmpty(txtNota.Text))
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
                                }

                                HtmlTableRow trComponente = (HtmlTableRow)rptItem.FindControl("tr1");
                                if (trComponente.Cells[colunaComponenteRegenciaNotaPosConselho].Visible)
                                {
                                    TextBox txtNotaPosConselho = (TextBox)rptItem.FindControl("txtNotaPosConselho");
                                    if (txtNotaPosConselho != null
                                    && !string.IsNullOrEmpty(txtNotaPosConselho.Text))
                                    {
                                        existeAlunoComNotaPosConselho = true;
                                        if (txtNotaPosConselho.Enabled)
                                        {
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
                                    alunosErroConversao.Add(((Label)row.FindControl("lblNomeAluno")).Text);
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

                    if (Tud_id > 0)
                    {
                        lock (lockObject)
                        {
                            listaDisciplinaLocal.AddRange(AdicionaLinhaDisciplina(row));
                        }
                    }
                }
            );

            listaDisciplina = listaDisciplinaLocal;

            int numeroCasasDecimais = RetornaNumeroCasasDecimais();
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

            if (alunosErroConversao.Count == 1)
            {
                stringErro += string.Format("Nota para o aluno {0} é inválida.", string.Join(", ", alunosErroConversao.ToArray()));
            }
            else if (alunosErroConversao.Count > 1)
            {
                stringErro += string.Format("Nota para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversao.ToArray()));
            }

            if (alunosErroConversaoNotaPosConselho.Count == 1)
            {
                stringErro += string.Format("Nota pós-conselho para o aluno {0} é inválida.", string.Join(", ", alunosErroConversaoNotaPosConselho.ToArray()));
            }
            else if (alunosErroConversaoNotaPosConselho.Count > 1)
            {
                stringErro += string.Format("Nota pós-conselho para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversaoNotaPosConselho.ToArray()));
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
                lblMessage3.Text = UtilBO.GetErroMessage("Existem alunos sem preenchimento de " + gvAlunos.Columns[colunaNotaPosConselho].HeaderText.ToLower() + ".", UtilBO.TipoMensagem.Alerta);
            }
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

                    if (_UCFechamento.VS_NomeAvaliacaoRecuperacaoFinal != "" && resultado != MtrTurmaDisciplinaResultado.Aprovado
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

            ent.fav_id = VS_fav_id;
            ent.ava_id = VS_ava_id;
            ent.tpc_id = VS_tpc_id;
            ent.atd_situacao = dispensadisciplina ? (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Excluido : (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

            // Setar o registroExterno para false.
            ent.atd_registroexterno = false;

            #region Campos das aulas / frequências

            ent.atd_numeroFaltasReposicao = Convert.ToInt32(((HiddenField)row.FindControl("hdnQtFaltasReposicao")).Value);

            TextBox txtQtdeFalta = (TextBox)row.FindControl("txtQtdeFalta");
            ent.atd_numeroFaltas = gvAlunos.Columns[colunaFaltas].Visible ?
                (String.IsNullOrEmpty(txtQtdeFalta.Text) ? 0 : Convert.ToInt32(txtQtdeFalta.Text)) :
                0;

            TextBox txtAusenciasCompensadas = (TextBox)row.FindControl("txtAusenciasCompensadas");
            ent.atd_ausenciasCompensadas = gvAlunos.Columns[colunaAusenciasCompensadas].Visible ?
                (String.IsNullOrEmpty(txtAusenciasCompensadas.Text) ? 0 : Convert.ToInt32(txtAusenciasCompensadas.Text)) :
                0;

            TextBox txtFrequenciaFinalAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
            if (gvAlunos.Columns[colunaFrequenciaAjustada].Visible)
            {
                ent.atd_frequenciaFinalAjustada = String.IsNullOrEmpty(txtFrequenciaFinalAjustada.Text) ? 0 : Convert.ToDecimal(txtFrequenciaFinalAjustada.Text);
            }

            #endregion Campos das aulas / frequências

            bool salvarRelatorio;
            ent.atd_avaliacao = RetornaAvaliacao(row, out salvarRelatorio);

            ent.atd_avaliacaoPosConselho = VS_Avaliacao.ava_exibeNotaPosConselho ?
                                           RetornaAvaliacaoPosConselho(row) :
                                           string.Empty;

            ent.atd_semProfessor = false;

            resultado = RetornaResultadoDisciplina(row, ent);

            if (salvarRelatorio && !gvAlunos.Columns[colunaNotaRegencia].Visible)
            {
                ent.atd_relatorio = (VS_Nota_Relatorio.Find
                                      (p => (p.Id == (tur_id.ToString() + ";"
                                                      + tud_id.ToString() + ";"
                                                      + alu_id.ToString() + ";"
                                                      + mtu_id.ToString() + ";"
                                                      + mtd_id.ToString() + ";"
                                                      + atd_id.ToString())))).Valor;

                string arq_idRelatorio = (VS_Nota_Relatorio.Find
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

            ent.atd_frequencia = Convert.ToDecimal(((HiddenField)row.FindControl("hdnFrequencia")).Value);
            ent.atd_numeroAulas = Convert.ToInt32(((HiddenField)row.FindControl("hdnQtAulas")).Value);
            ent.atd_numeroAulasReposicao = Convert.ToInt32(((HiddenField)row.FindControl("hdnQtAulasReposicao")).Value);

            listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
            {
                entity = ent,
                resultado = resultado,
                mtu_idAnterior = mtu_id,
                mtd_idAnterior = mtd_id
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
                    tud_id = Convert.ToInt64(dataKeys[0]);
                    mtd_id = Convert.ToInt32(dataKeys[1]);
                    dispensadisciplina = Convert.ToBoolean(Convert.ToByte(dataKeys[2]));
                    atd_id = Convert.ToInt32(dataKeys[3]);

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

                    ent.fav_id = VS_fav_id;
                    ent.ava_id = VS_ava_id;
                    ent.tpc_id = VS_tpc_id;
                    ent.atd_situacao = dispensadisciplina ? (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Excluido : (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                    // Setar o registroExterno para false.
                    ent.atd_registroexterno = false;

                    ent.atd_numeroFaltas = 0;
                    ent.atd_numeroAulas = 0;
                    ent.atd_ausenciasCompensadas = 0;
                    ent.atd_frequencia = 0;

                    salvarRelatorio = false;
                    ent.atd_avaliacao = RetornaAvaliacao(rptItem, out salvarRelatorio);
                    ent.atd_avaliacaoPosConselho = VS_Avaliacao.ava_exibeNotaPosConselho ? RetornaAvaliacaoPosConselho(rptItem) : string.Empty;
                    ent.atd_semProfessor = false;

                    if (salvarRelatorio)
                    {
                        ent.atd_relatorio = (VS_Nota_Relatorio.Find(p => (p.Id == commandArgument))).Valor;
                        string arq_idRelatorio = (VS_Nota_Relatorio.Find(p => (p.Id == commandArgument))).arq_idRelatorio;
                        ent.arq_idRelatorio = string.IsNullOrEmpty(arq_idRelatorio) ? 0 : Convert.ToInt64(arq_idRelatorio);
                    }
                    else
                    {
                        ent.atd_relatorio = string.Empty;
                        ent.arq_idRelatorio = 0;
                    }

                    ent.atd_numeroAulasReposicao = 0;
                    ent.atd_numeroFaltasReposicao = 0;

                    listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                    {
                        entity = ent,
                        resultado = resultado,
                        mtu_idAnterior = mtu_id,
                        mtd_idAnterior = mtd_id
                    });
                }
            }

            return listaDisciplina;
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
                && _UCFechamento.VS_NomeAvaliacaoRecuperacaoFinal != "")
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
                    item = new ListItem(_UCFechamento.VS_NomeAvaliacaoRecuperacaoFinal, "9");
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
        private void SetaComponentesRelatorioLinhaGrid(GridViewRow Row, bool exibeCampoNotaAluno, bool posConselho)
        {
            ImageButton btnRelatorio = (ImageButton)Row.FindControl(posConselho ? "btnRelatorioPosConselho" : "btnRelatorio");
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            bool mostraBotaoRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios && exibeCampoNotaAluno;

            // Deixar com display na tela para poder acessar por javascript.
            if (btnRelatorio != null)
            {
                if (mostraBotaoRelatorio)
                {
                    btnRelatorio.Visible = true;
                    btnRelatorio.CommandArgument = gvAlunos.DataKeys[Row.RowIndex]["tur_id"] + ";" +
                                                   gvAlunos.DataKeys[Row.RowIndex]["tud_id"] + ";" +
                                                   gvAlunos.DataKeys[Row.RowIndex]["alu_id"] + ";" +
                                                   gvAlunos.DataKeys[Row.RowIndex]["mtu_id"] + ";" +
                                                   gvAlunos.DataKeys[Row.RowIndex]["mtd_id"] + ";" +
                                                   (gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] != DBNull.Value ?
                                                    gvAlunos.DataKeys[Row.RowIndex]["AvaliacaoID"] : "-1");

                    btnRelatorio.ToolTip = tipo == EscalaAvaliacaoTipo.Relatorios ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                    // Pesquisa o item pelo id.
                    UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
                    AtualizaIconesStatusPreenchimentoRelatorio(Row, posConselho, true, nota.Valor, nota.arq_idRelatorio);
                }
                else
                {
                    btnRelatorio.Visible = false;
                    AtualizaIconesStatusPreenchimentoRelatorio(Row, posConselho, false, "", "");
                }
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

        /// <summary>
        /// Seta componentes relacionados à observação do aluno na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        private void SetaComponenteObservacaoConselhoLinhaGrid(GridViewRow Row, CLS_AlunoAvaliacaoTurmaObservacao observacao, byte resultado)
        {
            Image imgObservacaoConselhoSituacao = (Image)Row.FindControl("imgObservacaoConselhoSituacao");
            // se a observacao for nula, significa que o usuario nao tinha permissao para alterar o periodo, entao nao houve alteracao
            if (imgObservacaoConselhoSituacao != null && observacao != null && observacao != new CLS_AlunoAvaliacaoTurmaObservacao())
            {
                string qualidade = observacao.ato_qualidade;
                string desempenho = observacao.ato_desempenhoAprendizado;
                string recomendacaoAluno = observacao.ato_recomendacaoAluno;
                string recomendacaoResp = observacao.ato_recomendacaoResponsavel;

                List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);
                int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
                avaliacaoUltimoPerido = ((AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.Periodica
                                            || (AvaliacaoTipo)VS_Avaliacao.ava_tipo == AvaliacaoTipo.PeriodicaFinal)
                                            && VS_Avaliacao.tpc_id > 0 && VS_Avaliacao.tpc_id == tpc_idUltimoPerido;

                // mostra a situacao da observacao se pelo menos um campo foi preenchido
                imgObservacaoConselhoSituacao.Visible = !(
                                                            string.IsNullOrEmpty(qualidade)
                                                            && string.IsNullOrEmpty(desempenho)
                                                            && string.IsNullOrEmpty(recomendacaoAluno)
                                                            && string.IsNullOrEmpty(recomendacaoResp)
                                                        )
                                                        || (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && avaliacaoUltimoPerido && resultado > 0);

                if (imgObservacaoConselhoSituacao.Visible)
                {
                    // todos os campos preenchidos
                    if (!string.IsNullOrEmpty(desempenho)
                        && !string.IsNullOrEmpty(recomendacaoAluno)
                        && !string.IsNullOrEmpty(recomendacaoResp)
                        && (!avaliacaoUltimoPerido || resultado > 0))
                    {
                        imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmar.png";
                    }
                    else
                    {
                        imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmarAmarelo.png";
                    }
                }
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

            HiddenField hdnSituacao = (HiddenField)row.FindControl("hdnSituacao");
            int situacao = Convert.ToInt32(hdnSituacao.Value);

            HiddenField hdnDispensaDisciplina = (HiddenField)row.FindControl("hdnDispensaDisciplina");
            bool dispensadisciplina = hdnDispensaDisciplina.Value == "1";

            HiddenField hdnQtAulas = (HiddenField)row.FindControl("hdnQtAulas");
            if (hdnQtAulas != null)
            {
                int.TryParse(hdnQtAulas.Value, out aat_numeroAulas);
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

            TextBox txtFrequenciaAjustada = (TextBox)row.FindControl("txtFrequenciaFinalAjustada");
            if (txtFrequenciaAjustada != null)
            {
                txtFrequenciaAjustada.Text =
                    string.Format(
                        VS_FormatacaoDecimaisFrequencia
                        , CLS_AlunoAvaliacaoTurmaBO.RetornaFrequenciaAjustadaCalculada(
                            Tud_id,
                            VS_tur_id,
                            alu_id,
                            mtu_id,
                            VS_fav_id,
                            VS_ava_id,
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
                        if (dispensadisciplina)
                            row.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
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
                    if (dispensadisciplina)
                        row.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                }
            }
        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        /// <param name="adicional">Indica se serão carregados os pareceres da avaliação adicional</param>
        private void CarregarPareceres(DropDownList ddlPareceres)
        {
            ListItem li = new ListItem("-- Selecione um conceito --", "-1;-1", true);
            ddlPareceres.Items.Add(li);

            foreach (ACA_EscalaAvaliacaoParecer eap in LtPareceres)
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
        private int RetornaOrdemParecer(string eap_valor)
        {
            // Busca o campo Ordem de acordo com o valor do parecer.
            var x = from ACA_EscalaAvaliacaoParecer eap in LtPareceres
                    where eap.eap_valor.Equals(eap_valor)
                    select eap.eap_ordem;

            if (x.Count() > 0)
            {
                return x.First();
            }

            return -1;
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

            if (VS_Nota_Relatorio.Exists(p => p.Id == idRelatorio))
            {
                int alterar = VS_Nota_Relatorio.FindIndex(p => p.Id == idRelatorio);
                string arq_idRelatorio = visivelAnexo ? VS_Nota_Relatorio.Find(p => p.Id == idRelatorio).arq_idRelatorio : string.Empty;
                arq_idRelatorioSalvo = arq == null ? arq_idRelatorio : arq.arq_id.ToString();

                VS_Nota_Relatorio[alterar] = new UCFechamento.NotasRelatorio
                {
                    Id = idRelatorio,
                    Valor = nota,
                    arq_idRelatorio = arq_idRelatorioSalvo
                };
            }
            else
            {
                arq_idRelatorioSalvo = arq == null ? string.Empty : arq.arq_id.ToString();
                UCFechamento.NotasRelatorio rel = new UCFechamento.NotasRelatorio
                {
                    Id = idRelatorio,
                    Valor = nota,
                    arq_idRelatorio = arq_idRelatorioSalvo
                };
                VS_Nota_Relatorio.Add(rel);
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar atualizar a frequência acumulada.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os dados na tela.
        /// </summary>
        /// <returns></returns>
        public void SetaProcessamentoConcluido()
        {
            lblMessageInfo.Visible = true;
            _UCComboOrdenacao1.Visible = true;
            gvAlunos.Visible = true;
            lblMessageInfo2.Visible = true;
            //
            LoadFromEntity();
            MostraTelaAlunos();
        }
        
        /// <summary>
        /// Verifica se o grupo do usuário logado possui permissão para visualizar a coluna de observação do fechamento.
        /// </summary>
        public void VerificarPermissaoVisualizarObservacoesFechamento()
        {
           
        }

        #endregion Métodos

        #region Eventos Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HtmlTableCell cellAlunoFrequencia = lnAlunoFrequencia.Cells[0];
                if (cellAlunoFrequencia != null)
                {
                    cellAlunoFrequencia.BgColor = ApplicationWEB.AlunoFrequenciaLimite;
                }

                HtmlTableCell cellAlunoProximoBaixaFrequencia = lnAlunoProximoBaixaFrequencia.Cells[0];
                if (cellAlunoProximoBaixaFrequencia != null)
                {
                    cellAlunoProximoBaixaFrequencia.BgColor = ApplicationWEB.CorAlunoProximoBaixaFrequencia;
                }

                HtmlTableCell cell = lnInativos.Cells[0];
                if (cell != null)
                {
                    cell.BgColor = ApplicationWEB.AlunoInativo;
                }

                lnAlunoDispensado.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                HtmlTableCell cellDispensado = lnAlunoDispensado.Cells[0];
                if (cellDispensado != null)
                {
                    cellDispensado.BgColor = ApplicationWEB.AlunoDispensado;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    SetaEventosTxtFrequenciaAcumulada();

                    VerificarPermissaoVisualizarObservacoesFechamento();
                }

                _UCComboOrdenacao1._OnSelectedIndexChange += ReCarregarGridAlunos;
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
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
                            , (_UCFechamento.VS_NomeAvaliacaoRecuperacaoFinal != "" && VS_Avaliacao.ava_tipo != (byte)AvaliacaoTipo.RecuperacaoFinal).ToString().ToLower()
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
            EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
            string nomeNota = "Nota";

            if ((_UCFechamento.VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
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

            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblTituloResultadoEfetivacao = (Label)e.Row.FindControl("lblTituloResultadoEfetivacao");

                if (lblTituloResultadoEfetivacao != null)
                    lblTituloResultadoEfetivacao.Text = GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO").ToString();

                ((Literal)e.Row.FindControl("litNotaRegencia")).Text = nomeNota;
                ((LinkButton)e.Row.FindControl("btnExpandir")).ToolTip = "Expandir para todos os alunos";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "alu_id"));
                ImageButton btnObservacaoConselho = (ImageButton)e.Row.FindControl("btnObservacaoConselho");
                Image imgObservacaoConselhoSituacao = (Image)e.Row.FindControl("imgObservacaoConselhoSituacao");
                ImageButton btnBoletim = (ImageButton)e.Row.FindControl("btnBoletim");

                if (btnBoletim != null)
                {
                    btnBoletim.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (btnObservacaoConselho != null)
                {
                    btnObservacaoConselho.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (imgObservacaoConselhoSituacao != null && (AvaliacaoTipo)VS_Avaliacao.ava_tipo != AvaliacaoTipo.RecuperacaoFinal)
                {
                    byte observacaoConselhoPreenchida = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "observacaoConselhoPreenchida"));
                    byte resultado = DataBinder.Eval(e.Row.DataItem, "mtu_resultado") == DBNull.Value ? (byte)0 : Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "mtu_resultado").ToString());
                    imgObservacaoConselhoSituacao.Visible = observacaoConselhoPreenchida == 1 // todos os campos preenchidos
                                                            || observacaoConselhoPreenchida == 2 // algum campo preenchido
                                                            || (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && avaliacaoUltimoPerido && resultado > 0);
                    if (imgObservacaoConselhoSituacao.Visible)
                    {
                        if (observacaoConselhoPreenchida == 1
                            && (!avaliacaoUltimoPerido || resultado > 0))
                        {
                            imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmar.png";
                        }
                        else
                        {
                            imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmarAmarelo.png";
                        }
                    }
                }

                Image imgStatusFechamento = (Image)e.Row.FindControl("imgStatusFechamento");
                if (imgStatusFechamento != null)
                {
                    //verifica se o aluno tem pendencia de fechamento
                    if (lstAlunosPendentes != null && lstAlunosPendentes.Any(p => p.aluId == alu_id))
                    {
                        imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "statusAlertaPendencia.png";
                        if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            List<UCFechamento.AlunoDisciplina> lstAlunoDisciplina = lstAlunosPendentes.FindAll(p => p.aluId == Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "alu_id"))
                                                                                                    && !String.IsNullOrEmpty(p.nomeDisciplina));
                            if (lstAlunoDisciplina.Any())
                            {
                                string disciplinas = string.Empty;
                                for (int i = 0; i < lstAlunoDisciplina.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        disciplinas += lstAlunoDisciplina[i].nomeDisciplina;
                                    }
                                    else if (i == lstAlunoDisciplina.Count - 1)
                                    {
                                        disciplinas += " e " + lstAlunoDisciplina[i].nomeDisciplina;
                                    }
                                    else
                                    {
                                        disciplinas += ", " + lstAlunoDisciplina[i].nomeDisciplina;
                                    }
                                }
                                imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip.Regencia").ToString(), nomeNota.ToLower(), disciplinas);
                            }
                            else
                            {
                                imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip").ToString(), nomeNota.ToLower());
                            }
                        }
                        else
                        {
                            imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip").ToString(), nomeNota.ToLower());
                        }
                    }
                    else
                    {
                        imgStatusFechamento.Visible = false;
                    }
                }

                TextBox txtQtdeFalta = (TextBox)e.Row.FindControl("txtQtdeFalta");
                if (txtQtdeFalta != null)
                {
                    txtQtdeFalta.Text = DataBinder.Eval(e.Row.DataItem, "QtFaltasAluno").ToString();
                }

                ImageButton btnFaltasExternas = (ImageButton)e.Row.FindControl("btnFaltasExternas");
                if (btnFaltasExternas != null && listaFrequenciaExterna != null && listaFrequenciaExterna.Count > 0)
                {
                    int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[e.Row.RowIndex]["mtu_id"]);
                    int mtd_id = Convert.ToInt32(gvAlunos.DataKeys[e.Row.RowIndex]["mtd_id"]);

                    CLS_AlunoFrequenciaExterna ext = listaFrequenciaExterna.Find(p => p.alu_id == alu_id && p.mtu_id == mtu_id && p.mtd_id == mtd_id);
                    if (ext != null)
                    {
                        btnFaltasExternas.Visible = true;
                        btnFaltasExternas.OnClientClick =
                            "AbrePopupFrequenciaExterna('" + ext.afx_qtdAulas + "','" + ext.afx_qtdFaltas +
                                "'); return false;";
                    }
                }

                LinkButton btnRelatorioAEE = (LinkButton)e.Row.FindControl("btnRelatorioAEE");
                if (btnRelatorioAEE != null)
                {
                    btnRelatorioAEE.Visible = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "alu_situacaoID")) == (byte)ACA_AlunoSituacao.Ativo
                                                && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "PossuiDeficiencia"));
                    btnRelatorioAEE.CommandArgument = alu_id.ToString();
                }

                // Mostra o ícone para as anotações de recuperação paralela (RP):
                // - para todos os alunos, quando a turma for de recuperação paralela,
                // - ou apenas para alunos com anotações de RP, quando for a turma regular relacionada com a recuperação paralela.
                if (VS_Turma.tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno
                    || lstAlunosRelatorioRP.Any(p => p.alu_id == alu_id))
                {
                    LinkButton btnRelatorioRP = (LinkButton)e.Row.FindControl("btnRelatorioRP");
                    if (btnRelatorioRP != null)
                    {
                        btnRelatorioRP.Visible = true;
                        btnRelatorioRP.CommandArgument = string.Format("{0};-1", alu_id.ToString());
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
            else if (e.CommandName == "ObservacaoConselho")
            {
                try
                {
                    if (this.AbrirObservacaoConselho != null)
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        long alu_id = Convert.ToInt64(gvAlunos.DataKeys[index].Values["alu_id"].ToString());
                        int mtu_id = Convert.ToInt32(gvAlunos.DataKeys[index].Values["mtu_id"].ToString());
                        AbrirObservacaoConselho(index, alu_id, mtu_id
                                                , GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.gvAlunos.ColunaRegistroConselho")
                                                    + " - " + VS_Avaliacao.ava_nome
                                                , VS_Avaliacao.tpc_id);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a observação do aluno.", UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
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
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o boletim completo do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "RelatorioRP")
            {
                try
                {
                    if (AbrirRelatorioRP != null)
                    {
                        string[] args = e.CommandArgument.ToString().Split(';');
                        AbrirRelatorioRP(Convert.ToInt64(args[0]), args[1]);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir as anotações da recuperação paralela para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "RelatorioAEE")
            {
                try
                {
                    if (AbrirRelatorioAEE != null)
                    {
                        AbrirRelatorioAEE(Convert.ToInt64(e.CommandArgument.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir os relatórios do AEE para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        public void ddlTurmaDisciplina_SelectedIndexChanged()
        {
            VS_BuscouTiposResultados = false;

            bool permiteConsultar = true;
            if (!_UCFechamento.TurmaDisciplina_Ids[0].Equals("-1") && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                permiteConsultar = VS_turmaDisciplinaCompartilhada != null || _UCFechamento.VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoConsulta);
            }

            if (permiteConsultar)
                MostraTelaAlunos();
            else
                EscondeTelaAlunos("Docente não possui permissão para consultar notas do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionado(a).");
        }

        public void UCComboAvaliacao1_IndexChanged()
        {
            try
            {
                if (VS_ava_id > 0)
                {
                    bool permiteConsultar = true;
                    if (!_UCFechamento.TurmaDisciplina_Ids[0].Equals("-1") && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        permiteConsultar = VS_turmaDisciplinaCompartilhada != null || _UCFechamento.VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoConsulta);
                    }

                    if (permiteConsultar)
                    {
                        // se mesmo apos o processamento do handler, ainda existir registro na fila
                        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                        {
                            int tempoProcessar = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA_TEMPO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                            if (tempoProcessar > 0)
                            {
                                lblMessageInfo.Visible = false;
                                _UCComboOrdenacao1.Visible = false;
                                lblQtdeAulasPrevistas.Visible = false;
                                lblQtdeAulasDadas.Visible = false;
                                lblTotalAulasExperiencia.Visible = false;
                                gvAlunos.Visible = false;
                                lblMessageInfo2.Visible = false;
                                divLegenda.Visible = false;
                                //
                                if (MostrarLoading != null)
                                {
                                    MostrarLoading(tempoProcessar);
                                }
                            }
                            else
                            {
                                // mostra uma mensagem de que nao foi possivel processar
                                EscondeTelaAlunos(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamento.MensagemErroProcessamento").ToString());
                            }
                        }
                        else if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_VERIFICAR_PENDENCIAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                        {
                            VerificaPendenciaFilaProcessamento();
                        }
                        else
                        {
                            lblMessageInfo.Visible = true;
                            _UCComboOrdenacao1.Visible = true;
                            lblQtdeAulasPrevistas.Visible = false;
                            lblQtdeAulasDadas.Visible = false;
                            lblTotalAulasExperiencia.Visible = false;
                            gvAlunos.Visible = true;
                            lblMessageInfo2.Visible = true;
                            //
                            LoadFromEntity();
                            MostraTelaAlunos();

                        }
                    }
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
                TrataErro(ex, lblMessage, "carregar os dados");
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

        public void UCAlunoEfetivacaoObservacao_ReturnValues(Int32 indiceAluno, CLS_AlunoAvaliacaoTurmaObservacao observacao, byte resultado)
        {
            try
            {
                GridViewRow row = gvAlunos.Rows[indiceAluno];
                SetaComponenteObservacaoConselhoLinhaGrid(row, observacao, resultado);
                uppGridAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
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
            else if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
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
                                DataBinder.Eval(e.Item.DataItem, "dispensadisciplina").ToString() + ";" +
                                (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString()) ?
                                    DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString() : "-1");

                // Deixar com display na tela para poder acessar por javascript.
                if (btnRelatorio != null)
                {
                    if (mostraBotaoRelatorio)
                    {
                        btnRelatorio.Visible = true;
                        btnRelatorio.CommandArgument = commandArgument;
                        btnRelatorio.ToolTip = mostraBotaoRelatorio ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                        // Pesquisa o item pelo id.
                        UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
                        AtualizaIconesStatusPreenchimentoRelatorio(e.Item, false, true, nota.Valor, nota.arq_idRelatorio);
                    }
                    else
                    {
                        btnRelatorio.Visible = false;
                        AtualizaIconesStatusPreenchimentoRelatorio(e.Item, false, false, "", "");
                    }
                }

                //
                // BOTAO RELATORIO NOTA POS-CONSELHO
                //
                ImageButton btnRelatorioPosConselho = (ImageButton)e.Item.FindControl("btnRelatorioPosConselho");
                // Deixar com display na tela para poder acessar por javascript.
                if (btnRelatorioPosConselho != null)
                {
                    if (mostraBotaoRelatorio)
                    {
                        btnRelatorioPosConselho.Visible = true;
                        btnRelatorioPosConselho.CommandArgument = commandArgument;
                        btnRelatorioPosConselho.ToolTip = mostraBotaoRelatorio ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                        // Pesquisa o item pelo id.
                        UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == btnRelatorioPosConselho.CommandArgument);
                        AtualizaIconesStatusPreenchimentoRelatorio(e.Item, true, true, nota.Valor, nota.arq_idRelatorio);
                    }
                    else
                    {
                        btnRelatorioPosConselho.Visible = false;
                        AtualizaIconesStatusPreenchimentoRelatorio(e.Item, true, false, "", "");
                    }
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
        }

        #endregion Eventos
    }
}