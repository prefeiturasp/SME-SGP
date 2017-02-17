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
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da matrícula na turma do aluno
    /// </summary>
    public enum MTR_MatriculaTurmaSituacao : byte
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
    /// Resultado da avaliação geral do aluno na turma.
    /// </summary>
    public enum MtrTurmaResultado : byte
    {
        Aprovado = 1
        ,
        Reprovado = 2
        ,
        ReprovadoFrequencia = 8
        ,
        RecuperacaoFinal = 9
        ,
        AprovadoConselho = 10
    }

    #endregion Enumeradores

    #region Excessões

    public class QuantidadeVagasTurmaException : Exception
    {
        private string _mensagem;

        public QuantidadeVagasTurmaException()
        {
        }

        public QuantidadeVagasTurmaException(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    #endregion Excessões

    #region Estrutura

    /// <summary>
    /// Estrutura utilizada para guardar as notas finais dos alunos
    /// </summary>
    public struct NotaFinalAlunoTurma
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
        /// Nota do aluno
        /// </summary>
        public string nota;
    }

    /// <summary>
    /// Estrutura para armazenar o retorna da efetivação padrão em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoTurmaPadrao
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
        public bool aat_semProfessor { get; set; }
        public decimal Frequencia { get; set; }
        public int QtFaltasAluno { get; set; }
        public int QtAulasAluno { get; set; }
        public int QtFaltasAlunoReposicao { get; set; }
        public string pes_nome { get; set; }
        public string pes_dataNascimento { get; set; }
        public string mtd_numeroChamada { get; set; }
        public string id { get; set; }
        public decimal frequenciaAcumulada { get; set; }
        public string aat_relatorio { get; set; }
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
        public bool observacaoConselhoPreenchida { get; set; }
        public bool observacaoPreenchida { get; set; }
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
    /// Estrutura para armazenar o retorna da efetivação final em cache.
    /// </summary>
    [Serializable]
    public struct AlunosEfetivacaoTurmaFinal
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
        public string aat_relatorio { get; set; }
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
        public string AvaliacaoAdicional { get; set; }
        public byte mtu_resultado { get; set; }
        public int AlunoForaDaRede { get; set; }
    }

    #endregion

    public class MTR_MatriculaTurmaBO : BusinessBase<MTR_MatriculaTurmaDAO, MTR_MatriculaTurma>
    {
        #region Cache

        public const string Cache_GetSelectBy_Turma_Periodo = "Cache_GetSelectBy_Turma_Periodo";
        public const string Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma = "Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma";
        public const string Cache_GetSelectBy_Turma_Final = "Cache_GetSelectBy_Turma_Final";

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_Turma_Periodo
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                Cache_GetSelectBy_Turma_Periodo + "_{0}_{1}_{2}"
                , tur_id
                , fav_id
                , ava_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento (Recuperacao Final)
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                Cache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma + "_{0}_{1}_{2}"
                , tur_id
                , fav_id
                , ava_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o fechamento (Recuperacao Final)
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_Turma_Final
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            return string.Format(
                Cache_GetSelectBy_Turma_Final + "_{0}_{1}_{2}"
                , tur_id
                , fav_id
                , ava_id);
        }

        #endregion Cache

        #region Consultas

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(MTR_MatriculaTurma entity)
        {
            return string.Format(ModelCache.MATRICULA_TURMA_MODEL_KEY, entity.alu_id, entity.mtu_id);
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(MTR_MatriculaTurma entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static MTR_MatriculaTurma GetEntity(MTR_MatriculaTurma entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                (
                    chave,
                    () =>
                    {
                        dao.Carregar(entity);
                        return entity;
                    },
                    GestaoEscolarUtilBO.MinutosCacheMedio
                ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Retorna para a entidade a ultima matricula cadastrada.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>Retorna entidade para MatriculaTurma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MTR_MatriculaTurma GetSelectMatriculaAluno
        (
             long alu_id
        )
        {
            totalRecords = 0;
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            MTR_MatriculaTurma entity = new MTR_MatriculaTurma();
            DataTable dt = dao.SelectBy_MatriculaAluno(alu_id);

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna para a entidade a ultima matricula cadastrada,
        /// para os alunos da lista passada por parametro.
        /// </summary>
        /// <param name="dtAlunos">DataTable com os alunos para retornar a entidade</param>
        /// <returns>Retorna entidade para MatriculaTurma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MTR_MatriculaTurma> GetSelectMatriculaListaAlunos
        (
             DataTable dtAlunos
        )
        {
            totalRecords = 0;
            List<MTR_MatriculaTurma> entities = new List<MTR_MatriculaTurma>();
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            DataTable dt = dao.SelectBy_MatriculaListaAlunos(dtAlunos);
            foreach (DataRow dr in dt.Rows)
            {
                entities.Add(dao.DataRowToEntity(dr, new MTR_MatriculaTurma()));
            }

            return entities;
        }

        /// <summary>
        /// Busca os anos em que o aluno teve alguma matricula
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <returns>DataTable com os anos</returns>

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable GetSelectAnoMatricula
        (
           long alu_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectAnoMatricula(alu_id);
        }

        /// <summary>
        /// Retorna alunos deficiente na turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns>DataTable</returns>
        public static DataTable RetornaAlunoDeficienteTurma
        (
           long tur_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.RetornaAlunoDeficienteTurma(tur_id);
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma, com a frequência e nota dele
        /// totais no período informado de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoTurmaPadrao> GetSelectBy_Turma_Periodo
        (
            Int64 tur_id
            , Int32 tpc_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscala
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoTurmaPadrao> dados = null;

            if (appMinutosCacheFechamento > 0)
            {
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_GetSelectBy_Turma_Periodo
                                    (
                                        tur_id
                                        , fav_id
                                        , ava_id
                                    );
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Turma_Periodo
                                                                          (
                                                                          tur_id
                                                                          , tpc_id
                                                                          , ava_id
                                                                          , ordenacao
                                                                          , fav_id
                                                                          , tipoAvaliacao
                                                                          , esa_id
                                                                          , tipoEscala
                                                                          , avaliacaoesRelacionadas
                                                                          , notaMinimaAprovacao
                                                                          , ordemParecerMinimo
                                                                          , tipoLancamento
                                                                          , esa_tipoAvaliacaoAdicional
                                                                          , permiteAlterarResultado
                                                                          , documentoOficial
                                                                          ))
                        {
                            dados = dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoTurmaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaPadrao())).ToList();

                            // Adiciona cache com validade do tempo informado na configuração.
                            HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheFechamento), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        dados = (List<AlunosEfetivacaoTurmaPadrao>)cache;
                    }
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Turma_Periodo
                                                                  (
                                                                  tur_id
                                                                  , tpc_id
                                                                  , ava_id
                                                                  , ordenacao
                                                                  , fav_id
                                                                  , tipoAvaliacao
                                                                  , esa_id
                                                                  , tipoEscala
                                                                  , avaliacaoesRelacionadas
                                                                  , notaMinimaAprovacao
                                                                  , ordemParecerMinimo
                                                                  , tipoLancamento
                                                                  , esa_tipoAvaliacaoAdicional
                                                                  , permiteAlterarResultado
                                                                  , documentoOficial
                                                                  ))
                {
                    dados = dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoTurmaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaPadrao())).ToList();
                }
            }

            return ordenacao == 0 ?
                dados.OrderBy(p => p.mtd_numeroChamada == "-" ? 0 : Convert.ToInt32(p.mtd_numeroChamada)).ToList() :
                dados.OrderBy(p => p.pes_nome).ToList();
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoTurmaFinal> GetSelectBy_Turma_Final
        (
            Int64 tur_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , Int32 cal_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoTurmaFinal> dados = null;

            if (appMinutosCacheFechamento > 0)
            {
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_GetSelectBy_Turma_Final
                                    (
                                        tur_id
                                        , fav_id
                                        , ava_id
                                    );
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Turma_Final
                                                                        (
                                                                        tur_id
                                                                        , ava_id
                                                                        , ordenacao
                                                                        , fav_id
                                                                        , cal_id
                                                                        , tipoEscala
                                                                        , tipoLancamento
                                                                        , esa_tipoAvaliacaoAdicional
                                                                        , permiteAlterarResultado
                                                                        , documentoOficial
                                                                        ))
                        {
                            dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoTurmaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaFinal())).ToList();

                            // Adiciona cache com validade do tempo informado na configuração.
                            HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheFechamento), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        dados = (List<AlunosEfetivacaoTurmaFinal>)cache;
                    }
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Turma_Final
                                                                       (
                                                                       tur_id
                                                                       , ava_id
                                                                       , ordenacao
                                                                       , fav_id
                                                                       , cal_id
                                                                       , tipoEscala
                                                                       , tipoLancamento
                                                                       , esa_tipoAvaliacaoAdicional
                                                                       , permiteAlterarResultado
                                                                       , documentoOficial
                                                                       ))
                {
                    dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoTurmaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaFinal())).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="alunos">Lista dos alunos para filtro</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoTurmaFinal> GetSelectBy_Turma_Final_ByAluno
        (
            Int64 tur_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , Int32 cal_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , DataTable alunos
        )
        {
            List<AlunosEfetivacaoTurmaFinal> dados = new List<AlunosEfetivacaoTurmaFinal>();

            using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Turma_Final_ByAluno
                                                                        (
                                                                        tur_id
                                                                        , ava_id
                                                                        , ordenacao
                                                                        , fav_id
                                                                        , cal_id
                                                                        , tipoEscala
                                                                        , tipoLancamento
                                                                        , esa_tipoAvaliacaoAdicional
                                                                        , permiteAlterarResultado
                                                                        , alunos
                                                                        ))
            {
                dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoTurmaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaFinal())).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna uma lista com as notas dos alunos
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<NotaFinalAlunoTurma> GetSelect_NotaFinalAluno_By_Turma_Periodo
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscala
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , bool documentoOficial
        )
        {
            return GetSelectBy_Turma_Periodo(tur_id, tpc_id, ava_id, ordenacao, fav_id, tipoAvaliacao, esa_id, tipoEscala, avaliacaoesRelacionadas, notaMinimaAprovacao, ordemParecerMinimo, tipoLancamento, esa_tipoAvaliacaoAdicional, permiteAlterarResultado, documentoOficial)
                .Where(p => !p.naoAvaliado && !p.aat_semProfessor)
                .Select(p =>
                new NotaFinalAlunoTurma
                {
                    alu_id = p.alu_id,
                    mtu_id = p.mtu_id,
                    nota = p.Avaliacao,
                }).ToList();
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<AlunosEfetivacaoTurmaPadrao> GetSelectBy_Alunos_RecuperacaoFinal_By_Turma
        (
            Int64 tur_id
            , Int32 tpc_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , int esa_id
            , byte tipoEscala
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool documentoOficial
            , int appMinutosCacheFechamento = 0
        )
        {
            List<AlunosEfetivacaoTurmaPadrao> dados = null;

            if (appMinutosCacheFechamento > 0)
            {
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_Turma
                                    (
                                        tur_id
                                        , fav_id
                                        , ava_id
                                    );
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Alunos_RecuperacaoFinal_By_Turma
                                                                            (
                                                                            tur_id
                                                                            , tpc_id
                                                                            , ava_id
                                                                            , ordenacao
                                                                            , fav_id
                                                                            , esa_id
                                                                            , tipoEscala
                                                                            , avaliacaoesRelacionadas
                                                                            , notaMinimaAprovacao
                                                                            , ordemParecerMinimo
                                                                            , tipoLancamento
                                                                            , esa_tipoAvaliacaoAdicional
                                                                            , documentoOficial
                                                                            ))
                        {
                            dados = dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoTurmaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaPadrao())).ToList();

                            // Adiciona cache com validade do tempo informado na configuração.
                            HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheFechamento), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        dados = (List<AlunosEfetivacaoTurmaPadrao>)cache;
                    }
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new MTR_MatriculaTurmaDAO().SelectBy_Alunos_RecuperacaoFinal_By_Turma
                                                                             (
                                                                             tur_id
                                                                             , tpc_id
                                                                             , ava_id
                                                                             , ordenacao
                                                                             , fav_id
                                                                             , esa_id
                                                                             , tipoEscala
                                                                             , avaliacaoesRelacionadas
                                                                             , notaMinimaAprovacao
                                                                             , ordemParecerMinimo
                                                                             , tipoLancamento
                                                                             , esa_tipoAvaliacaoAdicional
                                                                             , documentoOficial
                                                                             ))
                {
                    dados = dt.AsEnumerable().AsParallel().Select(p => (AlunosEfetivacaoTurmaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoTurmaPadrao())).ToList();
                }
            }

            return ordenacao == 0 ?
                 dados.OrderBy(p => p.mtd_numeroChamada == "-" ? 0 : Convert.ToInt32(p.mtd_numeroChamada)).ToList() :
                 dados.OrderBy(p => p.pes_nome).ToList();
        }

        /// <summary>
        /// Seleciona turma, periodo, escola, curso e numero da chamada
        /// que o aluno esta ativo ou em matricula
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>DataTable turma, periodo, escola, curso e numero da chamada do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectDadosMatriculaAluno
        (
             long alu_id
        )
        {
            totalRecords = 0;
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectDadosMatriculaAluno(alu_id);
        }

        /// <summary>
        /// Seleciona turma, periodo, escola, curso e numero da chamada pelo aluno e mtu_id
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula turma</param>
        /// <returns>DataTable turma, periodo, escola, curso e numero da chamada do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectDadosMatriculaAlunoMtu
        (
             long alu_id
            , int mtu_id

        )
        {
            totalRecords = 0;
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectDadosMatriculaAlunoMtu(alu_id, mtu_id);
        }

        /// <summary>
        /// Seleciona o id da última matrícula turma cadastrada para o aluno + 1
        /// se não houver matrícula turma cadastrada para o aluno retorna 1
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns>mtu_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimaMatriculaTurmaCadastrada
        (
            long alu_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectBy_alu_id_top_one(alu_id);
        }

        /// <summary>
        /// Retorna o novo número de chamada da turma informada, pega o último número de chamada cadastrado
        /// + 1.
        /// </summary>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        /// <param name="cur_id">ID do curso - obrigatório</param>
        /// <param name="crr_id">ID do currículo - obrigatório</param>
        /// <param name="crp_id">ID do período - obrigatório</param>
        /// <returns>Último número de chamada + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 RetornaNovoNumeroChamada_Turma
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.RetornaNovoNumeroChamada_Turma(tur_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Traz todos os alunos de uma turma em ordem alfabetica
        /// com as situações ativo ou em matrícula
        /// </summary>
        public static DataTable MatriculaTurmaAlunosOrdemAlfabetica
        (
            long tur_id
            , TalkDBTransaction banco
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO { _Banco = banco };
            return dao.SelectBy_tur_id(tur_id);
        }

        /// <summary>
        /// Description:	Lista dos alunos matriculados para aquela turma por odem de chamada
        /// 				Número de chamada;
        /// 				Número de matrícula ou matrícula estadual de acordo com o parâmetro ;
        /// 				Nome do Aluno.
        /// </summary>
        /// <param name="tur_id">Id filtro da turma</param>
        /// <returns>Datatable com alunos da turma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable MatriculaTurmaAlunosNumeroChamada(long tur_id)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectAlunosBy_tur_id(tur_id);
        }

        /// <summary>
        /// Description:	Lista dos alunos ativos matriculados para aquela turma
        ///
        /// </summary>
        /// <param name="tur_id">Id de filtro da turma</param>
        /// <returns>Datatable com alunos da turma</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable MatriculaTurmaAlunosAtivosNumeroChamada(long tur_id)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectAlunosAtivosBy_tur_id(tur_id);
        }

        /// <summary>
        /// Retorna a quantidade de alunos na turma que estejam ativos.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static int QuantidadeAlunosAtivosTurma(long tur_id)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            DataTable dt = dao.SelectAlunosBy_tur_id(tur_id);

            var x = from DataRow dr in dt.Rows
                    where Convert.ToByte(dr["mtuSituacao"]) == (byte)MTR_MatriculaTurmaSituacao.Ativo
                    select 1;

            return x.Count();
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a
        /// frequência).
        /// Filtra pela matrícula do aluno na turma, e pelo período.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da mtrícula na turma</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="frequencia">Frequência do aluno</param>
        /// <param name="qtdeAulas">Quantidade de aulas do aluno</param>
        /// <param name="qtdeFaltas">Quantidade de faltas do aluno</param>
        /// <param name="frequenciaAcumulada">Frequência acumulada calculada de acordo com a quantidade de aulas e faltas retornada</param>
        /// <returns></returns>
        public static void CalculaFrequencia_Media_Aluno
        (
            long alu_id
            , int mtu_id
            , long tur_id
            , int fav_id
            , int tpc_id
            , int ava_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , out decimal frequencia
            , out int qtdeAulas
            , out int qtdeFaltas
            , out decimal frequenciaAcumulada
            , out int ausenciasCompensadas
            , out decimal FrequenciaFinalAjustada
            , out string Avaliacao
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            DataTable dt =
                dao.CalculaFrequencia_Media_Aluno
                    (alu_id, mtu_id, tur_id, fav_id, tpc_id, ava_id, tipoEscala, tipoLancamento, fav_calculoQtdeAulasDadas);

            if (dt.Rows.Count > 0)
            {
                qtdeAulas = Convert.ToInt32(dt.Rows[0]["QtAulasAluno"]);
                qtdeFaltas = Convert.ToInt32(dt.Rows[0]["QtFaltasAluno"]);
                frequencia = Convert.ToDecimal(dt.Rows[0]["Frequencia"]);
                frequenciaAcumulada = Convert.ToDecimal(dt.Rows[0]["FrequenciaAcumulada"]);
                ausenciasCompensadas = Convert.ToInt32(dt.Rows[0]["ausenciasCompensadas"]);
                FrequenciaFinalAjustada = Convert.ToDecimal(dt.Rows[0]["FrequenciaFinalAjustada"]);
                Avaliacao = dt.Rows[0]["Avaliacao"].ToString();
            }
            else
            {
                qtdeAulas = 0;
                qtdeFaltas = 0;
                frequencia = 0;
                frequenciaAcumulada = 0;
                ausenciasCompensadas = 0;
                FrequenciaFinalAjustada = 0;
                Avaliacao = "";
            }
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a
        /// frequência).
        /// Filtra pela matrícula do aluno na disciplina, e pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns></returns>
        public static DataTable CalculaFrequencia_Media_TodosAlunos
        (
            long tur_id
            , int fav_id
            , int tpc_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , int ava_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return
                dao.CalculaFrequencia_Media_TodosAlunos
                    (tur_id, fav_id, tpc_id, tipoEscala, tipoLancamento, fav_calculoQtdeAulasDadas, ava_id);
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma selecionada dentro do período informado.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ordenacao">0 - ordena pelo numero de chamada / 1- ordena pelo nome do aluno</param>
        /// <param name="dataInicio">Data de início do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <param name="tpc_id"></param>
        /// <returns>DataTable de alunos matriculados</returns>
        public static DataTable SelecionaPorTurmaDataMatricula
        (
            long tur_id
            , int ordenacao
            , DateTime dataInicio
            , DateTime dataFim
            , int tpc_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelectBy_Turma_DataMatricula(tur_id, ordenacao, dataInicio, dataFim, tpc_id);
        }

        /// <summary>
        /// Retorna os alunos de uma turma
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns>DataTable com os ids dos alunos</returns>
        public static DataTable BuscaAlunosPorTurma
        (
            long tur_id
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.BuscaAlunosPorTurma(tur_id);
        }

        /// <summary>
        /// Lista dos alunos matriculados na turma dentro do periodo de calendário.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <param name="trazerInativos">1 - traz inativos | 0 - não traz inativos</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunosPorTurmaPeriodoCalendario(int tur_id, Guid ent_id, int cal_id, int cap_id, byte trazerInativos)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelecionaAlunosPorTurmaPeriodoCalendario(tur_id, ent_id, cal_id, cap_id, trazerInativos);
        }

        /// <summary>
        /// Verifica o status da turma atual da matricula turma
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula turma</param>
        /// <returns></returns>
        public static DataTable VerificaStatusTurmaAtual(long alu_id, int mtu_id)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.VerificaStatusTurmaAtual(alu_id, mtu_id);
        }

        /// <summary>
        /// Seleciona os períodos de avaliação do curso/currículo/período do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <returns></returns>
        public static DataTable SelecionaPeriodosAvaliacaoPorAluno(long alu_id, int cur_id, int crr_id, int crp_id)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.SelecionaPeriodosAvaliacaoPorAluno(alu_id, cur_id, crr_id, crp_id);
        }

        #endregion Consultas

        #region Saves

        /// <summary>
        /// Override do método Save, se estiver setando a situação Inativo no registro,
        /// seta a data de saída.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <returns></returns>
        public new static bool Save(MTR_MatriculaTurma entity)
        {
            MTR_MatriculaTurma entAux = new MTR_MatriculaTurma
            {
                alu_id = entity.alu_id
                ,
                mtu_id = entity.mtu_id
            };
            GetEntity(entAux);

            if ((!entAux.IsNew) &&
                (entity.mtu_dataSaida == new DateTime()) &&
                (entity.mtu_situacao == (byte)MTR_MatriculaTurmaSituacao.Inativo) &&
                (entity.mtu_situacao != entAux.mtu_situacao))
            {
                // Se a situação mudou para Inativo, setar a data de saída do aluno.
                entity.mtu_dataSaida = DateTime.Now;
            }

            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            return dao.Salvar(entity);

            LimpaCache(entity);
        }

        /// <summary>
        /// Override do método Save, se estiver setando a situação Inativo no registro,
        /// seta a data de saída.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        public new static bool Save(MTR_MatriculaTurma entity, TalkDBTransaction banco)
        {
            MTR_MatriculaTurma entAux = new MTR_MatriculaTurma
            {
                alu_id = entity.alu_id
                ,
                mtu_id = entity.mtu_id
            };
            GetEntity(entAux, banco);

            if ((!entAux.IsNew) &&
                (entity.mtu_dataSaida == new DateTime()) &&
                (entity.mtu_situacao == (byte)MTR_MatriculaTurmaSituacao.Inativo) &&
                (entity.mtu_situacao != entAux.mtu_situacao))
            {
                // Se a situação mudou para Inativo, setar a data de saída do aluno.
                entity.mtu_dataSaida = DateTime.Now;
            }

            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO { _Banco = banco };

            LimpaCache(entity);
            return dao.Salvar(entity);

            LimpaCache(entity);
        }

        /// <summary>
        /// Adiciona o número de chamada dos alunos que ainda não tinham, passados por parametro
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="alunosNumeroChamada">DataTable com alu_id, mtu_id e mtu_numeroChamada</param>
        public static void AdicionaNumeroChamada
        (
            long tur_id
            , DataTable alunosNumeroChamada
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Sequencia o número de chamada dos alunos ativos
                foreach (DataRow dr in alunosNumeroChamada.Rows)
                {
                    int numeroChamada = Convert.ToInt32(dr["numeroChamada"]);
                    MTR_MatriculaTurma entity = new MTR_MatriculaTurma
                    {
                        alu_id = Convert.ToInt64(dr["alu_id"]),
                        mtu_id = Convert.ToInt32(dr["mtu_id"])
                    };

                    GetEntity(entity, banco);
                    List<MTR_MatriculaTurmaDisciplina> List_MatriculaTurmaDisciplina = MTR_MatriculaTurmaDisciplinaBO.SelecionaPor_MatriculaTurma(entity, banco);
                    foreach (MTR_MatriculaTurmaDisciplina entityMTD in List_MatriculaTurmaDisciplina)
                    {
                        entityMTD.mtd_numeroChamada = numeroChamada;
                        MTR_MatriculaTurmaDisciplinaBO.Save(entityMTD, banco);
                    }

                    entity.mtu_numeroChamada = numeroChamada;
                    Save(entity, banco);
                }

                // Remover número de chamda dos alunos inativos
                DataTable dtAlunosMatriculados = MatriculaTurmaAlunosNumeroChamada(tur_id);

                var x = from DataRow dr in dtAlunosMatriculados.Rows
                        where Convert.ToByte(dr["mtuSituacao"].ToString()) == (byte)MTR_MatriculaTurmaSituacao.Inativo
                        select dr;

                foreach (DataRow dr in x.ToList())
                {
                    long alu_id = Convert.ToInt64(dr["alu_id"]);
                    int mtu_id = Convert.ToInt32(dr["mtu_id"]);

                    const int numeroChamada = -1;

                    MTR_MatriculaTurmaDisciplinaBO.SalvarNumeroChamadaAluno(alu_id, mtu_id, numeroChamada, banco);
                }
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método recalcula e atualiza as notas e frequência finais de um aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="banco">Transação.</param>
        /// <param name="mtu_resultado">Resultado final global do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public static bool CalcularNotaFrequenciaMatriculaAnoAnterior(long alu_id, int mtu_id, TalkDBTransaction banco, byte mtu_resultado, bool permiteAlterarResultado)
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO { _Banco = banco };
            return dao.CalcularNotaFrequenciaMatriculaAnoAnterior(alu_id, mtu_id, mtu_resultado, permiteAlterarResultado);
        }

        #endregion Saves

        #region Validações

        /// <summary>
        /// Metodo que monta um List de Entidades MTR_MatriculaTurma para cada registro do DataTable recebido.
        /// </summary>
        /// <param name="dtMatriculaTurma">DataTable com registros da entidade MTR_MatriculaTurma</param>
        /// <param name="mtu_situacao"></param>
        /// <returns></returns>
        public static List<MTR_MatriculaTurma> CriaList_Entities_MatriculaTurma(DataTable dtMatriculaTurma, byte mtu_situacao)
        {
            //cria List
            List<MTR_MatriculaTurma> lt = new List<MTR_MatriculaTurma>();
            for (int i = 0; i < dtMatriculaTurma.Rows.Count; i++)
            {
                //cria entidade
                MTR_MatriculaTurma entityMatriculaTurma = new MTR_MatriculaTurma();
                //verifica se registro do DataTable é um novo registro
                if (dtMatriculaTurma.Rows[i].RowState == DataRowState.Added)
                {
                    if (Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id"]) <= 0)
                        dtMatriculaTurma.Rows[i]["mtu_id"] = VerificaUltimaMatriculaTurmaCadastrada(entityMatriculaTurma.alu_id);

                    dtMatriculaTurma.Rows[i]["mtu_situacao"] = mtu_situacao;

                    entityMatriculaTurma.alc_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alc_id"]);
                    entityMatriculaTurma.alu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alu_id"]);
                    entityMatriculaTurma.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id"]);
                    entityMatriculaTurma.tur_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["tur_id"]);
                    entityMatriculaTurma.cur_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["cur_id"]);
                    entityMatriculaTurma.crr_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crr_id"]);
                    entityMatriculaTurma.crp_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crp_id"]);
                    entityMatriculaTurma.mtu_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula"]).Date;
                    entityMatriculaTurma.mtu_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada"]);
                    entityMatriculaTurma.mtu_situacao = Convert.ToByte(dtMatriculaTurma.Rows[i]["mtu_situacao"]);
                    entityMatriculaTurma.IsNew = true;

                    //adiciona entidade na List
                    lt.Add(entityMatriculaTurma);
                }
                else if (dtMatriculaTurma.Rows[i].RowState == DataRowState.Deleted)
                {
                    //instancia valores para entidade como um registro deletado logicamente.
                    entityMatriculaTurma.alc_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alc_id", DataRowVersion.Original]);
                    entityMatriculaTurma.alu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alu_id", DataRowVersion.Original]);
                    entityMatriculaTurma.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id", DataRowVersion.Original]);
                    entityMatriculaTurma.tur_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["tur_id", DataRowVersion.Original]);
                    entityMatriculaTurma.cur_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["cur_id", DataRowVersion.Original]);
                    entityMatriculaTurma.crr_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crr_id", DataRowVersion.Original]);
                    entityMatriculaTurma.crp_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crp_id", DataRowVersion.Original]);
                    entityMatriculaTurma.mtu_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula", DataRowVersion.Original]).Date;
                    entityMatriculaTurma.mtu_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada", DataRowVersion.Original]);
                    entityMatriculaTurma.mtu_situacao = Convert.ToByte(MTR_MatriculaTurmaSituacao.Excluido);
                    entityMatriculaTurma.IsNew = false;
                    //adiciona entidade na List
                    lt.Add(entityMatriculaTurma);
                }
                //em ultimo caso registro é um registro já existente e não foi modificado.
                else
                {
                    //instancia valores para entidade como um registro já existente sem modificação. Atualiza apenas data de alteração para este registro.
                    dtMatriculaTurma.Rows[i]["mtu_situacao"] = mtu_situacao;

                    entityMatriculaTurma.alc_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alc_id"]);
                    entityMatriculaTurma.alu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["alu_id"]);
                    entityMatriculaTurma.mtu_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_id"]);
                    entityMatriculaTurma.tur_id = Convert.ToInt64(dtMatriculaTurma.Rows[i]["tur_id"]);
                    entityMatriculaTurma.cur_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["cur_id"]);
                    entityMatriculaTurma.crr_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crr_id"]);
                    entityMatriculaTurma.crp_id = Convert.ToInt32(dtMatriculaTurma.Rows[i]["crp_id"]);
                    entityMatriculaTurma.mtu_dataMatricula = Convert.ToDateTime(dtMatriculaTurma.Rows[i]["mtu_dataMatricula"]).Date;
                    entityMatriculaTurma.mtu_numeroChamada = Convert.ToInt32(dtMatriculaTurma.Rows[i]["mtu_numeroChamada"]);
                    entityMatriculaTurma.mtu_situacao = Convert.ToByte(dtMatriculaTurma.Rows[i]["mtu_situacao"]);
                    entityMatriculaTurma.IsNew = false;

                    //adiciona entidade na List
                    lt.Add(entityMatriculaTurma);
                }
            }
            //retorna List
            return lt;
        }

        /// <summary>
        /// Verifica se o aluno já está cadastrado em alguma turma
        /// com as situações ativo ou em matrícula
        /// </summary>
        public static bool VerificaAlunoMatriculaTurmaCadastrado
        (
            MTR_MatriculaTurma entity
            , out string pes_nome
        )
        {
            MTR_MatriculaTurmaDAO dao = new MTR_MatriculaTurmaDAO();

            if (dao.SelectBy_alu_id(entity.alu_id))
            {
                ACA_Aluno alu = new ACA_Aluno { alu_id = entity.alu_id };
                ACA_AlunoBO.GetEntity(alu);

                PES_Pessoa pes = new PES_Pessoa { pes_id = alu.pes_id };
                PES_PessoaBO.GetEntity(pes);

                pes_nome = pes.pes_nome;

                return true;
            }

            pes_nome = string.Empty;
            return false;
        }

        /// <summary>
        /// Verifica se a quantidade de vagas de uma turma foi excedido.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns></returns>
        public static bool VerificarQuantidadeVagasTurma
        (
            long tur_id
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            int qtVagas, qtMatriculados, qtDeficientes;
            string tur_codigo;
            TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(tur_id, out qtVagas, out qtMatriculados, banco, out qtDeficientes, out tur_codigo);

            if (qtMatriculados + 1 > qtVagas)
            {
                // Se existir aluno deficiente na turma.
                if (qtDeficientes > 0)
                {
                    throw new QuantidadeVagasTurmaException(
                        String.Format(
                            "Essa turma possui aluno(s) {0}(s). Não se recomenda exceder a quantidade da turma. O número máximo de alunos para a turma " +
                            tur_codigo + " (" + (qtVagas) + " alunos) será excedido em " +
                            (qtMatriculados + 1 - qtVagas) + " aluno(s)." +
                            "<BR /><BR /> Confirma a inclusão do aluno na turma?",
                            ACA_ParametroAcademicoBO.ParametroValorPorEntidade(
                                eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, ent_id).ToLower()));
                }

                throw new QuantidadeVagasTurmaException("O número máximo de alunos para a turma "
                                                        + tur_codigo + " (" + (qtVagas) + " alunos) será excedido em " +
                                                        (qtMatriculados + 1 - qtVagas) + " aluno(s)." +
                                                        "<BR /><BR /> Confirma a inclusão do aluno na turma?");
            }

            return true;
        }

        /// <summary>
        /// Verifica se tem aluno de recuperação final.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="esa_id">ID da escala de avaliação</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="notaMinimaAprovacao">Nota mínima para aprovação</param>
        /// <param name="ordemParecerMinimo">Ordem de parecer mínima para aprovação</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas</param>
        /// <returns></returns>
        public static bool VerificaResultadoRecuperacaoFinal(long tur_id, int esa_id, byte tipoEscala, double notaMinimaAprovacao, int ordemParecerMinimo, string avaliacaoesRelacionadas)
        {
            return new MTR_MatriculaTurmaDAO().VerificaResultadoRecuperacaoFinal(tur_id, esa_id, tipoEscala, notaMinimaAprovacao, ordemParecerMinimo, avaliacaoesRelacionadas);
        }

        #endregion Validações
    }
}