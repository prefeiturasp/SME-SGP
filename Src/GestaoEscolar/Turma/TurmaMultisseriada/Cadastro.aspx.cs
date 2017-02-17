using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.Combos;
using GestaoEscolar.WebControls.ControleVigenciaDocentes;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Turma.TurmaMultisseriada
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// ValidationGroup usado na tela.
        /// </summary>
        protected const string validationGroup = "TurmaMultisseriada";

        /// <summary>
        /// ValidationGroup usado na tela.
        /// </summary>
        protected const string validationGroupDisciplinas = "TurmaDisciplina";

        #endregion Constantes

        #region Propriedades

        // Propriedades para realizar o databound do repeater de docentes.
        private DataTable dtDocentes;

        private DataTable DtVigenciasDocentes;

        private DataTable dtDocentesEscola;

        private int tds_id;
        private int esc_id;
        private int uni_id;
        private bool bloqueioAtribuicao;
        private bool tur_docenteEspecialista;
        private bool buscaDocente;

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tur_id (ID da turma)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private long VS_tur_id
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
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tud_id (ID da turmaDisciplina)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private long VS_tud_id
        {
            get
            {
                if (ViewState["VS_tud_id"] != null)
                {
                    return Convert.ToInt64(ViewState["VS_tud_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        ///// <summary>
        ///// Propriedade em ViewState que armazena valor de tdt_id (ID da turmaDocente)
        ///// no caso de atualização de um registro ja existente.
        ///// </summary>
        //private int VS_tdt_id
        //{
        //    get
        //    {
        //        if (ViewState["VS_tdt_id"] != null)
        //        {
        //            return Convert.ToInt32(ViewState["VS_tdt_id"]);
        //        }

        //        return -1;
        //    }

        //    set
        //    {
        //        ViewState["VS_tdt_id"] = value;
        //    }
        //}

        /// <summary>
        /// Indica qual o método que chamou a confirmação padrão
        /// 1-Num Alunos Matriculados
        /// 2-Capacidade da turma
        /// </summary>
        public byte VS_ConfirmacaoPadrao
        {
            get
            {
                if (ViewState["VS_ConfirmacaoPadrao"] != null)
                {
                    return Convert.ToByte(ViewState["VS_ConfirmacaoPadrao"]);
                }

                return 0;
            }

            set
            {
                ViewState["VS_ConfirmacaoPadrao"] = value;
            }
        }

        /// <summary>
        /// Lista de curso, periodo e disciplina
        /// </summary>
        public List<ACA_CurriculoDisciplina> VS_ListaCurriculoDisciplina
        {
            get
            {
                if (ViewState["VS_ListaCurriculoDisciplina"] == null)
                    ViewState["VS_ListaCurriculoDisciplina"] = new List<ACA_CurriculoDisciplina>();
                return (List<ACA_CurriculoDisciplina>)ViewState["VS_ListaCurriculoDisciplina"];
            }
            set
            {
                ViewState["VS_ListaCurriculoDisciplina"] = value;
            }
        }

        public int? qtdeDocentes;

        public int QtdeDocentes
        {
            get
            {
                return Convert.ToInt32(qtdeDocentes ?? (qtdeDocentes = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA, __SessionWEB.__UsuarioWEB.Usuario.ent_id)));
            }
        }

        private List<TUR_Turma_Docentes_Disciplina> listTurmaDocentes;

        /// <summary>
        /// Lista de docentes na disciplina.
        /// </summary>
        private List<TUR_Turma_Docentes_Disciplina> ListaTurmaDocentes
        {
            get
            {
                return (List<TUR_Turma_Docentes_Disciplina>)(listTurmaDocentes ?? (listTurmaDocentes = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(VS_tud_id.ToString())));
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Carrega dados da turma na tela para alteração.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        private void CarregarDadosAlteracao(long tur_id)
        {
            try
            {
                VS_tur_id = tur_id;
                // Carregar entidades.
                TUR_Turma entTurma = new TUR_Turma { tur_id = tur_id };
                TUR_TurmaBO.GetEntity(entTurma);

                ESC_Escola entEscola = new ESC_Escola { esc_id = entTurma.esc_id };
                ESC_EscolaBO.GetEntity(entEscola);

                if (!ValidaDadosTurma(entTurma, entEscola))
                {
                    return;
                }

                uccFiltroEscola.Uad_ID = entEscola.uad_idSuperiorGestao;
                uccFiltroEscola_IndexChangedUA();
                uccFiltroEscola.SelectedValueEscolas = new[] { entTurma.esc_id, entTurma.uni_id };
                uccFiltroEscola_IndexChangedUnidadeEscola();
                uccCalendario.Valor = entTurma.cal_id;

                DtVigenciasDocentes = TUR_TurmaDocenteBO.SelecionaVigenciasDocentesPorDisciplina(tur_id);

                VS_ListaCurriculoDisciplina = new List<ACA_CurriculoDisciplina>();
                DataTable dt = TUR_TurmaDisciplinaBO.SelecionarTurmaDisciplina_CurriculoDisciplina_By_Turma(tur_id);
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    ACA_CurriculoDisciplina obj = new ACA_CurriculoDisciplina();
                    obj.dis_nome = dt.Rows[row]["dis_nome"].ToString();
                    obj.cur_nome = dt.Rows[row]["cur_nome"].ToString();
                    obj.crp_descricao = dt.Rows[row]["crp_descricao"].ToString();
                    obj.crp_id = (int)dt.Rows[row]["crp_id"];
                    obj.crr_id = (int)dt.Rows[row]["crr_id"];
                    obj.cur_id = (int)dt.Rows[row]["cur_id"];
                    obj.dis_id = (int)dt.Rows[row]["dis_id"];
                    obj.tds_id = (int)dt.Rows[row]["tds_id"];
                    obj.IsNew = false;
                    VS_ListaCurriculoDisciplina.Add(obj);
                }

                divDiciplinas.Visible = true;
                AdicionaVazio();
                grvDiciplinas.DataSource = VS_ListaCurriculoDisciplina;
                grvDiciplinas.DataBind();

                uccFormatoAvaliacao.CarregarFormatoPorFormatoPadraoAtivo(entTurma.fav_id);
                uccFormatoAvaliacao.Valor = entTurma.fav_id;

                txtCodigoTurma.Text = entTurma.tur_codigo;
                txtCodigoInep.Text = entTurma.tur_codigoInep;
                txtCapacidade.Text = entTurma.tur_vagas.ToString();

                if (dt.Rows.Count > 0)
                {
                    // Carga horária semanal da disciplina.
                    txtAulasSemanais.Text = dt.Rows[0]["tud_cargaHorariaSemanal"].ToString();

                    string nomeDocente = string.Empty;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["doc_id"].ToString()))
                    {
                        DataTable informacoesDocente =
                            ACA_DocenteBO.SelecionaPorColaboradorDocente(
                                Convert.ToInt64(dt.Rows[0]["col_id"]),
                                Convert.ToInt64(dt.Rows[0]["doc_id"]));
                        nomeDocente = informacoesDocente.Rows[0]["pes_nome"].ToString();
                    }

                    VS_tud_id = Convert.ToInt64(dt.Rows[0]["tud_id"]);
                    //VS_tdt_id = (int)dt.Rows[0]["tdt_id"];
                    divDocente.Visible = true;

                    dtDocentes = new DataTable();
                    dtDocentes.Columns.Add("posicao");
                    dtDocentes.Columns.Add("tud_id");
                    dtDocentes.Columns.Add("qtdedocentes");
                    dtDocentes.Columns.Add("tds_id");

                    for (int i = 1; i <= QtdeDocentes; i++)
                    {
                        DataRow dr = dtDocentes.NewRow();
                        dr["posicao"] = i;
                        dr["tud_id"] = VS_tud_id;
                        dr["qtdedocentes"] = QtdeDocentes;
                        dr["tds_id"] = Convert.ToInt32(dt.Rows[0]["tds_id"]);
                        dtDocentes.Rows.Add(dr);
                    }

                    bloqueioAtribuicao = false;
                    esc_id = entTurma.esc_id;
                    uni_id = entTurma.uni_id;
                    tds_id = Convert.ToInt32(dt.Rows[0]["tds_id"]);
                    buscaDocente = true;
                    tur_docenteEspecialista = entTurma.tur_docenteEspecialista;

                    rptDocentes.DataSource = dtDocentes;
                    rptDocentes.DataBind();
                }

                uccTurno.Valor = entTurma.trn_id;

                if (entTurma.tur_participaRodizio)
                {
                    chkRodizio.Checked = true;
                }

                ddlSituacao.SelectedValue = entTurma.tur_situacao.ToString();

                DesabilitaDadosAlteracao();

                RegistraScriptConfirmacao(entTurma);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a turma multisseriada.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Registra scripts necessários para colocar a mensagem de validação no botão salvar só
        /// quando alterar o combo de situação para 5-Encerrada.
        /// </summary>
        /// <param name="entTurma">Entidade da turma que está sendo editada</param>
        private void RegistraScriptConfirmacao(TUR_Turma entTurma)
        {
            if (entTurma == null)
            {
                try
                {
                    // Recuperar entidade da turma.
                    entTurma = new TUR_Turma
                    {
                        tur_id = VS_tur_id
                    };
                    TUR_TurmaBO.GetEntity(entTurma);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
            {
                if ((!entTurma.IsNew) && (entTurma.tur_situacao == (byte)TUR_TurmaSituacao.Ativo))
                {
                    ScriptManager sm = ScriptManager.GetCurrent(this);
                    if (sm != null)
                    {
                        string script = "var idDdlSituacao = '#" + ddlSituacao.ClientID + "';";

                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ids", script, true);
                    }
                }
            }
        }

        /// <summary>
        /// Desabilita campos para alteração de acordo com as regras da tela.
        /// </summary>
        /// <param name="entityFormacao">Entidade MTR_ParametroFormacaoTurma.</param>
        private void DesabilitaDadosAlteracao()
        {
            uccFiltroEscola.PermiteAlterarCombos = false;
            uccCalendario.PermiteEditar = false;
        }

        /// <summary>
        /// Valida dados da turma e permissão de usuário necessários para alteração.
        /// </summary>
        /// <param name="entTurma">Turma a ser validada</param>
        /// <param name="entEscola">Escola da turma</param>
        /// <param name="listaCurriculos">Curriculos da turma</param>
        /// <param name="listaDisciplinas">Disciplinas da turma</param>
        /// <returns></returns>
        private bool ValidaDadosTurma(TUR_Turma entTurma, ESC_Escola entEscola)
        {
            // Verifica se usuário logado pertence à mesma entidade da turma, caso não seja, não é permitido editar.
            if (entEscola.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A turma não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Turma/TurmaMultisseriada/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            if (entTurma.tur_tipo != (byte)TUR_TurmaTipo.Multisseriada)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Não é permitido editar essa turma.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Turma/TurmaMultisseriada/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            //if (VS_ListaCurriculoDisciplina.Count < 1)
            //{
            //    throw new Exception("A turma (tur_id: " + entTurma.tur_id +
            //                        ") não possui [MSG_DISCIPLINAS] cadastrados(as).");
            //}

            return true;
        }
        
        /// <summary>
        /// Retorna a lista de TurmaCurriculo que será necessária para salvar.
        /// </summary>
        /// <param name="entTurma">Entidade da turma que será salva</param>
        /// <returns></returns>
        private List<TUR_TurmaCurriculo> RetornaListaCurriculos(TUR_Turma entTurma)
        {
            List<TUR_TurmaCurriculo> listaCurriculos = new List<TUR_TurmaCurriculo>();

            foreach (ACA_CurriculoDisciplina item in VS_ListaCurriculoDisciplina.Where(p => p.dis_id > 0))
            {
                TUR_TurmaCurriculo entTurCurriculo = new TUR_TurmaCurriculo
                {
                    cur_id = item.cur_id,
                    crr_id = item.crr_id,
                    crp_id = item.crp_id,
                    tur_id = entTurma.tur_id,
                    tcr_prioridade = 1,
                    tcr_situacao = 1,
                    IsNew = item.IsNew
                };

                listaCurriculos.Add(entTurCurriculo);
            }
            return listaCurriculos;
        }

        /// <summary>
        /// Retorna a lista com a estrutura necessária para salvar a TurmaDisciplina.
        /// </summary>
        /// <param name="entTurma">Entidade da turma que será salva</param>
        /// <returns></returns>
        private List<CadastroTurmaDisciplina> RetornaTurmaDisciplina(TUR_Turma entTurma)
        {
            List<CadastroTurmaDisciplina> listaTurmaDisciplina = new List<CadastroTurmaDisciplina>();

            TUR_TurmaDisciplina entTurmaDisciplina = new TUR_TurmaDisciplina
            {
                tud_codigo = entTurma.tur_codigo,
                tud_vagas = entTurma.tur_vagas,
                tud_minimoMatriculados = entTurma.tur_minimoMatriculados,
                tud_duracao = entTurma.tur_duracao,
                tud_cargaHorariaSemanal = Convert.ToInt32(txtAulasSemanais.Text),
                tud_aulaForaPeriodoNormal = false,
                tud_global = false,
                tud_nome = VS_ListaCurriculoDisciplina[0].dis_nome,
                tud_situacao = (byte)TurmaDisciplinaSituacao.Ativo,
                tud_tipo = (byte)TurmaDisciplinaTipo.Multisseriada,
                tud_modo = (byte)TurmaDisciplinaModo.Normal,
                tud_multiseriado = false,
                tud_id = VS_tud_id,
                IsNew = entTurma.IsNew
            };

            List<TUR_TurmaDisciplinaRelDisciplina> lstEntTurmaDiscRelDisciplina = new List<TUR_TurmaDisciplinaRelDisciplina>();
            foreach (ACA_CurriculoDisciplina item in VS_ListaCurriculoDisciplina.Where(p => p.dis_id > 0))
            {
                TUR_TurmaDisciplinaRelDisciplina entRelDis = new TUR_TurmaDisciplinaRelDisciplina
                {
                    dis_id = item.dis_id,
                    tud_id = entTurmaDisciplina.tud_id
                };
                lstEntTurmaDiscRelDisciplina.Add(entRelDis);
            }

            List<TUR_Turma_Docentes_Disciplina> docentes = (from RepeaterItem item in rptDocentes.Items
                                                            let UCControleVigenciaDocentes = (ControleVigenciaDocentes)item.FindControl("UCControleVigenciaDocentes")
                                                            let posicao = Convert.ToByte(((Label)item.FindControl("lblposicao")).Text)
                                                            from TUR_Turma_Docentes_Disciplina turmadocente in UCControleVigenciaDocentes.RetornaDadosGrid()
                                                            let entityDocente = turmadocente.entDocente
                                                            select new TUR_Turma_Docentes_Disciplina
                                                            {
                                                                doc_nome = turmadocente.doc_nome
                                                                ,
                                                                indice = turmadocente.indice
                                                                ,
                                                                entDocente = new TUR_TurmaDocente
                                                                {
                                                                    doc_id = entityDocente.doc_id,
                                                                    col_id = entityDocente.col_id,
                                                                    crg_id = entityDocente.crg_id,
                                                                    coc_id = entityDocente.coc_id,
                                                                    tdt_vigenciaInicio = entityDocente.tdt_vigenciaInicio,
                                                                    tdt_vigenciaFim = entityDocente.tdt_vigenciaFim,
                                                                    tdt_id = entityDocente.tdt_id,
                                                                    tdt_tipo = entityDocente.tdt_tipo,
                                                                    tdt_posicao = posicao
                                                                }
                                                            }).ToList();

            List<TUR_TurmaDisciplinaCalendario> listaCalendario = new List<TUR_TurmaDisciplinaCalendario>();

            CadastroTurmaDisciplina entCadTurmaDisciplina = new CadastroTurmaDisciplina
            {
                listaTurmaDocente = docentes,
                entTurmaCalendario = listaCalendario,
                entTurmaDisciplina = entTurmaDisciplina,
                listaEntTurmaDiscRelDisciplina = lstEntTurmaDiscRelDisciplina
            };

            listaTurmaDisciplina.Add(entCadTurmaDisciplina);
            return listaTurmaDisciplina;
        }

        /// <summary>
        /// Configura valores iniciais nos user controls da tela.
        /// </summary>
        private void InicializarUserControls()
        {
            uccFiltroEscola.ObrigatorioEscola = true;
            uccFiltroEscola.ObrigatorioUA = true;
            uccFiltroEscola.FiltroEscolasControladas = true;
            uccFiltroEscola.MostraApenasAtivas = true;
            uccFiltroEscola.Inicializar();

            // Carregar formatos de avaliação ativos - todos os padrões (não são mais por escola).
            uccFormatoAvaliacao.CarregarFormatoPorFormatoPadraoAtivo(-1);

            // Carrega todos os turnos ativos no combo, não tem como buscar pelo controle de tempo.
            uccTurno.CarregarTurnoPorTurnoAtivo(-1);

            divDocente.Visible = false;
        }

        /// <summary>
        /// Atualiza os dados do grid conforme o list passado.
        /// </summary>
        /// <param name="list"></param>
        private void AtualizaGrid()
        {
            grvDiciplinas.DataSource = VS_ListaCurriculoDisciplina;
            grvDiciplinas.DataBind();
        }

        /// <summary>
        /// Adiciona disciplina vazia
        /// </summary>
        private void AdicionaVazio()
        {
            ACA_CurriculoDisciplina obj = new ACA_CurriculoDisciplina { IsNew = true };
            VS_ListaCurriculoDisciplina.Add(obj);
        }

        /// <summary>
        /// Valida as diciplinas.
        /// </summary>
        private bool ValidarDiciplinas(ACA_CurriculoDisciplina obj)
        {
            if (VS_ListaCurriculoDisciplina.Count > 1 && obj.tds_id != VS_ListaCurriculoDisciplina[0].tds_id)
            {
                lblMessageDisciplina.Text = UtilBO.GetErroMessage("Todos os itens precisam ser do mesmo tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + ".", UtilBO.TipoMensagem.Alerta);
                return false;
            }
            if (VS_ListaCurriculoDisciplina.Count(p => p.cur_id == obj.cur_id && p.crr_id == obj.crr_id
                                               && p.crp_id == obj.crp_id && p.dis_id == obj.dis_id) > 0)
            {
                lblMessageDisciplina.Text = UtilBO.GetErroMessage("Já existe o(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " para o(a) " +
                                                                  GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " no(a) " +
                                                                  GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " selecionado(a).", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Seta os delegates.
        /// </summary>
        private void SetaDelegates()
        {
            uccFiltroEscola.IndexChangedUA += uccFiltroEscola_IndexChangedUA;
            uccFiltroEscola.IndexChangedUnidadeEscola += uccFiltroEscola_IndexChangedUnidadeEscola;
            uccCalendario.IndexChanged += UCComboCalendario1_IndexChanged;
        }

        #endregion Métodos

        #region Delegates

        protected void UCComboCalendario1_IndexChanged()
        {
            try
            {
                if (uccCalendario.Valor != -1)
                {
                    divDiciplinas.Visible = true;
                    VS_ListaCurriculoDisciplina = new List<ACA_CurriculoDisciplina>();
                    AdicionaVazio();
                    AtualizaGrid();
                }
                else
                {
                    divDiciplinas.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void uccFiltroEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                uccCalendario.Valor = -1;

                if (uccFiltroEscola.Esc_ID > 0)
                {
                    // Carregar calendário da escola.
                    uccCalendario.CarregarCalendarioAnualRelCurso_EscId(uccFiltroEscola.Esc_ID);
                }
                else
                {
                    divDiciplinas.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void uccFiltroEscola_IndexChangedUA()
        {
            try
            {
                uccFiltroEscola_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }
        
        #endregion Delegates

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            SetaDelegates();

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tools.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroTurmaEletiva.js"));

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "exitConfirm", "SetExitPageConfirmer();", true);

                    if (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnCancelar.CssClass += " btnMensagemUnload";
                    }
                }
            }

            if (!IsPostBack)
            {
                try
                {
                    InicializarUserControls();

                    if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                    {
                        uccFiltroEscola.FiltroEscolasControladas = null;
                        uccFiltroEscola.MostraApenasAtivas = false;
                        CarregarDadosAlteracao(PreviousPage.Edit_tur_id);
                    }

                    foreach (RepeaterItem item in rptDocentes.Items)
                    {
                        ControleVigenciaDocentes UCControleVigenciaDocentes = (ControleVigenciaDocentes)item.FindControl("UCControleVigenciaDocentes");
                        if (UCControleVigenciaDocentes != null)
                            UCControleVigenciaDocentes.PermiteEditar = false;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            lblLegendDisciplinas.Text = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString();
            grvDiciplinas.Columns[0].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grvDiciplinas.Columns[1].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grvDiciplinas.Columns[2].HeaderText = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString();
            summaryDisciplina.ValidationGroup = validationGroupDisciplinas;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        
        protected void grvDiciplinas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isnew = (bool)grvDiciplinas.DataKeys[e.Row.RowIndex].Value;
                
                WebControls_Combos_UCComboCursoCurriculo uccCursoCurriculo =
                    (WebControls_Combos_UCComboCursoCurriculo)e.Row.FindControl("uccCursoCurriculo");
                WebControls_Combos_UCComboCurriculoPeriodo uccCurriculoPeriodo =
                    (WebControls_Combos_UCComboCurriculoPeriodo)e.Row.FindControl("uccCurriculoPeriodo");
                UCComboDisciplina uccDisciplina =
                    (UCComboDisciplina)e.Row.FindControl("uccDisciplina");

                Label lblCursoCurriculo = (Label)e.Row.FindControl("lblCursoCurriculo");
                if (lblCursoCurriculo != null)
                {
                    lblCursoCurriculo.Visible = !isnew;
                }
                Label lblCurriculoPeriodo = (Label)e.Row.FindControl("lblCurriculoPeriodo");
                if (lblCurriculoPeriodo != null)
                {
                    lblCurriculoPeriodo.Visible = !isnew;
                }
                Label lblDisciplina = (Label)e.Row.FindControl("lblDisciplina");
                if (lblDisciplina != null)
                {
                    lblDisciplina.Visible = !isnew;
                }

                if (uccCursoCurriculo != null)
                {
                    if (isnew)
                    {
                        uccCursoCurriculo.Obrigatorio = true;
                        uccCursoCurriculo.ValidationGroup = validationGroupDisciplinas;
                        uccCursoCurriculo.LabelVisible = false;
                        uccCursoCurriculo.CarregarCursoCurriculoPorEscolaCalendario(uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID, 1, uccCalendario.Valor);
                    }
                    uccCursoCurriculo.Visible = isnew;
                }

                if (uccCurriculoPeriodo != null)
                {
                    if (isnew)
                    {
                        uccCurriculoPeriodo.Obrigatorio = true;
                        uccCurriculoPeriodo.ValidationGroup = validationGroupDisciplinas;
                        uccCurriculoPeriodo.LabelVisible = false;
                    }
                    uccCurriculoPeriodo.Visible = isnew;
                }

                if (uccDisciplina != null)
                {
                    if (isnew)
                    {
                        uccDisciplina.Obrigatorio = true;
                        uccDisciplina.ValidationGroup = validationGroupDisciplinas;
                        uccDisciplina.LabelVisible = false;
                    }
                    uccDisciplina.Visible = isnew;
                }
            }
        }
        
        protected void grvDiciplinas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            grvDiciplinas.PageIndex = e.NewSelectedIndex;
            AtualizaGrid();
        }

        protected void rptDocentes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            byte posicao = Convert.ToByte(((Label)e.Item.FindControl("lblposicao")).Text);

            ControleVigenciaDocentes UCControleVigenciaDocentes = (ControleVigenciaDocentes)e.Item.FindControl("UCControleVigenciaDocentes");

            string doc_nome = buscaDocente ?
                ListaTurmaDocentes.Find(p => (p.entDocente.tud_id == VS_tud_id && p.entDocente.tdt_posicao == posicao && p.entDocente.tdt_situacao == 1)).doc_nome :
                string.Empty;

            UCControleVigenciaDocentes.CarregarDocente
                (doc_nome, posicao, QtdeDocentes, VS_tud_id, ref dtDocentesEscola, tds_id
                , esc_id, uni_id, tur_docenteEspecialista, bloqueioAtribuicao, ref DtVigenciasDocentes);
        }

        #endregion Eventos
    }
}