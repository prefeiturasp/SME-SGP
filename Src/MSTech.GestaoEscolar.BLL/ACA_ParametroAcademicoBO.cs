using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Reflection;

namespace MSTech.GestaoEscolar.BLL
{

    #region CustomAttribute

    /// <summary>
    /// Custom atributo para os enumeradores de parametro academico
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class parametroAcademicoAttributes : Attribute
    {
        private parametroAttributes paramAttributes;

        public parametroAcademicoAttributes(
            string description
            , string errorMessage
            , bool obrigatorio
            , bool integridadeEscolas
            , TipoParametroAcademico tipo
            , DataTypeParametroAcademico dataType)
        {
            this.paramAttributes.Description = description;
            this.paramAttributes.ErrorMessage = errorMessage;
            this.paramAttributes.Obrigatorio = obrigatorio;
            this.paramAttributes.IntegridadeEscolas = integridadeEscolas;
            this.paramAttributes.Tipo = tipo;
            this.paramAttributes.DataType = dataType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <example parametroAcademicoAttributes.Get("TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA").Description;></example>
        /// <returns></returns>
        public static parametroAttributes Get(string name)
        {

            MemberInfo[] mi = typeof(eChaveAcademico).GetMember(name);
            if (mi != null && mi.Length > 0)
            {
                parametroAcademicoAttributes attr = Attribute.GetCustomAttribute(mi[0],
                    typeof(parametroAcademicoAttributes)) as parametroAcademicoAttributes;
                if (attr != null)
                {
                    attr.paramAttributes.Parametro = (eChaveAcademico) Enum.Parse(typeof(eChaveAcademico), name);
                    return attr.paramAttributes;
                }
            }
            return new parametroAttributes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enm"></param>
        /// <example parametroAcademicoAttributes.Get(eChaveAcademico.TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA).Description;></example>
        /// <returns></returns>
        public static parametroAttributes Get(object enm)
        {
            if (enm != null)
            {
                MemberInfo[] mi = enm.GetType().GetMember(enm.ToString());
                if (mi != null && mi.Length > 0)
                {
                    parametroAcademicoAttributes attr = Attribute.GetCustomAttribute(mi[0],
                        typeof(parametroAcademicoAttributes)) as parametroAcademicoAttributes;
                    if (attr != null)
                    {
                        attr.paramAttributes.Parametro = (eChaveAcademico) enm;
                        return attr.paramAttributes;
                    }
                }
            }
            return new parametroAttributes();
        }
    }
    #endregion CustomAttribute

    #region Estruturas
        
    /// <summary>
    /// Struct para os atributos customizados dos enumeradores de paramatros academicos
    /// </summary>
    public struct parametroAttributes
    {
        public eChaveAcademico Parametro { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        public bool Obrigatorio { get; set; }
        public bool IntegridadeEscolas { get; set; }
        public TipoParametroAcademico Tipo { get; set; }
        public DataTypeParametroAcademico DataType { get; set; }
    }

    #endregion Estruturas

    #region Enumerador
    /// <summary>
    /// Tipos de dado que sera gravado no parâmetros acadêmicos.
    /// </summary>
    public enum DataTypeParametroAcademico
    {
        //parametros com tipos especificos de dados, tratados no metodo SetaBuscaComboValor
        specific
        ,
        //parametros do tipo texto(string)
        text
        ,
        //parametros do tipo numerico(int)
        integer
        ,
        //parametros do tipo logico(bool)
        logic
    }

    /// <summary>
    /// Tipos de parâmetros acadêmicos que podem ser cadastrados no sistema.
    /// </summary>
    public enum TipoParametroAcademico
    {
        /// <summary>
        /// Único: possuirá sempre um único valor, sem controle de vigência.
        /// </summary>
        Unico = 1
        ,

        /// <summary>
        /// Múltiplo: permitirá vários valores simultâneos, sem controle de vigência.
        /// </summary>
        Multiplo = 2
        ,

        /// <summary>
        /// Vigência (obrigatório): permite um valor por período de vigência e é
        /// obrigatório seu preenchimento.
        /// </summary>
        VigenciaObrigatorio = 3
        ,

        /// <summary>
        /// Vigência (opcional): possui controle de vigência, permite apenas um valor
        /// por período de vigência.
        /// </summary>
        VigenciaOpcional = 4
    }

    public enum eChaveAcademico
    {
        [parametroAcademicoAttributes("Tipo de unidade administrativa", "erro Tipo de unidade administrativa", true, true, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.specific)]
        TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA
        ,

        [parametroAcademicoAttributes("Filtrar escola por unidade administrativa superior", "Filtro de escola unidade administrativa supérior é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        FILTRAR_ESCOLA_UA_SUPERIOR
        ,

        [parametroAcademicoAttributes("Tipo de unidade administrativa para filtrar escolas", "Tipo de unidade administrativa é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA
        ,

        [parametroAcademicoAttributes("Tipo de dependência - Sala de aula", "Tipo dependência é obrigatório.", true, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_DEPENDENCIA_SALA_AULA
        ,

        [parametroAcademicoAttributes("Tipo de responsável - mãe", "Tipo aluno responsável é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_RESPONSAVEL_MAE
        ,

        [parametroAcademicoAttributes("Tipo de responsável - pai", "Tipo aluno responsável é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_RESPONSAVEL_PAI
        ,

        [parametroAcademicoAttributes("Tipo de responsável - o próprio", "Tipo aluno responsável é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_RESPONSAVEL_O_PROPRIO
        ,

        [parametroAcademicoAttributes("Tipo de responsável - outro", "Tipo aluno responsável é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_RESPONSAVEL_OUTRO
        ,

        [parametroAcademicoAttributes("Grupo padrão - colaboradores", "Grupo perfil colaborador é obrigatório.", true, false, TipoParametroAcademico.VigenciaOpcional, DataTypeParametroAcademico.specific)]
        PAR_GRUPO_PERFIL_COLAB
        ,

        [parametroAcademicoAttributes("Grupo padrão - docentes", "Grupo perfil docente é obrigatório.", false, false, TipoParametroAcademico.VigenciaOpcional, DataTypeParametroAcademico.specific)]
        PAR_GRUPO_PERFIL_DOCENTE
        ,

        [parametroAcademicoAttributes("Grupo padrão - alunos", "Grupo perfil aluno é obrigatório.", false, false, TipoParametroAcademico.VigenciaOpcional, DataTypeParametroAcademico.specific)]
        PAR_GRUPO_PERFIL_ALUNO
        ,

        [parametroAcademicoAttributes("Grupo padrão - responsáveis pelo aluno", "Grupo perfil aluno responsável é obrigatório.", false, false, TipoParametroAcademico.VigenciaOpcional, DataTypeParametroAcademico.specific)]
        PAR_GRUPO_PERFIL_ALUNO_RESPONSAVEL
        ,

        [parametroAcademicoAttributes("Preencher cidade automaticamente nos cadastros", "Filtro de preenchimento automático de cidade é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PAR_PREENCHER_CIDADE
        ,

        [parametroAcademicoAttributes("Rede de ensino padrão", "Tipo de rede de ensino é obrigatório", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        PAR_REDE_ENSINO_PADRAO
        ,

        [parametroAcademicoAttributes("Tipo de documentação NIS - Número de identificação social", "Tipo de documentação NIS é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_DOCUMENTACAO_NIS
        ,

        [parametroAcademicoAttributes("Nome da matrícula estadual", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        MATRICULA_ESTADUAL
        ,

        [parametroAcademicoAttributes("Amplitude de idade do aluno para alerta (anos)", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        AMPLITUDE_IDADE_ALERTA
        ,

        [parametroAcademicoAttributes("Nome padrão de curso no sistema", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_CADASTRO_CURSO
        ,


        [parametroAcademicoAttributes("Nome padrão de período no sistema", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_PERIODO_CURSO
        ,

        [parametroAcademicoAttributes("Permanecer na tela após gravações", "Filtro de permanecer na tela após gravações é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        BOTAO_SALVAR_PERMANECE_TELA
        ,

        [parametroAcademicoAttributes("Tipo de evento de efetivação de notas do período", "Tipo de evento de efetivação de notas do período é obrigatório.", true, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_EFETIVACAO_NOTAS
        ,

        [parametroAcademicoAttributes("Tipo da validade de solicitação de vaga", "Tipo de validade para solicitação de vaga é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        SOLICITACAO_VAGA_VALIDADE_TIPO
        ,

        [parametroAcademicoAttributes("Valor da validade de solicitação de vaga", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        SOLICITACAO_VAGA_VALIDADE_VALOR
        ,

        [parametroAcademicoAttributes("Tipo de rede de ensino particular", "Tipo de rede de ensino é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_REDE_ENSINO_PARTICULAR
        ,

        [parametroAcademicoAttributes("Tipo de rede de ensino de outros municípios", "Tipo de rede de ensino é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS
        ,

        [parametroAcademicoAttributes("Tipo de rede de ensino de outros estados/federal", "Tipo de rede de ensino é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL
        ,

        [parametroAcademicoAttributes("Trava validade de solicitação de vaga", "Trava de solicitação de vagas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        SOLICITACAO_VAGA_TRAVAR_VALIDADE
        ,

        [parametroAcademicoAttributes("Possui planejamento anual por orientações curriculares", "Possui planejamento anual por orientações curriculares é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES
        ,

        [parametroAcademicoAttributes("Curso de ensino fundamental regular", "Curso de ensino fundamental regular é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        CURSO_ENSINO_FUNDAMENTAL
        ,

        [parametroAcademicoAttributes("Tipo de evento de efetivação da recuperação", "Tipo de evento de efetivação da recuperação é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_EFETIVACAO_RECUPERACAO
        ,

        [parametroAcademicoAttributes("Tipo de disciplina eletiva do aluno", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        TIPO_DISCIPLINA_ELETIVA_ALUNO
        ,

        [parametroAcademicoAttributes("Intervalo máximo da ação retroativa (dias)", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        INTERVALO_MAXIMO_ACAO_RETROATIVA
        ,

        [parametroAcademicoAttributes("Nome do período do calendário no sistema", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_PERIODO_CALENDARIO
        ,

        [parametroAcademicoAttributes("Incluir ao menos um contato na matrícula de aluno", "Contato na matrícula de aluno é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTATO_OBRIGATORIO_MATRICULA_ALUNO
        ,

        [parametroAcademicoAttributes("Controlar ordem das disciplinas", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_ORDEM_DISCIPLINAS
        ,

        [parametroAcademicoAttributes("Organizar combos de escola por código", "Organizar escolas por código é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ORDENAR_ESCOLAS_POR_CODIGO
        ,

        [parametroAcademicoAttributes("Controlar ausência compensada na efetivação", "Controlar ausência compensada na efetivação é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_AUSENCIA_COMPENSADA_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Preencher pelo menos um documento do responsável pelo aluno no cadastro", "Preencher pelo menos um documento do responsável pelo aluno no cadastro é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        DOCUMENTO_OBRIGATORIO_RESPONSAVEL_ALUNO
        ,

        [parametroAcademicoAttributes("Habilitar as opções de exportações na impressão de documentos", "Habilitar as opções de exportações na impressão de documentos é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        HABILITA_EXPORTACAO_IMPRESSAO_DOCUMENTOS
        ,
        
        [parametroAcademicoAttributes("Tipo de evento da efetivação da nota final", "Tipo de evento da efetivação da nota final é obrigatório.", true, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_EFETIVACAO_FINAL
        ,

        [parametroAcademicoAttributes("Habilitar a impressão de relatório sem Active-x", "Habilitar a impressão de relatórios sem Active-x é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        HABILITA_IMPRESSAO_RELATORIO
        ,

        [parametroAcademicoAttributes("Programa social do cartão da família carioca", "Programa social do cartão da família carioca é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        PROGRAMA_SOCIAL_CARTAO_FAMILIA_CARIOCA
        ,

        [parametroAcademicoAttributes("Tipo de classificação das Escolas do Amanhã", "Tipo de classificação das Escolas do Amanhã é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_CLASSIFICACAO_ESCOLAS_AMANHA
        ,

        [parametroAcademicoAttributes("Controlar colaboradores somente por integração", "Controle de colaboradores apenas por integração é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_COLABORADOR_APENAS_INTEGRACAO
        ,

        [parametroAcademicoAttributes("Controlar permissões dos colaboradores e docentes pelos cargos", "Controlar permissões dos colaboradores e docentes pelos cargos é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_PERMISSAO_COLABORADOR_CARGO
        ,

        [parametroAcademicoAttributes("Campo tipo de deficiência do aluno", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        CAMPO_TIPO_DEFICIENCIA_ALUNO
        ,

        [parametroAcademicoAttributes("Termo para alunos com deficiência em turmas normais", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS
        ,

        [parametroAcademicoAttributes("Controlar carga horária no vínculo do docente", "Controlar carga horária no vínculo do docente é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_CARGA_HORARIA_DOCENTE
        ,

        [parametroAcademicoAttributes("Controlar dependências em físicas e lógicas", "Controlar dependências em físicas e lógicas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_DEPENDENCIAS_EM_FISICAS_LOGICAS
        ,

        [parametroAcademicoAttributes("Endereço eletrônico do boletim online", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        ENDERECO_ELETRONICO_BOLETIM_ONLINE
        ,

        [parametroAcademicoAttributes("Tipo de nível de ensino de educação infantil", "Tipo de nível de ensino de educação infantil é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL
        ,

        [parametroAcademicoAttributes("Permitir inclusão de novos usuários", "Permitir inclusão de novos usuários é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_INCLUSAO_NOVOS_USUARIOS
        ,

        [parametroAcademicoAttributes("Programas sociais - relatório de bônus por desempenho - Disciplinas da média do 2º ano", "", true, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.integer)]
        PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO
        ,
        [parametroAcademicoAttributes("Programas sociais - relatório de bônus por desempenho - Disciplinas da média do 3º ao 9º anos", "", false, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.integer)]
        PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO
        ,

        [parametroAcademicoAttributes("Quantidade de docentes vigentes por disciplina.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA
        ,

        [parametroAcademicoAttributes("Programa social do bolsa família", "Programa social do bolsa família é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        PROGRAMA_SOCIAL_BOLSA_FAMILIA
        ,

        [parametroAcademicoAttributes("Aprovar disciplinas eletivas das escolas automaticamente", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        APROVAR_DISCIPLINAS_ELETIVAS_AUTOMATICAMENTE
        ,

        [parametroAcademicoAttributes("Exibir observação para cadastro de turmas", "Exibir observação para cadastro de turmas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_OBSERVACAO_CADASTRO_TURMA
        ,

        [parametroAcademicoAttributes("Preencher data de movimentação automaticamente", "Preencher data de movimentação automaticamente.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PREENCHER_DATA_MOVIMENTACAO_AUTOMATICAMENTE
        ,

        [parametroAcademicoAttributes("Nível de ensino que se refere ao curso do ensino fundamental", "Nível de ensino que se refere ao curso ensino fundamental é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_NIVEL_ENSINO_FUNDAMENTAL
        ,

        [parametroAcademicoAttributes("Modalidade de ensino que se refere a modalidade regular do ensino fundamental", "Modalidade de ensino que se refere a modalidade regular do ensino fundamental é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_MODALIDADE_ENSINO_REGULAR
        ,

        [parametroAcademicoAttributes("Modalidade de ensino para jovens e adultos(Integração Censo Escolar)", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        TIPO_MODALIDADE_ENSINO_JOVENS_ADULTOS
        ,

        [parametroAcademicoAttributes("Tipo de evento da efetivação da nota de recuperação final", "Tipo de evento da efetivação da nota de recuperação final é obrigatório.", true, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL
        ,

        [parametroAcademicoAttributes("Permitir alterar o resultado final", "Permitir alterar o resultado final é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL
        ,

        [parametroAcademicoAttributes("Controlar colaboradores e vínculos integrados e virtuais", "Controlar colaboradores e vínculos integrados e virtuais é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL
        ,

        [parametroAcademicoAttributes("Validar a idade minima para ingressar em um curso do PEJA", "Validar a idade minima para ingressar em um curso do PEJA é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        VALIDAR_IDADE_MINIMA_PEJA
        ,

        [parametroAcademicoAttributes("Realizar validações no histórico escolar", "Realizar validações no histórico escolar é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        REALIZAR_VALIDACOES_HISTORICO_ESCOLAR
        ,

        [parametroAcademicoAttributes("Tipo de disciplina de enriquecimento curricular", "", true, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.integer)]
        TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR
        ,

        //novos parametros 11/02/2013
        //Parâmetros da customização para o Diário de classe
        [parametroAcademicoAttributes("Esconder botão gerar aulas", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_BOTAO_GERAR_AULAS
        ,

        [parametroAcademicoAttributes("Esconder botão atividade avaliativa", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_BOTAO_ATIVIDADE_AVALIATIVA
        ,

        [parametroAcademicoAttributes("Esconder opções de passeio extraclasse", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_PASSEIO_EXTRACLASSE
        ,

        [parametroAcademicoAttributes("Esconder atividade para casa", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_ATIVIDADE_PRACASA
        ,

        [parametroAcademicoAttributes("Nome padrão de atividade avaliativa no sistema", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_ATIVIDADE
        ,

        [parametroAcademicoAttributes("Esconder botão registrar avaliação", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_BOTAO_REGISTRAR_AVALIACAO
        ,

        [parametroAcademicoAttributes("Esconder botão de cancelar atividade", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_BOTAO_CANCELAR_ATIVIDADE
        ,

        [parametroAcademicoAttributes("Esconder navegação do menu de classes", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_NAVEGACAO_MENU_CLASSES
        ,

        [parametroAcademicoAttributes("Esconder coluna de planejamento anual", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_COLUNA_PLANEJAMENTO_ANUAL
        ,

        [parametroAcademicoAttributes("Esconder coluna imprimir planejamento", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ESCONDER_COLUNA_IMPRIMIR_PLANEJANENTO
        ,

        [parametroAcademicoAttributes("Exibir boletim escolar por escolas", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_BOLETIM_UNICO_ESCOLA
        ,

        [parametroAcademicoAttributes("Escola padrão responsável pela efetivação na transferência entre escolas", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        ESCOLA_RESPONSAVEL_EFETIVACAO_TRANSFERENCIA_ENTRE_ESCOLAS
        ,

        [parametroAcademicoAttributes("Controlar colaboradores e vínculos virtuais - matrícula", "Controlar colaboradores e vínculos virtuais - matrícula - é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL_MATRICULA_OBRIGATORIA
        ,

        [parametroAcademicoAttributes("Realizar validações no histórico escolar - importação", "Realizar validações no histórico escolar - importação - é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        REALIZAR_VALIDACOES_HISTORICO_ESCOLAR_IMPORTACAO
        ,

        [parametroAcademicoAttributes("Realizar validações no histórico escolar - exportação", "Realizar validações no histórico escolar - exportação - é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        REALIZAR_VALIDACOES_HISTORICO_ESCOLAR_EXPORTACAO
        ,

        [parametroAcademicoAttributes("Exibir o campo de possuir irmão gêmeo no cadastro de alunos", "Exibir o campo de possuir irmão gêmeo no cadastro de alunos é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_CAMPO_ALUNO_GEMEO
        ,

        [parametroAcademicoAttributes("Validar quantidade de caracteres da matrícula da certidão de nascimento", "Validar quantidade de caracteres da matrícula da certidão de nascimento é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        VALIDAR_QUANTIDADE_CARACTERES_MATRICULA_CERTIDAO
        ,

        [parametroAcademicoAttributes("Tipo de contato - Celular", "Tipo de contato celular é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_CONTATO_CELULAR
        ,

        [parametroAcademicoAttributes("Tipo de contato - Telefone recado", "Tipo de contato telefone de recado é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_CONTATO_TELEFONE_RECADO
        ,

        [parametroAcademicoAttributes("Cadastrar reuniões de responsáveis por período do calendário", "Cadastrar reuniões de responsáveis por período do calendário.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CADASTRAR_REUNIOES_POR_PERIODO_CALENDARIO
        ,

        [parametroAcademicoAttributes("Exibir notas e frequências no boletim online apenas quando o forem liberadas", "Exibir notas e frequências no boletim online apenas quando o forem liberadas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_NOTAS_FREQ_BOLETIM_ONLINE_APENAS_LIBERADO
        ,

        [parametroAcademicoAttributes("Possui planejamento anual por orientações curriculares e por aulas trabalhadas", "Possui planejamento anual por orientações curriculares e por aulas trabalhadas.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS
        ,

        [parametroAcademicoAttributes("Exibe curso/modalidade de ensino no combo de curso.", "Exibe curso/modalidade de ensino no combo de curso é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_MODALIDADE_ENSINO_CURSO
        ,

        [parametroAcademicoAttributes("Replicar planejamento anual entre turmas", "Replicar planejamento anual entre turmas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        REPLICAR_PLANEJAMENTO_ANUAL_ENTRE_TURMAS
        ,

        [parametroAcademicoAttributes("Descrição para unidades de aula", "Descrição para unidades de aula é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        TIPO_UNIDADES_AULA
        ,

        [parametroAcademicoAttributes("Permitir alterar a foto do aluno em modo de consulta.", "Permitir alterar a foto do aluno em modo de consulta.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_ALTERAR_FOTO_ALUNO_CONSULTA
        ,

        [parametroAcademicoAttributes("Tipo de disciplina que possui controle por frequência", "", true, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.integer)]
        TIPO_DISCIPLINA_CONTROLE_FREQUENCIA
        ,

        [parametroAcademicoAttributes("Não exibe mensagem de erro ao carregar disciplinas no cadastro de turmas.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        REMOVER_MSG_ERRO_CARREGAR_TURMADISCIPLINA
        ,

        [parametroAcademicoAttributes("Exibir botão para incluir aula", "Exibir botão para incluir aula é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_BOTAO_INCLUIR_AULA
        ,

        [parametroAcademicoAttributes("Termo para conteúdo da atividade avaliativa", "Exibir botão para incluir aula é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        TERMO_CONTEUDO_ATIVIDADE_AVALIATIVA
        ,

        [parametroAcademicoAttributes("Permitir editar formação de docente", "Permitir editar formação de docente é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_EDITAR_FORMACAO_DOCENTE
        ,

        [parametroAcademicoAttributes("Permitir editar ficha médica de alunos", "Permitir editar ficha médica de alunos é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_EDITAR_FICHA_MEDICA_ALUNO
        ,

        [parametroAcademicoAttributes("Serviço da educação especial que oferece atendimento especializado aos alunos com deficiência", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
        ,

        [parametroAcademicoAttributes("Verificar integridade fonética no cadastro de alunos", "Verificar integridade fonética no cadastro de alunos é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        VERIFICAR_ALUNO_INTEGRIDADE_FONETICA
        ,

        [parametroAcademicoAttributes("Mostrar as observações por disciplina no boletim online da área do aluno.", "Mostrar as observações por disciplina no boletim online da área do aluno é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MOSTRAR_OBSERVACAO_POR_DISCIPLINA_NO_BOLETIM
        ,

        [parametroAcademicoAttributes("Mostrar coluna boletim na consulta de manutenção de aluno", "Mostrar coluna boletim na consulta de manutenção de aluno é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MOSTRAR_COLUNA_BOLETIM_MANUTENCAO_ALUNO
        ,

        [parametroAcademicoAttributes("Prefixo concatenado com login importado da integração quando for aluno", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        PREFIXO_LOGIN_ALUNO_AREA_ALUNO
        ,

        [parametroAcademicoAttributes("Prefixo concatenado com login importado da integração quando for responsavel", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        PREFIXO_LOGIN_RESPONSAVEL_AREA_ALUNO
        ,

        [parametroAcademicoAttributes("Permitir alterar o código e o nome abreviado da disciplina", "Permitir alterar o código e o nome abreviado da disciplina é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ALTERAR_CODIGO_NOME_DISCIPLINA
        ,

        [parametroAcademicoAttributes("Permitir nome para todas as atividades avaliativas", "Permitir nome para todas as atividades avaliativas é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        NOME_TODAS_ATIVIDADES_AVALIATIVAS
        ,

        [parametroAcademicoAttributes("Exibe itens de regência no cadastro de curso.", "Exibe itens de regência no cadastro de curso.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ITENS_REGENCIA_CADASTRO_CURSO
        ,

        [parametroAcademicoAttributes("Exibe coluna laçamento de nota final no registro de avaliações.", "Exibe coluna laçamento de nota final no registro de avaliações é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES
        ,

        [parametroAcademicoAttributes("Posição de docente atribuido como Projeto", "Posição do docente", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        POSICAO_DOCENTE_PROJETO
        ,
        [parametroAcademicoAttributes("Posição de docencia compartilhada", "", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        POSICAO_DOCENCIA_COMPARTILHADA
        ,

        [parametroAcademicoAttributes("Posição de docente atribuido como Substituto", "", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        POSICAO_DOCENTE_SUBSTITUTO
        ,

        [parametroAcademicoAttributes("Exibir o campo [MSG_SinteseDaAula] no cadastro do planejamento de classes.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_SINTESE_REGENCIA_AULA_TURMA
        ,

        [parametroAcademicoAttributes("Permitir exibir cor em notas abaixo da média no fechamento do bimestre.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_COR_MEDIA_FINAL_FECHAMENTO_BIMESTRE
        ,

        [parametroAcademicoAttributes("Permite a criação de atividades avaliativas exclusivas.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS
        ,

        [parametroAcademicoAttributes("Habilita compensação de ausência pelo cadastro de ausência.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA
        ,

        [parametroAcademicoAttributes("Permitir cadastrar aulas de reposição.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CADASTRAR_AULA_REPOSICAO
        ,

        [parametroAcademicoAttributes("Exibir o sábado ao gerar aulas.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_SABADO_GERAR_AULAS
        ,

        [parametroAcademicoAttributes("Permitir incluir mais de uma aula por dia.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_INCLUIR_VARIAS_AULAS_POR_DIA
        ,

        [parametroAcademicoAttributes("Exibir todos os alunos selecionados na tela para emissão dos boletins escolares.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_BOLETINS_ALUNOS_NA_TELA
        ,

        [parametroAcademicoAttributes("ID do tipo de ciclo ALFABETIZAÇÃO, para poder alterar o css do Boletim Online.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        TIPO_CICLO_ALFABETIZACAO
        ,

        [parametroAcademicoAttributes("ID do tipo de ciclo INTERDISCIPLINAR, para poder alterar o css do Boletim Online.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        TIPO_CICLO_INTERDISCIPLINAR
        ,

        [parametroAcademicoAttributes("ID do tipo de ciclo AUTORAL, para poder alterar o css do Boletim Online.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        TIPO_CICLO_AUTORAL
        ,

        [parametroAcademicoAttributes("Desabilitar o lançamento de notas na efetivação.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        DESABILITAR_LANCAMENTO_NOTA_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Habilita a aprovação manual na efetivação.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        HABILITAR_APROVACAO_MANUAL_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Na exclusão de aulas, excluir atividade cadastrada na mesma data da aula.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXCLUIR_ATIVIDADE_MESMA_DATA_AULA
        ,

        [parametroAcademicoAttributes("Mostrar relatórios de diário de classe (em branco) na tela minhas turmas", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MOSTRAR_RELATORIOS_DIARIO_DE_CLASSE
        ,

        [parametroAcademicoAttributes("Arredondar nota da avaliação do aluno.", "Arredondar nota da avaliação do aluno é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ARREDONDAMENTO_NOTA_AVALIACAO
        ,

        [parametroAcademicoAttributes("Ordenação padrão nos combos de ordenação de alunos.", "Ordenação padrão nos combos de ordenação de alunos é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        ORDENACAO_COMBO_ALUNO
        ,

        [parametroAcademicoAttributes("Destacar campo nota da avaliação quando estiver acima do valor permitido.", "Destacar campo nota da avaliação quando estiver acima do valor permitido é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        DESTACAR_CAMPO_NOTA_AVALIACAO_ACIMA_PERMITIDO
        ,

        [parametroAcademicoAttributes("Alterar a posição das aulas dos docentes para titular ao desvinculá-lo da disciplina.", "Alterar a posição das aulas dos docentes para titular ao desvinculá-lo da disciplina é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        ALTERAR_AULAS_PARA_TITULAR_ATRIBUICAO_DOCENTES
        ,

        [parametroAcademicoAttributes("Exibir legenda de aluno dispensado", "Exibir legenda de aluno dispensado é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_LEGENDA_ALUNO_DISPENSADO
        ,

        [parametroAcademicoAttributes("Permitir a exibição do filtro de avaliação no relatório de planejamento anual.", "Permitir a exibição do filtro de avaliação no relatório de planejamento anual.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_FILTRO_AVALIACAO_RELATORIO_PLANEJAMENTO_ANUAL
        ,

        [parametroAcademicoAttributes("Permitir a exibição do filtro de aulas no relatório de planejamento anual.", "Permitir a exibição do filtro de aulas no relatório de planejamento anual.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_FILTRO_AULAS_RELATORIO_PLANEJAMENTO_ANUAL
        ,

        [parametroAcademicoAttributes("Permitir importação de dados para efetivação.", "Permitir importação de dados para efetivação é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_IMPORTACAO_DADOS_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Tipo de evento de liberação do boletim online.", "Tipo de evento de liberação do boletim online é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_LIBERACAO_BOLETIM_ONLINE
        ,

        [parametroAcademicoAttributes("Expressão regular para o formato da matricula estadual do aluno.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        FORMATO_MATRICULA_ESTADUAL
        ,

        [parametroAcademicoAttributes("Permitir o cadastro de evento retroativo no calendário escolar.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CADASTRO_EVENTO_RETROATIVO
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de aula carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_AULA
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de planejamento carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_PLANEJAMENTO
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de logs carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_LOGS
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de justificativa de falta carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_JUSTIFICATIVA
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de foto carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_FOTO
        ,

        [parametroAcademicoAttributes("Quantidade máxima de protocolos de compensação de ausência carregados na sincronização com o diário de classe.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_COMPENSACAO
        ,

        [parametroAcademicoAttributes("Grupos de acesso dos responsáveis no sistema Boletim Online.", "Grupo responsável é obrigatório.", true, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.specific)]
        GRUPO_RESPONSAVEL_BOLETIMONLINE
        ,

        [parametroAcademicoAttributes("Exibe o campo de nis na ficha de inscrição.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_NIS_FICHA_INSCRICAO
        ,

        [parametroAcademicoAttributes("Carregar no fechamento de bimestre apenas a disciplina selecionada na navegação", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EFETIVACAO_CARREGAR_APENAS_DISCIPLINA_SELECIONADA
        ,

        [parametroAcademicoAttributes("Exibe o campo 'Qtde. dia de aula' no fechamento bimestral", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_CAMPO_QTDE_DIAS_AULA
        ,

        [parametroAcademicoAttributes("Exibe o campo 'Turno' no fechamento bimestral", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_CAMPO_TURNO_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Exibe a coluna de anotações na busca de alunos.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ANOTACOES_BUSCA_ALUNO
        ,

        [parametroAcademicoAttributes("Tipo de responsável - Não Existe", "Tipo aluno responsável é obrigatório.", true, true, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_RESPONSAVEL_NAO_EXISTE
        ,

        [parametroAcademicoAttributes("Permite transformar o registro de colaborador em docente.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CRIACAO_DOCENTE_POR_COLABORADOR
        ,

        [parametroAcademicoAttributes("Permite transformar o registro de aluno em colaborador.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CRIACAO_COLABORADOR_POR_ALUNO
        ,

        [parametroAcademicoAttributes("Grupo de usuarios que são os administradores da UE.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        GRUPO_USUARIO_ADMINISTRADOR_UE
        ,

        [parametroAcademicoAttributes("Exibir na inclusão o campo 'Com atividade discente' selecionado.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_SELECIONADO_COM_ATIVIDADE_DISCENTE
        ,

        [parametroAcademicoAttributes("Campo certidão de nascimento obrigatório.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CAMPO_CERTIDAO_NASCIMENTO_OBRIGATORIO
        ,

        [parametroAcademicoAttributes("Não mostrar posição dos docentes no planejamento", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        NAO_MOSTRAR_POSICAO_DOCENTES_PLANEJAMENTO
        ,

        [parametroAcademicoAttributes("Data de início do controle da previsão de aulas na tela de minhas turmas", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        DATA_VALIDADE_BLOQUEIO_ACESSO_MINHAS_TURMAS
        ,

        [parametroAcademicoAttributes("Mensagem de alerta tela de minhas turmas", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        MENSAGEM_ALERTA_DOCENTE_MINHAS_TURMAS
        ,

        [parametroAcademicoAttributes("Exibir a sessão de recuperação no boletim.", "Exibir a sessão de recuperação no boletim.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MOSTRA_RECUPERACAO_BOLETIM
        ,

        [parametroAcademicoAttributes("Exibir a lista de sistemas na area do aluno", "Exibir a lista de sistemas na area do aluno é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_LISTA_SISTEMAS_AREA_ALUNO
        ,

        [parametroAcademicoAttributes("Exibir o status da efetivação (concluído ou pendente), na tela do fechamento.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_STATUS_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Exibir os campos de cadastro de nome de arquivo de fundo e verso de carteirinha.", "Exibir os campos de cadastro de nome de arquivo de fundo e verso de carteirinha é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_FUNDO_CARTEIRINHA
        ,

        [parametroAcademicoAttributes("Exibir o editor html na tela de cadastro de observações do histórico escolar.", "Exibir o editor html na tela de cadastro de observações do histórico escolar.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_EDITOR_HTML_CADASTRO_OBSERVACOES
        ,

        [parametroAcademicoAttributes("Relatório de anotações gerais na busca de alunos.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        RELATORIO_ANOTACAO_GERAL_BUSCA_ALUNO
        ,

        [parametroAcademicoAttributes("Gerar histórico escolar ao salvar o parecer conclusivo.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        GERAR_HISTORICO_SALVAR_PARECER_CONCLUSIVO
        ,

        [parametroAcademicoAttributes("Tipo de evento de atividade diversificada.", "Tipo de evento de atividade diversificada é obrigatório.", true, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA
        ,

        [parametroAcademicoAttributes("Permitir a inclusão, exclusão e alteração de eventos retroativos sem atividades discentes para o bimestre corrente.", "Tipo de evento de atividade diversificada é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE
        ,

        [parametroAcademicoAttributes("Permitir o cadastro de anotações gerais do aluno.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_CADASTRO_ANOTACOES_GERAIS_ALUNO
        ,

        [parametroAcademicoAttributes("Modalidade de ensino que se refere a modalidade EJA.", "Modalidade de ensino que se refere a modalidade EJA é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_MODALIDADE_EJA
        ,

        [parametroAcademicoAttributes("Programas sociais que irão processar frequências de alunos.", "Programa social é obrigatório.", false, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.specific)]
        PROG_SOCIAL_PROCESSAR_FREQUENCIAS
        ,

        [parametroAcademicoAttributes("Permite relacionar habilidades da matriz padrão com uma atividade avaliativa(Minhas Turmas).", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        RELACIONAR_HABILIDADES_AVALIACAO
        ,

        [parametroAcademicoAttributes("Possui planejamento anual por ciclo.", "Possui planejamento anual por ciclo.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PLANEJAMENTO_ANUAL_CICLO
        ,

        [parametroAcademicoAttributes("Exibir mensagem ao sair da tela.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_MENSAGEM_SAIR_TELA
        ,

        [parametroAcademicoAttributes("Exibir mensagem padrão do navegador ao sair da tela.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_MENSAGEM_NAVEGADOR_PADRAO_SAIR_TELA
        ,

        [parametroAcademicoAttributes("Exibir mensagem padrão do navegador ao sair da tela junto com a mensagem do JQuery.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_MENSAGEM_NAVEGADOR_COM_MENSAGEM_JQUERY
        ,

        [parametroAcademicoAttributes("Permitir adicionar mais de um aluno nas anotações da aula.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_ANOTACOES_MAIS_DE_UM_ALUNO
        ,

        // Parâmetro que guarda o ano letivo para abrir campos de lançamento do fechamento.
        [parametroAcademicoAttributes("Ano letivo para abrir campos de lançamento do fechamento", "", false, false, TipoParametroAcademico.Multiplo, DataTypeParametroAcademico.integer)]
        ANO_LETIVO_ABRIR_FECHAMENTO
        ,

        [parametroAcademicoAttributes("Pré carrega os dados do fechamento bimestral/final no cache.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PRE_CARREGAR_CACHE_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Exibir mensagem de aviso de alunos sem lançamento de pós-conselho no fechamento.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_MENSAGEM_SEM_LANCAMENTO_POS_CONSELHO
        ,

        [parametroAcademicoAttributes("Permite exibir habilidades por plano de aula.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_HABILIDADES_PLANO_AULA
        ,

        [parametroAcademicoAttributes("Quantidade de dias de aulas que serão exibidos no listão de frequência.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_AULAS_LISTAO_FREQUENCIA
        ,
        [parametroAcademicoAttributes("Define se o lançamento de frequência e avaliação ficarão em uma única tela (Listão).", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        TELA_UNICA_LANCAMENTO_FREQUENCIA_AVALIACAO
        ,

        [parametroAcademicoAttributes("Permite cadastrar turmas para o próximo anos em ter feito o fechamento do ano atual.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITE_TURMAS_PROXIMO_ANO
        ,

        [parametroAcademicoAttributes("Bloquear ficha do aluno para usuário com permissão de consulta.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        BLOQUEAR_FICHA_ALUNO_USUARIOS_PERMISSAO_CONSULTA
        ,

        [parametroAcademicoAttributes("Esconder botão alunos na tela minhas turmas.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MINHAS_TURMAS_ESCONDER_BOTAO_ALUNO
        ,

        [parametroAcademicoAttributes("Esconder botão voltar na tela minhas turmas.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MINHAS_TURMAS_ESCONDER_BOTAO_VOLTAR
        ,

        [parametroAcademicoAttributes("Exibir a vigência de início e fim na atribuição de docente.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_VIGENCIA_ATRIBUICAO_DOCENTE
        ,

        [parametroAcademicoAttributes("Esconder botão efetivação na tela minhas turmas.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        MINHAS_TURMAS_ESCONDER_BOTAO_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Ao acessar a tela do Fechamento, verificar se existe registro na fila que ainda não tenha sido processado e fazer a atualização.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PROCESSAR_FILA_FECHAMENTO_TELA
        ,

        [parametroAcademicoAttributes("Tempo de espera para o processamento de um registro na fila, ao acessar a tela do Fechamento.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        PROCESSAR_FILA_FECHAMENTO_TELA_TEMPO
        ,

        [parametroAcademicoAttributes("Bloquear o cadastro de eventos de efetivação que terminam antes do fim do período do calendário.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EVENTOEFETIVACAO_BLOQUEAR_CADASTRO_ANTES_FIM_PERIODO
        ,

        [parametroAcademicoAttributes("Número de tentativas para verificar se o registro ainda está na fila aguardando processamento, na tela do Fechamento.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        PROCESSAR_FILA_FECHAMENTO_TELA_TENTATIVAS
        ,

        [parametroAcademicoAttributes("Permitir que o docente visualize apenas as anotações dos alunos criadas por ele.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_APENAS_ANOTACOES_ALUNO_DOCENTE
        ,

        [parametroAcademicoAttributes("Bloquear usuário do(a) aluno(a) ao inativá-lo(a).", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        BLOQUEAR_USUARIO_APOS_INATIVAR_ALUNO
        ,

        [parametroAcademicoAttributes("As novas turmas serão criadas com o status \"Aguardando\".", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        NOVAS_TURMAS_AGUARDANDO
        ,

        [parametroAcademicoAttributes("Define se irá exibir a idade do aluno em anos, meses e dias na efetivação de notas.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBE_IDADE_EFETIVACAO
        ,

        [parametroAcademicoAttributes("Habilitar controle de peso nas avaliações periódicas do formato de avaliação.", "É obrigatório selecionar uma opção.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        HABILITAR_PESO_AVALIACOESPERIODICAS
        ,

        [parametroAcademicoAttributes("Nome do Web Service que retorna a imagem da carteira de vacinação do aluno", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_WS_RETORNA_IMAGEM_CARTEIRA_VACINACAO
        ,

        [parametroAcademicoAttributes("Define se não será permitido incluir alunos se a capacidade da turma for excedida.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        BLOQUEAR_MATRICULA_EXCEDE_CAPACIDADE_TURMA
        ,
        
        [parametroAcademicoAttributes("Permitir o controle de liberação do planejamento anual para edição.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        CONTROLAR_LIBERACAO_PLANEJAMENTO_ANUAL
        ,

        [parametroAcademicoAttributes("Define se as recomendações para o aluno e para os responsáveis serão exibidas de acordo com o grupo de usuário.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_RECOMENDACOES_POR_GRUPO_USUARIO
        ,

        [parametroAcademicoAttributes("Tipo de evento de liberação da edição dos dados cadastrais na área do aluno.", "Tipo de evento de liberação da edição dos dados cadastrais na área do aluno é obrigatório.", false, false, TipoParametroAcademico.VigenciaObrigatorio, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_EDICAO_DADOS_AREA_ALUNO
        ,

        [parametroAcademicoAttributes("Permitir a impressão de boletim online.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_IMPRESSAO_BOLETIM_ONLINE
        ,

        [parametroAcademicoAttributes("Tipo de evento de planejamento anual.", "Tipo de evento de planejamento anual é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_EVENTO_PLANEJAMENTO_ANUAL
        ,

        [parametroAcademicoAttributes("Nome padrão de tipo de currículo período no sistema.", "Tipo de evento de planejamento anual é obrigatório.", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        NOME_TIPO_CURRICULO_PERIODO
        ,

        [parametroAcademicoAttributes("Permitir a seleção do(s) aluno(s) para realizar o fechamento da matrícula.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PERMITIR_SELECAO_ALUNOS_FECHAMENTO_MATRICULA
        ,

        [parametroAcademicoAttributes("Define se haverá permissão por tipo de evento.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_PERMISSAO_TIPO_EVENTO
        ,

        [parametroAcademicoAttributes("Gerar histórico escolar automaticamente para todas as disciplinas do aluno.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        GERAR_HISTORICO_ESCOLAR_TODAS_DISCIPLINAS
        ,

        [parametroAcademicoAttributes("Salvar o total da carga horária (base comum e diversificada) na geração automática de histórico escolar.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        SALVAR_CARGA_HORARIA_GERAR_HISTORICO_ESCOLAR
        ,

        [parametroAcademicoAttributes("Quantidade máxima de pendências carregadas no processamento do fechamento de bimestre.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_MAXIMA_BUSCA_FILA_FECHAMENTO
        ,

        [parametroAcademicoAttributes("Serviço que realizará o processamento da fila e atualização de dados na tela de fechamento.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        PROCESSAR_FILA_FECHAMENTO_TELA_SERVICO
        ,

        [parametroAcademicoAttributes("Nome padrão da designação da escola", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        DESIGNACAO
        ,

        [parametroAcademicoAttributes("Determina se o número de matrícula será exibido.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBE_NUMERO_MATRICULA
        ,

        [parametroAcademicoAttributes("Define se a opção para limitar docentes será exibida no cadastro de evento.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_LIMITAR_DOCENTES_EVENTO
        ,

        [parametroAcademicoAttributes("Exibir número do arquivo passivo.", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_NUMERO_ARQUIVO_PASSIVO
        ,

        [parametroAcademicoAttributes("Executar pré-processamento dos relatórios de pendências.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXECUTAR_PREPROCESSAMENTO_RELATORIO_PENDENCIAS
        ,

        [parametroAcademicoAttributes("Quantidade de threads que serão executadas a cada execução do serviço de processamento do fechamento de bimestre.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.integer)]
        QUANTIDADE_THREADS_FILA_FECHAMENTO
        ,

        [parametroAcademicoAttributes("Exibir mensagem de 'Aguarde' ao acessar o fechamento(automatico) com FuraFila desativado e processamento pendente.", "", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PROCESSAR_FILA_FECHAMENTO_VERIFICAR_PENDENCIAS
        ,

        [parametroAcademicoAttributes("Área de conhecimento para disciplinas de fora da rede.", "Área de conhecimento para disciplinas de fora da rede é obrigatória.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO
        ,

        [parametroAcademicoAttributes("Tipo de período do calendário de recesso", "", false, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_PERIODO_CALENDARIO_RECESSO
        ,

        [parametroAcademicoAttributes("Preencher login e senha automaticamente no cadastro de colaboradores e docentes.", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        PREENCHER_LOGIN_SENHA_AUTOMATICAMENTE_COLABORADORES_DOCENTES
        ,

        [parametroAcademicoAttributes("Exibir alerta de aula criada sem plano, para o ensino infantil na visão gestão", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL
        ,

        [parametroAcademicoAttributes("Exibir no planejamento a aba plano do ciclo/série para o ensino infantil", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ABA_PLANEJAMENTO_PLANO_CICLO_ENSINO_INFANTIL
        ,

        [parametroAcademicoAttributes("Exibir no planejamento a aba plano anual para o ensino infantil", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ABA_PLANEJAMENTO_PLANO_ANUAL_ENSINO_INFANTIL
        ,

        [parametroAcademicoAttributes("Exibir no planejamento a aba plano para o aluno para o ensino infantil", "É obrigatório selecionar uma opção.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.logic)]
        EXIBIR_ABA_PLANEJAMENTO_PLANO_ALUNO_ENSINO_INFANTIL
        ,

        [parametroAcademicoAttributes("Mensagem de aviso para alunos com frequência externa", "É obrigatório informar uma mensagem.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.text)]
        MENSAGEM_FREQUENCIA_EXTERNA
        ,

        [parametroAcademicoAttributes("Nível de ensino que se refere ao curso do ensino médio", "Nível de ensino que se refere ao curso do ensino médio é obrigatório.", true, false, TipoParametroAcademico.Unico, DataTypeParametroAcademico.specific)]
        TIPO_NIVEL_ENSINO_MEDIO
    }   

    #endregion Enumerador

    public class ACA_ParametroAcademicoBO : BusinessBase<ACA_ParametroAcademicoDAO, ACA_ParametroAcademico>
    {
        #region Propriedades

        private static Dictionary<Guid, Dictionary<string, string[]>> listaParametros;

        /// <summary>
        /// Retorna os parâmetros do sistema.
        /// </summary>
        private static Dictionary<Guid, Dictionary<string, string[]>> ListaParametros
        {
            get
            {
                if ((listaParametros == null) || (listaParametros.Count == 0))
                {
                    // O objeto não pode estar nulo quando lock.
                    listaParametros = new Dictionary<Guid, Dictionary<string, string[]>>();
                    lock (listaParametros)
                    {
                        SelecionaListaParametrosVigente(out listaParametros);
                    }
                }
                return listaParametros;
            }
        }

        #endregion Propriedades

        #region Estruturas

        /// <summary>
        /// Estrutura para ser usada no cadastro, guarda o tipo de parâmetro para
        /// ser tratado na tela (cada tipo possui uma regra para cadastrar).
        /// </summary>
        public struct tmpParametros
        {
            public ACA_ParametroAcademico parametro;
            public TipoParametroAcademico tipo;
            public bool integridadeEscolas;
        }

        #endregion Estruturas

        #region Métodos

        #region Parâmetro

        /// <summary>
        /// Retorna o valor do parâmetro convertido em Boolean.
        /// Caso o parâmetro esteja vazio, retorna "false".
        /// </summary>
        /// <param name="pac_chave">Chave para buscar o parâmetro</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static bool ParametroValorBooleanoPorEntidade(eChaveAcademico pac_chave, Guid ent_id)
        {
            string valor = ParametroValorPorEntidade(pac_chave, ent_id);

            bool ret;

            if (!Boolean.TryParse(valor, out ret))
                return false;

            return ret;
        }

        /// <summary>
        /// Retorna o valor do parâmetro convertido em Int32.
        /// Caso o parâmetro esteja vazio, retorna "0".
        /// </summary>
        /// <param name="pac_chave">Chave para buscar o parâmetro</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static int ParametroValorInt32PorEntidade(eChaveAcademico pac_chave, Guid ent_id)
        {
            string valor = ParametroValorPorEntidade(pac_chave, ent_id);

            Int32 ret;

            if (!Int32.TryParse(valor, out ret))
                return 0;

            return ret;
        }

        /// <summary>
        /// Retorna o valor do parâmetro convertido em Guid.
        /// Caso o parâmetro esteja vazio, retorna "Guid.Empty".
        /// </summary>
        /// <param name="pac_chave">Chave para buscar o parâmetro</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static Guid ParametroValorGuidPorEntidade(eChaveAcademico pac_chave, Guid ent_id)
        {
            string valor = ParametroValorPorEntidade(pac_chave, ent_id);

            if (string.IsNullOrEmpty(valor))
                return Guid.Empty;

            return new Guid(valor);
        }

        /// <summary>
        /// Seleciona o valor de um parametro filtrados por pac_chave.
        /// </summary>
        /// <param name="pac_chave">Enum que representa a chave a ser pesquisada</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>pac_valor</returns>
        public static string ParametroValorPorEntidade(eChaveAcademico pac_chave, Guid ent_id)
        {
            string valor = string.Empty;

            if (ent_id.Equals(Guid.Empty))
                valor = (from lista in ListaParametros
                         where lista.Value.Keys.Contains(Enum.GetName(typeof(eChaveAcademico), pac_chave))
                         let ret = lista.Key
                         select ListaParametros[ret][Enum.GetName(typeof(eChaveAcademico), pac_chave)].FirstOrDefault()).FirstOrDefault();
            else if (ListaParametros[ent_id].ContainsKey(Enum.GetName(typeof(eChaveAcademico), pac_chave)))
                valor = ListaParametros[ent_id][Enum.GetName(typeof(eChaveAcademico), pac_chave)].FirstOrDefault();

            return valor;
        }

        /// <summary>
        /// Recarrega os parâmetros do sistema.
        /// </summary>
        public static void RecarregaListaParametrosVigente()
        {
            // O objeto não pode estar nulo quando lock.
            listaParametros = new Dictionary<Guid, Dictionary<string, string[]>>();
            lock (listaParametros)
            {
                SelecionaListaParametrosVigente(out listaParametros);
            }
        }

        /// <summary>
        /// Retorna um parametro pela chave.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="pac_chave">Campo pac_chave da tabela ACA_ParametroAcademico do bd</param>
        /// <returns>Entidade ACA_ParametroAcademico</returns>
        public static ACA_ParametroAcademico GetEntityByChave(Guid ent_id, eChaveAcademico pac_chave)
        {
            ACA_ParametroAcademicoDAO dal = new ACA_ParametroAcademicoDAO();
            return dal.LoadBy_pac_chave(ent_id, Enum.GetName(typeof(eChaveAcademico), pac_chave));
        }

        /// <summary>
        /// Retorna um Objeto de tmpParametros preenchido.
        /// </summary>
        /// <param name="tipoparametroAcademico">Campo pac_chave da tabela ACA_ParametroAcademico do bd</param>
        /// <returns>Entidade tmpParametros</returns>
        public static tmpParametros CriaTmpParametros(eChaveAcademico pac_chave)
        {

            parametroAttributes atrr = parametroAcademicoAttributes.Get(pac_chave);

            return new tmpParametros
                {
                    parametro = new ACA_ParametroAcademico
                    {
                    pac_chave = pac_chave.ToString()
                        ,
                    pac_obrigatorio = atrr.Obrigatorio
                        ,
                    pac_descricao = atrr.Description
                    }
                    ,
                tipo = atrr.Tipo
                    ,
                integridadeEscolas = atrr.IntegridadeEscolas   
            };           
                    }

        /// <summary>
        /// Cria lista de parâmetros padrão
        /// </summary>
        /// <returns></returns>
        public static List<tmpParametros> CriaListaParametroPadrao()
                    {
            List<tmpParametros> lista = new List<tmpParametros>();

            foreach (eChaveAcademico echaveacademico in Enum.GetValues(typeof(eChaveAcademico)))
                {
                lista.Add(CriaTmpParametros(echaveacademico));
                    }
            return lista;
        }

        #endregion Parâmetro

        #region Consultar

        /// <summary>
        /// Retorna um datatable contendo todos os parametros cadastrados no BD
        /// não excluidos logicamente.
        /// </summary>
        /// <returns>Datatable com paramtros</returns>
        public static DataTable GetSelect(Guid ent_id, bool paginado, int currentPage, int pageSize)
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_ParametroAcademicoDAO dal = new ACA_ParametroAcademicoDAO();

            DataTable dt = dal.Select(ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["pac_VigenciaInicio"].ToString()) && Convert.ToDateTime(dt.Rows[i]["pac_VigenciaInicio"]) >= DateTime.Now)
                {
                    dt.Rows[i]["pac_vigencia"] = "";
                    dt.Rows[i]["pac_valor_nome"] = "";
                }
            }
            return dt;
        }

        /// <summary>
        /// Busca os tipos de Uas cadastrados nos parâmetros para o parâmetro
        /// TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoUAEscola(Guid ent_id)
        {
            ACA_ParametroAcademicoDAO dao = new ACA_ParametroAcademicoDAO();
            return dao.Select_TipoUABy_Chave(ent_id, "TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA");
        }

        /// <summary>
        /// Retorna um Datatable com todos os valores cadastrados para um parametro nas quais não foram excluidas logicamente.
        /// filtrados por pac_chave
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo pac_chave da tabela ACA_ParametroAcademico do bd</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns>Datatable com paramtros</returns>
        public static DataTable SelecionaParametroValores(Guid ent_id, string pac_chave, bool paginado, int currentPage, int pageSize)
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_ParametroAcademicoDAO dal = new ACA_ParametroAcademicoDAO();
            return dal.SelectBy_pac_chave2(ent_id, pac_chave, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um Datatable vazio.
        /// </summary>
        /// <returns>Datatable vazio</returns>
        public static DataTable SelectVazio
        (
            int currentPage
            , int pageSize
        )
        {
            try
            {
                DataTable dt = new DataTable();
                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna os parâmetros ativos e vigentes.
        /// </summary>
        private static void SelecionaListaParametrosVigente(out Dictionary<Guid, Dictionary<string, string[]>> dictionary)
        {
            ACA_ParametroAcademicoDAO dao = new ACA_ParametroAcademicoDAO();
            List<ACA_ParametroAcademico> lt = dao.SelectBy_ParametrosVigente();

            dictionary = (from ACA_ParametroAcademico pac in lt
                          group pac by pac.ent_id into t
                          select new
                          {
                              ent_id = t.Key
                              ,
                              dicionario = (from pac2 in t
                                            group pac2 by pac2.pac_chave into t2
                                            select new
                                            {
                                                chave = t2.Key
                                                ,
                                                valor = t2.Select(p => p.pac_valor).ToArray()
                                            }
                                            ).ToDictionary(p => p.chave, p => p.valor)
                          }
                          ).ToDictionary(p => p.ent_id, p => p.dicionario);

            return;
        }

        #endregion Consultar

        #region Validação

        /// <summary>
        /// Retorna se o parâmetro FILTRAR_ESCOLA_UA_SUPERIOR está setado como "Sim" na tabela
        /// de parâmetros.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>True - Utiliza filtro escola | False - Não utiliza filtro escola</returns>
        public static bool VerificaFiltroUniAdmSuperiorPorEntidade(Guid ent_id)
        {
            return ParametroValorBooleanoPorEntidade(eChaveAcademico.FILTRAR_ESCOLA_UA_SUPERIOR, ent_id);
        }

        /// <summary>
        ///  Retorna valor do parâmetro TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA cadastrado.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>Valor do parâmetro</returns>
        public static Guid VerificaFiltroEscolaPorEntidade(Guid ent_id)
        {
            string valor = ParametroValorPorEntidade(eChaveAcademico.TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA, ent_id);
            return !string.IsNullOrEmpty(valor) ? new Guid(valor) : Guid.Empty;
        }

        /// <summary>
        /// Retorna valor do parâmetro cadastrado.
        /// </summary>
        /// <param name="pac_chave">Chave do parâmetro</param>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>Valor do parâmetro</returns>
        [Obsolete("Utilizar o novo método (VerificaFiltroEscola) sem parâmetros.")]
        public static Guid VerificaFiltroEscola(string pac_chave, bool paginado, int currentPage, int pageSize, Guid ent_id)
        {
            string valor = ParametroValorPorEntidade((eChaveAcademico)Enum.Parse(typeof(eChaveAcademico), pac_chave), ent_id);
            return !string.IsNullOrEmpty(valor) ? new Guid(valor) : Guid.Empty;
        }

        /// <summary>
        /// Valida dados relacionados ao valor do parâmetro, para saber se será possível salvar.
        /// Parâmetro TIPO_DISCIPLINA_ELETIVA_ALUNO: não pode salvar caso a disciplina já cadastrada ou a que será
        ///     inserida já possua registros ligados a ela.
        /// </summary>
        /// <param name="entity">Parâmetro acadêmico que será salvo</param>
        private static void ValidaDadosRelacionados(ACA_ParametroAcademico entity)
        {
            if ((eChaveAcademico)Enum.Parse(typeof(eChaveAcademico), entity.pac_chave) == eChaveAcademico.TIPO_DISCIPLINA_ELETIVA_ALUNO)
            {
                // Verifica se o parâmetro tipo de disciplina eletiva do aluno atual pode ser alterado
                int tp_disciplinaAtual = ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_DISCIPLINA_ELETIVA_ALUNO, entity.ent_id);
                int tds_idAtual = tp_disciplinaAtual;

                if (tds_idAtual > 0)
                {
                    if (ACA_DisciplinaBO.VerificaTipoDisciplina(tds_idAtual).Rows.Count > 0)
                    {
                        throw new ValidationException
                            ("Não é possível alterar o parâmetro, pois possui outros registros ligados a ele.");
                    }
                }

                // Verifica se o novo parâmetro tipo de disciplina eletiva do aluno pode ser utilizado
                string tp_disciplina = entity.pac_valor;
                int tds_id = string.IsNullOrEmpty(tp_disciplina) ? -1 : Convert.ToInt32(tp_disciplina);

                if (tds_id == -1)
                    throw new ValidationException("Tipo de " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " é obrigatório.");

                if (ACA_DisciplinaBO.RetornaDisciplinasPorTipo(tds_id).Rows.Count > 0)
                {
                    throw new ValidationException
                        ("Não é possível utilizar o(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " , pois possui outros registros ligados a ele(a).");
                }
            }
        }

        /// <summary>
        /// Valida as vigências para parâmetros do tipo Unico ou Multiplo.
        /// Independe de outros registros para inserir.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool ValidaUnicoMultiplo(ACA_ParametroAcademico entity, out string msg)
        {
            bool isValid = true;
            msg = "";

            if (entity.pac_vigenciaInicio.Date < DateTime.Now.Date)
            {
                ACA_ParametroAcademico entAux = new ACA_ParametroAcademico { pac_id = entity.pac_id };
                GetEntity(entAux);

                if ((entity.IsNew) || (entAux.pac_vigenciaInicio != entity.pac_vigenciaInicio))
                {
                    isValid = false;
                    msg = "Vigência inicial não pode ser anterior à data atual.";
                }
            }
            else if ((entity.pac_vigenciaFim != new DateTime()) &&
                     (entity.pac_vigenciaFim.Date < entity.pac_vigenciaInicio))
            {
                isValid = false;
                msg = "Vigência final não pode ser anterior à vigência inicial.";
            }
            else
            {
                if ((entity.IsNew) && (ExisteParametro(entity)))
                {
                    isValid = false;
                    msg = "Já existe um parâmetro cadastrado com o mesmo valor.";
                }
            }
            return isValid;
        }

        /// <summary>
        /// Retorna um Booleano na qual faz verificação de existencia de vigencia conflitante com relação as datas de vigencia
        /// na entidade do parametro.
        /// </summary>
        /// <param name="entity">Entidade do Parametro</param>
        /// <returns>True - caso exista uma vigencia em conflito;</returns>
        public static bool VerificaVigencia(ACA_ParametroAcademico entity)
        {
            ACA_ParametroAcademicoDAO dal = new ACA_ParametroAcademicoDAO();
            try
            {
                return dal.SelectBy_Vigencia(entity.ent_id, entity.pac_chave, entity.pac_vigenciaInicio, entity.pac_vigenciaFim, entity.pac_obrigatorio);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna se foi encontrado um parâmetro cadastrado com o mesmo valor, na mesma
        /// chave do parâmetro passado.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool ExisteParametro(ACA_ParametroAcademico entity)
        {
            ACA_ParametroAcademicoDAO dao = new ACA_ParametroAcademicoDAO();

            // Verificar se existe um parâmetro cadastrado com o mesmo valor.
            return (dao.SelectBy_Chave_Valor(entity.ent_id, entity.pac_chave, entity.pac_valor).Rows.Count > 0);
        }

        #endregion Validação

        #region Salvar

        /// <summary>
        /// Retorna um Booleano na qual faz verificação das regras de validação e inserção de um novo parametro
        /// </summary>
        /// <param name="entity">Entidade do Parametro</param>
        /// <param name="tipo">Tipo de parâmetro</param>
        /// <returns>True - caso realize a inserção;</returns>
        public static bool Save(ACA_ParametroAcademico entity, TipoParametroAcademico tipo)
        {
            //Remove os espaços em branco do valor do parametro para efetuar a validação
            entity.pac_valor = entity.pac_valor.Trim();

            ACA_ParametroAcademicoDAO dao = new ACA_ParametroAcademicoDAO();

            //Valida a entidade e verifica se o valor do parametro é diferente de nulo
            if ((entity.Validate()))
            {
                ValidaDadosRelacionados(entity);

                if (tipo == TipoParametroAcademico.Unico || tipo == TipoParametroAcademico.Multiplo)
                {
                    string msg;

                    // Validação diferenciada para o parâmetro tipo Unico ou Multiplo.
                    if (ValidaUnicoMultiplo(entity, out msg))
                        return dao.Salvar(entity);

                    throw new ArgumentException(msg);
                }

                if (entity.pac_obrigatorio && UtilBO.VerificaDataMaior(DateTime.Now.Date, entity.pac_vigenciaInicio.Date))
                    throw new ArgumentException("Vigência inicial não pode ser anterior à data atual.");

                if (VerificaVigencia(entity))
                {
                    if (entity.pac_obrigatorio)
                        throw new ArgumentException("Vigência inicial deve ser maior à data do último valor cadastrado.");

                    if (entity.IsNew == false)
                        return dao.Salvar(entity);

                    if (entity.pac_vigenciaInicio > entity.pac_vigenciaFim)
                        throw new ArgumentException("A data inicial da vigência deve ser menor que a data final.");

                    throw new ArgumentException("Parâmetro apresenta conflito nas vigências.");
                }

                if ((entity.pac_vigenciaFim != new DateTime()) && (entity.pac_vigenciaInicio > entity.pac_vigenciaFim))
                    throw new ArgumentException("A data inicial da vigência deve ser menor que a data final.");

                if (entity.pac_obrigatorio && AdequaVigencia(entity))
                    throw new Exception("Erro na adequação da data.");

                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Retorna um Booleano na qual faz atualização/adequação da data de vigencia final do ultimo parametro (anterior)
        /// ao parametro a ser inserido. Executado somente para parametros obrigatórios;
        /// </summary>
        /// <param name="entity">Entidade do Parametro</param>
        /// <returns>True - caso realize a atualização;</returns>
        public static bool AdequaVigencia(ACA_ParametroAcademico entity)
        {
            ACA_ParametroAcademicoDAO dal = new ACA_ParametroAcademicoDAO();
            try
            {
                return dal.Update_VigenciaFim(entity.ent_id, entity.pac_chave, entity.pac_vigenciaInicio.AddDays(-1));
            }
            catch
            {
                throw;
            }
        }

        #endregion Salvar

        #region Excluir

        /// <summary>
        /// Deleta um parâmetro.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Delete(ACA_ParametroAcademico entity, TipoParametroAcademico tipo)
        {
            ACA_ParametroAcademicoDAO dao = new ACA_ParametroAcademicoDAO();

            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                dao._Banco = banco;

                entity = GetEntity(entity);

                if (entity.pac_chave.Equals("TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA") && ESC_EscolaBO.VerificaEscolaComTipo(new Guid(entity.pac_valor)))
                {
                    throw new ValidationException("Não é possível excluir um parâmetro que possua outros registros relacionados a ele.");
                }

                if ((!entity.pac_chave.Equals("TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA")) &&
                    (!entity.pac_chave.Equals("PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO")) &&
                    (!entity.pac_chave.Equals("PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO")) &&
                    (!entity.pac_chave.Equals("TIPO_DISCIPLINA_CONTROLE_FREQUENCIA")) &&
                    (!entity.pac_chave.Equals("GRUPO_RESPONSAVEL_BOLETIMONLINE")) &&
                    (!entity.pac_chave.Equals("CARGO_DOCENTE_JEX")) &&
                    (entity.pac_vigenciaInicio.Date <= DateTime.Now.Date))
                {
                    throw new ValidationException("Não é possível excluir um parâmetro que possua a data de vigência inicial igual ou anterior à data atual.");
                }

                if (tipo == TipoParametroAcademico.VigenciaObrigatorio)
                {
                    // Se for vigência obrigatório, tem que pegar o último parâmetro e voltar
                    // a vigência fim dele para Null.
                    ACA_ParametroAcademico entityAlerar = dao.Load_UltimoVigenciaFim_By_Chave(entity.ent_id, entity.pac_chave);

                    if (!entityAlerar.IsNew)
                    {
                        entityAlerar.pac_vigenciaFim = new DateTime();
                        entityAlerar.pac_dataAlteracao = DateTime.Now;

                        dao.Salvar(entityAlerar);
                    }
                }

                return dao.Delete(entity);
            }
            catch (Exception ex)
            {
                banco.Close(ex);

                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        #endregion Excluir

        #endregion Métodos
    }
}