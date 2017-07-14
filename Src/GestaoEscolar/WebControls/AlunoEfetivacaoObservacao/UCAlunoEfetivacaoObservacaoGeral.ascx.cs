using GestaoEscolar.WebControls.EfetivacaoNotas;
using GestaoEscolar.WebControls.Fechamento;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.AlunoEfetivacaoObservacao
{
    public partial class UCAlunoEfetivacaoObservacaoGeral : MotherUserControl
    {
        #region Constantes

        /// <summary>
        /// Nome do resource
        /// </summary>
        protected const string RESOURCE_NAME = "UserControl";

        /// <summary>
        /// Prefixo da chave do resource
        /// </summary>
        protected const string RESOURCE_KEY = "UCAlunoEfetivacaoObservacaoGeral.Cadastro.{0}";

        #endregion

        #region Propriedades

        public int SelectedTab
        {
            set
            {
                txtSelectedTab.Value = value.ToString();
            }

            get
            {
                int selectedtab = 0;

                Int32.TryParse(txtSelectedTab.Value, out selectedtab);

                return selectedtab;
            }
        }

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
        /// Viewstate que armazena o ID da escola.
        /// </summary>
        private int VS_esc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esc_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_esc_id"] = value;
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
                return Convert.ToInt32(ViewState["VS_ava_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_ava_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ano do calendário.
        /// </summary>
        private int VS_cal_ano
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_ano"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_ano"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o id do calendário.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do currículo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do curso.
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do tipo de período do calendário.
        /// </summary>
        public int VS_tpc_id
        {
            get
            {
                if (ViewState["VS_tpc_id"] != null)
                    return Convert.ToInt32(ViewState["VS_tpc_id"]);
                return 0;
            }
            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        /// <summary>
        /// Id da anotação.
        /// </summary>
        public int VS_ano_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_ano_id"] ?? -1);
            }

            set
            {
                ViewState["VS_ano_id"] = value;
            }
        }

        /// <summary>
        /// Ids das turma-disciplinas listadas
        /// </summary>
        public long[] VS_tud_ids
        {
            get
            {
                return ViewState["VS_tud_ids"] as long[] ?? new long[0];
            }

            set
            {
                ViewState["VS_tud_ids"] = value;
            }
        }

        /// <summary>
        /// Lista com as configurações do serviço de pendência.
        /// </summary>
        public List<ACA_ConfiguracaoServicoPendencia> VS_ConfiguracaoServicoPendencia
        {
            get
            {
                if (ViewState["VS_ConfiguracaoServicoPendencia"] != null)
                {
                    return (List<ACA_ConfiguracaoServicoPendencia>)ViewState["VS_ConfiguracaoServicoPendencia"];
                }

                return new List<ACA_ConfiguracaoServicoPendencia>();
            }
            set
            {
                ViewState["VS_ConfiguracaoServicoPendencia"] = value;
            }
        }

        /// <summary>
        /// Lista com as anotações do aluno para exibir no grid.
        /// </summary>
        public List<ACA_AlunoAnotacao> VS_ListaAlunoAnotacaoGrid
        {
            get
            {
                if (ViewState["VS_ListaAlunoAnotacaoGrid"] != null)
                {
                    return (List<ACA_AlunoAnotacao>)ViewState["VS_ListaAlunoAnotacaoGrid"];
                }

                return new List<ACA_AlunoAnotacao>();
            }
            set
            {
                ViewState["VS_ListaAlunoAnotacaoGrid"] = value;
            }
        }

        /// <summary>
        /// Lista com as anotações que deverão ser salvas no banco.
        /// </summary>
        public List<ACA_AlunoAnotacao> VS_ListaAlunoAnotacaoSalvar
        {
            get
            {
                if (ViewState["VS_ListaAlunoAnotacaoSalvar"] != null)
                {
                    return (List<ACA_AlunoAnotacao>)ViewState["VS_ListaAlunoAnotacaoSalvar"];
                }

                return new List<ACA_AlunoAnotacao>();
            }
            set
            {
                ViewState["VS_ListaAlunoAnotacaoSalvar"] = value;
            }
        }

        /// <summary>
        /// Indica o tipo de fechamento da turma.
        /// </summary>
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
                return 0;
            }
        }

        /// <summary>
        /// Controle de efetivação de notas.
        /// </summary>
        private Control _UCEfetivacaoNotas
        {
            get
            {
                return this.Parent.Parent.Parent;
            }
        }

        /// <summary>
        /// Guarda a mensagem que deve aparecer antes de salvar o log.
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
        /// Retorna o parametro academico para exibição da compensação de ausência
        /// </summary>
        private bool ExibeCompensacaoAusencia
        {
            get
            {
                if (ViewState["_VS_Exibe_Compensacao_Ausencia"] == null)
                {
                    ViewState["_VS_Exibe_Compensacao_Ausencia"] = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(
                        eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                return Convert.ToBoolean(ViewState["_VS_Exibe_Compensacao_Ausencia"]);
            }
        }

        /// <summary>
        /// Formatação para a % de frequencia
        /// </summary>
        public string VS_FormatacaoPorcentagemFrequencia
        {
            get
            {
                if (ViewState["VS_FormatacaoPorcentagemFrequencia"] != null)
                    return ViewState["VS_FormatacaoPorcentagemFrequencia"].ToString();
                return string.Empty;
            }
            set
            {
                ViewState["VS_FormatacaoPorcentagemFrequencia"] = value;
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        private ACA_FormatoAvaliacao formatoAvaliacao;
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

        /// <summary>
        /// Retorna a escala de avaliação do formato. Se for lançamento na disciplina,
        /// retorna de acordo com o esa_idPorDisciplina, se for global, retorna
        /// o esa_idConceitoGlobal.
        /// </summary>
        private ACA_EscalaAvaliacao escala;
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

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> _ltPareceres;
        private List<ACA_EscalaAvaliacaoParecer> LtPareceres
        {
            get
            {
                return _ltPareceres ??
                       (_ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(EntEscalaAvaliacao.esa_id));
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
        /// Lista com os dados dos períodos do aluno.
        /// </summary>
        private List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> VS_ListaDadosPeriodo
        {
            get
            {
                if (ViewState["VS_ListaDadosPeriodo"] != null)
                {
                    return (List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>)ViewState["VS_ListaDadosPeriodo"];
                }
                return new List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>();
            }

            set
            {
                ViewState["VS_ListaDadosPeriodo"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o último período do calendário.
        /// </summary>
        private int VS_tpc_idUltimoPeriodo
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_idUltimoPeriodo"] ?? "-1");
            }

            set
            {
                ViewState["VS_tpc_idUltimoPeriodo"] = value;
            }
        }

        /// <summary>
        /// Indica se os dados da observação estarão visíveis.
        /// </summary>
        public bool ObservacaoVisible
        {
            set
            {
                pnlObservacao.Visible = value;
            }
        }

        /// <summary>
        /// Indica se a aba de Anotações do aluno estará visível.
        /// </summary>
        public bool AnotacoesAlunoVisible
        {
            get
            {
                return string.IsNullOrEmpty(hdfAbaAnotacaoAlunoVisivel.Value) ? true : Convert.ToBoolean(hdfAbaAnotacaoAlunoVisivel.Value);
            }
            set
            {
                //liAnotacoesAluno.Visible = fdsAnotacoes.Visible = value;
                hdfAbaAnotacaoAlunoVisivel.Value = value.ToString();
            }
        }

        /// <summary>
        /// Guarda o id do periodo do calendario selecionado na tela Minhas turmas.
        /// </summary>
        private int TpcIdFechamento
        {
            set
            {
                hdnTpcIdFechamento.Value = value.ToString();
            }
            get
            {
                return Convert.ToInt32(hdnTpcIdFechamento.Value);
            }
        }

        /// <summary>
        /// Mensagem.
        /// </summary>
        public string Mensagem
        {
            set
            {
                lblMensagem.Text = value;
            }
        }

        /// <summary>
        /// Pendência por disciplina
        /// </summary>
        private Dictionary<long, bool> VS_PendenciaPorTud
        {
            get
            {
                return (Dictionary<long, bool>)(ViewState["VS_PendenciaPorTud"] ?? new Dictionary<long, bool>());
            }

            set
            {
                ViewState["VS_PendenciaPorTud"] = value;
            }
        }

        /// <summary>
        /// Indica se o nome do aluno será exibido como documento oficial.
        /// </summary>
        private bool VS_DocumentoOficial
        {
            get
            {
                if (ViewState["VS_DocumentoOficial"] == null)
                {
                    ViewState["VS_DocumentoOficial"] = false;
                }

                return Convert.ToBoolean(ViewState["VS_DocumentoOficial"]);
            }

            set
            {
                ViewState["VS_DocumentoOficial"] = value;
            }
        }

        #endregion

        #region Variáveis

        /// <summary>
        /// Indica se deve mostrar o total.
        /// </summary>
        private bool mostraTotalLinha = false;

        /// <summary>
        /// Indica se deve mostrar o conceito global.
        /// </summary>
        protected bool mostraConceitoGlobal;

        /// <summary>
        /// The nome nota
        /// </summary>
        protected string nomeNota;

        /// <summary>
        /// Variável de controle para colocar Rowspan apenas na primeira linha quando for disciplina de regência.
        /// </summary>
        private bool ControleMescla = false;

        /// <summary>
        /// Variável de controle para colocar Rowspan apenas na primeira linha quando for disciplina de regência.
        /// </summary>
        private bool ControleMesclaDisciplinaRegencia = false;

        /// <summary>
        /// Guarda a quantidade de itens do tipo filho
        /// </summary>
        private int QtComponenteRegencia;

        /// <summary>
        /// Guarda a quantidade de itens a serem exibidos no boletim
        /// </summary>
        private int QtComponentes;

        /// <summary>
        /// Guarda se o evento do fechamento final está aberto
        /// </summary>
        private bool fechamentoFinalAberto = false;

        private List<ACA_Evento> ltEvento
        {
            get
            {
                if (ltEventoAux == null)
                {
                    ltEventoAux = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                }
                return ltEventoAux;
            }
        }

        private List<ACA_Evento> ltEventoAux;

        private bool exibirFaltasExternas = false;

        #endregion Variáveis

        #region Delegates

        public delegate void OnReturnValues(CLS_AlunoAvaliacaoTurmaObservacao entityObservacaoSelecionada, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina);

        public event OnReturnValues ReturnValues;

        #endregion Delegates

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Attributes.Remove("hide");
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsUCAlunoEfetivacaoObservacaoGeral.js"));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                }
                ControlarExibicaoAbas();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensagemResultadoErro.Attributes.Add("class", "hide");
                lblMensagemResultadoInvalido.Attributes.Add("class", "hide");
                byte resultado = 0;
                string justificativaResultado = string.Empty;

                List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
                List<CLS_AlunoAvaliacaoTurmaDisciplina> listaEfetivacaoPosConselho = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
                List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();
                List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina = new List<MTR_MatriculaTurmaDisciplina>();

                List<CLS_AlunoAvaliacaoTurma> listaAlunoAvaliacaoTurma = new List<CLS_AlunoAvaliacaoTurma>();

                List<CLS_AlunoAvaliacaoTurmaObservacao> listaObservacao = new List<CLS_AlunoAvaliacaoTurmaObservacao>();
                listaObservacao = CLS_AlunoAvaliacaoTurmaObservacaoBO.SelecionaListaPorAluno(VS_alu_id, VS_mtu_id, VS_tur_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), VS_DocumentoOficial);

                ValidaGeraDados(out listaDisciplina, out listaMatriculaTurmaDisciplina, out listaEfetivacaoPosConselho);

                // Resultado final.
                bool permiteEditarResultadoFinal = PermiteEditar(VS_tpc_idUltimoPeriodo);
                if (permiteEditarResultadoFinal)
                {
                    if (!string.IsNullOrEmpty(ddlResultado.SelectedValue))
                    {
                        resultado = Convert.ToByte(ddlResultado.SelectedValue == "-1" ? "0" : ddlResultado.SelectedValue);
                    }

                    if (resultado > 0 && 
                        !Convert.ToBoolean(VerificarIntegridadeParecerEOL(hdnCodigoEOLTurma.Value, hdnCodigoEOLAluno.Value, ddlResultado.SelectedItem.Text, false)))
                    {
                        throw new ValidationException("O parecer conclusivo selecionado está divergente com o cadastrado no EOL.");
                    }
                }

                foreach (RepeaterItem rptItem in rptJustificativaPosConselho.Items)
                {
                    if (PermiteEditar(Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTpcId")).Value)))
                    {
                        long tur_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTurId")).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnMtuId")).Value);
                        int fav_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnAvaId")).Value);
                        int aat_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnAatId")).Value);
                        TextBox txtJustificativaPosConselho = (TextBox)rptItem.FindControl("txtJustificativaPosConselho");

                        CLS_AlunoAvaliacaoTurma entityAat = new CLS_AlunoAvaliacaoTurma
                        {
                            tur_id = tur_id
                            ,
                            alu_id = VS_alu_id
                            ,
                            mtu_id = mtu_id
                            ,
                            aat_id = aat_id
                            ,
                            fav_id = fav_id
                            ,
                            ava_id = ava_id
                            ,
                            aat_justificativaPosConselho = txtJustificativaPosConselho.Text
                        };

                        listaAlunoAvaliacaoTurma.Add(entityAat);
                    }
                }

                CLS_AlunoAvaliacaoTurmaObservacao entityObservacaoSelecionada = null;
                IDictionary<int, int> dicAvaTpc = new Dictionary<int, int>();
                foreach (RepeaterItem rptItem in rptResumoDesempenho.Items)
                {
                    int tpcId = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTpcId")).Value);
                    if (PermiteEditar(tpcId))
                    {
                        long tur_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTurId")).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnMtuId")).Value);
                        int fav_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnAvaId")).Value);
                        TextBox txtResumoDesempenho = (TextBox)rptItem.FindControl("txtResumoDesempenho");

                        if (listaObservacao.Exists(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id))
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao observacao = listaObservacao.Find(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id);
                            observacao.ato_desempenhoAprendizado = txtResumoDesempenho.Text;
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = observacao;
                            }
                        }
                        else
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao entityObservacao = new CLS_AlunoAvaliacaoTurmaObservacao
                            {
                                tur_id = tur_id
                            ,
                                alu_id = VS_alu_id
                            ,
                                mtu_id = mtu_id
                            ,
                                fav_id = fav_id
                            ,
                                ava_id = ava_id
                            ,
                                ato_qualidade = string.Empty
                            ,
                                ato_desempenhoAprendizado = txtResumoDesempenho.Text
                            ,
                                ato_recomendacaoAluno = string.Empty
                            ,
                                ato_recomendacaoResponsavel = string.Empty
                            };

                            listaObservacao.Add(entityObservacao);
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = entityObservacao;
                            }
                        }
                    }
                }

                foreach (CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao item in VS_ListaDadosPeriodo)
                {
                    if (PermiteEditar(item.tpc_id))
                    {
                        dicAvaTpc.Add(item.ava_id, item.tpc_id);
                    }
                }

                foreach (RepeaterItem rptItem in rptRecomendacaoAluno.Items)
                {
                    int tpcId = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTpcId")).Value);
                    if (PermiteEditar(tpcId))
                    {
                        long tur_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTurId")).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnMtuId")).Value);
                        int fav_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnAvaId")).Value);
                        TextBox txtRecomendacaoAluno = (TextBox)rptItem.FindControl("txtRecomendacaoAluno");

                        if (listaObservacao.Exists(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id))
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao observacao = listaObservacao.Find(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id);
                            observacao.ato_recomendacaoAluno = txtRecomendacaoAluno.Text;
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = observacao;
                            }
                        }
                        else
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao entityObservacao = new CLS_AlunoAvaliacaoTurmaObservacao
                            {
                                tur_id = tur_id
                                ,
                                alu_id = VS_alu_id
                                ,
                                mtu_id = mtu_id
                                ,
                                fav_id = fav_id
                                ,
                                ava_id = ava_id
                                ,
                                ato_qualidade = string.Empty
                                ,
                                ato_desempenhoAprendizado = string.Empty
                                ,
                                ato_recomendacaoAluno = txtRecomendacaoAluno.Text
                                ,
                                ato_recomendacaoResponsavel = string.Empty
                            };

                            listaObservacao.Add(entityObservacao);
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = entityObservacao;
                            }
                        }
                    }
                }

                foreach (RepeaterItem rptItem in rptRecomendacaoResponsavel.Items)
                {
                    int tpcId = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTpcId")).Value);
                    if (PermiteEditar(tpcId))
                    {
                        long tur_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnTurId")).Value);
                        int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnMtuId")).Value);
                        int fav_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnAvaId")).Value);
                        TextBox txtRecomendacaoResp = (TextBox)rptItem.FindControl("txtRecomendacaoResp");

                        if (listaObservacao.Exists(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id))
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao observacao = listaObservacao.Find(p => p.tur_id == tur_id && p.alu_id == VS_alu_id && p.mtu_id == mtu_id && p.fav_id == fav_id && p.ava_id == ava_id);
                            observacao.ato_recomendacaoResponsavel = txtRecomendacaoResp.Text;
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = observacao;
                            }
                        }
                        else
                        {
                            CLS_AlunoAvaliacaoTurmaObservacao entityObservacao = new CLS_AlunoAvaliacaoTurmaObservacao
                            {
                                tur_id = tur_id
                                ,
                                alu_id = VS_alu_id
                                ,
                                mtu_id = mtu_id
                                ,
                                fav_id = fav_id
                                ,
                                ava_id = ava_id
                                ,
                                ato_qualidade = string.Empty
                                ,
                                ato_desempenhoAprendizado = string.Empty
                                ,
                                ato_recomendacaoAluno = string.Empty
                                ,
                                ato_recomendacaoResponsavel = txtRecomendacaoResp.Text
                            };

                            listaObservacao.Add(entityObservacao);
                            if (TpcIdFechamento > 0 && TpcIdFechamento == tpcId)
                            {
                                entityObservacaoSelecionada = entityObservacao;
                            }
                        }
                    }
                }

                if (CLS_AlunoAvaliacaoTurmaObservacaoBO.SalvarObservacao(
                                                            VS_tur_id
                                                            , VS_alu_id
                                                            , VS_mtu_id
                                                            , VS_tud_ids
                                                            , listaObservacao.Where(p => p.mtu_id > 0).ToList()
                                                            , listaAlunoAvaliacaoTurma
                                                            , listaEfetivacaoPosConselho
                                                            , VS_ListaAlunoAnotacaoSalvar
                                                            , permiteEditarResultadoFinal
                                                            , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                                                            , resultado
                                                            , String.IsNullOrEmpty(hfDataUltimaAlteracaoParecerConclusivo.Value) ? DateTime.MaxValue : Convert.ToDateTime(hfDataUltimaAlteracaoParecerConclusivo.Value)
                                                            , DateTime.MaxValue
                                                            , EntFormatoAvaliacao
                                                            , listaDisciplina
                                                            , listaMatriculaTurmaDisciplina
                                                            , ApplicationWEB.TamanhoMaximoArquivo
                                                            , ApplicationWEB.TiposArquivosPermitidos
                                                            , ref listaAtualizacaoEfetivacao
                                                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                            , dicAvaTpc
                                                            , VS_ListaDadosPeriodo
                                ))

                {
                    switch (TipoFechamento)
                    {
                        case 1:
                            ((UCEfetivacaoNotas)_UCEfetivacaoNotas).MensagemTela = UtilBO.GetErroMessage(RetornaValorResource("SalvoComSucesso"), UtilBO.TipoMensagem.Sucesso);
                            break;
                        case 2:
                            ((UCFechamento)_UCEfetivacaoNotas).MensagemTela = UtilBO.GetErroMessage(RetornaValorResource("SalvoComSucesso"), UtilBO.TipoMensagem.Sucesso);
                            break;
                        default:
                            CarregarDadosAluno(VS_alu_id, VS_mtu_id, VS_tur_id, VS_esc_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), VS_DocumentoOficial);
                            lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("SalvoComSucesso"), UtilBO.TipoMensagem.Sucesso);
                            break;
                    }
                    VS_ListaAlunoAnotacaoSalvar = null;
                }

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, VS_MensagemLogEfetivacaoObservacao + " Salvar avaliação para as disciplinas do aluno na turma, tur_id: " + VS_tur_id + "; alu_id: " + VS_alu_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }

                if (ReturnValues != null)
                    ReturnValues(entityObservacaoSelecionada, listaAtualizacaoEfetivacao, resultado, listaMatriculaTurmaDisciplina);
                else
                    throw new NotImplementedException();
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('$(\\'#divCadastroObservacaoGeral\\').scrollTo(0,0);', 0);", true);
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('$(\\'#divCadastroObservacaoGeral\\').scrollTo(0,0);', 0);", true);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroSalvar"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDisciplinas_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (!mostraTotalLinha)
                    {
                        Literal litTotalFaltas = (Literal)e.Item.FindControl("litTotalFaltas");
                        if (litTotalFaltas != null)
                        {
                            litTotalFaltas.Text = "-";
                        }
                    }
                    mostraTotalLinha = false;

                    //Mescla as linhas de falta, total de ausencias, %freq e total comp. para os componentes da regencia
                    byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                    int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                    int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));
                    byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tur_tipo"));

                    if (tipoComponenteRegencia(tud_tipo))
                    {
                        if (!ControleMesclaDisciplinaRegencia)
                        {
                            if (ExibeCompensacaoAusencia)
                            {
                                var tdTotAusenciasCompensadas = new HtmlTableCell();
                                tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                                tdTotAusenciasCompensadas.RowSpan = QtComponenteRegencia;
                                tdTotAusenciasCompensadas.Visible = true;

                                if (exibirFaltasExternas)
                                {
                                    var tdTotFaltasExternas = new HtmlTableCell();
                                    tdTotFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                                    tdTotFaltasExternas.RowSpan = QtComponenteRegencia;
                                    tdTotFaltasExternas.Visible = true;
                                }

                                var tdTotFrequenciaAjustada = new HtmlTableCell();
                                tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                                tdTotFrequenciaAjustada.RowSpan = QtComponenteRegencia;
                                tdTotFrequenciaAjustada.Visible = true;
                            }

                            ControleMesclaDisciplinaRegencia = true;
                        }
                        else
                        {
                            if (ExibeCompensacaoAusencia)
                            {
                                var tdTotAusenciasCompensadas = new HtmlTableCell();
                                tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                                tdTotAusenciasCompensadas.Visible = false;

                                var tdTotFaltasExternas = new HtmlTableCell();
                                tdTotFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                                tdTotFaltasExternas.Visible = false;

                                var tdTotFrequenciaAjustada = new HtmlTableCell();
                                tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                                tdTotFrequenciaAjustada.Visible = false;
                            }
                        }
                        ControleMescla = true;
                    }
                    else if (ExibeCompensacaoAusencia)
                    {
                        var tdTotAusenciasCompensadas = new HtmlTableCell();
                        tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                        tdTotAusenciasCompensadas.Visible = true;

                        if (exibirFaltasExternas)
                        {
                            var tdTotFaltasExternas = new HtmlTableCell();
                            tdTotFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                            tdTotFaltasExternas.Visible = true;
                        }

                        var tdTotFrequenciaAjustada = new HtmlTableCell();
                        tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                        tdTotFrequenciaAjustada.Visible = true;
                    }

                    // Campo nota final.
                    Label lblNotaFinal = (Label)e.Item.FindControl("lblNotaFinal");
                    TextBox txtNotaFinal = (TextBox)e.Item.FindControl("txtNotaFinal");
                    DropDownList ddlParecerFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");
                    SetaCamposAvaliacao((EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo, txtNotaFinal, DataBinder.Eval(e.Item.DataItem, "MediaFinal").ToString(),
                                        ddlParecerFinal, lblNotaFinal, VS_tpc_idUltimoPeriodo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre").ToString()),
                                        Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()), fechamentoFinalAberto, true);
                    
                    HtmlTableCell tdNotaFinal = (HtmlTableCell)e.Item.FindControl("tdNotaFinal");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdNotaFinal, true, DataBinder.Eval(e.Item.DataItem, "MediaFinal").ToString(), false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()), -1, -1,
                                    VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semSintese));

                    HtmlTableCell tdFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFrequenciaAjustada, false, "", false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                    string freqFinal = DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").ToString();

                    if (!VS_ListaDadosPeriodo.Any(p => p.mtu_id == VS_mtu_id))
                    {
                        Literal litFrequenciaAjustada = (Literal)e.Item.FindControl("litFrequenciaAjustada");
                        if (litFrequenciaAjustada != null)
                        {
                            litFrequenciaAjustada.Text = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(freqFinal) && freqFinal != "-")
                    {
                        decimal frequenciaFinalAjustada = Convert.ToDecimal(freqFinal);
                        if (frequenciaFinalAjustada < EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                        {
                            tdFrequenciaAjustada.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                            tdFrequenciaAjustada.Style.Add("border-style", "solid");
                        }
                    }

                    HtmlTableCell tdAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdAusenciasCompensadas, false, "", false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);

                    HtmlTableCell tdFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFaltasExternas, false, "", false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDisciplinasEnriquecimentoCurricular_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    bool permiteEditar = PermiteEditar(VS_tpc_idUltimoPeriodo) && fechamentoFinalAberto;
                    bool bimestreAtivo = BimestreAtivo(VS_tpc_idUltimoPeriodo);

                    DropDownList ddlParecerFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");

                    byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                    int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                    int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));
                    byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tur_tipo"));

                    if (ddlParecerFinal != null)
                    {
                        bool lancaParecerFinal = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "lancaParecerFinal"));

                        AdicionaItemsResultado(ddlParecerFinal, Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id")), VS_cur_id, VS_crr_id, VS_crp_id, false);
                        string valor = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "parecerFinal"));
                        if (ddlParecerFinal.Items.FindByText(valor) != null && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre")))
                            ddlParecerFinal.SelectedValue = ddlParecerFinal.Items.FindByText(valor).Value;

                        HtmlTableCell tdParecerFinal = (HtmlTableCell)e.Item.FindControl("tdParecerFinal");
                        AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdParecerFinal, lancaParecerFinal, valor, false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                        Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()), -1, -1,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semResultadoFinal));
                        ddlParecerFinal.Enabled = permiteEditar && TipoFechamento <= 0 && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre"));
                        ddlParecerFinal.Visible = permiteEditar && TipoFechamento <= 0 && bimestreAtivo && lancaParecerFinal;

                        Label lblParecerFinal = (Label)e.Item.FindControl("lblParecerFinal");
                        if (lblParecerFinal != null)
                        {
                            if (permiteEditar && TipoFechamento <= 0 && bimestreAtivo)
                            {
                                lblParecerFinal.Visible = false;
                            }
                            else
                            {
                                lblParecerFinal.Visible = true;
                                lblParecerFinal.Text = string.IsNullOrEmpty(valor) ? "-" : valor;
                            }
                        }
                    }

                    if (exibirFaltasExternas)
                    {
                        var tdTotFaltasExternas = new HtmlTableCell();
                        tdTotFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                        tdTotFaltasExternas.Visible = true;
                    }

                    //Mescla as linhas de falta, total de ausencias e total comp. para os componentes da regencia
                    if (tipoComponenteRegencia(tud_tipo))
                    {
                        if (!ControleMesclaDisciplinaRegencia)
                        {
                            ControleMesclaDisciplinaRegencia = true;
                        }

                        ControleMescla = true;
                    }

                    HtmlTableCell tdFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFrequenciaAjustada, false, "", false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                    Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                    string freqFinal = DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").ToString();

                    if (!VS_ListaDadosPeriodo.Any(p => p.mtu_id == VS_mtu_id))
                    {
                        Literal litFrequenciaAjustada = (Literal)e.Item.FindControl("litFrequenciaAjustadaEnriquec");
                        if (litFrequenciaAjustada != null)
                        {
                            litFrequenciaAjustada.Text = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(freqFinal) && freqFinal != "-")
                    {
                        decimal frequenciaFinalAjustada = Convert.ToDecimal(freqFinal);
                        if (frequenciaFinalAjustada < EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                        {
                            tdFrequenciaAjustada.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                            tdFrequenciaAjustada.Style.Add("border-style", "solid");
                        }
                    }

                    HtmlTableCell tdFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFaltasExternas, false, "", false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDisciplinasEnsinoInfantil_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    bool permiteEditar = PermiteEditar(VS_tpc_idUltimoPeriodo) && fechamentoFinalAberto;
                    bool bimestreAtivo = BimestreAtivo(VS_tpc_idUltimoPeriodo);

                    DropDownList ddlParecerFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");

                    byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                    int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                    int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));
                    byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tur_tipo"));

                    if (ddlParecerFinal != null)
                    {
                        bool lancaParecerFinal = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "lancaParecerFinal"));

                        AdicionaItemsResultado(ddlParecerFinal, Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id")), VS_cur_id, VS_crr_id, VS_crp_id, false);
                        string valor = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "parecerFinal"));
                        if (ddlParecerFinal.Items.FindByText(valor) != null && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre")))
                            ddlParecerFinal.SelectedValue = ddlParecerFinal.Items.FindByText(valor).Value;

                        HtmlTableCell tdParecerFinal = (HtmlTableCell)e.Item.FindControl("tdParecerFinal");
                        AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdParecerFinal, lancaParecerFinal, valor, false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                        Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()), -1, -1,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semResultadoFinal));
                        ddlParecerFinal.Enabled = permiteEditar && TipoFechamento <= 0 && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre"));
                        ddlParecerFinal.Visible = permiteEditar && TipoFechamento <= 0 && bimestreAtivo && lancaParecerFinal;

                        Label lblParecerFinal = (Label)e.Item.FindControl("lblParecerFinal");
                        if (lblParecerFinal != null)
                        {
                            if (permiteEditar && TipoFechamento <= 0 && bimestreAtivo)
                            {
                                lblParecerFinal.Visible = false;
                            }
                            else
                            {
                                lblParecerFinal.Visible = true;
                                lblParecerFinal.Text = string.IsNullOrEmpty(valor) ? "-" : valor;
                            }
                        }
                    }

                    if (exibirFaltasExternas)
                    {
                        var tdTotFaltasExternas = new HtmlTableCell();
                        tdTotFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                        tdTotFaltasExternas.Visible = true;
                    }

                    //Mescla as linhas de falta, total de ausencias e total comp. para os componentes da regencia
                    if (tipoComponenteRegencia(tud_tipo))
                    {
                        if (!ControleMesclaDisciplinaRegencia)
                        {
                            ControleMesclaDisciplinaRegencia = true;
                        }

                        ControleMescla = true;
                    }

                    HtmlTableCell tdFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFrequenciaAjustada, false, "", false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                    Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                    string freqFinal = DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").ToString();

                    if (!VS_ListaDadosPeriodo.Any(p => p.mtu_id == VS_mtu_id))
                    {
                        Literal litFrequenciaAjustada = (Literal)e.Item.FindControl("litFrequenciaAjustadaEI");
                        if (litFrequenciaAjustada != null)
                        {
                            litFrequenciaAjustada.Text = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(freqFinal) && freqFinal != "-")
                    {
                        decimal frequenciaFinalAjustada = Convert.ToDecimal(freqFinal);
                        if (frequenciaFinalAjustada < EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                        {
                            tdFrequenciaAjustada.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                            tdFrequenciaAjustada.Style.Add("border-style", "solid");
                        }
                    }

                    HtmlTableCell tdFaltasExternas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasExternas");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFaltasExternas, false, "", false, false, false, false, false, false,
                                    eSituacaoMatriculaTurmaDisicplina.Ativo, Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptNotasDisciplina_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (!mostraTotalLinha)
                        mostraTotalLinha = (DataBinder.Eval(e.Item.DataItem, "nota") != null
                                            && (DataBinder.Eval(e.Item.DataItem, "nota.Nota").ToString() != "-"
                                                || DataBinder.Eval(e.Item.DataItem, "nota.numeroFaltas").ToString() != "-"));

                    byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "nota.tud_Tipo"));
                    if (tipoComponenteRegencia(tud_tipo))
                    {
                        if (!ControleMescla)
                        {
                            var tdFaltas = new HtmlTableCell();
                            tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                            tdFaltas.RowSpan = QtComponenteRegencia;
                        }
                        else
                        {
                            var tdFaltas = new HtmlTableCell();
                            tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                            tdFaltas.Visible = false;
                        }
                    }

                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));
                    int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                    int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));
                    byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tur_tipo"));
                    string nota = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.Nota"));
                    if (string.IsNullOrEmpty(nota) || nota == "-")
                    {
                        nota = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.NotaPosConselho"));
                    }

                    // Campo nota pós-conselho
                    TextBox txtNota = (TextBox)e.Item.FindControl("txtNotaFinal");
                    DropDownList ddlPareceres = (DropDownList)e.Item.FindControl("ddlParecerFinal");
                    Label lblNotaPosConselho = (Label)e.Item.FindControl("lblNotaPosConselho");
                    bool esconderPendencia = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "nota.esconderPendencia") ?? true);

                    SetaCamposAvaliacao((EscalaAvaliacaoTipo)EntEscalaAvaliacao.esa_tipo, txtNota, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.NotaPosConselho")),
                                        ddlPareceres, lblNotaPosConselho, tpc_id, true, esconderPendencia, true, false);

                    bool existeAulaBimestre = false;
                    bool.TryParse(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.existeAulaBimestre")), out existeAulaBimestre);

                    bool possuiAnotacaoRP = false;
                    bool.TryParse(DataBinder.Eval(e.Item.DataItem, "nota.possuiAnotacaoRP").ToString(), out possuiAnotacaoRP);

                    bool existeAulaSemPlano = false;
                    bool.TryParse(DataBinder.Eval(e.Item.DataItem, "nota.existeAulaSemPlano").ToString(), out existeAulaSemPlano);

                    eSituacaoMatriculaTurmaDisicplina SituacaoDisciplina = (eSituacaoMatriculaTurmaDisicplina)(DataBinder.Eval(e.Item.DataItem, "nota.SituacaoDisciplina") ?? eSituacaoMatriculaTurmaDisicplina.Ativo);


                    HtmlTableCell tdConceito = (HtmlTableCell)e.Item.FindControl("tdConceito");
                    AlterarCorFundo(tpc_id, tdConceito, false, nota, false, existeAulaBimestre, false, false, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, -1, -1,
                                    VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semNota));

                    HtmlTableCell tdNota = (HtmlTableCell)e.Item.FindControl("tdNota");
                    AlterarCorFundo(tpc_id, tdNota, true, nota, false, existeAulaBimestre, false, false, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, -1, -1,
                                    VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semNota));

                    HtmlTableCell tdNotaPosConselho = (HtmlTableCell)e.Item.FindControl("tdNotaPosConselho");
                    AlterarCorFundo(tpc_id, tdNotaPosConselho, true, nota, false, existeAulaBimestre, false, false, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, -1, -1,
                                    VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semNota));

                    bool recuperacao = false;
                    bool enriquecimentoCurricular = false;
                    bool ensinoInfantil = false;
                    bool.TryParse(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.recuperacao")), out recuperacao);
                    bool.TryParse(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.enriquecimentoCurricular")), out enriquecimentoCurricular);
                    bool.TryParse(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.ensinoInfantil")), out ensinoInfantil);

                    bool validarQtdAulas = (recuperacao || enriquecimentoCurricular || ensinoInfantil);
                    HtmlTableCell tdQtdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                    if (recuperacao)
                    {
                        long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "nota.tud_id"));
                        int cal_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "nota.cal_id"));
                        AlterarCorFundo(tpc_id, tdQtdFaltas, false, nota, validarQtdAulas, existeAulaBimestre, false, false, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, tud_id, cal_id,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_disciplinaSemAula));

                        AlterarCorFundo(tpc_id, tdQtdFaltas, false, nota, false, existeAulaBimestre, false, false, validarQtdAulas, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, tud_id, cal_id,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semPlanoAula));

                        eConfiguracaoServicoPendenciaSemRelatorioAtendimento cspRelatorio = eConfiguracaoServicoPendenciaSemRelatorioAtendimento.Nenhum;
                        if (VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semRelatorioAtendimento > 0))
                        {
                            cspRelatorio = 
                                (eConfiguracaoServicoPendenciaSemRelatorioAtendimento)Enum.Parse
                                (typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento), 
                                VS_ConfiguracaoServicoPendencia.Find(p => (p.tne_id == tne_id || p.tne_id <= 0) && 
                                                                          (p.tme_id == tme_id || p.tme_id <= 0) && 
                                                                          (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && 
                                                                          p.csp_semRelatorioAtendimento > 0).csp_semRelatorioAtendimento.ToString());
                        }

                        AlterarCorFundo(tpc_id, tdQtdFaltas, false, nota, false, existeAulaBimestre, true, possuiAnotacaoRP, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, tud_id, cal_id,
                                      cspRelatorio.HasFlag(eConfiguracaoServicoPendenciaSemRelatorioAtendimento.RP));
                    }
                    else
                    {
                        AlterarCorFundo(tpc_id, tdQtdFaltas, false, nota, validarQtdAulas, existeAulaBimestre, false, false, false, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, -1, -1,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_disciplinaSemAula));

                        AlterarCorFundo(tpc_id, tdQtdFaltas, false, nota, false, existeAulaBimestre, false, false, true, existeAulaSemPlano, SituacaoDisciplina, esconderPendencia, -1, -1,
                                        VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semPlanoAula));
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDisciplinasRecuperacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    bool permiteEditar = PermiteEditar(VS_tpc_idUltimoPeriodo) && fechamentoFinalAberto;
                    bool bimestreAtivo = BimestreAtivo(VS_tpc_idUltimoPeriodo);
                    long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"));
                    int cal_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "cal_id"));
                    int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                    int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));
                    byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tur_tipo"));

                    DropDownList ddlParecerFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");
                    if (ddlParecerFinal != null)
                    {
                        AdicionaItemsResultado(ddlParecerFinal, tud_id, VS_cur_id, VS_crr_id, VS_crp_id, false);
                        string valor = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "parecerFinal"));
                        if (ddlParecerFinal.Items.FindByText(valor) != null && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre")))
                            ddlParecerFinal.SelectedValue = ddlParecerFinal.Items.FindByText(valor).Value;

                        HtmlTableCell tdParecerFinal = (HtmlTableCell)e.Item.FindControl("tdParecerFinal");
                        AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdParecerFinal, true, valor, false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                        Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto
                                        , tud_id, cal_id, VS_ConfiguracaoServicoPendencia.Any(p => (p.tne_id == tne_id || p.tne_id <= 0) && (p.tme_id == tme_id || p.tme_id <= 0) && (p.tur_tipo == tur_tipo || p.tur_tipo <= 0) && p.csp_semResultadoFinal));
                        ddlParecerFinal.Enabled = permiteEditar && TipoFechamento <= 0 && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "UltimoBimestre"));
                        ddlParecerFinal.Visible = permiteEditar && TipoFechamento <= 0 && bimestreAtivo;

                        Label lblParecerFinal = (Label)e.Item.FindControl("lblParecerFinal");
                        if (lblParecerFinal != null)
                        {
                            if (permiteEditar && TipoFechamento <= 0 && bimestreAtivo)
                            {
                                lblParecerFinal.Visible = false;
                            }
                            else
                            {
                                lblParecerFinal.Visible = true;
                                lblParecerFinal.Text = string.IsNullOrEmpty(valor) ? "-" : valor;
                            }
                        }

                    }

                    HtmlTableCell tdFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    AlterarCorFundo(VS_tpc_idUltimoPeriodo, tdFrequenciaAjustada, false, "", false, false, false, false, false, false, eSituacaoMatriculaTurmaDisicplina.Ativo,
                                    Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "esconderPendenciaFinal").ToString()) || !fechamentoFinalAberto
                                    , tud_id, cal_id);
                    string freqFinal = DataBinder.Eval(e.Item.DataItem, "FrequenciaFinalAjustada").ToString();

                    if (!VS_ListaDadosPeriodo.Any(p => p.mtu_id == VS_mtu_id))
                    {
                        Literal litFrequenciaAjustada = (Literal)e.Item.FindControl("litFrequenciaAjustada");
                        if (litFrequenciaAjustada != null)
                        {
                            litFrequenciaAjustada.Text = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(freqFinal) && freqFinal != "-")
                    {
                        decimal frequenciaFinalAjustada = Convert.ToDecimal(freqFinal);
                        if (frequenciaFinalAjustada < EntFormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
                        {
                            tdFrequenciaAjustada.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                            tdFrequenciaAjustada.Style.Add("border-style", "solid");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptJustificativaPosConselho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                    TextBox txtJustificativaPosConselho = (TextBox)e.Item.FindControl("txtJustificativaPosConselho");
                    HtmlGenericControl divJustificativaPosConselho = (HtmlGenericControl)e.Item.FindControl("divJustificativaPosConselho");
                    ImageButton btnTextoGrandeBimestre = (ImageButton)e.Item.FindControl("btnTextoGrandeBimestre");
                    ImageButton btnVoltaEstadoAnteriorTextoBimestre = (ImageButton)e.Item.FindControl("btnVoltaEstadoAnteriorTextoBimestre");
                    Label lblPeriodoCalendario = (Label)e.Item.FindControl("lblPeriodoCalendario");
                    HtmlGenericControl divPeriodoCalendario = (HtmlGenericControl)e.Item.FindControl("divPeriodoCalendario");

                    AtribuirPermissaoCamposPeriodo(tpc_id, btnTextoGrandeBimestre, btnVoltaEstadoAnteriorTextoBimestre, txtJustificativaPosConselho, divJustificativaPosConselho, lblPeriodoCalendario, divPeriodoCalendario);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptResumoDesempenho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                    TextBox txtResumoDesempenho = (TextBox)e.Item.FindControl("txtResumoDesempenho");
                    HtmlGenericControl divResumoDesempenho = (HtmlGenericControl)e.Item.FindControl("divResumoDesempenho");
                    ImageButton btnTextoGrandeBimestre = (ImageButton)e.Item.FindControl("btnTextoGrandeBimestre");
                    ImageButton btnVoltaEstadoAnteriorTextoBimestre = (ImageButton)e.Item.FindControl("btnVoltaEstadoAnteriorTextoBimestre");
                    Label lblPeriodoCalendario = (Label)e.Item.FindControl("lblPeriodoCalendario");
                    HtmlGenericControl divPeriodoCalendario = (HtmlGenericControl)e.Item.FindControl("divPeriodoCalendario");

                    AtribuirPermissaoCamposPeriodo(tpc_id, btnTextoGrandeBimestre, btnVoltaEstadoAnteriorTextoBimestre, txtResumoDesempenho, divResumoDesempenho, lblPeriodoCalendario, divPeriodoCalendario);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptRecomendacaoAluno_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                    TextBox txtRecomendacaoAluno = (TextBox)e.Item.FindControl("txtRecomendacaoAluno");
                    HtmlGenericControl divRecomendacaoAluno = (HtmlGenericControl)e.Item.FindControl("divRecomendacaoAluno");
                    ImageButton btnTextoGrandeBimestre = (ImageButton)e.Item.FindControl("btnTextoGrandeBimestre");
                    ImageButton btnVoltaEstadoAnteriorTextoBimestre = (ImageButton)e.Item.FindControl("btnVoltaEstadoAnteriorTextoBimestre");
                    Label lblPeriodoCalendario = (Label)e.Item.FindControl("lblPeriodoCalendario");
                    HtmlGenericControl divPeriodoCalendario = (HtmlGenericControl)e.Item.FindControl("divPeriodoCalendario");

                    AtribuirPermissaoCamposPeriodo(tpc_id, btnTextoGrandeBimestre, btnVoltaEstadoAnteriorTextoBimestre, txtRecomendacaoAluno, divRecomendacaoAluno, lblPeriodoCalendario, divPeriodoCalendario);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptRecomendacaoResponsavel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                    TextBox txtRecomendacaoResp = (TextBox)e.Item.FindControl("txtRecomendacaoResp");
                    HtmlGenericControl divRecomendacaoResp = (HtmlGenericControl)e.Item.FindControl("divRecomendacaoResp");
                    ImageButton btnTextoGrandeBimestre = (ImageButton)e.Item.FindControl("btnTextoGrandeBimestre");
                    ImageButton btnVoltaEstadoAnteriorTextoBimestre = (ImageButton)e.Item.FindControl("btnVoltaEstadoAnteriorTextoBimestre");
                    Label lblPeriodoCalendario = (Label)e.Item.FindControl("lblPeriodoCalendario");
                    HtmlGenericControl divPeriodoCalendario = (HtmlGenericControl)e.Item.FindControl("divPeriodoCalendario");

                    AtribuirPermissaoCamposPeriodo(tpc_id, btnTextoGrandeBimestre, btnVoltaEstadoAnteriorTextoBimestre, txtRecomendacaoResp, divRecomendacaoResp, lblPeriodoCalendario, divPeriodoCalendario);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDesempenho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkDesempenho = (LinkButton)e.Item.FindControl("lnkDesempenho");
                    lnkDesempenho.OnClientClick = String.Format("IncluirTextoArea('resumoDesempenho','{0}'); return false;", lnkDesempenho.Text);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptRecomendacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkRecomendacaoAluno = (LinkButton)e.Item.FindControl("lnkRecomendacaoAluno");
                    lnkRecomendacaoAluno.OnClientClick = String.Format("IncluirTextoArea('recomendacaoAluno','{0}'); return false;", lnkRecomendacaoAluno.Text);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptRecomendacaoResp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkRecomendacaoResp = (LinkButton)e.Item.FindControl("lnkRecomendacaoResp");
                    lnkRecomendacaoResp.OnClientClick = String.Format("IncluirTextoArea('recomendacaoResp','{0}'); return false;", lnkRecomendacaoResp.Text);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAddAnotacao_Click(object sender, EventArgs e)
        {
            try
            {
                VS_ano_id = -1;
                txtDataAnotacao.Text = DateTime.Today.ToShortDateString();
                txtAnotacao.Text = "";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddAnotacao", "$('.divCadastroAnotacao').dialog('open');", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroSalvarAnotacaoAluno"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAnotacoesGerais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                    if (btnExcluir != null)
                    {
                        btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    }

                    ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.CommandArgument = e.Row.RowIndex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAnotacoesGerais_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_ano_id = Convert.ToInt32(grvAnotacoesGerais.DataKeys[index]["ano_id"].ToString());

                    ACA_AlunoAnotacao ano = VS_ListaAlunoAnotacaoGrid.Find(p => p.alu_id == VS_alu_id && p.ano_id == VS_ano_id);
                    txtDataAnotacao.Text = ano.ano_dataAnotacao.ToShortDateString();
                    txtAnotacao.Text = ano.ano_anotacao;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddAnotacao", "$('.divCadastroAnotacao').dialog('open');", true);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int ano_id = Convert.ToInt32(grvAnotacoesGerais.DataKeys[index]["ano_id"].ToString());

                    List<ACA_AlunoAnotacao> listaGrid = VS_ListaAlunoAnotacaoGrid;
                    List<ACA_AlunoAnotacao> listaSalvar = VS_ListaAlunoAnotacaoSalvar;

                    ACA_AlunoAnotacao ano = listaGrid.Find(p => p.alu_id == VS_alu_id && p.ano_id == ano_id);
                    ano.ano_dataAlteracao = DateTime.Now;
                    ano.ano_situacao = 3;

                    listaGrid.RemoveAll(p => p.alu_id == VS_alu_id && p.ano_id == ano_id);
                    listaSalvar.RemoveAll(p => p.alu_id == VS_alu_id && p.ano_id == ano_id);
                    listaSalvar.Add(ano);

                    VS_ListaAlunoAnotacaoGrid = listaGrid;
                    VS_ListaAlunoAnotacaoSalvar = listaSalvar;
                    LoadGridAnotacoes();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroExcluirAnotacao"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnSalvarAnotacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    DateTime dataAnotacao = new DateTime();
                    if (!DateTime.TryParse(txtDataAnotacao.Text, out dataAnotacao))
                        throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData(RetornaValorResource("DataAnotacao")));
                    if (dataAnotacao == new DateTime())
                        throw new ValidationException(RetornaValorResource("DataAnotacaoInvalida"));
                    if (dataAnotacao > VS_ListaDadosPeriodo.OrderBy(p => p.tpc_ordem).Last().cap_dataFim)
                        throw new ValidationException(RetornaValorResource("DataAnotacaoAnoLetivo"));

                    List<ACA_AlunoAnotacao> listaGrid = VS_ListaAlunoAnotacaoGrid;
                    List<ACA_AlunoAnotacao> listaSalvar = VS_ListaAlunoAnotacaoSalvar;

                    ACA_AlunoAnotacao ano = listaGrid.Find(p => p.alu_id == VS_alu_id && p.ano_id == VS_ano_id);
                    if (ano == null)
                    {
                        ano = new ACA_AlunoAnotacao();
                    }
                    ano.alu_id = VS_alu_id;
                    if (VS_ano_id <= 0)
                    {
                        if (listaGrid.Count() > 0)
                        {
                            VS_ano_id = Convert.ToInt32(listaGrid.Max(p => p.ano_id)) + 1;
                        }
                        else
                        {
                            VS_ano_id = 1;
                        }
                    }
                    ano.ano_id = VS_ano_id;
                    ano.ano_anotacao = txtAnotacao.Text;
                    ano.ano_dataAnotacao = Convert.ToDateTime(txtDataAnotacao.Text);
                    ano.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                    ano.gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id;
                    ano.gru_nome = __SessionWEB.__UsuarioWEB.Grupo.gru_nome;
                    ano.ano_situacao = 1;
                    ano.ano_dataAlteracao = DateTime.Now;

                    // Caso seja inclusão, adiciona nas duas listas.
                    if (listaGrid.Exists(p => p.alu_id == VS_alu_id && p.ano_id == VS_ano_id))
                    {
                        // Atualiza o dado nas listas em caso de alteração.
                        listaSalvar.RemoveAll(p => p.alu_id == VS_alu_id && p.ano_id == VS_ano_id);
                        listaSalvar.Add(ano);

                        listaGrid.RemoveAll(p => p.alu_id == VS_alu_id && p.ano_id == VS_ano_id);
                        listaGrid.Add(ano);
                    }
                    else
                    {
                        ano.IsNew = true;
                        ano.ano_dataCriacao = DateTime.Now;

                        listaGrid.Add(ano);
                        listaSalvar.Add(ano);
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharAnotacao", "$('.divCadastroAnotacao').dialog('close');", true);

                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("AvisoInclusaoAnotacao"), UtilBO.TipoMensagem.Informacao);

                    VS_ListaAlunoAnotacaoGrid = listaGrid;
                    VS_ListaAlunoAnotacaoSalvar = listaSalvar;
                    LoadGridAnotacoes();
                }
            }
            catch (ValidationException ex)
            {
                lblMessageCadastro.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroSalvarAnotacao"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarDadosAluno(VS_alu_id, VS_mtu_id, VS_tur_id, VS_esc_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), VS_DocumentoOficial, TpcIdFechamento);

                if (TipoFechamento <= 0 && ReturnValues != null)
                {
                    ReturnValues(new CLS_AlunoAvaliacaoTurmaObservacao(), new List<CLS_AlunoAvaliacaoTurmaDisciplina>(), 0, new List<MTR_MatriculaTurmaDisciplina>());
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnParecerConclusivo_Click(object sender, EventArgs e)
        {
            CarregarParecerConclusivo(VS_alu_id, VS_mtu_id, VS_tpc_id);
            btnParecerConclusivo.Visible = false;
            SelectedTab = 0;
        }

        protected void aDesempenho_ServerClick(object sender, EventArgs e)
        {
            rptResumoDesempenho.DataSource = VS_ListaDadosPeriodo;
            rptResumoDesempenho.DataBind();

            CarregarDesempenhoAprendizado(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);

            btnDesempenho.Visible = false;
            SelectedTab = 2;
        }

        protected void aJusstificativa_ServerClick(object sender, EventArgs e)
        {
            rptJustificativaPosConselho.DataSource = VS_ListaDadosPeriodo;
            rptJustificativaPosConselho.DataBind();
            btnJustificativaPosConselho.Visible = false;
            SelectedTab = 1;
        }

        protected void aRecomendacaoAluno_ServerClick(object sender, EventArgs e)
        {
            rptRecomendacaoAluno.DataSource = VS_ListaDadosPeriodo;
            rptRecomendacaoAluno.DataBind();

            CarregarRecomendacaoAluno(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);
            btnRecomendacaoAluno.Visible = false;
            SelectedTab = 3;
        }

        protected void aRecomendacaoResponsavel_ServerClick(object sender, EventArgs e)
        {
            rptRecomendacaoResponsavel.DataSource = VS_ListaDadosPeriodo;
            rptRecomendacaoResponsavel.DataBind();

            CarregarRecomendacaoResponsavel(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);

            btnRecomendacaoResponsavel.Visible = false;
            SelectedTab = 4;
        }

        protected void aAnotacao_ServerClick(object sender, EventArgs e)
        {
            if (liAnotacoesAluno.Visible || TipoFechamento <= 0)
            {
                // Carrega as anotações.
                grvAnotacoes.DataSource = CLS_TurmaAulaAlunoBO.SelecionaAnotacaoPorAlunoCalendario(VS_alu_id, VS_cal_ano, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0);
                grvAnotacoes.DataBind();

                VS_ListaAlunoAnotacaoGrid = ACA_AlunoAnotacaoBO.SelecionaAnotacoesAluno(VS_alu_id, VS_cal_ano);
            }
            else
            {
                grvAnotacoes.DataSource = new DataTable();
                grvAnotacoes.DataBind();

                VS_ListaAlunoAnotacaoGrid = new List<ACA_AlunoAnotacao>();
            }

            grvAnotacoesGerais.DataSource = VS_ListaAlunoAnotacaoGrid;
            grvAnotacoesGerais.DataBind();

            btnAnotacao.Visible = false;
            SelectedTab = 5;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os dados do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula do aluno.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="tev_idFechamento">Id do tipo de evento de fechamento.</param>
        public void CarregarDadosAluno(long alu_id, int mtu_id, long tur_id, int esc_id, int tev_idFechamento, bool documentoOficial, int tpcIdSelecionado = -1)
        {
            VS_DocumentoOficial = documentoOficial;
            lblMensagem.Text = string.Empty;
            VS_ListaDadosPeriodo = CLS_AlunoAvaliacaoTurmaObservacaoBO.SelecionarPorAluno(alu_id, mtu_id, tur_id, tev_idFechamento, documentoOficial);

            btnJustificativaPosConselho.Visible = btnDesempenho.Visible = btnRecomendacaoAluno.Visible =
                btnRecomendacaoResponsavel.Visible = btnAnotacao.Visible = btnParecerConclusivo.Visible = true;

            if (VS_ListaDadosPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao dados = VS_ListaDadosPeriodo.Find(p => p.bimestreAtual);

                VS_tur_id = tur_id;
                VS_alu_id = alu_id;
                VS_mtu_id = mtu_id;
                VS_cal_ano = dados.cal_ano;
                VS_cal_id = dados.cal_id;
                VS_fav_id = dados.fav_id;
                VS_ava_id = dados.ava_id;
                VS_esc_id = esc_id;
                VS_tpc_id = dados.tpc_id;
                VS_tpc_idUltimoPeriodo = VS_ListaDadosPeriodo.Find(p => p.ultimoPeriodo == true).tpc_id;
                TpcIdFechamento = tpcIdSelecionado;

                #region Cabeçalho - dados do aluno

                // Carrega os dados do aluno.
                lblNomeAluno.Text = dados.pes_nome;
                if (dados.mtu_numeroChamada > 0)
                {
                    lblNumeroChamada.Text = Convert.ToString(dados.mtu_numeroChamada);
                }
                else
                {
                    lblNumeroChamada.Text = "-";
                }
                lblCodigoEol.Text = dados.alc_matricula;

                hdnCodigoEOLAluno.Value = dados.alc_matricula;
                hdnCodigoEOLTurma.Value = dados.tur_codigoEOL;

                lblMensagemResultadoInvalido.Text = UtilBO.GetErroMessage("O parecer conclusivo selecionado está divergente com o cadastrado no EOL.", UtilBO.TipoMensagem.Alerta);

                lblMensagemResultadoErro.Text = UtilBO.GetErroMessage("Erro ao tentar buscar os dados cadastrados no EOL.", UtilBO.TipoMensagem.Alerta);

                lblSituacaoMatriculaEntrada.Text = string.Format("{0} {1:dd/MM/yyyy}", RetornaValorResource("MatriculadoEm"), dados.mtu_dataMatricula);
                if (dados.mtu_dataSaida != DateTime.MinValue)
                {
                    lblSituacaoMatriculaSaida.Text = string.Format("{0} {1:dd/MM/yyyy}", RetornaValorResource("TransferidoEm"), dados.mtu_dataSaida);
                }
                else
                {
                    lblSituacaoMatriculaSaida.Text = "";
                }

                if (dados.arq_idFoto > 0)
                {
                    imgFotoAluno.ImageUrl = string.Format("~/WebControls/BoletimCompletoAluno/Imagem.ashx?idfoto={0}", dados.arq_idFoto);
                }
                else
                {
                    imgFotoAluno.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "imgsAreaAluno/fotoAluno.png";
                }

                #endregion

                #region Rodapé - Histórico de alteração

                if (!String.IsNullOrEmpty(dados.usuarioAlteracao))
                {
                    lblHistoricoObservacao.Visible = true;
                    lblHistoricoObservacao.Text = String.Format(CustomResource.GetGlobalResourceObject(RESOURCE_NAME, "UCAlunoEfetivacaoObservacaoGeral.lblHistoricoObservacao.Text").ToString(),
                                                   dados.ato_dataAlteracao.ToString("dd/MM/yyyy"), dados.ato_dataAlteracao.ToString("HH:mm"), dados.usuarioAlteracao);
                }
                else
                {
                    lblHistoricoObservacao.Visible = false;
                }

                #endregion

                //Limpa todos os repeaters ao carregar a popup
                LimparDados();

                switch (SelectedTab)
                {
                    case 0:
                        CarregarParecerConclusivo(alu_id, mtu_id, dados.tpc_id);
                        btnParecerConclusivo.Visible = false;
                        break;
                    case 1:
                        rptJustificativaPosConselho.DataSource = VS_ListaDadosPeriodo;
                        rptJustificativaPosConselho.DataBind();
                        btnJustificativaPosConselho.Visible = false;
                        break;
                    case 2:
                        rptResumoDesempenho.DataSource = VS_ListaDadosPeriodo;
                        rptResumoDesempenho.DataBind();
                        CarregarDesempenhoAprendizado(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);
                        btnDesempenho.Visible = false;
                        break;
                    case 3:
                        rptRecomendacaoAluno.DataSource = VS_ListaDadosPeriodo;
                        rptRecomendacaoAluno.DataBind();
                        CarregarRecomendacaoAluno(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);
                        btnRecomendacaoAluno.Visible = false;
                        break;
                    case 4:
                        rptRecomendacaoResponsavel.DataSource = VS_ListaDadosPeriodo;
                        rptRecomendacaoResponsavel.DataBind();
                        CarregarRecomendacaoResponsavel(VS_tur_id, VS_alu_id, VS_mtu_id, VS_fav_id, VS_ava_id);
                        btnRecomendacaoResponsavel.Visible = false;
                        break;
                    case 5:
                        if (liAnotacoesAluno.Visible || TipoFechamento <= 0)
                        {
                            // Carrega as anotações.
                            grvAnotacoes.DataSource = CLS_TurmaAulaAlunoBO.SelecionaAnotacaoPorAlunoCalendario(alu_id, dados.cal_ano, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0);
                            grvAnotacoes.DataBind();

                            VS_ListaAlunoAnotacaoGrid = ACA_AlunoAnotacaoBO.SelecionaAnotacoesAluno(VS_alu_id, VS_cal_ano);
                        }
                        else
                        {
                            grvAnotacoes.DataSource = new DataTable();
                            grvAnotacoes.DataBind();

                            VS_ListaAlunoAnotacaoGrid = new List<ACA_AlunoAnotacao>();
                        }

                        grvAnotacoesGerais.DataSource = VS_ListaAlunoAnotacaoGrid;
                        grvAnotacoesGerais.DataBind();

                        btnAnotacao.Visible = false;
                        break;
                    default:
                        break;
                }

                string tituloJustificativa = CustomResource.GetGlobalResourceObject(RESOURCE_NAME, "UCAlunoEfetivacaoObservacaoGeral.JustificativaNotaPosConselho").ToString();
                if (EntEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                {
                    tituloJustificativa = CustomResource.GetGlobalResourceObject(RESOURCE_NAME, "UCAlunoEfetivacaoObservacaoGeral.JustificativaConceitoPosConselho").ToString();
                }

                litJustificativaPosConselho.Text = tituloJustificativa;

                ObservacaoVisible = true;
            }

            if (tpcIdSelecionado > 0)
            {
                btnSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar) && PermiteEditar(tpcIdSelecionado);
            }
            else
            {
                btnSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
            }
        }

        /// <summary>
        /// Limpa os dados das outras abas.
        /// </summary>
        private void LimparDados()
        {
            rptJustificativaPosConselho.DataSource = new List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>();
            rptJustificativaPosConselho.DataBind();

            rptResumoDesempenho.DataSource = new List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>();
            rptResumoDesempenho.DataBind();
            rptDesempenho.DataSource = new List<ACA_TipoDesempenhoAprendizado>();
            rptDesempenho.DataBind();

            rptRecomendacaoAluno.DataSource = new List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>();
            rptRecomendacaoAluno.DataBind();
            rptRecomendacao.DataSource = new List<ACA_RecomendacaoAlunoResponsavel>();
            rptRecomendacao.DataBind();

            rptRecomendacaoResponsavel.DataSource = new List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao>();
            rptRecomendacaoResponsavel.DataBind();
            rptRecomendacaoResp.DataSource = new List<ACA_RecomendacaoAlunoResponsavel>();
            rptRecomendacaoResp.DataBind();

            grvAnotacoes.DataSource = new DataTable();
            grvAnotacoes.DataBind();

            grvAnotacoesGerais.DataSource = new List<ACA_AlunoAnotacao>();
            grvAnotacoesGerais.DataBind();
        }

        /// <summary>
        /// Carrega os dados do parecer conclusivo.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula do aluno.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        private void CarregarParecerConclusivo(long alu_id, int mtu_id, int tpc_id)
        {
            List<ACA_AlunoBO.BoletimDadosAlunoFechamento> lDadosAluno = CLS_AlunoAvaliacaoTurmaDisciplinaBO.BuscaDadosAlunoFechamentoGestor(alu_id, mtu_id, tpc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (lDadosAluno.Count > 0)
            {
                ACA_AlunoBO.BoletimDadosAlunoFechamento dadosAluno = lDadosAluno[0];
                VS_cur_id = dadosAluno.cur_id;
                VS_crr_id = dadosAluno.crr_id;
                VS_crp_id = dadosAluno.crp_id;

                VS_ConfiguracaoServicoPendencia = new List<ACA_ConfiguracaoServicoPendencia>();

                foreach (var dado in dadosAluno.listaNotasEFaltas.GroupBy(d => new { tne_id = d.tne_id, tme_id = d.tme_id, tur_tipo = d.tur_tipo }))
                    VS_ConfiguracaoServicoPendencia.AddRange(ACA_ConfiguracaoServicoPendenciaBO.SelectTodasBy_tne_id_tme_id_tur_tipo(dado.Key.tne_id, dado.Key.tme_id, dado.Key.tur_tipo));
                
                // Seta o valor do parecer conclusivo do aluno.
                if (dadosAluno.listaNotasEFaltas.Count > 0)
                {
                    divFrequenciaGlobal.Visible = true;
                    divParecerConclusivo.Visible = true;
                    lblMensagemSemDados.Visible = false;

                    if (TpcIdFechamento > 0)
                    {
                        VS_tpc_id = TpcIdFechamento;
                    }
                    else
                    {
                        DadosFechamento dadosBimestreAtivo = dadosAluno.listaNotasEFaltas.FindAll(p => p.mtu_id == mtu_id && p.bimestreComLancamento).OrderByDescending(p => p.tpc_ordem).FirstOrDefault();

                        if (dadosBimestreAtivo.tpc_id > 0)
                        {
                            VS_tpc_id = dadosBimestreAtivo.tpc_id;
                        }
                    }

                    VS_PendenciaPorTud = (from DadosFechamento dado in dadosAluno.listaNotasEFaltas.Where(p => p.mtu_id == mtu_id)
                                          group dado by dado.tud_id into grupo
                                          select new
                                          {
                                              chave = grupo.Key
                                              ,                     //Verifica se o bimestre tem lançamento ou se não aparece no fechamento
                                              valor = grupo.ToList().TrueForAll(p => p.bimestreComLancamento || !p.ApareceFechamento)
                                          }).ToDictionary(p => p.chave, p => p.valor);

                    DadosFechamento dadosNotaEFaltasAluno = dadosAluno.listaNotasEFaltas.Where(p => p.mtu_id == VS_mtu_id).FirstOrDefault();

                    decimal variacao = dadosAluno.listaNotasEFaltas.FirstOrDefault().fav_variacao;
                    VS_FormatacaoPorcentagemFrequencia = GestaoEscolarUtilBO.CriaFormatacaoDecimal(variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(variacao) : 2);

                    decimal FrequenciaFinalAjustadaRegencia = dadosAluno.listaNotasEFaltas.LastOrDefault
                        (p => ((p.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || p.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                // Só considera a regência, quando a turma tem tipo de apuração de frequência por Horas (pois EJA é regência mas é por tempos).
                                && EntFormatoAvaliacao.fav_tipoApuracaoFrequencia != (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula
                                && (p.FrequenciaFinalAjustada > 0 && p.bimestreComLancamento))).FrequenciaFinalAjustada;

                    if (FrequenciaFinalAjustadaRegencia > 0)
                    {
                        dadosNotaEFaltasAluno.FrequenciaGlobal = FrequenciaFinalAjustadaRegencia.ToString(VS_FormatacaoPorcentagemFrequencia);
                    }

                    lblFrequenciaGlobal.Text = dadosNotaEFaltasAluno.FrequenciaGlobal != null ?
                        string.Format("{0}%", dadosNotaEFaltasAluno.FrequenciaGlobal) : "-";
                    AdicionaItemsResultado(ddlResultado, 0, VS_cur_id, VS_crr_id, VS_crp_id, true);
                    ddlResultado.SelectedValue = dadosNotaEFaltasAluno.mtu_resultado > 0 ? dadosNotaEFaltasAluno.mtu_resultado.ToString() : "-1";

                    CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = VS_ListaDadosPeriodo.FirstOrDefault(p => p.tpc_id == VS_tpc_idUltimoPeriodo);
                    if (!periodo.inativoBimestre && PermiteEditar(VS_tpc_idUltimoPeriodo) && (TpcIdFechamento == -1 || TpcIdFechamento == VS_tpc_idUltimoPeriodo))
                    {
                        ddlResultado.Enabled = true;
                        if (ddlResultado.SelectedValue == "-1" && !VS_ConfiguracaoServicoPendencia.Any(p => (p.tur_tipo == 1 || p.tur_tipo <= 0) && p.csp_semParecer))
                        {
                            divParecerConclusivo.Style["background-color"] = ApplicationWEB.CorPendenciaDisciplina;
                        }
                        else
                        {
                            divParecerConclusivo.Style["background-color"] = "";
                        }
                    }
                    else
                    {
                        ddlResultado.Enabled = false;
                        divParecerConclusivo.Style["background-color"] = "";
                    }

                    lblParecerConclusivo.Visible = BimestreAtivo(VS_tpc_idUltimoPeriodo);
                    ddlResultado.Visible = BimestreAtivo(VS_tpc_idUltimoPeriodo);


                    if (Convert.ToDecimal(dadosNotaEFaltasAluno.FrequenciaGlobal) < EntFormatoAvaliacao.percentualMinimoFrequencia && dadosNotaEFaltasAluno.FrequenciaGlobal != null)
                    {
                        divFrequenciaGlobal.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                        divFrequenciaGlobal.Style.Add("border-style", "solid");
                        divFrequenciaGlobal.Style.Add("padding", "5px");
                    }
                    else
                    {
                        divFrequenciaGlobal.Style.Remove("border-color");
                        divFrequenciaGlobal.Style.Remove("border-style");
                        divFrequenciaGlobal.Style.Remove("padding");
                    }

                    hfDataUltimaAlteracaoParecerConclusivo.Value = dadosNotaEFaltasAluno.dataAlteracaoParecerConclusivo.ToString();

                    if (!string.IsNullOrEmpty(dadosNotaEFaltasAluno.usuarioParecerConclusivo))
                    {
                        divInseridoPor.Visible = true;
                        lblHistoricoParecer.Text = String.Format(CustomResource.GetGlobalResourceObject(RESOURCE_NAME, "UCAlunoEfetivacaoObservacaoGeral.lblHistoricoParecer.Text").ToString(),
                                                    dadosNotaEFaltasAluno.usuarioParecerConclusivo, dadosNotaEFaltasAluno.dataAlteracaoParecerConclusivo.ToString("dd/MM/yyyy"), dadosNotaEFaltasAluno.dataAlteracaoParecerConclusivo.ToString("HH:mm"));
                    }
                    else
                    {
                        divInseridoPor.Visible = false;
                    }

                    CarregaDadosParecerConclusivo(dadosAluno);
                    CarregarLegenda();
                }
                else
                {
                    divFrequenciaGlobal.Visible = false;
                    divParecerConclusivo.Visible = false;
                    divInseridoPor.Visible = false;
                    divBoletim.Visible = false;
                    lblMensagemSemDados.Visible = true;
                    lblMensagemSemDados.Text = UtilBO.GetErroMessage(RetornaValorResource("lblMensagemSemDados.Text"), UtilBO.TipoMensagem.Informacao);
                }

                lblMensagemFrequenciaExterna.Text = UtilBO.GetErroMessage(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_FREQUENCIA_EXTERNA, __SessionWEB.__UsuarioWEB.Usuario.ent_id), UtilBO.TipoMensagem.Informacao);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Boletim",
                       "$('.tblBoletim tbody tr:even').addClass('linhaImpar');"
                       + "$('.tblBoletim tbody tr:odd').addClass('linhaPar');"
                       + "RemoveNosTextoVazioTabelasIE9();"
                       , true);
            }
        }

        /// <summary>
        /// Carrega os desempenhos e aprendizados do aluno.
        /// </summary>
        private void CarregarDesempenhoAprendizado(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id)
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaDesempenhoBO.SelecionaPorMatriculaTurma(tur_id, alu_id, mtu_id, fav_id, ava_id);
            List<ACA_TipoDesempenhoAprendizado> lst = new List<ACA_TipoDesempenhoAprendizado>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["tda_id"].ToString()))
                    lst.Add(new ACA_TipoDesempenhoAprendizado { tda_id = Convert.ToInt32(row["tda_id"]), tda_descricao = row["tda_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.tda_descricao).ToList();
            rptDesempenho.DataSource = lst;
            rptDesempenho.DataBind();

            if (lst.Count == 0)
            {
                divDesempenho.Visible = false;
            }
        }

        /// <summary>
        /// Carrega as recomendações ao aluno.
        /// </summary>
        private void CarregarRecomendacaoAluno(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id)
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaRecomendacaoBO.SelecionaPorMatriculaTurmaTipo(tur_id, alu_id, mtu_id, fav_id, ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.Aluno);

            List<ACA_RecomendacaoAlunoResponsavel> lst = new List<ACA_RecomendacaoAlunoResponsavel>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["rar_id"].ToString()))
                    lst.Add(new ACA_RecomendacaoAlunoResponsavel { rar_id = Convert.ToInt32(row["rar_id"]), rar_descricao = row["rar_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.rar_descricao).ToList();
            rptRecomendacao.DataSource = lst;
            rptRecomendacao.DataBind();

            if (lst.Count == 0)
            {
                divListaRecomendacaoAluno.Visible = false;
            }
        }

        /// <summary>
        /// Carrega as recomendações aos pais-reponsáveis do aluno.
        /// </summary>
        private void CarregarRecomendacaoResponsavel(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id)
        {
            DataTable dt = CLS_AlunoAvaliacaoTurmaRecomendacaoBO.SelecionaPorMatriculaTurmaTipo(tur_id, alu_id, mtu_id, fav_id, ava_id, (byte)ACA_RecomendacaoAlunoResponsavelTipo.PaisResponsavel);

            List<ACA_RecomendacaoAlunoResponsavel> lst = new List<ACA_RecomendacaoAlunoResponsavel>();
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["rar_id"].ToString()))
                    lst.Add(new ACA_RecomendacaoAlunoResponsavel { rar_id = Convert.ToInt32(row["rar_id"]), rar_descricao = row["rar_descricao"].ToString() });
            }

            lst = lst.OrderBy(p => p.rar_descricao).ToList();
            rptRecomendacaoResp.DataSource = lst;
            rptRecomendacaoResp.DataBind();

            if (lst.Count == 0)
            {
                divListaRecomendacaoResp.Visible = false;
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
        private void SetaCamposAvaliacao(EscalaAvaliacaoTipo tipo, TextBox txtNota, string aat_avaliacao, DropDownList ddlPareceres, Label lblNotaPosConselho, int tpc_id, bool ultimoBimestre, bool esconderPendencia, bool permiteFinal, bool fechamentoFinal)
        {
            bool permiteEditar = PermiteEditar(tpc_id, fechamentoFinal) && TipoFechamento <= 0 && permiteFinal;
            bool bimestreAtivo = BimestreAtivo(tpc_id);
            string avaliacao = aat_avaliacao;

            if (txtNota != null)
            {
                if (tipo == EscalaAvaliacaoTipo.Numerica && ultimoBimestre)
                {
                    if (aat_avaliacao != "-")
                    {
                        avaliacao = NotaFormatada(aat_avaliacao);
                        txtNota.Text = avaliacao;
                    }
                }

                txtNota.Enabled = ultimoBimestre && !esconderPendencia && permiteEditar;
                txtNota.Visible = (tipo == EscalaAvaliacaoTipo.Numerica) && bimestreAtivo && permiteEditar;
            }

            if (ddlPareceres != null)
            {
                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    if (ddlPareceres.Items.Count == 0)
                    {
                        CarregarPareceres(ddlPareceres);
                    }

                    // Encontra parecer
                    ListItem parecer = ddlPareceres.Items.FindByText(aat_avaliacao);
                    if (parecer != null && ultimoBimestre)
                    {
                        ddlPareceres.SelectedValue = parecer.Value;
                    }
                    else
                    {
                        ddlPareceres.SelectedIndex = 0;
                    }
                }

                ddlPareceres.Enabled = ultimoBimestre && !esconderPendencia && permiteEditar;
                ddlPareceres.Visible = (tipo == EscalaAvaliacaoTipo.Pareceres) && bimestreAtivo && permiteEditar;
            }

            if (lblNotaPosConselho != null)
            {
                if (permiteEditar || !bimestreAtivo)
                {
                    lblNotaPosConselho.Visible = false;
                }
                else
                {
                    lblNotaPosConselho.Visible = true;
                    lblNotaPosConselho.Text = avaliacao;
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
            if (!string.IsNullOrEmpty(nota) && nota != "-")
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
        /// Adiciona os itens de resultado no dropDownList.
        /// </summary>
        private void AdicionaItemsResultado(DropDownList ddl, long tud_id, int cur_id, int crr_id, int crp_id, bool mostrarMensagemSelecione)
        {
            ddl.Items.Clear();

            ListItem item;
            if (mostrarMensagemSelecione)
            {
                // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
                item = tud_id > 0 ?
                new ListItem("-- Selecione um " + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.ParecerFinal") + " --", "-1") :
                new ListItem("-- Selecione um " + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") + " --", "-1");
                ddl.Items.Add(item);
            }
            else
            {
                // Adiciona os itens da tabela MTR_MatriculaTurmaDisciplina.
                item = new ListItem("-", "-1");
                ddl.Items.Add(item);

            }

            // Verifica se existe resultados para esse curso/curriculo/periodo
            List<Struct_TipoResultado> listaTiposResultados = tud_id > 0 ?
                ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(cur_id, crr_id, crp_id, EnumTipoLancamento.Disciplinas) :
                ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(cur_id, crr_id, crp_id, EnumTipoLancamento.ConceitoGlobal);

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
        /// Valida a nota máxima.
        /// </summary>
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

        /// <summary>
        /// Valida o parecer máximo.
        /// </summary>
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
        /// Retorna se é tipo da disciplina de componente de regência do enumerador.
        /// </summary>
        private bool tipoComponenteRegencia(byte tud_tipo)
        {
            return ((byte)TurmaDisciplinaTipo.ComponenteRegencia == tud_tipo || (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia == tud_tipo);
        }

        /// <summary>
        /// Carrega repeaters do boletim do aluno.
        /// </summary>
        /// <param name="listaNotasEFaltas">Lista com os dados do boletim.</param>
        private void CarregaDadosParecerConclusivo(ACA_AlunoBO.BoletimDadosAlunoFechamento dadosBoletimAluno)
        {

            List<DadosFechamento> ParecerConclusivoDados = dadosBoletimAluno.listaNotasEFaltas;

            int cur_id = dadosBoletimAluno.cur_id;
            int crr_id = dadosBoletimAluno.crr_id;
            int crp_id = dadosBoletimAluno.crp_id;

            mostraConceitoGlobal = ParecerConclusivoDados.Count(p => p.tud_global && p.mtu_id > 0) > 0;

            // Seta nota ou conceito com base no tipo da escala de avaliacao
            nomeNota = ParecerConclusivoDados.Any(p => p.esa_tipo == 1) ? "Nota" : "Conceito";

            divBoletim.Visible = true;

            ACA_Curso cur = new ACA_Curso { cur_id = cur_id };
            ACA_CursoBO.GetEntity(cur);
            //Verifica se o nível de ensino do curso é do ensino infantil.
            bool ensinoInfantil = cur.tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (ExibeCompensacaoAusencia)
            {
                var thTotalComp = new HtmlTableCell();
                thTotalComp = (HtmlTableCell)divBoletim.FindControl("thTotalComp");
                thTotalComp.Visible = true;

                var thFreqFinal = new HtmlTableCell();
                thFreqFinal = (HtmlTableCell)divBoletim.FindControl("thFreqFinal");
                thFreqFinal.Visible = true;

                var thFreqFinalEnriquecimento = new HtmlTableCell();
                thFreqFinalEnriquecimento = (HtmlTableCell)divBoletim.FindControl("thFreqFinalEnriquecimento");
                thFreqFinalEnriquecimento.Visible = true;

                var thFreqFinalEI = new HtmlTableCell();
                thFreqFinalEI = (HtmlTableCell)divBoletim.FindControl("thFreqFinalEI");
                thFreqFinalEI.Visible = true;

                var thParecerFinal = new HtmlTableCell();
                thParecerFinal = (HtmlTableCell)divBoletim.FindControl("thParecerFinal");
                thParecerFinal.Visible = true;

                var thParecerFinalEI = new HtmlTableCell();
                thParecerFinalEI = (HtmlTableCell)divBoletim.FindControl("thParecerFinalEI");
                thParecerFinalEI.Visible = true;

                var thFreqFinalRecuperacao = new HtmlTableCell();
                thFreqFinalRecuperacao = (HtmlTableCell)divBoletim.FindControl("thFreqFinalRecuperacao");
                thFreqFinalRecuperacao.Visible = true;

                var thFreqFinalAEE = new HtmlTableCell();
                thFreqFinalAEE = (HtmlTableCell)divBoletim.FindControl("thFreqFinalAEE");
                thFreqFinalAEE.Visible = true;
            }

            #region Periodos / COCs / Bimestres

            var periodos = from CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao item in VS_ListaDadosPeriodo
                           orderby item.tpc_ordem
                           group item by item.tpc_id
                               into g
                           select
                               new
                               {
                                   tpc_id = g.Key
                                   ,
                                   tpc_nome = g.First().cap_descricao
                                   ,
                                   g.First().tpc_ordem
                               };

            rptPeriodosNomes.DataSource = periodos;
            rptPeriodosColunasFixas.DataSource = periodos;
            rptPeriodosNomesEnriquecimento.DataSource = periodos;
            rptPeriodosNomesEI.DataSource = periodos;
            rptPeriodosColunasFixasEnriquecimento.DataSource = periodos;
            rptPeriodosColunasFixasEI.DataSource = periodos;
            rptPeriodosNomesRecuperacao.DataSource = periodos;
            rptPeriodosColunasFixasRecuperacao.DataSource = periodos;
            rptPeriodosNomesAEE.DataSource = periodos;
            rptPeriodosColunasFixasAEE.DataSource = periodos;
            rptPeriodosNomes.DataBind();
            rptPeriodosColunasFixas.DataBind();
            rptPeriodosNomesEnriquecimento.DataBind();
            rptPeriodosNomesEI.DataBind();
            rptPeriodosColunasFixasEnriquecimento.DataBind();
            rptPeriodosColunasFixasEI.DataBind();
            rptPeriodosNomesRecuperacao.DataBind();
            rptPeriodosColunasFixasRecuperacao.DataBind();
            rptPeriodosNomesAEE.DataBind();
            rptPeriodosColunasFixasAEE.DataBind();

            #endregion Periodos / COCs / Bimestres

            #region Disciplinas

            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            int tpc_ordem = ParecerConclusivoDados.FirstOrDefault(p => p.tpc_id == VS_tpc_id).tpc_ordem;
            int tpc_ordemBimestreAtual = VS_ListaDadosPeriodo.LastOrDefault(p => (p.bimestreAtivo && p.mtu_id == VS_mtu_id) || (p.bimestreAtual && p.bimestreAtivo && p.mtu_id == VS_mtu_id)).tpc_ordem;

            bool ultimoBimestre = ParecerConclusivoDados.OrderByDescending(i => i.tpc_ordem).FirstOrDefault().tpc_id.Equals(VS_tpc_id);

            var todasDisciplinas = (from DadosFechamento item in ParecerConclusivoDados
                                    where item.tur_id > 0 && item.tud_tipo != (byte)TurmaDisciplinaTipo.Experiencia
                                    orderby item.tud_tipo, item.tud_global descending, item.Disciplina
                                    group item by item.Disciplina
                                        into g
                                    select
                                        new
                                        {
                                            tud_id = g.First().tud_id
                                            ,
                                            Disciplina = g.First().nomeDisciplina
                                            ,
                                            tds_ordem = g.First().tds_ordem
                                            ,
                                            totalFaltas = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" :
                                                            g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" : (g.Sum(p =>
                                                                (p.mostraFrequencia && !p.naoExibirFrequencia &&
                                                                (p.NotaID > 0 || tipoComponenteRegencia(p.tud_tipo))
                                                                && p.tpc_ordem <= tpc_ordem)
                                                                ? p.numeroFaltas : 0)).ToString()
                                            ,
                                            ausenciasCompensadas = (!BimestreAtivo(VS_tpc_id)) ? "-" :
                                                                        g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                                         (g.Sum(p => p.tpc_ordem <= tpc_ordem ? p.ausenciasCompensadas : 0)).ToString()
                                            ,
                                            FrequenciaFinalAjustada = g.Any(p => p.naoExibirFrequencia) ? "-" : (g.Any(p => p.numeroAulas > 0 && p.bimestreComLancamento && p.tpc_ordem <= tpc_ordemBimestreAtual) ? g.LastOrDefault(p => p.numeroAulas > 0 && p.bimestreComLancamento && p.tpc_ordem <= tpc_ordemBimestreAtual).FrequenciaFinalAjustada.ToString(VS_FormatacaoPorcentagemFrequencia) : (g.Any(p => p.numeroAulas > 0 && p.tpc_ordem <= tpc_ordemBimestreAtual) ? g.LastOrDefault(p => p.numeroAulas > 0 && p.tpc_ordem <= tpc_ordemBimestreAtual).FrequenciaFinalAjustada : 100).ToString(VS_FormatacaoPorcentagemFrequencia))
                                            ,
                                            tud_Tipo = g.First().tud_tipo
                                            ,
                                            g.First().tud_global
                                            ,
                                            mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                                            ,
                                            MediaFinal = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" : (g.Any(p => p.naoExibirNota) || !ultimoBimestre) ? "-" : (!string.IsNullOrEmpty(g.First().NotaResultado) ? g.First().NotaResultado : "-")
                                            ,
                                            UltimoBimestre = VS_PendenciaPorTud.ContainsKey(g.First().tud_id) ? VS_PendenciaPorTud[g.First().tud_id] : ultimoBimestre
                                            ,
                                            tud_idResultado = g.Any(p => p.tud_idResultado > 0) ? g.First(p => p.tud_idResultado > 0).tud_idResultado : g.First().tud_idResultado
                                            ,
                                            mtu_idResultado = g.Any(p => p.mtu_idResultado > 0) ? g.First(p => p.mtu_idResultado > 0).mtu_idResultado : g.First().mtu_idResultado
                                            ,
                                            mtd_idResultado = g.Any(p => p.mtd_idResultado > 0) ? g.First(p => p.mtd_idResultado > 0).mtd_idResultado : g.First().mtd_idResultado
                                            ,
                                            atd_idResultado = g.Any(p => p.NotaIdResultado > 0) ? g.First(p => p.NotaIdResultado > 0).NotaIdResultado : g.First().NotaIdResultado
                                            ,
                                            fav_idResultado = g.Any(p => p.fav_idResultado > 0) ? g.First(p => p.fav_idResultado > 0).fav_idResultado : g.First().fav_idResultado
                                            ,
                                            ava_idResultado = g.Any(p => p.ava_idResultado > 0) ? g.First(p => p.ava_idResultado > 0).ava_idResultado : g.First().ava_idResultado
                                            ,
                                            regencia = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                                         || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                         || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                         && controleOrdemDisciplinas) ? 1 : 2
                                            ,
                                            enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                            ,
                                            parecerFinal = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" : g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" : g.Last().ParecerFinal
                                            ,
                                            recuperacao = g.First().Recuperacao
                                            ,
                                            esconderPendenciaFinal = ParecerConclusivoDados.Any(b => b.Disciplina == g.Key && b.tpc_id == g.Last().tpc_id && b.esconderPendencia)
                                            ,
                                            lancaParecerFinal = ParecerConclusivoDados.Any(b => b.Disciplina == g.Key && b.tpc_id == g.Last().tpc_id && b.lancaParecerFinal)
                                            ,
                                            notas = (
                                                        from per in periodos.ToList()
                                                        orderby per.tpc_ordem
                                                        select new
                                                        {
                                                            per.tpc_id
                                                            ,
                                                            nota = (
                                                                       from DadosFechamento bNota in ParecerConclusivoDados
                                                                       where
                                                                           bNota.Disciplina == g.Key
                                                                           && bNota.tpc_id == per.tpc_id
                                                                       select new
                                                                       {
                                                                           Nota = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                                             bNota.dda_id > 0 ? "-"
                                                                                             :
                                                                                             !bNota.mostraNota || bNota.naoExibirNota
                                                                                                 ? "-"
                                                                                                 : (bNota.NotaNumerica
                                                                                                        ? bNota.avaliacaoSemPosConselho ??
                                                                                                          "-"
                                                                                                        : (bNota.
                                                                                                               NotaAdicionalNumerica
                                                                                                               ? bNota.
                                                                                                                     avaliacaoAdicional ??
                                                                                                                 "-"
                                                                                                               : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                     ? bNota.avaliacaoSemPosConselho ?? "-"
                                                                                                                     : "-")
                                                                                                   )
                                                                                         ).Replace(".", ",")
                                                                                 ,
                                                                           NotaPosConselho = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                                             bNota.dda_id > 0 ? "-"
                                                                                             :
                                                                                             !bNota.mostraNota || bNota.naoExibirNota
                                                                                                 ? "-"
                                                                                                 : (bNota.NotaNumerica
                                                                                                        ? bNota.avaliacaoPosConselho ??
                                                                                                          "-"
                                                                                                        : (bNota.
                                                                                                               NotaAdicionalNumerica
                                                                                                               ? bNota.
                                                                                                                     avaliacaoAdicional ??
                                                                                                                 "-"
                                                                                                               : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                     ? bNota.avaliacaoPosConselho ?? "-"
                                                                                                                     : "-")
                                                                                                   )
                                                                                         ).Replace(".", ",")
                                                                                 ,
                                                                           Conceito =
                                                                                   (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                                   bNota.dda_id > 0 ? "-"
                                                                                   :
                                                                                   bNota.mostraConceito
                                                                                        ? (bNota.NotaNumerica
                                                                                               ? "-"
                                                                                               : bNota.avaliacaoSemPosConselho)
                                                                                        : "-")
                                                                           ,
                                                                           bNota.tpc_id
                                                                           ,
                                                                           bNota.numeroAulas
                                                                           ,
                                                                           bNota.existeAulaBimestre
                                                                           ,
                                                                           numeroFaltas = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :
                                                                                             ((bNota.mostraFrequencia && !bNota.naoExibirFrequencia && (bNota.NotaID > 0 || bNota.numeroFaltas > 0)) ? bNota.numeroFaltas.ToString() : "-")
                                                                           ,
                                                                           tud_Tipo = g.First().tud_tipo
                                                                           ,
                                                                           bNota.tud_id
                                                                           ,
                                                                           bNota.mtu_id
                                                                           ,
                                                                           bNota.mtd_id
                                                                           ,
                                                                           atd_id = bNota.NotaID
                                                                           ,
                                                                           bNota.fav_id
                                                                           ,
                                                                           bNota.ava_id
                                                                           ,
                                                                           recuperacao = bNota.Recuperacao
                                                                           ,
                                                                           enriquecimentoCurricular = bNota.EnriquecimentoCurricular
                                                                           ,
                                                                           ensinoInfantil = ensinoInfantil
                                                                           ,
                                                                           SituacaoDisciplina = bNota.SituacaoDisciplina
                                                                           ,
                                                                           esconderPendencia = bNota.esconderPendencia
                                                                           ,
                                                                           bNota.cal_id
                                                                           ,
                                                                           possuiAnotacaoRP = bNota.possuiAnotacaoRP
                                                                           ,
                                                                           existeAulaSemPlano = bNota.existeAulaSemPlano
                                                                       }).FirstOrDefault()
                                                            ,
                                                            tur_tipo = g.First().tur_tipo
                                                            ,
                                                            tne_id = g.First().tne_id
                                                            ,
                                                            tme_id = g.First().tme_id
                                                        })
                                                        ,
                                            cal_id = g.First().cal_id
                                            ,
                                            faltasExternas = (g.ToList().TrueForAll(b => !BimestreAtivo(b.tpc_id)) || g.ToList().TrueForAll(b => NaoVisualizarDados(b.tpc_id))) ? "-" :
                                                                g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" : (g.Sum(p =>
                                                                    (p.mostraFrequencia && !p.naoExibirFrequencia &&
                                                                    (p.NotaID > 0 || tipoComponenteRegencia(p.tud_tipo))
                                                                    && p.tpc_ordem <= tpc_ordem)
                                                                    ? p.faltasExternas : 0)).ToString()
                                            ,
                                            possuiFrequenciaExterna = (g.ToList().TrueForAll(b => !BimestreAtivo(b.tpc_id)) || g.ToList().TrueForAll(b => NaoVisualizarDados(b.tpc_id))) ? false :
                                                                        g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? false :
                                                                        ParecerConclusivoDados.Any(b => b.Disciplina == g.Key && b.possuiFrequenciaExterna)
                                            ,
                                            tur_tipo = g.First().tur_tipo
                                            ,
                                            tne_id = g.First().tne_id
                                            ,
                                            tme_id = g.First().tme_id
                                        }).ToList();

            var experiencias = (from DadosFechamento item in ParecerConclusivoDados
                                where item.tur_id > 0 && item.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia
                                orderby item.tud_tipo, item.tud_global descending, item.Disciplina
                                group item by item.tud_id
                                        into g
                                select
                                    new
                                    {
                                        tud_id = g.Key
                                        ,
                                        Disciplina = g.First().nomeDisciplina
                                        ,
                                        tds_ordem = g.First().tds_ordem
                                        ,
                                        totalFaltas = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" :
                                                        (g.Sum(p =>
                                                            (p.mostraFrequencia && !p.naoExibirFrequencia &&
                                                            (p.NotaID > 0)
                                                            && p.tpc_ordem <= tpc_ordem)
                                                            ? p.numeroFaltas : 0)).ToString()
                                        ,
                                        ausenciasCompensadas = (!BimestreAtivo(VS_tpc_id)) ? "-" :
                                                                    g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                                     (g.Sum(p => p.tpc_ordem <= tpc_ordem ? p.ausenciasCompensadas : 0)).ToString()
                                        ,
                                        FrequenciaFinalAjustada = g.Any(p => p.naoExibirFrequencia) ? "-" : (g.Any(p => p.numeroAulas > 0 && p.bimestreComLancamento && p.tpc_ordem <= tpc_ordemBimestreAtual) ? g.LastOrDefault(p => p.numeroAulas > 0 && p.bimestreComLancamento && p.tpc_ordem <= tpc_ordemBimestreAtual).FrequenciaFinalAjustada.ToString(VS_FormatacaoPorcentagemFrequencia) : (g.Any(p => p.numeroAulas > 0 && p.tpc_ordem <= tpc_ordemBimestreAtual) ? g.LastOrDefault(p => p.numeroAulas > 0 && p.tpc_ordem <= tpc_ordemBimestreAtual).FrequenciaFinalAjustada : 100).ToString(VS_FormatacaoPorcentagemFrequencia))
                                        ,
                                        tud_Tipo = g.First().tud_tipo
                                        ,
                                        g.First().tud_global
                                        ,
                                        mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                                        ,
                                        MediaFinal = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" : (g.Any(p => p.naoExibirNota) || !ultimoBimestre) ? "-" : (!string.IsNullOrEmpty(g.First().NotaResultado) ? g.First().NotaResultado : "-")
                                        ,
                                        UltimoBimestre = VS_PendenciaPorTud.ContainsKey(g.First().tud_id) ? VS_PendenciaPorTud[g.First().tud_id] : ultimoBimestre
                                        ,
                                        tud_idResultado = g.Any(p => p.tud_idResultado > 0) ? g.First(p => p.tud_idResultado > 0).tud_idResultado : g.First().tud_idResultado
                                        ,
                                        mtu_idResultado = g.Any(p => p.mtu_idResultado > 0) ? g.First(p => p.mtu_idResultado > 0).mtu_idResultado : g.First().mtu_idResultado
                                        ,
                                        mtd_idResultado = g.Any(p => p.mtd_idResultado > 0) ? g.First(p => p.mtd_idResultado > 0).mtd_idResultado : g.First().mtd_idResultado
                                        ,
                                        atd_idResultado = g.Any(p => p.NotaIdResultado > 0) ? g.First(p => p.NotaIdResultado > 0).NotaIdResultado : g.First().NotaIdResultado
                                        ,
                                        fav_idResultado = g.Any(p => p.fav_idResultado > 0) ? g.First(p => p.fav_idResultado > 0).fav_idResultado : g.First().fav_idResultado
                                        ,
                                        ava_idResultado = g.Any(p => p.ava_idResultado > 0) ? g.First(p => p.ava_idResultado > 0).ava_idResultado : g.First().ava_idResultado
                                        ,
                                        regencia = 2
                                        ,
                                        enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                        ,
                                        ensinoInfantil = ensinoInfantil
                                        ,
                                        parecerFinal = (!BimestreAtivo(g.Last().tpc_id) || NaoVisualizarDados(g.Last().tpc_id)) ? "-" : !ParecerConclusivoDados.Any(b => b.tud_id == g.Key && b.tpc_id == g.Last().tpc_id && b.lancaParecerFinal) ? "-" : g.Last().ParecerFinal
                                        ,
                                        recuperacao = g.First().Recuperacao
                                        ,
                                        esconderPendenciaFinal = ParecerConclusivoDados.Any(b => b.tud_id == g.Key && b.tpc_id == g.Last().tpc_id && b.esconderPendencia)
                                        ,
                                        lancaParecerFinal = ParecerConclusivoDados.Any(b => b.tud_id == g.Key && b.tpc_id == g.Last().tpc_id && b.lancaParecerFinal)
                                        ,
                                        notas = (
                                                    from per in periodos.ToList()
                                                    orderby per.tpc_ordem
                                                    select new
                                                    {
                                                        per.tpc_id
                                                        ,
                                                        nota = (
                                                                   from DadosFechamento bNota in ParecerConclusivoDados
                                                                   where
                                                                       bNota.tud_id == g.Key
                                                                       && bNota.tpc_id == per.tpc_id
                                                                   select new
                                                                   {
                                                                       Nota = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                                         bNota.dda_id > 0 ? "-"
                                                                                         :
                                                                                         !bNota.mostraNota || bNota.naoExibirNota
                                                                                             ? "-"
                                                                                             : (bNota.NotaNumerica
                                                                                                    ? bNota.avaliacaoSemPosConselho ??
                                                                                                      "-"
                                                                                                    : (bNota.
                                                                                                           NotaAdicionalNumerica
                                                                                                           ? bNota.
                                                                                                                 avaliacaoAdicional ??
                                                                                                             "-"
                                                                                                           : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                 ? bNota.avaliacaoSemPosConselho ?? "-"
                                                                                                                 : "-")
                                                                                               )
                                                                                     ).Replace(".", ",")
                                                                             ,
                                                                       NotaPosConselho = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                                         bNota.dda_id > 0 ? "-"
                                                                                         :
                                                                                         !bNota.mostraNota || bNota.naoExibirNota
                                                                                             ? "-"
                                                                                             : (bNota.NotaNumerica
                                                                                                    ? bNota.avaliacaoPosConselho ??
                                                                                                      "-"
                                                                                                    : (bNota.
                                                                                                           NotaAdicionalNumerica
                                                                                                           ? bNota.
                                                                                                                 avaliacaoAdicional ??
                                                                                                             "-"
                                                                                                           : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                 ? bNota.avaliacaoPosConselho ?? "-"
                                                                                                                 : "-")
                                                                                               )
                                                                                     ).Replace(".", ",")
                                                                             ,
                                                                       Conceito =
                                                                               (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : (
                                                                               bNota.dda_id > 0 ? "-"
                                                                               :
                                                                               bNota.mostraConceito
                                                                                    ? (bNota.NotaNumerica
                                                                                           ? "-"
                                                                                           : bNota.avaliacaoSemPosConselho)
                                                                                    : "-")
                                                                       ,
                                                                       bNota.tpc_id
                                                                       ,
                                                                       bNota.numeroAulas
                                                                       ,
                                                                       bNota.existeAulaBimestre
                                                                       ,
                                                                       numeroFaltas = (!BimestreAtivo(per.tpc_id) || NaoVisualizarDados(per.tpc_id) || (bNota.mtu_id != VS_mtu_id && per.tpc_ordem > tpc_ordem)) ? "-" : bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :
                                                                                         ((bNota.mostraFrequencia && !bNota.naoExibirFrequencia && (bNota.NotaID > 0 || bNota.numeroFaltas > 0)) ? bNota.numeroFaltas.ToString() : "-")
                                                                       ,
                                                                       tud_Tipo = g.First().tud_tipo
                                                                       ,
                                                                       bNota.tud_id
                                                                       ,
                                                                       bNota.mtu_id
                                                                       ,
                                                                       bNota.mtd_id
                                                                       ,
                                                                       atd_id = bNota.NotaID
                                                                       ,
                                                                       bNota.fav_id
                                                                       ,
                                                                       bNota.ava_id
                                                                       ,
                                                                       recuperacao = bNota.Recuperacao
                                                                       ,
                                                                       enriquecimentoCurricular = bNota.EnriquecimentoCurricular
                                                                       ,
                                                                       ensinoInfantil = ensinoInfantil
                                                                       ,
                                                                       SituacaoDisciplina = bNota.SituacaoDisciplina
                                                                       ,
                                                                       esconderPendencia = bNota.esconderPendencia
                                                                       ,
                                                                       bNota.cal_id
                                                                       ,
                                                                       possuiAnotacaoRP = bNota.possuiAnotacaoRP
                                                                       ,
                                                                       existeAulaSemPlano = bNota.existeAulaSemPlano
                                                                   }).FirstOrDefault()
                                                        ,
                                                        tur_tipo = g.First().tur_tipo
                                                        ,
                                                        tne_id = g.First().tne_id
                                                        ,
                                                        tme_id = g.First().tme_id
                                                    })
                                                    ,
                                        cal_id = g.First().cal_id
                                        ,
                                        faltasExternas = (g.ToList().TrueForAll(b => !BimestreAtivo(b.tpc_id)) || g.ToList().TrueForAll(b => NaoVisualizarDados(b.tpc_id))) ? "-" :
                                                            (g.Sum(p =>
                                                                (p.mostraFrequencia && !p.naoExibirFrequencia &&
                                                                (p.NotaID > 0)
                                                                && p.tpc_ordem <= tpc_ordem)
                                                                ? p.faltasExternas : 0)).ToString()
                                        ,
                                        possuiFrequenciaExterna = (g.ToList().TrueForAll(b => !BimestreAtivo(b.tpc_id)) || g.ToList().TrueForAll(b => NaoVisualizarDados(b.tpc_id))) ? false :
                                                                    ParecerConclusivoDados.Any(b => b.tud_id == g.Key && b.possuiFrequenciaExterna)
                                        ,
                                        tur_tipo = g.First().tur_tipo
                                        ,
                                        tne_id = g.First().tne_id
                                        ,
                                        tme_id = g.First().tme_id
                                    }).ToList();

            List<ACA_Evento> eventos = ACA_EventoBO.GetEntity_Efetivacao_List(ParecerConclusivoDados.OrderByDescending(i => i.tpc_ordem).FirstOrDefault().cal_id,
                                                                              ParecerConclusivoDados.OrderByDescending(i => i.tpc_ordem).FirstOrDefault().tur_id,
                                                                              __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                              ApplicationWEB.AppMinutosCacheLongo, false);
            fechamentoFinalAberto = eventos.Exists(p => p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) && p.vigente);

            var disciplinas = (from item in todasDisciplinas
                               where !item.enriquecimentoCurricular //Retira as que são de enriquecimento curricular
                               && !item.recuperacao //Retira as recuperacoes
                               && item.tud_Tipo != (byte)TurmaDisciplinaTipo.AtendimentoEducacionalEspecializado //Retira AEE
                               select item
                               );

            // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
            var dispOrdenadas = from item in disciplinas
                                orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                select item;

            // "Agrupa" a frequência das disciplinas componentes e complementares à regência. 
            QtComponenteRegencia = dispOrdenadas.Count(p => (tipoComponenteRegencia(p.tud_Tipo)) && p.mostrarDisciplina > 0);

            // "Agrupa" a frequência das disciplinas
            QtComponentes = dispOrdenadas.Count(p => (p.mostrarDisciplina > 0));

            lblMensagemFrequenciaExterna.Visible = false;

            if (!ensinoInfantil)
            {
                divDisciplinas.Visible = true;
                exibirFaltasExternas = false;

                HtmlTableCell thFaltasExternas = (HtmlTableCell)divBoletim.FindControl("thFaltasExternas");
                if (thFaltasExternas != null)
                {
                    exibirFaltasExternas = ExibeCompensacaoAusencia && dispOrdenadas.Any(p => p.mostrarDisciplina > 0 && p.possuiFrequenciaExterna);
                    thFaltasExternas.Visible = exibirFaltasExternas;
                    if (exibirFaltasExternas)
                    {
                        lblMensagemFrequenciaExterna.Visible = true;
                    }
                }

                rptDisciplinas.DataSource = dispOrdenadas.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinas.DataBind();
            }
            else
                divDisciplinas.Visible = false;

            VS_tud_ids = disciplinas.Select(d => d.tud_id).ToArray();

            #endregion Disciplinas

            #region Disciplinas de enriquecimento curricular e experiências

            var disciplinasEnriquecimentoCurricular = (from item in todasDisciplinas
                                                       where item.enriquecimentoCurricular //Verifica se são de enriquecimento curricular
                                                       && !item.recuperacao //Retira as recuperacoes
                                                       && item.tud_Tipo != (byte)TurmaDisciplinaTipo.AtendimentoEducacionalEspecializado //Retira AEE
                                                       select item
                              );

            if (disciplinasEnriquecimentoCurricular.Count() > 0 || experiencias.Count() > 0)
            {
                divEnriquecimentoCurricular.Visible = true;
                exibirFaltasExternas = false;

                // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
                var dispOrdenadasEnriquecimento = from item in disciplinasEnriquecimentoCurricular
                                                  orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                                  select item;
                
                HtmlTableCell thFaltasExternasEnriquecimento = (HtmlTableCell)divBoletim.FindControl("thFaltasExternasEnriquecimento");
                if (thFaltasExternasEnriquecimento != null)
                {
                    exibirFaltasExternas = ExibeCompensacaoAusencia && (dispOrdenadasEnriquecimento.Any(p => p.mostrarDisciplina > 0 && p.possuiFrequenciaExterna) || experiencias.Any(p => p.mostrarDisciplina > 0 && p.possuiFrequenciaExterna));
                    thFaltasExternasEnriquecimento.Visible = exibirFaltasExternas;
                    if (exibirFaltasExternas)
                    {
                        lblMensagemFrequenciaExterna.Visible = true;
                    }
                }

                rptDisciplinasEnriquecimentoCurricular.DataSource = dispOrdenadasEnriquecimento.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasEnriquecimentoCurricular.DataBind();

                rptDisciplinasExperiencias.DataSource = experiencias.Where(p => p.mostrarDisciplina > 0).OrderBy(p => controleOrdemDisciplinas ? p.tds_ordem.ToString() : p.Disciplina);
                rptDisciplinasExperiencias.DataBind();
            }
            else
            {
                divEnriquecimentoCurricular.Visible = false;
            }

            #endregion Disciplinas de enriquecimento curricular

            #region Ensino Infantil

            if (ensinoInfantil)
            {
                divEnsinoInfantil.Visible = true;
                exibirFaltasExternas = false;

                HtmlTableCell thFaltasExternasEI = (HtmlTableCell)divBoletim.FindControl("thFaltasExternasEI");
                if (thFaltasExternasEI != null)
                {
                    exibirFaltasExternas = ExibeCompensacaoAusencia && dispOrdenadas.Any(p => p.mostrarDisciplina > 0 && p.possuiFrequenciaExterna);
                    thFaltasExternasEI.Visible = exibirFaltasExternas;
                    if (exibirFaltasExternas)
                    {
                        lblMensagemFrequenciaExterna.Visible = true;
                    }
                }

                rptDisciplinasEnsinoInfantil.DataSource = dispOrdenadas.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasEnsinoInfantil.DataBind();
            }
            else
                divEnsinoInfantil.Visible = false;

            #endregion

            #region Recuperacao

            var disciplinasRecuperacao = (from item in todasDisciplinas
                                          where item.recuperacao //Seleciona as recuperacoes
                                          select item
                              );

            if (disciplinasRecuperacao.Count() > 0)
            {
                divRecuperacao.Visible = true;
                var dispOrdenadasRecuperacao = from item in disciplinasRecuperacao
                                               orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                               select item;

                rptDisciplinasRecuperacao.DataSource = disciplinasRecuperacao.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasRecuperacao.DataBind();
            }
            else
            {
                divRecuperacao.Visible = false;
            }

            #endregion Recuperacao

            #region AEE

            var disciplinasAEE = (from item in todasDisciplinas
                                  where item.tud_Tipo == (byte)TurmaDisciplinaTipo.AtendimentoEducacionalEspecializado //Seleciona AEE
                                  select item
                              );

            if (disciplinasAEE.Count() > 0)
            {
                divAEE.Visible = true;
                var dispOrdenadasAEE = from item in disciplinasAEE
                                       orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                       select item;

                rptDisciplinasAEE.DataSource = dispOrdenadasAEE.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasAEE.DataBind();
            }
            else
            {
                divAEE.Visible = false;
            }

            #endregion AEE
        }

        /// <summary>
        /// Altera a cor do fundo de acordo com a entrada e saída do aluno da rede.
        /// </summary>
        /// <param name="tpc_id">Período do calendário.</param>
        /// <param name="td">Célula.</param>
        /// <param name="validarNota">Indica se deve validar a hora.</param>
        /// <param name="nota">Nota.</param>
        /// <param name="validarFreq">Indica se deve validar frequência</param>
        /// <param name="existeAulaBimestre">Indica se existe aula no bimestre.</param>
        private void AlterarCorFundo(int tpc_id, HtmlTableCell td, bool validarNota, string nota, bool validarFreq, bool existeAulaBimestre, bool validarAnotacaoRP, bool possuiAnotacaoRP, bool validarAulaSemPlano, bool aulaSemPlano
            , eSituacaoMatriculaTurmaDisicplina SituacaoDisciplina, bool esconderPendencia, long tud_id = -1, int cal_id = -1, bool configServPendencia = false)
        {
            if (td != null)
            {
                List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);

                // Se for uma turma disciplina com justificativa para a pendência no fechamento
                // não mostro como pendente.
                if (tud_id > 0)
                {
                    CLS_FechamentoJustificativaPendencia justificativaPendencia = CLS_FechamentoJustificativaPendenciaBO.GetSelectBy_TurmaDisciplinaPeriodo(tud_id, cal_id, tpc_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (justificativaPendencia.fjp_id > 0)
                    {
                        esconderPendencia = true;
                    }
                }

                // Verifica o campo SituacaoDisicplina, que verifica em cada bimestre e disciplina a situação específica (exemplo: Recuperação e Multisseriadas que possuem 
                // diferentes datas de matrículas).
                switch (SituacaoDisciplina)
                {
                    case eSituacaoMatriculaTurmaDisicplina.Dispensado:
                        {
                            if (!esconderPendencia && !configServPendencia)
                                td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                            break;
                        }
                    case eSituacaoMatriculaTurmaDisicplina.ForaRede:
                        {
                            td.Style.Add("background-color", ApplicationWEB.CorAlunoForaDaRede);
                            break;
                        }
                    default:
                        {
                            if (listaPeriodo.Count > 0)
                            {
                                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                                if (periodo.foraRede)
                                {
                                    td.Style.Add("background-color", ApplicationWEB.CorAlunoForaDaRede);
                                }
                                else if (periodo.inativoBimestre)
                                {
                                    td.Style.Add("background-color", ApplicationWEB.AlunoInativo);
                                }
                                else if (periodo.bimestreAtivo && periodo.esc_id == VS_esc_id && (periodo.periodoPassado || (periodo.bimestreAtual && periodo.eventoAberto)))
                                {
                                    if (validarNota)
                                    {
                                        if (string.IsNullOrEmpty(nota) || nota == "-")
                                        {
                                            if (!esconderPendencia && !configServPendencia)
                                                td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                        }
                                    }

                                    if (validarFreq)
                                    {
                                        if (!existeAulaBimestre)
                                        {
                                            if (!esconderPendencia && !configServPendencia)
                                                td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                        }
                                    }

                                    if (validarAnotacaoRP)
                                    {
                                        if (!possuiAnotacaoRP)
                                        {
                                            if (!esconderPendencia && !configServPendencia)
                                                td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                        }
                                    }

                                    if (validarAulaSemPlano)
                                    {
                                        if (aulaSemPlano)
                                        {
                                            if (!esconderPendencia && !configServPendencia)
                                                td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                        }
                                    }
                                }
                            }
                            else if (fechamentoFinalAberto)
                            {
                                if (validarNota)
                                {
                                    if (string.IsNullOrEmpty(nota) || nota == "-")
                                    {
                                        if (!esconderPendencia && !configServPendencia)
                                            td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                    }
                                }

                                if (validarFreq)
                                {
                                    if (!existeAulaBimestre)
                                    {
                                        if (!esconderPendencia && !configServPendencia)
                                            td.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Indica se permite editar os dados do bimestre.
        /// </summary>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        private bool PermiteEditar(int tpc_id, bool fechamentoFinal = true)
        {
            int tev_EfetivacaoFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);
            if (listaPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                return (!periodo.foraRede && !periodo.inativoBimestre && periodo.esc_id == VS_esc_id &&
                        ((periodo.bimestreAtivo && periodo.eventoAberto) || ((tpc_id == VS_tpc_idUltimoPeriodo && ltEvento.Exists(p => p.tev_id == tev_EfetivacaoFinal)) && fechamentoFinal)));
            }

            return false;
        }

        /// <summary>
        /// Indica se é o bimestre atual.
        /// </summary>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        private bool BimestreAtual(int tpc_id)
        {
            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);
            if (listaPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                return (periodo.bimestreAtual);
            }

            return false;
        }

        /// <summary>
        /// Indica se é o não é para exibir os da.
        /// </summary>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        private bool NaoVisualizarDados(int tpc_id)
        {
            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);
            if (listaPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                return (periodo.naoVisualizarDados) || !VS_ListaDadosPeriodo.Any(p => p.mtu_id == VS_mtu_id);
            }

            return false;
        }

        /// <summary>
        /// Indica se o bimestre está ativo na turma.
        /// </summary>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns></returns>
        private bool BimestreAtivo(int tpc_id)
        {
            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);
            if (listaPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                return (periodo.bimestreAtivo) || VS_ListaDadosPeriodo.Any(p => p.bimestreAtivo && p.tpc_ordem > periodo.tpc_ordem);
            }

            return false;
        }

        /// <summary>
        /// Indica se permite editar os dados do bimestre.
        /// </summary>
        /// <param name="tpc_id"></param>
        private bool CalendarioFinalizado(int tpc_id)
        {
            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaPeriodo = VS_ListaDadosPeriodo.FindAll(p => p.tpc_id == tpc_id);
            if (listaPeriodo.Count > 0)
            {
                CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao periodo = listaPeriodo[0];
                return (periodo.calendarioFinalizado);
            }

            return false;
        }

        /// <summary>
        /// Faz a validação dos dados na tela e gera as listas necessárias para salvar.
        /// </summary>
        /// <param name="listaMatriculaTurmaDisciplina">Lista de disciplinas do aluno para usar para salvar</param>
        private void ValidaGeraDados(out List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina, out List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina, out List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAlunoAvaliacaoTurmaDisciplinaPosConselho)
        {
            listaDisciplina = new List<CLS_AvaliacaoTurDisc_Cadastro>();
            listaMatriculaTurmaDisciplina = new List<MTR_MatriculaTurmaDisciplina>();
            listaAlunoAvaliacaoTurmaDisciplinaPosConselho = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();

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

            foreach (RepeaterItem rptItem in rptDisciplinas.Items)
            {
                // Nota por bimestre
                Repeater rptNotasDisciplina = (Repeater)rptItem.FindControl("rptNotasDisciplina");
                if (rptNotasDisciplina != null)
                {
                    foreach (RepeaterItem rptNotas in rptNotasDisciplina.Items)
                    {
                        if (rptNotas.FindControl("tdNotaPosConselho").Visible)
                        {
                            HiddenField hfTpcId = (HiddenField)rptNotas.FindControl("hfTpcId");
                            if (hfTpcId != null && !string.IsNullOrEmpty(hfTpcId.Value))
                            {
                                int tpc_id = Convert.ToInt32(hfTpcId.Value);
                                if (PermiteEditar(tpc_id) && TipoFechamento <= 0)
                                {
                                    if (tipoEscalaNumerica)
                                    {
                                        TextBox txtNota = (TextBox)rptNotas.FindControl("txtNotaFinal");
                                        if (txtNota != null && txtNota.Visible)
                                        {
                                            // Recupera o valor da avaliação normal.
                                            if (!string.IsNullOrEmpty(txtNota.Text))
                                            {
                                                decimal nota;
                                                if (decimal.TryParse(txtNota.Text, out nota))
                                                {
                                                    if ((nota < escalaNumerica.ean_menorValor) || (nota > escalaNumerica.ean_maiorValor))
                                                    {
                                                        alunosErroIntervalo.Add(((Literal)rptItem.FindControl("litNomeDisciplina")).Text);
                                                    }
                                                }
                                                else
                                                {
                                                    alunosErroConversao.Add(((Literal)rptItem.FindControl("litNomeDisciplina")).Text);
                                                }
                                            }
                                        }
                                    }

                                    AdicionaLinhaAvaliacaoPosConselho(rptNotas, ref listaAlunoAvaliacaoTurmaDisciplinaPosConselho);
                                }
                            }
                        }
                    }
                }

                // Parecer final, caso o último período esteja liberado.
                if (PermiteEditar(VS_tpc_idUltimoPeriodo) && TipoFechamento <= 0)
                {
                    // Nota final
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
                                            alunosErroIntervalo.Add(((Literal)rptItem.FindControl("litNomeDisciplina")).Text);
                                        }
                                    }
                                    else
                                    {
                                        alunosErroConversao.Add(((Literal)rptItem.FindControl("litNomeDisciplina")).Text);
                                    }
                                }
                            }
                        }

                        AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                    }
                }
            }

            if (PermiteEditar(VS_tpc_idUltimoPeriodo) && TipoFechamento <= 0)
            {
                foreach (RepeaterItem rptItem in rptDisciplinasEnriquecimentoCurricular.Items)
                {
                    if (rptItem.FindControl("tdParecerFinal").Visible)
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

                            AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                        }
                    }
                }

                foreach (RepeaterItem rptItem in rptDisciplinasExperiencias.Items)
                {
                    if (rptItem.FindControl("tdParecerFinal").Visible)
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

                            AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                        }
                    }
                }

                foreach (RepeaterItem rptItem in rptDisciplinasEnsinoInfantil.Items)
                {
                    if (rptItem.FindControl("tdParecerFinal").Visible)
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

                            AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                        }
                    }
                }


                foreach (RepeaterItem rptItem in rptDisciplinasRecuperacao.Items)
                {
                    if (rptItem.FindControl("tdParecerFinal").Visible)
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

                            AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                        }
                    }
                }

                foreach (RepeaterItem rptItem in rptDisciplinasAEE.Items)
                {
                    if (rptItem.FindControl("tdParecerFinal").Visible)
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

                            AdicionaLinhaDisciplina(rptItem, ref listaDisciplina);
                        }
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
            DropDownList ddlParecerFinal = (DropDownList)rptItem.FindControl("ddlParecerFinal");
            if ((txtNotaFinal != null && txtNotaFinal.Visible) ||
                (ddlParecerFinal != null && ddlParecerFinal.Visible))
            {
                long tud_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfTudId")).Value);
                if (tud_id <= 0)
                    return;
                long alu_id = VS_alu_id;
                int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtuId")).Value);
                if (mtu_id <= 0)
                    return;
                int mtd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtdId")).Value);
                if (mtd_id <= 0)
                    return;

                int atd_id = -1;
                if (!String.IsNullOrEmpty(((HiddenField)rptItem.FindControl("hfAtdId")).Value))
                {
                    if (Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAtdId")).Value) > 0)
                    {
                        atd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAtdId")).Value);
                    }
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

                ent.fav_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfFavId")).Value);
                if (ent.fav_id <= 0)
                    return;
                ent.ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAvaId")).Value);
                if (ent.ava_id <= 0)
                    return;
                ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;

                // Setar o registroExterno para false.
                ent.atd_registroexterno = false;

                HtmlControl hcNotaFinal = (HtmlControl)rptItem.FindControl("tdNotaFinal");
                bool salvarRelatorio;
                ent.atd_avaliacao = RetornaAvaliacaoFinal(rptItem, out salvarRelatorio);
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
        /// Adiciona uma linha na lista com os dados da linha do grid.
        /// </summary>
        /// <param name="row">Linha do grid.</param>
        /// <param name="listaDisciplina"></param>
        private void AdicionaLinhaAvaliacaoPosConselho(RepeaterItem rptItem, ref List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAlunoAvaliacaoTurmaDisciplina)
        {
            TextBox txtNotaFinal = (TextBox)rptItem.FindControl("txtNotaFinal");
            DropDownList ddlParecerFinal = (DropDownList)rptItem.FindControl("ddlParecerFinal");
            if (txtNotaFinal.Visible || ddlParecerFinal.Visible)
            {
                long tur_id = VS_tur_id;
                long tud_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfTudId")).Value);
                long alu_id = VS_alu_id;
                int mtu_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtuId")).Value);
                int mtd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfMtdId")).Value);
                int ava_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAvaId")).Value);
                int fav_id = VS_fav_id;
                int tpc_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfTpcId")).Value);
                int atd_id = -1;
                if (!String.IsNullOrEmpty(((HiddenField)rptItem.FindControl("hfAtdId")).Value))
                {
                    if (Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAtdId")).Value) > 0)
                    {
                        atd_id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hfAtdId")).Value);
                    }
                }

                CLS_AlunoAvaliacaoTurmaDisciplina ent = new CLS_AlunoAvaliacaoTurmaDisciplina();
                ent.tud_id = tud_id;
                ent.alu_id = alu_id;
                ent.mtu_id = mtu_id;
                ent.mtd_id = mtd_id;
                ent.atd_id = atd_id;
                ent.fav_id = fav_id;
                ent.ava_id = ava_id;
                ent.atd_situacao = (byte)CLS_AlunoAvaliacaoTurmaDisciplinaSituacao.Ativo;
                ent.tpc_id = tpc_id;
                bool salvarRelatorio;
                ent.atd_avaliacaoPosConselho = RetornaAvaliacaoFinal(rptItem, out salvarRelatorio);

                listaAlunoAvaliacaoTurmaDisciplina.Add(ent);
            }
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacaoFinal(Control row, out bool salvarRelatorio)
        {
            TextBox txtNota = (TextBox)row.FindControl("txtNotaFinal");
            DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlParecerFinal");

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

        /// <summary>
        /// Atribui a permissão aos campos dos bimestres (repeater das abas).
        /// </summary>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <param name="btnTextoGrandeBimestre">Botão para expandir área.</param>
        /// <param name="btnVoltaEstadoAnteriorTextoBimestre">Botão para voltar a área.</param>
        /// <param name="txt">TextBox com os dados.</param>
        /// <param name="div">Div que vai expandir.</param>
        private void AtribuirPermissaoCamposPeriodo(int tpc_id, ImageButton btnTextoGrandeBimestre, ImageButton btnVoltaEstadoAnteriorTextoBimestre, TextBox txt, HtmlGenericControl div, Label lbl, HtmlGenericControl divPeriodoCalendario)
        {
            txt.Enabled = PermiteEditar(tpc_id);
            if (txt.Visible)
            {
                divPeriodoCalendario.Visible = lbl.Visible = txt.Visible = btnTextoGrandeBimestre.Visible = BimestreAtivo(tpc_id);
            }

            btnTextoGrandeBimestre.OnClientClick =
                String.Format("abrirTextoComMensagem('{0}','{1}','{2}'); return false;",
                    div.ClientID,
                    btnTextoGrandeBimestre.ClientID,
                    btnVoltaEstadoAnteriorTextoBimestre.ClientID);

            btnVoltaEstadoAnteriorTextoBimestre.OnClientClick =
                String.Format("fecharTextoComMensagem('{0}','{1}','{2}'); return false;",
                    div.ClientID,
                    btnTextoGrandeBimestre.ClientID,
                    btnVoltaEstadoAnteriorTextoBimestre.ClientID);

            if ((tpc_id == VS_tpc_id) && txt.Enabled)
            {
                txt.CssClass += " bimestreAtual";
            }

            if ((tpc_id == VS_tpc_id) || CalendarioFinalizado(tpc_id))
            {
                btnTextoGrandeBimestre.Style.Add("display", "none");
                btnVoltaEstadoAnteriorTextoBimestre.Style.Add("display", "inside-block");
            }
            else
            {
                div.Style.Add("display", "none");
            }
        }

        /// <summary>
        /// Carrega o grid com as anotações.
        /// </summary>
        public void LoadGridAnotacoes()
        {
            grvAnotacoesGerais.DataSource = VS_ListaAlunoAnotacaoGrid;
            grvAnotacoesGerais.DataBind();
        }

        /// <summary>
        /// Carregar dados legenda.
        /// </summary>
        public void CarregarLegenda()
        {
            divLegenda.Visible = true;

            if (lnAlunoPendencia != null)
            {
                lnAlunoPendencia.Style.Add("background-color", ApplicationWEB.CorPendenciaDisciplina);
            }

            if (lnInativos != null)
            {
                lnInativos.Style.Add("background-color", ApplicationWEB.AlunoInativo);
            }

            if (lnAlunoForaRede != null)
            {
                lnAlunoForaRede.Style.Add("background-color", ApplicationWEB.CorAlunoForaDaRede);
            }

            if (lnAlunoFrequencia != null)
            {
                lnAlunoFrequencia.Style.Add("border-color", ApplicationWEB.CorBordaFrequenciaAbaixoMinimo);
                lnAlunoFrequencia.Style.Add("border-style", "solid");
                lnAlunoFrequencia.Style.Add("border-width", "2px");
            }
        }

        /// <summary>
        /// Retorna valor de um resource.
        /// </summary>
        /// <param name="chave">Chave do resource.</param>
        /// <returns></returns>
        private string RetornaValorResource(string chave)
        {
            return CustomResource.GetGlobalResourceObject(RESOURCE_NAME, String.Format(RESOURCE_KEY, chave)).ToString();
        }

        /// <summary>
        /// Controla a exibição das abas.
        /// </summary>
        private void ControlarExibicaoAbas()
        {
            bool possuiPermissaoVisualizacaoAbaParecerConclusivo = true;
            bool possuiPermissaoVisualizacaoAbaJustificativaPosConselho = true;
            bool possuiPermissaoVisualizacaoAbaDesempenhoAprendizagem = true;
            bool possuiPermissaoVisualizacaoAbaRecomendacaoAluno = true;
            bool possuiPermissaoVisualizacaoAbaRecomendacaoResponsavel = true;
            bool possuiPermissaoVisualizacaoAbaAnotacoesAluno = true;

            List<string> listOperacoes = new List<string> {
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaParecerConclusivo).ToString(),
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaJustificativaPosConselho).ToString(),
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaDesempenhoAprendizagem).ToString(),
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaRecomendacaoAluno).ToString(),
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaRecomendacaoResponsavel).ToString(),
                Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaAnotacoesAluno).ToString()
            };

            List<CFG_PermissaoModuloOperacao> lstPermissoesModuloOperacao = CFG_PermissaoModuloOperacaoBO.VerificaPermissaoModuloOperacao(ApplicationWEB.SistemaID, __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, listOperacoes);

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoAbaParecerConclusivo = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaParecerConclusivo)).FirstOrDefault();

            if (permissaoModuloOperacaoAbaParecerConclusivo != null && !permissaoModuloOperacaoAbaParecerConclusivo.IsNew && (!permissaoModuloOperacaoAbaParecerConclusivo.pmo_permissaoConsulta && !permissaoModuloOperacaoAbaParecerConclusivo.pmo_permissaoInclusao && !permissaoModuloOperacaoAbaParecerConclusivo.pmo_permissaoEdicao))
            {
                possuiPermissaoVisualizacaoAbaParecerConclusivo = false;
            }

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoJustificativaPosConselho = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaJustificativaPosConselho)).FirstOrDefault();

            if (permissaoModuloOperacaoJustificativaPosConselho != null && !permissaoModuloOperacaoJustificativaPosConselho.IsNew && (!permissaoModuloOperacaoJustificativaPosConselho.pmo_permissaoConsulta && !permissaoModuloOperacaoJustificativaPosConselho.pmo_permissaoInclusao && !permissaoModuloOperacaoJustificativaPosConselho.pmo_permissaoEdicao))
            {
                possuiPermissaoVisualizacaoAbaJustificativaPosConselho = false;
            }

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoDesempenhoAprendizagem = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaDesempenhoAprendizagem)).FirstOrDefault();

            if (permissaoModuloOperacaoDesempenhoAprendizagem != null && !permissaoModuloOperacaoDesempenhoAprendizagem.IsNew && (!permissaoModuloOperacaoDesempenhoAprendizagem.pmo_permissaoConsulta && !permissaoModuloOperacaoDesempenhoAprendizagem.pmo_permissaoInclusao && !permissaoModuloOperacaoDesempenhoAprendizagem.pmo_permissaoEdicao))
            {
                possuiPermissaoVisualizacaoAbaDesempenhoAprendizagem = false;
            }

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoRecomendacaoAluno = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaRecomendacaoAluno)).FirstOrDefault();

            if (permissaoModuloOperacaoRecomendacaoAluno != null && !permissaoModuloOperacaoRecomendacaoAluno.IsNew && (!permissaoModuloOperacaoRecomendacaoAluno.pmo_permissaoConsulta && !permissaoModuloOperacaoRecomendacaoAluno.pmo_permissaoInclusao && !permissaoModuloOperacaoRecomendacaoAluno.pmo_permissaoEdicao))
            {
                possuiPermissaoVisualizacaoAbaRecomendacaoAluno = false;
            }

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoRecomendacaoResponsavel = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaRecomendacaoResponsavel)).FirstOrDefault();

            if (permissaoModuloOperacaoRecomendacaoResponsavel != null && !permissaoModuloOperacaoRecomendacaoResponsavel.IsNew && (!permissaoModuloOperacaoRecomendacaoResponsavel.pmo_permissaoConsulta && !permissaoModuloOperacaoRecomendacaoResponsavel.pmo_permissaoInclusao && !permissaoModuloOperacaoRecomendacaoResponsavel.pmo_permissaoEdicao))
            {
                possuiPermissaoVisualizacaoAbaRecomendacaoResponsavel = false;
            }

            CFG_PermissaoModuloOperacao permissaoModuloOperacaoAnotacoesAluno = lstPermissoesModuloOperacao.Where(p => p.pmo_operacao == Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.FechamentoExibicaoAbaAnotacoesAluno)).FirstOrDefault();

            bool permissaoModuloOperacao = permissaoModuloOperacaoAnotacoesAluno == null || permissaoModuloOperacaoAnotacoesAluno.IsNew || ((permissaoModuloOperacaoAnotacoesAluno != null && permissaoModuloOperacaoAnotacoesAluno.pmo_permissaoConsulta) || (permissaoModuloOperacaoAnotacoesAluno != null && permissaoModuloOperacaoAnotacoesAluno.pmo_permissaoInclusao) || (permissaoModuloOperacaoAnotacoesAluno != null && permissaoModuloOperacaoAnotacoesAluno.pmo_permissaoEdicao));
            if (permissaoModuloOperacao && AnotacoesAlunoVisible)
            {
                possuiPermissaoVisualizacaoAbaAnotacoesAluno = true;
            }
            else
            {
                possuiPermissaoVisualizacaoAbaAnotacoesAluno = false;
            }

            liParecerConclusivo.Visible = fdsBoletim.Visible = possuiPermissaoVisualizacaoAbaParecerConclusivo;
            liJustificativaPosConselho.Visible = fdsJustificativaPosConselho.Visible = possuiPermissaoVisualizacaoAbaJustificativaPosConselho;
            liDesempenhoAprendizagem.Visible = fdsDesempenhoAprendizado.Visible = possuiPermissaoVisualizacaoAbaDesempenhoAprendizagem;
            liRecomendacaoAluno.Visible = fdsRecomendacoesAluno.Visible = possuiPermissaoVisualizacaoAbaRecomendacaoAluno;
            liRecomendacaoResponsavel.Visible = fdsRecomendacoesPais.Visible = possuiPermissaoVisualizacaoAbaRecomendacaoResponsavel;
            liAnotacoesAluno.Visible = fdsAnotacoes.Visible = possuiPermissaoVisualizacaoAbaAnotacoesAluno;
        }

        /// <summary>
        /// Verifica integridade do resultado do aluno com o EOL.
        /// </summary>
        /// <param name="CodigoEOLTurma"></param>
        /// <param name="CodigoEOLAluno"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static string VerificarIntegridadeParecerEOL(string CodigoEOLTurma, string CodigoEOLAluno, string resultado, bool chamadaJavaScript)
        {
            string retorno = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SYS_RecursoAPI recurso = new SYS_RecursoAPI { rap_id = (int)eRecursoAPI.ParecerConclusivoEOL };
                    SYS_RecursoAPIBO.GetEntity(recurso);

                    if (recurso.IsNew || string.IsNullOrEmpty(recurso.rap_url) || recurso.rap_situacao == (byte)RecursoAPISituacao.Excluido)
                        return "true";

                    client.BaseAddress = new Uri(recurso.rap_url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    List<SYS_UsuarioAPI> lstUsuario = SYS_RecursoUsuarioAPIBO.SelecionaUsuarioPorRecurso(eRecursoAPI.ParecerConclusivoEOL);

                    if (lstUsuario.Any())
                    {
                        var auth = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", lstUsuario.First().uap_usuario, lstUsuario.First().uap_senha));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(auth));
                    }

                    HttpResponseMessage response = client.GetAsync(string.Format("GetResultadoAluno?CodigoEOLTurma={0}&CodigoEOLAluno={1}", CodigoEOLTurma, CodigoEOLAluno)).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ParecerConclusivo parecerEOL = response.Content.ReadAsAsync<ParecerConclusivo>().Result;
                        retorno = (string.IsNullOrEmpty(parecerEOL.Resultado) || parecerEOL.Resultado.ToString().Equals(resultado, StringComparison.OrdinalIgnoreCase)).ToString();
                    }
                    else
                        throw new ValidationException("Erro ao tentar buscar os dados cadastrados no EOL.");
                }

                return retorno;
            }
            catch (ValidationException ex)
            {
                if (!chamadaJavaScript)
                    throw ex;
                else
                    return "";
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                if (!chamadaJavaScript)
                    throw new ValidationException("Erro ao tentar buscar os dados cadastrados no EOL.");
                else
                    return "";
            }
        }

        #endregion Métodos
    }
}