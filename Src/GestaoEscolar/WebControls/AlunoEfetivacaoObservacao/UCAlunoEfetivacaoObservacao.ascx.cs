using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.EfetivacaoNotas;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using GestaoEscolar.WebControls.Fechamento;
using MSTech.GestaoEscolar.CustomResourceProviders;

namespace GestaoEscolar.WebControls.AlunoEfetivacaoObservacao
{
    public partial class UCAlunoEfetivacaoObservacao : MotherUserControl
    {
        #region Constantes

        private const short criterioFrequenciaFinalAjustadaDisciplina = 6;

        #endregion Constantes

        #region Propriedades

        protected Dictionary<long, bool> dicSemNota = new Dictionary<long, bool>();

        protected IDictionary<string, object> returns = new Dictionary<string, object>();

        /// <summary>
        /// Viewstate que armazena o ID da turma.
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID da turma disciplina.
        /// </summary>
        private long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do aluno.
        /// </summary>
        public long VS_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID da matrícula turma do aluno.
        /// </summary>
        private int VS_mtu_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mtu_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_mtu_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID da matrícula turma disciplina do aluno.
        /// </summary>
        private int VS_mtd_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mtd_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_mtd_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do formato de avaliação.
        /// </summary>
        private int VS_fav_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_fav_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_fav_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID da avaliação.
        /// </summary>
        private int VS_ava_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_ava_id"] ?? -1);
            }

            set
            {
                ViewState["VS_ava_id"] = value;
            }
        }

        /// <summary>
        /// Armazena em viewstate valor que indica se a observação é para a disciplina ou para conselho pedagógico.
        /// </summary>
        private eTipoObservacao VS_tipoObservacao
        {
            get
            {
                return (eTipoObservacao)ViewState["VS_tipoObservacao"];
            }

            set
            {
                ViewState["VS_tipoObservacao"] = value;
            }
        }

        /// <summary>
        /// Nome padrão para o período do calendário.
        /// </summary>
        private string NomePadraoPeriodoCalendario
        {
            get
            {
                return GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            }
        }

        /// <summary>
        /// Guarda a mensagem que deve aparecer antes de salvar o log
        /// </summary>
        public string VS_MensagemLogEfetivacaoObservacao
        {
            get
            {
                if (ViewState["VS_MensagemLogEfetivacaoObservacao"] == null)
                {
                    return "";
                }

                return ViewState["VS_MensagemLogEfetivacaoObservacao"].ToString();
            }
            set
            {
                ViewState["VS_MensagemLogEfetivacaoObservacao"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando se a turma possui efetivação semestral
        /// </summary>
        private bool VS_EfetivacaoSemestral
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
        /// Viewstate que armazena o ID do calendario.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? -1);
            }

            set
            {
                ViewState["VS_cal_id"] = value;
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
                    if (EntEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    {
                        string valorMinimo = (EntFormatoAvaliacao.valorMinimoAprovacaoPorDisciplina);
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

                return Convert.ToDouble(ViewState["VS_NotaMinima"].ToString());
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
                    if (EntEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    {
                        string valorMinimo = (EntFormatoAvaliacao.valorMinimoAprovacaoPorDisciplina);

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
        /// Retorna se a turma selecionada no combo é do PEJA (currículo seriado por avaliações).
        /// </summary>
        private bool VS_turma_Peja
        {
            get
            {
                if (ViewState["VS_turma_Peja"] == null)
                {
                    if (VS_tur_id > 0)
                    {
                        List<TUR_TurmaCurriculo> lTurmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);
                        ACA_Curriculo curriculo = ACA_CurriculoBO.GetEntity(new ACA_Curriculo
                        {
                            cur_id = lTurmaCurriculo.First().cur_id
                            ,
                            crr_id = lTurmaCurriculo.First().crr_id
                        });
                        ViewState["VS_turma_Peja"] = (curriculo.crr_regimeMatricula == (byte)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes).ToString();
                    }
                }

                return Convert.ToBoolean((ViewState["VS_turma_Peja"] ?? "False"));
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
        /// Guarda as justificativas da nota final
        /// </summary>
        private List<UCEfetivacaoNotas.Justificativa> VS_JustificativaNotaFinal
        {
            get
            {
                if (ViewState["VS_JustificativaNotaFinal"] != null)
                {
                    return (List<UCEfetivacaoNotas.Justificativa>)ViewState["VS_JustificativaNotaFinal"];
                }

                return new List<UCEfetivacaoNotas.Justificativa>();
            }

            set
            {
                ViewState["VS_JustificativaNotaFinal"] = value;
            }
        }

        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool VS_periodoFechado
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_periodoFechado"] ?? true);
            }

            set
            {
                ViewState["VS_periodoFechado"] = value;
            }
        }

        private ACA_FormatoAvaliacao formatoAvaliacao;

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_FormatoAvaliacao EntFormatoAvaliacao
        {
            get
            {
                if (formatoAvaliacao == null)
                {
                    formatoAvaliacao = new ACA_FormatoAvaliacao
                    {
                        fav_id = VS_fav_id
                    };
                    ACA_FormatoAvaliacaoBO.GetEntity(formatoAvaliacao);
                }

                return formatoAvaliacao;
            }
        }

        private ACA_EscalaAvaliacao escala;

        /// <summary>
        /// Retorna a escala de avaliação do formato. Se for lançamento na disciplina,
        /// retorna de acordo com o esa_idPorDisciplina, se for global, retorna
        /// o esa_idConceitoGlobal.
        /// </summary>
        private ACA_EscalaAvaliacao EntEscalaAvaliacao
        {
            get
            {
                if (escala == null)
                {
                    escala = new ACA_EscalaAvaliacao
                    {
                        esa_id = EntFormatoAvaliacao.esa_idPorDisciplina
                    };

                    if (escala.esa_id > 0)
                    {
                        // Só chama o método de carregar se o id da escala for > 0.
                        ACA_EscalaAvaliacaoBO.GetEntity(escala);
                    }
                }

                return escala;
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
                       (_ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(EntEscalaAvaliacao.esa_id));
            }

            set
            {
                _ltPareceres = value;
            }
        }

        /// <summary>
        /// Object da query com os nomes das avaliações
        /// </summary>
        private object objAvaliacoes;

        /// <summary>
        /// DataTable com as notas e frequência finais das avaliações
        /// </summary>
        private DataTable dtAvaliacoes = new DataTable();

        /// <summary>
        /// Controle referente a coluna de frequência, comum para todos os componentes da regência
        /// </summary>
        private HtmlControl tdFrequenciaRegencia = null;

        /// <summary>
        /// Nome da avaliação final para colocar na coluna
        /// </summary>
        private string nomeAvaliacaoFinal;

        public UpdatePanel UppNotaDisciplinas
        {
            get
            {
                return uppNotaDisciplinas;
            }
        }

        private MTR_MatriculaTurma matriculaTurma;

        /// <summary>
        /// Entidade de matrícula turma do aluno.
        /// </summary>
        private MTR_MatriculaTurma EntMatriculaTurma
        {
            get
            {
                if (matriculaTurma == null || matriculaTurma.alu_id != VS_alu_id || matriculaTurma.mtu_id != VS_mtu_id)
                    matriculaTurma = MTR_MatriculaTurmaBO.GetEntity(new MTR_MatriculaTurma { alu_id = VS_alu_id, mtu_id = VS_mtu_id });

                return matriculaTurma;
            }

            set { matriculaTurma = value; }
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

        private Control _UCEfetivacaoNotas
        {
            get
            {
                return this.Parent.Parent.Parent;
            }
        }

        public byte TipoFechamento
        {
            //1 - Tela de fechamento padrao
            //2 - Tela de fechamento nova (fechamento automatico)
            set
            {
                hdnTipoFechamento.Value = value.ToString();
            }
            get
            {
                if (!String.IsNullOrEmpty(hdnTipoFechamento.Value))
                {
                    return Convert.ToByte(hdnTipoFechamento.Value);
                }
                return 1;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do tipo periodo calendario.
        /// </summary>
        private int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        public delegate void OnReturnValues(IDictionary<string, object> parameters, bool fecharJanela, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina);

        public event OnReturnValues ReturnValues;

        public delegate void commandAbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno);

        public event commandAbrirRelatorio AbrirRelatorio;

        public delegate void commandAbrirJustificativa(string id, string textoJustificativa, string pes_nome, string dis_nome);

        public event commandAbrirJustificativa AbrirJustificativa;

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Carrega as observações do aluno.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina do aluno.</param>
        /// <param name="atd_id">ID da avaliação turma disciplina do aluno.</param>
        public void CarregarObservacoes(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, string dadosAluno, bool periodoFechado, bool habilitado = true)
        {
            VS_tipoObservacao = eTipoObservacao.Disciplina;
            VS_tud_id = tud_id;
            VS_alu_id = alu_id;
            VS_mtu_id = mtu_id;
            VS_mtd_id = mtd_id;
            VS_fav_id = fav_id;
            VS_ava_id = ava_id;
            VS_periodoFechado = periodoFechado;
            lblMensagem.Text = lblMensagem2.Text = String.Empty;
            lblDadosAluno.Text = dadosAluno;

            liParecerConclusivo.Visible = fdsParecerConclusivo.Visible = false;
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_RECOMENDACOES_POR_GRUPO_USUARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                liDesempenhoAprendizado.Visible = fdsDesempenhoAprendizado.Visible = false;
            }
            else
            {
                liDesempenhoAprendizado.Visible = fdsDesempenhoAprendizado.Visible = true;
                CarregarDesempenhoAprendizadoTurDis();
            }
            CarregarRecomendacaoAlunoTurDis();
            CarregarRecomendacaoRespTurDis();

            if (rptDesempenho.Items.Count > 0)
                lblInfoDesempenho.Text = UtilBO.GetErroMessage("Marque acima o(s) " + (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO_MIN") + " observado(s) no aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoDesempenho.Text = "Não existem tipos de " + (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO_MIN") + " cadastrados.";

            if (rptRecomendacao.Items.Count > 0)
                lblInfoRecomendacaoAluno.Text = UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) ao aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoRecomendacaoAluno.Text = "Não existem tipos de recomendações cadastrados.";

            if (rptRecomendacaoResp.Items.Count > 0)
                lblInfoRecomendacaoResp.Text = UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) aos pais-reponsáveis do aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoRecomendacaoResp.Text = "Não existem tipos de recomendações cadastrados.";

            lblDesempenhoAprendizagem.Text = (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO");

            HabilitaControles(pnlObservacao.Controls, habilitado);
            btnSalvar.Visible = btnLimpar.Visible = habilitado;
        }

        /// <summary>
        /// Carrega as observações para conselho pedagógico do aluno.
        /// </summary>
        /// <param name="tud_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="aat_id">ID da avaliação turma do aluno.</param>
        public void CarregarObservacoes(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, string dadosAluno, bool fav_avaliacaoFinalAnalitica, AvaliacaoTipo ava_tipo, int cal_id, int tpc_id, bool efetivacaoSemestral, bool periodoFechado, bool habilitado = true)
        {
            VS_tipoObservacao = eTipoObservacao.ConselhoPedagogico;
            VS_tur_id = tur_id;
            VS_alu_id = alu_id;
            VS_mtu_id = mtu_id;
            VS_fav_id = fav_id;
            VS_ava_id = ava_id;
            VS_cal_id = cal_id;
            VS_periodoFechado = periodoFechado;
            VS_tpc_id = tpc_id;
            lblMensagem.Text = lblMensagem2.Text = String.Empty;
            VS_EfetivacaoSemestral = efetivacaoSemestral;
            _VS_Nota_Relatorio = new List<UCEfetivacaoNotas.NotasRelatorio>();
            lblDadosAluno.Text = dadosAluno;

            // Mostrar a aba de parecer conclusivo se a flag avaliação final analítica do formato de avaliação for verdadeira
            // e, se for a avaliação periódica do ultimo período
            // ou se for a avaliação final.
            if (fav_avaliacaoFinalAnalitica && !VS_periodoFechado)
            {
                if (ava_tipo == AvaliacaoTipo.Final)
                {
                    liParecerConclusivo.Visible = true;
                }
                else if ((ava_tipo == AvaliacaoTipo.Periodica || ava_tipo == AvaliacaoTipo.PeriodicaFinal) && tpc_id > 0)
                {
                    List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
                    int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;

                    int tev_EfetivacaoFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    List<ACA_Evento> ltEvento = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);

                    liParecerConclusivo.Visible = (tpc_id == tpc_idUltimoPerido) && (ltEvento.Exists(p => p.tev_id == tev_EfetivacaoFinal));
                }
                else
                {
                    liParecerConclusivo.Visible = false;
                }
            }
            else
            {
                liParecerConclusivo.Visible = false;
            }
            fdsParecerConclusivo.Visible = liParecerConclusivo.Visible;

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_RECOMENDACOES_POR_GRUPO_USUARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                liDesempenhoAprendizado.Visible = fdsDesempenhoAprendizado.Visible = false;
            }
            else
            {
                liDesempenhoAprendizado.Visible = fdsDesempenhoAprendizado.Visible = true;
                CarregarDesempenhoAprendizadoTur();
            } 
            CarregarRecomendacaoAlunoTur();
            CarregarRecomendacaoRespTur();

            HabilitaControles(pnlObservacao.Controls, habilitado);

            if (fdsParecerConclusivo.Visible)
            {
                #region Calcula numero Casas decimais da frequencia

                VS_FormatacaoDecimaisFrequencia = "{" + GestaoEscolarUtilBO.CriaFormatacaoDecimal
                    ((EntFormatoAvaliacao.fav_variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(EntFormatoAvaliacao.fav_variacao) : 2), "0:{0}")
                    + "}";

                #endregion Calcula numero Casas decimais da frequencia

                VS_JustificativaNotaFinal = new List<UCEfetivacaoNotas.Justificativa>();

                CarregarParecerConclusivoTur();
            }

            if (rptDesempenho.Items.Count > 0)
                lblInfoDesempenho.Text = UtilBO.GetErroMessage("Marque acima o(s) " + (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO_MIN") + " observado(s) no aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoDesempenho.Text = "Não existem tipos de " + (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO_MIN") + " cadastrados.";

            if (rptRecomendacao.Items.Count > 0)
                lblInfoRecomendacaoAluno.Text = UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) ao aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoRecomendacaoAluno.Text = "Não existem tipos de recomendações cadastrados.";

            if (rptRecomendacaoResp.Items.Count > 0)
                lblInfoRecomendacaoResp.Text = UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) aos pais-reponsáveis do aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao);
            else
                lblInfoRecomendacaoResp.Text = "Não existem tipos de recomendações cadastrados.";

            lblDesempenhoAprendizagem.Text = (string)GetGlobalResourceObject("Mensagens", "MSG_DESEMPENHOAPRENDIZADO");

            btnSalvar.Visible = btnLimpar.Visible = habilitado;
        }

        #region Desempenho

        /// <summary>
        /// Carrega os desempenhos e aprendizados do aluno.
        /// </summary>
        private void CarregarDesempenhoAprendizadoTurDis()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoBO.SelecionaPorMatriculaTurmaDisciplina(VS_tud_id, VS_alu_id, VS_mtu_id, VS_mtd_id, VS_fav_id, VS_ava_id);

            txtResumoDesempenho.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ado_desempenhoAprendizado"].ToString() : string.Empty;

            List<ACA_TipoDesempenhoAprendizado> lst = new List<ACA_TipoDesempenhoAprendizado>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["tda_id"].ToString()))
                    lst.Add(new ACA_TipoDesempenhoAprendizado { tda_id = Convert.ToInt32(row["tda_id"]), tda_descricao = row["tda_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.tda_descricao).ToList();
            rptDesempenho.DataSource = lst;
            rptDesempenho.DataBind();
        }

        /// <summary>
        /// Carrega os desempenhos e aprendizados do aluno.
        /// </summary>
        private void CarregarDesempenhoAprendizadoTur()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDesempenhoBO.SelecionaPorMatriculaTurma(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);

            txtResumoDesempenho.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ato_desempenhoAprendizado"].ToString() : string.Empty;

            List<ACA_TipoDesempenhoAprendizado> lst = new List<ACA_TipoDesempenhoAprendizado>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["tda_id"].ToString()))
                    lst.Add(new ACA_TipoDesempenhoAprendizado { tda_id = Convert.ToInt32(row["tda_id"]), tda_descricao = row["tda_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.tda_descricao).ToList();
            rptDesempenho.DataSource = lst;
            rptDesempenho.DataBind();
        }

        #endregion Desempenho

        #region Recomendação Aluno

        /// <summary>
        /// Carrega as recomendações ao aluno.
        /// </summary>
        private void CarregarRecomendacaoAlunoTurDis()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoBO.SelecionaPorMatriculaTurmaDisciplinaTipo(VS_tud_id, VS_alu_id, VS_mtu_id, VS_mtd_id, VS_fav_id, VS_ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.Aluno);

            txtRecomendacaoAluno.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ado_recomendacao"].ToString() : string.Empty;

            List<ACA_RecomendacaoAlunoResponsavel> lst = ACA_RecomendacaoAlunoResponsavelBO.SelecionarAtivosPorTipo((byte)ACA_RecomendacaoAlunoResponsavelTipo.Aluno);
            lst = lst.OrderBy(p => p.rar_descricao).ToList();

            rptRecomendacao.DataSource = lst;
            rptRecomendacao.DataBind();
        }

        /// <summary>
        /// Carrega as recomendações ao aluno.
        /// </summary>
        private void CarregarRecomendacaoAlunoTur()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaRecomendacaoBO.SelecionaPorMatriculaTurmaTipo(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.Aluno);

            txtRecomendacaoAluno.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ato_recomendacao"].ToString() : string.Empty;

            List<ACA_RecomendacaoAlunoResponsavel> lst = new List<ACA_RecomendacaoAlunoResponsavel>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["rar_id"].ToString()))
                    lst.Add(new ACA_RecomendacaoAlunoResponsavel { rar_id = Convert.ToInt32(row["rar_id"]), rar_descricao = row["rar_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.rar_descricao).ToList();
            rptRecomendacao.DataSource = lst;
            rptRecomendacao.DataBind();

            //rptRecomendacao.DataSource = dt;
            //rptRecomendacao.DataBind();

            //lblInfoRecomendacaoAluno.Text = rptRecomendacao.Items.Count > 0 ?
            //    UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) ao aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao) :
            //    "Não existem tipos de recomendações cadastrados.";
        }

        #endregion Recomendação Aluno

        #region Recomendação Pais-Responsáveis

        /// <summary>
        /// Carrega as recomendações aos pais-reponsáveis do aluno.
        /// </summary>
        private void CarregarRecomendacaoRespTurDis()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoBO.SelecionaPorMatriculaTurmaDisciplinaTipo(VS_tud_id, VS_alu_id, VS_mtu_id, VS_mtd_id, VS_fav_id, VS_ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.PaisResponsavel);

            txtRecomendacaoResp.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ado_recomendacao"].ToString() : string.Empty;

            List<ACA_RecomendacaoAlunoResponsavel> lst = ACA_RecomendacaoAlunoResponsavelBO.SelecionarAtivosPorTipo((byte)ACA_RecomendacaoAlunoResponsavelTipo.PaisResponsavel);
            lst = lst.OrderBy(p => p.rar_descricao).ToList();

            rptRecomendacaoResp.DataSource = lst;
            rptRecomendacaoResp.DataBind();

            //lblInfoRecomendacaoResp.Text = rptRecomendacaoResp.Items.Count > 0 ?
            //    UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) aos pais-reponsáveis do aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao) :
            //    "Não existem tipos de recomendações cadastrados.";
        }

        /// <summary>
        /// Carrega as recomendações aos pais-reponsáveis do aluno.
        /// </summary>
        private void CarregarRecomendacaoRespTur()
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaRecomendacaoBO.SelecionaPorMatriculaTurmaTipo(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.PaisResponsavel);

            txtRecomendacaoResp.Text = dt.Rows.Count > 0 ? dt.Rows[0]["ato_recomendacao"].ToString() : string.Empty;

            List<ACA_RecomendacaoAlunoResponsavel> lst = new List<ACA_RecomendacaoAlunoResponsavel>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["rar_id"].ToString()))
                    lst.Add(new ACA_RecomendacaoAlunoResponsavel { rar_id = Convert.ToInt32(row["rar_id"]), rar_descricao = row["rar_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.rar_descricao).ToList();
            rptRecomendacaoResp.DataSource = lst;
            rptRecomendacaoResp.DataBind();

            //rptRecomendacaoResp.DataSource = dt;
            //rptRecomendacaoResp.DataBind();

            //lblInfoRecomendacaoResp.Text = rptRecomendacaoResp.Items.Count > 0 ?
            //    UtilBO.GetErroMessage("Marque acima a(s) recomendação(ões) aos pais-reponsáveis do aluno neste " + NomePadraoPeriodoCalendario + ".", UtilBO.TipoMensagem.Informacao) :
            //    "Não existem tipos de recomendações cadastrados.";
        }

        #endregion Recomendação Pais-Responsáveis

        /// <summary>
        /// O método limpa os campos de cadastro de observações.
        /// </summary>
        private void Limpar()
        {
            txtResumoDesempenho.Text =
                txtRecomendacaoAluno.Text =
                txtRecomendacaoResp.Text = string.Empty;
        }

        /// <summary>
        /// Retorna dados da observação para conselho pedagógico.
        /// </summary>
        /// <returns></returns>
        private CLS_AlunoAvaliacaoTur_Observacao RetornaObservacao()
        {
            CLS_AlunoAvaliacaoTur_Observacao observacao = new CLS_AlunoAvaliacaoTur_Observacao
            {
                tur_id = VS_tur_id
                ,
                alu_id = VS_alu_id
                ,
                mtu_id = VS_mtu_id
                ,
                fav_id = VS_fav_id
                ,
                ava_id = VS_ava_id
                ,
                entityObservacao = new CLS_AlunoAvaliacaoTurmaObservacao
                {
                    tur_id = VS_tur_id
                    ,
                    alu_id = VS_alu_id
                    ,
                    mtu_id = VS_mtu_id
                    ,
                    fav_id = VS_fav_id
                    ,
                    ava_id = VS_ava_id
                    ,
                    ato_qualidade = string.Empty
                    ,
                    ato_desempenhoAprendizado = txtResumoDesempenho.Text
                    ,
                    ato_recomendacaoAluno = txtRecomendacaoAluno.Text
                    ,
                    ato_recomendacaoResponsavel = txtRecomendacaoResp.Text
                }
                ,
                ltQualidade = new List<CLS_AlunoAvaliacaoTurmaQualidade>()
                ,
                ltDesempenho = new List<CLS_AlunoAvaliacaoTurmaDesempenho>()
                ,
                ltRecomendacao = new List<CLS_AlunoAvaliacaoTurmaRecomendacao>()
            };

            return observacao;
        }

        /// <summary>
        /// Retorna dados da observação para a disciplina.
        /// </summary>
        /// <returns></returns>
        private CLS_AlunoAvaliacaoTurDis_Observacao RetornaObservacaoDisciplina()
        {
            CLS_AlunoAvaliacaoTurDis_Observacao observacao = new CLS_AlunoAvaliacaoTurDis_Observacao
            {
                tud_id = VS_tud_id
                ,
                alu_id = VS_alu_id
                ,
                mtu_id = VS_mtu_id
                ,
                mtd_id = VS_mtd_id
                ,
                fav_id = VS_fav_id
                ,
                ava_id = VS_ava_id
                ,
                entityObservacao = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacao
                {
                    tud_id = VS_tud_id
                    ,
                    alu_id = VS_alu_id
                    ,
                    mtu_id = VS_mtu_id
                    ,
                    mtd_id = VS_mtd_id
                    ,
                    fav_id = VS_fav_id
                    ,
                    ava_id = VS_ava_id
                    ,
                    ado_qualidade = string.Empty
                    ,
                    ado_desempenhoAprendizado = txtResumoDesempenho.Text
                    ,
                    ado_recomendacaoAluno = txtRecomendacaoAluno.Text
                    ,
                    ado_recomendacaoResponsavel = txtRecomendacaoResp.Text
                }
                ,
                ltQualidade = new List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade>()
                ,
                ltDesempenho = new List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho>()
                ,
                ltRecomendacao = new List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao>()
            };

            return observacao;
        }

        /// <summary>
        /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
        /// passado.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="enabled"></param>
        protected void HabilitaControles(ControlCollection controls, bool enabled)
        {
            foreach (Control c in controls)
            {
                if (c.Controls.Count > 0)
                    HabilitaControles(c.Controls, enabled);

                WebControl wb = c as WebControl;

                if (wb != null)
                    wb.Enabled = enabled;
            }
        }
        
        #region Parecer conclusivo

        /// <summary>
        /// Carrega o parecer conclusivo do aluno
        /// </summary>
        private void CarregarParecerConclusivoTur()
        {
            string[] dados = lblDadosAluno.Text.Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);

            if (dados.Any())
            {
                lblNomeAlunoJustificativa.Text = dados.First();
            }

            txtJustificativaParecerConclusivo.Text = hdnJustificativa.Text = string.Empty;

            DataTable dt = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaPor_AlunoTurma(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_cal_id);

            hdnVariacaoFrequencia.Value = dt.Rows.Count > 0 ? dt.Rows[0]["fav_variacao"].ToString().Replace(',', '.') : "0.01";

            DataTable dtAvaliacaoFinal;
            DataTable dtAvaliacaoEnriqCurr;
            DataTable dtAvaliacaoRecPar;

            try
            {
                dtAvaliacaoFinal = (from dadosGeral in dt.AsEnumerable()
                                    where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == -1
                                          && Convert.ToBoolean(dadosGeral.Field<object>("ExibirNota"))
                                          && Convert.ToInt32(dadosGeral.Field<object>("recuperacao")) != 1
                                    select dadosGeral).CopyToDataTable();
            }
            catch
            {
                dtAvaliacaoFinal = new DataTable();
            }

            try
            {
                dtAvaliacaoEnriqCurr = (from dadosGeral in dt.AsEnumerable()
                                        where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == -1
                                              && !Convert.ToBoolean(dadosGeral.Field<object>("ExibirNota"))
                                              && Convert.ToInt32(dadosGeral.Field<object>("recuperacao")) != 1
                                        select dadosGeral).CopyToDataTable();
            }
            catch
            {
                dtAvaliacaoEnriqCurr = new DataTable();
            }

            try
            {
                dtAvaliacaoRecPar = (from dadosGeral in dt.AsEnumerable()
                                        where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == -1
                                              && Convert.ToInt32(dadosGeral.Field<object>("recuperacao")) == 1
                                        select dadosGeral).CopyToDataTable();
            }
            catch
            {
                dtAvaliacaoRecPar = new DataTable();
            }

            SetaDadosRelatorio(dt);
            SetaDadosJustificativaNotaFinal(dtAvaliacaoFinal, "Relatorio");

            try
            {
                dtAvaliacoes = (from dadosGeral in dt.AsEnumerable()
                                where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) > 0
                                select dadosGeral).CopyToDataTable();
            }
            catch
            {
                dtAvaliacoes = new DataTable();
            }

            // Seta nome dos headers das colunas de nota..
            nomeAvaliacaoFinal = dtAvaliacaoFinal.Rows.Count > 0 ? dtAvaliacaoFinal.Rows[0]["NomeAvaliacao"].ToString() : "";
            if (TipoFechamento == 1)
            {
                ((UCEfetivacaoNotas)_UCEfetivacaoNotas).AlterarTituloJustificativa("Justificativa da " + nomeAvaliacaoFinal.ToLower());
            }

            objAvaliacoes = dtAvaliacoes.AsEnumerable()
                                .Select(row => new
                                {
                                    NomeAvaliacao = row.Field<string>("NomeAvaliacao")
                                    ,
                                    AvaliacaoFinal = row.Field<Int32>("AvaliacaoFinal")
                                })
                                .Distinct();

            try
            {
                hfDataUltimaAlteracaoNotaFinal.Value = (from dadosGeral in dt.AsEnumerable()
                                                        where dadosGeral.Field<object>("atd_dataAlteracao") != null
                                                        select Convert.ToDateTime(dadosGeral.Field<object>("atd_dataAlteracao").ToString())).Max().ToString();
            }
            catch
            {
                hfDataUltimaAlteracaoNotaFinal.Value = String.Empty;
            }

            dtAvaliacaoFinal.Columns.Add("IsHead");

            if (dtAvaliacaoEnriqCurr.Rows.Count > 0)
            {
                DataRow drEnriquecimento = dtAvaliacaoFinal.NewRow();
                drEnriquecimento["IsHead"] = "1";
                drEnriquecimento["recuperacao"] = "0";
                dtAvaliacaoFinal.Rows.Add(drEnriquecimento);
                dtAvaliacaoFinal.Merge(dtAvaliacaoEnriqCurr);
            }

            if (dtAvaliacaoRecPar.Rows.Count > 0)
            {
                DataRow drRecPar = dtAvaliacaoFinal.NewRow();
                drRecPar["IsHead"] = "1";
                drRecPar["recuperacao"] = "1";
                dtAvaliacaoFinal.Rows.Add(drRecPar);
                dtAvaliacaoFinal.Merge(dtAvaliacaoRecPar);
            }

            dicSemNota = (from DataRow dr in dtAvaliacoes.Rows
                          group dr by Convert.ToInt64(dr["tud_id"]) into grupo
                          select new
                          {
                              tud_id = grupo.Key
                              ,
                              SemNota = grupo.Any(p => !Convert.ToBoolean(p["PossuiNota"]))
                          }).ToDictionary(p => p.tud_id, p => p.SemNota);

            rptNotaDisciplinas.DataSource = dtAvaliacaoFinal;
            rptNotaDisciplinas.DataBind();

            AdicionaItemsResultado(ddlResultado, 0);
            MTR_MatriculaTurma entMatr = EntMatriculaTurma;
            ddlResultado.SelectedValue = entMatr.mtu_resultado > 0 ? entMatr.mtu_resultado.ToString() : "-1";
            txtJustificativaParecerConclusivo.Text = hdnJustificativa.Text = entMatr.mtu_relatorio;
            hfDataUltimaAlteracaoParecerConclusivo.Value = entMatr.mtu_dataAlteracao.ToString();

            lblInseridoPor.Visible = lblUsuarioJustificativa.Visible = litNomeDocente.Visible = (entMatr.usu_idResultado != Guid.Empty);
            lblDataAlteracao.Visible = litDataAlteracao.Visible = ((entMatr.usu_idResultado != Guid.Empty) && !string.IsNullOrEmpty(hfDataUltimaAlteracaoParecerConclusivo.Value));

            if (lblInseridoPor.Visible)
            {
                SYS_Usuario usuario = new SYS_Usuario { usu_id = entMatr.usu_idResultado };
                usuario = SYS_UsuarioBO.GetEntity(usuario);
                if (usuario.pes_id != Guid.Empty)
                {
                    PES_Pessoa pessoa = new PES_Pessoa { pes_id = usuario.pes_id };
                    pessoa = PES_PessoaBO.GetEntity(pessoa);
                    litNomeDocente.Text = pessoa.pes_nome;
                    if (lblDataAlteracao.Visible)
                        litDataAlteracao.Text = hfDataUltimaAlteracaoParecerConclusivo.Value;
                    lblUsuarioJustificativa.Text = String.Format("<b>Inserido por: {0}</b>", pessoa.pes_nome);
                }
                else
                {
                    litNomeDocente.Text = usuario.usu_login;
                    if (lblDataAlteracao.Visible)
                        litDataAlteracao.Text = hfDataUltimaAlteracaoParecerConclusivo.Value;
                    lblUsuarioJustificativa.Text = String.Format("<b>Inserido por: {0}</b>", usuario.usu_login);
                }
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

        /// <summary>
        /// Seta os campos relacionados à avaliação como visíveis, e seta os valores de acordo com
        /// o tipo de escala.
        /// Se escala numérica, configura o txt. Se escala for por pareceres, configura o ddl.
        /// </summary>
        /// <param name="tipo">Tipo de escala de avaliação</param>
        /// <param name="txtNota">Textbox de nota</param>
        /// <param name="aat_avaliacao">Nota do aluno</param>
        /// <param name="ddlPareceres">Combo de pareceres</param>
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres, int esa_id)
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
                        if (!LtPareceres.Any())
                        {
                            LtPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id);
                        }

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
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, Label lblNota, string aat_avaliacao, int esa_id)
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
                    if (!LtPareceres.Any())
                    {
                        LtPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id);
                    }

                    var conceitos = LtPareceres
                                        .Select(eap => new
                                        {
                                            eap_valor = eap.eap_valor
                                            ,
                                            descricao = eap.descricao
                                        })
                                        .Where(row => row.eap_valor.ToUpper() == aat_avaliacao.ToUpper());

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
            return Math.Round(nota, RetornaNumeroCasasDecimais()).ToString();
        }

        /// <summary>
        /// Retorna o número de casas decimais de acordo com a variação da escala de avaliação
        /// (só se for do tipo numérica.
        /// </summary>
        /// <returns></returns>
        private int RetornaNumeroCasasDecimais()
        {
            int numeroCasasDecimais = 1;
            if (EntEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
            {
                ACA_EscalaAvaliacaoNumerica entEscalaNumerica = new ACA_EscalaAvaliacaoNumerica
                {
                    esa_id = EntEscalaAvaliacao.esa_id
                };
                ACA_EscalaAvaliacaoNumericaBO.GetEntity(entEscalaNumerica);

                // Calcula a quantidade de casas decimais da variação de notas
                string variacao = Convert.ToDouble(entEscalaNumerica.ean_variacao).ToString();
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
        /// Seta componentes relacionados ao relatório na linha do grid de acordo com os dados que já
        /// foram inseridos pelo usuário.
        /// </summary>
        /// <param name="commandArgument">String que será utilizada como commandArgument do botão do relatório</param>
        /// <param name="btnRelatorio">Botão do relatório</param>
        /// <param name="cvRelatorioDesempenho">Validação do relatório</param>
        /// <param name="imgSituacao">Icone da situação</param>
        /// <param name="hplAnexo">Link do anexo</param>
        private void SetaComponentesRelatorioLinhaGrid(string commandArgument, ImageButton btnRelatorio, CustomValidator cvRelatorioDesempenho, Image imgSituacao, HyperLink hplAnexo)
        {
            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;
            bool mostraBotaoRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            if (cvRelatorioDesempenho != null)
            {
                // Se for turma do PEJA, e estiver configurado pra exibir o relatório, ele deve ser validado na hora de salvar.
                cvRelatorioDesempenho.Visible = VS_turma_Peja && mostraBotaoRelatorio;
            }

            // Deixar com display na tela para poder acessar por javascript.
            if (btnRelatorio != null)
            {
                btnRelatorio.Visible = mostraBotaoRelatorio;

                btnRelatorio.CommandArgument = commandArgument;
                btnRelatorio.ToolTip = tipo == EscalaAvaliacaoTipo.Relatorios ? "Lançar relatório para o aluno" : "Preencher relatório sobre o desempenho do aluno";

                // Pesquisa o item pelo id.
                UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == btnRelatorio.CommandArgument);
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
                if (string.IsNullOrEmpty(arqIdRelatorio))
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
        /// Adiciona os itens de resultado no dropDownList.
        /// </summary>
        private void AdicionaItemsResultado(DropDownList ddl, long tud_id)
        {
            ddl.Items.Clear();

            // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
            ListItem item = tud_id > 0 ?
                new ListItem("-- Selecione um " + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.ParecerFinal") + " --", "-1") :
                new ListItem("-- Selecione um " + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") + " --", "-1");
            ddl.Items.Add(item);

            // Verifica se existe resultados para esse curso/curriculo/periodo
            List<Struct_TipoResultado> listaTiposResultados = tud_id > 0 ?
                ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(EntMatriculaTurma.cur_id, EntMatriculaTurma.crr_id, EntMatriculaTurma.crp_id, EnumTipoLancamento.Disciplinas) :
                ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(EntMatriculaTurma.cur_id, EntMatriculaTurma.crr_id, EntMatriculaTurma.crp_id, EnumTipoLancamento.ConceitoGlobal);

            if (listaTiposResultados.Count > 0)
            {
                foreach (Struct_TipoResultado tipoResultado in listaTiposResultados)
                {
                    item = new ListItem(tipoResultado.tpr_nomenclatura, tipoResultado.tpr_resultado.ToString());
                    ddl.Items.Add(item);
                }
                return;
            }

            // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
            item = new ListItem("Aprovado", "1");
            ddl.Items.Add(item);

            // Só mostra a opção "Reprovado", caso o critério de avaliação seja
            // Conceito Global + Frequência ou Conceito Global ou  Nota por Disciplina
            if ((ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia ||
                (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobal ||
                (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.NotaDisciplina)
            {
                item = new ListItem("Reprovado", "2");
                ddl.Items.Add(item);
            }

            // Só mostra a opção "Reprovado por frequência", caso o critério de avaliação seja
            // Conceito Global + Frequência ou Apenas frequência
            if ((ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ConceitoGlobalFrequencia ||
                (ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal)EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal.ApenasFrequencia)
            {
                // Adiciona os itens da tabela MTR_MatriculaTurma.
                item = new ListItem("Reprovado por frequência", "8");
                ddl.Items.Add(item);
            }
        }

        /// <summary>
        /// Seta dados da opção de nota por relatório e dados da tela de acordo com os dados do
        /// DataTable informado.
        /// </summary>
        /// <param name="dt">DataTable para pegar os dados</param>
        private void SetaDadosRelatorio(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow linha in dt.Rows)
                {
                    string id = linha["tud_id"] + ";" +
                                (linha["AvaliacaoID"] != DBNull.Value ? linha["AvaliacaoID"] : "-1");

                    UCEfetivacaoNotas.NotasRelatorio rel = new UCEfetivacaoNotas.NotasRelatorio
                    {
                        Id = id,
                        Valor = linha["Relatorio"].ToString(),
                        arq_idRelatorio = linha["arq_idRelatorio"].ToString()
                    };

                    _VS_Nota_Relatorio.Add(rel);
                }
            }
        }

        /// <summary>
        /// Seta dados da justificativa de nota final e dados da tela de acordo com os dados do
        /// DataTable informado.
        /// </summary>
        /// <param name="dt">DataTable para pegar os dados</param>
        private void SetaDadosJustificativaNotaFinal(DataTable dt, string nomeColunaJustificativaNotaFinal)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow linha in dt.Rows)
                {
                    string id = linha["tud_id"] + ";" +
                                (linha["AvaliacaoID"] != DBNull.Value ? linha["AvaliacaoID"] : "-1");

                    UCEfetivacaoNotas.Justificativa justificativa = new UCEfetivacaoNotas.Justificativa
                    {
                        Id = id,
                        Valor = linha[nomeColunaJustificativaNotaFinal].ToString()
                    };

                    VS_JustificativaNotaFinal.Add(justificativa);
                }
            }
        }

        /// <summary>
        /// Trata o evento do botao relatorio dentro do grid
        /// </summary>
        /// <param name="id"></param>
        private void TrataEventoCommandRelatorio(string id, string dis_nome)
        {
            if (AbrirRelatorio != null)
            {
                UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == id);
                string dadosAluno = lblDadosAluno.Text;
                dadosAluno += "<br /><b>" + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text") + ":</b> " + dis_nome;
                AbrirRelatorio(id, nota.Valor, nota.arq_idRelatorio, dadosAluno);
            }
        }

        /// <summary>
        /// Método para salvar o relatorio antes de imprimir
        /// </summary>
        public string ImprimirRelatorio(long alu_idSalvar, string idNotaRelatorio, string notaRelatorio, HttpPostedFile arquivoRelatorio, bool visivelAnexo, out EscalaAvaliacaoTipo esa_tipo)
        {
            esa_tipo = (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;

            // Carrega o relatorio caso ele tenha sido salvo
            UCEfetivacaoNotas.NotasRelatorio nota = _VS_Nota_Relatorio.Find(p => p.Id == idNotaRelatorio);
            return nota.arq_idRelatorio;
        }

        /// <summary>
        /// Salva os dados da justificativa da nota final que está sendo lançado.
        /// </summary>
        public void SalvarJustificativaNotaFinal(string id, string textoJustificativa)
        {
            if (VS_JustificativaNotaFinal.Exists(p => p.Id == id))
            {
                int alterar = VS_JustificativaNotaFinal.FindIndex(p => p.Id == id);
                VS_JustificativaNotaFinal[alterar] = new UCEfetivacaoNotas.Justificativa
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
                VS_JustificativaNotaFinal.Add(justificativa);
            }

            // Atualiza o gv
            uppNotaDisciplinas.Update();

            if (!String.IsNullOrEmpty(hdnLocalImgCheckSituacao.Value))
            {
                Control controlPrincipal = rptNotaDisciplinas.Items[Convert.ToInt32(hdnLocalImgCheckSituacao.Value)];
                Image imgCheckSituacao = (Image)controlPrincipal.FindControl("imgJustificativaNotaFinalSituacao");
                imgCheckSituacao.Visible = !String.IsNullOrEmpty(textoJustificativa);
                hdnLocalImgCheckSituacao.Value = String.Empty;
            }
        }

        /// <summary>
        /// Faz a validação dos dados na tela e gera as listas necessárias para salvar.
        /// </summary>
        /// <param name="listaDisciplina">Lista de disciplinas para usar para salvar</param>
        private void ValidaGeraDados(out List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina, out List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina)
        {
            listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();
            listaMatriculaTurmaDisciplina = new List<MTR_MatriculaTurmaDisciplina>();

            List<string> alunosErroIntervalo = new List<string>(),
                         alunosErroConversao = new List<string>();
            string stringErro = string.Empty;

            // Se a escala de avaliação é numérica.
            bool tipoEscalaNumerica = false;
            ACA_EscalaAvaliacaoNumerica escalaNumerica = new ACA_EscalaAvaliacaoNumerica();
            if ((EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo == EscalaAvaliacaoTipo.Numerica)
            {
                // Traz os valores limite para a validação da nota.
                tipoEscalaNumerica = true;
                escalaNumerica = new ACA_EscalaAvaliacaoNumerica { esa_id = EntEscalaAvaliacao.esa_id };
                ACA_EscalaAvaliacaoNumericaBO.GetEntity(escalaNumerica);
            }

            foreach (RepeaterItem rptItem in rptNotaDisciplinas.Items)
            {
                if (rptItem.FindControl("tdNotaFinal").Visible)
                {
                    if (tipoEscalaNumerica)
                    {
                        TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
                        if (txtNotaFinal != null && txtNotaFinal.Visible)
                        {
                            // Recupera o valor da avaliação normal.
                            if (!string.IsNullOrEmpty(txtNotaFinal.Text))
                            {
                                decimal nota;
                                if (decimal.TryParse(txtNotaFinal.Text, out nota))
                                {
                                    if ((nota < escalaNumerica.ean_menorValor) || (nota > escalaNumerica.ean_maiorValor))
                                    {
                                        alunosErroIntervalo.Add(((Label)rptItem.FindControl("lblNomeDisciplina")).Text);
                                    }
                                }
                                else
                                {
                                    alunosErroConversao.Add(((Label)rptItem.FindControl("lblNomeDisciplina")).Text);
                                }
                            }
                        }
                    }
                    AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                }
                else if (rptItem.FindControl("tdParecerFinal").Visible)
                {
                    DropDownList ddlParecerFinal = (DropDownList)rptItem.FindControl("ddlParecerFinal");
                    if (ddlParecerFinal != null && ddlParecerFinal.Visible)
                    {
                        listaMatriculaTurmaDisciplina.Add(new MTR_MatriculaTurmaDisciplina
                                                            {
                                                                alu_id = VS_alu_id
                                                                ,
                                                                mtu_id = VS_mtu_id
                                                                ,
                                                                mtd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtdId")).Value)
                                                                ,
                                                                mtd_resultado = Convert.ToByte(ddlParecerFinal.SelectedValue == "-1" ? "0" : ddlParecerFinal.SelectedValue)
                                                                ,
                                                                tud_id = Convert.ToInt64(((HiddenField)rptItem.FindControl("hfTudId")).Value)
                                                                ,
                                                                apenasResultado = true
                                                            });
                    }
                }
            }

            int numeroCasasDecimais = RetornaNumeroCasasDecimais();

            if (alunosErroIntervalo.Count == 1)
            {
                stringErro += string.Format(
                                "Nota de {0} está fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                string.Join(", ", alunosErroIntervalo.ToArray()),
                                Math.Round(escalaNumerica.ean_menorValor, numeroCasasDecimais),
                                Math.Round(escalaNumerica.ean_maiorValor, numeroCasasDecimais));
            }
            else if (alunosErroIntervalo.Count > 1)
            {
                stringErro += string.Format(
                                "Notas de {0} estão fora do intervalo estipulado na escala de avaliação (de {1} a {2}).<br />",
                                string.Join(", ", alunosErroIntervalo.ToArray()),
                                Math.Round(escalaNumerica.ean_menorValor, numeroCasasDecimais),
                                Math.Round(escalaNumerica.ean_maiorValor, numeroCasasDecimais));
            }

            if (alunosErroConversao.Count == 1)
            {
                stringErro += string.Format("Nota de {0} é inválida.", string.Join(", ", alunosErroConversao.ToArray()));
            }
            else if (alunosErroConversao.Count > 1)
            {
                stringErro += string.Format("Notas de {0} são inválidas.", string.Join(", ", alunosErroConversao.ToArray()));
            }

            if (!string.IsNullOrEmpty(stringErro))
            {
                throw new ValidationException(stringErro);
            }
        }

        /// <summary>
        /// Adiciona uma linha na lista com os dados da linha do grid.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        /// <param name="listaDisciplina"></param>
        private void AdicionaLinhaDisciplina(RepeaterItem rptItem, ref List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina)
        {
            TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
            DropDownList ddlPareceresFinal = (DropDownList)rptItem.FindControl("ddlPareceresFinal");
            if (txtNotaFinal.Visible || ddlPareceresFinal.Visible)
            {
                long tur_id = VS_tur_id;
                long tud_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfTudId")).Value);
                long alu_id = VS_alu_id;
                int mtu_id = VS_mtu_id;
                int mtd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtdId")).Value);

                int atd_id = -1;
                if (!String.IsNullOrEmpty(((HiddenField)rptItem.FindControl("hfAvaliacaoId")).Value))
                {
                    atd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAvaliacaoId")).Value);
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
                ent.ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAvaId")).Value);
                ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                // Setar o registroExterno para false.
                ent.atd_registroexterno = false;

                HtmlControl hcNotaFinal = (HtmlControl)rptItem.FindControl("tdNotaFinal");
                bool salvarRelatorio;
                ent.atd_avaliacao = RetornaAvaliacaoFinal(rptItem, out salvarRelatorio);
                if (!string.IsNullOrEmpty(ent.atd_avaliacao))
                {
                    ImageButton btnJustificativaNotaFinal = (ImageButton)rptItem.FindControl("btnJustificativaNotaFinal");
                    string commandArgument = btnJustificativaNotaFinal.CommandArgument;

                    if (VS_JustificativaNotaFinal.Exists(p => (p.Id == commandArgument)))
                    {
                        ent.atd_relatorio = VS_JustificativaNotaFinal.Find(p => (p.Id == commandArgument)).Valor;
                    }
                }
                else
                {
                    ent.atd_relatorio = string.Empty;
                }

                ent.arq_idRelatorio = 0;
                ent.atd_avaliacaoPosConselho = string.Empty;
                ent.atd_justificativaPosConselho = string.Empty;
                ent.atd_semProfessor = false;
                ent.tpc_id = -1;

                listaDisciplina.Add(new CLS_AvaliacaoTurDisc_Cadastro
                {
                    entity = ent,
                    resultado = 0,
                    atualizarResultado = false,
                    mtu_idAnterior = mtu_id,
                    mtd_idAnterior = mtd_id
                });
            }
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacaoFinal(Control row, out bool salvarRelatorio)
        {
            TextBox txtNota = (TextBox)row.FindControl("txtNotaFinal");
            DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceresFinal");

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;

            // Verifica se o lançamento é por relatório ou se é conceito global
            salvarRelatorio = tipo == EscalaAvaliacaoTipo.Relatorios;

            // Se o formato de avaliação for por conceito global
            salvarRelatorio = salvarRelatorio || ((ACA_FormatoAvaliacaoTipo)EntFormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.ConceitoGlobal);

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

        #endregion Parecer conclusivo

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCAlunoEfetivacaoObservacao.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsLimiteCaracteres.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            bool sucessoSalvarNotaFinal = true;
            byte resultado = 0;
            string justificativaResultado = string.Empty;

            List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();
            List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina = new List<MTR_MatriculaTurmaDisciplina>();

            try
            {
                if (VS_periodoFechado)
                    throw new ValidationException(String.Format("{0} disponível apenas para consulta.", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

                returns.Add("tipoObservacao", VS_tipoObservacao);
                if (VS_tipoObservacao == eTipoObservacao.Disciplina)
                {
                    CLS_AlunoAvaliacaoTurDis_Observacao observacao = RetornaObservacaoDisciplina();
                    CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoBO.SalvarObservacao(VS_tud_id, VS_alu_id, VS_mtu_id, VS_mtd_id, VS_fav_id, VS_ava_id, observacao);
                    returns.Add("observacao", observacao);
                }
                else
                {
                    // se a nota da ultima avaliação está habilitada para edição
                    // salvo as notas
                    if (liParecerConclusivo.Visible && !(__SessionWEB.__UsuarioWEB.Docente.doc_id > 0))
                    {
                        ValidaGeraDados(out listaDisciplina, out listaMatriculaTurmaDisciplina);
                    }

                    CLS_AlunoAvaliacaoTur_Observacao observacao = RetornaObservacao();
                    if (liParecerConclusivo.Visible)
                    {
                        resultado = Convert.ToByte(ddlResultado.SelectedValue == "-1" ? "0" : ddlResultado.SelectedValue);
                        justificativaResultado = hdnJustificativa.Text;
                    }
                    if (CLS_AlunoAvaliacaoTurmaObservacaoBO.SalvarObservacao(
                                                                VS_tur_id
                                                                , VS_alu_id
                                                                , VS_mtu_id
                                                                , VS_fav_id
                                                                , VS_ava_id
                                                                , observacao
                                                                , liParecerConclusivo.Visible ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty
                                                                , resultado
                                                                , justificativaResultado
                                                                , String.IsNullOrEmpty(hfDataUltimaAlteracaoParecerConclusivo.Value) ? DateTime.MaxValue : Convert.ToDateTime(hfDataUltimaAlteracaoParecerConclusivo.Value)
                                                                , String.IsNullOrEmpty(hfDataUltimaAlteracaoNotaFinal.Value) ? DateTime.MaxValue : Convert.ToDateTime(hfDataUltimaAlteracaoNotaFinal.Value)
                                                                , EntFormatoAvaliacao
                                                                , listaDisciplina
                                                                , listaMatriculaTurmaDisciplina
                                                                , ApplicationWEB.TamanhoMaximoArquivo
                                                                , ApplicationWEB.TiposArquivosPermitidos
                                                                , ref listaAtualizacaoEfetivacao
                                                                , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                                , VS_tpc_id))
                    {
                        switch (TipoFechamento)
                        {
                            case 1:
                                ((UCEfetivacaoNotas)_UCEfetivacaoNotas).MensagemTela = 
                                    UtilBO.GetErroMessage(
                                    CustomResource.GetGlobalResourceObject("UserControl", "UCAlunoEfetivacaoObservacaoGeral.Cadastro.SalvoComSucesso").ToString()
                                    , UtilBO.TipoMensagem.Sucesso);
                                break;
                            case 2:
                                ((UCFechamento)_UCEfetivacaoNotas).MensagemTela = 
                                    UtilBO.GetErroMessage(
                                    CustomResource.GetGlobalResourceObject("UserControl", "UCAlunoEfetivacaoObservacaoGeral.Cadastro.SalvoComSucesso").ToString()
                                    , UtilBO.TipoMensagem.Sucesso);
                                break;
                        }
                    }
                    returns.Add("observacao", observacao);
                }

                try
                {
                    if (listaDisciplina.Count > 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, VS_MensagemLogEfetivacaoObservacao + " Salvar avaliação final para as disciplinas do aluno na turma, tur_id: " + VS_tur_id + "; alu_id: " + VS_alu_id + "; ava_id: " + listaDisciplina[0].entity.ava_id);
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, VS_MensagemLogEfetivacaoObservacao
                                                                                + "tur_id: " + VS_tur_id
                                                                                + "; tud_id: " + VS_tud_id
                                                                                + "; alu_id: " + VS_alu_id
                                                                                + "; mtu_id: " + VS_mtu_id
                                                                                + "; mtd_id: " + VS_mtd_id
                                                                                + "; fav_id: " + VS_fav_id
                                                                                + "; ava_id: " + VS_ava_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }

                if (ReturnValues != null)
                    ReturnValues(returns, sucessoSalvarNotaFinal, listaAtualizacaoEfetivacao, resultado, listaMatriculaTurmaDisciplina);
                else
                    throw new NotImplementedException();
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                if (lblMensagem.Text == lblMensagem2.Text)
                {
                    lblMensagem2.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        #region Parecer conclusivo

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaPor_AlunoTurma(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_cal_id);

            hdnVariacaoFrequencia.Value = dt.Rows.Count > 0 ? dt.Rows[0]["fav_variacao"].ToString().Replace(',', '.') : "0.01";

            string ultimaDataAlteracao;
            try
            {
                ultimaDataAlteracao = (from dadosGeral in dt.AsEnumerable()
                                       where dadosGeral.Field<object>("atd_dataAlteracao") != null

                                       select Convert.ToDateTime(dadosGeral.Field<object>("atd_dataAlteracao").ToString())).Max().ToString();
            }
            catch
            {
                ultimaDataAlteracao = String.Empty;
            }

            // se a alteração do banco é mais recente, atualizo a informação na tela
            if (hfDataUltimaAlteracaoNotaFinal.Value != ultimaDataAlteracao)
            {
                DataTable dtAvaliacaoFinal;
                try
                {
                    dtAvaliacaoFinal = (from dadosGeral in dt.AsEnumerable()
                                        where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) == -1
                                        select dadosGeral).CopyToDataTable();
                }
                catch
                {
                    dtAvaliacaoFinal = new DataTable();
                }

                try
                {
                    dtAvaliacoes = (from dadosGeral in dt.AsEnumerable()
                                    where Convert.ToInt32(dadosGeral.Field<object>("tpc_id")) > 0
                                    select dadosGeral).CopyToDataTable();
                }
                catch
                {
                    dtAvaliacoes = new DataTable();
                }

                // Seta nome dos headers das colunas de nota..
                nomeAvaliacaoFinal = dtAvaliacaoFinal.Rows.Count > 0 ? dtAvaliacaoFinal.Rows[0]["NomeAvaliacao"].ToString() : "";

                // atualiza o header das avaliacoes
                objAvaliacoes = dtAvaliacoes.AsEnumerable()
                                .Select(row => new
                                {
                                    NomeAvaliacao = row.Field<string>("NomeAvaliacao")
                                    ,
                                    AvaliacaoFinal = row.Field<Int32>("AvaliacaoFinal")
                                })
                                .Distinct();

                if (rptNotaDisciplinas.Controls.Count > 0 && rptNotaDisciplinas.Controls[0].Controls.Count > 0)
                {
                    Repeater rptHeaderPeriodos = (Repeater)rptNotaDisciplinas.Controls[0].Controls[0].FindControl("rptHeaderPeriodos");
                    rptHeaderPeriodos.DataSource = objAvaliacoes;
                    rptHeaderPeriodos.DataBind();
                }

                DateTime dataRegistroUltimaAlteracao = Convert.ToDateTime(hfDataUltimaAlteracaoNotaFinal.Value);
                foreach (RepeaterItem rptItem in rptNotaDisciplinas.Items)
                {
                    if (((HiddenField)rptItem.FindControl("hfIsHead")).Value == "1")
                    {
                        Repeater rptPeriodos = (Repeater)rptItem.FindControl("rptHeaderPeriodosEnriqCurr");
                        rptPeriodos.DataSource = objAvaliacoes;
                        rptPeriodos.DataBind();
                    }
                    else
                    {
                        HtmlControl tdFrequenciaFinal = (HtmlControl)rptItem.FindControl("tdFrequenciaFinal");
                        Label lblFrequenciaFinalAjustada = (Label)rptItem.FindControl("lblFrequenciaFinalAjustada");
                        Int64 tud_id = Convert.ToInt64(((HiddenField)rptItem.FindControl("hfTudId")).Value);

                        var x = from DataRow dr in dtAvaliacaoFinal.Rows
                                where Convert.ToInt64(dr["tud_id"]) == tud_id
                                select dr;

                        if (x.Count() > 0)
                        {
                            DataRow drDisciplina = x.First();

                            if (!drDisciplina["FrequenciaFinalAjustada"].Equals(DBNull.Value))
                            {
                                lblFrequenciaFinalAjustada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, Convert.ToDecimal(drDisciplina["FrequenciaFinalAjustada"]));
                            }

                            Repeater rptItemPeriodos = (Repeater)rptItem.FindControl("rptItemPeriodos");
                            try
                            {
                                rptItemPeriodos.DataSource = (from tRow in dtAvaliacoes.AsEnumerable()
                                                              where Convert.ToInt64(tRow["tud_id"]) == tud_id
                                                              select tRow).CopyToDataTable();
                            }
                            catch
                            {
                                rptItemPeriodos.DataSource = new DataTable();
                            }
                            rptItemPeriodos.DataBind();

                            // só atualizo o registro se houve alteração depois da ultima atualização.
                            if (rptItem.FindControl("tdNotaFinal").Visible
                                && !drDisciplina["atd_dataAlteracao"].Equals(DBNull.Value)
                                && Convert.ToDateTime(drDisciplina["atd_dataAlteracao"].ToString()) > dataRegistroUltimaAlteracao)
                            {
                                HiddenField hf = (HiddenField)rptItem.FindControl("hfMtdId");
                                hf.Value = drDisciplina["mtd_id"].ToString();
                                hf = (HiddenField)rptItem.FindControl("hfAvaliacaoId");
                                hf.Value = (drDisciplina["AvaliacaoID"].Equals(DBNull.Value) ? "-1" : drDisciplina["AvaliacaoID"].ToString());
                                hf = (HiddenField)rptItem.FindControl("hfAvaId");
                                hf.Value = drDisciplina["ava_id"].ToString();

                                TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
                                DropDownList ddlPareceresFinal = (DropDownList)rptItem.FindControl("ddlPareceresFinal");
                                ImageButton btnJustificativaNotaFinal = (ImageButton)rptItem.FindControl("btnJustificativaNotaFinal");

                                int esa_tipoPorDisciplina = Convert.ToInt32(drDisciplina["esa_tipoPorDisciplina"]);

                                int esa_id = Convert.ToInt32(drDisciplina["esa_id"]);

                                // exibir notas
                                EscalaAvaliacaoTipo tipo = esa_tipoPorDisciplina > 0 ?
                                    (EscalaAvaliacaoTipo)esa_tipoPorDisciplina :
                                    (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;

                                // Seta campos da avaliação principal.
                                SetaCamposAvaliacao(tipo, txtNotaFinal, drDisciplina["Avaliacao"].ToString(), ddlPareceresFinal, esa_id);

                                if (btnJustificativaNotaFinal != null)
                                {
                                    string valorJustificativaNota = drDisciplina["Relatorio"].ToString();
                                    if (VS_JustificativaNotaFinal.Exists(p => p.Id == btnJustificativaNotaFinal.CommandArgument))
                                    {
                                        int alterar = VS_JustificativaNotaFinal.FindIndex(p => p.Id == btnJustificativaNotaFinal.CommandArgument);
                                        VS_JustificativaNotaFinal[alterar] = new UCEfetivacaoNotas.Justificativa
                                        {
                                            Id = btnJustificativaNotaFinal.CommandArgument,
                                            Valor = valorJustificativaNota
                                        };
                                    }
                                    else
                                    {
                                        UCEfetivacaoNotas.Justificativa justificativa = new UCEfetivacaoNotas.Justificativa
                                        {
                                            Id = btnJustificativaNotaFinal.CommandArgument,
                                            Valor = valorJustificativaNota
                                        };
                                        VS_JustificativaNotaFinal.Add(justificativa);
                                    }

                                    Image imgJustificativaNotaFinalSituacao = (Image)rptItem.FindControl("imgJustificativaNotaFinalSituacao");
                                    if (imgJustificativaNotaFinalSituacao != null)
                                    {
                                        imgJustificativaNotaFinalSituacao.Visible = !String.IsNullOrEmpty(valorJustificativaNota);
                                    }
                                }
                            }
                            else if (rptItem.FindControl("tdParecerFinal").Visible)
                            {
                                DropDownList ddlPareceresFinal = (DropDownList)rptItem.FindControl("ddlParecerFinal");
                                bool permiteAlterarResultadoFinal = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                                if (!drDisciplina["atd_dataAlteracao"].Equals(DBNull.Value)
                                    && Convert.ToDateTime(drDisciplina["atd_dataAlteracao"].ToString()) > dataRegistroUltimaAlteracao)
                                {
                                    HiddenField hf = (HiddenField)rptItem.FindControl("hfMtdId");
                                    hf.Value = drDisciplina["mtd_id"].ToString();
                                    hf = (HiddenField)rptItem.FindControl("hfAvaliacaoId");
                                    hf.Value = (drDisciplina["AvaliacaoID"].Equals(DBNull.Value) ? "-1" : drDisciplina["AvaliacaoID"].ToString());
                                    hf = (HiddenField)rptItem.FindControl("hfAvaId");
                                    hf.Value = drDisciplina["ava_id"].ToString();

                                    // atualiza o parecer final com o dado efetivado no banco,
                                    // se ele pode ser alterado ou
                                    // se o criterio para a selecao do resultado por frequencia nao for automatico
                                    if (permiteAlterarResultadoFinal
                                        || EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal != criterioFrequenciaFinalAjustadaDisciplina)
                                    {
                                        AdicionaItemsResultado(ddlPareceresFinal, tud_id);
                                        string valor = drDisciplina["Resultado"].ToString();
                                        if (ddlPareceresFinal.Items.FindByValue(valor) != null)
                                        {
                                            ddlPareceresFinal.SelectedValue = valor;
                                        }
                                    }
                                }

                                // atualiza o parecer final com o valor automatico
                                // se o criterio para a selecao do resultado por frequencia for automatico e
                                // se o parecer final nao pode ser alterado, ou nenhum parecer foi selecionado e posso atribuir um valor automatico
                                if (EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == criterioFrequenciaFinalAjustadaDisciplina
                                    && (!permiteAlterarResultadoFinal || (ddlPareceresFinal.SelectedIndex == 0 && EntFormatoAvaliacao.fav_sugerirResultadoFinalDisciplina)))
                                {
                                    if (!drDisciplina["FrequenciaFinalAjustada"].Equals(DBNull.Value))
                                    {
                                        string valorResultado = Convert.ToDecimal(drDisciplina["FrequenciaFinalAjustada"])
                                            >= EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina ?
                                                ((byte)TipoResultado.Aprovado).ToString() : ((byte)TipoResultado.ReprovadoFrequencia).ToString();

                                        if (ddlPareceresFinal.Items.FindByValue(valorResultado) != null)
                                        {
                                            ddlPareceresFinal.SelectedValue = valorResultado;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                hfDataUltimaAlteracaoNotaFinal.Value = ultimaDataAlteracao;
            }

            MTR_MatriculaTurma entMatr = new MTR_MatriculaTurma
            {
                alu_id = VS_alu_id
                ,
                mtu_id = VS_mtu_id
            };
            MTR_MatriculaTurmaBO.GetEntity(entMatr);
            // se a alteração do banco é mais recente, atualizo a informação na tela
            if (hfDataUltimaAlteracaoParecerConclusivo.Value != entMatr.mtu_dataAlteracao.ToString())
            {
                EntMatriculaTurma = entMatr;
                ddlResultado.SelectedValue = entMatr.mtu_resultado > 0 ? entMatr.mtu_resultado.ToString() : "-1";
                txtJustificativaParecerConclusivo.Text = hdnJustificativa.Text = entMatr.mtu_relatorio;
                hfDataUltimaAlteracaoParecerConclusivo.Value = entMatr.mtu_dataAlteracao.ToString();

                lblInseridoPor.Visible = litNomeDocente.Visible = (entMatr.usu_idResultado != Guid.Empty);
                lblDataAlteracao.Visible = litDataAlteracao.Visible = ((entMatr.usu_idResultado != Guid.Empty) && !string.IsNullOrEmpty(hfDataUltimaAlteracaoParecerConclusivo.Value));

                if (lblInseridoPor.Visible)
                {
                    SYS_Usuario usuario = new SYS_Usuario { usu_id = entMatr.usu_idResultado };
                    usuario = SYS_UsuarioBO.GetEntity(usuario);
                    if (usuario.pes_id != Guid.Empty)
                    {
                        PES_Pessoa pessoa = new PES_Pessoa { pes_id = usuario.pes_id };
                        pessoa = PES_PessoaBO.GetEntity(pessoa);
                        litNomeDocente.Text = pessoa.pes_nome;
                        if (lblDataAlteracao.Visible)
                            litDataAlteracao.Text = hfDataUltimaAlteracaoParecerConclusivo.Value;
                        lblUsuarioJustificativa.Text = String.Format("<b>Inserido por: {0}</b>", pessoa.pes_nome);
                    }
                    else
                    {
                        litNomeDocente.Text = usuario.usu_login;
                        if (lblDataAlteracao.Visible)
                            litDataAlteracao.Text = hfDataUltimaAlteracaoParecerConclusivo.Value;
                        lblUsuarioJustificativa.Text = String.Format("<b>Inserido por: {0}</b>", usuario.usu_login);
                    }
                }
            }
        }

        protected void rptNotaDisciplinas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptPeriodos;
                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;

                if (e.Item.ItemType == ListItemType.Header)
                {
                    ((Literal)e.Item.FindControl("litHeadNotaFinal")).Text = nomeAvaliacaoFinal;
                    rptPeriodos = (Repeater)e.Item.FindControl("rptHeaderPeriodos");
                    rptPeriodos.DataSource = objAvaliacoes;
                }
                else
                {
                    HtmlControl tdFrequenciaFinal = (HtmlControl)e.Item.FindControl("tdFrequenciaFinal");
                    HtmlControl tdNotaFinal = (HtmlControl)e.Item.FindControl("tdNotaFinal");
                    HtmlControl tdParecerFinal = (HtmlControl)e.Item.FindControl("tdParecerFinal");

                    if (DataBinder.Eval(e.Item.DataItem, "isHead").ToString() == "1")
                    {
                        ((HiddenField)e.Item.FindControl("hfIsHead")).Value = "1";
                        tdFrequenciaFinal.Visible =
                            tdNotaFinal.Visible =
                            tdParecerFinal.Visible = false;

                        bool recuperacao = DataBinder.Eval(e.Item.DataItem, "recuperacao").ToString() == "1";

                        Literal litHeadNomeDisciplinaEnriqCurr = (Literal)e.Item.FindControl("litHeadNomeDisciplinaEnriqCurr");
                        Literal litHeadNomeDisciplinaRecPar = (Literal)e.Item.FindControl("litHeadNomeDisciplinaRecPar");

                        litHeadNomeDisciplinaEnriqCurr.Visible = !recuperacao;
                        litHeadNomeDisciplinaRecPar.Visible = recuperacao;

                        rptPeriodos = (Repeater)e.Item.FindControl("rptHeaderPeriodosEnriqCurr");
                        rptPeriodos.DataSource = objAvaliacoes;
                    }
                    else
                    {
                        rptPeriodos = (Repeater)e.Item.FindControl("rptItemPeriodos");
                        Label lblFrequenciaFinalAjustada = (Label)e.Item.FindControl("lblFrequenciaFinalAjustada");

                        if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ComponenteRegencia")))
                        {
                            if (tdFrequenciaRegencia == null)
                            {
                                tdFrequenciaRegencia = tdFrequenciaFinal;
                            }
                            else
                            {
                                tdFrequenciaFinal.Visible = false;
                                tdFrequenciaRegencia.Style["border-left"] = tdFrequenciaRegencia.Style["border-right"] = "1px solid #DDD";
                                tdFrequenciaRegencia.Attributes["rowspan"] = String.IsNullOrEmpty(tdFrequenciaRegencia.Attributes["rowspan"]) ? "2"
                                                                                : (Convert.ToInt32(tdFrequenciaRegencia.Attributes["rowspan"]) + 1).ToString();
                            }
                        }

                        try
                        {
                            rptPeriodos.DataSource = (from tRow in dtAvaliacoes.AsEnumerable()
                                                      where Convert.ToInt64(tRow["tud_id"]) == Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"))
                                                            && Convert.ToInt32(tRow["AvaliacaoFinal"]) != 1
                                                      select tRow).CopyToDataTable();
                        }
                        catch
                        {
                            rptPeriodos.DataSource = new DataTable();
                        }

                        if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ExibirFrequencia")))
                        {
                            if (!DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").Equals(DBNull.Value))
                            {
                                lblFrequenciaFinalAjustada.Text = string.Format(VS_FormatacaoDecimaisFrequencia, Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada")));
                            }
                        }
                        else
                        {
                            lblFrequenciaFinalAjustada.Visible = false;
                        }

                        HiddenField hf = (HiddenField)e.Item.FindControl("hfTudId");
                        hf.Value = DataBinder.Eval(e.Item.DataItem, "tud_id").ToString();
                        bool exibirNota = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ExibirNota"));
                        long tud_id = Convert.ToInt64(string.IsNullOrEmpty(hf.Value) ? "-1" : hf.Value);

                        hf = (HiddenField)e.Item.FindControl("hfMtdId");
                        hf.Value = DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString();
                        hf = (HiddenField)e.Item.FindControl("hfAvaliacaoId");
                        hf.Value = (DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString());
                        hf = (HiddenField)e.Item.FindControl("hfAvaId");
                        hf.Value = DataBinder.Eval(e.Item.DataItem, "ava_id").ToString();

                        bool semNota = false;
                        if (dicSemNota.Any(p => p.Key == tud_id))
                        {
                            semNota = dicSemNota[tud_id];
                        }

                        if (exibirNota)
                        {
                            TextBox txtNotaFinal = (TextBox)e.Item.FindControl("txtNotaFinal");
                            DropDownList ddlPareceresFinal = (DropDownList)e.Item.FindControl("ddlPareceresFinal");
                            ImageButton btnJustificativaNotaFinal = (ImageButton)e.Item.FindControl("btnJustificativaNotaFinal");

                            int esa_tipoPorDisciplina = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "esa_tipoPorDisciplina"));

                            int esa_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "esa_id"));

                            // exibir notas
                            EscalaAvaliacaoTipo tipo = esa_tipoPorDisciplina > 0 ?
                                (EscalaAvaliacaoTipo)esa_tipoPorDisciplina :
                                (EscalaAvaliacaoTipo)tipoEscala;

                            // Seta campos da avaliação principal.
                            SetaCamposAvaliacao(tipo, txtNotaFinal, DataBinder.Eval(e.Item.DataItem, "Avaliacao").ToString(), ddlPareceresFinal, esa_id);

                            if (btnJustificativaNotaFinal != null)
                            {
                                btnJustificativaNotaFinal.CommandArgument = DataBinder.Eval(e.Item.DataItem, "tud_id") + ";" +
                                                                            (DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ?
                                                                            "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID"));
                                btnJustificativaNotaFinal.ToolTip = "Infomar justificativa da " + nomeAvaliacaoFinal.ToLower();

                                Image imgJustificativaNotaFinalSituacao = (Image)e.Item.FindControl("imgJustificativaNotaFinalSituacao");
                                if (imgJustificativaNotaFinalSituacao != null)
                                {
                                    imgJustificativaNotaFinalSituacao.Visible = !String.IsNullOrEmpty(VS_JustificativaNotaFinal.FirstOrDefault(p => p.Id == btnJustificativaNotaFinal.CommandArgument).Valor);
                                }
                            }

                            // se for docente, não pode editar a nota final
                            txtNotaFinal.Enabled =
                                ddlPareceresFinal.Enabled =
                                btnJustificativaNotaFinal.Enabled = __SessionWEB.__UsuarioWEB.Docente.doc_id <= 0 && !semNota;
                        }
                        else
                        {
                            DropDownList ddlPareceresFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");
                            AdicionaItemsResultado(ddlPareceresFinal, Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id")));
                            string valor = DataBinder.Eval(e.Item.DataItem, "Resultado").ToString();
                            if (ddlPareceresFinal.Items.FindByValue(valor) != null)
                            {
                                ddlPareceresFinal.SelectedValue = valor;
                            }

                            // se nao veio valor do banco,
                            // verifico se posso atribuir o parecer final automaticamente
                            bool permiteAlterarResultadoFinal = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                            if (ddlResultado.SelectedIndex == 0 && EntFormatoAvaliacao.fav_criterioAprovacaoResultadoFinal == criterioFrequenciaFinalAjustadaDisciplina &&
                                Convert.ToInt32((DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ? "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").ToString())) <= 0)
                            {
                                if (!permiteAlterarResultadoFinal || EntFormatoAvaliacao.fav_sugerirResultadoFinalDisciplina)
                                {
                                    if (!DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").Equals(DBNull.Value))
                                    {
                                        string valorResultado = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada"))
                                            >= EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina ?
                                                ((byte)TipoResultado.Aprovado).ToString() : ((byte)TipoResultado.ReprovadoFrequencia).ToString();

                                        if (ddlPareceresFinal.Items.FindByValue(valorResultado) != null)
                                        {
                                            ddlPareceresFinal.SelectedValue = valorResultado;
                                        }
                                    }
                                }
                            }

                            // se for docente, não pode editar o parecer final
                            ddlPareceresFinal.Enabled = permiteAlterarResultadoFinal && __SessionWEB.__UsuarioWEB.Docente.doc_id <= 0 && !semNota;
                        }

                        tdNotaFinal.Visible &= exibirNota;
                        tdParecerFinal.Visible &= !exibirNota;
                    }
                }
                rptPeriodos.DataBind();
            }
        }

        protected void rptNotaDisciplinas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Relatorio")
            {
                Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                hdnLocalImgCheckSituacao.Value = ((RepeaterItem)rowControl).ItemIndex.ToString();
                TrataEventoCommandRelatorio(e.CommandArgument.ToString(), ((Label)rowControl.FindControl("lblNomeDisciplina")).Text);
            }
            else if (e.CommandName == "JustificativaNotaFinal")
            {
                try
                {
                    Control rowControl = ((ImageButton)e.CommandSource).Parent.BindingContainer;
                    string pes_nome = lblDadosAluno.Text;
                    hdnLocalImgCheckSituacao.Value = ((RepeaterItem)rowControl).ItemIndex.ToString();
                    string id = e.CommandArgument.ToString();
                    UCEfetivacaoNotas.Justificativa justificativa = VS_JustificativaNotaFinal.Find(p => p.Id == id);
                    AbrirJustificativa(id, justificativa.Valor, pes_nome, ((Label)rowControl.FindControl("lblNomeDisciplina")).Text);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a justificativa da " + nomeAvaliacaoFinal.ToLower() + " do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rptHeaderPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem)
                        || (e.Item.ItemType == ListItemType.Item))
            {
                Literal litHeadPeriodo = (Literal)e.Item.FindControl("litHeadPeriodo");
                // exibir notas
                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;
                string nomeNota = "Nota";

                if ((VS_EfetivacaoSemestral) && (tipoEscala == EscalaAvaliacaoTipo.Numerica))
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

                if (DataBinder.Eval(e.Item.DataItem, "AvaliacaoFinal").ToString() == "1")
                {
                    litHeadPeriodo.Text = DataBinder.Eval(e.Item.DataItem, "NomeAvaliacao").ToString();
                }
                else
                {
                    litHeadPeriodo.Text = nomeNota + " " + DataBinder.Eval(e.Item.DataItem, "NomeAvaliacao");
                }
            }
        }

        protected void rptHeaderPeriodosEnriqCurr_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem)
                        || (e.Item.ItemType == ListItemType.Item))
            {
                Literal litHeadPeriodo = (Literal)e.Item.FindControl("litHeadPeriodo");

                if (DataBinder.Eval(e.Item.DataItem, "AvaliacaoFinal").ToString() == "1")
                {
                    litHeadPeriodo.Text = "Freq. Final (%)";
                }
                else
                {
                    litHeadPeriodo.Text = "%Freq. " + DataBinder.Eval(e.Item.DataItem, "NomeAvaliacao");
                }
            }
        }

        protected void rptItemPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem)
                        || (e.Item.ItemType == ListItemType.Item))
            {
                Label lblNota = (Label)e.Item.FindControl("lblNota");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                Image imgSituacao = (Image)e.Item.FindControl("imgSituacao");
                HyperLink hplAnexo = (HyperLink)e.Item.FindControl("hplAnexo");
                Label lblFrequencia = (Label)e.Item.FindControl("lblFrequencia");

                if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ExibirNota")))
                {
                    lblFrequencia.Visible = false;

                    int esa_tipoPorDisciplina = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "esa_tipoPorDisciplina"));

                    int esa_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "esa_id"));

                    // exibir notas
                    EscalaAvaliacaoTipo tipo = esa_tipoPorDisciplina > 0 ?
                        (EscalaAvaliacaoTipo)esa_tipoPorDisciplina :
                        (EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo;

                    // Seta campos da avaliação principal.
                    SetaCamposAvaliacao(tipo, lblNota, DataBinder.Eval(e.Item.DataItem, "Avaliacao").ToString(), esa_id);

                    string commandArgument = DataBinder.Eval(e.Item.DataItem, "tud_id") + ";" +
                                                (DataBinder.Eval(e.Item.DataItem, "AvaliacaoID").Equals(DBNull.Value) ?
                                                "-1" : DataBinder.Eval(e.Item.DataItem, "AvaliacaoID"));

                    SetaComponentesRelatorioLinhaGrid(commandArgument
                                                        , btnRelatorio
                                                        , null
                                                        , imgSituacao
                                                        , hplAnexo);
                }
                else
                {
                    lblNota.Visible =
                        btnRelatorio.Visible =
                        imgSituacao.Visible =
                        hplAnexo.Visible = false;
                    lblFrequencia.Text = DataBinder.Eval(e.Item.DataItem, "Frequencia").Equals(DBNull.Value) ? "-" : string.Format(VS_FormatacaoDecimaisFrequencia, Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Frequencia").ToString()));
                }
            }
        }

        protected void btnSalvarJustificativaParecerConclusivo_Click(object sender, EventArgs e)
        {
            hdnJustificativa.Text = txtJustificativaParecerConclusivo.Text;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "FecharJustificativaParecer", "$(document).ready(function(){ $('#divJustificativaParecerConclusivo').dialog('close'); });", true);
        }

        protected void imgJustificativaParecerConclusivo_Click(object sender, ImageClickEventArgs e)
        {
            txtJustificativaParecerConclusivo.Text = hdnJustificativa.Text;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "AbrirJustificativaParecer", "$(document).ready(function(){ $('#divJustificativaParecerConclusivo').dialog('open'); });", true);
        }

        #endregion Parecer conclusivo

        #endregion Eventos
    }
}