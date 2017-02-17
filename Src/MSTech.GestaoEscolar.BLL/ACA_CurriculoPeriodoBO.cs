using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
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
    /// Situações do curriculo periodo
    /// </summary>
    public enum ACA_CurriculoPeriodoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    /// <summary>
    /// Tipos de controle de tempo
    /// </summary>
    public enum ACA_CurriculoPeriodoControleTempo : byte
    {
        TemposAula = 1
        ,

        Horas = 2
    }

    #endregion Enumeradores

    #region Estrutura

    public struct sComboPeriodo
    {
        public string cur_id_crr_id_crp_id { get; set; }
        public string crp_descricao { get; set; }
    }

    #endregion Estrutura

    public class ACA_CurriculoPeriodoBO : BusinessBase<ACA_CurriculoPeriodoDAO, ACA_CurriculoPeriodo>
    {
        public static string RetornaChaveCache_GetSelect(int cur_id, int crr_id, Guid ent_id)
        {
            return String.Format("Cache_GetSelect_{0}_{1}_{2}", cur_id, crr_id, ent_id);
        }

        public static string RetornaChaveCache_GetSelect(int cur_id, int crr_id, int esc_id, int uni_id, Guid ent_id)
        {
            return String.Format("Cache_GetSelect_{0}_{1}_{2}_{3}_{4}", cur_id, crr_id, esc_id, uni_id, ent_id);
        }

        public static string RetornaChaveCache_Select_Por_TipoCiclo(int cur_id, int crr_id, int tci_id, Guid ent_id)
        {
            return String.Format("Cache_Select_Por_TipoCiclo_{0}_{1}_{2}_{3}", cur_id, crr_id, tci_id, ent_id);
        }

        #region Consultas

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_CurriculoPeriodo GetEntity(ACA_CurriculoPeriodo entity, TalkDBTransaction banco = null)
        {
            ACA_CurriculoPeriodoDAO dao = banco == null ? new ACA_CurriculoPeriodoDAO() : new ACA_CurriculoPeriodoDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            dao.Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_CurriculoPeriodo entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Remove do cache a entidade pelo curso.
        /// </summary>
        /// <param name="entity"></param>
        public static void LimpaCache_PeloCurso(ACA_Curso entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                GestaoEscolarUtilBO.LimpaCache(string.Format("ACA_CurriculoPeriodo_GetEntity_{0}_", entity.cur_id));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_CurriculoPeriodo entity)
        {
            return string.Format("ACA_CurriculoPeriodo_GetEntity_{0}_{1}_{2}", entity.cur_id, entity.crr_id, entity.crp_id);
        }

        /// <summary>
        /// Retorna os currículos que concluem o nível de ensino.
        /// </summary>
        /// <param name="crr_id">Id do curso</param>
        /// <param name="cur_id">Id do currículo do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Períodos do currículo.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregarAnosFinais
        (
            int cur_id
            , int crr_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.CarregarAnosFinais(cur_id, crr_id, ent_id);
        }

        /// <summary>
        /// Retorna os curriculoPeriodos e os equivalentes ao passado por parâmetro, ligados à escola,
        /// que possuam turmas ativas na escola.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículo período</param>
        /// <param name="esc_id">ID da escola</param>
        /// <returns></returns>
        internal static List<ACA_CurriculoPeriodo> Seleciona_PeriodosEquivalentes_PorEscola_ComTurmasAtivas(int cur_id, int crr_id, int crp_id, int esc_id)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            DataTable dt = dao.Seleciona_PeriodosEquivalentes_PorEscola_ComTurmasAtivas(cur_id, crr_id, crp_id, esc_id);

            List<ACA_CurriculoPeriodo> retorno =
                (
                    from DataRow dr in dt.Rows
                    select
                        dao.DataRowToEntity(dr, new ACA_CurriculoPeriodo())
                ).ToList();

            return retorno;
        }

        /// <summary>
        /// Retorna os cur_id, crr_id e crp_id relacionados ao período informado.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículo período</param>
        /// <returns></returns>
        public static List<ACA_CurriculoPeriodo> Seleciona_PeriodosRelacionados_Equivalentes(int cur_id, int crr_id, int crp_id)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            DataTable dt = dao.Seleciona_PeriodosRelacionados_Equivalentes(cur_id, crr_id, crp_id);

            List<ACA_CurriculoPeriodo> retorno =
                (
                    from DataRow dr in dt.Rows
                    select
                        dao.DataRowToEntity(dr, new ACA_CurriculoPeriodo())
                ).ToList();

            return retorno;
        }

        /// <summary>
        /// Retorna os currículoPeriodo cadastrados para o curso e escola informados, e filtra também
        /// o currículoPeríodo que seja equivalente (mesmo crp_ordem) do crpEquivalente informado.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_idEquivalente">ID do curso equivalente</param>
        /// <param name="crr_idEquivalente">ID do currículo equivalente</param>
        /// <param name="crp_idEquivalente">ID do período equivalente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Seleciona_Por_CursoEscola_PeriodoEquivalente
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , int cur_idEquivalente
            , int crr_idEquivalente
            , int crp_idEquivalente
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.Seleciona_Por_CursoEscola_PeriodoEquivalente(cur_id, crr_id, esc_id, uni_id, cur_idEquivalente, crr_idEquivalente, crp_idEquivalente);
        }

        /// <summary>
        /// Retorna um List das entidades de CurriculoPeriodo, filtrados pelo Curso e
        /// Entidade.
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="ent_id">Entidade do usuário logado.</param>
        /// <returns></returns>
        public static List<ACA_CurriculoPeriodo> GetSelectBy_Curso(int cur_id, Guid ent_id, TalkDBTransaction banco = null)
        {
            List<ACA_CurriculoPeriodo> lista = new List<ACA_CurriculoPeriodo>();
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();

            if (banco != null)
            {
                dao._Banco = banco;
            }

            DataTable dt = dao.SelectBy_cur_id_crr_id(cur_id, -1, ent_id, false, 1, 1, out totalRecords);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_CurriculoPeriodo ent = new ACA_CurriculoPeriodo();
                ent = dao.DataRowToEntity(dr, ent);
                lista.Add(ent);
            }

            return lista;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os periodos curriculo/curso
        /// que não foram excluídos logicamente, filtrados por
        /// id do curso, id do curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com os periodos do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , int crr_id
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_cur_id_crr_id(cur_id, crr_id, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona os currículo períodos por curso currículo.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="appMinutosCacheLongo">Minutos em que os dados são armazenados em cache.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> GetSelect
        (
            int cur_id,
            int crr_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboPeriodo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_GetSelect(cur_id, crr_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                totalRecords = 0;

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_cur_id_crr_id(cur_id, crr_id, ent_id, false, 1, 1, out totalRecords))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboPeriodo
                                 {
                                     cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                     ,
                                     crp_descricao = dr["crp_descricao"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboPeriodo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_cur_id_crr_id(cur_id, crr_id, ent_id, false, 1, 1, out totalRecords))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboPeriodo
                             {
                                 cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                 ,
                                 crp_descricao = dr["crp_descricao"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Seleciona os currículo períodos por curso currículo e disciplina.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="appMinutosCacheLongo">Minutos em que os dados são armazenados em cache.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> SelecionaPorCursoDisciplina
        (
            int cur_id,
            int crr_id,
            int dis_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboPeriodo> dados = null;

            Func<List<sComboPeriodo>> retorno = delegate()
            {
                using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelecionaPorCursoDisciplina(cur_id, crr_id, dis_id, ent_id))
                {
                    return dt.Rows.Cast<DataRow>()
                                  .Select
                                  (
                                      p => new sComboPeriodo
                                      {
                                          cur_id_crr_id_crp_id = p["cur_id_crr_id_crp_id"].ToString(),
                                          crp_descricao = p["crp_descricao"].ToString()
                                      }
                                  ).ToList();
                }
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.CURRICULO_PERIODO_CURSO_DISCIPLINA_MODEL_KEY, cur_id, crr_id, dis_id, ent_id);

                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheLongo
                    );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        } 

        /// <summary>
        /// Retorna um datatable contendo todos os periodos curriculo/curso
        /// que não foram excluídos logicamente, filtrados por
        /// id do curso, id do curriculo, id de escola e id de unidade.
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="esc_id">Id de escola</param>
        /// <param name="uni_id">Id de unidade</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com os periodos do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_cur_id_crr_id_esc_id_uni_id(cur_id, crr_id, esc_id, uni_id, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona os currículo períodos por curso currículo.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="appMinutosCacheLongo">Minutos em que os dados são armazenados em cache.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> GetSelect
        (
            int cur_id,
            int crr_id,
            int esc_id,
            int uni_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboPeriodo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_GetSelect(cur_id, crr_id, esc_id, uni_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                totalRecords = 0;

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_cur_id_crr_id_esc_id_uni_id(cur_id, crr_id, esc_id, uni_id, ent_id, false, 1, 1, out totalRecords))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboPeriodo
                                 {
                                     cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                     ,
                                     crp_descricao = dr["crp_descricao"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboPeriodo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_cur_id_crr_id_esc_id_uni_id(cur_id, crr_id, esc_id, uni_id, ent_id, false, 1, 1, out totalRecords))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboPeriodo
                             {
                                 cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                 ,
                                 crp_descricao = dr["crp_descricao"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna os períodos por curso e disciplina da turma
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorCursoTurmaDisciplina
        (
            int cur_id
            , int crr_id
            , long tud_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_CursoTurmaDisciplina(cur_id, crr_id, tud_id, ent_id);
        }

        /// <summary>
        /// Retorna os CurriculoPeriodo cadastrados no curso e ligados à escola, que
        /// possuam o campo crp_qtdeEletivasAlunos > 0, ou seja, períodos
        /// que permitam cadastrar disciplinas eletivas de aluno.
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="esc_id">Id de escola</param>
        /// <param name="uni_id">Id de unidade da escola</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <returns>DataTable com os periodos do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Escola_Curso_EletivasAluno
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_Escola_Curso_EletivasAluno(cur_id, crr_id, esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os periodos curriculo/curso
        /// que não foram excluídos logicamente, filtrados por
        /// id do curso, id do curriculo
        /// </summary>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id">id do curriculo</param>
        /// <param name="ent_id">entidade do usuario logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Curso_EletivaAluno
        (
            int cur_id
            , int crr_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_Curso_EletivaAluno(cur_id, crr_id, ent_id);
        }

        /// <summary>
        ///  Lista todos os cursos e periodos ativos.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectPor_Entidade(Guid ent_id)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectPor_Entidade(ent_id);
        }

        /// <summary>
        ///  Lista todos os cursos e periodos ativos
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Select_Ativos(Guid ent_id)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            try
            {
                return dao.Select_Ativos(ent_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Lista todos os Curriculos Periodos para determinada disciplina eletiva
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Select_Por_Disciplina(int dis_id, int cur_id, int crr_id)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            try
            {
                return dao.SelectBy_Disciplina(dis_id, cur_id, crr_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona CurriculoPeriodo de uma turma do tipo normal
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <returns>ACA_CurriculoPeriodo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_CurriculoPeriodo SelecionaPorTurmaTipoNormal(long tur_id, int appMinutosCacheLongo = 0)
        {
            ACA_CurriculoPeriodo curriculoPeriodo = null;

            Func<ACA_CurriculoPeriodo> retorno = delegate()
            {
                ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
                ACA_CurriculoPeriodo entity = new ACA_CurriculoPeriodo();
                DataTable dt = dao.SelecionaPorTurmaTipoNormal(tur_id);

                if (dt.Rows.Count > 0)
                    entity = dao.DataRowToEntity(dt.Rows[0], entity);

                return entity;
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.CURRICULO_PERIODO_TURMA_TIPO_NORMAL_MODEL_KEY, tur_id);

                curriculoPeriodo = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
            }
            else
            {
                curriculoPeriodo = retorno();
            }

            return curriculoPeriodo;           

            //ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            //ACA_CurriculoPeriodo entity = new ACA_CurriculoPeriodo();
            //DataTable dt = dao.SelecionaPorTurmaTipoNormal(tur_id);

            //if (dt.Rows.Count > 0)
            //    entity = dao.DataRowToEntity(dt.Rows[0], entity);

            //return entity;
        }

        /// <summary>
        /// Seleciona CurriculoPeriodo de uma turma do tipo normal
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <returns>ACA_CurriculoPeriodo</returns>
        public static int[] SelecionaPorCursoHistoricoTipoCurriculoPeriodo(int cur_id, int tcp_id, int chp_anoLetivo)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            DataTable dt = dao.SelecionaPorCursoHistoricoTipoCurriculoPeriodo(cur_id, tcp_id, chp_anoLetivo);

            int[] result = new int[3];
            if (dt.Rows.Count > 0)
            {
                result[0] = Convert.ToInt32(dt.Rows[0]["cur_id"]);
                result[1] = Convert.ToInt32(dt.Rows[0]["crr_id"]);
                result[2] = Convert.ToInt32(dt.Rows[0]["crp_id"]);
            }

            return result;
        }

        /// <summary>
        /// Seleciona currículo perdiodo por turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public static List<ACA_CurriculoPeriodo> SelecionaPorTurmaDisciplina(long tud_id, TalkDBTransaction banco = null)
        {
            ACA_CurriculoPeriodoDAO dao = banco == null ?
                new ACA_CurriculoPeriodoDAO() :
                new ACA_CurriculoPeriodoDAO { _Banco = banco };
            return dao.SelecionaPorTurmaDisciplina(tud_id);
        }

        /// <summary>
        /// Seleciona períodos pela quantidade de níveis da orientação curricular
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaPorQtdeNiveisOrientacaoCurricular(int cur_id, int crr_id, int cal_id, int tds_id, Guid ent_id, int qtde_niveis)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelecionaPorQtdeNiveisOrientacaoCurricular(cur_id, crr_id, cal_id, tds_id, ent_id, qtde_niveis);
        }

        /// <summary>
        ///  Lista todos os Curriculos Periodos para determinado tipo de ciclo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="esc_id">Id de escola</param>
        /// <param name="uni_id">Id de unidade</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns>DataTable com os periodos do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> Select_Por_TipoCiclo(int cur_id, int crr_id, int tci_id, Guid ent_id, int appMinutosCacheLongo = 0)
        {

            List<sComboPeriodo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_Select_Por_TipoCiclo(cur_id, crr_id, tci_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                totalRecords = 0;

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_TipoCiclo(cur_id, crr_id, tci_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboPeriodo
                                 {
                                     cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                     ,
                                     crp_descricao = dr["crp_descricao"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboPeriodo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelectBy_TipoCiclo(cur_id, crr_id, tci_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboPeriodo
                             {
                                 cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                 ,
                                 crp_descricao = dr["crp_descricao"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Seleciona por escola, curso, e ordem do período (apenas períodos com ordem igual ou superior)
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_ordem">Ordem de validação do período.</param>
        /// <param name="appMinutosCacheLongo">Cache</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> SelecionaPorEscolaCursoPeriodoOrdem(int esc_id, int uni_id, int cur_id, int crr_id, int crp_ordem, int appMinutosCacheLongo = 0)
        {
            Func<List<sComboPeriodo>> retorno = delegate
            {
                using (DataTable dt = new ACA_CurriculoPeriodoDAO().SelecionaPorEscolaCursoPeriodoOrdem(esc_id, uni_id, cur_id, crr_id, crp_ordem))
                {
                    return (from DataRow dr in dt.Rows
                            select new sComboPeriodo
                            {
                                cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                ,
                                crp_descricao = dr["crp_descricao"].ToString()
                            }).ToList();
                }
            };

            if (appMinutosCacheLongo > 0)
            {
                return CacheManager.Factory.Get
                    (
                        String.Format(ModelCache.CURRICULO_PERIODO_ESCOLA_CURSO_PERIODO_ORDEM_MODEL_KEY, esc_id, uni_id, cur_id, crr_id, crp_ordem), 
                        retorno,
                        appMinutosCacheLongo
                    );
            }
            else
            {
                return retorno();
            }
        }

        #endregion Consultas

        #region Plataforma de Itens e Avaliações

        /// <summary>
        /// Retorna os curriculo periodos(serie) de acordo com entidade, curso e curriculo
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        public static DataTable BuscaCurriculoPeriodoPorEntidadeCursoCurriculo
        (
            Guid ent_id
            , int cur_id
            , int crr_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.BuscaCurriculoPeriodoPorEntidadeCursoCurriculo(ent_id, cur_id, crr_id);
        }

        /// <summary>
        /// Retorna os curriculo periodos(serie) de acordo com entidade, curso e curriculo
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        /// <param name="esc_id">Escola</param>
        public static DataTable BuscaCurriculoPeriodoPorEntidadeCursoCurriculoEscola
        (
            Guid ent_id
            , int cur_id
            , int crr_id
            , int esc_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.BuscaCurriculoPeriodoPorEntidadeCursoCurriculoEscola(ent_id, cur_id, crr_id, esc_id);
        }

        #endregion Plataforma de Itens e Avaliações

        #region Verificações

        /// <summary>
        /// Verifica se os cursos e períodos são equivalentes ou são iguais.
        /// Se o aluno pode ir do curso de origem para o curso de destino.
        /// </summary>
        /// <param name="cur_idOrigem">Curso de origem</param>
        /// <param name="crr_idOrigem">Currículo de origem</param>
        /// <param name="crp_idOrigem">Período do curso de origem</param>
        /// <param name="cur_idDestino">Curso de destino</param>
        /// <param name="crr_idDestino">Currículo de destino</param>
        /// <param name="crp_idDestino">Período de destino</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        internal static bool Sao_CursosPeriodos_Equivalentes
        (
            int cur_idOrigem
            , int crr_idOrigem
            , int crp_idOrigem
            , int cur_idDestino
            , int crr_idDestino
            , int crp_idDestino
            , TalkDBTransaction banco
        )
        {
            bool crpEquivalente = false;

            // Verifica se o curso de destino é equivalente.
            bool cursoEquivalente = ACA_CursoBO.Sao_Cursos_Equivalentes
                    (cur_idOrigem, crr_idOrigem, cur_idDestino, crr_idDestino, banco);

            if (cursoEquivalente)
            {
                crpEquivalente = Sao_Periodos_Equivalentes
                    (
                        cur_idOrigem
                        , crr_idOrigem
                        , crp_idOrigem
                        , cur_idDestino
                        , crr_idDestino
                        , crp_idDestino
                        , banco
                    );
            }

            return cursoEquivalente && crpEquivalente;
        }

        /// <summary>
        /// Verifica se possui cursos e períodos equivalentes
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        internal static bool Possui_CursosPeriodos_Equivalentes(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id, TalkDBTransaction banco)
        {
            bool possuiCursoPeriodoEquivalente = false;
            List<sComboCurso> dt;
            bool possuiCursosEquivalentes = ACA_CursoBO.Possui_Cursos_Equivalentes(cur_id, crr_id, esc_id, uni_id, banco, out dt);

            if (possuiCursosEquivalentes)
            {
                possuiCursoPeriodoEquivalente = Possui_Periodos_Equivalentes(cur_id, crr_id, crp_id, esc_id, uni_id, dt, banco);
            }

            return possuiCursoPeriodoEquivalente && possuiCursosEquivalentes;
        }

        /// <summary>
        /// Retorna se os períodos de origem e destino são iguais ou equivalentes (campo crp_ordem igual).
        /// </summary>
        /// <param name="cur_idOrigem">ID do curso de origem</param>
        /// <param name="crr_idOrigem">ID do currículo de origem</param>
        /// <param name="crp_idOrigem">ID do período de origem</param>
        /// <param name="cur_idDestino">ID do curso de destino</param>
        /// <param name="crr_idDestino">ID do currículo de destino</param>
        /// <param name="crp_idDestino">ID do período de destino</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        internal static bool Sao_Periodos_Equivalentes
            (
            int cur_idOrigem
            , int crr_idOrigem
            , int crp_idOrigem
            , int cur_idDestino
            , int crr_idDestino
            , int crp_idDestino
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoPeriodo entPeriodoOrigem = GetEntity(new ACA_CurriculoPeriodo
            {
                cur_id = cur_idOrigem
                ,
                crr_id = crr_idOrigem
                ,
                crp_id = crp_idOrigem
            }, banco);

            ACA_CurriculoPeriodo entPeriodoDestino = GetEntity(new ACA_CurriculoPeriodo
            {
                cur_id = cur_idDestino
                ,
                crr_id = crr_idDestino
                ,
                crp_id = crp_idDestino
            }, banco);

            // Verifica se o crp de destino é equivalente (mesma ordem).
            bool crpEquivalente = entPeriodoOrigem.crp_ordem == entPeriodoDestino.crp_ordem;

            return crpEquivalente;
        }

        /// <summary>
        /// Verifica se possui período equivalente.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        internal static bool Possui_Periodos_Equivalentes(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id, List<sComboCurso> dt, TalkDBTransaction banco)
        {
            ACA_CurriculoPeriodo entPeriodo = new ACA_CurriculoPeriodo
            {
                cur_id = cur_id
                ,
                crr_id = crr_id
                ,
                crp_id = crp_id
            };
            GetEntity(entPeriodo, banco);

            DataTable lista = ACA_CurriculoEscolaPeriodoBO.CarregaCursosPeriodoReferenteEscola(esc_id, false, -1, -1);

            int equivalentes = 0;

            foreach (sComboCurso dr in dt)
            {
                int dr_cur_id = Convert.ToInt32(dr.cur_crr_id.Split(';')[0]);
                int dr_crr_id = Convert.ToInt32(dr.cur_crr_id.Split(';')[1]);

                List<ACA_CurriculoPeriodo> periodos = (from DataRow cep in lista.Rows
                                                       where Convert.ToInt32(cep["cur_id"]) == dr_cur_id &&
                                                             Convert.ToInt32(cep["crr_id"]) == dr_crr_id &&
                                                             Convert.ToInt32(cep["esc_id"]) == esc_id &&
                                                             Convert.ToInt32(cep["uni_id"]) == uni_id
                                                       select GetEntity(new ACA_CurriculoPeriodo
                                                       {
                                                           cur_id = Convert.ToInt32(cep["cur_id"]),
                                                           crr_id = Convert.ToInt32(cep["crr_id"]),
                                                           crp_id = Convert.ToInt32(cep["crp_id"])
                                                       })).ToList();

                equivalentes += (from ACA_CurriculoPeriodo crp in periodos
                                 where crp.crp_ordem == entPeriodo.crp_ordem
                                 select crp).Count();
            }

            return equivalentes > 0;
        }

        /// <summary>
        /// Verifica se o período está sendo utilizado na matricula ou na turma curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="crp_id">Id da tabela ACA_CurriculoPeriodo</param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ExisteCurriculo_TurmaCurriculoMatricula
        (
            int cur_id
            , int crr_id
            , int crp_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_VerificaTurmaCurriculoMatricula(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Verifica se o período está sendo utilizado na turma curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="crp_id">Id da tabela ACA_CurriculoPeriodo</param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ExisteCurriculo_TurmaCurriculo
        (
            int cur_id
            , int crr_id
            , int crp_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_VerificaTurmaCurriculo(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Verifica se já existe um período do currículo cadastrado com o mesmo nome, curso e currículo
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoPeriodo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaPeriodoExistente
        (
            ACA_CurriculoPeriodo entity
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO { _Banco = banco };
            return dao.SelectBy_VerificaNomeExistente(entity.cur_id, entity.crr_id, entity.crp_id, entity.crp_descricao);
        }

        /// <summary>
        /// Verifica se existe o curriculo período para escola.
        /// </summary>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id">id do curriculo</param>
        /// <param name="crp_id">id do curriculo período</param>
        /// <param name="esc_id">Id de escola</param>
        /// <param name="uni_id">Id de unidade</param>
        /// <param name="ent_id">entidade do usuário logado</param>
        /// <returns>
        /// TRUE - existe curriculo periodo
        /// FALSE - NÃO existe currículo período
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaPeriodoExistente
        (
             int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_VerificaCurriculoPeriodoExistente(cur_id, crr_id, crp_id, esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna o período anterior do período informado
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="crp_id">Id do curriculo período</param>
        /// <returns>crp_id Anterior</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int VerificaPeriodoAnterior
        (
            int cur_id
            , int crr_id
            , int crp_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_VerificaPeriodoAnterior(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Seleciona o id do último periodo cadastrado para o curriculo + 1
        /// se não houver periodo cadastrado para curriculo retorna 1
        /// filtrados por cur_id, crr_id
        /// </summary>
        /// <param name="cur_id">Campo cur_id da tabela ACA_CurriculoPeriodo do bd</param>
        /// <param name="crr_id">Campo crr_id da tabela ACA_CurriculoPeriodo do bd</param>
        /// <returns>uec_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int VerificaUltimoPeriodoCadastrado
        (
            int cur_id
            , int crr_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_cur_id_crr_id_top_one(cur_id, crr_id);
        }

        /// <summary>
        /// Consulta o período pelo nome.
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoPeriodo</param>
        /// <returns>True = se encontrou o período / False = Não encontrou o período</returns>
        public static bool ConsultarNomeExistente(ref ACA_CurriculoPeriodo entity)
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_Nome(ref entity);
        }

        /// <summary>
        /// Consulta se existe o período com o nome, escola, curso e
        /// currículo passados e preenche a entidade.
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoPeriodo.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns> True = Se encontrou o período / False = Não encontrou</returns>
        public static bool ConsultaPeriodoPorNome_IdEscola
        (
              ACA_CurriculoPeriodo entity
            , int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();
            return dao.SelectBy_Nome_esc_id(entity, esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Indica se o curriculo período é do EJA e o campo turma por avaliação está marcado
        /// </summary>
        /// <param name="entityCurriculoPeriodo">Entidade ACA_CurriculoPeriodo</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <returns></returns>
        public static bool VerificaCurriculoPeriodoEJA(ACA_CurriculoPeriodo entityCurriculoPeriodo, TalkDBTransaction bancoGestao)
        {
            return VerificaCurriculoPeriodoEJA(ref entityCurriculoPeriodo, bancoGestao);
        }

        /// <summary>
        /// Indica se o curriculo período é do EJA de acordo com os dados informados na estrutura de movimentação.
        /// </summary>
        /// <param name="cadMov">Entidade de cadastro de movimentações alimentada</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <param name="entAlcAvaliacao">Entidade da avaliação que será utilizada para buscar o currículo</param>
        /// <returns></returns>
        public static bool VerificaCurriculoPeriodoEJA(MTR_Movimentacao_Cadastro cadMov, ACA_AlunoCurriculoAvaliacao entAlcAvaliacao, TalkDBTransaction bancoGestao)
        {
            ACA_Curriculo entCurriculo = null;

            if (cadMov.listasFechamentoMatricula.listCurriculosDestinos != null)
            {
                // Se a lista configurada do fechamento do ano letivo foi alimentada, busca dados dela.
                entCurriculo = cadMov.listasFechamentoMatricula.listCurriculosDestinos.Find
                    (p =>
                        p.cur_id == entAlcAvaliacao.cur_id
                        && p.crr_id == entAlcAvaliacao.crr_id
                    );
            }

            if (entCurriculo == null)
            {
                return ACA_CurriculoPeriodoBO.VerificaCurriculoPeriodoEJA
                    (new ACA_CurriculoPeriodo { cur_id = entAlcAvaliacao.cur_id, crr_id = entAlcAvaliacao.crr_id, crp_id = entAlcAvaliacao.crp_id }, bancoGestao);
            }

            return VerificaRegraPEJA(entCurriculo);
        }

        /// <summary>
        /// Indica se o curriculo período é do EJA e o campo turma por avaliação está marcado.
        /// </summary>
        /// <param name="entityCurriculoPeriodo">Entidade ACA_CurriculoPeriodo</param>
        /// <param name="verificaTurma">Indica se deve verificar o crp_turmaAvaliacao ou não</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <returns></returns>
        public static bool VerificaCurriculoPeriodoEJA(ref ACA_CurriculoPeriodo entityCurriculoPeriodo, TalkDBTransaction bancoGestao)
        {
            // Carrega o Curriculo
            ACA_Curriculo entityCurriculo = new ACA_Curriculo { cur_id = entityCurriculoPeriodo.cur_id, crr_id = entityCurriculoPeriodo.crr_id };
            if (bancoGestao == null)
                ACA_CurriculoBO.GetEntity(entityCurriculo);
            else
                ACA_CurriculoBO.GetEntity(entityCurriculo, bancoGestao);

            // Carrega o Curriculo período
            if (bancoGestao == null)
                GetEntity(entityCurriculoPeriodo);
            else
                GetEntity(entityCurriculoPeriodo, bancoGestao);

            return VerificaRegraPEJA(entityCurriculo);
        }

        /// <summary>
        /// Faz a validação se o curso é seriado por avaliações (verificando o campo crr_regimeMatricula). Considera que a entidade
        /// já está alimentada.
        /// </summary>
        /// <param name="entityCurriculo">Entidade alimentada</param>
        /// <returns></returns>
        private static bool VerificaRegraPEJA(ACA_Curriculo entityCurriculo)
        {
            return (((ACA_CurriculoRegimeMatricula)Enum.Parse(typeof(ACA_CurriculoRegimeMatricula), entityCurriculo.crr_regimeMatricula.ToString())).Equals(ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes));
        }

        /// <summary>
        /// Indica se o curriculo período tem o campo turma por avaliação marcado.
        /// </summary>
        /// <param name="entityCurriculoPeriodo">Entidade ACA_CurriculoPeriodo</param>
        /// <returns></returns>
        public static bool VerificaTurmaPorAvaliacao(ref ACA_CurriculoPeriodo entityCurriculoPeriodo)
        {
            // Carrega o Curriculo
            ACA_Curriculo entityCurriculo = new ACA_Curriculo { cur_id = entityCurriculoPeriodo.cur_id, crr_id = entityCurriculoPeriodo.crr_id };
            ACA_CurriculoBO.GetEntity(entityCurriculo);

            // Carrega o Curriculo período
            GetEntity(entityCurriculoPeriodo);

            return (entityCurriculoPeriodo.crp_turmaAvaliacao);
        }

        /// <summary>
        /// Indica se o curriculo período é do EJA independente do campo turma por avaliação.
        /// </summary>
        /// <param name="entityCurriculoPeriodo">Entidade ACA_CurriculoPeriodo</param>
        /// <returns></returns>
        public static bool VerificaSomenteCurriculoPeriodoEJA(ref ACA_CurriculoPeriodo entityCurriculoPeriodo)
        {
            // Carrega o Curriculo
            ACA_Curriculo entityCurriculo = new ACA_Curriculo { cur_id = entityCurriculoPeriodo.cur_id, crr_id = entityCurriculoPeriodo.crr_id };
            ACA_CurriculoBO.GetEntity(entityCurriculo);

            // Carrega o Curriculo período
            GetEntity(entityCurriculoPeriodo);

            return (((ACA_CurriculoRegimeMatricula)Enum.Parse(typeof(ACA_CurriculoRegimeMatricula), entityCurriculo.crr_regimeMatricula.ToString())).Equals(ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes));
        }

        public static string ValidaNomeFundoFrente(string nomeArquivo)
        {
            if (nomeArquivo.Contains("/") || nomeArquivo.Contains("\\") || nomeArquivo.Contains(":") || nomeArquivo.Contains("*") ||
                nomeArquivo.Contains("?") || nomeArquivo.Contains("\"") || nomeArquivo.Contains("<") || nomeArquivo.Contains(">") ||
                nomeArquivo.Contains("|"))
                return "O nome do arquivo de fundo da frente da carteirinha não pode conter os caracteres: \\ / : * ? \" < > |";
            if (nomeArquivo.Length > 260)
                return "O nome do arquivo de fundo da frente da carteirinha não pode ser maior que 260 caracteres.";
            return "";
        }

        #endregion Verificações

        #region Inclusão e Alteração

        /// <summary>
        /// Inclui um novo periodo para o curriculo/curso
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoPeriodo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoPeriodo entity
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            if (entity.Validate())
            {
                // Verifica se já existe um período cadastrado com o mesmo nome, curso e currículo
                if (VerificaPeriodoExistente(entity, banco))
                    throw new DuplicateNameException("Já existe um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " cadastrado com a descrição " + entity.crp_descricao + ".");

                // Verifica se a idade mínima ideal foi preenchida
                if (entity.crp_idadeIdealAnoInicio == 0 && entity.crp_idadeIdealMesInicio == 0)
                    throw new ArgumentException("A idade mínima ideal deve ser maior que 0 (zero).");

                // Verifica se o mês da idade mínima ideal é maior do que 11
                if (entity.crp_idadeIdealMesInicio > 11)
                    throw new ArgumentException("Mês(es) da idade mínima ideal não pode ser maior do que 11.");

                // Verifica se a idade máxima ideal foi preenchida
                if (entity.crp_idadeIdealAnoFim == 0 && entity.crp_idadeIdealMesFim == 0)
                    throw new ArgumentException("Idade máxima ideal deve ser maior que 0 (zero).");

                // Verifica se o mês da idade máxima ideal é maior do que 11
                if (entity.crp_idadeIdealMesFim > 11)
                    throw new ArgumentException("Mês(s) da idade máxima ideal não pode ser maior do que 11.");

                // Verifica se a idade mínima ideal é maior do que a idade máxima ideal
                if (entity.crp_idadeIdealAnoInicio > entity.crp_idadeIdealAnoFim)
                    throw new ArgumentException("Idade mínima ideal não pode ser maior que a idade máxima ideal.");
                if (entity.crp_idadeIdealAnoInicio == entity.crp_idadeIdealAnoFim)
                {
                    if (entity.crp_idadeIdealMesInicio > entity.crp_idadeIdealMesFim)
                        throw new ArgumentException("Idade mínima ideal não pode ser maior que a idade máxima ideal.");
                }

                // Verifica se a quantidade de tempos de aula por semana foi preenchido caso o controle
                // de tempo seja por tempos de aula
                if (entity.crp_controleTempo == Convert.ToByte(ACA_CurriculoPeriodoControleTempo.TemposAula))
                {
                    if (entity.crp_qtdeTemposSemana == 0)
                        throw new ArgumentException("Quantidade de tempos de aula de uma semana é obrigatório.");
                }

                // Verifica se a carga horária total do dia foi preenchida caso o controle
                // de tempo seja por horas
                else if (entity.crp_controleTempo == Convert.ToByte(ACA_CurriculoPeriodoControleTempo.Horas))
                {
                    if (entity.crp_qtdeHorasDia == 0 && entity.crp_qtdeMinutosDia == 0)
                        throw new ArgumentException("Carga horária total do dia deve ser maior que 0 (zero).");

                    // Verifica se as horas da carga horária total do dia é maior do que 23
                    if (entity.crp_qtdeHorasDia > 23)
                        throw new ArgumentException("Hora(s) da carga horária total do dia não pode ser maior do que 23.");

                    // Verifica se os minutos da carga horária total do dia é maior do que 59
                    if (entity.crp_qtdeMinutosDia > 59)
                        throw new ArgumentException("Minuto(s) da carga horária total do dia não pode ser maior do que 59.");
                }

                LimpaCache(entity);

                CacheManager.Factory.RemoveByPattern(ModelCache.CURRICULO_PERIODO_TURMA_TIPO_NORMAL_PATTERN_KEY);

                ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        #endregion Inclusão e Alteração

        #region Exclusão

        /// <summary>
        /// Deleta o período do currículo
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoPeriodo</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_CurriculoPeriodo entity
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            ACA_CurriculoPeriodoDAO dao = new ACA_CurriculoPeriodoDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                // Verifica se o período do currículo pode ser deletado
                if (GestaoEscolarUtilBO.VerificaIntegridadaChaveTripla
                (
                    "cur_id"
                    , "crr_id"
                    , "crp_id"
                    , entity.cur_id.ToString()
                    , entity.crr_id.ToString()
                    , entity.crp_id.ToString()
                    , "ACA_CurriculoPeriodo,ACA_CurriculoDisciplina"
                    , dao._Banco
                ))
                {
                    throw new ValidationException("Não é possível excluir o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " " + entity.crp_descricao + ", pois possui outros registros ligados a ele(a).");
                }

                LimpaCache(entity);

                // Deleta logicamente o período do currículo
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

        #endregion Exclusão
    }
}