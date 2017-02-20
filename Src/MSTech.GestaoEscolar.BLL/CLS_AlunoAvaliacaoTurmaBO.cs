using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situação da matrícula do aluno nas disciplinas.
    /// </summary>
    public enum eSituacaoMatriculaTurmaDisicplina : byte
    {
        Ativo = 1,
        Inativo = 5,
        Dispensado = 6,
        ForaRede = 7
    }

    /// <summary>
    /// URL's para retorno despois do cadastro de efetivação
    /// </summary>
    public enum URL_Retorno_Efetivacao : byte
    {
        EfetivacaoBusca = 1
        ,

        PlanejamentoCadastroAula = 2
        ,

        RenovacaoCadastro = 3
        ,

        EnturmacaoCadastro = 4
    }

    /// <summary>
    /// Situações da avaliação do aluno na turma.
    /// </summary>
    public enum CLS_AlunoAvaliacaoTurmaSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
    }

    #endregion Enumeradores

    #region Estrutura da efetivação

    public struct sDisciplinasDivergentesPorAluno
    {
        public long alu_id;
        public List<string> disciplinasDivergentes;
    }

    /// <summary>
    /// Estrutura usada para cadastrar a nota para a avaliação.
    /// </summary>
    public struct CLS_AvaliacaoTurma_Cadastro
    {
        public CLS_AlunoAvaliacaoTurma entity;
        public MtrTurmaResultado resultado;
        public int mtu_idAnterior;

        public int tpc_id { get; set; }
    }

    #endregion Estrutura da efetivação

    #region Estrutura do boletim do aluno

    /// <summary>
    /// Estrutura utilizada para motrar o boletim do aluno na tela.
    /// </summary>
    [Serializable]
    public struct BoletimAluno
    {
        public long alu_id;
        public int mtu_id;
        public string tur_codigo;
        public long tur_id;
        public int tpc_id;
        public int tpc_ordem;
        public int mtd_id;
        public long tud_id;
        public bool tud_global;
        public string Disciplina;
        public string DisciplinaEspecial;
        public bool tud_disciplinaEspecial;
        public string tpc_nome;
        public int numeroFaltas;
        public string avaliacao;
        public string avaliacaoOriginal;
        public string avaliacaoSemPosConselho;
        public string avaliacaoPosConselho;
        public bool NotaNumerica;
        public string avaliacaoAdicional;
        public bool NotaAdicionalNumerica;
        public string NotaRP;
        public int NotaIDRP;
        public bool mostraConceito;
        public bool mostraNota;
        public bool ava_mostraConceito;
        public bool ava_mostraNota;
        public bool mostraFrequencia;
        public bool naoExibirNota;
        public bool naoExibirFrequencia;
        public bool naoExibirBoletim;
        public decimal NotaSomar;
        public decimal frequenciaAcumulada;
        public bool MostrarLinhaDisciplina;
        public int NotaID;
        public int ava_id;
        public byte ava_tipo;
        public int fav_id;
        public byte fav_tipo;
        public bool ava_exibeSemProfessor;
        public bool ava_exibeNaoAvaliados;
        public bool semProfessor;
        public bool naoAvaliado;
        public bool naoLancarNota;
        public int ava_idRec;
        public string ava_nomeRec;
        public string esc_codigo;
        public string esc_nome;
        public string NotaRecEsp;
        public int ava_idRecEsp;
        public int NotaIDRecEsp;
        public decimal NotaTotal;
        public string NotaResultado;
        public long tud_idResultado;
        public int mtu_idResultado;
        public int mtd_idResultado;
        public int NotaIdResultado;
        public int fav_idResultado;
        public int ava_idResultado;
        public bool notaDisciplinaConceito;
        public int dda_id;
        public byte tud_tipo;
        public int ausenciasCompensadas;
        public decimal FrequenciaFinalAjustada;
        public decimal FrequenciaGlobal;
        public int esa_tipo;
        public byte regencia;
        public string nomeDisciplina;
        public int tds_id;
        public int tds_tipo;
        public int tds_ordem;
        public bool EnriquecimentoCurricular;
        public bool Recuperacao;
        public string ParecerFinal;
        public short mtu_resultado;
        public string ParecerConclusivo;
        public string usuarioParecerConclusivo;
        public DateTime dataAlteracaoParecerConclusivo;

        public decimal fav_variacao;
        public string frequencia;
        public bool PermiteEditar;
        public int esa_id;
        public byte eap_ordem;

        public int cur_id;
        public int crr_id;
        public int crp_id;

        // Variáveis projeto complementar histórico
        public int ProjetoId;
        public int NotaProjetoId;
        public byte ResultadoProjeto;
        public bool ProjetoComplementar;

        public int numeroAulas;
        public string strFrequenciaFinalAjustada;
        public long TudIdRegencia;
        public int MtdIdRegencia;
        public int AtdIdRegencia;
        public bool PermiteEdicaoDocente;

        //Disciplinas relacionadas (docencia compartilhada)
        public string disRelacionadas;
    }

    /// <summary>
    /// Estrutura utilizada para motrar o boletim do aluno na tela.
    /// </summary>
    [Serializable]
    public struct DadosFechamento
    {
        public long alu_id;
        public int mtu_id;
        public string tur_codigo;
        public long tur_id;
        public int cal_id;
        public int tpc_id;
        public int tpc_ordem;
        public bool bimestreComLancamento;
        public bool ApareceFechamento;
        public int mtd_id;
        public long tud_id;
        public bool tud_global;
        public string Disciplina;
        public string tpc_nome;
        public int numeroFaltas;
        public string avaliacao;
        public string avaliacaoSemPosConselho;
        public string avaliacaoPosConselho;
        public bool NotaNumerica;
        public string avaliacaoAdicional;
        public bool NotaAdicionalNumerica;
        public bool mostraConceito;
        public bool mostraNota;
        public bool mostraFrequencia;
        public bool naoExibirNota;
        public bool naoExibirFrequencia;
        public bool MostrarLinhaDisciplina;
        public int NotaID;
        public int ava_id;
        public int fav_id;
        public string NotaResultado;
        public long tud_idResultado;
        public int mtu_idResultado;
        public int mtd_idResultado;
        public int NotaIdResultado;
        public int fav_idResultado;
        public int ava_idResultado;
        public int dda_id;
        public byte tud_tipo;
        public int ausenciasCompensadas;
        public decimal FrequenciaFinalAjustada;
        public string FrequenciaGlobal;
        public int esa_tipo;
        public string nomeDisciplina;
        public int tds_id;
        public int tds_ordem;
        public bool EnriquecimentoCurricular;
        public bool Recuperacao;
        public string ParecerFinal;
        public short mtu_resultado;
        public string usuarioParecerConclusivo;
        public DateTime dataAlteracaoParecerConclusivo;
        public string esc_codigo;
        public string esc_nome;

        public decimal fav_variacao;
        public int cur_id;
        public int crr_id;
        public int crp_id;
        public int numeroAulas;
        public bool existeAulaBimestre;

        public bool esconderPendencia;

        public eSituacaoMatriculaTurmaDisicplina SituacaoDisciplina;

        public long tur_idDisciplina;

        public bool lancaParecerFinal;
        public int faltasExternas;
    }

    /// <summary>
    /// Estrutura utilizada para mostrar as avaliações da secretaria no boletim do aluno na tela.
    /// </summary>
    public struct BoletimAlunoAvaliacoesSecretaria
    {
        public int tpc_id;
        public int tds_id;
        public int tpc_ordem;
        public string avs_nome;
        public long alu_id;
        public long tur_id;
        public int mtu_id;
        public int fav_id;
        public int avs_id;
        public int NotaID;
        public string Nota;
        public string nomeDisciplina;
        public string tpc_nome;

        /// <summary>
        /// Chave utilizada para agrupar as avaliações.
        /// Nome da avaliação + o id do tipo de disciplina.
        /// </summary>
        public string chaveAvaliacao
        {
            get { return avs_nome + ";" + tds_id; }
        }
    }

    /// <summary>
    /// Estrutura utilizada para guardar as avaliacoes liberadas para mostrar no boletim
    /// </summary>
    public struct BoletimAlunoAvaliacoesLiberadas
    {
        public long alu_id;
        public int mtu_id;
        public int ava_id;
        public bool ava_apareceBoletim;
        public int tpc_id;
        public DateTime dataAtualizacao;
    }

    [Serializable]
    public struct BoletimAlunoDocCompartilhada
    {
        public long tud_id;
        public string Disciplina;
    }

    #endregion Estrutura do boletim do aluno

    #region Estrutura do boletim de aluno para lançamento de notas de anos anteriores

    /// <summary>
    /// Estrutura para armazenar os dados do boletim para mostrar na tela de lançamento de notas de anos anteriores.
    /// </summary>
    public struct BoletimAlunoAnterior
    {
        public long alu_id;
        public int mtu_id;
        public string tur_codigo;
        public long tur_id;
        public int tpc_id;
        public int tpc_ordem;
        public int mtd_id;
        public long tud_id;
        public bool tud_global;
        public string Disciplina;
        public string tpc_nome;
        public int numeroFaltas;
        public int numeroAulas;
        public string avaliacao;
        public bool NotaNumerica;
        public string avaliacaoAdicional;
        public bool NotaAdicionalNumerica;
        public bool mostraConceito;
        public bool mostraNota;
        public bool mostraFrequencia;
        public decimal frequenciaAcumulada;
        public byte resultado;
        public bool MostrarLinhaDisciplina;
        public int NotaID;
        public int ava_id;
        public byte ava_tipo;
        public string esc_codigo;
        public string esc_nome;
        public string NotaRec;
        public int ava_idRec;
        public int NotaIDRec;
    }

    #endregion Estrutura do boletim de aluno para lançamento de notas de anos anteriores

    #region Estrutura para cadastro de observação para conselho pedagógico

    /// <summary>
    /// Estrutura que armazena as observações do aluno na efetivação.
    /// </summary>
    [Serializable]
    public struct CLS_AlunoAvaliacaoTur_Observacao
    {
        public long tur_id;
        public long alu_id;
        public int mtu_id;
        public int fav_id;
        public int ava_id;
        public CLS_AlunoAvaliacaoTurmaObservacao entityObservacao;
        public List<CLS_AlunoAvaliacaoTurmaQualidade> ltQualidade;
        public List<CLS_AlunoAvaliacaoTurmaDesempenho> ltDesempenho;
        public List<CLS_AlunoAvaliacaoTurmaRecomendacao> ltRecomendacao;
    }

    #endregion Estrutura para cadastro de observação para conselho pedagógico

    public class CLS_AlunoAvaliacaoTurmaBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDAO, CLS_AlunoAvaliacaoTurma>
    {
        #region Consultas

        /// <summary>
        /// Retorna a frequência acumulada calculada na entidade informada.
        /// </summary>
        /// <param name="entity">A entidade que será calculada a frequência acumulada</param>
        /// <returns>
        /// Valor da frequência acumulada calculada.
        /// </returns>
        public static decimal RetornaFrequenciaAcumulada_Registro(CLS_AlunoAvaliacaoTurma entity)
        {
            Decimal frequenciaAcumuladaCalculada = 0;
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            DataTable dt = dao.RetornaFrequenciaAcumulada_Registro
                (entity.tur_id, entity.alu_id, entity.mtu_id, entity.fav_id, entity.ava_id);

            if (dt.Rows.Count > 0)
            {
                Decimal.TryParse(dt.Rows[0]["frequenciaAcumuladaCalculada"].ToString(), out frequenciaAcumuladaCalculada);
            }

            return frequenciaAcumuladaCalculada;
        }

        /// <summary>
        /// Retorna a frequência acumulada calculada.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ava_id">Id da avaliação.</param>
        /// <param name="aat_numeroAulas">Número de aulas.</param>
        /// <param name="aat_numeroFaltas">Número de faltas.</param>
        /// <returns>Valor da frequência acumulada calculada.</returns>
        public static decimal RetornaFrequenciaAcumuladaCalculada(
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id,
            int aat_numeroAulas,
            int aat_numeroFaltas
        )
        {
            Decimal frequenciaAcumuladaCalculada = 0;
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            DataTable dt = dao.RetornaFrequenciaAcumuladaCalculada(tur_id, alu_id, mtu_id, fav_id, ava_id, aat_numeroAulas, aat_numeroFaltas);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(string.IsNullOrEmpty(dt.Rows[0]["frequenciaAcumuladaCalculada"].ToString()) ? 0 : dt.Rows[0]["frequenciaAcumuladaCalculada"]);
            }

            return frequenciaAcumuladaCalculada;
        }

        /// <summary>
        /// Retorna a frequência ajustada calculada.
        /// </summary>
        /// <param name="tud_id">Id da turmaDisciplina.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ava_id">Id da avaliação.</param>
        /// <param name="tpc_id">Id do tipoPeriodoCalendario .</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="aat_numeroAulas">Número de aulas.</param>
        /// <param name="aat_numeroFaltas">Número de faltas.</param>
        /// <param name="aat_numeroAusenciasCompensadas">Número de ausências compensadas.</param>
        /// <returns>Valor da frequência ajustada calculada.</returns>
        public static decimal RetornaFrequenciaAjustadaCalculada(
            long tud_id,
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id,
            int tpc_id,
            byte tipoEscalaDisciplina,
            byte tipoEscalaDocente,
            byte tipoLancamento,
            byte fav_calculoQtdeAulasDadas,
            int aat_numeroAulas,
            int aat_numeroFaltas,
            int aat_numeroAusenciasCompensadas
        )
        {
            Decimal frequenciaAcumuladaCalculada = 0;
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            DataTable dt = dao.RetornaFrequenciaAjustadaCalculada(tud_id, tur_id, alu_id, mtu_id, fav_id, ava_id, tpc_id, tipoEscalaDisciplina, tipoEscalaDocente, tipoLancamento, fav_calculoQtdeAulasDadas, aat_numeroAulas, aat_numeroFaltas, aat_numeroAusenciasCompensadas);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(string.IsNullOrEmpty(dt.Rows[0]["frequenciaAjustadaCalculada"].ToString()) ? 0 : dt.Rows[0]["frequenciaAjustadaCalculada"]);
            }

            return frequenciaAcumuladaCalculada;
        }

        /// <summary>
        /// Busca o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// Busca também disciplinas eletivas que o aluno cursou (tem nota) mas que a turma atual não oferece.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public static List<BoletimAluno> RetornaBoletimAluno(long alu_id, int mtu_id)
        {
            DataTable dt = new CLS_AlunoAvaliacaoTurmaDAO().SelecionaBoletimAluno(alu_id, mtu_id);

            List<BoletimAluno> lista = (from DataRow dr in dt.Rows
                                        select
                                            (BoletimAluno)GestaoEscolarUtilBO.DataRowToEntity(dr, new BoletimAluno())
                                       ).ToList();

            return lista;
        }

        /// <summary>
        /// Busca o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// Busca também disciplinas eletivas que o aluno cursou (tem nota) mas que a turma atual não oferece.
        /// </summary>
        /// <param name="dtAlunoMatriculaTurma">Alunos.</param>
        /// <returns>Lista com os dados do boletim.</returns>
        public static List<BoletimAluno> RetornaBoletimAluno(DataTable dtAlunoMatriculaTurma)
        {
            DataTable dt = new CLS_AlunoAvaliacaoTurmaDAO().SelecionaBoletimAluno(dtAlunoMatriculaTurma);

            List<BoletimAluno> lista = (from DataRow dr in dt.Rows
                                        select
                                            (BoletimAluno)GestaoEscolarUtilBO.DataRowToEntity(dr, new BoletimAluno())
                                       ).ToList();

            return lista;
        }

        /// <summary>
        /// Busca as notas das avaliações da secretaria para o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public static List<BoletimAlunoAvaliacoesSecretaria> RetornaBoletimAlunoAvaliacoesSecretaria(long alu_id, int mtu_id)
        {
            DataTable dt = new CLS_AlunoAvaliacaoTurmaDAO().SelecionaBoletimAlunoAvaliacoesSecretaria(alu_id, mtu_id);

            List<BoletimAlunoAvaliacoesSecretaria> lista = (from DataRow dr in dt.Rows
                                                            select
                                                                (BoletimAlunoAvaliacoesSecretaria)GestaoEscolarUtilBO.DataRowToEntity(dr, new BoletimAlunoAvaliacoesSecretaria())
                                       ).ToList();

            return lista;
        }

        /// <summary>
        /// Seleciona ou Corrige a frequencia acumulada do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="tpc_id">Tcp_id a corrigir</param>
        /// <param name="mtu_id">Id da mátrícula do aluno</param>
        /// <param name="realizaUpdateFrequenciaAcumulada">Se realiza ou não a alteração</param>
        /// <param name="calcularQtAulasNovamente">Se calcula a quantidade de aulas novamente</param>
        /// <param name="buscarLancamentosDocente">Se busca lancamentos do docente</param>
        /// <returns>Resultados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCorrigeFrequenciaAcumulada(
            long alu_id
            , int tpc_id
            , int mtu_id
            , byte realizaUpdateFrequenciaAcumulada
            , byte calcularQtAulasNovamente
            , byte buscarLancamentosDocente
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            return dao.SelecionaCorrigeFrequenciaAcumulada(alu_id, tpc_id, mtu_id, realizaUpdateFrequenciaAcumulada, calcularQtAulasNovamente, buscarLancamentosDocente, out totalRecords);
        }

        /// <summary>
        /// Lista os mut_id de um aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">mtu_id</param>
        /// <returns>Resultados</returns>
        public static DataTable ListaMtuDeAluno(
            long alu_id
            , int mtu_id
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            return dao.ListaMtuDeAluno(alu_id, mtu_id, out totalRecords);
        }

        /// <summary>
        /// Retorna uma entidade carregada, buscando pela "chave" da avaliação do aluno
        /// (parâmetros).
        /// </summary>
        /// <param name="tur_id">Id da turma - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Entidade CLS_AlunoAvaliacaoTurma</returns>
        public static CLS_AlunoAvaliacaoTurma GetEntityBy_ChaveAvaliacaoAluno
        (
            Int64 tur_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 fav_id
            , Int32 ava_id
            , TalkDBTransaction banco
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO { _Banco = banco };

            return dao.LoadBy_ChaveAvaliacaoAluno(tur_id, alu_id, mtu_id, fav_id, ava_id);
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurma que estejam cadastradas na avaliação para a turma.
        /// </summary>
        /// <param name="tur_id">Id da turma - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurma> SelecionaPor_TurmaAvaliacao
        (
            Int64 tur_id
            , Int32 fav_id
            , Int32 ava_id
            , TalkDBTransaction banco
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO { _Banco = banco };
            dao._Banco = banco;
            DataTable dt = dao.SelectBy_TurmaAvaliacao(tur_id, fav_id, ava_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurma())).ToList();
        }

        public static List<BoletimAlunoAvaliacoesLiberadas> SelecionaAvaliacoesLiberadasView(long alu_id, int mtu_id)
        {
            DataTable dt = new CLS_AlunoAvaliacaoTurmaDAO().SelecionaAvaliacoesLiberadasView(alu_id, mtu_id);

            List<BoletimAlunoAvaliacoesLiberadas> lista = (from DataRow dr in dt.Rows
                                                           select
                                                               (BoletimAlunoAvaliacoesLiberadas)GestaoEscolarUtilBO.DataRowToEntity(dr, new BoletimAlunoAvaliacoesLiberadas())
                                       ).ToList();

            return lista;
        }

        /// <summary>
        /// O método carrega dados do boletim de um aluno de anos anteriores.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static List<BoletimAlunoAnterior> SelecionaBoletimAlunoAnosAnteriores(long alu_id, int mtu_id, Guid ent_id)
        {
            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            return (new CLS_AlunoAvaliacaoTurmaDAO()
                    .SelecionaBoletimAlunoAnosAnteriores(alu_id, mtu_id, controleOrdemDisciplinas)).Rows.Cast<DataRow>()
                    .Select(p => (BoletimAlunoAnterior)GestaoEscolarUtilBO.DataRowToEntity(p, new BoletimAlunoAnterior())).ToList();
        }

        /// <summary>
        /// Seleciona os dados das notas da tela de VisualizaConteudo
        /// </summary>
        /// <param name="tipo">Tipo de busca: 
        ///                    "Turma" seleciona os dados das notas da turma por escola, ano e turma
        ///                    "Aluno" seleciona os dados das notas do aluno na turma por id de aluno e id de matricula na turma</param>
        /// <param name="parametro1">Nome OU código da escola / Id do aluno</param>
        /// <param name="parametro2">Ano letivo / Id da matricula do aluno na turma</param>
        /// <param name="parametro3">Código da turma / Vazio</param>
        /// <returns>Retorna dados das notas</returns>
        public static DataSet VisualizaConteudo(string tipo, string parametro1, string parametro2, string parametro3)
        {
            return new CLS_AlunoAvaliacaoTurmaDAO().SelecionaVisualizaConteudo(tipo, parametro1, parametro2, parametro3);
        }

        #endregion Consultas

        #region Saves

        /// <summary>
        /// Salva os resultados das avaliações da lista.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="resultadoFinal">Indica se é o resultado final da turma</param>
        /// <param name="tamanhoMaximoKB"></param>
        /// <param name="TiposArquivosPermitidos"></param>
        /// <param name="entTurma"></param>
        /// <param name="entFormatoAvaliacao"></param>
        /// <param name="tipoFrequencia">Tipo de frequência do formato de avaliação</param>
        /// <param name="tipoCalculo">Cálculo de quantidade de aulas dadas do formato</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tipoAvaliacao"></param>
        /// <param name="salvarEfetivacao">Indica se o método foi chamado pela tela de salvar efetivação</param>
        /// <param name="listDisciplinasDivergentesPorAluno">Lista de discplinas divergentes por aluno</param>
        /// <param name="listDisciplinasNaoLancadas"></param>
        /// <param name="listAlunosComDivergencia">Lista de alunos com divergência</param>
        /// <param name="entEscalaAvaliacao">Escala de avaliação do conceito global</param>
        /// <param name="listaNotaFinalAluno">Lista com as notas finais dos alunos</param>
        /// <param name="realizarProgressao">Indica se é necessário realizar a progressão do aluno</param>
        /// <param name="cursoSeriadoAvaliacoes">Indica se o curso da turma é seriado por avaliações (PEJA)</param>
        /// <param name="ava_id">ID da avaliação que está sendo salva</param>
        /// <param name="RealizouProgressao">Flag que indica se foi realizada a progressão dos alunos da turma (quando é PEJA)</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns></returns>
        public static bool Save
        (
            List<CLS_AvaliacaoTurma_Cadastro> lista
            , bool resultadoFinal
            , int tamanhoMaximoKB
            , string[] TiposArquivosPermitidos
            , TUR_Turma entTurma
            , ACA_FormatoAvaliacao entFormatoAvaliacao
            , ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia
            , ACA_FormatoAvaliacaoCalculoQtdeAulasDadas tipoCalculo
            , out string msgLancamentoFrequencia
            , int tpc_id
            , AvaliacaoTipo tipoAvaliacao
            , bool salvarEfetivacao
            , out List<sDisciplinasDivergentesPorAluno> listDisciplinasDivergentesPorAluno
            , out List<string> listDisciplinasNaoLancadas
            , out List<long> listAlunosComDivergencia
            , ACA_EscalaAvaliacao entEscalaAvaliacao
            , List<NotaFinalAlunoTurma> listaNotaFinalAluno
            , bool cursoSeriadoAvaliacoes
            , int ava_id
            , bool notaPosConselho
            , Guid usu_idLogado
            , Guid ent_id
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            listDisciplinasDivergentesPorAluno = new List<sDisciplinasDivergentesPorAluno>();
            listDisciplinasNaoLancadas = new List<string>();
            listAlunosComDivergencia = new List<long>();
            msgLancamentoFrequencia = string.Empty;

            try
            {
                // Valida dados do formato e avaliação de acordo com o curso.
                if (lista.Count > 0)
                {
                    // se for avaliacao final analitica, pode ocorrer de salvar o fechamento do ultimo bimestre também
                    int ava_idUltimoPeriodo = -1;
                    int tpc_idUltimoPeriodo = -1;
                    List<CLS_AvaliacaoTurma_Cadastro> listaUltimoPeriodo = new List<CLS_AvaliacaoTurma_Cadastro>();
                    if (tipoAvaliacao == AvaliacaoTipo.Final && entFormatoAvaliacao.fav_avaliacaoFinalAnalitica && lista.Count > 0)
                    {
                        listaUltimoPeriodo = lista.Where(d => d.entity.ava_id != ava_id).ToList();
                        if (listaUltimoPeriodo.Count > 0)
                        {
                            ava_idUltimoPeriodo = listaUltimoPeriodo[0].entity.ava_id;
                            ACA_Avaliacao avaliacao = ACA_AvaliacaoBO.GetEntity(new ACA_Avaliacao { ava_id = ava_idUltimoPeriodo, fav_id = entFormatoAvaliacao.fav_id }, banco);
                            tpc_idUltimoPeriodo = avaliacao.tpc_id;

                            // Ordeno deixando primeiro a avaliação final
                            lista = lista.OrderBy(turma => turma.tpc_id).ThenBy(turma => turma.entity.ava_id).ToList();
                        }
                    }

                    ValidaRegrasCurso(entTurma, entFormatoAvaliacao, lista[0].entity.ava_id, banco);
                    if (ava_idUltimoPeriodo > 0)
                    {
                        ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_idUltimoPeriodo, banco);
                    }

                    // Lista de tud_ids que teve lançamento mensal.
                    List<long> tud_idsLancamentoMensal;
                    if (tpc_idUltimoPeriodo > 0)
                    {
                        tud_idsLancamentoMensal = TUR_TurmaDisciplinaBO.SelecionaTud_ids_PorTurma_LancamentoMensal(entTurma.tur_id, tpc_idUltimoPeriodo);
                    }
                    else
                    {
                        tud_idsLancamentoMensal = TUR_TurmaDisciplinaBO.SelecionaTud_ids_PorTurma_LancamentoMensal(entTurma.tur_id, tpc_id);
                    }
                    DateTime cap_dataInicio = new DateTime();
                    DateTime cap_dataFim = new DateTime();

                    string tud_ids = string.Join(",", tud_idsLancamentoMensal.Select(item => item.ToString()).ToArray());

                    List<MTR_MatriculaTurmaDisciplina> listaMatriculasDisc = new List<MTR_MatriculaTurmaDisciplina>();
                    List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAvaliacaoCadastrados = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
                    List<TUR_TurmaDisciplina> listaNomeDisciplinas = new List<TUR_TurmaDisciplina>();

                    if (!string.IsNullOrEmpty(tud_ids))
                    {
                        listaMatriculasDisc = MTR_MatriculaTurmaDisciplinaBO.SelecionaMatriculasPorTurmaDisciplina(tud_ids, banco);
                        if (ava_idUltimoPeriodo > 0)
                        {
                            listaAvaliacaoCadastrados = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaPor_DisciplinaAvaliacaoTurmaDisciplina(tud_ids, entTurma.fav_id, ava_idUltimoPeriodo, banco);
                        }
                        else
                        {
                            listaAvaliacaoCadastrados = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaPor_DisciplinaAvaliacaoTurmaDisciplina(tud_ids, entTurma.fav_id, ava_id, banco);
                        }
                        listaNomeDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(entTurma.tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);
                    }

                    List<CLS_AlunoAvaliacaoTurma> listaCadastrados = SelecionaPor_TurmaAvaliacao(entTurma.tur_id, entTurma.fav_id, ava_id, banco);

                    bool salvarUltimoBimestre = false;
                    foreach (CLS_AvaliacaoTurma_Cadastro item in lista)
                    {
                        CLS_AlunoAvaliacaoTurma entity = item.entity;

                        // eh a avaliação periodica do ultimo bimestre que está salvando junto
                        // nesse caso, validar se ainda não existe registro na CLS_AlunoAvaliacaoTurma
                        // (como a lista esta ordenada, se já tiver registro, pode sair do loop)
                        if (entity.ava_id != ava_id && !salvarUltimoBimestre)
                        {
                            if (CLS_AlunoAvaliacaoTurmaBO.SelecionaPor_TurmaAvaliacao(entTurma.tur_id, entity.fav_id, entity.ava_id, banco).Count > 0)
                            {
                                break;
                            }
                            else
                            {
                                listaCadastrados = SelecionaPor_TurmaAvaliacao(entTurma.tur_id, entTurma.fav_id, ava_idUltimoPeriodo, banco);
                                salvarUltimoBimestre = true;
                                tipoAvaliacao = AvaliacaoTipo.Periodica;
                                tpc_id = tpc_idUltimoPeriodo;
                            }
                        }

                        // Se for resultado final, não salva na CLS_AvaliacaoTurma.
                        if (resultadoFinal || entity.Validate())
                        {
                            if (entity.aat_situacao == 3 && entity.aat_id > 0)
                            {
                                Delete(entity, banco);
                            }
                            else
                            {
                                if (!resultadoFinal)
                                {
                                    if (!listAlunosComDivergencia.Exists(p => p == entity.alu_id))
                                    {
                                        if (entity.arq_idRelatorio > 0)
                                        {
                                            SYS_Arquivo arq = new SYS_Arquivo { arq_id = entity.arq_idRelatorio };
                                            SYS_ArquivoBO.GetEntity(arq, banco);
                                            arq.arq_situacao = (byte)SYS_ArquivoSituacao.Ativo;
                                            SYS_ArquivoBO.Save(arq, tamanhoMaximoKB, TiposArquivosPermitidos, banco);
                                        }

                                        Save(entity, banco, listaCadastrados);

                                        // se for a avaliação final salvo o registro no MTR_MatriculaTurmaDisciplina
                                        if (entity.ava_id == ava_id)
                                        {
                                            if (cursoSeriadoAvaliacoes)
                                            {
                                                if (tipoAvaliacao == AvaliacaoTipo.Periodica ||
                                                    tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal ||
                                                    tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal && tpc_id > 0)
                                                {
                                                    if (cap_dataInicio == new DateTime())
                                                    {
                                                        // Se ainda não foi buscada a data de início e fim do período, busca.
                                                        bool UltimoPeriodo;
                                                        // Buscar data de início e fim do período do calendário da avaliação.
                                                        ACA_CalendarioPeriodoBO.RetornaDatasPeriodoPor_FormatoAvaliacaoTurma
                                                            (tpc_id, entTurma.tur_id, entTurma.fav_id, banco,
                                                             out cap_dataInicio, out cap_dataFim,
                                                             out UltimoPeriodo);
                                                    }

                                                    // Busca registro da ACA_AlunoCurriculoAvaliacao, para salvar caso o
                                                    // curso seja Seriado por avaliações.
                                                    ACA_AlunoCurriculoAvaliacaoBO.SalvarAvaliacaoAlunoSemAvanco(banco,
                                                                                                                entity,
                                                                                                                cap_dataInicio,
                                                                                                                cap_dataFim);
                                                }
                                            }

                                            // Se for avaliação final, per+final ou rec.final salva MTU.
                                            // Ou se for avaliação periódica do PEJA, agora salvará o resultado,
                                            // além de efetuar a progressão do aluno.
                                            if ((tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal ||
                                                tipoAvaliacao == AvaliacaoTipo.Final ||
                                                tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                                                || (tipoAvaliacao == AvaliacaoTipo.Periodica && cursoSeriadoAvaliacoes)
                                                )
                                            {
                                                // Se for passado o resultado, salva na MTR_MatriculaTurma.
                                                MTR_MatriculaTurma entMatr = new MTR_MatriculaTurma
                                                {
                                                    alu_id = entity.alu_id
                                                    ,
                                                    mtu_id = entity.mtu_id
                                                };
                                                MTR_MatriculaTurmaBO.GetEntity(entMatr, banco);

                                                entMatr.usu_idResultado = usu_idLogado;
                                                entMatr.mtu_resultado = Convert.ToByte(item.resultado);

                                                // Considera a média do aluno a nota da avaliação do tipo final
                                                if (tipoAvaliacao == AvaliacaoTipo.Final)
                                                {
                                                    entMatr.mtu_avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                            entity.aat_avaliacaoPosConselho :
                                                                            entity.aat_avaliacao;
                                                    entMatr.mtu_frequencia = entity.aat_frequencia;
                                                }
                                                else
                                                {
                                                    if (!resultadoFinal)
                                                    {
                                                        if (entEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                                                        {
                                                            List<ACA_EscalaAvaliacaoParecer> pareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(entEscalaAvaliacao.esa_id);

                                                            ACA_EscalaAvaliacaoParecer entityParecerAvaliacao = pareceres.Find(p => p.eap_valor.CompareTo(entity.aat_avaliacao) == 0) ?? new ACA_EscalaAvaliacaoParecer();

                                                            if (tipoAvaliacao != AvaliacaoTipo.RecuperacaoFinal)
                                                            {
                                                                entMatr.mtu_avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                                        entity.aat_avaliacaoPosConselho :
                                                                                        entity.aat_avaliacao;
                                                            }
                                                            else
                                                            {
                                                                string notaFinal = string.Empty;
                                                                if (listaNotaFinalAluno.Count > 0)
                                                                    notaFinal = listaNotaFinalAluno.Find(p => p.alu_id == entMatr.alu_id && p.mtu_id == entMatr.mtu_id).nota;

                                                                ACA_EscalaAvaliacaoParecer entityParecerNotaFinal = pareceres.Find(p => p.eap_valor.CompareTo(notaFinal) == 0) ?? new ACA_EscalaAvaliacaoParecer();

                                                                if (entityParecerAvaliacao.eap_ordem >= entityParecerNotaFinal.eap_ordem)
                                                                {
                                                                    entMatr.mtu_avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                                            entity.aat_avaliacaoPosConselho :
                                                                                            entity.aat_avaliacao;
                                                                }
                                                                else
                                                                {
                                                                    entMatr.mtu_avaliacao = notaFinal;
                                                                }
                                                            }
                                                        }
                                                        else if (entEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                                                        {
                                                            decimal notaAvaliacaoFinal = 0, notaAvaliacao;
                                                            string avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                               entity.aat_avaliacaoPosConselho :
                                                                               entity.aat_avaliacao;

                                                            Decimal.TryParse(avaliacao, out notaAvaliacao);

                                                            if (tipoAvaliacao != AvaliacaoTipo.RecuperacaoFinal)
                                                            {
                                                                entMatr.mtu_avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                                        entity.aat_avaliacaoPosConselho :
                                                                                        entity.aat_avaliacao;
                                                            }
                                                            else
                                                            {
                                                                if (listaNotaFinalAluno.Count > 0)
                                                                    Decimal.TryParse(listaNotaFinalAluno.Find(p => p.alu_id == entMatr.alu_id && p.mtu_id == entMatr.mtu_id).nota, out notaAvaliacaoFinal);

                                                                if (notaAvaliacao >= notaAvaliacaoFinal)
                                                                {
                                                                    entMatr.mtu_avaliacao = notaPosConselho && !string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho) ?
                                                                                            entity.aat_avaliacaoPosConselho :
                                                                                            entity.aat_avaliacao;
                                                                }
                                                                else
                                                                {
                                                                    entMatr.mtu_avaliacao = notaAvaliacaoFinal.ToString();
                                                                }
                                                            }
                                                        }

                                                        // Frequência acumulada no período.
                                                        entMatr.mtu_frequencia = RetornaFrequenciaAcumulada_Registro(entity);
                                                        entMatr.mtu_relatorio = entity.aat_relatorio;
                                                    }
                                                }

                                                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal && item.entity.mtu_id != item.mtu_idAnterior)
                                                {
                                                    MTR_MatriculaTurma entMatrAnterior = new MTR_MatriculaTurma
                                                    {
                                                        alu_id = entity.alu_id
                                                        ,
                                                        mtu_id = item.mtu_idAnterior
                                                    };
                                                    MTR_MatriculaTurmaBO.GetEntity(entMatrAnterior, banco);

                                                    entMatrAnterior.mtu_avaliacao = entMatr.mtu_avaliacao;
                                                    entMatrAnterior.mtu_frequencia = entMatr.mtu_frequencia;
                                                    entMatrAnterior.usu_idResultado = usu_idLogado;
                                                    entMatrAnterior.mtu_resultado = entMatr.mtu_resultado;

                                                    MTR_MatriculaTurmaBO.Save(entMatrAnterior, banco);
                                                }

                                                MTR_MatriculaTurmaBO.Save(entMatr, banco);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
                        }
                    }

                    if (listDisciplinasNaoLancadas.Count > 0 || listDisciplinasDivergentesPorAluno.Count == lista.Count)
                    {
                        // Se a existir disciplina ainda não efetivada ou se todos os alunos com frequencia estiverem divergentes não permite gravar
                        // Se existirem alunos divergentes, mas também existirem alunosOK, chama a exception para fechar o banco e tratar depois
                        throw new ValidationException();
                    }

                    if (HttpContext.Current != null)
                    {
                        string chave = string.Empty;

                        if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                        {
                            chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_id);
                            HttpContext.Current.Cache.Remove(chave);
                        }
                        // Esta salvando uma efetivacao do bimestre ou efetivacao final
                        else
                        {
                            // Limpa o cache da efetivacao do bimestre
                            if (tipoAvaliacao == AvaliacaoTipo.Periodica || tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal)
                            {
                                chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_id);
                                HttpContext.Current.Cache.Remove(chave);

                                // Recupero o id da avaliacao final para limpar o cache
                                DataTable avaFinal = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(AvaliacaoTipo.Final, entFormatoAvaliacao.fav_id);
                                ava_id = avaFinal.Rows.Count == 0 ? -1 : Convert.ToInt32(avaFinal.Rows[0]["ava_id"]);
                            }
                            // No caso de ser a efetivacao final, limpo apenas o cache da efetivacao do ultimo bimestre
                            else if (ava_idUltimoPeriodo > 0)
                            {
                                chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_idUltimoPeriodo);
                                HttpContext.Current.Cache.Remove(chave);
                            }

                            // Limpa o cache da efetivacao final
                            if (ava_id > 0)
                            {
                                chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Final(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_id);
                                HttpContext.Current.Cache.Remove(chave);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }
                
        /// <summary>
        /// Override do Save passando o banco - se a entidade for nova, faz uma verificação
        /// se já existe uma entidade para a turma / aluno / avaliação. Se já existir,
        /// seta os valores e altera o registro que já existe, ao invés de inserir.
        /// Não pode salvar mais de uma avaliação por aluno na turma.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="listaCadastrados">Lista de entidades cadastradas para a turma e avaliação</param>
        /// <returns>Se salvou com sucesso</returns>
        public static bool Save(CLS_AlunoAvaliacaoTurma entity, TalkDBTransaction banco, List<CLS_AlunoAvaliacaoTurma> listaCadastrados)
        {
            if (!entity.Validate())
                throw new ValidationException(UtilBO.ErrosValidacao(entity));

            CLS_AlunoAvaliacaoTurma entAux;

            if (entity.IsNew)
            {
                // Se for nova entidade - verificar se já não existe um registro com a chave
                // do lançamento de notas para esse aluno - se já foi lançado, deve
                // ser alterado o registro para não duplicar.
                entAux = listaCadastrados.Find(p =>
                                               p.tur_id == entity.tur_id
                                               && p.alu_id == entity.alu_id
                                               && p.mtu_id == entity.mtu_id
                                               && p.fav_id == entity.fav_id
                                               && p.ava_id == entity.ava_id) ?? new CLS_AlunoAvaliacaoTurma();
            }
            else
            {
                entAux = listaCadastrados.Find(p =>
                                               p.tur_id == entity.tur_id
                                               && p.alu_id == entity.alu_id
                                               && p.mtu_id == entity.mtu_id
                                               && p.aat_id == entity.aat_id) ?? new CLS_AlunoAvaliacaoTurma();
            }

            if (!entAux.IsNew)
            {
                // Seta o id da entidade auxiliar, que já existe no banco.
                entity.IsNew = entAux.IsNew;
                entity.aat_id = entAux.aat_id;

                // Verifica se existe algum arquivo de relatório para substituir o registro antigo.
                if (entAux.arq_idRelatorio > 0)
                {
                    // Se for diferente do atual
                    if (entAux.arq_idRelatorio != entity.arq_idRelatorio)
                    {
                        // Exclui do banco o registro anterior.
                        SYS_Arquivo entityArquivo = new SYS_Arquivo { arq_id = entAux.arq_idRelatorio };

                        //SYS_ArquivoBO.GetEntity(entityArquivo, banco);
                        SYS_ArquivoBO.Delete(entityArquivo, banco);
                    }
                }
            }

            // Valida dados se o aluno estiver marcado como Faltoso.
            ValidaFaltoso(entity, banco);

            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO
            {
                _Banco = banco
            };

            return dao.Salvar(entity);
        }

        /// <summary>
        /// Override do Save passando o banco - se a entidade for nova, faz uma verificação
        /// se já existe uma entidade para a turma / aluno / avaliação. Se já existir,
        /// seta os valores e altera o registro que já existe, ao invés de inserir.
        /// Não pode salvar mais de uma avaliação por aluno na turma.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns>Se salvou com sucesso</returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurma entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
                throw new ValidationException(UtilBO.ErrosValidacao(entity));

            CLS_AlunoAvaliacaoTurma entAux;

            if (entity.IsNew)
            {
                // Se for nova entidade - verificar se já não existe um registro com a chave
                // do lançamento de notas para esse aluno - se já foi lançado, deve
                // ser alterado o registro para não duplicar.
                entAux = GetEntityBy_ChaveAvaliacaoAluno
                    (
                        entity.tur_id
                        , entity.alu_id
                        , entity.mtu_id
                        , entity.fav_id
                        , entity.ava_id
                        , banco
                    );
            }
            else
            {
                entAux = new CLS_AlunoAvaliacaoTurma
                {
                    tur_id = entity.tur_id
                    ,
                    alu_id = entity.alu_id
                    ,
                    mtu_id = entity.mtu_id
                    ,
                    aat_id = entity.aat_id
                };
                GetEntity(entAux, banco);
            }

            if (!entAux.IsNew)
            {
                // Seta o id da entidade auxiliar, que já existe no banco.
                entity.IsNew = entAux.IsNew;
                entity.aat_id = entAux.aat_id;

                // Verifica se existe algum arquivo de relatório para substituir o registro antigo.
                if (entAux.arq_idRelatorio > 0)
                {
                    // Se for diferente do atual
                    if (entAux.arq_idRelatorio != entity.arq_idRelatorio)
                    {
                        // Exclui do banco o registro anterior.
                        SYS_Arquivo entityArquivo = new SYS_Arquivo { arq_id = entAux.arq_idRelatorio };

                        //SYS_ArquivoBO.GetEntity(entityArquivo, banco);
                        SYS_ArquivoBO.Delete(entityArquivo, banco);
                    }
                }
            }

            // Valida dados se o aluno estiver marcado como Faltoso.
            ValidaFaltoso(entity, banco);

            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO
            {
                _Banco = banco
            };

            return dao.Salvar(entity);
        }

        /// <summary>
        /// Retorna a soma da quantidade de tempos de aula da turma/disciplina selecionada
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tipoLancamento">1 - Aulas planejadas / 2 - Período / 3 - Mensal / 4 - Aulas planejadas e mensal</param>
        /// <param name="fav_calculoQtdeAulasDadas">1 - Automático / 2 - Manual</param>
        /// <returns></returns>
        public static long CalculaQtdeTemposAula
        (
            long tur_id
            , int tpc_id
            , long tud_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
        )
        {
            CLS_AlunoAvaliacaoTurmaDAO dao = new CLS_AlunoAvaliacaoTurmaDAO();
            return dao.CalculaQtdeTemposAula(tur_id, tpc_id, tud_id, tipoLancamento, fav_calculoQtdeAulasDadas);
        }

        /// <summary>
        /// Atualiza nota final, frequência e resultado de matrícula do aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="anoLetivoHistorico">Ano da matrícula.</param>
        /// <param name="banco">Transação.</param>
        /// <param name="mtu_resultado">Resultado final globla do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public static bool AtualizarNotaFrequenciaMatriculaHistorico(long alu_id, int mtu_id, int anoLetivoHistorico, TalkDBTransaction banco, byte mtu_resultado, bool permiteAlterarResultado)
        {
            return MTR_MatriculaTurmaBO.CalcularNotaFrequenciaMatriculaAnoAnterior(alu_id, mtu_id, banco, mtu_resultado, permiteAlterarResultado) &&
                   ACA_AlunoHistoricoBO.CalcularNotaFrequenciaHistorico(alu_id, mtu_id, anoLetivoHistorico, banco, mtu_resultado, permiteAlterarResultado);
        }

        /// <summary>
        /// Atualiza nota final, frequência e resultado de matrícula do aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="anoLetivoHistorico">Ano da matrícula.</param>
        /// <param name="mtu_resultado">Resultado final globla do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public static bool AtualizarNotaFrequenciaMatriculaHistorico(long alu_id, int mtu_id, int anoLetivoHistorico, byte mtu_resultado = 0, bool permiteAlterarResultado = false)
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return AtualizarNotaFrequenciaMatriculaHistorico(alu_id, mtu_id, anoLetivoHistorico, banco, mtu_resultado, permiteAlterarResultado);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        #endregion Saves

        #region Validações

        /// <summary>
        /// Caso o aluno esteja marcado como faltoso, verifica se o aluno possui alguma nota lançada
        /// para alguma disciplina, nessa avaliação.
        /// </summary>
        /// <param name="entity">Entidade de avaliação na turma</param>
        /// <param name="banco">Transação com banco</param>
        private static void ValidaFaltoso(CLS_AlunoAvaliacaoTurma entity, TalkDBTransaction banco)
        {
            if (entity.aat_faltoso)
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAvaliacoes =
                  CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaEfetivacaoDisciplinasOferecidas_Aluno(entity.alu_id, entity.mtu_id, entity.fav_id, entity.ava_id, banco);

                foreach (CLS_AlunoAvaliacaoTurmaDisciplina entAvaliacao in listaAvaliacoes)
                {
                    // Valida se existe avaliação lançada para ele em alguma disciplina.

                    if ((!entAvaliacao.IsNew) &&
                        (!string.IsNullOrEmpty(entAvaliacao.atd_avaliacao)))
                    {
                        ACA_Aluno aluno = new ACA_Aluno
                        {
                            alu_id = entity.alu_id
                        };
                        ACA_AlunoBO.GetEntity(aluno, banco);

                        PES_Pessoa pessoa = new PES_Pessoa
                        {
                            pes_id = aluno.pes_id
                        };
                        PES_PessoaBO.GetEntity(pessoa);

                        throw new ValidationException("O aluno \"" + pessoa.pes_nome +
                               "\" é faltoso e não deve possuir nota lançada nos(as) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + ", para essa avaliação.");
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se as regras do curso estão sendo cumpridas.
        /// Quando o regime de matrícula é Seriado por avaliações, o formato tem que
        /// ser do tipo Conceito Global e a avaliação selecionada tem que ser do tipo
        /// Periódica ou Periódica + Final.
        /// </summary>
        /// <param name="entTurma"></param>
        /// <param name="entFormatoAvaliacao"></param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="entCurriculoPeriodo">Entidade de CurriculoPeriodo da turma (a primeira da lista)</param>
        /// <param name="Seriado">Retorna se o regime de matrícula é Seriado por avaliações.</param>
        public static bool ValidaRegrasCurso(TUR_Turma entTurma, ACA_FormatoAvaliacao entFormatoAvaliacao, int ava_id, out ACA_CurriculoPeriodo entCurriculoPeriodo, out bool Seriado)
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_id, banco, out entCurriculoPeriodo, out Seriado);
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

        /// <summary>
        /// Verifica se as regras do curso estão sendo cumpridas.
        /// Quando o regime de matrícula é Seriado por avaliações, o formato tem que
        /// ser do tipo Conceito Global e a avaliação selecionada tem que ser do tipo
        /// Periódica ou Periódica + Final.
        /// </summary>
        /// <param name="entTurma"></param>
        /// <param name="entFormatoAvaliacao"></param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        public static bool ValidaRegrasCurso(TUR_Turma entTurma, ACA_FormatoAvaliacao entFormatoAvaliacao, int ava_id, TalkDBTransaction banco)
        {
            ACA_CurriculoPeriodo entCurriculoPeriodo;
            bool Seriado;
            return ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_id, banco, out entCurriculoPeriodo, out Seriado);
        }

        /// <summary>
        /// Verifica se as regras do curso estão sendo cumpridas.
        /// Quando o regime de matrícula é Seriado por avaliações, o formato tem que
        /// ser do tipo "Conceito Global" ou "Global + Disciplina" e a avaliação selecionada
        /// tem que ser do tipo Periódica ou Periódica + Final.
        /// </summary>
        /// <param name="entTurma"></param>
        /// <param name="EntFormatoAvaliacao"></param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="entCurriculoPeriodo">Entidade de CurriculoPeriodo da turma (a primeira da lista)</param>
        /// <param name="Seriado">Retorna se o regime de matrícula é Seriado por avaliações.</param>
        public static bool ValidaRegrasCurso(TUR_Turma entTurma, ACA_FormatoAvaliacao EntFormatoAvaliacao, int ava_id, TalkDBTransaction banco, out ACA_CurriculoPeriodo entCurriculoPeriodo, out bool Seriado)
        {
            Seriado = false;

            List<TUR_TurmaCurriculo> listCurriculos =
                TUR_TurmaCurriculoBO.GetSelectBy_Turma(entTurma.tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

            if (listCurriculos.Count == 0)
                throw new Exception("A turma (tur_id: " + entTurma.tur_id + ") não possui nenhum curriculoPeriodo cadastrado.");

            ACA_Curriculo entCurriculo = new ACA_Curriculo
            {
                cur_id = listCurriculos[0].cur_id
                ,
                crr_id = listCurriculos[0].crr_id
            };
            ACA_CurriculoBO.GetEntity(entCurriculo, banco);

            // Se curso for seriado por avaliações - EJA.
            if (entCurriculo.crr_regimeMatricula ==
                (byte)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                Seriado = true;

                if ((EntFormatoAvaliacao.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.ConceitoGlobal) &&
                    (EntFormatoAvaliacao.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.GlobalDisciplina))
                {
                    // Curso do EJA não pode efetivar notas por disciplina - não possui ligação
                    // com lançamento por disciplina.
                    throw new ValidationException("O formato de avaliação \"" + EntFormatoAvaliacao.fav_nome +
                                                  "\" deve ser do tipo \"Conceito global\" ou " +
                                                  "\"Conceito global e nota por " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + "\", " +
                                                  "pois o curso da turma é seriado por avaliações.");
                }

                ACA_Avaliacao entAvaliacao = new ACA_Avaliacao
                {
                    fav_id = entTurma.fav_id
                    ,
                    ava_id = ava_id
                };
                ACA_AvaliacaoBO.GetEntity(entAvaliacao, banco);

                if ((entAvaliacao.ava_tipo != (byte)AvaliacaoTipo.Periodica) &&
                    (entAvaliacao.ava_tipo != (byte)AvaliacaoTipo.PeriodicaFinal))
                {
                    // Curso do EJA só pode efetivar em avaliações que sejam periódia ou periódica +
                    // final.
                    throw new ValidationException("O formato de avaliação \"" + EntFormatoAvaliacao.fav_nome +
                                                  "\" possui avaliações que não são do tipo Periódica ou Periódica + Final.");
                }
            }

            entCurriculoPeriodo = new ACA_CurriculoPeriodo
            {
                cur_id = entCurriculo.cur_id
                ,
                crr_id = entCurriculo.crr_id
                ,
                crp_id = listCurriculos[0].crp_id
            };
            ACA_CurriculoPeriodoBO.GetEntity(entCurriculoPeriodo, banco);

            return true;
        }

        #endregion Validações
    }
}