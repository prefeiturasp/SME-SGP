using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situação do docente na disciplina.
    /// </summary>
    public enum TUR_TurmaDocenteSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
        Inativo = 4
    }

    #endregion Enumeradores

    #region Estruturas

    /// <summary>
    /// Estrutura para guardar dados de atribuições que devem bloquear a criação de aulas fora do período da vigência.
    /// </summary>
    [Serializable]
    public struct VigenciaCriacaoAulas
    {
        public byte crg_tipo { get; set; }
        public long tud_id { get; set; }
        public int tdt_id { get; set; }
        public DateTime tdt_vigenciaInicio { get; set; }
        public DateTime tdt_vigenciaFim { get; set; }
    }

    #endregion Estruturas

    public class TUR_TurmaDocenteBO : BusinessBase<TUR_TurmaDocenteDAO, TUR_TurmaDocente>
    {
        #region Consultas

        /// <summary>
        /// Retorna as atribuições de docentes criadas pelo cargo especificado (atribuiçao esporádica).
        /// </summary>
        public static List<TUR_TurmaDocente> PesquisaPor_AtribuicaoEsporadica
        (
             long col_id
            , int crg_id
            , int coc_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO { _Banco = banco };
            DataTable dt = dao.PesquisaPor_AtribuicaoEsporadica(col_id, crg_id, coc_id);
            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new TUR_TurmaDocente())).ToList();
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        public static void LimpaCache(TUR_TurmaDocente entity, Guid ent_id, Int64 tur_id = 0)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelecionaPosicaoPorDocenteTurma(entity.doc_id, entity.tud_id));
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelecionaPosicaoPorDocenteTurma_ComInativos(entity.doc_id, entity.tud_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaDisciplinaPorTurmaDocente, entity.doc_id.ToString());
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaDisciplinaPorTurmaDocente);
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(entity.tud_id, entity.doc_id.ToString()));
                CacheManager.Factory.RemoveByPattern(ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY);
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaBO.RetornaChaveCache_SelecionaPorDocenteControleTurma(ent_id.ToString(), entity.doc_id.ToString()));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(entity.tud_id, entity.doc_id.ToString()));
                TUR_TurmaBO.RemoveCacheDocenteControleTurma(ent_id, entity.doc_id);
                if (tur_id > 0)
                    TUR_TurmaBO.RemoveCacheDocente_TurmaDisciplina(tur_id, entity.doc_id, ent_id);
                else
                    GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_GetSelectBy_TurmaDocente, entity.doc_id.ToString());

                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(entity.tud_id, ""));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(entity.tud_id, entity.doc_id.ToString()));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaBO.RetornaChaveCache_DocenteControleTurmas(ent_id, entity.doc_id));
                CacheManager.Factory.RemoveByPattern(string.Format("{0}_{1}_{2}",
                                                     ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY,
                                                     tur_id, entity.doc_id));

                if (ent_id != Guid.Empty)
                {
                    GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(entity.doc_id, ent_id, 1));
                    GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(entity.doc_id, ent_id, 0));
                }
            }
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPosicaoPorDocenteTurma(long doc_id, long tud_id)
        {
            return string.Format("Cache_SelecionaPosicaoPorDocenteTurma_{0}_{1}", doc_id, tud_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar a última posição do docente.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPosicaoPorDocenteTurma_ComInativos(long doc_id, long tud_id)
        {
            return string.Format("Cache_SelecionaPosicaoPorDocenteTurma_ComInativos_{0}_{1}", doc_id, tud_id);
        }


        /// <summary>
        /// Transfere todos os lançamentos para a posição 1.
        /// </summary>
        /// <param name="tud_id"> Id da disciplina.</param>
        /// <param name="banco">Transação com o banco.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static void TransferirLancamentos_Posicao
        (
            long tud_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();

            if (banco != null)
            {
                dao._Banco = banco;
            }

            dao.TransferirLancamentos_Posicao(tud_id);
        }

        /// <summary>
        /// Retorna os docentes da disciplina da turma
        /// </summary>
        /// <param name="tur_id">ID da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDocentesPorTurma
        (
            long tur_id
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            return dao.SelectBy_Turma(tur_id);
        }

        /// <summary>
        /// Retorna os docentes das disciplinas.
        /// </summary>
        /// <param name="tud_id">String contendo os ids das disciplinas</param>
        public static List<TUR_Turma_Docentes_Disciplina> SelecionaDocentesDisciplina
        (
            string tud_id
        )
        {
            return SelecionaDocentesDisciplina(tud_id, null);
        }

        /// <summary>
        /// Retorna os nomes dos docentes para cada posição da disciplina.
        /// </summary>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <returns>Lista de nomes dos docentes.</returns>
        public static List<KeyValuePair<int, string>> SelecionaDocentesPosicaoPorDisciplina(long tud_id)
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            DataTable dt = dao.SelecionaDocentePosicaoPorDisciplina(tud_id);

            List<KeyValuePair<int, string>> docentesPorPosicao = new List<KeyValuePair<int, string>>();

            foreach (DataRow dr in dt.Rows)
            {
                KeyValuePair<int, string> nomeDocente = new KeyValuePair<int, string>(Convert.ToInt32(dr["tdt_posicao"]), dr["pes_nome"].ToString());
                docentesPorPosicao.Add(nomeDocente);
            }

            return docentesPorPosicao;
        }

        /// <summary>
        /// Retorna os docentes das disciplinas.
        /// </summary>
        /// <param name="tud_id">String contendo os ids das disciplinas</param>
        /// <param name="banco">Transação com banco - opcional</param>
        public static List<TUR_Turma_Docentes_Disciplina> SelecionaDocentesDisciplina
        (
            string tud_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            if (banco != null)
                dao._Banco = banco;

            DataTable dt = string.IsNullOrEmpty(tud_id) ? new DataTable()
                : dao.SelecionaDocentesDisciplinas(tud_id);

            return (from DataRow row in dt.Rows
                    select new TUR_Turma_Docentes_Disciplina
                               {
                                   entDocente = dao.DataRowToEntity(row, new TUR_TurmaDocente())
                                    ,
                                   doc_nome = row["doc_nome"].ToString()
                               }).ToList();
        }

        /// <summary>
        /// Retorna vigências do docentes da disciplina
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        public static DataTable SelecionaVigenciasDocentesPorDisciplina(long tur_id)
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            return dao.SelecionaVigenciasDocentesPorDisciplina(tur_id);
        }

        /// <summary>
        /// Retorna a posição do docente nas TurmaDisciplinas.
        /// </summary>
        /// <param name="tud_ids">Ids das TurmaDisciplinas.</param>
        /// <returns>Posição do docente nas TurmaDisciplinas.</returns>
        public static List<TUR_TurmaDocente> SelecionaPosicaoPorTudIds(string tud_ids)
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            return dao.SelecionaPosicaoPorTudIds(tud_ids);
        }

        /// <summary>
        /// Retorna a última posição do docente na disciplina, como ativo ou inativo.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <returns></returns>
        public static byte SelecionaPosicaoPorDocenteTurma_ComInativos(long doc_id, long tud_id
            , out bool AtribuicaoAtiva, int appMinutosCacheLongo = 0)
        {
            KeyValuePair<byte, bool> dados = new KeyValuePair<byte, bool>(0, false);
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();

            if (appMinutosCacheLongo > 0)
            {
                    string chave = RetornaChaveCache_SelecionaPosicaoPorDocenteTurma_ComInativos(doc_id, tud_id);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () => { return dao.SelectPosicaoByDocenteTurma_ComInativos(doc_id, tud_id); },
                                appMinutosCacheLongo
                            );
                    }
                    else
                dados = dao.SelectPosicaoByDocenteTurma_ComInativos(doc_id, tud_id);

            AtribuicaoAtiva = dados.Value;

            return dados.Key;
        }

        /// <summary>
        /// Retorna a posição do docente na turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>Posição do docente na turma disciplina</returns>
        public static byte SelecionaPosicaoPorDocenteTurma(long doc_id, long tud_id, int appMinutosCacheLongo = 0)
        {
            byte dados;
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();

            if (appMinutosCacheLongo > 0)
            {
                    string chave = RetornaChaveCache_SelecionaPosicaoPorDocenteTurma(doc_id, tud_id);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () => { return dao.SelectPosicaoByDocenteTurma(doc_id, tud_id); },
                                appMinutosCacheLongo
                            );
                    }
                    else
                dados = dao.SelectPosicaoByDocenteTurma(doc_id, tud_id);

            return dados;
        }

        /// <summary>
        /// Seleciona as turmas em que o professor leciona determinada disciplina, exceto na turma passa por parâmetro.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorTurmaDisciplinaPosicao(long tur_id, int cal_id, int cur_id, int crr_id, int crp_id, long tud_id, byte tdt_posicao)
        {
            return new TUR_TurmaDocenteDAO().SelecionaPorTurmaDisciplinaPosicao(tur_id, cal_id, cur_id, crr_id, crp_id, tud_id, tdt_posicao);
        }

        /// <summary>
        /// Retorna os docentes das disciplinas para a tela de atribuição de docentes
        /// </summary>
        /// <param name="tur_id">ID da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtribuicaoDocentes
        (
            long tur_id,
            long doc_id,
            long tud_id_Compartilhada
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            return dao.SelecionaAtribuicaoDocentes(tur_id, doc_id, tud_id_Compartilhada);
        }

        /// <summary>
        /// Retorna o id da turma disciplina de docencia compartilhada,
        /// cujo docente é o titular.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public static Int64 SelectTitularDisciplinaDocenciaCompartilhada(long tur_id, long doc_id)
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            return dao.SelectTitularDisciplinaDocenciaCompartilhada(tur_id, doc_id);
        }

        public static DataTable SelecionaDocentesAtivos(int esc_id, int cur_id, int crr_id, int crp_id, long tur_id)
        {
            return new TUR_TurmaDocenteDAO().SelecionaDocentesAtivos(esc_id, cur_id, crr_id, crp_id, tur_id);
        }

        #endregion Consultas

        #region Saves

        /// <summary>
        /// Salva as entidades dos docentes para a disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cad">Estrutura com dados da disciplina e lista de docentes</param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="listaDocentesTodasDisciplinas">Lista de todos os docentes cadastrados para todas as disciplinas da turma - para verificar os que irá excluir</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        public static void SalvarDocentesDisciplina(long tur_id
                                                    , CadastroTurmaDisciplina cad
                                                    , TalkDBTransaction banco
                                                    , List<TUR_Turma_Docentes_Disciplina> listaDocentesTodasDisciplinas
                                                    , Guid ent_id)
        {
            // Pegar os docentes que existiam antes na disciplina.
            List<TUR_Turma_Docentes_Disciplina> docentesCadastrados =
                (from TUR_Turma_Docentes_Disciplina item in listaDocentesTodasDisciplinas
                 where item.entDocente.tud_id == cad.entTurmaDisciplina.tud_id
                 select item
                ).ToList();

            foreach (TUR_Turma_Docentes_Disciplina docente in cad.listaTurmaDocente)
            {
                DateTime VigenciaFinalDocente = docente.entDocente.tdt_vigenciaFim == new DateTime() ? DateTime.MaxValue : docente.entDocente.tdt_vigenciaFim;

                // Validar se existem vigências com conflito de data.
                var x = from TUR_Turma_Docentes_Disciplina item in cad.listaTurmaDocente
                        where
                            GestaoEscolarUtilBO.ExisteConflitoDatas
                                (item.entDocente.tdt_vigenciaInicio, item.entDocente.tdt_vigenciaFim == new DateTime() ? DateTime.MaxValue : item.entDocente.tdt_vigenciaFim
                                 , docente.entDocente.tdt_vigenciaInicio, VigenciaFinalDocente)
                            && item.indice != docente.indice
                            && item.entDocente.tdt_posicao == docente.entDocente.tdt_posicao
                        select item;

                if (x.Count() > 0)
                {
                    throw new ValidationException(
                        "Existe um conflito de datas da vigência do docente \"" + docente.doc_nome + "\".");
                }

                // Verifica se o mesmo docente está vigente em mais de uma posição.
                var y = from TUR_Turma_Docentes_Disciplina item in cad.listaTurmaDocente
                        where
                            GestaoEscolarUtilBO.ExisteConflitoDatas
                                (item.entDocente.tdt_vigenciaInicio, item.entDocente.tdt_vigenciaFim == new DateTime() ? DateTime.MaxValue : item.entDocente.tdt_vigenciaFim
                                 , docente.entDocente.tdt_vigenciaInicio, VigenciaFinalDocente)
                            && item.entDocente.tdt_posicao != docente.entDocente.tdt_posicao
                            && item.entDocente.doc_id == docente.entDocente.doc_id
                            && item.entDocente.coc_id == docente.entDocente.coc_id
                            && item.entDocente.col_id == docente.entDocente.col_id
                            && item.entDocente.crg_id == docente.entDocente.crg_id
                        select item;

                if (y.Count() > 0)
                    throw new ValidationException(
                        "O docente \"" + docente.doc_nome + "\" não pode estar vigente em mais de uma posição.");

                // Remove do cache as turmas do docente.
                TUR_TurmaBO.RemoveCacheDocenteControleTurma(ent_id, docente.entDocente.doc_id);
                TUR_TurmaBO.RemoveCacheDocente_TurmaDisciplina(tur_id, docente.entDocente.doc_id, ent_id);

                TUR_TurmaDocente entTurmaDocente = docente.entDocente;
                entTurmaDocente.tud_id = cad.entTurmaDisciplina.tud_id;

                LimpaCache(entTurmaDocente, ent_id);

                // Salvar entidade.
                Save(entTurmaDocente, banco);
            }

            // Se remover docente, transfere os lançamentos.
            if (!cad.listaTurmaDocente.Exists(p => (p.entDocente.tdt_posicao > 1 && p.entDocente.tdt_situacao == 1)) &&
                ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ALTERAR_AULAS_PARA_TITULAR_ATRIBUICAO_DOCENTES, ent_id))
            {
                TransferirLancamentos_Posicao(cad.entTurmaDisciplina.tud_id, banco);
            }

            foreach (TUR_Turma_Docentes_Disciplina docente in docentesCadastrados)
            {
                // Verificar se o docente existia antes e não existe mais.
                if (!cad.listaTurmaDocente.Exists(p =>
                        (p.entDocente.tud_id == docente.entDocente.tud_id
                        && p.entDocente.tdt_id == docente.entDocente.tdt_id)))
                {
                    // Excluir.
                    Delete(docente.entDocente, banco);
                }
            }
        }

        /// <summary>
        /// Salva a entidade validando ela, e setando a situação de acordo com a vigência.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static new bool Save(TUR_TurmaDocente entity, TalkDBTransaction banco)
        {
            // Validar entidade.
            if (!entity.Validate())
            {
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
            }

            if (entity.tdt_situacao != (byte)TUR_TurmaDocenteSituacao.Excluido)
            {
                // Colocar situação ativa ou inativa pro docente, de acordo com a vigência.
                if (entity.tdt_vigenciaInicio.Date <= DateTime.Now.Date
                    && (entity.tdt_vigenciaFim == new DateTime() || entity.tdt_vigenciaFim.Date >= DateTime.Now.Date)
                    )
                {
                    entity.tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo;
                }
                else
                {
                    entity.tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Inativo;
                }
            }

            LimpaCache(entity, Guid.Empty);

            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO { _Banco = banco };
            return dao.Salvar(entity);
        }

        /// <summary>
        /// Exclui logicamente a ligação do docente com a disciplina da turma
        /// </summary>
        /// <param name="col_id">Id do colaborador</param>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="coc_id">Id do colaboradorcargo</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>Verdadeiro se conseguiu fazer a exclusão lógica.</returns>
        public static bool AtualizarTurmaDocentePorColaboradorDocente
        (
            long col_id
            , int crg_id
            , int coc_id
            , int tds_id
            , Guid ent_id
            , Guid uad_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO { _Banco = banco };
            return dao.AtualizarTurmaDocentePorColaboradorDocente(col_id, crg_id, coc_id, tds_id, ent_id, uad_id);
        }

        /// <summary>
        /// Salva os registros de TurmaDocente, efetua todas as validações necessárias, se precisar excluir um registro
        ///     do TurmaDocente é necessário mandar esse registro na linha com situação de excluido.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="listTurmaDocente">Lista de entidade TUR_TurmaDocente</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        public static void SalvarTurmaDocente(long tur_id, List<TUR_TurmaDocente> listTurmaDocente, Guid ent_id)
        {
            TUR_TurmaDocenteDAO dao = new TUR_TurmaDocenteDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Salva a lista de turmaDocente enviada
                foreach (TUR_TurmaDocente turmaDocente in listTurmaDocente)
                {
                    // Remove do cache as turmas do docente.
                    TUR_TurmaBO.RemoveCacheDocenteControleTurma(ent_id, turmaDocente.doc_id);

                    if (!turmaDocente.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(turmaDocente));

                    //Verifica se as datas das vigências estão válidas
                    if (turmaDocente.tdt_vigenciaFim != new DateTime() && turmaDocente.tdt_vigenciaInicio > turmaDocente.tdt_vigenciaFim)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a vigência final.");

                    if (turmaDocente.tdt_situacao != (byte)TUR_TurmaDocenteSituacao.Excluido)
                    {
                        // Colocar situação ativa ou inativa pro docente, de acordo com a vigência.
                        if (turmaDocente.tdt_vigenciaInicio.Date <= DateTime.Now.Date
                            && (turmaDocente.tdt_vigenciaFim == new DateTime() || turmaDocente.tdt_vigenciaFim.Date >= DateTime.Now.Date))
                            turmaDocente.tdt_situacao = turmaDocente.tdt_situacao;
                        else
                            turmaDocente.tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Inativo;
                    }

                    turmaDocente.tdt_dataAlteracao = DateTime.Now;

                    if (!dao.Salvar(turmaDocente))
                        throw new ArgumentException("Erro ao salvar a atribuição de aula.");
                }

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ALTERAR_AULAS_PARA_TITULAR_ATRIBUICAO_DOCENTES, ent_id))
                {
                    //Se houver algum registro na posição 2 ou maior transfere os lançamentos para a posição 1
                    foreach (TUR_TurmaDocente turmaDocente in listTurmaDocente.Where(turmaDocente => turmaDocente.tdt_posicao > 1 &&
                                                                                                     turmaDocente.tdt_situacao == 1))
                        TransferirLancamentos_Posicao(turmaDocente.tud_id, dao._Banco);
                }
                
                foreach (TUR_TurmaDocente turmaDocente in listTurmaDocente)
                    LimpaCache(turmaDocente, ent_id);
            }
            catch (SqlException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ValidationException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        #endregion Saves
    }
}