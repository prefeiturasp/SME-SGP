using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;
using System.Threading.Tasks;
using MSTech.GestaoEscolar.BLL.Caching;
using System.Collections.ObjectModel;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da avaliação disciplina do aluno na turma.
    /// </summary>
    public enum CLS_AlunoAvaliacaoTurmaDisciplinaSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Estrutura usada para cadastrar a nota para a avaliação.
    /// </summary>
    public struct CLS_AvaliacaoTurDisc_Cadastro
    {
        public CLS_AlunoAvaliacaoTurmaDisciplina entity;
        public MtrTurmaDisciplinaResultado resultado;
        public bool atualizarResultado;
        public int mtu_idAnterior;
        public int mtd_idAnterior;
    }

    /// <summary>
    /// Estrutura usada para transferir efetivação na movimentação.
    /// </summary>
    public class CLS_AlunoAvaliacaoTurDis_Efetivacao
    {
        public CLS_AlunoAvaliacaoTurmaDisciplina entity;
        public int tds_id;
    }

    /// <summary>
    /// Estrutura que armazena as observações do aluno na efetivação.
    /// </summary>
    [Serializable]
    public struct CLS_AlunoAvaliacaoTurDis_Observacao
    {
        public long tud_id;
        public long alu_id;
        public int mtu_id;
        public int mtd_id;
        public int fav_id;
        public int ava_id;
        public CLS_AlunoAvaliacaoTurmaDisciplinaObservacao entityObservacao;
        public List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> ltQualidade;
        public List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> ltDesempenho;
        public List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> ltRecomendacao;
    }

    /// <summary>
    /// Estrutura que armazena as pendências de efetivação.
    /// </summary>
    [Serializable]
    public struct CLS_AlunoAvaliacaoTurDis_PendenciaEfetivacao
    {
        public long tud_idRegencia { get; set; }
        public long tud_id { get; set; }
        public byte tud_tipo { get; set; }
        public bool processado { get; set; }
        public bool pendencia { get; set; }
        public bool pendenciaParecer { get; set; }
        public DateTime dataProcessamento { get; set; }
    }

    /// <summary>
    /// Estrutura com os dados dos alunos.
    /// </summary>
    [Serializable]
    public struct CLS_AlunoAvaliacaoTurDis_DadosAlunos
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int tpc_idUltimoPeriodoLancamento { get; set; }
        public int mtu_numeroChamada { get; set; }
        public string pes_nome { get; set; }
        public string pes_nome_infoCompl { get; set; }
        public string nomeFormatado { get; set; }
        public bool inativo { get; set; }
        public bool baixaFrequencia { get; set; }
        public bool pendencia { get; set; }
        public string disciplinaPendencia { get; set; }
    }

    #endregion

    public class CLS_AlunoAvaliacaoTurmaDisciplinaBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaDAO, CLS_AlunoAvaliacaoTurmaDisciplina>
    {

        #region Propriedades

        /// <summary>
        /// Retorno os Tipos de turma disciplina que devem ser considerados
        /// como para referencia para verificar a situação dos alunos
        /// </summary>
        private static readonly ReadOnlyCollection<TurmaDisciplinaTipo> TurmaDisciplinaTipoSituacao =
            new ReadOnlyCollection<TurmaDisciplinaTipo>(new[]
                            {
                TurmaDisciplinaTipo.Obrigatoria,
                TurmaDisciplinaTipo.DocenteTurmaObrigatoria,
                TurmaDisciplinaTipo.Regencia,
                TurmaDisciplinaTipo.ComponenteRegencia,
                TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
        });

        #endregion Propriedades

        #region Consultas

        public static List<BoletimAluno> SelecionaDadosHistoricoTransferencia(long alu_id, int mtu_id, long doc_id)
        {
            using (DataTable dt = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().SelecionaDadosHistoricoTransferencia(alu_id, mtu_id, doc_id))
            {
                return dt.Rows.Cast<DataRow>().Select(p => (BoletimAluno)GestaoEscolarUtilBO.DataRowToEntity(p, new BoletimAluno())).ToList();
            }
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para a disciplina
        /// e avaliação informados.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="bancoGestao">Transação com banco</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplina> SelecionaPor_DisciplinaAvaliacao(long tur_id, long tud_id, int fav_id, int ava_id, TurmaDisciplinaTipo tipoDisciplina, TalkDBTransaction bancoGestao)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO();//{ _Banco = bancoGestao };
            if (bancoGestao != null)
                dao._Banco = bancoGestao;
            DataTable dt = tipoDisciplina != TurmaDisciplinaTipo.Regencia ? dao.SelectBy_DisciplinaAvaliacao(tud_id, fav_id, ava_id) : dao.SelectBy_DisciplinaAvaliacaoRegencia(tur_id, fav_id, ava_id);

            return (from DataRow dr in dt.Rows.AsParallel()
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplina())).ToList();
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para a disciplinas (tud_ids)
        /// e avaliação informados.
        /// </summary>
        /// <param name="tud_ids">Id da turma disciplina - obrigatório</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="bancoGestao">Transação com banco</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplina> SelecionaPor_DisciplinaAvaliacaoTurmaDisciplina(string tud_ids, int fav_id, int ava_id, TalkDBTransaction bancoGestao)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelectBy_DisciplinaAvaliacaoTurmaDisciplina(tud_ids, fav_id, ava_id);

            return (from DataRow dr in dt.Rows.AsParallel()
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplina())).ToList();
        }


        /// <summary>
        /// Busca as efetivações da matrícula turma disciplina do aluno de todas as
        /// disciplinas oferecidas de acordo com o formato de avaliação.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula do aluno</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da Avaliação</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        /// <returns>True - existe efetivação para a matrícula do aluno | False - não existe efetivação para a matrícula do aluno</returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplina> SelecionaEfetivacaoDisciplinasOferecidas_Aluno(long alu_id, int mtu_id, int fav_id, int ava_id, TalkDBTransaction bancoGestao)
        {
            List<CLS_AlunoAvaliacaoTurmaDisciplina> lista = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelecionaEfetivacaoDisciplinasOferecidas_Aluno(alu_id, mtu_id, fav_id, ava_id);

            foreach (DataRow dr in dt.Rows)
            {
                CLS_AlunoAvaliacaoTurmaDisciplina ent =
                    dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplina());

                lista.Add(ent);
            }

            return lista;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina que sejam pela 
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Lista de  CLS_AlunoAvaliacaoTurma</returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplina> GetSelectBy_Disciplina_Aluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
            , TalkDBTransaction banco
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO
            {
                _Banco = banco
            };

            return dao.SelectBy_Disciplina_Aluno(tud_id, alu_id, mtu_id, mtd_id);
        }

        /// <summary>
        /// Retorna uma entidade carregada, buscando pela "chave" da avaliação do aluno 
        /// (parâmetros).
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Entidade CLS_AlunoAvaliacaoTurma</returns>
        public static CLS_AlunoAvaliacaoTurmaDisciplina GetEntityBy_ChaveAvaliacaoAluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
            , Int32 fav_id
            , Int32 ava_id
            , TalkDBTransaction banco
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO();
            if (banco != null)
            {
                dao._Banco = banco;
            }
            return dao.LoadBy_ChaveAvaliacaoAluno(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id);
        }

        /// <summary>
        /// Retorna as notas de fechamento para todas as disciplinas da turma
        /// e para um determinado aluno.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="fav_id">ID do formato de avaliacao</param>
        /// <param name="fav_id">ID do calendario</param>
        /// <returns>DataTable</returns>
        public static DataTable SelecionaPor_AlunoTurma(long tur_id, long alu_id, int mtu_id, int fav_id, int cal_id)
        {
            return new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().SelectBy_AlunoTurma(tur_id, alu_id, mtu_id, fav_id, cal_id);
        }

        /// <summary>
        /// Seleciona os alunos da turma para exibir na tela de fechamento do gestor.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ordenacao">Ordenação (Por número de chamado ou por nome).</param>
        /// <param name="tev_idFechamento">Id do tipo de evento do fechamento.</param>
        /// <param name="alu_id">Id do aluno (passar 0 para trazer os dados de todos os alunos).</param>
        /// <returns>Alunos da turma.</returns>
        public static List<CLS_AlunoAvaliacaoTurDis_DadosAlunos> SelecionarAlunosTurma(long tur_id, byte ordenacao, int tev_idFechamento, bool documentoOficial, long alu_id = 0)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO();
            DataTable dt = dao.SelecionarAlunosTurma(tur_id, ordenacao, tev_idFechamento, documentoOficial, alu_id);

            return dt.Rows.Count > 0 ?
                dt.Rows.Cast<DataRow>().Select(dr => (CLS_AlunoAvaliacaoTurDis_DadosAlunos)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurDis_DadosAlunos())).ToList() :
                new List<CLS_AlunoAvaliacaoTurDis_DadosAlunos>();
        }

        /// <summary>
        /// Retorna todos os dados do boletim de todos os alunos informados na lista
        /// em suas respectivas matriculas turmas
        /// </summary>
        /// <param name="alu_ids">Ids dos alunos (separados por ',')</param>
        /// <param name="mtu_ids">Ids das matriculas turmas (separados por ',')</param>
        /// <param name="tpc_id">Id do Bimestre</param>
        /// <returns></returns>
        public static List<ACA_AlunoBO.BoletimDadosAlunoFechamento> BuscaDadosAlunoFechamentoGestor(long alu_id, int mtu_id, int tpc_id, Guid ent_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            DataTable dtAlunoMatriculaTurma = MTR_MatriculaTurma.TipoTabela_AlunoMatriculaTurma();
            DataRow dr = dtAlunoMatriculaTurma.NewRow();
            dr["alu_id"] = alu_id;
            dr["mtu_id"] = mtu_id;

            dtAlunoMatriculaTurma.Rows.Add(dr);

            // Busca os dados gerais dos alunos
            DataTable dt = dao.BuscaBoletimAlunos(dtAlunoMatriculaTurma, tpc_id);

            // Busca os dados do boletim de todos os alunos
            List<DadosFechamento> listaBoletimAlunos = SelecionaDadosAlunoFechamentoGestor(alu_id, mtu_id);
            listaBoletimAlunos = AjustaDadosExibicaoBoletim(listaBoletimAlunos, ent_id);

            List<ACA_AlunoBO.BoletimDadosAlunoFechamento> lista = new List<ACA_AlunoBO.BoletimDadosAlunoFechamento>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ACA_AlunoBO.BoletimDadosAlunoFechamento boletim = new ACA_AlunoBO.BoletimDadosAlunoFechamento();
                boletim = (ACA_AlunoBO.BoletimDadosAlunoFechamento)GestaoEscolarUtilBO.DataRowToEntity(dt.Rows[i], boletim);
                boletim.listaNotasEFaltas = listaBoletimAlunos.FindAll(p => p.alu_id == boletim.alu_id);

                lista.Add(boletim);
            }

            return lista;
        }

        /// <summary>
        /// Ajusta os dados para exibição correta do boletim, todas as disciplinas devem aparecer em todos os bimestres
        /// para não quebrar o layout da table no repeater.
        /// </summary>
        /// <param name="listaBoletimAlunos"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        private static List<DadosFechamento> AjustaDadosExibicaoBoletim(List<DadosFechamento> listaBoletimAlunos, Guid ent_id)
        {

            List<DadosFechamento> lstAdd = new List<DadosFechamento>();
            List<long> lstAluIds = listaBoletimAlunos.GroupBy(p => p.alu_id).Select(p => p.Key).ToList();
            foreach (long alu_id in lstAluIds)
            {
                List<int> lstTpcIds = listaBoletimAlunos.Where(p => p.alu_id == alu_id)
                                      .GroupBy(p => new { tpc_id = p.tpc_id, tpc_ordem = p.tpc_ordem })
                                      .OrderBy(p => p.Key.tpc_ordem).Select(p => p.Key.tpc_id).ToList();
                int qtdPeriodos = lstTpcIds.Count;
                int tds_id = 0;
                string Disciplina = "";
                foreach (DadosFechamento item in listaBoletimAlunos.Where(p => p.alu_id == alu_id))
                {
                    if (tds_id == item.tds_id && Disciplina.Equals(item.Disciplina))
                        continue;

                    tds_id = item.tds_id;
                    Disciplina = item.Disciplina;

                    if (listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id && p.Disciplina.Equals(Disciplina)).GroupBy(p => p.tpc_id).Count() == qtdPeriodos)
                        continue;

                    foreach (int tpc_id in lstTpcIds)
                    {
                        if (listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id && p.Disciplina.Equals(Disciplina)).Any(p => p.tpc_id == tpc_id))
                            continue;

                        var situacaoDisciplinas = listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tpc_id == tpc_id && TurmaDisciplinaTipoSituacao.Contains((TurmaDisciplinaTipo)p.tud_tipo))
                            .GroupBy(g => g.SituacaoDisciplina).Select(s => s.Key).ToList();

                        DadosFechamento itemCopiar = listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id && p.Disciplina.Equals(Disciplina)).FirstOrDefault();
                        DadosFechamento itemTpc = listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tpc_id == tpc_id).FirstOrDefault();
                        lstAdd.Add(new DadosFechamento
                        {
                            tds_id = tds_id,
                            alu_id = alu_id,
                            mtu_id = itemCopiar.mtu_id,
                            tur_id = itemCopiar.tur_id,
                            tur_codigo = itemCopiar.tur_codigo,
                            mtd_id = itemCopiar.mtd_id,
                            cal_id = itemCopiar.cal_id,
                            tud_id = itemCopiar.tud_id,
                            tud_global = itemCopiar.tud_global,
                            Disciplina = Disciplina,
                            ava_id = itemTpc.ava_id,
                            tpc_id = tpc_id,
                            tpc_ordem = itemTpc.tpc_ordem,
                            tpc_nome = itemTpc.tpc_nome,
                            fav_variacao = itemCopiar.fav_variacao,
                            mostraConceito = itemCopiar.mostraConceito,
                            mostraNota = itemCopiar.mostraNota,
                            mostraFrequencia = itemCopiar.mostraFrequencia,
                            naoExibirNota = itemCopiar.naoExibirNota,
                            naoExibirFrequencia = itemCopiar.naoExibirFrequencia,
                            MostrarLinhaDisciplina = itemCopiar.MostrarLinhaDisciplina,
                            dda_id = itemCopiar.dda_id,
                            esc_codigo = itemCopiar.esc_codigo,
                            esc_nome = itemCopiar.esc_nome,
                            ava_idResultado = itemCopiar.ava_idResultado,
                            fav_idResultado = itemCopiar.fav_idResultado,
                            tds_ordem = itemCopiar.tds_ordem,
                            tud_tipo = itemCopiar.tud_tipo,
                            esa_tipo = itemCopiar.esa_tipo,
                            nomeDisciplina = itemCopiar.nomeDisciplina,
                            EnriquecimentoCurricular = itemCopiar.EnriquecimentoCurricular,
                            Recuperacao = itemCopiar.Recuperacao,
                            cur_id = itemCopiar.cur_id,
                            crr_id = itemCopiar.crr_id,
                            crp_id = itemCopiar.crp_id,
                            bimestreComLancamento = itemCopiar.bimestreComLancamento,
                            existeAulaBimestre = itemCopiar.existeAulaBimestre,
                            SituacaoDisciplina = (itemCopiar.tud_tipo == (int)TurmaDisciplinaTipo.Experiencia ?
                                (itemCopiar.tur_id != itemCopiar.tur_idDisciplina ? eSituacaoMatriculaTurmaDisicplina.ForaRede : situacaoDisciplinas.FirstOrDefault()) :
                                (itemCopiar.tur_id != itemCopiar.tur_idDisciplina ? eSituacaoMatriculaTurmaDisicplina.ForaRede : itemCopiar.SituacaoDisciplina)),
                            esconderPendencia = true,
                            mtu_resultado = itemCopiar.mtu_resultado,
                            usuarioParecerConclusivo = itemCopiar.usuarioParecerConclusivo,
                            dataAlteracaoParecerConclusivo = itemCopiar.dataAlteracaoParecerConclusivo,
                            faltasExternas = itemCopiar.faltasExternas
                        });
                    }
                }
            }

            if (!lstAdd.Any())
                return listaBoletimAlunos;

            listaBoletimAlunos.AddRange(lstAdd);

            bool controleOrdemDisc = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            if (controleOrdemDisc)
                return listaBoletimAlunos.OrderBy(p => p.tds_ordem).ThenBy(p => p.tpc_ordem).ToList();
            else
                return listaBoletimAlunos.OrderBy(p => p.Disciplina).ThenBy(p => p.tpc_ordem).ToList();
        }

        /// <summary>
        /// Traz os dados do fechamento da matricula do aluno (Baseada na procedure NEW_Relatorio_0001_SubBoletimEscolarAluno).
        /// </summary>
        /// <returns>Lista com os dados do boletim.</returns>
        public static List<DadosFechamento> SelecionaDadosAlunoFechamentoGestor(long alu_id, int mtu_id)
        {
            DataTable dt = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().SelecionaDadosAlunoFechamentoGestor(alu_id, mtu_id);

            List<DadosFechamento> lista = (from DataRow dr in dt.Rows
                                           select
                                               (DadosFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new DadosFechamento())
                                       ).ToList();

            return lista;
        }

        #endregion

        #region Validação

        /// <summary>
        /// Retorna true/false
        /// para saber se a Efetivação (CLS_AlunoAvaliacaoTurmaDisciplina) já está cadastrada
        /// filtradas por tud_id, alu_id, mtd_id, atd_id
        /// </summary>
        /// <param name="tud_id">Id da tabela TUR_TurmaDisciplina do bd</param>        
        /// <param name="alu_id">Campo alu_id da tabela CLS_TurmaAulaAluno do bd</param>
        /// <param name="mtd_id">Campo mtd_id da tabela CLS_TurmaAulaAluno do bd</param>
        /// <param name="mtu_id"></param>
        /// <param name="atd_id">Campo atd_id da tabela CLS_TurmaAulaAluno do bd</param>
        /// <returns>Retorna true = frequencia já cadastrada | false para frequencia ainda não cadastrado</returns>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaEfetivacaoTurmaDisciplinaExistente
        (
            long tud_id
            , long alu_id
            , int mtd_id
            , int mtu_id
            , int atd_id
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO();
            return dao.SelectBy_Chaves(tud_id, alu_id, mtd_id, mtu_id, atd_id);
        }

        /// <summary>
        /// Verifica todos os alunos da lista, caso algum esteja marcado como faltoso no lançamento global,
        /// verifica se o aluno possui alguma nota lançada para a disciplina, nessa avaliação.
        /// Se tiver, dispara uma excessão, pois não pode lançar avaliação para aluno faltoso.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="listaDisciplinas">Lista de avaliação nas disciplinas</param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        private static void VerificaFaltoso(TalkDBTransaction banco, long tur_id, List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinas, int fav_id, int ava_id)
        {
            List<CLS_AlunoAvaliacaoTurma> listaEntAvaliacao =
                CLS_AlunoAvaliacaoTurmaBO.SelecionaPor_TurmaAvaliacao(tur_id, fav_id, ava_id, banco);

            var faltosos = from CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas.AsParallel()
                           let entDisciplina = item.entity
                           let entTurma = listaEntAvaliacao.Find(
                    p => p.tur_id == tur_id
                         && p.alu_id == entDisciplina.alu_id
                         && p.mtu_id == entDisciplina.mtu_id
                         && p.fav_id == entDisciplina.fav_id
                         && p.ava_id == entDisciplina.ava_id
                                                                 && p.aat_faltoso)
                           where entTurma != null && !entTurma.IsNew &&
                                (!string.IsNullOrEmpty(entDisciplina.atd_avaliacao))
                           select new
                           {
                               alu_id = entDisciplina.alu_id
                           };

            if (faltosos.Any())
            {
                // Se existe um registro no conceito global marcado como "faltoso" e
                // tem nota lançada na disciplina, dispara excessão, pois não pode ter nota
                // em nenhuma disciplina.
                ACA_Aluno aluno = new ACA_Aluno
                {
                    alu_id = faltosos.First().alu_id
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

            //foreach (CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas)
            //{
            //    CLS_AlunoAvaliacaoTurmaDisciplina entDisciplina = item.entity;

            //    // Busca o registro para o aluno que esteja marcado como faltoso.
            //    CLS_AlunoAvaliacaoTurma entTurma = listaEntAvaliacao.Find(
            //        p => p.tur_id == tur_id
            //             && p.alu_id == entDisciplina.alu_id
            //             && p.mtu_id == entDisciplina.mtu_id
            //             && p.fav_id == entDisciplina.fav_id
            //             && p.ava_id == entDisciplina.ava_id
            //             && p.aat_faltoso);

            //    if (entTurma != null && !entTurma.IsNew &&
            //        (!string.IsNullOrEmpty(entDisciplina.atd_avaliacao)))
            //    {
            //        // Se existe um registro no conceito global marcado como "faltoso" e
            //        // tem nota lançada na disciplina, dispara excessão, pois não pode ter nota
            //        // em nenhuma disciplina.
            //        ACA_Aluno aluno = new ACA_Aluno
            //        {
            //            alu_id = entDisciplina.alu_id
            //        };
            //        ACA_AlunoBO.GetEntity(aluno, banco);

            //        PES_Pessoa pessoa = new PES_Pessoa
            //        {
            //            pes_id = aluno.pes_id
            //        };
            //        PES_PessoaBO.GetEntity(pessoa);

            //        throw new ValidationException("O aluno \"" + pessoa.pes_nome +
            //                                      "\" é faltoso e não deve possuir nota lançada nos(as) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + ", para essa avaliação.");
            //    }
            //}
        }

        #endregion

        #region Saves

        /// <summary>
        /// Salva as avaliações da lista.
        /// </summary>    
        /// <param name="entTurma"></param>
        /// <param name="tud_id"></param>
        /// <param name="entFormatoAvaliacao"></param>
        /// <param name="listaDisciplinas">Lista de notas dos alunos na disciplina</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo de arquivos</param>
        /// <param name="TiposArquivosPermitidos">Tipos de arquivos válidos</param>
        /// <param name="tipoFrequencia">Tipo de frequência do formato de avaliação</param>
        /// <param name="msgLancamentoFrequencia">Mensagem que será somente exibida (não impede a gravação de dados) relacionada à frequência mensal</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tipoCalculo">Cálculo de quantidade de aulas dadas do formato</param>
        /// <param name="tipoAvaliacao"></param>
        /// <param name="listAlunosComDivergenciaEmDisciplina"></param>
        /// <param name="entEscalaAvaliacao">Escala de avaliação da disciplina</param>
        /// <param name="listaNotaFinalAluno">Lista com as notas finais dos alunos</param>
        /// <param name="efetivacaoSemestral">Informa se o curso é por efetivação semestral</param>
        /// <param name="ava_id">ID da avaliação que está sendo salva</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static bool Save
        (
            TUR_Turma entTurma
            , long tud_id
            , ACA_FormatoAvaliacao entFormatoAvaliacao
            , List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinas
            , int tamanhoMaximoKB
            , string[] TiposArquivosPermitidos
            , ACA_FormatoAvaliacaoTipoLancamentoFrequencia tipoFrequencia
            , ACA_FormatoAvaliacaoCalculoQtdeAulasDadas tipoCalculo
            , out string msgLancamentoFrequencia
            , int tpc_id
            , AvaliacaoTipo tipoAvaliacao
            , out List<long> listAlunosComDivergenciaEmDisciplina
            , ACA_EscalaAvaliacao entEscalaAvaliacao
            , List<NotaFinalAlunoTurmaDisciplina> listaNotaFinalAluno
            , bool efetivacaoSemestral
            , int ava_id
            , bool notaPosConselho
            , TurmaDisciplinaTipo tipoDisciplina
            , bool temComponentesRegencia
            , Guid ent_id
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // se for avaliacao final analitica, pode ocorrer de salvar o fechamento do ultimo bimestre também
                int ava_idUltimoPeriodo = -1;
                int tpc_idUltimoPeriodo = -1;
                int tpc_idUtilizado = -1;
                List<AlunoFechamentoPendencia> FilaProcessamento = new List<AlunoFechamentoPendencia>();

                List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinasUltimoPeriodo = new List<CLS_AvaliacaoTurDisc_Cadastro>();
                if (tipoAvaliacao == AvaliacaoTipo.Final && entFormatoAvaliacao.fav_avaliacaoFinalAnalitica && listaDisciplinas.Count > 0)
                {
                    listaDisciplinasUltimoPeriodo = listaDisciplinas.Where(d => d.entity.ava_id != ava_id).ToList();
                    if (listaDisciplinasUltimoPeriodo.Count > 0)
                    {
                        ava_idUltimoPeriodo = listaDisciplinasUltimoPeriodo[0].entity.ava_id;
                        ACA_Avaliacao avaliacao = ACA_AvaliacaoBO.GetEntity(new ACA_Avaliacao { ava_id = ava_idUltimoPeriodo, fav_id = entFormatoAvaliacao.fav_id }, banco);
                        tpc_idUltimoPeriodo = avaliacao.tpc_id;
                    }
                    if (tpc_idUltimoPeriodo > 0)
                        tpc_idUtilizado = tpc_idUltimoPeriodo;
                    else
                        tpc_idUtilizado = ACA_AvaliacaoBO.SelecionaMaiorBimestre_ByFormatoAvaliacao(entFormatoAvaliacao.fav_id, banco);

                    if (temComponentesRegencia)
                    {
                        // Se for do tipo regencia, ordeno a lista de acordo com os componentes da regencia
                        if (listaDisciplinasUltimoPeriodo.Count > 0)
                        {
                            listaDisciplinas = listaDisciplinas.OrderBy(disciplina => disciplina.entity.tpc_id)
                                                .ThenBy(disciplina => disciplina.entity.ava_id)
                                                .ThenBy(disciplina => disciplina.entity.tud_id).ToList();
                        }
                        else
                        {
                            listaDisciplinas = listaDisciplinas.OrderBy(disciplina => disciplina.entity.tud_id).ToList();
                        }
                    }
                    else if (listaDisciplinasUltimoPeriodo.Count > 0)
                    {
                        // Senão, ordeno deixando primeiro a avaliação final
                        listaDisciplinas = listaDisciplinas.OrderBy(disciplina => disciplina.entity.tpc_id)
                                            .ThenBy(disciplina => disciplina.entity.ava_id).ToList();
                    }
                }
                else if (listaDisciplinas.Count > 0 && temComponentesRegencia)
                {
                    // Se for do tipo regencia, ordeno a lista de acordo com os componentes da regencia
                    listaDisciplinas = listaDisciplinas.OrderBy(disciplina => disciplina.entity.tud_id).ToList();
                }

                if (listaDisciplinas.Count > 0)
                {
                    // Valida se o formato e a avaliação estão de acordo com as regras
                    // do curso.
                    CLS_AlunoAvaliacaoTurmaBO.ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_id, banco);
                }
                if (ava_idUltimoPeriodo > 0)
                {
                    CLS_AlunoAvaliacaoTurmaBO.ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_idUltimoPeriodo, banco);
                }

                msgLancamentoFrequencia = "";
                listAlunosComDivergenciaEmDisciplina = new List<long>();

                List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular> listaUltimoPeriodoNota = new List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular>();
                if (efetivacaoSemestral)
                {
                    listaUltimoPeriodoNota = ACA_CurriculoControleSemestralDisciplinaPeriodoBO.SelecionaUltimoPeriodoNotaTurma(entTurma.tur_id);
                }

                // Verifica se não tem avaliação caso aluno esteja marcado como faltoso no lançamento global.
                if (ava_idUltimoPeriodo > 0)
                {
                    VerificaFaltoso(banco, entTurma.tur_id, listaDisciplinas.Where(d => d.entity.ava_id == ava_id).ToList(), entTurma.fav_id, ava_id);
                    VerificaFaltoso(banco, entTurma.tur_id, listaDisciplinasUltimoPeriodo, entTurma.fav_id, ava_idUltimoPeriodo);
                }
                else
                {
                    VerificaFaltoso(banco, entTurma.tur_id, listaDisciplinas, entTurma.fav_id, ava_id);
                }

                List<CLS_AlunoAvaliacaoTurmaDisciplina> listaCadastrados = SelecionaPor_DisciplinaAvaliacao(entTurma.tur_id, tud_id, entTurma.fav_id, ava_id, tipoDisciplina, banco);

                using (DataTable dtAlunoAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplina.TipoTabela_AlunoAvaliacaoTurmaDisciplina())
                {
                    object lockObject = new Object();

                    Parallel.ForEach
                    (
                        listaDisciplinas,
                        item =>
                        {
                            CLS_AlunoAvaliacaoTurmaDisciplina entity = item.entity;

                            // eh a avaliação periodica do ultimo bimestre que está salvando junto
                            // nesse caso, validar se ainda não existe registro na CLS_AlunoAvaliacaoTurmaDisciplina 
                            if (entity.ava_id != ava_id)
                            {
                                // [Carla 07/05] Coloquei NULL na variável de transação, para não causar erro (o paralelismo estava causando erro aqui).
                                listaCadastrados = SelecionaPor_DisciplinaAvaliacao(entTurma.tur_id, tud_id, entTurma.fav_id, entity.ava_id, tipoDisciplina, null);
                            }

                            if (entity.arq_idRelatorio > 0)
                            {
                                SYS_Arquivo arq = new SYS_Arquivo { arq_id = entity.arq_idRelatorio };
                                SYS_ArquivoBO.GetEntity(arq, banco);
                                arq.arq_situacao = (byte)SYS_ArquivoSituacao.Ativo;
                                SYS_ArquivoBO.Save(arq, tamanhoMaximoKB, TiposArquivosPermitidos, banco);
                            }

                            if (entity.Validate())
                            {
                                CLS_AlunoAvaliacaoTurmaDisciplina entAux = listaCadastrados.Find
                                    (p => p.tud_id == entity.tud_id
                                          && p.alu_id == entity.alu_id
                                          && p.mtu_id == entity.mtu_id
                                          && p.mtd_id == entity.mtd_id
                                          && p.fav_id == entity.fav_id
                                          && p.ava_id == entity.ava_id) ?? new CLS_AlunoAvaliacaoTurmaDisciplina();

                                // Verifica se existe algum arquivo de relatório para substituir o registro antigo.
                                if (entAux.arq_idRelatorio > 0)
                                {
                                    // Se for diferente do atual
                                    if (entAux.arq_idRelatorio != entity.arq_idRelatorio)
                                    {
                                        // Exclui do banco o registro anterior.
                                        SYS_Arquivo entityArquivo = new SYS_Arquivo { arq_id = entAux.arq_idRelatorio };
                                        SYS_ArquivoBO.GetEntity(entityArquivo, banco);
                                        SYS_ArquivoBO.Delete(entityArquivo, banco);
                                    }
                                }

                                if (entity.IsNew)
                                {
                                    if (entAux != null && !entAux.IsNew)
                                    {
                                        // Seta o id da entidade auxiliar, que já existe no banco.
                                        entity.IsNew = entAux.IsNew;
                                        entity.atd_id = entAux.atd_id;

                                        // No fechamento automatico a justificativa pos-conselho
                                        // é salva em outra tela.
                                        if (entFormatoAvaliacao.fav_fechamentoAutomatico)
                                        {
                                            entity.atd_justificativaPosConselho = entAux.atd_justificativaPosConselho;
                                        }
                                    }
                                }
                                lock (lockObject)
                                {
                                    DataRow drAlunoAvaliacaoTurmaDisciplina = dtAlunoAvaliacaoTurmaDisciplina.NewRow();
                                    dtAlunoAvaliacaoTurmaDisciplina.Rows.Add(EntityToDataRow(entity, drAlunoAvaliacaoTurmaDisciplina));
                                }
                            }
                            else
                            {
                                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
                            }
                        }
                    );

                    if (dtAlunoAvaliacaoTurmaDisciplina.Rows.Count > 0)
                    {
                        SalvarEmLote(dtAlunoAvaliacaoTurmaDisciplina, banco);

                        if (entFormatoAvaliacao.fav_fechamentoAutomatico)
                            FilaProcessamento.AddRange(listaDisciplinas
                                .Select(p => new AlunoFechamentoPendencia
                                {
                                    tud_id = p.entity.tud_id,
                                    tpc_id = (p.entity.tpc_id > 0 ? p.entity.tpc_id : tpc_idUltimoPeriodo),
                                    afp_frequencia = true,
                                    afp_nota = true,
                                    afp_processado = (Byte)(p.entity.atd_id <= 0 ? 0 : 2)
                                }).ToList());
                    }
                }

                // Lista de médias da disciplina.
                List<MediaNotaAlunos> listaMedias = null;
                // Lista de pareceres.
                List<ACA_EscalaAvaliacaoParecer> pareceres = null;

                Int64 tud_idAuxiliar = tud_id;

                listaDisciplinas.RemoveAll(p => p.entity.ava_id != ava_id);
                List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplina = MTR_MatriculaTurmaDisciplinaBO.CriarListaPorAvaliacaoTurmaDisciplina(listaDisciplinas, banco);

                List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplinaAnterior = new List<MTR_MatriculaTurmaDisciplina>();

                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                {
                    using (DataTable dtMatriculaAnterior = MTR_MatriculaTurmaDisciplina.TipoTabela_AlunoMatriculaTurmaDisciplina())
                    {
                        (from CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas
                         where item.mtu_idAnterior != item.entity.mtu_id
                         group item by new { item.entity.alu_id, item.mtu_idAnterior, item.mtd_idAnterior } into grupo
                         select grupo.Key).ToList()
                         .ForEach
                         (
                            p =>
                            {
                                DataRow drMatriculaAnterior = dtMatriculaAnterior.NewRow();

                                drMatriculaAnterior["alu_id"] = p.alu_id;
                                drMatriculaAnterior["mtu_id"] = p.mtu_idAnterior;
                                drMatriculaAnterior["mtd_id"] = p.mtd_idAnterior;

                                dtMatriculaAnterior.Rows.Add(drMatriculaAnterior);
                            }
                         );

                        ltMatriculaTurmaDisciplinaAnterior = MTR_MatriculaTurmaDisciplinaBO.SelecionaPorMatriculaTurmaDisciplina(dtMatriculaAnterior, banco);
                    }
                }

                foreach (MTR_MatriculaTurmaDisciplina entMatr in ltMatriculaTurmaDisciplina)
                {
                    if (listaDisciplinas.Any(p => p.entity.alu_id == entMatr.alu_id
                                                    && p.entity.mtu_id == entMatr.mtu_id
                                                    && p.entity.mtd_id == entMatr.mtd_id))
                    {

                        CLS_AvaliacaoTurDisc_Cadastro item = listaDisciplinas.Find(p => p.entity.alu_id == entMatr.alu_id
                                                                                        && p.entity.mtu_id == entMatr.mtu_id
                                                                                        && p.entity.mtd_id == entMatr.mtd_id);
                        CLS_AlunoAvaliacaoTurmaDisciplina entity = item.entity;

                        // se for a avaliação final salvo o registro no MTR_MatriculaTurmaDisciplina
                        if (entity.Validate())
                        {
                            if (tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal ||
                                tipoAvaliacao == AvaliacaoTipo.Final ||
                                tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal ||
                                // Caso seja o último período marcado na tela de configuração de Matriz Curricular
                                // também salva a nota final (cursos de aceleração).
                                (listaUltimoPeriodoNota.Count > 0 && item.entity.tud_id > 0 && listaUltimoPeriodoNota.Exists(p => p.tud_id == item.entity.tud_id && p.tpc_id == tpc_id)))
                            {
                                entMatr.mtd_resultado = Convert.ToByte(item.resultado);

                                // Considera a média do aluno a nota da avaliação do tipo final
                                if (tipoAvaliacao == AvaliacaoTipo.Final)
                                {
                                    entMatr.mtd_avaliacao = notaPosConselho && !string.IsNullOrEmpty(item.entity.atd_avaliacaoPosConselho) ?
                                                            item.entity.atd_avaliacaoPosConselho :
                                                            item.entity.atd_avaliacao;
                                }
                                else
                                {
                                    if (entEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                                    {
                                        if (pareceres == null)
                                        {
                                            // Só carrega a lista de pareceres uma vez.
                                            pareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(entEscalaAvaliacao.esa_id);
                                        }

                                        ACA_EscalaAvaliacaoParecer entityParecerAvaliacao = pareceres.Find(p => p.eap_valor.CompareTo(item.entity.atd_avaliacao) == 0) ?? new ACA_EscalaAvaliacaoParecer();

                                        if (tipoAvaliacao != AvaliacaoTipo.RecuperacaoFinal)
                                        {
                                            entMatr.mtd_avaliacao = notaPosConselho && !string.IsNullOrEmpty(item.entity.atd_avaliacaoPosConselho) ?
                                                                    item.entity.atd_avaliacaoPosConselho :
                                                                    item.entity.atd_avaliacao;
                                        }
                                        else
                                        {
                                            string notaFinal = string.Empty;
                                            if (listaNotaFinalAluno.Count > 0)
                                                notaFinal = listaNotaFinalAluno.Find(p => p.alu_id == entMatr.alu_id && p.mtu_id == entMatr.mtu_id && p.mtd_id == entMatr.mtd_id).nota;

                                            ACA_EscalaAvaliacaoParecer entityParecerNotaFinal = pareceres.Find(p => p.eap_valor.CompareTo(notaFinal) == 0) ?? new ACA_EscalaAvaliacaoParecer();

                                            entMatr.mtd_avaliacao = entityParecerAvaliacao.eap_ordem >= entityParecerNotaFinal.eap_ordem ? item.entity.atd_avaliacao : notaFinal;
                                        }
                                    }
                                    else if (entEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                                    {
                                        if (listaMedias == null || item.entity.tud_id != tud_idAuxiliar)
                                        {
                                            // A lista esta ordenada pelo id da disciplina, no caso da regencia + componentes da regencia
                                            tud_idAuxiliar = item.entity.tud_id;

                                            // Calcula e salva a média dos alunos na disciplina, uma única vez.
                                            listaMedias = MTR_MatriculaTurmaDisciplinaBO.CalculaNota_Media_PorTurmaDisciplina
                                                (entTurma.tur_id, tud_idAuxiliar, tpc_id);
                                        }
                                        string sMedia = listaMedias.Find(p => p.alu_id == entity.alu_id).media.ToString();

                                        string avaliacao = notaPosConselho && !string.IsNullOrEmpty(item.entity.atd_avaliacaoPosConselho) ?
                                                           item.entity.atd_avaliacaoPosConselho :
                                                           item.entity.atd_avaliacao;
                                        decimal media, notaRecuperacao;
                                        Decimal.TryParse(sMedia, out media);
                                        Decimal.TryParse(avaliacao, out notaRecuperacao);

                                        if (tipoAvaliacao != AvaliacaoTipo.RecuperacaoFinal)
                                        {
                                            entMatr.mtd_avaliacao = media.ToString();
                                        }
                                        else
                                        {
                                            entMatr.mtd_avaliacao = notaRecuperacao >= media ? notaRecuperacao.ToString() : media.ToString();
                                        }
                                    }

                                }

                                entMatr.mtd_frequencia = entity.atd_frequencia;
                                entMatr.mtd_relatorio = entity.atd_relatorio;

                                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal && entity.mtu_id != item.mtu_idAnterior &&
                                    ltMatriculaTurmaDisciplinaAnterior.Any(p => p.alu_id == entity.alu_id &&
                                                                                p.mtu_id == item.mtu_idAnterior &&
                                                                                p.mtd_id == item.mtd_idAnterior))
                                {
                                    MTR_MatriculaTurmaDisciplina entMatrAnterior = ltMatriculaTurmaDisciplinaAnterior.Find(p => p.alu_id == entity.alu_id &&
                                                                                                                                p.mtu_id == item.mtu_idAnterior &&
                                                                                                                                p.mtd_id == item.mtd_idAnterior);

                                    entMatrAnterior.mtd_avaliacao = entMatr.mtd_avaliacao;
                                    entMatrAnterior.mtd_frequencia = entMatr.mtd_frequencia;
                                    entMatrAnterior.mtd_resultado = entMatr.mtd_resultado;

                                    MTR_MatriculaTurmaDisciplinaBO.Save(entMatrAnterior, banco);
                                }

                                MTR_MatriculaTurmaDisciplinaBO.Save(entMatr, banco);
                            }
                        }
                    }
                }

                tpc_idUtilizado = tpc_idUtilizado > 0 ? tpc_idUtilizado : tpc_id;
                if (entFormatoAvaliacao.fav_fechamentoAutomatico && (tpc_idUtilizado > 0))
                    FilaProcessamento.AddRange(ltMatriculaTurmaDisciplina
                        .Select(p => new AlunoFechamentoPendencia
                        {
                            tud_id = p.tud_id,
                            tpc_id = tpc_idUtilizado,
                            afp_frequencia = true,
                            afp_nota = true,
                            afp_processado = (Byte)(p.mtd_id <= 0 ? 0 : 2)
                        }).ToList());

                string chave = string.Empty;

                if (tipoAvaliacao == AvaliacaoTipo.RecuperacaoFinal)
                {
                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina(tud_id, entFormatoAvaliacao.fav_id, ava_id);
                    CacheManager.Factory.Remove(chave);

                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia(tud_id, entFormatoAvaliacao.fav_id, ava_id);
                    CacheManager.Factory.Remove(chave);
                }
                // Esta salvando uma efetivacao do bimestre ou efetivacao final
                else
                {
                    // Limpa o cache da efetivacao do bimestre
                    if (tipoAvaliacao == AvaliacaoTipo.Periodica || tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal)
                    {
                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_id);
                            CacheManager.Factory.Remove(chave);
                        }

                        // Chaves do fechamento automatico
                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_id, tpc_id);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, tpc_id);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, entTurma.tur_id, tpc_id);
                            CacheManager.Factory.Remove(chave);
                        }
                        //

                        // Recupero o id da avaliacao final para limpar o cache
                        DataTable avaFinal = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(AvaliacaoTipo.Final, entFormatoAvaliacao.fav_id);
                        ava_id = avaFinal.Rows.Count == 0 ? -1 : Convert.ToInt32(avaFinal.Rows[0]["ava_id"]);
                    }
                    // No caso de ser a efetivacao final, limpo apenas o cache da efetivacao do ultimo bimestre
                    else if (ava_idUltimoPeriodo > 0)
                    {
                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, entFormatoAvaliacao.fav_id, ava_idUltimoPeriodo, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, entFormatoAvaliacao.fav_id, ava_idUltimoPeriodo, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_idUltimoPeriodo);
                            CacheManager.Factory.Remove(chave);
                        }

                        // Chaves do fechamento automatico
                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_id, tpc_idUltimoPeriodo);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, tpc_idUltimoPeriodo);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, entTurma.tur_id, tpc_idUltimoPeriodo);
                            CacheManager.Factory.Remove(chave);
                        }
                        //
                    }

                    // Limpa o cache da efetivacao final
                    if (ava_id > 0)
                    {
                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal(tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia(tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final(entTurma.tur_id, entFormatoAvaliacao.fav_id, ava_id);
                            CacheManager.Factory.Remove(chave);
                        }

                        // Chaves do fechamento automatico
                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_id);
                        CacheManager.Factory.RemoveByPattern(chave);

                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id);
                        CacheManager.Factory.RemoveByPattern(chave);

                        if (tipoDisciplina == TurmaDisciplinaTipo.Regencia)
                        {
                            chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, entTurma.tur_id);
                            CacheManager.Factory.Remove(chave);
                        }
                        //
                    }
                }

                if (entFormatoAvaliacao.fav_fechamentoAutomatico && FilaProcessamento.Any(a => a.tpc_id > 0))
                {
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                        FilaProcessamento
                          .GroupBy(g => new { g.tud_id, g.tpc_id })
                          .Select(p => new AlunoFechamentoPendencia
                          {
                              tud_id = p.FirstOrDefault().tud_id,
                              tpc_id = p.FirstOrDefault().tpc_id,
                              afp_frequencia = p.FirstOrDefault().afp_frequencia,
                              afp_nota = p.FirstOrDefault().afp_nota,
                              afp_processado = FilaProcessamento
                                .Where(w => w.tud_id == p.FirstOrDefault().tud_id && w.tpc_id == p.FirstOrDefault().tpc_id)
                                .Min(m => m.afp_processado)
                          })
                          .Where(w => w.tpc_id > 0 && w.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                          .ToList()
                      , banco);
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
        /// se já existe uma entidade para a turma / disciplina / aluno / avaliação. 
        /// Se já existir, seta os valores e altera o registro que já existe, 
        /// ao invés de inserir. Não pode salvar mais de uma avaliação por aluno na 
        /// turmaDisciplina.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="listaCadastrados">Lista de entidades cadastradas na disciplina/avaliação</param>
        /// <returns>Se salvou com sucesso</returns>
        public static bool Save(CLS_AlunoAvaliacaoTurmaDisciplina entity, TalkDBTransaction banco, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaCadastrados)
        {
            if (!entity.Validate())
                throw new ValidationException(UtilBO.ErrosValidacao(entity));

            if (entity.IsNew)
            {
                // Se for nova entidade - verificar se já não existe um registro com a chave
                // do lançamento de notas para esse aluno - se já foi lançado, deve
                // ser alterado o registro para não duplicar.
                CLS_AlunoAvaliacaoTurmaDisciplina entAux =
                    listaCadastrados.Find(p =>
                    p.tud_id == entity.tud_id
                    && p.alu_id == entity.alu_id
                    && p.mtu_id == entity.mtu_id
                    && p.mtd_id == entity.mtd_id
                    && p.fav_id == entity.fav_id
                    && p.ava_id == entity.ava_id);

                if (entAux != null && !entAux.IsNew)
                {
                    // Seta o id da entidade auxiliar, que já existe no banco.
                    entity.IsNew = entAux.IsNew;
                    entity.atd_id = entAux.atd_id;
                }
            }

            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO
            {
                _Banco = banco
            };

            return dao.Salvar(entity);
        }

        /// <summary>
        /// Override do Save passando o banco - se a entidade for nova, faz uma verificação
        /// se já existe uma entidade para a turma / disciplina / aluno / avaliação. 
        /// Se já existir, seta os valores e altera o registro que já existe, 
        /// ao invés de inserir. Não pode salvar mais de uma avaliação por aluno na 
        /// turmaDisciplina.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns>Se salvou com sucesso</returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplina entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
                throw new ValidationException(UtilBO.ErrosValidacao(entity));

            if (entity.IsNew)
            {
                // Se for nova entidade - verificar se já não existe um registro com a chave
                // do lançamento de notas para esse aluno - se já foi lançado, deve
                // ser alterado o registro para não duplicar.
                CLS_AlunoAvaliacaoTurmaDisciplina entAux = GetEntityBy_ChaveAvaliacaoAluno
                    (
                        entity.tud_id
                        , entity.alu_id
                        , entity.mtu_id
                        , entity.mtd_id
                        , entity.fav_id
                        , entity.ava_id
                        , banco
                    );

                if (!entAux.IsNew)
                {
                    // Seta o id da entidade auxiliar, que já existe no banco.
                    entity.IsNew = entAux.IsNew;
                    entity.atd_id = entAux.atd_id;
                }
            }

            CLS_AlunoAvaliacaoTurmaDisciplinaDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO
            {
                _Banco = banco
            };

            return dao.Salvar(entity);
        }

        /// <summary>
        /// Salva as avaliações da lista.
        /// </summary>    
        /// <param name="tur_id">ID da turma</param>
        /// <param name="entFormatoAvaliacao">Formato de avaliação</param>
        /// <param name="listaDisciplinas">Lista de notas do aluno nas disciplinas</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo de arquivos</param>
        /// <param name="TiposArquivosPermitidos">Tipos de arquivos válidos</param> 
        /// <param name="alterarResultado">Informa se deve alterar o valor do resultado</param>
        /// <param name="dataUltimaAlteracao">Data da alteração mais recente, no momento em que os dados foram carregados</param>
        /// <param name="listaAtualizacaoEfetivacao">Retorna a lista de CLS_AlunoAvaliacaoTurmaDisciplina que foram atualizadas</param>
        /// <returns></returns>
        public static bool SaveAvaliacaoFinal
        (
            long tur_id
            , ACA_FormatoAvaliacao entFormatoAvaliacao
            , List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinas
            , int tamanhoMaximoKB
            , string[] TiposArquivosPermitidos
            , DateTime dataUltimaAlteracao
            , List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina
            , ref List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao
            , TalkDBTransaction banco
        )
        {
            if (listaDisciplinas.Count > 0)
            {
                int ava_id = listaDisciplinas[0].entity.ava_id;

                TUR_Turma entTurma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = tur_id });

                // Valida se o formato e a avaliação estão de acordo com as regras
                // do curso.
                CLS_AlunoAvaliacaoTurmaBO.ValidaRegrasCurso(entTurma, entFormatoAvaliacao, ava_id, banco);

                // Verifica se não tem avaliação caso aluno esteja marcado como faltoso no lançamento global.
                VerificaFaltoso(banco, entTurma.tur_id, listaDisciplinas, entTurma.fav_id, ava_id);

                DataTable dtAlunoAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplina.TipoTabela_AlunoAvaliacaoTurmaDisciplina();
                foreach (CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas)
                {
                    DataRow drAlunoAvaliacaoTurmaDisciplina = dtAlunoAvaliacaoTurmaDisciplina.NewRow();
                    dtAlunoAvaliacaoTurmaDisciplina.Rows.Add(EntityToDataRow(item.entity, drAlunoAvaliacaoTurmaDisciplina));
                }
                List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAlunoAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplinaBO.GetEntity_EmLote(dtAlunoAvaliacaoTurmaDisciplina, banco);

                foreach (CLS_AvaliacaoTurDisc_Cadastro item in listaDisciplinas)
                {
                    CLS_AlunoAvaliacaoTurmaDisciplina entity = item.entity;

                    if (entity.arq_idRelatorio > 0)
                    {
                        SYS_Arquivo arq = new SYS_Arquivo { arq_id = entity.arq_idRelatorio };
                        SYS_ArquivoBO.GetEntity(arq, banco);
                        arq.arq_situacao = (byte)SYS_ArquivoSituacao.Ativo;
                        SYS_ArquivoBO.Save(arq, tamanhoMaximoKB, TiposArquivosPermitidos, banco);
                    }

                    if (entity.Validate())
                    {
                        CLS_AlunoAvaliacaoTurmaDisciplina entAux = listaAlunoAvaliacaoTurmaDisciplina.Find(p => p.tud_id == entity.tud_id && p.alu_id == entity.alu_id && p.mtu_id == entity.mtu_id && p.mtd_id == entity.mtd_id && p.fav_id == entity.fav_id && p.ava_id == entity.ava_id);

                        // Se o registro foi alterado depois da data da alteração mais recente no momento em que os dados foram carregados,
                        // interrompe o salvamento e alerta o usuário de que é necessário atualizar os dados 
                        if (entAux != null && !entAux.IsNew && Convert.ToDateTime(entAux.atd_dataAlteracao.ToString()) > dataUltimaAlteracao)
                        {
                            throw new ValidationException("Existe registro alterado mais recentemente, é necessário atualizar os dados.");
                        }
                        else
                        {
                            // Verifica se existe algum arquivo de relatório para substituir o registro antigo.
                            if (entAux.arq_idRelatorio > 0)
                            {
                                // Se for diferente do atual
                                if (entAux.arq_idRelatorio != entity.arq_idRelatorio)
                                {
                                    // Exclui do banco o registro anterior.
                                    SYS_Arquivo entityArquivo = new SYS_Arquivo { arq_id = entAux.arq_idRelatorio };
                                    SYS_ArquivoBO.GetEntity(entityArquivo, banco);
                                    SYS_ArquivoBO.Delete(entityArquivo, banco);
                                }
                            }

                            if (entity.atd_situacao == 3 && entity.atd_id > 0)
                            {
                                Delete(entity, banco);
                            }
                            else
                            {
                                // Salva, verificando existência do registro a partir da lista.
                                Save(entity, banco);

                                listaAtualizacaoEfetivacao.Add(entity);
                            }
                        }
                    }
                    else
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
                    }

                    string chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal(entity.tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                    CacheManager.Factory.RemoveByPattern(chave);

                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia(entity.tud_id, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                    CacheManager.Factory.RemoveByPattern(chave);
                }

                string chaveComponentes = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final(tur_id, entFormatoAvaliacao.fav_id, ava_id);
                CacheManager.Factory.Remove(chaveComponentes);

                listaMatriculaTurmaDisciplina.GroupBy(p => p.tud_id)
                                             .Select(p => p.Key)
                                             .ToList()
                                             .ForEach
                                             (
                                                p =>
                                                {
                                                    string chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal(p, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                                                    CacheManager.Factory.RemoveByPattern(chave);

                                                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia(p, entFormatoAvaliacao.fav_id, ava_id, string.Empty);
                                                    CacheManager.Factory.RemoveByPattern(chave);
                                                }
                                             );

                if (HttpContext.Current != null)
                {
                    string chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Final(tur_id, entFormatoAvaliacao.fav_id, ava_id);
                    HttpContext.Current.Cache.Remove(chave);
                }

                List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplina = MTR_MatriculaTurmaDisciplinaBO.CriarListaPorAvaliacaoTurmaDisciplina(listaDisciplinas, banco);

                foreach (MTR_MatriculaTurmaDisciplina entMatr in ltMatriculaTurmaDisciplina)
                {
                    CLS_AvaliacaoTurDisc_Cadastro item = listaDisciplinas.Any(p => p.entity.alu_id == entMatr.alu_id &&
                                                                                   p.entity.mtu_id == entMatr.mtu_id &&
                                                                                   p.entity.mtd_id == entMatr.mtd_id) ?
                                                         listaDisciplinas.Find(p => p.entity.alu_id == entMatr.alu_id &&
                                                                                    p.entity.mtu_id == entMatr.mtu_id &&
                                                                                    p.entity.mtd_id == entMatr.mtd_id) :
                                                         new CLS_AvaliacaoTurDisc_Cadastro { entity = new CLS_AlunoAvaliacaoTurmaDisciplina() };

                    CLS_AlunoAvaliacaoTurmaDisciplina entity = item.entity;

                    if (entity.Validate())
                    {
                        if (item.atualizarResultado)
                        {
                            entMatr.mtd_resultado = Convert.ToByte(item.resultado);
                        }
                        else if (entMatr.IsNew)
                        {
                            entMatr.mtd_resultado = 0;
                        }

                        // Considera a média do aluno a nota da avaliação do tipo final
                        entMatr.mtd_avaliacao = entity.atd_avaliacao;

                        entMatr.mtd_frequencia = entity.atd_frequencia;
                        entMatr.mtd_relatorio = entity.atd_relatorio;
                        entMatr.apenasResultado = false;
                    }
                }

                ltMatriculaTurmaDisciplina.AddRange(listaMatriculaTurmaDisciplina);

                MTR_MatriculaTurmaDisciplinaBO.AtualizarResultado(ltMatriculaTurmaDisciplina, banco);

                return true;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Salva uma lista de avaliação nas disciplinas.
        /// </summary>
        /// <param name="listaAvaliacao">Lista de avaliações</param>
        /// <param name="listaAlunoProjeto">Lista de resultado em projetos complementares.</param>
        /// <returns></returns>
        public static bool SalvarListaAvaliacaoTransferencia
        (
            List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAvaliacao,
            List<CLS_AlunoProjeto> listaAlunoProjeto
        )
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaDisciplinaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                listaAvaliacao.Aggregate(true, (salvou, avaliacao) => salvou & Save(avaliacao, banco));
                listaAlunoProjeto.Aggregate(true, (salvou, projeto) => salvou & CLS_AlunoProjetoBO.Save(projeto, banco));

                return true;
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

        #region Importacao de dados em lote da efetivacao

        /// <summary>
        /// Salva os dados obtidos na importação de dados da efetivação de bimestre.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable de dados do fechamento.</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static bool ImportarDadosFechamento(DataTable dtAlunoAvaliacaoTurmaDisciplina, Int64 tpc_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                   new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().ImportarDadosFechamento(dtAlunoAvaliacaoTurmaDisciplina, tpc_id) :
                   new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = banco }.ImportarDadosFechamento(dtAlunoAvaliacaoTurmaDisciplina, tpc_id);
        }

        #endregion Importacao de dados em lote da efetivacao

        #endregion Saves

        #region Métodos em lote

        /// <summary>
        /// O método retorna um DataRow com as informações da entidade.
        /// </summary>
        /// <param name="entity">Entidade de avaliação do aluno.</param>
        /// <param name="dr">DataRow de avaliação.</param>
        /// <returns></returns>
        public static DataRow EntityToDataRow(CLS_AlunoAvaliacaoTurmaDisciplina entity, DataRow dr)
        {
            dr["tud_id"] = entity.tud_id;
            dr["alu_id"] = entity.alu_id;
            dr["mtu_id"] = entity.mtu_id;
            dr["mtd_id"] = entity.mtd_id;
            dr["atd_id"] = entity.atd_id;
            dr["fav_id"] = entity.fav_id;
            dr["ava_id"] = entity.ava_id;

            if (!string.IsNullOrEmpty(entity.atd_avaliacao))
            {
                dr["atd_avaliacao"] = entity.atd_avaliacao;
            }
            else
            {
                dr["atd_avaliacao"] = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(entity.atd_avaliacaoPosConselho))
            {
                dr["atd_avaliacaoPosConselho"] = entity.atd_avaliacaoPosConselho;
            }
            else
            {
                dr["atd_avaliacaoPosConselho"] = DBNull.Value;
            }

            if (entity.atd_numeroAulas > -1)
            {
                dr["atd_numeroAulas"] = entity.atd_numeroAulas;
            }
            else
            {
                dr["atd_numeroAulas"] = DBNull.Value;
            }

            if (entity.atd_numeroFaltas > -1)
            {
                dr["atd_numeroFaltas"] = entity.atd_numeroFaltas;
            }
            else
            {
                dr["atd_numeroFaltas"] = DBNull.Value;
            }

            if (entity.atd_ausenciasCompensadas > -1)
            {
                dr["atd_ausenciasCompensadas"] = entity.atd_ausenciasCompensadas;
            }
            else
            {
                dr["atd_ausenciasCompensadas"] = DBNull.Value;
            }

            dr["atd_registroexterno"] = entity.atd_registroexterno;

            if (entity.atd_frequencia > -1)
            {
                dr["atd_frequencia"] = entity.atd_frequencia;
            }
            else
            {
                dr["atd_frequencia"] = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(entity.atd_comentarios))
            {
                dr["atd_comentarios"] = entity.atd_comentarios;
            }
            else
            {
                dr["atd_comentarios"] = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(entity.atd_relatorio))
            {
                dr["atd_relatorio"] = entity.atd_relatorio;
            }
            else
            {
                dr["atd_relatorio"] = DBNull.Value;
            }

            dr["atd_semProfessor"] = entity.atd_semProfessor;
            dr["atd_situacao"] = entity.atd_situacao;

            if (entity.arq_idRelatorio > 0)
            {
                dr["arq_idRelatorio"] = entity.arq_idRelatorio;
            }
            else
            {
                dr["arq_idRelatorio"] = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(entity.atd_justificativaPosConselho))
            {
                dr["atd_justificativaPosConselho"] = entity.atd_justificativaPosConselho;
            }
            else
            {
                dr["atd_justificativaPosConselho"] = DBNull.Value;
            }

            if (entity.atd_frequenciaFinalAjustada > -1)
            {
                dr["atd_frequenciaFinalAjustada"] = entity.atd_frequenciaFinalAjustada;
            }
            else
            {
                dr["atd_frequenciaFinalAjustada"] = DBNull.Value;
            }

            if (entity.atd_numeroAulasReposicao > -1)
            {
                dr["atd_numeroAulasReposicao"] = entity.atd_numeroAulasReposicao;
            }
            else
            {
                dr["atd_numeroAulasReposicao"] = DBNull.Value;
            }

            if (entity.atd_numeroFaltasReposicao > -1)
            {
                dr["atd_numeroFaltasReposicao"] = entity.atd_numeroFaltasReposicao;
            }
            else
            {
                dr["atd_numeroFaltasReposicao"] = DBNull.Value;
            }

            if (entity.atd_numeroAulasExterna > -1)
            {
                dr["atd_numeroAulasExterna"] = entity.atd_numeroAulasExterna;
            }
            else
            {
                dr["atd_numeroAulasExterna"] = DBNull.Value;
            }

            if (entity.atd_numeroFaltasExterna > -1)
            {
                dr["atd_numeroFaltasExterna"] = entity.atd_numeroFaltasExterna;
            }
            else
            {
                dr["atd_numeroFaltasExterna"] = DBNull.Value;
            }

            if (entity.atd_numeroAtividadeExtraclasse > 0)
            {
                dr["atd_numeroAtividadeExtraclasse"] = entity.atd_numeroAtividadeExtraclasse;
            }
            else
            {
                dr["atd_numeroAtividadeExtraclasse"] = DBNull.Value;
            }

            return dr;
        }

        /// <summary>
        /// Salva os dados do fechamento de bimestre em lote.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable com os dados do fechamento do bimestre.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(DataTable dtAlunoAvaliacaoTurmaDisciplina, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().SalvarEmLote(dtAlunoAvaliacaoTurmaDisciplina) :
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = banco }.SalvarEmLote(dtAlunoAvaliacaoTurmaDisciplina);
        }

        /// <summary>
        /// Salva os dados do fechamento de bimestre do gestor em lote.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable com os dados do fechamento do bimestre.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarEmLotePosConselho(DataTable dtAlunoAvaliacaoTurmaDisciplina, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().SalvarEmLotePosConselho(dtAlunoAvaliacaoTurmaDisciplina) :
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = banco }.SalvarEmLotePosConselho(dtAlunoAvaliacaoTurmaDisciplina);
        }


        /// <summary>
        /// Retorna do banco uma lista de entidades com as chaves passadas na tabela
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Tabela com as chaves da tabela CLS_AlunoAvaliacaoTurmaDisciplina</param>
        /// <param name="banco">Transacao do banco</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplina> GetEntity_EmLote
        (
           DataTable dtAlunoAvaliacaoTurmaDisciplina,
           TalkDBTransaction banco = null
        )
        {
            return banco == null ?
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO().GetEntity_EmLote(dtAlunoAvaliacaoTurmaDisciplina) :
                new CLS_AlunoAvaliacaoTurmaDisciplinaDAO { _Banco = banco }.GetEntity_EmLote(dtAlunoAvaliacaoTurmaDisciplina);
        }
        #endregion
    }
}
