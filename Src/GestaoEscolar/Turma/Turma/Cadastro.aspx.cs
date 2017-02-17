using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.TurmaDisciplina;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_Turma_Cadastro : MotherPageLogado
{
    /// <summary>
    /// Variável usada na hora de carregar as disciplinas, sendo passada por parâmetro para cada disciplina, para
    /// que seja carregada na primeira vez somente, evitando assim que seja carregada essa tabela
    /// em cada item de disciplina da turma (problema de performance identificado em ambiente de produção
    /// do cliente).
    /// </summary>
    private DataTable dtDocentesEscola;

    private DataTable _dtAvaliacoesFormato;

    private DataTable dtAvaliacoesFormato
    {
        get
        {
            // Carrega as avaliações do formato caso o curso selecionado esteja marcado como
            // "controlar efetivação semestral".
            if (_dtAvaliacoesFormato == null && uccFormatoAvaliacao.Valor > 0 &&
                (!EntCurso.IsNew) && EntCurso.cur_efetivacaoSemestral)
            {
                _dtAvaliacoesFormato =
                    ACA_AvaliacaoBO.SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato(uccFormatoAvaliacao.Valor);
            }

            return _dtAvaliacoesFormato;
        }
    }

    private DataTable dtDisciplinaNaoAvaliado;

    #region Propriedades

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tur_id (ID da turma)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private long _VS_tur_id
    {
        get
        {
            if (ViewState["_VS_tur_id"] != null)
            {
                return Convert.ToInt64(ViewState["_VS_tur_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_tur_id"] = value;
            ViewState["VS_cal_ano"] = null;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de crp_TurmaAvaliacao
    /// </summary>
    private bool _VS_TurmaPorAvaliacao
    {
        get
        {
            if (ViewState["_VS_TurmaPorAvaliacao"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_TurmaPorAvaliacao"]);
            }
            return false;
        }
        set
        {
            ViewState["_VS_TurmaPorAvaliacao"] = value;
        }
    }

    /// <summary> 
    /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano 
    /// </summary> 
    protected int VS_cal_ano
    {
        get
        {
            if (_VS_tur_id > 0)
            {
                if (ViewState["VS_cal_ano"] == null)
                {
                    ViewState["VS_cal_ano"] = ACA_CalendarioAnualBO.SelecionaPorTurma(_VS_tur_id).cal_ano;
                }
                return Convert.ToInt32(ViewState["VS_cal_ano"]);
            }
            else
            {
                return uccCalendario._cal_ano;
            }
        }
    }

    /// <summary> 
    /// Retorna se a nova regra de docencia compartilhada ja esta valendo, 
    /// turma de calendário anterior à 2015 não deve aplicar essa nova regra. 
    /// </summary> 
    private bool aplicarNovaRegraDocenciaCompartilhada
    {
        get
        {
            return VS_cal_ano >= 2015;
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
                return Convert.ToByte(ViewState["VS_ConfirmacaoPadrao"]);
            return 0;
        }
        set
        {
            ViewState["VS_ConfirmacaoPadrao"] = value;
        }
    }

    /// <summary>
    /// Retorna o valor salvo no parâmetro acadêmico referente à funcionalidade de permanecer na tela.
    /// </summary>
    private bool PermaneceTela
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    private ACA_Curriculo _entCurriculo;

    private ACA_Curriculo EntCurriculo
    {
        get
        {
            if ((_entCurriculo == null) || (_entCurriculo.IsNew))
            {
                // Recupera os dados do curriculo do curso
                _entCurriculo = new ACA_Curriculo
                                        {
                                            cur_id = uccCursoCurriculo.Valor[0],
                                            crr_id = uccCursoCurriculo.Valor[1]
                                        };
                ACA_CurriculoBO.GetEntity(_entCurriculo);
            }

            return _entCurriculo;
        }
    }

    private ACA_Curso _entCurso;

    private ACA_Curso EntCurso
    {
        get
        {
            if ((_entCurso == null) || (_entCurso.IsNew))
            {
                // Recupera os dados do curriculo do curso
                _entCurso = new ACA_Curso
                {
                    cur_id = uccCursoCurriculo.Valor[0]
                };
                ACA_CursoBO.GetEntity(_entCurso);
            }

            return _entCurso;
        }
    }

    private ACA_CurriculoPeriodo _entCurPeriodo;

    private ACA_CurriculoPeriodo EntCurPeriodo
    {
        get
        {
            if ((_entCurPeriodo == null) || (_entCurPeriodo.IsNew))
            {
                _entCurPeriodo = new ACA_CurriculoPeriodo
                                     {
                                         cur_id = uccCurriculoPeriodo.Valor[0]
                                         ,
                                         crr_id = uccCurriculoPeriodo.Valor[1]
                                         ,
                                         crp_id = uccCurriculoPeriodo.Valor[2]
                                     };
                ACA_CurriculoPeriodoBO.GetEntity(_entCurPeriodo);
            }

            return _entCurPeriodo;
        }
    }

    private bool? _bloqueioAtribuicaoDocente_Escola;

    /// <summary>
    /// Propriedade que indica se está marcado para bloquear a atribuição de docentes da escola.
    /// </summary>
    private bool BloqueioAtribuicaoDocente_Escola
    {
        get
        {
            return _bloqueioAtribuicaoDocente_Escola ?? false;
        }
    }

    private DataTable _dtPeriodosCalendario;

    /// <summary>
    /// Tabela com períodos do calendário da turma.
    /// </summary>
    private DataTable dtPeriodosCalendario
    {
        get
        {
            if (_dtPeriodosCalendario == null)
            {
                if (uccCalendario.Valor > 0)
                {
                    _dtPeriodosCalendario = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario_MarcacaoTurmas
                        (uccCalendario.Valor, _VS_tur_id);
                }
            }

            return _dtPeriodosCalendario;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena a lista de docencia compartilhada para salvar
    /// </summary>
    private DataTable _VS_ListaDocenciaCompartilhada
    {
        get
        {
            if (ViewState["_VS_ListaDocenciaCompartilhada"] == null)
            {
                ViewState["_VS_ListaDocenciaCompartilhada"] = TUR_TurmaDisciplinaBO.SelectRelacionadoDocenciaCompartilhadaBy_Turma(_VS_tur_id);
            }
            return (DataTable)ViewState["_VS_ListaDocenciaCompartilhada"];
        }
        set
        {
            ViewState["_VS_ListaDocenciaCompartilhada"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tud_id da disciplina compartilhada 
    /// selecionada no combo.
    /// </summary>
    private long _VS_TudIdCompartilhada
    {
        get
        {
            if (ViewState["_VS_TudIdCompartilhada"] != null)
            {
                return Convert.ToInt64(ViewState["_VS_TudIdCompartilhada"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_TudIdCompartilhada"] = value;
        }
    }

    #endregion Propriedades

    #region Estrutura

    /// <summary>
    /// Struct que contem a lista de disciplinas visiveis no grid.
    /// </summary>
    private struct ListDisciplinasVisiveis
    {
        public WebControls_TurmaDisciplina_UCGridDisciplina grid;
        public WebControls_TurmaDisciplina_UCRepeaterDisciplina repeater;
        public UCGridDisciplinaRegencia gridRegencia;
        public ACA_CurriculoDisciplinaTipo tipo;
        public byte controleTempo;
    }

    #endregion Estrutura

    #region Constantes

    private const int indexColumnCompartilhada = 1;

    #endregion Constantes

    #region Métodos

    /// <summary>
    /// Carrega os grids de disciplinas da turma
    /// </summary>
    private void CarregaDisciplinas()
    {
        try
        {
            if ((uccCursoCurriculo.Valor[0] > 0) &&
                (uccCurriculoPeriodo.Valor[2] > 0) &&
                (uccCalendario.Valor > 0) &&
                (uccFormatoAvaliacao.Valor > 0))
            {
                fdsSemDisciplinas.Visible = false;
                fdsDisciplinas.Visible = true;

                DataTable dtVigenciasDocentes = TUR_TurmaDocenteBO.SelecionaVigenciasDocentesPorDisciplina(_VS_tur_id);

                IEnumerable<ListDisciplinasVisiveis> listDiscVisiveis = RetornaGridsDisciplinas();

                foreach (ListDisciplinasVisiveis disciplina in listDiscVisiveis)
                {
                    // Chamar o método de carregar.
                    CarregarUCDisciplina(disciplina, ref dtVigenciasDocentes);
                }
            }
            else
            {
                lblSemDisciplinas.Text = UtilBO.GetErroMessage("É necessário selecionar " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                        ", " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ", calendário escolar e formato de avaliação para carregar os(as) " +
                        GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + ".", UtilBO.TipoMensagem.Informacao);
                fdsSemDisciplinas.Visible = true;
                fdsDisciplinas.Visible = false;
            }

            upnDisciplinas.Update();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + ".", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Chama o método de carregar do item passado por parâmetro, utilizando os
    /// dados selecionados na tela.
    /// </summary>
    /// <param name="disciplina">Item com o usercontrol e o tipo</param>
    private void CarregarUCDisciplina(ListDisciplinasVisiveis disciplina, ref DataTable dtVigenciasDocentes)
    {
        if (disciplina.grid != null)
        {
            // Se controleTempo == 3, é para os dois tipos de controle de tempo.
            if ((disciplina.controleTempo == EntCurPeriodo.crp_controleTempo)
                || (disciplina.controleTempo == 3))
            {
                disciplina.grid.Visible = true;
                disciplina.grid.CarregaGridDisciplinas
                (
                    uccCursoCurriculo.Valor[0]
                      , uccCursoCurriculo.Valor[1]
                      , uccCurriculoPeriodo.Valor[2]
                      , disciplina.tipo
                      , _VS_tur_id
                      , uccUAEscola.Esc_ID
                      , uccUAEscola.Uni_ID
                      , chkProfessorEspecialista.Checked
                      , ref dtDocentesEscola
                      , dtAvaliacoesFormato
                      , ref dtDisciplinaNaoAvaliado
                      , BloqueioAtribuicaoDocente_Escola
                      , ref dtVigenciasDocentes
                      , aplicarNovaRegraDocenciaCompartilhada
                );
                disciplina.grid.PermiteEditar = false;
            }
            else
            {
                disciplina.grid.Visible = false;
            }
        }
        else if (disciplina.repeater != null)
        {
            // Se controleTempo == 3, é para os dois tipos de controle de tempo.
            if ((disciplina.controleTempo == EntCurPeriodo.crp_controleTempo)
                || (disciplina.controleTempo == 3))
            {
                // Só faz a consulta do repeater se estiver selecionado o calendário.
                disciplina.repeater.Visible = ((uccCalendario.Valor > 0) &&
                                               (disciplina.repeater.CarregaRepeaterDisciplinas
                                                   (
                                                       uccCursoCurriculo.Valor[0]
                                                       , uccCursoCurriculo.Valor[1]
                                                       , uccCurriculoPeriodo.Valor[2]
                                                       , disciplina.tipo
                                                       , _VS_tur_id
                                                       , uccCalendario.Valor
                                                       , uccUAEscola.Esc_ID
                                                       , uccUAEscola.Uni_ID
                                                       , chkProfessorEspecialista.Checked
                                                       , ref dtDocentesEscola
                                                       , dtAvaliacoesFormato
                                                       , ref dtDisciplinaNaoAvaliado
                                                       , BloqueioAtribuicaoDocente_Escola
                                                       , dtPeriodosCalendario
                                                       , ref dtVigenciasDocentes
                                                       , aplicarNovaRegraDocenciaCompartilhada
                                                   )));
                disciplina.repeater.PermiteEditar = false;
            }
            else
            {
                disciplina.repeater.Visible = false;
            }
        }
        else if (disciplina.gridRegencia != null)
        {
            // Se controleTempo == 3, é para os dois tipos de controle de tempo.
            if (disciplina.controleTempo == 3)
            {
                disciplina.gridRegencia.Visible = true;
                disciplina.gridRegencia.CarregaGridDisciplinas
                (
                    uccCursoCurriculo.Valor[0]
                      , uccCursoCurriculo.Valor[1]
                      , uccCurriculoPeriodo.Valor[2]
                      , disciplina.tipo
                      , _VS_tur_id
                      , uccUAEscola.Esc_ID
                      , uccUAEscola.Uni_ID
                      , chkProfessorEspecialista.Checked
                      , ref dtDocentesEscola
                      , dtAvaliacoesFormato
                      , ref dtDisciplinaNaoAvaliado
                      , BloqueioAtribuicaoDocente_Escola
                      , ref dtVigenciasDocentes
                      , aplicarNovaRegraDocenciaCompartilhada
                );
                disciplina.gridRegencia.PermiteEditar = false;
            }
            else
            {
                disciplina.gridRegencia.Visible = false;
            }
        }
    }

    /// <summary>
    /// Adiciona os grids para os tipos de disciplina permitidos para esse tipo de
    /// controle:
    /// Dis. Principal, Optativa,
    /// Docente da turma e docente específico – obrigatória,
    /// Docente da turma e docente específico – eletiva,
    /// Depende da disponibilidade de professor – obrigatória,
    /// Depende da disponibilidade de professor – eletiva.
    /// </summary>
    /// <param name="listDiscVisiveis"></param>
    private void AdicionaDisciplinasControle_Horas(ref List<ListDisciplinasVisiveis> listDiscVisiveis)
    {
        ListDisciplinasVisiveis disciplina = new ListDisciplinasVisiveis
        {
            grid = ucDiscDocenteTurmaObrigatoria
            ,
            tipo = ACA_CurriculoDisciplinaTipo.DocenteTurmaObrigatoria
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.Horas
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            repeater = ucDiscDocenteTurmaEletiva
            ,
            tipo = ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.Horas
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            repeater = ucDiscDisponibilidadeProfObrigatoria
            ,
            tipo = ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorObrigatoria
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.Horas
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            repeater = ucDiscDisponibilidadeProfEletiva
            ,
            tipo = ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.Horas
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            grid = ucDiscMultisseriadaAluno
            ,
            tipo = ACA_CurriculoDisciplinaTipo.MultisseriadaAluno
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.Horas
        };
        listDiscVisiveis.Add(disciplina);

        if (aplicarNovaRegraDocenciaCompartilhada)
        {
            disciplina = new ListDisciplinasVisiveis
            {
                grid = ucDiscDocenciaCompartilhada
                ,
                tipo = ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                ,
                controleTempo = 3
            };
            listDiscVisiveis.Add(disciplina);
        }
        else
        {
            ucDiscDocenciaCompartilhada.Visible = false;
        }
    }

    /// <summary>
    /// Retorna uma lista com todos os user controls de disciplinas, com seu
    /// devido tipo e controle de tempo, para carregar posteriormente.
    /// </summary>
    /// <returns>A lista com os dados preenchidos.</returns>
    private IEnumerable<ListDisciplinasVisiveis> RetornaGridsDisciplinas()
    {
        // Adidiona todos os usercontrols de grid numa lista.
        List<ListDisciplinasVisiveis> listDiscVisiveis = new List<ListDisciplinasVisiveis>();
        AdicionaDisciplinasControle_Horas(ref listDiscVisiveis);
        AdicionaDisciplinasControle_TemposAula(ref listDiscVisiveis);

        // Duas disciplinas que são para os 2 controles de tempo.
        ListDisciplinasVisiveis disciplina = new ListDisciplinasVisiveis
        {
            grid = ucDiscPrincipal
            ,
            tipo = ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal
            ,
            controleTempo = 3
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            repeater = ucDiscOptativa
            ,
            tipo = ACA_CurriculoDisciplinaTipo.Optativa
            ,
            controleTempo = 3
        };
        listDiscVisiveis.Add(disciplina);

        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ITENS_REGENCIA_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            disciplina = new ListDisciplinasVisiveis
            {
                gridRegencia = UCGridDisciplinaRegencia
                ,
                tipo = ACA_CurriculoDisciplinaTipo.Regencia
                ,
                controleTempo = 3
            };
            listDiscVisiveis.Add(disciplina);

            disciplina = new ListDisciplinasVisiveis
            {
                grid = ucDiscComplementoRegencia
                ,
                tipo = ACA_CurriculoDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                ,
                controleTempo = 3
            };
            listDiscVisiveis.Add(disciplina);
        }

        return listDiscVisiveis;
    }

    /// <summary>
    /// Adiciona os grids para os tipos de disciplina permitidos para esse tipo de
    /// controle: Obrigatória, Dis. Principal, Optativa e Eletiva.
    /// Lista de grids e repeaters que estarão visíveis.
    /// </summary>
    /// <param name="listDiscVisiveis"></param>
    private void AdicionaDisciplinasControle_TemposAula(ref List<ListDisciplinasVisiveis> listDiscVisiveis)
    {
        ListDisciplinasVisiveis disciplina = new ListDisciplinasVisiveis
                                                 {
                                                     grid = ucDiscObrigatoria
                                                     ,
                                                     tipo = ACA_CurriculoDisciplinaTipo.Obrigatoria
                                                     ,
                                                     controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.TemposAula
                                                 };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
        {
            grid = ucDiscMultisseriadaAluno
            ,
            tipo = ACA_CurriculoDisciplinaTipo.MultisseriadaAluno
            ,
            controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.TemposAula
        };
        listDiscVisiveis.Add(disciplina);

        disciplina = new ListDisciplinasVisiveis
                         {
                             repeater = ucDiscEletiva
                             ,
                             tipo = ACA_CurriculoDisciplinaTipo.Eletiva
                             ,
                             controleTempo = (byte)ACA_CurriculoPeriodoControleTempo.TemposAula
                         };
        listDiscVisiveis.Add(disciplina);
    }

    /// <summary>
    /// Carrega os turnos de acordo com o Crp selecionado, e seleciona o valor passado por parâmetro.
    /// </summary>
    /// <param name="valorSelecionado">Valor para selecionar</param>
    private void CarregarTurnoPorCurriculoPeriodo(int valorSelecionado)
    {
        ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo
        {
            cur_id = uccCurriculoPeriodo.Valor[0],
            crr_id = uccCurriculoPeriodo.Valor[1],
            crp_id = uccCurriculoPeriodo.Valor[2]
        };
        ACA_CurriculoPeriodoBO.GetEntity(crp);

        if (!crp.IsNew)
        {
            uccTurno.CarregarTurnoPorTurnoPeriodoControleTempoAtivo(valorSelecionado, crp.crp_controleTempo, crp.crp_qtdeDiasSemana, crp.crp_qtdeTemposSemana, crp.crp_qtdeHorasDia, crp.crp_qtdeMinutosDia);
        }
        else
        {
            uccTurno.AdicionaItemVazio();
            uccTurno.PermiteEditar = false;
        }

        if (valorSelecionado > 0)
        {
            // Verificar se existe o valor no combo de turno, se não existir, mostrar mensagem
            // para o usuário, que o turno está inválido (não bateu a conferência do tempo do
            // turno com relação ao curriculoPeriodo).
            if (uccTurno.ExisteValor(valorSelecionado))
            {
                uccTurno.Valor = valorSelecionado;
            }
            else
            {
                ACA_Turno entTurno = new ACA_Turno { trn_id = valorSelecionado };
                ACA_TurnoBO.GetEntity(entTurno);

                if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.REMOVER_MSG_ERRO_CARREGAR_TURMADISCIPLINA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    lblMessage.Text = UtilBO.GetErroMessage("O turno " + entTurno.trn_descricao + " não possui o mesmo número de tempos de aula semanal do curso.", UtilBO.TipoMensagem.Alerta);
            }
        }
    }

    /// <summary>
    /// Metodo para carregar um registro de Turma, a fim de atualizar suas informações.
    /// Recebe dados referente a Turma para realizar busca.
    /// </summary>
    /// <param name="tur_id">ID da turma</param>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do curriculo</param>
    /// <param name="crp_id">ID do curriculo periodo</param>
    public bool LoadByEntity(long tur_id, int cur_id, int crr_id, int crp_id)
    {
        try
        {
            //armazena valor ID da turma a ser alterada
            _VS_tur_id = tur_id;

            //Busca da turma baseado no ID da turma
            TUR_Turma entTurma = new TUR_Turma { tur_id = tur_id };
            TUR_TurmaBO.GetEntity(entTurma);

            if (entTurma.tur_tipo != (byte)TUR_TurmaTipo.Normal)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Não é permitido editar essa turma.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            ESC_Escola entEscola = new ESC_Escola { esc_id = entTurma.esc_id };
            ESC_EscolaBO.GetEntity(entEscola);

            // Verifica se usuário logado pertence à mesma entidade da turma, caso não seja, não é permitido editar.
            if (entEscola.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A turma não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            if (uccUAEscola.VisibleUA)
            {
                // Buscar Unidade Administrativa Superior.
                SYS_UnidadeAdministrativa entUA = new SYS_UnidadeAdministrativa
                {
                    ent_id = entEscola.ent_id
                    ,
                    uad_id = entEscola.uad_id
                };

                SYS_UnidadeAdministrativaBO.GetEntity(entUA);

                Guid uad_idSuperior = entEscola.uad_idSuperiorGestao.Equals(Guid.Empty) ? entUA.uad_idSuperior : entEscola.uad_idSuperiorGestao;

                uccUAEscola.Uad_ID = uad_idSuperior;

                // Recarrega o combo de escolas com a uad_idSuperior.
                uccUAEscola.CarregaEscolaPorUASuperiorSelecionada();
            }

            uccUAEscola.SelectedValueEscolas = new[] { entTurma.esc_id, entTurma.uni_id };

            // Carrega os combos baseados na UnidadeEscola carregada.
            //CarregarCombos_ByEscola();
            uccUAEscola_IndexChangedUnidadeEscola();

            // Recupera os dados do curriculo do curso
            ACA_Curriculo crr = new ACA_Curriculo
            {
                cur_id = cur_id
                ,
                crr_id = crr_id
            };
            ACA_CurriculoBO.GetEntity(crr);

            // [Carla - alterações de performance] Guarda a entidade na propriedade da tela, para evitar carregamento desnecessário.
            _entCurriculo = crr;

            uccCursoCurriculo.Valor = new[] { cur_id, crr_id };
            _UCComboCursoCurriculo_IndexChanged();

            uccCurriculoPeriodo.Valor = new[] { cur_id, crr_id, crp_id };
            _UCComboCurriculoPeriodo__OnSelectedIndexChange();

            // Verifica se o curso tem regime de matrícula seriado por avaliações
            if ((ACA_CurriculoRegimeMatricula)crr.crr_regimeMatricula == ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                // Carrega as avaliações cadastradas para a turma
                List<TUR_TurmaCurriculoAvaliacao> listaAvaliacaoBanco = TUR_TurmaCurriculoAvaliacaoBO.SelecionaAvaliacaoPorTurma(_VS_tur_id);
                foreach (ListItem item in chkAvaliacao.Items)
                {
                    int tca_numeroAvaliacao = Convert.ToInt32(item.Value.Split(';')[1]);
                    if (listaAvaliacaoBanco.Exists(p => p.tca_numeroAvaliacao == tca_numeroAvaliacao))
                    {
                        int tca_id = listaAvaliacaoBanco.Find(p => p.tca_numeroAvaliacao == tca_numeroAvaliacao).tca_id;
                        item.Selected = true;
                        item.Value = tca_id + ";" + tca_numeroAvaliacao;
                        item.Enabled = false;
                    }
                }
            }

            if (entTurma.tur_docenteEspecialista)
            {
                chkProfessorEspecialista.Checked = true;
                chkProfessorEspecialista_CheckedChanged(this, null);
            }

            if (entTurma.tur_participaRodizio)
            {
                chkRodizio.Checked = true;
            }

            uccCalendario.Valor = entTurma.cal_id;
            uccFormatoAvaliacao.Valor = entTurma.fav_id;

            // Busca parâmetro de formação de turmas.
            MTR_ParametroFormacaoTurma ppe = MTR_ParametroFormacaoTurmaBO.
                SelecionaParametroPorAnoCursoPeriodo(cur_id, crr_id, crp_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (ppe == null)
            {
                // Carrega o combo de turno pelo período e já seta o valor.
                CarregarTurnoPorCurriculoPeriodo(entTurma.trn_id);
            }
            else
            {
                // Carrega turnos do parâmetro.
                uccTurno.CarregarTurnoPorParametroPeriodo(ppe);

                if (uccTurno.ExisteValor(entTurma.trn_id))
                {
                    uccTurno.Valor = entTurma.trn_id;
                }
            }

            // Configuração da tela para preenchimento de valores nos campos.
            txtCodigoTurma.Text = entTurma.tur_codigo;
            txtCodigoInep.Text = entTurma.tur_codigoInep;
            txtMinimoMatriculados.Text = Convert.ToString(entTurma.tur_minimoMatriculados);
            txtCapacidade.Text = Convert.ToString(entTurma.tur_vagas);
            ddlDuracao.SelectedValue = Convert.ToString(entTurma.tur_duracao);

            if (txtObservacao.Visible)
                txtObservacao.Text = entTurma.tur_observacao;

            if (ddlSituacao.Items.Contains(new ListItem("Aguardando", ((byte)TUR_TurmaSituacao.Aguardando).ToString())))
                ddlSituacao.Items.Remove(new ListItem("Aguardando", ((byte)TUR_TurmaSituacao.Aguardando).ToString()));

            if (entTurma.tur_situacao == (byte)TUR_TurmaSituacao.Aguardando)
                ddlSituacao.Items.Add(new ListItem("Aguardando", ((byte)TUR_TurmaSituacao.Aguardando).ToString()));
            ddlSituacao.SelectedValue = Convert.ToString(entTurma.tur_situacao);

            // Desabilitar campos que não podem ser alterados se existir matrícula turma ou registros associados
            if (TUR_TurmaBO.Existe_MatriculaTurma(_VS_tur_id)
                || TUR_TurmaBO.VerificaRegistrosAssociados(_VS_tur_id))
            {
                chkProfessorEspecialista.Enabled = false;
                uccFormatoAvaliacao.PermiteEditar = false;
            }

            // Desabilita os campos, caso exista parâmetros para criação de turmas
            if (ppe != null)
            {
                uccCalendario.PermiteEditar = false;
                uccFormatoAvaliacao.PermiteEditar = false;
                
                chkProfessorEspecialista.Enabled = false;
            }

            if (entTurma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada || entTurma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta)
            {
                divDataEncerramento.Visible = true;

                if (entTurma.tur_dataEncerramento != new DateTime())
                {
                    txtDataEncerramento.Text = entTurma.tur_dataEncerramento.ToString("dd/MM/yyyy");
                }
            }

            // Atualiza as disciplinas.
            CarregaDisciplinas();

            return true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a Turma.", UtilBO.TipoMensagem.Erro);
            return false;
        }
        finally
        {
            // Desabilitar campos que não podem ser alterados.
            uccUAEscola.PermiteAlterarCombos = false;
            uccCalendario.PermiteEditar = false;
            uccCursoCurriculo.PermiteEditar = false;
            uccCurriculoPeriodo._Combo.Enabled = false;
        }
    }
    
    /// <summary>
    /// Retorna os lists necessários para salvar a turma.
    /// </summary>
    /// <param name="tud_codigo">Código da Turma</param>
    /// <param name="tud_vagas">Vagas da turma</param>
    /// <param name="tud_minimoMatriculados">Quantidade mínima de matriculados</param>
    /// <param name="tud_duracao">Duração da turma disciplina</param>
    /// <returns>Retorna as disciplinas da turma</returns>
    private List<CadastroTurmaDisciplina> RetornaDisciplinas
    (
        string tud_codigo
        , int tud_vagas
        , int tud_minimoMatriculados
        , byte tud_duracao
    )
    {
        //Lits de todas as disciplinas
        List<CadastroTurmaDisciplina> listTurmaDisciplina = new List<CadastroTurmaDisciplina>();

        IEnumerable<ListDisciplinasVisiveis> listaDisc = RetornaGridsDisciplinas();
        foreach (ListDisciplinasVisiveis disc in listaDisc)
        {
            if ((disc.repeater != null) && (disc.repeater.Visible))
            {
                // Adiciona os itens na lista.
                listTurmaDisciplina.AddRange(disc.repeater.RetornaDisciplinas
                (
                     tud_codigo
                     , tud_vagas
                     , tud_minimoMatriculados
                     , tud_duracao
                ));
            }

            if ((disc.grid != null) && (disc.grid.Visible))
            {
                // Adiciona os itens na lista.
                listTurmaDisciplina.AddRange(disc.grid.RetornaDisciplinas
                (
                    tud_codigo
                     , tud_vagas
                     , tud_minimoMatriculados
                     , tud_duracao
                ));
            }

            if ((disc.gridRegencia != null) && (disc.gridRegencia.Visible))
            {
                // Adiciona os itens na lista.
                listTurmaDisciplina.AddRange(disc.gridRegencia.RetornaDisciplinas
                (
                    tud_codigo
                     , tud_vagas
                     , tud_minimoMatriculados
                     , tud_duracao
                ));
            }
        }

        return listTurmaDisciplina;
    }

    /// <summary>
    /// Retorna o list de avaliação para salvar a turma.
    /// </summary>
    private List<TUR_TurmaCurriculoAvaliacao> RetornaAvaliacao()
    {
        List<TUR_TurmaCurriculoAvaliacao> listaAvaliacao = new List<TUR_TurmaCurriculoAvaliacao>();

        // Verifica se é turma avaliação, Pois se for turma por avaliação o divAvaliacao estara false mas o chkAvaliacao estará carregado

        if ((divAvaliacao.Visible) || (_VS_TurmaPorAvaliacao))
        {
            foreach (ListItem item in chkAvaliacao.Items)
            {
                // Adicionar um TurmaCurriculoAvaliacao para cada item checado.
                if (item.Selected)
                {
                    int tca_id = Convert.ToInt32(item.Value.Split(';')[0]);
                    int tca_numeroAvaliacao = Convert.ToInt32(item.Value.Split(';')[1]);

                    TUR_TurmaCurriculoAvaliacao entity = new TUR_TurmaCurriculoAvaliacao
                    {
                        tur_id = _VS_tur_id
                        ,
                        cur_id = uccCursoCurriculo.Valor[0]
                        ,
                        crr_id = uccCursoCurriculo.Valor[1]
                        ,
                        crp_id = uccCurriculoPeriodo.Valor[2]
                        ,
                        tca_id = tca_id
                        ,
                        tca_numeroAvaliacao = tca_numeroAvaliacao
                        ,
                        tca_situacao = 1
                        ,
                        IsNew = tca_id <= 0
                        ,
                    };

                    listaAvaliacao.Add(entity);
                }
            }
        }
        else
        {
            listaAvaliacao = null;
        }

        return listaAvaliacao;
    }

    /// <summary>
    /// Retorna a lista avaliacao com a qdd de itens passada.
    /// </summary>
    /// <param name="crr_qtdeAvaliacaoProgressao"></param>
    /// <param name="crp_nomeAvaliacao"></param>
    /// <returns></returns>
    private List<TUR_TurmaCurriculoAvaliacao> RetornaListaAvaliacaoCheboxList
    (
        int crr_qtdeAvaliacaoProgressao
        , string crp_nomeAvaliacao
    )
    {
        List<TUR_TurmaCurriculoAvaliacao> listaAvaliacao = new List<TUR_TurmaCurriculoAvaliacao>();
        for (int i = 1; i <= crr_qtdeAvaliacaoProgressao; i++)
        {
            TUR_TurmaCurriculoAvaliacao tur = new TUR_TurmaCurriculoAvaliacao
                                                  {
                                                      crp_nomeAvaliacao = crp_nomeAvaliacao + " " + i
                                                      ,
                                                      tca_id_numeroAvaliacao = -1 + ";" + i
                                                  };

            listaAvaliacao.Add(tur);
        }

        return listaAvaliacao;
    }

    /// <summary>
    /// Carrega combos e textos iniciais da tela.
    /// </summary>
    private void CarregarTelaInicial()
    {
        ucDiscMultisseriadaAluno.nomeTipoDisciplina = CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " multisseriadas do aluno";

        uccCurriculoPeriodo.Obrigatorio = true;
        uccFormatoAvaliacao.Obrigatorio = true;

        // Carregar combo de turnos.
        uccTurno.PermiteEditar = false;
        uccTurno.AdicionaItemVazio();
        uccTurno.Obrigatorio = true;

        // Carregar combo de cursos.
        uccCursoCurriculo.Obrigatorio = true;

        // Carregar formatos de avaliação
        uccFormatoAvaliacao.AdicionaItemVazio();
        uccFormatoAvaliacao.PermiteEditar = false;
        divAvaliacao.Visible = false;
        chkAvaliacao.DataSource = new DataTable();
        chkAvaliacao.DataBind();

        uccCalendario.Obrigatorio = true;

        //uccCalendario.CarregarCalendarioAnual();

        uccCalendario.PermiteEditar = false;
        uccCursoCurriculo.PermiteEditar = false;
        uccCurriculoPeriodo._Combo.Enabled = false;

        uccUAEscola.Inicializar();

        if (uccUAEscola.VisibleUA)
        {
            if (uccUAEscola.Uad_ID == Guid.Empty)
            {
                // Força a atualização dos combos para cancelar os selects.
                uccUAEscola_IndexChangedUnidadeEscola();
            }
        }
        else
        {
            if (uccUAEscola.Esc_ID <= 0)
            {
                // Força a atualização dos combos para cancelar os selects.
                uccUAEscola_IndexChangedUnidadeEscola();
            }
        }

        if (aplicarNovaRegraDocenciaCompartilhada)
        {
            ddlDisciplinasDocenciaCompartilhada.Visible = lblDisciplinasDocenciaCompartilhada.Visible = false;
            if (_VS_ListaDocenciaCompartilhada.Rows.Count > 0)
            {
                var disciplinasCompartilhadas = _VS_ListaDocenciaCompartilhada.AsEnumerable().Select(r => new
                                                                                                    {
                                                                                                        tud_id = r.Field<long>("tud_idCompartilhada"),
                                                                                                        tud_nome = r.Field<string>("tud_nomeCompartilhada"),
                                                                                                    }).ToList().Distinct();
                if (disciplinasCompartilhadas.Count() > 0)
                {
                    ddlDisciplinasDocenciaCompartilhada.Visible = lblDisciplinasDocenciaCompartilhada.Visible = true;
                    ddlDisciplinasDocenciaCompartilhada.DataValueField = "tud_id";
                    ddlDisciplinasDocenciaCompartilhada.DataTextField = "tud_nome";
                    ddlDisciplinasDocenciaCompartilhada.DataSource = disciplinasCompartilhadas;
                    ddlDisciplinasDocenciaCompartilhada.DataBind();

                    _VS_TudIdCompartilhada = Convert.ToInt64(ddlDisciplinasDocenciaCompartilhada.SelectedValue);
                    ddlDisciplinasDocenciaCompartilhada_SelectedIndexChanged(null, null);
                }
            }
            if (!ddlDisciplinasDocenciaCompartilhada.Visible)
            {
                lblMensagemSemDocenciaCompartilhada.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Turma", "Turma.Cadastro.lblMensagemSemDocenciaCompartilhada.Text").ToString(), UtilBO.TipoMensagem.Alerta);
                fdsDisciplinasDocenciaCompartilhada.Visible = false;
            }
        }
        tabDocenciaCompartilhada.Visible = upnDocenciaCompartilhada.Visible = aplicarNovaRegraDocenciaCompartilhada;
    }

    /// <summary>
    /// Verifica se é curso do peja, e carrega os combos de formato de avaliação e calendário de acordo com as regras.
    /// Carrega as avaliações também, quando é curso do peja.
    /// </summary>
    private void CarregaDados_By_RegrasCurso()
    {
        // Verifica se o curso tem regime de matrícula seriado por avaliações
        if ((ACA_CurriculoRegimeMatricula)EntCurriculo.crr_regimeMatricula ==
            ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
        {
            // Se o regime for seriado por avaliações, carrega apenas os calendários e formatos de avaliação
            // que tenham a mesma quantidade de avaliações que o curriculo.
            uccCalendario.CarregarCalendarioAnualPorCursoQtdePeriodos(uccCurriculoPeriodo.Valor[0],
                EntCurriculo.crr_qtdeAvaliacaoProgressao);

            List<TUR_TurmaCurriculoAvaliacao> listaAvaliacao = RetornaListaAvaliacaoCheboxList(EntCurriculo.crr_qtdeAvaliacaoProgressao, EntCurPeriodo.crp_nomeAvaliacao);

            chkAvaliacao.DataSource = listaAvaliacao;
            chkAvaliacao.DataBind();
            _VS_TurmaPorAvaliacao = true;

            // Se for turma por avaliação, carrega as avaliações do período
            if (EntCurPeriodo.crp_turmaAvaliacao)
            {
                divAvaliacao.Visible = true;
            }
            else
            {
                //se não for turma por avaliação esconde o checkbox mas mantem todos eles carregados
                divAvaliacao.Visible = false;

                for (int i = 0; i < EntCurriculo.crr_qtdeAvaliacaoProgressao; i++)
                {
                    chkAvaliacao.Items[i].Selected = true;
                }
            }
        }
        else
        {
            uccCalendario.CarregarCalendarioAnualPorCurso(uccCurriculoPeriodo.Valor[0]);
            divAvaliacao.Visible = false;
            _VS_TurmaPorAvaliacao = false;
        }
        uccCalendario.SetarFoco();
    }

    /// <summary>
    /// Carrega o combo de formatos de avaliação de acordo com o curso e período.
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="crp_id">ID do período do curso</param>
    private void CarregaFormatoAvaliacaoPorCurso(int cur_id, int crr_id, int crp_id)
    {
        if (_VS_tur_id > 0)
        {
            TUR_Turma tur = new TUR_Turma { tur_id = _VS_tur_id };
            TUR_TurmaBO.GetEntity(tur);

            uccFormatoAvaliacao.CarregarPorRegrasCurso(tur.fav_id, cur_id, crr_id, crp_id, chkProfessorEspecialista.Checked);
        }
        else
        {
            uccFormatoAvaliacao.CarregarPorRegrasCurso(-1, cur_id, crr_id, crp_id, chkProfessorEspecialista.Checked);
        }
    }

    /// <summary>
    /// Verifica se existe um parâmetro de formação de turmas cadastrado no CurriculoPeriodo, e seta os valores
    /// nos campos configurados caso exista.
    /// Caso não exista ou seja alteração da turma, carrega os campos pela regra normal.
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="crp_id">ID do período do curso</param>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    private void CarregaDados_ParametroFormacao(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id)
    {
        if (_VS_tur_id <= 0)
        {
            // Verifica parâmetros para criação de turmas.
            MTR_ParametroFormacaoTurma ptf = MTR_ParametroFormacaoTurmaBO.SelecionaParametroPorAnoCursoPeriodo(cur_id, crr_id, crp_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (ptf == null)
            {
                // Limpa campos que podem ter sido desabilitados caso tenha sido selecionado antes
                // por parâmetro de formação de turmas.
                txtCodigoTurma.Text = string.Empty;

                txtCapacidade.Text = string.Empty;

                chkProfessorEspecialista.Checked = false;
                
                CarregarTurnoPorCurriculoPeriodo(-1);

                CarregaFormatoAvaliacaoPorCurso(cur_id, crr_id, crp_id);

                // Carrega os dados pelo curso normalmente.
                CarregaDados_By_RegrasCurso();
            }
            else
            {
                CarregaDados_By_RegrasCurso();
                uccCalendario.Valor = ptf.cal_id;
                _UCComboCalendario_IndexChanged();
                uccCalendario.PermiteEditar = false;

                txtCodigoTurma.Text = ptf.pft_tipoDigitoCodigoTurma != Convert.ToByte(MTR_ParametroFormacaoTurmaTipoDigito.SemControleAutomatico) ? TUR_TurmaBO.GerarCodigoTurmaNormal(esc_id, uni_id, 1, ptf, __SessionWEB.__UsuarioWEB.Usuario.ent_id) : string.Empty;

                chkProfessorEspecialista.Checked = ptf.pft_docenteEspecialista;

                CarregaFormatoAvaliacaoPorCurso(cur_id, crr_id, crp_id);

                chkProfessorEspecialista.Enabled = false;

                uccFormatoAvaliacao.Valor = ptf.fav_id;
                uccFormatoAvaliacao.PermiteEditar = false;

                uccTurno.CarregarTurnoPorParametroPeriodo(ptf);
            }
        }
        else
        {
            CarregaFormatoAvaliacaoPorCurso(cur_id, crr_id, crp_id);
            CarregaDados_By_RegrasCurso();
        }
    }

    private void AtualizarListaDocenciaCompartilhada()
    {
        if (_dgvTurma.Rows.Count > 0)
        {
            bool relacionadaRegencia = false;
            bool docCompartilhadaAntes = false;
            bool docCompartilhadaDepois = false;
            string nomeDisciplinaCompartilhada = String.Empty;
            int qtdDocenciaCompartilhada = 0;
            int maxQtdDocenciaCompartilhada = 0;
            if (aplicarNovaRegraDocenciaCompartilhada && _VS_TudIdCompartilhada > 0)
            {
                ACA_TipoDisciplina tipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaPorTudId(_VS_TudIdCompartilhada);
                maxQtdDocenciaCompartilhada = tipoDisciplina.tds_qtdeDisciplinaRelacionada;
            }

            foreach (GridViewRow disciplina in _dgvTurma.Rows)
            {
                int tud_tipo = Convert.ToInt32(_dgvTurma.DataKeys[disciplina.RowIndex]["tud_tipo"]);
                if (tud_tipo != (int)TurmaDisciplinaTipo.ComponenteRegencia)
                {
                    DataRow[] dr = _VS_ListaDocenciaCompartilhada.Select("tud_idCompartilhada="
                                                                        + _VS_TudIdCompartilhada
                                                                        + " AND tud_id="
                                                                        + _dgvTurma.DataKeys[disciplina.RowIndex]["tud_id"]);

                    if (_dgvTurma.Columns[indexColumnCompartilhada].Visible)
                    {
                        nomeDisciplinaCompartilhada = dr[0]["tud_nomeCompartilhada"].ToString();

                        bool checado = ((CheckBox)disciplina.FindControl("_chkCompartilhada")).Checked;
                        dr[0]["relacionada"] = checado;
                        if (checado)
                        {
                            docCompartilhadaDepois = true;
                            qtdDocenciaCompartilhada++;
                        }

                        if (!String.IsNullOrEmpty(dr[0]["tdr_id"].ToString()))
                        {
                            docCompartilhadaAntes = true;
                        }
                    }

                    if (tud_tipo == (int)TurmaDisciplinaTipo.Regencia)
                    {
                        relacionadaRegencia = Convert.ToBoolean(dr[0]["relacionada"]);
                    }
                }
            }

            if (docCompartilhadaAntes && !docCompartilhadaDepois)
                throw new ValidationException(String.Format(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoDocenciaCompartilhada2").ToString(), nomeDisciplinaCompartilhada));

            if (qtdDocenciaCompartilhada > 0
                && maxQtdDocenciaCompartilhada > 0
                && qtdDocenciaCompartilhada > maxQtdDocenciaCompartilhada)
            {
                throw new ValidationException(string.Format(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoQuantidadeDocenciaCompartilhada").ToString(), _dgvTurma.Columns[indexColumnCompartilhada].HeaderText, maxQtdDocenciaCompartilhada));
            }

            DataRow[] drComponentesRegencia = _VS_ListaDocenciaCompartilhada.Select("tud_idCompartilhada="
                                                                    + _VS_TudIdCompartilhada
                                                                    + " AND tud_tipo="
                                                                    + (int)TurmaDisciplinaTipo.ComponenteRegencia);
            foreach (DataRow drComponenteRegencia in drComponentesRegencia)
            {
                drComponenteRegencia["relacionada"] = relacionadaRegencia;
            }
        }
    }

    #region Métodos dos delegates

    /// <summary>
    /// Seta métodos dos delegates.
    /// </summary>
    private void SetaDelegates()
    {
        uccCurriculoPeriodo._OnSelectedIndexChange += _UCComboCurriculoPeriodo__OnSelectedIndexChange;
        uccCursoCurriculo.IndexChanged += _UCComboCursoCurriculo_IndexChanged;

        uccCalendario.IndexChanged += _UCComboCalendario_IndexChanged;
    }

    protected void uccUAEscola_IndexChangedUnidadeEscola()
    {
        try
        {
            uccCursoCurriculo.Valor = new[] { -1, -1 };

            if (uccUAEscola.Esc_ID > 0)
            {
                //caso for alteração não filtrar por situação
                byte sitCurso = _VS_tur_id > 0 ? (byte)0 : (byte)1;

                uccCursoCurriculo.CarregarCursoCurriculoPorEscola(uccUAEscola.Esc_ID, uccUAEscola.Uni_ID, sitCurso);
                uccCursoCurriculo.SetarFoco();

                if (uccCursoCurriculo.QuantidadeItensCombo == 2)
                {
                    uccCursoCurriculo.SelectedIndex = 1;
                }
            }
            else
            {
                // Se for novo, cancela os selects.
                uccCursoCurriculo.CancelSelect = _VS_tur_id <= 0;
            }

            _UCComboCursoCurriculo_IndexChanged();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _UCComboCursoCurriculo_IndexChanged()
    {
        try
        {
            uccCurriculoPeriodo.Valor = new[] { -1, -1, -1 };

            if (uccCursoCurriculo.Valor[0] > 0)
            {
                // Carrega combo de períodos do curso para a escola.
                uccCurriculoPeriodo._Combo.Items.Clear();
                uccCurriculoPeriodo._MostrarMessageSelecione = true;

                if (_VS_tur_id > 0)
                    uccCurriculoPeriodo._Load(uccCursoCurriculo.Valor[0], uccCursoCurriculo.Valor[1]);
                else
                    uccCurriculoPeriodo._LoadBy_cur_id_crr_id_esc_id_uni_id(uccCursoCurriculo.Valor[0], uccCursoCurriculo.Valor[1], uccUAEscola.Esc_ID, uccUAEscola.Uni_ID);

                uccCurriculoPeriodo._Combo.Focus();

                if (uccCurriculoPeriodo.QuantidadeItensCombo == 2)
                {
                    uccCurriculoPeriodo.SelectedIndex = 1;
                }
            }
            else
            {
                // Se for novo, cancela os selects.
                uccCurriculoPeriodo.CancelSelect = _VS_tur_id <= 0;
            }

            _UCComboCurriculoPeriodo__OnSelectedIndexChange();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _UCComboCurriculoPeriodo__OnSelectedIndexChange()
    {
        try
        {
            uccCalendario.Valor = -1;
            uccCalendario.PermiteEditar = false;
            uccFormatoAvaliacao.Valor = -1;
            uccFormatoAvaliacao.PermiteEditar = false;
            divAvaliacao.Visible = false;
            chkAvaliacao.DataSource = new DataTable();
            chkAvaliacao.DataBind();

            int cur_id = uccCurriculoPeriodo.Valor[0];
            int crr_id = uccCurriculoPeriodo.Valor[1];
            int crp_id = uccCurriculoPeriodo.Valor[2];

            int esc_id = uccUAEscola.Esc_ID;
            int uni_id = uccUAEscola.Uni_ID;

            if (cur_id > 0)
            {
                // Verifica se tem parâmetro de formação de turma ou não e carrega os dados
                // do calendário, turno e formato.
                CarregaDados_ParametroFormacao(cur_id, crr_id, crp_id, esc_id, uni_id);

                if (uccCalendario.QuantidadeItensCombo == 2)
                {
                    uccCalendario.SelectedIndex = 1;
                }

                if (_VS_tur_id <= 0 && uccCurriculoPeriodo.Valor[2] > 0)
                    uccFormatoAvaliacao.SelecionaPrimeiroItem();
            }
            else
            {
                txtCodigoTurma.Text = "";

                // Se for nova turma, cancela os selects.
                uccCalendario.CancelSelect = _VS_tur_id <= 0;

                uccTurno.AdicionaItemVazio();
                uccTurno.PermiteEditar = false;

                //// Chama o método para limpar o combo de turno.
                //CarregarTurnoPorCurriculoPeriodo(-1);
            }

            _UCComboCalendario_IndexChanged();
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _UCComboCalendario_IndexChanged()
    {
        if (uccCalendario.Valor > 0)
        {
            uccFormatoAvaliacao.Focus();
        }

        uccFormatoAvaliacao_IndexChanged();
    }

    protected void uccFormatoAvaliacao_IndexChanged()
    {
        // Atualizar disciplinas, alguns tipos dependem de calendário.
        CarregaDisciplinas();
    }

    #endregion Métodos dos delegates

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryCore));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));       
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jquery.tools.min.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroTurma.js"));

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                if (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload"))
                {
                    btnCancelar.CssClass += " btnMensagemUnload";
                }
            }
        }

        if (!IsPostBack)
        {
            // exibe/oculta campo de observação da turma segundo parâmetro.
            lblObservacao.Visible = txtObservacao.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_OBSERVACAO_CADASTRO_TURMA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            try
            {
                #region Mensagens Parâmetros Integração

                string paramHabilitaIntegracao = ACA_ParametroIntegracaoBO.ParametroValor(eChaveIntegracao.HABILITA_INTEG_COLAB_DOCENTES);

                if ((paramHabilitaIntegracao.Equals("SIM", StringComparison.OrdinalIgnoreCase)))
                {
                    string paramAvisoDocente = ACA_ParametroIntegracaoBO.ParametroValor(eChaveIntegracao.MSG_AVISO_DOCENTES);
                    string paramAtualDados = ACA_ParametroIntegracaoBO.ParametroValor(eChaveIntegracao.MSG_ATUALIZACAO_DADOS);
                    string paramDataHoraDados = ACA_ParametroIntegracaoBO.ParametroValor(eChaveIntegracao.DATA_HORA_ATUALIZACAO_DADOS_MAGISTER);

                    lblMensagemAvisoDocente.Text = UtilBO.GetErroMessage(paramAvisoDocente, UtilBO.TipoMensagem.Informacao);
                    lblMensagemAvisoDocente.Visible = (!string.IsNullOrEmpty(paramAvisoDocente));

                    if (!string.IsNullOrEmpty(paramDataHoraDados) && paramDataHoraDados != new DateTime().ToString())
                    {
                        lblMensagemAtualizacaoDados.Text = paramAtualDados + " " + paramDataHoraDados;
                    }

                    lblMensagemAtualizacaoDados.Visible = (!string.IsNullOrEmpty(paramAtualDados) && !string.IsNullOrEmpty(paramDataHoraDados));
                }

                #endregion Mensagens Parâmetros Integração

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _VS_tur_id = PreviousPage.Edit_tur_id;

                    if (_VS_tur_id > 0)
                    {
                        uccUAEscola.FiltroEscolasControladas = null;
                        uccUAEscola.MostraApenasAtivas = false;
                    }
                    else
                    {
                        uccUAEscola.FiltroEscolasControladas = true;
                        uccUAEscola.MostraApenasAtivas = true;
                    }

                    CarregarTelaInicial();
                    LoadByEntity(PreviousPage.Edit_tur_id, PreviousPage.Edit_cur_id, PreviousPage.Edit_crr_id, PreviousPage.Edit_crp_id);
                }
                else
                {
                    uccUAEscola.FiltroEscolasControladas = true;
                    uccUAEscola.MostraApenasAtivas = true;
                    CarregarTelaInicial();

                    Page.Form.DefaultFocus = uccUAEscola.VisibleUA
                                                 ? uccUAEscola.ComboUA_ClientID
                                                 : uccUAEscola.ComboEscola_ClientID;
                }
                
                HabilitaControles(fdsTurma.Controls, false);
                btnCancelar.Text = "Voltar";
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }
        
        SetaDelegates();
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Seta os javascripts para não permitir chegar duas matérias diferentes
        // no mesmo período, para as eletivas só.
        string script = ucDiscDisponibilidadeProfEletiva.Script +
                        ucDiscDocenteTurmaEletiva.Script +
                        ucDiscEletiva.Script;

        if (!String.IsNullOrEmpty(script))
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CheckBoxPeriodo", script, true);
        }
    }
    
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void chkProfessorEspecialista_CheckedChanged(object sender, EventArgs e)
    {
        int cur_id = uccCurriculoPeriodo.Valor[0];
        int crr_id = uccCurriculoPeriodo.Valor[1];
        int crp_id = uccCurriculoPeriodo.Valor[2];

        CarregaFormatoAvaliacaoPorCurso(cur_id, crr_id, crp_id);
        CarregaDisciplinas();
    }
    
    /// <summary>
    /// Método de validação do campo quantidade minima da capacidade
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValidarCapacidade_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int minCadastrados = Convert.ToInt32(args.Value);
        int totalCapacidade = Convert.ToInt32(txtCapacidade.Text);

        if (minCadastrados > 0)
        {
            args.IsValid = minCadastrados <= totalCapacidade ? true : false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        byte situacao = Convert.ToByte(ddlSituacao.SelectedValue);
        divDataEncerramento.Visible = situacao == (byte)TUR_TurmaSituacao.Encerrada || situacao == (byte)TUR_TurmaSituacao.Extinta;
        if (string.IsNullOrEmpty(txtDataEncerramento.Text) && divDataEncerramento.Visible)
        {
            txtDataEncerramento.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
        }
    }

    protected void _dgvTurma_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox _chkCompartilhada = (CheckBox)e.Row.FindControl("_chkCompartilhada");

            _chkCompartilhada.Checked = false;

            //Mostra legenda quando há uma disciplina de regência.
            if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.Regencia)
            {
                _lblMsgRegencia.Text = UtilBO.GetErroMessage("Configuração selecionada para regência será válida para todos os seus componentes.", UtilBO.TipoMensagem.Informacao);
                _lblMsgRegencia.Visible = true;
            }

            //Se for disciplina do tipo Componente Regencia então não mostra a opção
            if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.ComponenteRegencia)
            {
                _chkCompartilhada.Visible = _chkCompartilhada.Checked = e.Row.Visible = false;
            }
            else
            {
                // Checa compartilhado se é uma turma disciplina relacionada com a turma disciplina compartilhada
                _chkCompartilhada.Checked = _dgvTurma.Columns[indexColumnCompartilhada].Visible && _chkCompartilhada.Visible && Convert.ToBoolean(_dgvTurma.DataKeys[e.Row.RowIndex].Values["relacionada"]);
            }
        }
    }

    protected void ddlDisciplinasDocenciaCompartilhada_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            AtualizarListaDocenciaCompartilhada();
            _VS_TudIdCompartilhada = Convert.ToInt64(ddlDisciplinasDocenciaCompartilhada.SelectedValue);

            DataTable disciplinaCompartilhada = _VS_ListaDocenciaCompartilhada.Select("tud_idCompartilhada=" + _VS_TudIdCompartilhada.ToString()).CopyToDataTable();
            if (disciplinaCompartilhada.Rows.Count > 0)
            {
                _dgvTurma.Columns[indexColumnCompartilhada].Visible = true;
                _dgvTurma.Columns[indexColumnCompartilhada].HeaderText = ddlDisciplinasDocenciaCompartilhada.SelectedItem.Text;

                _dgvTurma.DataSource = disciplinaCompartilhada;
                _dgvTurma.DataBind();
            }
        }
        catch (ValidationException ex)
        {
            ddlDisciplinasDocenciaCompartilhada.SelectedValue = _VS_TudIdCompartilhada.ToString();
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
    }

    protected void btnVisualizarHistorico_Click(object sender, EventArgs e)
    {
        try
        {
            litHistoricoDocenciaCompartilhada.Text = ddlDisciplinasDocenciaCompartilhada.SelectedItem.Text;

            gvHistorico.PageIndex = 0;
            gvHistorico.DataSourceID = odsHistorico.ID;
            odsHistorico.SelectParameters.Clear();
            odsHistorico.SelectParameters.Add("tud_id", _VS_TudIdCompartilhada.ToString());

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacaoHistorico.Valor = itensPagina;
            // atribui essa quantidade para o grid
            gvHistorico.PageSize = itensPagina;
            // atualiza o grid
            gvHistorico.DataBind();
            UCComboQtdePaginacaoHistorico.Visible = gvHistorico.Rows.Count > 0;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar histórico.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            upnDocenciaCompartilhada.Update();
            upnHistoricoDocenciaCompartilhada.Update();
        }
        
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "VisualizarHistorico", "$(document).ready(function() { $('.divHistoricoDocenciaCompartilhada').dialog('open'); });", true);
    }

    protected void gvHistorico_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistrosHistorico.Total = TUR_TurmaDisciplinaRelacionadaBO.GetTotalRecords();
    }

    protected void UCComboQtdePaginacaoHistorico_IndexChanged()
    {
        try
        {
            // atribui nova quantidade itens por página para o grid
            gvHistorico.PageSize = UCComboQtdePaginacaoHistorico.Valor;
            gvHistorico.PageIndex = 0;
            // atualiza o grid
            gvHistorico.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar histórico.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Eventos
}