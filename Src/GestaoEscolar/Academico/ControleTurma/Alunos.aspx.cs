using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Entities;
using System.IO;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class Alunos : MotherPageLogadoCompressedViewState
    {
        #region Constantes

        private const int grvAluno_ColunaAnotacoes = 6;
        private const int grvAluno_ColunaBoletim = 7;
        private const int grvAluno_ColunaRelatorioPedagogico = 8;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        private bool usuarioPermissao
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// Guarda todas as entities utilizadas pela pagina
        /// </summary>
        private ControleTurmas VS_EntitiesControleTurma
        {
            get
            {
                if (ViewState["VS_EntitiesControleTurma"] == null)
                {
                    ViewState["VS_EntitiesControleTurma"] = TUR_TurmaDisciplinaBO.SelecionaEntidadesControleTurmas(UCControleTurma1.VS_tud_id, ApplicationWEB.AppMinutosCacheLongo);
                }
                return (ControleTurmas)(ViewState["VS_EntitiesControleTurma"]);
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

        /// <summary>
        /// Lista de permissões do docente para consultar boletim.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoBoletim
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoBoletim"] ??
                            (
                                ViewState["VS_ltPermissaoBoletim"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Boletim)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de anotações da aula.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoAnotacoes
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoAnotacoes"] ??
                            (
                                ViewState["VS_ltPermissaoAnotacoes"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Anotacoes)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de frequencia.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoFrequencia
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoFrequencia"] ??
                            (
                                ViewState["VS_ltPermissaoFrequencia"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Frequencia)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de avaliações.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoAvaliacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoAvaliacao"] ??
                            (
                                ViewState["VS_ltPermissaoAvaliacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Avaliacoes)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de efetivacap.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoEfetivacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoEfetivacao"] ??
                            (
                                ViewState["VS_ltPermissaoEfetivacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Efetivacao)
                            )
                        );
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
                                ViewState["VS_ltPermissaoPlanejamentoAnual"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.PlanejamentoAnual)
                            )
                        );
            }
        }

        private bool CancelaSelect = true;

        /// <summary>
        /// ViewState que armazena a situação da turma disciplina.
        /// </summary>
        private int VS_situacaoTurmaDisciplina
        {
            get
            {
                if (ViewState["VS_situacaoTurmaDisciplina"] != null)
                    return Convert.ToInt32(ViewState["VS_situacaoTurmaDisciplina"]);
                return 1;
            }

            set
            {
                ViewState["VS_situacaoTurmaDisciplina"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
        /// </summary>
        protected int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] == null)
                {
                    ViewState["VS_cal_ano"] = ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_ano;
                }
                return Convert.ToInt32(ViewState["VS_cal_ano"]);
            }
        }


        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de turmasAnoAtual que indica se ha turmas ativas turma no ano atual
        /// </summary>
        protected bool VS_turmasAnoAtual
        {
            get
            {
                if (ViewState["VS_turmasAnoAtual"] == null)
                    return false;

                return Convert.ToBoolean(ViewState["VS_turmasAnoAtual"]);
            }
            set
            {
                ViewState["VS_turmasAnoAtual"] = value;
            }
        }

        /// <summary>
        /// Carrega a turma disciplina relacionada (para as disciplinas de docencia compartilhada).
        /// </summary>
        private sTurmaDisciplinaRelacionada VS_turmaDisciplinaRelacionada
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
        /// Armazena se a tela foi carregada pelo Historico de turmas.
        /// </summary>
        private bool VS_historico
        {
            get
            {
                if (ViewState["VS_historico"] != null)
                    return (bool)ViewState["VS_historico"];
                return false;
            }
            set
            {
                ViewState["VS_historico"] = value;
            }
        }

        private List<Struct_PreenchimentoAluno> lstAlunosRelatorioRP = new List<Struct_PreenchimentoAluno>();

        #endregion

        #region Métodos

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno()
        {
            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Tud_idRetorno_ControleTurma", UCControleTurma1.VS_tud_id.ToString());
            listaDados.Add("Edit_tdt_posicao", UCControleTurma1.VS_tdt_posicao.ToString());
            listaDados.Add("Edit_esc_id", UCControleTurma1.VS_esc_id.ToString());
            listaDados.Add("Edit_uni_id", UCControleTurma1.VS_uni_id.ToString());
            listaDados.Add("Edit_tur_id", UCControleTurma1.VS_tur_id.ToString());
            listaDados.Add("Edit_tud_naoLancarNota", UCControleTurma1.VS_tud_naoLancarNota.ToString());
            listaDados.Add("Edit_tud_naoLancarFrequencia", UCControleTurma1.VS_tud_naoLancarFrequencia.ToString());
            listaDados.Add("Edit_tur_dataEncerramento", UCControleTurma1.VS_tur_dataEncerramento.ToString());
            listaDados.Add("Edit_tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
            listaDados.Add("Edit_tpc_ordem", UCNavegacaoTelaPeriodo.VS_tpc_ordem.ToString());
            listaDados.Add("Edit_cal_id", UCNavegacaoTelaPeriodo.VS_cal_id.ToString());
            listaDados.Add("TextoTurmas", UCControleTurma1.LabelTurmas);
            listaDados.Add("OpcaoAbaAtual", Convert.ToByte(UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual).ToString());
            listaDados.Add("Edit_tciIds", UCControleTurma1.VS_tciIds);
            listaDados.Add("Edit_tur_tipo", UCControleTurma1.VS_tur_tipo.ToString());
            listaDados.Add("Edit_tud_idAluno", UCControleTurma1.VS_tud_idAluno.ToString());
            listaDados.Add("Edit_tur_idNormal", UCControleTurma1.VS_tur_idNormal.ToString());
            listaDados.Add("PaginaRetorno", UCNavegacaoTelaPeriodo.VS_paginaRetorno);
            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);

            if (VS_turmaDisciplinaRelacionada.tud_id > 0)
            {
                Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
            }

            Session["tur_tud_ids"] = UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => String.Format("{0};{1}", p.tur_id, p.tud_id)).ToList();

            Session["Historico"] = VS_historico;
        }

        /// <summary>
        /// Carrega dados da tela.
        /// </summary>
        private void CarregarTela(bool zeraPagina)
        {
            try
            {
                if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.Normal)
                {
                    lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(UCNavegacaoTelaPeriodo.VS_tpc_id, UCControleTurma1.VS_tur_id, UCControleTurma1.VS_tud_id, ApplicationWEB.AppMinutosCacheMedio);
                }

                CancelaSelect = false;

                if (zeraPagina)
                {
                    grvAluno.PageIndex = 0;
                }

                grvAluno.PageSize = UCComboQtdePaginacao1.Valor;

                odsAluno.SelectMethod = "SelecionaAlunosAtivosCOCPorTurmaDisciplina";
                odsAluno.SelectParameters.Clear();
                odsAluno.SelectParameters.Add("tud_id", UCControleTurma1.VS_tud_id.ToString());
                odsAluno.SelectParameters.Add("tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
                odsAluno.SelectParameters.Add("tipoDocente", VS_tipoDocente.ToString());
                odsAluno.SelectParameters.Add("documentoOficial", "false");
                odsAluno.SelectParameters.Add("cap_dataInicio", TypeCode.DateTime, UCNavegacaoTelaPeriodo.cap_dataInicio.ToString());
                odsAluno.SelectParameters.Add("cap_dataFim", TypeCode.DateTime, UCNavegacaoTelaPeriodo.cap_dataFim.ToString());
                odsAluno.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheMedio.ToString());
                odsAluno.SelectParameters.Add("tur_ids", UCControleTurma1.TurmasNormaisMultisseriadas.Any() ? string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString())) : string.Empty);
                grvAluno.DataBind();

                CFG_PermissaoModuloOperacao permissaoModuloOperacao = new CFG_PermissaoModuloOperacao()
                {
                    gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                    sis_id = ApplicationWEB.SistemaID,
                    mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                    pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseAnotacoesAluno)
                };
                CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloOperacao);

                bool possuiPermissaoVisualizacao = true;

                if (!permissaoModuloOperacao.IsNew && (!permissaoModuloOperacao.pmo_permissaoConsulta && !permissaoModuloOperacao.pmo_permissaoInclusao && !permissaoModuloOperacao.pmo_permissaoEdicao))
                {
                    possuiPermissaoVisualizacao = false;
                }

                grvAluno.Columns[grvAluno_ColunaAnotacoes].Visible = VS_ltPermissaoAnotacoes.Any(p => p.pdc_permissaoConsulta) && possuiPermissaoVisualizacao;
                grvAluno.Columns[grvAluno_ColunaBoletim].Visible = VS_ltPermissaoBoletim.Any(p => p.pdc_permissaoConsulta) &&
                                                                   ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_COLUNA_BOLETIM_MANUTENCAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                grvAluno.Columns[grvAluno_ColunaRelatorioPedagogico].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_COLUNA_RELATORIOPEDAGOGICO_MANUTENCAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                UpdAlunos.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega dados da tela.
        /// </summary>
        private void CarregarTela()
        {
            CarregarTela(true);
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                try
                {
                    if (PreviousPage == null && Session["DadosPaginaRetorno"] == null && Session["tud_id"] == null)
                    {
                        // Se não carregou nenhuma turma, redireciona pra busca.
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("É necessário selecionar uma turma.", UtilBO.TipoMensagem.Alerta);
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao ||
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                            RedirecionarPagina("~/Academico/ControleTurma/MinhaEscolaGestor.aspx");
                        else
                            RedirecionarPagina("~/Academico/ControleTurma/Busca.aspx");
                    }
                    else
                    {
                        List<Struct_MinhasTurmas> dadosTodasTurmas = new List<Struct_MinhasTurmas>();
                        long tud_idCompartilhada = -1;

                        if (Session["Historico"] != null)
                        {
                            VS_historico = Convert.ToBoolean(Session["Historico"]) && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0;
                            Session.Remove("Historico");
                        }
                        if (Session["TudIdCompartilhada"] != null)
                        {
                            tud_idCompartilhada = Convert.ToInt64(Session["TudIdCompartilhada"]);
                            Session.Remove("TudIdCompartilhada");
                        }
                        if (Session["tud_id"] != null && Session["tdt_posicao"] != null && Session["PaginaRetorno"] != null)
                        {
                            UCControleTurma1.VS_tud_id = Convert.ToInt64(Session["tud_id"]);
                            UCControleTurma1.VS_tdt_posicao = Convert.ToByte(Session["tdt_posicao"]);
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = Session["PaginaRetorno"].ToString();
                            if (Session["tur_tipo"] != null && Session["tur_idNormal"] != null && Session["tud_idAluno"] != null)
                            {
                                UCControleTurma1.VS_tur_tipo = Convert.ToByte(Session["tur_tipo"]);
                                UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(Session["tur_idNormal"]);
                                UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(Session["tud_idAluno"]);
                            }
                            if (VS_EntitiesControleTurma.escola == null)
                            {
                                ViewState["VS_EntitiesControleTurma"] = null;
                            }
                            UCControleTurma1.VS_esc_id = VS_EntitiesControleTurma.escola.esc_id;
                            UCControleTurma1.VS_uni_id = VS_EntitiesControleTurma.turma.uni_id;
                            UCControleTurma1.VS_tur_id = VS_EntitiesControleTurma.turma.tur_id;
                            UCControleTurma1.VS_tud_naoLancarNota = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarNota;
                            UCControleTurma1.VS_tud_naoLancarFrequencia = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarFrequencia;
                            UCControleTurma1.VS_tur_dataEncerramento = VS_EntitiesControleTurma.turma.tur_dataEncerramento;
                            UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;
                            UCControleTurma1.VS_tciIds = VS_EntitiesControleTurma.tciIds;
                            if (Session["VS_TpcId"] != null)
                                UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(Session["VS_TpcId"]);
                            if (Session["VS_TpcOrdem"] != null)
                                UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(Session["VS_TpcOrdem"]);
                        }
                        else if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                        {
                            UCControleTurma1.VS_tud_id = PreviousPage.Edit_tud_id;
                            UCControleTurma1.VS_tdt_posicao = PreviousPage.Edit_tdt_posicao;
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = PreviousPage.PaginaRetorno;
                            if (VS_EntitiesControleTurma.escola == null)
                            {
                                ViewState["VS_EntitiesControleTurma"] = null;
                            }
                            UCControleTurma1.VS_esc_id = VS_EntitiesControleTurma.escola.esc_id;
                            UCControleTurma1.VS_uni_id = VS_EntitiesControleTurma.turma.uni_id;
                            UCControleTurma1.VS_tur_id = VS_EntitiesControleTurma.turma.tur_id;
                            UCControleTurma1.VS_tud_naoLancarNota = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarNota;
                            UCControleTurma1.VS_tud_naoLancarFrequencia = VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarFrequencia;
                            UCControleTurma1.VS_tur_dataEncerramento = VS_EntitiesControleTurma.turma.tur_dataEncerramento;
                            UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;
                            UCControleTurma1.VS_tciIds = VS_EntitiesControleTurma.tciIds;
                            UCControleTurma1.VS_tur_tipo = VS_EntitiesControleTurma.turma.tur_tipo;
                        }
                        else if (Session["DadosPaginaRetorno"] != null)
                        {
                            Dictionary<string, string> listaDados = (Dictionary<string, string>)Session["DadosPaginaRetorno"];
                            UCControleTurma1.VS_tud_id = Convert.ToInt64(listaDados["Tud_idRetorno_ControleTurma"]);
                            UCControleTurma1.VS_tdt_posicao = Convert.ToByte(listaDados["Edit_tdt_posicao"]);
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = listaDados["PaginaRetorno"].ToString();
                            UCControleTurma1.VS_esc_id = Convert.ToInt32(listaDados["Edit_esc_id"]);
                            UCControleTurma1.VS_uni_id = Convert.ToInt32(listaDados["Edit_uni_id"]);
                            UCControleTurma1.VS_tur_id = Convert.ToInt64(listaDados["Edit_tur_id"]);
                            UCControleTurma1.VS_tud_naoLancarNota = Convert.ToBoolean(listaDados["Edit_tud_naoLancarNota"]);
                            UCControleTurma1.VS_tud_naoLancarFrequencia = Convert.ToBoolean(listaDados["Edit_tud_naoLancarFrequencia"]);
                            UCControleTurma1.VS_tur_dataEncerramento = Convert.ToDateTime(listaDados["Edit_tur_dataEncerramento"]);
                            UCNavegacaoTelaPeriodo.VS_cal_id = Convert.ToInt32(listaDados["Edit_cal_id"]);
                            UCControleTurma1.VS_tciIds = listaDados["Edit_tciIds"];
                            UCControleTurma1.VS_tur_tipo = Convert.ToByte(listaDados["Edit_tur_tipo"]);
                            UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(listaDados["Edit_tud_idAluno"]);
                            UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(listaDados["Edit_tur_idNormal"]);
                            UCControleTurma1.VS_tur_tud_ids = (List<string>)(Session["tur_tud_ids"] ?? new List<string>());

                            UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(listaDados["Edit_tpc_id"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(listaDados["Edit_tpc_ordem"]);
                            UCControleTurma1.LabelTurmas = listaDados["TextoTurmas"];
                        }

                        // Remove os dados que possam estar na sessao
                        Session.Remove("tud_id");
                        Session.Remove("tdt_posicao");
                        Session.Remove("PaginaRetorno");
                        Session.Remove("DadosPaginaRetorno");
                        Session.Remove("VS_DadosTurmas");
                        Session.Remove("VS_TpcId");
                        Session.Remove("tur_tipo");
                        Session.Remove("tur_idNormal");
                        Session.Remove("tud_idAluno");
                        Session.Remove("tur_tud_ids");
                        //

                        List<Struct_MinhasTurmas.Struct_Turmas> dadosTurma = new List<Struct_MinhasTurmas.Struct_Turmas>();

                        // Se for perfil Administrador
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                        {
                            dadosTodasTurmas.Add
                            (
                                new Struct_MinhasTurmas
                                {
                                    Turmas = TUR_TurmaBO.SelecionaMinhasTurmasComboPorTurId
                                                             (
                                                                 VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.MultisseriadaDocente
                                                                 ? UCControleTurma1.VS_tur_idNormal : UCControleTurma1.VS_tur_id,
                                                                 ApplicationWEB.AppMinutosCacheCurto
                                                             )
                                }
                            );

                            // Não busca pela posição
                            dadosTodasTurmas.All(p =>
                            {
                                dadosTurma.AddRange(p.Turmas.Where(t => t.tud_id == UCControleTurma1.VS_tud_id));
                                return true;
                            });

                            UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                        }
                        else
                        {
                            dadosTodasTurmas = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto, false);

                            dadosTodasTurmas.All(p =>
                            {
                                dadosTurma.AddRange(p.Turmas.Where(t => t.tud_id == UCControleTurma1.VS_tud_id && t.tdt_posicao == UCControleTurma1.VS_tdt_posicao));
                                return true;
                            });

                            VS_situacaoTurmaDisciplina = dadosTurma.FirstOrDefault().tdt_situacao;

                            UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                        }

                        VS_turmasAnoAtual = dadosTurma.FirstOrDefault().turmasAnoAtual;

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.Alunos;
                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);

                        // Carrega o combo de disciplinas e seta o valor selecionado.
                        List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmas = new List<Struct_MinhasTurmas.Struct_Turmas>();

                        dadosTodasTurmas.All(p =>
                        {
                            dadosTurmas.AddRange(p.Turmas);
                            return true;
                        });

                        // Carrega combo de turmas
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                        {
                            List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasCombo = TUR_TurmaBO.SelecionaMinhasTurmasComboPorTurId
                            (
                                VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.MultisseriadaDocente
                                ? UCControleTurma1.VS_tur_idNormal : UCControleTurma1.VS_tur_id,
                                ApplicationWEB.AppMinutosCacheCurto
                            );

                            UCControleTurma1.CarregaTurmas(dadosTurmasCombo, UCNavegacaoTelaPeriodo.VS_cal_id, VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico);
                        }
                        else
                        {
                            List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasCombo = new List<Struct_MinhasTurmas.Struct_Turmas>();

                            if (VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && VS_situacaoTurmaDisciplina == 1)
                            {
                                // dadosTurmasAtivas
                                dadosTurmasCombo = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dadosTodasTurmas, 0);
                            }
                            else
                            {
                                dadosTurmasCombo = dadosTurmas;
                            }

                            UCControleTurma1.CarregaTurmas(dadosTurmasCombo, UCNavegacaoTelaPeriodo.VS_cal_id, VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico);
                        }

                        TUR_TurmaDisciplina entDisciplinaRelacionada = null;
                        if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                        {
                            List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                        : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo);
                            if (lstDisciplinaCompartilhada.Count > 0)
                            {
                                if (tud_idCompartilhada > 0 && lstDisciplinaCompartilhada.Any(p => p.tud_id == tud_idCompartilhada))
                                {
                                    entDisciplinaRelacionada = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_idCompartilhada });
                                    VS_turmaDisciplinaRelacionada = lstDisciplinaCompartilhada.Find(p => p.tud_id == tud_idCompartilhada);
                                }
                            }
                        }

                        UCNavegacaoTelaPeriodo.CarregarPeriodos(VS_ltPermissaoFrequencia, VS_ltPermissaoEfetivacao,
                                                                VS_ltPermissaoPlanejamentoAnual, VS_ltPermissaoAvaliacao,
                                                                entDisciplinaRelacionada, UCControleTurma1.VS_esc_id,
                                                                VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, UCControleTurma1.VS_tdt_posicao, UCControleTurma1.VS_tur_id, VS_EntitiesControleTurma.turmaDisciplina.tud_id);

                        if (UCNavegacaoTelaPeriodo.VS_tpc_id <= 0)
                        {
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Escola não permite lançar dados.", UtilBO.TipoMensagem.Alerta);
                            RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                        }

                        CarregarTela();

                        //if (VS_situacaoTurmaDisciplina != 1)
                        //{
                        //bloquear botoes -> se vier do historico e a turma tiver com situação 4 (docente ja nao possui mais essa turma)
                        //UCNavegacaoTelaPeriodo.VisiblePlanejamentoAnual = false;
                        //UCNavegacaoTelaPeriodo.VisibleEfetivacao = false;
                        //}
                    }

                    bool mudaCorTitulo = VS_cal_ano < DateTime.Now.Year && VS_turmasAnoAtual && VS_EntitiesControleTurma.turma.tur_situacao == 1;

                    UCControleTurma1.CorTituloTurma = mudaCorTitulo ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black;
                    divMessageTurmaAnterior.Visible = mudaCorTitulo;
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }

            UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
            UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
            UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            UCControleTurma1.chkTurmasNormaisMultisseriadasIndexChanged += UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged;

            // Configura javascripts da tela.
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.PrintBoletim.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsControleTurma_Alunos.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));

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
                }
            }
        }

        private void uccTurmaDisciplina_IndexChanged()
        {
            try
            {
                string[] valor = UCControleTurma1.ValorTurmas.Split(';');
                if (valor.Length > 4)
                {
                    byte tud_tipo = Convert.ToByte(valor[4]);
                    bool dialogDocCompartilhada = false;
                    if (tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                    {
                        long tud_id = Convert.ToInt64(valor[1]);
                        List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                    : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, ApplicationWEB.AppMinutosCacheLongo);
                        if (lstDisciplinaCompartilhada.Count > 1)
                        {
                            UCSelecaoDisciplinaCompartilhada1.AbrirDialog(tud_id, VS_historico ? __SessionWEB.__UsuarioWEB.Docente.doc_id : 0, UCControleTurma1.TextoSelecionadoTurmas);
                            dialogDocCompartilhada = true;
                            hdnValorTurmas.Value = UCControleTurma1.ValorTurmas;
                        }
                    }
                    if (!dialogDocCompartilhada)
                    {
                        Session["tud_id"] = valor[1].ToString();
                        Session["tdt_posicao"] = valor[3].ToString();
                        Session["PaginaRetorno"] = UCNavegacaoTelaPeriodo.VS_paginaRetorno;
                        Session["VS_TpcId"] = UCNavegacaoTelaPeriodo.VS_tpc_id;
                        Session["VS_TpcOrdem"] = UCNavegacaoTelaPeriodo.VS_tpc_ordem;

                        if (valor.Length > 7)
                        {
                            Session["tur_tipo"] = valor[5].ToString();
                            Session["tur_idNormal"] = valor[6].ToString();
                            Session["tud_idAluno"] = valor[7].ToString();
                        }

                        if (VS_turmaDisciplinaRelacionada.tud_id > 0)
                        {
                            Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
                        }
                        Session["Historico"] = VS_historico;
                        Response.Redirect("~/Academico/ControleTurma/Alunos.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina(long tud_id)
        {
            try
            {
                string[] valor = hdnValorTurmas.Value.Split(';');
                if (valor.Length > 4)
                {
                    Session["tud_id"] = valor[1].ToString();
                    Session["tdt_posicao"] = valor[3].ToString();
                    Session["PaginaRetorno"] = UCNavegacaoTelaPeriodo.VS_paginaRetorno;
                    Session["VS_TpcId"] = UCNavegacaoTelaPeriodo.VS_tpc_id;
                    Session["VS_TpcOrdem"] = UCNavegacaoTelaPeriodo.VS_tpc_ordem;
                    Session["TudIdCompartilhada"] = tud_id.ToString();
                    Session["Historico"] = VS_historico;
                    Session["tur_tipo"] = valor[5].ToString();
                    Session["tur_idNormal"] = valor[6].ToString();
                    Session["tud_idAluno"] = valor[7].ToString();

                    Response.Redirect("~/Academico/ControleTurma/Alunos.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged()
        {
            try
            {
                CarregarTela();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Boletim")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]);
                    int mtu_id = Convert.ToInt32(grvAluno.DataKeys[index].Values["mtu_id"]);

                    UCBoletim.Carregar(UCNavegacaoTelaPeriodo.VS_tpc_id, Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]), Convert.ToInt32(grvAluno.DataKeys[index].Values["mtu_id"]));
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "BoletimCompletoAluno", "$(document).ready(function(){ $('#divBoletimCompleto').dialog('open'); });", true);
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
            else if (e.CommandName == "Editar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]);

                    //Limpa o cache quando abre a tela de captura de foto para não manter dados desatualizados
                    GestaoEscolarUtilBO.LimpaCache(string.Format("{0}_{1}", MTR_MatriculaTurmaDisciplinaBO.Cache_SelecionaAlunosAtivosCOCPorTurmaDisciplina, UCControleTurma1.VS_tud_id.ToString()));

                    Session.Add("alu_id", alu_id);
                    Session.Add("PaginaRetorno_CapturaFoto", "~/Academico/ControleTurma/Alunos.aspx");
                    CarregaSessionPaginaRetorno();

                    Response.Redirect("~/Academico/Aluno/CapturaFoto/Default.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir a captura de foto.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Anotacoes")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]);
                    int mtu_id = Convert.ToInt32(grvAluno.DataKeys[index].Values["mtu_id"]);

                    Session.Remove("alu_id_anotacoes");
                    Session.Remove("tud_id_anotacoes");
                    Session.Remove("PaginaRetorno_AnotacoesAluno");

                    Session.Add("alu_id_anotacoes", alu_id);
                    Session.Add("tud_id_anotacoes", UCControleTurma1.VS_tud_id);
                    Session.Add("mtu_id_anotacoes", mtu_id);
                    Session.Add("PaginaRetorno_AnotacoesAluno", "~/Academico/ControleTurma/Alunos.aspx");
                    CarregaSessionPaginaRetorno();

                    Response.Redirect("Anotacoes.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir as anotações do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "RelatorioPedagogico")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    Session.Remove("alu_id_RelatorioPedagogico");
                    Session.Remove("PaginaRetorno_RelatorioPedagogico");


                    Session.Add("alu_id_RelatorioPedagogico", Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]));
                    Session.Add("PaginaRetorno_RelatorioPedagogico", Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/ControleTurma/Alunos.aspx"));

                    CarregaSessionPaginaRetorno();
                    RedirecionarPagina("~/Documentos/RelatorioPedagogico/RelatorioPedagogico.aspx");
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o relatório pedagógico do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "RelatorioRP")
            {
                try
                {
                    Session.Remove("alu_id_RelatorioRP");
                    Session.Remove("tur_id_RelatorioRP");
                    Session.Remove("tud_id_RelatorioRP");
                    Session.Remove("tpc_id_RelatorioRP");
                    Session.Remove("PaginaRetorno_RelatorioRP");

                    Session.Add("alu_id_RelatorioRP", Convert.ToInt64(e.CommandArgument.ToString()));
                    Session.Add("tur_id_RelatorioRP", UCControleTurma1.VS_tur_id);
                    Session.Add("tud_id_RelatorioRP", UCControleTurma1.VS_tud_id);
                    Session.Add("tpc_id_RelatorioRP", UCNavegacaoTelaPeriodo.VS_tpc_id);
                    Session.Add("PaginaRetorno_RelatorioRP", Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/ControleTurma/Alunos.aspx"));

                    CarregaSessionPaginaRetorno();
                    //RedirecionarPagina("~/");
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
                    Session.Remove("alu_id_RelatorioAEE");
                    Session.Remove("tur_id_RelatorioAEE");
                    Session.Remove("tud_id_RelatorioAEE");
                    Session.Remove("tpc_id_RelatorioAEE");
                    Session.Remove("PaginaRetorno_RelatorioAEE");

                    Session.Add("alu_id_RelatorioAEE", Convert.ToInt64(e.CommandArgument.ToString()));
                    Session.Add("tur_id_RelatorioAEE", UCControleTurma1.VS_tur_id);
                    Session.Add("tud_id_RelatorioAEE", UCControleTurma1.VS_tud_id);
                    Session.Add("tpc_id_RelatorioAEE", UCNavegacaoTelaPeriodo.VS_tpc_id);
                    Session.Add("PaginaRetorno_RelatorioAEE", Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/ControleTurma/Alunos.aspx"));

                    CarregaSessionPaginaRetorno();
                    //RedirecionarPagina("~/");
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir os relatórios do AEE para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "alu_id"));
                byte situacao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "mtu_situacao"));
                long arq_idFoto = Convert.ToInt64((DataBinder.Eval(e.Row.DataItem, "arq_idFoto")) == DBNull.Value ? 0 : (DataBinder.Eval(e.Row.DataItem, "arq_idFoto")));
                bool possuiimagem = (arq_idFoto > 0);

                //Pinta a linha de acordo com a situação do aluno
                if ((MTR_MatriculaTurmaSituacao)situacao == MTR_MatriculaTurmaSituacao.Inativo)
                    e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;

                Image _imgPossuiImagem = (Image)e.Row.FindControl("_imgPossuiImagem");
                if (_imgPossuiImagem != null)
                    _imgPossuiImagem.Visible = possuiimagem;

                ImageButton _btnBoletim = (ImageButton)e.Row.FindControl("_btnBoletim");
                if (_btnBoletim != null)
                {
                    _btnBoletim.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnRelatorioPedagogico = (ImageButton)e.Row.FindControl("btnRelatorioPedagogico");
                if (btnRelatorioPedagogico != null)
                {
                    btnRelatorioPedagogico.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnCapturarFoto = (ImageButton)e.Row.FindControl("btnCapturarFoto");
                if (btnCapturarFoto != null)
                {
                    btnCapturarFoto.CommandArgument = e.Row.RowIndex.ToString();
                    btnCapturarFoto.Visible = usuarioPermissao;
                }
                ImageButton _btnDetalhes = (ImageButton)e.Row.FindControl("_btnDetalhes");
                if (_btnDetalhes != null)
                {
                    _btnDetalhes.CommandArgument = e.Row.RowIndex.ToString();
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
                if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno
                    || lstAlunosRelatorioRP.Any(p => p.alu_id == alu_id))
                {
                    LinkButton btnRelatorioRP = (LinkButton)e.Row.FindControl("btnRelatorioRP");
                    if (btnRelatorioRP != null)
                    {
                        btnRelatorioRP.Visible = true;
                        btnRelatorioRP.CommandArgument = alu_id.ToString();
                    }
                }
            }
        }

        protected void grvAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grvAluno.PageIndex = e.NewPageIndex;

                CarregarTela(false);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                UpdAlunos.Update();
            }
        }

        protected void grvAluno_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = MTR_MatriculaTurmaDisciplinaBO.GetTotalRecords();
        }

        /// <summary>
        /// Trata o numero de linhas por pagina da grid.
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                CarregarTela();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                UpdAlunos.Update();
            }
        }

        protected void odsAluno_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.Cancel = CancelaSelect;
        }

        #endregion
    }
}