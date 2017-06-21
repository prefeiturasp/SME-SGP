using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Fechamento
{
    public partial class UCFechamentoFinal : MotherUserControl
    {
        #region Constantes

        // constantes criadas para a ordem das colunas do grid de efetivação de notas
        private const int colunaStatus = 0;
        //private const int colunaNumeroChamada = 1;
        //private const int colunaNomeAluno = 2;
        private const int colunaPeriodos = 3;

        private const int colunaNotaRegencia = 4;
        private const int colunaFrequenciaAjustada = 5;
        private const int colunaNotaFinal = 6;
        private const int colunaParecerFinal = 7;
        private const int colunaParecerConclusivo = 8;
        private const int colunaBoletim = 9;

        private bool[] visibilidadeColunas = new bool[10] { true, true, true, true, true, true, true, true, true, true };

        // constantes criadas para a ordem das colunas da tabela de efetivação de notas
        // para os componentes da Regencia
        private const int colunaComponenteRegenciaPeriodos = 1;

        private const int colunaComponenteRegenciaNotaFinal = 2;

        private bool[] visibilidadeColunasComponenteRegencia = new bool[3] { true, true, true };

        private const short criterioFrequenciaFinalAjustadaDisciplina = 6;

        #endregion Constantes

        #region Propriedades

        #region Armazenadas no ViewState

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

        /// <summary>
        /// Guarda se existe aluno com nota não efetivada em
        /// algum período que ele estava presente,
        /// </summary>
        private bool VS_SemNota
        {
            get
            {
                return
                    (bool)
                    (
                        ViewState["VS_SemNota"] ??
                        (
                            ViewState["VS_SemNota"] = false
                        )
                    );
            }
            set
            {
                ViewState["VS_SemNota"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do tipo de período do calendário do último bimestre periódico
        /// </summary>
        private int VS_tpc_idUltimoBimestre
        {
            get
            {
                if (ViewState["VS_tpc_idUltimoBimestre"] == null)
                {
                    List<Struct_CalendarioPeriodos> dtPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);
                    ViewState["VS_tpc_idUltimoBimestre"] = dtPeriodos.Count > 0 ? dtPeriodos.Last().tpc_id : -1;
                }

                return Convert.ToInt32(ViewState["VS_tpc_idUltimoBimestre"]);
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
        private List<AlunosFechamentoFinalComponenteRegencia> listaAlunosComponentesRegencia;

        /// <summary>
        /// DataTable com as notas e frequência finais das avaliações periódicas
        /// </summary>
        private List<AlunosFechamentoFinal> listaFinalAvaliacoesPeriodicas = new List<AlunosFechamentoFinal>();

        /// <summary>
        /// DataTable com as frequência finais da ultima avaliação periódica da Regência,
        /// que ainda não está efetivada, para ser salva também.
        /// </summary>
        private List<AlunosFechamentoFinal> listaFinalUltimaAvaliacaoPeriodicaRegencia = new List<AlunosFechamentoFinal>();

        /// <summary>
        /// Nome da avaliação final para colocar na coluna
        /// </summary>
        private string nomeAvaliacaoFinal;

        /// <summary>
        /// Guarda o tabIndex para setar a ordem na atribuição da nota final
        /// </summary>
        private short tabIndexNotaFinal = 1;

        /// <summary>
        /// Object da query com os nomes das avaliações periódicas
        /// </summary>
        private object objAvaliacoesPeriodicas;

        #endregion Usadas no DataBound

        #region Parâmetros acadêmicos
        
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

        #endregion Parâmetros acadêmicos

        /// <summary>
        /// Retorna a propriedade indicando se a tela estará habilitada para salvar.
        /// </summary>
        public bool TelaHabilitadaEdicao
        {
            get
            {
                return (_UCFechamento.usuarioPermissao && _UCFechamento.DocentePodeEditar)
                    && pnlAlunos.Visible
                    && !periodoFechado
                    && _UCFechamento.usuarioPermissao
                    && !VS_SemNota;
            }
        }

        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool periodoFechado
        {
            get
            {
                return !VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal && p.vigente);
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
                return uppAlunos;
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

        private Label lblFixMessage
        {
            get
            {
                return _UCFechamento.LblFixMessage;
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

        private List<UCFechamento.AlunoDisciplina> lstAlunosPendentes = new List<UCFechamento.AlunoDisciplina>();

        /// <summary>
        /// Tipo do colaborador selecionado no gridview para edição
        /// </summary>
        private bool VS_LancaParecerFinal
        {
            get
            {
                if (ViewState["VS_LancaParecerFinal"] != null)
                    return Convert.ToBoolean(ViewState["VS_LancaParecerFinal"]);
                return true;
            }
            set
            {
                ViewState["VS_LancaParecerFinal"] = value;
            }
        }

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

        public delegate void commandMostrarSalvar();

        public event commandMostrarSalvar MostrarSalvar;

        public delegate void commandAbrirRelatorioRP(long alu_id);

        public event commandAbrirRelatorioRP AbrirRelatorioRP;

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
            var listaVerificacao = from T item in lista
                                   let propInfoAvaliacaoID = item.GetType().GetProperty("AvaliacaoID")
                                   let propInfoAvaliacao = item.GetType().GetProperty("Avaliacao")
                                   let propInfoAvaliacaoPosConselho = item.GetType().GetProperty("avaliacaoPosConselho")
                                   let propInfoAvaliacaoResultado = item.GetType().GetProperty("AvaliacaoResultado")
                                   let propAluId = item.GetType().GetProperty("alu_id")
                                   select new
                                   {
                                       AvaliacaoID = Convert.ToInt64(propInfoAvaliacaoID.GetValue(item, null) ?? "0")
                                       ,
                                       Avaliacao = (propInfoAvaliacao.GetValue(item, null) ?? string.Empty).ToString()
                                       ,
                                       AvaliacaoResultado = Convert.ToByte(propInfoAvaliacaoResultado.GetValue(item, null) ?? "0")
                                       ,
                                       aluId = Convert.ToInt64(propAluId.GetValue(item, null) ?? "-1")
                                       ,
                                       disNome = componenteRegencia ? (item.GetType().GetProperty("dis_nome").GetValue(item, null) ?? string.Empty).ToString() : string.Empty
                                   }
                                   ;

            bool retorno = false;

            if (!tud_naoLancarNota && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
            {
                var filtroPendentes = listaVerificacao.Where(dadosGeral =>
                                        // existe aluno sem fechamento
                                        ((dadosGeral.AvaliacaoID <= 0
                                          // ou a nota do aluno no fechamento esta vazia
                                          || string.IsNullOrEmpty(dadosGeral.Avaliacao)) && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semResultadoFinal))
                                            // ou nota incompativel com o tipo de escala de avaliacao
                                        || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica
                                            &&
                                            (
                                                !string.IsNullOrEmpty(dadosGeral.Avaliacao)
                                                && string.IsNullOrEmpty(NotaFormatada(dadosGeral.Avaliacao))
                                            )
                                        )
                                        || ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Pareceres
                                            &&
                                            (
                                                !string.IsNullOrEmpty(dadosGeral.Avaliacao)
                                                && !LtPareceres.Any(p => p.eap_valor.Equals(dadosGeral.Avaliacao))
                                            )
                                        )
                                    );

                if (filtroPendentes.Any())
                {
                    foreach (var alunoPendente in filtroPendentes)
                    {
                        UCFechamento.AlunoDisciplina alunoDisciplina = new UCFechamento.AlunoDisciplina();
                        alunoDisciplina.aluId = alunoPendente.aluId;
                        alunoDisciplina.nomeDisciplina = alunoPendente.disNome;
                        lstAlunosPendentes.Add(alunoDisciplina);
                    }

                    retorno = true;
                }
            }
            // Caso não exista o lançamento de nota
            else
            {
                // Só faz a verificação de pendência, caso seja possível lançar o parecer final
                if (VS_LancaParecerFinal)
                {
                    var filtroPendentes = listaVerificacao.Where(dadosGeral =>
                    // existe aluno sem fechamento
                                        (dadosGeral.AvaliacaoID <= 0 && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semResultadoFinal))
                                        // ou nao possui parecer final selecionado
                                        || (
                                                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                && dadosGeral.AvaliacaoResultado <= 0 && !VS_ListConfiguracaoServicoPendencia.Any(p => p.csp_semResultadoFinal)
                                            )
                                        );

                    if (filtroPendentes.Any())
                    {
                        foreach (var alunoPendente in filtroPendentes)
                        {
                            UCFechamento.AlunoDisciplina alunoDisciplina = new UCFechamento.AlunoDisciplina();
                            alunoDisciplina.aluId = alunoPendente.aluId;
                            alunoDisciplina.nomeDisciplina = alunoPendente.disNome;
                            lstAlunosPendentes.Add(alunoDisciplina);
                        }

                        retorno = true;
                    }
                }
            }

            return retorno;
        }

        /// <summary>
        /// Carrega o grid com os alunos por turma ou disciplina.
        /// </summary>
        private void CarregarGridAlunos()
        {
            List<AlunosFechamentoFinal> lista = new List<AlunosFechamentoFinal>();

            #region Calcula numero Casas decimais da frequencia

            VS_FormatacaoDecimaisFrequencia = "{" + GestaoEscolarUtilBO.CriaFormatacaoDecimal
                ((VS_FormatoAvaliacao.fav_variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(VS_FormatoAvaliacao.fav_variacao) : 2), "0:{0}")
                + "}";

            #endregion Calcula numero Casas decimais da frequencia

            if (Tud_id > 0)
            {
                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                // Busca os alunos por disciplina na turma.
                lista = VS_DisciplinaEspecial ?
                    MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamentoFiltroDeficienciaFinal
                    (
                        Tud_id
                        , VS_tur_id
                        , VS_ava_id
                        , Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue)
                        , VS_fav_id
                        , VS_Turma.tur_tipo
                        , VS_Turma.cal_id
                        , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        , _UCFechamento.VS_tipoDocente
                        , false
                        , ApplicationWEB.AppMinutosCacheFechamento
                        , VS_listaTur_ids
                    ) :
                    MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamentoFinal
                    (
                        Tud_id
                        , VS_tur_id
                        , VS_ava_id
                        , Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue)
                        , VS_fav_id
                        , VS_Turma.tur_tipo
                        , VS_Turma.cal_id
                        , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        , false
                        , ApplicationWEB.AppMinutosCacheFechamento
                        , VS_listaTur_ids
                    );

                // Se for disciplina de regencia, carrego os dados referentes aos componentes da regencia
                if (lista.Any() && EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    var alunos = lista
                            .Select(p => new
                            {
                                tpc_id = p.tpc_id
                                ,
                                alu_id = p.alu_id
                                ,
                                mtu_id = p.mtu_id
                            })
                            .Where(p => p.tpc_id == -1);

                    DataTable dtAlunos = new DataTable();
                    dtAlunos.Columns.Add("alu_id");
                    dtAlunos.Columns.Add("mtu_id");
                    foreach (var aluno in alunos)
                    {
                        if (!dtAlunos.AsEnumerable().Any(a => Convert.ToInt64(a["alu_id"]) == aluno.alu_id && Convert.ToInt32(a["mtu_id"]) == aluno.mtu_id))
                        {
                            DataRow drAluno = dtAlunos.NewRow();
                            drAluno["alu_id"] = aluno.alu_id;
                            drAluno["mtu_id"] = aluno.mtu_id;
                            dtAlunos.Rows.Add(drAluno);
                        }
                    }

                    listaAlunosComponentesRegencia = MTR_MatriculaTurmaDisciplinaBO.GetSelectFechamentoComponentesRegenciaFinal
                                                    (
                                                        VS_tur_id
                                                        , VS_ava_id
                                                        , VS_fav_id
                                                        , VS_Turma.cal_id
                                                        , dtAlunos
                                                        , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                        , ApplicationWEB.AppMinutosCacheFechamento
                                                    );
                }
            }

            if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
            {
                UpdateSemNota(listaAlunosComponentesRegencia);
            }
            else if (Tud_id > 0)
            {
                UpdateSemNota(lista);
            }

            try
            {
                // Se for disciplina de regencia, carrego os dados referentes aos componentes da regencia
                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    if (listaAlunosComponentesRegencia != null)
                    {
                        listaFinalAvaliacoesPeriodicas = (from dadosGeral in listaAlunosComponentesRegencia
                                                          where dadosGeral.tpc_id > 0
                                                          select new AlunosFechamentoFinal
                                                          {
                                                              AvaliacaoID = dadosGeral.AvaliacaoID
                                                              ,
                                                              Avaliacao = dadosGeral.Avaliacao
                                                              ,
                                                              AvaliacaoPosConselho = dadosGeral.AvaliacaoPosConselho
                                                              ,
                                                              AvaliacaoResultado = dadosGeral.AvaliacaoResultado
                                                              ,
                                                              NomeAvaliacao = dadosGeral.NomeAvaliacao
                                                              ,
                                                              alu_id = dadosGeral.alu_id
                                                              ,
                                                              mtu_id = dadosGeral.mtu_id
                                                              ,
                                                              mtd_id = dadosGeral.mtd_id
                                                              ,
                                                              tud_id = dadosGeral.tud_id
                                                              ,
                                                              tur_id = dadosGeral.tur_id
                                                              ,
                                                              AlunoForaDaRede = dadosGeral.AlunoForaDaRede
                                                              ,
                                                              ava_id = dadosGeral.ava_id
                                                              ,
                                                              UltimoPeriodo = dadosGeral.UltimoPeriodo
                                                          }).ToList();
                    }

                    try
                    {
                        // Guardo a avaliação do ultimo período que não ainda não foi efetivada,
                        // para salvar também.
                        listaFinalUltimaAvaliacaoPeriodicaRegencia = (from dadosGeral in lista
                                                                      where dadosGeral.UltimoPeriodo == 1
                                                                           && dadosGeral.AvaliacaoID <= 0
                                                                      select dadosGeral).ToList();
                    }
                    catch
                    {
                        listaFinalUltimaAvaliacaoPeriodicaRegencia = new List<AlunosFechamentoFinal>();
                    }
                }
                else
                {
                    listaFinalAvaliacoesPeriodicas = (from dadosGeral in lista
                                                      where dadosGeral.tpc_id > 0
                                                      select dadosGeral).ToList();
                }
            }
            catch
            {
                listaFinalAvaliacoesPeriodicas = new List<AlunosFechamentoFinal>();
            }

            objAvaliacoesPeriodicas = listaFinalAvaliacoesPeriodicas
                                       .Select(row => new
                                       {
                                           NomeAvaliacao = row.NomeAvaliacao
                                       })
                                       .Distinct();

            List<AlunosFechamentoFinal> listaAvaliacaoFinal;
            try
            {
                listaAvaliacaoFinal = (from dadosGeral in lista
                                       where dadosGeral.tpc_id == -1
                                       select dadosGeral).ToList();
            }
            catch
            {
                listaAvaliacaoFinal = new List<AlunosFechamentoFinal>();
            }

            List<AlunosFechamentoFinalComponenteRegencia> listaAvaliacaoFinalRegencia = new List<AlunosFechamentoFinalComponenteRegencia>();
            if (lista != null && lista.Any() && Tud_id > 0)
            {
                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    if (listaAlunosComponentesRegencia != null && listaAlunosComponentesRegencia.Any())
                    {
                        try
                        {
                            listaAvaliacaoFinalRegencia = (from tRow in listaAlunosComponentesRegencia
                                                           where tRow.tpc_id == -1
                                                           select tRow).ToList();
                        }
                        catch
                        {
                            listaAvaliacaoFinalRegencia = new List<AlunosFechamentoFinalComponenteRegencia>();
                        }
                    }
                }
                SetaDadosRelatorio(listaFinalAvaliacoesPeriodicas, "atd_relatorio");
            }

            bool pendenciaFechamento = false;
            lstAlunosPendentes = new List<UCFechamento.AlunoDisciplina>();
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_STATUS_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                // Exibe a mensagem de pendencia na efetivacao apos o inicio do evento de fechamento final,
                // ou apos o termino do evento de fechamento. 
                &&
                (VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal && p.vigente)
                    || VS_ListaEventos.Exists(p => p.tev_id == tev_EfetivacaoFinal && !p.vigente && p.evt_dataFim < DateTime.Now)))
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
                    if (listaAvaliacaoFinalRegencia != null
                        && listaAvaliacaoFinalRegencia.Any())
                    {
                        pendenciaFechamento = RetornaAlunosFechamentoPendente(listaAvaliacaoFinalRegencia, tud_naoLancarNota, true);
                        this._UCFechamento.AtualizarStatusEfetivacao(pendenciaFechamento, tud_naoLancarNota);
                    }
                }
                else if (listaAvaliacaoFinal != null && listaAvaliacaoFinal.Any())
                {
                    pendenciaFechamento = RetornaAlunosFechamentoPendente(listaAvaliacaoFinal, tud_naoLancarNota, false);
                    this._UCFechamento.AtualizarStatusEfetivacao(pendenciaFechamento, tud_naoLancarNota);
                }
            }

            // Seta a visibilidade das colunas do grid de acordo com o tipo de avaliação.
            SetaColunasVisiveisGrid(VS_Avaliacao, pendenciaFechamento);

            // Seta nome dos headers das colunas de nota..
            nomeAvaliacaoFinal = listaAvaliacaoFinal.Any() ? listaAvaliacaoFinal[0].NomeAvaliacao.ToString() : "";

            lblMsgRepeater.Visible = !listaAvaliacaoFinal.Any();
            tabIndexNotaFinal = 1;

            if (VS_Turma.tur_tipo == (byte)TUR_TurmaTipo.Normal)
            {
                lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(-1, VS_Turma.tur_id, Tud_id, ApplicationWEB.AppMinutosCacheMedio);
            }

            // Ordenação dos alunos.
            int numeroChamada;
            rptAlunos.DataSource = Convert.ToInt32(_UCComboOrdenacao1._Combo.SelectedValue) == 0 ?
                                    listaAvaliacaoFinal.OrderBy(p => Int32.TryParse(p.mtd_numeroChamada, out numeroChamada) ? numeroChamada : -1).ThenBy(p => p.pes_nome).ToList()
                                    : listaAvaliacaoFinal.OrderBy(p => p.pes_nome).ToList();
            rptAlunos.DataBind();

            rptAlunos.Visible = rptAlunos.Items.Count > 0;
            divLegenda.Visible = rptAlunos.Items.Count > 0;

            if (ExisteDispensaDisciplina)
                lblMessageInfo.Text = UtilBO.GetErroMessage("Alunos com dispensa de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                                            " terão suas notas e frequência desconsideradas.", UtilBO.TipoMensagem.Informacao);
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
        /// <param name="ddlPareceres">Combo de pareceres</param>
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres)
        {
            if (txtNota != null)
            {
                if (tipo == EscalaAvaliacaoTipo.Numerica)
                {
                    txtNota.Visible = true;
                    txtNota.Text = NotaFormatada(aat_avaliacao);
                }
                else
                {
                    txtNota.Visible = false;
                }
            }

            if (ddlPareceres != null)
            {
                ddlPareceres.Visible = tipo == EscalaAvaliacaoTipo.Pareceres;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    if (ddlPareceres.Items.Count == 0)
                    {
                        CarregarPareceres(ddlPareceres);
                    }

                    // Encontra parecer
                    ListItem parecer = ddlPareceres.Items.FindByText(aat_avaliacao);
                    if (parecer != null)
                    {
                        ddlPareceres.SelectedValue = parecer.Value;
                    }
                    else
                    {
                        ddlPareceres.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Atribui a nota no label, atribuindo o tooltip no caso de nota por conceito.
        /// </summary>
        /// <param name="tipo">Tipo da escala de avaliação</param>
        /// <param name="lblNota">Label de nota</param>
        /// <param name="aat_avaliacao">Valor da nota</param>
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, Label lblNota, string aat_avaliacao)
        {
            if (lblNota != null)
            {
                lblNota.Text = lblNota.ToolTip = string.Empty;
                ((HtmlControl)lblNota.Parent).Attributes.Remove("title");

                if (tipo == EscalaAvaliacaoTipo.Numerica)
                {
                    lblNota.Text = NotaFormatada(aat_avaliacao);
                }
                else if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    var conceitos = LtPareceres
                                        .Select(eap => new
                                        {
                                            eap_valor = eap.eap_valor
                                            ,
                                            descricao = eap.descricao
                                        })
                                        .Where(row => row.eap_valor.ToUpper().Equals(aat_avaliacao.ToUpper()));

                    if (conceitos.Count() > 0)
                    {
                        lblNota.Text = aat_avaliacao;
                        lblNota.ToolTip = conceitos.First().descricao;
                        ((HtmlControl)lblNota.Parent).Attributes.Add("title", lblNota.ToolTip);
                    }
                }
                if (lblNota.Text == string.Empty)
                {
                    lblNota.Text = "-";
                }
            }
        }

        /// <summary>
        /// Seta dados da opção de nota por relatório e dados da tela de acordo com os dados do
        /// DataTable informado.
        /// </summary>
        /// <param name="dt">DataTable para pegar os dados</param>
        /// <param name="nomeColunaRelatorio">Nome da coluna onde tem a nota por relatório</param>
        private void SetaDadosRelatorio<T>(List<T> lista, string nomeColunaRelatorio)
        {
            _UCComboOrdenacao1.Visible = false;

            if (lista.Any())
            {
                PropertyInfo propInfo;

                foreach (T item in lista)
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
                    string relatorio = (propInfo.GetValue(item, null) ?? string.Empty).ToString();

                    propInfo = item.GetType().GetProperty("arq_idRelatorio");
                    string arq_idRelatorio = (propInfo.GetValue(item, null) ?? string.Empty).ToString();

                    UCFechamento.NotasRelatorio rel = new UCFechamento.NotasRelatorio
                    {
                        Id = id,
                        Valor = relatorio,
                        arq_idRelatorio = arq_idRelatorio
                    };

                    VS_Nota_Relatorio.Add(rel);
                }

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
            TurmaDisciplinaTipo tipoDisciplina = (TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo;

            bool disciplinaEletivaAluno = tipoDisciplina == TurmaDisciplinaTipo.DisciplinaEletivaAluno;
            visibilidadeColunas[colunaNotaRegencia] = tipoDisciplina == TurmaDisciplinaTipo.Regencia && ExibirItensRegencia && !tud_naoLancarNota;
            visibilidadeColunas[colunaPeriodos] = !visibilidadeColunas[colunaNotaRegencia] && tipoDisciplina != TurmaDisciplinaTipo.Regencia;
            visibilidadeColunas[colunaPeriodos] &= (!tud_naoLancarNota
                                                    || (tud_naoLancarNota && !tud_naoLancarFrequencia && tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                                                    || (disciplinaEletivaAluno && !tud_naoLancarFrequencia));
            visibilidadeColunas[colunaFrequenciaAjustada] = (disciplinaEletivaAluno || (tud_naoLancarNota && (!ExibirItensRegencia || tipoDisciplina != TurmaDisciplinaTipo.ComponenteRegencia)));
            visibilidadeColunas[colunaFrequenciaAjustada] &= !tud_naoLancarFrequencia;
            visibilidadeColunas[colunaFrequenciaAjustada] &= tipoDisciplina != TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;
            visibilidadeColunas[colunaNotaFinal] = !visibilidadeColunas[colunaNotaRegencia] && !tud_naoLancarNota && !disciplinaEletivaAluno;
            
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

            visibilidadeColunas[colunaParecerConclusivo] = possuiPermissaoVisualizacao;

            visibilidadeColunas[colunaParecerFinal] = VS_LancaParecerFinal && (tud_naoLancarNota || disciplinaEletivaAluno);
            visibilidadeColunas[colunaBoletim] = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_COLUNA_BOLETIM_MANUTENCAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            // Faz a verificação de permissão de acesso ao boletim, apenas se for docente
            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                visibilidadeColunas[colunaBoletim] &= (VS_turmaDisciplinaCompartilhada == null && _UCFechamento.VS_ltPermissaoBoletim.Any(p => p.pdc_permissaoConsulta));
            }

            //
            // VISIBILIDADE DAS COLUNAS DOS COMPONENTES DA REGENCIA
            //
            if (visibilidadeColunas[colunaNotaRegencia])
            {
                // Mais uma condição nas colunas de nota = checkbox de professor checada.
                visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaPeriodos] =
                visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaFinal] = true;
            }

            visibilidadeColunas[colunaStatus] = pendenciaFechamento;
        }

        /// <summary>
        /// Recarrega o repeater de alunos.
        /// </summary>
        private void ReCarregarGridAlunos()
        {
            CarregarGridAlunos();
        }

        /// <summary>
        /// Carrega a mensagem das disciplinas divergentes por alunos, após o retorno do save do conceito global
        /// </summary>
        /// <param name="listDisciplinasDivergentesPorAluno">Lista de disciplinas divergentes por aluno</param>
        private void CarregaGridAlunosComDisciplinasDivergentes(List<sDisciplinasDivergentesPorAluno> listDisciplinasDivergentesPorAluno)
        {
            CarregarGridAlunos();
            List<string> disciplinasDivergentes;

            foreach (RepeaterItem rptItem in rptAlunos.Items)
            {
                Label lblAluno = (Label)rptItem.FindControl("lblAluno");
                long alu_id = Convert.ToInt64(((HiddenField)rptItem.FindControl("hfAluId")).Value);

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
            List<long> alunosDivergentes = (from RepeaterItem rptItem in rptAlunos.Items
                                            let alu_id = Convert.ToInt64(((HiddenField)rptItem.FindControl("hfAluId")).Value)
                                            where listAlunosComDivergencia.Exists(p => p == alu_id)
                                            select alu_id).ToList();

            foreach (RepeaterItem rptItem in rptAlunos.Items)
            {
                Label lblAluno = (Label)rptItem.FindControl("lblAluno");
                Label lblNomeAluno = (Label)rptItem.FindControl("lblNomeAluno");
                long alu_id = Convert.ToInt64(((HiddenField)rptItem.FindControl("hfAluId")).Value);

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
                    dadosAluno += "<br /><b>" + (string)GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text") + ":</b> " + dis_nome;

                UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == id);
                AbrirRelatorio(id, nota.Valor, nota.arq_idRelatorio, dadosAluno);
            }
        }

        /// <summary>
        /// Mostra a mensagem de alerta e habilita o formulário de acordo
        /// com os dados sobre o preenchimento das notas dos alunos
        /// </summary>
        private void UpdateSemNota<T>(List<T> lista)
        {
            if (lista != null)
            {
                if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
                {
                    var x = from T item in lista
                            let SemNota = item.GetType().GetProperty("SemNota")
                            select new
                            {
                                SemNota = Convert.ToInt32(SemNota.GetValue(item, null) ?? "1")
                            };

                    bool semNota = x.Any(p => p.SemNota == 1);

                    if (!semNota && EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && listaAlunosComponentesRegencia != null)
                    {
                        semNota = listaAlunosComponentesRegencia.Any(p => p.SemNota == 1);
                    }

                    //semNota = false;
                    if (semNota)
                    {
                        List<string> nomesAvaliacao = EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && listaAlunosComponentesRegencia != null ?
                                               (from item in listaAlunosComponentesRegencia
                                                where item.SemNota == 1 && item.tpc_id > 0
                                                orderby item.tpc_ordem
                                                select item.NomeAvaliacao).Distinct().ToList() :
                                               (from item in lista
                                                let SemNota = item.GetType().GetProperty("SemNota")
                                                let Tpc_id = item.GetType().GetProperty("tpc_id")
                                                let NomeAvaliacao = item.GetType().GetProperty("NomeAvaliacao")
                                                let Tpc_ordem = item.GetType().GetProperty("tpc_ordem")
                                                where Convert.ToInt32(SemNota.GetValue(item, null) ?? "1") == 1 && Convert.ToInt32(Tpc_id.GetValue(item, null) ?? "-1") > 0
                                                orderby Convert.ToInt32(Tpc_ordem.GetValue(item, null) ?? "0")
                                                select (NomeAvaliacao.GetValue(item, null) ?? string.Empty).ToString()).Distinct().ToList();

                        string nomeAvaliacao = string.Empty;
                        for (int i = 0; i < nomesAvaliacao.Count; i++)
                        {
                            if (i == 0)
                            {
                                nomeAvaliacao += nomesAvaliacao[i];
                            }
                            else if (i == nomesAvaliacao.Count - 1)
                            {
                                nomeAvaliacao += " e " + nomesAvaliacao[i];
                            }
                            else
                            {
                                nomeAvaliacao += ", " + nomesAvaliacao[i];
                            }
                        }

                        // Exibe mensagem
                        lblFixMessage.Text = UtilBO.GetErroMessage(String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoFinal.lblFixMessage.Text").ToString(), nomeAvaliacao), UtilBO.TipoMensagem.Alerta);
                    }

                    VS_SemNota = semNota;
                }
                else
                {
                    VS_SemNota = false;
                }
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
            rptAlunos.DataSource = new DataTable();
            rptAlunos.DataBind();
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
                EscondeTelaAlunos((string)GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoFinal.lblMessage.Text.MsgPendenciaProcessamento"), UtilBO.TipoMensagem.Informacao);
            }
        }

        /// <summary>
        /// Mostra o grid de alunos e botões de salvar.
        /// </summary>
        private void MostraTelaAlunos()
        {
            pnlAlunos.Visible = true;
            _UCFechamento.VisibleBotaoSalvar = !periodoFechado && _UCFechamento.usuarioPermissao;
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
        /// Seta enabled nos controles da tela, de acordo com o parâmetro.
        /// </summary>
        /// <param name="value">Indica se campo será habilitado/desabilitado</param>
        private void HabilitarControlesTela(bool value)
        {
            if (!value)
            {
                // desabilitar todos os controles da lista de alunos
                HabilitaControles(pnlAlunos.Controls, value);
            }

            HabilitaControles(_UCFechamento.FdsRelatorio.Controls, value);
            _UCFechamento.VisibleBotaoSalvar = value;

            foreach (RepeaterItem rptItem in rptAlunos.Items)
            {
                Repeater rptComponenteRegencia = (Repeater)rptItem.FindControl("rptComponenteRegencia");

                TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
                if (txtNotaFinal != null)
                {
                    txtNotaFinal.Enabled = value;
                }

                DropDownList ddlPareceresFinal = (DropDownList)rptItem.FindControl("ddlPareceresFinal");
                if (ddlPareceresFinal != null)
                {
                    ddlPareceresFinal.Enabled = value;
                }

                DropDownList ddlResultado = (DropDownList)rptItem.FindControl("ddlResultado");
                ddlResultado.Enabled = value && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ((ImageButton)rptItem.FindControl("btnObservacaoConselho")).Enabled = value;
                ((ImageButton)rptItem.FindControl("btnBoletim")).Enabled = value;
                ((LinkButton)rptItem.FindControl("btnRelatorioRP")).Enabled = value;

                foreach (RepeaterItem rptItemRegencia in rptComponenteRegencia.Items)
                {
                    txtNotaFinal = (TextBox)rptItemRegencia.FindControl("txtNotaFinal");
                    if (txtNotaFinal != null)
                    {
                        txtNotaFinal.Enabled = value;
                    }

                    ddlPareceresFinal = (DropDownList)rptItemRegencia.FindControl("ddlPareceresFinal");
                    if (ddlPareceresFinal != null)
                    {
                        ddlPareceresFinal.Enabled = value;
                    }
                }
            }

            if (!value)
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
            foreach (RepeaterItem rptItem in rptAlunos.Items)
            {
                ImageButton btnBoletim = (ImageButton)rptItem.FindControl("btnBoletim");
                if (btnBoletim != null)
                {
                    btnBoletim.Enabled = true;
                }

                ImageButton btnObservacaoConselho = (ImageButton)rptItem.FindControl("btnObservacaoConselho");
                if (btnObservacaoConselho != null)
                {
                    btnObservacaoConselho.Enabled = true;
                }

                LinkButton btnRelatorioRP = (LinkButton)rptItem.FindControl("btnRelatorioRP");
                if (btnRelatorioRP != null)
                {
                    btnRelatorioRP.Enabled = true;
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
                VS_Nota_Relatorio = new List<UCFechamento.NotasRelatorio>();
                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                // Por padrão é realizado o lançamento do parecer final
                VS_LancaParecerFinal = true;

                // Caso a disciplina seja do tipo "Experiência", só lança parecer final se a disciplina for oferecida no último período do calendário
                if ((TurmaDisciplinaTipo)EntTurmaDisciplina.tud_tipo == TurmaDisciplinaTipo.Experiencia)
                {
                    VS_LancaParecerFinal = TUR_TurmaDisciplinaTerritorioBO.VerificaOferecimentoExperienciaBimestre(Tud_id, VS_Turma.cal_id, VS_tpc_idUltimoBimestre);
                }

                // Carregar grid.
                CarregarGridAlunos();
            }
            catch (ThreadAbortException) { }
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
                bool notaRegencia;
                ValidaGeraDados(out listaDisciplina, out notaRegencia);

                if (Tud_id > 0)
                {
                    CLS_AlunoAvaliacaoTurmaDisciplinaBO.Save(
                        VS_Turma,
                        Tud_id,
                        VS_FormatoAvaliacao,
                        listaDisciplina,
                        ApplicationWEB.TamanhoMaximoArquivo,
                        ApplicationWEB.TiposArquivosPermitidos,
                        (ACA_FormatoAvaliacaoTipoLancamentoFrequencia)VS_FormatoAvaliacao.fav_tipoLancamentoFrequencia,
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
                        notaRegencia,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update,
                        _UCFechamento.VS_MensagemLogEfetivacao
                        + "tur_id: " + VS_tur_id.ToString()
                        + "; tud_id: " + Tud_id.ToString()
                        + "; ava_id: " + VS_ava_id.ToString()
                    );
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

                if (tipoMsg == UtilBO.TipoMensagem.Alerta)
                {
                    lblMessage2.Text = UtilBO.GetErroMessage(TrocaParametroMensagem(msgValidacao), tipoMsg);
                }
            }
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
        private void ValidaGeraDados(out List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina, out bool notaRegencia)
        {
            listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();

            notaRegencia = false;
            List<string> alunosErroIntervalo = new List<string>(),
                         alunosErroConversao = new List<string>();
            string stringErro = string.Empty;

            // Se a escala de avaliação é numérica.
            bool tipoEscalaNumerica = false;
            if ((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica)
            {
                // Traz os valores limite para a validação da nota.
                tipoEscalaNumerica = true;
            }

            foreach (RepeaterItem rptItem in rptAlunos.Items)
            {
                HtmlControl hcNotaRegencia = (HtmlControl)rptItem.FindControl("tdNotaRegencia");
                HtmlControl hcNotaFinal = (HtmlControl)rptItem.FindControl("tdNotaFinal");

                if (tipoEscalaNumerica)
                {
                    if (!hcNotaRegencia.Visible)
                    {
                        if (hcNotaFinal.Visible)
                        {
                            // Recupera o valor da avaliação normal.
                            TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
                            if (txtNotaFinal != null && !string.IsNullOrEmpty(txtNotaFinal.Text) && txtNotaFinal.Enabled)
                            {
                                decimal nota;
                                if (decimal.TryParse(txtNotaFinal.Text, out nota))
                                {
                                    if ((nota < VS_EscalaNumerica.ean_menorValor) || (nota > VS_EscalaNumerica.ean_maiorValor))
                                    {
                                        alunosErroIntervalo.Add(((Label)rptItem.FindControl("lblNomeAluno")).Text);
                                    }
                                }
                                else
                                {
                                    alunosErroConversao.Add(((Label)rptItem.FindControl("lblNomeAluno")).Text);
                                }
                            }
                        }
                    }
                    else
                    {
                        Repeater rptComponenteRegencia = (Repeater)rptItem.FindControl("rptComponenteRegencia");
                        // Guarda se possui algum erro de validacao na nota de algum dos componentes da regencia
                        bool erroIntervaloNota = false;
                        bool erroConversaoNota = false;
                        //
                        foreach (RepeaterItem rptItemRegencia in rptComponenteRegencia.Items)
                        {
                            hcNotaFinal = (HtmlControl)rptItemRegencia.FindControl("tdNotaFinal");
                            if (hcNotaFinal.Visible)
                            {
                                // Recupera o valor da avaliação final.
                                TextBox txtNotaFinal = (TextBox)rptItemRegencia.FindControl("txtNotaFinal");
                                if (txtNotaFinal != null && !string.IsNullOrEmpty(txtNotaFinal.Text) && txtNotaFinal.Enabled)
                                {
                                    decimal nota;
                                    if (decimal.TryParse(txtNotaFinal.Text, out nota))
                                    {
                                        if ((nota < VS_EscalaNumerica.ean_menorValor) || (nota > VS_EscalaNumerica.ean_maiorValor))
                                        {
                                            erroIntervaloNota |= true;
                                        }
                                    }
                                    else
                                    {
                                        erroConversaoNota |= true;
                                    }
                                }
                            }
                        }
                        if (erroIntervaloNota)
                        {
                            alunosErroIntervalo.Add(((Label)rptItem.FindControl("lblNomeAluno")).Text);
                        }
                        if (erroConversaoNota)
                        {
                            alunosErroConversao.Add(((Label)rptItem.FindControl("lblNomeAluno")).Text);
                        }
                    }
                }
                notaRegencia = hcNotaRegencia.Visible;

                if (Tud_id > 0)
                {
                    AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                }
            }

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

            if (alunosErroConversao.Count == 1)
            {
                stringErro += string.Format("Nota para o aluno {0} é inválida.", string.Join(", ", alunosErroConversao.ToArray()));
            }
            else if (alunosErroConversao.Count > 1)
            {
                stringErro += string.Format("Nota para os alunos {0} são inválidas.", string.Join(", ", alunosErroConversao.ToArray()));
            }

            if (!string.IsNullOrEmpty(stringErro))
            {
                throw new ValidationException(stringErro);
            }
        }

        /// <summary>
        /// Retorna o enum de resultado, verificando se é necessário calcular o resultado automaticamente, ou se será
        /// utilizado o valor do combo da tela.
        /// </summary>
        /// <param name="row">Linha do grid</param>
        /// <param name="ent">Entidade da avaliação na turma</param>
        /// <returns></returns>
        private MtrTurmaDisciplinaResultado RetornaResultadoDisciplina(RepeaterItem rptItem, CLS_AlunoAvaliacaoTurmaDisciplina ent)
        {
            MtrTurmaDisciplinaResultado resultado = 0;
            HtmlControl hcParecerFinal = (HtmlControl)rptItem.FindControl("tdParecerFinal");
            if (hcParecerFinal.Visible)
            {
                DropDownList ddlResultado = (DropDownList)rptItem.FindControl("ddlResultado");
                byte valor = Convert.ToByte(ddlResultado.SelectedValue == "-1" ? "0" : ddlResultado.SelectedValue);
                resultado = (MtrTurmaDisciplinaResultado)Convert.ToByte(valor);
            }
            return resultado;
        }

        /// <summary>
        /// Adiciona uma linha na lista com os dados da linha do grid.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        /// <param name="listaDisciplina"></param>
        private void AdicionaLinhaDisciplina(RepeaterItem rptItem, ref List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina)
        {
            long tur_id = VS_tur_id;
            long tud_id = Tud_id;
            long alu_id = 0;
            int mtu_id = 0;
            int mtd_id = 0;

            if (long.TryParse(((HiddenField)rptItem.FindControl("hfAluId")).Value, out alu_id)
                && int.TryParse(((HiddenField)rptItem.FindControl("hfMtuId")).Value, out mtu_id)
                && int.TryParse(((HiddenField)rptItem.FindControl("hfMtdId")).Value, out mtd_id))
            {
                int atd_id;
                MtrTurmaDisciplinaResultado resultado = 0;

                if (!int.TryParse(((HiddenField)rptItem.FindControl("hfAvaliacaoId")).Value, out atd_id))
                    atd_id = -1;

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
                ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                // Setar o registroExterno para false.
                ent.atd_registroexterno = false;

                #region Campos das aulas / frequências

                HtmlControl hcFrequenciaAjustada = (HtmlControl)rptItem.FindControl("tdFrequenciaFinal");
                Label lblFrequenciaFinalAjustada = (Label)rptItem.FindControl("lblFrequenciaFinalAjustada");
                if (hcFrequenciaAjustada.Visible)
                {
                    ent.atd_frequenciaFinalAjustada = String.IsNullOrEmpty(lblFrequenciaFinalAjustada.Text) ? 0 : Convert.ToDecimal(lblFrequenciaFinalAjustada.Text);
                }

                #endregion Campos das aulas / frequências

                HtmlControl hcNotaFinal = (HtmlControl)rptItem.FindControl("tdNotaFinal");
                if (hcNotaFinal.Visible)
                {
                    bool salvarRelatorio;
                    ent.atd_avaliacao = RetornaAvaliacaoFinal(rptItem, out salvarRelatorio);
                }

                ent.arq_idRelatorio = 0;
                ent.atd_avaliacaoPosConselho = string.Empty;
                ent.atd_semProfessor = false;
                ent.tpc_id = -1;

                resultado = RetornaResultadoDisciplina(rptItem, ent);
                listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                {
                    entity = ent,
                    resultado = resultado,
                    mtu_idAnterior = mtu_id,
                    mtd_idAnterior = mtd_id
                });

                Repeater rptItemPeriodos;

                //
                // Adiciona as disciplinas componentes da regencia
                //
                HtmlControl hcNotaRegencia = (HtmlControl)rptItem.FindControl("tdNotaRegencia");
                if (hcNotaRegencia.Visible)
                {
                    Repeater rptComponenteRegencia = (Repeater)rptItem.FindControl("rptComponenteRegencia");
                    resultado = 0;
                    foreach (RepeaterItem rptItemRegencia in rptComponenteRegencia.Items)
                    {
                        long tud_idRegencia = 0;
                        int mtd_idRegencia = 0;
                        int atd_idRegencia = 0;

                        long.TryParse(((HiddenField)rptItemRegencia.FindControl("hfTudId")).Value, out tud_idRegencia);
                        int.TryParse(((HiddenField)rptItemRegencia.FindControl("hfMtdId")).Value, out mtd_idRegencia);
                        int.TryParse(((HiddenField)rptItemRegencia.FindControl("hfAvaliacaoId")).Value, out atd_idRegencia);

                        string commandArgument;

                        ent = new CLS_AlunoAvaliacaoTurmaDisciplina
                        {
                            tud_id = tud_idRegencia,
                            alu_id = alu_id,
                            mtu_id = mtu_id,
                            mtd_id = mtd_idRegencia,
                            atd_id = atd_idRegencia,
                            IsNew = atd_id <= 0
                        };

                        ent.fav_id = VS_fav_id;
                        ent.ava_id = VS_ava_id;
                        ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                        // Setar o registroExterno para false.
                        ent.atd_registroexterno = false;

                        ent.atd_numeroFaltas = 0;
                        ent.atd_numeroAulas = 0;
                        ent.atd_ausenciasCompensadas = 0;
                        ent.atd_frequencia = 0;

                        hcNotaFinal = (HtmlControl)rptItemRegencia.FindControl("tdNotaFinal");
                        if (hcNotaFinal.Visible)
                        {
                            bool salvarRelatorio;
                            ent.atd_avaliacao = RetornaAvaliacaoFinal(rptItemRegencia, out salvarRelatorio);
                        }

                        ent.arq_idRelatorio = 0;
                        ent.atd_avaliacaoPosConselho = string.Empty;
                        ent.atd_semProfessor = false;
                        ent.tpc_id = -1;

                        listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                        {
                            entity = ent,
                            resultado = resultado,
                            mtu_idAnterior = mtu_id,
                            mtd_idAnterior = mtd_id
                        });

                        // se o ultimo periodo ainda não foi efetivado,
                        // ele também deve ser salvo.
                        rptItemPeriodos = (Repeater)rptItemRegencia.FindControl("rptItemPeriodos");
                        resultado = 0;
                        if (rptItemPeriodos.Items.Count > 0)
                        {
                            RepeaterItem rptItemPeriodo = rptItemPeriodos.Items[rptItemPeriodos.Items.Count - 1];
                            HiddenField hfAvaId = (HiddenField)rptItemPeriodo.FindControl("hfAvaId");
                            if (!String.IsNullOrEmpty(hfAvaId.Value))
                            {
                                ent = new CLS_AlunoAvaliacaoTurmaDisciplina
                                {
                                    tud_id = tud_idRegencia,
                                    alu_id = alu_id,
                                    mtu_id = mtu_id,
                                    mtd_id = mtd_idRegencia,
                                    atd_id = -1,
                                    IsNew = true
                                };

                                ImageButton btnRelatorio = (ImageButton)rptItemPeriodo.FindControl("btnRelatorio");
                                commandArgument = btnRelatorio.CommandArgument;

                                ent.fav_id = VS_fav_id;
                                ent.ava_id = Convert.ToInt32(hfAvaId.Value);
                                ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                                // Setar o registroExterno para false.
                                ent.atd_registroexterno = false;

                                ent.atd_avaliacaoPosConselho = string.Empty;
                                ent.atd_semProfessor = false;
                                ent.tpc_id = 0;

                                ent.atd_frequencia = 0;
                                ent.atd_numeroFaltas = 0;
                                ent.atd_numeroAulas = 0;
                                ent.atd_ausenciasCompensadas = 0;
                                ent.atd_numeroAulasReposicao = 0;
                                ent.atd_numeroFaltasReposicao = 0;

                                bool salvarRelatorio;
                                ent.atd_avaliacao = RetornaAvaliacao(rptItemPeriodo, out salvarRelatorio);

                                if (salvarRelatorio && btnRelatorio.Visible)
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

                                listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                                {
                                    entity = ent,
                                    resultado = resultado,
                                    mtu_idAnterior = mtu_id,
                                    mtd_idAnterior = mtd_id
                                });
                            }
                        }
                    }
                }

                // se o ultimo periodo ainda não foi efetivado,
                // ele também deve ser salvo.
                rptItemPeriodos = (Repeater)rptItem.FindControl("rptItemPeriodos");
                resultado = 0;
                if (rptItemPeriodos.Items.Count > 0)
                {
                    RepeaterItem rptItemPeriodo = rptItemPeriodos.Items[rptItemPeriodos.Items.Count - 1];
                    HiddenField hfAvaId = (HiddenField)rptItemPeriodo.FindControl("hfAvaId");
                    if (!String.IsNullOrEmpty(hfAvaId.Value))
                    {
                        ent = new CLS_AlunoAvaliacaoTurmaDisciplina
                        {
                            tud_id = tud_id,
                            alu_id = alu_id,
                            mtu_id = mtu_id,
                            mtd_id = mtd_id,
                            atd_id = -1,
                            IsNew = true
                        };

                        HiddenField hfQtFaltas = (HiddenField)rptItemPeriodo.FindControl("hfQtFaltas");
                        HiddenField hfQtAulas = (HiddenField)rptItemPeriodo.FindControl("hfQtAulas");
                        HiddenField hfQtAusenciasCompensadas = (HiddenField)rptItemPeriodo.FindControl("hfQtAusenciasCompensadas");
                        Label lblFrequencia = (Label)rptItemPeriodo.FindControl("lblFrequencia");

                        ent.fav_id = VS_fav_id;
                        ent.ava_id = Convert.ToInt32(hfAvaId.Value);
                        ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                        // Setar o registroExterno para false.
                        ent.atd_registroexterno = false;

                        ent.atd_avaliacaoPosConselho = string.Empty;
                        ent.atd_semProfessor = false;
                        ent.tpc_id = 0;

                        if (!String.IsNullOrEmpty(hfQtFaltas.Value))
                            ent.atd_numeroFaltas = Convert.ToInt32(hfQtFaltas.Value);

                        if (!String.IsNullOrEmpty(hfQtAulas.Value))
                            ent.atd_numeroAulas = Convert.ToInt32(hfQtAulas.Value);

                        if (!String.IsNullOrEmpty(hfQtAusenciasCompensadas.Value))
                            ent.atd_ausenciasCompensadas = Convert.ToInt32(hfQtAusenciasCompensadas.Value);

                        ent.atd_frequencia = String.IsNullOrEmpty(lblFrequencia.Text) || lblFrequencia.Text == "-" ? 0 : Convert.ToDecimal(lblFrequencia.Text);

                        // Se for disciplina de regencia, não salvo a nota
                        if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            ImageButton btnRelatorio = (ImageButton)rptItemPeriodo.FindControl("btnRelatorio");
                            string commandArgument = btnRelatorio.CommandArgument;

                            bool salvarRelatorio;
                            ent.atd_avaliacao = RetornaAvaliacao(rptItemPeriodo, out salvarRelatorio);

                            if (salvarRelatorio && btnRelatorio.Visible)
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
                        }

                        ent.atd_frequenciaFinalAjustada = String.IsNullOrEmpty(lblFrequenciaFinalAjustada.Text) ? 0 : Convert.ToDecimal(lblFrequenciaFinalAjustada.Text);
                        ent.atd_frequencia = Convert.ToDecimal(((HiddenField)rptItemPeriodo.FindControl("hdnFrequencia")).Value);
                        ent.atd_numeroAulasReposicao = Convert.ToInt32(((HiddenField)rptItemPeriodo.FindControl("hdnQtAulasReposicao")).Value);
                        ent.atd_numeroFaltasReposicao = Convert.ToInt32(((HiddenField)rptItemPeriodo.FindControl("hdnQtFaltasReposicao")).Value);

                        listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                        {
                            entity = ent,
                            resultado = resultado,
                            mtu_idAnterior = mtu_id,
                            mtd_idAnterior = mtd_id
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacaoFinal(Control row, out bool salvarRelatorio)
        {
            TextBox txtNota = (TextBox)row.FindControl("txtNotaFinal");
            DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceresFinal");

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            // Verifica se o lançamento é por relatório ou se é conceito global
            salvarRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            if (txtNota != null && txtNota.Visible)
            {
                return txtNota.Text;
            }
            else if (ddlPareceres != null && ddlPareceres.Visible)
            {
                if (ddlPareceres.SelectedValue.StartsWith("-"))
                {
                    return string.Empty;
                }
                return ddlPareceres.SelectedItem.Text;
            }

            return string.Empty;
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacao(Control row, out bool salvarRelatorio)
        {
            Label lblNota = (Label)row.FindControl("lblNota");
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

            // Verifica se o lançamento é por relatório ou se é conceito global
            salvarRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            if (lblNota != null && lblNota.Visible)
            {
                return lblNota.Text == "-" ? string.Empty : lblNota.Text;
            }

            return string.Empty;
        }

        /// <summary>
        /// Adiciona os itens de resultado no dropDownList.
        /// </summary>
        private void AdicionaItemsResultado(DropDownList ddl, Int64 alu_id, int mtu_id)
        {
            // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
            ListItem item = new ListItem("-- Selecione um " + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.ParecerFinal") + " --", "-1");
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
        /// Seta componentes relacionados ao relatório na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        /// <param name="commandArgument">String que será utilizada como commandArgument do botão do relatório</param>
        /// <param name="btnRelatorio">Botão do relatório</param>
        /// <param name="cvRelatorioDesempenho">Validação do relatório</param>
        /// <param name="imgSituacao">Icone da situação</param>
        /// <param name="hplAnexo">Link do anexo</param>
        private void SetaComponentesRelatorioLinhaGrid(string commandArgument, ImageButton btnRelatorio, Image imgSituacao, HyperLink hplAnexo)
        {
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
            bool mostraBotaoRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            // Deixar com display na tela para poder acessar por javascript.
            if (btnRelatorio != null)
            {
                btnRelatorio.Visible = mostraBotaoRelatorio;

                btnRelatorio.CommandArgument = commandArgument;
                btnRelatorio.ToolTip = tipo == EscalaAvaliacaoTipo.Relatorios ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                // Pesquisa o item pelo id.
                UCFechamento.NotasRelatorio nota = VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
                AtualizaIconesStatusPreenchimentoRelatorio(mostraBotaoRelatorio, nota.Valor, nota.arq_idRelatorio, imgSituacao, hplAnexo);
            }
        }

        /// <summary>
        /// Atualiza os ícones na linha do grid que indicam o status do preenchimento do relatório
        /// </summary>
        /// <param name="mostraBotaoRelatorio">Indica se o botão de relatório é mostrado</param>
        /// <param name="valorNota">Valor da nota do relatório</param>
        /// <param name="arqIdRelatorio">Id do arquivo anexado ao relatório</param>
        /// <param name="imgSituacao">Icone da situação</param>
        /// <param name="hplAnexo">Link do anexo</param>
        private void AtualizaIconesStatusPreenchimentoRelatorio(bool mostraBotaoRelatorio, string valorNota, string arqIdRelatorio, Image imgSituacao, HyperLink hplAnexo)
        {
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
        private void SetaComponenteObservacaoConselhoLinhaGrid(RepeaterItem rptItem, CLS_AlunoAvaliacaoTurmaObservacao observacao, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, byte parecerFinal)
        {
            Image imgObservacaoConselhoSituacao = (Image)rptItem.FindControl("imgObservacaoConselhoSituacao");
            // se a observacao for nula, significa que o usuario nao tinha permissao para alterar o periodo, entao nao houve alteracao
            if (imgObservacaoConselhoSituacao != null && observacao != null && observacao != new CLS_AlunoAvaliacaoTurmaObservacao())
            {
                string qualidade = observacao.ato_qualidade;
                string desempenho = observacao.ato_desempenhoAprendizado;
                string recomendacaoAluno = observacao.ato_recomendacaoAluno;
                string recomendacaoResp = observacao.ato_recomendacaoResponsavel;

                // mostra a situacao da observacao se pelo menos um campo foi preenchido
                imgObservacaoConselhoSituacao.Visible = !(
                                                            string.IsNullOrEmpty(qualidade)
                                                            && string.IsNullOrEmpty(desempenho)
                                                            && string.IsNullOrEmpty(recomendacaoAluno)
                                                            && string.IsNullOrEmpty(recomendacaoResp)
                                                        )
                                                        || (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && resultado > 0);

                if (imgObservacaoConselhoSituacao.Visible)
                {
                    // todos os campos preenchidos
                    if (!string.IsNullOrEmpty(desempenho)
                        && !string.IsNullOrEmpty(recomendacaoAluno)
                        && !string.IsNullOrEmpty(recomendacaoResp)
                        && resultado > 0)
                    {
                        imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmar.png";
                    }
                    else
                    {
                        imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmarAmarelo.png";
                    }
                }
            }

            // Comentado - Usuario nao pode editar mais a nota final e o parecer final na janela do Registro do conselho
            //if (Tud_id > 0 && listaAtualizacaoEfetivacao.Count > 0)
            //{
            //    CLS_AlunoAvaliacaoTurmaDisciplina avaliacaoAluno = new CLS_AlunoAvaliacaoTurmaDisciplina();
            //    if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
            //    {
            //        if (rptItem.FindControl("tdNotaFinal").Visible)
            //        {
            //            avaliacaoAluno = listaAtualizacaoEfetivacao.FirstOrDefault(p => p.tud_id == Tud_id && p.ava_id == VS_ava_id);
            //            if (avaliacaoAluno.tud_id == Tud_id)
            //            {
            //                TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
            //                DropDownList ddlPareceresFinal = (DropDownList)rptItem.FindControl("ddlPareceresFinal");

            //                // Seta campos da avaliação principal.
            //                SetaCamposAvaliacao((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo, txtNotaFinal, avaliacaoAluno.atd_avaliacao, ddlPareceresFinal);
            //            }
            //        }

            //        // Atualiza o parecer final com o que foi salvo na janela
            //        DropDownList ddlResultado = (DropDownList)rptItem.FindControl("ddlResultado");
            //        if (ddlResultado.Visible && parecerFinal > 0 && ddlResultado.Items.FindByValue(parecerFinal.ToString()) != null)
            //        {
            //            ddlResultado.SelectedValue = parecerFinal.ToString();
            //        }
            //    }
            //    else
            //    {
            //        Repeater rptComponenteRegencia = (Repeater)rptItem.FindControl("rptComponenteRegencia");
            //        foreach (RepeaterItem rptItemRegencia in rptComponenteRegencia.Items)
            //        {
            //            if (rptItemRegencia.FindControl("tdNotaFinal").Visible)
            //            {
            //                HiddenField hf = (HiddenField)rptItemRegencia.FindControl("hfTudId");
            //                avaliacaoAluno = listaAtualizacaoEfetivacao.FirstOrDefault(p => p.tud_id == Convert.ToInt64(hf.Value) && p.ava_id == VS_ava_id);
            //                if (avaliacaoAluno.tud_id == Convert.ToInt64(hf.Value))
            //                {
            //                    TextBox txtNotaFinal = (TextBox)rptItemRegencia.FindControl("txtNotaFinal");
            //                    DropDownList ddlPareceresFinal = (DropDownList)rptItemRegencia.FindControl("ddlPareceresFinal");

            //                    // Seta campos da avaliação principal.
            //                    SetaCamposAvaliacao((EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo, txtNotaFinal, avaliacaoAluno.atd_avaliacao, ddlPareceresFinal);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        private void CarregarPareceres(DropDownList ddlPareceres)
        {
            ListItem li = new ListItem("-", "-- Selecione um conceito --", true);
            li.Attributes.Add("title", "-- Selecione um conceito --");
            ddlPareceres.Items.Add(li);

            foreach (ACA_EscalaAvaliacaoParecer eap in LtPareceres)
            {
                li = new ListItem(eap.eap_valor, eap.descricao);
                li.Attributes.Add("title", eap.descricao);
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
            List<ACA_EscalaAvaliacaoParecer> dt = LtPareceres;

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
        /// Carrega os dados na tela.
        /// </summary>
        /// <returns></returns>
        public void SetaProcessamentoConcluido()
        {
            lblMessageInfo.Visible = true;
            _UCComboOrdenacao1.Visible = true;
            lblMsgRepeater.Visible = true;
            rptAlunos.Visible = true;
            //
            LoadFromEntity();
            MostraTelaAlunos();
        }

        #endregion Métodos

        #region Eventos Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Nota atribuída pelo Conselho de Classe
                HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
                if (cell != null)
                {
                    cell.BgColor = ApplicationWEB.CorNotaPosConselho;
                }
                // Aluno fora da rede
                cell = tbLegenda.Rows[1].Cells[0];
                if (cell != null)
                {
                    cell.BgColor = ApplicationWEB.CorAlunoForaDaRede;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _UCComboOrdenacao1._OnSelectedIndexChange += ReCarregarGridAlunos;

                bool tud_naoLancarNota = false;
                if (Tud_id > 0)
                {
                    tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
                }

                // exibir notas
                if (!tud_naoLancarNota && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
                {
                    EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;
                    string nomeNotaAtribuida = "Nota atribuída";

                    if ((_UCFechamento.VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
                    {
                        nomeNotaAtribuida = "Média atribuída";
                    }
                    else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                    {
                        nomeNotaAtribuida = "Conceito atribuído";
                    }
                    else if (tipoEscala == EscalaAvaliacaoTipo.Relatorios)
                    {
                        nomeNotaAtribuida = "Relatório atribuído";
                    }
                    lnNotaConselho.Visible = true;
                    litLegendaNotaConselho.Text = nomeNotaAtribuida + " pelo Conselho de Classe";
                }
                // exibir frequencia
                else
                {
                    lnNotaConselho.Visible = false;
                }
                litLegendaAlunoForaDaRede.Text = GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotasFinal.litLegendaAlunoForaDaRede.Text").ToString();
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

                string script = "var parametro = " + fav.fav_obrigatorioRelatorioReprovacao.ToString().ToLower() + ";" +
                    "var numeroCasasDecimais = " + RetornaNumeroCasasDecimais() + ";" +
                    "var periodoFechado = " + periodoFechado.ToString().ToLower() + ";" +
                    "var arredondamento = " + arredondamento.ToString().ToLower() + ";" +
                    "var variacaoEscala = '" + VS_EscalaNumerica.ean_variacao.ToString().Replace(',', '.') + "';" +
                    "var exibeCorMedia = false;";

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

                sm.Scripts.Add(new ScriptReference("~/Includes/jsAlunoEfetivacaoObservacao.js"));
            }

            HabilitarControlesTela(TelaHabilitadaEdicao);
            pnlAlunos.DefaultButton = _UCFechamento.BtnSalvar.ID;
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void rptAlunos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptPeriodos;
                HtmlControl hcNotaRegencia
                            , hcFrequenciaAjustada
                            , hcNotaFinal
                            , hcParecerConclusivo
                            , hcParecerFinal
                            , hcBoletim
                            , hcStatus;

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

                if (e.Item.ItemType == ListItemType.Header)
                {
                    rptPeriodos = (Repeater)e.Item.FindControl("rptHeaderPeriodos");
                    hcNotaRegencia = (HtmlControl)e.Item.FindControl("thNotaRegencia");
                    hcFrequenciaAjustada = (HtmlControl)e.Item.FindControl("thFrequenciaFinal");
                    hcNotaFinal = (HtmlControl)e.Item.FindControl("thNotaFinal");
                    hcParecerConclusivo = (HtmlControl)e.Item.FindControl("thParecerConclusivo");
                    hcParecerFinal = (HtmlControl)e.Item.FindControl("thParecerFinal");
                    hcBoletim = (HtmlControl)e.Item.FindControl("thBoletim");
                    hcStatus = (HtmlControl)e.Item.FindControl("thStatus");

                    // nome da avaliação final
                    ((Literal)e.Item.FindControl("litHeadNotaFinal")).Text = nomeAvaliacaoFinal;
                    ((Literal)e.Item.FindControl("litHeadNotaRegencia")).Text = nomeNota;
                    ((LinkButton)e.Item.FindControl("btnExpandir")).ToolTip = "Expandir para todos os alunos";

                    if (visibilidadeColunas[colunaPeriodos])
                    {
                        rptPeriodos.DataSource = objAvaliacoesPeriodicas;
                    }
                }
                else
                {
                    rptPeriodos = (Repeater)e.Item.FindControl("rptItemPeriodos");
                    hcNotaRegencia = (HtmlControl)e.Item.FindControl("tdNotaRegencia");
                    hcFrequenciaAjustada = (HtmlControl)e.Item.FindControl("tdFrequenciaFinal");
                    hcNotaFinal = (HtmlControl)e.Item.FindControl("tdNotaFinal");
                    hcParecerConclusivo = (HtmlControl)e.Item.FindControl("tdParecerConclusivo");
                    hcParecerFinal = (HtmlControl)e.Item.FindControl("tdParecerFinal");
                    hcBoletim = (HtmlControl)e.Item.FindControl("tdBoletim");
                    hcStatus = (HtmlControl)e.Item.FindControl("tdStatus");

                    HiddenField hf = (HiddenField)e.Item.FindControl("hfAluId");
                    hf.Value = DataBinder.Eval(e.Item.DataItem, "alu_id").ToString();
                    hf = (HiddenField)e.Item.FindControl("hfMtuId");
                    hf.Value = DataBinder.Eval(e.Item.DataItem, "mtu_id").ToString();
                    hf = (HiddenField)e.Item.FindControl("hfMtdId");
                    hf.Value = DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString();
                    hf = (HiddenField)e.Item.FindControl("hfAvaliacaoId");
                    hf.Value = (DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString());
                    hf = (HiddenField)e.Item.FindControl("hfAlcMatricula");
                    hf.Value = (DataBinder.Eval(e.Item.DataItem, "alc_matricula") ?? string.Empty).ToString();

                    TextBox txtNotaFinal = (TextBox)e.Item.FindControl("txtNotaFinal");
                    DropDownList ddlPareceresFinal = (DropDownList)e.Item.FindControl("ddlPareceresFinal");

                    ImageButton btnObservacaoConselho = (ImageButton)e.Item.FindControl("btnObservacaoConselho");
                    Image imgObservacaoConselhoSituacao = (Image)e.Item.FindControl("imgObservacaoConselhoSituacao");
                    ImageButton btnBoletim = (ImageButton)e.Item.FindControl("btnBoletim");

                    long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                    if (btnBoletim != null)
                    {
                        btnBoletim.CommandArgument = e.Item.ItemIndex.ToString();
                    }

                    if (btnObservacaoConselho != null)
                    {
                        btnObservacaoConselho.CommandArgument = e.Item.ItemIndex.ToString();
                    }

                    if (imgObservacaoConselhoSituacao != null)
                    {
                        byte observacaoConselhoPreenchida = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "observacaoConselhoPreenchida"));
                        byte resultado = DataBinder.Eval(e.Item.DataItem, "mtu_resultado") == DBNull.Value ? (byte)0 : Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "mtu_resultado").ToString());
                        imgObservacaoConselhoSituacao.Visible = observacaoConselhoPreenchida == 1 // todos os campos preenchidos
                                                                || observacaoConselhoPreenchida == 2 // algum campo preenchido
                                                                || (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && resultado > 0);
                        if (imgObservacaoConselhoSituacao.Visible)
                        {
                            if (observacaoConselhoPreenchida == 1
                                && resultado > 0)
                            {
                                imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmar.png";
                            }
                            else
                            {
                                imgObservacaoConselhoSituacao.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmarAmarelo.png";
                            }
                        }
                    }

                    // Seta campos da avaliação principal.
                    SetaCamposAvaliacao(tipoEscala, txtNotaFinal, (DataBinder.Eval(e.Item.DataItem, "Avaliacao") ?? string.Empty).ToString(), ddlPareceresFinal);

                    if (visibilidadeColunas[colunaNotaFinal] && (txtNotaFinal.Visible || ddlPareceresFinal.Visible))
                    {
                        if (txtNotaFinal.Visible)
                        {
                            txtNotaFinal.TabIndex = tabIndexNotaFinal;
                        }
                        else if (ddlPareceresFinal.Visible)
                        {
                            ddlPareceresFinal.TabIndex = tabIndexNotaFinal;
                        }
                        tabIndexNotaFinal++;
                    }

                    if (visibilidadeColunas[colunaParecerFinal])
                    {
                        DropDownList ddlResultado = (DropDownList)e.Item.FindControl("ddlResultado");
                        AdicionaItemsResultado(ddlResultado, alu_id, Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id")));
                        if (ddlResultado != null)
                        {
                            ddlResultado.SelectedValue = DataBinder.Eval(e.Item.DataItem, "AvaliacaoResultado").ToString();
                            ddlResultado.TabIndex = tabIndexNotaFinal;
                            tabIndexNotaFinal++;

                            if (Convert.ToInt32((DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString())) <= 0)
                                ScriptManager.RegisterStartupScript(Page, GetType(), "MostrarSalvar", "$(document).ready(function(){ MostrarSalvar(); });", true);

                            // se nao veio valor do banco,
                            // verifico se posso atribuir o parecer final automaticamente
                            if (ddlResultado.SelectedIndex == 0 
                                && TelaHabilitadaEdicao
                                && VS_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == criterioFrequenciaFinalAjustadaDisciplina 
                                && Convert.ToInt32((DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString())) <= 0)
                            {
                                bool permiteAlterarResultadoFinal = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                if (!permiteAlterarResultadoFinal || VS_FormatoAvaliacao.fav_sugerirResultadoFinalDisciplina)
                                {
                                    if (!DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").Equals(DBNull.Value))
                                    {
                                        string valorResultado = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada"))
                                            >= VS_FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina ?
                                                ((byte)TipoResultado.Aprovado).ToString() : ((byte)TipoResultado.ReprovadoFrequencia).ToString();

                                        if (ddlResultado.Items.FindByValue(valorResultado) != null)
                                            ddlResultado.SelectedValue = valorResultado;
                                    }
                                }
                            }
                        }
                    }

                    if (visibilidadeColunas[colunaPeriodos])
                    {
                        try
                        {
                            rptPeriodos.DataSource = (from tRow in listaFinalAvaliacoesPeriodicas
                                                      where tRow.alu_id == alu_id
                                                      select tRow).ToList();
                        }
                        catch
                        {
                            rptPeriodos.DataSource = new List<AlunosFechamentoFinal>();
                        }
                    }
                    else if (listaFinalUltimaAvaliacaoPeriodicaRegencia.Any())
                    {
                        try
                        {
                            rptPeriodos.DataSource = (from tRow in listaFinalUltimaAvaliacaoPeriodicaRegencia.AsEnumerable()
                                                      where tRow.alu_id == alu_id
                                                      select tRow).ToList();
                        }
                        catch
                        {
                            rptPeriodos.DataSource = new List<AlunosFechamentoFinalComponenteRegencia>();
                        }
                    }

                    Label lblFrequenciaFinalAjustada = (Label)e.Item.FindControl("lblFrequenciaFinalAjustada");
                    if (!DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").Equals(DBNull.Value))
                    {
                        lblFrequenciaFinalAjustada.Text = string.Format(
                            VS_FormatacaoDecimaisFrequencia
                            , Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada"))
                        );
                    }

                    if (visibilidadeColunas[colunaNotaRegencia] && listaAlunosComponentesRegencia != null && listaAlunosComponentesRegencia.Any())
                    {
                        Repeater rptComponenteRegencia = (Repeater)e.Item.FindControl("rptComponenteRegencia");
                        LinkButton btnExpandir = (LinkButton)e.Item.FindControl("btnExpandir");
                        btnExpandir.ToolTip = nomeNota;

                        try
                        {
                            rptComponenteRegencia.DataSource = (from tRow in listaAlunosComponentesRegencia
                                                                where tRow.alu_id == alu_id
                                                                        && tRow.tpc_id == -1
                                                                select tRow);
                        }
                        catch
                        {
                            rptComponenteRegencia.DataSource = new List<AlunosFechamentoFinalComponenteRegencia>();
                        }

                        rptComponenteRegencia.DataBind();
                    }

                    if (visibilidadeColunas[colunaStatus])
                    {
                        Image imgStatusFechamento = (Image)e.Item.FindControl("imgStatusFechamento");
                        if (imgStatusFechamento != null)
                        {
                            //verifica se o aluno tem pendencia de fechamento
                            if (lstAlunosPendentes != null && lstAlunosPendentes.Any(p => p.aluId == alu_id))
                            {
                                imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "statusAlertaPendencia.png";
                                string nomePendencia = string.Empty;
                                if (visibilidadeColunas[colunaNotaFinal] || visibilidadeColunas[colunaNotaRegencia])
                                {
                                    nomePendencia = nomeAvaliacaoFinal.ToLower();
                                }
                                else
                                {
                                    nomePendencia = GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.ParecerFinal").ToString().ToLower();
                                }
                                if (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                {
                                    List<UCFechamento.AlunoDisciplina> lstAlunoDisciplina = lstAlunosPendentes.FindAll(p => p.aluId == alu_id
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
                                        imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip.Regencia").ToString(), nomePendencia, disciplinas);
                                    }
                                    else
                                    {
                                        imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip").ToString(), nomePendencia);
                                    }
                                }
                                else
                                {
                                    imgStatusFechamento.ToolTip = String.Format(GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.imgStatusFechamento.ToolTip").ToString(), nomePendencia);
                                }
                            }
                            else
                            {
                                imgStatusFechamento.Visible = false;
                            }
                        }
                    }

                    // Mostra o ícone para as anotações de recuperação paralela (RP):
                    // - para todos os alunos, quando a turma for de recuperação paralela,
                    // - ou apenas para alunos com anotações de RP, quando for a turma regular relacionada com a recuperação paralela.
                    if (VS_Turma.tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno
                        || lstAlunosRelatorioRP.Any(p => p.alu_id == alu_id))
                    {
                        LinkButton btnRelatorioRP = (LinkButton)e.Item.FindControl("btnRelatorioRP");
                        if (btnRelatorioRP != null)
                        {
                            btnRelatorioRP.Visible = true;
                            btnRelatorioRP.CommandArgument = alu_id.ToString();
                        }
                    }
                }

                rptPeriodos.Visible = visibilidadeColunas[colunaPeriodos];
                hcNotaRegencia.Visible = visibilidadeColunas[colunaNotaRegencia];
                hcFrequenciaAjustada.Visible = visibilidadeColunas[colunaFrequenciaAjustada];
                hcNotaFinal.Visible = visibilidadeColunas[colunaNotaFinal];
                hcParecerConclusivo.Visible = visibilidadeColunas[colunaParecerConclusivo];
                hcParecerFinal.Visible = visibilidadeColunas[colunaParecerFinal];
                hcBoletim.Visible = visibilidadeColunas[colunaBoletim];
                hcStatus.Visible = visibilidadeColunas[colunaStatus];

                if (visibilidadeColunas[colunaPeriodos] || listaFinalUltimaAvaliacaoPeriodicaRegencia.Any())
                {
                    rptPeriodos.DataBind();
                }
            }
        }

        protected void rptAlunos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Relatorio")
            {
                Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                string pes_nome = ((Label)(rowControl.FindControl("lblAluno"))).Text;
                hdnLocalImgCheckSituacao.Value = ((RepeaterItem)rowControl).ItemIndex.ToString();
                TrataEventoCommandRelatorio(e.CommandArgument.ToString(), pes_nome);
            }
            else if (e.CommandName == "ObservacaoConselho")
            {
                try
                {
                    if (this.AbrirObservacaoConselho != null)
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                        hdnLocalImgCheckSituacao.Value = ((RepeaterItem)rowControl).ItemIndex.ToString();
                        long alu_id = Convert.ToInt64(((HiddenField)(rowControl.FindControl("hfAluId"))).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)(rowControl.FindControl("hfMtuId"))).Value);

                        string ava_nome = string.Empty;
                        if (VS_tpc_idUltimoBimestre > 0)
                        {
                            using (DataTable dtAvaliacao = ACA_AvaliacaoBO.SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato(VS_FormatoAvaliacao.fav_id))
                            {
                                if (dtAvaliacao.Rows.Count > 0)
                                {
                                    DataRow[] rows = dtAvaliacao.Select("tpc_id = " + VS_tpc_idUltimoBimestre.ToString());

                                    if (rows.Length > 0)
                                    {
                                        ava_nome = rows[0]["ava_nome"].ToString();
                                    }
                                }
                            }
                        }
                        AbrirObservacaoConselho(index, alu_id, mtu_id
                                                , GetGlobalResourceObject("UserControl", "Fechamento.UCFechamentoPadrao.gvAlunos.ColunaRegistroConselho")
                                                    + " - " + (string.IsNullOrEmpty(ava_nome) ? VS_Avaliacao.ava_nome : ava_nome)
                                                , VS_tpc_idUltimoBimestre > 0 ? VS_tpc_idUltimoBimestre : VS_Avaliacao.tpc_id);
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
                        Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                        long alu_id = Convert.ToInt64(((HiddenField)(rowControl.FindControl("hfAluId"))).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)(rowControl.FindControl("hfMtuId"))).Value);
                        List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_Turma.cal_id, ApplicationWEB.AppMinutosCacheLongo);
                        // abre o boletim com o ultimo periodo selecionado
                        AbrirBoletim(alu_id, tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1, mtu_id);
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
                        AbrirRelatorioRP(Convert.ToInt64(e.CommandArgument.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir as anotações da recuperação paralela para o aluno.", UtilBO.TipoMensagem.Erro);
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
                                lblMsgRepeater.Visible = false;
                                rptAlunos.Visible = false;
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
                            lblMsgRepeater.Visible = true;
                            rptAlunos.Visible = true;
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

                int ordemSelecionada = RetornaOrdemParecer(ddlPareceresValidar.SelectedItem.Text);

                args.IsValid = ordemSelecionada <= VS_ParecerMinimo;
            }
            catch
            {
                args.IsValid = false;
            }
        }

        public void UCAlunoEfetivacaoObservacao_ReturnValues(Int32 indiceAluno, CLS_AlunoAvaliacaoTurmaObservacao observacao, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, byte parecerFinal)
        {
            try
            {
                RepeaterItem rptItem = rptAlunos.Items[indiceAluno];
                SetaComponenteObservacaoConselhoLinhaGrid(rptItem, observacao, listaAtualizacaoEfetivacao, resultado, parecerFinal);
                uppAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptComponenteRegencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptPeriodos;
                HtmlControl hcComponenteRegenciaNotaFinal;

                if (e.Item.ItemType == ListItemType.Header)
                {
                    rptPeriodos = (Repeater)e.Item.FindControl("rptHeaderPeriodos");
                    hcComponenteRegenciaNotaFinal = (HtmlControl)e.Item.FindControl("thNotaFinal");

                    ((Literal)e.Item.FindControl("litHeadNotaFinal")).Text = nomeAvaliacaoFinal;

                    if (visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaPeriodos])
                    {
                        rptPeriodos.DataSource = objAvaliacoesPeriodicas;
                    }
                }
                else
                {
                    rptPeriodos = (Repeater)e.Item.FindControl("rptItemPeriodos");
                    hcComponenteRegenciaNotaFinal = (HtmlControl)e.Item.FindControl("tdNotaFinal");

                    TextBox txtNotaFinal = (TextBox)e.Item.FindControl("txtNotaFinal");
                    DropDownList ddlPareceresFinal = (DropDownList)e.Item.FindControl("ddlPareceresFinal");

                    EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                    // Seta campos da avaliação principal.
                    SetaCamposAvaliacao(tipo, txtNotaFinal, (DataBinder.Eval(e.Item.DataItem, "Avaliacao") ?? string.Empty).ToString(), ddlPareceresFinal);

                    if (visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaFinal])
                    {
                        if (txtNotaFinal.Visible)
                        {
                            txtNotaFinal.TabIndex = tabIndexNotaFinal;
                        }
                        else if (ddlPareceresFinal.Visible)
                        {
                            ddlPareceresFinal.TabIndex = tabIndexNotaFinal;
                        }
                        tabIndexNotaFinal++;
                    }

                    if (visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaPeriodos])
                    {
                        try
                        {
                            rptPeriodos.DataSource = (from tRow in listaFinalAvaliacoesPeriodicas.AsEnumerable()
                                                      where tRow.alu_id == Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"))
                                                         && tRow.tud_id == Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"))
                                                      select tRow).ToList();
                        }
                        catch
                        {
                            rptPeriodos.DataSource = new List<AlunosFechamentoFinalComponenteRegencia>();
                        }
                    }

                    HiddenField hf = (HiddenField)e.Item.FindControl("hfTudId");
                    hf.Value = DataBinder.Eval(e.Item.DataItem, "tud_id").ToString();
                    hf = (HiddenField)e.Item.FindControl("hfMtdId");
                    hf.Value = DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString();
                    hf = (HiddenField)e.Item.FindControl("hfAvaliacaoId");
                    hf.Value = (DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString());
                }

                rptPeriodos.Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaPeriodos];
                hcComponenteRegenciaNotaFinal.Visible = visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaNotaFinal];

                if (visibilidadeColunasComponenteRegencia[colunaComponenteRegenciaPeriodos])
                {
                    rptPeriodos.DataBind();
                }
            }
        }

        protected void rptComponenteRegencia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Control rptItemControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
            Control rowControl = rptItemControl.Parent.Parent;
            string pes_nome = ((Label)(rowControl.FindControl("lblNomeAluno"))).Text;
            string dis_nome = ((Label)(rptItemControl.FindControl("lblNomeDisciplina"))).Text;
            hdnLocalImgCheckSituacao.Value = ((RepeaterItem)rowControl).ItemIndex + "," + e.Item.ItemIndex;

            if (e.CommandName == "Relatorio")
            {
                TrataEventoCommandRelatorio(e.CommandArgument.ToString(), pes_nome, dis_nome);
            }
        }

        protected void rptHeaderPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                bool tud_naoLancarNota = false;
                if (Tud_id > 0)
                {
                    tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
                }

                Literal litHeadPeriodo = (Literal)e.Item.FindControl("litHeadPeriodo");
                // exibir notas
                if (!tud_naoLancarNota && EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno)
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

                    litHeadPeriodo.Text = nomeNota + " " + DataBinder.Eval(e.Item.DataItem, "NomeAvaliacao");
                }
                // exibir frequencia (enriquecimento curricular/recuperação paralela)
                else
                {
                    litHeadPeriodo.Text = "% Freq. " + DataBinder.Eval(e.Item.DataItem, "NomeAvaliacao");
                }
            }
        }

        protected void rptItemPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                bool tud_naoLancarFrequencia = false;
                bool tud_naoLancarNota = false;
                if (Tud_id > 0)
                {
                    tud_naoLancarFrequencia = EntTurmaDisciplina.tud_naoLancarFrequencia;
                    tud_naoLancarNota = EntTurmaDisciplina.tud_naoLancarNota;
                }

                Label lblFrequencia = (Label)e.Item.FindControl("lblFrequencia");
                Label lblNota = (Label)e.Item.FindControl("lblNota");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                Image imgSituacao = (Image)e.Item.FindControl("imgSituacao");
                HyperLink hplAnexo = (HyperLink)e.Item.FindControl("hplAnexo");
                HtmlControl hcPeriodos = (HtmlControl)e.Item.FindControl("tdPeriodos");

                if (DataBinder.Eval(e.Item.DataItem, "UltimoPeriodo").Equals(1) && DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(0))
                {
                    HiddenField hfAvaId = (HiddenField)e.Item.FindControl("hfAvaId");
                    HiddenField hfQtFaltas = (HiddenField)e.Item.FindControl("hfQtFaltas");
                    HiddenField hfQtAulas = (HiddenField)e.Item.FindControl("hfQtAulas");
                    HiddenField hfQtAusenciasCompensadas = (HiddenField)e.Item.FindControl("hfQtAusenciasCompensadas");

                    hfAvaId.Value = DataBinder.Eval(e.Item.DataItem, "ava_id").Equals(DBNull.Value) ? String.Empty : DataBinder.Eval(e.Item.DataItem, "ava_id").ToString();

                    if (hfQtFaltas != null)
                    {
                        hfQtFaltas.Value = DataBinder.Eval(e.Item.DataItem, "QtFaltasAluno").Equals(DBNull.Value) ? String.Empty : DataBinder.Eval(e.Item.DataItem, "QtFaltasAluno").ToString();
                    }
                    if (hfQtAulas != null)
                    {
                        hfQtAulas.Value = DataBinder.Eval(e.Item.DataItem, "QtAulasAluno").Equals(DBNull.Value) ? String.Empty : DataBinder.Eval(e.Item.DataItem, "QtAulasAluno").ToString();
                    }
                    if (hfQtAusenciasCompensadas != null)
                    {
                        hfQtAusenciasCompensadas.Value = DataBinder.Eval(e.Item.DataItem, "ausenciasCompensadas").Equals(DBNull.Value) ? String.Empty : DataBinder.Eval(e.Item.DataItem, "ausenciasCompensadas").ToString();
                    }
                }

                bool disciplinaEletivaAluno = EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno;
                bool alunoForaRede = DataBinder.Eval(e.Item.DataItem, "AlunoForaDaRede").ToString() == "1";
                if (lblFrequencia != null)
                {
                    lblFrequencia.Visible = (tud_naoLancarNota || disciplinaEletivaAluno) && !tud_naoLancarFrequencia;
                    try
                    {
                        lblFrequencia.Text = DataBinder.Eval(e.Item.DataItem, "Frequencia").Equals(DBNull.Value) || alunoForaRede ? "-" : string.Format(VS_FormatacaoDecimaisFrequencia, Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Frequencia").ToString()));
                    }
                    catch
                    {
                        lblFrequencia.Text = "-";
                    }
                }

                // exibir notas
                if (!tud_naoLancarNota && !disciplinaEletivaAluno)
                {
                    if (hcPeriodos != null)
                    {
                        hcPeriodos.Attributes.Add("class", "colunaNota grid-responsive-item-inline grid-responsive-col4");
                    }

                    EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EscalaAvaliacao.esa_tipo;

                    if (tipo == EscalaAvaliacaoTipo.Relatorios)
                    {
                        ((HtmlControl)e.Item.FindControl("divNotaPeriodo")).Visible = false;
                    }
                    else
                    {
                        if (DataBinder.Eval(e.Item.DataItem, "AlunoForaDaRede").ToString() == "1")
                        {
                            // Pinta a celula se o aluno estava fora da rede no periodo
                            ((HtmlControl)e.Item.FindControl("divNotaPeriodo")).Style.Add("background-color", ApplicationWEB.CorAlunoForaDaRede);
                        }
                        else if (!string.IsNullOrEmpty((DataBinder.Eval(e.Item.DataItem, "AvaliacaoPosConselho") ?? string.Empty).ToString()))
                        {
                            // Pinta a celula se for nota atribuída pelo conselho de classe.
                            ((HtmlControl)e.Item.FindControl("divNotaPeriodo")).Style.Add("background-color", ApplicationWEB.CorNotaPosConselho);
                        }
                    }

                    // Seta campos da avaliação principal.
                    SetaCamposAvaliacao(tipo, lblNota, (DataBinder.Eval(e.Item.DataItem, "Avaliacao") ?? string.Empty).ToString());

                    string commandArgument = VS_tur_id.ToString() + ";" +
                                                Tud_id.ToString() + ";" +
                                                DataBinder.Eval(e.Item.DataItem, "alu_id").ToString() + ";" +
                                                DataBinder.Eval(e.Item.DataItem, "mtu_id").ToString() + ";" +
                                                DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString() + ";" +
                                                (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString()) ?
                                                    DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString() : "-1");

                    SetaComponentesRelatorioLinhaGrid(commandArgument
                                                        , btnRelatorio
                                                        , imgSituacao
                                                        , hplAnexo);

                    btnRelatorio.Enabled = false;
                }
                // exibir frequencia (enriquecimento curricular/recuperação paralela)
                else
                {
                    if (hcPeriodos != null)
                    {
                        hcPeriodos.Attributes.Remove("class");
                    }

                    if (DataBinder.Eval(e.Item.DataItem, "AlunoForaDaRede").ToString() == "1")
                    {
                        // Pinta a celula se o aluno estava fora da rede no periodo
                        ((HtmlControl)e.Item.FindControl("divNotaPeriodo")).Style.Add("background-color", ApplicationWEB.CorAlunoForaDaRede);
                    }

                    lblNota.Visible =
                    btnRelatorio.Visible =
                    imgSituacao.Visible =
                    hplAnexo.Visible = false;
                }
            }
        }

        #endregion Eventos
    }
}