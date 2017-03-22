using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class PlanejamentoAnual : MotherPageLogadoCompressedViewState
    {
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
        /// Parâmetro acadêmico que indica se há a possíbilidade do usuário replicar o planejamento para outras turmas.
        /// </summary>
        private bool ParametroReplicarTurmas
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.REPLICAR_PLANEJAMENTO_ANUAL_ENTRE_TURMAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Valor do parâmetro acadêmico PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS
        /// </summary>
        private bool ParametroOrientacoesCurricularesAula
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private byte PosicaoDocente
        {
            get
            {
                return (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada 
                        || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Compartilhado, ApplicationWEB.AppMinutosCacheLongo) == UCControleTurma1.VS_tdt_posicao)
                       || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo) == UCControleTurma1.VS_tdt_posicao
                       ? ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo) : UCControleTurma1.VS_tdt_posicao;
            }
        }

        /// <summary>
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        private long Tud_idPlanAnual
        {
            get
            {
                if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                {
                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        || VS_turmaDisciplinaRelacionada.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Regencia)
                    {
                        return VS_turmaDisciplinaRelacionada.tud_id;
                    }

                    // Disciplina de regencia no modelo de planejamento anual (nao por ciclo).
                    return Convert.ToInt64(ddlTurmaDisciplinaPlanAnual.SelectedValue);
                }
                else
                {
                string[] ids = ddlTurmaDisciplinaPlanAnual.SelectedValue.Split(';');
                return Convert.ToInt64(ids.Length > 1 ? ids[1] : "-1");
            }
        }
        }

        /// <summary>
        /// Retorna o tud_id para gerar o relatório do planejamento anual.
        /// </summary>
        private long VS_Tud_idPlanAnual
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_Tud_idPlanAnual"] ?? Tud_idPlanAnual);
            }

            set
            {
                ViewState["VS_Tud_idPlanAnual"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
        /// </summary>
        protected int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] == null || Convert.ToInt32(ViewState["VS_cal_ano"]) <= 0)
                {
                    ViewState["VS_cal_ano"] = ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_ano;
                }
                return Convert.ToInt32(ViewState["VS_cal_ano"]);
            }
        }

        protected string VS_mensagem
        {
            get
            {
                if (ViewState["VS_mensagem"] != null)
                    return ViewState["VS_mensagem"].ToString();
                return "";
            }
            set
            {
                ViewState["VS_mensagem"] = value;
            }
        }

        /// <summary>
        /// Guarda todas as entities utilizadas pela pagina
        /// </summary>
        /// <author>juliano.real</author>
        /// <datetime>05/05/2014-15:29</datetime>
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

        private byte PosicaoTitular
        {
            get
            {
                return ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo);
            }
        }

        private byte PosicaoSegundoTitular
        {
            get
            {
                return ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo);
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

        #endregion Propriedades

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

            Session["tur_tud_ids"] = UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => String.Format("{0};{1}", p.tur_id, p.tud_id)).ToList();

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);
            if (VS_turmaDisciplinaRelacionada.tud_id > 0)
            {
                Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
        }
            Session["Historico"] = VS_historico;
        }

        /// <summary>
        /// Carrega dados da tela.
        /// </summary>
        private void CarregarTela()
        {
            ckbAulasRel.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_FILTRO_AULAS_RELATORIO_PLANEJAMENTO_ANUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            ckbAvaliacoesRel.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_FILTRO_AVALIACAO_RELATORIO_PLANEJAMENTO_ANUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool permiteConsulta = false;
            if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
            {
                if (VS_turmaDisciplinaRelacionada.tud_id > 0)
                {
                    TUR_TurmaDisciplina disciplinaRelacionada = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = VS_turmaDisciplinaRelacionada.tud_id });
                    permiteConsulta = disciplinaRelacionada.tud_naoLancarPlanejamento == false
                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                }
            }
            else
            {
                permiteConsulta = VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoConsulta)
                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }

            if (permiteConsulta)
            {
                bool planejamento_anual_ciclo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    //Carrega as turmas / disciplinas do docente
                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(0, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                    var turmaDisciplina = from dr in dtTurmaDisciplina
                                          let tur_idPlan = Convert.ToInt64(dr.tur_tud_id.Split(';')[0])
                                            where tur_idPlan == UCControleTurma1.VS_tur_id
                                            select new
                                            {
                                              tur_tud_nome = dr.tur_tud_nome
                                                ,
                                              tur_tud_id = dr.tur_tud_id
                                            };

                    ddlTurmaDisciplinaPlanAnual.Items.Clear();

                    //O modelo de plano de aula projeto foi implementado para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
                    if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Regencia
                        && (VS_cal_ano < 2015 || !planejamento_anual_ciclo))
                    {
                        ddlTurmaDisciplinaPlanAnual.DataSource = (from y in turmaDisciplina.AsEnumerable()
                                                                  let tud_tipo = Convert.ToByte(y.tur_tud_id.ToString().Split(';')[3])
                                                                  where tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia
                                                                  select new
                                                                  {
                                                                    tur_tud_nome = y.tur_tud_nome.ToString()
                                                                    ,
                                                                    tur_tud_id = y.tur_tud_id.ToString()
                                                                  });
                    }
                    else if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                            && !planejamento_anual_ciclo
                            && VS_turmaDisciplinaRelacionada.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Regencia)
                    {
                        List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                        : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, true);
                        ddlTurmaDisciplinaPlanAnual.DataSource = (from y in lstDisciplinaCompartilhada.AsEnumerable()
                                                                  let tud_tipo = y.tud_tipo
                                                                  where tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia
                                                                      select new
                                                                      {
                                                                      tur_tud_nome = y.tud_nome
                                                                          ,
                                                                      tur_tud_id = y.tud_id
                                                                      });
                        }
                        else
                        {
                        ddlTurmaDisciplinaPlanAnual.DataSource = (from y in turmaDisciplina.AsEnumerable()
                                                                  where y.tur_tud_id.ToString().Split(';')[1] == UCControleTurma1.VS_tud_id.ToString()
                                                                  select new
                                                                  {
                                                                    tur_tud_nome = y.tur_tud_nome.ToString()
                                                                    ,
                                                                    tur_tud_id = y.tur_tud_id.ToString()
                                                                  });
                    }
                    ddlTurmaDisciplinaPlanAnual.DataBind();

                    if (UCControleTurma1.VS_tdt_posicao <= 0)
                    {
                        UCControleTurma1.VS_tdt_posicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id, Tud_idPlanAnual, ApplicationWEB.AppMinutosCacheLongo);
                    }
                }
                else
                {
                    //Carrega as disciplinas da turma caso a visão do usuário não seja individual.
                    ddlTurmaDisciplinaPlanAnual.Items.Clear();

                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(UCControleTurma1.VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                    var turmaDisciplina = from dr in dtTurmaDisciplina
                                          let tur_idPlan = Convert.ToInt64(dr.tur_tud_id.Split(';')[0])
                                          where tur_idPlan == UCControleTurma1.VS_tur_id
                                          select new
                                          {
                                              tur_tud_nome = dr.tur_tud_nome
                                              ,
                                              tur_tud_id = dr.tur_tud_id
                                          };

                    // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                    //O modelo de plano de aula projeto foi implementado para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
                    if (VS_EntitiesControleTurma.turmaDisciplina != null
                        && VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia)
                        && (VS_cal_ano < 2015 || !planejamento_anual_ciclo))
                    {
                        turmaDisciplina = from dr in dtTurmaDisciplina
                                          let tur_idPlan = Convert.ToInt64(dr.tur_tud_id.Split(';')[0])
                                          where tur_idPlan == UCControleTurma1.VS_tur_id
                                                && Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                          select new
                                          {
                                              tur_tud_nome = dr.tur_tud_nome
                                              ,
                                              tur_tud_id = dr.tur_tud_id
                                          };
                    }
                    else if (VS_EntitiesControleTurma.turmaDisciplina != null
                        && VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                        && !planejamento_anual_ciclo
                        && VS_turmaDisciplinaRelacionada.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Regencia)
                    {
                        List<sTurmaDisciplinaRelacionada> lstDisciplinaCompartilhada = VS_historico ? TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id)
                                                                                                        : TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(VS_EntitiesControleTurma.turmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheLongo, true);
                        ddlTurmaDisciplinaPlanAnual.DataSource = (from y in lstDisciplinaCompartilhada.AsEnumerable()
                                                                  let tud_tipo = y.tud_tipo
                                                                  where tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia
                                              select new
                                              {
                                                                      tur_tud_nome = y.tud_nome
                                                  ,
                                                                      tur_tud_id = y.tud_id
                                                                  });
                        }
                    else // Senão carrega somente a disciplina selecionada
                    {
                        turmaDisciplina = from dr in dtTurmaDisciplina
                                          let tur_idPlan = Convert.ToInt64(dr.tur_tud_id.Split(';')[0])
                                          where tur_idPlan == UCControleTurma1.VS_tur_id
                                                && Convert.ToInt64(dr.tur_tud_id.Split(';')[1]) == UCControleTurma1.VS_tud_id
                                          select new
                                          {
                                              tur_tud_nome = dr.tur_tud_nome
                                              ,
                                              tur_tud_id = dr.tur_tud_id
                                          };
                    }

                    ddlTurmaDisciplinaPlanAnual.DataSource = turmaDisciplina;
                    ddlTurmaDisciplinaPlanAnual.DataBind();

                    if (ddlTurmaDisciplinaPlanAnual.Items.Count > 0)
                        ddlTurmaDisciplinaPlanAnual.SelectedValue = ddlTurmaDisciplinaPlanAnual.Items[0].Value;
                }

                if (ddlTurmaDisciplinaPlanAnual.Items.Count <= 0)
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("A turma está fora do período de planejamento de aulas.", UtilBO.TipoMensagem.Alerta);
                    RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                }
                pnlTurmaDisciplinaPlanAnual.Visible = ddlTurmaDisciplinaPlanAnual.Items.Count > 1;

                // Carrega os dados da turma.
                CarregarPlanejamentoAnual();
            }
        }

        /// <summary>
        /// Carrega a guia de planejamento anual.
        /// </summary>
        private void CarregarPlanejamentoAnual()
        {
            byte tdt_posicao = 0;

            if (UCControleTurma1.VS_tdt_posicao > 0)
            {
                tdt_posicao = UCControleTurma1.VS_tdt_posicao;
            }
            else if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                tdt_posicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id, Tud_idPlanAnual, ApplicationWEB.AppMinutosCacheLongo);
            }
            else
            {
                tdt_posicao = PosicaoDocente;
            }

            bool visibleBotoes = false;

            if (UCPlanejamentoAnual.Visible)
            {
                if (((VS_EntitiesControleTurma.turmaDisciplina.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                        && VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoEdicao))
                    // se for docencia compartilhada, verifico se tem permissao para editar e se existe disciplina relacionada vigente
                    || (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                        && !VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarPlanejamento
                            && VS_turmaDisciplinaRelacionada.tud_id > 0))
                    && usuarioPermissao)
                {
                    //mostra o botao salvar -> se o docente possuir essa turma ou se for turma extinta
                    if (VS_situacaoTurmaDisciplina == 1 
                        || UCControleTurma1.VS_tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || UCControleTurma1.VS_tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                    {
                        visibleBotoes = true;
                    }
                }
            }
            else if (VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoEdicao) && usuarioPermissao)
            {
                //mostra o botao salvar -> se o docente possuir essa turma ou se for turma extinta
                if (VS_situacaoTurmaDisciplina == 1 
                    || UCControleTurma1.VS_tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                    || UCControleTurma1.VS_tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                {
                    visibleBotoes = true;
                }
            }

            // [Carla 21/02] Regra: Docente e usuários da escola (CP e Diretor) podem editar tudo.
            // Usuários de DRE e administrador não podem.
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || 
                __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
            {
                visibleBotoes = false;
            }

            btnSalvarPlanejamentoAnual.Visible = btnSalvarPlanejamentoAnualCima.Visible = visibleBotoes;
            btnCancelarPlanejamentoAnual.Text = btnCancelarPlanejamentoAnualCima.Text = visibleBotoes ? btnCancelarPlanejamentoAnual.Text : "Voltar";
            UCPlanejamentoProjetos.PermiteEdicao = visibleBotoes;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairPlanejamentoAnual", "var exibeMensagemSair=" + btnSalvarPlanejamentoAnual.Visible.ToString().ToLower() + ";", true);

            string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                    string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                    string.Empty;

            //O modelo de plano de aula projeto foi implementado para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
            if (VS_cal_ano >= 2015 &&
                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                btnImprimirPlanejamentoAnualCima.Visible = btnImprimirPlanejamentoAnual.Visible = UCPlanejamentoAnual.Visible = false;
                UCPlanejamentoProjetos.Visible = true;
                UCPlanejamentoProjetos.CarregarTurma(UCControleTurma1.VS_tur_id, UCNavegacaoTelaPeriodo.VS_cal_id, UCControleTurma1.VS_esc_id,
                                                     VS_EntitiesControleTurma.turma.uni_id, VS_EntitiesControleTurma.curso.cur_id,
                                                     VS_EntitiesControleTurma.curriculo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                                                     Tud_idPlanAnual, VS_EntitiesControleTurma.disciplina.tds_id, VS_EntitiesControleTurma.turmaDisciplina.tud_tipo,
                                                     tdt_posicao, UCControleTurma1.VS_tciIds, tur_ids, VS_EntitiesControleTurma.curso.tne_id);
            }
            else
            {
                btnImprimirPlanejamentoAnualCima.Visible = btnImprimirPlanejamentoAnual.Visible = UCPlanejamentoAnual.Visible = true;
                UCPlanejamentoProjetos.Visible = false;
                UCPlanejamentoAnual.CarregarTurma(UCControleTurma1.VS_tur_id, Tud_idPlanAnual, tdt_posicao, VS_EntitiesControleTurma.turma, VS_EntitiesControleTurma.turmaDisciplina, tur_ids);
                if (tdt_posicao > 0 && ParametroReplicarTurmas && VS_EntitiesControleTurma.turma.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno)
                {
                    chkTurmas.DataSource = TUR_TurmaDocenteBO.SelecionaPorTurmaDisciplinaPosicao(UCControleTurma1.VS_tur_id, UCPlanejamentoAnual.VS_cal_id, UCPlanejamentoAnual.VS_cur_id, UCPlanejamentoAnual.VS_crr_id, UCPlanejamentoAnual.VS_crp_id, Tud_idPlanAnual, tdt_posicao);
                    chkTurmas.DataBind();
                }
            }

            UCCPosicaoDocente1.VS_ltPermissao = VS_ltPermissaoPlanejamentoAnual;
            UCCPosicaoDocente1.CarregarPorParametro(VS_Tud_idPlanAnual, __SessionWEB.__UsuarioWEB.Docente.doc_id);
        }

        /// <summary>
        /// O método cria uma lista de ids das turmas para os quais o planejamento anual será replicado.
        /// </summary>
        /// <returns></returns>
        private List<long> CriarListaTurmasReplicarPlanejamentoAnual()
        {
            return ParametroReplicarTurmas ?
                   chkTurmas.Items.Cast<ListItem>()
                   .Where(item => item.Selected)
                   .Select(item => Convert.ToInt64(item.Value)).ToList() :
                   new List<long>();
        }

        /// <summary>
        /// Gera o relatório conforme os filtros.
        /// </summary>
        private void GerarRelatorioPlanejamentoAnual()
        {
            string mensagem;
            if (ValidarDatas(out mensagem))
            {
                if (ckbPlanejamentoAnualRel.Checked || ckbPlanejamentoPeriodoRel.Checked ||
                    ckbAulasRel.Checked || ckbAvaliacoesRel.Checked)
                {
                    try
                    {
                        string tud_idsComponentes = Return_tudIdsComponentes();
                        string parameter = "tud_id=" + VS_Tud_idPlanAnual
                                           + "&MostrarPlanejamentoAnual=" + ckbPlanejamentoAnualRel.Checked
                                           + "&MostrarPlanejamentoPeriodo=" + ckbPlanejamentoPeriodoRel.Checked
                                           + "&MostrarAulas=" + ckbAulasRel.Checked
                                           + "&MostrarAvaliacoes=" + ckbAvaliacoesRel.Checked
                                           + "&tdt_posicao=" + (UCCPosicaoDocente1.Visible ? UCCPosicaoDocente1.Valor : PosicaoDocente)
                                           + "&MostrarOrientacoesCurriculares=" + (
                                                    ParametroOrientacoesCurricularesAula
                                                        ? ckbPlanejamentoAnualRel.Checked
                                                        : ParametroOrientacoesCurricularesAula)
                                           + "&tud_idsComponentes=" + (string.IsNullOrEmpty(tud_idsComponentes) ? UCControleTurma1.VS_tud_id.ToString() : tud_idsComponentes);

                        if (ckbAulasRel.Checked)
                        {
                            if (!string.IsNullOrEmpty(txtInicioRel.Text))
                                parameter += "&DataInicial=" + txtInicioRel.Text;
                            else
                                parameter += "&DataInicial=" + SqlDateTime.MinValue.Value.ToShortDateString();
                            if (!string.IsNullOrEmpty(txtFimRel.Text))
                                parameter += "&DataFinal=" + txtFimRel.Text;
                            else
                                parameter += "&DataFinal=" + SqlDateTime.MaxValue.Value.ToShortDateString();
                        }

                        parameter += "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                        parameter += "&NomeComponenteCurricular=" + (string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA");

                        string report;
                        // parametro define qual relatorio vai ser utilizado
                        if (ParametroOrientacoesCurricularesAula)
                        {
                            report = ((int)ReportNameGestaoAcademica.PlanejamentoAnualOrientacoesCurriculares).ToString();

                            parameter += "&Texto_Avaliacao=" + (string)GetGlobalResourceObject("Mensagens", "MSG_AVALIACAO")
                                         + "&Texto_AvaliacaoBimestre=" + (string)GetGlobalResourceObject("Mensagens", "MSG_AVALIACAOBIMESTRE")
                                         + "&Texto_DiagnosticoInicial=" + (string)GetGlobalResourceObject("Mensagens", "MSG_DIAGNOSTICOINICIAL")
                                         + "&Texto_Recurso=" + (string)GetGlobalResourceObject("Mensagens", "MSG_RECURSOSBIMESTRE")
                                         + "&Texto_Replanejamento=" + (string)GetGlobalResourceObject("Mensagens", "MSG_REPLANEJAMENTOBIMESTRE")
                                         + "&Texto_ReplanejamentoFinal=" + (string)GetGlobalResourceObject("Mensagens", "MSG_REPLANEJAMENTOBIMESTREFINAL")
                                         + "&Texto_IntervencoesPedagogicas=" + (string)GetGlobalResourceObject("Mensagens", "MSG_INTERVENCOESPEDAGOGICASBIMESTRE")
                                         + "&Texto_RegistroIntervencoes=" + (string)GetGlobalResourceObject("Mensagens", "MSG_REGISTROINTERVENCOESBIMESTRE")
                                         + "&Texto_PropostaMetodologica=" + (string)GetGlobalResourceObject("Mensagens", "MSG_PROPOSTAMETODOLOGICA")
                                         + "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarCredencialServidorPorRelatorio(__SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)).srr_pastaRelatorios.ToString()
                                            , ApplicationWEB.LogoRelatorioSSRS)
                                         + "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();
                        }
                        else
                        {
                            report = ((int)ReportNameGestaoAcademica.GestaoAcademicaPlanejamento).ToString();

                            parameter += "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarCredencialServidorPorRelatorio(__SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)).srr_pastaRelatorios.ToString()
                                            , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();
                        }

                        CarregaSessionPaginaRetorno();

                        CFG_RelatorioBO.CallReport("Relatorios", report, parameter, HttpContext.Current);
                    }
                    catch (Exception error)
                    {
                        ApplicationWEB._GravaErro(error);
                        lblMensagemRelatorio.Text = UtilBO.GetErroMessage("Erro ao tentar gerar relatório de planejamento de aula.", UtilBO.TipoMensagem.Erro);
                    }
                }
                else
                {
                    lblMensagemRelatorio.Text = UtilBO.GetMessage("Selecione pelo menos um item.", UtilBO.TipoMensagem.Alerta);
                }
            }
            else
            {
                lblMensagemRelatorio.Text = UtilBO.GetMessage(mensagem, UtilBO.TipoMensagem.Alerta);
            }
        }

        /// <summary>
        /// Retorna tud_ids de componentes da regência como parametro de relatorio
        /// </summary>
        /// <returns></returns>
        private string Return_tudIdsComponentes()
        {
            //Carrega componentes da regência.
            List<sComboTurmaDisciplina> turmaDisciplinaComponenteRegencia = (from dr in TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(UCControleTurma1.VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio)
                                                                             where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                             select new sComboTurmaDisciplina
                                                                             {
                                                                                 tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                                 ,
                                                                                 tur_tud_id = dr.tur_tud_id.ToString()
                                                                             }).ToList();

            string tudIdsComponentes = string.Empty;

            try
            {
                DropDownList ddlItems = new DropDownList
                {
                    AppendDataBoundItems = true,
                    DataValueField = "tur_tud_id"
                };

                ddlItems.Items.Clear();
                ddlItems.DataSource = turmaDisciplinaComponenteRegencia;
                ddlItems.DataBind();

                if (ddlItems.Items.Count > 0)
                    tudIdsComponentes = string.Join(",", (from ListItem item in ddlItems.Items
                                                          select item.Value.Split(';')[1]).ToArray());
            }
            catch
            {
                return tudIdsComponentes;
            }

            return tudIdsComponentes;
        }

        /// <summary>
        /// Insere e altera os dados do planejamento anual.
        /// </summary>
        private void SalvarPlanejamentoAnual(bool PermaneceTela)
        {
            //O modelo de plano de aula projeto foi implementado para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
            if (VS_cal_ano >= 2015 &&
                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                bool ensinoInfantil = VS_EntitiesControleTurma.curso.tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                VS_mensagem = "";
                if (!ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_CICLO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    try
                    {
                        if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                            UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                            ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_dataInicio > UCControleTurma1.VS_tur_dataEncerramento)
                        {
                            throw new ValidationException("Data de início do calendário da turma é maior que a data de encerramento da turma.");
                        }

                        //Salva plano do ciclo
                        if (UCPlanejamentoProjetos.SalvarPlanoCiclo())
                            VS_mensagem = UtilBO.GetErroMessage("Plano do ciclo salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    }
                    catch (ValidationException ex)
                    {
                        VS_mensagem = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (ArgumentException ex)
                    {
                        VS_mensagem = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (DuplicateNameException ex)
                    {
                        VS_mensagem = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        VS_mensagem = UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento de ciclo.", UtilBO.TipoMensagem.Erro);
                    }
                }
                
                // se for docencia compartilhada, so pode editar o Plano anual e Plano para o aluno se a
                // configuracao da disciplina permitir a edicao do planejamento com o titular da discipina relacionada.
                if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                    // se for docencia compartilhada, verifico se tem permissao para editar e se existe disciplina relacionada vigente
                    || (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                        && !VS_EntitiesControleTurma.turmaDisciplina.tud_naoLancarPlanejamento
                        && VS_turmaDisciplinaRelacionada.tud_id > 0))
                {
                    if (!ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_ANUAL_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        try
                        {
                            if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                                UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                                ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_dataInicio > UCControleTurma1.VS_tur_dataEncerramento)
                            {
                                throw new ValidationException("Data de início do calendário da turma é maior que a data de encerramento da turma.");
                            }

                            //Salva plano anual
                            if ((!(VS_visaoDocente && UCControleTurma1.VS_tdt_posicao != PosicaoTitular && UCControleTurma1.VS_tdt_posicao != PosicaoSegundoTitular)) &&
                                UCPlanejamentoProjetos.SalvarPlanoAnual())
                                VS_mensagem += UtilBO.GetErroMessage("Plano anual salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                        catch (ValidationException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (ArgumentException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (DuplicateNameException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento anual.", UtilBO.TipoMensagem.Erro);
                        }
                    }

                    if (!ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_ALUNO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        try
                        {
                            //Salva plano anual
                            if ((!(VS_visaoDocente && UCControleTurma1.VS_tdt_posicao != PosicaoTitular && UCControleTurma1.VS_tdt_posicao != PosicaoSegundoTitular)) &&
                                UCPlanejamentoProjetos.SalvarPlanoAluno())
                                VS_mensagem += UtilBO.GetErroMessage("Planejamento de aluno salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                        catch (ValidationException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (ArgumentException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (DuplicateNameException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento de aluno.", UtilBO.TipoMensagem.Erro);
                        }
                    }
                }

                if (UCPlanejamentoProjetos.abaObjAprendVisivel)
                {
                    try
                    {
                        if (UCPlanejamentoProjetos.rptObjetosVisible)
                        {
                            UCPlanejamentoProjetos.SalvarObjetoAprendizagemTurmaDisciplina();
                            VS_mensagem += UtilBO.GetErroMessage("Objetos de aprendizagem salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                        if (PermaneceTela)
                            UCPlanejamentoProjetos.CarregarObjetosAprendizagem();
                    }
                    catch (ValidationException ex)
                    {
                        VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (ArgumentException ex)
                    {
                        VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (DuplicateNameException ex)
                    {
                        VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar objetos de aprendizagem.", UtilBO.TipoMensagem.Erro);
                    }
                }

                lblMessage.Text = VS_mensagem;
            }
            else
            {
                try
                {
                    VS_mensagem = "";

                    if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                        UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                        ACA_CalendarioAnualBO.SelecionaPorTurma(UCControleTurma1.VS_tur_id).cal_dataInicio > UCControleTurma1.VS_tur_dataEncerramento)
                    {
                        throw new ValidationException("Data de início do calendário da turma é maior que a data de encerramento da turma.");
                    }

                    //Salva os planejamentos digitados.
                    if (CLS_PlanejamentoOrientacaoCurricularBO.SalvaPlanejamentoTurmaDisciplina
                        (
                            UCPlanejamentoAnual.CriarListaPlanejamento(),
                            UCPlanejamentoAnual.CriarListaHabilidades(),
                            UCPlanejamentoAnual.CriarListaDiagnostico(),
                            CriarListaTurmasReplicarPlanejamentoAnual(),
                            ParametroReplicarTurmas,
                            UCControleTurma1.VS_tur_id
                         ))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Planejamento anual | " + "tur_id: " + UCControleTurma1.VS_tur_id + ";tud_id: " + UCControleTurma1.VS_tud_id);

                        if (PermaneceTela)
                            CarregarPlanejamentoAnual();

                        VS_mensagem += UtilBO.GetErroMessage("Planejamento salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento.", UtilBO.TipoMensagem.Erro);
                    }

                    if (UCPlanejamentoProjetos.abaObjAprendVisivel)
                    {
                        try
                        {
                            if (UCPlanejamentoProjetos.rptObjetosVisible)
                            {
                                UCPlanejamentoProjetos.SalvarObjetoAprendizagemTurmaDisciplina();
                                VS_mensagem += UtilBO.GetErroMessage("Objetos de aprendizagem salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);
                            }
                            if (PermaneceTela)
                                UCPlanejamentoProjetos.CarregarObjetosAprendizagem();
                        }
                        catch (ValidationException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (ArgumentException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (DuplicateNameException ex)
                        {
                            VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (Exception ex)
                        {
                            ApplicationWEB._GravaErro(ex);
                            VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar objetos de aprendizagem.", UtilBO.TipoMensagem.Erro);
                        }
                    }
                }
                catch (ValidationException ex)
                {
                    VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (ArgumentException ex)
                {
                    VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (DuplicateNameException ex)
                {
                    VS_mensagem += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    VS_mensagem += UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento.", UtilBO.TipoMensagem.Erro);
                }

                if (PermaneceTela)
                    lblMessage.Text = VS_mensagem;
                else
                {
                    __SessionWEB.PostMessages = VS_mensagem;
                    Response.Redirect(UCNavegacaoTelaPeriodo.VS_paginaRetorno, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        private void UCPlanejamentoProjetos_SalvarPlanoCiclo()
        {
            //Caso tenha alguma mensagem de erro ou validação ao salvar o ciclo exibe;
            if (!string.IsNullOrEmpty(UCPlanejamentoProjetos.sMensagemErroPlanoCiclo))
            {
                VS_mensagem += UCPlanejamentoProjetos.sMensagemErroPlanoCiclo;
                lblMessage.Text = VS_mensagem;
                updMensagem.Update();
            }

        }

        private void UCPlanejamentoProjetos_ReplicaPlanoAnual()
        {
            //Salva plano anual
            if (UCPlanejamentoProjetos.ReplicarPlanoAnual())
            {
                VS_mensagem += UtilBO.GetErroMessage("Plano anual replicado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                lblMessage.Text = VS_mensagem;
                updMensagem.Update();
            }
        }

        /// <summary>
        /// Valida as datas do relatório de aulas com o período informado.
        /// </summary>
        /// <param name="mensagem">Mensagem de erro.</param>
        private bool ValidarDatas(out string mensagem)
        {
            mensagem = "";
            try
            {
                if (ckbAulasRel.Checked)
                {
                    if (ckbPeriodoRel.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtInicioRel.Text))
                        {
                            DateTime dtIni;
                            if (!DateTime.TryParse(txtInicioRel.Text.Trim(), out dtIni))
                                throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData("Início do período de aulas"));
                        }
                        if (!string.IsNullOrEmpty(txtFimRel.Text))
                        {
                            DateTime dtFim;
                            if (!DateTime.TryParse(txtFimRel.Text.Trim(), out dtFim))
                                throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData("Fim do período de aulas"));
                        }
                        if (!string.IsNullOrEmpty(txtInicioRel.Text) && !string.IsNullOrEmpty(txtFimRel.Text))
                        {
                            DateTime dtIni = Convert.ToDateTime(txtInicioRel.Text);
                            DateTime dtFim = Convert.ToDateTime(txtFimRel.Text);

                            if (dtIni > dtFim)
                                throw new ValidationException("Início do período de aulas não pode ser maior que o fim do período de aulas.");
                        }
                    }
                }

                return true;
            }
            catch (ValidationException ex)
            {
                mensagem = ex.Message;
                return false;
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
                        RedirecionarPagina("~/Academico/ControleTurma/PlanejamentoAnual.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void uccDisciplinaCompartilhada_IndexChanged()
        {
            try
            {
                string[] valor = UCControleTurma1.ValorDisciplinaCompartilhada.Split(';');
                if (valor.Length > 0)
                {
                    VS_turmaDisciplinaRelacionada = new sTurmaDisciplinaRelacionada { tud_id = Convert.ToInt64(valor[0]) };
                    CarregaSessionPaginaRetorno();
                    RedirecionarPagina("~/Academico/ControleTurma/PlanejamentoAnual.aspx");
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

                    RedirecionarPagina("~/Academico/ControleTurma/PlanejamentoAnual.aspx");
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

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "var corPlanejado='" + ApplicationWEB.OrientacaoCurricularPlanejada + "';" +
                            "var corTrabalhado='" + ApplicationWEB.OrientacaoCurricularTrabalhada + "';";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Cores", script, true);

            script = "var idckbPeriodo = '#" + ckbPeriodoRel.ClientID + "';" +
                     "var idckbTodasAulas = '#" + ckbTodasAulas.ClientID + "';" +
                     "var idTxtInicio = '#" + txtInicioRel.ClientID + "';" +
                     "var idTxtFim = '#" + txtFimRel.ClientID + "';";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ItensImpressaoRelatorio", script, true);

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
                            UCControleTurma1.VS_esc_id = PreviousPage.Edit_esc_id;
                            UCControleTurma1.VS_uni_id = PreviousPage.Edit_uni_id;
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
                            UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(listaDados["Edit_tpc_id"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(listaDados["Edit_tpc_ordem"]);
                            UCControleTurma1.VS_tur_tud_ids = (List<string>)(Session["tur_tud_ids"] ?? new List<string>());
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

                        UCControleTurma1.VS_tur_situacao = VS_EntitiesControleTurma.turma.tur_situacao;
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

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.PlanejamentoAnual;

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
                            bool docenciaCompartilhadaOk = false;
                            if (lstDisciplinaCompartilhada.Count > 0)
                            {
                                if (tud_idCompartilhada <= 0 || !lstDisciplinaCompartilhada.Any(p => p.tud_id == tud_idCompartilhada))
                                {
                                    tud_idCompartilhada = lstDisciplinaCompartilhada[0].tud_id;
                                }

                                if (tud_idCompartilhada > 0)
                                {
                                    docenciaCompartilhadaOk = true;
                                    entDisciplinaRelacionada = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_idCompartilhada });
                                    VS_turmaDisciplinaRelacionada = lstDisciplinaCompartilhada.Find(p => p.tud_id == tud_idCompartilhada);
                                    UCControleTurma1.CarregarDisciplinaCompartilhada(lstDisciplinaCompartilhada, VS_turmaDisciplinaRelacionada.tud_id, VS_turmaDisciplinaRelacionada.tdr_id);
                                    // Se a situação da disciplina é ativa para o docente, 
                                    // considera a situação do relacionamento com a disciplina compartilhada
                                    // para habilitar a edição.
                                    if (VS_situacaoTurmaDisciplina == 1)
                                    {
                                        VS_situacaoTurmaDisciplina = VS_turmaDisciplinaRelacionada.tdr_situacao;
                                    }
                                }
                            }

                            if (!docenciaCompartilhadaOk)
                            {
                                __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("{0} {1} - {2}.",
                                                                                    GetGlobalResourceObject("Mensagens", "MSG_SEM_RELACIONAMENTO_DOCENCIA_COMPARTILHADA").ToString()
                                                                                    , VS_EntitiesControleTurma.turma.tur_codigo
                                                                                    , VS_EntitiesControleTurma.turmaDisciplina.tud_nome)
                                                                                , UtilBO.TipoMensagem.Alerta);
                                RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                            }

                            this.UCPlanejamentoAnual.VS_turmaDisciplinaCompartilhada = VS_EntitiesControleTurma.turmaDisciplina;
                            this.UCPlanejamentoProjetos.VS_turmaDisciplinaCompartilhada = VS_EntitiesControleTurma.turmaDisciplina;
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

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                    script = "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
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

                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmerEfetivacao.js"));
                }
                else
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                }
                //sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tablesorter.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.metadata.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.treeview.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetaMensagemCampos.js"));
                //O modelo de plano de aula projeto foi implementado para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
                if (VS_cal_ano < 2015 ||
                    !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroPlanejamentoAnualHabilidade.js"));
                else
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsUCPlanejamentoProjetos.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                    !Convert.ToString(btnCancelarPlanejamentoAnual.CssClass).Contains("btnMensagemUnload"))
                    btnCancelarPlanejamentoAnual.CssClass += " btnMensagemUnload";

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                    !Convert.ToString(btnCancelarPlanejamentoAnualCima.CssClass).Contains("btnMensagemUnload"))
                    btnCancelarPlanejamentoAnualCima.CssClass += " btnMensagemUnload";
            }

            UCNavegacaoTelaPeriodo.DesabilitarPeriodos();

            UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
            UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
            UCControleTurma1.DisciplinaCompartilhadaIndexChanged = uccDisciplinaCompartilhada_IndexChanged;
            UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
            UCPlanejamentoProjetos.ReplicaPlanoAnual += UCPlanejamentoProjetos_ReplicaPlanoAnual;
            UCPlanejamentoProjetos.SalvarPlanoCicloDelegate += UCPlanejamentoProjetos_SalvarPlanoCiclo;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            UCControleTurma1.chkTurmasNormaisMultisseriadasIndexChanged += UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged;
        }

        protected void btnCancelarPlanejamentoAnual_Click(object sender, EventArgs e)
        {
            Response.Redirect(UCNavegacaoTelaPeriodo.VS_paginaRetorno, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            GerarRelatorioPlanejamentoAnual();
        }

        protected void btnImprimirPlanejamentoAnual_Click(object sender, EventArgs e)
        {
            try
            {
                TUR_TurmaDisciplina entityTud = new TUR_TurmaDisciplina { tud_id = Tud_idPlanAnual };
                TUR_TurmaDisciplinaBO.GetEntity(entityTud);

                if (entityTud.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia ||
                    entityTud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    grvDisciplinas.DataSource = TUR_TurmaDisciplinaBO.SelecionaDisciplinasDocentesPorTurma(UCControleTurma1.VS_tur_id, __SessionWEB.__UsuarioWEB.Docente.doc_id,
                                                                                                           __SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Ativo);
                    grvDisciplinas.DataBind();
                    grvDisciplinas.Visible = true;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "SelecionaDisciplina", "$(document).ready(function(){  $('.divSelecionaDisciplina').dialog('open');});", true);
                }
                else
                {
                    VS_Tud_idPlanAnual = Tud_idPlanAnual;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FiltrosRelatório", "$(document).ready(function(){ $('#divFiltrosRelatorio').dialog('open');});", true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar imprimir relatório de planejamento.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void btnReplicar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarPlanejamentoAnual(ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            }
        }

        protected void btnSalvarPlanejamentoAnual_Click(object sender, EventArgs e)
        {
            if (ParametroReplicarTurmas && chkTurmas.Items.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ReplicarPlanejamento", "$(document).ready(function() { $('#divReplicarPlanejamento').dialog('open'); return false; });", true);
            }
            else if (Page.IsValid)
            {
                SalvarPlanejamentoAnual(ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            }
        }

        protected void ddlTurmaDisciplinaPlanAnual_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarPlanejamentoAnual();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvDisciplinas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = int.Parse(e.CommandArgument.ToString());
            VS_Tud_idPlanAnual = Convert.ToInt64(grvDisciplinas.DataKeys[index].Values["tud_id"]);

            if (e.CommandName == "Imprimir")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FiltrosRelatório", "$(document).ready(function(){ $('#divFiltrosRelatorio').dialog('open');});", true);
            }
        }

        protected void grvDisciplinas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnSelecionar = (LinkButton)e.Row.FindControl("btnSelecionar");
                if (btnSelecionar != null)
                {
                    btnSelecionar.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }
        #endregion Eventos
    }
}