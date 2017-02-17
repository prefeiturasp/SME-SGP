using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.PlanejamentoAnual
{
    public partial class UCPlanejamentoAnual : MotherUserControl
    {
        #region Estruturas

        /// <summary>
        /// Estrutura que armazena a estrutra de orientações curriculares de cada período do calendário.
        /// </summary>
        [Serializable]
        private struct OrientacaoCurricular
        {
            public int tpc_id;
            public DataTable dtOrientacaoCurricular;
        }

        #endregion Estruturas

        #region Propriedades

        /// <summary>
        /// Armazena o valor do ID da turma.
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o valor do ID da turma disciplina.
        /// </summary>
        private long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        private byte PosicaoDocente
        {
            get
            {
                return (VS_turmaDisciplinaCompartilhada != null || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Compartilhado, ApplicationWEB.AppMinutosCacheLongo) == VS_tdt_posicao)
                       || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo) == VS_tdt_posicao
                       ? ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo) : VS_tdt_posicao;
            }
        }

        /// <summary>
        /// Guarda a posição do docente.
        /// </summary>
        public byte VS_posicao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_posicao"] ?? 0);
            }

            set
            {
                ViewState["VS_posicao"] = value;
            }
        }

        /// <summary>
        /// ViewState com o id do curriculo periodo
        /// </summary>
        private byte VS_tur_tipo
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tur_tipo"] ?? 0);
            }
            set
            {
                ViewState["VS_tur_tipo"] = value;
            }
        }

        /// <summary>
        /// ViewState com o id do formato de avaliação.
        /// </summary>
        private int VS_fav_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_fav_id"] ?? -1);
            }
            set
            {
                ViewState["VS_fav_id"] = value;
            }
        }

        /// <summary>
        /// ViewState com o id do calendario
        /// </summary>
        public int VS_cal_id
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
        /// ViewState que armazena o valor do ID da escola.
        /// </summary>
        private int VS_esc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID da unidade de escola.
        /// </summary>
        private int VS_uni_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_uni_id"] ?? -1);
            }

            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do curso.
        /// </summary>
        public int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do currículo do curso.
        /// </summary>
        public int VS_crr_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crr_id"] ?? -1);
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do período do curso.
        /// </summary>
        public int VS_crp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_id"] ?? -1);
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// ViewState com o id do curriculo periodo
        /// </summary>
        private int VS_crp_idAnterior
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_idAnterior"] ?? -1);
            }
            set
            {
                ViewState["VS_crp_idAnterior"] = value;
            }
        }

        /// <summary>
        /// ViewState com a posição do docente responsável
        /// </summary>
        public byte VS_tdt_posicao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tdt_posicao"] ?? 0);
            }
            set
            {
                ViewState["VS_tdt_posicao"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da orientação curricular para lançamento de alcance das habilidades.
        /// </summary>
        private long VS_ocr_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_ocr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_ocr_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do período do calendáio para lançamento de alcance das habilidades.
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
        /// Armazena o valor do ID do tipo de disciplina da turma disciplina.
        /// </summary>
        private int VS_tds_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tds_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tds_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena as orientãções curriculares para cada período.
        /// </summary>
        private List<OrientacaoCurricular> VS_OrientacaoCurricular
        {
            get
            {
                return (List<OrientacaoCurricular>)(ViewState["VS_OrientacaoCurricular"] ?? ((ViewState["VS_OrientacaoCurricular"] = new List<OrientacaoCurricular>())));
            }

            set
            {
                ViewState["VS_OrientacaoCurricular"] = value;
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
                                ViewState["VS_ltPermissaoPlanejamentoAnual"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(PosicaoDocente, (byte)EnumModuloPermissao.PlanejamentoAnual)
                            )
                        );
            }

            set
            {
                ViewState["VS_ltPermissaoPlanejamentoAnual"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena os calendarios do período(mesmos itens do repeater mostrado em tela).
        /// </summary>
        private List<Struct_CalendarioPeriodos> VS_CalendarioPeriodo
        {
            get
            {
                return (List<Struct_CalendarioPeriodos>)(ViewState["VS_CalendarioPeriodo"] ?? ((ViewState["VS_CalendarioPeriodo"] = new List<Struct_CalendarioPeriodos>())));
            }

            set
            {
                ViewState["VS_CalendarioPeriodo"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena os ids das turmas normais de matricula de alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        private string VS_TurmasNormais_Ids
        {
            get
            {
                return (ViewState["VS_TurmasNormais_Ids"] ?? "").ToString();
            }

            set
            {
                ViewState["VS_TurmasNormais_Ids"] = value;
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de diagnóstico inicial.
        /// </summary>
        private string DiagnosticoInicial
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_DIAGNOSTICOINICIAL);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de planejamento anual.
        /// </summary>
        private string PlanejamentoAnual
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_PLANEJAMENTOANUAL);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de recursos.
        /// </summary>
        private string Recursos
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_RECURSOS);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de avaliação do COC.
        /// </summary>
        private string Coc_Avaliacao
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_COC_AVALIACAO);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de replanejamento.
        /// </summary>
        private string Coc_Replanejamento
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_COC_REPLANEJAMENTO);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de replanejamento para o último bimestre.
        /// </summary>
        private string Coc_ReplanejamentoFinal
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_COC_REPLANEJAMENTOFINAL);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de intervenções pedagócgicas.
        /// </summary>
        private string BimestreIntervencoesPedagogicas
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_BIMESTRE_INTERVENCOESPEDAGOGICAS);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de registro de intervenções.
        /// </summary>
        private string BimestreRegistroIntervencoes
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_BIMESTRE_REGISTROINTERVENCOES);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de avaliação do planejamento anual.
        /// </summary>
        private string Planejamento_Avaliacao
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_AVALIACAO);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de recursos do coc.
        /// </summary>
        private string Planejamento_Coc_Recursos
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_COC_RECURSOS);
            }
        }

        private int? nivel;

        /// <summary>
        /// Nível da orientação curricular anterior usado no databound desses.
        /// </summary>
        private int Nivel
        {
            get
            {
                return Convert.ToInt32(nivel ?? 0);
            }

            set
            {
                nivel = value;
            }
        }

        private string nomeOrientacaoCurricular;

        /// <summary>
        /// Hierarquia das orientações curriculares.
        /// </summary>
        private string NomeOrientacaoCurricular
        {
            get
            {
                return (nomeOrientacaoCurricular ?? string.Empty);
            }

            set
            {
                nomeOrientacaoCurricular = value;
            }
        }

        private string nomeOrientacaoCurricularUltimoNivel;

        /// <summary>
        /// Último nível da orientação curricular
        /// </summary>
        private string NomeOrientacaoCurricularUltimoNivel
        {
            get
            {
                return (nomeOrientacaoCurricularUltimoNivel ?? string.Empty);
            }

            set
            {
                nomeOrientacaoCurricularUltimoNivel = value;
            }
        }

        private string nomeOrientacaoCurricularUltimoNivelPlural;

        /// <summary>
        /// Último nível da orientação curricular
        /// </summary>
        private string NomeOrientacaoCurricularUltimoNivelPlural
        {
            get
            {
                return (nomeOrientacaoCurricularUltimoNivelPlural ?? string.Empty);
            }

            set
            {
                nomeOrientacaoCurricularUltimoNivelPlural = value;
            }
        }

        private string tev_id;

        /// <summary>
        /// Tipos de eventos que bloqueiam as abas.
        /// </summary>
        private string Tev_id
        {
            get
            {
                return tev_id ??
                       (tev_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) 
                                 + ";" +
                                 ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            }
        }

        private int? tpc_ordemMax;

        /// <summary>
        /// Último período do calendário.
        /// </summary>
        private int Tpc_ordemMax
        {
            get
            {
                return Convert.ToInt32(tpc_ordemMax ?? 0);
            }

            set
            {
                tpc_ordemMax = value;
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

        private bool? permiteEditar;

        private bool PermiteEditar
        {
            get
            {
                return Convert.ToBoolean(permiteEditar ?? (permiteEditar = ACA_EventoBO.SelecionaMaiorDataPorTipoPeriodoEscola(VS_esc_id, VS_uni_id, VS_tpc_id, Tev_id).Date >= DateTime.Now.Date));
            }
        }

        /// <summary>
        /// Flag que indica se a disciplina é oferecia para alunos de libras.
        /// </summary>
        private bool VS_DisciplinaEspecial
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_DisciplinaEspecial"] ?? false);
            }

            set
            {
                ViewState["VS_DisciplinaEspecial"] = value;
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

        public byte VS_tur_situacao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tur_situacao"] ?? 0);
            }

            set
            {
                ViewState["VS_tur_situacao"] = value;
            }
        }

        /// <summary>
        /// Armazena a data de encerramento da turma em viewstate.
        /// </summary>
        public DateTime VS_tur_dataEncerramento
        {
            get
            {
                if (ViewState["VS_tur_dataEncerramento"] == null)
                    return new DateTime();
                else
                    return Convert.ToDateTime(ViewState["VS_tur_dataEncerramento"]);
            }
            set
            {
                ViewState["VS_tur_dataEncerramento"] = value;
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

        private List<sOrientacaoNivelAprendizado> dtOrientacaoNiveisAprendizado;
        private List<sNivelAprendizado> dtNivelArendizadoCurriculo;
        private List<sNivelAprendizado> dtNivelArendizadoCurriculoAnterior;


        private List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas> listHabilidadesComAulaPlanejada
        {
            get
            {
                if (ViewState["VS_listHabilidadesComAulaPlanejada"] == null)
                    return new List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas>();

                return (List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas>)ViewState["VS_listHabilidadesComAulaPlanejada"];
            }
            set
            {
                ViewState["VS_listHabilidadesComAulaPlanejada"] = value;
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsgMarcarAlcancadoAnoAnterior.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCPlanejamentoAnual.lblMsgMarcarAlcancadoAnoAnterior.Text").ToString(), 
                                                                              UtilBO.TipoMensagem.Informacao);
            }

            SetarJavaScript();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Seta funções de javascript nos textbox.
        /// </summary>
        private void SetarJavaScript()
        {
            btnTextoGrandeDiagnosticoInicial.OnClientClick = "abrirTextoGrande('" + lblDiagnosticoInicial.ClientID + "', '" + txtDiagnosticoInicial.ClientID + "'); return false;";
            btnTextoGrandePlanejamento.OnClientClick = "abrirTextoGrande('" + lblPlanejamento.ClientID + "', '" + txtPlanejamento.ClientID + "'); return false;";
            btnTextoGrandeAvaliacaoTrabalho.OnClientClick = "abrirTextoGrande('" + lblAvaliacaoTrabalho.ClientID + "', '" + txtAvaliacaoTrabalho.ClientID + "'); return false;";

            if (!string.IsNullOrEmpty(DiagnosticoInicial))
            {
                // Altera a função para configurar a mensagem também no txt grande.
                btnTextoGrandeDiagnosticoInicial.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                        lblDiagnosticoInicial.ClientID,
                        txtDiagnosticoInicial.ClientID,
                        DiagnosticoInicial);

                lblDiagnosticoInicialInfo.Text = DiagnosticoInicial;
            }

            if (!string.IsNullOrEmpty(Planejamento_Avaliacao))
            {
                btnTextoGrandeAvaliacaoTrabalho.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                    lblAvaliacaoTrabalho.ClientID,
                    txtAvaliacaoTrabalho.ClientID,
                    Planejamento_Avaliacao);

                lblAvaliacaoTrabalhoInfo.Text = Planejamento_Avaliacao;
            }

            if (!string.IsNullOrEmpty(PlanejamentoAnual))
            {
                // Altera a função para configurar a mensagem também no txt grande.
                btnTextoGrandePlanejamento.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                        lblPlanejamento.ClientID,
                        txtPlanejamento.ClientID,
                        PlanejamentoAnual);

                lblPlanejamentoInfo.Text = PlanejamentoAnual;
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
        /// O método copia n vezes uma string e a concatena para si mesma.
        /// </summary>
        /// <param name="valor">String a ser copiado.</param>
        /// <param name="multiplicacao">Quantidade de vezes que o valor será replicado.</param>
        /// <returns></returns>
        private string MultiplicaString(string valor, int multiplicacao)
        {
            StringBuilder sb = new StringBuilder(multiplicacao * valor.Length);
            for (int i = 0; i < multiplicacao; i++)
            {
                sb.Append(valor);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Carrega os dados de planejamento anual da turma selecionada.
        /// </summary>
        public void CarregarTurma(
            long tur_id
            , long tud_id
            , byte tdt_posicao
            , TUR_Turma entTurma = null
            , TUR_TurmaDisciplina entTurmaDisciplina = null
            , string tur_ids = null)
        {
            VS_TurmasNormais_Ids = tur_ids;
            VS_tur_id = tur_id;
            VS_tud_id = tud_id;
            DataTable dtTipoDisciplina = TUR_TurmaDisciplinaBO.SelecionaTipoDisciplinaPorTurmaDisciplina(VS_tud_id);
            VS_tds_id = dtTipoDisciplina.Rows.Count > 0 ?
                Convert.ToInt32(dtTipoDisciplina.Rows[0]["tds_id"]) : -1;

            VS_tdt_posicao = tdt_posicao > 0 ? tdt_posicao :
                TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id, VS_tud_id, ApplicationWEB.AppMinutosCacheLongo);

            VS_ltPermissaoPlanejamentoAnual = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(PosicaoDocente, (byte)EnumModuloPermissao.PlanejamentoAnual) :
                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo((byte)1, (byte)EnumModuloPermissao.PlanejamentoAnual);

            // Carrega os dados da disciplina da turma
            TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina { tud_id = VS_tud_id };
            if (entTurmaDisciplina == null || entTurmaDisciplina.tud_id != VS_tud_id)
                TUR_TurmaDisciplinaBO.GetEntity(tud);
            else
                tud = entTurmaDisciplina;

            // Carrega os dados da turma
            TUR_Turma tur = new TUR_Turma { tur_id = VS_tur_id };
            if (entTurma == null || entTurma.tur_id != VS_tur_id)
                TUR_TurmaBO.GetEntity(tur);
            else
                tur = entTurma;

            VS_tur_tipo = tur.tur_tipo;
            VS_cal_id = tur.cal_id;
            VS_esc_id = tur.esc_id;
            VS_uni_id = tur.uni_id;
            VS_DisciplinaEspecial = tud.tud_disciplinaEspecial;
            VS_tur_situacao = tur.tur_situacao;
            VS_tur_dataEncerramento = tur.tur_dataEncerramento;

            VS_tipoDocente = VS_tdt_posicao > 0 ?
                ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(PosicaoDocente, ApplicationWEB.AppMinutosCacheLongo) :
                EnumTipoDocente.Titular;

            bool permiteEditar = true;

            //Carrega informações do diagnóstico inicial da turma, proposta de trabalho, e avaliação do trabalho
            DataTable dtPlanejamentoAnual = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorDisciplinaPermissaoDocente(VS_tud_id, PosicaoDocente) :
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplinaPeriodoCalendarioNulo(VS_tud_id, PosicaoDocente);

            listHabilidadesComAulaPlanejada = CLS_TurmaAulaOrientacaoCurricularBO.AulasPlanejadasSelecionaPorDisciplina(VS_tud_id);

            if (dtPlanejamentoAnual.Rows.Count > 0)
            {
                //  hdnPosicaoAnual.Value = dtPlanejamentoAnual.Rows[0]["tdt_posicao"].ToString();

                txtDiagnosticoInicial.Text = dtPlanejamentoAnual.Rows[0]["tdp_diagnostico"].ToString();
                txtPlanejamento.Text = dtPlanejamentoAnual.Rows[0]["tdp_planejamento"].ToString();
                txtAvaliacaoTrabalho.Text = dtPlanejamentoAnual.Rows[0]["tdp_avaliacaoTrabalho"].ToString();

                lblTdp_id_Anual.Text = dtPlanejamentoAnual.Rows[0]["tdp_id"].ToString();

                if (!string.IsNullOrEmpty(dtPlanejamentoAnual.Rows[0]["cur_id"].ToString()))
                    VS_cur_id = Convert.ToInt32(dtPlanejamentoAnual.Rows[0]["cur_id"].ToString());

                if (!string.IsNullOrEmpty(dtPlanejamentoAnual.Rows[0]["crr_id"].ToString()))
                    VS_crr_id = Convert.ToInt32(dtPlanejamentoAnual.Rows[0]["crr_id"].ToString());

                if (!string.IsNullOrEmpty(dtPlanejamentoAnual.Rows[0]["crp_id"].ToString()))
                    VS_crp_id = Convert.ToInt32(dtPlanejamentoAnual.Rows[0]["crp_id"].ToString());

                byte posicao = Convert.ToByte(dtPlanejamentoAnual.Rows[0]["tdt_posicao"].ToString());

                permiteEditar = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                    ((VS_turmaDisciplinaCompartilhada == null && VS_ltPermissaoPlanejamentoAnual.Any(p => p.tdt_posicaoPermissao == posicao && p.pdc_permissaoEdicao))
                        || (VS_turmaDisciplinaCompartilhada != null && !VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento)) :
                    __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
            else
            {
                // Atualiza o curso, curriculo e periodo da turma
                List<TUR_TurmaCurriculo> Curriculos = TUR_TurmaCurriculoBO.GetSelectBy_Turma(tur.tur_id, ApplicationWEB.AppMinutosCacheLongo);
                VS_cur_id = Curriculos.First().cur_id;
                VS_crr_id = Curriculos.First().crr_id;
                VS_crp_id = Curriculos.First().crp_id;

                txtDiagnosticoInicial.Text =
                txtPlanejamento.Text =
                txtAvaliacaoTrabalho.Text =
                lblTdp_id_Anual.Text = string.Empty;

                permiteEditar = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                    ((VS_turmaDisciplinaCompartilhada == null && VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoEdicao))
                        || (VS_turmaDisciplinaCompartilhada != null && !VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento)) :
                    __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            if ((TurmaDisciplinaTipo)tud.tud_tipo == TurmaDisciplinaTipo.Optativa ||
                    (TurmaDisciplinaTipo)tud.tud_tipo == TurmaDisciplinaTipo.Eletiva ||
                    (TurmaDisciplinaTipo)tud.tud_tipo == TurmaDisciplinaTipo.DocenteTurmaEletiva ||
                    (TurmaDisciplinaTipo)tud.tud_tipo == TurmaDisciplinaTipo.DependeDisponibilidadeProfessorEletiva)
            {
                if (dtPlanejamentoAnual.Rows.Count > 0)
                {
                    CarregarHabilidades(PosicaoDocente);
                }
                else
                {
                    rptPeriodos.Visible = rptPeriodosAbas.Visible = fds0COC.Visible = false;
                }
            }
            else
            {
                CarregarHabilidades(PosicaoDocente);
            }

            // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
            {
                permiteEditar = false;
            }

            HabilitaControles(fdsPlanejamentoAnual.Controls, permiteEditar);
        }

        /// <summary>
        /// O método carrega as orientações curriculares da turma.
        /// </summary>
        private void CarregarHabilidades(byte posicaoAux)
        {
            rptPeriodos.Visible = rptPeriodosAbas.Visible = false;

            //Como agora possuem visoes para esse modulo, esse codigo foi inserido pois o compartilhado nunca vai possuir
            //Seu proprio planejamento, ou seja, irá carregar o do titular.
            //byte posicaoCompartilhado = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocente(EnumTipoDocente.Compartilhado);
            //byte posicaoAux = posicaoCompartilhado == VS_tdt_posicao 
            //                ? ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocente(EnumTipoDocente.Titular) 
            //                : VS_tdt_posicao;

            DataTable dtPlanejamentoPeriodo = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorDisciplinaCalendarioPermissaoDocente(VS_cal_id, VS_tud_id, posicaoAux) :
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplinaCalendario(VS_cal_id, VS_tud_id, PosicaoDocente);

            // Carrega os dados do Grupamento Anterior
            VS_crp_idAnterior = ACA_CurriculoPeriodoBO.VerificaPeriodoAnterior(VS_cur_id, VS_crr_id, VS_crp_id);
            if (VS_tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno)
            {
                VS_CalendarioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);
            }
            else
            {
                List<TUR_TurmaDisciplinaCalendario> listCal = TUR_TurmaDisciplinaCalendarioBO.GetSelectBy_TurmaDisciplina(VS_tud_id);
                List<Struct_CalendarioPeriodos> periodosCalendario = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);
                List<Struct_CalendarioPeriodos> periodosCalendarioTud = periodosCalendario.FindAll(p => listCal.Exists(q => q.tpc_id == p.tpc_id));
                VS_CalendarioPeriodo = periodosCalendarioTud;
            }

            DataTable dtOrientacaoCurricularAnterior = new DataTable();

            if (dtPlanejamentoPeriodo.Rows.Count > 0)
            {

                DataTable dtOrientacaoCurricular = CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplina(VS_cur_id, VS_crr_id, VS_crp_id, -1, -1, VS_tur_id, VS_tud_id, VS_cal_id, PosicaoDocente, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                VS_OrientacaoCurricular = dtOrientacaoCurricular.Rows.Count > 0 ?
                                          (from int tpc_id in dtPlanejamentoPeriodo.Rows.Cast<DataRow>().GroupBy(dr => Convert.ToInt32(dr["tpc_id"])).Select(g => g.Key)
                                           select new OrientacaoCurricular
                                           {
                                               tpc_id = tpc_id
                                               ,
                                               dtOrientacaoCurricular = dtOrientacaoCurricular.Rows.Cast<DataRow>().Where(dr => Convert.ToInt32(dr["tpc_id"]) == tpc_id).CopyToDataTable()
                                           }).ToList() :
                                           new List<OrientacaoCurricular>();



                DataTable dtNivel = ORC_NivelBO.SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(VS_cur_id, VS_crr_id, VS_crp_id, VS_cal_id, VS_tds_id);

                //verifica se tem pelo menos 2 niveis de orientações
                if (dtNivel.Rows.Count >= 2)
                {

                    NomeOrientacaoCurricular = string.Join(
                                                      ", ",
                                                      (from DataRow dr in dtNivel.Rows
                                                       where Convert.ToByte(dr["nvl_situacao"]) == 1
                                                       orderby Convert.ToInt32(dr["nvl_ordem"]) ascending
                                                       group dr by dr["nvl_nome"].ToString() into grupo
                                                       select grupo.Key).ToArray()
                                                  );

                    if (!string.IsNullOrEmpty(NomeOrientacaoCurricular))
                    {
                        string nome = NomeOrientacaoCurricular;
                        NomeOrientacaoCurricular = nome[0].ToString() + nome.Substring(1).ToLower();
                    }

                    NomeOrientacaoCurricularUltimoNivel =
                                                       (from DataRow dr in dtNivel.Rows
                                                        where Convert.ToByte(dr["nvl_situacao"]) == 1
                                                        orderby Convert.ToInt32(dr["nvl_ordem"]) descending
                                                        select dr["nvl_nome"].ToString()).ToList().First();

                    NomeOrientacaoCurricularUltimoNivelPlural =
                                                        (from DataRow dr in dtNivel.Rows
                                                         where Convert.ToByte(dr["nvl_situacao"]) == 1
                                                         orderby Convert.ToInt32(dr["nvl_ordem"]) descending
                                                         select dr["nvl_nomePlural"].ToString()).ToList().First();

                    divLancamentoAlcance.Attributes["title"] = String.Format("Lançamento de alcance de {0}", NomeOrientacaoCurricularUltimoNivel.ToLower());
                    string infoAlcance = String.Format("Marque apenas os alunos que não alcançaram os(as) {0}.", NomeOrientacaoCurricularUltimoNivelPlural.ToLower());
                    infoAlcance += "<br />Marque a opção Efetivado para indicar que o lançamento de alcance foi finalizado.";

                    lblInfoAlcance.Text = UtilBO.GetErroMessage(infoAlcance, UtilBO.TipoMensagem.Informacao);

                    lblMsgMarcarAlcancadoAnoAnterior.Text = UtilBO.GetErroMessage("Marcar os(as) " + NomeOrientacaoCurricularUltimoNivelPlural.ToLower() + " alcançados(as) durante o ano letivo anterior.", UtilBO.TipoMensagem.Informacao);

                    int index = NomeOrientacaoCurricular.LastIndexOf(',');
                    NomeOrientacaoCurricular = string.IsNullOrEmpty(NomeOrientacaoCurricular) ?
                        "Objetivos, conteúdos e habilidades das orientações curriculares" :
                        NomeOrientacaoCurricular.Remove(index, 1).Insert(index, " e");
                }

                Tpc_ordemMax = dtPlanejamentoPeriodo.Rows.Cast<DataRow>().Select(dr => Convert.ToInt32(dr["tpc_ordem"])).Max();

                List<string> ltOcr_id = (from OrientacaoCurricular ocr in VS_OrientacaoCurricular
                                         from DataRow dr in ocr.dtOrientacaoCurricular.Rows
                                         let chave = dr["Chave"].ToString()
                                         let idsChave = chave.Split(';')
                                         let ocr_id = idsChave.Length > 1 ? idsChave[1] : ""
                                         where !string.IsNullOrEmpty(ocr_id)
                                         select ocr_id).ToList();

                if (VS_crp_idAnterior > 0)
                {
                    dtOrientacaoCurricularAnterior = CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplina(VS_cur_id, VS_crr_id, VS_crp_id, VS_crp_idAnterior, 1, VS_tur_id, VS_tud_id, VS_cal_id, PosicaoDocente, true, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ltOcr_id.AddRange((from DataRow dr in dtOrientacaoCurricularAnterior.Rows
                                       let chave = dr["Chave"].ToString()
                                       let idsChave = chave.Split(';')
                                       let ocr_id = idsChave.Length > 1 ? idsChave[1] : ""
                                       where !string.IsNullOrEmpty(ocr_id)
                                       select ocr_id).ToList());
                }

                string ocr_ids = string.Join(";", ltOcr_id.Distinct().ToArray());

                dtOrientacaoNiveisAprendizado = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, null, ApplicationWEB.AppMinutosCacheLongo);
                dtNivelArendizadoCurriculo = ORC_NivelAprendizadoBO.GetSelectNiveisAprendizadoAtivos(VS_cur_id, VS_crr_id, VS_crp_id, ApplicationWEB.AppMinutosCacheLongo);



                rptPeriodos.Visible = rptPeriodosAbas.Visible = true;
                rptPeriodosAbas.DataSource = dtPlanejamentoPeriodo;
                rptPeriodos.DataSource = VS_CalendarioPeriodo;
                rptPeriodos.DataBind();
                rptPeriodosAbas.DataBind();
            }
            
            if (VS_crp_idAnterior > 0)
            {
                // Se for turma eletiva do aluno ou se não existir nenhum período do calendário cadastrado
                // para a disciplina, não exibe o grid de diagnostico
                if (VS_tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno || rptPeriodos.Items.Count <= 0)
                {
                    fds0COC.Visible = false;
                }
                else
                {
                    Nivel = 0;

                    dtNivelArendizadoCurriculoAnterior = ORC_NivelAprendizadoBO.GetSelectNiveisAprendizadoAtivos(VS_cur_id, VS_crr_id, VS_crp_idAnterior, ApplicationWEB.AppMinutosCacheLongo);
                    rptHabilidades.DataSource = dtOrientacaoCurricularAnterior;
                    rptHabilidades.DataBind();

                    fds0COC.Visible = rptHabilidades.Items.Count > 0;
                }
            }
            else
            {
                fds0COC.Visible = false;
            }

        }

        /// <summary>
        /// Carregar alunos para lançamento de alcance de habilidades.
        /// </summary>
        private void CarregarLancamentoAlcance()
        {
            ORC_OrientacaoCurricular entOrientacao = new ORC_OrientacaoCurricular
            {
                ocr_id = VS_ocr_id
            };
            ORC_OrientacaoCurricularBO.GetEntity(entOrientacao);

            ORC_Nivel entNivel = new ORC_Nivel
            {
                nvl_id = entOrientacao.nvl_id
            };
            ORC_NivelBO.GetEntity(entNivel);

            lblOrientacaoCurricular.Text = String.Format("<b>{0}: {1}</b>", entNivel.nvl_nome, entOrientacao.ocr_descricao);

            List<AlunosTurmaDisciplina> lstAlunos = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(VS_tud_id, VS_tpc_id, VS_tipoDocente, false, new DateTime(), new DateTime(), ApplicationWEB.AppMinutosCacheMedio, VS_TurmasNormais_Ids);
            DataTable dtItensAlcancados = CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO.SelecionaAlunosPorTurmaDisciplinaPeriodo(VS_tud_id, VS_ocr_id, VS_tpc_id, VS_cal_id, VS_TurmasNormais_Ids);

            var alunosItensAlcancados = from AlunosTurmaDisciplina aluno in lstAlunos
                                        join DataRow dr in dtItensAlcancados.Rows on aluno.alu_id equals Convert.ToInt64(dr["alu_id"]) into itens
                                        from dr in itens.DefaultIfEmpty()
                                        select new
                                        {
                                            aluno.alu_id
                                            , aha_id = Convert.ToInt32(dr != null ? dr["aha_id"] : 0)
                                            , aha_alcancada = Convert.ToBoolean(dr != null ? dr["aha_alcancada"]: 1)
                                            , aha_efetivada = Convert.ToBoolean(dr != null ? dr["aha_efetivada"]: 0)
                                            , tud_id = VS_tud_id
                                            , aluno.mtu_id
                                            , aluno.mtd_id
                                            , tpc_id = VS_tpc_id
                                            , aluno.numeroChamada
                                            , nomeAluno = aluno.pes_nome
                                            , ocr_id = VS_ocr_id
                                            , aluno.mtd_situacao
                                        };

            grvAlunos.DataSource = alunosItensAlcancados;
            grvAlunos.DataBind();

            if (grvAlunos.HeaderRow != null)
            {
                // Muda essa propriedade pra renderizar <thead> na tabela html, para funcionar o tableSorter.
                grvAlunos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            HabilitaControles(grvAlunos.Controls, PermiteEditar);

            if (grvAlunos.Rows.Count > 0)
            {
                CheckBox chkEfetivado = (CheckBox)grvAlunos.HeaderRow.FindControl("chkEfetivado");
                chkEfetivado.Checked = dtItensAlcancados.Rows.Cast<DataRow>().Any(p => Convert.ToBoolean(p["aha_efetivada"]));
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LancarAlcance", "$(document).ready(function() { $('.divLancamentoAlcance').dialog('open'); });", true);
        }

        /// <summary>
        /// Retorna lista de lançamentos de alcance.
        /// </summary>
        /// <returns></returns>
        public List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> CriarListaAlcance()
        {
            if (grvAlunos.Rows.Count > 0)
            {
                CheckBox chkEfetivado = (CheckBox)grvAlunos.HeaderRow.FindControl("chkEfetivado");

                return (from GridViewRow row in grvAlunos.Rows
                        let dataKeys = grvAlunos.DataKeys[row.RowIndex]
                        let tud_id = Convert.ToInt64(dataKeys.Values["tud_id"])
                        let alu_id = Convert.ToInt64(dataKeys.Values["alu_id"])
                        let mtu_id = Convert.ToInt32(dataKeys.Values["mtu_id"])
                        let mtd_id = Convert.ToInt32(dataKeys.Values["mtd_id"])
                        let ocr_id = Convert.ToInt64(dataKeys.Values["ocr_id"])
                        let aha_id = Convert.ToInt32(dataKeys.Values["aha_id"])
                        let tpc_id = Convert.ToInt32(dataKeys.Values["tpc_id"])
                        let chkAlcancado = (CheckBox)row.FindControl("chkAlcancado")
                        select new CLS_AlunoTurmaDisciplinaOrientacaoCurricular
                        {
                            tud_id = tud_id,
                            alu_id = alu_id,
                            mtu_id = mtu_id,
                            mtd_id = mtd_id,
                            ocr_id = ocr_id,
                            aha_id = aha_id,
                            tpc_id = tpc_id,
                            aha_alcancada = !chkAlcancado.Checked,
                            aha_efetivada = chkEfetivado.Checked,
                            IsNew = aha_id <= 0
                        }).ToList();
            }

            return null;
        }

        /// <summary>
        /// Seta a visibilidade da imagem que indica se o lançamento de alcance foi efetivado.
        /// </summary>
        /// <param name="efetivado"></param>
        private void SetaVisibilidadeSituacaoAlcancado(bool efetivado, bool modificado)
        {
            (from RepeaterItem itemAba in rptPeriodosAbas.Items
             let hdnTpcId = (HiddenField)itemAba.FindControl("hdnTpcId")
             where Convert.ToInt32(hdnTpcId.Value) == VS_tpc_id
             let rptHabilidadesCOC = (Repeater)itemAba.FindControl("rptHabilidadesCOC")
             from RepeaterItem itemHabilidade in rptHabilidadesCOC.Items
             let hdnChave = (HiddenField)itemHabilidade.FindControl("hdnChave")
             where !string.IsNullOrEmpty(hdnChave.Value)
             let ids = hdnChave.Value.Split(';')
             where ids.Length > 1
             let ocr_id = Convert.ToInt64(ids[1])
             where ocr_id == VS_ocr_id
             select (Image)itemHabilidade.FindControl("imgSituacaoEfetivada")).ToList().ForEach(p => p.Visible = efetivado);

            (from RepeaterItem itemAba in rptPeriodosAbas.Items
             let hdnTpcId = (HiddenField)itemAba.FindControl("hdnTpcId")
             where Convert.ToInt32(hdnTpcId.Value) == VS_tpc_id
             let rptHabilidadesCOC = (Repeater)itemAba.FindControl("rptHabilidadesCOC")
             from RepeaterItem itemHabilidade in rptHabilidadesCOC.Items
             let hdnChave = (HiddenField)itemHabilidade.FindControl("hdnChave")
             where !string.IsNullOrEmpty(hdnChave.Value)
             let ids = hdnChave.Value.Split(';')
             where ids.Length > 1
             let ocr_id = Convert.ToInt64(ids[1])
             where ocr_id == VS_ocr_id
             select (Image)itemHabilidade.FindControl("imgSituacao")).ToList().ForEach(p => p.Visible = modificado);
        }

        /// <summary>
        /// Cria lista de dignósticos do grupamento no ano anterior para salvar.
        /// </summary>
        /// <returns></returns>
        public List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> CriarListaDiagnostico()
        {
            List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> lista = new List<CLS_PlanejamentoOrientacaoCurricularDiagnostico>();

            if (fds0COC.Visible)
            {
                lista.AddRange(
                    (from RepeaterItem habilidade in rptHabilidades.Items
                     let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                     let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                     where permiteLancamento
                     let hdnPosicao = (HiddenField)habilidade.FindControl("hdnPosicao")
                     let posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicao.Value) ? PosicaoDocente.ToString() : hdnPosicao.Value)
                     let chave = ((HiddenField)habilidade.FindControl("hdnChave")).Value
                     let chkAlcancado = (CheckBox)habilidade.FindControl("chkAlcancado")
                     select new CLS_PlanejamentoOrientacaoCurricularDiagnostico
                     {
                         tud_id = Convert.ToInt64(chave.Split(';')[0])
                         ,
                         ocr_id = Convert.ToInt64(chave.Split(';')[1])
                         ,
                         pod_alcancado = chkAlcancado.Checked
                         ,
                         tdt_posicao = posicao
                     }).ToList()
                );
            }

            return lista;
        }

        /// <summary>
        /// Cria lista de orientações curriculares para salvar
        /// </summary>
        /// <returns></returns>
        public List<CLS_PlanejamentoOrientacaoCurricular> CriarListaHabilidades()
        {
            List<CLS_PlanejamentoOrientacaoCurricular> lista = new List<CLS_PlanejamentoOrientacaoCurricular>();

            if (rptPeriodosAbas.Visible)
            {
                lista.AddRange(
                        (from RepeaterItem coc in rptPeriodosAbas.Items
                         let fdsHabilidadesCOC = (HtmlGenericControl)coc.FindControl("fdsHabilidadesCOC")
                         where fdsHabilidadesCOC.Visible
                         let rptHabilidadesCOC = (Repeater)coc.FindControl("rptHabilidadesCOC")
                         from RepeaterItem habilidade in rptHabilidadesCOC.Items
                         let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                         let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                         where permiteLancamento
                         let hdnPosicao = (HiddenField)habilidade.FindControl("hdnPosicao")
                         let posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicao.Value) ? PosicaoDocente.ToString() : hdnPosicao.Value)
                         let chave = ((HiddenField)habilidade.FindControl("hdnChave")).Value
                         let chkPlanejado = (CheckBox)habilidade.FindControl("chkPlanejado")
                         let chkTrabalhado = (CheckBox)habilidade.FindControl("chkTrabalhado")
                         //let chkAlcancado = (CheckBox)habilidade.FindControl("chkAlcancado")
                         select new CLS_PlanejamentoOrientacaoCurricular
                         {
                             tud_id = Convert.ToInt64(chave.Split(';')[0])
                             ,
                             ocr_id = Convert.ToInt64(chave.Split(';')[1])
                             ,
                             tpc_id = Convert.ToInt32(chave.Split(';')[2])
                             ,
                             poc_planejado = chkPlanejado.Checked
                             ,
                             poc_trabalhado = chkTrabalhado.Checked
                             ,
                             poc_alcancado = false
                             ,
                             tdt_posicao = posicao
                         }).ToList()
                );
            }

            return lista;
        }

        /// <summary>
        /// Lista os dados do planejamento anual da turma.
        /// </summary>
        /// <returns></returns>
        public List<CLS_TurmaDisciplinaPlanejamento> CriarListaPlanejamento()
        {
            List<CLS_TurmaDisciplinaPlanejamento> lista = new List<CLS_TurmaDisciplinaPlanejamento>();

            int tdp_idAnual = string.IsNullOrEmpty(lblTdp_id_Anual.Text) ? -1 : Convert.ToInt32(lblTdp_id_Anual.Text);

            lista.Add(
                new CLS_TurmaDisciplinaPlanejamento
                {
                    tud_id = VS_tud_id
                     ,
                    tdp_id = tdp_idAnual
                     ,
                    tdp_diagnostico = txtDiagnosticoInicial.Text
                     ,
                    tdp_planejamento = txtPlanejamento.Text
                     ,
                    tdp_avaliacaoTrabalho = txtAvaliacaoTrabalho.Text
                     ,
                    tdp_recursos = string.Empty
                     ,
                    cur_id = VS_cur_id
                     ,
                    crr_id = VS_crr_id
                     ,
                    crp_id = VS_crp_id
                     ,
                    tdt_posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicaoAnual.Value) ? PosicaoDocente.ToString() : hdnPosicaoAnual.Value)
                     ,
                    tdp_situacao = 1
                     ,
                    IsNew = tdp_idAnual <= 0
                }
            );

            if (rptPeriodosAbas.Visible)
            {
                lista.AddRange(
                    (from RepeaterItem coc in rptPeriodosAbas.Items
                     let txtDiagnosticoCOC = (TextBox)coc.FindControl("txtDiagnosticoCOC")
                     let txtPlanejamentoCOC = (TextBox)coc.FindControl("txtPlanejamentoCOC")
                     let txtRecursosCOC = (TextBox)coc.FindControl("txtRecursosCOC")
                     let txtIntervencoesPedagogicasCOC = (TextBox)coc.FindControl("txtIntervencoesPedagogicasCOC")
                     let txtRegistroIntervencoesCOC = (TextBox)coc.FindControl("txtRegistroIntervencoesCOC")
                     let lblTdp_id_COC = (Label)coc.FindControl("lblTdp_id_COC")
                     let tdp_id = string.IsNullOrEmpty(lblTdp_id_COC.Text) ? -1 : Convert.ToInt32(lblTdp_id_COC.Text)
                     let hdnTpcId = (HiddenField)coc.FindControl("hdnTpcId")
                     let tpc_id = Convert.ToInt32(hdnTpcId.Value)
                     let hdnPosicao = (HiddenField)coc.FindControl("hdnPosicao")
                     let posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicao.Value) ? PosicaoDocente.ToString() : hdnPosicao.Value)
                     select new CLS_TurmaDisciplinaPlanejamento
                     {
                         tud_id = VS_tud_id
                         ,
                         tdp_id = tdp_id
                         ,
                         tpc_id = tpc_id
                         ,
                         tdp_diagnostico = txtDiagnosticoCOC.Text
                         ,
                         tdp_planejamento = txtPlanejamentoCOC.Text
                         ,
                         tdp_recursos = txtRecursosCOC.Text
                         ,
                         tdp_intervencoesPedagogicas = txtIntervencoesPedagogicasCOC.Text
                         ,
                         tdp_registroIntervencoes = txtRegistroIntervencoesCOC.Text
                         ,
                         cur_id = VS_cur_id
                         ,
                         crr_id = VS_crr_id
                         ,
                         crp_id = VS_crp_id
                         ,
                         tdt_posicao = posicao
                         ,
                         tdp_situacao = 1
                         ,
                         IsNew = tdp_id <= 0
                     }).ToList()
                );
            }

            return lista;
        }

        #endregion

        #region Eventos

        protected void rptPeriodosAbas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));
                int tpc_ordem = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_ordem"));
                string cap_descricao = DataBinder.Eval(e.Item.DataItem, "cap_descricao").ToString();
                string tdp_diagnostico = DataBinder.Eval(e.Item.DataItem, "tdp_diagnostico").ToString();
                string tdp_planejamento = DataBinder.Eval(e.Item.DataItem, "tdp_planejamento").ToString();
                string tdp_recursos = DataBinder.Eval(e.Item.DataItem, "tdp_recursos").ToString();
                string tdp_intervencoesPedagogicas = DataBinder.Eval(e.Item.DataItem, "tdp_intervencoesPedagogicas").ToString();
                string tdp_registroIntervencoes = DataBinder.Eval(e.Item.DataItem, "tdp_registroIntervencoes").ToString();
                int tur_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tur_id"));
                byte tdt_posicao = String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tdt_posicao").ToString()) ? (byte)0 :
                                   Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tdt_posicao"));

                VS_fav_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "fav_id"));

                HtmlTable tbLegenda = (HtmlTable)e.Item.FindControl("tbLegenda");

                Repeater rptHabilidadesCOC = (Repeater)e.Item.FindControl("rptHabilidadesCOC");
                HtmlControl fdsHabilidadesCOC = (HtmlControl)e.Item.FindControl("fdsHabilidadesCOC");
                HtmlGenericControl spanOrientacao = (HtmlGenericControl)e.Item.FindControl("spanOrientacao");
                HtmlGenericControl spanHabilidadePlanej = (HtmlGenericControl)e.Item.FindControl("spanHabilidadePlanej");
                HtmlGenericControl spanHabilidadeTrab = (HtmlGenericControl)e.Item.FindControl("spanHabilidadeTrab");

                Label lblDiagnosticoCOC = (Label)e.Item.FindControl("lblDiagnosticoCOC");
                ImageButton btnTextoGrandeDiagnosticoCOC = (ImageButton)e.Item.FindControl("btnTextoGrandeDiagnosticoCOC");
                Label lblDiagnosticoCOCInfo = (Label)e.Item.FindControl("lblDiagnosticoCOCInfo");
                TextBox txtDiagnosticoCOC = (TextBox)e.Item.FindControl("txtDiagnosticoCOC");

                Label lblPlanejamentoCOC = (Label)e.Item.FindControl("lblPlanejamentoCOC");
                ImageButton btnTextoGrandePlanejamentoCOC = (ImageButton)e.Item.FindControl("btnTextoGrandePlanejamentoCOC");
                Label lblPlanejamentoCOCInfo = (Label)e.Item.FindControl("lblPlanejamentoCOCInfo");
                TextBox txtPlanejamentoCOC = (TextBox)e.Item.FindControl("txtPlanejamentoCOC");

                Label lblRecursosCOC = (Label)e.Item.FindControl("lblRecursosCOC");
                ImageButton btnTextoGrandeRecursosCOC = (ImageButton)e.Item.FindControl("btnTextoGrandeRecursosCOC");
                Label lblRecursosCOCInfo = (Label)e.Item.FindControl("lblRecursosCOCInfo");
                TextBox txtRecursosCOC = (TextBox)e.Item.FindControl("txtRecursosCOC");

                Label lblIntervencoesPedagogicasCOC = (Label)e.Item.FindControl("lblIntervencoesPedagogicasCOC");
                ImageButton btnTextoGrandeIntervencoesPedagogicasCOC = (ImageButton)e.Item.FindControl("btnTextoGrandeIntervencoesPedagogicasCOC");
                Label lblIntervencoesPedagogicasCOCInfo = (Label)e.Item.FindControl("lblIntervencoesPedagogicasCOCInfo");
                TextBox txtIntervencoesPedagogicasCOC = (TextBox)e.Item.FindControl("txtIntervencoesPedagogicasCOC");

                Label lblRegistroIntervencoesCOC = (Label)e.Item.FindControl("lblRegistroIntervencoesCOC");
                ImageButton btnTextoGrandeRegistroIntervencoesCOC = (ImageButton)e.Item.FindControl("btnTextoGrandeRegistroIntervencoesCOC");
                Label lblRegistroIntervencoesCOCInfo = (Label)e.Item.FindControl("lblRegistroIntervencoesCOCInfo");
                TextBox txtRegistroIntervencoesCOC = (TextBox)e.Item.FindControl("txtRegistroIntervencoesCOC");

                Label lblMsgMarcarHabilidades = (Label)e.Item.FindControl("lblMsgMarcarHabilidades");

                byte tipoLancamento = EntFormatoAvaliacao.fav_tipoLancamentoFrequencia;
                byte calculoQtdeAulasDadas = EntFormatoAvaliacao.fav_calculoQtdeAulasDadas;

                bool ultimoPeriodo = tpc_ordem == Tpc_ordemMax;

                txtDiagnosticoCOC.Text = tdp_diagnostico;
                txtPlanejamentoCOC.Text = tdp_planejamento;
                txtRecursosCOC.Text = tdp_recursos;
                txtIntervencoesPedagogicasCOC.Text = tdp_intervencoesPedagogicas;
                txtRegistroIntervencoesCOC.Text = tdp_registroIntervencoes;

                if ((spanOrientacao != null) && (!string.IsNullOrEmpty(NomeOrientacaoCurricularUltimoNivel)))
                {
                    spanOrientacao.InnerText = NomeOrientacaoCurricularUltimoNivel;
                }

                string NomePeriodoCalendario = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (!string.IsNullOrEmpty(NomeOrientacaoCurricularUltimoNivel))
                {
                    if (spanHabilidadePlanej != null)
                    {
                        spanHabilidadePlanej.InnerText = NomeOrientacaoCurricularUltimoNivel + " planejado(a) em " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " anterior";
                    }

                    if (spanHabilidadeTrab != null)
                    {
                        spanHabilidadeTrab.InnerText = NomeOrientacaoCurricularUltimoNivel + " trabalhado(a) em " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " anterior";
                    }

                    lblMsgMarcarHabilidades.Text = UtilBO.GetErroMessage("Marcar os(as) " + NomeOrientacaoCurricularUltimoNivelPlural.ToLower() + " planejados(as) a ser trabalhados(as) durante o " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ".", UtilBO.TipoMensagem.Informacao);
                }
                else 
                {
                    if (spanHabilidadePlanej != null)
                    {
                        spanHabilidadePlanej.InnerText = "Habilidades planejadas em " + NomePeriodoCalendario.ToLower() + " anteriores";
                    }

                    if (spanHabilidadeTrab != null)
                    {
                        spanHabilidadeTrab.InnerText = "Habilidades trabalhadas em " + NomePeriodoCalendario.ToLower() + " anteriores";
                    }
                }

                lblDiagnosticoCOC.Text = GetGlobalResourceObject("Mensagens", "MSG_AVALIACAOBIMESTRE") + " " + cap_descricao;

                lblPlanejamentoCOC.Text = ultimoPeriodo ?
                    GetGlobalResourceObject("Mensagens","MSG_REPLANEJAMENTOBIMESTREFINAL").ToString() :
                    GetGlobalResourceObject("Mensagens","MSG_REPLANEJAMENTOBIMESTRE").ToString() + cap_descricao + " para o " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " seguinte";

                lblRecursosCOC.Text = GetGlobalResourceObject("Mensagens","MSG_RECURSOSBIMESTRE") + " " + cap_descricao;

                lblIntervencoesPedagogicasCOC.Text = GetGlobalResourceObject("Mensagens","MSG_INTERVENCOESPEDAGOGICASBIMESTRE") + " do " + cap_descricao;

                lblRegistroIntervencoesCOC.Text = GetGlobalResourceObject("Mensagens","MSG_REGISTROINTERVENCOESBIMESTRE") + " do " + cap_descricao;

                btnTextoGrandeDiagnosticoCOC.OnClientClick = "abrirTextoGrande('" + lblDiagnosticoCOC.ClientID + "', '" + txtDiagnosticoCOC.ClientID + "'); return false;";

                string planejamento = ultimoPeriodo ?
                    Coc_ReplanejamentoFinal :
                    Coc_Replanejamento;

                //if (!string.IsNullOrEmpty(Coc_Avaliacao))
                //{
                // Altera a função para configurar a mensagem também no txt grande.
                btnTextoGrandeDiagnosticoCOC.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                        lblDiagnosticoCOC.ClientID,
                        txtDiagnosticoCOC.ClientID,
                        Coc_Avaliacao);

                btnTextoGrandePlanejamentoCOC.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                        lblPlanejamentoCOC.ClientID,
                        txtPlanejamentoCOC.ClientID,
                        Coc_Avaliacao);

                lblDiagnosticoCOCInfo.Text = Coc_Avaliacao;
                //}

                //if (!string.IsNullOrEmpty(planejamento))
                //{
                btnTextoGrandePlanejamentoCOC.OnClientClick =
                   String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                       lblPlanejamentoCOC.ClientID,
                       txtPlanejamentoCOC.ClientID,
                       planejamento);

                lblPlanejamentoCOCInfo.Text = planejamento;
                //}

                //if (!string.IsNullOrEmpty(Planejamento_Coc_Recursos))
                //{
                btnTextoGrandeRecursosCOC.OnClientClick =
                   String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                       lblRecursosCOC.ClientID,
                       txtRecursosCOC.ClientID,
                       Planejamento_Coc_Recursos);

                lblRecursosCOCInfo.Text = Planejamento_Coc_Recursos;
                //}


                btnTextoGrandeIntervencoesPedagogicasCOC.OnClientClick =
                   String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                       lblIntervencoesPedagogicasCOC.ClientID,
                       txtIntervencoesPedagogicasCOC.ClientID,
                       BimestreIntervencoesPedagogicas);

                lblIntervencoesPedagogicasCOCInfo.Text = BimestreIntervencoesPedagogicas;


                btnTextoGrandeRegistroIntervencoesCOC.OnClientClick =
                   String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}'); return false;",
                       lblRegistroIntervencoesCOC.ClientID,
                       txtRegistroIntervencoesCOC.ClientID,
                       BimestreRegistroIntervencoes);

                lblRegistroIntervencoesCOCInfo.Text = BimestreRegistroIntervencoes;

                if (tbLegenda != null)
                {
                    HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.OrientacaoCurricularPlanejada;
                    cell = tbLegenda.Rows[1].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.OrientacaoCurricularTrabalhada;
                }

                fdsHabilidadesCOC.Visible = false;

                if (rptHabilidadesCOC != null && VS_OrientacaoCurricular.Any())
                {
                    Nivel = 0;
                    rptHabilidadesCOC.DataSource = VS_OrientacaoCurricular.Where(item => item.tpc_id == tpc_id).First().dtOrientacaoCurricular;
                    rptHabilidadesCOC.DataBind();

                    fdsHabilidadesCOC.Visible = rptHabilidadesCOC.Items.Count > 0;

                    bool permiteEditar;

                    // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                    {
                        permiteEditar = false;
                    }
                    else
                    {
                        permiteEditar = ACA_EventoBO.SelecionaMaiorDataPorTipoPeriodoEscola(VS_esc_id, VS_uni_id, tpc_id, Tev_id).Date >= DateTime.Now.Date &&
                                             (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                                             ((VS_turmaDisciplinaCompartilhada == null && VS_ltPermissaoPlanejamentoAnual.Any(p => p.tdt_posicaoPermissao == (tdt_posicao > 0 ? tdt_posicao : PosicaoDocente) && p.pdc_permissaoEdicao)) 
                                             || (VS_turmaDisciplinaCompartilhada != null && !VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento)):
                                             __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
                    }

                    HabilitaControles(e.Item.Controls, permiteEditar);

                }
            }

        }

        protected void rptHabilidades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int nivelLinha = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Nivel"));
                bool permiteLancamento = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PermiteLancamento"));
                string ocr_codigo = DataBinder.Eval(e.Item.DataItem, "Codigo").ToString();
                string ocr_descricao = DataBinder.Eval(e.Item.DataItem, "Descricao").ToString();

                bool AlcanceEfetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlcanceEfetivado"));

                bool HouveModificacao = !Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "NI"));
                if (AlcanceEfetivado)
                    HouveModificacao = false;
                Image imgSituacaoEfetivada = (Image)e.Item.FindControl("imgSituacaoEfetivada");
                if (imgSituacaoEfetivada != null)
                    imgSituacaoEfetivada.Visible = AlcanceEfetivado;
                Image imgSituacao = (Image)e.Item.FindControl("imgSituacao");
                if (imgSituacao != null)
                    imgSituacao.Visible = HouveModificacao;

                Literal litCabecalho = (Literal)e.Item.FindControl("litCabecalho");
                Literal litConteudo = (Literal)e.Item.FindControl("litConteudo");
                HtmlControl divHabilidade = (HtmlControl)e.Item.FindControl("divHabilidade");
                Literal lblHabilidade = (Literal)e.Item.FindControl("lblHabilidade");

                Literal litRodape = (Literal)e.Item.FindControl("litRodape");

                string ul = String.Format("<ul class='treeview' style='display: {0};'>", nivelLinha == 1 ? "block" : "none");
                string li = nivelLinha == Nivel ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") :
                                                  (nivelLinha > Nivel ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") : "<li class='expandable'>");

                string cabecalho = nivelLinha == Nivel ? li :
                    (nivelLinha > Nivel ? ul + li : MultiplicaString("</li></ul>", Nivel - nivelLinha) + li);
                cabecalho += permiteLancamento ? string.Empty : "<div class='hitarea expandable-hitarea'></div>";
                litCabecalho.Text = cabecalho;


                lblHabilidade.Text = litConteudo.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                divHabilidade.Visible = permiteLancamento;
                litConteudo.Visible = !permiteLancamento;

                litRodape.Visible = permiteLancamento || (nivelLinha == Nivel);
                litRodape.Text = permiteLancamento ? "</li>" : string.Empty;
                litRodape.Visible = false;

                bool mostraLegenda = false;

                string chave = DataBinder.Eval(e.Item.DataItem, "Chave").ToString();

                CheckBox chkPlanejado = (CheckBox)e.Item.FindControl("chkPlanejado");
                CheckBox chkAlcancado = (CheckBox)e.Item.FindControl("chkAlcancado");
                CheckBox chkTrabalhado = (CheckBox)e.Item.FindControl("chkTrabalhado");
                ImageButton imgMarcarAlcancado = (ImageButton)e.Item.FindControl("imgMarcarAlcancado");

                HiddenField hdnChave = (HiddenField)e.Item.FindControl("hdnChave");

                #region Busca níveis de aprendizado da orientação curricular

                string[] idsChave = chave.Split(';');
                long ocrId = idsChave.Length > 1 ? Convert.ToInt64(idsChave[1]) : -1;
                long tpcId = idsChave.Length >= 2 ? Convert.ToInt64(idsChave[2]) : -1;
                if (ocrId > 0)
                {
                    var nivelAprendizado = from dr in dtOrientacaoNiveisAprendizado
                                           where dr.ocr_id == ocrId
                                           select new
                                           {
                                               nap_id = dr.nap_id
                                               ,
                                               nap_sigla = dr.nap_sigla.ToString()
                                               ,
                                               nap_descricao = dr.nap_descricao.ToString()
                                           };

                    if (nivelAprendizado.Any())
                    {
                        Repeater rptHabilidadesCOC = (Repeater)e.Item.NamingContainer;

                        if (rptHabilidadesCOC.NamingContainer is RepeaterItem)
                        {
                            RepeaterItem itemPeriodosAbas = (RepeaterItem)rptHabilidadesCOC.NamingContainer;
                            Repeater rptLegendaNivelAprendizado = (Repeater)itemPeriodosAbas.FindControl("rptLegendaNivelAprendizado");
                            HtmlControl divLegendaNivelAprendizado = (HtmlControl)itemPeriodosAbas.FindControl("divLegendaNivelAprendizado");

                            if (rptLegendaNivelAprendizado != null)
                            {
                                rptLegendaNivelAprendizado.DataSource = dtNivelArendizadoCurriculo;
                                rptLegendaNivelAprendizado.DataBind();

                                if (rptLegendaNivelAprendizado.Items.Count > 0)
                                {
                                    divLegendaNivelAprendizado.Visible = true;
                                    mostraLegenda = true;
                                }
                            }
                        }
                        else
                        {
                            rptLegendaNiveisAprend.DataSource = dtNivelArendizadoCurriculoAnterior;
                            rptLegendaNiveisAprend.DataBind();

                            if (dtNivelArendizadoCurriculo.Count() > 0)
                            {
                                divLegendaNivelApDiagIni.Visible = true;
                            }
                            else
                            {
                                divLegendaNivelApDiagIni.Visible = false;
                            }
                        }
                    }

                    string niveisSiglas = string.Empty;
                    string niveisLegenda = string.Empty;

                    foreach (var item in nivelAprendizado)
                    {
                        niveisSiglas += item.nap_sigla + " / ";
                        niveisLegenda += item.nap_sigla + " - " + item.nap_descricao + "<br>";
                    }

                    if (!string.IsNullOrEmpty(niveisSiglas))
                    {
                        niveisSiglas = niveisSiglas.Substring(0, niveisSiglas.Length - 3);

                        Label lblLegPlanej = (Label)e.Item.FindControl("lblLegendaPlanejado");
                        Label lblLegTrab = (Label)e.Item.FindControl("lblLegendaTrabalhado");
                        Label lblLegAlcan = (Label)e.Item.FindControl("lblLegendaAlcancado");

                        if (lblLegPlanej != null && lblLegTrab != null && lblLegAlcan != null)
                        {
                            lblLegPlanej.Visible = true;
                            lblLegTrab.Visible = true;
                            lblLegAlcan.Visible = true;

                            lblLegPlanej.Text = niveisSiglas;
                            lblLegTrab.Text = niveisSiglas;
                            lblLegAlcan.Text = niveisSiglas;

                            //lblLegendaNivelAprendizado.Text = niveisLegenda;
                            //mostraLegenda = true;
                            //divLegendaNivelAprendizado.Visible = true;
                        }

                        // Repeater Diagnostico inicial
                        Label lblLegDiagIni = (Label)e.Item.FindControl("lblLegendaDiagInicialOrientacao");

                        if (lblLegDiagIni != null)
                        {
                            lblLegDiagIni.Text = niveisSiglas;
                        }
                    }
                }

                #endregion

                //Verifica se pode desplanejar
                if (chkPlanejado != null && listHabilidadesComAulaPlanejada.Any(p => p.ocr_id == ocrId && p.tpc_id == tpcId))
                    chkPlanejado.CssClass += " OrientacaoPlanejadaAula ";

                if (permiteLancamento)
                {
                    if (imgMarcarAlcancado != null)
                    {
                        imgMarcarAlcancado.CssClass += " OrientacaoMarcarAlcancado ";
                    }

                    if (chkAlcancado != null)
                    {
                        chkAlcancado.CssClass += " OrientacaoAlcancada ";
                    }

                    if (chkPlanejado != null && chkTrabalhado != null)
                    {
                        chkPlanejado.CssClass += " OrientacaoPlanejada ";

                        chkTrabalhado.CssClass += " OrientacaoTrabalhada ";

                        if (!chkPlanejado.Checked)
                        {
                            chkTrabalhado.Enabled = false;
                            imgMarcarAlcancado.Enabled = false;
                        }
                        else if (!chkTrabalhado.Checked)
                        {
                            imgMarcarAlcancado.Enabled = false;
                        }
                    }

                    if (hdnChave != null)
                    {
                        hdnChave.Value = chave;
                    }
                }

                //divLegendaNivelAprendizado.Visible = mostraLegenda;

                Nivel = nivelLinha;
            }
        }

        protected void grvAlunos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool aha_alcancada = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "aha_alcancada"));

                CheckBox chkAlcancado = (CheckBox)e.Row.FindControl("chkAlcancado");

                chkAlcancado.Checked = !aha_alcancada;
                chkAlcancado.Enabled = PermiteEditar;

                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "mtd_situacao"));

                if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkEfetivado = (CheckBox)e.Row.FindControl("chkEfetivado");
                chkEfetivado.Enabled = PermiteEditar;
            }
        }

        protected void imgMarcarAlcancado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgMarcarAlcancado = (ImageButton)sender;
                RepeaterItem itemHabilidadeCOC = (RepeaterItem)imgMarcarAlcancado.NamingContainer;
                CheckBox chkPlanejado = (CheckBox)itemHabilidadeCOC.FindControl("chkPlanejado");
                CheckBox chkTrabalhado = (CheckBox)itemHabilidadeCOC.FindControl("chkTrabalhado");

                HiddenField hdnChave = (HiddenField)itemHabilidadeCOC.FindControl("hdnChave");
                string[] idsChave = hdnChave.Value.Split(';');

                VS_ocr_id = idsChave.Length > 1 ? Convert.ToInt64(idsChave[1]) : -1;
                VS_tpc_id = idsChave.Length > 2 ? Convert.ToInt32(idsChave[2]) : -1;

                if (chkPlanejado.Checked && chkTrabalhado.Checked)
                    CarregarLancamentoAlcance();
                else
                    lblMensagem.Text = UtilBO.GetErroMessage("Para lançar alcance para a habilidade é necessário que esteja marcada como planejada e trabalhada.", UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarAlcance_Click(object sender, EventArgs e)
        {
            try
            {
                List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> lista = CriarListaAlcance();
                if (lista != null && CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO.Salvar(lista))
                {
                    long ocr_id = grvAlunos.Rows.Cast<GridViewRow>().Select(p => Convert.ToInt32(grvAlunos.DataKeys[p.RowIndex].Values["ocr_id"])).First();
                    string msg = divLancamentoAlcance.Attributes["title"].ToString() + " realizado com sucesso.";
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tud_id: " + VS_tud_id + ", ocr_id: " + ocr_id);
                    lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharLancamentoAlcance", "$('.divLancamentoAlcance').dialog('close');", true);
                    bool efetivado = lista.Any(p => p.aha_efetivada);
                    bool modificado = lista.Any(p => !p.aha_alcancada);
                    if (efetivado)
                        modificado = false;
                    SetaVisibilidadeSituacaoAlcancado(efetivado, modificado);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                string msg = String.Format("Erro ao tentar salvar o {0}.", divLancamentoAlcance.Attributes["title"].ToString().ToLower());
                ApplicationWEB._GravaErro(ex);
                lblMensagemAlcance.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}