using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class Busca : MotherPageLogadoCompressedViewState
    {
        #region Estrutura

        /// <summary>
        /// Estrutura que armazena e agrupa os grids de turma por escola e calendário.
        /// </summary>
        public struct sGridTurmaEscolaCalendario
        {
            public GridView gridTurma { get; set; }
            public Guid uad_idSuperior { get; set; }
            public int esc_id { get; set; }
            public int uni_id { get; set; }
            public int cal_id { get; set; }
            public int cal_ano { get; set; }
        }

        #endregion Estrutura

        #region Constantes

        private const int grvTurma_ColunaTurmaDoc = 0;
        private const int grvTurma_ColunaTurmaAdmin = 1;
        private const int grvHistorico_ColunaDocenciaCompartilhada = 2;
        private const int grvHistoricoTurmasExtintas_ColunaIndicadores = 2;
        private const int grvHistoricoTurmasInativas_ColunaIndicadores = 2;
        private const int grvTurma_ColunaListao = 7;
        private const int grvTurma_ColunaFrequencia = 8;
        private const int grvTurma_ColunaAvaliacao = 9;
        private const int grvTurma_ColunaEfetivacao = 10;
        private const int grvTurma_ColunaAlunos = 11;
        private const int grvPeriodosAulas_ColunaSugestao = 2;
        private const int grvPeriodosAulas_ColunaAplicarSugestao = 3;
        private const int grvPeriodosAulas_ColunaAulasCriadas = 5;
        private const int grvPendencias_ColunaDisciplina = 2;

        #endregion Constantes

        #region Propriedades

        private string periodosEfetivados;

        private int VS_rptTurmasIndice
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_rptTurmasIndice"] ?? -1);
            }

            set
            {
                ViewState["VS_rptTurmasIndice"] = value;
            }
        }

        private bool VS_titular
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_titular"] ?? false);
            }

            set
            {
                ViewState["VS_titular"] = value;
            }
        }

        private bool VS_semDados
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_semDados"] ?? false);
            }

            set
            {
                ViewState["VS_semDados"] = value;
            }
        }

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        private GridView Edit_grvTurma
        {
            get
            {
                return (GridView)(rptTurmas.Items[VS_rptTurmasIndice].FindControl("grvTurma"));
            }
        }

        public int Edit_esc_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["esc_id"] ?? -1);
            }
        }

        public int Edit_uni_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["uni_id"] ?? -1);
            }
        }

        public long Edit_tur_id
        {
            get
            {
                return Convert.ToInt64(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_id"] ?? -1);
            }
        }

        public string Edit_escola
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_escolaUnidade"].ToString();
            }
        }

        public string Edit_tur_codigo
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_codigo"].ToString();
            }
        }

        public string Edit_tud_nome
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_nome"].ToString();
            }
        }

        public int Edit_cal_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["cal_id"] ?? -1);
            }
        }

        public long Edit_tud_id
        {
            get
            {
                return Convert.ToInt64(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_id"] ?? -1);
            }
        }

        public byte Edit_tdt_posicao
        {
            get
            {
                return Convert.ToByte(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tdt_posicao"] ?? 0);
            }
        }

        public bool Edit_tud_naoLancarNota
        {
            get
            {
                return Convert.ToBoolean(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_naoLancarNota"] ?? false);
            }
        }

        public bool Edit_tud_naoLancarFrequencia
        {
            get
            {
                return Convert.ToBoolean(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_naoLancarFrequencia"] ?? false);
            }
        }

        public bool Edit_tud_disciplinaEspecial
        {
            get
            {
                return Convert.ToBoolean(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_disciplinaEspecial"] ?? false);
            }
        }

        public string Edit_tciIds
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tciIds"].ToString();
            }
        }

        public DateTime Edit_tur_dataEncerramento
        {
            get
            {
                return Convert.ToDateTime(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_dataEncerramento"] ?? new DateTime());
            }
        }

        public string PaginaRetorno
        {
            get
            {
                return "~/Academico/ControleTurma/Busca.aspx";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                    }
                }

                return SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Lista com o histórico do docente.
        /// </summary>
        private List<sHistoricoDocente> VS_ltHistoricoDocente
        {
            get
            {
                return (List<sHistoricoDocente>)(ViewState["VS_ltHistoricoDocente"]);
            }
            set
            {
                ViewState["VS_ltHistoricoDocente"] = value;
            }
        }

        /// <summary>
        /// Indica os ids na disciplina selecionada, e foi bloqueado o redirecionamento à
        /// tela do diário, por faltar o lançamento das aulas previstas.
        /// </summary>
        private long[] VS_ChavesRedirecionaDiario
        {
            get
            {
                if (ViewState["VS_ChavesRedirecionaDiario"] == null)
                {
                    return new long[] { -1, -1 };
                }

                return (long[])ViewState["VS_ChavesRedirecionaDiario"];
            }
            set
            {
                ViewState["VS_ChavesRedirecionaDiario"] = value;
            }
        }

        /// <summary>
        /// Verifica se o usuário logado pode salvar os dados das aulas previstas
        /// </summary>
        private bool VS_permiteSalvarAulasPrevistas
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_permiteSalvarAulasPrevistas"] ?? true);
            }

            set
            {
                ViewState["VS_permiteSalvarAulasPrevistas"] = value;
            }
        }

        private int totalPrevistas = 0, totalDadas = 0, totalRepostas = 0, totalSugestao = 0;
        private bool possuiAulasCriadas = false;

        private byte tdt_posicao;

        /// <summary>
        /// Guarda a tela que precisa redirecionar.
        /// </summary>
        private string VS_TelaRedirecionar
        {
            get
            {
                if (ViewState["VS_TelaRedirecionar"] == null)
                    return "";

                return ViewState["VS_TelaRedirecionar"].ToString();
            }
            set
            {
                ViewState["VS_TelaRedirecionar"] = value;
            }
        }

        /// <summary>
        /// Parâmetro que indica se o fechamento será pré carregado no cache.
        /// </summary>
        private bool PreCarregarFechamentoCache
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private long tur_idAula;
        private int cal_idAula;
        private bool mostraSalvar;

        /// <summary>
        /// Informa se o período já foi fechado (evento de fechamento já acabou) e não há nenhum evento de fechamento por vir.
        /// Se o período ainda estiver ativo então não verifica o evento de fechamento
        /// </summary>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cap_dataFim">Data fim do período</param>
        /// <returns></returns>
        private bool VS_PeriodoEfetivado(int tpc_id, int cal_id, long tur_id, DateTime cap_dataFim)
        {
            bool efetivado = false;

            //Se o bimestre está ativo ou nem começou então não bloqueia pelo evento de fechamento
            if (DateTime.Today <= cap_dataFim)
                return false;

            List<ACA_Evento> lstEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false);

            //Só permite editar o bimestre se tiver evento ativo
            efetivado = !lstEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                                DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

            return efetivado;
        }

        /// <summary>
        /// ViewState que armazena a lista de pendências das disciplina.
        /// </summary>
        private Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>> VS_listaPendencias
        {
            get
            {
                return (Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>>)(ViewState["VS_listaPendencias"] ?? (ViewState["VS_listaPendencias"] = new Dictionary<string, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>>()));
            }

            set
            {
                ViewState["VS_listaPendencias"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a lista de disciplinas com divergência entre aulas criadas e aulas previstas.
        /// </summary>
        private List<long> VS_listaDivergenciasAulasPrevistas
        {
            get
            {
                if (ViewState["VS_listaDivergenciasAulasPrevistas"] == null)
                {
                    ViewState["VS_listaDivergenciasAulasPrevistas"] = new List<long>();
                }
                return (List<long>)ViewState["VS_listaDivergenciasAulasPrevistas"];
            }

            set
            {
                ViewState["VS_listaDivergenciasAulasPrevistas"] = value;
            }
        }

        private string tudIds;

        List<sTipoPeriodoCalendario> lstPeriodosCalendario;

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                UCComboUAEscola1.FiltroEscolasControladas = true;
                UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }
                //else
                //{
                //    // Limpa o combo de cursos - carrega todos.ss
                //    UCComboCursoCurriculo1.Valor = new[] { -1, -1, -1 };
                //    UCComboCursoCurriculo1.Carregar();
                //}

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCurriculoPeriodo1.PermiteEditar = false;
                UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCCCalendario1.PermiteEditar = false;
                UCCCalendario1.Valor = -1;

                if (UCComboUAEscola1.Esc_ID > 0)
                {
                    UCCCalendario1.CarregarCalendarioAnual();
                    UCCCalendario1.PermiteEditar = true;
                    UCCCalendario1.SetarFoco();
                }

                UCCCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo calendario
        /// </summary>
        private void UCCCalendario1_IndexChanged()
        {
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCComboCursoCurriculo1.PermiteEditar = false;
            UCComboCursoCurriculo1.Valor = new[] { -1, -1 };

            if (UCComboUAEscola1.Esc_ID > 0 && UCCCalendario1.Valor > 0)
            {
                UCComboCursoCurriculo1.PermiteEditar = true;
                // Carregar todos os cursos, não só ativos, para exibir turmas encerradas na busca.
                UCComboCursoCurriculo1.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCCCalendario1.Valor, 0);
                UCComboCursoCurriculo1.SetarFoco();
            }
            //else
            //{
            //    UCComboCursoCurriculo1.Carregar();
            //}

            UCComboCursoCurriculo1_IndexChanged();
        }

        /// <summary>
        /// Evento change do combo de curso curriculo
        /// </summary>
        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                if (UCComboCursoCurriculo1.Valor[0] > 0)
                {
                    // carrego o ciclo
                    UCComboTipoCiclo.CarregarCicloPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    if (UCComboTipoCiclo.ddlCombo.Items.Count > 0)
                    {
                        UCComboTipoCiclo.Visible = true;
                        UCComboTipoCiclo.Enabled = true;
                        UCComboTipoCiclo.ddlCombo.Focus();

                        UCComboTipoCiclo_IndexChanged();
                    }
                    else
                    {
                        UCCCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                        UCCCurriculoPeriodo1.PermiteEditar = true;
                        UCCCurriculoPeriodo1.SetarFoco();
                    }
                }
                else
                {
                    UCComboTipoCiclo.Visible = false;
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de tipo de ciclo
        /// </summary>
        private void UCComboTipoCiclo_IndexChanged()
        {
            try
            {
                if (UCComboTipoCiclo.Valor > 0)
                {
                    UCCCurriculoPeriodo1.CarregarPorCursoCurriculoTipoCiclo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], UCComboTipoCiclo.Valor);
                    UCCCurriculoPeriodo1.PermiteEditar = true;
                    UCCCurriculoPeriodo1.SetarFoco();
                }
                else
                {
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de tipo de docente
        /// </summary>
        private void UCComboTipoDocente1_IndexChanged()
        {
            try
            {
                if (UCComboTipoDocente1.Valor > 0)
                {
                    string[] valor = uccTurmaDisciplina.Valor.Split(';');

                    if (valor.Length > 2)
                    {
                        long tur_id = Convert.ToInt64(valor[0]);
                        long tud_id = Convert.ToInt64(valor[1]);
                        int cal_id = Convert.ToInt32(valor[2]);
                        byte tdt_posicao = Convert.ToByte(UCComboTipoDocente1.Valor);

                        tur_idAula = tur_id;
                        cal_idAula = cal_id;

                        totalPrevistas = totalDadas = totalRepostas = totalSugestao = 0;
                        possuiAulasCriadas = false;

                        mostraSalvar = false;
                        periodosEfetivados = "";
                        grvPeriodosAulas.DataSource = ACA_CalendarioPeriodoBO.Seleciona_QtdeAulas_TurmaDiscplina(tur_id, tud_id, cal_id, tdt_posicao, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                        grvPeriodosAulas.DataBind();
                        btnSalvar.Visible = mostraSalvar;

                        if (!string.IsNullOrEmpty(periodosEfetivados))
                        {
                            lblPeriodoEfetivado.Visible = true;
                            lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.AulasPrevistas.MensagemEfetivado").ToString(),
                                                                             UtilBO.TipoMensagem.Informacao);
                        }

                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaSugestao].Visible = totalSugestao > 0;
                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaAulasCriadas].Visible = possuiAulasCriadas;
                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaSugestao].Visible = grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaAplicarSugestao].Visible = totalSugestao > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnIndicadores.Update();
            }
        }

        /// <summary>
        /// Evento change do combo de turma disciplina
        /// </summary>
        private void uccTurmaDisciplina_IndexChanged()
        {
            try
            {
                string[] valor = uccTurmaDisciplina.Valor.Split(';');

                if (valor.Length > 2)
                {
                    long tur_id = Convert.ToInt64(valor[0]);
                    long tud_id = Convert.ToInt64(valor[1]);
                    int cal_id = Convert.ToInt32(valor[2]);
                    byte tdt_posicaoLocal = 0;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        tdt_posicaoLocal = Convert.ToByte(valor[3]);
                    }
                    else if (UCComboTipoDocente1.Valor != -1)
                    {
                        tdt_posicaoLocal = Convert.ToByte(UCComboTipoDocente1.Valor);
                    }

                    if (tdt_posicaoLocal > 0)
                    {
                        tur_idAula = tur_id;
                        cal_idAula = cal_id;

                        totalPrevistas = totalDadas = totalRepostas = totalSugestao = 0;
                        possuiAulasCriadas = false;
                        mostraSalvar = false;
                        grvPeriodosAulas.DataSource = ACA_CalendarioPeriodoBO.Seleciona_QtdeAulas_TurmaDiscplina(tur_id, tud_id, cal_id, tdt_posicaoLocal, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                        tdt_posicao = tdt_posicao == 0 ? tdt_posicaoLocal : tdt_posicao;
                        grvPeriodosAulas.DataBind();
                        btnSalvar.Visible = mostraSalvar;

                        if (!string.IsNullOrEmpty(periodosEfetivados))
                        {
                            lblPeriodoEfetivado.Visible = true;
                            lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.AulasPrevistas.MensagemEfetivado").ToString(),
                                                                             UtilBO.TipoMensagem.Informacao);
                        }

                        if (VS_ChavesRedirecionaDiario.Length > 0 && VS_ChavesRedirecionaDiario[0] > 0)
                        {
                            // Se está alimentada a propriedade de chaves para redirecionar pro Diário de classe, altera
                            // quando o usuário muda o combo.
                            VS_ChavesRedirecionaDiario = new long[] { tud_id, tdt_posicaoLocal };
                        }

                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaSugestao].Visible = totalSugestao > 0;
                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaAulasCriadas].Visible = possuiAulasCriadas;
                        grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaSugestao].Visible = grvPeriodosAulas.Columns[grvPeriodosAulas_ColunaAplicarSugestao].Visible = totalSugestao > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemIndicador.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnIndicadores.Update();
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
        /// realizada.
        /// </summary>
        /// <param name="filtroEscolas"></param>
        private void SelecionarEscola(bool filtroEscolas)
        {
            if (filtroEscolas)
                UCComboUAEscola1_IndexChangedUA();

            string esc_id, uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Atualiza o grid de acordo com a quantidade de paginação
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // Atribui nova quantidade de itens por página para o grid.
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                grvTurmas.PageIndex = 0;
                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola da poupup Histórico de Turmas
        /// </summary>
        private void UCComboUAEscola2_IndexChangedUnidadeEscola()
        {
            try
            {
                hdnTabSelecionado.Value = "";
                if (UCComboUAEscola2.Esc_ID > 0)
                {
                    CarregaHistoricoDocente(UCComboUAEscola2.Esc_ID, string.Empty);
                }
                else
                {
                    divMostrarAbas.Visible = false;
                    btnAtribuirTurma.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Page life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaControleTurma.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    VS_semDados = false;
                    string message = __SessionWEB.PostMessages;

                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;

                    if (VS_visaoDocente)
                    {
                        List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);

                        // Guarda em uma variável as escolas que possuem alguma turma ativa
                        var dadosEscolasAtivas = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo)).ToList();

                        if (dadosEscolasAtivas.Count == 0)
                        {  // se o docente não possuir nenhuma turma - exibir a mensagem informativa
                            lblMensagem.Text = UtilBO.GetErroMessage((String)GetGlobalResourceObject("Mensagens", "MSG_ATRIBUICAODOCENTES"), UtilBO.TipoMensagem.Informacao);
                            lblMensagem1.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemTurma").ToString(),
                                                                      UtilBO.TipoMensagem.Alerta);
                            VS_semDados = true;
                        }

                        VS_listaDivergenciasAulasPrevistas = new List<long>();

                        //VS_Dados = dados;
                        rptTurmas.DataSource = dadosEscolasAtivas;
                        rptTurmas.DataBind();

                        VS_titular = dados.Exists(p => p.Turmas.Any(t => t.tdc_id == (int)EnumTipoDocente.Titular));
                        btnGerarAula.Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ESCONDER_BOTAO_GERAR_AULAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        if (btnGerarAula.Visible)
                        {   // se for true eu testo novamente para verificar se tem tipo docencia diferente de substituto para exibir o botão
                            btnGerarAula.Visible = dados.Exists(p => p.Turmas.Exists(x => x.tdc_id != (byte)EnumTipoDocente.Substituto));
                        }

                        divFiltros.Visible = false;
                        divAgenda.Visible = true;
                        divResultadoDocente.Visible = true;
                    }
                    else
                    {
                        VS_titular = false;
                        divFiltros.Visible = true;
                        divAgenda.Visible = false;
                        divResultadoDocente.Visible = false;
                        grvTurmas.PageSize = ApplicationWEB._Paginacao;

                        #region Inicializar

                        UCComboUAEscola1.FocusUA();
                        UCComboUAEscola1.Inicializar();

                        //UCComboCursoCurriculo1.Carregar();
                        UCComboTipoCiclo.Carregar();
                        UCComboTipoCiclo.SelectedValue = "-1";

                        this.VerificarBusca();

                        #endregion Inicializar

                        Page.Form.DefaultButton = btnPesquisar.UniqueID;
                        Page.Form.DefaultFocus = UCComboUAEscola1.VisibleUA ? UCComboUAEscola1.ComboUA_ClientID : UCComboUAEscola1.ComboEscola_ClientID;
                        //__SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                        //Response.Redirect("~/Index.aspx", false);
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    hdnProcessarFilaFechamentoTela.Value = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ? "true" : "false";
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (VS_semDados && VS_visaoDocente)
            {
                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);

                // Guarda em uma variável as escolas que possuem alguma turma ativa
                var dadosEscolasAtivas = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo)).ToList();

                if (dadosEscolasAtivas.Count == 0)
                {  // se o docente não possuir nenhuma turma - exibir a mensagem informativa
                    lblMensagem.Text = UtilBO.GetErroMessage((String)GetGlobalResourceObject("Mensagens", "MSG_ATRIBUICAODOCENTES"), UtilBO.TipoMensagem.Informacao);
                    lblMensagem1.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemTurma").ToString(),
                                                                UtilBO.TipoMensagem.Alerta);
                }
            }

            if (VS_visaoDocente && VS_titular)
            {
                string mensagemDocente = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_ALERTA_DOCENTE_MINHAS_TURMAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (!string.IsNullOrEmpty(mensagemDocente))
                {
                    lblMensagemAlertaDocente.Text = UtilBO.GetErroMessage(mensagemDocente, UtilBO.TipoMensagem.Alerta);
                }
            }

            #region Associando Delegates

            UCComboUAEscola1.IndexChangedUA += UCComboUAEscola1_IndexChangedUA;
            UCComboUAEscola1.IndexChangedUnidadeEscola += UCComboUAEscola1_IndexChangedUnidadeEscola;
            UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            uccTurmaDisciplina.IndexChanged += uccTurmaDisciplina_IndexChanged;
            UCComboTipoDocente1.IndexChanged += UCComboTipoDocente1_IndexChanged;
            UCComboTipoCiclo.IndexChanged += UCComboTipoCiclo_IndexChanged;
            UCComboUAEscola2.IndexChangedUnidadeEscola += UCComboUAEscola2_IndexChangedUnidadeEscola;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;

            #endregion Associando Delegates
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
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
            }
        }

        #endregion Page life Cycle

        #region Métodos

        /// <summary>
        /// Adiciona uma classe css ao um controle da página.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void AddClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Add(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Remove uma classe css ao um controle da página.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void RemoveClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Remove(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Verifica se um controle possui uma classe css.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private bool HasClass(WebControl control, string cssClass)
        {
            List<string> classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return classes.Exists(p => p.Equals(cssClass));
        }

        /// <summary>
        /// Formata string para retornar dados para o cabeçalho por escola e calendário.
        /// </summary>
        /// <param name="escola">Dados escola.</param>
        /// <param name="calendario">Dados calendário.</param>
        /// <returns></returns>
        public string RetornaCabecalho(string escola, string calendario)
        {
            return String.Format("{0}<br />{1}", escola, calendario);
        }

        /// <summary>
        /// Retorna lista de períodos de um calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário anual.</param>
        /// <returns></returns>
        public List<ACA_CalendarioPeriodo> RetornaPeriodo(int cal_id)
        {
            return ACA_CalendarioPeriodoBO.SelecionaPeriodoPorCalendarioEntidade(cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToList();
        }

        /// <summary>
        /// Realiza a pesquisa mediante aos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                mensagemPendenciaFechamentoMinhasTurmas.Visible = false;

                odsMinhasTurmas.SelectParameters.Clear();
                odsMinhasTurmas.SelectParameters.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                odsMinhasTurmas.SelectParameters.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());
                odsMinhasTurmas.SelectParameters.Add("cal_id", UCCCalendario1.Valor.ToString());
                odsMinhasTurmas.SelectParameters.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
                odsMinhasTurmas.SelectParameters.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
                odsMinhasTurmas.SelectParameters.Add("crp_id", UCCCurriculoPeriodo1.Valor[2].ToString());
                odsMinhasTurmas.SelectParameters.Add("ent_id", ent_id.ToString());
                odsMinhasTurmas.SelectParameters.Add("tur_codigo", txtCodigoTurma.Text);
                odsMinhasTurmas.SelectParameters.Add("appMinutosCacheCurto", ApplicationWEB.AppMinutosCacheCurto.ToString());
                odsMinhasTurmas.SelectParameters.Add("tci_id", UCComboTipoCiclo.Tci_id.ToString());

                grvTurmas.PageIndex = 0;
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                divResultadoVisaoSuperior.Visible = true;

                // Limpar a ordenação realizada.
                grvTurmas.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsMinhasTurmas.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                if (UCComboUAEscola1.FiltroEscola)
                    filtros.Add("ua_superior", UCComboUAEscola1.Uad_ID.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.MinhasTurmas, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                VS_listaDivergenciasAulasPrevistas = new List<long>();
                tudIds = string.Empty;

                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas)
            {
                string valor1;
                string valor2;
                string valor3;
                string esc_id;
                string uni_id;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                if (!string.IsNullOrEmpty(valor1))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor1);
                    SelecionarEscola(UCComboUAEscola1.FiltroEscola);
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }
                else if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                {
                    UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor1);
                UCCCalendario1.Valor = Convert.ToInt32(valor1);
                UCCCalendario1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                UCComboCursoCurriculo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                UCComboCursoCurriculo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tci_id", out valor1);
                UCComboTipoCiclo.Tci_id = Convert.ToInt32(valor1);
                UCComboTipoCiclo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                if (Convert.ToInt32(valor3) > 0)
                    UCCCurriculoPeriodo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2), Convert.ToInt32(valor3) };

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor1);
                txtCodigoTurma.Text = valor1;
                txtCodigoTurma.Focus();

                Pesquisar();
            }
            else
            {
                UCComboUAEscola1_IndexChangedUnidadeEscola();
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
        /// Carrega histórico das turmas do docente
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="esc_nome"></param>
        private void CarregaHistoricoDocente(int esc_id, string esc_nome)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                bool mostrarAbas = true;

                //Busca apenas as turmas que não estão ativas ou as ativas que possuem vigência na disciplina encerrada
                VS_ltHistoricoDocente = TUR_TurmaBO.SelecionaHistoricoPorDocenteControleTurma(esc_id, doc_id, ent_id)
                                        .Where(p => p.tur_situacao != (byte)TUR_TurmaSituacao.Ativo ||
                                               ((p.tur_situacao == (byte)TUR_TurmaSituacao.Ativo) &&
                                                (p.tdt_situacao == (byte)TUR_TurmaDocenteSituacao.Inativo || !((p.tdt_vigenciaFim != new DateTime() && p.tdt_vigenciaInicio <= DateTime.Now.Date && p.tdt_vigenciaFim >= DateTime.Now.Date) ||
                                                  (p.tdt_vigenciaFim == new DateTime() && p.tdt_vigenciaInicio <= DateTime.Now.Date))))).ToList();

                var ciclos = VS_ltHistoricoDocente.Where(y => y.tur_situacao == (byte)TUR_TurmaSituacao.Ativo).GroupBy(x => x.tci_id).Select(g => g.First()).ToList();

                if (ciclos.Count<sHistoricoDocente>() >= 0)
                {
                    rptCiclosAbas.DataSource = ciclos;
                    rptCiclos.DataSource = ciclos;
                    rptCiclos.DataBind();
                    rptCiclosAbas.DataBind();
                }

                // Carrega o combo de anos anteriores
                List<sHistoricoDocente> anos = VS_ltHistoricoDocente.Where(x => x.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada)
                                                                            .GroupBy(x => x.cal_ano)
                                                                            .Select(g => new sHistoricoDocente { cal_ano = g.Key })
                                                                            .ToList<sHistoricoDocente>();
                anos.OrderByDescending(g => g.cal_ano);
                ddlAnoInativos.DataSource = anos;
                ddlAnoInativos.DataValueField = ddlAnoInativos.DataTextField = "cal_ano";
                ddlAnoInativos.DataBind();

                // turmas de anos anteriores
                var historicoTurmasAnosAnteriores = VS_ltHistoricoDocente.Where(x => x.tur_situacao ==
                    (byte)TUR_TurmaSituacao.Encerrada && Convert.ToInt32(ddlAnoInativos.Items[0].Value) == x.cal_ano).ToList();
                grvHistoricoTurmasInativas.DataSource = historicoTurmasAnosAnteriores;
                grvHistoricoTurmasInativas.DataBind();

                // turmas extintas
                var historicoTurmasExtintas = VS_ltHistoricoDocente.Where(x => x.tur_situacao ==
                    (byte)TUR_TurmaSituacao.Extinta).ToList();
                grvHistoricoTurmasExtintas.DataSource = historicoTurmasExtintas;
                grvHistoricoTurmasExtintas.DataBind();


                // Se estiver exibindo turmas de anos anteriores (encerradas), não exibir a coluna de indicadores.
                // [Carla 25/02/2015] Alteração feita para esconder para as turmas encerradas de 2014, pois não exibem 
                // os bimestres na popup, porque as turams tem data de encerramento = 01/01/2014.
                //grvHistoricoTurmasExtintas.Columns[grvHistoricoTurmasExtintas_ColunaIndicadores].Visible = 
                //    !ApplicationWEB.ExibirTurmasAnosAnterioresDocente;
                grvHistoricoTurmasInativas.Columns[grvHistoricoTurmasInativas_ColunaIndicadores].Visible = false;

                liTurmasInat.Visible = fdsTurmasInativas.Visible = grvHistoricoTurmasInativas.Rows.Count > 0;
                liTurmasEx.Visible = fdsTurmasExtintas.Visible = grvHistoricoTurmasExtintas.Rows.Count > 0;

                //if (grvHistoricoTurmasInativas.Rows.Count > 0)
                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocenciaInat", "$(document).ready(function() { $('#liTurmasInat').css('display','block'); });", true);
                //else
                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocenciaInat", "$(document).ready(function() { $('#liTurmasInat').css('display','none'); });", true);

                //if (grvHistoricoTurmasExtintas.Rows.Count > 0)
                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocenciaExt", "$(document).ready(function() { $('#liTurmasEx').css('display','block'); });", true);
                //else
                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocenciaExt", "$(document).ready(function() { $('#liTurmasEx').css('display','none'); });", true);

                // Se não tiver histórico e nenhuma turma extinta
                if (rptCiclosAbas.Items.Count == 0 && grvHistoricoTurmasExtintas.Rows.Count == 0 && grvHistoricoTurmasInativas.Rows.Count == 0)
                {
                    mostrarAbas = false;
                    lblMensagemHistorico.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemHistorico").ToString(),
                                                                      UtilBO.TipoMensagem.Alerta);
                }

                divMostrarAbas.Visible = mostrarAbas;

                if (mostrarAbas)
                {
                    // Retirar o botão 'Atribuir nova turma' para as escolas que o docente não possui atribuição.
                    sHistoricoDocente historico = new sHistoricoDocente();
                    historico = VS_ltHistoricoDocente.Find(p => p.tud_situacao == 1
                                        && ((p.tdt_vigenciaFim != new DateTime() && p.tdt_vigenciaInicio <= DateTime.Now.Date && p.tdt_vigenciaFim >= DateTime.Now.Date)
                                        || (p.tdt_vigenciaFim == new DateTime() && p.tdt_vigenciaInicio <= DateTime.Now.Date)) && p.tdt_situacao == (byte)TUR_TurmaDocenteSituacao.Ativo);
                    if (historico.esc_id <= 0)
                    {
                        btnAtribuirTurma.Visible = false;
                    }
                    else
                    {
                        btnAtribuirTurma.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Redireciona pro Diário de classe, criando as variáveis de sessão necessárias.
        /// </summary>
        /// <param name="grid"></param>
        private void RedirecionaDiarioClasse(GridView grid)
        {
            // Cria variáveis na sessão
            Session["tud_id"] = grid.DataKeys[grid.EditIndex].Values["tud_id"].ToString();
            Session["tdt_posicao"] = grid.DataKeys[grid.EditIndex].Values["tdt_posicao"].ToString();
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

            bool disciplinaEspecial = Convert.ToBoolean(grid.DataKeys[grid.EditIndex].Values["tud_disciplinaEspecial"].ToString());

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ||
                (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 && !disciplinaEspecial))
            {
                Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionarTipoDocencia", "$(document).ready(function() { $('#divSelecionaTipoDocencia').dialog('open'); });", true);
            }
        }

        /// <summary>
        /// Verifica se houve o lançamento das aulas previstas, se não houve e precisa, abre o popup para salvar os dados
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdt_posicaoLocal"></param>
        /// <param name="esc_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="tud_tipo"></param>
        /// <returns></returns>
        private bool VerificaLancamentoAulasPrevistas(long tud_id, byte tdt_posicaoLocal, int esc_id, long tur_id, int cal_id, byte tud_tipo, bool docenteAtivo = true)
        {
            if (tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada &&
                tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia)
            {
                DateTime dataLimiteLancamento = new DateTime();
                string dataBloqueio = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.DATA_VALIDADE_BLOQUEIO_ACESSO_MINHAS_TURMAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (!string.IsNullOrEmpty(dataBloqueio))
                {
                    if (!DateTime.TryParse(dataBloqueio, out dataLimiteLancamento))
                        throw new ValidationException("A data de bloqueio informada no parâmetro não é válida.");
                }

                if (DateTime.Today >= dataLimiteLancamento && VS_visaoDocente &&
                        ((tdt_posicaoLocal == (byte)EnumTipoDocente.Titular) ||
                            (tdt_posicaoLocal == (byte)EnumTipoDocente.SegundoTitular) ||
                            (tdt_posicaoLocal == (byte)EnumTipoDocente.Especial)))
                {
                    if (!TUR_TurmaDisciplinaAulaPrevistaBO.VerificaLancamento(tud_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, tur_id, cal_id))
                    {
                        string mensagemInfoBloqueio = GetGlobalResourceObject("Academico", "ControleTurma.Busca.lblMensagemIndicador.MensagemBloqueioMinhasTurmas").ToString();
                        if (mensagemInfoBloqueio.Length > 0)
                        {
                            lblMensagemBloqueio.Text = UtilBO.GetErroMessage(mensagemInfoBloqueio, UtilBO.TipoMensagem.Informacao);
                            lblMensagemBloqueio.Visible = true;
                        }
                        else
                        {
                            lblMensagemBloqueio.Visible = false;
                        }

                        tdt_posicao = tdt_posicaoLocal;

                        // Se clicou na turma e está bloqueando o acesso à tela, porque já passou a data, após salvar, ele redireciona pro diário.
                        VS_ChavesRedirecionaDiario = new long[] { tud_id, tdt_posicao };

                        CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, true, 0, docenteAtivo);

                        uccTurmaDisciplina.PermiteEditar = false;

                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno(long tud_id, int esc_id, int uni_id, long tur_id, bool tud_naoLancarNota, bool tud_naoLancarFrequencia, int cal_id, string EscolaTurmaDisciplina, int tdt_posicao, DateTime tur_dataEncerramento, string tciIds, byte tur_tipo, long tud_idAluno, long tur_idNormal, byte tipoPendencia, int tpcIdPendencia, long tudIdPendencia)
        {
            Session.Remove("tud_id");
            Session.Remove("tdt_posicao");
            Session.Remove("PaginaRetorno");
            Session.Remove("TudIdCompartilhada");

            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            byte opcaoAba = Convert.ToByte(eOpcaoAbaMinhasTurmas.DiarioClasse);

            List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
            Struct_CalendarioPeriodos periodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();

            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Tud_idRetorno_ControleTurma", tud_id.ToString());
            listaDados.Add("Edit_tdt_posicao", tdt_posicao.ToString());
            listaDados.Add("Edit_esc_id", esc_id.ToString());
            listaDados.Add("Edit_uni_id", uni_id.ToString());
            listaDados.Add("Edit_tur_id", tur_id.ToString());
            listaDados.Add("Edit_tud_naoLancarNota", tud_naoLancarNota.ToString());
            listaDados.Add("Edit_tud_naoLancarFrequencia", tud_naoLancarFrequencia.ToString());
            listaDados.Add("Edit_tur_dataEncerramento", tur_dataEncerramento.ToString());
            listaDados.Add("Edit_tpc_id", periodo.tpc_id.ToString());
            listaDados.Add("Edit_tpc_ordem", periodo.tpc_ordem.ToString());
            listaDados.Add("Edit_cal_id", cal_id.ToString());
            listaDados.Add("TextoTurmas", EscolaTurmaDisciplina);
            listaDados.Add("OpcaoAbaAtual", opcaoAba.ToString());
            listaDados.Add("Edit_tciIds", tciIds);
            listaDados.Add("Edit_tur_tipo", tur_tipo.ToString());
            listaDados.Add("Edit_tud_idAluno", tud_idAluno.ToString());
            listaDados.Add("Edit_tur_idNormal", tur_idNormal.ToString());
            listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/Busca.aspx");

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
            Session["Historico"] = false;

            if (tipoPendencia > 0)
            {
                Session["tipoPendencia"] = tipoPendencia;
                Session["tpcIdPendencia"] = tpcIdPendencia;
                Session["tudIdPendencia"] = tudIdPendencia;
            }
            else
            {
                Session.Remove("tipoPendencia");
                Session.Remove("tpcIdPendencia");
                Session.Remove("tudIdPendencia");
            }
        }

        /// <summary>
        /// Colocar a turma/disciplina selecionada no historico na sessão.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdt_posicao"></param>
        private void CarregaSessionHistorico(string tud_id, string tdt_posicao)
        {
            Session.Remove("DadosPaginaRetorno");
            Session.Remove("VS_DadosTurmas");
            Session.Remove("TudIdCompartilhada");

            // Cria variáveis na sessão
            Session["tud_id"] = tud_id;
            Session["tdt_posicao"] = tdt_posicao;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";
            Session["Historico"] = true;
        }

        /// <summary>
        /// Redireciona para a página informada dentro da pasta Controle de Turmas.
        /// </summary>
        /// <param name="pagina">Página</param>
        private void RedirecionaTela(string pagina)
        {
            Response.Redirect("~/Academico/ControleTurma/" + pagina + ".aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redireciona para a tela de atribuição de docentes.
        /// </summary>
        private void RedirecionaTelaAtribuicaoDocente()
        {
            Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoDocentes/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// metodo que carrega as aulas previstas para a disciplina
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="turmasAtivas"></param>
        /// <param name="situacaoTurmaNaoAtiva"></param>
        /// <param name="docenteAtivo"></param>
        private void CarregaAulasPrevistas(int esc_id, long tur_id, long tud_id, int cal_id, Guid ent_id, bool turmasAtivas, byte situacaoTurmaNaoAtiva = 0, bool docenteAtivo = true)
        {
            long docente = __SessionWEB.__UsuarioWEB.Docente.doc_id;

            if (docente == 0)
            {
                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorFiltrosMinhasTurmas(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCComboCursoCurriculo1.Valor[0], UCCCurriculoPeriodo1.Valor[1], UCCCurriculoPeriodo1.Valor[2], ent_id, txtCodigoTurma.Text, UCComboTipoCiclo.Tci_id, ApplicationWEB.AppMinutosCacheCurto);

                var dadosTurmas = dados.Find(p => p.esc_id == esc_id).Turmas;

                var turmaSelecionada = dadosTurmas.Where(p => p.tud_id == tud_id && (tdt_posicao <= 0 || p.tdt_posicao == tdt_posicao)).ToList().FirstOrDefault();
                pnlIndicadores.GroupingText = turmaSelecionada.tur_escolaUnidade + "<br />" + turmaSelecionada.tur_calendario;

                // Filtra as turmas do calendário sendo exibido na popup.
                uccTurmaDisciplina.CarregarCombo(dadosTurmas.Where(d => d.cal_id == turmaSelecionada.cal_id), "EscolaTurmaDisciplina", "DataValueFieldCombo");

                UCComboTipoDocente1.Visible = true;
                UCComboTipoDocente1.CarregarComboTipoDocente();

                uccTurmaDisciplina.Valor = turmaSelecionada.DataValueFieldCombo;

                UCComboTipoDocente1_IndexChanged();
            }
            else
            {
                UCComboTipoDocente1.Visible = false;
                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, docente, ApplicationWEB.AppMinutosCacheCurto, turmasAtivas && docenteAtivo);

                var dadosTurmas = dados.SelectMany(p => p.Turmas);

                var turmaSelecionada = dadosTurmas.Where(p => p.tud_id == tud_id).ToList().First();

                // Filtra as turmas do calendário sendo exibido na popup.
                dadosTurmas = dadosTurmas.Where(p => p.cal_id == turmaSelecionada.cal_id);

                pnlIndicadores.GroupingText = turmaSelecionada.tur_escolaUnidade + "<br />" + turmaSelecionada.tur_calendario;

                if (turmasAtivas)
                {
                    List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, turmaSelecionada.cal_id, docenteAtivo);
                    uccTurmaDisciplina.CarregarCombo(dadosTurmasAtivas, "EscolaTurmaDisciplina", "DataValueFieldCombo");
                }
                else
                {
                    DateTime dtVigenciaFim = new DateTime();

                    // turmas de anos anteriores ou turmas extintas, dependendo da aba
                    var dadosTurmasAnosAnteriores = dadosTurmas.Where(x => x.tur_situacao == situacaoTurmaNaoAtiva).ToList();
                    uccTurmaDisciplina.CarregarCombo(dadosTurmasAnosAnteriores, "EscolaTurmaDisciplina", "DataValueFieldCombo");
                    //

                    List<TUR_Turma_Docentes_Disciplina> listaDocentesTodasDisciplinas = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(tud_id.ToString(), null);

                    // Busca a data de vigência do docente logado
                    TUR_Turma_Docentes_Disciplina docenteLogado = new TUR_Turma_Docentes_Disciplina();
                    docenteLogado = listaDocentesTodasDisciplinas.Find(p => p.entDocente.tdt_posicao == tdt_posicao && p.entDocente.doc_id == docente);

                    // Verifica permissão de edição para o professor que teve como última posição na turma a posição de titular.
                    if (docenteLogado.entDocente != null && docenteLogado.entDocente.tdt_vigenciaFim != new DateTime())
                    {
                        dtVigenciaFim = docenteLogado.entDocente.tdt_vigenciaFim;

                        TUR_Turma_Docentes_Disciplina docenteUltimo = new TUR_Turma_Docentes_Disciplina();
                        docenteUltimo = listaDocentesTodasDisciplinas.Find(x => x.entDocente.tdt_posicao == tdt_posicao && x.entDocente.doc_id != docente && x.entDocente.tdt_vigenciaInicio > dtVigenciaFim);

                        if (docenteUltimo.entDocente != null)
                            VS_permiteSalvarAulasPrevistas = false;
                    }
                }

                uccTurmaDisciplina.Valor = turmaSelecionada.DataValueFieldCombo;

                // Carregar os dados da turma.
                uccTurmaDisciplina_IndexChanged();
            }

            hdnEscId.Value = esc_id.ToString();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "TrocarTurma", "$(document).ready(function() { $('.divIndicadores').dialog('open'); });", true);
        }

        /// <summary>
        /// Verifica se a disciplina esta sendo compartilhada com mais de uma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_codigo">Codigo da turma</param>
        /// <param name="tud_nome">Nome da disciplina</param>
        /// <returns></returns>
        private bool VerificaDisciplinasCompartilhadas(long tud_id, string turma, string disciplina, long doc_id = 0)
        {
            List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo, false, doc_id);
            if (lstDisciplinaCompartilhada.Count == 0)
            {
                if (String.IsNullOrEmpty(disciplina))
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(String.Format("{0} {1}.",
                                                                            GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                            , turma)
                                                                        , UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(String.Format("{0} {1} - {2}.",
                                                                            GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                            , turma
                                                                            , disciplina)
                                                                        , UtilBO.TipoMensagem.Alerta);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "VerificaDisciplinasCompartilhadas", "$(document).ready(scrollToTop);", true);
                return false;
            }
            else if (lstDisciplinaCompartilhada.Count == 1)
            {
                // Atualiza a sessao com a disciplina compartilhada
                Session["TudIdCompartilhada"] = lstDisciplinaCompartilhada[0].tud_id.ToString();
            }
            else if (lstDisciplinaCompartilhada.Count > 1)
            {
                UCSelecaoDisciplinaCompartilhada1.AbrirDialog(tud_id, doc_id, (String.IsNullOrEmpty(disciplina) ? turma : turma + " - " + disciplina));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Redireciona para uma das telas do Minhas Turmas, de acordo com o evento do grid.
        /// </summary>
        /// <param name="nomeTela"></param>
        /// <param name="nomePagina"></param>
        /// <param name="grid"></param>
        /// <param name="indice"></param>
        /// <param name="validarDisciplinaCompartilhada"></param>
        private void RedirecionaTelaMinhasTurmas(string nomeTela, string nomePagina, GridView grid, string indice, bool validarDisciplinaCompartilhada, byte tipoPendencia = 0, int tpcIdPendencia = -1, long tudIdPendencia = -1)
        {
            try
            {
                int index = 0;
                if (int.TryParse(indice, out index))
                {
                    index = index % grid.PageSize;
                    grid.EditIndex = index;
                    if (grid != null)
                    {
                        long tud_id = 0;
                        long tur_id = 0;
                        int esc_id = 0;
                        int uni_id = 0;
                        int cal_id = 0;
                        bool tud_naoLancarNota = false;
                        bool tud_naoLancarFrequencia = false;
                        string EscolaTurmaDisciplina = string.Empty;
                        byte posicao;
                        byte tud_tipo = 0;
                        byte tur_tipo = 0;
                        DateTime tur_dataEncerramento = new DateTime();
                        string tciIds = string.Empty;
                        bool disciplinaAtiva = true;
                        long tud_idAluno = 0;
                        long tur_idNormal = 0;

                        Int64.TryParse(grid.DataKeys[index].Values["tud_id"].ToString(), out tud_id);
                        Int64.TryParse(grid.DataKeys[index].Values["tur_id"].ToString(), out tur_id);
                        Int32.TryParse(grid.DataKeys[index].Values["esc_id"].ToString(), out esc_id);
                        Int32.TryParse(grid.DataKeys[index].Values["uni_id"].ToString(), out uni_id);
                        Int32.TryParse(grid.DataKeys[index].Values["cal_id"].ToString(), out cal_id);
                        Boolean.TryParse(grid.DataKeys[index].Values["tud_naoLancarNota"].ToString(), out tud_naoLancarNota);
                        Boolean.TryParse(grid.DataKeys[index].Values["tud_naoLancarFrequencia"].ToString(), out tud_naoLancarFrequencia);
                        EscolaTurmaDisciplina = grid.DataKeys[index].Values["EscolaTurmaDisciplina"].ToString();
                        byte.TryParse(grid.DataKeys[index].Values["tdt_posicao"].ToString(), out posicao);
                        tud_tipo = Convert.ToByte(grid.DataKeys[index].Values["tud_tipo"]);
                        DateTime.TryParse(grid.DataKeys[index].Values["tur_dataEncerramento"].ToString(), out tur_dataEncerramento);
                        tciIds = grid.DataKeys[index].Values["tciIds"].ToString();
                        Byte.TryParse(grid.DataKeys[index].Values["tur_tipo"].ToString(), out tur_tipo);
                        Int64.TryParse(grid.DataKeys[index].Values["tud_idAluno"].ToString(), out tud_idAluno);
                        Int64.TryParse(grid.DataKeys[index].Values["tur_idNormal"].ToString(), out tur_idNormal);

                        CarregaSessionPaginaRetorno(tud_id, esc_id, uni_id, tur_id, tud_naoLancarNota, tud_naoLancarFrequencia, cal_id, EscolaTurmaDisciplina, posicao, tur_dataEncerramento, tciIds, tur_tipo, tud_idAluno, tur_idNormal, tipoPendencia, tpcIdPendencia, tudIdPendencia);

                        Boolean.TryParse(grid.DataKeys[index].Values["disciplinaAtiva"].ToString(), out disciplinaAtiva);
                        if (!validarDisciplinaCompartilhada || disciplinaAtiva || !VS_visaoDocente)
                        {
                            if (VerificaLancamentoAulasPrevistas(tud_id, posicao, esc_id, tur_id, cal_id, tud_tipo)
                                && (!validarDisciplinaCompartilhada
                                    || tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                    || VerificaDisciplinasCompartilhadas(tud_id, grid.DataKeys[index].Values["tur_codigo"].ToString(), grid.DataKeys[index].Values["tud_nome"].ToString())))
                            {
                                if (VS_visaoDocente)
                                {
                                    RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                                    VS_rptTurmasIndice = itemTurma.ItemIndex;
                                }

                                RedirecionaTela(nomePagina);
                            }
                            else
                            {
                                VS_TelaRedirecionar = nomePagina;
                                grid.EditIndex = -1;
                            }
                        }
                        else
                        {
                            RedirecionaTelaAtribuicaoDocente();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", nomeTela), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// O método verifica as pendências de fechamento das disciplinas dos grids de turmas.
        /// </summary>
        /// <param name="listaGrid">Lista de grids.</param>
        private void VerificaPendenciasFechamento(GridView grid, List<sTurmaDisciplinaEscolaCalendario> lstCarregarPendencias, bool mostrarPendencia)
        {
            if (lstCarregarPendencias != null)
            {
                List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> lst = REL_TurmaDisciplinaSituacaoFechamentoBO.SelecionaPendencias(lstCarregarPendencias, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);
                if (!VS_listaPendencias.ContainsKey(grid.ClientID))
                    VS_listaPendencias.Add(grid.ClientID, lst);
                else
                    VS_listaPendencias[grid.ClientID] = lst;
            }

            if (mostrarPendencia)
            {
                bool possuiPendencia = false;

                foreach (GridViewRow row in grid.Rows)
                {
                    long tud_id = Convert.ToInt64(grid.DataKeys[row.RowIndex].Values["tud_id"]);
                    byte tud_tipo = Convert.ToByte(grid.DataKeys[row.RowIndex].Values["tud_tipo"]);
                    ImageButton imgPendenciaFechamento = (ImageButton)row.FindControl("imgPendenciaFechamento");
                    ImageButton imgPendenciaPlanejamento = (ImageButton)row.FindControl("imgPendenciaPlanejamento");
                    ImageButton imgPendenciaPlanoAula = (ImageButton)row.FindControl("imgPendenciaPlanoAula");
                    if (imgPendenciaFechamento != null)
                    {
                        imgPendenciaFechamento.Visible = false;
                        imgPendenciaFechamento.CommandArgument = string.Format("{0},{1},{2}", row.RowIndex, tud_id, tud_tipo); 
                    }
                    if (imgPendenciaPlanejamento != null)
                    {
                        imgPendenciaPlanejamento.Visible = false;
                        imgPendenciaPlanejamento.CommandArgument = string.Format("{0},{1},{2}", row.RowIndex, tud_id, tud_tipo);
                    }
                    if (imgPendenciaPlanoAula != null)
                    {
                        imgPendenciaPlanoAula.Visible = false;
                        imgPendenciaPlanoAula.CommandArgument = string.Format("{0},{1},{2}", row.RowIndex, tud_id, tud_tipo);
                    }

                    if (tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                    {
                        if (VS_listaPendencias[grid.ClientID].Any(item =>
                            (
                                item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                //|| item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                            )
                            && item.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                            && item.tud_idRegencia == tud_id))
                        {
                            if (imgPendenciaFechamento != null)
                                imgPendenciaFechamento.Visible = possuiPendencia = true;
                        }
                        if (!possuiPendencia && VS_listaPendencias[grid.ClientID].Any(item => 
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                            && item.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                            && item.tud_idRegencia == tud_id))
                        {
                            // Não vai exibir o ícone de pendência, mas vai exibir o link para o relatório
                            possuiPendencia = true;
                        }

                        if (VS_listaPendencias[grid.ClientID].Any(item => 
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento 
                            && item.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia 
                            && item.tud_idRegencia == tud_id))
                        {
                            if (imgPendenciaPlanejamento != null)
                                imgPendenciaPlanejamento.Visible = true;
                        }
                    }
                    else
                    {
                        if (VS_listaPendencias[grid.ClientID].Any(item =>
                            (
                                item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                //|| item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                            )
                            && item.tud_id == tud_id))
                        {
                            if (imgPendenciaFechamento != null)
                                imgPendenciaFechamento.Visible = possuiPendencia = true;
                        }
                        if (!possuiPendencia && VS_listaPendencias[grid.ClientID].Any(item =>
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                            && item.tud_id == tud_id))
                        {
                            // Não vai exibir o ícone de pendência, mas vai exibir o link para o relatório
                            possuiPendencia = true;
                        }

                        if (VS_listaPendencias[grid.ClientID].Any(item =>
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                            && item.tud_id == tud_id))
                        {
                            if (imgPendenciaPlanejamento != null)
                                imgPendenciaPlanejamento.Visible = true;
                        }
                    }

                    if (VS_listaPendencias[grid.ClientID].Any(item =>
                            item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula
                            && item.tud_id == tud_id)
                        &&
                        (
                            // Mesma regra para exibir o ícone no Listão e no Diário de Classe.
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                            || Convert.ToInt32(grid.DataKeys[row.RowIndex].Values["tne_id"]) != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                            || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        ))
                    {
                        if (imgPendenciaPlanoAula != null)
                            imgPendenciaPlanoAula.Visible = true;
                    }
                }

                if (!possuiPendencia && VS_listaPendencias[grid.ClientID].Any(item =>
                    (
                        item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                        || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                        || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                        || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                        || item.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                    )))
                {
                    possuiPendencia = true;
                }
                
                if (VS_visaoDocente)
                {
                    RepeaterItem rptItem = (RepeaterItem)grid.NamingContainer;
                    if (rptItem != null)
                    {
                        HtmlGenericControl divMensagemFechamentoPendencia = (HtmlGenericControl)rptItem.FindControl("mensagemPendenciaFechamentoMinhasTurmas");
                        if (divMensagemFechamentoPendencia != null)
                        {
                            divMensagemFechamentoPendencia.Visible = possuiPendencia;
                        }
                        
                        HtmlGenericControl mensagemSemPendenciaFechamento = (HtmlGenericControl)rptItem.FindControl("mensagemSemPendenciaFechamento");
                        if (mensagemSemPendenciaFechamento != null)
                        {
                            mensagemSemPendenciaFechamento.Visible = !possuiPendencia;
                        }

                        Label lblDataProcessamento = (Label)rptItem.FindControl("lblDataProcessamento");
                        if (lblDataProcessamento != null)
                        {
                            lblDataProcessamento.Text = VS_listaPendencias[grid.ClientID].Any(p => p.DataProcessamento != new DateTime()) ?
                                String.Format(GetGlobalResourceObject("Academico", "ControleTurma.Busca.lblDataProcessamento.Text").ToString(), VS_listaPendencias[grid.ClientID].Max(item => item.DataProcessamento).ToString("dd'/'MM'/'yyyy HH:mm:ss")) :
                                string.Empty;
                        }
                    }
                }
                else
                {
                    mensagemPendenciaFechamentoMinhasTurmas.Visible = possuiPendencia;
                    mensagemSemPendenciaFechamento.Visible = !possuiPendencia;
                    lblDataProcessamentoAdm.Text = VS_listaPendencias[grid.ClientID].Any(p => p.DataProcessamento != new DateTime()) ?
                        String.Format(GetGlobalResourceObject("Academico", "ControleTurma.Busca.lblDataProcessamento.Text").ToString(), VS_listaPendencias[grid.ClientID].Max(item => item.DataProcessamento).ToString("dd'/'MM'/'yyyy HH:mm:ss")) :
                        string.Empty;
                }

                upnResultado.Update();
            }
        }

        /// <summary>
        /// Atualiza as indicacoes de pendencias no fechamento por turma/disciplina e a mensagem de pendencia geral.
        /// </summary>
        /// <param name="grid"></param>
        private void CarregarPendencias(GridView grid, bool mostrarPendencia)
        {
            if (!VS_visaoDocente)
            {
                // Verifico se existe evento de fechamento vigente para a escola e calendario,
                // para mostrar o botao de atualizar pendencias.
                string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                List<ACA_Evento> lstEventosEscola = ACA_EventoBO.GetEntity_Efetivacao_ListPorPeriodo(UCCCalendario1.Valor, -1, Guid.Empty, UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool existeEventoVigente = lstEventosEscola.Any(p => Convert.ToString(p.tev_id) == valor || Convert.ToString(p.tev_id) == valorFinal);

                // Apenas para carregar as pendencias armazenadas
                if (existeEventoVigente)
                {
                    List<sTurmaDisciplinaEscolaCalendario> lstCarregarPendencias = new List<sTurmaDisciplinaEscolaCalendario>();
                    List<Struct_MinhasTurmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaPorFiltrosMinhasTurmas(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCCCalendario1.Valor,
                                                                                                              UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1],
                                                                                                              UCCCurriculoPeriodo1.Valor[2], __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                                                              txtCodigoTurma.Text, UCComboTipoCiclo.Tci_id, ApplicationWEB.AppMinutosCacheCurto);
                    lstCarregarPendencias.AddRange(dadosTurmasAtivas.SelectMany(p => p.Turmas.Select(t =>
                                                                                new sTurmaDisciplinaEscolaCalendario
                                                                                {
                                                                                    tur_id = t.tur_id
                                                                                    ,
                                                                                    tud_id = t.tud_id
                                                                                    ,
                                                                                    tud_tipo = t.tud_tipo
                                                                                    ,
                                                                                    esc_id = t.esc_id
                                                                                    ,
                                                                                    uni_id = t.uni_id
                                                                                    ,
                                                                                    cal_id = t.cal_id
                                                                                }
                                                                                ).Distinct().ToList()).ToList());

                    lstCarregarPendencias = lstCarregarPendencias.Select(p => p).Distinct().ToList();
                    VerificaPendenciasFechamento(grid, lstCarregarPendencias, mostrarPendencia);
                }
            }
            else
            {
                RepeaterItem rptItem = (RepeaterItem)grid.NamingContainer;

                int esc_id = 0;
                int uni_id = 0;
                int cal_id = 0;
                HiddenField hdnId = rptItem.FindControl("hdnEscola") as HiddenField;
                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                    esc_id = Convert.ToInt32(hdnId.Value);
                HiddenField hdnUniId = rptItem.FindControl("hdnUnidadeEscola") as HiddenField;
                if (hdnUniId != null && !string.IsNullOrEmpty(hdnUniId.Value))
                    uni_id = Convert.ToInt32(hdnUniId.Value);
                HiddenField hdnCalendario = rptItem.FindControl("hdnCalendario") as HiddenField;
                if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                    cal_id = Convert.ToInt32(hdnCalendario.Value);

                // Verifico se existe evento de fechamento vigente para a escola e calendario,
                // para mostrar o botao de atualizar pendencias.
                string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                List<ACA_Evento> lstEventosEscola = ACA_EventoBO.GetEntity_Efetivacao_ListPorPeriodo(cal_id, -1, Guid.Empty, esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool existeEventoVigente = lstEventosEscola.Any(p => Convert.ToString(p.tev_id) == valor || Convert.ToString(p.tev_id) == valorFinal);

                // Apenas para carregar as pendencias armazenadas
                if (existeEventoVigente)
                {

                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                                                   __SessionWEB.__UsuarioWEB.Docente.doc_id,
                                                                                                   ApplicationWEB.AppMinutosCacheCurto);
                    List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);

                    List<sTurmaDisciplinaEscolaCalendario> lstCarregarPendencias = new List<sTurmaDisciplinaEscolaCalendario>();
                    lstCarregarPendencias.AddRange(dadosTurmasAtivas.Select(p =>
                                                                            new sTurmaDisciplinaEscolaCalendario
                                                                            {
                                                                                tur_id = p.tur_id
                                                                                ,
                                                                                tud_id = p.tud_id
                                                                                ,
                                                                                tud_tipo = p.tud_tipo
                                                                                ,
                                                                                esc_id = p.esc_id
                                                                                ,
                                                                                uni_id = p.uni_id
                                                                                ,
                                                                                cal_id = p.cal_id
                                                                            }
                                                                            ).Distinct().ToList());

                    lstCarregarPendencias = lstCarregarPendencias.Select(p => p).Distinct().ToList();
                    VerificaPendenciasFechamento(grid, lstCarregarPendencias, mostrarPendencia);
                }
            }
        }

        /// <summary>
        /// Abre uma janela para resolução das pendências.
        /// </summary>
        private void AbrirPopUpPendencias(GridView grid, byte tud_tipo, List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> pendencias, int indexTurma, string comandoTurma)
        {
            hdnIdGrid.Value = grid.ClientID;
            grvPendencias.Columns[grvPendencias_ColunaDisciplina].Visible = tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && pendencias.Any(p => p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento);
            lstPeriodosCalendario = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario();
            hdnIndexTurma.Value = indexTurma.ToString();
            hdnComandoTurma.Value = comandoTurma;
            grvPendencias.DataSource = pendencias;
            grvPendencias.DataBind();
            upnPendencias.Update();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbrirPendencias", "$(document).ready(function() { $('.divPendencias').dialog('open'); });", true);
        }

        private Control getControl(Control root, string pClientID)
        {
            if (root.ClientID == pClientID)
                return root;
            foreach (Control c in root.Controls)
                using (Control subc = getControl(c, pClientID))
                    if (subc != null)
                        return subc;
            return null;
        }

        #endregion Métodos

        #region Eventos

        protected void grvTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && PreCarregarFechamentoCache)
            {
                long tur_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tur_id"));
                int fav_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "fav_id"));
                int esc_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "esc_id"));
                int cal_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cal_id"));
                byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tur_tipo"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tud_tipo"));
                bool tud_disciplinaEspecial = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "tud_disciplinaEspecial"));
                byte tdt_posicao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tdt_posicao"));

                ACA_FormatoAvaliacao entityFormatoAvaliacao = new ACA_FormatoAvaliacao { fav_id = fav_id };
                ACA_FormatoAvaliacaoBO.GetEntity(entityFormatoAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacao = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacaoDocente = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idDocente };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacaoDocente);

                ImageButton btnFechamento = (ImageButton)e.Row.FindControl("btnFechamento");
                if (btnFechamento != null)
                {
                    btnFechamento.OnClientClick = "CarregarCacheEfetivacao(this);";
                    btnFechamento.CommandName = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "FechamentoAutomatico" : "Fechamento";
                }

                ImageButton imgPendenciaFechamento = (ImageButton)e.Row.FindControl("imgPendenciaFechamento");
                if (imgPendenciaFechamento != null)
                {
                    imgPendenciaFechamento.CommandName = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "PendenciaFechamentoAutomatico" : "PendenciaFechamento";
                }

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                // Valor do conceito global ou por disciplina.
                string valorMinimo = tud_id > 0 ?
                    entityFormatoAvaliacao.valorMinimoAprovacaoPorDisciplina :
                    entityFormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = Convert.ToDouble(valorMinimo.Replace(',', '.'));
                }
                else if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(entityEscalaAvaliacao.esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                bool incluirFinal = entityFormatoAvaliacao.fav_avaliacaoFinalAnalitica;

                List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);

                List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(cal_id, ApplicationWEB.AppMinutosCacheCurto);

                List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = tabelaPeriodos.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();

                int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
                int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

                //Busca o bimestre corrente
                Struct_CalendarioPeriodos periodoCorrente = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();
                int tpc_id = periodoCorrente.tpc_id;
                int tpc_ordem = periodoCorrente.tpc_ordem;

                if (tpc_id <= 0 && !incluirFinal)
                {
                    //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                    Struct_CalendarioPeriodos proximoPeriodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                    tpc_id = proximoPeriodo.tpc_id;
                    tpc_ordem = proximoPeriodo.tpc_ordem;

                    if (tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado então seleciona o ultimo
                        tpc_id = tpc_idUltimoPerido;
                        tpc_ordem = tpc_ordemUltimoPeriodo;
                    }
                }

                if (tpc_id >= 0 && incluirFinal)
                {
                    if (tpc_id == tpc_idUltimoPerido)
                    {
                        // Se for o ultimo periodo e a avaliacao final estiver aberta,
                        // selecionar a avaliacao final
                        List<ACA_Evento> listaEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);
                        if (listaEventos.Exists(p => p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                        {
                            tpc_id = tpc_ordem - 1;
                        }
                    }

                    if (tpc_id == 0)
                    {
                        //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                        Struct_CalendarioPeriodos proximoPeriodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                        tpc_id = proximoPeriodo.tpc_id;
                        tpc_ordem = proximoPeriodo.tpc_ordem;

                        if (tpc_id <= 0)
                        {
                            //Se não tem bimestre selecionado então seleciona o final
                            tpc_id = tpc_ordem = -1;
                        }
                    }
                }

                int ava_id = -1;
                byte ava_tipo = 0;

                if (tpc_id > 0)
                {
                    List<ACA_Avaliacao> listaAvaliacao = ACA_AvaliacaoBO.ConsultaPor_Periodo_Relacionadas(fav_id, tpc_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (listaAvaliacao.Any())
                    {
                        ava_id = listaAvaliacao.First().ava_id;
                        ava_tipo = (byte)listaAvaliacao.First().ava_tipo;
                    }
                }
                else
                {
                    List<ACA_Avaliacao> listaAvaliacao = ACA_AvaliacaoBO.SelectAvaliacaoFinal_PorFormato(fav_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (listaAvaliacao.Any(p => p.ava_tipo == (byte)AvaliacaoTipo.Final))
                    {
                        ava_id = listaAvaliacao.Find(p => p.ava_tipo == (byte)AvaliacaoTipo.Final).ava_id;
                        ava_tipo = (byte)AvaliacaoTipo.Final;
                    }
                }

                HiddenField hdn = (HiddenField)e.Row.FindControl("hdnTudId");
                hdn.Value = tud_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTurId");
                hdn.Value = tur_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTpcId");
                hdn.Value = tpc_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnAvaId");
                hdn.Value = ava_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnFavId");
                hdn.Value = fav_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoAvaliacao");
                hdn.Value = ava_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnEsaId");
                hdn.Value = entityEscalaAvaliacao.esa_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoEscala");
                hdn.Value = entityEscalaAvaliacao.esa_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoEscalaDocente");
                hdn.Value = entityEscalaAvaliacaoDocente.esa_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnNotaMinima");
                hdn.Value = notaMinimaAprovacao.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnParecerMinimo");
                hdn.Value = ordemParecerMinimo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoLancamento");
                hdn.Value = entityFormatoAvaliacao.fav_tipoLancamentoFrequencia.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnCalculoQtAulasDadas");
                hdn.Value = entityFormatoAvaliacao.fav_calculoQtdeAulasDadas.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTurTipo");
                hdn.Value = tur_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnCalId");
                hdn.Value = cal_id.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTudTipo");
                hdn.Value = tud_tipo.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTpcOrdem");
                hdn.Value = tpc_ordem.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnVariacao");
                hdn.Value = entityFormatoAvaliacao.fav_variacao.ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnTipoDocente");
                hdn.Value = (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            (byte)ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo) : (byte)0).ToString();

                hdn = (HiddenField)e.Row.FindControl("hdnDisciplinaEspecial");
                hdn.Value = tud_disciplinaEspecial ? "true" : "false";

                hdn = (HiddenField)e.Row.FindControl("hdnFechamentoAutomatico");
                hdn.Value = entityFormatoAvaliacao.fav_fechamentoAutomatico ? "true" : "false";

                tudIds += string.IsNullOrEmpty(tudIds) ? tud_id.ToString() : string.Format(",{0}", tud_id);
            }
        }

        /// <summary>
        /// Evento generico utilizando no grvTurma (docente) e grvTurmas (admin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMinhasTurmas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            VS_ChavesRedirecionaDiario = new long[] { -1, -1 };

            GridView grid = (GridView)sender;
            switch (e.CommandName)
            {
                #region Indicadores

                case "Indicadores":
                    {
                        try
                        {
                            tdt_posicao = 0;
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int esc_id = Convert.ToInt32(args[0]);
                                long tur_id = Convert.ToInt64(args[1]);
                                long tud_id = Convert.ToInt64(args[2]);
                                int cal_id = Convert.ToInt32(args[3]);
                                tdt_posicao = Convert.ToByte(args[4]);
                                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                                lblMensagemBloqueio.Visible = false;
                                CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id, true);
                                uccTurmaDisciplina.PermiteEditar = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }

                #endregion Indicadores

                #region Turma

                //case "Turma":
                //    {
                //        if (grid != null)
                //        {
                //            try
                //            {
                //                // Divisão pelo PageSize pois dá problema com o ItemIndex (commandArgument) na segunda página.
                //                grid.EditIndex = Convert.ToInt32(e.CommandArgument.ToString()) % grid.PageSize;

                //                tud_id = Convert.ToInt64(grid.DataKeys[grid.EditIndex].Values["tud_id"]);
                //                byte posicao = Convert.ToByte(grid.DataKeys[grid.EditIndex].Values["tdt_posicao"]);
                //                esc_id = Convert.ToInt32(grid.DataKeys[grid.EditIndex].Values["esc_id"]);
                //                tur_id = Convert.ToInt64(grid.DataKeys[grid.EditIndex].Values["tur_id"]);
                //                cal_id = Convert.ToInt32(grid.DataKeys[grid.EditIndex].Values["cal_id"]);

                //                if (VerificaLancamentoAulasPrevistas(tud_id, posicao, esc_id, tur_id, cal_id))
                //                {
                //                    if (VS_visaoDocente)
                //                    {
                //                        RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                //                        VS_rptTurmasIndice = itemTurma.ItemIndex;
                //                    }

                //                    RedirecionaDiarioClasse(grid);
                //                }
                //                else
                //                {
                //                    grid.EditIndex = -1;
                //                }
                //            }
                //            catch (ValidationException ex)
                //            {
                //                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                //            }
                //            catch (Exception ex)
                //            {
                //                ApplicationWEB._GravaErro(ex);
                //                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao carregar os dados.", UtilBO.TipoMensagem.Erro);
                //            }
                //        }
                //        break;
                //    }

                #endregion Turma

                #region Planejamento

                case "Planejamento":
                    {
                        RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Planejamento

                #region Diario de Classe

                case "DiarioClasse":
                    {
                        RedirecionaTelaMinhasTurmas("Diário de Classe", "DiarioClasse", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Diario de Classe

                #region Listao

                case "Listao":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Listao

                #region Fechamento

                case "Fechamento":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Efetivacao", grid, e.CommandArgument.ToString(), false);
                        break;
                    }

                case "FechamentoAutomatico":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), "Fechamento", grid, e.CommandArgument.ToString(), false);
                        break;
                    }

                #endregion Fechamento

                #region Alunos

                case "Alunos":
                    {
                        RedirecionaTelaMinhasTurmas("Alunos", "Alunos", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Alunos

                #region Frequencia

                case "Frequencia":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnFrequencia.Text").ToString(), "Frequencia", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Frequencia

                #region Avaliacao

                case "Avaliacao":
                    {
                        RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("WebControls", "UCNavegacaoTelaPeriodo.btnAvaliacao.Text").ToString(), "Avaliacao", grid, e.CommandArgument.ToString(), true);
                        break;
                    }

                #endregion Avaliacao

                #region Pendência no fechamento

                case "PendenciaFechamento":
                case "PendenciaFechamentoAutomatico":
                    {
                        try
                        {
                            string[] args = e.CommandArgument.ToString().Split(',');
                            int index = Convert.ToInt32(args[0]);
                            long tud_id = Convert.ToInt64(args[1]);
                            byte tud_tipo = Convert.ToByte(args[2]);

                            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> pendencias;
                            if (tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                            {
                                pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_id == tud_id
                                        &&
                                        (
                                            p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                        //|| p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                        )
                                    )
                                )
                                .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).ToList();
                            }
                            else
                            {
                                bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                                if (controleOrdemDisciplinas)
                                {
                                    pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                    (
                                        p =>
                                        (
                                            p.tud_idRegencia == tud_id
                                            &&
                                            (
                                                p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            //|| p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                            )
                                        )
                                    )
                                    .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).ThenBy(p => p.tds_ordem).ToList();
                                }
                                else
                                {
                                    pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                    (
                                        p =>
                                        (
                                            p.tud_idRegencia == tud_id
                                            &&
                                            (
                                                p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                                || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                            //|| p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                                            )
                                        )
                                    )
                                    .OrderBy(p => p.tipo_ordem).ThenBy(p => p.tpc_ordem).ThenBy(p => p.tud_nome).ToList();
                                }
                            }

                            if (pendencias.Count > 1)
                            {
                                AbrirPopUpPendencias(grid, tud_tipo, pendencias, index, e.CommandName);
                            }
                            else
                            {
                                REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia = pendencias.FirstOrDefault();
                                if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota)
                                {
                                    // Redireciona para o Listão de Avaliação
                                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, index.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);

                                }
                                else if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula)
                                {
                                    // Redireciona para o Diário de Classe
                                    RedirecionaTelaMinhasTurmas("Diário de Classe", "DiarioClasse", grid, index.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id);

                                }
                                else if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                                    || pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                                    || pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer)
                                {
                                    // Redireciona para o Fechamento final
                                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), e.CommandName == "PendenciaFechamentoAutomatico" ? "Fechamento" : "Efetivacao", grid, index.ToString(), false, pendencia.tipoPendencia);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar pendências.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }

                #endregion Pendência no fechamento

                #region Pendência no planejamento

                case "PendenciaPlanejamento":
                    {
                        try
                        {
                            string[] args = e.CommandArgument.ToString().Split(',');
                            int index = Convert.ToInt32(args[0]);
                            long tud_id = Convert.ToInt64(args[1]);
                            byte tud_tipo = Convert.ToByte(args[2]);

                            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> pendencias;
                            if (tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                            {
                                pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                (
                                    p =>
                                    (
                                        p.tud_id == tud_id
                                        && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                    )
                                )
                                .OrderBy(p => p.tpc_ordem).ToList();
                            }
                            else
                            {
                                bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                                if (controleOrdemDisciplinas)
                                {
                                    pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                    (
                                        p =>
                                        (
                                            p.tud_idRegencia == tud_id
                                            && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                        )
                                    )
                                    .OrderBy(p => p.tpc_ordem).ThenBy(p => p.tds_ordem).ToList();
                                }
                                else
                                {
                                    pendencias = VS_listaPendencias[grid.ClientID].FindAll
                                    (
                                        p =>
                                        (
                                            p.tud_idRegencia == tud_id
                                            && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento
                                        )
                                    )
                                    .OrderBy(p => p.tpc_ordem).ThenBy(p => p.tud_nome).ToList();
                                }
                            }

                            if (pendencias.Count > 1)
                            {
                                AbrirPopUpPendencias(grid, tud_tipo, pendencias, index, e.CommandName);
                            }
                            else
                            {
                                REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia = pendencias.FirstOrDefault();
                                if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento)
                                {
                                    // Redireciona para o Planejamento anual
                                    RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", grid, index.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar pendências.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }

                #endregion Pendência no planejamento

                #region Pendência no plano de aula

                case "PendenciaPlanoAula":
                    {
                        try
                        {
                            string[] args = e.CommandArgument.ToString().Split(',');
                            int index = Convert.ToInt32(args[0]);
                            long tud_id = Convert.ToInt64(args[1]);

                            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> pendencias;
                            pendencias = VS_listaPendencias[grid.ClientID].FindAll
                            (
                                p =>
                                (
                                    p.tud_id == tud_id
                                    && p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula
                                )
                            )
                            .OrderBy(p => p.tpc_ordem).ToList();

                            if (pendencias.Count > 1)
                            {
                                AbrirPopUpPendencias(grid, 0, pendencias, index, e.CommandName);
                            }
                            else
                            {
                                REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia = pendencias.FirstOrDefault();
                                if (pendencia.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula)
                                {
                                    // Redireciona para o Listão de plano de aula
                                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", grid, index.ToString(), true, pendencia.tipoPendencia, pendencia.tpc_id);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar pendências.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }

                #endregion Pendência no plano de aula

                default:
                    {
                        break;
                    }
            }
        }

        protected void grvTurmas_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.TELA_UNICA_LANCAMENTO_FREQUENCIA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                grid.Columns[grvTurma_ColunaListao].Visible = true;
                grid.Columns[grvTurma_ColunaFrequencia].Visible = grid.Columns[grvTurma_ColunaAvaliacao].Visible = false;
            }
            else
            {
                grid.Columns[grvTurma_ColunaListao].Visible = false;
                grid.Columns[grvTurma_ColunaFrequencia].Visible = grid.Columns[grvTurma_ColunaAvaliacao].Visible = true;
            }

            grid.Columns[grvTurma_ColunaAlunos].Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grid.Columns[grvTurma_ColunaEfetivacao].Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (!VS_visaoDocente)
            {
                UCTotalRegistros.Total = TUR_TurmaBO.GetTotalRecords();

                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(grvTurmas);

                if ((!string.IsNullOrEmpty(grvTurmas.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.MinhasTurmas))
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                    if (filtros.ContainsKey("VS_Ordenacao"))
                    {
                        filtros["VS_Ordenacao"] = grvTurmas.SortExpression;
                    }
                    else
                    {
                        filtros.Add("VS_Ordenacao", grvTurmas.SortExpression);
                    }

                    if (filtros.ContainsKey("VS_SortDirection"))
                    {
                        filtros["VS_SortDirection"] = grvTurmas.SortDirection.ToString();
                    }
                    else
                    {
                        filtros.Add("VS_SortDirection", grvTurmas.SortDirection.ToString());
                    }

                    __SessionWEB.BuscaRealizada = new BuscaGestao
                    {
                        PaginaBusca = PaginaGestao.MinhasTurmas
                        ,
                        Filtros = filtros
                    };
                }
            }
            if (grid.Rows.Count > 0)
            {
                CarregarPendencias(grid, true);

                VS_listaDivergenciasAulasPrevistas = TUR_TurmaDisciplinaBO.SelecionaDisciplinasDivergenciasAulasPrevistas(tudIds);
                foreach (GridViewRow row in grid.Rows)
                {
                    Image imgDivergenciaAulaPrevista = (Image)row.FindControl("imgDivergenciaAulaPrevista");
                    if (imgDivergenciaAulaPrevista != null)
                    {
                        imgDivergenciaAulaPrevista.Visible = VS_listaDivergenciasAulasPrevistas.Any(p => p == Convert.ToInt64(grid.DataKeys[row.RowIndex].Values["tud_id"]));
                    }
                }
            }
        }

        protected void btnGerarAula_Click(object sender, EventArgs e)
        {
            Session["URL_Retorno"] = "~/Academico/ControleTurma/Busca.aspx";
            RedirecionarPagina("~/Classe/PlanejamentoDiario/Cadastro.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void lkRegular_Click(object sender, EventArgs e)
        {
            Session["tdt_posicao"] = (byte)EnumTipoDocente.Titular;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

            Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void lkEspecial_Click(object sender, EventArgs e)
        {
            Session["tdt_posicao"] = (byte)EnumTipoDocente.Especial;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

            Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                UCComboUAEscola2.InicializarVisaoIndividual(doc_id, ent_id, 3);

                if (UCComboUAEscola2.QuantidadeItemsComboEscolas <= 1)
                {
                    divHistoricoTurmas.Visible = false;
                    lblMensagemHistorico.Text = UtilBO.GetErroMessage((String)GetGlobalResourceObject("Academico", "Academico.ControleTurma.Busca.HistoricoTurmas.MensagemNaoExisteHistorico"), UtilBO.TipoMensagem.Informacao);
                }
                else
                {
                    UCComboUAEscola2.DdlEscola.Enabled = true;

                    if (UCComboUAEscola2.QuantidadeItemsComboEscolas > 2)
                    {
                        UCComboUAEscola2.SelectedIndexEscolas = 1;
                        UCComboUAEscola2_IndexChangedUnidadeEscola();
                    }
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MostrarHistorico", "$(document).ready(function() { $('#divHistorico').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar histórico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptTurmas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int indice = e.Item.ItemIndex;
                int esc_id = 0;
                int cal_id = 0;

                // Id Escola
                HiddenField hdnId = e.Item.FindControl("hdnEscola") as HiddenField;
                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                {
                    esc_id = Convert.ToInt32(hdnId.Value);
                }

                // Calendario
                HiddenField hdnCalendario = e.Item.FindControl("hdnCalendario") as HiddenField;
                if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                {
                    cal_id = Convert.ToInt32(hdnCalendario.Value);
                }

                int uni_id = 0;
                HiddenField hdnUnidadeEscola = e.Item.FindControl("hdnUnidadeEscola") as HiddenField;
                if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                {
                    uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                }

                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);
                if (dadosTurmasAtivas.Any())
                {
                    VS_listaDivergenciasAulasPrevistas.AddRange(TUR_TurmaDisciplinaBO.SelecionaDisciplinasDivergenciasAulasPrevistas(string.Join(",", dadosTurmasAtivas.Select(p => p.tud_id.ToString()))));
                    dadosTurmasAtivas.ForEach(p => p.divergenciasAulasPrevistas = VS_listaDivergenciasAulasPrevistas.Any(t => t == p.tud_id));
                }

                GridView grdVw = e.Item.FindControl("grvTurma") as GridView;
                grdVw.DataSource = dadosTurmasAtivas;
                grdVw.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView grid = ((GridView)(sender));
                if (grid != null)
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                    RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                    int indice = itemTurma.ItemIndex;
                    int esc_id = 0;
                    int cal_id = 0;

                    // Id Escola
                    HiddenField hdnId = itemTurma.FindControl("hdnEscola") as HiddenField;
                    if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                    {
                        esc_id = Convert.ToInt32(hdnId.Value);
                    }

                    // Calendario
                    HiddenField hdnCalendario = itemTurma.FindControl("hdnCalendario") as HiddenField;
                    if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                    {
                        cal_id = Convert.ToInt32(hdnCalendario.Value);
                    }

                    grid.PageIndex = e.NewPageIndex;

                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                    List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);
                    if (dadosTurmasAtivas.Any())
                    {
                        dadosTurmasAtivas.ForEach(p => p.divergenciasAulasPrevistas = VS_listaDivergenciasAulasPrevistas.Any(t => t == p.tud_id));
                    }

                    grid.DataSource = dadosTurmasAtivas;
                    grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptCiclosAbas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int ciclo = 0;

                HiddenField hdnCiclo = e.Item.FindControl("hdnCiclo") as HiddenField;
                if (hdnCiclo != null && !string.IsNullOrEmpty(hdnCiclo.Value))
                {
                    ciclo = Convert.ToInt32(hdnCiclo.Value);
                }

                GridView grdVw = e.Item.FindControl("grvHistorico") as GridView;
                if (grdVw != null)
                {
                    var historico = VS_ltHistoricoDocente.Where(x => x.tci_id == ciclo
                                                                    && x.tur_situacao == (byte)TUR_TurmaSituacao.Ativo).ToList();
                    grdVw.Columns[grvHistorico_ColunaDocenciaCompartilhada].Visible = historico.Any(p => !String.IsNullOrEmpty(p.docenciaCompartilhada));
                    grdVw.DataSource = historico;
                    grdVw.DataBind();
                }
            }
        }

        protected void btnAtribuirTurma_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MostrarHistorico", "$(document).ready(function() { $('#divHistorico').dialog('close'); });", true);

            Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoDocentes/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Evento para direcionar a turma para a tela do Diário
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvHistorico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid != null)
            {
                switch (e.CommandName)
                {
                    case "Turma":
                        {
                            if (VS_visaoDocente)
                            {
                                RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                                VS_rptTurmasIndice = itemTurma.ItemIndex;
                            }

                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int esc_id = Convert.ToInt32(args[0]);
                                long tur_id = Convert.ToInt64(args[1]);
                                int cal_id = Convert.ToInt32(args[3]);
                                byte tud_tipo = Convert.ToByte(args[5]);

                                // se for docencia compartilhada sem nenhuma disciplina relacionada,
                                // redireciono para a tela de atribuicao de docente
                                if (VS_visaoDocente
                                    && tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                    && args[8] == "0")
                                {
                                    Dictionary<string, string> listaDados = new Dictionary<string, string>();
                                    listaDados.Add("Edit_esc_id", esc_id.ToString());
                                    listaDados.Add("Edit_tur_id", tur_id.ToString());
                                    listaDados.Add("Edit_cal_id", cal_id.ToString());
                                    listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/Busca.aspx");

                                    Session["DadosPaginaRetorno"] = listaDados;
                                    RedirecionaTelaAtribuicaoDocente();
                                }
                                else
                                {
                                    long tud_id = Convert.ToInt64(args[2]);
                                    tdt_posicao = Convert.ToByte(args[4]);
                                    byte tdt_situacao = Convert.ToByte(args[6]);
                                    byte index = Convert.ToByte(args[7]);

                                    CarregaSessionHistorico(tud_id.ToString(), tdt_posicao.ToString());
                                    if (VerificaLancamentoAulasPrevistas(tud_id, tdt_posicao, esc_id, tur_id, cal_id, tud_tipo, tdt_situacao == (byte)TUR_TurmaDocenteSituacao.Ativo)
                                        && (tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                            || VerificaDisciplinasCompartilhadas(tud_id, grid.DataKeys[index].Values["Turma"].ToString(), "", __SessionWEB.__UsuarioWEB.Docente.doc_id)))
                                    {
                                        RedirecionaTela("DiarioClasse");
                                    }
                                    else
                                    {
                                        VS_TelaRedirecionar = "DiarioClasse";
                                    }
                                }
                                lblMensagemBloqueio.Visible = false;
                            }
                            break;
                        }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                long tud_id = Int64.Parse(grvPeriodosAulas.DataKeys[0]["tud_id"].ToString());
                byte tud_tipo = Byte.Parse(grvPeriodosAulas.DataKeys[0]["tud_tipo"].ToString());
                bool fav_fechamentoAutomatico = bool.Parse(grvPeriodosAulas.DataKeys[0]["fav_fechamentoAutomatico"].ToString());

                List<TUR_TurmaDisciplinaAulaPrevista> aulasPrevistas = TUR_TurmaDisciplinaAulaPrevistaBO.SelecionaPorDisciplina(tud_id);

                totalPrevistas = 0;

                List<TUR_TurmaDisciplinaAulaPrevista> listaSalvar = new List<TUR_TurmaDisciplinaAulaPrevista>();
                List<TUR_TurmaDisciplinaAulaPrevista> listaProcessarPend = new List<TUR_TurmaDisciplinaAulaPrevista>();

                foreach (GridViewRow row in grvPeriodosAulas.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        int tpc_id = int.Parse(grvPeriodosAulas.DataKeys[row.RowIndex]["tpc_id"].ToString());

                        TextBox text = (TextBox)row.FindControl("txtPrevistas");
                        if (text != null)
                        {
                            if (text.Enabled && string.IsNullOrEmpty(text.Text))
                                throw new ValidationException("Quantidade de aulas previstas é obrigatório.");

                            int qtAulasPrevistas = string.IsNullOrEmpty(text.Text) ? 0 : int.Parse(text.Text);
                            if (text.Enabled && string.IsNullOrEmpty(text.Text) && qtAulasPrevistas < 1)
                                throw new ValidationException("Quantidade de aulas previstas deve ser maior que 0.");

                            TUR_TurmaDisciplinaAulaPrevista aulaPrevista = aulasPrevistas.Find(p => p.tpc_id == tpc_id);
                            if (aulaPrevista == null)
                            {
                                aulaPrevista = new TUR_TurmaDisciplinaAulaPrevista();

                                // Seta os dados para uma nova insercao.
                                aulaPrevista.tud_id = tud_id;
                                aulaPrevista.tpc_id = tpc_id;
                                aulaPrevista.tap_registrosCorrigidos = false;
                            }
                            totalPrevistas += qtAulasPrevistas;

                            if ((aulaPrevista.tap_registrosCorrigidos && aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas) || !text.Enabled)
                                continue;

                            aulaPrevista.tap_registrosCorrigidos = aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas;

                            if (!(aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas || !text.Enabled))
                                listaProcessarPend.Add(aulaPrevista);

                            // Atualiza ou seta as aulas previstas.
                            aulaPrevista.tap_aulasPrevitas = qtAulasPrevistas;

                            aulaPrevista.tud_tipo = tud_tipo;

                            listaSalvar.Add(aulaPrevista);
                        }
                    }
                }

                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                if (TUR_TurmaDisciplinaAulaPrevistaBO.SalvarAulasPrevistas(listaSalvar, listaProcessarPend, ent_id, int.Parse(hdnEscId.Value), doc_id, fav_fechamentoAutomatico))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Aulas previstas | tud_id: " + tud_id);

                    if (grvPeriodosAulas.FooterRow != null)
                    {
                        Label label = (Label)grvPeriodosAulas.FooterRow.FindControl("lblTotalPrevistas");
                        if (label != null)
                            label.Text = totalPrevistas.ToString();
                    }

                    lblMensagemIndicador.Text = UtilBO.GetErroMessage("Dados salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    // Recarrega o grid.
                    if (VS_visaoDocente)
                    {
                        List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                        foreach (RepeaterItem item in rptTurmas.Items)
                        {
                            int indice = item.ItemIndex;
                            int esc_id = 0;
                            int cal_id = 0;

                            // Id Escola
                            HiddenField hdnId = item.FindControl("hdnEscola") as HiddenField;
                            if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                            {
                                esc_id = Convert.ToInt32(hdnId.Value);
                            }

                            // Calendario
                            HiddenField hdnCalendario = item.FindControl("hdnCalendario") as HiddenField;
                            if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                            {
                                cal_id = Convert.ToInt32(hdnCalendario.Value);
                            }

                            List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);
                            if (dadosTurmasAtivas.Any())
                            {
                                dadosTurmasAtivas.ForEach(p => p.divergenciasAulasPrevistas = VS_listaDivergenciasAulasPrevistas.Any(t => t == p.tud_id));
                            }

                            GridView grdVw = item.FindControl("grvTurma") as GridView;
                            grdVw.DataSource = dadosTurmasAtivas;
                            grdVw.DataBind();
                        }
                    }
                    else
                    {
                        // Atualiza o grid
                        grvTurmas.DataBind();
                    }

                    if (VS_visaoDocente && VS_ChavesRedirecionaDiario.Length > 0 && VS_ChavesRedirecionaDiario[0] > 0)
                    {
                        // Redirecionar pra tela do diário de classe, pois o usuário clicou no botão da turma.
                        Session["tud_id"] = VS_ChavesRedirecionaDiario[0];
                        Session["tdt_posicao"] = VS_ChavesRedirecionaDiario[1];
                        Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";

                        if (!string.IsNullOrEmpty(VS_TelaRedirecionar))
                        {
                            RedirecionaTela(VS_TelaRedirecionar);
                        }
                        else
                        {
                            Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
                else
                {
                    lblMensagemIndicador.Text = UtilBO.GetErroMessage("Não foi possível salvar as aulas previstas.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemIndicador.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemIndicador.Text = UtilBO.GetErroMessage("Erro ao tentar salvar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnResultado.Update();
                lblMensagemIndicador.Focus();
            }
        }

        protected void grvPeriodosAulas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ((DataControlField)grvPeriodosAulas.Columns
                       .Cast<DataControlField>()
                       .Where(fld => fld.HeaderText == "Reposições")
                       .SingleOrDefault()).Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRAR_AULA_REPOSICAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        protected void grvPeriodosAulas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox text = (TextBox)e.Row.FindControl("txtPrevistas");
                RequiredFieldValidator rfv = (RequiredFieldValidator)e.Row.FindControl("rvPrevistas");
                CompareValidator cpv = (CompareValidator)e.Row.FindControl("cvPrevistas");
                if (text != null)
                {
                    int tpc_id = int.Parse(grvPeriodosAulas.DataKeys[e.Row.RowIndex]["tpc_id"].ToString());
                    DateTime cap_dataFim = DateTime.Parse(grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_dataFim"].ToString());

                    bool periodoEfetivado = VS_PeriodoEfetivado(tpc_id, cal_idAula, tur_idAula, cap_dataFim);

                    if (periodoEfetivado)
                        periodosEfetivados += string.IsNullOrEmpty(periodosEfetivados) ? grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_descricao"].ToString() :
                                              ", " + grvPeriodosAulas.DataKeys[e.Row.RowIndex]["cap_descricao"].ToString();

                    text.CssClass += " txtPrevistas";
                    totalPrevistas += int.Parse(string.IsNullOrEmpty(text.Text) ? "0" : text.Text);

                    // Se nao for docente bloqueia os campos
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                    {
                        mostraSalvar = false;
                        text.Enabled = false;
                        if (rfv != null)
                            rfv.Enabled = false;
                        if (cpv != null)
                            cpv.Enabled = false;
                    }
                    // Somente os docentes titulares e o especial podem alterar ou salvar e o período não pode estar efetivado
                    else if ((!(tdt_posicao == (byte)EnumTipoDocente.Titular ||
                               tdt_posicao == (byte)EnumTipoDocente.SegundoTitular ||
                               tdt_posicao == (byte)EnumTipoDocente.Especial) ||
                              !VS_permiteSalvarAulasPrevistas) || periodoEfetivado)
                    {
                        text.Enabled = false;
                        if (rfv != null)
                            rfv.Enabled = false;
                        if (cpv != null)
                            cpv.Enabled = false;
                    }
                    else
                    {
                        mostraSalvar = true;
                        text.Enabled = true;
                    }

                    HyperLink lnkSugestao = (HyperLink)e.Row.FindControl("lnkSugestao");
                    Label lblSugestao = (Label)e.Row.FindControl("lblSugestao");
                    Button btnSugestao = (Button)e.Row.FindControl("btnSugestao");

                    if (lblSugestao != null)
                    {
                        int sugestao = int.Parse(lblSugestao.Text);
                        totalSugestao += sugestao;

                        lblSugestao.Visible = !text.Enabled || sugestao <= 0;
                        if (sugestao <= 0)
                        {
                            lblSugestao.Text = "-";
                        }

                        if (lnkSugestao != null)
                            lnkSugestao.Visible = sugestao > 0 && text.Enabled;

                        if (btnSugestao != null)
                            btnSugestao.Visible = sugestao > 0 && text.Enabled;
                    }
                }

                Label label = (Label)e.Row.FindControl("lblDadas");
                if (label != null)
                    totalDadas += int.Parse(label.Text);

                label = (Label)e.Row.FindControl("lblReposicoes");
                if (label != null)
                    totalRepostas += int.Parse(label.Text);

                if (DataBinder.Eval(e.Row.DataItem, "aulasCriadas") != DBNull.Value)
                    possuiAulasCriadas = true;
                else
                {
                    label = (Label)e.Row.FindControl("lblCriadas");
                    if (label != null)
                    {
                        label.Text = "-";
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label label = (Label)e.Row.FindControl("lblTotalPrevistas");
                if (label != null)
                    label.Text = totalPrevistas.ToString();
                label = (Label)e.Row.FindControl("lblTotalDadas");
                if (label != null)
                    label.Text = totalDadas.ToString();
                label = (Label)e.Row.FindControl("lblTotalReposicoes");
                if (label != null)
                    label.Text = totalRepostas.ToString();
                label = (Label)e.Row.FindControl("lblTotalSugestao");
                if (label != null)
                    label.Text = totalSugestao.ToString();

                e.Row.CssClass = "gridRow";
            }

            // Acertar mensagem de obrigatoriedade dos validators.
            RequiredFieldValidator rvPrevistas = (RequiredFieldValidator)e.Row.FindControl("rvPrevistas");
            CompareValidator cvPrevistas = (CompareValidator)e.Row.FindControl("cvPrevistas");

            if (rvPrevistas != null)
            {
                rvPrevistas.ErrorMessage = string.Format("Quantidade de aulas previstas do {0} é obrigatório.", DataBinder.Eval(e.Row.DataItem, "cap_descricao").ToString());
            }

            if (cvPrevistas != null)
            {
                cvPrevistas.ErrorMessage = string.Format("Quantidade de aulas previstas do {0} deve ser maior que 0 (zero).", DataBinder.Eval(e.Row.DataItem, "cap_descricao").ToString());
            }
        }

        /// <summary>
        /// Evento de comandos do grid de Turmas Extintas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvHistoricoTurmasExtintas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Turma":
                    {
                        try
                        {
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int tud_tipo = Convert.ToByte(args[6]);

                                // se for docencia compartilhada sem nenhuma disciplina relacionada,
                                // redireciono para a tela de atribuicao de docente
                                if (VS_visaoDocente
                                    && tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                    && args[7] == "0")
                                {
                                    int esc_id = Convert.ToInt32(args[0]);
                                    long tur_id = Convert.ToInt64(args[1]);
                                    int cal_id = Convert.ToInt32(args[3]);

                                    Dictionary<string, string> listaDados = new Dictionary<string, string>();
                                    listaDados.Add("Edit_esc_id", esc_id.ToString());
                                    listaDados.Add("Edit_tur_id", tur_id.ToString());
                                    listaDados.Add("Edit_cal_id", cal_id.ToString());
                                    listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/Busca.aspx");

                                    Session["DadosPaginaRetorno"] = listaDados;
                                    RedirecionaTelaAtribuicaoDocente();
                                }
                                else
                                {
                                    GridView grid = (GridView)sender;
                                    long tud_id = Convert.ToInt64(args[2]);
                                    tdt_posicao = Convert.ToByte(args[4]);
                                    int index = Convert.ToByte(args[5]);

                                    CarregaSessionHistorico(tud_id.ToString(), tdt_posicao.ToString());
                                    if (tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                        || VerificaDisciplinasCompartilhadas(tud_id, grid.DataKeys[index].Values["Turma"].ToString(), "", __SessionWEB.__UsuarioWEB.Docente.doc_id))
                                    {
                                        RedirecionaTela("DiarioClasse");
                                    }
                                    else
                                    {
                                        VS_TelaRedirecionar = "DiarioClasse";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao carregar os dados.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }
                case "Indicadores":
                    {
                        try
                        {
                            tdt_posicao = 0;
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int esc_id = Convert.ToInt32(args[0]);
                                long tur_id = Convert.ToInt64(args[1]);
                                long tud_id = Convert.ToInt64(args[2]);
                                int cal_id = Convert.ToInt32(args[3]);
                                tdt_posicao = Convert.ToByte(args[4]);
                                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                                lblMensagemBloqueio.Visible = false;
                                CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id, false, (byte)TUR_TurmaSituacao.Extinta);
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }
            }
        }

        protected void grvHistoricoTurmasInativas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Turma":
                    {
                        try
                        {
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int tud_tipo = Convert.ToByte(args[6]);

                                // se for docencia compartilhada sem nenhuma disciplina relacionada,
                                // redireciono para a tela de atribuicao de docente
                                if (VS_visaoDocente
                                    && tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                    && args[8] == "0")
                                {
                                    int esc_id = Convert.ToInt32(args[0]);
                                    long tur_id = Convert.ToInt64(args[1]);
                                    int cal_id = Convert.ToInt32(args[3]);

                                    Dictionary<string, string> listaDados = new Dictionary<string, string>();
                                    listaDados.Add("Edit_esc_id", esc_id.ToString());
                                    listaDados.Add("Edit_tur_id", tur_id.ToString());
                                    listaDados.Add("Edit_cal_id", cal_id.ToString());
                                    listaDados.Add("PaginaRetorno", "~/Academico/ControleTurma/Busca.aspx");

                                    Session["DadosPaginaRetorno"] = listaDados;
                                    RedirecionaTelaAtribuicaoDocente();
                                }
                                else
                                {
                                    GridView grid = (GridView)sender;
                                    long tud_id = Convert.ToInt64(args[2]);
                                    tdt_posicao = Convert.ToByte(args[4]);
                                    int index = Convert.ToByte(args[5]);

                                    CarregaSessionHistorico(tud_id.ToString(), tdt_posicao.ToString());
                                    if (tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                        || VerificaDisciplinasCompartilhadas(tud_id, grid.DataKeys[index].Values["Turma"].ToString(), "", __SessionWEB.__UsuarioWEB.Docente.doc_id))
                                    {
                                        RedirecionaTela("DiarioClasse");
                                    }
                                    else
                                    {
                                        VS_TelaRedirecionar = "DiarioClasse";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao carregar os dados.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }
                case "Indicadores":
                    {
                        try
                        {
                            tdt_posicao = 0;
                            string[] args = e.CommandArgument.ToString().Split(',');
                            if (args.Length > 4)
                            {
                                int esc_id = Convert.ToInt32(args[0]);
                                long tur_id = Convert.ToInt64(args[1]);
                                long tud_id = Convert.ToInt64(args[2]);
                                int cal_id = Convert.ToInt32(args[3]);
                                tdt_posicao = Convert.ToByte(args[4]);
                                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                                lblMensagemBloqueio.Visible = false;
                                CarregaAulasPrevistas(esc_id, tur_id, tud_id, cal_id, ent_id, false, (byte)TUR_TurmaSituacao.Encerrada);
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os indicadores.", UtilBO.TipoMensagem.Erro);
                        }

                        break;
                    }
            }
        }

        protected void ddlAnoInativos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var historicoTurmasAnosAnteriores = VS_ltHistoricoDocente.Where(x => x.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada &&
                                                    Convert.ToInt32(ddlAnoInativos.SelectedValue) == x.cal_ano).ToList();

            grvHistoricoTurmasInativas.DataSource = historicoTurmasAnosAnteriores;
            grvHistoricoTurmasInativas.DataBind();

            updHistorico.Update();
        }

        private void UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina(long tud_id)
        {
            // Atualiza a sessao com a disciplina compartilhada selecionada
            Session["TudIdCompartilhada"] = tud_id.ToString();
            //
            RedirecionaTela(VS_TelaRedirecionar);
        }

        protected void lkbMensagemPendenciaFechamento_Click(object sender, EventArgs e)
        {
            sGridTurmaEscolaCalendario grid = new sGridTurmaEscolaCalendario();
            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> lstPendencia = new List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>();

            try
            {
                if (VS_visaoDocente)
                {
                    LinkButton lkb = (LinkButton)sender;
                    if (lkb != null)
                    {
                        RepeaterItem rptItem = (RepeaterItem)lkb.NamingContainer;
                        if (rptItem != null)
                        {
                            HiddenField hdnUadSuperior = (HiddenField)rptItem.FindControl("hdnUadSuperior");
                            HiddenField hdnEscola = (HiddenField)rptItem.FindControl("hdnEscola");
                            HiddenField hdnUnidadeEscola = (HiddenField)rptItem.FindControl("hdnUnidadeEscola");
                            HiddenField hdnCalendario = (HiddenField)rptItem.FindControl("hdnCalendario");
                            HiddenField hdnCalendarioAno = (HiddenField)rptItem.FindControl("hdnCalendarioAno");
                            GridView grv = (GridView)rptItem.FindControl("grvTurma");

                            if (grv != null && VS_listaPendencias.ContainsKey(grv.ClientID))
                                lstPendencia = VS_listaPendencias[grv.ClientID];

                            if (hdnUadSuperior != null && hdnEscola != null && hdnUnidadeEscola != null && hdnCalendario != null && hdnCalendarioAno != null && grv != null)
                            {
                                grid = new sGridTurmaEscolaCalendario
                                {
                                    gridTurma = grv
                                            ,
                                    uad_idSuperior = new Guid(string.IsNullOrEmpty(hdnUadSuperior.Value) ? Guid.Empty.ToString() : hdnUadSuperior.Value)
                                            ,
                                    esc_id = Convert.ToInt32(string.IsNullOrEmpty(hdnEscola.Value) ? "-1" : hdnEscola.Value)
                                            ,
                                    uni_id = Convert.ToInt32(string.IsNullOrEmpty(hdnUnidadeEscola.Value) ? "-1" : hdnUnidadeEscola.Value)
                                            ,
                                    cal_id = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendario.Value) ? "-1" : hdnCalendario.Value)
                                            ,
                                    cal_ano = Convert.ToInt32(string.IsNullOrEmpty(hdnCalendarioAno.Value) ? "-1" : hdnCalendarioAno.Value)
                                };
                            }
                        }
                    }
                }
                else
                {
                    if (VS_listaPendencias.ContainsKey(grvTurmas.ClientID))
                        lstPendencia = VS_listaPendencias[grvTurmas.ClientID];

                    grid = new sGridTurmaEscolaCalendario
                    {
                        gridTurma = grvTurmas
                            ,
                        uad_idSuperior = UCComboUAEscola1.Uad_ID
                            ,
                        esc_id = UCComboUAEscola1.Esc_ID
                            ,
                        uni_id = UCComboUAEscola1.Uni_ID
                            ,
                        cal_id = UCCCalendario1.Valor
                            ,
                        cal_ano = UCCCalendario1.Cal_ano
                    };
                }

                string report, parametros;

                if (grid.gridTurma != null && grid.esc_id > 0 && grid.uni_id > 0 && grid.cal_id > 0 && grid.cal_ano > 0)
                {
                    var turmadisciplina = lstPendencia.Where(p =>
                        (
                            p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                        )
                    ).Select(p => p.tud_id);

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_id in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_id));
                    }

                    string tud_ids = string.Join(",", turmadisciplina.GroupBy(p => p.ToString()).Select(p => p.Key.ToString()).ToArray());

                    turmadisciplina = lstPendencia.Where(p =>
                        (
                            p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                            || p.tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                        )
                    ).Select(p => p.tud_idRegencia);

                    //Limpa o cache apenas dos tud_ids que serão recarregados no relatório
                    foreach (Int64 tud_idRegencia in turmadisciplina.GroupBy(p => p).Select(p => p.Key))
                    {
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                        CacheManager.Factory.RemoveByPattern(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, grid.esc_id, grid.uni_id, grid.cal_id, tud_idRegencia));
                    }

                    report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
                    parametros = "uad_idSuperiorGestao=" + grid.uad_idSuperior +
                                 "&esc_id=" + grid.esc_id +
                                 "&uni_id=" + grid.uni_id +
                                 "&cal_id=" + grid.cal_id +
                                 "&cal_ano=" + grid.cal_ano +
                                 "&cur_id=-1" +
                                 "&crr_id=-1" +
                                 "&crp_id=-1" +
                                 "&tur_id=-1" +
                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                 "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                                 "&tud_ids=" + tud_ids +
                                 "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                                 "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                 "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                 "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                 "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                 "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                         , ApplicationWEB.LogoRelatorioSSRS) +
                                 "&DataProcessamento=" + lstPendencia.Max(p => p.DataProcessamento);

                    CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvPendencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte tipoPendencia = Convert.ToByte(grvPendencias.DataKeys[e.Row.RowIndex]["tipoPendencia"].ToString());
                Label lblPendencia = (Label)e.Row.FindControl("lblPendencia");
                if (lblPendencia != null)
                {
                    if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                        || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula
                        || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                        || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                        || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer
                    )
                    {
                        lblPendencia.Text = GetGlobalResourceObject("Academico", "ControleTurma.Busca.grvPendencias.lblPendencia.Fechamento").ToString();
                    }
                    else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento)
                    {
                        lblPendencia.Text = GetGlobalResourceObject("Academico", "ControleTurma.Busca.grvPendencias.lblPendencia.Planejamento").ToString();
                    }
                    else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula)
                    {
                        lblPendencia.Text = GetGlobalResourceObject("Academico", "ControleTurma.Busca.grvPendencias.lblPendencia.PlanoAula").ToString();
                    }
                }

                Label lblBimestre = (Label)e.Row.FindControl("lblBimestre");
                if (lblBimestre != null)
                {
                    if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese)
                    {
                        lblBimestre.Text = GetGlobalResourceObject("Academico", "ControleTurma.Busca.grvPendencias.lblBimestre.SemSintese").ToString();
                    }
                    else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal)
                    {
                        lblBimestre.Text = GetGlobalResourceObject("Academico", "ControleTurma.Busca.grvPendencias.lblBimestre.SemResultadoFinal").ToString();
                    }
                    else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer)
                    {
                        lblBimestre.Text = GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO").ToString();
                    }
                    else
                    {
                        int tpc_id = Convert.ToInt32(grvPendencias.DataKeys[e.Row.RowIndex]["tpc_id"].ToString());
                        if (lstPeriodosCalendario.Any(p => p.tpc_id == tpc_id))
                        {
                            lblBimestre.Text = lstPeriodosCalendario.Find(p => p.tpc_id == tpc_id).tpc_nome;
                        }
                    }
                }

                if (grvPendencias.Columns[grvPendencias_ColunaDisciplina].Visible
                    && (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota
                        || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento))
                {
                    Label lblDisciplina = (Label)e.Row.FindControl("lblDisciplina");
                    if (lblDisciplina != null)
                    {
                        lblDisciplina.Text = DataBinder.Eval(e.Row.DataItem, "tud_nome").ToString();
                    }
                }
            }
        }

        protected void grvPendencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Pendencia")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                byte tipoPendencia = Convert.ToByte(grvPendencias.DataKeys[index].Values["tipoPendencia"].ToString());
                int tpc_id = Convert.ToInt32(grvPendencias.DataKeys[index].Values["tpc_id"].ToString());
                long tud_id = Convert.ToInt64(grvPendencias.DataKeys[index].Values["tud_id"].ToString());
                GridView gridPendencia = divResultadoDocente.Visible ? (GridView)getControl(rptTurmas, hdnIdGrid.Value) : grvTurmas;

                REL_TurmaDisciplinaSituacaoFechamento_Pendencia pendencia = VS_listaPendencias[gridPendencia.ClientID].Find(
                        p => p.tipoPendencia == tipoPendencia && p.tpc_id == tpc_id && p.tud_id == tud_id);

                if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota)
                {
                    // Redireciona para o Listão de Avaliação
                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", gridPendencia, hdnIndexTurma.Value, true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);
                }
                else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.DisciplinaSemAula)
                {
                    // Redireciona para o Diário de Classe
                    RedirecionaTelaMinhasTurmas("Diário de Classe", "DiarioClasse", gridPendencia, hdnIndexTurma.Value, true, pendencia.tipoPendencia, pendencia.tpc_id);
                }
                else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemSintese
                    || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemResultadoFinal
                    || tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemParecer)
                {
                    // Redireciona para o Fechamento final
                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO").ToString(), hdnComandoTurma.Value == "PendenciaFechamentoAutomatico" ? "Fechamento" : "Efetivacao", gridPendencia, hdnIndexTurma.Value, false, pendencia.tipoPendencia);
                }
                else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.PendentePlanejamento)
                {
                    // Redireciona para o Planejamento anual
                    RedirecionaTelaMinhasTurmas("Planejamento", "PlanejamentoAnual", gridPendencia, hdnIndexTurma.Value, true, pendencia.tipoPendencia, pendencia.tpc_id, pendencia.tud_id);
                }
                else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula)
                {
                    // Redireciona para o Listão de plano de aula
                    RedirecionaTelaMinhasTurmas(GetGlobalResourceObject("Mensagens", "MSG_Listao").ToString(), "Listao", gridPendencia, hdnIndexTurma.Value, true, pendencia.tipoPendencia, pendencia.tpc_id);
                }
            }
        }

        #endregion Eventos
    }
}