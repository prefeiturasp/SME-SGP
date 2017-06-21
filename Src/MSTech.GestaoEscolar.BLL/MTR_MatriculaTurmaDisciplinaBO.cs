using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DevExpress.Data.Linq;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da disciplina na matrícula turma do aluno
    /// </summary>
    public enum MTR_MatriculaTurmaDisciplinaSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
        ,
        EmMatricula = 4
        ,
        Inativo = 5
        ,
        Efetivado = 6
    }

    /// <summary>
    /// Resultado da avaliação geral do aluno na turmaDisciplina.
    /// </summary>
    public enum MtrTurmaDisciplinaResultado : byte
    {
        Aprovado = 1
        ,
        Reprovado = 2
        ,
        ReprovadoFrequencia = 8
        ,
        RecuperacaoFinal = 9
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Estrutura utilizada para guardar as notas finais dos alunos
    /// </summary>
    public struct NotaFinalAlunoTurmaDisciplina
    {
        /// <summary>
        /// Id do aluno.
        /// </summary>
        public long alu_id;

        /// <summary>
        /// Id da matrícula do aluno.
        /// </summary>
        public int mtu_id;

        /// <summary>
        /// Id da matrícula turma disciplina do aluno.
        /// </summary>
        public int mtd_id;

        /// <summary>
        /// Nota do aluno
        /// </summary>
        public string nota;
    }

    /// <summary>
    /// Estrutura usada para transferir efetivação na movimentação.
    /// </summary>
    public class MTR_MatriculaTurDis_Efetivacao
    {
        public MTR_MatriculaTurmaDisciplina entity;
        public int tds_id;
    }

    /// <summary>
    /// Estrutura utilizada para guardar a média das notas dos alunos
    /// </summary>
    public struct MediaNotaAlunos
    {
        /// <summary>
        /// Id do aluno.
        /// </summary>
        public long alu_id;

        /// <summary>
        /// Média do aluno
        /// </summary>
        public decimal media;
    }

    /// <summary>
    /// Estrutura utilizada para alterar o resultado das disciplinas no lançamento de notas de anos anteriores.
    /// </summary>
    public struct MatriculaTurmaDisciplinaResultado
    {
        public long alu_id;
        public int mtu_id;
        public int mtd_id;
        public byte mtd_resultado;
    }


    /// <summary>
    /// Estrutura utilizada para alterar o resultado das disciplinas no lançamento de notas de anos anteriores.
    /// </summary>
    public struct AlunosTurmaDisciplina
    {
        public string pes_nome { get; set; }
        public DateTime pes_dataNascimento { get; set; }
        public string NomeMae { get; set; }
        public string pes_nome_abreviado { get; set; }
        public DateTime alu_dataCriacao { get; set; }
        public DateTime alu_dataAlteracao { get; set; }
        public long alu_id { get; set; }
        public byte alu_situacaoID { get; set; }
        public long? arq_idFoto { get; set; }
        public long mtd_id { get; set; }
        public long mtu_id { get; set; }
        public int? mtd_numeroChamada { get; set; }
        public string numeroChamada { get; set; }
        public DateTime mtd_dataMatricula { get; set; }
        public DateTime? mtd_dataSaida { get; set; }
        public byte mtd_situacao { get; set; }
        public int? ava_id { get; set; }
        public bool PossuiDeficienciaDisciplina { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public string Nome { get; set; }
        public string alu_mtu_mtd_id { get; set; }
        public bool AlunoDispensado { get; set; }
        public DateTime dataAlteracao { get; set; }
        public byte mtu_situacao { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação padrão em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoDisciplinaPadrao
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtu_idAnterior { get; set; }
        public int mtd_id { get; set; }
        public int mtd_idAnterior { get; set; }
        public long tud_id { get; set; }
        public long tur_id { get; set; }
        public string tur_codigo { get; set; }
        public string alc_matricula { get; set; }
        public long tud_idPrincipal { get; set; }
        public int mtd_idPrincipal { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public bool atd_semProfessor { get; set; }
        public decimal Frequencia { get; set; }
        public int QtFaltasAluno { get; set; }
        public int QtAulasAluno { get; set; }
        public int QtFaltasAlunoReposicao { get; set; }
        public string pes_nome { get; set; }
        public string pes_dataNascimento { get; set; }
        public string mtd_numeroChamada { get; set; }
        public string id { get; set; }
        public decimal frequenciaAcumulada { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public int alc_id { get; set; }
        public int ala_id { get; set; }
        public int tca_numeroAvaliacao { get; set; }
        public byte situacaoMatriculaAluno { get; set; }
        public DateTime dataMatricula { get; set; }
        public DateTime dataSaida { get; set; }
        public int ausenciasCompensadas { get; set; }
        public bool ala_avaliado { get; set; }
        public string AvaliacaoSalaRecurso { get; set; }
        public string AvaliacaoAdicional { get; set; }
        public bool faltoso { get; set; }
        public bool naoAvaliado { get; set; }
        public int dispensadisciplina { get; set; }
        public bool observacaoPreenchida { get; set; }
        public bool observacaoConselhoPreenchida { get; set; }
        public string avaliacaoPosConselho { get; set; }
        public string justificativaPosConselho { get; set; }
        public decimal FrequenciaFinalAjustada { get; set; }
        public byte mtu_resultado { get; set; }
        public int QtAulasEfetivado { get; set; }
        public bool recuperacaoPorNota { get; set; }
        public bool recuperacaoPorFrequencia { get; set; }
        public int faltasAnteriores { get; set; }
        public int compensadasAnteriores { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação padrão de componentes da regência em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoPadraoComponenteRegencia
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public bool atd_semProfessor { get; set; }
        public string id { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public int dispensadisciplina { get; set; }
        public string avaliacaoPosConselho { get; set; }
        public string justificativaPosConselho { get; set; }
        public string dis_nome { get; set; }
        public bool faltoso { get; set; }
        public bool naoAvaliado { get; set; }
        public long tur_id { get; set; }
        public int mtu_idAnterior { get; set; }
        public int mtd_idAnterior { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação final em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoDisciplinaFinal
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public string alc_matricula { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public decimal Frequencia { get; set; }
        public string pes_nome { get; set; }
        public string pes_dataNascimento { get; set; }
        public string mtd_numeroChamada { get; set; }
        public string id { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public byte situacaoMatriculaAluno { get; set; }
        public DateTime dataMatricula { get; set; }
        public DateTime dataSaida { get; set; }
        public decimal FrequenciaFinalAjustada { get; set; }
        public int tpc_id { get; set; }
        public string NomeAvaliacao { get; set; }
        public string AvaliacaoPosConselho { get; set; }
        public bool observacaoConselhoPreenchida { get; set; }
        public int SemNota { get; set; }
        public int ava_id { get; set; }
        public int UltimoPeriodo { get; set; }
        public int QtFaltasAluno { get; set; }
        public int QtAulasAluno { get; set; }
        public int ausenciasCompensadas { get; set; }
        public byte mtu_resultado { get; set; }
        public int AlunoForaDaRede { get; set; }
        public int QtAulasEfetivado { get; set; }
        public int tpc_ordem { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação final de componentes da regência em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoFinalComponenteRegencia
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public string id { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public string dis_nome { get; set; }
        public int tpc_id { get; set; }
        public string NomeAvaliacao { get; set; }
        public string AvaliacaoPosConselho { get; set; }
        public int SemNota { get; set; }
        public int ava_id { get; set; }
        public int UltimoPeriodo { get; set; }
        public int AlunoForaDaRede { get; set; }
        public int tpc_ordem { get; set; }
        public string Frequencia { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna do fechamento automatico em cache.
    /// </summary>
    [Serializable]
    public struct AlunosFechamentoPadrao
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public long tur_id { get; set; }
        public string tur_codigo { get; set; }
        public string alc_matricula { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public int QtAulasAluno { get; set; }
        public int QtAulasAlunoReposicao { get; set; }
        public int QtFaltasAluno { get; set; }
        public int QtFaltasAlunoReposicao { get; set; }
        public string pes_nome { get; set; }
        public string mtd_numeroChamada { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public byte situacaoMatriculaAluno { get; set; }
        public DateTime dataMatricula { get; set; }
        public DateTime dataSaida { get; set; }
        public int ausenciasCompensadas { get; set; }
        public int dispensadisciplina { get; set; }
        public byte observacaoConselhoPreenchida { get; set; }
        public string avaliacaoPosConselho { get; set; }
        public decimal Frequencia { get; set; }
        public decimal FrequenciaFinalAjustada { get; set; }
        public byte mtu_resultado { get; set; }
        public int QtAulasEfetivado { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna do fechamento automatico de componentes da regência em cache.
    /// </summary>
    [Serializable]
    public struct AlunosFechamentoPadraoComponenteRegencia
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public int dispensadisciplina { get; set; }
        public string avaliacaoPosConselho { get; set; }
        public string dis_nome { get; set; }
        public long tur_id { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação final em cache.
    /// </summary>
    [Serializable]
    public struct AlunosFechamentoFinal
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public string alc_matricula { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public decimal Frequencia { get; set; }
        public string pes_nome { get; set; }
        public string mtd_numeroChamada { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public byte situacaoMatriculaAluno { get; set; }
        public DateTime dataMatricula { get; set; }
        public DateTime dataSaida { get; set; }
        public decimal FrequenciaFinalAjustada { get; set; }
        public int tpc_id { get; set; }
        public string NomeAvaliacao { get; set; }
        public string AvaliacaoPosConselho { get; set; }
        public byte observacaoConselhoPreenchida { get; set; }
        public int SemNota { get; set; }
        public int ava_id { get; set; }
        public int UltimoPeriodo { get; set; }
        public int QtAulasAluno { get; set; }
        public int QtAulasAlunoReposicao { get; set; }
        public int QtFaltasAluno { get; set; }
        public int QtFaltasAlunoReposicao { get; set; }
        public int ausenciasCompensadas { get; set; }
        public byte mtu_resultado { get; set; }
        public int AlunoForaDaRede { get; set; }
        public int QtAulasEfetivado { get; set; }
        public int tpc_ordem { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public byte alu_situacaoID { get; set; }
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação final de componentes da regência em cache.
    /// </summary>
    [Serializable]
    public struct AlunosFechamentoFinalComponenteRegencia
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public int AvaliacaoID { get; set; }
        public string Avaliacao { get; set; }
        public byte AvaliacaoResultado { get; set; }
        public string atd_relatorio { get; set; }
        public long arq_idRelatorio { get; set; }
        public string dis_nome { get; set; }
        public int tpc_id { get; set; }
        public string NomeAvaliacao { get; set; }
        public string AvaliacaoPosConselho { get; set; }
        public int SemNota { get; set; }
        public int ava_id { get; set; }
        public int UltimoPeriodo { get; set; }
        public int AlunoForaDaRede { get; set; }
        public int tpc_ordem { get; set; }
    }

    #endregion

    public class MTR_MatriculaTurmaDisciplinaBO : BusinessBase<MTR_MatriculaTurmaDisciplinaDAO, MTR_MatriculaTurmaDisciplina>
    {
        #region Cache

        public const string Cache_SelecionaAlunosAtivosCOCPorTurmaDisciplina = "Cache_SelecionaAlunosAtivosCOCPorTurmaDisciplina";

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , int fav_id
            , int ava_id
            , string tur_ids
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_BIMESTRE_MODEL_KEY
                , tud_id
                , fav_id
                , ava_id
                , tur_ids);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento para disciplinas especiais.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , int fav_id
            , int ava_id
            , string tur_ids
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY
                , tud_id
                , fav_id
                , ava_id
                , tur_ids);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento para componentes da regência.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY
                    , tur_id
                    , fav_id
                    , ava_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento (Recuperação Final)
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
        (
            long tud_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_RECUPERACAO_FINAL_MODEL_KEY
                    , tud_id
                    , fav_id
                    , ava_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento (Recuperação Final especial)
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_RECUPERACAO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY
                    , tud_id
                    , fav_id
                    , ava_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento final
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal
        (
            long tud_id
            , int fav_id
            , int ava_id
            , string tur_ids
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_FINAL_MODEL_KEY
                , tud_id
                , fav_id
                , ava_id
                , tur_ids);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento final de disciplinas especiais.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia
        (
            long tud_id
            , int fav_id
            , int ava_id
            , string tur_ids
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY
                , tud_id
                , fav_id
                , ava_id
                , tur_ids);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento final para componentes da regência.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                ModelCache.FECHAMENTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY
                , tur_id
                , fav_id
                , ava_id);
        }

        #endregion Cache

        #region Consultas

        /// <summary>
        /// Retorna as turmas eletivas do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <returns>Turmas eletivas.</returns>
        public static DataTable PesquisarTurmasEletivas
        (
            long alu_id
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.PesquisarTurmasEletivas(alu_id);
        }

        /// <summary>
        /// Calcula a média das notas do aluno por turma e disciplina
        /// </summary>
        /// <param name="tur_id">Id do turma</param>
        /// <param name="tud_id">Id da disciplina da turma</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <returns>Nota média dos alunos da turma</returns>
        public static List<MediaNotaAlunos> CalculaNota_Media_PorTurmaDisciplina
        (
            long tur_id
            , long tud_id
            , int tpc_id
        )
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().CalculaNota_Media_PorTurmaDisciplina(tur_id, tud_id, tpc_id).Rows.Cast<DataRow>().Select(dr =>
                    new MediaNotaAlunos
                    {
                        alu_id = Convert.ToInt64(dr["alu_id"]),
                        media = Convert.ToDecimal(dr["Media"]),
                    }).ToList();
        }

        /// <summary>
        /// Verifica as  MatriculaTurmaDisciplina cadastradas para um tud_id de uma turma do 
        /// tipo 2-Eletiva do aluno, e retorna os tur_ids das turmas eletivas.
        /// </summary>
        /// <param name="entMatriculaTurma">Entidade da MatriculaTurma</param>
        /// <param name="bancoGestao">Tranasação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        private static List<long> RetornaTurmaAtivaBy_TurmaEletivaAluno(MTR_MatriculaTurma entMatriculaTurma, TalkDBTransaction bancoGestao)
        {
            List<long> lista = new List<long>();
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = bancoGestao };

            DataTable dt = dao.SelectTurmasBy_TurmaEletivaAluno(entMatriculaTurma.alu_id, entMatriculaTurma.mtu_id);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(Convert.ToInt64(dr["tur_id"]));
            }

            return lista;
        }

        /// <summary>
        /// Verifica as  MatriculaTurmaDisciplina cadastradas para um tud_id de uma turma do 
        /// tipo 2-Eletiva do aluno, e retorna os tud_ids das turmas eletivas.
        /// </summary>
        /// <param name="entMatriculaTurma">Entidade da MatriculaTurma</param>
        /// <param name="bancoGestao">Tranasação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        private static List<long> RetornaTurmaDisciplinaAtivaBy_TurmaEletivaAluno(MTR_MatriculaTurma entMatriculaTurma, TalkDBTransaction bancoGestao)
        {
            List<long> lista = new List<long>();
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = bancoGestao };

            DataTable dt = dao.SelecionaTurmaDisciplinaEletiva_MatriculaAtiva(entMatriculaTurma.alu_id, entMatriculaTurma.mtu_id);

            foreach (DataRow dr in dt.Rows)
                lista.Add(Convert.ToInt64(dr["tud_id"]));

            return lista;
        }

        /// <summary>
        /// Retorna a turma do tipo 4-Multisseriada do docente com alunos ativos,
        /// que estejam relacionados com a MatriculaTurma passada por parametro.
        /// </summary>
        /// <param name="entMatriculaTurma">Entidade da MatriculaTurma</param>
        /// <returns></returns>
        public static string RetornaTurmaDisciplinaAtivaBy_TurmaMultisseriadaDocenteAluno(MTR_MatriculaTurma entMatriculaTurma)
        {
            string retorno = string.Empty;
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            DataTable dt = dao.SelecionaTurmaDisciplinaMultisseriadaDocente_MatriculaAtiva(entMatriculaTurma.alu_id, entMatriculaTurma.mtu_id);
            foreach (DataRow dr in dt.Rows)
            {
                retorno += string.IsNullOrEmpty(retorno) ? dr["tur_codigo"].ToString() : string.Format(", {0}", dr["tur_codigo"].ToString());
            }
            return retorno;
        }

        /// <summary>
        /// Verifica se o aluno está matriculado no tud_id informado
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="mtu_id">ID da matrícula na turma disciplina</param>
        /// <returns>True - Aluno está matriculado / False - Aluno não está matriculado</returns>
        public static bool VerificaAlunoPertenceTurmaEletiva(long alu_id, int tud_id, int mtu_id)
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();

            DataTable dt = dao.SelectTurmasDisciplinasBy_TurmaEletivaAluno(alu_id, mtu_id);

            foreach (DataRow dr in dt.Rows)
            {
                int tud_idRow = Convert.ToInt32(dr["tud_id"]);
                if (tud_idRow == tud_id)
                    return true;
            }
            return false;

        }

        /// <summary>
        /// Retorna a média de todos os alunos na disciplina e no período informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="tipoEscalaAvaliacaoAdicional">Tipo de escala da avaliação adicional</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public static DataTable CalculaMediaAvaliacaoAdicionalTodosAlunos
        (
            long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaAvaliacaoAdicional
            , byte tipoEscalaDocente
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.CalculaMediaAvaliacaoAdicional_TodosAlunos(tud_id, fav_id, tpc_id, tipoEscalaAvaliacaoAdicional, tipoEscalaDocente);
        }

        /// <summary>
        /// Retorna a média do aluno na disciplina e no período informados.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula na turma</param>
        /// <param name="mtd_id">ID da matricula na turma disciplina</param>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="tipoEscalaAvaliacaoAdicional">Tipo de escala da avaliação adicional</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public static decimal CalculaMediaAvaliacaoAdicionalPorAluno
        (
            long alu_id
            , int mtu_id
            , int mtd_id
            , long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaAvaliacaoAdicional
            , byte tipoEscalaDocente
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.CalculaMediaAvaliacaoAdicional_Alunos(alu_id, mtu_id, mtd_id, tud_id, fav_id, tpc_id, tipoEscalaAvaliacaoAdicional, tipoEscalaDocente);
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaPadrao> GetSelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , Guid ent_id
            , byte tud_tipo
            , int tpc_ordem
            , decimal fav_variacao
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosEfetivacaoDisciplinaPadrao> dados = null;

            Func<List<AlunosEfetivacaoDisciplinaPadrao>> retorno = delegate()
            {
                DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                if (listaTur_ids != null)
                {
                    listaTur_ids.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurma.NewRow();
                                dr["tur_id"] = p;
                                dtTurma.Rows.Add(dr);
                            }
                        );
                }

                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscPeriodoFormato
                                        (
                                        tud_id
                                        , tur_id
                                        , tpc_id
                                        , ava_id
                                        , ordenacao
                                        , fav_id
                                        , tipoAvaliacao
                                        , esa_id
                                        , tipoEscalaDisciplina
                                        , tipoEscalaDocente
                                        , avaliacaoesRelacionadas
                                        , notaMinimaAprovacao
                                        , ordemParecerMinimo
                                        , tipoLancamento
                                        , fav_calculoQtdeAulasDadas
                                        , permiteAlterarResultado
                                        , tur_tipo
                                        , cal_id
                                        , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id)
                                        , tud_tipo
                                        , tpc_ordem
                                        , fav_variacao
                                        , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id)
                                        , dtTurma
                                        , documentoOficial
                                        ))
                {
                    return dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, fav_id, ava_id, listaTur_ids.Any() ? string.Join(";", listaTur_ids.ToArray()) : "-1");

                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheFechamento
                    );
            }
            else
            {
                dados = retorno();
            }

            dados.Sort(
                delegate(AlunosEfetivacaoDisciplinaPadrao p1, AlunosEfetivacaoDisciplinaPadrao p2)
                {
                    return ordenacao == 0 ? Convert.ToInt32(string.IsNullOrEmpty(p1.mtd_numeroChamada) || p1.mtd_numeroChamada == "-" ? "999" : p1.mtd_numeroChamada)
                                                .CompareTo(Convert.ToInt32(string.IsNullOrEmpty(p2.mtd_numeroChamada) || p2.mtd_numeroChamada == "-" ? "999" : p2.mtd_numeroChamada))
                                            : p1.pes_nome.CompareTo(p2.pes_nome);
                });
            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaPadrao> GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , EnumTipoDocente tipoDocente
            , Guid ent_id
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosEfetivacaoDisciplinaPadrao> dados = null;

            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (listaTur_ids != null)
            {
                listaTur_ids.ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurma.NewRow();
                            dr["tur_id"] = p;
                            dtTurma.Rows.Add(dr);
                        }
                    );
            }

            if (appMinutosCacheFechamento > 0)
            {
                    string chave = RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
                                    (
                                         tud_id
                                        , fav_id
                                        , ava_id
                                        , listaTur_ids.Any() ? string.Join(";", listaTur_ids.ToArray()) : "-1"
                                    );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscPeriodoFormatoFiltroDeficiencia
                                                (
                                                 tud_id
                                                , tur_id
                                                , tpc_id
                                                , ava_id
                                                , ordenacao
                                                , fav_id
                                                , tipoAvaliacao
                                                , esa_id
                                                , tipoEscalaDisciplina
                                                , tipoEscalaDocente
                                                , avaliacaoesRelacionadas
                                                , notaMinimaAprovacao
                                                , ordemParecerMinimo
                                                , tipoLancamento
                                                , fav_calculoQtdeAulasDadas
                                                , permiteAlterarResultado
                                                , tur_tipo
                                                , cal_id
                                                , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id)
                                                , (byte)tipoDocente
                                                , dtTurma
                                                , documentoOficial
                                                ))
                        {
                                        return dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
                        }
                                },
                                appMinutosCacheFechamento
                            );
                    }
                    else
                    {
                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscPeriodoFormatoFiltroDeficiencia
                                                (
                                                 tud_id
                                                , tur_id
                                                , tpc_id
                                                , ava_id
                                                , ordenacao
                                                , fav_id
                                                , tipoAvaliacao
                                                , esa_id
                                                , tipoEscalaDisciplina
                                                , tipoEscalaDocente
                                                , avaliacaoesRelacionadas
                                                , notaMinimaAprovacao
                                                , ordemParecerMinimo
                                                , tipoLancamento
                                                , fav_calculoQtdeAulasDadas
                                                , permiteAlterarResultado
                                                , tur_tipo
                                                , cal_id
                                                , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id)
                                                , (byte)tipoDocente
                                                , dtTurma
                                                , documentoOficial
                                                ))
                {
                    dados = dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
                }
            }

            return ordenacao == 0 ?
                dados.OrderBy(p => p.mtd_numeroChamada == "-" ? 0 : Convert.ToInt32(p.mtd_numeroChamada)).ToList() :
                dados.OrderBy(p => p.pes_nome).ToList();
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para os componentes da regencia
        /// para o período informado, de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoPadraoComponenteRegencia> GetSelect_ComponentesRegencia_By_TurmaFormato
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , bool permiteAlterarResultado
            , byte tur_tipo
            , DataTable alunos
            , Guid ent_id
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoPadraoComponenteRegencia> dados = null;

            Func<List<AlunosEfetivacaoPadraoComponenteRegencia>> retorno = delegate()
            {
                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectComponentesRegenciaBy_TurmaFormato
                                                                                    (
                                                                                    tur_id
                                                                                    , tpc_id
                                                                                    , ava_id
                                                                                    , fav_id
                                                                                    , tipoAvaliacao
                                                                                    , esa_id
                                                                                    , tipoEscalaDisciplina
                                                                                    , tipoEscalaDocente
                                                                                    , avaliacaoesRelacionadas
                                                                                    , notaMinimaAprovacao
                                                                                    , ordemParecerMinimo
                                                                                    , permiteAlterarResultado
                                                                                    , tur_tipo
                                                                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id)
                                                                                    , alunos
                                                                                    ))
                        {
                    return dt.AsEnumerable().AsParallel().AsOrdered().Select(p => (AlunosEfetivacaoPadraoComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoPadraoComponenteRegencia())).ToList();
                        }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato
                                                                                   (
                                                                                   tur_id
                                                                                   , fav_id
                                    , ava_id
                                );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
                {
                dados = retorno();
                }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<NotaFinalAlunoTurmaDisciplina> GetSelect_NotaFinalAluno_By_Turma_Disciplina_Periodo
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , Guid ent_id
            , byte tud_tipo
            , int tpc_ordem
            , decimal fav_variacao
            , bool documentoOficial
            , List<long> listaTur_ids = null
        )
        {
            return GetSelectBy_TurmaDisciplinaPeriodo(tud_id, tur_id, tpc_id, ava_id, ordenacao, fav_id, tipoAvaliacao, esa_id, tipoEscalaDisciplina, tipoEscalaDocente, avaliacaoesRelacionadas, notaMinimaAprovacao, ordemParecerMinimo, tipoLancamento, fav_calculoQtdeAulasDadas, permiteAlterarResultado, tur_tipo, cal_id, ent_id, tud_tipo, tpc_ordem, fav_variacao, documentoOficial, 0, listaTur_ids)
                .Where(p => !p.atd_semProfessor)
                .Select(p =>
                new NotaFinalAlunoTurmaDisciplina
                    {
                        alu_id = p.alu_id,
                        mtu_id = p.mtu_id,
                        mtd_id = p.mtd_id,
                        nota = p.Avaliacao,
                    }).ToList();
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<NotaFinalAlunoTurmaDisciplina> GetSelect_NotaFinalAluno_By_Turma_Disciplina_PeriodoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , EnumTipoDocente tipoDocente
            , Guid ent_id
            , bool documentoOficial
            , List<long> listaTur_ids = null

        )
        {
            return GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, tur_id, tpc_id, ava_id, ordenacao, fav_id, tipoAvaliacao, esa_id, tipoEscalaDisciplina, tipoEscalaDocente, avaliacaoesRelacionadas, notaMinimaAprovacao, ordemParecerMinimo, tipoLancamento, fav_calculoQtdeAulasDadas, permiteAlterarResultado, tur_tipo, cal_id, tipoDocente, ent_id, documentoOficial, 0, listaTur_ids)
                .Where(p => !p.atd_semProfessor)
                .Select(p =>
                new NotaFinalAlunoTurmaDisciplina
                {
                    alu_id = p.alu_id,
                    mtu_id = p.mtu_id,
                    mtd_id = p.mtd_id,
                    nota = p.Avaliacao,
                }).ToList();
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para os componentes da regencia
        /// para o período informado, de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="alunos">Lista de alunos previamente filtradas da regencia</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<NotaFinalAlunoTurmaDisciplina> GetSelect_NotaFinalAluno_ComponentesRegencia_By_TurmaFormato
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , bool permiteAlterarResultado
            , byte tur_tipo
            , DataTable alunos
            , Guid ent_id
        )
        {
            return GetSelect_ComponentesRegencia_By_TurmaFormato(tur_id, tpc_id, ava_id, fav_id, tipoAvaliacao, esa_id, tipoEscalaDisciplina, tipoEscalaDocente, avaliacaoesRelacionadas, notaMinimaAprovacao, ordemParecerMinimo, permiteAlterarResultado, tur_tipo, alunos, ent_id)
                .Where(p => !p.atd_semProfessor)
                .Select(p =>
                new NotaFinalAlunoTurmaDisciplina
                {
                    alu_id = p.alu_id,
                    mtu_id = p.mtu_id,
                    mtd_id = p.mtd_id,
                    nota = p.Avaliacao,
                }).ToList();
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaPadrao> GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoDisciplinaPadrao> dados = null;

            Func<List<AlunosEfetivacaoDisciplinaPadrao>> retorno = delegate()
            {
                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
                                                                                    (
                                                                                    tud_id
                                                                                    , tur_id
                                                                                    , tpc_id
                                                                                    , ava_id
                                                                                    , ordenacao
                                                                                    , fav_id
                                                                                    , esa_id
                                                                                    , tipoEscalaDisciplina
                                                                                    , tipoEscalaDocente
                                                                                    , avaliacaoesRelacionadas
                                                                                    , notaMinimaAprovacao
                                                                                    , ordemParecerMinimo
                                                                                    , tipoLancamento
                                                                                    , fav_calculoQtdeAulasDadas
                                                                                    , documentoOficial
                                                                                    ))
                        {
                    return dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
                        }
            };

            if (appMinutosCacheFechamento > 0)
            {
                
                    string chave = RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
                                                                                     (
                                                                                     tud_id
                                                                                     , fav_id
                                        , ava_id
                                    );

                    dados = CacheManager.Factory.Get
                                (
                                    chave,
                                    retorno,
                                    appMinutosCacheFechamento
                                );
            }
            else
                {
                dados = retorno();
            }

            return ordenacao == 0 ?
                dados.OrderBy(p => p.mtd_numeroChamada == "-" ? 0 : Convert.ToInt32(p.mtd_numeroChamada)).ToList() :
                dados.OrderBy(p => p.pes_nome).ToList();
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        ///	Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaPadrao> GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , EnumTipoDocente tipoDocente
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoDisciplinaPadrao> dados = null;

            Func<List<AlunosEfetivacaoDisciplinaPadrao>> retorno = delegate()
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
                                                                                    (
                                                                                    tud_id
                                                                                    , tur_id
                                                                                    , tpc_id
                                                                                    , ava_id
                                                                                    , ordenacao
                                                                                    , fav_id
                                                                                    , esa_id
                                                                                    , tipoEscalaDisciplina
                                                                                    , tipoEscalaDocente
                                                                                    , avaliacaoesRelacionadas
                                                                                    , notaMinimaAprovacao
                                                                                    , ordemParecerMinimo
                                                                                    , tipoLancamento
                                                                                    , fav_calculoQtdeAulasDadas
                                                                                    , (byte)tipoDocente
                                                                                    , documentoOficial
                                                                                    ))
                        {
                            return dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
            }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
                                                                                     (
                                                                                     tud_id
                                        , fav_id
                                                                                     , ava_id
                                    );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
                {
                dados = retorno();
            }

            return ordenacao == 0 ?
                dados.OrderBy(p => p.mtd_numeroChamada == "-" ? 0 : Convert.ToInt32(p.mtd_numeroChamada)).ToList() :
                dados.OrderBy(p => p.pes_nome).ToList();
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// (Utilizado na tela de registro de avaliação)
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="cap_dataInicio">Data inicial do período do calendário</param>
        /// <param name="cap_dataFim">Data final do período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>        
        /// <param name="ent_id">ID da entidade</param>    
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivosCOCPorTurmaDisciplina
        (
            long tur_id
            , long tud_id
            , int tpc_id
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
            , int ordenacao
            , Guid ent_id
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelectAtivosCOCByTurmaDisciplina(tur_id, tud_id, tpc_id, cap_dataInicio, cap_dataFim, ordenacao, ent_id);
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="cap_dataInicio">Data inicial do período do calendário</param>
        /// <param name="cap_dataFim">Data final do período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>        
        /// <param name="ent_id">ID da entidade</param>    
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivosCOCPorTurmaDisciplinaFiltroDeficiencia
        (
            long tur_id
            , long tud_id
            , int tpc_id
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
            , EnumTipoDocente tipoDocente
            , int ordenacao
            , Guid ent_id
        )
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().SelectAtivosCOCByTurmaDisciplinaFiltroDeficiencia(tur_id, tud_id, tpc_id, cap_dataInicio, cap_dataFim, (byte)tipoDocente, ordenacao, ent_id);
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// com o total de faltas do aluno e o total de faltas compensadas (se existir)
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>          
        /// <param name="cpa_id">ID da compensacao de ausencia</param>
        /// <param name="filtrarAlunosComFalta">Caso true, filtra para retornar apenas os alunos com faltas a serem compensadas</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivosCompensacaoAusencia
        (
             long tud_id
            , int tpc_id            
            , int ordenacao
            , int cpa_id
            , bool documentoOficial = false
            , bool filtrarAlunosComFalta = false
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelecionaAtivosCompensacaoAusencia(tud_id, tpc_id, ordenacao, cpa_id, documentoOficial, filtrarAlunosComFalta);
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>         
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <param name="cpa_id">ID da compensacao de ausencia</param>
        /// <param name="filtrarAlunosComFalta">Caso true, filtra para retornar apenas os alunos com faltas a serem compensadas</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivosCompensacaoAusenciaFiltroDeficiencia
        (
            long tur_id
            , long tud_id
            , int tpc_id           
            , int ordenacao
            , EnumTipoDocente tipoDocente
            , int cpa_id
            , bool documentoOficial = false
            , bool filtrarAlunosComFalta = false
        )
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().SelecionaAtivosCompensacaoAusenciaFiltroDeficiencia(tur_id, tud_id, tpc_id,  ordenacao, (byte)tipoDocente, cpa_id, documentoOficial, filtrarAlunosComFalta);
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa ou inativa no COC por disciplina
        /// (Utilizado na tela de controle de turmas na lista de alunos)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosTurmaDisciplina> SelecionaAlunosAtivosCOCPorTurmaDisciplina
        (
            long tud_id
            , int tpc_id
            , EnumTipoDocente tipoDocente
            , bool documentoOficial
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
            , int appMinutosCacheLongo = 0
            , string tur_ids = null
        )
        {
            totalRecords = 0;
            List<AlunosTurmaDisciplina> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                string chave = string.Format(ModelCache.ALUNOS_ATIVOS_COC_DISCIPLINA, tud_id, tpc_id, tipoDocente, documentoOficial, cap_dataInicio.Date, cap_dataFim.Date, string.IsNullOrEmpty(tur_ids) ? "-1" : tur_ids);

                dados = CacheManager.Factory.Get
                    (
                        chave,
                        () =>
                        {
                            return SelecionaAlunosAtivosCOCPorTurmaDisciplina(tud_id, tpc_id, tipoDocente, documentoOficial, cap_dataInicio.Date, cap_dataFim.Date, tur_ids);
                        },
                        appMinutosCacheLongo
                    );
            }
            else
            {
                dados = SelecionaAlunosAtivosCOCPorTurmaDisciplina(tud_id, tpc_id, tipoDocente, documentoOficial, cap_dataInicio.Date, cap_dataFim.Date, tur_ids);
            }

            totalRecords = ((List<AlunosTurmaDisciplina>)dados).Count;

            return dados;
        }

        /// <summary>
        /// Retorna todos os alunos com matricula ativa ou inativa no COC por disciplina
        /// (Utilizado na tela de controle de turmas na lista de alunos)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <returns></returns>
        private static List<AlunosTurmaDisciplina> SelecionaAlunosAtivosCOCPorTurmaDisciplina(long tud_id, int tpc_id, EnumTipoDocente tipoDocente, bool documentoOficial, DateTime cap_dataInicio, DateTime cap_dataFim, string tur_ids)
        {
            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
               string[] ltTurIds = tur_ids.Split(';');

               ltTurIds.ToList().ForEach
                    (
                        tur_id =>
                        {
                            DataRow dr = dtTurma.NewRow();
                            dr["tur_id"] = tur_id;
                            dtTurma.Rows.Add(dr);
                        }
                    );
            }

            return new MTR_MatriculaTurmaDisciplinaDAO().SelecionaAlunosAtivosCOCPorTurmaDisciplina(tud_id, tpc_id, dtTurma, documentoOficial, cap_dataInicio, cap_dataFim)
                .Rows.Cast<DataRow>()
                .Where(dr => ((tipoDocente == EnumTipoDocente.Especial) ? Convert.ToBoolean(dr["PossuiDeficienciaDisciplina"]) : !Convert.ToBoolean(dr["PossuiDeficienciaDisciplina"])))
                .Select(dr =>
                new AlunosTurmaDisciplina
                {
                    pes_nome = dr["pes_nome"].ToString()
                    ,
                    pes_dataNascimento = Convert.ToDateTime(dr["pes_dataNascimento"])
                    ,
                    NomeMae = dr["NomeMae"].ToString()
                    ,
                    pes_nome_abreviado = dr["pes_nome_abreviado"].ToString()
                    ,
                    alu_dataCriacao = Convert.ToDateTime(dr["alu_dataCriacao"])
                    ,
                    alu_dataAlteracao = Convert.ToDateTime(dr["alu_dataAlteracao"])
                    ,
                    alu_id = Convert.ToInt64(dr["alu_id"])
                    ,
                    alu_situacaoID = Convert.ToByte(dr["alu_situacaoID"])
                    ,
                    mtd_id = Convert.ToInt64(dr["mtd_id"])
                    ,
                    mtu_id = Convert.ToInt64(dr["mtu_id"])
                    ,
                    mtd_numeroChamada = Convert.ToInt32(string.IsNullOrEmpty(dr["mtd_numeroChamada"].ToString()) ? null : dr["mtd_numeroChamada"])
                    ,
                    numeroChamada = string.IsNullOrEmpty(dr["mtd_numeroChamada"].ToString()) || (dr["mtd_numeroChamada"].ToString() == "0")
                        ? "-" : dr["mtd_numeroChamada"].ToString().PadLeft(2, '0')
                    ,
                    mtd_dataMatricula = Convert.ToDateTime(dr["mtd_dataMatricula"])
                    ,
                    mtd_dataSaida = string.IsNullOrEmpty(dr["mtd_dataSaida"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dr["mtd_dataSaida"])
                    ,
                    mtd_situacao = Convert.ToByte(dr["mtd_situacao"])
                    ,
                    ava_id = Convert.ToInt32(string.IsNullOrEmpty(dr["ava_id"].ToString()) ? null : dr["ava_id"])
                    ,
                    arq_idFoto = string.IsNullOrEmpty(dr["arq_idFoto"].ToString()) ? (long?)null : Convert.ToInt64(dr["arq_idFoto"])
                    ,
                    PossuiDeficienciaDisciplina = Convert.ToBoolean(dr["PossuiDeficienciaDisciplina"])
                    ,
                    PossuiDeficiencia = Convert.ToBoolean(dr["PossuiDeficiencia"])
                    ,
                    Nome = string.IsNullOrEmpty(dr["mtd_numeroChamada"].ToString()) || (dr["mtd_numeroChamada"].ToString() == "0")
                        ? "-" : dr["mtd_numeroChamada"].ToString().PadLeft(2, '0') + " - " + dr["pes_nome"].ToString()
                    ,
                    alu_mtu_mtd_id = dr["alu_id"].ToString() + ";" + dr["mtu_id"].ToString() + ";" + dr["mtd_id"].ToString()
                    ,
                    AlunoDispensado = Convert.ToBoolean(dr["dispensadisciplina"])
                    ,
                    dataAlteracao = Convert.ToDateTime(dr["dataAlteracao"])
                    ,
                    mtu_situacao = Convert.ToByte(dr["mtu_situacao"])
                }).ToList();

        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado. Traz alunos
        /// ativos e inativos na turma, para efetuar lançamentos retroativos para os alunos que já sairam
        /// da turma.
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>        
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplinaPeriodo_Inativos
        (
            long tud_id
            , Guid ent_id
            , int tpc_id
            , int ordenacao
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDisciplinaPeriodo(tud_id, ent_id, tpc_id, ordenacao, true);
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado. Traz alunos
        /// ativos e inativos na turma, para efetuar lançamentos retroativos para os alunos que já sairam
        /// da turma.
        /// filtrando os alunos com ou sem deficiência, dependendo do docente
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>        
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , Guid ent_id
            , int tpc_id
            , int ordenacao
            , EnumTipoDocente tipoDocente
        )
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, ent_id, tpc_id, ordenacao, true, (byte)tipoDocente);
        }

        /// <summary>
        /// Seleciona os alunos da disciplina de uma turma, pegando
        /// os alunos ativos e inativos, se houver um mesmo 
        /// aluno com mais de uma matrícula, pega a última  matrícula cadastrada
        /// desse aluno, pelo maior mtu_id.
        /// A data de matrícula do aluno tem que ser maior ou igual a data da aula
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <param name="ent_id">Id da entidade.</param>        
        /// <param name="tau_data">Data da aula.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_UltimaMatricula_Inativos
        (
            long tud_id
            , Guid ent_id
            , DateTime tau_data
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDisciplina_Max_Mtu(tud_id, ent_id, tau_data);
        }

        /// <summary>
        /// Seleciona os alunos da disciplina de uma turma, pegando
        /// os alunos ativos e inativos, se houver um mesmo 
        /// aluno com mais de uma matrícula, pega a última  matrícula cadastrada
        /// desse aluno, pelo maior mtu_id.
        /// A data de matrícula do aluno tem que ser maior ou igual a data da aula
        /// filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <param name="ent_id">Id da entidade.</param>        
        /// <param name="tau_data">Data da aula.</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_UltimaMatricula_FiltroDeficiencia
        (
            long tud_id
            , Guid ent_id
            , DateTime tau_data
            , EnumTipoDocente tipoDocente
        )
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurmaDisciplina_Max_Mtu_FiltroDeficiencia(tud_id, ent_id, tau_data, (byte)tipoDocente);
        }

        /// <summary>
        /// Retorna as matrículas realizadas para as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns>DataTable com registros da MatriculaTurma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ExisteMatriculaTurmaDisciplina_Turma
        (
           long tur_id
        )
        {

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelectBy_tur_id(tur_id).Rows.Count > 0;
        }

        /// <summary>
        /// Retorna as matrículas realizadas para as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>DataTable com registros da MatriculaTurma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ExisteMatriculaTurmaDisciplina_Turma
        (
           long tur_id
           , TalkDBTransaction banco
        )
        {

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_tur_id(tur_id).Rows.Count > 0;
        }

        /// <summary>
        /// Retorna a quantidade de alunos ativos matriculados na turma eletiva de acordo 
        /// com o código da turma passado no parâmetro.
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <returns>Quantidade de alunos</returns>
        public static int QuantidadeAlunosAtivosTurmaEletiva(long tur_id)
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            DataTable dt = dao.SelectBy_tur_id(tur_id);

            var x = from DataRow dr in dt.Rows
                    where Convert.ToByte(dr["mtd_Situacao"]) == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo
                    select 1;

            return x.Count();
        }

        /// <summary>
        /// Retorna dataTable contendo todos os alunos na Matricula Turma Disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com registros da MatriculaTurma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_tur_id
        (
           long tur_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelectBy_tur_id(tur_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna a MatriculaTurmaDisciplina para a matricula turma, o aluno, a turma disciplina e a data de matricula selecionados
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns>Entidade de MatriculaTurmaDisciplina</returns>
        public static MTR_MatriculaTurmaDisciplina SelecionaPorDisciplinaPeriodoAluno
        (
            long alu_id
            , int mtu_id
            , int tud_id
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            DataTable dt = dao.SelectByDisciplinaPeriodoAluno(alu_id, mtu_id, tud_id);
            MTR_MatriculaTurmaDisciplina entity = new MTR_MatriculaTurmaDisciplina();

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a 
        /// frequência).
        /// Filtra pela matrícula do aluno na disciplina, e pelo período.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da mtrícula na turma</param>
        /// <param name="mtd_id">ID da matrícula na disciplina</param>
        /// <param name="tud_id">ID da turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="frequencia">Frequência do aluno</param>
        /// <param name="qtdeAulas">Quantidade de aulas do aluno</param>
        /// <param name="qtdeFaltas">Quantidade de faltas do aluno</param>
        /// <param name="media">Média do aluno</param>
        /// <param name="mediaConceito">Média do aluno (Pareceres)</param>
        /// <returns></returns>
        public static void CalculaFrequencia_Media_Aluno
        (
            long alu_id
            , int mtu_id
            , int mtd_id
            , long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , out decimal frequencia
            , out int qtdeAulas
            , out int qtdeFaltas
            , out decimal media
            , out string mediaConceito
            , out decimal frequenciaFinal
            , out int ausenciasCompensadas
            , out decimal FrequenciaFinalAjustada
            , byte ava_tipo
            , bool fav_calcularMediaAvaliacaoFinal
            , byte tipoDocente = 0
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            DataTable dt =
                dao.CalculaFrequencia_Media_Aluno
                    (alu_id, mtu_id, mtd_id, tud_id, fav_id, tpc_id, tipoEscalaDisciplina, tipoEscalaDocente, tipoLancamento, fav_calculoQtdeAulasDadas, tipoDocente
                    , ava_tipo, fav_calcularMediaAvaliacaoFinal);

            if (dt.Rows.Count > 0)
            {
                qtdeAulas = Convert.ToInt32(dt.Rows[0]["QtAulasAluno"]);
                qtdeFaltas = Convert.ToInt32(dt.Rows[0]["QtFaltasAluno"]);
                frequencia = Convert.ToDecimal(dt.Rows[0]["Frequencia"]);
                media = Convert.ToDecimal(dt.Rows[0]["Avaliacao"]);
                mediaConceito = dt.Rows[0]["AvaliacaoPareceres"].ToString();
                frequenciaFinal = Convert.ToDecimal(dt.Rows[0]["frequenciaAcumulada"]);
                ausenciasCompensadas = Convert.ToInt32(dt.Rows[0]["ausenciasCompensadas"]);
                FrequenciaFinalAjustada = Convert.ToDecimal(dt.Rows[0]["FrequenciaFinalAjustada"]);
            }
            else
            {
                qtdeAulas = 0;
                qtdeFaltas = 0;
                frequencia = 0;
                media = 0;
                mediaConceito = "";
                frequenciaFinal = 0;
                ausenciasCompensadas = 0;
                FrequenciaFinalAjustada = 0;
            }
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a 
        /// frequência).
        /// Filtra pela matrícula do aluno na disciplina, e pelo período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <returns></returns>
        public static DataTable CalculaFrequencia_Media_TodosAlunos
        (
            long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , byte ava_tipo
            , bool fav_calcularMediaAvaliacaoFinal
            , byte tipoDocente = 0
            , List<long> listaTur_ids = null
        )
        {
            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (listaTur_ids != null)
            {
                listaTur_ids.ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurma.NewRow();
                            dr["tur_id"] = p;
                            dtTurma.Rows.Add(dr);
                        }
                    );
            }

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return
                dao.CalculaFrequencia_Media_TodosAlunos
                    (tud_id, fav_id, tpc_id, tipoEscalaDisciplina, tipoEscalaDocente, tipoLancamento, fav_calculoQtdeAulasDadas, tipoDocente, dtTurma, ava_tipo, fav_calcularMediaAvaliacaoFinal);
        }

        /// <summary>        
        /// Verifica a qtde de tempos de aulas em turma eletivas do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_idNaoConsiderar">ID da disciplina da turma</param>  
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        public static int VerificaTempoEletivasAluno
        (
            long alu_id
            , long tur_id
            , long tud_idNaoConsiderar
            , TalkDBTransaction banco
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_VerificaTempoEletivasAluno(alu_id, tur_id, tud_idNaoConsiderar);
        }

        public static void VerificaRegrasIncluiAlunosEmTurmaIndividual
        (
            MTR_MatriculaTurmaDisciplina mtd
            , Guid ent_id
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();

            // Valida se a turma eletiva do aluno atende o período que o aluno está
            // Valida se o aluno não ultrapassou a quantidade de tempos de aula por semana
            // de disciplinas eletivas do aluno oferecidos para o seu período
            string mensagem = string.Empty;

            if ((MTR_MatriculaTurmaSituacao)mtd.mtd_situacao == MTR_MatriculaTurmaSituacao.Ativo)
            {

                List<TUR_TurmaCurriculo> listTurmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(mtd.tur_id, dao._Banco, GestaoEscolarUtilBO.MinutosCacheLongo);
                if (!listTurmaCurriculo.Exists(p => p.crp_id == mtd.crp_id))
                    mensagem = mensagem + "A turma de eletiva " + mtd.tur_codigo + " não atende o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " do(a) aluno(a). " + mtd.pes_nome + ". <BR />";

                ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = mtd.cur_id, crr_id = mtd.crr_id, crp_id = mtd.crp_id };
                ACA_CurriculoPeriodoBO.GetEntity(crp, dao._Banco);

                TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina { tud_id = mtd.tud_id };
                TUR_TurmaDisciplinaBO.GetEntity(tud, dao._Banco);

                int tempoTotalAluno = VerificaTempoEletivasAluno(mtd.alu_id, mtd.tur_id, mtd.tud_id, dao._Banco) + tud.tud_cargaHorariaSemanal;

                if (tempoTotalAluno > crp.crp_qtdeEletivasAlunos)
                    mensagem = mensagem + "O(A) aluno(a) " + mtd.pes_nome + " excedeu a quantidade máxima de tempos de aula por semana de " +
                             CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " eletivos(as) para o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " " + crp.crp_descricao + ". <BR />";

                DateTime IniCal;
                ACA_CalendarioPeriodoBO.SelecionaPor_Calendario_tud_id(mtd.tud_id, out IniCal);

                if (IniCal > mtd.mtd_dataMatricula)
                    mensagem = mensagem + "A data de matrícula do(a) aluno(a) " + mtd.pes_nome + " não pode ser anterior a data de ínicio dos períodos que a turma oferece." + "<BR />";
            }

            if (!string.IsNullOrEmpty(mensagem))
                throw new ValidationException(mensagem);
        }

        /// <summary>
        /// Retorna quantidade de faltas e total já compensado pelo aluno.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="mtd_id"></param>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static DataTable CalculaFaltaCompensacaoAluno(long alu_id, int mtu_id, int mtd_id, long tud_id)
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.CalculaFaltaCompensacaoAluno(alu_id,
                mtu_id,
                mtd_id,
                tud_id);
        }

        /// <summary>
        /// Valida se a quantidade de aulas podem ser compensadas para o aluno.
        /// </summary>
        /// <param name="compensacaoAusenciaAluno"></param>
        /// <param name="quantidadeAulasCompensadas"></param>
        /// <returns></returns>
        public static bool ValidaQuantidadeAusenciasCompensadas(CLS_CompensacaoAusenciaAluno compensacaoAusenciaAluno, int quantidadeAulasCompensadas, TalkDBTransaction banco)
        {
            MTR_MatriculaTurmaDisciplinaDAO dao;

            if (banco == null)
            {
                dao = new MTR_MatriculaTurmaDisciplinaDAO();
            }
            else
            {
                dao = new MTR_MatriculaTurmaDisciplinaDAO
                {
                    _Banco = banco
                };
            }

            DataTable dt = dao.CalculaFaltaCompensacaoAluno(compensacaoAusenciaAluno.alu_id,
                compensacaoAusenciaAluno.mtu_id,
                compensacaoAusenciaAluno.mtd_id,
                compensacaoAusenciaAluno.tud_id);

            DataRow row = dt.Rows[0];

            // Faltas = 0
            // Compensacao = 1
            // Diferenca = 2
            if ((quantidadeAulasCompensadas + Convert.ToInt32(row[1].ToString())) > Convert.ToInt32(row[0].ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// Retorna a matricula ativa do aluno por turma disciplina
        /// </summary>
        /// <param name="alu_id">Aluno</param>
        /// <param name="tud_id">Turma disciplina</param>
        /// <param name="banco">Instancia do banco</param>
        /// <returns>matricula ativa</returns>
        public static MTR_MatriculaTurmaDisciplina BuscarPorAlunoTurmaDisciplina(long alu_id, long tud_id, TalkDBTransaction banco)
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco };
            DataTable dt = dao.SelectPorAlunoTurmaDisciplina(alu_id, tud_id);

            if (dt.Rows.Count == 0) return null;

            MTR_MatriculaTurmaDisciplina matricula = new MTR_MatriculaTurmaDisciplina
            {
                alu_id = alu_id,
                mtu_id = Convert.ToInt32(dt.Rows[0]["mtu_id"]),
                mtd_id = Convert.ToInt32(dt.Rows[0]["mtd_id"])
            };

            return GetEntity(matricula, banco);
        }

        /// <summary>
        /// Retorna os alunos com compensação de ausência
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="cpa_id">ID da compensacao de ausencia</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>        
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQtdeAlunosAusenciaCompensadas
        (
            int cpa_id
            , long tud_id
            , int tpc_id
            , long doc_id
            , bool documentoOficial
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.SelecionaQtdeAlunosAusenciasCompensadas(cpa_id, tud_id, tpc_id, doc_id, documentoOficial);
        }

        /// <summary>
        /// Calcula a média do aluno.
        /// Filtra pela matrícula do aluno nos componentes da regencia da turma, e pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da mtrícula na turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <returns></returns>
        public static DataTable Calcula_MediaComponentesRegencia_Aluno
        (
            long alu_id
            , int mtu_id
            , long tur_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.Calcula_MediaComponentesRegencia_Aluno
                    (tur_id, alu_id, mtu_id, fav_id, tpc_id, tipoEscalaDisciplina, tipoEscalaDocente);


        }

        /// <summary>
        /// Calcula a média de todos os alunos nos componentes da regencia da turma.
        /// Filtra pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>        
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public static DataTable Calcula_MediaComponentesRegencia_TodosAlunos
        (
            long tur_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.Calcula_MediaComponentesRegencia_TodosAlunos
                    (tur_id, fav_id, tpc_id, tipoEscalaDisciplina, tipoEscalaDocente);
        }

        /// <summary>
        /// Retorna os alunos matriculados na Turma, de acordo com as regras necessárias
        /// para ele aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaFinal> GetSelectBy_TurmaDisciplinaFinal
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , bool documentoOficial
            , byte tipoDocente = 0
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosEfetivacaoDisciplinaFinal> dados = null;

            Func<List<AlunosEfetivacaoDisciplinaFinal>> retorno = delegate()
                    {
                        DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                        if (listaTur_ids != null)
                        {
                            listaTur_ids.ForEach
                                (
                                    p =>
                                    {
                                        DataRow dr = dtTurma.NewRow();
                                        dr["tur_id"] = p;
                                        dtTurma.Rows.Add(dr);
                                    }
                                );
                        }

                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscFinalFormato
                                                                                    (
                                                                                    tud_id
                                                                                    , tur_id
                                                                                    , ava_id
                                                                                    , ordenacao
                                                                                    , fav_id
                                                                                    , tipoEscalaDisciplina
                                                                                    , tipoEscalaDocente
                                                                                    , tur_tipo
                                                                                    , cal_id
                                                                                    , tipoLancamento
                                                                                    , fav_calculoQtdeAulasDadas
                                                                                    , permiteAlterarResultado
                                                                                    , tipoDocente
                                                                                    , dtTurma
                                                                                    , documentoOficial
                                                                                    ))
                        {
                            return dt.AsEnumerable().Select(p => (AlunosEfetivacaoDisciplinaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaFinal())).ToList();
                        }
                    };

            if (appMinutosCacheFechamento > 0)
            {

                string chave = RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal
                                (
                                    tud_id
                                    , fav_id
                                    , ava_id
                                    , listaTur_ids.Any() ? string.Join(";", listaTur_ids.ToArray()) : "-1"
                                );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na Turma, de acordo com as regras necessárias
        /// para ele aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="alunos">Lista dos alunos para filtro</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaFinal> GetSelectBy_TurmaDisciplinaFinal_ByAluno
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , DataTable alunos
            , byte tipoDocente = 0
        )
        {
            List<AlunosEfetivacaoDisciplinaFinal> dados = new List<AlunosEfetivacaoDisciplinaFinal>();

            using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscFinalFormato_ByAluno
                                                                                    (
                                                                                    tud_id
                                                                                    , tur_id
                                                                                    , ava_id
                                                                                    , ordenacao
                                                                                    , fav_id
                                                                                    , tipoEscalaDisciplina
                                                                                    , tipoEscalaDocente
                                                                                    , tur_tipo
                                                                                    , cal_id
                                                                                    , tipoLancamento
                                                                                    , fav_calculoQtdeAulasDadas
                                                                                    , permiteAlterarResultado
                                                                                    , tipoDocente
                                                                                    , alunos
                                                                                    ))
            {
                dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoDisciplinaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaFinal())).ToList();


            }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na Turma, de acordo com as regras necessárias
        /// para ele aparecer na listagem para efetivar da avaliacao Final.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoDisciplinaFinal> GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , bool documentoOficial
            , byte tipoDocente = 0
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosEfetivacaoDisciplinaFinal> dados = null;

            Func<List<AlunosEfetivacaoDisciplinaFinal>> retorno = delegate()
                    {
                        DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                        if (listaTur_ids != null)
                        {
                            listaTur_ids.ForEach
                                (
                                    p =>
                                    {
                                        DataRow dr = dtTurma.NewRow();
                                        dr["tur_id"] = p;
                                        dtTurma.Rows.Add(dr);
                                    }
                                );
                        }

                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectBy_TurDiscFinalFormatoFiltroDeficiencia
                                                                                    (
                                                                                        tud_id
                                                                                        , tur_id
                                                                                        , ava_id
                                                                                        , ordenacao
                                                                                        , fav_id
                                                                                        , tipoEscalaDisciplina
                                                                                        , tipoEscalaDocente
                                                                                        , tur_tipo
                                                                                        , cal_id
                                                                                        , tipoLancamento
                                                                                        , fav_calculoQtdeAulasDadas
                                                                                        , permiteAlterarResultado
                                                                                        , tipoDocente
                                                                                        , dtTurma
                                                                                        , documentoOficial
                                                                                    ))
                        {
                            return dt.AsEnumerable().Select(p => (AlunosEfetivacaoDisciplinaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaFinal())).ToList();
                        }
                    };

            if (appMinutosCacheFechamento > 0)
            {

                string chave = RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia
                                (
                                    tud_id
                                    , fav_id
                                    , ava_id
                                    , listaTur_ids.Any() ? string.Join(";", listaTur_ids.ToArray()) : "-1"
                                );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os dados cadastrados para os componentes da regencia,
        /// de acordo com as regras necessárias para ele aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns>DataTable</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoFinalComponenteRegencia> GetSelect_ComponentesRegencia_By_TurmaFormato_Final
        (
            long tur_id
            , int ava_id
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , DataTable alunos
            , bool permiteAlterarResultado
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoFinalComponenteRegencia> dados = null;

            Func<List<AlunosEfetivacaoFinalComponenteRegencia>> retorno = delegate()
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectComponentesRegenciaBy_TurmaFormato_Final
                                                                                    (
                                                                                        tur_id
                                                                                        , ava_id
                                                                                        , fav_id
                                                                                        , tipoEscalaDisciplina
                                                                                        , tipoEscalaDocente
                                                                                        , tur_tipo
                                                                                        , cal_id
                                                                                        , alunos
                                                                                        , permiteAlterarResultado
                                                                                    ))
                        {
                    return dt.AsEnumerable().Select(p => (AlunosEfetivacaoFinalComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoFinalComponenteRegencia())).ToList();
            }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final
                                                                                    (
                                                                                        tur_id
                                        , fav_id
                                                                                        , ava_id
                                    );

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
                {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Seleciona os alunos matriculados na disciplina dada por determinado docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunosPorTurmaDocente(long doc_id, long tud_id)
        {
            return new MTR_MatriculaTurmaDisciplinaDAO().SelecionaAlunosPorTurmaDocente(doc_id, tud_id);
        }

        /// <summary>
        /// O método retorna uma lista de matrícula turma disciplina, passando uma tabela de chaves.
        /// </summary>
        /// <param name="dtMatriculaTurmaDisciplina">Tabela de chaves da MTR_MatriculaTurmaDisciplina.</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionaPorMatriculaTurmaDisciplina(DataTable dtMatriculaTurmaDisciplina, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new MTR_MatriculaTurmaDisciplinaDAO().SelecionaPorMatriculaTurmaDisciplina(dtMatriculaTurmaDisciplina) :
                new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco }.SelecionaPorMatriculaTurmaDisciplina(dtMatriculaTurmaDisciplina);
        }

        /// <summary>
        /// O método retorna uma lista de matrícula turma disciplina, passando uma lista de avaliações na disciplina.
        /// </summary>
        /// <param name="listaDisciplinas">Lista de notas de alunos nas disciplinas </param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> CriarListaPorAvaliacaoTurmaDisciplina(List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinas, TalkDBTransaction banco = null)
        {
            List<MTR_MatriculaTurmaDisciplina> listaMatriculas = new List<MTR_MatriculaTurmaDisciplina>();

            using (DataTable dtMatriculaTurmaDisciplina = MTR_MatriculaTurmaDisciplina.TipoTabela_AlunoMatriculaTurmaDisciplina())
            {
                (from CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas
                 group item by new { item.entity.alu_id, item.entity.mtu_id, item.entity.mtd_id } into grupo
                 select grupo.Key).ToList()
                .ForEach(
                    p =>
                    {
                        DataRow drMatriculaTurmaDisciplina = dtMatriculaTurmaDisciplina.NewRow();

                        drMatriculaTurmaDisciplina["alu_id"] = p.alu_id;
                        drMatriculaTurmaDisciplina["mtu_id"] = p.mtu_id;
                        drMatriculaTurmaDisciplina["mtd_id"] = p.mtd_id;

                        dtMatriculaTurmaDisciplina.Rows.Add(drMatriculaTurmaDisciplina);
                    }
                );

                listaMatriculas = SelecionaPorMatriculaTurmaDisciplina(dtMatriculaTurmaDisciplina, banco);
            }

            return listaMatriculas;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tud_tipo">Tipo da turma disciplina</param> 
        /// <param name="appMinutosCacheFechamento">Tempo de cache</param> 
        /// <param name="listaTur_ids">Turmas da multisseriada</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoPadrao> GetSelectFechamento
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , bool documentoOficial       
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosFechamentoPadrao> dados = null;

            Func<List<AlunosFechamentoPadrao>> retorno = delegate()
            {
                DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                if (listaTur_ids != null)
                {
                    listaTur_ids.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurma.NewRow();
                                dr["tur_id"] = p;
                                dtTurma.Rows.Add(dr);
                            }
                        );
                }

                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamento
                                        (
                                        tud_id
                                        , tur_id
                                        , tpc_id
                                        , ava_id
                                        , ordenacao
                                        , fav_id
                                        , tipoAvaliacao
                                        , permiteAlterarResultado  
                                        , tur_tipo
                                        , cal_id                                    
                                        , dtTurma
                                        , documentoOficial
                                        ))
                {
                    return dt.AsEnumerable().AsParallel().Select(p => (AlunosFechamentoPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoPadrao())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_id, tpc_id);
                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheFechamento
                    );
            }
            else
            {
                dados = retorno();
            }

            dados.Sort(
                delegate(AlunosFechamentoPadrao p1, AlunosFechamentoPadrao p2)
                {
                    return ordenacao == 0 ? Convert.ToInt32(string.IsNullOrEmpty(p1.mtd_numeroChamada) || p1.mtd_numeroChamada == "-" ? "999" : p1.mtd_numeroChamada)
                                                .CompareTo(Convert.ToInt32(string.IsNullOrEmpty(p2.mtd_numeroChamada) || p2.mtd_numeroChamada == "-" ? "999" : p2.mtd_numeroChamada))
                                            : p1.pes_nome.CompareTo(p2.pes_nome);
                });
            return dados;
        }

        /// <summary>
        /// Retorna os dados do fechamento cadastrados para os componentes da regencia
        /// para o período informado, de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoPadraoComponenteRegencia> GetSelectFechamentoComponentesRegencia
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , DataTable alunos
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosFechamentoPadraoComponenteRegencia> dados = null;

            Func<List<AlunosFechamentoPadraoComponenteRegencia>> retorno = delegate()
            {
                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamentoComponentesRegencia
                                                                            (
                                                                            tur_id
                                                                            , tpc_id
                                                                            , ava_id
                                                                            , fav_id
                                                                            , tipoAvaliacao
                                                                            , permiteAlterarResultado
                                                                            , tur_tipo
                                                                            , alunos
                                                                            ))
                {
                    return dt.AsEnumerable().AsParallel().AsOrdered().Select(p => (AlunosFechamentoPadraoComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoPadraoComponenteRegencia())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpc_id);
                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar. Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tud_tipo">Tipo da turma disciplina</param> 
        /// <param name="tipoDocente">Tipo de docente</param> 
        /// <param name="listaTur_ids">Turmas da multisseriada</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoPadrao> GetSelectFechamentoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , EnumTipoDocente tipoDocente
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0         
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosFechamentoPadrao> dados = null;

            Func<List<AlunosFechamentoPadrao>> retorno = delegate()
            {
                DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                if (listaTur_ids != null)
                {
                    listaTur_ids.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurma.NewRow();
                                dr["tur_id"] = p;
                                dtTurma.Rows.Add(dr);
                            }
                        );
                }

                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamentoFiltroDeficiencia
                                        (
                                                tud_id
                                            , tur_id
                                            , tpc_id
                                            , ava_id
                                            , ordenacao
                                            , fav_id
                                            , tipoAvaliacao
                                            , permiteAlterarResultado
                                            , tur_tipo
                                            , cal_id
                                            , (byte)tipoDocente
                                            , dtTurma
                                            , documentoOficial
                                        ))
                {
                    return dt.AsEnumerable().AsParallel().Select(p => (AlunosFechamentoPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoPadrao())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, tpc_id);
                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheFechamento
                    );
            }
            else
            {
                dados = retorno();
            }

            dados.Sort(
                delegate(AlunosFechamentoPadrao p1, AlunosFechamentoPadrao p2)
                {
                    return ordenacao == 0 ? Convert.ToInt32(string.IsNullOrEmpty(p1.mtd_numeroChamada) || p1.mtd_numeroChamada == "-" ? "999" : p1.mtd_numeroChamada)
                                                .CompareTo(Convert.ToInt32(string.IsNullOrEmpty(p2.mtd_numeroChamada) || p2.mtd_numeroChamada == "-" ? "999" : p2.mtd_numeroChamada))
                                            : p1.pes_nome.CompareTo(p2.pes_nome);
                });
            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na Turma, de acordo com as regras necessárias
        /// para ele aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="appMinutosCacheFechamento">Tempo de cache</param> 
        /// <param name="listaTur_ids">Turmas da multisseriada</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoFinal> GetSelectFechamentoFinal
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tur_tipo
            , int cal_id
            , bool permiteAlterarResultado
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosFechamentoFinal> dados = null;

            Func<List<AlunosFechamentoFinal>> retorno = delegate()
            {
                DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                if (listaTur_ids != null)
                {
                    listaTur_ids.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurma.NewRow();
                                dr["tur_id"] = p;
                                dtTurma.Rows.Add(dr);
                            }
                        );
                }

                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamentoFinal
                                        (
                                        tud_id
                                        , tur_id
                                        , ava_id
                                        , ordenacao
                                        , fav_id
                                        , tur_tipo
                                        , cal_id
                                        , permiteAlterarResultado
                                        , dtTurma
                                        , documentoOficial
                                        ))
                {
                    return dt.AsEnumerable().AsParallel().AsOrdered().Select(p => (AlunosFechamentoFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoFinal())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_id);
                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheFechamento
                    );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os dados cadastrados para os componentes da regencia,
        /// de acordo com as regras necessárias para ele aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>        
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoFinalComponenteRegencia> GetSelectFechamentoComponentesRegenciaFinal
        (
            long tur_id
            , int ava_id
            , int fav_id
            , int cal_id
            , DataTable alunos
            , bool permiteAlterarResultado            
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosFechamentoFinalComponenteRegencia> dados = null;

            Func<List<AlunosFechamentoFinalComponenteRegencia>> retorno = delegate()
            {
                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamentoComponentesRegenciaFinal
                                                                            (
                                                                            tur_id
                                                                            , ava_id
                                                                            , fav_id
                                                                            , cal_id
                                                                            , alunos
                                                                            , permiteAlterarResultado                                                                            
                                                                            ))
                {
                    return dt.AsEnumerable().AsParallel().AsOrdered().Select(p => (AlunosFechamentoFinalComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoFinalComponenteRegencia())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_id);
                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheFechamento
                            );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na Turma, de acordo com as regras necessárias
        /// para ele aparecer na listagem para efetivar da avaliacao Final.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param> 
        /// <param name="tipoDocente">Tipo de docente</param> 
        /// <param name="listaTur_ids">Turmas da multisseriada</param>   
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosFechamentoFinal> GetSelectFechamentoFiltroDeficienciaFinal
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tur_tipo
            , int cal_id
            , bool permiteAlterarResultado
            , EnumTipoDocente tipoDocente
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
            , List<long> listaTur_ids = null
        )
        {
            List<AlunosFechamentoFinal> dados = null;

            Func<List<AlunosFechamentoFinal>> retorno = delegate()
            {
                DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

                if (listaTur_ids != null)
                {
                    listaTur_ids.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurma.NewRow();
                                dr["tur_id"] = p;
                                dtTurma.Rows.Add(dr);
                            }
                        );
                }

                using (DataTable dt = new MTR_MatriculaTurmaDisciplinaDAO().SelectFechamentoFiltroDeficienciaFinal
                                        (
                                            tud_id
                                            , tur_id
                                            , ava_id
                                            , ordenacao
                                            , fav_id
                                            , tur_tipo
                                            , cal_id
                                            , permiteAlterarResultado
                                            , (byte)tipoDocente
                                            , dtTurma
                                            , documentoOficial
                                        ))
                {
                    return dt.AsEnumerable().AsParallel().AsOrdered().Select(p => (AlunosFechamentoFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoFinal())).ToList();
                }
            };

            if (appMinutosCacheFechamento > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id);
                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheFechamento
                    );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        #endregion

        #region Saves

        /// <summary>
        /// Replica os lançamentos realizados para os Tud_id informados, da MatriculaTurmaDisciplina antiga
        /// para a nova.
        /// </summary>
        /// <param name="listaMtdAntigo">Lista de MatriculaTurmaDisciplina antiga</param>
        /// <param name="listaMtdNovo">Lista de MatriculaTurmaDisciplina nova</param>
        /// <param name="tud_idsReplicar">IDs da TurmaDisciplina a replicar</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        internal static void ReplicarLancamentos_MatriculaDisciplina_TurmaEletiva
        (
            List<MTR_MatriculaTurmaDisciplina> listaMtdAntigo
            , List<MTR_MatriculaTurmaDisciplina> listaMtdNovo
            , List<long> tud_idsReplicar
            , TalkDBTransaction bancoGestao
        )
        {
            foreach (long tud_id in tud_idsReplicar)
            {
                MTR_MatriculaTurmaDisciplina mtdAnterior = listaMtdAntigo.Find(p => p.tud_id == tud_id);
                MTR_MatriculaTurmaDisciplina mtdNovo = listaMtdNovo.Find(p => p.tud_id == tud_id);

                // Replicar todos os lançamentos.
                if ((mtdAnterior != null) && (mtdNovo != null))
                {
                    // Replicar registros da CLS_AlunoAvaliacaoTurmaDisciplina.
                    List<CLS_AlunoAvaliacaoTurmaDisciplina> listaEfetivacao =
                        CLS_AlunoAvaliacaoTurmaDisciplinaBO.GetSelectBy_Disciplina_Aluno
                            (tud_id, mtdAnterior.alu_id, mtdAnterior.mtu_id, mtdAnterior.mtd_id, bancoGestao);

                    foreach (CLS_AlunoAvaliacaoTurmaDisciplina ent in listaEfetivacao)
                    {
                        ent.IsNew = true;
                        ent.mtu_id = mtdNovo.mtu_id;
                        ent.mtd_id = mtdNovo.mtd_id;
                        ent.atd_id = -1;

                        // Salva a entidade para a nova matrícula do aluno.
                        CLS_AlunoAvaliacaoTurmaDisciplinaBO.Save(ent, bancoGestao);
                    }

                    // Replicar registros da CLS_TurmaAulaAluno.
                    List<CLS_TurmaAulaAluno> listaFreqNormal =
                        CLS_TurmaAulaAlunoBO.GetSelectBy_Disciplina_Aluno
                            (tud_id, mtdAnterior.alu_id, mtdAnterior.mtu_id, mtdAnterior.mtd_id, bancoGestao);

                    foreach (CLS_TurmaAulaAluno ent in listaFreqNormal)
                    {
                        ent.IsNew = true;
                        ent.mtu_id = mtdNovo.mtu_id;
                        ent.mtd_id = mtdNovo.mtd_id;

                        // Salva a entidade para a nova matrícula do aluno.
                        CLS_TurmaAulaAlunoBO.Save(ent, bancoGestao);
                    }

                    // Replicar registros da CLS_TurmaNotaAluno.
                    List<CLS_TurmaNotaAluno> listaNotas =
                        CLS_TurmaNotaAlunoBO.GetSelectBy_Disciplina_Aluno
                            (tud_id, mtdAnterior.alu_id, mtdAnterior.mtu_id, mtdAnterior.mtd_id, bancoGestao);

                    foreach (CLS_TurmaNotaAluno ent in listaNotas)
                    {
                        ent.IsNew = true;
                        ent.mtu_id = mtdNovo.mtu_id;
                        ent.mtd_id = mtdNovo.mtd_id;

                        // Salva a entidade para a nova matrícula do aluno.
                        CLS_TurmaNotaAlunoBO.Save(ent, bancoGestao);
                    }
                }
            }
        }
        
        /// <summary>
        /// Override do método Save, se estiver setando a situação Inativo no registro,
        /// seta a data de saída.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <returns></returns>
        public new static bool Save(MTR_MatriculaTurmaDisciplina entity)
        {
            MTR_MatriculaTurmaDisciplina entAux = new MTR_MatriculaTurmaDisciplina
            {
                alu_id = entity.alu_id
                ,
                mtu_id = entity.mtu_id
                ,
                mtd_id = entity.mtd_id
            };
            GetEntity(entAux);

            if ((!entAux.IsNew) &&
                (entity.mtd_dataSaida == new DateTime()) &&
                (entity.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Inativo) &&
                (entity.mtd_situacao != entAux.mtd_situacao))
            {
                // Se a situação mudou para Inativo, setar a data de saída do aluno.
                entity.mtd_dataSaida = DateTime.Now;
            }

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();
            return dao.Salvar(entity);
        }

        /// <summary>
        /// Override do método Save, se estiver setando a situação Inativo no registro,
        /// seta a data de saída.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        public new static bool Save(MTR_MatriculaTurmaDisciplina entity, TalkDBTransaction banco)
        {
            MTR_MatriculaTurmaDisciplina entAux = new MTR_MatriculaTurmaDisciplina
            {
                alu_id = entity.alu_id
                ,
                mtu_id = entity.mtu_id
                ,
                mtd_id = entity.mtd_id
            };
            GetEntity(entAux, banco);

            if ((!entAux.IsNew) &&
                (entity.mtd_dataSaida == new DateTime()) &&
                (entity.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Inativo) &&
                (entity.mtd_situacao != entAux.mtd_situacao))
            {
                // Se a situação mudou para Inativo, setar a data de saída do aluno.
                entity.mtd_dataSaida = DateTime.Now;
            }

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco };
            return dao.Salvar(entity);
        }

        /// <summary>
        /// Salva número de chamada alterado na tela de consulta de alunos por turma
        /// </summary>
        /// <param name="lista">Lista de matrículas e números de chamada</param>
        /// <returns>TRUE - quando alteracao foi bem sucedida</returns>
        public static bool SalvarNumeroChamada
        (
            List<MTR_MatriculaTurma> lista
        )
        {
            //Termino da validação inicio do processo de salvar.
            MTR_MatriculaTurmaDAO dao_MatriculaTurma = new MTR_MatriculaTurmaDAO();
            dao_MatriculaTurma._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (MTR_MatriculaTurma mtu in lista)
                {
                    SalvarNumeroChamadaAluno(mtu.alu_id, mtu.mtu_id, mtu.mtu_numeroChamada, dao_MatriculaTurma._Banco);
                }

                return true;
            }
            catch (Exception err)
            {
                dao_MatriculaTurma._Banco.Close(err);
                throw;
            }
            finally
            {
                dao_MatriculaTurma._Banco.Close();
            }
        }

        /// <summary>
        /// Salva número de chamada do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno (filtro)</param>
        /// <param name="mtu_id">Id da matricula na turma (filtro)</param>
        /// <param name="mtu_numeroChamada">Novo número de chamada</param>
        /// <param name="banco">Novo banco</param>
        /// <returns>TRUE - quando alteracao foi bem sucedida</returns>
        public static bool SalvarNumeroChamadaAluno
        (
            long alu_id
            , int mtu_id
            , int mtu_numeroChamada
            , TalkDBTransaction banco
        )
        {
            MTR_MatriculaTurmaDAO dao_MatriculaTurma = new MTR_MatriculaTurmaDAO { _Banco = banco };
            return dao_MatriculaTurma.AtualizaNumeroChamada(alu_id, mtu_id, mtu_numeroChamada);
        }

        /// <summary>
        /// Metodo que monta um List de Entidades MTR_MatriculaTurmaDisciplina para cada registro do DataTable MatriculaTurma é criada entidade de MatriculaTurmaDisciplina.
        /// </summary>        
        public static List<MTR_MatriculaTurmaDisciplina> CriaList_Entities_MatriculaTurmaDisciplina
        (
            long tur_id
            , DataTable dtTurmaDisciplina
            , DataTable dtMatriculaTurma
            , DataTable dtMatriculaTurmaDisciplina
        )
        {
            try
            {
                //cria List
                List<MTR_MatriculaTurmaDisciplina> lt = new List<MTR_MatriculaTurmaDisciplina>();
                for (int i = 0; i < dtMatriculaTurma.Rows.Count; i++)
                {
                    int mtd_id_sequencial = 0;
                    //verifica se registro do DataTable é um novo registro
                    if (dtMatriculaTurma.Rows[i].RowState == DataRowState.Added)
                    {
                        for (int j = 0; j < dtTurmaDisciplina.Rows.Count; j++)
                        {
                            if (dtMatriculaTurma.Rows[i]["tur_id"].ToString() == tur_id.ToString())
                            {
                                MTR_MatriculaTurmaDisciplina entityMatriculaTurmaDisciplina = new MTR_MatriculaTurmaDisciplina();

                                entityMatriculaTurmaDisciplina.alu_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["alu_id"]);
                                entityMatriculaTurmaDisciplina.mtd_id = mtd_id_sequencial + 1;
                                entityMatriculaTurmaDisciplina.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id"]);
                                entityMatriculaTurmaDisciplina.tud_id = Convert.ToInt32(dtTurmaDisciplina.Rows[j]["tud_id"]);
                                entityMatriculaTurmaDisciplina.mtd_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada"]);
                                entityMatriculaTurmaDisciplina.mtd_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula"]).Date;
                                entityMatriculaTurmaDisciplina.mtd_situacao = Convert.ToByte(dtMatriculaTurma.Rows[i]["mtu_situacao"]);
                                entityMatriculaTurmaDisciplina.mtd_dataCriacao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.mtd_dataAlteracao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.IsNew = true;

                                lt.Add(entityMatriculaTurmaDisciplina);
                            }
                        }
                    }
                    //verifica se registro do Datable foi deletado.
                    else if (dtMatriculaTurma.Rows[i].RowState == DataRowState.Deleted)
                    {
                        //instancia valores para entidade como um registro deletado logicamente.
                        for (int j = 0; j < dtTurmaDisciplina.Rows.Count; j++)
                        {
                            if (dtMatriculaTurmaDisciplina.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtMatriculaTurmaDisciplina.Rows.Count; k++)
                                {
                                    if (Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["alu_id", DataRowVersion.Original]) == Convert.ToInt32(dtMatriculaTurma.Rows[i]["alu_id", DataRowVersion.Original]) &&
                                        Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["tud_id", DataRowVersion.Original]) == Convert.ToInt32(dtTurmaDisciplina.Rows[j]["tud_id", DataRowVersion.Original]))
                                    {
                                        mtd_id_sequencial = Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["mtd_id", DataRowVersion.Original]);
                                    }
                                }
                            }

                            if (dtMatriculaTurma.Rows[i]["tur_id", DataRowVersion.Original].ToString() == tur_id.ToString())
                            {
                                MTR_MatriculaTurmaDisciplina entityMatriculaTurmaDisciplina = new MTR_MatriculaTurmaDisciplina();

                                entityMatriculaTurmaDisciplina.alu_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["alu_id", DataRowVersion.Original]);
                                entityMatriculaTurmaDisciplina.mtd_id = mtd_id_sequencial;
                                entityMatriculaTurmaDisciplina.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id", DataRowVersion.Original]);
                                entityMatriculaTurmaDisciplina.tud_id = Convert.ToInt32(dtTurmaDisciplina.Rows[j]["tud_id", DataRowVersion.Original]);
                                entityMatriculaTurmaDisciplina.mtd_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada", DataRowVersion.Original]);
                                entityMatriculaTurmaDisciplina.mtd_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula", DataRowVersion.Original]).Date;
                                entityMatriculaTurmaDisciplina.mtd_situacao = Convert.ToByte(3);
                                entityMatriculaTurmaDisciplina.mtd_dataCriacao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.mtd_dataAlteracao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.IsNew = false;

                                lt.Add(entityMatriculaTurmaDisciplina);
                            }
                        }
                    }
                    //em ultimo caso registro é um registro já existente e não foi modificado.
                    else
                    {
                        //instancia valores para entidade como um registro já existente sem modificação. Atualiza apenas data de alteração para este registro.
                        for (int j = 0; j < dtTurmaDisciplina.Rows.Count; j++)
                        {
                            if (dtMatriculaTurmaDisciplina.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtMatriculaTurmaDisciplina.Rows.Count; k++)
                                {
                                    if (Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["alu_id"]) == Convert.ToInt32(dtMatriculaTurma.Rows[i]["alu_id"]) &&
                                        Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["tud_id"]) == Convert.ToInt32(dtTurmaDisciplina.Rows[j]["tud_id"]))
                                    {
                                        mtd_id_sequencial = Convert.ToInt32(dtMatriculaTurmaDisciplina.Rows[k]["mtd_id"]);
                                    }
                                }
                            }

                            if (dtMatriculaTurma.Rows[i]["tur_id"].ToString() == tur_id.ToString())
                            {
                                MTR_MatriculaTurmaDisciplina entityMatriculaTurmaDisciplina = new MTR_MatriculaTurmaDisciplina();

                                entityMatriculaTurmaDisciplina.alu_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["alu_id"]);
                                entityMatriculaTurmaDisciplina.mtd_id = mtd_id_sequencial;
                                entityMatriculaTurmaDisciplina.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id"]);
                                entityMatriculaTurmaDisciplina.tud_id = Convert.ToInt32(dtTurmaDisciplina.Rows[j]["tud_id"]);
                                entityMatriculaTurmaDisciplina.mtd_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada"]);
                                entityMatriculaTurmaDisciplina.mtd_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula"]).Date;
                                entityMatriculaTurmaDisciplina.mtd_situacao = Convert.ToByte(dtMatriculaTurma.Rows[i]["mtu_situacao"]);
                                entityMatriculaTurmaDisciplina.mtd_dataCriacao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.mtd_dataAlteracao = DateTime.Now;
                                entityMatriculaTurmaDisciplina.IsNew = false;

                                lt.Add(entityMatriculaTurmaDisciplina);
                            }
                        }
                    }
                }
                //retorna List
                return lt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna a lista de entidades da MatriculaTurmaDisciplina existentes de acordo com a 
        /// MatriculaTurma.
        /// </summary>
        /// <param name="entityMatriculaTurma">Entidade da MatriculaTurma</param>
        /// <param name="bancoGestao">Tranasação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionaPor_MatriculaTurma
        (
            MTR_MatriculaTurma entityMatriculaTurma
            , TalkDBTransaction bancoGestao
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelectBy_MatriculaTurma(entityMatriculaTurma.alu_id, entityMatriculaTurma.mtu_id);

            List<MTR_MatriculaTurmaDisciplina> lista =
                (from DataRow dr in dt.Rows
                 select
                     dao.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna as MatriculaTurmaDisciplina por turma disciplina.
        /// </summary>
        /// <param name="tud_ids">Lista de tud_ids do lançamento mensal.</param>
        /// <param name="bancoGestao">Tranasação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionaMatriculasPorTurmaDisciplina
        (
            string tud_ids
            , TalkDBTransaction bancoGestao
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelecionaMatriculasPorTurmaDisciplina(tud_ids);

            List<MTR_MatriculaTurmaDisciplina> lista =
                (from DataRow dr in dt.Rows
                 select
                     dao.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna a lista de entidades da MatriculaTurmaDisciplina existentes de acordo com a 
        /// MatriculaTurma.
        /// </summary>
        /// <param name="entityMatriculaTurma">Entidade da MatriculaTurma</param>
        /// <param name="bancoGestao">Tranasação com banco do Gestão - obrigatório</param>
        /// <param name="tur_idsEletivaAluno">ID das turmas eletivas (se alguma das disciplinas do aluno é de uma turma do tipo 2-Eletiva do aluno, traz uma lista de tur_id)</param>
        /// <param name="tud_idsEletivaAluno">ID das turmas eletivas (se alguma das disciplinas do aluno é de uma turma do tipo 2-Eletiva do aluno, traz uma lista de tud_id)</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionaPor_MatriculaTurma
        (
            MTR_MatriculaTurma entityMatriculaTurma
            , TalkDBTransaction bancoGestao
            , out List<long> tur_idsEletivaAluno
            , out List<long> tud_idsEletivaAluno
        )
        {
            tur_idsEletivaAluno = RetornaTurmaAtivaBy_TurmaEletivaAluno(entityMatriculaTurma, bancoGestao);
            tud_idsEletivaAluno = RetornaTurmaDisciplinaAtivaBy_TurmaEletivaAluno(entityMatriculaTurma, bancoGestao);

            List<MTR_MatriculaTurmaDisciplina> lista = new List<MTR_MatriculaTurmaDisciplina>();
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelecionaMatriculaTurmaDisciplinaAtiva(entityMatriculaTurma.alu_id, entityMatriculaTurma.mtu_id);

            foreach (DataRow dr in dt.Rows)
            {
                MTR_MatriculaTurmaDisciplina ent = dao.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina());
                lista.Add(ent);
            }

            return lista;
        }

        /// <summary>
        /// Retorna uma lista de MatriculaTurmaDisciplina para todos os Tud_id da turma.
        /// Adiciona os tud_ids das turmas eletivas passadas no list, verifica se a turma da entidade
        /// MatriculaTurma está no mesmo currículoPeríodo das turmas eletivas, caso positivo, adiciona esses 
        /// registros.
        /// </summary>
        /// <param name="entityMatriculaTurma">Entidade MatriculaTurma</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> CriaListaMatriculaTurmaDisciplina_Inclusao
        (
            MTR_MatriculaTurma entityMatriculaTurma
            , TalkDBTransaction banco
        )
        {
            List<long> tud_idsEletivas;
            return CriaListaMatriculaTurmaDisciplina_Inclusao(entityMatriculaTurma, banco, new List<long>(),
                                                              out tud_idsEletivas, null);
        }

        /// <summary>
        /// Retorna uma lista de MatriculaTurmaDisciplina para todos os Tud_id da turma.
        /// Adiciona os tud_ids das turmas eletivas passadas no list, verifica se a turma da entidade
        /// MatriculaTurma está no mesmo currículoPeríodo das turmas eletivas, caso positivo, adiciona esses 
        /// registros.
        /// </summary>
        /// <param name="entityMatriculaTurma">Entidade MatriculaTurma</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <param name="listaTur_id_Eletivas">IDs das turmas eletivas em que o aluno será matriculado</param>
        /// <param name="listaTud_ID_Eletivas">IDs das turmaDisciplinas que foram inseridas na lista, referentes as turmas eletivas do aluno</param>
        /// <param name="listaFechamentoMatricula">Lista de dados do fechamento de matrícula</param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurmaDisciplina> CriaListaMatriculaTurmaDisciplina_Inclusao
        (
            MTR_MatriculaTurma entityMatriculaTurma
            , TalkDBTransaction banco
            , List<long> listaTur_id_Eletivas
            , out List<long> listaTud_ID_Eletivas
            , FormacaoTurmaBO.ListasFechamentoMatricula listaFechamentoMatricula
        )
        {
            List<long> tud_ids = new List<long>();
            if (listaFechamentoMatricula != null && listaFechamentoMatricula.listDisciplinasTurmas != null)
            {
                // Se a lista de fechamento de matrícula estiver alimentada, buscar as disciplinas dela.
                tud_ids = (from TUR_TurmaRelTurmaDisciplina item in listaFechamentoMatricula.listDisciplinasTurmas
                           where item.tur_id == entityMatriculaTurma.tur_id
                           select item.tud_id).ToList();
            }

            if (tud_ids.Count <= 0)
            {
                DataTable dtTurmaDisciplina = TUR_TurmaRelTurmaDisciplinaBO.GetSelectBy_tur_id(entityMatriculaTurma.tur_id, banco);

                tud_ids = (from DataRow dr in dtTurmaDisciplina.Rows
                           select Convert.ToInt64(dr["tud_id"])).ToList();
            }

            List<MTR_MatriculaTurmaDisciplina> listaMtu = new List<MTR_MatriculaTurmaDisciplina>();
            foreach (long tud_id in tud_ids)
            {
                MTR_MatriculaTurmaDisciplina entityMatriculaTurmaDisciplina =
                    new MTR_MatriculaTurmaDisciplina
                        {
                            alu_id = entityMatriculaTurma.alu_id,
                            mtu_id = entityMatriculaTurma.mtu_id,
                            tud_id = tud_id,
                            mtd_numeroChamada = entityMatriculaTurma.mtu_numeroChamada,
                            mtd_dataMatricula = entityMatriculaTurma.mtu_dataMatricula.Date,
                            mtd_situacao = entityMatriculaTurma.mtu_situacao,
                            mtd_dataCriacao = DateTime.Now,
                            mtd_dataAlteracao = DateTime.Now,
                            IsNew = true
                        };

                listaMtu.Add(entityMatriculaTurmaDisciplina);
            }

            // Se estiver no fechamento de matrícula, não traz as matrículas de turmas eletivas.
            if (listaFechamentoMatricula == null)
            {
                // Currículos da turma normal.
                listaTud_ID_Eletivas = AdicionaMatriculas_TurmaEletiva
                (
                    entityMatriculaTurma
                    , banco
                    , ref listaMtu
                    , listaTur_id_Eletivas
                );
            }
            else
            {
                listaTud_ID_Eletivas = new List<long>();
            }

            return listaMtu;
        }

        /// <summary>
        /// Adiciona na lista de MatriculaTurmaDisciplina os tud_id das turmas eletivas.
        /// Retorna os tud_id inseridos.
        /// Valida se as turmas eletivas estão na mesma escola e possuem o mesmo curso, currículo e período
        /// da turma normal.
        /// </summary>
        /// <param name="entityMatriculaTurma">Entidade da matrícula turma para criar as MatriculaTurmaDisciplina</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="listaMtu">Lista de MatriculaTurma</param>
        /// <param name="listaTur_Id_Eletivas">Lista de Tur_id das turmas eletivas a verificar</param>
        /// <returns></returns>
        private static List<long> AdicionaMatriculas_TurmaEletiva
        (
            MTR_MatriculaTurma entityMatriculaTurma
            , TalkDBTransaction banco
            , ref List<MTR_MatriculaTurmaDisciplina> listaMtu
            , List<long> listaTur_Id_Eletivas
        )
        {
            List<long> listaTud_ID_Eletivas = new List<long>();

            List<TUR_TurmaCurriculo> listaCurriculos = TUR_TurmaCurriculoBO.GetSelectBy_Turma(entityMatriculaTurma.tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

            var varCurriculos = from TUR_TurmaCurriculo tcr in listaCurriculos
                                select new
                                           {
                                               tcr.cur_id
                                               ,
                                               tcr.crr_id
                                               ,
                                               tcr.crp_id
                                           };

            TUR_Turma entTurmaNormal = new TUR_Turma
                                           {
                                               tur_id = entityMatriculaTurma.tur_id
                                           };
            TUR_TurmaBO.GetEntity(entTurmaNormal, banco);

            foreach (long tur_id in listaTur_Id_Eletivas)
            {
                TUR_Turma entTurmaEletiva = new TUR_Turma
                                                {
                                                    tur_id = tur_id
                                                };
                TUR_TurmaBO.GetEntity(entTurmaEletiva, banco);

                // Se escola for a mesma.
                if ((entTurmaEletiva.esc_id == entTurmaNormal.esc_id) &&
                    (entTurmaEletiva.uni_id == entTurmaNormal.uni_id))
                {
                    // Currículos da turma eletiva.
                    List<TUR_TurmaCurriculo> listaCurriculosEletiva =
                        TUR_TurmaCurriculoBO.GetSelectBy_Turma(tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

                    var varCurriculosEletiva = from TUR_TurmaCurriculo tcr in listaCurriculosEletiva
                                               select new
                                                          {
                                                              tcr.cur_id
                                                              ,
                                                              tcr.crr_id
                                                              ,
                                                              tcr.crp_id
                                                          };

                    // Se existir pelo menos 1 currículoPeríodo igual entre os currículos da turma normal e eletiva.
                    if (varCurriculosEletiva.Intersect(varCurriculos).Count() > 0)
                    {
                        List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

                        var x = from TUR_TurmaDisciplina tud in listaDisciplinas
                                select
                                    new MTR_MatriculaTurmaDisciplina
                                        {
                                            alu_id = entityMatriculaTurma.alu_id,
                                            mtu_id = entityMatriculaTurma.mtu_id,
                                            tud_id = tud.tud_id,
                                            mtd_numeroChamada = entityMatriculaTurma.mtu_numeroChamada,
                                            mtd_dataMatricula = entityMatriculaTurma.mtu_dataMatricula.Date,
                                            mtd_situacao = entityMatriculaTurma.mtu_situacao,
                                            IsNew = true
                                        };

                        // Adiciona as disciplinas na lista de MatriculaTurmaDisciplina.
                        listaMtu.AddRange(x.ToList());

                        listaTud_ID_Eletivas.AddRange(
                            from TUR_TurmaDisciplina tud in listaDisciplinas
                            select tud.tud_id
                            );
                    }
                }
            }

            return listaTud_ID_Eletivas;
        }

        /// <summary>
        /// Deleta a matrícula na disciplina da turma, mas não verifica se possui outros registros ligados a ele.
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurmaDisciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DeleteEletiva
        (
            MTR_MatriculaTurmaDisciplina entity
            , TalkDBTransaction banco
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                if ((entity.mtd_dataSaida < entity.mtd_dataMatricula) && (entity.mtd_dataSaida != new DateTime()))
                {
                    throw new ValidationException("A data de saída do aluno não pode ser anterior a sua data de matrícula.");
                }

                // Deleta logicamente a matrícula na disciplina da turma
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Deleta a matrícula na disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurmaDisciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            MTR_MatriculaTurmaDisciplina entity
            , TalkDBTransaction banco
        )
        {
            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                if ((entity.mtd_dataSaida < entity.mtd_dataMatricula) && (entity.mtd_dataSaida != new DateTime()))
                    throw new ValidationException("A data de saída do aluno não pode ser anterior a sua data de matrícula.");

                // Verifica se a matrícula na disciplina da turma pode ser deletada
                if (GestaoEscolarUtilBO.VerificaIntegridadaChaveTripla
                (
                    "alu_id"
                    , "mtu_id"
                    , "mtd_id"
                    , entity.alu_id.ToString()
                    , entity.mtu_id.ToString()
                    , entity.mtd_id.ToString()
                    , "MTR_MatriculaTurmaDisciplina"
                    , dao._Banco
                ))
                {
                    throw new ValidationException("Não é possível excluir a matrícula no(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " pois possui outros registros ligados a ele(a).");
                }

                // Deleta logicamente a matrícula na disciplina da turma
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Inativa a matrícula turma disciplina de todos os alunos
        /// da turma disciplina passada.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina.</param>        
        /// <param name="banco">Transação com banco - obrigatório</param>
        public static void InativaPorTurmaDisciplina
        (
            long tud_id
            , TalkDBTransaction banco
        )
        {
            DateTime mtd_dataSaida = DateTime.Now;

            MTR_MatriculaTurmaDisciplinaDAO dao = new MTR_MatriculaTurmaDisciplinaDAO
                                                      {
                                                          _Banco = banco
                                                      };
            dao.UpdateSituacaoBy_TurmaDisciplina(tud_id, mtd_dataSaida, (byte)MTR_MatriculaTurmaDisciplinaSituacao.Inativo);
        }

        /// <summary>
        /// Atualiza o resultados das matrículas turma disciplina.
        /// </summary>
        /// <param name="ltMatriculaTurmaDisciplina">Lista de matrículas turma disciplina.</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static bool AtualizarResultado(List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplina, TalkDBTransaction banco = null)
        {
            using (DataTable dt = MTR_MatriculaTurmaDisciplina.TipoTabela_MatriculaTurmaDisciplinaResultado())
            {
                ltMatriculaTurmaDisciplina
                .ForEach(p =>
                            {
                                DataRow row = dt.NewRow();
                                row["alu_id"] = p.alu_id;
                                row["mtu_id"] = p.mtu_id;
                                row["mtd_id"] = p.mtd_id;

                                if (string.IsNullOrEmpty(p.mtd_avaliacao))
                                {
                                    row["mtd_avaliacao"] = DBNull.Value;
                                }
                                else
                                {
                                    row["mtd_avaliacao"] = p.mtd_avaliacao;
                                }

                                if (string.IsNullOrEmpty(p.mtd_relatorio))
                                {
                                    row["mtd_relatorio"] = DBNull.Value;
                                }
                                else
                                {
                                    row["mtd_relatorio"] = p.mtd_relatorio;
                                }

                                if (p.mtd_frequencia < 0)
                                {
                                    row["mtd_frequencia"] = DBNull.Value;
                                }
                                else
                                {
                                    row["mtd_frequencia"] = p.mtd_frequencia;
                                }

                                if (p.mtd_resultado <= 0)
                                {
                                    row["mtd_resultado"] = DBNull.Value;
                                }
                                else
                                {
                                    row["mtd_resultado"] = p.mtd_resultado;
                                }

                                row["apenasResultado"] = p.apenasResultado;

                                dt.Rows.Add(row);
                            });

                return banco == null ?
                    new MTR_MatriculaTurmaDisciplinaDAO().AtualizarResultado(dt) :
                    new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco }.AtualizarResultado(dt);
            }
        }

        #endregion

        #region Exclusão

        /// <summary>
        /// Excluir a matrícula em turma eletiva e salva o log.
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurmaDisciplina.</param>
        /// <param name="log">Entidade LOG_MatriculaTurmaDisciplinaExcluida.</param>
        /// <returns>Verdadeiro se conseguiu realizar a exclusão da matrícula e inclusão do log.</returns>
        public static bool ExcluirMatricula(MTR_MatriculaTurmaDisciplina entity, LOG_MatriculaTurmaDisciplinaExcluida log)
        {
            TalkDBTransaction banco = new MTR_MatriculaTurmaDisciplinaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                DeleteEletiva(entity, banco);
                LOG_MatriculaTurmaDisciplinaExcluidaBO.Save(log, banco);
                return true;
            }
            catch (ValidationException ex)
            {
                banco.Close(ex);
                throw;
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

        #endregion
    }
}
