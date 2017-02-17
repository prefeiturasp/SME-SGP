using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Turma.TurmaEletivaAluno
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// ValidationGroup usado na tela.
        /// </summary>
        protected const string validationGroup = "TurmaEletivaAluno";

        #endregion Constantes

        #region Propriedades

        private DataTable dtDocentes;

        private DataTable DtVigenciasDocentes;

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

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tdt_id (ID da turmaDocente)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tdt_id
        {
            get
            {
                if (ViewState["VS_tdt_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tdt_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_tdt_id"] = value;
            }
        }

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

                DtVigenciasDocentes = TUR_TurmaDocenteBO.SelecionaVigenciasDocentesPorDisciplina(entTurma.tur_id);

                List<TUR_TurmaCurriculo> listaCurriculos = TUR_TurmaCurriculoBO.GetSelectBy_Turma(entTurma.tur_id, ApplicationWEB.AppMinutosCacheLongo);
                List<CadastroTurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectCadastradosBy_Turma(tur_id);

                if (!ValidaDadosTurma(entTurma, entEscola, listaCurriculos, listaDisciplinas))
                {
                    return;
                }

                uccFiltroEscola.Uad_ID = entEscola.uad_idSuperior;
                uccFiltroEscola_IndexChangedUA();

                uccFiltroEscola.SelectedValueEscolas = new[] { entTurma.esc_id, entTurma.uni_id };

                uccFiltroEscola_IndexChangedUnidadeEscola();

                int cur_id = listaCurriculos[0].cur_id;
                int crr_id = listaCurriculos[0].crr_id;

                uccCursoCurriculo.Valor = new[] { cur_id, crr_id };
                UCComboCursoCurriculo1_IndexChanged();

                uccCalendario.Valor = entTurma.cal_id;
                UCComboCalendario1_IndexChanged();

                VS_tud_id = listaDisciplinas[0].entTurmaDisciplina.tud_id;
                VS_tdt_id = listaDisciplinas[0].entTurmaDocente.tdt_id;

                uccDisciplina.Valor = listaDisciplinas[0].entTurmaDiscRelDisciplina.dis_id;

                UCComboDisciplina1_OnSelectedIndexChanged();

                uccFormatoAvaliacao.CarregarFormatoPorFormatoPadraoAtivo(entTurma.fav_id);
                uccFormatoAvaliacao.Valor = entTurma.fav_id;
                UCComboFormatoAvaliacao1_IndexChanged();

                txtCodigoTurma.Text = entTurma.tur_codigo;
                txtCodigoInep.Text = entTurma.tur_codigoInep;
                txtCapacidade.Text = entTurma.tur_vagas.ToString();
                txtMinimoMatriculados.Text = entTurma.tur_minimoMatriculados.ToString();

                // Carga horária semanal da disciplina.
                txtAulasSemanais.Text = listaDisciplinas[0].entTurmaDisciplina.tud_cargaHorariaSemanal.ToString();

                foreach (ListItem item in chkPeriodosCurso.Items)
                {
                    int crp_id = Convert.ToInt32(item.Value);

                    item.Selected = listaCurriculos.Exists(p => p.crp_id == crp_id);
                }

                foreach (ListItem item in chkPeriodosCalendario.Items)
                {
                    int tpc_id = Convert.ToInt32(item.Value);

                    item.Selected = listaDisciplinas[0].entTurmaCalendario.Exists(p => p.tpc_id == tpc_id);
                }

                ACA_Disciplina entDis = new ACA_Disciplina
                {
                    dis_id = listaDisciplinas[0].entTurmaDiscRelDisciplina.dis_id
                };
                ACA_DisciplinaBO.GetEntity(entDis);

                DataTable informacoesDocente =
                    ACA_DocenteBO.SelecionaPorColaboradorDocente(
                        listaDisciplinas[0].entTurmaDocente.col_id,
                        listaDisciplinas[0].entTurmaDocente.doc_id);

                bool bloqueioAtribuicao = false;

                divDocente.Visible = true;
                if (informacoesDocente.Rows.Count > 0)
                {
                    UCControleVigenciaDocentes.CarregarDocente(
                        informacoesDocente.Rows[0]["pes_nome"].ToString(),
                        1,
                        1,
                        VS_tud_id,
                        ref dtDocentes,
                        entDis.tds_id,
                        entTurma.esc_id,
                        entTurma.uni_id,
                        entTurma.tur_docenteEspecialista
                        , bloqueioAtribuicao
                        , ref DtVigenciasDocentes);
                }

                // Verifica se possui parametros de formacao
                MTR_ParametroFormacaoTurma entityFormacao = MTR_ParametroFormacaoTurmaBO.SelecionaParametroPorAnoCurso(cur_id, crr_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (entityFormacao != null)
                {
                    uccTurno.CarregarTurnoPorParametroPeriodo(entityFormacao);
                }
                else
                {
                    uccTurno.CarregarTurnoPorTurnoAtivo(entTurma.trn_id);
                }

                uccTurno.Valor = entTurma.trn_id;

                if (entTurma.tur_participaRodizio)
                {
                    chkRodizio.Checked = true;
                }

                if (entTurma.tur_situacao == (byte)TUR_TurmaSituacao.Aguardando)
                    ddlSituacao.Items.Add(new ListItem("Aguardando", ((byte)TUR_TurmaSituacao.Aguardando).ToString()));
                ddlSituacao.SelectedValue = entTurma.tur_situacao.ToString();

                DesabilitaDadosAlteracao(entityFormacao);

                RegistraScriptConfirmacao(entTurma);
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a turma de eletiva.", UtilBO.TipoMensagem.Erro);
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
        private void DesabilitaDadosAlteracao(MTR_ParametroFormacaoTurma entityFormacao)
        {
            uccFiltroEscola.PermiteAlterarCombos = false;
            uccCursoCurriculo.PermiteEditar = false;
            uccDisciplina.PermiteEditar = false;
            uccCalendario.PermiteEditar = false;
            
            // Verifica se possui parametros de formacao
            if (entityFormacao != null)
            {
                uccFormatoAvaliacao.PermiteEditar = false;
                txtCodigoTurma.Enabled = false;
            }
        }

        /// <summary>
        /// Valida dados da turma e permissão de usuário necessários para alteração.
        /// </summary>
        /// <param name="entTurma">Turma a ser validada</param>
        /// <param name="entEscola">Escola da turma</param>
        /// <param name="listaCurriculos">Curriculos da turma</param>
        /// <param name="listaDisciplinas">Disciplinas da turma</param>
        /// <returns></returns>
        private bool ValidaDadosTurma(TUR_Turma entTurma, ESC_Escola entEscola, List<TUR_TurmaCurriculo> listaCurriculos, List<CadastroTurmaDisciplina> listaDisciplinas)
        {
            // Verifica se usuário logado pertence à mesma entidade da turma, caso não seja, não é permitido editar.
            if (entEscola.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A turma não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Turma/TurmaEletivaAluno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            if (entTurma.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Não é permitido editar essa turma.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Turma/TurmaEletivaAluno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            if (listaCurriculos.Count == 0)
            {
                throw new ValidationException("A turma selecionada não possui nenhum currículo.");
            }

            if (listaDisciplinas.Count < 1)
            {
                throw new ValidationException("A turma não possui " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " cadastrados.");
            }

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

            foreach (ListItem item in chkPeriodosCurso.Items)
            {
                // Adicionar um TurmaCurriculo para cada item checado.
                if (item.Selected)
                {
                    TUR_TurmaCurriculo entTurCurriculo = new TUR_TurmaCurriculo
                    {
                        cur_id = uccCursoCurriculo.Valor[0],
                        crr_id = uccCursoCurriculo.Valor[1],
                        crp_id = Convert.ToInt32(item.Value),
                        tur_id = entTurma.tur_id,
                        tcr_prioridade = 1,
                        tcr_situacao = 1
                    };

                    listaCurriculos.Add(entTurCurriculo);
                }
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
                tud_nome = RetornaNomeDisciplina(),
                tud_situacao = (byte)TurmaDisciplinaSituacao.Ativo,
                tud_tipo = (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno,
                tud_modo = (byte)TurmaDisciplinaModo.Normal,
                tud_multiseriado = false,
                tud_id = VS_tud_id,
                IsNew = entTurma.IsNew
            };

            TUR_TurmaDisciplinaRelDisciplina entRelDis = new TUR_TurmaDisciplinaRelDisciplina
            {
                dis_id = uccDisciplina.Valor,
                tud_id = entTurmaDisciplina.tud_id
            };

            List<TUR_Turma_Docentes_Disciplina> listaDocentes = UCControleVigenciaDocentes.RetornaDadosGrid();
            listaDocentes.ForEach(p => p.entDocente.tdt_posicao = 1);

            List<TUR_TurmaDisciplinaCalendario> listaCalendario = new List<TUR_TurmaDisciplinaCalendario>();

            foreach (ListItem item in chkPeriodosCalendario.Items)
            {
                if (item.Selected)
                {
                    TUR_TurmaDisciplinaCalendario ent = new TUR_TurmaDisciplinaCalendario
                    {
                        tpc_id = Convert.ToInt32(item.Value),
                        tud_id = entTurmaDisciplina.tud_id
                    };

                    listaCalendario.Add(ent);
                }
            }

            CadastroTurmaDisciplina entCadTurmaDisciplina = new CadastroTurmaDisciplina
            {
                listaTurmaDocente = listaDocentes,
                entTurmaCalendario = listaCalendario,
                entTurmaDisciplina = entTurmaDisciplina,
                entTurmaDiscRelDisciplina = entRelDis
            };

            listaTurmaDisciplina.Add(entCadTurmaDisciplina);
            return listaTurmaDisciplina;
        }

        /// <summary>
        /// Retorna o nome da disciplina selecionada no combo.
        /// </summary>
        /// <returns></returns>
        private string RetornaNomeDisciplina()
        {
            ACA_Disciplina entDisciplina = new ACA_Disciplina { dis_id = uccDisciplina.Valor };
            ACA_DisciplinaBO.GetEntity(entDisciplina);
            return entDisciplina.dis_nome;
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

            uccCursoCurriculo.Obrigatorio = true;

            uccDisciplina.Texto = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " eletivo(a)";
            uccDisciplina.Obrigatorio = true;

            // Carregar formatos de avaliação ativos - todos os padrões (não são mais por escola).
            uccFormatoAvaliacao.CarregarFormatoPorFormatoPadraoAtivo(-1);

            // Carrega todos os turnos ativos no combo, não tem como buscar pelo controle de tempo.
            uccTurno.CarregarTurnoPorTurnoAtivo(-1);

            divDocente.Visible = false;
        }

        /// <summary>
        /// Verifica se existe um parâmetro de formação de turmas eletiva cadastrada.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="dis">ID da disciplina</param>
        private void CarregaDados_ParametroFormacao(int cur_id, int crr_id, ACA_Disciplina dis)
        {
            if (VS_tur_id <= 0)
            {
                MTR_ParametroFormacaoTurma pft = MTR_ParametroFormacaoTurmaBO.SelecionaParametroPorAnoCurso(cur_id, crr_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (pft != null && dis != null)
                {
                    if (ACA_TipoMacroCampoEletivaAlunoBO.SelecionaMacroCamposAssociado(dis.dis_id).Rows.Count > 0)
                    {
                        txtCodigoTurma.Text = TUR_TurmaBO.GerarCodigoTurmaEletiva(uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID, cur_id, crr_id, dis.dis_id, 1, pft, null);
                    }
                    else
                    {
                        txtCodigoTurma.Text = string.Empty;
                    }

                    uccCalendario.Valor = pft.cal_id;
                    uccCalendario.PermiteEditar = false;
                    UCComboCalendario1_IndexChanged();

                    uccFormatoAvaliacao.Valor = pft.fav_id;
                    uccFormatoAvaliacao.PermiteEditar = false;
                    UCComboFormatoAvaliacao1_IndexChanged();

                    txtCapacidade.Text = Convert.ToString(pft.pft_capacidade);
                    txtAulasSemanais.Text = Convert.ToString(pft.pft_cargaHorariaSemanal);

                    uccTurno.CarregarTurnoPorParametroPeriodo(pft);
                }
                else
                {
                    txtCodigoTurma.Text = txtCodigoInep.Text = string.Empty;

                    uccFormatoAvaliacao.Valor = -1;
                    UCComboFormatoAvaliacao1_IndexChanged();

                    txtCapacidade.Text = string.Empty;
                    txtAulasSemanais.Text = string.Empty;

                    uccTurno.CarregarTurnoPorTurnoAtivo(-1);
                }
            }
        }

        /// <summary>
        /// Seta os delegates.
        /// </summary>
        private void SetaDelegates()
        {
            uccFiltroEscola.IndexChangedUA += uccFiltroEscola_IndexChangedUA;
            uccFiltroEscola.IndexChangedUnidadeEscola += uccFiltroEscola_IndexChangedUnidadeEscola;

            uccCursoCurriculo.IndexChanged += UCComboCursoCurriculo1_IndexChanged;

            uccDisciplina.OnSelectedIndexChanged = UCComboDisciplina1_OnSelectedIndexChanged;

            uccCalendario.IndexChanged += UCComboCalendario1_IndexChanged;

            uccFormatoAvaliacao.IndexChanged += UCComboFormatoAvaliacao1_IndexChanged;
        }

        #endregion Métodos

        #region Delegates

        protected void UCComboCalendario1_IndexChanged()
        {
            try
            {
                if (uccCalendario.Valor != -1 && uccFormatoAvaliacao.Valor != -1)
                {
                    chkPeriodosCalendario.Items.Clear();
                    divPeriodosCalendario.Visible = true;

                    // De acorco com a Task 15303, não deverá aparecer a partir da quarta COC.
                    DataTable periodos = ACA_TipoPeriodoCalendarioBO.SelecionaCalendarioComAvaliacao(uccCalendario.Valor, uccFormatoAvaliacao.Valor);

                    lblSemPeriodoCalendario.Visible = periodos.Rows.Count == 0;

                    if (periodos.Rows.Count == 0)
                    {
                        lblSemPeriodoCalendario.Text = UtilBO.GetErroMessage("Não existem avaliações ligadas a um período no formato de avaliação selecionado.", UtilBO.TipoMensagem.Informacao);
                    }
                    else
                    {
                        foreach (DataRow row in periodos.Rows)
                        {
                            if (Convert.ToInt32(row["tpc_ordem"]) > 4)
                            {
                                row.Delete();
                            }
                        }

                        // Carregar períodos do calendário.
                        chkPeriodosCalendario.DataSource = periodos;
                        chkPeriodosCalendario.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboDisciplina1_OnSelectedIndexChanged()
        {
            try
            {
                int cur_id = uccCursoCurriculo.Valor[0];
                int crr_id = uccCursoCurriculo.Valor[1];
                int dis_id = uccDisciplina.Valor;

                divPeriodosCurso.Visible = dis_id > 0;

                divDocente.Visible = dis_id > 0;

                if ((cur_id > 0) && (crr_id > 0) && (dis_id > 0))
                {
                    // Carregar períodos do curso que oferecem a disciplina.
                    DataTable dt = ACA_CurriculoDisciplinaBO.SelecionaPeriodosPor_Escola_EletivaAluno(cur_id, crr_id, uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID, dis_id);
                    chkPeriodosCurso.DataSource = dt;
                    chkPeriodosCurso.DataBind();

                    ACA_Disciplina dis = new ACA_Disciplina { dis_id = dis_id };
                    ACA_DisciplinaBO.GetEntity(dis);

                    lblSemPeriodoCurso.Visible = dt.Rows.Count == 0;

                    if (dt.Rows.Count == 0)
                    {
                        lblSemPeriodoCurso.Text = UtilBO.GetErroMessage("Não foi encontrado nenhum(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " para a escola, " +
                                                                        GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                                                                        " e " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionados.", UtilBO.TipoMensagem.Informacao);
                    }

                    // Gera Codigo da turma
                    CarregaDados_ParametroFormacao(cur_id, crr_id, dis);

                    if (String.IsNullOrEmpty(txtAulasSemanais.Text))
                    {
                        txtAulasSemanais.Text = dis.dis_cargaHorariaTeorica.ToString();
                    }

                    bool bloqueioAtribuicao = false;

                    // Carrega os docentes no controle de vigência.
                    UCControleVigenciaDocentes.CarregarDocente(
                        string.Empty,
                        1,
                        1,
                        VS_tud_id,
                        ref dtDocentes,
                        dis.tds_id,
                        uccFiltroEscola.Esc_ID,
                        uccFiltroEscola.Uni_ID,
                        false,
                        bloqueioAtribuicao,
                        ref DtVigenciasDocentes);
                }
                else
                {
                    txtCodigoTurma.Text = string.Empty;
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                int cur_id = uccCursoCurriculo.Valor[0];
                int crr_id = uccCursoCurriculo.Valor[1];

                uccDisciplina.Valor = -1;
                uccCalendario.Valor = -1;

                if (cur_id > 0)
                {
                    // Carregar disciplinas do curso.
                    uccDisciplina.CarregarDisciplinasEletivasAlunoPeriodo
                        (cur_id, crr_id, uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID);

                    // uccDisciplina.CarregarDisciplinasEletivasAluno(cur_id, crr_id);
                    uccCalendario.CarregarCalendarioAnualPorCurso(cur_id);

                    // Verifica parâmetros de formação para criação de turmas eletiva.
                    CarregaDados_ParametroFormacao(cur_id, crr_id, null);
                }

                UCComboDisciplina1_OnSelectedIndexChanged();
                UCComboCalendario1_IndexChanged();
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
                uccCursoCurriculo.Valor = new[] { -1, -1 };

                if (uccFiltroEscola.Esc_ID > 0)
                {
                    int sitCurso = VS_tur_id > 0 ? 0 : 1;

                    // Carregar cursos da escola.
                    uccCursoCurriculo.CarregarCursoComDisciplinaEletiva(uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID, sitCurso);

                    if (uccTurno.Valor > 0)
                    {
                        uccCursoCurriculo.SetarFoco();
                    }
                }

                UCComboCursoCurriculo1_IndexChanged();
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

        protected void UCComboFormatoAvaliacao1_IndexChanged()
        {
            if ((uccCalendario.Valor != -1) && (uccFormatoAvaliacao.Valor != -1))
            {
                chkPeriodosCalendario.Items.Clear();
                divPeriodosCalendario.Visible = true;

                // De acorco com a Task 15303, não deverá aparecer a partir da quarta COC.
                DataTable periodos = ACA_TipoPeriodoCalendarioBO.SelecionaCalendarioComAvaliacao(uccCalendario.Valor, uccFormatoAvaliacao.Valor);

                lblSemPeriodoCalendario.Visible = periodos.Rows.Count == 0;

                if (periodos.Rows.Count == 0)
                {
                    lblSemPeriodoCalendario.Text = UtilBO.GetErroMessage("Não existem avaliações ligadas a um período no formato de avaliação selecionado.", UtilBO.TipoMensagem.Informacao);
                }
                else
                {
                    foreach (DataRow row in periodos.Rows)
                    {
                        if (Convert.ToInt32(row["tpc_ordem"]) > 4)
                        {
                            row.Delete();
                        }
                    }

                    // Carregar períodos do calendário.
                    chkPeriodosCalendario.DataSource = periodos;
                    chkPeriodosCalendario.DataBind();
                }
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

                    //Se é um cadastro de turma nova e o parâmetro de novas turmas "aguardando" estiver ativo então remove as opções de situação
                    if (VS_tur_id <= 0 &&
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.NOVAS_TURMAS_AGUARDANDO,
                                                                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        ddlSituacao.Items.Clear();
                        ddlSituacao.Items.Add(new ListItem("Aguardando", ((byte)TUR_TurmaSituacao.Aguardando).ToString()));
                    }

                    lblPeriodoCursos.Text = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";

                    UCControleVigenciaDocentes.PermiteEditar = false;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        
        protected void ValidarCapacidade_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int minCadastrados = Convert.ToInt32(args.Value);
            int totalCapacidade = string.IsNullOrEmpty(txtCapacidade.Text) ? 0 :
                Convert.ToInt32(txtCapacidade.Text);

            if (minCadastrados > 0)
            {
                args.IsValid = minCadastrados <= totalCapacidade;
            }
            else
            {
                args.IsValid = true;
            }
        }

        #endregion Eventos
    }
}