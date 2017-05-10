using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;

namespace GestaoEscolar.Academico.ControleTurma
{
    public partial class DiarioClasse : MotherPageLogadoCompressedViewState
    {
        #region Enumerador

        /// <summary>
        /// Opções de operações possíveis na tela.
        /// </summary>
        protected enum OperacaoAtual : byte
        {
            Nenhuma,
            InserindoAula,
            InserindoAtividade,
            AlterandoAula,
            AlterandoAtividade,
            GerarAulas,
            InserindoAtividadeSemNota,
            AlterandoAtividadeSemNota
        }

        #endregion Enumerador

        #region Constantes

        private const int grvAulas_ColunaDataAula = 0;
        private const int grvAulas_ColunaFrequencia = 2;
        private const int grvAulas_ColunaAtAvaliada = 3;
        private const int grvAulas_ColunaAnotacoes = 4;
        private const int grvAulas_ColunaPlanoAula = 5;
        private const int grvAulas_ColunaExcluirAula = 6;
        private const int grvAnotacaoAluno_ColunaTipoAnotacao = 1;
        private const int grvAnotacoesMaisdeUmAluno_ColunaTipoAnotacao = 1;

        #endregion Constantes

        #region Structs

        /// <summary>
        /// Estrutura que armazena a estrutra de orientações curriculares de cada período do calendário.
        /// </summary>
        [Serializable]
        private struct OrientacaoCurricular
        {
            public int tpc_id;
            public DataTable dtOrientacaoCurricular;
        }

        /// <summary>
        /// Estrutura que armazena a estrutra de orientações curriculares dos períodos do calendário anteriores
        /// ao que esta sendo trabalhado e apenas se foram planejados ou trabalhados
        /// </summary>
        [Serializable]
        private struct OrientacaoCurricularPeriodosAnteriores
        {
            public string id;
            public bool planejado;
            public bool trabalhado;
        }

        /// <summary>
        /// Classe utilizada para dar DataBind no repeater de aulas.
        /// </summary>
        protected class AulasAlunos
        {
            public int tau_id { get; set; }

            public DateTime tau_data { get; set; }

            public bool tau_efetivado { get; set; }

            public byte tdt_posicao { get; set; }

            public int tau_numeroAulas { get; set; }

            public int taa_frequencia { get; set; }

            public string taa_frequenciaBitMap { get; set; }

            public string falta_justificada { get; set; }

            public bool AlunoDispensado { get; set; }

            public bool permissaoAlteracao { get; set; } //

            public int mtd_situacao { get; set; }
        }

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

        /// <summary>
        /// Estrutura que indica se uma atividade possui alunos com nota lançada.
        /// </summary>
        private struct AtividadeIndicacaoNota
        {
            public long tud_id { get; set; }

            public int tnt_id { get; set; }

            public bool PossuiNota { get; set; }
        }

        /// <summary>
        /// Estrutura usada para a legenda de tipos de docentes.
        /// </summary>
        private struct LegendaDiario
        {
            public byte tdc_id { get; set; }

            public string tipoDocente { get; set; }

            public string tdc_corDestaque { get; set; }
        };

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
        /// Lista de permissões do docente para cadastro de aula.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoAula
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoAula"] ??
                            (
                                ViewState["VS_ltPermissaoAula"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(UCControleTurma1.VS_tdt_posicao, (byte)EnumModuloPermissao.Aula)
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
        /// Armazena a linha que está sendo modificada.
        /// </summary>
        private int VS_grvRow
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_grvRow"] ?? -1);
            }

            set
            {
                ViewState["VS_grvRow"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma disciplina da aula.
        /// </summary>
        private Int64 VS_tud_id_Aula
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id_Aula"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_id_Aula"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma disciplina que irá excluir.
        /// </summary>
        private Int64 VS_tud_id_Excluir
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id_Excluir"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_id_Excluir"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da atividade que irá excluir.
        /// </summary>
        private Int32 VS_tnt_id_Excluir
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tnt_id_Excluir"] ?? -1);
            }

            set
            {
                ViewState["VS_tnt_id_Excluir"] = value;
            }
        }

        /// <summary>
        /// Armazena o tipo turma disciplina da aula.
        /// </summary>
        private byte VS_tud_tipo_Aula
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tud_tipo_Aula"] ?? 0);
            }

            set
            {
                ViewState["VS_tud_tipo_Aula"] = value;
            }
        }

        /// <summary>
        /// Armazena o tipo de apuração de frequência do formato de avaliação da turma disciplina da aula.
        /// </summary>
        private byte VS_fav_tipoApuracaoFrequencia
        {
            get
            {
                return Convert.ToByte(ViewState["VS_fav_tipoApuracaoFrequencia"] ?? 0);
            }

            set
            {
                ViewState["VS_fav_tipoApuracaoFrequencia"] = value;
            }
        }

        /// <summary>
        /// Armazena o nome da turma disciplina da aula.
        /// </summary>
        private string VS_tud_nome_Aula
        {
            get
            {
                return Convert.ToString(ViewState["VS_tud_nome_Aula"] ?? "");
            }

            set
            {
                ViewState["VS_tud_nome_Aula"] = value;
            }
        }

        /// <summary>
        /// Armazena se a turma disciplina da aula é global.
        /// </summary>
        private bool VS_tud_global_Aula
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_tud_global_Aula"] ?? "False");
            }

            set
            {
                ViewState["VS_tud_global_Aula"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da aula em edição em viewstate.
        /// </summary>
        private int VS_tau_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tau_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tau_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o data da aula.
        /// </summary>
        private DateTime VS_tau_data
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_tau_data"] ?? new DateTime());
            }

            set
            {
                ViewState["VS_tau_data"] = value;
            }
        }

        /// <summary>
        /// Armazena a posição do docente da aula em edição em viewstate.
        /// </summary>
        private byte VS_tdt_posicaoEdicao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tdt_posicaoEdicao"] ?? 0);
            }

            set
            {
                ViewState["VS_tdt_posicaoEdicao"] = value;
            }
        }

        /// <summary>
        /// Guarda se o usuario tera permissao de alteracao, essa valor vira de uma SP
        /// </summary>
        private bool VS_permissaoAlteracao
        {
            get
            {
                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                {
                    return false;
                }

                if (ViewState["VS_permissaoAlteracao"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_permissaoAlteracao"]);
                }

                return false;
            }
            set
            {
                ViewState["VS_permissaoAlteracao"] = value;
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
        /// Armazena o ID da avaliação
        /// </summary>
        private int VS_tnt_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tnt_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tnt_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o nome da disciplina.
        /// </summary>
        private string VS_tud_nome
        {
            get
            {
                if (ViewState["VS_tud_nome"] == null)
                {
                    if (VS_EntitiesControleTurma.turmaDisciplina.tud_disciplinaEspecial)
                    {
                        ViewState["VS_tud_nome"] = TUR_TurmaDisciplinaBO.SelecionaNomePorTipoDocente
                            (UCControleTurma1.VS_tud_id, VS_tipoDocente);
                    }
                    else
                    {
                        ViewState["VS_tud_nome"] = VS_EntitiesControleTurma.turmaDisciplina.tud_nome;
                    }
                }

                return ViewState["VS_tud_nome"].ToString();
            }
        }

        /// <summary>
        /// Data de alteração utilizada para validação de duplicidade de aula.
        /// </summary>
        private DateTime VS_DataAlteracaoAula_Validacao
        {
            get
            {
                if (ViewState["VS_DataAlteracaoAula_Validacao"] != null)
                    return Convert.ToDateTime(ViewState["VS_DataAlteracaoAula_Validacao"]);

                return new DateTime();
            }

            set
            {
                ViewState["VS_DataAlteracaoAula_Validacao"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena as orientãções curriculares para cada período.
        /// </summary>
        private List<OrientacaoCurricular> VS_OrientacaoCurricular
        {
            get
            {
                return (List<OrientacaoCurricular>)(ViewState["VS_OrientacaoCurricular"] ??
                    ((ViewState["VS_OrientacaoCurricular"] = new List<OrientacaoCurricular>())));
            }

            set
            {
                ViewState["VS_OrientacaoCurricular"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena as orientãções curriculares para os periodos anterios ao que esta sendo tratado na tela
        /// </summary>
        private List<OrientacaoCurricularPeriodosAnteriores> VS_OrientacaoCurricular_PeriodosAnteriores
        {
            get
            {
                return (List<OrientacaoCurricularPeriodosAnteriores>)(ViewState["VS_OrientacaoCurricular_PeriodosAnteriores"] ??
                    ((ViewState["VS_OrientacaoCurricular_PeriodosAnteriores"] = new List<OrientacaoCurricularPeriodosAnteriores>())));
            }

            set
            {
                ViewState["VS_OrientacaoCurricular_PeriodosAnteriores"] = value;
            }
        }

        /// <summary>
        /// Data de alteração utilizada para validação de duplicidade de aula.
        /// </summary>
        private DateTime VS_dataAlteracaoAula
        {
            get
            {
                if (Operacao == OperacaoAtual.InserindoAula)
                    return new DateTime();

                if (ViewState["VS_dataAlteracaoAula"] != null)
                    return Convert.ToDateTime(ViewState["VS_dataAlteracaoAula"]);

                return new DateTime();
            }

            set
            {
                ViewState["VS_dataAlteracaoAula"] = value;
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
                            ViewState["VS_ListaEventos"] = ACA_EventoBO.GetEntity_Efetivacao_List(UCNavegacaoTelaPeriodo.VS_cal_id, UCControleTurma1.VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false)
                        )
                    );
            }
        }

        /// <summary>
        /// Guarda as notas de relatório.
        /// </summary>
        private List<NotasRelatorio> VS_Nota_Relatorio
        {
            get
            {
                if (ViewState["VS_Nota_Relatorio"] != null)
                    return (List<NotasRelatorio>)(ViewState["VS_Nota_Relatorio"]);
                return new List<NotasRelatorio>();
            }
            set
            {
                ViewState["VS_Nota_Relatorio"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena se a turma possui a disciplina do tipo Regência.
        /// </summary>
        private bool VS_PossuiRegencia
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_PossuiRegencia"] ?? false);
            }

            set
            {
                ViewState["VS_PossuiRegencia"] = value;
            }
        }

        /// <summary>
        /// Guarda o valor de crp_controleTempo.
        /// </summary>
        private byte VS_crp_controleTempo
        {
            get
            {
                if (ViewState["VS_crp_controleTempo"] == null)
                {
                    return 0;
                }

                return Convert.ToByte(ViewState["VS_crp_controleTempo"]);
            }
            set
            {
                ViewState["VS_crp_controleTempo"] = value;
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
        /// Retorna a operação atual armazenada no HiddenField.
        /// </summary>
        private OperacaoAtual Operacao
        {
            get
            {
                return (OperacaoAtual)Enum.Parse(typeof(OperacaoAtual), hdnOperacao.Value);
            }
            set
            {
                hdnOperacao.Value = value.ToString();
            }
        }

        /// <summary>
        /// Tabela carregada com as aulas e alunos para a disciplina e período.
        /// </summary>
        private List<sTurmaAulaAluno> VS_Aulas_Alunos;

        /// <summary>
        /// Armazena o data da alteração do registro para ser exibida no rodapé.
        /// </summary>
        private DateTime VS_DataAlteracaoRegistro
        {
            get
            {
                if (ViewState["VS_DataAlteracaoRegistro"] != null)
                    return Convert.ToDateTime(ViewState["VS_DataAlteracaoRegistro"]);

                return new DateTime();
            }

            set
            {
                ViewState["VS_DataAlteracaoRegistro"] = value;
            }
        }

        /// <summary>
        /// Armazena o nome do usuário que alterou o registro para ser exibida no rodapé.
        /// </summary>
        private String VS_nome_usu_alteracao
        {
            get
            {
                if (ViewState["VS_nome_usu_alteracao"] == null)
                {
                    return string.Empty;
                }

                return ViewState["VS_nome_usu_alteracao"].ToString();
            }

            set
            {
                ViewState["VS_nome_usu_alteracao"] = value;
            }
        }

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
        /// Carrega a entidade CLS_TurmaAula, para não precisar acessar o banco diversas vezes
        /// </summary>
        private CLS_TurmaAula aula;

        /// <summary>
        /// Carrega a entidade CLS_TurmaAula, para não precisar acessar o banco diversas vezes
        /// </summary>
        private CLS_TurmaAula entityAula
        {
            get
            {
                return aula ??
                    (
                        aula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula { tud_id = VS_tud_id_Aula, tau_id = VS_tau_id })
                    );
            }
        }

        /// <summary>
        /// Carrega as anotações dos alunos, para não precisar acessar o banco diversas vezes
        /// </summary>
        private List<AlunosTurmaDisciplina> dtAlunosAnotacoes;

        /// <summary>
        /// Carrega os tipos de anotações dos alunos, para não precisar acessar o banco diversas vezes
        /// </summary>
        private List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaTipoAnotacao;

        /// <summary>
        /// Valor do parâmetro acadêmico PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS
        /// </summary>
        private bool? parametroPermitirAtividadesExclusivas;

        /// <summary>
        /// Valor do parâmetro acadêmico PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS
        /// </summary>
        private bool ParametroPermitirAtividadesExclusivas
        {
            get
            {
                return (bool)
                       (parametroPermitirAtividadesExclusivas ??
                       (parametroPermitirAtividadesExclusivas =
                       ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id)));
            }
        }

        /// <summary>
        /// Tabela com atividades da disciplina e período, junto com as notas dos alunos nas atividades.
        /// </summary>
        private DataTable DTAtividades;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
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
        /// Calcula a quantidade de casas decimais da variação de notas
        /// </summary>
        private int numeroCasasDecimais
        {
            get
            {
                string variacao = Convert.ToDouble(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao).ToString();

                return variacao.IndexOf(",") >= 0 ?
                    variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1 :
                    1;
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
        /// Armazena disciplinas do docente em ViewState.
        /// </summary>
        private List<sComboTurmaDisciplina> VS_TurmaDisciplinaDocente
        {
            get
            {
                if (ViewState["VS_TurmaDisciplinaDocente"] == null)
                    ViewState["VS_TurmaDisciplinaDocente"] = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(0, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio)
                            : TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(UCControleTurma1.VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                return (List<sComboTurmaDisciplina>)(ViewState["VS_TurmaDisciplinaDocente"]);
            }
            set
            {
                ViewState["VS_TurmaDisciplinaDocente"] = value;
            }
        }

        /// <summary>
        /// Retorna o tud_id selecionado no combo ddlComponenteAtAvaliativa.
        /// Valores do combo:
        /// [0] - Tur_id
        /// [1] - Tud_id
        /// [2] - Permissão
        /// [3] - Tud_tipo
        /// </summary>
        private long ddlComponenteAtAvaliativa_Tud_Id_Selecionado
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlComponenteAtAvaliativa.SelectedValue))
                {
                    string[] valores = ddlComponenteAtAvaliativa.SelectedValue.Split(';');

                    if (valores.Length > 1)
                    {
                        return Convert.ToInt64(valores[1]);
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Retorna se o período selecionado no combo está válido para lançar notas e frequências:
        /// se o período for o período corrente, ou se houver um evento de efetivação do período que esteja
        /// vigente.
        /// </summary>
        private bool PeriodoValidoLancamentoNotasFrequencias
        {
            get
            {
                List<sComboPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(VS_EntitiesControleTurma.turma.cal_id, VS_tud_id_Aula, UCControleTurma1.VS_tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

                var tpcs_validos =
                    (from dr in dt
                     select new
                     {
                         tpc_id = dr.tpc_id
                     }).ToList();

                // Se existe o período na lista de períodos válidos, permite lançar nota e frequência.
                return (tpcs_validos.Exists(p => p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id));
            }
        }

        /// <summary>
        /// Propriedade indica se o período selecionada indica um período futuro.
        /// </summary>
        private bool periodoFuturo
        {
            get
            {
                return DateTime.Now < UCNavegacaoTelaPeriodo.cap_dataInicio;
            }
        }

        /// <summary>
        /// Retorna um booleano informando se o tipo da disciplina selecionada é Principal.
        /// </summary>
        public bool DisciplinaPrincipal
        {
            get
            {
                if (VS_tud_global_Aula)
                {
                    List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(UCControleTurma1.VS_tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                    return listaDisciplinas.Exists(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal);
                }

                return (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal);
            }
        }

        /// <summary>
        /// Retorna um booleano informando se o tipo da disciplina selecionada é Regencia.
        /// </summary>
        public bool DisciplinaRegencia
        {
            get
            {
                return (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Regencia && VS_tipoDocente != EnumTipoDocente.Projeto);
            }
        }

        /// <summary>
        /// Retorna um booleano informando se é regência E o tipo de apuração de frequencia do formato de avaliação é por tempos de aula
        /// </summary>
        public bool RegenciaETemposAula
        {
            get
            {
                return (VS_fav_tipoApuracaoFrequencia == (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula && VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Regencia);
            }
        }

        /// <summary>
        /// Retorna um booleano informando se o tipo da disciplina selecionada é experiência.
        /// </summary>
        public bool DisciplinaExperiencia
        {
            get
            {
                return (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia);
            }
        }

        /// <summary>
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        protected long Tud_idComponente
        {
            get
            {
                string[] ids = _ddlTurmaDisciplinaComponente.SelectedValue.Split(';');
                return Convert.ToInt64(ids.Length > 1 ? ids[1] : "-1");
            }
        }

        /// <summary>
        /// Hierarquia das orientações curriculares.
        /// </summary>
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

        /// <summary>
        /// Último nível da orientação curricular
        /// </summary>
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

        /// <summary>
        /// Último período do calendário.
        /// </summary>
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

        /// <summary>
        /// Nível da orientação curricular anterior usado no databound desses.
        /// </summary>
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

        /// <summary>
        /// Carrega os níveis de aprendizado da orientação para não buscar no banco várias vezes
        /// </summary>
        private List<sOrientacaoNivelAprendizado> dtOrientacaoNiveisAprendizado;

        /// <summary>
        /// Carrega os níveis de aprendizado para não buscar no banco várias vezes
        /// </summary>
        private List<sNivelAprendizado> dtNivelArendizadoCurriculo;

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
        /// Usada no dataBind do repeater de aulas, para saber a data de matrícula do aluno.
        /// </summary>
        private DateTime mtd_dataMatriculaAluno;

        /// <summary>
        /// Usada no dataBind do repeater de aulas, para saber a data de saída do aluno.
        /// Se for nulo, guarda DateTime.MaxValue.
        /// </summary>
        private DateTime mtd_dataSaidaAluno;

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
        /// ViewState que armazena o usuário que criou a aula.
        /// </summary>
        private Guid VS_usu_id
        {
            get
            {
                if (ViewState["VS_usu_id"] != null)
                    return new Guid(ViewState["VS_usu_id"].ToString());
                return Guid.Empty;
            }
            set
            {
                ViewState["VS_usu_id"] = value;
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
        /// Retorna o dropdown de componentes da regencia que deve ser utilizado.
        /// As turmas a partir do ano de 2015 possuem um unico plano de aula 
        /// para os componentes da regencia, o planejamento possui um dropdown exclusivo. 
        /// </summary>
        private DropDownList _ddlTurmaDisciplinaComponente
        {
            get
            {
                if (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    return ddlTurmaDisciplinaComponentePlanejamento;
                }
                else
                {
                    return ddlTurmaDisciplinaComponente;
                }
            }
        }

        /// <summary>
        /// Retorna o label de componentes da regencia que deve ser utilizado.
        /// As turmas a partir do ano de 2015 possuem um unico plano de aula 
        /// para os componentes da regencia, o planejamento possui um label exclusivo. 
        /// </summary>
        private Label _lblTurmaDisciplinaComponente
        {
            get
            {
                if (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    return lblTurmaDisciplinaComponentePlanejamento;
                }
                else
                {
                    return lblTurmaDisciplinaComponente;
                }
            }
        }

        /// <summary>
        /// Carregar as atividades e se os alunos possuem notas nas atividades.
        /// </summary>
        private List<AtividadeIndicacaoNota> ltAtividadeIndicacaoNota;

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

        /// <summary>
        /// ViewState que armazena a data e hora que carregou a TurmaAula
        /// </summary>
        private DateTime VS_Data_Diario_TurmaAula
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Diario_TurmaAula"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Diario_TurmaAula"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a data e hora que carregou a TurmaNota
        /// </summary>
        private DateTime VS_Data_Diario_TurmaNota
        {
            get
            {
                return Convert.ToDateTime(ViewState["VS_Data_Diario_TurmaNota"] ?? DateTime.Now);
            }

            set
            {
                ViewState["VS_Data_Diario_TurmaNota"] = value;
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

        /// <summary>
        /// Flag que indica se a permissão de excluir aulas é do grupo Diretor.
        /// </summary>
        private bool VS_PermissaoExcluirDiretor
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_PermissaoExcluirDiretor"] ?? false);
            }

            set
            {
                ViewState["VS_PermissaoExcluirDiretor"] = value;
            }
        }

        private CFG_PermissaoModuloOperacao PermissaoModuloLancamentoFrequencia
        {
            get
            {
                if (permissaoModuloLancamentoFrequencia == null)
                {
                    permissaoModuloLancamentoFrequencia = new CFG_PermissaoModuloOperacao()
                    {
                        gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        sis_id = ApplicationWEB.SistemaID,
                        mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                        pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequencia)
                    };
                    CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloLancamentoFrequencia);
                }
                return permissaoModuloLancamentoFrequencia;
            }
        }

        private CFG_PermissaoModuloOperacao PermissaoModuloLancamentoFrequenciaInfantil
        {
            get
            {
                if (permissaoModuloLancamentoFrequenciaInfantil == null)
                {
                    if (VS_EntitiesControleTurma.curso.tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        permissaoModuloLancamentoFrequenciaInfantil = new CFG_PermissaoModuloOperacao()
                        {
                            gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            sis_id = ApplicationWEB.SistemaID,
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequenciaInfantil)
                        };
                        CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloLancamentoFrequenciaInfantil);
                    }
                    else
                    {
                        permissaoModuloLancamentoFrequenciaInfantil = new CFG_PermissaoModuloOperacao();
                    }
                }
                return permissaoModuloLancamentoFrequenciaInfantil;
            }
        }

        private CFG_PermissaoModuloOperacao permissaoModuloLancamentoFrequencia;
        private CFG_PermissaoModuloOperacao permissaoModuloLancamentoFrequenciaInfantil;

        #endregion Propriedades

        #region Métodos

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
            btnIncluirAula.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                        && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoInclusao);
            // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
            {
                btnIncluirAula.Visible = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoInclusao;
            }
            else
            {
                //bloquear botoes -> se o docente nao possuir mais essa turma e se nao for turma extinta
                if (VS_situacaoTurmaDisciplina != 1
                    && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Encerrada
                    && VS_EntitiesControleTurma.turma.tur_situacao != (byte)TUR_TurmaSituacao.Extinta)
                {
                    btnIncluirAula.Visible = false;
                }
            }
            btnIncluirAula.Visible &= !VS_PeriodoEfetivado;

            UCComboTipoAtividadeAvaliativa.CarregarTipoAtividadeAvaliativa(true);
            UCComboTipoAtividadeAvaliativa.Obrigatorio = true;

            divEventoSemAtividade.Visible = divAvisoSubstituto.Visible = divAvisoAulaSemPlano.Visible = false;

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_RELATORIOS_DIARIO_DE_CLASSE, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {  // validação especifica para o tipo do docente logado
                    EnumTipoDocente tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);

                    if (tipoDocente == EnumTipoDocente.Compartilhado || tipoDocente == EnumTipoDocente.Projeto)
                    {
                        // atribuo false direto para DC – Frequência e DC – Avaliação
                        btnRelatorioFrequencia.Visible = btnRelatorioAvaliacao.Visible = false;
                    }
                    else
                    {
                        // DC – Frequência
                        if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                        {
                            btnRelatorioFrequencia.Visible = false; // atribuo false para não ser exibido, conforme a especificação
                        }
                        else
                        {
                            btnRelatorioFrequencia.Visible = !UCControleTurma1.VS_tud_naoLancarFrequencia;
                        }

                        // DC – Avaliação
                        if (UCControleTurma1.VS_tud_naoLancarNota)
                        {
                            btnRelatorioAvaliacao.Visible = false; // atribuo false para não ser exibido, conforme a especificação
                        }
                        else
                        {
                            btnRelatorioAvaliacao.Visible = !UCControleTurma1.VS_tud_naoLancarNota;
                        }
                    }
                }
                else
                {   // validação padrão que já existia
                    btnRelatorioFrequencia.Visible = !UCControleTurma1.VS_tud_naoLancarFrequencia;
                    btnRelatorioAvaliacao.Visible = !UCControleTurma1.VS_tud_naoLancarNota;
                }
            }
            else
            {
                btnRelatorioFrequencia.Visible = btnRelatorioAvaliacao.Visible = false;
            }

            DataTable dtAulas = CLS_TurmaAulaBO.SelecionaPorTurmaDisciplinaPeriodoCalendario(UCControleTurma1.VS_tud_id, UCNavegacaoTelaPeriodo.VS_tpc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, UCControleTurma1.VS_tdt_posicao, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0, VS_turmaDisciplinaRelacionada.tud_id);
            dtAulas.Columns.Add("mensagemSubstituto");
            DataTable dtAulasDistintas = dtAulas.Clone();
            if (dtAulas.Rows.Count > 0)
            {
                long tud_id = -1;
                int tau_id = -1;
                foreach (DataRow drAula in dtAulas.Rows)
                {
                    DateTime dataAula = Convert.ToDateTime(drAula["tau_data"]);
                    if (UCNavegacaoTelaPeriodo.VS_tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        || (dataAula >= UCNavegacaoTelaPeriodo.cap_dataInicio && dataAula <= UCNavegacaoTelaPeriodo.cap_dataFim))
                    {
                        long tud_idAula = Convert.ToInt64(drAula["tud_id"]);
                        int tau_idAula = Convert.ToInt32(drAula["tau_id"]);
                        if (tud_idAula != tud_id || tau_idAula != tau_id)
                        {
                            tud_id = tud_idAula;
                            tau_id = tau_idAula;
                            DataRow drAulaDistinta = dtAulasDistintas.NewRow();
                            drAulaDistinta.ItemArray = drAula.ItemArray;
                            if ((VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia || Convert.ToByte(drAula["tud_tipo"]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia) &&
                                MostraMsgSubstituto(tud_id, tau_id, dataAula, Convert.ToByte(drAula["tdt_posicao"]), dtAulas))
                                drAulaDistinta["mensagemSubstituto"] = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemSubstitutoRegencia").ToString();

                            dtAulasDistintas.Rows.Add(drAulaDistinta);
                        }
                        else if (dtAulasDistintas.Rows.Count > 0)
                        {
                            dtAulasDistintas.Rows[dtAulasDistintas.Rows.Count - 1]["NomeDisciplinaRelacionada"] += ", " + Convert.ToString(drAula["NomeDisciplinaRelacionada"]);
                        }
                    }
                }
            }
            grvAulas.DataSource = dtAulasDistintas;
            grvAulas.DataBind();

            grvAulas.Columns[grvAulas_ColunaFrequencia].Visible = !UCControleTurma1.VS_tud_naoLancarFrequencia && VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoConsulta) &&
                                                                  VS_EntitiesControleTurma.turmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia;
            grvAulas.Columns[grvAulas_ColunaAtAvaliada].Visible = !UCControleTurma1.VS_tud_naoLancarNota && VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoConsulta) &&
                                                                  ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.TELA_UNICA_LANCAMENTO_FREQUENCIA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

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

            grvAulas.Columns[grvAulas_ColunaAnotacoes].Visible = VS_ltPermissaoAnotacoes.Any(p => p.pdc_permissaoConsulta) && possuiPermissaoVisualizacao;
            grvAulas.Columns[grvAulas_ColunaPlanoAula].Visible = VS_ltPermissaoPlanoAula.Any(p => p.pdc_permissaoConsulta);

            List<LegendaDiario> legendaDiario = new List<LegendaDiario>();
            if (VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                legendaDiario = (from ACA_TipoDocente tipo in ACA_TipoDocenteBO.SelecionaAtivos()
                                 where !string.IsNullOrEmpty(tipo.tdc_corDestaque)
                                 select new LegendaDiario
                                 {
                                     tdc_id = tipo.tdc_id
                                     ,
                                     tipoDocente = ACA_TipoDocenteBO.ListaTipoDocentes().ToList().Find(p => p.Key == tipo.tdc_id).Value
                                     ,
                                     tdc_corDestaque = tipo.tdc_corDestaque
                                 }).ToList();
            }
            else
            {
                legendaDiario = (from ACA_TipoDocente tipo in ACA_TipoDocenteBO.SelecionaAtivos()
                                 where !string.IsNullOrEmpty(tipo.tdc_corDestaque)
                                     && tipo.tdc_id != (byte)EnumTipoDocente.Projeto
                                     && tipo.tdc_id != (byte)EnumTipoDocente.Compartilhado
                                 select new LegendaDiario
                                 {
                                     tdc_id = tipo.tdc_id
                                     ,
                                     tipoDocente = ACA_TipoDocenteBO.ListaTipoDocentes().ToList().Find(p => p.Key == tipo.tdc_id).Value
                                     ,
                                     tdc_corDestaque = tipo.tdc_corDestaque
                                 }).ToList();

                // item referente a disciplina com docencia compartilhada
                legendaDiario.Add(
                    new LegendaDiario
                    {
                        tdc_id = 0
                        ,
                        tipoDocente = ""
                        ,
                        tdc_corDestaque = ApplicationWEB.CorDocenciaCompartilhada
                    });
            }

            rptLegendaDiario.DataSource = legendaDiario;
            rptLegendaDiario.DataBind();

            divLegendaDiario.Visible = rptLegendaDiario.Items.Count > 0;

            chkReposicao.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRAR_AULA_REPOSICAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            string msg = "Excluir " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + "?";
            UCConfirmacaoOperacao1.Mensagem = msg;
            UCConfirmacaoOperacao1.ObservacaoVisivel = false;
            UCConfirmacaoOperacao1.ObservacaoObrigatorio = false;

            updCadastroAula.Update();
        }

        private bool MostraMsgSubstituto(long tud_id, int tau_id, DateTime tau_data, byte tdt_posicao, DataTable dtAulas)
        {
            byte posicaoTitular = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo);
            byte posicaoSegundoTitular = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo);
            byte posicaoSubstituto = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Substituto, ApplicationWEB.AppMinutosCacheLongo);

            if (!(tdt_posicao == posicaoTitular || tdt_posicao == posicaoSegundoTitular || tdt_posicao == posicaoSubstituto))
                return false;

            var dtAulasDia = dtAulas.AsEnumerable().Where(p => Convert.ToInt64(p["tud_id"]) == tud_id &&
                                                               Convert.ToInt32(p["tau_id"]) != tau_id &&
                                                               Convert.ToDateTime(p["tau_data"]) == tau_data);

            if (!dtAulasDia.Any())
                return false;

            if (tdt_posicao == posicaoTitular)
            {
                return dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoSubstituto) &&
                       (!VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia || dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoSegundoTitular));
            }
            else if (tdt_posicao == posicaoSegundoTitular)
            {
                return dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoSubstituto) &&
                       (!VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia ||
                        dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoTitular));
            }
            else if (tdt_posicao == posicaoSubstituto)
            {
                return (dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoTitular) ||
                        dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoSegundoTitular)) &&
                       (!VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia ||
                        (dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoTitular) &&
                         dtAulasDia.Any(p => Convert.ToByte(p["tdt_posicao"]) == posicaoSegundoTitular)));
            }
            else
                return false;
        }

        /// <summary>
        /// Chama a página de relatório de acordo com o relatório informado.
        /// </summary>
        /// <param name="relatorio">Relatório a ser chamado</param>
        private void ImprimirRelatorio(string relatorio)
        {
            string parameter = string.Empty, report = relatorio;
            parameter = "esc_id=" + VS_EntitiesControleTurma.turma.esc_id +
                        "&uni_id=" + VS_EntitiesControleTurma.turma.uni_id +
                        "&cal_id=" + UCNavegacaoTelaPeriodo.VS_cal_id +
                        "&cur_id=" + VS_EntitiesControleTurma.curriculoPeriodo.cur_id +
                        "&crr_id=" + VS_EntitiesControleTurma.curriculoPeriodo.crr_id +
                        "&crp_id=" + VS_EntitiesControleTurma.curriculoPeriodo.crp_id +
                        "&tur_id=" + UCControleTurma1.VS_tur_id +
                        "&tpc_id=" + UCNavegacaoTelaPeriodo.VS_tpc_id +
                        "&tud_id=" + UCControleTurma1.VS_tud_id +
                        "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                        "&nomeComponenteCurricular=" + (string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                        "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                        "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                        "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                        "&cal_ano=" + UCNavegacaoTelaPeriodo.VS_cal_ano.ToString() +
                        "&documentoOficial=false";

            CarregaSessionPaginaRetorno();

            CFG_RelatorioBO.CallReport("Relatorios", report, parameter, HttpContext.Current);
        }

        /// <summary>
        /// Carrega o lançamento de frequência dos alunos da turma
        /// </summary>
        private void CarregarAlunosFrequencia()
        {
            try
            {
                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                    string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                    string.Empty;

                rptDiarioAlunosFrequenciaTerriorio.Visible = VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia;
                rptDiarioAlunosFrequencia.Visible = !rptDiarioAlunosFrequenciaTerriorio.Visible;

                if (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia)
                {
                    VS_Aulas_Alunos = CLS_TurmaAulaAlunoBO.SelecionaFrequenciaAulaTurmaDisciplinaTerritorio(VS_tud_id_Aula, VS_tau_id);
                    VS_Aulas_Alunos = VS_Aulas_Alunos.FindAll(p => !p.falta_abonada);

                    rptDiarioAlunosFrequenciaTerriorio.DataSource =
                        MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                        VS_tud_id_Aula, UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids)
                        .Where(p => ((p.mtd_dataSaida > entityAula.tau_data) || (p.mtd_dataSaida == null)) && (p.mtd_dataMatricula <= entityAula.tau_data)
                                && VS_Aulas_Alunos.Any(q => q.alu_id == p.alu_id));
                    rptDiarioAlunosFrequenciaTerriorio.DataBind();
                }
                else
                {
                    VS_Aulas_Alunos = CLS_TurmaAulaAlunoBO.SelecionaFrequenciaAulaTurmaDisciplina(VS_tud_id_Aula, VS_tau_id, tur_ids);
                    VS_Aulas_Alunos = VS_Aulas_Alunos.FindAll(p => !p.falta_abonada);

                    rptDiarioAlunosFrequencia.DataSource =
                        MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                        VS_tud_id_Aula, UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids)
                        .Where(p => ((p.mtd_dataSaida > entityAula.tau_data) || (p.mtd_dataSaida == null)) && (p.mtd_dataMatricula <= entityAula.tau_data)
                                && VS_Aulas_Alunos.Any(q => q.alu_id == p.alu_id));

                    rptDiarioAlunosFrequencia.DataBind();
                }

                string[] diasSemana = { "Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado" };
                string strDataAula = entityAula.tau_data == new DateTime().Date ? string.Empty : entityAula.tau_data.ToString("dd/MM/yyyy") + " - " + diasSemana[Convert.ToInt32(entityAula.tau_data.DayOfWeek)];
                lblDataAula.Text = "<b>Aula:</b> " + strDataAula;

                bool permiteEditar = (VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                        && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao)) 
                                        || (!PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao);

                if (permiteEditar)
                {
                    permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                }

                permiteEditar &= VS_Periodo_Aberto;

                permiteEditar &= !VS_PeriodoEfetivado;

                btnSalvarFrequencia.Visible = btnSalvarFrequenciaCima.Visible = VS_Aulas_Alunos.Count > 0 && permiteEditar;

                // localizo o nome do usuário que realizou a última alteração nos dados
                var usuarioAlteracao = (from aula in VS_Aulas_Alunos
                                        where !string.IsNullOrEmpty(aula.nomeUsuAlteracao)
                                        select new
                                        {
                                            nomeDocenteAlteracao = aula.nomeUsuAlteracao,
                                            dataAlteracao = aula.tau_dataAlteracao
                                        });

                divUsuarioAlteracaoFrequencia.Visible = false;
                if ((usuarioAlteracao != null) && usuarioAlteracao.Any())
                {
                    var usuario = usuarioAlteracao.First();

                    VS_nome_usu_alteracao = usuario.nomeDocenteAlteracao;
                    VS_DataAlteracaoRegistro = usuario.dataAlteracao;

                    lblAlteracaoFreq.Text = CarregarUsuarioAlteracao();
                    if (!string.IsNullOrEmpty(lblAlteracaoFreq.Text))
                    {
                        divUsuarioAlteracaoFrequencia.Visible = true;
                    }
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairFrequencia", "var exibeMensagemSair=" + btnSalvarFrequencia.Visible.ToString().ToLower() + ";", true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroFrequencia",
                    "$('#divLancamentoFrequencia').dialog('option', 'title', 'Lançamento de frequência - " + strDataAula + "');" +
                    "$('#divLancamentoFrequencia').dialog('open');", true);

                updFrequencia.Update();
                pnlLancamentoFrequencia.Focus();

                VS_Data_Diario_TurmaAula = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos para lançamento de frequência.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o nome do usuário que alterou os dados
        /// </summary>
        private String CarregarUsuarioAlteracao()
        {
            if (string.IsNullOrEmpty(VS_nome_usu_alteracao))
            {
                return string.Empty;
            }
            return "</br>Incluído/Alterado por: " + VS_nome_usu_alteracao.Trim() + " em " + VS_DataAlteracaoRegistro.ToString("G");
        }

        /// <summary>
        /// Carrega as anotações dos alunos para a aula.
        /// </summary>
        private void CarregarAnotacoesAluno()
        {
            try
            {
                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                   string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                   string.Empty;

                DataTable dt = CLS_TurmaAulaAlunoBO.SelecionaAnotacaoPorAulaTurmaDisciplina(VS_tud_id_Aula, VS_tau_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, tur_ids);

                // Adiciona nova linha do grid.
                if (dt.Rows.Count <= 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["alu_mtu_mtd_id"] = "-1;-1;-1";
                    dt.Rows.Add(dr);
                }

                string strDataAula = entityAula.tau_data == new DateTime().Date ? string.Empty : entityAula.tau_data.ToString("dd/MM/yyyy");

                dtAlunosAnotacoes =
                       MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina
                           (
                               VS_tud_id_Aula,
                               UCNavegacaoTelaPeriodo.VS_tpc_id,
                               VS_tipoDocente,
                               false,
                               UCNavegacaoTelaPeriodo.cap_dataInicio, 
                               UCNavegacaoTelaPeriodo.cap_dataFim,
                               ApplicationWEB.AppMinutosCacheMedio,
                               tur_ids
                           );

                // Mostra nova linha.
                grvAnotacaoAluno.DataSource = dt;
                grvAnotacaoAluno.DataBind();

                int ultimaLinha = grvAnotacaoAluno.Rows.Count - 1;

                // Mostra botões de adicionar e cancelar.
                grvAnotacaoAluno.Rows[ultimaLinha].FindControl("btnAdicionar").Visible = true;
                grvAnotacaoAluno.Rows[ultimaLinha].FindControl("btnCancelar").Visible = true;
                //grvAnotacaoAluno.Rows[ultimaLinha].FindControl("cpvAluno").Visible = false;

                bool permiteEditar = VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                if (permiteEditar)
                {
                    permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                }

                HabilitaControles(grvAnotacaoAluno.Controls, permiteEditar);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairAnotacoes", "var exibeMensagemSair=" + btnSalvarAnotacoes.Visible.ToString().ToLower() + ";", true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroAnotacoes",
                    "$('#divAnotacoesAluno').dialog('option', 'title', 'Anotações sobre os alunos - aula " + strDataAula + "');" +
                    "$('#divAnotacoesAluno').dialog('open');", true);

                updAnotacoes.Update();

                btnSalvarAnotacoes.Visible = permiteEditar;

                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || !permiteEditar)
                {
                    btnSalvarAnotacoes.Visible = false;
                }
                else
                {
                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ANOTACOES_MAIS_DE_UM_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        btnAdicionarMaisdeUmAluno.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos cadastro de anotações.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega disciplina(s componente(s) da regencia somente da turma selecionada.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="ddlDisciplinaComponentes">Combo que será carregado</param>
        private void CarregaComponenteRegenciaDocente(long tur_id, DropDownList ddlDisciplinaComponentes)
        {
            List<sComboTurmaDisciplina> turmaDisciplinaComponenteRegencia = (from dr in VS_TurmaDisciplinaDocente
                                                                             where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                             && (Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == tur_id)
                                                                             select new sComboTurmaDisciplina
                                                                             {
                                                                                 tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                                 ,
                                                                                 tur_tud_id = dr.tur_tud_id.ToString()
                                                                                 ,
                                                                                 tud_nome = dr.tud_nome.ToString()
                                                                             }).ToList();

            if (ddlDisciplinaComponentes.Items.Count == 0)
            {
                ddlDisciplinaComponentes.DataSource = turmaDisciplinaComponenteRegencia;
                ddlDisciplinaComponentes.DataBind();
            }

            if (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                ddlDisciplinaComponentes == _ddlTurmaDisciplinaComponente && cblComponentesRegencia.Items.Count == 0)
            {
                cblComponentesRegencia.DataSource = turmaDisciplinaComponenteRegencia;
                cblComponentesRegencia.DataBind();
            }
        }

        /// <summary>
        /// Carrega o combo de disciplinas, verificando se usuário logado é docente.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_idSelecionada">Id da turma disciplina selecionada</param>
        /// <returns></returns>
        private bool CarregarDisciplinasComboAtAvaliativa(long tur_id, long tud_idSelecionada)
        {
            try
            {
                List<sComboTurmaDisciplina> turmaDisciplina = (from dr in VS_TurmaDisciplinaDocente
                                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                     && Convert.ToInt64(dr.tur_tud_id.Split(';')[1]) == VS_tud_id_Aula
                                                               select new sComboTurmaDisciplina
                                                               {
                                                                   tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                   ,
                                                                   tur_tud_id = dr.tur_tud_id.ToString()
                                                               }).ToList();

                //Carrega as disciplinas da turma caso a visão do usuário não seja individual, exceto as componentes de regência.
                ddlTurmaDisciplinaAtAvaliativa.Items.Clear();
                ddlTurmaDisciplinaAtAvaliativa.DataSource = turmaDisciplina;
                ddlTurmaDisciplinaAtAvaliativa.DataBind();

                // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                if (VS_EntitiesControleTurma.turmaDisciplina != null && VS_tud_tipo_Aula == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) && ddlComponenteAtAvaliativa.Items.Count == 0)
                {
                    CarregaComponenteRegenciaDocente(
                            Convert.ToInt64(ddlTurmaDisciplinaAtAvaliativa.SelectedValue.Split(';')[0])
                            , ddlComponenteAtAvaliativa);
                }

                if (ddlTurmaDisciplinaAtAvaliativa.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlTurmaDisciplinaAtAvaliativa.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlTurmaDisciplinaAtAvaliativa.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlTurmaDisciplinaAtAvaliativa.SelectedValue = x.First();
                }

                if (ddlComponenteAtAvaliativa.Items.Count > 0)
                {
                    // Seleciona a disciplina setada ou a primeira da turma.
                    IEnumerable<string> x;
                    if (tud_idSelecionada > 0)
                    {
                        x = from ListItem item in ddlComponenteAtAvaliativa.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                   && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                            select item.Value;
                    }
                    else
                    {
                        x = from ListItem item in ddlComponenteAtAvaliativa.Items
                            where item.Value.Split(';')[0].Equals(tur_id.ToString())
                            select item.Value;
                    }

                    if (x.Count() > 0)
                        ddlComponenteAtAvaliativa.SelectedValue = x.First();
                }

                return true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                return false;
            }
        }

        /// <summary>
        /// Indica se a visibilidade é do tipo regência-componente regência.
        /// </summary>
        /// <param name="ddlTurmas">Combo da turma disciplina</param>
        /// <returns></returns>
        private bool VisibilidadeRegencia(DropDownList ddlTurmas)
        {
            return VS_tud_tipo_Aula == (byte)ACA_CurriculoDisciplinaTipo.Regencia
                    && ddlTurmas.Items.Count > 0 && Convert.ToByte(ddlTurmas.SelectedValue.Split(';')[3]) == (byte)ACA_CurriculoDisciplinaTipo.Regencia;
        }

        /// <summary>
        /// Carrega o painel com atividades e combos relacionados à atividade.
        /// </summary>
        /// <param name="selecionaDisciplina">Indica se vai selecionar a disciplina de componente da regência automaticamente.</param>
        private void CarregarAtividades(bool selecionaDisciplina)
        {
            long tud_id = VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula;
            UCComboTipoAtividadeAvaliativa.Valor = -1;
            if (selecionaDisciplina)
            {
                if (!CarregarDisciplinasComboAtAvaliativa(UCControleTurma1.VS_tur_id, tud_id))
                {
                    return;
                }
            }

            if (VS_tud_tipo_Aula != (byte)ACA_CurriculoDisciplinaTipo.Regencia || ddlComponenteAtAvaliativa.Items.Count == 0)
            {
                ddlComponenteAtAvaliativa.Visible = lblComponenteAtAvaliativa.Visible = false;
            }
            else
            {
                ddlComponenteAtAvaliativa.Visible = lblComponenteAtAvaliativa.Visible = true;
            }

            UCComboTipoAtividadeAvaliativa.CarregarTipoAtividadeAvaliativa(true);
            txtNomeAtividade.Text = txtConteudoAtividade.Text = string.Empty;
            rblAtividadeAvaliativa.Items.Cast<ListItem>().ToList().ForEach(p => p.Selected = Convert.ToBoolean(p.Value));

            if (ParametroPermitirAtividadesExclusivas)
            {
                divAtividadeExclusiva.Visible = true;
                chkAtividadeExclusiva.Checked = false;
            }
            else
                divAtividadeExclusiva.Visible = false;

            string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                   string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                   string.Empty;

            // Carregar as atividades e notas dos alunos nas atividades.
            DTAtividades = VS_DisciplinaEspecial ?
                CLS_TurmaNotaBO.GetSelectBy_TurmaDisciplina_PeriodoFiltroDeficiencia
                (
                    VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                        ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                    , VS_tipoDocente
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    , UCControleTurma1.VS_tdt_posicao
                ) :
                CLS_TurmaNotaBO.GetSelectBy_TurmaDisciplina_Periodo
                (
                    VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                        ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                    , UCNavegacaoTelaPeriodo.VS_tpc_id
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    , UCControleTurma1.VS_tdt_posicao
                    , tur_ids
                );

            lblInfoAtividade.Text = UtilBO.GetErroMessage("Marque a opção Efetivado para indicar que o lançamento de nota foi finalizado.", UtilBO.TipoMensagem.Informacao);

            var x = from DataRow dr in DTAtividades.Rows
                    where Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(dr["tau_id"])) ? dr["tau_id"] : 0) == entityAula.tau_id
                    select dr;

            List<AlunosTurmaDisciplina> ltAlunos = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(
                VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula,
                UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tipoDocente, false, UCNavegacaoTelaPeriodo.cap_dataInicio, UCNavegacaoTelaPeriodo.cap_dataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids)
                .Where(p => ((p.mtd_dataSaida > entityAula.tau_data) || (p.mtd_dataSaida == null)) && (p.mtd_dataMatricula <= entityAula.tau_data)).ToList();

            ltAtividadeIndicacaoNota = (from DataRow dr in DTAtividades.Rows
                                        where Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(dr["tau_id"])) ? dr["tau_id"] : 0) == entityAula.tau_id
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

            // Carrega os alunos matriculados
            rptAlunos.DataSource = ltAlunos;

            rptAlunos.DataBind();

            string strDataAula = entityAula.tau_data == new DateTime().Date ? string.Empty : entityAula.tau_data.ToString("dd/MM/yyyy");

            bool permiteEditar = VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            // Se for um usuário que não pertence mais a turma, mas está visualizando uma aula de outro docente da mesma posição
            bool usuarioHistorico = false;
            if (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id != VS_usu_id)
            {
                usuarioHistorico = true;
            }

            if (permiteEditar && VS_usu_id != Guid.Empty)
            {
                permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
            }

            permiteEditar &= VS_Periodo_Aberto;

            permiteEditar &= !VS_PeriodoEfetivado;

            btnNovaAtividade.Visible = true;
            btnNovaAtividade.Enabled = permiteEditar;
            btnCancelarAtividade.Visible = btnEditarAtividade.Visible = false;
            VS_tnt_id = -1;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroAtividade",
                "$('#divAtividadeAvaliativa').dialog('option', 'title', 'Lançar " +
                    ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_ATIVIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                    " - aula " + strDataAula + " - " + VS_tud_nome_Aula + "');" +
                "$('#divAtividadeAvaliativa').dialog('open');", true);

            VS_Data_Diario_TurmaNota = DateTime.Now;

            // se permite alteração ou não for o docente logado, for um perfil superior
            btnSalvarNota.Visible = divSalvarAvaliacaoCima.Visible = (permiteEditar && ((x.Any(n => Convert.ToBoolean(n["permissaoAlteracao"])) || (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 && x.Count<DataRow>() > 0)) && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar));

            if (usuarioHistorico)
            {
                btnSalvarNota.Visible = divSalvarAvaliacaoCima.Visible = false;
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairAvaliacoes", "var exibeMensagemSair=" + btnSalvarNota.Visible.ToString().ToLower() + ";", true);

            pnlAvaliacao.Visible = x.Any();
            fdsAtividadeAvaliativa.Visible = x.Any() || permiteEditar;
            pnlCadastroAtividade.Visible = permiteEditar;

            pnlCadastroAtividade.GroupingText = @"Nova avaliação";
            if (!permiteEditar && VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa))
            {
                // Mostra apenas o combo dos componentes da regência.
                pnlCadastroAtividade.GroupingText = "";
                fdsAtividadeAvaliativa.Visible = true;
                pnlCadastroAtividade.Visible = true;
                divAtividadeExclusiva.Visible = divBotaoAcaoAtividade.Visible = divItensAtividade.Visible =
                    UCCamposObrigatorios2.Visible = false;
            }
            else
            {
                divAtividadeExclusiva.Visible = ParametroPermitirAtividadesExclusivas;
                divBotaoAcaoAtividade.Visible = divItensAtividade.Visible =
                    UCCamposObrigatorios2.Visible = true;
            }

            if (!x.Any())
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Não existe nenhuma atividade cadastrada.",
                    UtilBO.TipoMensagem.Informacao);
            }

            if (!VS_Periodo_Aberto)
            {
                // Se o período estiver fechado, mostrar mensagem que o lançamento é só pra edição.
                lblMessageAtividade.Text += UtilBO.GetErroMessage("Tela disponível apenas para consulta.", UtilBO.TipoMensagem.Informacao);
            }

            updAtividade.Update();
            pnlAtividadeAvaliativa.Focus();

            // localizo o nome do usuário que realizou a última alteração nos dados
            var usuarioAlteracao = (from DataRow dr in DTAtividades.Rows
                                    where
                                    !string.IsNullOrEmpty(dr["nomeUsuAlteracao"].ToString())
                                    && Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(dr["tau_id"])) ? dr["tau_id"] : 0) == entityAula.tau_id

                                    group dr by dr["nomeUsuAlteracao"].ToString() into g

                                    select new
                                    {
                                        nomeDocenteAlteracao = g.Key,
                                        dataAlteracao = g.Cast<DataRow>().Select(p => Convert.ToDateTime(p["tnt_dataAlteracao"])).Max()
                                        // dr["nomeUsuAlteracao"].ToString(),
                                        //Convert.ToDateTime(dr["tnt_dataAlteracao"])
                                    });

            divUsuarioAlteracaoAtividadeAvaliativa.Visible = false;
            if ((usuarioAlteracao != null) && usuarioAlteracao.Any())
            {
                var usuario = usuarioAlteracao.First();

                VS_nome_usu_alteracao = usuario.nomeDocenteAlteracao;
                VS_DataAlteracaoRegistro = usuario.dataAlteracao;

                lblAlteracaoAtividadeAvaliativa.Text = CarregarUsuarioAlteracao();
                if (!string.IsNullOrEmpty(lblAlteracaoAtividadeAvaliativa.Text))
                {
                    divUsuarioAlteracaoAtividadeAvaliativa.Visible = true;  // exibo o usuário que alterou o registro
                }
            }

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.RELACIONAR_HABILIDADES_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                CarregarHabilidadesAvaliacao();
            }
            else
            {
                fdsHabilidadesRelacionadas.Visible = false;
            }
        }

        /// <summary>
        /// Carrega as habilidades do planejamento de aulas
        /// </summary>
        private void CarregarHabilidadesAvaliacao()
        {
            long _Tud_id = ddlComponenteAtAvaliativa.Visible ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula;

            if (VS_EntitiesControleTurma.turma.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno)
            {
                try
                {
                    UCHabilidades.CarregarHabilidades(
                        VS_EntitiesControleTurma.curriculoPeriodo.cur_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crr_id,
                        VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                        UCControleTurma1.VS_tur_id,
                        _Tud_id,
                        UCNavegacaoTelaPeriodo.VS_cal_id,
                        UCControleTurma1.VS_tdt_posicao,
                        VS_tnt_id,
                        UCNavegacaoTelaPeriodo.VS_tpc_id
                    );

                    UCHabilidades.Visible = true;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as orientações curriculares.", UtilBO.TipoMensagem.Erro);
                    btnSalvarPlanoAula.Visible = btnSalvarPlanoAulaCima.Visible = false;
                }
            }
            else
            {
                UCHabilidades.Visible = false;
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairHabPlanoAula", "var exibeMensagemSair=" + btnSalvarPlanoAula.Visible.ToString().ToLower() + ";", true);
        }

        /// <summary>
        /// Salva as avaliações para as atividades avaliativas.
        /// </summary>
        private void SalvarNotasAvaliacao()
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                if (rptAlunos.Items.Count == 0)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Lançamento de avaliações realizado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharLancamentoAvaliacao", "var exibirMensagemConfirmacao=false;$('#divAtividadeAvaliativa').dialog('close');", true);

                    updFrequencia.Update();
                }

                Guid UsuIdLogado = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

                List<CLS_TurmaNotaAluno> listTurmaNotaAluno = new List<CLS_TurmaNotaAluno>();
                List<CLS_TurmaNota> listTurmaNota = new List<CLS_TurmaNota>();

                RepeaterItem header = (RepeaterItem)rptAlunos.Controls[0];
                Repeater rptAtividadesHeader = (Repeater)header.FindControl("rptAtividadesEfetivado");

                foreach (RepeaterItem itemAtividade in rptAtividadesHeader.Items)
                {
                    Int16 tdt_posicao = Convert.ToInt16(((Label)itemAtividade.FindControl("lblPosicao")).Text);
                    CheckBox chkEfetivado = (CheckBox)itemAtividade.FindControl("chkEfetivado");
                    int tnt_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltnt_id")).Text);
                    bool tnt_exclusiva = Convert.ToBoolean(((Label)itemAtividade.FindControl("lblAtividadeExclusiva")).Text);
                    Guid usu_id = (!string.IsNullOrEmpty(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) ? new Guid(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty);
                    if (usu_id == UsuIdLogado || VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao) || __SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                    {
                        CLS_TurmaNota ent = new CLS_TurmaNota
                        {
                            tud_id = VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                                        ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                                     ,
                            tnt_id = tnt_id
                        };
                        CLS_TurmaNotaBO.GetEntity(ent);
                        if (!ent.IsNew && ent.tnt_dataAlteracao > VS_Data_Diario_TurmaNota)
                            throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.Validacao_Data_TurmaNota").ToString());

                        ent.tnt_efetivado = chkEfetivado.Checked;
                        ent.tdt_posicao = UCControleTurma1.VS_tdt_posicao;
                        ent.tnt_exclusiva = tnt_exclusiva;
                        ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

                        listTurmaNota.Add(ent);
                    }
                }

                listTurmaNotaAluno = (from RepeaterItem itemAluno in rptAlunos.Items
                                      let rptAtividades = (Repeater)itemAluno.FindControl("rptAtividades")
                                      let alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text)
                                      let mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text)
                                      let mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text)
                                      let pes_nome = Convert.ToString(((Label)itemAluno.FindControl("lblNome")).Text)
                                      from RepeaterItem itemAtividadeAluno in rptAtividades.Items
                                      let divAtividades = (HtmlGenericControl)itemAtividadeAluno.FindControl("divAtividades")
                                      where divAtividades != null
                                      let tdt_posicao = Convert.ToInt16(((Label)divAtividades.FindControl("lblPosicao")).Text)
                                      let usu_id = (!string.IsNullOrEmpty(((Label)divAtividades.FindControl("lblUsuIdAtiv2")).Text) ? new Guid(((Label)divAtividades.FindControl("lblUsuIdAtiv2")).Text) : Guid.Empty)
                                      where (VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao) || usu_id == UsuIdLogado || __SessionWEB.__UsuarioWEB.Docente.doc_id == 0) &&
                                            CLS_TurmaNotaAlunoBO.VerificaValoresNotas(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica, RetornaAvaliacao(itemAtividadeAluno), pes_nome)
                                      let tnt_id = Convert.ToInt32(((Label)itemAtividadeAluno.FindControl("lbltnt_id")).Text)
                                      let rel = VS_Nota_Relatorio.Find(p =>
                                                ((p.alu_id == alu_id) &&
                                                 (p.tnt_id == tnt_id) &&
                                                 (p.mtu_id == mtu_id)))
                                      let tna_participante = (itemAtividadeAluno.FindControl("chkParticipante") != null && ((CheckBox)itemAtividadeAluno.FindControl("chkParticipante")).Visible) ?
                                                             ((CheckBox)itemAtividadeAluno.FindControl("chkParticipante")).Checked :
                                                             true
                                      select new CLS_TurmaNotaAluno
                                      {
                                          tud_id = VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                                            ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                                          ,
                                          tnt_id = tnt_id
                                          ,
                                          alu_id = alu_id
                                          ,
                                          mtu_id = mtu_id
                                          ,
                                          mtd_id = mtd_id
                                          ,
                                          tna_avaliacao = RetornaAvaliacao(itemAtividadeAluno)
                                          ,
                                          tna_relatorio = rel.valor
                                          ,
                                          tna_naoCompareceu = false
                                          ,
                                          tna_situacao = 1
                                          ,
                                          tna_participante = tna_participante
                                      }).ToList();

                List<CLS_TurmaAula> listTurmaAula = new List<CLS_TurmaAula>();
                listTurmaAula.Add(new CLS_TurmaAula
                {
                    tud_id = VS_tud_id_Aula,
                    tau_id = VS_tau_id,
                    tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(listTurmaNota)
                });

                if (CLS_TurmaNotaAlunoBO.Save
                        (
                            listTurmaNotaAluno
                            , listTurmaNota
                            , null
                            , UCControleTurma1.VS_tur_id
                            , VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                                ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                            , UCNavegacaoTelaPeriodo.VS_tpc_id
                            , VS_EntitiesControleTurma.formatoAvaliacao.fav_id
                            , UCControleTurma1.VS_tdt_posicao
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , listTurmaAula
                            , VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico
                            , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                            , 0
                            , (byte)LOG_TurmaNota_Alteracao_Origem.WebDiarioClasse
                            , (byte)LOG_TurmaNota_Alteracao_Tipo.LancamentoNotas
                        ))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Lançamento de avaliações | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tur_id: " + UCControleTurma1.VS_tur_id + "; tud_id: " + (VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                                                                                                                             ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula));
                    lblMessage.Text = UtilBO.GetErroMessage("Lançamento de avaliações realizado com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Image imgAtividadeSituacaoEfetivada = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacaoEfetivada");
                    if (imgAtividadeSituacaoEfetivada != null)
                        imgAtividadeSituacaoEfetivada.Visible = listTurmaAula.First().tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada;

                    Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacao");
                    if (imgSituacao != null)
                        imgSituacao.Visible = listTurmaAula.First().tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharLancamentoAvaliacao", "var exibirMensagemConfirmacao=false;$('#divAtividadeAvaliativa').dialog('close');", true);

                    updFrequencia.Update();
                }
            }
            catch (ValidationException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar salvar as notas das avaliações.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o combo de disciplinas, verificando se usuário logado é docente.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_idSelecionada">Id da turma disciplina selecionada</param>
        private void CarregarDisciplinasCombo(long tur_id, long tud_idSelecionada)
        {
            //Foi inserida esse código pois nesse momento o viewState está com a posição da docente da linha do grid,
            //porém logo abaixo isso é alterado, e para carregar o plano de aula é necessária a posição desse momento.
            byte posicao = VS_tdt_posicaoEdicao;

            try
            {
                List<sComboTurmaDisciplina> turmaDisciplina = (from dr in VS_TurmaDisciplinaDocente
                                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                        && Convert.ToInt64(dr.tur_tud_id.Split(';')[1]) == VS_tud_id_Aula
                                                               select new sComboTurmaDisciplina
                                                               {
                                                                   tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                   ,
                                                                   tur_tud_id = dr.tur_tud_id.ToString()
                                                               }).ToList();

                //Carrega as disciplinas da turma caso a visão do usuário não seja individual, exceto as componentes de regência.
                ddlTurmaDisciplina.Items.Clear();
                ddlTurmaDisciplina.DataSource = turmaDisciplina;
                ddlTurmaDisciplina.DataBind();

                // Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
                if (VS_EntitiesControleTurma.turmaDisciplina != null && VS_tud_tipo_Aula == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) && _ddlTurmaDisciplinaComponente.Items.Count == 0)
                {

                    long turma;
                    Int64.TryParse(ddlTurmaDisciplina.SelectedValue.Split(';')[0], out turma);
                    CarregaComponenteRegenciaDocente(
                            turma
                            , _ddlTurmaDisciplinaComponente);
                }

                cblComponentesRegencia.Visible = VS_tud_tipo_Aula == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia);

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    if (_ddlTurmaDisciplinaComponente.Items.Count > 0)
                    {
                        var x = from ListItem item in _ddlTurmaDisciplinaComponente.Items
                                where item.Value.Split(';')[0].Equals(tur_id.ToString())
                                select item.Value;

                        if (x.Count() > 0)
                            _ddlTurmaDisciplinaComponente.SelectedValue = x.First();
                    }

                    VS_tdt_posicaoEdicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id, Tud_idComponente, ApplicationWEB.AppMinutosCacheLongo);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            if (ddlTurmaDisciplina.Items.Count > 0)
            {
                // Seleciona a disciplina setada ou a primeira da turma.
                IEnumerable<string> x;
                if (tud_idSelecionada > 0)
                {
                    x = from ListItem item in ddlTurmaDisciplina.Items
                        where item.Value.Split(';')[0].Equals(tur_id.ToString())
                               && item.Value.Split(';')[1].Equals(tud_idSelecionada.ToString())
                        select item.Value;
                }
                else
                {
                    x = from ListItem item in ddlTurmaDisciplina.Items
                        where item.Value.Split(';')[0].Equals(tur_id.ToString())
                        select item.Value;
                }

                if (x.Count() > 0)
                    ddlTurmaDisciplina.SelectedValue = x.First();
            }

            // Carrega os dados na tela com a turma selecionada no combo.
            CarregarPlanoAula(posicao);
        }

        /// <summary>
        /// Carrega a janela de plano de aula e planejamento do bimestre.
        /// </summary>
        private void CarregarPlanoAula(byte posicao)
        {
            LimparCamposPlanoAula();
            lblMessage.Text = string.Empty;

            ddlTurmaDisciplinaComponente.Visible = lblTurmaDisciplinaComponente.Visible =
            divTurmaDisciplinaComponentePlanejamento.Visible = false;

            divSinteseDaAula.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SINTESE_REGENCIA_AULA_TURMA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                List<ACA_RecursosAula> lRecursos = ACA_RecursosAulaBO.GetRecursoAulaBy_All(ApplicationWEB.AppMinutosCacheLongo);
                chkRecursos.DataSource = lRecursos;
                chkRecursos.DataBind();
            }

            Operacao = OperacaoAtual.AlterandoAula;
            // Atualiza estado da tela (divs visíveis).

            // As turmas a partir do ano de 2015 possuem um unico plano de aula 
            // para os componentes da regencia.
            if ((VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)) ||
                !VisibilidadeRegencia(ddlTurmaDisciplina))
            {
                CarregarAula(entityAula);
            }
            else
            {
                CarregarPlanoAulaRegencia(new CLS_TurmaAulaRegencia());
            }
            CarregarTurmaPeriodo();
            CarregarTurmaAulaOrientacaoCurricular();

            if (string.IsNullOrEmpty(lblMessage.Text))
            {
                string strDataAula = entityAula.tau_data == new DateTime().Date ? string.Empty : entityAula.tau_data.ToString("dd/MM/yyyy");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroPlanoAula",
                                                    "$('#divPlanoAula').dialog('option', 'title', 'Plano de aula - aula " + strDataAula + "');"
                                                    + "$('#divPlanoAula').dialog('option', 'width'," + (aPlanejamentoBimestre.Visible ? "'90%'" : "'60%'") + ");"
                                                    + "$(document).ready(function() { $('#divPlanoAula').dialog('open'); });", true);
                pnlPanoAulaDados.Focus();
                btnSalvarPlanoAulaCima.Focus();

                VS_Data_Diario_TurmaAula = DateTime.Now;
            }

            bool permiteEditar = VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            bool permiteEditarPlanoAulaPlanejamento = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                                                    VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoEdicao) :
                                                    __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            if (permiteEditar)
            {
                permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
            }

            if (permiteEditarPlanoAulaPlanejamento)
            {
                permiteEditarPlanoAulaPlanejamento = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
            }

            HabilitaControles(divCadastroPlanoAula.Controls, permiteEditar);
            HabilitaControles(fdsCOC.Controls, permiteEditarPlanoAulaPlanejamento);

            btnSalvarPlanoAula.Visible = btnSalvarPlanoAulaCima.Visible = permiteEditar;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairPlanoAula", "var exibeMensagemSair=" + btnSalvarPlanoAula.Visible.ToString().ToLower() + ";", true);

            updPlanejamento.Update();
            SetaTela(Operacao);

            VS_nome_usu_alteracao = entityAula.nomeUsuAlteracao;
            VS_DataAlteracaoRegistro = entityAula.tau_dataAlteracao;

            // carrego o usuário que fez a última alteração.
            lblAlteracaoPlanoAula.Text = CarregarUsuarioAlteracao();
            divUsuarioAlteracaoPlanoAula.Visible = !string.IsNullOrEmpty(lblAlteracaoPlanoAula.Text);

            divRegistroAula.Visible = fsRecursosUtilizados.Visible = VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (chkAtividadeCasa.Visible && VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                chkAtividadeCasa.Visible = false;
                divAtividadeCasa.Visible = true;
                SetaDisplayCss(divAtividadeCasa, true);
            }

            //Não vai exibir o lançamento de objetos de aprendizagem ainda
            DataTable dtCampos = new DataTable();// CLS_ObjetoAprendizagemTurmaDisciplinaBO.SelecionaObjTudTpc(VS_tud_id_Aula, UCNavegacaoTelaPeriodo.VS_tpc_id);
            divObjetosAprendizagem.Visible = dtCampos.Rows.Count > 0;
            if (divObjetosAprendizagem.Visible)
            {
                CarregarObjetosAprendizagem(dtCampos);
            }
        }

        /// <summary>
        /// Carrega a lista de orientações curriculares da aula.
        /// </summary>
        private void CarregarTurmaAulaOrientacaoCurricular()
        {
            //Verificar se será carregada a orientação curricular apartir do parâmetro acadêmico
            if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_HABILIDADES_PLANO_AULA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                fdsHabilidadesAula.Visible = false;
                return;
            }

            fdsHabilidadesAula.Visible = true;

            spanLegend.InnerText = (string)GetGlobalResourceObject("Classe", "DiarioClasse.fdsHabilidadesAula.spanLegend");
            spanTrabalhado.InnerText = (string)GetGlobalResourceObject("Classe", "DiarioClasse.fdsHabilidadesAula.spanTrabalhado");

            long tud_id = _ddlTurmaDisciplinaComponente.Visible ? Tud_idComponente : VS_tud_id_Aula;
            long tud_idRegencia = _ddlTurmaDisciplinaComponente.Visible ? VS_tud_id_Aula : 0;

            int tau_id = VS_tau_id;

            var turmaaulaOrientacaocurricular = CLS_TurmaAulaOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplina(tud_id, tud_idRegencia, tau_id);
            string ocr_ids = string.Join(";", (from DataRow dr in turmaaulaOrientacaocurricular.Rows
                                               let ocr_id = dr["ocr_id"].ToString()
                                               where !string.IsNullOrEmpty(ocr_id)
                                               select ocr_id).Distinct().ToArray());

            dtOrientacaoNiveisAprendizado = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, null, ApplicationWEB.AppMinutosCacheLongo);
            dtNivelArendizadoCurriculo = ORC_NivelAprendizadoBO.GetSelectNiveisAprendizadoAtivos(VS_EntitiesControleTurma.curriculoPeriodo.cur_id, VS_EntitiesControleTurma.curriculoPeriodo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id, ApplicationWEB.AppMinutosCacheLongo);

            for (int i = 0; i <= (turmaaulaOrientacaocurricular.Rows.Count - 1); i++)
            {
                if ((int)turmaaulaOrientacaocurricular.Rows[i]["Nivel"] > 1)
                {
                    if ((int)turmaaulaOrientacaocurricular.Rows[i]["Nivel"] > ((int)turmaaulaOrientacaocurricular.Rows[i - 1]["Nivel"] + 1))
                    {
                        turmaaulaOrientacaocurricular.Rows[i]["Nivel"] = (int)turmaaulaOrientacaocurricular.Rows[i - 1]["Nivel"] + 1;
                    }
                }
            }

            rptHabilidadesAula.DataSource = turmaaulaOrientacaocurricular;
            rptHabilidadesAula.DataBind();
            Nivel = 0;

        }

        /// <summary>
        /// Seta os campos na tela de acordo com a operação, e guarda a operação no campo
        /// hidden.
        /// </summary>
        /// <param name="operacao">Operação da tela</param>
        private void SetaTela(OperacaoAtual operacao)
        {
            hdnOperacao.Value = operacao.ToString();

            // Atualiza propriedades de componentes na tela, pois quando é alterado via js,
            // e entra no evento do servidor, algumas propriedades são perdidas.
            SetaDisplayCss(divAtividadeCasa, chkAtividadeCasa.Checked);

            string script = "";

            if (!string.IsNullOrEmpty(script))
            {
                ScriptManager.RegisterStartupScript(updPlanejamento, typeof(UpdatePanel), "ValidatorCombos", script, true);
            }
        }

        /// <summary>
        /// Seta o valor da propriedade display, para mostrar/esconder o controle, mantendo ele no html,
        /// para poder ser acessado via Javascript.
        /// </summary>
        /// <param name="controle">Controle a ser escondido/mostrado</param>
        /// <param name="exibe">Flag que informa se é para exibir ou esconder</param>
        private void SetaDisplayCss(Control controle, bool exibe)
        {
            if (controle.GetType() == typeof(HtmlGenericControl))
            {
                HtmlGenericControl div = controle as HtmlGenericControl;
                if (div != null)
                {
                    div.Style["display"] = exibe ? "" : "none";
                }
            }

            if (controle.GetType() == typeof(Button))
            {
                Button btn = controle as Button;
                if (btn != null)
                {
                    btn.Style["display"] = exibe ? "" : "none";
                }
            }

            if (controle.GetType() == typeof(TextBox))
            {
                TextBox txt = controle as TextBox;
                if (txt != null)
                {
                    txt.Style["display"] = exibe ? "" : "none";
                }
            }
        }

        /// <summary>
        /// Excluir uma aula.
        /// </summary>
        /// <param name="tud_id">Id da TurmaDisciplina.</param>
        /// <param name="tau_id">Id da aula.</param>
        private void ExcluirAula(long tud_id, int tau_id)
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                CLS_TurmaAula entity = new CLS_TurmaAula
                {
                    tud_id = tud_id,
                    tau_id = tau_id,
                    tau_situacao = (byte)CLS_TurmaNotaSituacao.Excluido,
                    tau_dataAlteracao = DateTime.Now,
                    tau_data = VS_tau_data,
                    tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id
                };

                int tje_id = string.IsNullOrEmpty(ddlTipoJustificativaExclusaoAula.SelectedValue) ? 0 : Convert.ToInt32(ddlTipoJustificativaExclusaoAula.SelectedValue);

                if (CLS_TurmaAulaBO.Delete(entity, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico,
                                           VS_PermissaoExcluirDiretor,
                                           tje_id,
                                           txtObservacaoExclusaoAula.Text,
                                           __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                           (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse,
                                           (byte)LOG_TurmaAula_Alteracao_Tipo.ExclusaoAula,
                                           VS_EntitiesControleTurma.turmaDisciplina.tud_tipo,
                                           VS_EntitiesControleTurma.formatoAvaliacao.fav_id,
                                           UCControleTurma1.VS_tur_id,
                                           __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "Aula | " + "tud_id: " + tud_id + "; tau_id: " + tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Aula excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    CarregarTela();
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScrollTop", "$(document).ready(function() { scrollToTop(); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a aula.", UtilBO.TipoMensagem.Erro);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScrollTop", "$(document).ready(function() { scrollToTop(); });", true);
            }
        }

        /// <summary>
        /// Carrega os dados de planejamento anual da turma selecionada.
        /// </summary>
        private void CarregarTurmaPeriodo()
        {
            //Como agora possuem visoes para esse modulo, esse codigo foi inserido pois o compartilhado nunca vai possuir
            //Seu proprio planejamento, ou seja, irá carregar o do titular.
            byte posicaoCompartilhado = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Compartilhado, ApplicationWEB.AppMinutosCacheLongo);
            byte posicaoSegundoTitular = ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo);
            byte posicaoAux = VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                            || posicaoCompartilhado == UCControleTurma1.VS_tdt_posicao
                            || posicaoSegundoTitular == UCControleTurma1.VS_tdt_posicao
                            ? ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo)
                            : UCControleTurma1.VS_tdt_posicao;

            //Carrega informações do diagnóstico inicial da turma, proposta de trabalho, e avaliação do trabalho
            DataTable dtPlanejamentoAnual = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorDisciplinaPermissaoDocente(VS_tud_id_Aula, posicaoAux) :
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplinaPeriodoCalendarioNulo(VS_tud_id_Aula, UCControleTurma1.VS_tdt_posicao);

            if ((TurmaDisciplinaTipo)VS_tud_tipo_Aula == TurmaDisciplinaTipo.Optativa ||
                (TurmaDisciplinaTipo)VS_tud_tipo_Aula == TurmaDisciplinaTipo.Eletiva ||
                (TurmaDisciplinaTipo)VS_tud_tipo_Aula == TurmaDisciplinaTipo.DocenteTurmaEletiva ||
                (TurmaDisciplinaTipo)VS_tud_tipo_Aula == TurmaDisciplinaTipo.DependeDisponibilidadeProfessorEletiva)
            {
                if (dtPlanejamentoAnual.Rows.Count > 0)
                {
                    CarregarHabilidades();
                }
            }
            else
            {
                CarregarHabilidades();
            }
            aPlanejamentoBimestre.InnerText = string.Format("Planejamento {0} ", UCNavegacaoTelaPeriodo.VS_cap_Descricao);
            aPlanejamentoBimestre.Visible = pnlPanoAulaPlanejamento.Visible = VS_tud_tipo_Aula != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada &&
                                                                              (VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            HabilitaControles(rptHabilidadesCOC.Controls, true);
        }

        /// <summary>
        /// O método carrega as orientações curriculares da turma.
        /// </summary>
        private void CarregarHabilidades()
        {
            DataTable dtPlanejamentoPeriodo;

            DataTable dtTodosPlanejamentosPeriodo =
                _ddlTurmaDisciplinaComponente.Visible && Tud_idComponente > -1 ?
                CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorDisciplinaCalendarioPermissaoDocente(UCNavegacaoTelaPeriodo.VS_cal_id, Tud_idComponente, UCControleTurma1.VS_tdt_posicao)
                : CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorDisciplinaCalendarioPermissaoDocente(UCNavegacaoTelaPeriodo.VS_cal_id, VS_tud_id_Aula, UCControleTurma1.VS_tdt_posicao);

            var x = from DataRow dr in dtTodosPlanejamentosPeriodo.Rows
                    where Convert.ToInt32(dr["tpc_id"]) == UCNavegacaoTelaPeriodo.VS_tpc_id
                    select dr;

            dtPlanejamentoPeriodo = x.Count() > 0 ? x.CopyToDataTable() : new DataTable();

            if (dtPlanejamentoPeriodo.Rows.Count > 0 && VS_EntitiesControleTurma.turma.tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno)
            {
                try
                {
                    DataTable dtOrientacaoCurricular =
                        CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplina(
                        VS_EntitiesControleTurma.curriculoPeriodo.cur_id, VS_EntitiesControleTurma.curriculoPeriodo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id, -1, -1, UCControleTurma1.VS_tur_id,
                        _ddlTurmaDisciplinaComponente.Visible && Tud_idComponente > -1 ? Tud_idComponente : VS_tud_id_Aula,
                        UCNavegacaoTelaPeriodo.VS_cal_id, UCControleTurma1.VS_tdt_posicao, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    //Carrega as orientacoes do periodo selecionado no diario de classe
                    VS_OrientacaoCurricular = dtOrientacaoCurricular.Rows.Count > 0 ?
                                              (from int tpc_id in dtPlanejamentoPeriodo.Rows.Cast<DataRow>().GroupBy(dr => Convert.ToInt32(dr["tpc_id"])).Select(g => g.Key)
                                               select new OrientacaoCurricular
                                               {
                                                   tpc_id = tpc_id
                                                   ,
                                                   dtOrientacaoCurricular = dtOrientacaoCurricular.Rows.Cast<DataRow>().Where(dr => Convert.ToInt32(dr["tpc_id"]) == tpc_id).CopyToDataTable()
                                               }).ToList() :
                                               new List<OrientacaoCurricular>();

                    #region Carrega as orientacoes dos periodos anteriores

                    VS_OrientacaoCurricular_PeriodosAnteriores =
                        (
                            from DataRow drTpc in dtTodosPlanejamentosPeriodo.Rows
                            where Convert.ToInt32(drTpc["tpc_ordem"]) < Convert.ToInt32(dtPlanejamentoPeriodo.Rows[0]["tpc_ordem"])
                            join DataRow dr in dtOrientacaoCurricular.Rows on Convert.ToInt32(drTpc["tpc_id"]) equals Convert.ToInt32(dr["tpc_id"])
                            where Convert.ToBoolean(dr["Planejado"]) || Convert.ToBoolean(dr["Trabalhado"])
                            select new OrientacaoCurricularPeriodosAnteriores
                            {
                                id = dr["Id"].ToString(),
                                planejado = Convert.ToBoolean(dr["Planejado"]),
                                trabalhado = Convert.ToBoolean(dr["Trabalhado"])
                            }
                        ).Distinct().ToList();

                    #endregion

                    long tudId = 0;
                    if (_ddlTurmaDisciplinaComponente.Visible)
                    {
                        tudId = Tud_idComponente;
                    }
                    else
                    {
                        tudId = VS_tud_id_Aula;
                    }

                    Int32 dis_id = TUR_TurmaDisciplinaRelDisciplinaBO.GetSelectBy_tud_id(tudId);
                    ACA_Disciplina disciplina = new ACA_Disciplina { dis_id = dis_id };
                    ACA_DisciplinaBO.GetEntity(disciplina);

                    DataTable dtNivel = ORC_NivelBO.SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(VS_EntitiesControleTurma.curriculoPeriodo.cur_id, VS_EntitiesControleTurma.curriculoPeriodo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id, UCNavegacaoTelaPeriodo.VS_cal_id, disciplina.tds_id);

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

                        NomeOrientacaoCurricularUltimoNivel =
                                                           (from DataRow dr in dtNivel.Rows
                                                            where Convert.ToByte(dr["nvl_situacao"]) == 1
                                                            orderby Convert.ToInt32(dr["nvl_ordem"]) descending
                                                            select dr["nvl_nome"].ToString()).ToList().First();

                        int index = NomeOrientacaoCurricular.LastIndexOf(',');
                        NomeOrientacaoCurricular = string.IsNullOrEmpty(NomeOrientacaoCurricular) ?
                        "Objetivos, conteúdos e " + GetGlobalResourceObject("Mensagens", "MSG_HABILIDADES_MIN") + " das orientações curriculares" :
                            NomeOrientacaoCurricular.Remove(index, 1).Insert(index, " e");
                    }

                    Tpc_ordemMax = dtTodosPlanejamentosPeriodo.Rows.Cast<DataRow>().Select(dr => Convert.ToInt32(dr["tpc_ordem"])).Max();

                    //será apenas um período por vez
                    CarregarPlanejamentoPeriodo(dtPlanejamentoPeriodo);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as orientações curriculares.", UtilBO.TipoMensagem.Erro);
                    btnSalvarPlanoAula.Visible = btnSalvarPlanoAulaCima.Visible = false;
                }
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairHabPopPlanoAula", "var exibeMensagemSair=" + btnSalvarPlanoAula.Visible.ToString().ToLower() + ";", true);

            // Carrega os dados do Grupamento Anterior
            //VS_crp_idAnterior = ACA_CurriculoPeriodoBO.VerificaPeriodoAnterior(VS_EntitiesControleTurma.curriculoPeriodo.cur_id, VS_EntitiesControleTurma.curriculoPeriodo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id);
            Nivel = 0;
            listHabilidadesComAulaPlanejada = new List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas>();

            if (VS_OrientacaoCurricular.Any())
            {
                //Carrega as habilidades com aulas planejadas
                listHabilidadesComAulaPlanejada = CLS_TurmaAulaOrientacaoCurricularBO.AulasPlanejadasSelecionaPorDisciplina
                    (ddlTurmaDisciplinaComponente.Visible ? Tud_idComponente : VS_tud_id_Aula);

                string ocr_ids = string.Join(";", (from DataRow dr in VS_OrientacaoCurricular.Where(item => item.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id).First().dtOrientacaoCurricular.Rows
                                                   let chave = dr["Chave"].ToString()
                                                   let idsChave = chave.Split(';')
                                                   let ocr_id = idsChave.Length > 1 ? idsChave[1] : ""
                                                   where !string.IsNullOrEmpty(ocr_id)
                                                   select ocr_id).Distinct().ToArray());

                dtOrientacaoNiveisAprendizado = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, null, ApplicationWEB.AppMinutosCacheLongo);
                dtNivelArendizadoCurriculo = ORC_NivelAprendizadoBO.GetSelectNiveisAprendizadoAtivos(VS_EntitiesControleTurma.curriculoPeriodo.cur_id, VS_EntitiesControleTurma.curriculoPeriodo.crr_id, VS_EntitiesControleTurma.curriculoPeriodo.crp_id, ApplicationWEB.AppMinutosCacheLongo);

                rptHabilidadesCOC.DataSource = VS_OrientacaoCurricular.Where(item => item.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id).First().dtOrientacaoCurricular;
                rptHabilidadesCOC.DataBind();

            }

            if ((spanOrientacao != null) && (!string.IsNullOrEmpty(NomeOrientacaoCurricularUltimoNivel)))
            {
                spanOrientacao.InnerText = NomeOrientacaoCurricularUltimoNivel;
            }

            Nivel = 0;
        }

        /// <summary>
        /// Carrega os dados de planejamento para o período selecionado
        /// </summary>
        /// <param name="dt">Dados do planejamento do periodo</param>
        private void CarregarPlanejamentoPeriodo(DataTable dt)
        {
            int tpc_ordem = Convert.ToInt32(dt.Rows[0]["tpc_ordem"].ToString());
            bool ultimoPeriodo = tpc_ordem == Tpc_ordemMax;

            lblDiagnosticoCOC.Text = (string)GetGlobalResourceObject("Mensagens", "MSG_AVALIACAOBIMESTRE") + UCNavegacaoTelaPeriodo.VS_cap_Descricao;

            lblPlanejamentoCOC.Text = ultimoPeriodo ?
                    (string)GetGlobalResourceObject("Mensagens", "MSG_REPLANEJAMENTOBIMESTREFINAL") :
                    (string)GetGlobalResourceObject("Mensagens", "MSG_REPLANEJAMENTOBIMESTRE") + UCNavegacaoTelaPeriodo.VS_cap_Descricao + " para o " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " seguinte";

            //lblPlanejamentoCOC.Text = string.Format("Replanejamento do {0} para o {1} seguinte", VS_cap_Descricao, GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id));

            lblRecursosCOC.Text = (string)GetGlobalResourceObject("Mensagens", "MSG_RECURSOSBIMESTRE") + UCNavegacaoTelaPeriodo.VS_cap_Descricao;

            byte tipoLancamento = VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoLancamentoFrequencia;
            byte calculoQtdeAulasDadas = VS_EntitiesControleTurma.formatoAvaliacao.fav_calculoQtdeAulasDadas;

            //txtQuantidadeAulas.Text = CLS_AlunoAvaliacaoTurmaBO.CalculaQtdeTemposAula(VS_tur_id, VS_tpc_id, VS_tud_id, tipoLancamento, calculoQtdeAulasDadas).ToString();

            txtDiagnosticoCOC.Text = dt.Rows[0]["tdp_diagnostico"].ToString();
            txtPlanejamentoCOC.Text = dt.Rows[0]["tdp_planejamento"].ToString();
            txtRecursosCOC.Text = dt.Rows[0]["tdp_recursos"].ToString();
            txtIntervencoesPedagogicasCOC.Text = dt.Rows[0]["tdp_intervencoesPedagogicas"].ToString();
            txtRegistroIntervencoesCOC.Text = dt.Rows[0]["tdp_registroIntervencoes"].ToString();
        }

        /// <summary>
        /// Limpa campos da pop-up plano diário para disciplinas componentes regência.
        /// </summary>
        protected void LimparCamposPlanoAula()
        {
            txtPlanoAula.Text = txtRegistroAula.Text = txtAtividadeCasa.Text = txtSinteseAula.Text = string.Empty;
            chkAtividadeCasa.Checked = false;
            SetaDisplayCss(divAtividadeCasa, false);
            SetaDisplayCss(txtOutroRecurso, false);
            for (int i = 0; i < chkRecursos.Items.Count; i++)
                chkRecursos.Items[i].Selected = false;
            txtOutroRecurso.Text = string.Empty;
            divObjetosAprendizagem.Visible = false;
        }

        /// <summary>
        /// Carrega os dados do plano de aula selecionado para a disciplina componente da regência.
        /// </summary>
        /// <param name="entity">Entidade da turma aula regencia</param>
        private void CarregarPlanoAulaRegencia(CLS_TurmaAulaRegencia entity)
        {
            try
            {
                CLS_TurmaAula entityAula = new CLS_TurmaAula
                {
                    tud_id = VS_tud_id_Aula
                     ,
                    tau_id = VS_tau_id
                };
                CLS_TurmaAulaBO.GetEntity(entityAula);

                if (VisibilidadeRegencia(ddlTurmaDisciplina))
                {
                    _ddlTurmaDisciplinaComponente.Visible = _lblTurmaDisciplinaComponente.Visible = true;

                    entity = new CLS_TurmaAulaRegencia
                    {
                        tud_id = VS_tud_id_Aula
                         ,
                        tau_id = VS_tau_id
                         ,
                        tud_idFilho = Convert.ToInt64(_ddlTurmaDisciplinaComponente.SelectedValue.Split(';')[1])
                    };
                    CLS_TurmaAulaRegenciaBO.GetEntity(entity);

                    if (!entity.IsNew)
                    {
                        VS_tau_data = entity.tuf_data;
                        VS_dataAlteracaoAula = entity.tuf_dataAlteracao;

                        //CarregarAlunoAnotacao();
                        SetaTela(OperacaoAtual.AlterandoAula);
                        CarregaAulaRecursoRegencia(entity.tud_idFilho);

                        txtRegistroAula.Text = entity.tuf_diarioClasse;
                        txtSinteseAula.Text = entity.tuf_sintese;
                        txtPlanoAula.Text = entity.tuf_planoAula;
                        chkAtividadeCasa.Checked = !String.IsNullOrEmpty(entity.tuf_atividadeCasa);
                        txtAtividadeCasa.Text = chkAtividadeCasa.Checked
                                                    ? entity.tuf_atividadeCasa
                                                    : "";

                        txtAvaliacao.Text = entity.tuf_diarioClasse;
                    }
                }

                bool esconderAtividadeCasa = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ESCONDER_ATIVIDADE_PRACASA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                chkAtividadeCasa.Visible = esconderAtividadeCasa;
                divAtividadeCasa.Visible = esconderAtividadeCasa;

                SetaDisplayCss(divAtividadeCasa, chkAtividadeCasa.Checked);

                aPlanoAula.InnerText = string.Format("Plano de aula {0} - Regência", entityAula.tau_data.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar plano de aula.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os recursos utilizados para disciplina componente da regência.
        /// </summary>
        /// <param name="tud_idFilho">Id da turma disciplina ligada à regência</param>
        private void CarregaAulaRecursoRegencia(long tud_idFilho)
        {
            for (int i = 0; i < chkRecursos.Items.Count; i++)
                chkRecursos.Items[i].Selected = false;
            txtOutroRecurso.Text = string.Empty;

            List<CLS_TurmaAulaRecursoRegencia> listaBanco = CLS_TurmaAulaRecursoRegenciaBO.GetSelectBy_Turma_Aula_DisciplinaComponente(VS_tud_id_Aula, VS_tau_id, tud_idFilho, ApplicationWEB.AppMinutosCacheLongo);
            foreach (CLS_TurmaAulaRecursoRegencia recurso in listaBanco)
            {
                chkRecursos.Items.FindByValue(Convert.ToString(recurso.rsa_id)).Selected = true;
                if (recurso.rsa_id == 0)
                {
                    SetaDisplayCss(txtOutroRecurso, true);
                    txtOutroRecurso.Text = String.IsNullOrEmpty(recurso.trr_observacao) ? "" : recurso.trr_observacao;
                }
            }

            SetaDisplayCss(txtOutroRecurso, chkRecursos.Items[chkRecursos.Items.Count - 1].Selected);
        }

        /// <summary>
        /// Carrega os dados da aula selecionada
        /// </summary>
        /// <param name="entity">Entidade da turma aula</param>
        private void CarregarAula(CLS_TurmaAula entity)
        {
            try
            {
                if (entity.tau_id <= 0)
                {
                    entity = new CLS_TurmaAula { tud_id = VS_tud_id_Aula, tau_id = VS_tau_id };
                    CLS_TurmaAulaBO.GetEntity(entity);
                }

                VS_tau_data = entity.tau_data;

                SetaTela(OperacaoAtual.AlterandoAula);

                CarregaAulaRecurso();

                txtPlanoAula.Text = entity.tau_planoAula;

                chkAtividadeCasa.Checked = entity.tau_checadoAtividadeCasa || !String.IsNullOrEmpty(entity.tau_atividadeCasa);

                bool esconderAtividadeCasa = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ESCONDER_ATIVIDADE_PRACASA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                chkAtividadeCasa.Visible = esconderAtividadeCasa;
                divAtividadeCasa.Visible = esconderAtividadeCasa;

                txtAtividadeCasa.Text = chkAtividadeCasa.Checked
                                            ? entity.tau_atividadeCasa
                                            : "";

                txtRegistroAula.Text = entity.tau_diarioClasse;
                txtSinteseAula.Text = entity.tau_sintese;
                VS_dataAlteracaoAula = entity.tau_dataAlteracao;

                int diaSemana = Convert.ToInt32(entity.tau_data.DayOfWeek);
                string[] diasSemana = { "Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado" };

                string nomeDocente = RetornaDocentePorPosicao(entity.tdt_posicao);

                string strDataAula = string.Format("{0} - {1} - {2}", entity.tau_data.ToString("dd/MM/yyyy"), diasSemana[diaSemana], nomeDocente);

                lblDataAula.Text = "<b>Aula:</b> " + strDataAula;

                if ((VS_tdt_posicaoEdicao > 0 && !VS_ltPermissaoAula.Any(p => p.tdt_posicaoPermissao == entity.tdt_posicao && p.pdc_permissaoEdicao))
                    || (!PeriodoValidoLancamentoNotasFrequencias && !periodoFuturo))
                {
                    HabilitaControles(divDados.Controls, false);
                    SetaDisplayCss(btnSalvarAula, false);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairSalvarAula", "var exibeMensagemSair=false;", true);
                }
                else
                {
                    HabilitaControles(divDados.Controls, true);
                    SetaDisplayCss(btnSalvarAula, true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairSalvarAula", "var exibeMensagemSair=true;", true);
                }

                SetaDisplayCss(divAtividadeCasa, chkAtividadeCasa.Checked);

                if (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                    VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Regencia)
                {
                    divTurmaDisciplinaComponentePlanejamento.Visible = true;
                    _ddlTurmaDisciplinaComponente.Visible = _lblTurmaDisciplinaComponente.Visible = true;
                    List<CLS_TurmaAulaPlanoDisciplina> lstComponentesRegencia = CLS_TurmaAulaPlanoDisciplinaBO.SelectBy_aulaDisciplina(VS_tud_id_Aula, VS_tau_id);
                    string[] ids;

                    foreach (ListItem item in cblComponentesRegencia.Items)
                    {
                        ids = item.Value.Split(';');
                        if (ids.Length > 1 && lstComponentesRegencia.Exists(p => p.tud_idPlano == Convert.ToInt64(ids[1])))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                }

                aPlanoAula.InnerText = string.Format("Plano de aula {0} - {1} ", entityAula.tau_data.ToString("dd/MM/yyyy"), VS_tud_nome_Aula);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a aula.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Retorna o nome do docente de acordo com a posição passada.
        /// </summary>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <returns>Nome do docente.</returns>
        private string RetornaDocentePorPosicao(byte tdt_posicao)
        {
            return string.Format("Docente {0}", tdt_posicao);
        }

        /// <summary>
        /// Carrega os recursos utilizados na aula.
        /// </summary>
        private void CarregaAulaRecurso()
        {
            if (VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                for (int i = 0; i < chkRecursos.Items.Count; i++)
                    chkRecursos.Items[i].Selected = false;
                txtOutroRecurso.Text = string.Empty;

                List<CLS_TurmaAulaRecurso> listaBanco = CLS_TurmaAulaRecursoBO.GetSelectBy_Turma_Aula(VS_tud_id_Aula, VS_tau_id, ApplicationWEB.AppMinutosCacheLongo);
                foreach (CLS_TurmaAulaRecurso recurso in listaBanco)
                {
                    chkRecursos.Items.FindByValue(Convert.ToString(recurso.rsa_id)).Selected = true;
                    if (recurso.rsa_id == 0)
                    {
                        SetaDisplayCss(txtOutroRecurso, true);
                        txtOutroRecurso.Text = String.IsNullOrEmpty(recurso.tar_observacao) ? "" : recurso.tar_observacao;
                    }
                }

                SetaDisplayCss(txtOutroRecurso, chkRecursos.Items[chkRecursos.Items.Count - 1].Selected);
            }
        }

        private void CarregarObjetosAprendizagem(DataTable dtCampos)
        {
            DataTable dtAssociados = CLS_ObjetoAprendizagemTurmaAulaBO.SelecionaObjTudTau(VS_tud_id_Aula, VS_tau_id);

            rptCampos.DataSource = dtCampos.AsEnumerable().OrderBy(r => r["oap_descricao"])
                                   .Select(p => new { oap_id = p["oap_id"], oap_descricao = p["oap_descricao"] });
            rptCampos.DataBind();

            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

                if (ckbCampo != null && hdnId != null)
                {
                    ckbCampo.Checked = dtAssociados.AsEnumerable().Any(r => Convert.ToInt32(r["oap_id"]) == Convert.ToInt32(hdnId.Value));
                }
            }
        }

        /// <summary>
        /// Editar uma aula.
        /// </summary>
        /// <param name="tud_id">Id da TurmaDisciplina.</param>
        /// <param name="tpc_id">ID do tipo periodo calendario</param>
        /// <param name="tau_id">Id da aula.</param>
        private void EditarAula(long tud_id, int tpc_id, int tau_id)
        {
            try
            {
                ExibeQuantidadeAulas();

                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                CLS_TurmaAula entity = new CLS_TurmaAula { tud_id = tud_id, tau_id = tau_id };
                CLS_TurmaAulaBO.GetEntity(entity);

                VS_DataAlteracaoAula_Validacao = entity.tau_dataAlteracao;

                if (!VS_PeriodoEfetivado)
                {
                    txtDataAula.Text = entity.tau_data.ToString();
                    txtQtdeAulas.Text = entity.tau_numeroAulas.ToString();
                    chkReposicao.Checked = entity.tau_reposicao;
                    hdfIsNewAula.Value = "false";

                    //Se for regência e o tipo de apuração de frequência for por tempos de aula, exibe o campo de quantidade de aulas.

                    // Se o professor que criou a aula for de projeto, exibe o campo de quantidade de aulas.
                    if (((VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Regencia) && // Verifica se é regencia.
                            (entity.tdt_posicao == (byte)EnumTipoDocente.Projeto)) || RegenciaETemposAula)
                    {
                        txtQtdeAulas.Visible = lblQtdeAulas.Visible = true;
                    }

                    if ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao))
                        || (!PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao))
                    {
                        SetaDisplayCss(btnSalvarAula, true);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairEditarAula", "var exibeMensagemSair=true;", true);
                    }
                    else
                    {
                        SetaDisplayCss(btnSalvarAula, false);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairEditarAula", "var exibeMensagemSair=false;", true);
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "EditarAulas", "$('#divCadastroAula').dialog('open');", true);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar editar a aula.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updCadastroAula.Update();
            }
        }

        /// <summary>
        /// O método salva uma nova aula para a turma disciplina e período.
        /// </summary>
        private bool SalvarAula()
        {
            DateTime dtAula;
            try
            {
                if (!DateTime.TryParse(txtDataAula.Text, out dtAula))
                {
                    if (String.IsNullOrEmpty(txtDataAula.Text.Trim()))
                    {
                        lblMessage3.Text = UtilBO.GetErroMessage("Data da aula é obrigatório.", UtilBO.TipoMensagem.Alerta);
                    }
                    else
                    {
                        lblMessage3.Text = UtilBO.GetErroMessage("Data da aula é inválida.", UtilBO.TipoMensagem.Alerta);
                    }
                    return false;
                }
                else if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                        UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                        dtAula > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    lblMessage3.Text = UtilBO.GetErroMessage("Data da aula é maior que a data de encerramento da turma.", UtilBO.TipoMensagem.Alerta);
                    return false;
                }

                CLS_TurmaAula entity;
                if (hdfIsNewAula.Value == "true")
                {
                    entity = new CLS_TurmaAula
                    {
                        tau_id = -1,
                        tud_id = VS_tud_id_Aula,
                        tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                        tau_data = string.IsNullOrEmpty(txtDataAula.Text) ? new DateTime() : Convert.ToDateTime(txtDataAula.Text),

                        tau_numeroAulas = 
                        
                        (DisciplinaPrincipal || DisciplinaRegencia) && !RegenciaETemposAula ? 1 :
                            (string.IsNullOrEmpty(txtQtdeAulas.Text) ? 0 :
                                Convert.ToInt32(txtQtdeAulas.Text)),


                        tdt_posicao = UCControleTurma1.VS_tdt_posicao,
                        tau_reposicao = chkReposicao.Visible && chkReposicao.Checked,
                        tau_dataAlteracao = VS_DataAlteracaoAula_Validacao,
                        IsNew = true,
                        tur_id = UCControleTurma1.VS_tur_id,
                        usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        tud_tipo = VS_EntitiesControleTurma.turmaDisciplina.tud_tipo
                    };
                }
                else
                {
                    entity = new CLS_TurmaAula
                    {
                        tud_id = VS_tud_id_Aula
                        ,
                        tau_id = VS_tau_id
                    };

                    CLS_TurmaAulaBO.GetEntity(entity);

                    entity.tau_data = string.IsNullOrEmpty(txtDataAula.Text) ? new DateTime() : Convert.ToDateTime(txtDataAula.Text);
                    entity.tau_numeroAulas = ((DisciplinaPrincipal || DisciplinaRegencia) &&
                                              !((VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Regencia)
                                                && (entity.tdt_posicao == (byte)EnumTipoDocente.Projeto)))
                                                    ? 1 : (string.IsNullOrEmpty(txtQtdeAulas.Text) ? 0 : Convert.ToInt32(txtQtdeAulas.Text));
                    entity.tdt_posicao = UCControleTurma1.VS_tdt_posicao;
                    entity.tau_reposicao = chkReposicao.Visible && chkReposicao.Checked;
                    entity.tau_dataAlteracao = VS_DataAlteracaoAula_Validacao;
                    entity.IsNew = false;
                    entity.tur_id = UCControleTurma1.VS_tur_id;
                    entity.tud_tipo = VS_EntitiesControleTurma.turmaDisciplina.tud_tipo;
                }

                string mensagemInfo = "";

                if (CLS_TurmaAulaBO.Save(entity, null, out mensagemInfo, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                         VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico
                                         , UCControleTurma1.AtribuicoesVerificarVigencia
                                         , VS_EntitiesControleTurma.turma, VS_EntitiesControleTurma.turmaDisciplina,
                                         VS_EntitiesControleTurma.calendarioAnual, UCNavegacaoTelaPeriodo.cap_dataInicio,
                                         UCNavegacaoTelaPeriodo.cap_dataFim, VS_turmaDisciplinaRelacionada,
                                         __SessionWEB.__UsuarioWEB.Usuario.usu_id, (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse,
                                         (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoAula
                                         , VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Aula | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + entity.tau_id);
                    if (string.IsNullOrEmpty(mensagemInfo))
                        lblMessage.Text = UtilBO.GetErroMessage("Aula salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    else
                        lblMessage.Text = UtilBO.GetErroMessage(mensagemInfo, UtilBO.TipoMensagem.Informacao);
                    CarregarTela();
                    return true;
                }
                lblMessage3.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a aula.", UtilBO.TipoMensagem.Erro);
                return false;
            }
            catch (EditarAula_ValidationException ex)
            {
                lblMessage3.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                return false;
            }
            catch (ValidationException ex)
            {
                lblMessage3.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                return false;
            }
            catch (ArgumentException ex)
            {
                lblMessage3.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage3.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a aula.", UtilBO.TipoMensagem.Erro);
                return false;
            }
            finally
            {
                updCadastroAula.Update();
            }
        }

        /// <summary>
        /// Salva os lançamentos de frequência para os alunos da aula selecionada.
        /// </summary>
        private void SalvarFrequencia()
        {
            try
            {
                bool permiteEditar = (VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                        && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao)) 
                                        || (!PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao);
                if (permiteEditar)
                {
                    permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                }

                permiteEditar &= VS_Periodo_Aberto;

                if (permiteEditar)
                {

                    if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                            || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                        UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                        VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                    {
                        throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                    }

                    List<CLS_TurmaAulaAluno> listTurmaAulaAluno = new List<CLS_TurmaAulaAluno>();
                    List<CLS_TurmaAula> listTurmaAula = new List<CLS_TurmaAula>();

                    RepeaterItem header;
                    Repeater rptDiarioAulas;

                    Func<RepeaterItem, CLS_TurmaAula> RetornaAula = (RepeaterItem item) =>
                    {
                        CheckBox chkEfetivado = (CheckBox)item.FindControl("chkEfetivado");
                        int tau_id = Convert.ToInt32(((Label)item.FindControl("lbltau_id")).Text);

                        CLS_TurmaAula ent = new CLS_TurmaAula
                        {
                            tud_id = VS_tud_id_Aula,
                            tau_id = tau_id
                        };
                        CLS_TurmaAulaBO.GetEntity(ent);

                        if (!ent.IsNew && ent.tau_dataAlteracao > VS_Data_Diario_TurmaAula)
                        {
                            throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.Validacao_Data_TurmaFrequencia").ToString());
                        }


                        ent.tau_efetivado = chkEfetivado.Checked;
                        ent.tau_statusFrequencia = (byte)(chkEfetivado.Checked ? CLS_TurmaAulaStatusFrequencia.Efetivada
                                                                               : CLS_TurmaAulaStatusFrequencia.Preenchida);
                        ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                        ent.tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id;

                        return ent;
                    };

                    if (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia)
                    {
                        header = (RepeaterItem)rptDiarioAlunosFrequenciaTerriorio.Controls[0];
                        rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");

                        if (rptDiarioAulas != null)
                            foreach (RepeaterItem aula in rptDiarioAulas.Items)
                            {
                                listTurmaAula.Add(RetornaAula(aula));
                            }

                        foreach (RepeaterItem itemAluno in rptDiarioAlunosFrequenciaTerriorio.Items)
                        {
                            rptDiarioAulas = (Repeater)itemAluno.FindControl("rptDiarioAulas");
                            long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                            int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);

                            foreach (RepeaterItem itemAula in rptDiarioAulas.Items)
                            {
                                Repeater rptAulasTerritorio = (Repeater)itemAula.FindControl("rptAulasTerritorio");

                                foreach (RepeaterItem itemTerritorio in rptAulasTerritorio.Items)
                                {
                                    CheckBoxList cblFrequencia = (CheckBoxList)itemTerritorio.FindControl("cblFrequencia");

                                    int frequencia = 0;
                                    string bitmap = "";
                                    for (int i = 0; i < cblFrequencia.Items.Count; i++)
                                    {
                                        frequencia += cblFrequencia.Items[i].Selected ? 1 : 0;
                                        bitmap += cblFrequencia.Items[i].Selected ? "1" : "0";
                                    }

                                    string[] ids = cblFrequencia.Items[0].Value.Split(';');
                                    long tud_id = Convert.ToInt64(ids[0]);
                                    int tau_id = Convert.ToInt32(ids[1]);
                                    int mtd_id = Convert.ToInt32(ids[2]);

                                    CLS_TurmaAulaAluno ent = new CLS_TurmaAulaAluno
                                    {
                                        tud_id = tud_id
                                        ,
                                        tau_id = tau_id
                                        ,
                                        alu_id = alu_id
                                        ,
                                        mtu_id = mtu_id
                                        ,
                                        mtd_id = mtd_id
                                        ,
                                        taa_frequencia = frequencia
                                        ,
                                        taa_situacao = 1
                                        ,
                                        taa_frequenciaBitMap = bitmap
                                    };

                                    listTurmaAulaAluno.Add(ent);
                                }
                            }
                        }
                    }
                    else
                    {
                        header = (RepeaterItem)rptDiarioAlunosFrequencia.Controls[0];
                        rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");

                        // Adiciona itens na lista de TurmaAula - só pra alterar os campos: tau_efetivado e tau_statusFrequencia.
                        if (rptDiarioAulas != null)
                            foreach (RepeaterItem itemAtividade in rptDiarioAulas.Items)
                            {
                                listTurmaAula.Add(RetornaAula(itemAtividade));
                            }

                        foreach (RepeaterItem itemAluno in rptDiarioAlunosFrequencia.Items)
                        {
                            rptDiarioAulas = (Repeater)itemAluno.FindControl("rptDiarioAulas");
                            Int64 alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                            Int32 mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                            Int32 mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);

                            // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                            foreach (RepeaterItem itemAtividadeAluno in rptDiarioAulas.Items)
                            {
                                int tau_id = Convert.ToInt32(((Label)itemAtividadeAluno.FindControl("lbltau_id")).Text);
                                CheckBoxList cblFrequencia = (CheckBoxList)itemAtividadeAluno.FindControl("cblFrequencia");
                                int frequencia = 0;
                                string bitmap = "";
                                for (int i = 0; i < cblFrequencia.Items.Count; i++)
                                {
                                    frequencia += cblFrequencia.Items[i].Selected ? 1 : 0;
                                    bitmap += cblFrequencia.Items[i].Selected ? "1" : "0";
                                }

                                CLS_TurmaAulaAluno ent = new CLS_TurmaAulaAluno
                                {
                                    tud_id = VS_tud_id_Aula
                                    ,
                                    tau_id = tau_id
                                    ,
                                    alu_id = alu_id
                                    ,
                                    mtu_id = mtu_id
                                    ,
                                    mtd_id = mtd_id
                                    ,
                                    taa_frequencia = frequencia
                                    ,
                                    taa_situacao = 1
                                    ,
                                    taa_frequenciaBitMap = bitmap
                                };

                                listTurmaAulaAluno.Add(ent);
                            }
                        }
                    }

                    if (CLS_TurmaAulaAlunoBO.Save(listTurmaAulaAluno, listTurmaAula, UCControleTurma1.VS_tur_id, VS_tud_id_Aula,
                                              UCControleTurma1.VS_tdt_posicao, VS_EntitiesControleTurma.turma,
                                              VS_EntitiesControleTurma.formatoAvaliacao, VS_EntitiesControleTurma.curriculoPeriodo,
                                              __SessionWEB.__UsuarioWEB.Usuario.usu_id, (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse,
                                              (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoFreq, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        // Atualizar dados da aula.
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Lançamento de frequência | " +
                                                                                "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                                " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + VS_tau_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Lançamento de frequências salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharLancamentoFrequencia", "var exibirMensagemConfirmacao=false;$('#divLancamentoFrequencia').dialog('close');", true);

                        bool efetivado = listTurmaAula.Exists(p => p.tau_statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Efetivada);
                        bool preenchida = !efetivado && listTurmaAula.Exists(p => p.tau_statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Preenchida);

                        updFrequencia.Update();

                        Image imgFrequenciaSituacaoEfetivada = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgFrequenciaSituacaoEfetivada");
                        if (imgFrequenciaSituacaoEfetivada != null)
                            imgFrequenciaSituacaoEfetivada.Visible = efetivado;

                        Image imgFrequenciaSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgFrequenciaSituacao");
                        if (imgFrequenciaSituacao != null)
                            imgFrequenciaSituacao.Visible = preenchida;
                    }
                    else
                    {
                        lblMessageFrequencia.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a frequência dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMessageFrequencia.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (DuplicateNameException ex)
            {
                lblMessageFrequencia.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (ArgumentException ex)
            {
                lblMessageFrequencia.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageFrequencia.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a frequência dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            }
        }

        /// <summary>
        /// Adiciona uma nova linha ao grid de anotações do aluno
        /// </summary>
        private void AdicionaLinhaGridAnotacao()
        {
            // criando o datatable
            DataTable dt = RetornaDadosGridAnotacao();

            if (dt.AsEnumerable().LastOrDefault().Field<object>("alu_mtu_mtd_id").ToString().Equals("-1;-1;-1") ||
                (
                    dt.AsEnumerable().LastOrDefault().Field<object>("taa_anotacao").ToString().Equals("") // Não preencheu o campo texto da anotacao
                    && dt.AsEnumerable().LastOrDefault().Field<object>("tia_ids").ToString().Equals("") // Não preencheu o campo de tipos de anotacao
               ))
            {
                return;
            }

            // adiciona nova linha do grid
            DataRow dr = dt.NewRow();
            dr["alu_mtu_mtd_id"] = "-1;-1;-1";
            dt.Rows.Add(dr);

            string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                    string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                    string.Empty;

            dtAlunosAnotacoes =
                      MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina

                           (
                                VS_tud_id_Aula,
                                UCNavegacaoTelaPeriodo.VS_tpc_id,
                                VS_tipoDocente,
                                false,
                                UCNavegacaoTelaPeriodo.cap_dataInicio,
                                UCNavegacaoTelaPeriodo.cap_dataFim,
                                ApplicationWEB.AppMinutosCacheMedio,
                                tur_ids
                           );

            // mostra nova linha
            grvAnotacaoAluno.DataSource = dt;
            grvAnotacaoAluno.DataBind();

            // Mostra botões de adicionar e cancelar, enconde o de excluir.
            grvAnotacaoAluno.Rows[grvAnotacaoAluno.Rows.Count - 1].FindControl("btnAdicionar").Visible = true;
            grvAnotacaoAluno.Rows[grvAnotacaoAluno.Rows.Count - 1].FindControl("btnCancelar").Visible = true;
            grvAnotacaoAluno.Rows[grvAnotacaoAluno.Rows.Count - 1].FindControl("ddlAnotacaoAluno").Focus();
        }

        /// <summary>
        /// Verifica se já existe uma anotação com o mesmo aluno
        /// </summary>
        private bool ValidarAlunoAnotacao()
        {
            try
            {
                if (grvAnotacaoAluno.Rows.Count > 0)
                {
                    Label lblIdsAnotAluAnterior = (Label)grvAnotacaoAluno.Rows[grvAnotacaoAluno.Rows.Count - 1].FindControl("lblIdsAnotAlu");

                    if (!string.IsNullOrEmpty(lblIdsAnotAluAnterior.Text))
                    {
                        foreach (GridViewRow linha in grvAnotacaoAluno.Rows)
                        {
                            Label lblIdsAnotAlu = (Label)linha.FindControl("lblIdsAnotAlu");

                            if (lblIdsAnotAluAnterior.Text == lblIdsAnotAlu.Text &&
                                linha.RowIndex != grvAnotacaoAluno.Rows.Count - 1)
                            {
                                lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Só é permitido cadastrar uma anotação por aluno.", UtilBO.TipoMensagem.Alerta, "margin: 10px;");
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar validar a anotação para o aluno.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                return false;
            }
        }

        /// <summary>
        /// Retorna os dados que estão no grid de anotações.
        /// </summary>
        /// <returns>Data table com as anotações.</returns>
        private DataTable RetornaDadosGridAnotacao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("alu_mtu_mtd_id");
            dt.Columns.Add("tia_ids");
            dt.Columns.Add("taa_anotacao");
            dt.Columns.Add("pes_nome");
            dt.Columns.Add("nomeUsuAlteracao");
            dt.Columns.Add("dataAlteracao");

            foreach (GridViewRow linha in grvAnotacaoAluno.Rows)
            {
                TextBox txtAnotacao = (TextBox)linha.FindControl("txtAnotacao");
                Label lblIdsAnotAlu = (Label)linha.FindControl("lblIdsAnotAlu");
                Label lblNomeAluno = (Label)linha.FindControl("lblNomeAluno");

                DropDownList ddlAnotacaoAluno = (DropDownList)linha.FindControl("ddlAnotacaoAluno");

                if (string.IsNullOrEmpty(lblNomeAluno.Text) || ddlAnotacaoAluno.Visible)
                {
                    lblNomeAluno.Text = ddlAnotacaoAluno.SelectedItem.Text;
                }

                HiddenField hdnAlteracaoAnotacoes = (HiddenField)linha.FindControl("hdnUsuAleracao");

                CheckBoxList cblAnotacoesPredefinidas = (CheckBoxList)linha.FindControl("cblAnotacoesPredefinidas");
                string sTia_ids = string.Empty;
                foreach (ListItem l in cblAnotacoesPredefinidas.Items)
                    if (l.Selected)
                        sTia_ids += (!string.IsNullOrEmpty(sTia_ids) ? ";" : string.Empty) + l.Value;

                DataRow drLinha = dt.NewRow();
                drLinha["alu_mtu_mtd_id"] = lblIdsAnotAlu.Text;
                drLinha["tia_ids"] = sTia_ids;
                drLinha["taa_anotacao"] = txtAnotacao.Text;
                drLinha["pes_nome"] = lblNomeAluno.Text;
                drLinha["nomeUsuAlteracao"] = hdnAlteracaoAnotacoes.Value;

                drLinha["dataAlteracao"] = DateTime.Now;

                dt.Rows.Add(drLinha);
            }

            return dt;
        }

        /// <summary>
        /// Salva as anotações dos alunos para a aula
        /// </summary>
        /// <param name="fecharPopUp">Informa se irá fechar o pop-up após salvar</param>
        private void SalvarAnotacoesAluno(bool fecharPopUp)
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                List<CLS_TurmaAulaAluno> listTurmaAulaAluno =
                    (from GridViewRow linha in grvAnotacaoAluno.Rows
                     let txtAnotacao = (TextBox)linha.FindControl("txtAnotacao")
                     let lblIdsAnotAlu = (Label)linha.FindControl("lblIdsAnotAlu")
                     let alu_id = Convert.ToInt64(lblIdsAnotAlu.Text.Split(';')[0])
                     let mtu_id = Convert.ToInt32(lblIdsAnotAlu.Text.Split(';')[1])
                     let mtd_id = Convert.ToInt32(lblIdsAnotAlu.Text.Split(';')[2])
                     where alu_id > 0
                     select new CLS_TurmaAulaAluno
                     {
                         tud_id = VS_tud_id_Aula
                         ,
                         tau_id = VS_tau_id
                         ,
                         alu_id = alu_id
                         ,
                         mtu_id = mtu_id
                         ,
                         mtd_id = mtd_id
                         ,
                         taa_anotacao = txtAnotacao.Text.Trim()
                         ,
                         taa_situacao = 1
                         ,
                         usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                     }).ToList();

                List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaAlunoTipoAnotacao
                   = (from GridViewRow linha in grvAnotacaoAluno.Rows
                      let txtAnotacao = (TextBox)linha.FindControl("txtAnotacao")
                      let lblIdsAnotAlu = (Label)linha.FindControl("lblIdsAnotAlu")
                      let alu_id = Convert.ToInt64(lblIdsAnotAlu.Text.Split(';')[0])
                      let mtu_id = Convert.ToInt32(lblIdsAnotAlu.Text.Split(';')[1])
                      let mtd_id = Convert.ToInt32(lblIdsAnotAlu.Text.Split(';')[2])
                      where alu_id > 0
                      from ListItem tia in ((CheckBoxList)linha.FindControl("cblAnotacoesPredefinidas")).Items
                      let tia_id = Convert.ToInt32(tia.Value)
                      where tia.Selected
                      select new CLS_TurmaAulaAlunoTipoAnotacao
                      {
                          tud_id = VS_tud_id_Aula
                          ,
                          tau_id = VS_tau_id
                          ,
                          alu_id = alu_id
                          ,
                          mtu_id = mtu_id
                          ,
                          mtd_id = mtd_id
                          ,
                          tia_id = tia_id
                      }).ToList();

                var x = from CLS_TurmaAulaAluno turmaAulaAluno in listTurmaAulaAluno
                        group turmaAulaAluno by new { turmaAulaAluno.alu_id, turmaAulaAluno.mtu_id, turmaAulaAluno.mtd_id }
                            into grupo
                        select new
                        {
                            grupo.Key.alu_id,
                            grupo.Key.mtu_id,
                            grupo.Key.mtd_id,
                            anotacoes = grupo.ToList()
                        };

                if (x.ToList().Exists(p => p.anotacoes.Count > 1))
                {
                    throw new ValidationException("Não é permitido inserir mais de uma anotação por aluno.");
                }

                CLS_TurmaAula entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
                {
                    tud_id = VS_tud_id_Aula,
                    tau_id = VS_tau_id
                });

                entityTurmaAula.tau_statusAnotacoes = listTurmaAulaAlunoTipoAnotacao.Any()
                    ? (byte)CLS_TurmaAulaStatusAnotacoes.Preenchida
                    : (byte)CLS_TurmaAulaBO.RetornaStatusAnotacoes(listTurmaAulaAluno, listTurmaAulaAlunoTipoAnotacao);

                if (CLS_TurmaAulaBO.SalvarAulaAnotacoesRecursos(entityTurmaAula, listTurmaAulaAluno, new List<CLS_TurmaAulaRecurso>(),
                                                                listTurmaAulaAlunoTipoAnotacao, null, true, null, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse, (byte)LOG_TurmaAula_Alteracao_Tipo.AnotacaoAluno))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Anotações alunos | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + VS_tau_id);

                    if (fecharPopUp)
                    {
                        lblMessage.Text = UtilBO.GetErroMessage("Anotações salvas com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharCadastroAnotacoes", "var exibirMensagemConfirmacao=false;$('#divAnotacoesAluno').dialog('close');", true);
                    }
                    updAnotacoes.Update();

                    //Verifica se foi adicionada alguma anotação, se sim exibe a imagem que está ok
                    Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAnotacaoSituacao");
                    if (imgSituacao != null)
                        imgSituacao.Visible = entityTurmaAula.tau_statusAnotacoes == (byte)CLS_TurmaAulaStatusAnotacoes.Preenchida;
                }
                else
                {
                    lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar salvar anotações dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                }
            }
            catch (ValidationException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (DuplicateNameException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (ArgumentException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar salvar anotações dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            }
        }

        /// <summary>
        /// Salva as anotações dos alunos para a aula.
        /// </summary>
        private void SalvarAnotacoesMaisdeUmAluno()
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                List<CLS_TurmaAulaAluno> listTurmaAulaAluno
                    = (from GridViewRow linha in grvAnotacoesMaisdeUmAluno.Rows
                       let txtAnotacao = (TextBox)linha.FindControl("txtAnotacao")
                       from ListItem cb in ((CheckBoxList)linha.FindControl("cblAnotacaoAluno")).Items
                       let alu_id = Convert.ToInt64(cb.Value.Split(';')[0])
                       let mtu_id = Convert.ToInt32(cb.Value.Split(';')[1])
                       let mtd_id = Convert.ToInt32(cb.Value.Split(';')[2])
                       where alu_id > 0 && cb.Selected
                       select new CLS_TurmaAulaAluno
                       {
                           tud_id = VS_tud_id_Aula
                           ,
                           tau_id = VS_tau_id
                           ,
                           alu_id = alu_id
                           ,
                           mtu_id = mtu_id
                           ,
                           mtd_id = mtd_id
                           ,
                           taa_anotacao = txtAnotacao.Text.Trim()
                           ,
                           taa_situacao = 1
                           ,
                           usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                       }).ToList();

                List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaAlunoTipoAnotacao
                    = (from GridViewRow linha in grvAnotacoesMaisdeUmAluno.Rows
                       let txtAnotacao = (TextBox)linha.FindControl("txtAnotacao")
                       from ListItem cb in ((CheckBoxList)linha.FindControl("cblAnotacaoAluno")).Items
                       let alu_id = Convert.ToInt64(cb.Value.Split(';')[0])
                       let mtu_id = Convert.ToInt32(cb.Value.Split(';')[1])
                       let mtd_id = Convert.ToInt32(cb.Value.Split(';')[2])
                       where alu_id > 0 && cb.Selected
                       from ListItem tia in ((CheckBoxList)linha.FindControl("cblAnotacoesPredefinidas")).Items
                       let tia_id = Convert.ToInt32(tia.Value)
                       where tia.Selected
                       select new CLS_TurmaAulaAlunoTipoAnotacao
                       {
                           tud_id = VS_tud_id_Aula
                           ,
                           tau_id = VS_tau_id
                           ,
                           alu_id = alu_id
                           ,
                           mtu_id = mtu_id
                           ,
                           mtd_id = mtd_id
                           ,
                           tia_id = tia_id
                       }).ToList();


                CLS_TurmaAula entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
                {
                    tud_id = VS_tud_id_Aula,
                    tau_id = VS_tau_id
                });

                entityTurmaAula.tau_statusAnotacoes = (byte)CLS_TurmaAulaBO.RetornaStatusAnotacoes(listTurmaAulaAluno, listTurmaAulaAlunoTipoAnotacao);

                if (CLS_TurmaAulaBO.SalvarAulaAnotacoes(entityTurmaAula, listTurmaAulaAluno, listTurmaAulaAlunoTipoAnotacao,
                                                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                        (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse, (byte)LOG_TurmaAula_Alteracao_Tipo.AnotacaoAluno))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Anotações alunos | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + VS_tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Anotações salvas com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharCadastroAnotacoes", "var exibirMensagemConfirmacao=false;$('#divAnotacoesMaisdeUmAluno').dialog('close');", true);
                    updAnotacoes.Update();

                    CarregarAnotacoesAluno();

                    //Verifica se foi adicionada alguma anotação, se sim exibe a imagem que está ok
                    Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAnotacaoSituacao");
                    if (imgSituacao != null)
                        imgSituacao.Visible = entityTurmaAula.tau_statusAnotacoes == (byte)CLS_TurmaAulaStatusAnotacoes.Preenchida;
                }
                else
                {
                    lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar salvar anotações dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                }
            }
            catch (ValidationException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (DuplicateNameException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (ArgumentException ex)
            {
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar salvar anotações dos alunos.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            }
        }

        /// <summary>
        /// Adiciona uma nova atividade avaliativa para a aula.
        /// </summary>
        private void SalvarNovaAtividade()
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                bool avaliativa = Convert.ToBoolean((rblAtividadeAvaliativa.Items.Cast<ListItem>().ToList()
                                                                                 .Find(p => p.Selected).Value) ?? "True");
                bool salvou;
                bool visibilidadeRegencia = VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa);
                int id;
                bool edicao = btnEditarAtividade.Visible;
                CLS_TurmaNotaRegencia entityTurmaNotaRegencia = null;

                CLS_TurmaAula entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
                {
                    tud_id = VS_tud_id_Aula,
                    tau_id = VS_tau_id
                });

                if (entityTurmaAula.tau_situacao == (byte)CLS_TurmaAulaSituacao.Excluido)
                    throw new ValidationException("A aula está marcada como excluída, não é possível adicionar a atividade.");

                List<CLS_TurmaNota> listTurmaNota = new List<CLS_TurmaNota>();

                if (avaliativa)
                {
                    CLS_TurmaNota entity = new CLS_TurmaNota();

                    if (edicao)
                    {
                        entity = new CLS_TurmaNota
                        {
                            tnt_id = VS_tnt_id,
                            tud_id = visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                        };
                        CLS_TurmaNotaBO.GetEntity(entity);

                        entity.tur_id = UCControleTurma1.VS_tur_id;
                        entity.tud_id = visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula;
                        entity.tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id;
                        entity.tau_id = visibilidadeRegencia ? -1 : VS_tau_id;
                        entity.tnt_nome = txtNomeAtividade.Text;
                        entity.tnt_descricao = txtConteudoAtividade.Text;
                        entity.tav_id = UCComboTipoAtividadeAvaliativa.Valor;
                        entity.tnt_data = string.IsNullOrEmpty(entityAula.tau_data.ToString()) ? new DateTime() : entityAula.tau_data;
                        entity.tdt_posicao = UCControleTurma1.VS_tdt_posicao;
                        entity.tnt_exclusiva = ParametroPermitirAtividadesExclusivas && chkAtividadeExclusiva.Checked;
                    }
                    else
                    {
                        entity = new CLS_TurmaNota
                        {
                            tur_id = UCControleTurma1.VS_tur_id,
                            tud_id = visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula,
                            tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                            tau_id = visibilidadeRegencia ? -1 : VS_tau_id,
                            tnt_nome = txtNomeAtividade.Text,
                            tnt_descricao = txtConteudoAtividade.Text,
                            tnt_situacao = 1,
                            tav_id = UCComboTipoAtividadeAvaliativa.Valor,
                            tnt_data = string.IsNullOrEmpty(entityAula.tau_data.ToString()) ? new DateTime() : entityAula.tau_data,
                            tdt_posicao = UCControleTurma1.VS_tdt_posicao,
                            tnt_exclusiva = ParametroPermitirAtividadesExclusivas && chkAtividadeExclusiva.Checked,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                            usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                        };

                        if (visibilidadeRegencia)
                        {
                            entityTurmaNotaRegencia = new CLS_TurmaNotaRegencia
                            {
                                tud_id = entity.tud_id,
                                tnt_id = entity.tnt_id,
                                tud_idAula = VS_tud_id_Aula,
                                tau_idAula = VS_tau_id
                            };
                        }
                    }

                    listTurmaNota.Add(entity);
                    entityTurmaAula.tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(listTurmaNota);

                    salvou = CLS_TurmaNotaBO.Save(
                        entity,
                        VS_EntitiesControleTurma.turma,
                        UCNavegacaoTelaPeriodo.cap_dataInicio,
                        UCNavegacaoTelaPeriodo.cap_dataFim,
                        UCNavegacaoTelaPeriodo.cap_dataInicio,
                        UCNavegacaoTelaPeriodo.cap_dataFim,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCHabilidades.RetornaListaHabilidades(),
                        VS_EntitiesControleTurma.formatoAvaliacao.fav_permiteRecuperacaoForaPeriodo,
                        entityTurmaAula,
                        entityTurmaNotaRegencia,
                        true,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        (byte)LOG_TurmaNota_Alteracao_Origem.WebDiarioClasse,
                        (byte)LOG_TurmaNota_Alteracao_Tipo.AlteracaoAtividade
                    );

                    id = entity.tnt_id;
                }
                else
                {
                    CLS_TurmaNota entity = new CLS_TurmaNota();

                    if (edicao)
                    {
                        entity = new CLS_TurmaNota { tnt_id = VS_tnt_id, tud_id = visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula };
                        CLS_TurmaNotaBO.GetEntity(entity);

                        entity.tur_id = UCControleTurma1.VS_tur_id;
                        entity.tud_id = visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula;
                        entity.tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id;
                        entity.tau_id = VS_tau_id;
                        entity.tnt_nome = txtNomeAtividade.Text;
                        entity.tnt_descricao = txtConteudoAtividade.Text;
                        entity.tnt_situacao = 1;
                        entity.tav_id = UCComboTipoAtividadeAvaliativa.Valor;
                        entity.tnt_data = string.IsNullOrEmpty(entityAula.tau_data.ToString()) ? new DateTime() : entityAula.tau_data;
                        entity.tdt_posicao = UCControleTurma1.VS_tdt_posicao;
                    }
                    else
                    {
                        entity = new CLS_TurmaNota
                        {
                            tur_id = UCControleTurma1.VS_tur_id,
                            tud_id = visibilidadeRegencia ?
                                ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula,
                            tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                            tau_id = VS_tau_id,
                            tnt_nome = txtNomeAtividade.Text,
                            tnt_descricao = txtConteudoAtividade.Text,
                            tnt_situacao = 1,
                            tav_id = UCComboTipoAtividadeAvaliativa.Valor,
                            tnt_data = string.IsNullOrEmpty(entityAula.tau_data.ToString()) ? new DateTime() : entityAula.tau_data,
                            tdt_posicao = UCControleTurma1.VS_tdt_posicao,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                            usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                        };

                        if (visibilidadeRegencia)
                        {
                            entityTurmaNotaRegencia = new CLS_TurmaNotaRegencia
                            {
                                tud_id = entity.tud_id,
                                tnt_id = entity.tnt_id,
                                tud_idAula = VS_tud_id_Aula,
                                tau_idAula = VS_tau_id
                            };
                        }
                    }

                    listTurmaNota.Add(entity);
                    entityTurmaAula.tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(listTurmaNota);

                    salvou = CLS_TurmaNotaBO.Save(
                        entity,
                        VS_EntitiesControleTurma.turma,
                        UCNavegacaoTelaPeriodo.cal_dataInicio,
                        UCNavegacaoTelaPeriodo.cal_dataFim,
                        UCNavegacaoTelaPeriodo.cap_dataInicio,
                        UCNavegacaoTelaPeriodo.cap_dataFim,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCHabilidades.RetornaListaHabilidades(),
                        VS_EntitiesControleTurma.formatoAvaliacao.fav_permiteRecuperacaoForaPeriodo,
                        entityTurmaAula,
                        entityTurmaNotaRegencia,
                        true,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        (byte)LOG_TurmaNota_Alteracao_Origem.WebDiarioClasse,
                        (byte)LOG_TurmaNota_Alteracao_Tipo.AlteracaoAtividade
                    );
                    id = entity.tnt_id;
                }

                if (salvou)
                {
                    string idNome = avaliativa ? "tnt_id" : "ttv_id";
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Atividade | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + (visibilidadeRegencia ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula) +
                                                                            "; " + idNome + ": " + id);
                    lblMessageAtividade.Text = UtilBO.GetErroMessage("Atividade " + (edicao ? "alterada" : "incluída") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Image imgAtividadeSituacaoEfetivada = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacaoEfetivada");
                    if (imgAtividadeSituacaoEfetivada != null)
                        imgAtividadeSituacaoEfetivada.Visible = entityTurmaAula.tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada;

                    Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacao");
                    if (imgSituacao != null)
                        imgSituacao.Visible = entityTurmaAula.tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida;

                    CarregarAtividades(true);
                }
            }
            catch (ValidationException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a atividade.", UtilBO.TipoMensagem.Erro);
            }
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

                // Recupera a data de saída do aluno na turma/disciplina
                string sDataSaida = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida"));
                if (!string.IsNullOrEmpty(sDataSaida))
                {
                    DateTime dataSaida = Convert.ToDateTime(sDataSaida);
                    if (dataSaida.Date < UCNavegacaoTelaPeriodo.cap_dataFim)
                    {
                        if (lblNome != null)
                            lblNome.Text += "<br/>" + "<b>Data de saída:</b> " + dataSaida.ToString("dd/MM/yyyy");
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona um item na lista de relatórios do ViewState.
        /// </summary>
        /// <param name="tnt_id">Id da turma nota</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matricula turma do aluno</param>
        /// <param name="valor">Nota que será adicionada</param>
        private void AdicionaItemRelatorio(int tnt_id, long alu_id, int mtu_id, string valor)
        {
            NotasRelatorio rel = new NotasRelatorio
            {
                tnt_id = tnt_id
                ,
                alu_id = alu_id
                ,
                mtu_id = mtu_id
                ,
                valor = valor
            };

            List<NotasRelatorio> lista = VS_Nota_Relatorio;

            lista.Add(rel);

            VS_Nota_Relatorio = lista;
        }

        /// <summary>
        /// Carrega repeater de atividades com dataSources para o pop up de atividades avaliativas.
        /// </summary>
        /// <param name="e">Item do repeater de alunos</param>
        /// <param name="alu_id">ID do aluno</param>
        private void CarregaRepeatersInternosAtAvaliativa(RepeaterItemEventArgs e, long alu_id)
        {
            Repeater rptAtividades = (Repeater)e.Item.FindControl("rptAtividades");
            Repeater rptAtividadesEfetivado = (Repeater)e.Item.FindControl("rptAtividadesEfetivado");
            DataTable dtAtividades = new DataTable();
            List<DataRow> ltAtividades;

            //Repeater rptAvaliacaoSecretaria = (Repeater)e.Item.FindControl("rptAvaliacaoSecretaria");
            //DataTable dtAvaliacaoSecretaria = new DataTable();

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

            // Mostra coluna da média só se a escala for do tipo Numérica.
            HtmlTableCell tdMedia = (HtmlTableCell)e.Item.FindControl("tdMedia");
            if (tdMedia != null)
                tdMedia.Visible = (tipo == EscalaAvaliacaoTipo.Numerica) || ((tipo == EscalaAvaliacaoTipo.Pareceres) && Vs_calcula_notaFinal);

            if (e.Item.ItemType == ListItemType.Header)
            {
                // Busca todas as atividades para o cabeçalho.
                ltAtividades = (from DataRow dr in DTAtividades.Rows
                                let tdt_posicao = Convert.ToByte(dr["tdt_posicao"])
                                where
                                    Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(dr["tau_id"])) ? dr["tau_id"] : 0) == VS_tau_id
                                    && VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao))
                                group dr by dr["tnt_id"] into g
                                orderby Convert.ToDateTime(g.FirstOrDefault()["tnt_data"])
                                                             , Convert.ToInt32(g.FirstOrDefault()["tnt_id"])

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
                                let tdt_posicao = Convert.ToByte(dr["tdt_posicao"])
                                where
                                    Convert.ToInt64(dr["alu_id"]) == alu_id
                                    && Convert.ToInt32(dr["mtu_id"]) == mtu_id
                                    && Convert.ToInt32(dr["mtd_id"]) == mtd_id
                                    && Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(dr["tau_id"])) ? dr["tau_id"] : 0) == VS_tau_id
                                    && VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoEdicao || p.pdc_permissaoConsulta))
                                orderby Convert.ToDateTime(dr["tnt_data"])
                                    , Convert.ToInt32(dr["tnt_id"])
                                select dr).ToList();
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
                                ((p.alu_id == alu_id) &&
                                 (p.tnt_id == tnt_id) &&
                                 (p.mtu_id == mtu_id)));

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
        /// O método salva o planejamento de aula diário de do período.
        /// </summary>
        private void SalvarPlanoAula()
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                // As turmas a partir do ano de 2015 possuem um unico plano de aula 
                // para os componentes da regencia.
                if (((VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)) ||
                    !VisibilidadeRegencia(ddlTurmaDisciplina) ? SalvarPlanejamentoDiario() : SalvarPlanejamentoDiarioRegencia())
                    && (!aPlanejamentoBimestre.Visible || SalvarPlanejamentoPeriodo()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Plano de aula | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tur_id: " + UCControleTurma1.VS_tur_id + ";tud_id: " + VS_tud_id_Aula);

                    lblMessage.Text = UtilBO.GetErroMessage("Plano de aula salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharPlanejamento", "var exibirMensagemConfirmacao=false;$('#divPlanoAula').dialog('close');", true);
                }
            }
            catch (EditarAula_ValidationException ex)
            {
                lblMessagePlanoAula.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (ValidationException ex)
            {
                lblMessagePlanoAula.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (DuplicateNameException ex)
            {
                lblMessagePlanoAula.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (ArgumentException ex)
            {
                lblMessagePlanoAula.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta, "margin: 10px;");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessagePlanoAula.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a aula.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            }
        }

        /// <summary>
        /// Salva a janela de plano de aula diario.
        /// </summary>
        private bool SalvarPlanejamentoDiario()
        {
            if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
            {
                throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
            }

            // Validar operação atual.
            if ((Operacao != OperacaoAtual.InserindoAula) &&
                (Operacao != OperacaoAtual.AlterandoAula))
            {
                throw new ValidationException("Operação inválida.");
            }

            bool isNew = Operacao == OperacaoAtual.InserindoAula;

            // Se for uma alteração, pega a situação atual da aula.
            CLS_TurmaAula entity = new CLS_TurmaAula();
            entity.tud_id = VS_tud_id_Aula;
            entity.tau_id = VS_tau_id;
            CLS_TurmaAulaBO.GetEntity(entity);
            if (!entity.IsNew && entity.tau_dataAlteracao > VS_Data_Diario_TurmaAula)
                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.Validacao_Data_TurmaPlanejamento").ToString());

            if (isNew)
            {
                entity.tdt_posicao = UCControleTurma1.VS_tdt_posicao;
                entity.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
            }

            // Se a disciplina for principal, salva 1 na quantidade de tempos de aula.
            //int qtAulas = DisciplinaPrincipal ? 1 : (string.IsNullOrEmpty(txtNumeroAulas.Text) ? 0 : Convert.ToInt32(txtNumeroAulas.Text));

            entity.tur_id = UCControleTurma1.VS_tur_id;
            entity.tud_id = VS_tud_id_Aula;
            entity.tau_id = isNew ? -1 : VS_tau_id;
            entity.tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id;
            //não possui as proximas informações em tela
            //entity.tau_data = string.IsNullOrEmpty(txtDataAula.Text) ? new DateTime() : Convert.ToDateTime(txtDataAula.Text);
            //entity.tau_numeroAulas = qtAulas;
            entity.tau_planoAula = txtPlanoAula.Text;
            entity.tau_diarioClasse = txtRegistroAula.Text;
            entity.tau_sintese = txtSinteseAula.Text;
            entity.tau_atividadeCasa = chkAtividadeCasa.Checked || (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                                       ? txtAtividadeCasa.Text : "";
            entity.tau_checadoAtividadeCasa = (chkAtividadeCasa.Checked || (VS_cal_ano >= 2015 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)));
            entity.tau_situacao = isNew ? Convert.ToByte(1) : entity.tau_situacao;
            entity.tau_dataAlteracao = VS_dataAlteracaoAula;
            entity.IsNew = isNew;

            // se for uma alteração gravo o usuario que fez a atualização dos dados.
            entity.usu_idDocenteAlteracao = (!isNew) ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty;

            //Conta a quantia de itens selecionados
            //int countItensSelected = 0;
            // Cria a lista derecursos
            List<CLS_TurmaAulaRecurso> listTurmaAulaRecurso = new List<CLS_TurmaAulaRecurso>();
            if (VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                for (int i = 0; i < chkRecursos.Items.Count; i++)
                {
                    if (chkRecursos.Items[i].Selected)
                    {
                        //countItensSelected++;
                        CLS_TurmaAulaRecurso ent = new CLS_TurmaAulaRecurso
                        {
                            tud_id = VS_tud_id_Aula
                            ,
                            tau_id = VS_tau_id
                            ,
                            rsa_id = Convert.ToInt32(chkRecursos.Items[i].Value)
                            ,
                            tar_observacao = Convert.ToInt32(chkRecursos.Items[i].Value) > 0 ? "" : txtOutroRecurso.Text
                            ,
                            tar_dataAlteracao = DateTime.Now
                            ,
                            tar_dataCriacao = DateTime.Now
                        };

                        listTurmaAulaRecurso.Add(ent);
                    }
                }
            }

            List<Int64> lstComponentesRegencia = null;
            if (cblComponentesRegencia.Visible)
            {
                lstComponentesRegencia = new List<Int64>();
                string[] ids;
                foreach (ListItem item in cblComponentesRegencia.Items)
                {
                    if (item.Selected)
                    {
                        ids = item.Value.Split(';');
                        if (ids.Length > 1)
                        {
                            lstComponentesRegencia.Add(Convert.ToInt64(ids[1]));
                        }
                    }
                }
            }

            entity.tau_statusPlanoAula = (byte)CLS_TurmaAulaBO.RetornaStatusPlanoAula(entity,
                                                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SINTESE_REGENCIA_AULA_TURMA, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                                                    VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula = CriarListaHabilidadesTurmaAula();

            List<CLS_ObjetoAprendizagemTurmaAula> listObjTudTau = null;
            if (divObjetosAprendizagem.Visible)
                listObjTudTau = CriarListaObjetoAprendizagemAula();

            if (CLS_TurmaAulaBO.SalvarAulaAnotacoesRecursos(entity, new List<CLS_TurmaAulaAluno>(), listTurmaAulaRecurso, null,
                                                            lstComponentesRegencia, true, listOriCurTurAula,
                                                            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                            (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse,
                                                            (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoPlanoAula,
                                                            listObjTudTau))
            {
                if (isNew)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Planejamento diário | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + entity.tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Planejamento diário incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Planejamento diário | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + entity.tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Planejamento diário salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Image imgPlanoAulaSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgPlanoAulaSituacao");
                Image imgPlanoAulaSituacaoIncompleta = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgPlanoAulaSituacaoIncompleta");
                if (imgPlanoAulaSituacaoIncompleta != null && imgPlanoAulaSituacaoIncompleta != null)
                {
                    imgPlanoAulaSituacao.Visible = entity.tau_statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Preenchida;
                    imgPlanoAulaSituacaoIncompleta.Visible = entity.tau_statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Incompleto;
                }

                // Apenas aulas dos dias anteriores sem plano de aula devem exibir o aviso.
                Image imgSemPlanoAula = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgSemPlanoAula");
                if (imgSemPlanoAula != null && entity.tau_data.Date < DateTime.Now.Date && 
                    UCNavegacaoTelaPeriodo.VS_tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    imgSemPlanoAula.Visible = string.IsNullOrEmpty(entity.tau_planoAula)
                                                && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                                    || VS_EntitiesControleTurma.curso.tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                    || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                    imgSemPlanoAula.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.imgSemPlanoAula").ToString();
                }
                ControlarExibicaoLegendaAulaSemPlano();

                return true;
            }

            return false;
        }

        private void ControlarExibicaoLegendaAulaSemPlano()
        {
            divAvisoAulaSemPlano.Visible = false;
            foreach (GridViewRow dr in grvAulas.Rows)
            {
                Image imgSemPlanoAula = (Image)(dr.FindControl("imgSemPlanoAula"));
                if (imgSemPlanoAula != null && imgSemPlanoAula.Visible)
                {
                    divAvisoAulaSemPlano.Visible = true;
                }
            }

        }

        /// <summary>
        /// Inserir ou altera uma aula das componentes da regência.
        /// </summary>
        private bool SalvarPlanejamentoDiarioRegencia()
        {
            if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
            {
                throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
            }

            string[] ids = _ddlTurmaDisciplinaComponente.SelectedValue.Split(';');
            long Tud_idFilho = -1;

            if (ids.Length > 1)
            {
                Tud_idFilho = Convert.ToInt64(ids[1]);
            }

            // Se for uma alteração, pega a situação atual da aula.
            CLS_TurmaAulaRegencia entity = new CLS_TurmaAulaRegencia
            {
                tud_id = VS_tud_id_Aula,
                tau_id = VS_tau_id,
                tud_idFilho = Tud_idFilho
            };
            CLS_TurmaAulaRegenciaBO.GetEntity(entity);

            bool isNew = entity.IsNew;

            // Se a disciplina for principal, salva 1 na quantidade de tempos de aula.
            //int qtAulas = DisciplinaPrincipal ? 1 : (string.IsNullOrEmpty(txtNumeroAulas.Text) ? 0 : Convert.ToInt32(txtNumeroAulas.Text));

            entity.tuf_data = VS_tau_data == new DateTime() ? new DateTime() : VS_tau_data;
            //entity.tuf_numeroAulas = qtAulas;
            entity.tuf_planoAula = txtPlanoAula.Text;
            entity.tuf_diarioClasse = txtRegistroAula.Text;
            entity.tuf_sintese = txtSinteseAula.Text;
            entity.tuf_atividadeCasa = chkAtividadeCasa.Checked ? txtAtividadeCasa.Text : "";
            entity.tuf_checadoAtividadeCasa = chkAtividadeCasa.Checked && !String.IsNullOrEmpty(txtAtividadeCasa.Text);
            entity.tuf_situacao = isNew ? Convert.ToByte(1) : entity.tuf_situacao;
            entity.tuf_dataAlteracao = isNew ? DateTime.Now : entity.tuf_dataAlteracao;
            entity.tuf_dataCriacao = isNew ? DateTime.Now : entity.tuf_dataCriacao;

            entity.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

            //Conta a quantia de itens selecionados
            //int countItensSelected = 0;
            // Cria a lista de recursos
            List<CLS_TurmaAulaRecursoRegencia> listTurmaAulaRecurso = new List<CLS_TurmaAulaRecursoRegencia>();
            for (int i = 0; i < chkRecursos.Items.Count; i++)
            {
                if (chkRecursos.Items[i].Selected)
                {
                    //countItensSelected++;
                    CLS_TurmaAulaRecursoRegencia ent = new CLS_TurmaAulaRecursoRegencia
                    {
                        tud_id = VS_tud_id_Aula,
                        tau_id = VS_tau_id,
                        trr_id = -1,
                        tud_idFilho = Tud_idFilho,
                        rsa_id = Convert.ToInt32(chkRecursos.Items[i].Value),
                        trr_observacao = Convert.ToInt32(chkRecursos.Items[i].Value) > 0 ? "" : txtOutroRecurso.Text,
                        trr_dataAlteracao = DateTime.Now,
                        trr_dataCriacao = DateTime.Now
                    };

                    listTurmaAulaRecurso.Add(ent);
                }
            }

            CLS_TurmaAula entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
            {
                tud_id = VS_tud_id_Aula,
                tau_id = VS_tau_id
            });
            if (!entityTurmaAula.IsNew && entityTurmaAula.tau_dataAlteracao > VS_Data_Diario_TurmaAula)
                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.Validacao_Data_TurmaPlanejamento").ToString());

            List<CLS_TurmaAulaRegencia> listTurmaAulaRegencia = CLS_TurmaAulaRegenciaBO.SelecionaPorDisciplinaTurmaAula(entityTurmaAula.tud_id, entityTurmaAula.tau_id);
            listTurmaAulaRegencia.RemoveAll(p => p.tud_idFilho == entity.tud_idFilho);
            listTurmaAulaRegencia.Add(entity);

            entityTurmaAula.tau_statusPlanoAula = (byte)CLS_TurmaAulaBO.RetornaStatusPlanoAulaRegencia(listTurmaAulaRegencia);

            List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula = CriarListaHabilidadesTurmaAula();

            if (CLS_TurmaAulaRegenciaBO.SalvarAulaAnotacoesRecursos(entity, listTurmaAulaRecurso, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                    VS_EntitiesControleTurma.turmaDisciplina.tud_duplaRegencia, VS_EntitiesControleTurma.formatoAvaliacao.fav_fechamentoAutomatico
                                                                    , UCControleTurma1.AtribuicoesVerificarVigencia
                                                                    , entityTurmaAula, listOriCurTurAula,
                                                                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                    (byte)LOG_TurmaAula_Alteracao_Origem.WebDiarioClasse,
                                                                    (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoPlanoAula))
            {
                if (isNew)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Planejamento diário | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + entity.tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Planejamento diário incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Planejamento diário | " +
                                                                            "cal_id: " + UCNavegacaoTelaPeriodo.VS_cal_id + " | tpc_id: " + UCNavegacaoTelaPeriodo.VS_tpc_id +
                                                                            " | " + "tud_id: " + VS_tud_id_Aula + "; tau_id: " + entity.tau_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Planejamento diário salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                // Verifica o que esta gravado para poder mostrar o icone certo.
                Image imgPlanoAulaSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgPlanoAulaSituacao");
                Image imgPlanoAulaSituacaoIncompleta = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgPlanoAulaSituacaoIncompleta");
                if (imgPlanoAulaSituacaoIncompleta != null && imgPlanoAulaSituacaoIncompleta != null)
                {
                    imgPlanoAulaSituacao.Visible = entityTurmaAula.tau_statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Preenchida;
                    imgPlanoAulaSituacaoIncompleta.Visible = entityTurmaAula.tau_statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Incompleto;
                }

                // Apenas aulas dos dias anteriores sem plano de aula devem exibir o aviso.
                Image imgSemPlanoAula = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgSemPlanoAula");

                if (imgSemPlanoAula != null && entityTurmaAula.tau_data.Date < DateTime.Now.Date &&
                    UCNavegacaoTelaPeriodo.VS_tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    imgSemPlanoAula.Visible = string.IsNullOrEmpty(entityTurmaAula.tau_planoAula)
                                                && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                                    || VS_EntitiesControleTurma.curso.tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                    || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                    imgSemPlanoAula.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.imgSemPlanoAula").ToString();
                }
                ControlarExibicaoLegendaAulaSemPlano();

                return true;
            }
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a aula.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        /// <summary>
        /// Salva a janela de plano de aula diario.
        /// </summary>
        private bool SalvarPlanejamentoPeriodo()
        {
            if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                    || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
            {
                throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
            }

            List<CLS_TurmaDisciplinaPlanejamento> listTurmaDIsPlan = CriarPlanejamentoPeriodo();
            List<CLS_PlanejamentoOrientacaoCurricular> listPlanOriCur = CriarListaHabilidades();

            // Salva os planejamentos digitados.
            if (CLS_PlanejamentoOrientacaoCurricularBO.SalvaPlanejamentoTurmaDisciplina(
                listTurmaDIsPlan, listPlanOriCur,
                new List<CLS_PlanejamentoOrientacaoCurricularDiagnostico>(),
                CriarListaTurmasReplicar(), ParametroReplicarTurmas, UCControleTurma1.VS_tur_id))
            {
                return true;
            }
            lblMessagePlanoAula.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o planejamento do período.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            return false;
        }

        /// <summary>
        /// O método cria uma lista de ids das turmas para os quais o planejamento anual será replicado.
        /// </summary>
        /// <returns></returns>
        private List<long> CriarListaTurmasReplicar()
        {
            //a pop up não será aberta portanto sempre retorna uma lista vazia
            //uma vez que o método salvar necessita de ums lista
            return new List<long>();
        }

        /// <summary>
        /// Cria lista de orientações curriculares para salvar
        /// </summary>
        /// <returns></returns>
        private List<CLS_PlanejamentoOrientacaoCurricular> CriarListaHabilidades()
        {
            List<CLS_PlanejamentoOrientacaoCurricular> lista = new List<CLS_PlanejamentoOrientacaoCurricular>();
            lista.AddRange(
                    (
                     from RepeaterItem habilidade in rptHabilidadesCOC.Items
                     let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                     let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                     where permiteLancamento
                     let chave = ((HiddenField)habilidade.FindControl("hdnChave")).Value
                     let chkPlanejado = (CheckBox)habilidade.FindControl("chkPlanejado")
                     let chkTrabalhado = (CheckBox)habilidade.FindControl("chkTrabalhado")
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
                         tdt_posicao = UCControleTurma1.VS_tdt_posicao
                     }).ToList()
            );
            return lista;
        }

        /// <summary>
        /// Cria lista de orientações curriculares da aula da turma para salvar
        /// </summary>
        /// <returns></returns>
        private List<CLS_TurmaAulaOrientacaoCurricular> CriarListaHabilidadesTurmaAula()
        {
            List<CLS_TurmaAulaOrientacaoCurricular> lista = new List<CLS_TurmaAulaOrientacaoCurricular>();
            lista.AddRange(
                    (
                     from RepeaterItem habilidade in rptHabilidadesAula.Items
                     let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                     let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                     where permiteLancamento
                     let ocrId = Convert.ToInt32(((HiddenField)habilidade.FindControl("hdnOcrId")).Value)
                     let chkTrabalhado = (CheckBox)habilidade.FindControl("chkTrabalhado")
                     where chkTrabalhado.Checked == true
                     select new CLS_TurmaAulaOrientacaoCurricular
                     {
                         tud_id = VS_tud_id_Aula
                         ,
                         tau_id = VS_tau_id
                         ,
                         ocr_id = ocrId
                         ,
                         tao_pranejado = false
                         ,
                         tao_trabalhado = chkTrabalhado.Checked
                         ,
                         tao_alcancado = false
                         ,
                         IsNew = true
                         ,
                         tud_idRegencia = _ddlTurmaDisciplinaComponente.Visible ? Tud_idComponente : 0
                     }).ToList()
            );

            //É inserido um item com apenas os dados para consultar possíveis exclusoes.
            if (lista.Count == 0)
            {
                lista.Add(new CLS_TurmaAulaOrientacaoCurricular
                {
                    tud_id = _ddlTurmaDisciplinaComponente.Visible ? Tud_idComponente : VS_tud_id_Aula
                         ,
                    tau_id = VS_tau_id
                         ,
                    ocr_id = 0
                         ,
                    tud_idRegencia = _ddlTurmaDisciplinaComponente.Visible ? VS_tud_id_Aula : 0
                });
            }
            return lista;
        }

        private List<CLS_ObjetoAprendizagemTurmaAula> CriarListaObjetoAprendizagemAula()
        {
            List<CLS_ObjetoAprendizagemTurmaAula> lstObjTudTau = new List<CLS_ObjetoAprendizagemTurmaAula>();
            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");

                if (ckbCampo != null && ckbCampo.Checked)
                {
                    HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                    if (hdnId != null)
                    {
                        lstObjTudTau.Add(new CLS_ObjetoAprendizagemTurmaAula
                                            {
                                                tud_id = VS_tud_id_Aula,
                                                tau_id = VS_tau_id,
                                                oap_id = Convert.ToInt32(hdnId.Value)
                                            });
                    }
                }
            }
            return lstObjTudTau;
        }

        /// <summary>
        /// Lista os dados do planejamento anual da turma.
        /// </summary>
        /// <returns></returns>
        private List<CLS_TurmaDisciplinaPlanejamento> CriarPlanejamentoPeriodo()
        {
            List<CLS_TurmaDisciplinaPlanejamento> planejamento = new List<CLS_TurmaDisciplinaPlanejamento>();
            int tdp_id = string.IsNullOrEmpty(lblTdp_id_COC.Text) ? -1 : Convert.ToInt32(lblTdp_id_COC.Text);
            planejamento.Add(
                new CLS_TurmaDisciplinaPlanejamento
                {
                    tud_id = _ddlTurmaDisciplinaComponente.Visible && Tud_idComponente > -1 ? Tud_idComponente : VS_tud_id_Aula,
                    tdp_id = tdp_id,
                    tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id,
                    tdp_diagnostico = txtDiagnosticoCOC.Text,
                    tdp_planejamento = txtPlanejamentoCOC.Text,
                    tdp_recursos = txtRecursosCOC.Text,
                    tdp_intervencoesPedagogicas = txtIntervencoesPedagogicasCOC.Text,
                    tdp_registroIntervencoes = txtRegistroIntervencoesCOC.Text,
                    cur_id = VS_EntitiesControleTurma.curriculoPeriodo.cur_id,
                    crr_id = VS_EntitiesControleTurma.curriculoPeriodo.crr_id,
                    crp_id = VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                    tdt_posicao = UCControleTurma1.VS_tdt_posicao,
                    tdp_situacao = 1,
                    IsNew = tdp_id <= 0
                }
                );
            return planejamento;
        }

        private void UCConfirmacaoOperacao1_ConfimaOperacao()
        {
            if (VS_tud_id_Excluir > 0 && VS_tnt_id_Excluir > 0)
                ExcluirAtividade(VS_tud_id_Excluir, VS_tnt_id_Excluir);
            else
                lblInfoAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a "
                + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_ATIVIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                ".", UtilBO.TipoMensagem.Erro);
        }

        /// <summary>
        /// Excluir a atividade avaliativa e as notas lançadas nela.
        /// </summary>
        /// <param name="sender">Botão que chamou a ação - deve estar dentro do repeater de atividades</param>
        private void ExcluirAtividade(Int64 tud_id, int tnt_id)
        {
            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                if (tud_id != null && tnt_id != null)
                {
                    CLS_TurmaNota entity = new CLS_TurmaNota
                    {
                        tud_id = tud_id
                        ,
                        tnt_id = tnt_id
                        ,
                        tnt_situacao = (byte)CLS_TurmaNotaSituacao.Excluido
                        ,
                        tnt_dataAlteracao = DateTime.Now
                        ,
                        tpc_id = UCNavegacaoTelaPeriodo.VS_tpc_id
                    };

                    if (CLS_TurmaNotaBO.Delete(entity, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                               (byte)LOG_TurmaNota_Alteracao_Origem.WebDiarioClasse,
                                               (byte)LOG_TurmaNota_Alteracao_Tipo.ExclusaoAtividade))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " | " + "tud_id: " + entity.tud_id + "; tnt_id: " + entity.tnt_id);
                        lblInfoAtividade.Text = UtilBO.GetErroMessage(string.Format("{0} excluído(a) com sucesso.", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id)), UtilBO.TipoMensagem.Sucesso, "margin: 10px;");

                        #region Atualiza o status das atividades avaliativas

                        Repeater rptAtividadesHeader = (Repeater)((RepeaterItem)rptAlunos.Controls[0]).FindControl("rptAtividadesEfetivado");

                        List<CLS_TurmaNota> listTurmaNota = (from RepeaterItem itemAtividade in rptAtividadesHeader.Items
                                                             let tdt_posicao = Convert.ToInt16(((Label)itemAtividade.FindControl("lblPosicao")).Text)
                                                             let chkEfetivado = (CheckBox)itemAtividade.FindControl("chkEfetivado")
                                                             let tnt_id2 = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltnt_id")).Text)
                                                             let tnt_exclusiva = Convert.ToBoolean(((Label)itemAtividade.FindControl("lblAtividadeExclusiva")).Text)
                                                             let usu_id = (!string.IsNullOrEmpty(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text)
                                                                                ? new Guid(((Label)itemAtividade.FindControl("lblUsuIdAtiv")).Text) : Guid.Empty)
                                                             where (usu_id == __SessionWEB.__UsuarioWEB.Usuario.usu_id
                                                                        || VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao))
                                                                        && entity.tnt_id != tnt_id2
                                                             select new CLS_TurmaNota
                                                             {
                                                                 tud_id = VisibilidadeRegencia(ddlTurmaDisciplinaAtAvaliativa) ?
                                                                    ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id_Aula
                                                                 ,
                                                                 tnt_id = tnt_id2
                                                                 ,
                                                                 tnt_efetivado = chkEfetivado.Checked
                                                                 ,
                                                                 tdt_posicao = UCControleTurma1.VS_tdt_posicao
                                                                 ,
                                                                 tnt_exclusiva = tnt_exclusiva
                                                                 ,
                                                                 usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                                                             }).ToList();

                        CLS_TurmaAula entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
                        {
                            tud_id = VS_tud_id_Aula,
                            tau_id = VS_tau_id
                        });

                        entityTurmaAula.tau_statusAtividadeAvaliativa = (byte)CLS_TurmaAulaBO.RetornaStatusAtividadeAvaliativa(listTurmaNota);

                        CLS_TurmaAulaBO.Save(entityTurmaAula);

                        Image imgAtividadeSituacaoEfetivada = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacaoEfetivada");
                        if (imgAtividadeSituacaoEfetivada != null)
                            imgAtividadeSituacaoEfetivada.Visible = entityTurmaAula.tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada;

                        Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAtividadeSituacao");
                        if (imgSituacao != null)
                            imgSituacao.Visible = entityTurmaAula.tau_statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida;

                        #endregion Atualiza o status das atividades avaliativas

                        CarregarAtividades(true);
                    }
                }
            }
            catch (ValidationException ex)
            {
                TrataValidationException(ex, lblInfoAtividade);
                updAtividade.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblInfoAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a "
                + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_ATIVIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                ".", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                updAtividade.Update();
            }
        }

        /// <summary>
        /// Trata as execeções do tipo ValidationException e as que herdam dela.
        /// Verifica qual o tipo da execeção, e redireciona pra tela de busca se necessário
        /// </summary>
        /// <param name="ex">Execeção a ser verificada</param>
        /// <param name="lblMensagemExibir">Label onde será exibida a mensagem</param>
        private void TrataValidationException(ValidationException ex, Label lblMensagemExibir)
        {
            if (ex is IncluirTurmaNotaAluno_ValidationException)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);

                Response.Redirect(UCNavegacaoTelaPeriodo.VS_paginaRetorno, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            string msgErro = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            if (lblMensagemExibir == null)
            {
                lblMessage.Text = msgErro;
            }
            else
            {
                lblMensagemExibir.Text = msgErro;
            }
        }

        /// <summary>
        /// Habilita a edição da atividade avaliativa
        /// </summary>
        /// <param name="sender">LinkButton que chamou a ação - deve estar dentro do repeater de atividades</param>
        private void EditarAtividade(object sender)
        {
            // Encontrar os labels que estão no item do repeater correspondente ao botão.
            ImageButton btnEditarAtividadePopup = (ImageButton)sender;
            RepeaterItem item = (RepeaterItem)btnEditarAtividadePopup.NamingContainer;
            Label lbltud_id = (Label)item.FindControl("lbltud_id");
            Label lbltnt_id = (Label)item.FindControl("lbltnt_id");

            try
            {
                if ((VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                        || VS_EntitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                    UCControleTurma1.VS_tur_dataEncerramento != new DateTime() &&
                    VS_tau_data > UCControleTurma1.VS_tur_dataEncerramento)
                {
                    throw new ValidationException("Data da aula é maior que a data de encerramento da turma.");
                }

                if (lbltud_id != null && lbltnt_id != null)
                {
                    CLS_TurmaNota entity = new CLS_TurmaNota
                    {
                        tud_id = Convert.ToInt64(lbltud_id.Text),
                        tnt_id = Convert.ToInt32(lbltnt_id.Text)
                    };
                    CLS_TurmaNotaBO.GetEntity(entity);

                    VS_tnt_id = entity.tnt_id;

                    UCComboTipoAtividadeAvaliativa.CarregaTipoAtividadeAvaliativaAtivosMaisInativo(true, entity.tav_id);

                    IEnumerable<string> x;

                    if (entity.tud_id > 0)
                    {
                        x = from ListItem lItem in ddlComponenteAtAvaliativa.Items
                            where lItem.Value.Split(';')[0].Equals(UCControleTurma1.VS_tur_id.ToString())
                                   && lItem.Value.Split(';')[1].Equals(lbltud_id.Text)
                            select lItem.Value;
                    }
                    else
                    {
                        x = from ListItem lItem in ddlComponenteAtAvaliativa.Items
                            where lItem.Value.Split(';')[0].Equals(UCControleTurma1.VS_tur_id.ToString())
                            select lItem.Value;
                    }

                    if (x.Count() > 0)
                        ddlComponenteAtAvaliativa.SelectedValue = x.First();

                    UCComboTipoAtividadeAvaliativa.Valor = entity.tav_id;
                    txtNomeAtividade.Text = entity.tnt_nome;
                    txtConteudoAtividade.Text = entity.tnt_descricao;
                    chkAtividadeExclusiva.Checked = entity.tnt_exclusiva;

                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.RELACIONAR_HABILIDADES_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        UCHabilidades.CarregarHabilidades(
                            VS_EntitiesControleTurma.curriculoPeriodo.cur_id,
                            VS_EntitiesControleTurma.curriculoPeriodo.crr_id,
                            VS_EntitiesControleTurma.curriculoPeriodo.crp_id,
                            UCControleTurma1.VS_tur_id,
                            entity.tud_id,
                            UCNavegacaoTelaPeriodo.VS_cal_id,
                            UCControleTurma1.VS_tdt_posicao,
                            VS_tnt_id,
                            UCNavegacaoTelaPeriodo.VS_tpc_id
                        );

                        UCHabilidades.Visible = true;
                    }

                    btnNovaAtividade.Visible = false;
                    btnCancelarAtividade.Visible = btnEditarAtividade.Visible = true;

                    if (ddlComponenteAtAvaliativa.Visible)
                        ddlComponenteAtAvaliativa.Focus();
                    else
                        UCComboTipoAtividadeAvaliativa.Focus();
                }
            }
            catch (ValidationException ex)
            {
                TrataValidationException(ex, lblMessageAtividade);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar editar a "
                + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_ATIVIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                ".", UtilBO.TipoMensagem.Erro);
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

            // Calcular variáveis de acordo com a escala de avaliação.
            //Calcula a quantidade de casas decimais da variação de notas
            string variacao = "0,1";
            int numeroCasasDecimais = 1;
            if (VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica != null)
            {
                variacao = Convert.ToDouble(VS_EntitiesControleTurma.escalaDocente.escalaAvaliacaoNumerica.ean_variacao).ToString();
                if (variacao.IndexOf(",") >= 0)
                {
                    numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                }
            }
            string escala_variacao = "0";
            string escala_maior_valor = "0";

            if (VS_EntitiesControleTurma.escalaDiciplina.escalaAvaliacaoNumerica != null)
            {
                escala_variacao = VS_EntitiesControleTurma.escalaDiciplina.escalaAvaliacaoNumerica.ean_variacao.ToString();
                escala_maior_valor = VS_EntitiesControleTurma.escalaDiciplina.escalaAvaliacaoNumerica.ean_maiorValor.ToString().Replace(',', '.');
            }

            string script = "var corPlanejado='" + ApplicationWEB.OrientacaoCurricularPlanejada + "';" +
                      "var corTrabalhado='" + ApplicationWEB.OrientacaoCurricularTrabalhada + "';" +
                      "var calcularMediaAutomatica = " + (!Vs_calcula_notaFinal).ToString().ToLower() + ";" +
                      "var arredondamento = " + arredondamento.ToLower() + ";" +
                      "var qtdCasasDecimais = parseInt('" + numeroCasasDecimais + "');" +
                      "var variacaoEscala = '" + escala_variacao.Replace(',', '.') + "';" +
                      "var destacarCampoNota = " + destacarCampoNota.ToLower() + ";" +
                      "var maiorValor = " + escala_maior_valor + ";";

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                script += "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
                    "exibirNotaFinal=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id) ? "1" : "0") + ";" +
                    "ExibeCompensacao=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id) ? "1" : "0") + ";" +
                    "MinutosCacheFechamento=" + ApplicationWEB.AppMinutosCacheFechamento + ";";

            }
            return script;
        }

        /// <summary>
        /// metodo que controla a visivilidade dos campos de quantidade de aula
        /// </summary>
        protected void ExibeQuantidadeAulas()
        {
            lblQtdeAulas.Visible = txtQtdeAulas.Visible = (!(DisciplinaPrincipal || DisciplinaRegencia)) || RegenciaETemposAula;
        }

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                {
                    lblMessage.Text = message;
                }

                imgLegendaEventoSemAtividade.ToolTip = string.Empty;
                LabelSinteseAula.Text += ApplicationWEB.TextoAsteriscoObrigatorio;

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
                            if (Session["tur_tipo"] != null && Session["tur_idNormal"] != null && Session["tud_idAluno"] != null)
                            {
                                UCControleTurma1.VS_tur_tipo = Convert.ToByte(Session["tur_tipo"]);
                                UCControleTurma1.VS_tur_idNormal = Convert.ToInt64(Session["tur_idNormal"]);
                                UCControleTurma1.VS_tud_idAluno = Convert.ToInt64(Session["tud_idAluno"]);
                            }
                            UCNavegacaoTelaPeriodo.VS_paginaRetorno = Session["PaginaRetorno"].ToString();
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

                        int tpcIdPendencia = -1;
                        if (Session["tpcIdPendencia"] != null)
                        {
                            tpcIdPendencia = Convert.ToInt32(Session["tpcIdPendencia"]);
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

                        // Se for perfil Administrador e possuir disciplina especial
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
                                hdnSituacaoTurmaDisciplina.Value = VS_situacaoTurmaDisciplina.ToString();

                                UCControleTurma1.LabelTurmas = dadosTurma.FirstOrDefault().TurmaDisciplinaEscola;
                            }
                        }

                        UCNavegacaoTelaPeriodo.VS_opcaoAbaAtual = eOpcaoAbaMinhasTurmas.DiarioClasse;

                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(UCControleTurma1.VS_tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);
                        VS_DisciplinaEspecial = VS_EntitiesControleTurma.turmaDisciplina.tud_disciplinaEspecial;
                        ACA_CurriculoPeriodo entityCrp = ACA_CurriculoPeriodoBO.SelecionaPorTurmaTipoNormal(UCControleTurma1.VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);
                        VS_crp_controleTempo = entityCrp.crp_controleTempo;
                        VS_PossuiRegencia = TUR_TurmaBO.VerificaPossuiDisciplinaPorTipo(UCControleTurma1.VS_tur_id, TurmaDisciplinaTipo.Regencia, ApplicationWEB.AppMinutosCacheLongo);

                        VS_turmasAnoAtual = dadosTurma.FirstOrDefault().turmasAnoAtual;

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
            else if (chkAtividadeCasa.Visible && VS_cal_ano >= 2015)
            {
                chkAtividadeCasa.Visible = false;
            }

            UCNavegacaoTelaPeriodo.OnCarregaDadosTela += CarregaSessionPaginaRetorno;
            UCComboTipoAtividadeAvaliativa.IndexChanged += UCComboTipoAtividadeAvaliativa_IndexChanged;
            UCControleTurma1.IndexChanged = uccTurmaDisciplina_IndexChanged;
            UCControleTurma1.DisciplinaCompartilhadaIndexChanged = uccDisciplinaCompartilhada_IndexChanged;
            UCNavegacaoTelaPeriodo.OnAlteraPeriodo += CarregarTela;
            UCConfirmacaoOperacao1.ConfimaOperacao += UCConfirmacaoOperacao1_ConfimaOperacao;
            UCSelecaoDisciplinaCompartilhada1.SelecionarDisciplina += UCSelecaoDisciplinaCompartilhada1_SelecionarDisciplina;
            UCControleTurma1.chkTurmasNormaisMultisseriadasIndexChanged += UCControleTurma_chkTurmasNormaisMultisseriadasIndexChanged;

            // Configura javascripts da tela.
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryBlockUI));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsLoader.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.treeview.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tablesorter.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.metadata.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetaMensagemCampos.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsControleTurma_DiarioClasse.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCSelecaoDisciplinaCompartilhada.js"));

                string script = GeraScriptVariaveisTurma();
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "VariaveisScript", script, true);

            }

            trExibirAlunoDispensadoFrequencia.Visible = trExibirAlunoDispensadoAtividade.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LEGENDA_ALUNO_DISPENSADO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
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
                    HtmlTableCell cell;

                    // Legenda lançamento frequencia
                    cell = tbLegenda.Rows[0].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.AlunoDispensado;

                    cell = tbLegenda.Rows[1].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.AlunoInativo;

                    if (tbLegendaHabilidades != null)
                    {
                        cell = tbLegendaHabilidades.Rows[0].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.OrientacaoCurricularPlanejada;
                        cell = tbLegendaHabilidades.Rows[1].Cells[0];
                        if (cell != null)
                            cell.BgColor = ApplicationWEB.OrientacaoCurricularTrabalhada;
                    }

                    // Legenda atividade
                    cell = tbLegendaAtiv.Rows[0].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.AlunoDispensado;
                    cell = tbLegendaAtiv.Rows[1].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.AlunoAusente;
                    cell = tbLegendaAtiv.Rows[2].Cells[0];
                    if (cell != null)
                        cell.BgColor = ApplicationWEB.AlunoInativo;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rptLegendaDiario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string tdc_corDestaque = DataBinder.Eval(e.Item.DataItem, "tdc_corDestaque").ToString();
                int idDocente = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tdc_id"));

                HtmlControl tdCorLegendaDiario = (HtmlControl)e.Item.FindControl("tdCorLegendaDiario");
                Label lblLegendaDiario = (Label)e.Item.FindControl("lblLegendaDiario");

                if (tdCorLegendaDiario != null && !string.IsNullOrEmpty(tdc_corDestaque))
                {
                    tdCorLegendaDiario.Style["background-color"] = tdc_corDestaque;
                }

                string textoLegenda = String.Empty;
                if (idDocente <= 0)
                {
                    textoLegenda = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.rptLegendaDiario.DisciplinaDocenciaCompartilhada").ToString();
                }
                else
                {
                    string tipoDocente = DataBinder.Eval(e.Item.DataItem, "tipoDocente").ToString();
                    EnumTipoDocente tdc_id = (EnumTipoDocente)idDocente;
                    if (lblLegendaDiario != null)
                    {
                        textoLegenda = "Aula criada pelo professor ";
                        switch (tdc_id)
                        {
                            case EnumTipoDocente.Titular:
                                textoLegenda += tipoDocente.ToLower();
                                break;

                            case EnumTipoDocente.Compartilhado:
                                textoLegenda += "de docência compartilhada";
                                break;

                            case EnumTipoDocente.Projeto:
                                textoLegenda += "de " + tipoDocente.ToLower() + "s";
                                break;

                            case EnumTipoDocente.Substituto:
                                textoLegenda = "Aula criada por um professor " + tipoDocente.ToLower();
                                break;

                            case EnumTipoDocente.Especial:
                                textoLegenda += " especial";
                                break;

                            case EnumTipoDocente.SegundoTitular:
                                textoLegenda += tipoDocente.ToLower();
                                break;

                            default:
                                textoLegenda = String.Empty;
                                break;
                        }
                        textoLegenda += ".";
                    }
                }
                lblLegendaDiario.Text = textoLegenda;
            }
        }

        protected void btnRelatorioFrequencia_Click(object sender, EventArgs e)
        {
            ImprimirRelatorio(((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseFrequencia).ToString());
        }

        protected void btnRelatorioAvaliacao_Click(object sender, EventArgs e)
        {
            ImprimirRelatorio(((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseAvaliacao).ToString());
        }

        protected void grvAulas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte tdt_posicao = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tdt_posicao"]);
                string tdc_corDestaque = grvAulas.DataKeys[e.Row.RowIndex].Values["tdc_corDestaque"].ToString();
                string pes_nome = grvAulas.DataKeys[e.Row.RowIndex].Values["pes_nome"].ToString();
                byte tdc_id = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tdc_id"].ToString());

                byte statusFrequencia = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_statusFrequencia"].ToString());
                byte statusAtividadeAvaliativa = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_statusAtividadeAvaliativa"].ToString());
                byte statusAnotacoes = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_statusAnotacoes"].ToString());
                byte statusPlanoAula = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_statusPlanoAula"].ToString());
                bool semPlanoAula = Convert.ToBoolean(grvAulas.DataKeys[e.Row.RowIndex].Values["semPlanoAula"].ToString());

                bool permissaoAlteracao = Convert.ToInt16(grvAulas.DataKeys[e.Row.RowIndex].Values["permissaoAlteracao"].ToString()) > 0;
                bool permissaoModuloAlteracao = false;
                bool permissaoModuloExclusao = false;
                bool permissaoModuloAlteracaoInfantil = false;
                bool permissaoModuloExclusaoInfantil = false;

                bool tau_reposicao = Convert.ToBoolean(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_reposicao"]);

                // Se for visão de Gestor (Coordenador Pedagógico etc) não permite salvar dados
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                {
                    permissaoAlteracao = false;
                    permissaoModuloExclusao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoExclusao;
                    permissaoModuloAlteracao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao;
                }
                else if (permissaoAlteracao)
                {
                    permissaoModuloExclusaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoExclusao;
                    permissaoModuloAlteracaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;
                }

                DateTime dataAula = (!string.IsNullOrEmpty(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_data"].ToString())
                                        ? Convert.ToDateTime(grvAulas.DataKeys[e.Row.RowIndex].Values["tau_data"].ToString()) : new DateTime());

                ImageButton btnFrequencia = (ImageButton)e.Row.FindControl("btnFrequencia");
                ImageButton btnAtividade = (ImageButton)e.Row.FindControl("btnAtividade");
                ImageButton btnAnotacao = (ImageButton)e.Row.FindControl("btnAnotacao");
                ImageButton btnPlanoAula = (ImageButton)e.Row.FindControl("btnPlanoAula");
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                LinkButton lnkDataAulaAlterar = (LinkButton)e.Row.FindControl("lnkDataAulaAlterar");
                Label lblAula = (Label)e.Row.FindControl("lblAula");
                Image imgEventoSemAtividade = (Image)e.Row.FindControl("imgEventoSemAtividade");
                Label lblAulaReposicao = (Label)e.Row.FindControl("lblAulaReposicao");
                Image imgAvisoSubstituto = (Image)e.Row.FindControl("imgAvisoSubstituto");

                Guid usu_id = Guid.Empty;
                if (!string.IsNullOrEmpty(grvAulas.DataKeys[e.Row.RowIndex].Values["usu_id"].ToString()))
                    usu_id = (Guid)grvAulas.DataKeys[e.Row.RowIndex].Values["usu_id"];

                if (VS_situacaoTurmaDisciplina != 1 && usu_id != __SessionWEB.__UsuarioWEB.Usuario.usu_id)
                    permissaoAlteracao = false;

                if ((!((permissaoAlteracao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao))
                     ||
                     (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 &&
                      !grvAulas.DataKeys[e.Row.RowIndex].Values["usu_id"].ToString().ToLower().Equals(__SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString().ToLower())
                     )
                     ||
                     // somente exibo o nome do titular na aula quando a aula não foi criada pelo titular logado.
                     (permissaoAlteracao && permissaoModuloAlteracaoInfantil && (tdc_id == (byte)EnumTipoDocente.Titular || tdc_id == (byte)EnumTipoDocente.SegundoTitular)
                       && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && usu_id != __SessionWEB.__UsuarioWEB.Usuario.usu_id
                     )
                   )
                {
                    string tipoDocente = ACA_TipoDocenteBO.ListaTipoDocentes().ToList().Find(p => p.Key == tdc_id).Value;
                    e.Row.Style["background-color"] = tdc_corDestaque;
                    if (!string.IsNullOrEmpty(pes_nome))
                    {
                        if (string.IsNullOrEmpty(lnkDataAulaAlterar.Text) && dataAula != new DateTime())
                        {
                            lnkDataAulaAlterar.Text = dataAula.ToString("dd/MM/yyyy");
                        }

                        lnkDataAulaAlterar.Text += String.Format("<br />Docente: {0} - {1}", pes_nome, tipoDocente);
                    }
                    else
                    {
                        lnkDataAulaAlterar.Text += String.Format("<br />Docente: {0}", tipoDocente);
                    }
                    lblAula.Text = lnkDataAulaAlterar.Text;
                }
                else if (permissaoAlteracao && permissaoModuloAlteracaoInfantil && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && usu_id == __SessionWEB.__UsuarioWEB.Usuario.usu_id && tdt_posicao != UCControleTurma1.VS_tdt_posicao)
                {
                    e.Row.Style["background-color"] = tdc_corDestaque;
                }

                imgEventoSemAtividade.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemEventoSemAtivDiscente").ToString();
                imgEventoSemAtividade.Visible = Convert.ToBoolean(grvAulas.DataKeys[e.Row.RowIndex].Values["EventoSemAtividade"]);

                bool docenciaCompartilhada = Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tdc_id"]) == (byte)EnumTipoDocente.Projeto
                                             || Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tdc_id"]) == (byte)EnumTipoDocente.Compartilhado;

                if (imgEventoSemAtividade.Visible)
                    divEventoSemAtividade.Visible = true;

                // Caso a aula foi compartilhada com outra disciplina,
                // exibo o nome da disciplina relacionada
                if (!String.IsNullOrEmpty(grvAulas.DataKeys[e.Row.RowIndex].Values["NomeDisciplinaRelacionada"].ToString()))
                {
                    if (Convert.ToByte(grvAulas.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                    {
                        if (VS_EntitiesControleTurma.turmaDisciplina.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                        {
                            lnkDataAulaAlterar.Text += String.Format("<br />{0} - {1}", VS_tud_nome, grvAulas.DataKeys[e.Row.RowIndex].Values["NomeDisciplinaRelacionada"].ToString());
                        }
                        else
                        {
                            lnkDataAulaAlterar.Text += String.Format("<br />{0} - {1}", grvAulas.DataKeys[e.Row.RowIndex].Values["NomeDisciplinaRelacionada"].ToString(), VS_tud_nome);
                        }
                        lblAula.Text = lnkDataAulaAlterar.Text;
                    }
                    else
                    {
                        lnkDataAulaAlterar.Text += String.Format("<br />{0}", grvAulas.DataKeys[e.Row.RowIndex].Values["NomeDisciplinaRelacionada"].ToString());
                        lblAula.Text = lnkDataAulaAlterar.Text;
                    }
                }

                if (btnFrequencia != null)
                {
                    btnFrequencia.Visible = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tud_naoLancarFrequencia")) == 0;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        btnFrequencia.Visible &= VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao));
                    }
                    else if (docenciaCompartilhada)
                    {
                        btnFrequencia.Visible = false;
                    }

                    Image imgFrequenciaSituacaoEfetivada = (Image)e.Row.FindControl("imgFrequenciaSituacaoEfetivada");
                    if (imgFrequenciaSituacaoEfetivada != null)
                        imgFrequenciaSituacaoEfetivada.Visible = btnFrequencia.Visible && statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Efetivada;

                    Image imgFrequenciaSituacao = (Image)e.Row.FindControl("imgFrequenciaSituacao");
                    if (imgFrequenciaSituacao != null)
                        imgFrequenciaSituacao.Visible = btnFrequencia.Visible && statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Preenchida;
                }

                if (btnAtividade != null)
                {
                    btnAtividade.Visible = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tud_naoLancarNota")) == 0;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        btnAtividade.Visible &= VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao));
                    }
                    else if (docenciaCompartilhada)
                    {
                        btnAtividade.Visible = false;
                    }

                    Image imgAtividadeSituacaoEfetivada = (Image)e.Row.FindControl("imgAtividadeSituacaoEfetivada");
                    if (imgAtividadeSituacaoEfetivada != null)
                        imgAtividadeSituacaoEfetivada.Visible = btnAtividade.Visible && statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada;

                    Image imgAtividadeSituacao = (Image)e.Row.FindControl("imgAtividadeSituacao");
                    if (imgAtividadeSituacao != null)
                        imgAtividadeSituacao.Visible = btnAtividade.Visible && statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida;
                }

                if (btnAnotacao != null)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        btnAnotacao.Visible = VS_ltPermissaoAnotacoes.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao));
                    }

                    Image imgAnotacaoSituacao = (Image)e.Row.FindControl("imgAnotacaoSituacao");
                    if (imgAnotacaoSituacao != null)
                        imgAnotacaoSituacao.Visible = btnAnotacao.Visible && statusAnotacoes == (byte)CLS_TurmaAulaStatusAnotacoes.Preenchida;
                }

                if (btnPlanoAula != null)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        btnPlanoAula.Visible = VS_ltPermissaoPlanoAula.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao));
                    }

                    Image imgPlanoAulaSituacao = (Image)e.Row.FindControl("imgPlanoAulaSituacao");
                    Image imgPlanoAulaSituacaoIncompleta = (Image)e.Row.FindControl("imgPlanoAulaSituacaoIncompleta");
                    if (imgPlanoAulaSituacao != null && imgPlanoAulaSituacaoIncompleta != null)
                    {
                        imgPlanoAulaSituacao.Visible = statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Preenchida;
                        imgPlanoAulaSituacaoIncompleta.Visible = statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Incompleto;
                    }

                    // Apenas aulas dos dias anteriores sem plano de aula devem exibir o aviso.
                    Image imgSemPlanoAula = (Image)e.Row.FindControl("imgSemPlanoAula");
                    if (imgSemPlanoAula != null && dataAula.Date < DateTime.Now.Date &&
                        UCNavegacaoTelaPeriodo.VS_tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    { 
                        imgSemPlanoAula.Visible = semPlanoAula
                                                    && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                                        || VS_EntitiesControleTurma.curso.tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                        || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                        imgSemPlanoAula.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.imgSemPlanoAula").ToString();
                    }

                    if (imgSemPlanoAula.Visible)
                        divAvisoAulaSemPlano.Visible = true;
                }

                if (btnExcluir != null)
                {
                    if ((permissaoAlteracao && permissaoModuloExclusaoInfantil) || permissaoModuloExclusao)
                    {
                        btnExcluir.Visible = !VS_PeriodoEfetivado;

                        string sDataAula = "Confirma a exclusão da aula do dia " + dataAula.ToString("dd/MM/yyyy");
                        string script = "SetConfirmDialog('#" + btnExcluir.ClientID + "','" + sDataAula + " ?');";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), btnExcluir.ClientID, script, true);
                    }
                    else
                    {
                        CFG_PermissaoModuloOperacao permissaoModuloOperacao = new CFG_PermissaoModuloOperacao()
                        {
                            gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            sis_id = ApplicationWEB.SistemaID,
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseExclusaoAulas)
                        };
                        CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloOperacao);

                        if (permissaoModuloOperacao.pmo_permissaoExclusao)
                        {
                            VS_PermissaoExcluirDiretor = true;
                            btnExcluir.Visible = true;
                        }
                        else
                        {
                            btnExcluir.Visible = false;
                        }
                    }
                }

                if (lnkDataAulaAlterar != null && lblAula != null)
                {
                    lnkDataAulaAlterar.Visible = ((permissaoAlteracao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao) && !VS_PeriodoEfetivado;
                    lblAula.Visible = !((permissaoAlteracao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao) || VS_PeriodoEfetivado;
                }

                if (tau_reposicao)
                {   // exibe a literal "- Aula de reposição"
                    lblAulaReposicao.Visible = tau_reposicao;
                }

                string msgSubstituto = DataBinder.Eval(e.Row.DataItem, "mensagemSubstituto").ToString();

                if (!string.IsNullOrEmpty(msgSubstituto))
                {
                    if (imgAvisoSubstituto != null)
                    {
                        imgAvisoSubstituto.ToolTip = msgSubstituto;
                        imgAvisoSubstituto.Visible = true;
                    }

                    divAvisoSubstituto.Visible = true;
                }
            }
        }

        protected void grvAulas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int linha = Convert.ToInt32(e.CommandArgument);
                VS_grvRow = linha;
                VS_tud_id_Aula = Convert.ToInt32(grvAulas.DataKeys[linha].Values["tud_id"]);
                VS_tud_tipo_Aula = Convert.ToByte(grvAulas.DataKeys[linha].Values["tud_tipo"]);
                VS_fav_tipoApuracaoFrequencia = Convert.ToByte(grvAulas.DataKeys[linha].Values["fav_tipoApuracaoFrequencia"]);
                VS_tau_id = Convert.ToInt32(grvAulas.DataKeys[linha].Values["tau_id"]);
                VS_tau_data = Convert.ToDateTime(grvAulas.DataKeys[linha].Values["tau_data"]);
                VS_tdt_posicaoEdicao = Convert.ToByte(grvAulas.DataKeys[linha].Values["tdt_posicao"]);
                VS_permissaoAlteracao = Convert.ToInt16(grvAulas.DataKeys[linha].Values["permissaoAlteracao"].ToString()) > 0;
                VS_tud_nome_Aula = VS_tud_id_Aula != UCControleTurma1.VS_tud_id && !String.IsNullOrEmpty(grvAulas.DataKeys[linha].Values["NomeDisciplinaRelacionada"].ToString())
                                    ? grvAulas.DataKeys[linha].Values["NomeDisciplinaRelacionada"].ToString() : VS_tud_nome;
                VS_tud_global_Aula = Convert.ToBoolean(grvAulas.DataKeys[linha].Values["tud_global"].ToString());

                if (!string.IsNullOrEmpty(grvAulas.DataKeys[linha].Values["usu_id"].ToString()))
                {
                    VS_usu_id = new Guid(grvAulas.DataKeys[linha].Values["usu_id"].ToString());
                }

                switch (e.CommandName)
                {
                    case "LancarFrequencia":
                        CarregarAlunosFrequencia();
                        break;

                    case "AnotacoesAluno":
                        CarregarAnotacoesAluno();
                        break;

                    case "Atividade":
                        CarregarAtividades(true);
                        break;

                    case "PlanoAula":
                        CarregarDisciplinasCombo(UCControleTurma1.VS_tur_id, VS_tud_id_Aula);
                        break;

                    case "ExcluirAula":
                        if (VS_PermissaoExcluirDiretor)
                        {
                            byte statusFrequencia = Convert.ToByte(grvAulas.DataKeys[linha].Values["tau_statusFrequencia"].ToString());
                            byte statusAtividadeAvaliativa = Convert.ToByte(grvAulas.DataKeys[linha].Values["tau_statusAtividadeAvaliativa"].ToString());
                            byte statusAnotacoes = Convert.ToByte(grvAulas.DataKeys[linha].Values["tau_statusAnotacoes"].ToString());
                            byte statusPlanoAula = Convert.ToByte(grvAulas.DataKeys[linha].Values["tau_statusPlanoAula"].ToString());

                            if (statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Preenchida ||
                               statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida ||
                               statusAnotacoes == (byte)CLS_TurmaAulaStatusAnotacoes.Preenchida ||
                               statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Preenchida ||
                               statusFrequencia == (byte)CLS_TurmaAulaStatusFrequencia.Efetivada ||
                               statusAtividadeAvaliativa == (byte)CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada ||
                               statusPlanoAula == (byte)CLS_TurmaAulaStatusPlanoAula.Incompleto)
                            {
                                throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemNaoEPossivelExcluirAula").ToString());
                            }
                            else
                            {
                                ddlTipoJustificativaExclusaoAula.Items.Clear();
                                List<ACA_TipoJustificativaExclusaoAulas> lstTipoJustificativaExclusaoAulas = ACA_TipoJustificativaExclusaoAulasBO.GetSelectAtivos();
                                ddlTipoJustificativaExclusaoAula.DataSource = lstTipoJustificativaExclusaoAulas;
                                ddlTipoJustificativaExclusaoAula.DataBind();
                                ddlTipoJustificativaExclusaoAula.Items.Insert(0, new ListItem("-- Selecione uma justificativa --", "-1", true));

                                ddlTipoJustificativaExclusaoAula.SelectedIndex = 0;
                                txtObservacaoExclusaoAula.Text = string.Empty;

                                updConfirmacaoExclusaoAulaDiretor.Update();

                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmarExclusaoAula", "$('#divConfirmacaoExclusaoAulaDiretor').dialog('open');", true);
                            }
                        }
                        else
                        {
                            ExcluirAula(VS_tud_id_Aula, VS_tau_id);
                        }
                        break;

                    case "EditarAula":
                        EditarAula(VS_tud_id_Aula, UCNavegacaoTelaPeriodo.VS_tpc_id, VS_tau_id);
                        break;

                    default:
                        break;
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rpDiarioAulasHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Se for cabeçalho, setar valor do checkbox.
                CheckBox chkEfetivado = (CheckBox)e.Item.FindControl("chkEfetivado");

                //Verifica se o lançamento de frequência foi efetivado.
                if (chkEfetivado != null)
                {
                    bool tau_efetivado = false;
                    //bool permissaoAlteracao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;

                    if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tau_efetivado").ToString()))
                    {
                        tau_efetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tau_efetivado"));
                    }

                    chkEfetivado.Checked = tau_efetivado;

                    if (UCControleTurma1.VS_tdt_posicao > 0)
                    {
                        Int16 tdt_posicao = Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "tdt_posicao"));
                        bool permiteEditar = VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao & p.pdc_permissaoEdicao);
                        chkEfetivado.Enabled &= permiteEditar;
                    }

                    chkEfetivado.Enabled &= ((usuarioPermissao && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao)) 
                                                || (!PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao)) && VS_Periodo_Aberto;//&& permissaoAlteracao;
                }
            }
        }

        protected void rptDiarioAlunosFrequencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDiarioAulas = (Repeater)e.Item.FindControl("rptDiarioAulas");
                Repeater rptDiarioAulasEfetivado = (Repeater)e.Item.FindControl("rptDiarioAulasEfetivado");
                //DataTable dtAulas;
                List<AulasAlunos> dados;

                bool AlunoDispensado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlunoDispensado"));

                if (e.Item.ItemType == ListItemType.Header)
                {
                    // Carrega o cabeçalho com os nomes das Aulas.
                    dados = (from aula in VS_Aulas_Alunos
                             group aula by aula.tau_id
                                 into g
                             let tdt_posicao = g.First().tdt_posicao
                             where __SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao))
                             select new AulasAlunos
                             {
                                 tau_id = g.Key
                                 ,
                                 tau_data = g.First().tau_data
                                 ,
                                 tau_efetivado = g.First().tau_efetivado
                                 ,
                                 AlunoDispensado = AlunoDispensado
                                 ,
                                 tdt_posicao = tdt_posicao
                                 //,
                                 //permissaoAlteracao = Convert.ToInt32(g.First()["permissaoAlteracao"]) > 0
                             }).ToList();
                }
                else
                {
                    long Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                    int Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));

                    Int32 mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));
                    //HtmlControl tdAvaliacaoAluno = (HtmlControl)e.Item.FindControl("tdAvaliacaoAluno");

                    HtmlControl tdNomeAluno = (HtmlControl)e.Item.FindControl("tdNomeAluno");
                    HtmlControl tdNumeroChamada = (HtmlControl)e.Item.FindControl("tdNumeroChamada");
                    int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                    if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        tdNumeroChamada.Style["background-color"] = tdNomeAluno.Style["background-color"] = ApplicationWEB.AlunoInativo;
                        //tdAvaliacaoAluno.Style["background-color"] =
                    }

                    dados = (from aula in VS_Aulas_Alunos
                             let tdt_posicao = aula.tdt_posicao
                             where
                                 aula.alu_id == Alu_id
                                 && aula.mtu_id == Mtu_id
                                 && aula.mtd_id == mtd_id
                                 && (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao)))
                             select new AulasAlunos
                             {
                                 tau_id = aula.tau_id
                                              ,
                                 tau_data = aula.tau_data
                                              ,
                                 taa_frequencia = aula.taa_frequencia
                                              ,
                                 taa_frequenciaBitMap = aula.taa_frequenciaBitMap
                                              ,
                                 tau_efetivado = aula.tau_efetivado
                                              ,
                                 tau_numeroAulas = aula.tau_numeroAulas
                                              ,
                                 tdt_posicao = tdt_posicao
                                              ,
                                 falta_justificada = aula.falta_justificada
                                              ,
                                 AlunoDispensado = AlunoDispensado
                                 //,
                                 //permissaoAlteracao = Convert.ToInt32(dr["permissaoAlteracao"]) > 0
                                 ,
                                 mtd_situacao = situacao
                             }).ToList();

                    // Seta as datas de matrícula e saída para serem usadas no databind de Aulas.
                    mtd_dataMatriculaAluno = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataMatricula"));
                    mtd_dataSaidaAluno = DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida") != null ? Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida")) : DateTime.MaxValue;
                }

                rptDiarioAulas.DataSource = dados;
                rptDiarioAulas.DataBind();

                if (rptDiarioAulasEfetivado != null)
                {
                    rptDiarioAulasEfetivado.DataSource = dados;
                    rptDiarioAulasEfetivado.DataBind();
                }
            }
        }

        protected void rptDiarioAlunosFrequenciaTerriorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDiarioAulas = (Repeater)e.Item.FindControl("rptDiarioAulas");
                Repeater rptDiarioAulasEfetivado = (Repeater)e.Item.FindControl("rptDiarioAulasEfetivado");
                //DataTable dtAulas;

                bool AlunoDispensado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlunoDispensado"));

                if (e.Item.ItemType == ListItemType.Header)
                {
                    // Carrega o cabeçalho com os nomes das Aulas.
                    var dados = (from aula in VS_Aulas_Alunos
                                 where aula.tau_id == VS_tau_id
                                 group aula by aula.tau_id
                                 into g
                                 let tdt_posicao = g.First().tdt_posicao
                                 where __SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao))
                                 select new
                                 {
                                     tau_id = g.Key
                                     ,
                                     tau_data = g.First().tau_data
                                     ,
                                     tau_efetivado = g.First().tau_efetivado
                                     ,
                                     AlunoDispensado = AlunoDispensado
                                     ,
                                     tdt_posicao = tdt_posicao
                                     //,
                                     //permissaoAlteracao = Convert.ToInt32(g.First()["permissaoAlteracao"]) > 0
                                 }).ToList();

                    rptDiarioAulas.DataSource = dados;
                    rptDiarioAulas.DataBind();

                    if (rptDiarioAulasEfetivado != null)
                    {
                        rptDiarioAulasEfetivado.DataSource = dados;
                        rptDiarioAulasEfetivado.DataBind();
                    }
                }
                else
                {
                    long Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                    int Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));

                    Int32 mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));
                    //HtmlControl tdAvaliacaoAluno = (HtmlControl)e.Item.FindControl("tdAvaliacaoAluno");

                    HtmlControl tdNomeAluno = (HtmlControl)e.Item.FindControl("tdNomeAluno");
                    HtmlControl tdNumeroChamada = (HtmlControl)e.Item.FindControl("tdNumeroChamada");
                    int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                    if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        tdNumeroChamada.Style["background-color"] = tdNomeAluno.Style["background-color"] = ApplicationWEB.AlunoInativo;
                        //tdAvaliacaoAluno.Style["background-color"] =
                    }

                    var dados = (from aula in VS_Aulas_Alunos
                                 let tdt_posicao = aula.tdt_posicao
                                 where
                                     aula.alu_id == Alu_id
                                     && aula.mtu_id == Mtu_id
                                     && (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || VS_ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao)))
                                 group aula by new { alu_id = aula.alu_id, mtu_id = aula.mtu_id } into gAulas
                                 select new
                                 {
                                     alu_id = gAulas.Key.alu_id
                                     ,
                                     mtu_id = gAulas.Key.mtu_id
                                     ,
                                     tdt_posicao = gAulas.First().tdt_posicao
                                     ,
                                     AlunoDispensado = AlunoDispensado
                                     ,
                                     mtd_situacao = situacao
                                     ,
                                     aulas = gAulas.ToList()
                                 }).ToList();

                    // Seta as datas de matrícula e saída para serem usadas no databind de Aulas.
                    mtd_dataMatriculaAluno = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataMatricula"));
                    mtd_dataSaidaAluno = DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida") != null ? Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida")) : DateTime.MaxValue;

                    rptDiarioAulas.DataSource = dados;
                    rptDiarioAulas.DataBind();

                    if (rptDiarioAulasEfetivado != null)
                    {
                        rptDiarioAulasEfetivado.DataSource = dados;
                        rptDiarioAulasEfetivado.DataBind();
                    }
                }
            }
        }

        protected void btnSalvarAula_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || SalvarAula())
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "GerarAulasVar", "var exibirMensagemConfirmacao=false;", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "GerarAulas", "$(document).ready(function() { var exibirMensagemConfirmacao=false;$('#divCadastroAula').dialog('close'); });", true);
                hdfIsNewAula.Value = "true";
            }
        }

        protected void btnSalvarFrequencia_Click(object sender, EventArgs e)
        {
            RepeaterItem header;
            Repeater rptDiarioAulas;
            if (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia)
            {
                header = (RepeaterItem)rptDiarioAlunosFrequenciaTerriorio.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }
            else
            {
                header = (RepeaterItem)rptDiarioAlunosFrequencia.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }


            if (!rptDiarioAulas.Items.Cast<RepeaterItem>().Select(p => (CheckBox)p.FindControl("chkEfetivado")).Any(p => p != null && p.Checked))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmarLancamento", "$('#divConfirmarLancamento').dialog('open');", true);
            }
            else
            {
                if (Page.IsValid)
                {
                    SalvarFrequencia();
                }
            }
        }

        protected void btnEfetivarLancamento_Click(object sender, EventArgs e)
        {
            RepeaterItem header;
            Repeater rptDiarioAulas;
            if (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia)
            {
                header = (RepeaterItem)rptDiarioAlunosFrequenciaTerriorio.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }
            else
            {
                header = (RepeaterItem)rptDiarioAlunosFrequencia.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }
            CheckBox chkEfetivado = new CheckBox();

            foreach (RepeaterItem itemAula in rptDiarioAulas.Items)
            {
                chkEfetivado = (CheckBox)itemAula.FindControl("chkEfetivado");
                if (chkEfetivado != null)
                {
                    chkEfetivado.Checked = true;
                }
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmarLancamento", "var exibirMensagemConfirmacao=false;$('#divConfirmarLancamento').dialog('close');", true);

            updConfirmarLancamento.Update();

            SalvarFrequencia();
        }

        protected void btnSalvarLancamento_Click(object sender, EventArgs e)
        {
            RepeaterItem header;
            Repeater rptDiarioAulas;
            if (VS_tud_tipo_Aula == (byte)TurmaDisciplinaTipo.Experiencia)
            {
                header = (RepeaterItem)rptDiarioAlunosFrequenciaTerriorio.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }
            else
            {
                header = (RepeaterItem)rptDiarioAlunosFrequencia.Controls[0];
                rptDiarioAulas = (Repeater)header.FindControl("rptDiarioAulasEfetivado");
            }

            CheckBox chkEfetivado = new CheckBox();

            foreach (RepeaterItem itemAula in rptDiarioAulas.Items)
            {
                chkEfetivado = (CheckBox)itemAula.FindControl("chkEfetivado");
                if (chkEfetivado != null)
                {
                    chkEfetivado.Checked = false;
                }
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmarLancamento", "var exibirMensagemConfirmacao=false;$('#divConfirmarLancamento').dialog('close');", true);

            updConfirmarLancamento.Update();

            SalvarFrequencia();
        }

        protected void grvAnotacaoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string alu_mtu_mtd_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "alu_mtu_mtd_id"));
                string nomeAluno = "";

                string nomeUsuarioAlteracao = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nomeUsuAlteracao"));

                DropDownList ddlAnotacaoAluno = (DropDownList)e.Row.FindControl("ddlAnotacaoAluno");

                Label lblIdsAnotAlu = (Label)e.Row.FindControl("lblIdsAnotAlu");
                CustomValidator cpvAluno = (CustomValidator)e.Row.FindControl("cpvAluno");

                if (ddlAnotacaoAluno != null && ddlAnotacaoAluno.Items.Count <= 0)
                {
                    var x = from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                            where atd.mtd_numeroChamada <= 0 && atd.mtd_dataMatricula <= VS_tau_data &&
                                  (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                            select atd.mtd_numeroChamada;

                    if (x.Count() > 0)
                    {
                        var dtOrdenadoPorNome = (from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                                                 where atd.mtd_dataMatricula <= VS_tau_data && (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                                                 orderby atd.pes_nome ascending
                                                 select new
                                                 {
                                                     Nome = atd.pes_nome
                                                     ,
                                                     atd.alu_mtu_mtd_id
                                                 }).ToList();

                        ddlAnotacaoAluno.DataSource = dtOrdenadoPorNome;
                    }
                    else
                    {
                        var dtOrdenadoPorChamada = (from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                                                    where atd.mtd_dataMatricula <= VS_tau_data && (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                                                    orderby atd.numeroChamada ascending
                                                    select new
                                                    {
                                                        atd.Nome
                                                        ,
                                                        atd.alu_mtu_mtd_id
                                                    }).ToList();

                        ddlAnotacaoAluno.DataSource = dtOrdenadoPorChamada;
                    }

                    ddlAnotacaoAluno.Items.Insert(0, new ListItem("-- Selecione um aluno --", "-1;-1;-1", true));
                    ddlAnotacaoAluno.DataBind();

                    if (!string.IsNullOrEmpty(alu_mtu_mtd_id))
                    {
                        if (lblIdsAnotAlu != null)
                            lblIdsAnotAlu.Text = alu_mtu_mtd_id;

                        // Pesquisa o registro do aluno com os ids passados.
                        AlunosTurmaDisciplina alunoTurmaDisciplina = (from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                                                                      where atd.alu_mtu_mtd_id == alu_mtu_mtd_id && atd.mtd_dataMatricula <= VS_tau_data &&
                                                                            (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                                                                      select atd).FirstOrDefault();

                        if (alunoTurmaDisciplina.Nome != null)
                            nomeAluno = alunoTurmaDisciplina.Nome;
                    }

                    ddlAnotacaoAluno.Visible = string.IsNullOrEmpty(alu_mtu_mtd_id) || alu_mtu_mtd_id == "-1;-1;-1";

                    Label lblNomeAluno = (Label)e.Row.FindControl("lblNomeAluno");
                    if (lblNomeAluno != null)
                    {
                        if (string.IsNullOrEmpty(lblNomeAluno.Text))
                            lblNomeAluno.Text = nomeAluno;
                        lblNomeAluno.Visible = !string.IsNullOrEmpty(alu_mtu_mtd_id) && alu_mtu_mtd_id != "-1;-1;-1";
                        //cpvAluno.Visible = string.IsNullOrEmpty(alu_mtu_mtd_id) || alu_mtu_mtd_id == "-1;-1;-1";
                    }
                }

                TextBox txtAnotacao = (TextBox)e.Row.FindControl("txtAnotacao");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "TamanhoCampoAnotacoes" + txtAnotacao.ClientID, "$(document).ready(function(){ LimitarCaracter(" + txtAnotacao.ClientID + ",'contadesc3','4000'); });", true);

                // monto o nome do usuário que fez a última alteração
                Label lblAlteracaoAnotacoes = (Label)e.Row.FindControl("lblAlteracaoAnotacoes");

                CheckBoxList cblAnotacoesPredefinidas = (CheckBoxList)e.Row.FindControl("cblAnotacoesPredefinidas");
                DataTable dtTipoAnotacao = ACA_TipoAnotacaoAlunoBO.SelecionarTipoAnotacaoAluno_ent_id(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                cblAnotacoesPredefinidas.DataSource = dtTipoAnotacao;
                cblAnotacoesPredefinidas.DataBind();

                grvAnotacaoAluno.Columns[grvAnotacaoAluno_ColunaTipoAnotacao].Visible = dtTipoAnotacao.Rows.Count > 0;

                #region Tipo de Anotacoes

                List<CLS_TurmaAulaAlunoTipoAnotacao> listaAnotacoesAluno = new List<CLS_TurmaAulaAlunoTipoAnotacao>();

                //Caso exista a coluna "tia_ids" no DataSource significa que os dados estao vindo de uma alteração na tela
                //e nao do botao de carregar as anotacoes da aula
                if (((DataRowView)e.Row.DataItem).DataView.Table.Columns.Contains("tia_ids"))
                {
                    //Pega os dados que o usuario alterou
                    string tia_ids = DataBinder.Eval(e.Row.DataItem, "tia_ids").ToString();

                    if (!string.IsNullOrEmpty(tia_ids))
                        listaAnotacoesAluno.AddRange(
                                from id in tia_ids.Split(';')
                                select new CLS_TurmaAulaAlunoTipoAnotacao
                                {
                                    tia_id = Convert.ToInt32(id)
                                }
                            );
                }
                else
                {
                    //Pega os dados salvos no banco
                    listaAnotacoesAluno = listTurmaAulaTipoAnotacao.Where(
                                p => p.alu_id == Convert.ToInt64(alu_mtu_mtd_id.Split(';')[0])
                                    && p.mtu_id == Convert.ToInt32(alu_mtu_mtd_id.Split(';')[1])
                                    && p.mtd_id == Convert.ToInt32(alu_mtu_mtd_id.Split(';')[2])).ToList();
                }

                foreach (ListItem cb in cblAnotacoesPredefinidas.Items)
                    cb.Selected = listaAnotacoesAluno.Exists(p => p.tia_id == Convert.ToInt32(cb.Value));

                #endregion

                VS_nome_usu_alteracao = nomeUsuarioAlteracao;

                if (!string.IsNullOrEmpty(VS_nome_usu_alteracao))
                {
                    VS_DataAlteracaoRegistro = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "dataAlteracao"));
                }

                lblAlteracaoAnotacoes.Text = CarregarUsuarioAlteracao();
                lblAlteracaoAnotacoes.Visible = !string.IsNullOrEmpty(lblAlteracaoAnotacoes.Text);
            }
        }

        protected void grvAnotacaoAluno_DataBinding(object sender, EventArgs e)
        {
            listTurmaAulaTipoAnotacao = CLS_TurmaAulaAlunoTipoAnotacaoBO.SelecionaPorTurmaAula(VS_tud_id_Aula, VS_tau_id);
        }

        protected void grvAnotacoesMaisdeUmAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string alu_mtu_mtd_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "alu_mtu_mtd_id"));

                CheckBoxList cblAnotacaoAluno = (CheckBoxList)e.Row.FindControl("cblAnotacaoAluno");

                if (cblAnotacaoAluno != null && cblAnotacaoAluno.Items.Count <= 0)
                {
                    var x = from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                            where atd.mtd_numeroChamada <= 0 && atd.mtd_dataMatricula <= VS_tau_data &&
                                  (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                            select atd.mtd_numeroChamada;

                    if (x.Count() > 0)
                    {
                        var dtOrdenadoPorNome = (from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                                                 where atd.mtd_dataMatricula <= VS_tau_data && (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                                                 orderby atd.pes_nome ascending
                                                 select new
                                                 {
                                                     Nome = atd.pes_nome
                                                     ,
                                                     atd.alu_mtu_mtd_id
                                                 }).ToList();

                        cblAnotacaoAluno.DataSource = dtOrdenadoPorNome;
                    }
                    else
                    {
                        var dtOrdenadoPorChamada = (from AlunosTurmaDisciplina atd in dtAlunosAnotacoes
                                                    where atd.mtd_dataMatricula <= VS_tau_data && (atd.mtd_dataSaida == null || atd.mtd_dataSaida >= VS_tau_data)
                                                    orderby atd.numeroChamada ascending
                                                    select new
                                                    {
                                                        atd.Nome
                                                        ,
                                                        atd.alu_mtu_mtd_id
                                                    }).ToList();

                        cblAnotacaoAluno.DataSource = dtOrdenadoPorChamada;
                    }

                    cblAnotacaoAluno.DataBind();



                    cblAnotacaoAluno.Visible = string.IsNullOrEmpty(alu_mtu_mtd_id) || alu_mtu_mtd_id == "-1;-1;-1";

                    CheckBoxList cblAnotacoesPredefinidas = (CheckBoxList)e.Row.FindControl("cblAnotacoesPredefinidas");
                    DataTable dtTipoAnotacao = ACA_TipoAnotacaoAlunoBO.SelecionarTipoAnotacaoAluno_ent_id(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    cblAnotacoesPredefinidas.DataSource = dtTipoAnotacao;
                    cblAnotacoesPredefinidas.DataBind();

                    grvAnotacoesMaisdeUmAluno.Columns[grvAnotacoesMaisdeUmAluno_ColunaTipoAnotacao].Visible = dtTipoAnotacao.Rows.Count > 0;
                }

                TextBox txtAnotacao = (TextBox)e.Row.FindControl("txtAnotacao");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "TamanhoCampoAnotacoes" + txtAnotacao.ClientID, "$(document).ready(function(){ LimitarCaracter(" + txtAnotacao.ClientID + ",'contadesc3','4000'); });", true);

                // monto o nome do usuário que fez a última alteração
                Label lblAlteracaoAnotacoes = (Label)e.Row.FindControl("lblAlteracaoAnotacoes");

            }
        }

        protected void ddlAnotacaoAluno_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAnotacaoAluno = (DropDownList)sender;
            GridViewRow linha = (GridViewRow)ddlAnotacaoAluno.NamingContainer;

            // Atribui ao label o valor do item selecionado no drop down.
            Label lblIdsAnotAlu = (Label)linha.FindControl("lblIdsAnotAlu");
            lblIdsAnotAlu.Text = ddlAnotacaoAluno.SelectedValue;
        }

        protected void btnAdicionar_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidarAlunoAnotacao())
                AdicionaLinhaGridAnotacao();
        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                DataTable dt = RetornaDadosGridAnotacao();

                string idsAluno = dt.Rows[row.RowIndex]["alu_mtu_mtd_id"].ToString();
                long alu_id = Convert.ToInt64(idsAluno.Split(';')[0]);
                List<CLS_TurmaAulaAluno> listTurmaAulaAluno = new List<CLS_TurmaAulaAluno>();
                CLS_TurmaAula entityTurmaAula = new CLS_TurmaAula();

                if (alu_id > 0)
                {
                    int mtu_id = Convert.ToInt32(idsAluno.Split(';')[1]);
                    int mtd_id = Convert.ToInt32(idsAluno.Split(';')[2]);

                    CLS_TurmaAulaAluno aulaAluno = CLS_TurmaAulaAlunoBO.GetEntity(new CLS_TurmaAulaAluno
                    {
                        tud_id = VS_tud_id_Aula
                        ,
                        tau_id = VS_tau_id
                        ,
                        alu_id = alu_id
                        ,
                        mtu_id = mtu_id
                        ,
                        mtd_id = mtd_id
                    });

                    if (!String.IsNullOrEmpty(aulaAluno.taa_anotacao))
                    {
                        aulaAluno.taa_anotacao = string.Empty;
                        listTurmaAulaAluno.Add(aulaAluno);
                    }
                }

                //Deleta linha
                dt.Rows[row.RowIndex].Delete();

                if (listTurmaAulaAluno.Count > 0)
                {
                    entityTurmaAula = CLS_TurmaAulaBO.GetEntity(new CLS_TurmaAula
                    {
                        tud_id = VS_tud_id_Aula,
                        tau_id = VS_tau_id
                    });
                }

                // Adiciona nova linha quando for execluido todos os itens
                if (dt.Rows.Count == 0)
                {
                    // Adiciona nova linha do grid.
                    DataRow dr = dt.NewRow();
                    dr["alu_mtu_mtd_id"] = "-1;-1;-1";
                    dt.Rows.Add(dr);

                    entityTurmaAula.tau_statusAnotacoes = (byte)CLS_TurmaAulaStatusAnotacoes.NaoPreenchida;
                }

                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                    string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                    string.Empty;

                dtAlunosAnotacoes = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina
                            (
                                VS_tud_id_Aula,
                                UCNavegacaoTelaPeriodo.VS_tpc_id,
                                VS_tipoDocente,
                                false,
                                UCNavegacaoTelaPeriodo.cap_dataInicio, 
                                UCNavegacaoTelaPeriodo.cap_dataFim,
                                ApplicationWEB.AppMinutosCacheMedio,
                                tur_ids
                            );

                if (listTurmaAulaAluno.Count == 0 || CLS_TurmaAulaBO.SalvarAulaAnotacoesRecursos(entityTurmaAula, listTurmaAulaAluno, new List<CLS_TurmaAulaRecurso>(), new List<CLS_TurmaAulaAlunoTipoAnotacao>(), null, true))
                {
                    if (alu_id > 0)
                    {
                        lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Anotação excluída com sucesso.", UtilBO.TipoMensagem.Sucesso, "margin: 10px;");

                        if (listTurmaAulaAluno.Count > 0)
                        {
                            // Atualiza a imagem do status da anotacao
                            Image imgSituacao = (Image)grvAulas.Rows[VS_grvRow].FindControl("imgAnotacaoSituacao");
                            if (imgSituacao != null)
                            {
                                if (entityTurmaAula.tau_statusAnotacoes == (byte)CLS_TurmaAulaStatusAnotacoes.NaoPreenchida)
                                {
                                    // se nao tem mais anotacao, escondo o check
                                    imgSituacao.Visible = false;
                                }
                                else
                                {
                                    // senao, verifico se existe anotacao salva
                                    DataTable dtAnotacoesSalvas = CLS_TurmaAulaAlunoBO.SelecionaAnotacaoPorAulaTurmaDisciplina(VS_tud_id_Aula, VS_tau_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    imgSituacao.Visible = dtAnotacoesSalvas.Rows.Count > 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar excluir anotação do aluno.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
                }

                grvAnotacaoAluno.DataSource = dt;
                grvAnotacaoAluno.DataBind();

                if (dt.Rows.Count > 0)
                {
                    int ultimaLinha = grvAnotacaoAluno.Rows.Count - 1;

                    // Torna o botão de adicionar anotação para o aluno visível no último registro.
                    grvAnotacaoAluno.Rows[ultimaLinha].FindControl("btnAdicionar").Visible = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAnotacoes.Text = UtilBO.GetErroMessage("Erro ao tentar excluir anotação do aluno.", UtilBO.TipoMensagem.Erro, "margin: 10px;");
            }
        }

        protected void ddlComponenteAtAvaliativa_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarAtividades(false);
        }

        private void UCComboTipoAtividadeAvaliativa_IndexChanged()
        {
            if (UCComboTipoAtividadeAvaliativa.Valor == 0)
                divNomeAtividade.Visible = true;
            else
                divNomeAtividade.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.NOME_TODAS_ATIVIDADES_AVALIATIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        protected void btnNovaAtividade_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                SalvarNovaAtividade();
        }

        protected void btnEditarAtividade_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                SalvarNovaAtividade();
        }

        protected void btnCancelarAtividade_Click(object sender, EventArgs e)
        {
            CarregarAtividades(true);
        }

        protected void btnSalvarNota_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                SalvarNotasAvaliacao();
        }

        protected void rptAlunos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                // Altera o texto do nome do aluno de acordo com a data de matrícula e saída.
                SetaNomeAluno(e);

                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                int mtd_situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                HtmlTableCell tdNumChamadaAtivAva = (HtmlTableCell)e.Item.FindControl("tdNumChamadaAtivAva");
                HtmlTableCell tdNomeAtivAva = (HtmlTableCell)e.Item.FindControl("tdNomeAtivAva");

                if (mtd_situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    // Pinta célula que possui aluno ausente.
                    if (tdNumChamadaAtivAva != null && tdNomeAtivAva != null)
                    {
                        tdNumChamadaAtivAva.Style["background-color"] = tdNomeAtivAva.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                }

                // Carrega repeater de atividades e avaliações da secretaria.
                CarregaRepeatersInternosAtAvaliativa(e, alu_id);
            }
        }

        protected void rptAtividadesHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Se for cabeçalho, setar valor do checkbox.
                CheckBox chkEfetivado = (CheckBox)e.Item.FindControl("chkEfetivado");

                // Habilita os controles de acordo com a posição do docente.
                Int16 tdt_posicao = Convert.ToInt16(((Label)e.Item.FindControl("lblPosicao")).Text);
                //bool permiteEditar = VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao);

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                // se permite alteração ou não for o docente logado, for um perfil superior
                bool permissaoAlteracao = (VS_permissaoAlteracao && ((Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0 || __SessionWEB.__UsuarioWEB.Docente.doc_id == 0) && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar));

                // Se for um docente que está vendo pelo histórico pois não pertence mais a turma, não permite alterar atividades de outro docente
                // usu_id que criou a atividade for igual ao usu_id logado
                // porém se não for o titular atual que criou, nessa situação ele pode alterar pq ele é o atual
                if (permissaoAlteracao)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }

                if (chkEfetivado != null)
                {
                    bool tnt_efetivado = false;

                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tnt_efetivado").ToString()))
                    {
                        tnt_efetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tnt_efetivado"));
                    }

                    chkEfetivado.Checked = tnt_efetivado;
                    chkEfetivado.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);

                    chkEfetivado.Enabled = usuarioPermissao && VS_Periodo_Aberto && permissaoAlteracao;
                }

                //Verifica possibilidade de editar a avaliação
                ImageButton btnEditarAtividadePopup = (ImageButton)e.Item.FindControl("btnEditarAtividadePopup");

                if (btnEditarAtividadePopup != null)
                    btnEditarAtividadePopup.Visible = permissaoAlteracao;

                ImageButton btnExcluirAtividadePopup = (ImageButton)e.Item.FindControl("btnExcluirAtividadePopup");

                if (btnExcluirAtividadePopup != null)
                {
                    btnExcluirAtividadePopup.Visible = permissaoAlteracao && !VS_PeriodoEfetivado;

                    //string msg = "Excluir " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " \"" + DataBinder.Eval(e.Item.DataItem, "tnt_nome").ToString() + "\"?";
                    //string script = "SetConfirmDialog('#" + btnExcluirAtividadePopup.ClientID + "','" + msg + "');";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), btnExcluirAtividadePopup.ClientID, script, true);
                }
            }
        }

        protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Seta o campo de nota de acordo com o tipo de escala de avaliação.
                TextBox txtNota = (TextBox)e.Item.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)e.Item.FindControl("ddlPareceres");
                ImageButton btnRelatorio = (ImageButton)e.Item.FindControl("btnRelatorio");
                CheckBox chkParticipante = (CheckBox)e.Item.FindControl("chkParticipante");
                CheckBox chkDesconsiderar = (CheckBox)e.Item.FindControl("chkDesconsiderar");

                // Habilita os controles de acordo com a posição do docente.
                // Pinta célula que possui aluno ausente.
                HtmlGenericControl divAtividades = (HtmlGenericControl)e.Item.FindControl("divAtividades");
                Int16 tdt_posicao = Convert.ToInt16(((Label)divAtividades.FindControl("lblPosicao")).Text);

                // se permite alteração ou não for o docente logado, for um perfil superior
                bool permissaoAlteracao = (VS_permissaoAlteracao && (Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0 || __SessionWEB.__UsuarioWEB.Docente.doc_id == 0));
                //bool permiteEditar = (VS_ltPermissaoAvaliacao.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao)) || permissaoAlteracao;

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuIdAtiv2");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                if (permissaoAlteracao)
                {
                    permissaoAlteracao = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }

                string tna_avaliacao = DataBinder.Eval(e.Item.DataItem, "tna_avaliacao").ToString();
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id").ToString());
                int tnt_id = Convert.ToInt32(((Label)e.Item.FindControl("lbltnt_id")).Text);

                EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)VS_EntitiesControleTurma.escalaDocente.escalaAvaliacao.esa_tipo;

                txtNota.Visible = tipo == EscalaAvaliacaoTipo.Numerica;

                chkDesconsiderar.Visible = (tipo == EscalaAvaliacaoTipo.Numerica) && (ltAtividadeIndicacaoNota.Any(p => p.tud_id == tud_id && p.tnt_id == tnt_id && p.PossuiNota)) &&
                                           VS_EntitiesControleTurma.formatoAvaliacao.fav_exibirBotaoSomaMedia;
                if (!chkDesconsiderar.Visible)
                    chkDesconsiderar.Checked = false;

                ddlPareceres.Visible = tipo == EscalaAvaliacaoTipo.Pareceres;
                btnRelatorio.Visible = tipo == EscalaAvaliacaoTipo.Relatorios;

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
                    bool ausente = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "aluno_ausente").ToString());
                    if (ausente)
                    {
                        divAtividades.Style["background-color"] = ApplicationWEB.AlunoAusente;
                    }

                    HabilitaControles(divAtividades.Controls, usuarioPermissao && permissaoAlteracao && VS_Periodo_Aberto);
                }

                //txtNota.Enabled &= permissaoAlteracao;

                // Setar valores.
                Double tnaAvaliacao;
                txtNota.Text = Double.TryParse(tna_avaliacao, out tnaAvaliacao) ? String.Format("{0:F" + numeroCasasDecimais + "}", tnaAvaliacao) : tna_avaliacao;

                ddlPareceres.SelectedValue = tna_avaliacao;

                bool tnt_exclusiva = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tnt_exclusiva").ToString());

                // Setar visibilidade de controles para avaliações exclusivas.
                if (ParametroPermitirAtividadesExclusivas && tnt_exclusiva)
                {
                    chkParticipante.Visible = true;
                    chkParticipante.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tna_participante").ToString());

                    switch (tipo)
                    {
                        case EscalaAvaliacaoTipo.Numerica:
                            txtNota.Enabled = chkParticipante.Checked && permissaoAlteracao;
                            break;

                        case EscalaAvaliacaoTipo.Pareceres:
                            ddlPareceres.Enabled = chkParticipante.Checked && permissaoAlteracao;
                            break;

                        case EscalaAvaliacaoTipo.Relatorios:
                            btnRelatorio.Visible = chkParticipante.Checked && permissaoAlteracao;
                            break;
                    }
                }
                else
                    chkParticipante.Visible = false;

                // Setar relatórios.
                RepeaterItem itemAtividade = e.Item;
                Repeater rptAtividades = (Repeater)itemAtividade.NamingContainer;
                RepeaterItem itemAluno = (RepeaterItem)rptAtividades.NamingContainer;

                long alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                int mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);

                if (tipo == EscalaAvaliacaoTipo.Relatorios)
                {
                    string tna_relatorio = DataBinder.Eval(itemAtividade.DataItem, "tna_relatorio").ToString();
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

                bool alunoAusente = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "aluno_ausente") ?? false));

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

        protected void rptDiarioAulas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CheckBoxList cblFrequencia = (CheckBoxList)e.Item.FindControl("cblFrequencia");

                int frequencia = Convert.ToInt32(String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequencia").ToString()) ? "0" : DataBinder.Eval(e.Item.DataItem, "taa_frequencia"));
                char[] taa_frequenciaBitMap = (String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap").ToString()) ? "" : DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap")).ToString().ToCharArray();
                string sNumeroAulas = DataBinder.Eval(e.Item.DataItem, "tau_numeroAulas").ToString();
                int numeroAulas = string.IsNullOrEmpty(sNumeroAulas) ? 0 : Convert.ToInt32(sNumeroAulas);

                // Verificar se a data da aula está dentro do período da matrícula do aluno (data de matrícula e data de saída).
                string data = DataBinder.Eval(e.Item.DataItem, "tau_data").ToString();
                if (!String.IsNullOrEmpty(data))
                {
                    DateTime tau_data = Convert.ToDateTime(data);

                    cblFrequencia.Visible = ((tau_data.Date >= mtd_dataMatriculaAluno.Date) && (tau_data.Date < mtd_dataSaidaAluno.Date));
                }

                if (cblFrequencia != null)
                {
                    cblFrequencia.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                    bool permissaoModuloAlteracao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao;
                    bool permissaoModuloAlteracaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;

                    if (VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia == (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia &&
                        VS_crp_controleTempo == (byte)ACA_CurriculoPeriodoControleTempo.Horas)
                    {
                        if (VS_PossuiRegencia && VS_tud_tipo_Aula != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            for (int i = 0; i < numeroAulas; i++)
                            {
                                ListItem li = new ListItem();
                                if (taa_frequenciaBitMap.Length > i && taa_frequenciaBitMap[i].Equals('1'))
                                    li.Selected = true;
                                li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                                cblFrequencia.Items.Add(li);
                            }
                        }
                        else
                        {
                            if (numeroAulas > 0)
                            {
                                ListItem li = new ListItem();
                                if (frequencia > 0)
                                    li.Selected = true;
                                li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                                cblFrequencia.Items.Add(li);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numeroAulas; i++)
                        {
                            ListItem li = new ListItem();
                            if (taa_frequenciaBitMap.Length > i && taa_frequenciaBitMap[i].Equals('1'))
                                li.Selected = true;
                            li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                            cblFrequencia.Items.Add(li);
                        }
                    }

                    bool permiteEditar = (VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao;
                    if (permiteEditar)
                    {
                        permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                    }
                    permiteEditar &= VS_Periodo_Aberto;

                    cblFrequencia.Enabled &= permiteEditar;
                }

                bool AlunoDispensado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlunoDispensado"));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui falta justificada.
                    HtmlGenericControl divAulasAluno = (HtmlGenericControl)e.Item.FindControl("divAulasAluno");

                    // Pinta célula que possui aluno dispensado.
                    if (divAulasAluno != null)
                    {
                        divAulasAluno.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    HtmlControl tdAulas = (HtmlControl)e.Item.FindControl("tdAulas");
                    tdAulas.Style["background-color"] = ApplicationWEB.AlunoInativo;
                }
            }
        }

        protected void rptDiarioAulasTerritorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater rptAulasTerritorio = (Repeater)e.Item.FindControl("rptAulasTerritorio");
                List<sTurmaAulaAluno> listaFrequencia = (List<sTurmaAulaAluno>)DataBinder.Eval(e.Item.DataItem, "aulas");
                rptAulasTerritorio.DataSource = listaFrequencia.Where(p => p.tud_id != VS_tud_id_Aula);
                rptAulasTerritorio.DataBind();

                bool AlunoDispensado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlunoDispensado"));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui falta justificada.
                    HtmlGenericControl divAulasAluno = (HtmlGenericControl)e.Item.FindControl("divAulasAluno");

                    // Pinta célula que possui aluno dispensado.
                    if (divAulasAluno != null)
                    {
                        divAulasAluno.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    HtmlControl tdAulas = (HtmlControl)e.Item.FindControl("tdAulas");
                    tdAulas.Style["background-color"] = ApplicationWEB.AlunoInativo;
                }
            }
        }

        protected void btnSalvarAnotacoes_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                SalvarAnotacoesAluno(true);
        }

        protected void btnSalvarAnotacoesMaisdeUmAluno_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                SalvarAnotacoesMaisdeUmAluno();
        }

        protected void btnAdicionarMaisdeUmAluno_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarAnotacoesAluno(false);

                DataTable dt = new DataTable();
                dt.Columns.Add("alu_mtu_mtd_id");
                DataRow dr = dt.NewRow();
                dr["alu_mtu_mtd_id"] = "-1;-1;-1";
                dt.Rows.Add(dr);

                string strDataAula = entityAula.tau_data == new DateTime().Date ? string.Empty : entityAula.tau_data.ToString("dd/MM/yyyy");

                string tur_ids = UCControleTurma1.TurmasNormaisMultisseriadas.Any() ?
                    string.Join(";", UCControleTurma1.TurmasNormaisMultisseriadas.Select(p => p.tur_id.ToString()).ToArray()) :
                    string.Empty;

                dtAlunosAnotacoes =
                       MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina
                           (
                               VS_tud_id_Aula,
                               UCNavegacaoTelaPeriodo.VS_tpc_id,
                               VS_tipoDocente,
                               false,
                               UCNavegacaoTelaPeriodo.cap_dataInicio, 
                               UCNavegacaoTelaPeriodo.cap_dataFim,
                               ApplicationWEB.AppMinutosCacheMedio,
                               tur_ids
                           );

                // Mostra nova linha.
                grvAnotacoesMaisdeUmAluno.DataSource = dt;
                grvAnotacoesMaisdeUmAluno.DataBind();

                int ultimaLinha = grvAnotacoesMaisdeUmAluno.Rows.Count - 1;


                bool permiteEditar = VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                if (permiteEditar)
                {
                    permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                }

                HabilitaControles(grvAnotacoesMaisdeUmAluno.Controls, permiteEditar);
                btnSalvarAnotacoesMaisdeUmAluno.Visible = permiteEditar;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairAnotacoes", "var exibeMensagemSair=" + btnSalvarAnotacoesMaisdeUmAluno.Visible.ToString().ToLower() + ";", true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroAnotacoes",
                    "$('#divAnotacoesMaisdeUmAluno').dialog('option', 'title', 'Anotações sobre os alunos - aula " + strDataAula + "');" +
                    "$('#divAnotacoesMaisdeUmAluno').dialog('open');", true);

                updAnotacoesMaisdeUmAluno.Update();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos cadastro de anotações.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ddlTurmaDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            //se houver a necessidade de usar o ddl o método já esta sendo chamado
            CarregarPlanoAula(VS_tdt_posicaoEdicao);
        }

        protected void ddlTurmaDisciplinaComponente_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparCamposPlanoAula();
            CarregarPlanoAulaRegencia(new CLS_TurmaAulaRegencia());
            CarregarTurmaPeriodo();
            CarregarTurmaAulaOrientacaoCurricular();
        }

        protected void ddlTurmaDisciplinaComponentePlanejamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarTurmaPeriodo();
        }

        protected void btnSalvarPlanoAulaCima_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarPlanoAula();
            }
        }

        protected void btnSalvarPlanoAula_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarPlanoAula();
            }
        }

        protected void btnExcluirAtividadeListao_Click(object sender, EventArgs e)
        {
            ImageButton btnExcluirAtividade = (ImageButton)sender;
            RepeaterItem item = (RepeaterItem)btnExcluirAtividade.NamingContainer;
            Label lbltud_id = (Label)item.FindControl("lbltud_id");
            Label lbltnt_id = (Label)item.FindControl("lbltnt_id");

            if (lbltud_id != null && lbltnt_id != null)
            {
                VS_tud_id_Excluir = Convert.ToInt64(lbltud_id.Text);
                VS_tnt_id_Excluir = Convert.ToInt32(lbltnt_id.Text);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaMovimentacao", "$(document).ready(function(){ $('#divConfirmacao').dialog('open'); });", true);
            }
        }

        protected void btnEditarAtividadePopup_Click(object sender, EventArgs e)
        {
            EditarAtividade(sender);
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
                    (p.alu_id == alu_id) &&
                    (p.tnt_id == tnt_id) &&
                    (p.mtu_id == mtu_id));

                // Guarda o tipo de alteração, o alu_id, o mtu_id e o tnt_id da linha que está sendo editada.
                hdnIds.Value = 1 + ";" + alu_id + ";" + tnt_id + ";" + mtu_id;

                txtRelatorio.Text = rel.valor;

                // Abrir relatório.
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
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

                if (tipoAlteracao == 1)
                {
                    if (VS_Nota_Relatorio.Exists(p =>
                        ((p.tnt_id == id) &&
                        (p.alu_id == alu_id) &&
                        (p.mtu_id == mtu_id))))
                    {
                        int alterar = VS_Nota_Relatorio.FindIndex(p => ((p.tnt_id == id) && (p.alu_id == alu_id) && (p.mtu_id == mtu_id)));

                        List<NotasRelatorio> lista = VS_Nota_Relatorio;

                        lista[alterar] = new NotasRelatorio
                        {
                            valor = txtRelatorio.Text
                            ,
                            tnt_id = id
                            ,
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                        };

                        VS_Nota_Relatorio = lista;
                    }
                    else
                        AdicionaItemRelatorio(id, alu_id, mtu_id, txtRelatorio.Text);

                    // Percorre os itens do repeater para atualizar os botões de relatório.
                    foreach (RepeaterItem item in rptAlunos.Items)
                    {
                        Repeater rptAtividades = (Repeater)item.FindControl("rptAtividades");
                        foreach (RepeaterItem itemAtividade in rptAtividades.Items)
                            SetaImgRelatorio(itemAtividade);
                    }

                    ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ var exibirMensagemConfirmacao=false;$('#divRelatorio').dialog('close'); });", true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void chkRecursos_DataBound(object sender, EventArgs e)
        {
            ListItem outros = new ListItem { Value = "0", Text = "Outros" };
            chkRecursos.Items.Add(outros);
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
                if (litCabecalho != null)
                {
                    litCabecalho.Text = cabecalho;
                }

                lblHabilidade.Text = litConteudo.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                divHabilidade.Visible = permiteLancamento;
                litConteudo.Visible = !permiteLancamento;

                litRodape.Visible = permiteLancamento || (nivelLinha == Nivel);
                litRodape.Text = permiteLancamento ? "</li>" : string.Empty;
                litRodape.Visible = false;

                bool mostraLegenda = false;

                if (permiteLancamento)
                {
                    string chave = DataBinder.Eval(e.Item.DataItem, "Chave").ToString();

                    CheckBox chkPlanejado = (CheckBox)e.Item.FindControl("chkPlanejado");
                    CheckBox chkAlcancado = (CheckBox)e.Item.FindControl("chkAlcancado");
                    CheckBox chkTrabalhado = (CheckBox)e.Item.FindControl("chkTrabalhado");

                    HiddenField hdnChave = (HiddenField)e.Item.FindControl("hdnChave");

                    #region Busca níveis de aprendizado da orientação curricular

                    string[] idsChave = chave.Split(';');
                    long ocrId = idsChave.Length > 1 ? Convert.ToInt64(idsChave[1]) : -1;
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
                            rptLegendaNivelAprendizado.DataSource = dtNivelArendizadoCurriculo;
                            rptLegendaNivelAprendizado.DataBind();

                            if (rptLegendaNivelAprendizado.Items.Count > 0)
                            {
                                divLegendaNivelAprendizado.Visible = true;
                                mostraLegenda = true;
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

                            if (lblLegPlanej != null && lblLegTrab != null)
                            {
                                lblLegPlanej.Visible = true;
                                lblLegTrab.Visible = true;

                                lblLegPlanej.Text = niveisSiglas;
                                lblLegTrab.Text = niveisSiglas;
                            }
                        }
                    }

                    #endregion Busca níveis de aprendizado da orientação curricular

                    if (chkAlcancado != null)
                    {
                        chkAlcancado.CssClass += " alcancado ";
                    }

                    if (chkPlanejado != null && chkTrabalhado != null)
                    {
                        chkPlanejado.CssClass += " planejado ";

                        chkTrabalhado.CssClass += " trabalhado ";

                        if (!chkPlanejado.Checked)
                        {
                            chkTrabalhado.Enabled = false;
                        }

                        string idOrientacao = DataBinder.Eval(e.Item.DataItem, "Id").ToString();

                        chkPlanejado.CssClass += VS_OrientacaoCurricular_PeriodosAnteriores.Any(p => p.id == idOrientacao && p.planejado)
                            ? " planejadoPeriodosAnteriores " : string.Empty;
                        chkTrabalhado.CssClass += VS_OrientacaoCurricular_PeriodosAnteriores.Any(p => p.id == idOrientacao && p.trabalhado)
                            ? " trabalhadoPeriodosAnteriores " : string.Empty;
                    }

                    Image imgSituacao = (Image)e.Item.FindControl("imgSituacao");

                    if (imgSituacao != null)
                    {
                        imgSituacao.Visible = AlcanceEfetivado;
                    }

                    if (hdnChave != null)
                    {
                        hdnChave.Value = chave;
                    }

                    //Verifica se pode desplanejar
                    if (chkPlanejado != null && listHabilidadesComAulaPlanejada.Any(p => p.ocr_id == ocrId && p.tpc_id == UCNavegacaoTelaPeriodo.VS_tpc_id))
                        chkPlanejado.CssClass += " OrientacaoPlanejadaAula ";
                }

                Nivel = nivelLinha;


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
                        Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
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

        private void uccDisciplinaCompartilhada_IndexChanged()
        {
            try
            {
                string[] valor = UCControleTurma1.ValorDisciplinaCompartilhada.Split(';');
                if (valor.Length > 0)
                {
                    VS_turmaDisciplinaRelacionada = new sTurmaDisciplinaRelacionada { tud_id = Convert.ToInt64(valor[0]) };
                    CarregaSessionPaginaRetorno();
                    Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
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

                    Response.Redirect("~/Academico/ControleTurma/DiarioClasse.aspx", false);
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

        protected void btnIncluirAula_Click(object sender, EventArgs e)
        {
            VS_tau_id = -1;
            VS_tud_tipo_Aula = VS_EntitiesControleTurma.turmaDisciplina.tud_tipo;
            VS_fav_tipoApuracaoFrequencia = VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia;
            VS_tud_global_Aula = VS_EntitiesControleTurma.turmaDisciplina.tud_global;
            VS_tud_id_Aula = VS_EntitiesControleTurma.turmaDisciplina.tud_id;
            SetaDisplayCss(btnSalvarAula, true);
            ExibeQuantidadeAulas();
            hdfIsNewAula.Value = "true";
            updCadastroAula.Update();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairIncluirAula", "var exibeMensagemSair=true;", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Nova aula", "AbrirPopUpLimpo('" + (DisciplinaRegencia || DisciplinaExperiencia) + "');", true);
        }

        protected void grvAulas_DataBound(object sender, EventArgs e)
        {
            grvAulas.Columns[grvAulas_ColunaExcluirAula].Visible = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoExclusao))
                                                                    || (!PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoExclusao)) 
                                                                    && !VS_PeriodoEfetivado;
        }

        protected void cpvAluno_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cpvAluno = (CustomValidator)source;
            GridViewRow gvrLinha = (GridViewRow)cpvAluno.NamingContainer;
            Label lblNomeAluno = (Label)gvrLinha.FindControl("lblNomeAluno");
            DropDownList ddlAnotacaoAluno = (DropDownList)gvrLinha.FindControl("ddlAnotacaoAluno");
            TextBox txtAnotacao = (TextBox)gvrLinha.FindControl("txtAnotacao");
            CheckBoxList cblTipos = (CheckBoxList)gvrLinha.FindControl("cblAnotacoesPredefinidas");

            List<ListItem> listChecado
                   = (
                      from ListItem tia in cblTipos.Items
                      where tia.Selected
                      select tia
                     ).ToList();

            bool alunoSelecionado = (ddlAnotacaoAluno != null && !ddlAnotacaoAluno.SelectedValue.Equals("-1;-1;-1") && ddlAnotacaoAluno.Visible)
                                    || (lblNomeAluno != null && !string.IsNullOrEmpty(lblNomeAluno.Text) && lblNomeAluno.Visible);
            bool anotacaoPreenchida = (txtAnotacao != null && !string.IsNullOrEmpty(txtAnotacao.Text)) || listChecado.Count > 0;

            args.IsValid = (alunoSelecionado && anotacaoPreenchida) || (!alunoSelecionado && !anotacaoPreenchida);

            if (!args.IsValid)
                args.IsValid = alunoSelecionado;
        }

        protected void cpvAnotacao_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cpvAluno = (CustomValidator)source;
            GridViewRow gvrLinha = (GridViewRow)cpvAluno.NamingContainer;
            Label lblNomeAluno = (Label)gvrLinha.FindControl("lblNomeAluno");
            DropDownList ddlAnotacaoAluno = (DropDownList)gvrLinha.FindControl("ddlAnotacaoAluno");
            TextBox txtAnotacao = (TextBox)gvrLinha.FindControl("txtAnotacao");
            CheckBoxList cblTipos = (CheckBoxList)gvrLinha.FindControl("cblAnotacoesPredefinidas");

            List<ListItem> listChecado
                   = (
                      from ListItem tia in cblTipos.Items
                      where tia.Selected
                      select tia
                     ).ToList();

            bool alunoSelecionado = (ddlAnotacaoAluno != null && !ddlAnotacaoAluno.SelectedValue.Equals("-1;-1;-1") && ddlAnotacaoAluno.Visible)
                                    || (lblNomeAluno != null && !string.IsNullOrEmpty(lblNomeAluno.Text) && lblNomeAluno.Visible);
            bool anotacaoPreenchida = (txtAnotacao != null && !string.IsNullOrEmpty(txtAnotacao.Text)) || listChecado.Count > 0;

            args.IsValid = (alunoSelecionado && anotacaoPreenchida) || (!alunoSelecionado && !anotacaoPreenchida);

            if (!args.IsValid)
                args.IsValid = anotacaoPreenchida;
        }

        protected void rptHabilidadesAula_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int nivelLinha = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Nivel"));
                bool permiteLancamento = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PermiteLancamento"));
                string ocr_codigo = DataBinder.Eval(e.Item.DataItem, "Codigo").ToString();
                string ocr_descricao = DataBinder.Eval(e.Item.DataItem, "Descricao").ToString();

                Literal litCabecalho = (Literal)e.Item.FindControl("litCabecalho");
                Literal litConteudo = (Literal)e.Item.FindControl("litConteudo");
                HtmlControl divHabilidade = (HtmlControl)e.Item.FindControl("divHabilidade");
                Literal lblHabilidade = (Literal)e.Item.FindControl("lblHabilidade");

                string ul = String.Format("<ul class='treeview' style='display: {0};'>", nivelLinha == 1 ? "block" : "none");
                string li = nivelLinha == Nivel ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") :
                                                  (nivelLinha > Nivel ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") : "<li class='expandable'>");

                string cabecalho = nivelLinha == Nivel ? li :
                    (nivelLinha > Nivel ? ul + li : MultiplicaString("</li></ul>", Nivel - nivelLinha) + li);
                cabecalho += permiteLancamento ? string.Empty : "<div class='hitarea expandable-hitarea'></div>";
                if (litCabecalho != null)
                {
                    litCabecalho.Text = cabecalho;
                }

                lblHabilidade.Text = litConteudo.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                divHabilidade.Visible = permiteLancamento;
                litConteudo.Visible = !permiteLancamento;

                if (permiteLancamento)
                {
                    CheckBox chkTrabalhado = (CheckBox)e.Item.FindControl("chkTrabalhado");

                    #region Busca níveis de aprendizado da orientação curricular

                    int ocrId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ocr_id"));
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
                            rptLegendaNivelAprendizado.DataSource = dtNivelArendizadoCurriculo;
                            rptLegendaNivelAprendizado.DataBind();

                            if (rptLegendaNivelAprendizado.Items.Count > 0)
                            {
                                divLegendaNivelAprendizado.Visible = true;
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

                            Label lblLegTrab = (Label)e.Item.FindControl("lblLegendaTrabalhado");

                            if (lblLegTrab != null)
                            {
                                lblLegTrab.Visible = true;
                                lblLegTrab.Text = niveisSiglas;
                            }
                        }
                    }

                    #endregion Busca níveis de aprendizado da orientação curricular
                }

                Nivel = nivelLinha;
            }
        }


        protected void rptAulasTerritorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CheckBoxList cblFrequencia = (CheckBoxList)e.Item.FindControl("cblFrequencia");

                int frequencia = Convert.ToInt32(String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequencia").ToString()) ? "0" : DataBinder.Eval(e.Item.DataItem, "taa_frequencia"));
                char[] taa_frequenciaBitMap = (String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap").ToString()) ? "" : DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap")).ToString().ToCharArray();
                string sNumeroAulas = DataBinder.Eval(e.Item.DataItem, "tau_numeroAulas").ToString();
                int numeroAulas = string.IsNullOrEmpty(sNumeroAulas) ? 0 : Convert.ToInt32(sNumeroAulas);

                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id").ToString());
                int tau_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id").ToString());
                int mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id").ToString());

                string ids = string.Format("{0};{1};{2}", tud_id, tau_id, mtd_id);

                // Verificar se a data da aula está dentro do período da matrícula do aluno (data de matrícula e data de saída).
                string data = DataBinder.Eval(e.Item.DataItem, "tau_data").ToString();
                if (!String.IsNullOrEmpty(data))
                {
                    DateTime tau_data = Convert.ToDateTime(data);

                    cblFrequencia.Visible = ((tau_data.Date >= mtd_dataMatriculaAluno.Date) && (tau_data.Date < mtd_dataSaidaAluno.Date));
                }

                if (cblFrequencia != null)
                {
                    cblFrequencia.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);
                    bool permissaoModuloAlteracao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao;
                    bool permissaoModuloAlteracaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;

                    if (VS_EntitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia == (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia &&
                        VS_crp_controleTempo == (byte)ACA_CurriculoPeriodoControleTempo.Horas)
                    {

                        if (numeroAulas > 0)
                        {
                            ListItem li = new ListItem(string.Empty, ids);
                            if (frequencia > 0)
                                li.Selected = true;
                            li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                            cblFrequencia.Items.Add(li);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numeroAulas; i++)
                        {
                            ListItem li = new ListItem(string.Empty, ids);
                            if (taa_frequenciaBitMap.Length > i && taa_frequenciaBitMap[i].Equals('1'))
                                li.Selected = true;
                            li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                            cblFrequencia.Items.Add(li);
                        }
                    }

                    bool permiteEditar = (VS_permissaoAlteracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao;
                    if (permiteEditar)
                    {
                        permiteEditar = (VS_situacaoTurmaDisciplina == 1 || (VS_situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == VS_usu_id));
                    }
                    permiteEditar &= VS_Periodo_Aberto;

                    cblFrequencia.Enabled &= permiteEditar;
                }
            }
        }

        protected void btnSalvarJustificativa_Click(object sender, EventArgs e)
        {
            ExcluirAula(VS_tud_id_Aula, VS_tau_id);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmacaoExclusaoAula", "var exibirMensagemConfirmacao=false;$('#divConfirmacaoExclusaoAulaDiretor').dialog('close');", true);
        }
        #endregion Eventos


    }
}