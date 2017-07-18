using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.IO;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class Listao : MotherPageLogadoCompressedViewState
    {
        #region Structs

        /// <summary>
        /// Estrutura usada para guardar as notas de relatório.
        /// </summary>
        [Serializable]
        private struct NotasRelatorio
        {
            public string Id;
            public long alu_id;
            public int tnt_id;
            public int mtu_id;
            public string valor;
            public string arq_idRelatorio;
        }

        [Serializable]
        private struct NotasRelatorioAtiExtra
        {
            public string Id;
            public long alu_id;
            public int tae_id;
            public int mtu_id;
            public string valor;
        }

        /// <summary>
        /// Estrutura que indica se uma atividade possui alunos com nota lançada.
        /// </summary>
        private struct AtividadeIndicacaoNota
        {
            public long tud_id { get; set; }

            public int tnt_id { get; set; }

            public bool PossuiNota { get; set; }
        }

        #endregion Structs

        #region Propriedades

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
                                ViewState["VS_ltPermissaoCompensacao"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Compensacoes)
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
        /// Lista de permissões do docente para cadastro de plano de aula.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoPlanoAula
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoPlanoAula"] ??
                            (
                                ViewState["VS_ltPermissaoPlanoAula"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.PlanoAula)
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
        /// Lista de permissões do docente para cadastro de avaliações.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoAtividadeExtraclasse
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoAtividadeExtraclasse"] ??
                            (
                                ViewState["VS_ltPermissaoAtividadeExtraclasse"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.AtividadesExtraClasse)
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
        /// Guarda as notas de relatório.
        /// </summary>
        private List<NotasRelatorio> VS_Nota_Relatorio
        {
            get
            {
                if (ViewState["VS_Nota_Relatorio"] == null)
                    ViewState["VS_Nota_Relatorio"] = new List<NotasRelatorio>();
                return (List<NotasRelatorio>)(ViewState["VS_Nota_Relatorio"]);
            }
            set
            {
                ViewState["VS_Nota_Relatorio"] = value;
            }
        }

        /// <summary>
        /// Guarda as notas de relatório.
        /// </summary>
        private List<NotasRelatorioAtiExtra> VS_Nota_RelatorioAtiExtra
        {
            get
            {
                if (ViewState["VS_Nota_RelatorioAtiExtra"] == null)
                    ViewState["VS_Nota_RelatorioAtiExtra"] = new List<NotasRelatorioAtiExtra>();
                return (List<NotasRelatorioAtiExtra>)(ViewState["VS_Nota_RelatorioAtiExtra"]);
            }
            set
            {
                ViewState["VS_Nota_RelatorioAtiExtra"] = value;
            }
        }

        /// <summary>
        /// Retorna o valor do parâmetro que informa se será exibida a coluna de nota final no lançamento
        /// de avaliações.
        /// </summary>
        private bool Vs_calcula_notaFinal
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        protected int VS_fav_id
        {
            get
            {
                if (VS_EntitiesControleTurma.turma != null)
                {
                    return VS_EntitiesControleTurma.turma.fav_id;
                }
                return -1;
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
        /// Armazena disciplina componentes da regência para docente em ViewState.
        /// </summary>
        private List<sComboTurmaDisciplina> VS_DisciplinaComponenteDocente
        {
            get
            {
                if (ViewState["VS_DisciplinaComponenteDocente"] != null)
                    return (List<sComboTurmaDisciplina>)(ViewState["VS_DisciplinaComponenteDocente"]);
                return new List<sComboTurmaDisciplina>();
            }
            set
            {
                ViewState["VS_DisciplinaComponenteDocente"] = value;
            }
        }

        /// <summary>
        /// Armazena a quantidade de dias
        /// que deve ser exibida, alem da semana atual
        /// </summary>
        private int qtdeSemanas
        {
            get
            {
                if (ViewState["qtdeSemanas"] != null)
                    return Convert.ToInt32(ViewState["qtdeSemanas"]);
                return 0;
            }
            set
            {
                ViewState["qtdeSemanas"] = value;
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
        /// Guarda os eventos cadastrados para a turma e calendário.
        /// </summary>
        private List<ACA_Evento> VS_ListaEventos
        {
            get
            {
                return
                    (List<ACA_Evento>)
                    (
                        ViewState["VS_ListaEventos"] ??
                        (
                            ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(UCNavegacaoTelaPeriodo.VS_cal_id
                                                                                                  , UCControleTurma1.VS_tur_id
                                                                                                  , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                                                                                                  , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                                                                  , ApplicationWEB.AppMinutosCacheLongo
                                                                                                  , false)
                        )
                    );
            }
        }
        
        /// <summary>
        /// Propriedade que indica o COC está fechado para lançamento.
        /// </summary>
        private bool VS_Periodo_Aberto
        {
            get
            {
                DateTime dataAtual = DateTime.Now.Date;

                bool aberto = (dataAtual.Date <= UCNavegacaoTelaPeriodo.cap_dataFim.Date
                                && dataAtual.Date >= UCNavegacaoTelaPeriodo.cap_dataInicio.Date)
                                || VS_ListaEventos.Exists(p => p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id &&
                                                            p.evt_dataInicio <= dataAtual.Date && p.evt_dataFim >= dataAtual.Date &&
                                                            p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS,
                                                                                                                                 __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                return aberto;
            }
        }

        /// <summary>
        /// Informa se o período já foi fechado (evento de fechamento já acabou) e não há nenhum evento de fechamento por vir.
        /// Se o período ainda estiver ativo então não verifica o evento de fechamento
        /// </summary>
        private bool VS_PeriodoEfetivado
        {
            get
            {
                bool efetivado = false;

                //Se o bimestre está ativo ou nem começou então não bloqueia pelo evento de fechamento
                if (DateTime.Today <= UCNavegacaoTelaPeriodo.cap_dataFim)
                    return false;

                //Só permite editar o bimestre se tiver evento ativo
                efetivado = !VS_ListaEventos.Exists(p => p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                                    DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

                return efetivado;
            }
        }

        /// <summary>
        /// Valor do parâmetro acadêmico PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS
        /// </summary>
        private bool ParametroPermitirAtividadesExclusivas
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Retorna o tud_id selecionado no combo ddlComponenteListao.
        /// Valores do combo:
        /// [0] - Tur_id
        /// [1] - Tud_id
        /// [2] - Permissão
        /// [3] - Tud_tipo
        /// </summary>
        private long ddlComponenteListao_Tud_Id_Selecionado
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlComponenteListao.SelectedValue))
                {
                    string[] valores = ddlComponenteListao.SelectedValue.Split(';');

                    if (valores.Length > 1)
                    {
                        return Convert.ToInt64(valores[1]);
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Calcula a quantidade de casas decimais da variação de notas.
        /// </summary>
        private int NumeroCasasDecimais
        {
            get
            {
                int numeroCasasDecimais = 1;
                if (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica != null)
                {
                    string variacao = Convert.ToDouble(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao).ToString();
                    if (variacao.IndexOf(",") >= 0)
                    {
                        numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                    }
                }

                return numeroCasasDecimais;
            }
        }

        /// <summary>
        /// Variável global que armazena a média final dos alunos
        /// </summary>
        private List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> alunoMediaFinal;

        /// <summary>
        /// Tabela com atividades da disciplina e período, junto com as notas dos alunos nas atividades.
        /// </summary>
        private DataTable DTAtividades;

        private List<ACA_EscalaAvaliacaoParecer> ltPareceres;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceres
        {
            get
            {
                return ltPareceres ??
                    (
                        ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_id)
                    );
            }
        }

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        private bool usuarioPermissao
        {
            get
            {
                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                {
                    return false;
                }

                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// Retorna um booleano informando se o tipo da disciplina selecionada é Regencia.
        /// </summary>
        public bool DisciplinaRegencia
        {
            get
            {
                if (EntTurmaDisciplina != null)
                {
                    return (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia);
                }
                return false;
            }
        }

        /// <summary>
        /// Retorna um booleano informando se o tipo da disciplina selecionada é Experiencia.
        /// </summary>
        public bool DisciplinaExperiencia
        {
            get
            {
                if (EntTurmaDisciplina != null)
                {
                    return (EntTurmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia);
                }
                return false;
            }
        }

        private long Alu_id;
        private Int32 Mtu_id;

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

        private List<AtividadeIndicacaoNota> ltAtividadeIndicacaoNota;

        private TUR_TurmaDisciplina EntTurmaDisciplina
        {
            get
            {
                return VS_EntitiesControleTurma.turmaDisciplina;
            }
        }

        private byte PosicaoDocente
        {
            get
            {
                return UCControleTurma1.VS_tdt_posicao;
            }
        }

        private bool PermiteLancarNota
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarNota && VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoEdicao);
            }
        }

        private bool PermiteLancarFrequencia
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoEdicao);
            }
        }

        private bool PermiteLancarPlanoAula
        {
            get
            {
                //A aba plano de aula foi implementada para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
                return VS_ltPermissaoPlanoAula.Any(p => p.pdc_permissaoEdicao) &&
                       VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private bool PermiteVisualizarNota
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarNota && VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoConsulta);
            }
        }

        private bool PermiteVisualizarFrequencia
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoConsulta);
            }
        }

        private bool PermiteVisualizarPlanoAula
        {
            get
            {
                //A aba plano de aula foi implementada para 2015, qualquer turma de calendário anterior à 2015 não deve mostrar essa aba nova
                return VS_ltPermissaoPlanoAula.Any(p => p.pdc_permissaoConsulta) &&
                       VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private bool PermiteLancarCompensacao
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoEdicao) && !VS_PeriodoEfetivado;
            }
        }

        private bool PermiteVisualizarCompensacao
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarFrequencia && VS_ltPermissaoCompensacao.Any(p => p.pdc_permissaoConsulta);
            }
        }

        private bool PermiteLancarAtividadeExtraclasse
        {
            get
            {
                return !EntTurmaDisciplina.tud_naoLancarNota && VS_ltPermissaoAtividadeExtraclasse.Any(p => p.pdc_permissaoEdicao);
            }
        }


        private List<sComboTurmaDisciplina> dtTurmaDisciplinaDoc
        {
            get
            {
                if (dtTurmaDisciplinaAux == null)
                    dtTurmaDisciplinaAux = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(0, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheLongo);
                return dtTurmaDisciplinaAux;
            }
        }

        private List<sComboTurmaDisciplina> dtTurmaDisciplinaTud
        {
            get
            {
                if (dtTurmaDisciplinaAux == null)
                    dtTurmaDisciplinaAux = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(UCControleTurma1.VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheLongo);
                return dtTurmaDisciplinaAux;
            }
        }

        private List<CLS_TurmaAulaPlanoDisciplina> dtTurmaAulaPlanoDisc
        {
            get
            {
                if (dtTurmaAulaPlanoDiscAux == null)
                    dtTurmaAulaPlanoDiscAux = CLS_TurmaAulaPlanoDisciplinaBO.SelectBy_tud_id(EntTurmaDisciplina.tud_id);
                return dtTurmaAulaPlanoDiscAux;
            }
        }

        private List<sComboTurmaDisciplina> dtTurmaDisciplinaAux;

        private List<CLS_TurmaAulaPlanoDisciplina> dtTurmaAulaPlanoDiscAux;

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
        /// ViewState que armazena a data e hora que carregou a TurmaAula
        /// </summary>
        private DateTime VS_Data_Listao_TurmaAula
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Listao_TurmaAula"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Listao_TurmaAula"] = value;
            }
        }

        /// <summary>
        /// Propriedade que armazena as aulas que foram salvas.
        /// </summary>
        private List<int> LstTauSalvas { get; set; }

        /// <summary>
        /// ViewState que armazena a data e hora que carregou a TurmaNota
        /// </summary>
        private DateTime VS_Data_Listao_TurmaNota
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Listao_TurmaNota"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Listao_TurmaNota"] = value;
            }
        }

        /// <summary>
        /// Informa se irá recarregar a data da Aula após salvar os dados.
        /// </summary>
        private bool VS_recarregarDataAula
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_recarregarDataAula"] ?? false);
            }
            set
            {
                ViewState["VS_recarregarDataAula"] = value;
            }
        }

        /// <summary>
        /// Informa se irá recarregar a data da Nota após salvar os dados.
        /// </summary>
        private bool VS_recarregarDataNota
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_recarregarDataNota"] ?? false);
            }
            set
            {
                ViewState["VS_recarregarDataNota"] = value;
            }
        }

        private bool permiteEdicao;

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

        /// <summary>
        /// ViewState que armazena a página da frequência do listão.
        /// </summary>
        public int VS_paginaFreq
        {
            get
            {
                if (ViewState["VS_paginaFreq"] == null)
                    ViewState["VS_paginaFreq"] = 1;
                return Convert.ToInt32(ViewState["VS_paginaFreq"]);
            }

            set
            {
                ViewState["VS_paginaFreq"] = value;
            }
        }

        public DataTable DTAtividadeExtraclasse;

        private long Alu_idExtraClasse;
        private int Mtu_idExtraClasse;

        private long VS_tud_idAtiExtraExcluir
        {
            get
            {
                if (ViewState["VS_tud_idAtiExtraExcluir"] == null)
                    ViewState["VS_tud_idAtiExtraExcluir"] = 1;
                return Convert.ToInt64(ViewState["VS_tud_idAtiExtraExcluir"]);
            }

            set
            {
                ViewState["VS_tud_idAtiExtraExcluir"] = value;
            }
        }

        private int VS_tae_idAtiExtraExcluir
        {
            get
            {
                if (ViewState["VS_tae_idAtiExtraExcluir"] == null)
                    ViewState["VS_tae_idAtiExtraExcluir"] = 1;
                return Convert.ToInt32(ViewState["VS_tae_idAtiExtraExcluir"]);
            }

            set
            {
                ViewState["VS_tae_idAtiExtraExcluir"] = value;
            }
        }

        private Guid VS_taer_idAtiExtraExcluir
        {
            get
            {
                if (ViewState["VS_taer_idAtiExtraExcluir"] == null)
                    ViewState["VS_taer_idAtiExtraExcluir"] = Guid.Empty;
                return (Guid)ViewState["VS_taer_idAtiExtraExcluir"];
            }

            set
            {
                ViewState["VS_taer_idAtiExtraExcluir"] = value;
            }
        }

        private List<Struct_PreenchimentoAluno> lstAlunosRelatorioRP = new List<Struct_PreenchimentoAluno>();

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

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);
            if (VS_turmaDisciplinaRelacionada.tud_id > 0)
            {
                Session["TudIdCompartilhada"] = VS_turmaDisciplinaRelacionada.tud_id.ToString();
            }
            Session["Historico"] = VS_historico;

            Session["tur_tud_ids"] = UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => String.Format("{0};{1}", p.tur_id, p.tud_id)).ToList();
        }

        /// <summary>
        /// Carrega dados da tela.
        /// </summary>
        private void CarregarTela()
        {
            try
            {
                VS_paginaFreq = 1;

                hdbtnCompensacaoAusenciaVisible.Value = PermiteLancarCompensacao.ToString();

                if (DisciplinaExperiencia)
                {
                    UCLancamentoFrequenciaTerritorio.TextoMsgParecer = UtilBO.GetErroMessage(UCLancamentoFrequenciaTerritorio.TextoMsgParecer, UtilBO.TipoMensagem.Informacao);
                }
                else
                {
                    UCLancamentoFrequencia.TextoMsgParecer = UtilBO.GetErroMessage(UCLancamentoFrequencia.TextoMsgParecer, UtilBO.TipoMensagem.Informacao);
                }

                if (Vs_calcula_notaFinal && VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica
                    && PermiteLancarNota && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && VS_EntitiesControleTurma.formatoAvaliacao.fav_exibirBotaoSomaMedia)
                {
                    lblMsgInfo.Text = UtilBO.GetErroMessage("Professor, mesmo utilizando as opções de cálculo da nota final, a coluna Nota Final está aberta para edição.", UtilBO.TipoMensagem.Informacao);
                    lblMsgInfo.Visible = divFormatoCalculo.Visible = true;
                }
                else
                {
                    divFormatoCalculo.Visible = false;
                    lblMsgInfo.Visible = false;
                }

                if (EntTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                {
                    string msgParecer = "";

                    switch (VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia)
                    {
                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia:
                            msgParecer = UtilBO.GetErroMessage("Marque apenas os dias de aula que o aluno não assistiu.<br>" +
                                     "Marque a opção Efetivado para indicar que o lançamento de frequência do " +
                                     "dia foi finalizado e todas as ausências foram apontadas.", UtilBO.TipoMensagem.Informacao);
                            break;

                        case (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula:
                            msgParecer = UtilBO.GetErroMessage("Cada caixa de marcação corresponde a um tempo de aula.<br>" +
                                     "Marque apenas os tempos de aula que o aluno não assistiu em cada dia.<br/>" +
                                     "Marque a opção Efetivado para indicar que o lançamento de frequência do " +
                                     "dia foi finalizado e todas as ausências foram apontadas.", UtilBO.TipoMensagem.Informacao);
                            break;
                    }

                    if (DisciplinaExperiencia)
                    {
                        UCLancamentoFrequenciaTerritorio.TextoMsgParecer = msgParecer;
                    }
                    else
                    {
                        UCLancamentoFrequencia.TextoMsgParecer = msgParecer;
                    }


                }

                bool permissaoModuloAlteracaoInfantil = true;
                if (VS_EntitiesControleTurma.curso.tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    CFG_PermissaoModuloOperacao permissaoModuloLancamentoFrequenciaInfantil = new CFG_PermissaoModuloOperacao()
                    {
                        gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        sis_id = ApplicationWEB.SistemaID,
                        mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                        pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequenciaInfantil)
                    };
                    CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloLancamentoFrequenciaInfantil);
                    permissaoModuloAlteracaoInfantil = permissaoModuloLancamentoFrequenciaInfantil.IsNew || permissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;
                }

                btnCompensacaoAusencia.Visible = PermiteLancarCompensacao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && permissaoModuloAlteracaoInfantil;

                permiteEdicao = false;
                divListao.Visible = true;

                if (DisciplinaExperiencia)
                {

                    aFrequencia.Visible = pnlListaoLancamentoFrequencias.Visible = PermiteVisualizarFrequencia;
                    if (aFrequencia.Visible)
                    {
                        UCLancamentoFrequencia.Visible = false;
                        UCLancamentoFrequenciaTerritorio.Visible = true;
                        CarregarListaoFrequenciaTerritorio(true, false, false, false);
                    }

                    aAvaliacao.Visible = pnlLancamentoAvaliacao.Visible = rptAlunosAvaliacao.Visible = false;
                }
                else
                {
                    aFrequencia.Visible = pnlListaoLancamentoFrequencias.Visible = PermiteVisualizarFrequencia;
                    if (aFrequencia.Visible)
                    {
                        UCLancamentoFrequenciaTerritorio.Visible = false;
                        UCLancamentoFrequencia.Visible = true;
                        CarregarListaoFrequencia(true, false, false, false);
                    }

                    aAvaliacao.Visible = pnlLancamentoAvaliacao.Visible = rptAlunosAvaliacao.Visible = PermiteVisualizarNota;
                    if (aAvaliacao.Visible)
                        CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true);
                }

                aPlanoAula.Visible = pnlPlanoAula.Visible = PermiteVisualizarPlanoAula;
                if (aPlanoAula.Visible)
                    CarregarListaoPlanoAula(EntTurmaDisciplina.tud_id, UCNavegacaoTelaPeriodo.VS_tpc_id, true);

                bool exibeAtividadeExtra = VS_EntitiesControleTurma.curso.tme_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_MODALIDADE_CIEJA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                aAtividadeExtraClasse.Visible = pnlAtividadesExtraClasse.Visible = exibeAtividadeExtra;

                if (exibeAtividadeExtra)
                {
                    UCComboTipoAtividadeAvaliativa.CarregarTipoAtividadeAvaliativa(true);
                    btnNovoAtiExtra.Visible = (PermiteLancarAtividadeExtraclasse || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao) && VS_Periodo_Aberto;
                    fdsCadastroAtiExtra.Visible = false;
                    fdsListagemAtiExtra.Visible = true;
                    CarregarListaoAtividadeExtraclasse();
                    hdnTaeId.Value = string.Empty;
                    hdnTaerId.Value = string.Empty;
                }

                bool permissaoModuloAlteracao = false;
                if (PermiteLancarFrequencia)
                {
                    CFG_PermissaoModuloOperacao permissaoModuloOperacao = new CFG_PermissaoModuloOperacao()
                    {
                        gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        sis_id = ApplicationWEB.SistemaID,
                        mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                        pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequencia)
                    };
                    CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloOperacao);
                    permissaoModuloAlteracao = !permissaoModuloOperacao.IsNew && permissaoModuloOperacao.pmo_permissaoEdicao;
                }

                //mostra o botao salvar -> se o docente possuir essa turma ou se for turma extinta
                //e se o usuario possuir permissao para editar a frequencia ou avaliacao
                btnSalvar.Visible = btnSalvarCima.Visible = (VS_situacaoTurmaDisciplina == 1
                                                                || (VS_situacaoTurmaDisciplina != 1 && permiteEdicao)
                                                                || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                                                || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
                                                            && ((PermiteLancarFrequencia && ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao)) 
                                                                || (PermiteLancarNota && usuarioPermissao) || (PermiteLancarPlanoAula && usuarioPermissao));

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairListao", "var exibeMensagemSair=" + btnSalvar.Visible.ToString().ToLower() + ";", true);

                if (!aFrequencia.Visible && !aAvaliacao.Visible && !aPlanoAula.Visible)
                    divListao.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o listão.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega dados de lançamento de frequência na tela.
        /// Só carrega caso a disciplina não seja do tipo
        /// complementação da regência.
        /// </summary>
        private void CarregarListaoFrequencia(bool atualizaData, bool proximo, bool anterior, bool inalterado)
        {
            try
            {
                // Só carrega a frequência caso a disciplina não seja do tipo
                // complementação da regência.
                if (EntTurmaDisciplina.tud_tipo !=
                    (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                {
                    int countAulas;
                    bool esconderSalvar = false;
                    int pagina = VS_paginaFreq;
                    string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                                        string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                                        string.Empty;

                    UCLancamentoFrequencia.Carregar(proximo
                                                    , anterior
                                                    , inalterado
                                                    , VS_EntitiesControleTurma
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , UCNavegacaoTelaPeriodo.cap_dataInicio.Date
                                                    , UCNavegacaoTelaPeriodo.cap_dataFim.Date
                                                    , PosicaoDocente
                                                    , VS_tipoDocente
                                                    , VS_turmaDisciplinaRelacionada.tud_id
                                                    , PermiteVisualizarCompensacao
                                                    , VS_ltPermissaoFrequencia
                                                    , PermiteLancarFrequencia
                                                    , out countAulas
                                                    , VS_situacaoTurmaDisciplina
                                                    , ref permiteEdicao
                                                    , usuarioPermissao
                                                    , VS_PeriodoEfetivado
                                                    , VS_Periodo_Aberto
                                                    , ref esconderSalvar
                                                    , ref pagina
                                                    , VS_EntitiesControleTurma.curso.tne_id
                                                    , tur_ids);

                    VS_paginaFreq = pagina;
                    btnSalvar.TabIndex = Convert.ToInt16(countAulas);
                    hdbtnCompensacaoAusenciaVisible.Value = PermiteLancarCompensacao.ToString();
                    hdnAlterouFrequencia.Value = string.Empty;

                    //Verifica se a aba que está aparecendo é a de frequência para mostrar o botão de compensação de ausência
                    //não usar btnCompensacaoAusencia.Visible = false; senão não funciona na troca de abas
                    if (PermiteLancarCompensacao &&
                        (string.IsNullOrEmpty(hdnListaoSelecionado.Value) || hdnListaoSelecionado.Value.Equals("0")))
                        btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "inline-block"); //mostra o botão assim para funcionar na troca de abas.
                    else
                        btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "none"); //esconde o botão assim para funcionar na troca de abas.
                }
                else
                {
                    // Caso a disciplina seja do tipo Complemento de regência, não lança frequência.
                    aFrequencia.Visible = pnlListaoLancamentoFrequencias.Visible = btnCompensacaoAusencia.Visible = false;
                    UCLancamentoFrequencia.ValorHdnOrdenacao = "";
                }

                if (atualizaData)
                    VS_Data_Listao_TurmaAula = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as frequências.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega dados de lançamento de frequência na tela para a experiencia dos territorios do saber.
        /// Só carrega caso a disciplina seja do tipo experiencia.
        /// </summary>
        private void CarregarListaoFrequenciaTerritorio(bool atualizaData, bool proximo, bool anterior, bool inalterado)
        {
            try
            {

                int countAulas;
                bool esconderSalvar = false;
                int pagina = VS_paginaFreq;
                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                                        string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                                        string.Empty;

                UCLancamentoFrequenciaTerritorio.Carregar(proximo
                                                , anterior
                                                , inalterado
                                                , VS_EntitiesControleTurma
                                                , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                , UCNavegacaoTelaPeriodo.cap_dataInicio.Date
                                                , UCNavegacaoTelaPeriodo.cap_dataFim.Date
                                                , PosicaoDocente
                                                , VS_tipoDocente
                                                , VS_turmaDisciplinaRelacionada.tud_id
                                                , PermiteVisualizarCompensacao
                                                , VS_ltPermissaoFrequencia
                                                , PermiteLancarFrequencia
                                                , out countAulas
                                                , VS_situacaoTurmaDisciplina
                                                , ref permiteEdicao
                                                , usuarioPermissao
                                                , VS_PeriodoEfetivado
                                                , VS_Periodo_Aberto
                                                , ref esconderSalvar
                                                , ref pagina
                                                , VS_EntitiesControleTurma.curso.tne_id
                                                , tur_ids);

                VS_paginaFreq = pagina;
                btnSalvar.TabIndex = Convert.ToInt16(countAulas);
                hdbtnCompensacaoAusenciaVisible.Value = PermiteLancarCompensacao.ToString();
                hdnAlterouFrequencia.Value = string.Empty;

                //Verifica se a aba que está aparecendo é a de frequência para mostrar o botão de compensação de ausência
                //não usar btnCompensacaoAusencia.Visible = false; senão não funciona na troca de abas
                if (PermiteLancarCompensacao &&
                    (string.IsNullOrEmpty(hdnListaoSelecionado.Value) || hdnListaoSelecionado.Value.Equals("0")))
                    btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "inline-block"); //mostra o botão assim para funcionar na troca de abas.
                else
                    btnCompensacaoAusencia.Style.Add(HtmlTextWriterStyle.Display, "none"); //esconde o botão assim para funcionar na troca de abas.

                if (atualizaData)
                    VS_Data_Listao_TurmaAula = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as frequências.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os dados do listão de avaliações na tela.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public void CarregarListaoAvaliacao(Guid ent_id, bool atualizaData)
        {
            try
            {
                divUsuarioAlteracaoMedia.Visible = false;
                string menAtividadeAvaliativaEfetivado = CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.ATIVIDADE_AVALIATIVA_EFETIVADO);
                lblMsgParecerAvaliacao.Text =
                    string.IsNullOrEmpty(menAtividadeAvaliativaEfetivado)
                        ? string.Empty
                        : UtilBO.GetErroMessage(menAtividadeAvaliativaEfetivado, UtilBO.TipoMensagem.Informacao);

                //////HabilitarControlesTela(true);

                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                                       string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                                       string.Empty;

                // Carrega os alunos matriculados
                List<AlunosTurmaDisciplina> ListaAlunos = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                     VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                    UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids);

                // Verifica se foram encontrado alunos
                if (ListaAlunos.Count <= 0)
                {
                    // Se não foi encontrado alunos, exibe mensagem para o usuário
                    UCLancamentoFrequencia.EscondeGridAlunosFrequencia("Não foram encontrados alunos na turma selecionada.");
                    _lblMsgRepeaterAvaliacao.Text = UtilBO.GetErroMessage("Não foram encontrados alunos na turma selecionada.", UtilBO.TipoMensagem.Alerta);
                    // Esconder repeter de alunos
                    rptAlunosAvaliacao.Visible = UCComboOrdenacaoAvaliacao.Visible = lblMsgParecerAvaliacao.Visible = false;
                    _lblMsgRepeaterAvaliacao.Visible = true;
                    // Limpa o hiddenfield do listão de avaliação pra zerar a ordenação.
                    hdnOrdenacaoAvaliacao.Value = "";
                }
                else
                {
                    DataTable dt = CLS_TurmaNotaBO.GetSelectBy_TurmaDisciplina_PeriodoTodos(
                        VisibilidadeRegencia(ddlTurmaDisciplinaListao) ?
                                ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                            , UCNavegacaoTelaPeriodo.VS_tpc_id
                            , (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                            , PosicaoDocente
                            , VS_turmaDisciplinaRelacionada.tud_id
                            , __SessionWEB.__UsuarioWEB.Docente.doc_id == 0, true, false, tur_ids);

                    if (Vs_calcula_notaFinal)
                    {
                        alunoMediaFinal = (
                            from DataRow dr in dt.Rows
                            group dr by new { alu_id = dr["alu_id"].ToString(), mtu_id = dr["mtu_id"].ToString() }
                                into g
                            select new CLS_AlunoAvaliacaoTurmaDisciplinaMedia
                            {
                                tud_id = Convert.ToInt64(g.First()["tud_id"].ToString()),
                                alu_id = Convert.ToInt64(g.First()["alu_id"].ToString()),
                                mtu_id = Convert.ToInt32(g.First()["mtu_id"].ToString()),
                                mtd_id = Convert.ToInt32(g.First()["mtd_id"].ToString()),
                                tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                                atm_media = g.FirstOrDefault()["atm_media"].ToString()
                            }).ToList();
                    }

                    if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.Normal)
                    {
                        lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(UCNavegacaoTelaPeriodo.VS_tpc_id, UCControleTurma1.VS_tur_id, VisibilidadeRegencia(ddlTurmaDisciplinaListao)? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id, ApplicationWEB.AppMinutosCacheMedio);
                    }

                    // Carregar as atividades e notas dos alunos nas atividades.
                    var x = (from DataRow dr in dt.Rows
                             where !string.IsNullOrEmpty(dr["tnt_id"].ToString())
                             select dr);

                    if (x.Count() > 0)
                    {
                        DTAtividades = x.CopyToDataTable();
                    }
                    else
                    {
                        DTAtividades = new DataTable();
                    }

                    ltAtividadeIndicacaoNota = (from DataRow dr in DTAtividades.Rows
                                                group dr by new { tud_id = Convert.ToInt64(dr["tud_id"]), tnt_id = Convert.ToInt32(dr["tnt_id"]) }
                                                    into grupo
                                                select new AtividadeIndicacaoNota
                                                {
                                                    tud_id = grupo.Key.tud_id
                                                    ,
                                                    tnt_id = grupo.Key.tnt_id
                                                    ,
                                                    PossuiNota = grupo.Any(p => Convert.ToBoolean(p["PossuiNota"]))
                                                }).ToList();

                    // Mostrar repeater de alunos
                    rptAlunosAvaliacao.Visible = lblMsgParecerAvaliacao.Visible = true;
                    // Carregar repeter de alunos
                    rptAlunosAvaliacao.DataSource = ListaAlunos;
                    rptAlunosAvaliacao.DataBind();
                    // Limpa o hiddenfield do listão de avaliação pra zerar a ordenação.
                    hdnOrdenacaoAvaliacao.Value = "";

                    RepeaterItem header = (RepeaterItem)rptAlunosAvaliacao.Controls[0];
                    Repeater rptAtividades = (Repeater)header.FindControl("rptAtividadesAvaliacao");

                    _lblMsgRepeaterAvaliacao.Visible = false;

                    if (rptAtividades.Items.Count == 0)
                    {
                        _lblMsgRepeaterAvaliacao.Text = UtilBO.GetErroMessage(string.Format("Não foi encontrado(a) {0} para a turma no período e " +
                            (string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionados.", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(ent_id).ToLower())
                            , UtilBO.TipoMensagem.Alerta);

                        _lblMsgRepeaterAvaliacao.Visible = true;
                        UCComboOrdenacaoAvaliacao.Visible = true;
                    }
                    else
                    {
                        UCComboOrdenacaoAvaliacao.Visible = true;
                    }

                    // localizo o nome do usuário que realizou a última alteração nos dados
                    var usuarioAlteracao = (from DataRow dr in dt.Rows
                                            where !string.IsNullOrEmpty(dr["usuarioAltMedia"].ToString())
                                            select new
                                            {
                                                usuarioAltMedia = dr["usuarioAltMedia"].ToString(),
                                                dataAltMedia = Convert.ToDateTime(dr["dataAltMedia"])
                                            });

                    if ((usuarioAlteracao != null) && usuarioAlteracao.Any())
                    {
                        var usuario = usuarioAlteracao.First();

                        lblAlteracaoMedia.Text = CarregarUsuarioAlteracao(usuario.usuarioAltMedia, usuario.dataAltMedia);
                        if (!string.IsNullOrEmpty(lblAlteracaoMedia.Text))
                            divUsuarioAlteracaoMedia.Visible = true;
                    }
                }

                if (atualizaData)
                    VS_Data_Listao_TurmaNota = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as notas.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o nome do usuário que alterou os dados
        /// </summary>
        private String CarregarUsuarioAlteracao(string nomeUsuario, DateTime dataAlteracao)
        {
            if (string.IsNullOrEmpty(nomeUsuario))
            {
                return string.Empty;
            }

            return "</br>" + GetGlobalResourceObject("Academico", "ControleTurma.Listao.lblAvaliacao.Text").ToString() +
                   " alterado por: " + nomeUsuario.Trim() + " em " + dataAlteracao.ToString("G");
        }

        /// <summary>
        /// Carrega os dados do listão de planos de aula na tela.
        /// </summary>
        /// <param name="tud_id">Id TurmaDisciplina</param>
        public void CarregarListaoPlanoAula(Int64 tud_id, int tpc_id, bool atualizaData)
        {
            try
            {
                // Carregar repeter de alunos
                DataTable dtPlanos = CLS_TurmaAulaBO.SelecionaPlanosAulaPor_Disciplina(tud_id, tpc_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, UCControleTurma1.VS_tdt_posicao, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0, VS_turmaDisciplinaRelacionada.tud_id);
                var planosPermissaoDocente = from DataRow dr in dtPlanos.AsEnumerable()
                                             where VS_ltPermissaoPlanoAula.Any(p => p.tdt_posicaoPermissao == (byte)dr["tdt_posicao"] && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao))
                                             select dr;

                bool existeAulaSemPlano = (from DataRow dr in dtPlanos.AsEnumerable()
                                           where (bool)dr["semPlanoAula"] && Convert.ToDateTime(dr["data"]).Date < DateTime.Now
                                           select dr).Count() > 0;

                if (existeAulaSemPlano
                    && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                        || VS_EntitiesControleTurma.curso.tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                {
                    lblAulasSemPlano.Visible = true;
                    lblAulasSemPlano.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Listao.lblAulasSemPlano").ToString(), UtilBO.TipoMensagem.Informacao);
                    divAvisoAulaSemPlano.Visible = true;
                }
                else
                {
                    lblAulasSemPlano.Visible = false;
                    divAvisoAulaSemPlano.Visible = false;
                }

                if (planosPermissaoDocente.Any())
                {
                    // seleciona apenas os planos que o docente tem permissao para consultar ou alterar
                    rptPlanoAula.DataSource = planosPermissaoDocente.CopyToDataTable();
                }
                else
                {
                    rptPlanoAula.DataSource = new DataTable();
                }
                rptPlanoAula.DataBind();

                if (rptPlanoAula.Items.Count == 0)
                {
                    rptPlanoAula.Visible = false;
                    lblDadoPlanoAula.Visible = true;
                    lblDadoPlanoAula.Text = UtilBO.GetErroMessage("Não há aulas cadastradas no bimestre selecionado.", UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    rptPlanoAula.Visible = true;
                    lblDadoPlanoAula.Visible = false;
                }

                if (atualizaData)
                    VS_Data_Listao_TurmaAula = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os planos de aula.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o combo de disciplinas, verificando se usuário logado é docente.
        /// </summary>
        private bool CarregarDisciplinasComboListao(long tur_id, long tud_idSelecionada)
        {
            try
            {
                ddlTurmaDisciplinaListao.Items.Clear();
                ddlComponenteListao.Items.Clear();

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia
                                                                    (tur_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);
                    List<sComboTurmaDisciplina> disciplinaComponenteDocente = (from dr in dtTurmaDisciplina
                                                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia
                                                                               select new sComboTurmaDisciplina
                                                                               {
                                                                                   tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                                   ,
                                                                                   tur_tud_id = dr.tur_tud_id.ToString()
                                                                               }).ToList();

                    if ((!VisibilidadeRegencia(ddlTurmaDisciplinaListao)) || (disciplinaComponenteDocente.Count() > 0))
                    {
                        VS_DisciplinaComponenteDocente = disciplinaComponenteDocente;

                        List<sComboTurmaDisciplina> turmaDisciplina = (from dr in dtTurmaDisciplina
                                                                       where (Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                             && (Convert.ToString(dr.tur_tud_id.Split(';')[1]).Equals(VS_EntitiesControleTurma.turmaDisciplina.tud_id.ToString()))
                                                                       select new sComboTurmaDisciplina
                                                                       {
                                                                           tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                           ,
                                                                           tur_tud_id = dr.tur_tud_id.ToString()
                                                                       }).ToList();

                        ddlTurmaDisciplinaListao.DataSource = turmaDisciplina;
                        ddlTurmaDisciplinaListao.DataBind();

                        if (turmaDisciplina.Any(p => Convert.ToByte(p.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia))
                            CarregaComponenteRegenciaDocente(
                                Convert.ToInt64(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[0]),
                                ddlComponenteListao);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        lblMessage.Text = UtilBO.GetErroMessage("Docente não possui permissão em disciplinas para lançamento de notas.", UtilBO.TipoMensagem.Alerta);
                        return false;
                    }
                }
                else
                {
                    List<sComboTurmaDisciplina> dtTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                    List<sComboTurmaDisciplina> turmaDisciplina = (from dr in dtTurmaDisciplina
                                                                   where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                        && (Convert.ToString(dr.tur_tud_id.Split(';')[1]).Equals(VS_EntitiesControleTurma.turmaDisciplina.tud_id.ToString()))
                                                                   select new sComboTurmaDisciplina
                                                                   {
                                                                       tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                       ,
                                                                       tur_tud_id = dr.tur_tud_id.ToString()
                                                                   }).ToList();

                    //Carrega as disciplinas da turma caso a visão do usuário não seja individual, exceto as componentes de regência.
                    ddlTurmaDisciplinaListao.DataSource = turmaDisciplina;
                    ddlTurmaDisciplinaListao.DataBind();

                    // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                    if (turmaDisciplina.Any(p => Convert.ToByte(p.tur_tud_id.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia))
                    {
                        //Carrega componentes da regência.
                        var turmaDisciplinaComponenteRegencia = (from dr in dtTurmaDisciplina
                                                                 where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                 select dr);
                        ddlComponenteListao.DataSource = turmaDisciplinaComponenteRegencia;
                        ddlComponenteListao.DataBind();
                    }
                }

                if (ddlTurmaDisciplinaListao.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlTurmaDisciplinaListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlTurmaDisciplinaListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlTurmaDisciplinaListao.SelectedValue = x.First();
                }

                if (ddlComponenteListao.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlComponenteListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlComponenteListao.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlComponenteListao.SelectedValue = x.First();
                }

                ddlComponenteListao.Visible = lblComponenteListao.Visible = ddlComponenteListao.Items.Count > 0 && Convert.ToByte(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
                return true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                return false;
            }
        }

        /// <summary>
        /// Carrega disciplina(s componente(s) da regencia somente da turma selecionada.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        private void CarregaComponenteRegenciaDocente(long tur_id, DropDownList ddlDisciplinaComponentes)
        {
            var turmaDisciplinaComponenteRegencia = (from dr in VS_DisciplinaComponenteDocente
                                                     where (Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == tur_id)
                                                     select new sComboTurmaDisciplina
                                                     {
                                                         tur_tud_nome = dr.tur_tud_nome.ToString()
                                                         ,
                                                         tur_tud_id = dr.tur_tud_id.ToString()
                                                     }).ToList();

            ddlDisciplinaComponentes.DataSource = turmaDisciplinaComponenteRegencia;
            ddlDisciplinaComponentes.DataBind();
        }

        /// <summary>
        /// Salva no banco as alteracoes no plano de aula.
        /// </summary>
        public bool SalvarPlanoAula(out string msg)
        {
            msg = "";

            List<CLS_TurmaAula> lstTurmaAula = new List<CLS_TurmaAula>();
            List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDisc = EntTurmaDisciplina.tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) ?
                                                                        new List<CLS_TurmaAulaPlanoDisciplina>() : null;
            List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletar = new List<CLS_TurmaAulaPlanoDisciplina>();

            List<CLS_TurmaAula> listaTurmaAulaBD = new List<CLS_TurmaAula>();
            if (rptPlanoAula.Items.Count > 0)
            {
                // Busca as aulas do banco de dados.
                List<string> lstTauIdsSalvar = (from RepeaterItem item in rptPlanoAula.Items
                                                select ((Literal)item.FindControl("litTau_id")).Text).ToList();
                listaTurmaAulaBD = CLS_TurmaAulaBO.SelecionarListaAulasPorIds(EntTurmaDisciplina.tud_id, string.Join(",", lstTauIdsSalvar));
            }

            foreach (RepeaterItem itemAtividade in rptPlanoAula.Items)
            {
                TextBox txtPlanoAula = (TextBox)itemAtividade.FindControl("txtPlanoAula");
                int permiteAlteracao;
                Int32.TryParse((((HiddenField)itemAtividade.FindControl("hdnPermissaoAlteracao")).Value), out permiteAlteracao);

                bool permissaoAlteracao = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Gestao
                                            && __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa
                                            && permiteAlteracao > 0
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                if (permissaoAlteracao)
                {
                    TextBox txtSinteseAula = (TextBox)itemAtividade.FindControl("txtSinteseAula");
                    Literal litTud_id = (Literal)itemAtividade.FindControl("litTud_id");
                    Literal litTau_id = (Literal)itemAtividade.FindControl("litTau_id");
                    //bool existeDisciplinaPlanoRegencia = false; // Comentado para retirar a exigencia de algum componente da regencia ser selecionado para salvar o planejamento

                    long tud_id = Convert.ToInt64(litTud_id.Text);
                    int tau_id = Convert.ToInt32(litTau_id.Text);
                    CLS_TurmaAula ent = listaTurmaAulaBD.FirstOrDefault(p => p.tud_id == EntTurmaDisciplina.tud_id && p.tau_id == tau_id);

                    if ((tud_id != EntTurmaDisciplina.tud_id || !LstTauSalvas.Contains(ent.tau_id)) &&
                        !ent.IsNew && ent.tau_dataAlteracao > VS_Data_Listao_TurmaAula)
                    {
                        VS_recarregarDataAula = false;
                        throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaPlanejamento").ToString());
                    }

                    if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                        UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                        ent.tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                    {
                        throw new ValidationException("Existem aulas com data maior que a data de encerramento da turma.");
                    }

                    Literal litTud_idFilho = (Literal)itemAtividade.FindControl("litTud_idFilho");
                    if (ent.tau_planoAula == null)
                        ent.tau_planoAula = String.Empty;
                    if (ent.tau_sintese == null)
                        ent.tau_sintese = String.Empty;

                    //Verifica se houve alguma alteração no plano de aula ou no resumo
                    if ((ent.tau_planoAula != txtPlanoAula.Text) || (ent.tau_sintese != txtSinteseAula.Text))
                    {
                        ent.tau_planoAula = txtPlanoAula.Text;
                        ent.tau_sintese = txtSinteseAula.Text;
                        ent.tau_dataAlteracao = DateTime.Now;
                        ent.IsNew = false;
                        lstTurmaAula.Add(ent);
                    }

                    if (EntTurmaDisciplina.tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
                    {
                        lstTurmaAulaPlanoDiscDeletar.Add(new CLS_TurmaAulaPlanoDisciplina
                        {
                            tud_id = ent.tud_id,
                            tau_id = ent.tau_id,
                            tud_idPlano = -1
                        });

                        CheckBoxList chlComponenteCurricular = (CheckBoxList)itemAtividade.FindControl("chlComponenteCurricular");
                        foreach (ListItem item in chlComponenteCurricular.Items)
                        {
                            if (item.Selected)
                            {
                                //existeDisciplinaPlanoRegencia = true;
                                lstTurmaAulaPlanoDisc.Add(new CLS_TurmaAulaPlanoDisciplina
                                {
                                    tud_id = ent.tud_id,
                                    tau_id = ent.tau_id,
                                    tud_idPlano = Convert.ToInt64(item.Value.Split(';')[1])
                                });
                            }
                        }

                        // 07/03/2015 - Solicitado para retirar a exigencia de algum componente da regencia ser selecionado para salvar o planejamento
                        // Caso algum campo do plano de aula esteja preenchido,
                        // é obrigatório possuir pelo menos um componente selecionado quando é regência.                  
                        //if (!existeDisciplinaPlanoRegencia && (!String.IsNullOrEmpty(ent.tau_planoAula) || !String.IsNullOrEmpty(ent.tau_sintese)))
                        //{
                        //    throw new ValidationException(
                        //        "É necessário selecionar um componente da regência para salvar o plano da aula \""
                        //        + ent.tau_data.ToString("dd/MM/yyyy") + "\".");
                        //}
                    }

                    //ent.tau_statusPlanoAula = (byte)CLS_TurmaAulaBO.RetornaStatusPlanoAula(ent, existeDisciplinaPlanoRegencia);
                    ent.tau_statusPlanoAula = (byte)CLS_TurmaAulaBO.RetornaStatusPlanoAula(ent,
                                                                       ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SINTESE_REGENCIA_AULA_TURMA, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                                                       VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                }
            }

            CLS_TurmaAulaPlanoDisciplinaBO.SalvarEmLote(lstTurmaAula, lstTurmaAulaPlanoDisc, lstTurmaAulaPlanoDiscDeletar);

            try
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Listão de planos de aula | "
                                                                        + "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id
                                                                        + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id
                                                                        + " | tur_id: " + UCControleTurma1.VS_tur_id + "; tud_id: " + EntTurmaDisciplina.tud_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }

            hdnAlterouPlanoAula.Value = string.Empty;

            CarregarListaoPlanoAula(EntTurmaDisciplina.tud_id, UCNavegacaoTelaPeriodo.VS_tpc_id, false);

            msg += UtilBO.GetErroMessage("Listão de planos de aula salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

            return true;
        }

        /// <summary>
        /// Indica se a visibilidade é do tipo regência-componente regência.
        /// </summary>
        /// <returns></returns>
        private bool VisibilidadeRegencia(DropDownList ddlTurmas)
        {
            return ddlTurmas.Items.Count > 0 && Convert.ToByte(ddlTurmas.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do repeater.
        /// </summary>
        private static string RetornaAvaliacao(RepeaterItem item)
        {
            TextBox txtNota = (TextBox)item.FindControl("txtNota");

            if (txtNota.Visible)
                return txtNota.Text;

            DropDownList ddlPareceres = (DropDownList)item.FindControl("ddlPareceres");

            if (ddlPareceres.Visible)
            {
                if (ddlPareceres.SelectedValue == "-1")
                    return string.Empty;

                return ddlPareceres.SelectedValue;
            }

            return string.Empty;
        }

        /// <summary>
        /// Retornas the nota final.
        /// </summary>
        /// <param name="item">The item.</param>
        private string RetornaNotaFinal(RepeaterItem item)
        {
            TextBox txtNotaFinal = (TextBox)item.FindControl("txtNotaFinal");

            if (txtNotaFinal.Visible)
                return txtNotaFinal.Text;

            DropDownList ddlParecerFinal = (DropDownList)item.FindControl("ddlParecerFinal");

            if (ddlParecerFinal.Visible)
            {
                if (ddlParecerFinal.SelectedValue == "-1")
                    return string.Empty;

                return ddlParecerFinal.SelectedValue;
            }

            return string.Empty;
        }

        /// <summary>
        /// Salva listão de avaliações.
        /// </summary>
        /// <param name="PermaneceTela"></param>
        /// <returns></returns>
        public bool SalvarAvaliacao(out string msg)
        {
            //try
            //{
            msg = "";
            if (!VS_Periodo_Aberto)
                throw new ValidationException(String.Format("Listão de avaliações de {0} disponível apenas para consulta.", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

            List<CLS_TurmaNotaAluno> listTurmaNotaAluno = new List<CLS_TurmaNotaAluno>();
            List<CLS_TurmaNota> listTurmaNota = new List<CLS_TurmaNota>();
            List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listAlunoAvaliacaoTurmaDisciplinaMedia = new List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia>();
            List<CLS_TurmaAula> listaTurmaAula = new List<CLS_TurmaAula>();

            RepeaterItem header = (RepeaterItem)rptAlunosAvaliacao.Controls[0];
            Repeater rptAtividadesEfetivado = (Repeater)header.FindControl("rptAtividadesAvaliacaoEfetivado");

            bool visibilidadeRegencia = VisibilidadeRegencia(ddlTurmaDisciplinaListao);

            Guid UsuIdLogado = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

            List<CLS_TurmaNota> listaTurmaNotaBD = new List<CLS_TurmaNota>();
            if (rptAtividadesEfetivado.Items.Count > 0)
            {
                List<string> lstTntIdsSalvar = (from RepeaterItem item in rptAtividadesEfetivado.Items
                                                select ((Label)item.FindControl("lbltnt_id")).Text).ToList();
                listaTurmaNotaBD = CLS_TurmaNotaBO.SelecionarListaAtividadesPorIds(visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id, string.Join(",", lstTntIdsSalvar));
            }

            // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
            foreach (RepeaterItem itemAtividade in rptAtividadesEfetivado.Items)
            {
                Int16 tdt_posicao = Convert.ToInt16(((Label)itemAtividade.FindControl("lblPosicao")).Text);
                Guid usu_id_ativ = (!string.IsNullOrEmpty(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) ? new Guid(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty);
                CheckBox chkEfetivado = (CheckBox)itemAtividade.FindControl("chkEfetivado");
                if (usu_id_ativ == UsuIdLogado || !(PosicaoDocente > 0 && !VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao)))
                {
                    int tau_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltau_id")).Text);
                    int tnt_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltnt_id")).Text);
                    bool tnt_exclusiva = Convert.ToBoolean(((Label)itemAtividade.FindControl("lblAtividadeExclusiva")).Text);
                    DateTime dataAula = Convert.ToDateTime(((Label)itemAtividade.FindControl("lblDataAula")).Text);

                    if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                        UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                        dataAula > UCControleTurma1.VS_tur_dataEncerramento)
                    {
                        throw new ValidationException("Existem atividades com data maior que a data de encerramento da turma.");
                    }

                    long tud_id = visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id;
                    CLS_TurmaNota ent = listaTurmaNotaBD.FirstOrDefault(p => p.tud_id == tud_id && p.tnt_id == tnt_id);
                    if (!ent.IsNew && ent.tnt_dataAlteracao > VS_Data_Listao_TurmaNota)
                    {
                        VS_recarregarDataNota = false;
                        throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaNota").ToString());
                    }

                    ent.tnt_efetivado = chkEfetivado.Checked;
                    ent.tdt_posicao = PosicaoDocente;
                    ent.tnt_exclusiva = tnt_exclusiva;
                    ent.tnt_data = dataAula;
                    ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

                    listTurmaNota.Add(ent);

                    if (tau_id > 0)
                    {
                        listaTurmaAula.Add(new CLS_TurmaAula
                        {
                            tud_id = ent.tud_id,
                            tau_id = tau_id,
                            tau_data = dataAula
                        });
                    }

                    LstTauSalvas.Add(tau_id);

                    chkEfetivado.Enabled = true;
                }
                else
                    chkEfetivado.Enabled = false;
            }

            listaTurmaAula = (
                    from aula in listaTurmaAula
                    group aula by new { aula.tud_id, aula.tau_id, aula.tau_data } into gAula
                    select new CLS_TurmaAula
                    {
                        tud_id = EntTurmaDisciplina.tud_id,
                        tau_id = gAula.Key.tau_id,
                        tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(
                            listTurmaNota.Where(p => p.tud_id == gAula.Key.tud_id && p.tnt_data == gAula.Key.tau_data).ToList())
                    }
                ).Distinct().ToList();

            foreach (RepeaterItem itemAluno in rptAlunosAvaliacao.Items)
            {
                Repeater rptAtividades = (Repeater)itemAluno.FindControl("rptAtividadesAvaliacao");
                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);
                string pes_nome = Convert.ToString(((Label)itemAluno.FindControl("lblNomeOficial")).Text);

                // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                foreach (RepeaterItem itemAtividadeAluno in rptAtividades.Items)
                {
                    Guid usu_id_ativ = (!string.IsNullOrEmpty(((Label)itemAtividadeAluno.FindControl("lblUsuIdAtiv")).Text) ? new Guid(((Label)itemAtividadeAluno.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty);

                    HtmlGenericControl divAtividades = (HtmlGenericControl)itemAtividadeAluno.FindControl("divAtividades");
                    if (divAtividades != null)
                    {
                        Int16 tdt_posicao = Convert.ToInt16(((Label)divAtividades.FindControl("lblPosicao")).Text);
                        CheckBox chkParticipante = (CheckBox)itemAtividadeAluno.FindControl("chkParticipante");

                        if (usu_id_ativ == UsuIdLogado || !(PosicaoDocente > 0 && PosicaoDocente != tdt_posicao))
                        {
                            if (CLS_TurmaNotaAlunoBO.VerificaValoresNotas(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica, RetornaAvaliacao(itemAtividadeAluno), pes_nome))
                            {
                                int tnt_id = Convert.ToInt32(((Label)itemAtividadeAluno.FindControl("lbltnt_id")).Text);

                                // Busca relatório lançado.
                                NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                                    p.alu_id == alu_id
                                    && p.tnt_id == tnt_id
                                    && p.mtu_id == mtu_id);

                                CLS_TurmaNotaAluno ent = new CLS_TurmaNotaAluno
                                {
                                    tud_id = visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                                    tnt_id = tnt_id,
                                    alu_id = alu_id,
                                    mtu_id = mtu_id,
                                    mtd_id = mtd_id,
                                    tna_avaliacao = RetornaAvaliacao(itemAtividadeAluno),
                                    tna_relatorio = rel.valor,
                                    tna_naoCompareceu = false,
                                    tna_situacao = 1,
                                    tna_participante = (chkParticipante != null && chkParticipante.Visible) ? chkParticipante.Checked : true
                                };

                                listTurmaNotaAluno.Add(ent);
                            }

                            HabilitaControles(divAtividades.Controls, true);
                        }
                        else
                            HabilitaControles(divAtividades.Controls, false);
                    }
                }

                if (Vs_calcula_notaFinal)
                {
                    if (CLS_TurmaNotaAlunoBO.VerificaValoresNotas(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica, RetornaNotaFinal(itemAluno), pes_nome))
                    {
                        CLS_AlunoAvaliacaoTurmaDisciplinaMedia entityMedia = new CLS_AlunoAvaliacaoTurmaDisciplinaMedia
                        {
                            tud_id = visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                            alu_id = alu_id,
                            mtu_id = mtu_id,
                            mtd_id = mtd_id,
                            tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                            atm_situacao = 1, //Ativo
                            atm_media = RetornaNotaFinal(itemAluno)
                        };

                        listAlunoAvaliacaoTurmaDisciplinaMedia.Add(entityMedia);
                    }
                }
            }

            CLS_TurmaNotaAlunoBO.Save
                (
                    listTurmaNotaAluno
                    , listTurmaNota
                    , listAlunoAvaliacaoTurmaDisciplinaMedia
                    , UCControleTurma1.VS_tur_id
                    , visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                    , VS_fav_id
                    , PosicaoDocente
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    , listaTurmaAula
                    , VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    , (byte)LOG_AvaliacaoMedia_Alteracao_Origem.Web
                    , (byte)LOG_TurmaNota_Alteracao_Origem.WebListao
                    , (byte)LOG_TurmaNota_Alteracao_Tipo.LancamentoNotas
                    , visibilidadeRegencia ? EntTurmaDisciplina.tud_id : -1
                );

            try
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Listão de nota | " +
                                                                        "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                        " | tur_id: " + UCControleTurma1.VS_tur_id + "; tud_id: " + (visibilidadeRegencia ?
                        ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id));
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }

            hdnAlterouNota.Value = string.Empty;

            CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, false);

            msg = UtilBO.GetErroMessage("Listão de avaliações salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

            return true;
        }

        /// <summary>
        /// Adiciona um item na lista de relatórios do ViewState.
        /// </summary>
        /// <param name="tnt_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="valor"></param>
        private void AdicionaItemRelatorio(int tnt_id, long alu_id, int mtu_id, string valor)
        {
            VS_Nota_Relatorio.Add(new NotasRelatorio
            {
                tnt_id = tnt_id
                ,
                alu_id = alu_id
                ,
                mtu_id = mtu_id
                ,
                valor = valor
            });
        }

        /// <summary>
        /// Adiciona um item na lista de relatórios do ViewState.
        /// </summary>
        /// <param name="tnt_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="valor"></param>
        private void AdicionaItemRelatorioAtiExtra(int tae_id, long alu_id, int mtu_id, string valor)
        {
            VS_Nota_RelatorioAtiExtra.Add(new NotasRelatorioAtiExtra
            {
                tae_id = tae_id
                ,
                alu_id = alu_id
                ,
                mtu_id = mtu_id
                ,
                valor = valor
            });
        }

        /// <summary>
        /// Seta imagem de relatório lançado para o item.
        /// </summary>
        /// <param name="itemAtividade">Item do repeater de atividades</param>
        private void SetaImgRelatorio(RepeaterItem itemAtividade)
        {
            ImageButton btnRelatorio = (ImageButton)itemAtividade.FindControl("btnRelatorio");
            Image imgSituacao = (Image)itemAtividade.FindControl("imgSituacao");

            Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
            RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

            long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
            int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
            int tnt_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltnt_id")).Text);

            NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                                    p.alu_id == alu_id
                                    && p.tnt_id == tnt_id
                                    && p.mtu_id == mtu_id);

            //Verifica se o relatório já foi lançado e seta a visibilidade do imgSituacao
            if (!string.IsNullOrEmpty(rel.valor))
            {
                imgSituacao.Visible = true;
                btnRelatorio.ToolTip = "Alterar lançamento do relatório";
            }
            else
                imgSituacao.Visible = false;
        }

        /// <summary>
        /// Seta imagem de relatório lançado para o item.
        /// </summary>
        /// <param name="itemAtividade">Item do repeater de atividades</param>
        private void SetaImgRelatorioAtiExtra(RepeaterItem itemAtividade)
        {
            ImageButton btnRelatorio = (ImageButton)itemAtividade.FindControl("btnRelatorio");
            Image imgSituacao = (Image)itemAtividade.FindControl("imgSituacao");

            Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
            RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

            long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
            int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
            int tae_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltae_id")).Text);

            NotasRelatorioAtiExtra rel = VS_Nota_RelatorioAtiExtra.Find(p =>
                                    p.alu_id == alu_id
                                    && p.tae_id == tae_id
                                    && p.mtu_id == mtu_id);

            //Verifica se o relatório já foi lançado e seta a visibilidade do imgSituacao
            if (!string.IsNullOrEmpty(rel.valor))
            {
                imgSituacao.Visible = true;
                btnRelatorio.ToolTip = "Alterar lançamento do relatório";
            }
            else
                imgSituacao.Visible = false;
        }

        /// <summary>
        /// Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
        /// </summary>
        /// <param name="e">Item do repeater</param>
        private void SetaNomeAluno(RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header)
            {
                string pes_nome = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "pes_nome"));
                Label lblNome = (Label)e.Item.FindControl("lblNome");
                if (lblNome != null)
                    lblNome.Text = pes_nome;

                // Recupera a data de matrícula do aluno na turma/disciplina
                string sDataMatricula = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mtd_dataMatricula"));
                if (!string.IsNullOrEmpty(sDataMatricula))
                {
                    DateTime dataMatricula = Convert.ToDateTime(sDataMatricula);
                    if (dataMatricula.Date > UCNavegacaoTelaPeriodo.cap_dataInicio.Date)
                    {
                        if (lblNome != null)
                            lblNome.Text += "<br/>" + "<b>Data de matrícula:</b> " + dataMatricula.ToString("dd/MM/yyyy");
                    }
                }

                // Issue #313 - removi texto de data de saída, pois já traz a informação no nome do aluno.
                //// Recupera a data de saída do aluno na turma/disciplina
                //string sDataSaida = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida"));
                //if (!string.IsNullOrEmpty(sDataSaida))
                //{
                //    DateTime dataSaida = Convert.ToDateTime(sDataSaida);
                //    if (dataSaida.Date < UCNavegacaoTelaPeriodo.cap_dataFim)
                //    {
                //        if (lblNome != null)
                //            lblNome.Text += "<br/>" + "<b>Data de saída:</b> " + dataSaida.ToString("dd/MM/yyyy");
                //    }
                //}
            }
        }

        /// <summary>
        /// Carrega repeater de atividades e avaliações da secretaria com dataSources para o listao.
        /// </summary>
        /// <param name="e">Item do repeater de alunos</param>
        /// <param name="alu_id">ID do aluno</param>
        private void CarregaRepeateresInternos(RepeaterItemEventArgs e, long alu_id)
        {
            Repeater rptAtividades = (Repeater)e.Item.FindControl("rptAtividadesAvaliacao");
            Repeater rptAtividadesEfetivado = (Repeater)e.Item.FindControl("rptAtividadesAvaliacaoEfetivado");
            DataTable dtAtividades = new DataTable();
            List<DataRow> ltAtividades;

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

            if (e.Item.ItemType == ListItemType.Header)
            {
                // Busca todas as atividades para o cabeçalho.
                ltAtividades = (from DataRow dr in DTAtividades.Rows
                                where !string.IsNullOrEmpty(Convert.ToString(dr["tnt_data"]))
                                group dr by dr["tnt_id"] into g
                                orderby Convert.ToDateTime(g.FirstOrDefault()["tnt_data"])
                                                             , Convert.ToInt32(g.FirstOrDefault()["tnt_id"])
                                                             , Convert.ToInt64(g.FirstOrDefault()["tud_id"])
                                let tdt_posicao = Convert.ToByte(g.FirstOrDefault()["tdt_posicao"])
                                where VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoConsulta)
                                select g.FirstOrDefault()).ToList();

                if (ltAtividades.Count > 0)
                    dtAtividades = ltAtividades.CopyToDataTable();
            }
            else
            {
                int mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                int mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));

                // Busca as notas das atividades para o aluno.
                ltAtividades = (from DataRow dr in DTAtividades.Rows
                                where
                                    !string.IsNullOrEmpty(Convert.ToString(dr["tnt_data"]))
                                    && Convert.ToInt64(dr["alu_id"]) == alu_id
                                    && Convert.ToInt32(dr["mtu_id"]) == mtu_id
                                    && Convert.ToInt32(dr["mtd_id"]) == mtd_id
                                group dr by dr["tnt_id"] into g
                                orderby Convert.ToDateTime(g.FirstOrDefault()["tnt_data"])
                                                             , Convert.ToInt32(g.FirstOrDefault()["tnt_id"])
                                                             , Convert.ToInt64(g.FirstOrDefault()["tud_id"])
                                let tdt_posicao = Convert.ToByte(g.FirstOrDefault()["tdt_posicao"])
                                where VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoConsulta)
                                select g.FirstOrDefault()).ToList();

                if (ltAtividades.Count > 0)
                    dtAtividades = ltAtividades.CopyToDataTable();
            }

            if (rptAtividades != null)
            {
                rptAtividades.DataSource = dtAtividades;
                rptAtividades.DataBind();
            }

            if (rptAtividadesEfetivado != null)
            {
                rptAtividadesEfetivado.DataSource = dtAtividades;
                rptAtividadesEfetivado.DataBind();
            }

            HtmlTableCell tdMedia = (HtmlTableCell)e.Item.FindControl("tdMedia");
            if (tdMedia != null && (rptAtividades.Items.Count > 0))
            {
                tdMedia.Visible = (tipo == EscalaAvaliacaoTipo.Numerica || tipo == EscalaAvaliacaoTipo.Pareceres) && Vs_calcula_notaFinal;

                HtmlTableCell tdMediaResponsivo = (HtmlTableCell)e.Item.FindControl("tdMediaResponsivo");
                if (tdMediaResponsivo != null)
                {
                    tdMediaResponsivo.Visible = tdMedia.Visible;
                }
            }
        }
        
        /// <summary>
        /// Retorna o script que adiciona variáveis necessárias para o javscript da tela.
        /// </summary>
        /// <returns></returns>
        private string GeraScriptVariaveisTurma()
        {
            string arredondamento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ARREDONDAMENTO_NOTA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
            string destacarCampoNota = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.DESTACAR_CAMPO_NOTA_AVALIACAO_ACIMA_PERMITIDO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();

            string escala_variacao = "0";
            string escala_maior_valor = "0";

            if (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica != null)
            {
                escala_variacao = VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao.ToString();
                escala_maior_valor = VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_maiorValor.ToString().Replace(',', '.');
            }

            string script = "var calcularMediaAutomatica = " + (!Vs_calcula_notaFinal).ToString().ToLower() + ";" +
                      "var arredondamento = " + arredondamento.ToLower() + ";" +
                      "var qtdCasasDecimais = parseInt('" + NumeroCasasDecimais + "');" +
                      "var variacaoEscala = '" + escala_variacao.Replace(',', '.') + "';" +
                      "var destacarCampoNota = " + destacarCampoNota.ToLower() + ";" +
                      "var maiorValor = " + escala_maior_valor + ";" +
                      "var DisciplinaRegencia='" + DisciplinaRegencia + "';";

            return script;
        }

        /// <summary>
        /// Carrega os listão de atividade extraclasse.
        /// </summary>
        private void CarregarListaoAtividadeExtraclasse()
        {
            string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                                      string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                                      string.Empty;

            // Carrega os alunos matriculados
            List<AlunosTurmaDisciplina> ListaAlunos = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                 VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids);

            if (ListaAlunos.Count <= 0)
            {
                rptAlunoAtivExtra.Visible = UCComboOrdenacaoAtivExtra.Visible = false;
                hdnOrdenacaoAtivExtra.Value = "";
            }
            else
            {
                DataTable dt = CLS_TurmaAtividadeExtraClasseBO.SelecionaPorPeriodoDisciplina_Alunos(
                           VisibilidadeRegencia(ddlTurmaDisciplinaListao) ?
                                   ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                               , UCNavegacaoTelaPeriodo.VS_tpc_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao
                                , PosicaoDocente, tur_ids);

                if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.Normal)
                {
                    lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(UCNavegacaoTelaPeriodo.VS_tpc_id, UCControleTurma1.VS_tur_id, UCControleTurma1.VS_tud_id, ApplicationWEB.AppMinutosCacheMedio);
                }

                // Carregar as atividades e notas dos alunos nas atividades.
                var x = (from DataRow dr in dt.Rows
                         where !string.IsNullOrEmpty(dr["tae_id"].ToString())
                         select dr);

                DTAtividadeExtraclasse = x.Any() ? x.CopyToDataTable() : new DataTable();

                lblSemAtividadeExtra.Text = UtilBO.GetErroMessage("Não foi encontrada atividade extraclasse para a turma no período e componente curricular selecionados.", UtilBO.TipoMensagem.Alerta);

                lblSemAtividadeExtra.Visible = !x.Any();

                rptAlunoAtivExtra.DataSource = ListaAlunos;
                rptAlunoAtivExtra.DataBind();

                hdnOrdenacaoAtivExtra.Value = "";

            }

            updAtiExtra.Update();
        }

        private void UCConfirmacaoOperacao_ConfimaOperacao()
        {
            ExcluirAtividadeExtraClasse();
        }


        /// <summary>
        /// Salva listão de avaliações.
        /// </summary>
        /// <param name="PermaneceTela"></param>
        /// <returns></returns>
        public bool SalvarAtividadeExtra(out string msg)
        {
            msg = "";
            if (!VS_Periodo_Aberto)
                throw new ValidationException(String.Format("Listão de atividades extraclasse de {0} disponível apenas para consulta.", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

            List<CLS_TurmaAtividadeExtraClasseAluno> listaTumaAtividadeExtraclasse = new List<CLS_TurmaAtividadeExtraClasseAluno>();

            bool visibilidadeRegencia = VisibilidadeRegencia(ddlTurmaDisciplinaListao);
            List<CLS_TurmaAtividadeExtraClasse> lstRelacionamento = new List<CLS_TurmaAtividadeExtraClasse>();
            foreach (RepeaterItem itemAluno in rptAlunoAtivExtra.Items)
            {
                Repeater rptAtividades = (Repeater)itemAluno.FindControl("rptAtividades");
                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);
                string pes_nome = Convert.ToString(((Label)itemAluno.FindControl("lblNomeOficial")).Text);

                // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                foreach (RepeaterItem itemAtividadeAluno in rptAtividades.Items)
                {
                    HtmlGenericControl divAtividades = (HtmlGenericControl)itemAtividadeAluno.FindControl("divAtividades");
                    if (divAtividades != null)
                    {
                        CheckBox chkEntregou = (CheckBox)itemAtividadeAluno.FindControl("chkEntregou");
                        bool aea_entregue = chkEntregou != null ? chkEntregou.Checked : false;

                        Int16 tdt_posicao = Convert.ToInt16(((Label)itemAtividadeAluno.FindControl("lblTaePosicao")).Text);

                        if (!(PosicaoDocente > 0 && !VS_ltPermissaoAtividadeExtraclasse.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao)))
                        {
                            if (CLS_TurmaNotaAlunoBO.VerificaValoresNotas(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica, RetornaAvaliacao(itemAtividadeAluno), pes_nome))
                            {
                                int tae_id = Convert.ToInt32(((Label)itemAtividadeAluno.FindControl("lbltae_id")).Text);

                                // Busca relatório lançado.
                                NotasRelatorioAtiExtra rel = VS_Nota_RelatorioAtiExtra.Find(p =>
                                    p.alu_id == alu_id
                                    && p.tae_id == tae_id
                                    && p.mtu_id == mtu_id);

                                CLS_TurmaAtividadeExtraClasseAluno ent = new CLS_TurmaAtividadeExtraClasseAluno
                                {
                                    tud_id = visibilidadeRegencia ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id,
                                    tae_id = tae_id,
                                    alu_id = alu_id,
                                    mtu_id = mtu_id,
                                    mtd_id = mtd_id,
                                    aea_avaliacao = RetornaAvaliacao(itemAtividadeAluno),
                                    aea_relatorio = rel.valor,
                                    aea_entregue = aea_entregue,
                                    aea_dataAlteracao = DateTime.Now,
                                    aea_situacao = 1
                                };
                                listaTumaAtividadeExtraclasse.Add(ent);

                                Label lbltaer_id = itemAtividadeAluno.FindControl("lbltaer_id") as Label;
                                if (lbltaer_id != null && !string.IsNullOrEmpty(lbltaer_id.Text))
                                {
                                    lstRelacionamento.Add(new CLS_TurmaAtividadeExtraClasse { tud_id = ent.tud_id, tae_id = ent.tae_id, taer_id = new Guid(lbltaer_id.Text) });
                                }
                            }
                        }

                        HabilitaControles(divAtividades.Controls, true);
                    }
                }
            }

            hdnAlterouAtividadeExtra.Value = "";

            if (CLS_TurmaAtividadeExtraClasseAlunoBO.SalvarEmLote(listaTumaAtividadeExtraclasse, EntTurmaDisciplina.tud_id, UCNavegacaoTelaPeriodo.VS_tpc_id, EntTurmaDisciplina.tud_tipo, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico, Ent_ID_UsuarioLogado, lstRelacionamento.FindAll(p => p.taer_id != Guid.Empty).GroupBy(p => p.taer_id).Select(g => g.First()).ToList(), UCControleTurma1.VS_tur_id))
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Listão de atividade extraclasse | " +
                                                                        "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                        " | tur_id: " + UCControleTurma1.VS_tur_id + "; tud_id: " + (visibilidadeRegencia ?
                                                    ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id));

                msg = UtilBO.GetErroMessage("Listão de atividades extraclasse salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

                CarregarListaoAtividadeExtraclasse();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Exclui atividade extraclasse
        /// </summary>
        private void ExcluirAtividadeExtraClasse()
        {
            try
            {
                CLS_TurmaAtividadeExtraClasse entity = new CLS_TurmaAtividadeExtraClasse
                {
                    tud_id = VS_tud_idAtiExtraExcluir
                    ,
                    tae_id = VS_tae_idAtiExtraExcluir
                    ,
                    taer_id = VS_taer_idAtiExtraExcluir
                };

                var lstNota =
                    (from RepeaterItem itemAluno in rptAlunoAtivExtra.Items
                     let rptAtividades = itemAluno.FindControl("rptAtividades") as Repeater
                     from RepeaterItem itemNota in rptAtividades.Items
                     let lbltud_idNota = itemNota.FindControl("lbltud_id") as Label
                     let lbltae_idNota = itemNota.FindControl("lbltae_id") as Label
                     where (lbltud_idNota != null && lbltud_idNota.Text == VS_tud_idAtiExtraExcluir.ToString()) &&
                           (lbltae_idNota != null && lbltae_idNota.Text == VS_tae_idAtiExtraExcluir.ToString())
                     let txtNota = itemNota.FindControl("txtNota") as TextBox
                     let ddlPareceres = itemNota.FindControl("ddlPareceres") as DropDownList
                     let imgSituacao = itemNota.FindControl("imgSituacao") as Image
                     let chkEntregou = itemNota.FindControl("chkEntregou") as CheckBox
                     select new
                     {
                         possuiNota = (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica && !string.IsNullOrEmpty(txtNota.Text)) ||
                                      (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres && ddlPareceres.SelectedValue != "-1") ||
                                      (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Relatorios && imgSituacao.Visible) ||
                                      chkEntregou.Checked
                     }).ToList();

                if (lstNota.Any(p => p.possuiNota))
                {
                    throw new ValidationException("Não foi possível excluir a atividade extraclasse, pois já foram registrados os lançamentos.");
                }

                if (CLS_TurmaAtividadeExtraClasseBO.Deletar(entity))
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Atividade extraclasse excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, string.Format("Listão de atividade extraclasse | Exclusão de atividade | tud_id: {0}, tae_id: {1}", VS_tud_idAtiExtraExcluir, VS_tae_idAtiExtraExcluir));
                    CarregarListaoAtividadeExtraclasse();
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a atividade extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        private void AbrirRelatorioRP(long alu_id, string tds_idRP)
        {
            Session.Remove("alu_id_RelatorioRP");
            Session.Remove("tds_id_RelatorioRP");
            Session.Remove("PaginaRetorno_RelatorioRP");

            Session.Add("alu_id_RelatorioRP", alu_id);
            Session.Add("tds_id_RelatorioRP", tds_idRP);
            Session.Add("PaginaRetorno_RelatorioRP", Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/ControleTurma/Listao.aspx"));

            CarregaSessionPaginaRetorno();
            RedirecionarPagina("~/Classe/RelatorioRecuperacaoParalela/Cadastro.aspx");
        }

        private void AbrirRelatorioAEE(long alu_id)
        {
            Session.Remove("alu_id_RelatorioAEE");
            Session.Remove("PaginaRetorno_RelatorioAEE");

            Session.Add("alu_id_RelatorioAEE", alu_id);
            Session.Add("PaginaRetorno_RelatorioAEE", Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/ControleTurma/Listao.aspx"));

            CarregaSessionPaginaRetorno();
            RedirecionarPagina("~/Classe/RelatorioAtendimento/Cadastro.aspx");
        }

        private void CarregarCadastroAtividadeExtraclasse(long tud_id, int tae_id, bool habilitaEdicao)
        {
            CLS_TurmaAtividadeExtraClasse entity;
            if (tud_id > 0 && tae_id > 0)
            {
                entity = new CLS_TurmaAtividadeExtraClasse
                {
                    tud_id = tud_id
                    ,
                    tae_id = tae_id
                };
                CLS_TurmaAtividadeExtraClasseBO.GetEntity(entity);
            }
            else
            {
                entity = new CLS_TurmaAtividadeExtraClasse();
            }

            hdnTaeId.Value = entity.tae_id.ToString();
            hdnTaerId.Value = entity.taer_id.ToString();
            UCComboTipoAtividadeAvaliativa.Valor = entity.tav_id > 0 ? entity.tav_id : -1;
            txtNomeAtiExtra.Text = entity.tae_nome;
            txtDescricaoAtiExtra.Text = entity.tae_descricao;
            txtCargaAtiExtra.Text = entity.tae_cargaHoraria > 0 ? Convert.ToInt32(entity.tae_cargaHoraria).ToString() : string.Empty;
            List<TUR_TurmaDisciplina> turmaDisciplina = new List<TUR_TurmaDisciplina>();
            if (entity.tae_id > 0)
            {
                if (entity.taer_id != Guid.Empty)
                {
                    turmaDisciplina = CLS_TurmaAtividadeExtraClasseBO.SelecionaDisciplinaAtividadeExtraclasseRelacionada(entity.taer_id);
                }
            }
            else
            {
                turmaDisciplina = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(UCControleTurma1.VS_tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
            }
            rptDisciplinasAtiExtra.DataSource = turmaDisciplina.FindAll(p => p.tud_id != EntTurmaDisciplina.tud_id);
            rptDisciplinasAtiExtra.DataBind();
            lblDisciplinasAtiExtra.Visible = rptDisciplinasAtiExtra.Visible = rptDisciplinasAtiExtra.Items.Count > 0;

            HabilitaControles(fdsCadastroAtiExtra.Controls, habilitaEdicao);
            btnCancelarAtiExtra.Enabled = true;

            if (habilitaEdicao && entity.tav_id > 0)
            {
                // Não permitir alterar as disciplinas
                HabilitaControles(rptDisciplinasAtiExtra.Controls, false);
            }
            btnAdicionarAtiExtra.Visible = habilitaEdicao;

            fdsCadastroAtiExtra.Visible = true;
            fdsListagemAtiExtra.Visible = false;

            UCComboTipoAtividadeAvaliativa.Obrigatorio = true;
            updAtiExtra.Update();
        }

        #endregion Métodos

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
                            UCNavegacaoTelaPeriodo.VS_tpc_id = Convert.ToInt32(listaDados["Edit_tpc_id"]);
                            UCNavegacaoTelaPeriodo.VS_tpc_ordem = Convert.ToInt32(listaDados["Edit_tpc_ordem"]);
                            UCControleTurma1.VS_tur_tud_ids = (List<string>)(Session["tur_tud_ids"] ?? new List<string>());
                            UCControleTurma1.LabelTurmas = listaDados["TextoTurmas"];
                        }

                        byte tipoPendencia = 0;
                        int tpcIdPendencia = -1;
                        long tudIdPendencia = -1;
                        if (Session["tipoPendencia"] != null)
                        {
                            tipoPendencia = Convert.ToByte(Session["tipoPendencia"]);
                        }
                        if (Session["tpcIdPendencia"] != null)
                        {
                            tpcIdPendencia = Convert.ToInt32(Session["tpcIdPendencia"]);
                        }
                        if (Session["tudIdPendencia"] != null)
                        {
                            tudIdPendencia = Convert.ToInt64(Session["tudIdPendencia"]);
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
                        Session.Remove("tipoPendencia");
                        Session.Remove("tpcIdPendencia");
                        Session.Remove("tudIdPendencia");
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

                            if (UCControleTurma1.VS_tud_id != null || UCControleTurma1.VS_tdt_posicao != null)
                            {
                                dadosTodasTurmas.All(p =>
                                {
                                    dadosTurma.AddRange(p.Turmas.Where(t => t.tud_id == UCControleTurma1.VS_tud_id && t.tdt_posicao == UCControleTurma1.VS_tdt_posicao));
                                    return true;
                                });

                                VS_situacaoTurmaDisciplina = dadosTurma.FirstOrDefault().tdt_situacao;

                                UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                            }
                        }

                        VS_turmasAnoAtual = dadosTurma.FirstOrDefault().turmasAnoAtual;

                        hdnListaoSelecionado.Value = "0";

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.Listao;

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
                        }

                        UCNavegacaoTelaPeriodo.CarregarPeriodos(VS_ltPermissaoFrequencia, VS_ltPermissaoEfetivacao,
                                        VS_ltPermissaoPlanejamentoAnual, VS_ltPermissaoAvaliacao,
                                        entDisciplinaRelacionada, UCControleTurma1.VS_esc_id,
                                        VS_EntitiesControleTurma.turmaDisciplina.tud_tipo, UCControleTurma1.VS_tdt_posicao, UCControleTurma1.VS_tur_id, VS_EntitiesControleTurma.turmaDisciplina.tud_id, true, tpcIdPendencia);

                        if (UCNavegacaoTelaPeriodo.VS_tpc_id <= 0)
                        {
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Escola não permite lançar dados.", UtilBO.TipoMensagem.Alerta);
                            RedirecionarPagina(UCNavegacaoTelaPeriodo.VS_paginaRetorno);
                        }

                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);

                        CarregarDisciplinasComboListao(UCControleTurma1.VS_tur_id, tudIdPendencia > 0 ? tudIdPendencia : VS_EntitiesControleTurma.turmaDisciplina.tud_id);
                        CarregarTela();

                        if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemNota)
                        {
                            int indiceAba = 1;
                            if (!aFrequencia.Visible)
                            {
                                indiceAba--;
                            }
                            hdnListaoSelecionado.Value = indiceAba.ToString();
                        }
                        else if (tipoPendencia == (byte)REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia.SemPlanoAula)
                        {
                            int indiceAba = 2;
                            if (!aFrequencia.Visible)
                            {
                                indiceAba--;
                            }
                            if (!aAvaliacao.Visible)
                            {
                                indiceAba--;
                            }
                            hdnListaoSelecionado.Value = indiceAba.ToString();
                        }

                        //bloquear botoes -> se o docente nao possuir mais essa turma e se nao for turma extinta
                        if (VS_situacaoTurmaDisciplina != 1
                            && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Encerrada
                            && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Extinta)
                        {
                            //UCNavegacaoTelaPeriodo.VisiblePlanejamentoAnual = false;
                            //UCNavegacaoTelaPeriodo.VisibleEfetivacao = false;
                            btnCompensacaoAusencia.Visible = false;
                        }
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
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }

            UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
            UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
            UCControleTurma1.DisciplinaCompartilhadaIndexChanged = uccDisciplinaCompartilhada_IndexChanged;
            UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
            UCLancamentoFrequencia.Recarregar += UCLancamentoFrequencia_Recarregar;
            UCLancamentoFrequencia.CarregarAusencias += UCLancamentoFrequencia_CarregarAusencias;
            UCLancamentoFrequenciaTerritorio.Recarregar += UCLancamentoFrequenciaTerritorio_Recarregar;
            UCLancamentoFrequenciaTerritorio.CarregarAusencias += UCLancamentoFrequenciaTerritorio_CarregarAusencias;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            UCControleTurma1.chkTurmasNormaisMultisseriadasIndexChanged += UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged;
            UCConfirmacaoOperacao.ConfimaOperacao += UCConfirmacaoOperacao_ConfimaOperacao;
            UCLancamentoFrequencia.AbrirRelatorioRP += UCLancamentoFrequencia_AbrirRelatorioRP;
            UCLancamentoFrequenciaTerritorio.AbrirRelatorioRP += UCLancamentoFrequencia_AbrirRelatorioRP;
            UCLancamentoFrequencia.AbrirRelatorioAEE += UCLancamentoFrequencia_AbrirRelatorioAEE;
            UCLancamentoFrequenciaTerritorio.AbrirRelatorioAEE += UCLancamentoFrequencia_AbrirRelatorioAEE;

            // Configura javascripts da tela.
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));

                string script;
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

                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tablesorter.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.metadata.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsLancamentoAvaliacoesGeral.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsControleTurma_Listao.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));

                script = GeraScriptVariaveisTurma();

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "VariaveisScript", script, true);
            }

            trExibirAlunoDispensadoListao.Visible = UCLancamentoFrequencia.VisivelAlunoDispensado = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        private void UCLancamentoFrequencia_AbrirRelatorioRP(long alu_id, string tds_idRP)
        {
            AbrirRelatorioRP(alu_id, tds_idRP);
        }

        private void UCLancamentoFrequencia_AbrirRelatorioAEE(long alu_id)
        {
            AbrirRelatorioAEE(alu_id);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (VS_PeriodoEfetivado)
            {
                lblPeriodoEfetivado.Visible = true;
                if (UCNavegacaoTelaPeriodo.VS_tpc_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemEfetivadoRecesso").ToString(),
                                                                    UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemEfetivado").ToString(),
                                                                    UtilBO.TipoMensagem.Alerta);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Legenda Listão de Avaliação
                    if (tbLegendaListao != null)
                    {
                        HtmlTableCell cell = tbLegendaListao.Rows[0].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.AlunoAusente;
                        cell = tbLegendaListao.Rows[1].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.AlunoDispensado;
                        cell = tbLegendaListao.Rows[2].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.AlunoInativo;
                    }

                    UCLancamentoFrequenciaTerritorio.CarregarLegenda();
                    UCLancamentoFrequencia.CarregarLegenda();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
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
                        Response.Redirect("~/Academico/ControleTurma/Listao.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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
                    Response.Redirect("~/Academico/ControleTurma/Listao.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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

                    Response.Redirect("~/Academico/ControleTurma/Listao.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Recarrega o grid do listao de frequencia.
        /// </summary>
        /// <param name="atualizaData"></param>
        /// <param name="proximo"></param>
        /// <param name="anterior"></param>
        /// <param name="inalterado"></param>
        private void UCLancamentoFrequencia_Recarregar(bool atualizaData, bool proximo, bool anterior, bool inalterado)
        {
            CarregarListaoFrequencia(atualizaData, proximo, anterior, inalterado);
        }

        /// <summary>
        /// Carrega as ausências pelos filtros informados.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="mtd_id"></param>
        void UCLancamentoFrequencia_CarregarAusencias(long alu_id, int mtu_id, int mtd_id)
        {
            try
            {
                gvCompAusencia.PageIndex = 0;
                gvCompAusencia.DataSourceID = odsCompAusencia.ID;
                odsCompAusencia.SelectParameters.Clear();
                odsCompAusencia.SelectParameters.Add("tud_id", EntTurmaDisciplina.tud_id.ToString());
                odsCompAusencia.SelectParameters.Add("tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
                odsCompAusencia.SelectParameters.Add("alu_id", alu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtu_id", mtu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtd_id", mtd_id.ToString());

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                gvCompAusencia.PageSize = itensPagina;
                // Seta nome da coluna periodos
                gvCompAusencia.Columns[2].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                // atualiza o grid
                gvCompAusencia.DataBind();
                fdsResultados.Visible = true;
                UCComboQtdePaginacao1.Visible = gvCompAusencia.Rows.Count > 0;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConsultaCompensacao", "$(document).ready(function() { $('#divCompensacao').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações de ausências.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnCompensacao.Update();
            }
        }

        /// <summary>
        /// Recarrega o grid do listao de frequencia dos territorios do saber.
        /// </summary>
        /// <param name="atualizaData"></param>
        /// <param name="proximo"></param>
        /// <param name="anterior"></param>
        /// <param name="inalterado"></param>
        private void UCLancamentoFrequenciaTerritorio_Recarregar(bool atualizaData, bool proximo, bool anterior, bool inalterado)
        {
            CarregarListaoFrequenciaTerritorio(atualizaData, proximo, anterior, inalterado);
        }

        /// <summary>
        /// Carrega as ausências pelos filtros informados para os territorios do saber.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="mtd_id"></param>
        void UCLancamentoFrequenciaTerritorio_CarregarAusencias(long alu_id, int mtu_id, int mtd_id)
        {
            try
            {
                gvCompAusencia.PageIndex = 0;
                gvCompAusencia.DataSourceID = odsCompAusencia.ID;
                odsCompAusencia.SelectParameters.Clear();
                odsCompAusencia.SelectParameters.Add("tud_id", EntTurmaDisciplina.tud_id.ToString());
                odsCompAusencia.SelectParameters.Add("tpc_id", UCNavegacaoTelaPeriodo.VS_tpc_id.ToString());
                odsCompAusencia.SelectParameters.Add("alu_id", alu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtu_id", mtu_id.ToString());
                odsCompAusencia.SelectParameters.Add("mtd_id", mtd_id.ToString());

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                gvCompAusencia.PageSize = itensPagina;
                // Seta nome da coluna periodos
                gvCompAusencia.Columns[2].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                // atualiza o grid
                gvCompAusencia.DataBind();
                fdsResultados.Visible = true;
                UCComboQtdePaginacao1.Visible = gvCompAusencia.Rows.Count > 0;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConsultaCompensacao", "$(document).ready(function() { $('#divCompensacao').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações de ausências.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnCompensacao.Update();
            }
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // atribui nova quantidade itens por página para o grid
                gvCompAusencia.PageSize = UCComboQtdePaginacao1.Valor;
                gvCompAusencia.PageIndex = 0;
                // atualiza o grid
                gvCompAusencia.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string msg = "";
            VS_recarregarDataAula = VS_recarregarDataNota = true;
            LstTauSalvas = new List<int>();

            int alterouFrequencia = 0;
            int alterouNota = 0;
            int alterouPlano = 0;
            int alterouAtividadeExtra = 0;

            //Salva frequência
            try
            {
                Int32.TryParse(hdnAlterouFrequencia.Value, out alterouFrequencia);

                if (Page.IsValid && pnlListaoLancamentoFrequencias.Visible && PermiteLancarFrequencia && !VS_PeriodoEfetivado && alterouFrequencia > 0)
                {
                    List<int> lstTauSalvas = LstTauSalvas;
                    bool recarregarDataAula = VS_recarregarDataAula;
                    if (UCLancamentoFrequencia.Visible)
                    {
                        UCLancamentoFrequencia.Salvar(out msg
                                                    , VS_PeriodoEfetivado
                                                    , VS_Periodo_Aberto
                                                    , ref lstTauSalvas
                                                    , VS_EntitiesControleTurma
                                                    , VS_Data_Listao_TurmaAula
                                                    , ref recarregarDataAula
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , PosicaoDocente
                                                    , PermiteLancarFrequencia
                                                    , VS_situacaoTurmaDisciplina);
                    }
                    else
                    {
                        UCLancamentoFrequenciaTerritorio.Salvar(out msg
                                                    , VS_PeriodoEfetivado
                                                    , VS_Periodo_Aberto
                                                    , ref lstTauSalvas
                                                    , VS_EntitiesControleTurma
                                                    , VS_Data_Listao_TurmaAula
                                                    , ref recarregarDataAula
                                                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                                                    , PosicaoDocente
                                                    , PermiteLancarFrequencia
                                                    , VS_situacaoTurmaDisciplina);
                    }
                    LstTauSalvas = lstTauSalvas;
                    VS_recarregarDataAula = recarregarDataAula;
                    hdnAlterouFrequencia.Value = string.Empty;
                }
            }
            catch (ThreadAbortException) { }
            catch (ValidationException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage("Erro ao tentar salvar as frequências.", UtilBO.TipoMensagem.Erro);
            }

            if (!string.IsNullOrEmpty(msg))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = msg;
            }
            msg = "";

            //Salva avaliação
            try
            {

                Int32.TryParse(hdnAlterouNota.Value, out alterouNota);

                if (Page.IsValid && pnlLancamentoAvaliacao.Visible && PermiteLancarNota && !VS_PeriodoEfetivado && alterouNota > 0)
                    SalvarAvaliacao(out msg);
            }
            catch (ValidationException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage("Erro ao tentar salvar as notas.", UtilBO.TipoMensagem.Erro);
            }

            if (!string.IsNullOrEmpty(msg))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text += msg;
            }
            msg = "";

            //Salva plano de aula
            try
            {
                Int32.TryParse(hdnAlterouPlanoAula.Value, out alterouPlano);

                if (Page.IsValid && PermiteLancarPlanoAula && alterouPlano > 0)
                    SalvarPlanoAula(out msg);
            }
            catch (ValidationException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage("Erro ao tentar salvar os planos de aula.", UtilBO.TipoMensagem.Erro);
            }

            //Salva atividade extraclasse
            try
            {
                if (fdsCadastroAtiExtra.Visible)
                {
                    if (btnAdicionarAtiExtra.Visible && btnAdicionarAtiExtra.Enabled)
                    {
                        Page.Validate("AtividadeExtraclasse");
                        if (Page.IsValid)
                            btnAdicionarAtiExtra_Click(null, null);
                    }
                    else
                    {
                        btnCancelarAtiExtra_Click(null, null);
                    }
                }

                Int32.TryParse(hdnAlterouAtividadeExtra.Value, out alterouAtividadeExtra);
                if (Page.IsValid && pnlAtividadesExtraClasse.Visible && aAtividadeExtraClasse.Visible && !VS_PeriodoEfetivado && alterouAtividadeExtra > 0 && PermiteLancarAtividadeExtraclasse)
                    SalvarAtividadeExtra(out msg);
            }
            catch (ValidationException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                msg += UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                msg += UtilBO.GetErroMessage("Erro ao tentar salvar as notas de atividades extraclasse.", UtilBO.TipoMensagem.Erro);
            }

            if (alterouFrequencia + alterouNota + alterouPlano + alterouAtividadeExtra == 0)
            {
                msg += UtilBO.GetErroMessage("Nenhum dado foi alterado.", UtilBO.TipoMensagem.Alerta);
            }

            if (VS_recarregarDataAula)
                VS_Data_Listao_TurmaAula = DateTime.Now.AddSeconds(1);

            if (VS_recarregarDataNota)
                VS_Data_Listao_TurmaNota = DateTime.Now.AddSeconds(1);

            if (!string.IsNullOrEmpty(msg))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text += msg;
            }
            msg = "";
        }

        protected void btnRelatorio_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Recuperando item que chamou.
                ImageButton btnRelatorio = (ImageButton)sender;
                RepeaterItem itemAtividade = (RepeaterItem)btnRelatorio.NamingContainer;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int tnt_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltnt_id")).Text);

                NotasRelatorio rel = VS_Nota_Relatorio.Find(p =>
                    p.alu_id == alu_id
                    && p.tnt_id == tnt_id
                    && p.mtu_id == mtu_id);

                // Guarda o tipo de alteração, o alu_id, o mtu_id e o tnt_id da linha que está sendo editada.
                hdnIds.Value = 1 + ";" + alu_id + ";" + tnt_id + ";" + mtu_id + ";1";

                lblDadosRelatorio.Text = "<b>Nome do aluno:</b> " + ((Label)itemAluno.FindControl("lblNome")).Text;
                txtRelatorio.Text = rel.valor;

                // Abrir relatório.
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnRelatorioAtiExtra_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Recuperando item que chamou.
                ImageButton btnRelatorio = (ImageButton)sender;
                RepeaterItem itemAtividade = (RepeaterItem)btnRelatorio.NamingContainer;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                int tae_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltae_id")).Text);

                NotasRelatorioAtiExtra rel = VS_Nota_RelatorioAtiExtra.Find(p =>
                    p.alu_id == alu_id
                    && p.tae_id == tae_id
                    && p.mtu_id == mtu_id);

                // Guarda o tipo de alteração, o alu_id, o mtu_id e o tnt_id da linha que está sendo editada.
                hdnIds.Value = 1 + ";" + alu_id + ";" + tae_id + ";" + mtu_id + ";2";

                lblDadosRelatorio.Text = "<b>Nome do aluno:</b> " + ((Label)itemAluno.FindControl("lblNome")).Text;
                txtRelatorio.Text = rel.valor;

                // Abrir relatório.
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }


        protected void btnCompensacaoAusencia_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Add("PaginaRetorno_CompensacaoAusencia", "~/Academico/ControleTurma/Listao.aspx");
                CarregaSessionPaginaRetorno();

                Response.Redirect("~/Classe/CompensacaoAusencia/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                string[] s = hdnIds.Value.Split(';');

                int tipoAlteracao = Convert.ToInt32(s[0]);
                long alu_id = Convert.ToInt32(s[1]);
                int id = Convert.ToInt32(s[2]);
                int mtu_id = Convert.ToInt32(s[3]);
                int tipoListao = Convert.ToInt32(s[4]);

                if (tipoAlteracao == 1 && tipoListao == 1)
                {
                    if (VS_Nota_Relatorio.Exists(p =>
                        p.tnt_id == id
                        && p.alu_id == alu_id
                        && p.mtu_id == mtu_id))
                    {
                        int alterar = VS_Nota_Relatorio.FindIndex(p =>
                            p.tnt_id == id
                            && p.alu_id == alu_id
                            && p.mtu_id == mtu_id);

                        VS_Nota_Relatorio[alterar] = new NotasRelatorio
                        {
                            valor = txtRelatorio.Text
                            ,
                            tnt_id = id
                            ,
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                        };
                    }
                    else
                        AdicionaItemRelatorio(id, alu_id, mtu_id, txtRelatorio.Text);

                    // Percorre os itens do repeater para atualizar os botões de relatório.
                    foreach (RepeaterItem item in rptAlunosAvaliacao.Items)
                    {
                        Repeater rptAtividades = (Repeater)item.FindControl("rptAtividadesAvaliacao");
                        foreach (RepeaterItem itemAtividade in rptAtividades.Items)
                            SetaImgRelatorio(itemAtividade);
                    }
                }
                else if (tipoAlteracao == 1 && tipoListao == 2)
                {
                    if (VS_Nota_RelatorioAtiExtra.Exists(p =>
                        p.tae_id == id
                        && p.alu_id == alu_id
                        && p.mtu_id == mtu_id))
                    {
                        int alterar = VS_Nota_RelatorioAtiExtra.FindIndex(p =>
                            p.tae_id == id
                            && p.alu_id == alu_id
                            && p.mtu_id == mtu_id);

                        VS_Nota_RelatorioAtiExtra[alterar] = new NotasRelatorioAtiExtra
                        {
                            valor = txtRelatorio.Text
                            ,
                            tae_id = id
                            ,
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                        };
                    }
                    else
                        AdicionaItemRelatorioAtiExtra(id, alu_id, mtu_id, txtRelatorio.Text);

                    // Percorre os itens do repeater para atualizar os botões de relatório.
                    foreach (RepeaterItem item in rptAlunoAtivExtra.Items)
                    {
                        Repeater rptAtividades = (Repeater)item.FindControl("rptAtividades");
                        foreach (RepeaterItem itemAtividade in rptAtividades.Items)
                            SetaImgRelatorioAtiExtra(itemAtividade);
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('close'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptAlunosAvaliacao_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RelatorioRP")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    AbrirRelatorioRP(Convert.ToInt64(args[0]), args[1]);
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
                    AbrirRelatorioAEE(Convert.ToInt64(e.CommandArgument.ToString()));
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir os relatórios do AEE para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rptAlunosAvaliacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                if (e.Item.ItemType == ListItemType.Header)
                {
                    Label lblMedia = (Label)e.Item.FindControl("lblMedia");

                    if (Vs_calcula_notaFinal)
                        lblMedia.Text = (tipo == EscalaAvaliacaoTipo.Numerica) ? "Nota final" : "Conceito final";
                }

                // Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
                SetaNomeAluno(e);

                Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                //Parametros dispensa disciplina - pinta linha
                Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;

                // Aluno inativo - pinta a linha
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));

                // Carrega repeater de atividades e avaliações da secretaria.
                CarregaRepeateresInternos(e, Alu_id);

                if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    Label lblMedia = (Label)e.Item.FindControl("lblMedia");
                    TextBox txtNotaFinal = (TextBox)e.Item.FindControl("txtNotaFinal");
                    DropDownList ddlParecerFinal = (DropDownList)e.Item.FindControl("ddlParecerFinal");

                    Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                    Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));

                    if (!Vs_calcula_notaFinal)
                    {
                        if (ddlParecerFinal != null)
                            ddlParecerFinal.Visible = false;

                        if (txtNotaFinal != null)
                            txtNotaFinal.Visible = false;
                    }
                    else
                    {
                        if (tipo == EscalaAvaliacaoTipo.Pareceres)
                        {
                            // Carregar combo de pareceres.
                            if (ddlParecerFinal != null)
                            {
                                ddlParecerFinal.Items.Insert(0, new ListItem("-- Selecione um conceito --", "-1", true));
                                ddlParecerFinal.AppendDataBoundItems = true;
                                ddlParecerFinal.DataSource = LtPareceres;
                                ddlParecerFinal.DataBind();

                                ddlParecerFinal.Enabled = VS_Periodo_Aberto && usuarioPermissao && PermiteLancarNota && VS_situacaoTurmaDisciplina == 1 && !VS_PeriodoEfetivado;
                            }

                            string atm_media = "";
                            if (alunoMediaFinal != null && alunoMediaFinal.Count > 0)
                            {
                                CLS_AlunoAvaliacaoTurmaDisciplinaMedia media = alunoMediaFinal.FirstOrDefault(p => p.alu_id == Alu_id && p.mtu_id == Mtu_id);
                                atm_media = media != null ? media.atm_media : "";
                            }

                            Double atmmedia;
                            ddlParecerFinal.SelectedValue = Double.TryParse(atm_media, out atmmedia)
                                ? String.Format("{0:F" + NumeroCasasDecimais + "}", atmmedia)
                                : atm_media;

                            if (lblMedia != null)
                                lblMedia.Visible = false;

                            if (txtNotaFinal != null)
                                txtNotaFinal.Visible = false;
                        }
                        else
                        {
                            if (txtNotaFinal != null)
                            {
                                string atm_media = (alunoMediaFinal == null || alunoMediaFinal.Count == 0) ? "" :
                                    alunoMediaFinal.FirstOrDefault(p => p.alu_id == Alu_id && p.mtu_id == Mtu_id).atm_media;

                                Double atmmedia;
                                txtNotaFinal.Text = Double.TryParse(atm_media, out atmmedia)
                                    ? String.Format("{0:F" + NumeroCasasDecimais + "}", atmmedia)
                                    : atm_media;

                                txtNotaFinal.Enabled = VS_Periodo_Aberto && usuarioPermissao && PermiteLancarNota && VS_situacaoTurmaDisciplina == 1 && !VS_PeriodoEfetivado;
                            }

                            if (lblMedia != null)
                                lblMedia.Visible = false;

                            if (ddlParecerFinal != null)
                                ddlParecerFinal.Visible = false;
                        }
                    }

                    if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        HtmlControl tdNumChamadaAvaliacao = (HtmlControl)e.Item.FindControl("tdNumChamadaAvaliacao");
                        HtmlControl tdNomeAvaliacao = (HtmlControl)e.Item.FindControl("tdNomeAvaliacao");
                        HtmlControl tdMedia = (HtmlControl)e.Item.FindControl("tdMedia");

                        tdNumChamadaAvaliacao.Style["background-color"] = tdNomeAvaliacao.Style["background-color"] =
                        tdMedia.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }

                    LinkButton btnRelatorioAEE = (LinkButton)e.Item.FindControl("btnRelatorioAEE");
                    if (btnRelatorioAEE != null)
                    {
                        btnRelatorioAEE.Visible = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "alu_situacaoID")) == (byte)ACA_AlunoSituacao.Ativo
                                                    && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PossuiDeficiencia"));
                        btnRelatorioAEE.CommandArgument = Alu_id.ToString();
                    }

                    // Mostra o ícone para as anotações de recuperação paralela (RP):
                    // - para todos os alunos, quando a turma for de recuperação paralela,
                    // - ou apenas para alunos com anotações de RP, quando for a turma regular relacionada com a recuperação paralela.
                    if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno
                        || lstAlunosRelatorioRP.Any(p => p.alu_id == Alu_id))
                    {
                        LinkButton btnRelatorioRP = (LinkButton)e.Item.FindControl("btnRelatorioRP");
                        if (btnRelatorioRP != null)
                        {
                            btnRelatorioRP.Visible = true;
                            btnRelatorioRP.CommandArgument = Alu_id.ToString();

                            if (VisibilidadeRegencia(ddlTurmaDisciplinaListao))
                            {
                                string strTds = string.Empty;
                                (from Struct_PreenchimentoAluno preenchimento in lstAlunosRelatorioRP.FindAll(p => p.alu_id == Alu_id)
                                 group preenchimento by new { tds_id = preenchimento.tds_idRelacionada } into grupo
                                 select grupo.Key.tds_id).ToList().ForEach(p => strTds += string.Format(",{0}", p.ToString()));
                                if (strTds.Length > 1)
                                {
                                    btnRelatorioRP.CommandArgument += string.Format(";{0}", strTds.Substring(1));
                                }
                                else
                                {
                                    btnRelatorioRP.CommandArgument += string.Format(";{0}", "-1");
                                }
                            }
                            else
                            {
                                btnRelatorioRP.CommandArgument += string.Format(";{0}", "-1");
                            }
                        }
                    }
                }
            }
        }

        protected void gvCompAusencia_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CLS_CompensacaoAusenciaBO.GetTotalRecords();
        }

        protected void ddlComponenteListao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarListaoAvaliacao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true);
        }

        protected void rptAtividadesHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Se for cabeçalho, setar valor do checkbox.
                CheckBox chkEfetivado = (CheckBox)e.Item.FindControl("chkEfetivado");

                // Se for cabeçalho, setar valor do checkbox.
                HtmlControl divDetalharHabilidades = (HtmlControl)e.Item.FindControl("divDetalharHabilidades");
                if (divDetalharHabilidades != null)
                {
                    divDetalharHabilidades.Visible =
                        ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(
                            eChaveAcademico.RELACIONAR_HABILIDADES_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                bool permissaoAlteracao = PermiteLancarNota && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;

                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= !VS_PeriodoEfetivado;

                if (chkEfetivado != null)
                {
                    bool tnt_efetivado = false;

                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tnt_efetivado").ToString()))
                    {
                        tnt_efetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tnt_efetivado"));
                    }

                    chkEfetivado.Checked = tnt_efetivado;
                    chkEfetivado.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                    chkEfetivado.Enabled = usuarioPermissao && VS_Periodo_Aberto && permissaoAlteracao && !VS_PeriodoEfetivado;
                }

                ImageButton btnDetalharHabilidades = (ImageButton)e.Item.FindControl("btnDetalharHabilidades");
                if (btnDetalharHabilidades != null)
                    btnDetalharHabilidades.Visible = permissaoAlteracao;
            }
        }

        protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                // Seta o campo de nota de acordo com o tipo de escala de avaliação.
                TextBox txtNota = (TextBox)e.Item.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)e.Item.FindControl("ddlPareceres");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                CheckBox chkParticipante = (CheckBox)e.Item.FindControl("chkParticipante");
                CheckBox chkDesconsiderar = (CheckBox)e.Item.FindControl("chkDesconsiderar");
                
                // Setar relatórios.
                RepeaterItem itemAtividade = e.Item;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;
                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                DateTime tnt_data = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "tnt_data"));

                MTR_MatriculaTurma mtu = new MTR_MatriculaTurma { alu_id = alu_id, mtu_id = mtu_id };
                MTR_MatriculaTurmaBO.GetEntity(mtu);

                bool exibeCampo = tnt_data >= mtu.mtu_dataMatricula && (mtu.mtu_dataSaida == new DateTime() || tnt_data <= mtu.mtu_dataSaida);

                // Habilita os controles de acordo com a posição do docente.
                // Pinta célula que possui aluno ausente.
                HtmlGenericControl divAtividades = (HtmlGenericControl)e.Item.FindControl("divAtividades");
                bool permissaoAlteracao = PermiteLancarNota && Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;
                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= VS_PeriodoEfetivado;

                string avaliacao = DataBinder.Eval(e.Item.DataItem, "avaliacao").ToString();

                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id").ToString());
                int tnt_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tnt_id").ToString());

                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                txtNota.Visible = tipo == EscalaAvaliacaoTipo.Numerica && exibeCampo;

                chkDesconsiderar.Visible = (tipo == EscalaAvaliacaoTipo.Numerica) && exibeCampo
                                            && (ltAtividadeIndicacaoNota.Any(p => p.tud_id == tud_id && p.tnt_id == tnt_id && p.PossuiNota))
                                            && VS_EntitiesControleTurma.formatoAvaliacao.fav_exibirBotaoSomaMedia;
                if (!chkDesconsiderar.Visible)
                    chkDesconsiderar.Checked = false;

                ddlPareceres.Visible = tipo == EscalaAvaliacaoTipo.Pareceres && exibeCampo;
                btnRelatorio.Visible = tipo == EscalaAvaliacaoTipo.Relatorios && exibeCampo;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    ddlPareceres.Items.Insert(0, new ListItem("-- Selecione um conceito --", "-1", true));
                    ddlPareceres.AppendDataBoundItems = true;
                    ddlPareceres.DataSource = LtPareceres;
                    ddlPareceres.DataBind();
                }

                if (divAtividades != null)
                {
                    bool ausente = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ausente").ToString());
                    if (ausente)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoAusente;
                    }

                    HabilitaControles(divAtividades.Controls, usuarioPermissao && permissaoAlteracao && VS_Periodo_Aberto && !VS_PeriodoEfetivado);
                }

                // Setar valores.
                Double tnaAvaliacao;
                txtNota.Text = Double.TryParse(avaliacao, out tnaAvaliacao) ? String.Format("{0:F" + NumeroCasasDecimais + "}", tnaAvaliacao) : avaliacao;

                ddlPareceres.SelectedValue = avaliacao;

                bool tnt_exclusiva = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tnt_exclusiva").ToString());

                // Setar visibilidade de controles para avaliações exclusivas.
                if (ParametroPermitirAtividadesExclusivas && tnt_exclusiva)
                {
                    chkParticipante.Visible = true;
                    chkParticipante.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tna_participante").ToString());

                    switch (tipo)
                    {
                        case EscalaAvaliacaoTipo.Numerica:
                            txtNota.Enabled = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;

                        case EscalaAvaliacaoTipo.Pareceres:
                            ddlPareceres.Enabled = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;

                        case EscalaAvaliacaoTipo.Relatorios:
                            btnRelatorio.Visible = chkParticipante.Checked && permissaoAlteracao && !VS_PeriodoEfetivado;
                            break;
                    }
                }
                else
                    chkParticipante.Visible = false;

                if (tipo == EscalaAvaliacaoTipo.Relatorios)
                {
                    string tna_relatorio = DataBinder.Eval(itemAtividade.DataItem, "relatorio").ToString();
                    AdicionaItemRelatorio(tnt_id, alu_id, mtu_id, tna_relatorio);

                    SetaImgRelatorio(itemAtividade);
                }

                txtNota.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                ddlPareceres.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);

                bool AlunoDispensado = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "AlunoDispensado") ?? false));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui aluno dispensado.
                    if (divAtividades != null)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                bool alunoAusente = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "ausente") ?? false));

                if (alunoAusente)
                {
                    // Pinta célula que possui aluno ausente.
                    if (divAtividades != null)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoAusente;
                    }
                }

                // Aluno Inativo
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                HtmlTableCell tdAtividadesAtivAva = (HtmlTableCell)e.Item.FindControl("tdAtividadesAtivAva");

                if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    // Pinta célula que possui aluno ausente.
                    if (tdAtividadesAtivAva != null)
                    {
                        tdAtividadesAtivAva.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                }
            }
        }

        protected void btnSalvarHabilidadesRelacionadas_Click(object sender, EventArgs e)
        {
            try
            {
                if (CLS_TurmaNotaOrientacaoCurricularBO.SalvarEmLote(UCHabilidades.RetornaListaHabilidades()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Atividade Orientação curricular | " +
                                                                            "tud_id: " + UCNavegacaoTelaPeriodo.VS_cal_id +
                                                                            " | tnt_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id);
                    lblMsgAvaliacoes.Text = UtilBO.GetErroMessage("Habilidades relacionadas a avaliação alteradas com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "SalvarHabilidadesRelacionadas"
                                                        , "$(document).ready(function() { $('#divHabilidadesRelacionadas').dialog('close'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar as habilidades relacionadas a avaliação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptAtividadesAvaliacao_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DetalharHabilidades")
            {
                int tnt_id = 0;
                long tud_id = 0;

                string[] commandArgs = e.CommandArgument.ToString().Split(';');
                if (commandArgs.Length == 2)
                {
                    tnt_id = Convert.ToInt32(commandArgs[0]);
                    tud_id = Convert.ToInt64(commandArgs[1]);
                }

                if (tnt_id > 0 && tud_id > 0)
                {
                    lblMsgAvaliacoes.Text = string.Empty;

                    UCHabilidades.CarregarHabilidades(
                        VS_EntitiesControleTurma.curriculoPeriodo.cur_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crr_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                        UCControleTurma1.VS_tur_id,
                        tud_id,
                        UCNavegacaoTelaPeriodo.VS_cal_id,
                        PosicaoDocente,
                        tnt_id,
                        UCNavegacaoTelaPeriodo.VS_tpc_id
                    );

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConsultaHabilidadesRelacionadas"
                                                            , "$(document).ready(function() { $('#divHabilidadesRelacionadas').dialog('open'); });", true);
                }
            }
        }

        protected void rptPlanoAula_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Header))
            {
                if (EntTurmaDisciplina.tud_tipo != Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
                {
                    Label lblComponenteCurricular = (Label)e.Item.FindControl("lblComponenteCurricular");
                    lblComponenteCurricular.Visible = false;

                    HtmlControl thComponenteCurricular = (HtmlControl)e.Item.FindControl("thComponenteCurricular");
                    thComponenteCurricular.Visible = false;
                }
                else
                {
                    Label lblComponenteCurricular = (Label)e.Item.FindControl("lblComponenteCurricular");
                    lblComponenteCurricular.Visible = true;

                    HtmlControl thComponenteCurricular = (HtmlControl)e.Item.FindControl("thComponenteCurricular");
                    thComponenteCurricular.Visible = true;
                }
            }
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Button btnTrocaPlano = (Button)e.Item.FindControl("btnTrocaPlano");
                Button btnTrocaResumo = (Button)e.Item.FindControl("btnTrocaResumo");
                Button btnTrocaPlanoResumo = (Button)e.Item.FindControl("btnTrocaPlanoResumo");
                TextBox txtPlanoAula = (TextBox)e.Item.FindControl("txtPlanoAula");
                TextBox txtSinteseAula = (TextBox)e.Item.FindControl("txtSinteseAula");
                Label lblData = (Label)e.Item.FindControl("lblData");
                byte tdt_posicao = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tdt_posicao"));

                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                bool permissaoAlteracao = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Gestao
                                            && __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa
                                            && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                if (permissaoAlteracao)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == new Guid(DataBinder.Eval(e.Item.DataItem, "usu_id").ToString())));
                }

                if (permissaoAlteracao)
                {
                    btnTrocaPlanoResumo.OnClientClick = "copiaValores('" + txtPlanoAula.ClientID + "','" + txtSinteseAula.ClientID + "'); return false;";
                    permiteEdicao = true;
                }
                else
                {
                    btnTrocaPlanoResumo.Enabled = txtPlanoAula.Enabled = txtSinteseAula.Enabled = false;
                    btnTrocaPlanoResumo.OnClientClick = "return false;";
                }

                // Apenas aulas dos dias anteriores sem plano de aula devem exibir o aviso.
                Image imgSemPlanoAula = (Image)e.Item.FindControl("imgSemPlanoAula");
                HiddenField hdfSemPlanoAula = (HiddenField)e.Item.FindControl("hdfSemPlanoAula");
                if (imgSemPlanoAula != null && hdfSemPlanoAula != null &&
                    Convert.ToDateTime(lblData.Text).Date < DateTime.Now.Date)
                {
                    imgSemPlanoAula.Visible = Convert.ToBoolean(hdfSemPlanoAula.Value)
                                                && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                                    || VS_EntitiesControleTurma.curso.tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                    || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                    imgSemPlanoAula.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.imgSemPlanoAula").ToString();
                }

                if (e.Item.ItemIndex > 0)
                {
                    TextBox txtPlanoAulaAnterior = (TextBox)rptPlanoAula.Items[e.Item.ItemIndex - 1].FindControl("txtPlanoAula");
                    TextBox txtSinteseAulaAnterior = (TextBox)rptPlanoAula.Items[e.Item.ItemIndex - 1].FindControl("txtSinteseAula");

                    if (permissaoAlteracao)
                    {
                        btnTrocaPlano.OnClientClick = "copiaValores('" + txtPlanoAulaAnterior.ClientID + "','" + txtPlanoAula.ClientID + "'); return false;";
                        btnTrocaResumo.OnClientClick = "copiaValores('" + txtSinteseAulaAnterior.ClientID + "','" + txtSinteseAula.ClientID + "'); return false;";
                    }
                    else
                    {
                        btnTrocaPlano.Enabled = btnTrocaResumo.Enabled = false;
                        btnTrocaPlano.OnClientClick = btnTrocaResumo.OnClientClick = "return false;";
                    }
                }
                else
                    btnTrocaPlano.Visible = btnTrocaResumo.Visible = false;

                CheckBoxList chlComponenteCurricular = (CheckBoxList)e.Item.FindControl("chlComponenteCurricular");
                HtmlControl tdComponenteCurricular = (HtmlControl)e.Item.FindControl("tdComponenteCurricular");
                HtmlControl tdComponenteCurricularLayout = (HtmlControl)e.Item.FindControl("tdComponenteCurricularLayout");

                if (EntTurmaDisciplina.tud_tipo != Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
                {
                    chlComponenteCurricular.Visible = false;
                    tdComponenteCurricular.Visible = false;
                    tdComponenteCurricularLayout.Visible = false;
                }
                else
                {
                    chlComponenteCurricular.Visible = true;
                    chlComponenteCurricular.Enabled = permissaoAlteracao;
                    tdComponenteCurricular.Visible = true;
                    tdComponenteCurricularLayout.Visible = true;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        var turmaDisciplina = (from dr in dtTurmaDisciplinaDoc
                                               where Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == UCControleTurma1.VS_tur_id
                                               && Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                               select dr);
                        chlComponenteCurricular.DataSource = turmaDisciplina;
                        chlComponenteCurricular.DataBind();

                        foreach (ListItem item in chlComponenteCurricular.Items)
                            item.Selected = dtTurmaAulaPlanoDisc.Any(p => p.tud_idPlano == Convert.ToInt64(item.Value.Split(';')[1]) &&
                                                                          p.tau_id == Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id")));
                    }
                    else
                    {
                        var turmaDisciplina = (from dr in dtTurmaDisciplinaTud
                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                               select dr);

                        // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                        if (EntTurmaDisciplina != null && EntTurmaDisciplina.tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
                        {
                            //Carrega componentes da regência.
                            var turmaDisciplinaComponenteRegencia = (from dr in dtTurmaDisciplinaTud
                                                                     where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                     select dr);
                            chlComponenteCurricular.DataSource = turmaDisciplinaComponenteRegencia;
                            chlComponenteCurricular.DataBind();

                            foreach (ListItem item in chlComponenteCurricular.Items)
                                item.Selected = dtTurmaAulaPlanoDisc.Any(p => p.tud_idPlano == Convert.ToInt64(item.Value.Split(';')[1]) &&
                                                                              p.tau_id == Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id")));
                        }
                    }
                }
            }
        }

        protected void ddlTurmaDisciplinaListao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarTela();
                ddlComponenteListao.Visible = lblComponenteListao.Visible = ddlComponenteListao.Items.Count > 0 && Convert.ToByte(ddlTurmaDisciplinaListao.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAdicionarAtiExtra_Click(object sender, EventArgs e)
        {
            try
            {
                int tae_id = string.IsNullOrEmpty(hdnTaeId.Value) ? -1 : Convert.ToInt32(hdnTaeId.Value);
                CLS_TurmaAtividadeExtraClasse entity = new CLS_TurmaAtividadeExtraClasse
                {
                    tud_id = VisibilidadeRegencia(ddlTurmaDisciplinaListao) ? ddlComponenteListao_Tud_Id_Selecionado : EntTurmaDisciplina.tud_id
                    ,
                    tae_id = tae_id
                    ,
                    tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id
                    ,
                    tav_id = UCComboTipoAtividadeAvaliativa.Valor
                    ,
                    tae_nome = txtNomeAtiExtra.Text
                    ,
                    tae_descricao = txtDescricaoAtiExtra.Text
                    ,
                    tae_cargaHoraria = Convert.ToInt32(string.IsNullOrEmpty(txtCargaAtiExtra.Text) ? "0" : txtCargaAtiExtra.Text)
                    ,
                    tdt_posicao = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao ? (byte)1 : PosicaoDocente
                    ,
                    taer_id = new Guid(hdnTaerId.Value)
                    ,
                    IsNew = tae_id <= 0
                };

                List<long> lstDisciplinas = new List<long>();
                if (rptDisciplinasAtiExtra.Visible)
                {
                    foreach (RepeaterItem disciplina in rptDisciplinasAtiExtra.Items)
                    {
                        CheckBox ckbDisciplinaAtiExtra = (CheckBox)disciplina.FindControl("ckbDisciplinaAtiExtra");
                        if (ckbDisciplinaAtiExtra != null && ckbDisciplinaAtiExtra.Checked)
                        {
                            HiddenField hdnIdDisciplinaAtiExtra = (HiddenField)disciplina.FindControl("hdnIdDisciplinaAtiExtra");
                            if (hdnIdDisciplinaAtiExtra != null)
                            {
                                lstDisciplinas.Add(Convert.ToInt64(hdnIdDisciplinaAtiExtra.Value));
                            }
                        }
                    }
                }

                if (CLS_TurmaAtividadeExtraClasseBO.Salvar(entity, VS_EntitiesControleTurma.calendarioAnual.cal_id, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico, Ent_ID_UsuarioLogado, lstDisciplinas, __SessionWEB.__UsuarioWEB.Usuario.usu_id, UCControleTurma1.VS_tur_id))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, string.Format("Listão de atividade extraclasse | Adição de atividade | tud_id: {0}, tae_id: {1}", entity.tud_id, entity.tae_id));
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Atividade extraclasse salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    fdsCadastroAtiExtra.Visible = false;
                    fdsListagemAtiExtra.Visible = true;
                    CarregarListaoAtividadeExtraclasse();
                    hdnTaeId.Value = string.Empty;
                    hdnTaerId.Value = string.Empty;
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a atividade extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void rptAlunoAtivExtra_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RelatorioRP")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    AbrirRelatorioRP(Convert.ToInt64(args[0]), args[1]);
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
                    AbrirRelatorioAEE(Convert.ToInt64(e.CommandArgument.ToString()));
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir os relatórios do AEE para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rptAlunoAtivExtra_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                // Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
                SetaNomeAluno(e);

                Alu_idExtraClasse = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                //Parametros dispensa disciplina - pinta linha
                Mtu_idExtraClasse = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                UCNavegacaoTelaPeriodo.VS_cal_id = VS_EntitiesControleTurma.turma.cal_id;

                // Aluno inativo - pinta a linha
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));

                Repeater rptAtividades = (Repeater)e.Item.FindControl("rptAtividades");
                DataTable dtAtividades = new DataTable();
                List<DataRow> ltAtividades;
                if (e.Item.ItemType == ListItemType.Header)
                {
                    // Busca todas as atividades para o cabeçalho.
                    ltAtividades = (from DataRow dr in DTAtividadeExtraclasse.Rows
                                    group dr by dr["tae_id"] into g
                                    orderby Convert.ToInt32(g.FirstOrDefault()["tae_id"])
                                            , Convert.ToInt64(g.FirstOrDefault()["tud_id"])
                                    select g.FirstOrDefault()).ToList();

                    if (ltAtividades.Count > 0)
                        dtAtividades = ltAtividades.CopyToDataTable();
                }
                else
                {
                    int mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));
                    int mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));

                    // Busca as notas das atividades para o aluno.
                    ltAtividades = (from DataRow dr in DTAtividadeExtraclasse.Rows
                                    where
                                        Convert.ToInt64(dr["alu_id"]) == Alu_idExtraClasse
                                        && Convert.ToInt32(dr["mtu_id"]) == mtu_id
                                        && Convert.ToInt32(dr["mtd_id"]) == mtd_id
                                    group dr by dr["tae_id"] into g
                                    orderby Convert.ToInt32(g.FirstOrDefault()["tae_id"])
                                            , Convert.ToInt64(g.FirstOrDefault()["tud_id"])
                                    select g.FirstOrDefault()).ToList();

                    if (ltAtividades.Count > 0)
                        dtAtividades = ltAtividades.CopyToDataTable();
                }

                if (rptAtividades != null)
                {
                    rptAtividades.DataSource = dtAtividades;
                    rptAtividades.DataBind();
                }

                if ((e.Item.ItemType == ListItemType.Item) ||
                    (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        HtmlControl tdNumChamadaAvaliacao = (HtmlControl)e.Item.FindControl("tdNumChamadaAvaliacao");
                        HtmlControl tdNomeAvaliacao = (HtmlControl)e.Item.FindControl("tdNomeAvaliacao");

                        tdNumChamadaAvaliacao.Style["background-color"] = tdNomeAvaliacao.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }

                    LinkButton btnRelatorioAEE = (LinkButton)e.Item.FindControl("btnRelatorioAEE");
                    if (btnRelatorioAEE != null)
                    {
                        btnRelatorioAEE.Visible = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "alu_situacaoID")) == (byte)ACA_AlunoSituacao.Ativo
                                                    && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PossuiDeficiencia"));
                        btnRelatorioAEE.CommandArgument = Alu_id.ToString();
                    }

                    // Mostra o ícone para as anotações de recuperação paralela (RP):
                    // - para todos os alunos, quando a turma for de recuperação paralela,
                    // - ou apenas para alunos com anotações de RP, quando for a turma regular relacionada com a recuperação paralela.
                    if (UCControleTurma1.VS_tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno
                        || lstAlunosRelatorioRP.Any(p => p.alu_id == Alu_idExtraClasse))
                    {
                        LinkButton btnRelatorioRP = (LinkButton)e.Item.FindControl("btnRelatorioRP");
                        if (btnRelatorioRP != null)
                        {
                            btnRelatorioRP.Visible = true;
                            btnRelatorioRP.CommandArgument = Alu_idExtraClasse.ToString();

                            if (VisibilidadeRegencia(ddlTurmaDisciplinaListao))
                            {
                                string strTds = string.Empty;
                                (from Struct_PreenchimentoAluno preenchimento in lstAlunosRelatorioRP.FindAll(p => p.alu_id == Alu_idExtraClasse)
                                 group preenchimento by new { tds_id = preenchimento.tds_idRelacionada } into grupo
                                 select grupo.Key.tds_id).ToList().ForEach(p => strTds += string.Format(",{0}", p.ToString()));
                                if (strTds.Length > 1)
                                {
                                    btnRelatorioRP.CommandArgument += string.Format(";{0}", strTds.Substring(1));
                                }
                                else
                                {
                                    btnRelatorioRP.CommandArgument += string.Format(";{0}", "-1");
                                }
                            }
                            else
                            {
                                btnRelatorioRP.CommandArgument += string.Format(";{0}", "-1");
                            }
                        }
                    }
                }
            }
        }

        protected void rptAtividadesExtraClasse_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Seta o campo de nota de acordo com o tipo de escala de avaliação.
                TextBox txtNota = (TextBox)e.Item.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)e.Item.FindControl("ddlPareceres");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                CheckBox chkEntregou = (CheckBox)e.Item.FindControl("chkEntregou");

                // Setar relatórios.
                RepeaterItem itemAtividade = e.Item;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;
                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                HtmlGenericControl divAtividades = (HtmlGenericControl)e.Item.FindControl("divAtividades");
                string avaliacao = DataBinder.Eval(e.Item.DataItem, "avaliacao").ToString();

                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id").ToString());
                int tae_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tae_id").ToString());

                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                txtNota.Visible = tipo == EscalaAvaliacaoTipo.Numerica;
                ddlPareceres.Visible = tipo == EscalaAvaliacaoTipo.Pareceres;
                btnRelatorio.Visible = tipo == EscalaAvaliacaoTipo.Relatorios;

                if (tipo == EscalaAvaliacaoTipo.Pareceres)
                {
                    // Carregar combo de pareceres.
                    ddlPareceres.Items.Insert(0, new ListItem("-- Selecione um conceito --", "-1", true));
                    ddlPareceres.AppendDataBoundItems = true;
                    ddlPareceres.DataSource = LtPareceres;
                    ddlPareceres.DataBind();

                    ddlPareceres.SelectedValue = avaliacao;
                }

                bool permissaoAlteracao = PermiteLancarAtividadeExtraclasse && Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "permissaoEdicao")) > 0;
                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }
                
                HabilitaControles(divAtividades.Controls, usuarioPermissao && VS_Periodo_Aberto && !VS_PeriodoEfetivado && permissaoAlteracao);

                double eaeAvaliacao;
                txtNota.Text = double.TryParse(avaliacao, out eaeAvaliacao) ? string.Format("{0:F" + NumeroCasasDecimais + "}", eaeAvaliacao) : avaliacao;

                if (tipo == EscalaAvaliacaoTipo.Relatorios)
                {
                    string aea_relatorio = DataBinder.Eval(itemAtividade.DataItem, "relatorio").ToString();
                    AdicionaItemRelatorioAtiExtra(tae_id, alu_id, mtu_id, aea_relatorio);

                    SetaImgRelatorioAtiExtra(itemAtividade);
                }

                chkEntregou.Checked = Convert.ToBoolean(DataBinder.Eval(itemAtividade.DataItem, "entregou").ToString());

                txtNota.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                ddlPareceres.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);

                bool AlunoDispensado = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "AlunoDispensado") ?? false));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui aluno dispensado.
                    if (divAtividades != null)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                // Aluno Inativo
                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                HtmlTableCell tdAtividadesAtivAva = (HtmlTableCell)e.Item.FindControl("tdAtividadesAtivAva");

                if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    // Pinta célula que possui aluno ausente.
                    if (tdAtividadesAtivAva != null)
                    {
                        tdAtividadesAtivAva.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                }
            }
        }

        protected void btnEditarAtiExtra_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnEditarAtiExtra = sender as ImageButton;
                if (btnEditarAtiExtra != null)
                {
                    RepeaterItem itemAtividade = btnEditarAtiExtra.NamingContainer as RepeaterItem;
                    Label lbltud_id = itemAtividade.FindControl("lbltud_id") as Label;
                    Label lbltae_id = itemAtividade.FindControl("lbltae_id") as Label;

                    Label lblTaePosicao = itemAtividade.FindControl("lblTaePosicao") as Label;
                    Label lblPermissao = itemAtividade.FindControl("lblPermissao") as Label;

                    if (lbltud_id != null && lbltae_id != null)
                    {
                        long tud_id = 0;
                        int tae_id = 0;

                        if (long.TryParse(lbltud_id.Text, out tud_id) && tud_id > 0 &&
                            int.TryParse(lbltae_id.Text, out tae_id) && tae_id > 0)
                        {
                            if (lblTaePosicao != null && lblPermissao != null)
                            {
                                int permissao = 0;
                                int.TryParse(lblPermissao.Text, out permissao);
                                CarregarCadastroAtividadeExtraclasse(tud_id, tae_id, permissao > 0 && PermiteLancarAtividadeExtraclasse);
                            }
                            else
                            {
                                CarregarCadastroAtividadeExtraclasse(tud_id, tae_id, (PermiteLancarAtividadeExtraclasse || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao) && VS_Periodo_Aberto);
                            }
                            updAtiExtra.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar editar a atividade extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void btnExcluirAtiExtra_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnExcluirAtiExtra = sender as ImageButton;
                if (btnExcluirAtiExtra != null)
                {
                    RepeaterItem itemAtividade = btnExcluirAtiExtra.NamingContainer as RepeaterItem;
                    Label lbltud_id = itemAtividade.FindControl("lbltud_id") as Label;
                    Label lbltae_id = itemAtividade.FindControl("lbltae_id") as Label;

                    if (lbltud_id != null && lbltae_id != null)
                    {
                        long tud_id = 0;
                        int tae_id = 0;

                        if (long.TryParse(lbltud_id.Text, out tud_id) && tud_id > 0 &&
                            int.TryParse(lbltae_id.Text, out tae_id) && tae_id > 0)
                        {
                            Label lbltaer_id = itemAtividade.FindControl("lbltaer_id") as Label;
                            Guid taer_id = Guid.Empty;
                            if (!string.IsNullOrEmpty(lbltaer_id.Text))
                            {
                                taer_id = new Guid(lbltaer_id.Text);
                            }

                            VS_tud_idAtiExtraExcluir = tud_id;
                            VS_tae_idAtiExtraExcluir = tae_id;
                            VS_taer_idAtiExtraExcluir = taer_id;
                            UCConfirmacaoOperacao.Mensagem = "Confirma exclusão?";
                            UCConfirmacaoOperacao.EventBtnNao = false;
                            UCConfirmacaoOperacao.Update();
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaExclusaoAtiExtra", "$(document).ready(function(){ scrollToTop(); $('#divConfirmacao').dialog('open'); });", true);
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a atividade extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void btnCancelarAtiExtra_Click(object sender, EventArgs e)
        {
            fdsCadastroAtiExtra.Visible = false;
            fdsListagemAtiExtra.Visible = true;
        }

        protected void rptAtividadesExtraClasseHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                bool permissaoAlteracao = PermiteLancarAtividadeExtraclasse && Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "permissaoEdicao")) > 0;
                if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1));
                }
                if (permissaoAlteracao)
                {
                    permiteEdicao = true;
                }

                // Verifico se a atividade é de uma avaliação paralela.
                ImageButton btnExcluirAtiExtra = (ImageButton)e.Item.FindControl("btnExcluirAtiExtra");
                if (btnExcluirAtiExtra != null)
                {
                    btnExcluirAtiExtra.Visible = usuarioPermissao && permissaoAlteracao && VS_Periodo_Aberto && !VS_PeriodoEfetivado;
                }

                ImageButton btnEditarAtiExtra = (ImageButton)e.Item.FindControl("btnEditarAtiExtra");
                if (btnEditarAtiExtra != null)
                {
                    btnEditarAtiExtra.Visible = usuarioPermissao && VS_Periodo_Aberto && !VS_PeriodoEfetivado;
                }
            }
        }

        protected void btnNovoAtiExtra_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarCadastroAtividadeExtraclasse(-1, -1, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar atividade extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion Eventos
    }
}