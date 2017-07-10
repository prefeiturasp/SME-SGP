namespace MSTech.GestaoEscolar.BLL.Caching
{
    /// <summary>
    /// Constantes: chaves para acesso aos objetos armazenados em cache.
    /// </summary>
    public class ModelCache
    {
        #region Consultas fechamento

        /// <summary>
        /// Key para o fechamento de bimestre para uma turma.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_BIMESTRE_MODEL_KEY = "Cache_GetSelectBy_TurmaDisciplinaPeriodo_{0}_{1}_{2}_{3}";
        public const string FECHAMENTO_BIMESTRE_PATTERN_KEY = "Cache_GetSelectBy_TurmaDisciplinaPeriodo";


        /// <summary>
        /// Key para o fechamento de bimestre para uma turma.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_FINAL_MODEL_KEY = "Cache_GetSelectBy_TurmaDisciplinaFinal_{0}_{1}_{2}_{3}";
        public const string FECHAMENTO_FINAL_PATTERN_KEY = "Cache_GetSelectBy_TurmaDisciplinaFinal";

        /// <summary>
        /// Key para o fechamento de bimestre para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY = "Cache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia_{0}_{1}_{2}_{3}";
        public const string FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY = "Cache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia";

        /// <summary>
        /// Key para o fechamento de bimestre para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY = "Cache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia_{0}_{1}_{2}_{3}";
        public const string FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY = "Cache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia";

        /// <summary>
        /// Key para o fechamento de bimestre para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_RECUPERACAO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY = "Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia_{0}_{1}_{2}";
        public const string FECHAMENTO_RECUPERACAO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY = "Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia";

        /// <summary>
        /// Key para o fechamento de bimestre para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_RECUPERACAO_FINAL_MODEL_KEY = "Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina_{0}_{1}_{2}";
        public const string FECHAMENTO_RECUPERACAO_FINAL_PATTERN_KEY = "Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina";

        /// <summary>
        /// Key para o fechamento de bimestre (componentes da regência) para uma turma.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY = "Cache_GetSelect_ComponentesRegencia_By_TurmaFormato_{0}_{1}_{2}";
        public const string FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY = "Cache_GetSelect_ComponentesRegencia_By_TurmaFormato";

        /// <summary>
        /// Key para o fechamento de bimestre (componentes da regência) para uma turma.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID da avaliação.
        /// </remarks>
        public const string FECHAMENTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY = "Cache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final_{0}_{1}_{2}";
        public const string FECHAMENTO_FINAL_COMPONENTES_REGENCIA_PATTERN_KEY = "Cache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final";

        /// <summary>
        /// Key para o fechamento automatico de bimestre para uma turma.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo periodo calendario.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo periodo calendario.
        /// </remarks>
        public const string FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY = "Cache_GetSelectFechamento_{0}_{1}";
        public const string FECHAMENTO_AUTO_BIMESTRE_PATTERN_KEY = "Cache_GetSelectFechamento";

        /// <summary>
        /// Key para o fechamento automatico de bimestre para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo periodo calendario.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo periodo calendario.
        /// </remarks>
        public const string FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY = "Cache_GetSelectFechamentoFiltroDeficiencia_{0}_{1}";
        public const string FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY = "Cache_GetSelectFechamentoFiltroDeficiencia";

        /// <summary>
        /// Key para o fechamento automatico de bimestre (componentes da regência) para uma turma.
        /// {0} : ID da turma.
        /// {1} : ID do tipo periodo calendario.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// {1} : ID do tipo periodo calendario.
        /// </remarks>
        public const string FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY = "Cache_GetSelectFechamentoComponentesRegencia_{0}_{1}";
        public const string FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY = "Cache_GetSelectFechamentoComponentesRegencia";

        /// <summary>
        /// Key para o fechamento automatico final para uma turma.
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string FECHAMENTO_AUTO_FINAL_MODEL_KEY = "Cache_GetSelectFechamentoFinal_{0}";
        public const string FECHAMENTO_AUTO_FINAL_PATTERN_KEY = "Cache_GetSelectFechamentoFinal";

        /// <summary>
        /// Key para o fechamento automatico final para uma turma (filtro deficiencia).
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY = "Cache_GetSelectFechamentoFiltroDeficienciaFinal_{0}";
        public const string FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY = "Cache_GetSelectFechamentoFiltroDeficienciaFinal";

        /// <summary>
        /// Key para o fechamento automatico final (componentes da regência) para uma turma.
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY = "Cache_GetSelectFechamentoComponentesRegenciaFinal_{0}";
        public const string FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_PATTERN_KEY = "Cache_GetSelectFechamentoComponentesRegenciaFinal";

        #endregion

        #region Turma

        /// <summary>
        /// Key para carregar turmas.
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string TURMA_MODEL_KEY = "TUR_Turma_GetEntity_{0}";
        public const string TURMA_PATTERN_KEY = "TUR_Turma_GetEntity";

        /// <summary>
        /// Key para carregar turmas.
        /// {0} : ID  da entidade.
        /// {1} : ID do docente
        /// </summary>
        /// <remarks>
        /// {0} : ID da emtidade .
        /// {1} : ID do docente
        /// </remarks>
        public const string TURMA_SELECIONA_POR_DOCENTE_CONTROLE_TURMA_MODEL_KEY = "TUR_Turma_SelecionaPorDocenteControleTurma_{0}_{1}";
        public const string TURMA_SELECIONA_POR_DOCENTE_CONTROLE_TURMA_PATTERN_KEY = "TUR_Turma_SelecionaPorDocenteControleTurma";

        /// <summary>
        /// Key para verificar se a turma possui um tipo de disciplina
        /// {0} : ID da turma
        /// {1} : Tipo de disciplina
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma
        /// {1} : Tipo de disciplina
        /// </remarks>
        public const string TURMA_POSSUI_DISCIPLINA_POR_TIPO_MODEL_KEY = "TUR_Turma_PossuiDisciplinaPorTipo_{0}_{1}";
        public const string TURMA_POSSUI_DISCIPLINA_POR_TIPO_PATTERN_KEY = "TUR_Turma_PossuiDisciplinaPorTipo";

        /// <summary>
        /// Key para carregar turmas por curso, escola e ano de calendário mínimo.
        /// {0} ID da escola
        /// {1} ID da unidade de escola.
        /// {2} ID do curso.
        /// {3} ID do currículo do curso.
        /// {4} ID do curriculo período.
        /// {5} Ano de validação do calendário.
        /// </summary>
        /// <remarks>
        /// {0} ID da escola
        /// {1} ID da unidade de escola.
        /// {2} ID do curso.
        /// {3} ID do currículo do curso.
        /// {4} ID do curriculo período.
        /// {5} Ano de validação do calendário.
        /// </remarks>
        public const string TURMA_ESCOLA_CURSO_PERIODO_CALENDARIO_MINIMO = "Cache_SelecionaPorEscolaCursoPeriodoCalendarioMinimo_{0}_{1}_{2}_{3}_{4}_{5}";

        #endregion Turma

        #region Turma Disciplina

        /// <summary>
        /// Key para carregar turmas disciplinas.
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string TURMA_DISCIPLINA_MODEL_KEY = "TUR_TurmaDisciplina_GetEntity_{0}";
        public const string TURMA_DISCIPLINA_PATTERN_KEY = "TUR_TurmaDisciplina_GetEntity";

        /// <summary>
        /// Key para carregar turmas disciplinas.
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string TURMA_DISCIPLINA_TURMA_MODEL_KEY = "TUR_TurmaDisciplina_ByTurma_{0}";
        public const string TURMA_DISCIPLINA_TURMA_PATTERN_KEY = "TUR_TurmaDisciplina_ByTurma";

        /// <summary>
        /// Key para carregar as disciplinas por turma docente sem vigencia.
        /// {0} : ID da turma.
        /// {1} : ID do docente.
        /// {2} : Tipo de lancamento de frequencia.
        /// {3} : Se a turma é de regencia.
        /// {4} : Tipo da regencia.
        /// {5} : Filtrar turmas ativas.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// {1} : ID do docente.
        /// {2} : Tipo de lancamento de frequencia.
        /// {3} : Se a turma é de regencia.
        /// {4} : Tipo da regencia.
        /// {5} : Filtrar turmas ativas.
        /// </remarks>
        public const string TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_MODEL_KEY = "Cache_SelecionaDisciplinaPorTurmaDocente_FrequenciaAulas_SemVigencia_{0}_{1}_{2}_{3}_{4}_{5}";
        public const string TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY = "Cache_SelecionaDisciplinaPorTurmaDocente_FrequenciaAulas_SemVigencia";

        /// <summary>
        /// Key para carregar as disciplinas por turma docente sem vigencia.
        /// {0} : ID da turma.
        /// {1} : Verifica disciplina principal.
        /// {2} : Se a turma é de regencia.
        /// {3} : Tipo da regencia.
        /// {4} : Filtrar turmas ativas.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// {1} : Verifica disciplina principal.
        /// {2} : Se a turma é de regencia.
        /// {3} : Tipo da regencia.
        /// {4} : Filtrar turmas ativas.
        /// </remarks>
        public const string TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMA_SEM_VIGENCIA_MODEL_KEY = "Cache_SelecionaDisciplinaPorTurma_FrequenciaAulas_SemVigencia_{0}_{1}_{2}_{3}_{4}";

        /// <summary>
        /// Key para carregar as disciplinas por turma docente sem vigencia.
        /// {0} : ID do docente.
        /// {1} : Tipo de lancamento de frequencia.
        /// {2} : Se a turma é de regencia.
        /// {3} : Tipo da regencia.
        /// {4} : Filtrar turmas ativas.
        /// </summary>
        /// <remarks>
        /// {0} : ID do docente.
        /// {1} : Tipo de lancamento de frequencia.
        /// {2} : Se a turma é de regencia.
        /// {3} : Tipo da regencia.
        /// {4} : Filtrar turmas ativas.
        /// </remarks>
        public const string TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_DOCENTE_SEM_VIGENCIA_MODEL_KEY = "Cache_SelecionaDisciplinaPorDocente_FrequenciaAulas_SemVigencia_{0}_{1}_{2}_{3}_{4}";

        /// <summary>
        /// Key para selecionar dados relacionados pelo tud_id para evitar varias buscas.
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string CONTROLE_TURMA_SELECIONA_ENTIDADES_POR_TURMA_DISCIPLINA_MODEL_KEY = "Cache_SelecionaEntidadesControleTurmas_{0}";

        /// <summary>
        /// Key para carregar turmas.
        /// {0} : ID da turma do docente .
        /// {1} : ID do docente
        /// </summary>
        /// <remarks>
        /// {0} : ID  da turma do docente.
        /// {1} : ID do docente
        /// </remarks>
        public const string TURMA_SELECIONA_RELACIONADA_VIGENTE_BY_DISCIPLINA_COMPARTILHADA_MODEL_KEY = "TUR_Turma_SelecionaRelacionadaVigenteBy_DisciplinaCompartilhada_{0}_{1}";
        public const string TURMA_SELECIONA_RELACIONADA_VIGENTE_BY_DISCIPLINA_COMPARTILHADA_PATTERN_KEY = "TUR_Turma_SelecionaRelacionadaVigenteBy_DisciplinaCompartilhada";

        #endregion Turma Disciplina

        #region Formato de avaliação

        /// <summary>
        /// Key para carregar formato de avaliação.
        /// {0} : ID do formato de avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID do formato de avaliação.
        /// </remarks>
        public const string FORMATO_AVALIACAO_MODEL_KEY = "ACA_FormatoAvaliacao_GetEntity_{0}";
        public const string FORMATO_AVALIACAO_PATTERN_KEY = "ACA_FormatoAvaliacao_GetEntity";
        public const string FORMATO_AVALIACAO_POR_TURMADISCIPLINA_MODEL_KEY = "Cache_SelecionaPorTud_{0}";

        #endregion Formato de avaliação

        #region Avaliação

        /// <summary>
        /// Key para avaliações periódicas ou periódicas mais final por formato e período.
        /// {0} : ID do formato de avaliação.
        /// {1} : ID do tipo de período do calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID do formato de avaliação.
        /// {1} : ID do tipo de período do calendário.
        /// </remarks>
        public const string AVALIACAO_PERIODO_RELACIONADAS_MODEL_KEY = "Cache_ConsultaPor_Periodo_Relacionadas_{0}_{1}";

        /// <summary>
        /// Key para avaliação final por formato.
        /// {0} : ID do formato de avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID do formato de avaliação.
        /// </remarks>
        public const string AVALIACAO_FINAL_FORMATO_MODEL_KEY = "Cache_SelectAvaliacaoFinal_PorFormato_{0}";

        /// <summary>
        /// Key para carregar as avaliações pelo tipo de avaliação e periodo
        /// {0} : ID do formato de avaliação.
        /// {1} : ID do periodo.
        /// </summary>
        /// <remarks>
        /// {0} : ID do formato de avaliação.
        /// {1} : ID do periodo.
        /// </remarks>
        public const string AVALIACAO_POR_FORMATO_PERIODO_MODEL_KEY = "Cache_Avaliacao_Por_FormatoAvaliacaoPeriodo_{0}_{1}";

        /// <summary>
        /// Key para avaliação final por periodoEfetivacao.
        /// {0} : ID da turma.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID do turmadisciplina.
        /// {3} : ID do periodo da avaliacao periodica/periodica final.
        /// {4} : ID do periodo da recuperacao.
        /// {5} : Existe avaliacao final.
        /// {6} : Existe recuperacao final.
        /// {7} : Verifica regras recuperacao.
        /// {8} : Efetivacao semestral.
        /// {9} : Filtrar por tpc_id.        
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// {1} : ID do formato de avaliação.
        /// {2} : ID do turmadisciplina.
        /// {3} : ID do periodo da avaliacao periodica/periodica final.
        /// {4} : ID do periodo da recuperacao.
        /// {5} : Existe avaliacao final.
        /// {6} : Existe recuperacao final.
        /// {7} : Verifica regras recuperacao.
        /// {8} : Efetivacao semestral.
        /// {9} : Filtrar por tpc_id.        
        /// </remarks>
        public const string AVALIACAO_PERIODO_EFETIVACAO_MODEL_KEY = "Cache_SelectAvaliacaoFinal_PorPeriodoEfetivacao_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}";        

        #endregion Avaliação

        #region Escala avaliação

        /// <summary>
        /// Key para carregar escala de avaliação.
        /// {0} : ID da escala de avaliação.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escala de avaliação.
        /// </remarks>
        public const string ESCALA_AVALIACAO_MODEL_KEY = "ACA_EscalaAvaliacao_GetEntity_{0}";
        public const string ESCALA_AVALIACAO_PATTERN_KEY = "ACA_EscalaAvaliacao_GetEntity";

        /// <summary>
        /// Key para retornar ordem parecer.
        /// {0} : ID da escala de avaliação.
        /// {0} : Valor da escala.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escala de avaliação.
        /// {0} : Valor da escala.
        /// </remarks>
        public const string ESCALA_AVALIACAO_RETORNA_ORDEM_PARECER_MODEL_KEY = "Cache_RetornaOrdem_Parecer_{0}_{1}";
        public const string ESCALA_AVALIACAO_RETORNA_ORDEM_PARECER_PATTERN_KEY = "Cache_RetornaOrdem_Parecer";

        #endregion Escala avaliação

        #region Escala avaliação numerica

        /// <summary>
        /// Key para carregar escala de avaliação numerica.
        /// {0} : ID da escala de avaliação numerica.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escala de avaliação numerica.
        /// </remarks>
        public const string ESCALA_AVALIACAO_NUMERICA_MODEL_KEY = "ACA_EscalaAvaliacaoNumerica_GetEntity_{0}";
        public const string ESCALA_AVALIACAO_NUMERICA_PATTERN_KEY = "ACA_EscalaAvaliacaoNumerica_GetEntity";

        public const string ESCALA_AVALIACAO_NUMERICA_SELECTBY_ESCALA_KEY = "Cache_GetSelectBy_Escala_Numerica_{0}";

        #endregion Escala avaliação numerica

        #region Escala avaliação parecer

        /// <summary>
        /// Key para carregar escala de avaliação parecer.
        /// {0} : ID da escala de avaliação.
        /// {1} : ID da escala de avaliação parecer.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escala de avaliação.
        /// {1} : ID da escala de avaliação parecer.
        /// </remarks>
        public const string ESCALA_AVALIACAO_PARECER_MODEL_KEY = "ACA_EscalaAvaliacaoParecer_GetEntity_{0}";
        public const string ESCALA_AVALIACAO_PARECER_PATTERN_KEY = "ACA_EscalaAvaliacaoParecer_GetEntity";

        public const string ESCALA_AVALIACAO_PARECER_SELECTBY_ESCALA_KEY = "Cache_GetSelectBy_Escala_Parecer_{0}";

        #endregion Escala avaliação numerica

        #region Período calendário

        /// <summary>
        /// Key para carregar período do calendário por calendário.
        /// {0} : ID do calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID do calendário.
        /// </remarks>
        public const string PERIODO_CALENDARIO_POR_CALENDARIO_MODEL_KEY = "Cache_SelecionaPeriodosCalendario_cal_id_{0}";

         /// <summary>
        /// Key para carregar período do calendário das escolas por calendário.
        /// {0} : ID do calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID do calendário.
        /// </remarks>
        public const string ESCOLA_PERIODO_CALENDARIO_POR_CALENDARIO_MODEL_KEY = "Cache_SelecionaEscolaPeriodosCalendario_cal_id_{0}";

        /// <summary>
        /// Key para carregar eventos por calendário, turma, tipo de evento e período.
        /// {0} : Id do calendário anual.
        /// {1} : Id do período do calendário.
        /// {2} : Id da escola.
        /// {3} : Id da unidade escolar.
        /// {4} : Id do tipo de evento.
        /// </summary>
        /// <remarks>
        /// {0} : Id do calendário anual.
        /// {1} : Id do período do calendário.
        /// {2} : Id da escola.
        /// {3} : Id da unidade escolar.
        /// {4} : Id do tipo de evento.
        /// </remarks>
        public const string EVENTOS_EFETIVACAO_POR_TIPO_PERIODO_ESCOLA_MODEL_KEY = "Cache_EventoEfetivacaoTodosPorPeriodoEscola_{0}_{1}_{2}_{3}_{4}_{5}";
        public const string EVENTOS_EFETIVACAO_POR_TIPO_PERIODO_ESCOLA_PATTERN_KEY = "Cache_EventoEfetivacaoTodosPorPeriodoEscola";
        #endregion Período calendário

        #region Eventos calendário

        /// <summary>
        /// Key para carregar eventos por calendário, turma, tipo de evento e período.
        /// {0} : ID do calendário.
        /// {1} : ID da turma.
        /// {2} : tipo de evento.
        /// {3} : ID do período de calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID do calendário.
        /// {1} : ID da turma.
        /// {2} : tipo de evento.
        /// {3} : ID do período de calendário.
        /// </remarks>
        public const string EVENTOS_CALENDARIO_TURMA_TIPO_PERIODO_MODEL_KEY = "Cache_EventoEfetivacaoTodos_{0}_{1}_{2}_{3}_{4}";
        public const string EVENTOS_CALENDARIO_TURMA_TIPO_PERIODO_PATTERN_KEY = "Cache_EventoEfetivacaoTodos";
        #endregion Eventos calendário

        #region Curso

        /// <summary>
        /// Key para carregar curriculo periodo por curso e disciplina.
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID da disciplina.
        /// {3} : Situação do curso.
        /// {4} : ID da entidade.
        /// {5} : ID do calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID da disciplina.
        /// {3} : Situação do curso.
        /// {4} : ID da entidade.
        /// {5} : ID do calendário.
        /// </remarks>
        public const string CURSO_CALENDARIO_ESCOLA_DISCIPLINA_MODEL_KEY = "Cache_SelecionaCursoCurriculoCalendarioEscolaDisciplina_{0}_{1}_{2}_{3}_{4}_{5}";

        /// <summary>
        /// Key para carregar os cursos por modalidade de ensino.
        /// {0} : ID tipo modalidade de ensino.
        /// {1} : ID da escola.
        /// {2} : ID da unidade da escola.
        /// {3} : ID da entidade.
        /// </summary>
        /// <remarks>
        /// {0} : ID tipo modalidade de ensino.
        /// {1} : ID da escola.
        /// {2} : ID da unidade da escola.
        /// {3} : ID da entidade.
        /// </remarks>
        public const string CURSO_MODALIDADE_ENSINO_MODEL_KEY = "Cache_Seleciona_Cursos_Por_ModalidadeEnsino_{0}_{1}_{2}_{3}";
        #endregion Curso

        #region Curriculo Periodo

        /// <summary>
        /// Key para carregar curriculo periodo por curso e disciplina.
        /// {0} : ID do curso.
        /// {1} : ID da curriculo.
        /// {2} : ID da disciplina.
        /// {3} : ID da entidade
        /// </summary>
        /// <remarks>
        /// {0} : ID do curso.
        /// {1} : ID da curriculo.
        /// {2} : ID da disciplina.
        /// {3} : ID da entidade
        /// </remarks>
        public const string CURRICULO_PERIODO_CURSO_DISCIPLINA_MODEL_KEY = "Cache_SelecionaPorCursoDisciplina_{0}_{1}_{2}_{3}";

        /// <summary>
        /// Key para carregar curriculo periodo por turma.
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string CURRICULO_PERIODO_TURMA_TIPO_NORMAL_MODEL_KEY = "Cache_SelecionaPorTurmaTipoNormal_{0}";
        public const string CURRICULO_PERIODO_TURMA_TIPO_NORMAL_PATTERN_KEY = "Cache_SelecionaPorTurmaTipoNormal";

        /// <summary>
        /// Key para carregar curriculo periodo por curso, escola e ordem de período mínimo.
        /// {0} ID da escola
        /// {1} ID da unidade de escola.
        /// {2} ID do curso.
        /// {3} ID do currículo do curso.
        /// {4} Ordem de validação do período.
        /// </summary>
        /// <remarks>
        /// {0} ID da escola
        /// {1} ID da unidade de escola.
        /// {2} ID do curso.
        /// {3} ID do currículo do curso.
        /// {4} Ordem de validação do período.
        /// </remarks>
        public const string CURRICULO_PERIODO_ESCOLA_CURSO_PERIODO_ORDEM_MODEL_KEY = "Cache_SelecionaPorEscolaCursoPeriodoOrdem_{0}_{1}_{2}_{3}_{4}";

        #endregion Curriculo Periodo

        #region Matricula Turma Disciplina

        /// <summary>
        /// Key para carregar curriculo periodo por curso e disciplina.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo de período de calendário.
        /// {2} : Tipo de docente.
        /// {3} : Indica se a exibição do nome do aluno é para documento oficial.
        /// {4} : Data início do período do calendário.
        /// {5} : Data fim do período do calendário.
        /// {6} : IDs das turmas concatenadas
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do tipo de período de calendário.
        /// {2} : Tipo de docente.
        /// {3} : Indica se a exibição do nome do aluno é para documento oficial.
        /// {4} : Data início do período do calendário.
        /// {5} : Data fim do período do calendário.
        /// {6} : IDs das turmas concatenadas
        /// </remarks>
        public const string ALUNOS_ATIVOS_COC_DISCIPLINA = "Cache_SelecionaAlunosAtivosCOCPorTurmaDisciplina_{0}_{1}_{2}_{3}_{4}_{5}_{6}";
        public const string ALUNOS_ATIVOS_COC_DISCIPLINA_PATTERN_KEY = "Cache_SelecionaAlunosAtivosCOCPorTurmaDisciplina";

        #endregion Matricula Turma Disciplina
        
        #region Pendência de Fechamento

        /// <summary>
        /// Key para carregar pendências de fechamento
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.
        
        /// </summary>
        /// <remarks>
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.        
        /// </remarks>
        public const string PENDENCIA_FECHAMENTO_ESCOLA_PATTERN_KEY = "Cache_SelecionaPendenciasFechamentoDisciplinas_{0}_{1}_{2}";

        #endregion Pendência de Fechamento

        #region Pendências disciplina

        /// <summary>
        /// Key para carregar pendências de fechamento
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.
        /// {3} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.
        /// {3} : ID da turma disciplina.
        /// </remarks>
        public const string PENDENCIAS_DISCIPLINA_MODEL_KEY = "Cache_SelecionaPendencias_{0}_{1}_{2}_{3}";

        /// <summary>
        /// Key para carregar pendências de fechamento
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.

        /// </summary>
        /// <remarks>
        /// {0} : ID da escola.
        /// {1} : ID da unidade de escola.
        /// {2} : ID do calendario.        
        /// </remarks>
        public const string PENDENCIAS_DISCIPLINAS_ESCOLA_PATTERN_KEY = "Cache_SelecionaPendencias_{0}_{1}_{2}";

        #endregion Pendências disciplina

        #region Calendario anual

        /// <summary>
        /// Key para carregar calendario anual.
        /// {0} : ID do calendario anual.
        /// {1} : Avaliacoes relacioandas.
        /// {2} : ID do turma disciplina.
        /// {3} : ID do formato avaliacao.
        /// </summary>
        /// <remarks>
        /// {0} : ID do calendario anual.
        /// {1} : Avaliacoes relacioandas.
        /// {2} : ID do turma disciplina.
        /// {3} : ID do formato avaliacao.
        /// </remarks>
        public const string CALENDARIO_ANUAL_FORMATOAVALIACAOTURMADISCIPLINA_MODEL_KEY = "ACA_CalendarioAnual_FormatoAvaliacaoTurmaDisciplina_{0}_{1}_{2}_{3}";
        public const string CALENDARIO_ANUAL_FORMATOAVALIACAOTURMADISCIPLINA_PATTERN_KEY = "ACA_CalendarioAnual_FormatoAvaliacaoTurmaDisciplina";

        /// <summary>
        /// Key para carregar calendario anual.
        /// {0} : ID do calendario anual.
        /// </summary>
        /// <remarks>
        /// {0} : ID do calendario anual.
        /// </remarks>
        public const string CALENDARIO_ANUAL_MODEL_KEY = "ACA_CalendarioAnual_GetEntity_{0}";
        public const string CALENDARIO_ANUAL_PATTERN_KEY = "ACA_CalendarioAnual_GetEntity";

        /// <summary>
        /// Key para carregar calendario anual por turma
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string CALENDARIO_ANUAL_POR_TURMA_MODEL_KEY = "ACA_CalendarioAnual_SelecionaPorTurma_{0}";
        public const string CALENDARIO_ANUAL_POR_TURMA_PATTERN_KEY = "ACA_CalendarioAnual_SelecionaPorTurma";

        /// <summary>
        /// Key para carregar calendario anual por turma disciplina
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string CALENDARIO_ANUAL_POR_TURMADISCIPLINA_MODEL_KEY = "ACA_CalendarioAnual_SelecionaPorTurmaDisciplina_{0}";
        public const string CALENDARIO_ANUAL_POR_TURMADISCIPLINA_PATTERN_KEY = "ACA_CalendarioAnual_SelecionaPorTurmaDisciplina";

        #endregion Calendario anual

        #region Tipo disciplina

        /// <summary>
        /// Key para carregar tipo de disciplina.
        /// {0} : ID do tipo de disciplia.
        /// </summary>
        /// <remarks>
        /// {0} : ID do tipo de disciplina.
        /// </remarks>
        public const string TIPO_DISCIPLINA_MODEL_KEY = "ACA_TipoDisciplina_GetEntity_{0}";
        public const string TIPO_DISCIPLINA_PATTERN_KEY = "ACA_TipoDisciplina_GetEntity";

        #endregion Tipo disciplina

        #region Matricula turma

        /// <summary>
        /// Key para carregar Matricula turma.
        /// {0} : ID do Aluno.
        /// {1} : ID do Matricula turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID do Aluno.
        /// {1} : ID do Matricula turma.
        /// </remarks>
        public const string MATRICULA_TURMA_MODEL_KEY = "MTR_MatriculaTurma_GetEntity_{0}_{1}";
        public const string MATRICULA_TURMA_PATTERN_KEY = "MTR_MatriculaTurma_GetEntity";

        #endregion Matricula turma

        #region Turma nota

        /// <summary>
        /// Key para carregar Matricula turma nota. 
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma nota.
        /// </summary>
        /// <remarks>
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma nota.
        /// </remarks>
        public const string TURMA_NOTA_MODEL_KEY = "CLS_TurmaNota_GetEntity_{0}_{1}";
        public const string TURMA_NOTA_PATTERN_KEY = "CLS_TurmaNota_GetEntity";

        #endregion Turma nota

        #region Turma aula

        /// <summary>
        /// Key para carregar Matricula turma aula. 
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma aula.
        /// </summary>
        /// <remarks>
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma aula.
        /// </remarks>
        public const string TURMA_AULA_MODEL_KEY = "CLS_TurmaAula_GetEntity_{0}_{1}";
        public const string TURMA_AULA_PATTERN_KEY = "CLS_TurmaAula_GetEntity";

        /// <summary>
        /// Key para carregar Matricula turma aula. 
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma aula.
        /// {2} : ID do TipoDocente.
        /// </summary>
        /// <remarks>
        /// {0} : ID do Turma disciplina.
        /// {1} : ID do Turma aula.
        /// {2} : ID do TipoDocente.
        /// </remarks>
        public const string TURMA_AULA_QTDE_AULASDADAS_MODEL_KEY = "CLS_TurmaAula_QtdeAulasDadas_{0}_{1}_{2}";
        public const string TURMA_AULA_QTDE_AULASDADAS_PATTERN_KEY = "CLS_TurmaAula_QtdeAulasDadas";

        #endregion Turma nota

        #region Tipo Turno

        /// <summary>
        /// Key para carregar tipo turno por turma.         
        /// {0} : ID do Turma aula.
        /// </summary>
        /// <remarks>        
        /// {0} : ID do Turma aula.
        /// </remarks>
        public const string TIPO_TURNO_TURMA_MODEL_KEY = "ACA_TipoTurnoPorTurma_{0}";
        public const string TIPO_TURNO_TURMA_PATTERN_KEY = "ACA_TipoTurnoPorTurma";
        
        #endregion

        #region Docente

        /// <summary>
        /// Key para carregar o docente
        /// {0} : ID da entidade do sistema.
        /// {1} : ID da pessoa do docente.
        /// </summary>
        /// <remarks>
        /// {0} : ID da entidade do sistema.
        /// {1} : ID da pessoa do docente.
        /// </remarks>
        public const string DOCENTE_POR_ENTIDADE_PESSOA_MODEL_KEY = "ACA_Docente_GetSelectBy_Pessoa_{0}_{1}";

        #endregion

        #region Tipo Docente

        /// <summary>
        /// Key para carregar o tipo do docente pela posição
        /// {0} : ID do tipo do docente.
        /// </summary>
        /// <remarks>
        /// {0} : ID do tipo do docente.
        /// </remarks>
        public const string TIPO_DOCENTE_POSICAO_POR_TIPO_DOCENTE_MODEL_KEY = "ACA_TipoDocente_Por_Posicao_{0}";

        #endregion

        #region Aulas Previstas

        /// <summary>
        /// Key para carregar as aulas previstas por turma disciplina.
        /// {0} : ID da turma disciplina.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// </remarks>
        public const string TURMA_DISCIPLINA_AULA_PREVISTA_MODEL_KEY = "TUR_TurmaDisciplinaAulaPrevista_GetEntity_{0}";
        public const string TURMA_DISCIPLINA_AULA_PREVISTA_PATERN_KEY = "TUR_TurmaDisciplinaAulaPrevista_GetEntity";

        #endregion

        #region Permissao Docente

        /// <summary>
        /// Key para carregar o docente
        /// {0} : ID da entidade do sistema.
        /// {1} : ID da pessoa do docente.
        /// </summary>
        /// <remarks>
        /// {0} : ID da entidade do sistema.
        /// {1} : ID da pessoa do docente.
        /// </remarks>
        public const string PERMISSAODOCENTE_PERMISSAOMODULO_MODEL_KEY = "CFG_PermissaoDocente_SelecionaPermissaoModulo_{0}_{1}";
        public const string PERMISSAODOCENTE_PERMISSAOMODULO_PATTERN_KEY = "CFG_PermissaoDocente_SelecionaPermissaoModulo";

        #endregion

        #region Justificativa de pendência no fechamento

        /// <summary>
        /// Key para carregar a justificativa de pendência por turma disciplina e período do calendário.
        /// {0} : ID da turma disciplina.
        /// {1} : ID do calendário.
        /// {2} : ID do período do calendário.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma disciplina.
        /// {1} : ID do calendário.
        /// {2} : ID do período do calendário.
        /// </remarks>
        public const string FECHAMENTO_JUSTIFICATIVA_PENDENCIA_MODEL_KEY = "CLS_FechamentoJustificativaPendencia_GetEntity_{0}_{1}_{2}";
        //public const string FECHAMENTO_JUSTIFICATIVA_PENDENCIA_PATERN_KEY = "CLS_FechamentoJustificativaPendencia_GetEntity";

        #endregion

        #region Territorio do saber

        /// <summary>
        /// Key para carregar relação de experiências e territórios do saber por turma
        /// {0} : ID da turma.
        /// </summary>
        /// <remarks>
        /// {0} : ID da turma.
        /// </remarks>
        public const string TURMA_DISCIPLINA_TERRITORIO_TURMA_MODEL_KEY = "TUR_TurmaDisciplinaTerritorio_PorTurma_{0}";

        #endregion Territorio do saber

        #region MenuCoreSSO

        public const string MENU_SISTEMA_GRUPO_VISAO_MODEL_KEY = "Cache_CarregarMenuXML_{0}_{1}_{2}";
        public const string MENU_SISTEMA_GRUPO_VISAO_PATTERN_KEY = "Cache_CarregarMenuXML";

        #endregion MenuCoreSSO

        #region Alunos

        public const string ALUNO_BUSCA_RELATORIOS_AEE_DOCENTE_KEY = "Cache_BuscaAlunosRelatoriosAEEPorDocente_{0}_{1}_{2}";
        
        #endregion

        #region Relatorio Atendimento

        public const string RELATORIO_ATENDIMENTO_BUSCA_ESTRUTURA_RELATORIO_KEY = "Cache_SelecionaRelatorio_{0}_{1}";

        #endregion 
    }
}
