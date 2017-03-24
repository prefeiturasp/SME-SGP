using System;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Linq;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    /// <summary>
    /// Estrutura para carregar o combo de calendário.
    /// </summary>
    public struct sComboCalendario
    {
        public string cal_id { get; set; }

        public string cal_ano_desc { get; set; }

        public string cal_ano { get; set; }
    }

    #endregion

    public class ACA_CalendarioAnualBO : BusinessBase<ACA_CalendarioAnualDAO, ACA_CalendarioAnual>
    {
        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_CalendarioAnual entity)
        {
            return string.Format(ModelCache.CALENDARIO_ANUAL_MODEL_KEY, entity.cal_id);
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_CalendarioAnual entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
            CacheManager.Factory.RemoveByPattern(ModelCache.CALENDARIO_ANUAL_POR_TURMA_PATTERN_KEY);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_CalendarioAnual GetEntity(ACA_CalendarioAnual entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
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
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnual_Esc_id(int esc_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnual_Esc_id_{0}_{1}", esc_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionarCalendarioAnualRelCurso_EscId(int esc_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionarCalendarioAnualRelCurso_EscId_{0}_{1}", esc_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnual_AnoBase_Esc_id(int ano_base, int esc_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnual_AnoBase_Esc_id_{0}_{1}_{2}", ano_base, esc_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnual(Guid ent_id, long doc_id, Guid usu_id, Guid gru_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnual_{0}_{1}_{2}_{3}", ent_id, doc_id, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualPorDocente(long doc_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualPorDocente_{0}_{1}", doc_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualPorCurso(int cur_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualPorCurso_{0}_{1}", cur_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualPorCursoAnoInicio(int cur_id, Guid ent_id, int pfi_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualPorCursoAnoInicio_{0}_{1}_{2}", cur_id, ent_id, pfi_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorCursoComDisciplinaEletiva(int cur_id, int esc_id, int uni_id, int tds_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaPorCursoComDisciplinaEletiva_{0}_{1}_{2}_{3}_{4}", cur_id, esc_id, uni_id, tds_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualPorCursoQtdePeriodos(int cur_id, int qtdePeriodos, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualPorCursoQtdePeriodos_{0}_{1}_{2}", cur_id, qtdePeriodos, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualByAno(int cal_ano, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualByAno_{0}_{1}", cal_ano, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCalendarioAnualPorCursoTurmaAtiva(int cur_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaCalendarioAnualPorCursoTurmaAtiva_{0}_{1}", cur_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de calendário
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaAnosAnterioresPorCurso(int cur_id, Guid ent_id)
        {
            return String.Format("Cache_SelecionaAnosAnterioresPorCurso_{0}_{1}", cur_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para guardar o calendario da turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static string RetornaChaveCache_SelecionaPorTurma(Int64 tur_id)
        {
            return String.Format(ModelCache.CALENDARIO_ANUAL_POR_TURMA_MODEL_KEY, tur_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para guardar o calendario da turma
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns></returns>
        public static string RetornaChaveCache_SelecionaPorTurmaDisciplina(Int64 tud_id)
        {
            return String.Format(ModelCache.CALENDARIO_ANUAL_POR_TURMADISCIPLINA_MODEL_KEY, tud_id);
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// filtrados entidade, escola
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnual_Esc_id
        (
            int esc_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnual_Esc_id(esc_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioAnual_Esc_id(esc_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }
            
            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioAnual_Esc_id(esc_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// filtrados entidade, escola
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionarCalendarioAnualRelCurso_EscId
        (
            int esc_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionarCalendarioAnualRelCurso_EscId(esc_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioAnualRelCurso_EscId(esc_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioAnualRelCurso_EscId(esc_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// filtrados por ano, entidade, escola
        /// </summary>        
        /// <param name="esc_id"></param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ano_base"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnual_AnoBase_Esc_id
        (
            int ano_base,
            int esc_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnual_AnoBase_Esc_id(ano_base, esc_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy__Ano_base_Esc_id(esc_id, ent_id, ano_base))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy__Ano_base_Esc_id(esc_id, ent_id, ano_base))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// Com paginação
        /// </summary>   
        /// <param name="cal_ano">Ano do calendário escolar</param>
        /// <param name="cal_descricao">Descrição do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCalendarioAnualPaginado
        (
            int cal_ano
            , string cal_descricao
            , Guid ent_id
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_Pesquisa(cal_ano, cal_descricao, ent_id, true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnual
        (
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            return SelecionaCalendarioAnual(ent_id, appMinutosCacheLongo, 0, new Guid(), new Guid());
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnual
        (
            Guid ent_id,
            int appMinutosCacheLongo
            , long doc_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            List<sComboCalendario> dados = null;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnual(ent_id, doc_id, usu_id, gru_id);
                object cache = HttpContext.Current.Cache[chave];
                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_Entidade(ent_id, doc_id, usu_id, gru_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                     ,
                                     cal_ano = dr["cal_ano"].ToString()
                                 }).ToList();
                    }
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }
            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_Entidade(ent_id, doc_id, usu_id, gru_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 ,
                                 cal_ano = dr["cal_ano"].ToString()
                             }).ToList();
                }
            }
            return dados;
        }


        /// <summary>
        /// Seleciona os calendarios com bimestres ativos por entidade e escola
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCalendariosComBimestresAberto_Por_EntidadeEscola
        (
            Guid ent_id,
            int esc_id,
            bool VerificaEscolaCalendarioPeriodo
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            int tev_idEfetivacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            return dao.SelecionaCalendariosComBimestresAberto_Por_EntidadeEscola(ent_id, esc_id, tev_idEfetivacao, VerificaEscolaCalendarioPeriodo);
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// filtrados por ano e entidade
        /// </summary>        
        /// <param name="cal_ano"></param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualByAno
        (
            int cal_ano
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualByAno(cal_ano, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_AnoBase(ent_id, cal_ano))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                     ,
                                     cal_ano = dr["cal_ano"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_AnoBase(ent_id, cal_ano))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 ,
                                 cal_ano = dr["cal_ano"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por docente
        /// Sem paginação
        /// </summary>        
        /// <param name="doc_id">ID do docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualPorDocente
        (
            long doc_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualPorDocente(doc_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_doc_id(doc_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                     ,
                                     cal_ano = dr["cal_ano"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_doc_id(doc_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 ,
                                 cal_ano = dr["cal_ano"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por curso
        /// Sem paginação
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualPorCurso
        (
            int cur_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualPorCurso(cur_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_cur_id(cur_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                     ,
                                     cal_ano = dr["cal_ano"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_cur_id(cur_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 ,
                                 cal_ano = dr["cal_ano"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por tipo de nível ensino
        /// Sem paginação
        /// </summary>        
        /// <param name="tne_id">ID do tipo de nível de ensino</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCalendarioAnualPorTipoNivelEnsino
        (
            int tne_id
            , Guid ent_id
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_tne_id(tne_id, ent_id);
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por curso
        /// com turma ativa Sem paginação
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualPorCursoTurmaAtiva
        (
            int cur_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualPorCursoTurmaAtiva(cur_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioCurso_TurmaAtiva(cur_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CalendarioCurso_TurmaAtiva(cur_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por curso e ano inicio processo.
        /// Sem paginação
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="pfi_id"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualPorCursoAnoInicio
        (
            int cur_id
            , Guid ent_id
            , int pfi_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualPorCursoAnoInicio(cur_id, ent_id, pfi_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_cur_id_pfi_id(cur_id, ent_id, pfi_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_cur_id_pfi_id(cur_id, ent_id, pfi_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários anuais de cursos que possuem 
        ///	disciplina eletiva filtrando (ou não) por curso e por escola.
        /// </summary>
        ///<param name="cur_id"></param>
        ///<param name="esc_id"></param>
        ///<param name="uni_id"></param>
        ///<param name="tds_id"></param>
        ///<param name="ent_id">Entidade do usuário logado</param>
        public static List<sComboCalendario> SelecionaPorCursoComDisciplinaEletiva
        (
            int cur_id
            ,int esc_id
            ,int uni_id
            ,int tds_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaPorCursoComDisciplinaEletiva(cur_id, esc_id, uni_id, tds_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().GetSelectPorCursoComDisciplinaEletiva(cur_id, esc_id, uni_id, tds_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().GetSelectPorCursoComDisciplinaEletiva(cur_id, esc_id, uni_id, tds_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente, por curso e quantidade de períodos do calendário
        /// Sem paginação
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="qtdePeriodos">Quantidade de períodos do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="banco">Transação com banco</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCalendarioAnualPorCursoQtdePeriodos
        (
            int cur_id
            , int qtdePeriodos
            , Guid ent_id
            , TalkDBTransaction banco
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO{_Banco = banco};
            return dao.SelectBy_CursoQtdePeriodos(cur_id, qtdePeriodos, ent_id);
        }

        /// <summary>
        /// Retorna todos os distintos anos letivos
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarAnosLetivos()
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_AnosLetivos();
        }

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente, por curso e quantidade de períodos do calendário
        /// Sem paginação
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="qtdePeriodos">Quantidade de períodos do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCalendario> SelecionaCalendarioAnualPorCursoQtdePeriodos
        (
            int cur_id
            , int qtdePeriodos
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCalendarioAnualPorCursoQtdePeriodos(cur_id, qtdePeriodos, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CursoQtdePeriodos(cur_id, qtdePeriodos, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelectBy_CursoQtdePeriodos(cur_id, qtdePeriodos, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna o calendário anual da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static ACA_CalendarioAnual SelecionaPorTurma(long tur_id, TalkDBTransaction banco = null)
        {
            ACA_CalendarioAnual dados = null;

            Func<ACA_CalendarioAnual> retorno = delegate()
            {
                ACA_CalendarioAnualDAO dao = banco == null ? new ACA_CalendarioAnualDAO() : new ACA_CalendarioAnualDAO { _Banco = banco };
                dados = dao.SelecionaPorTurma(tur_id);
                return dados;
            };

            string chave = RetornaChaveCache_SelecionaPorTurma(tur_id);

            dados = CacheManager.Factory.Get(
                        chave,
                        retorno,
                        GestaoEscolarUtilBO.MinutosCacheLongo
                    );

            return dados;
        }

        /// <summary>
        /// Retorna o calendário anual da turma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static ACA_CalendarioAnual SelecionaPorTurmaDisciplina(long tud_id, TalkDBTransaction banco = null)
        {
            ACA_CalendarioAnual dados = null;

            Func<ACA_CalendarioAnual> retorno = delegate()
            {
                ACA_CalendarioAnualDAO dao = banco == null ? new ACA_CalendarioAnualDAO() : new ACA_CalendarioAnualDAO { _Banco = banco };
                dados = dao.SelecionaPorTurmaDisciplina(tud_id);
                return dados;
            };

            string chave = RetornaChaveCache_SelecionaPorTurmaDisciplina(tud_id);

            dados = CacheManager.Factory.Get(
                        chave,
                        retorno,
                        GestaoEscolarUtilBO.MinutosCacheLongo
                    );

            return dados;
        }

        /// <summary>
        /// Carrega os calendários anuais por curso a partir de 2012, exceto o ano corrente
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public static List<sComboCalendario> SelecionaAnosAnterioresPorCurso(int cur_id, Guid ent_id, int appMinutosCacheLongo = 0)
        {
            List<sComboCalendario> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaAnosAnterioresPorCurso(cur_id, ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CalendarioAnualDAO().SelecionaAnosAnterioresPorCurso(cur_id, ent_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboCalendario
                                 {
                                     cal_id = dr["cal_id"].ToString()
                                     ,
                                     cal_ano_desc = dr["cal_ano_desc"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboCalendario>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CalendarioAnualDAO().SelecionaAnosAnterioresPorCurso(cur_id, ent_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboCalendario
                             {
                                 cal_id = dr["cal_id"].ToString()
                                 ,
                                 cal_ano_desc = dr["cal_ano_desc"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Seleciona ano letivo do calendário corrente
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public static int SelecionaAnoLetivoCorrente(Guid ent_id)
        {
            return new ACA_CalendarioAnualDAO().SelecionaAnoLetivoCorrente(ent_id);
        }

        /// <summary>
        /// Verifica se o calendário escolar está sendo utilizado em alguma turma do peja
        /// </summary>      
        /// <param name="cal_id">ID do calendário escolar</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTurmaPejaExistente
        (
            int cal_id
            , Guid ent_id
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_VerificaTurmaPeja(cal_id, ent_id);
        }

        /// <summary>
        /// Verifica se existe o Ano letivo cadastrado
        /// </summary>      
        /// <param name="cal_ano">Ano do calendário escolar</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAnoLetivoExistente
        (
            int cal_ano
            , Guid ent_id
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.VerificaAnoLetivoExistente(cal_ano, ent_id);
        }

        /// <summary>
        /// Verifica se já existe um calendário cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioAnual</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCalendarioExistente
        (
            ACA_CalendarioAnual entity
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_Nome(entity);
        }

        /// <summary>
        /// Verifica se já existe um calendário cadastrado com o mesmo ano
        /// </summary>
        /// <param name="cal_ano">Ano do calendário escolar</param> 
        /// <param name="ent_id">Entidade do usuário logado</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAnoBaseExistente
        (
            int cal_ano
            , Guid ent_id
        )
        {
            ACA_CalendarioAnualDAO dao = new ACA_CalendarioAnualDAO();
            return dao.SelectBy_AnoBase(cal_ano, ent_id);
        }

        /// <summary>
        /// Verifica se existe algum choque entre as datas dos períodos do calendário
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioAnual</param> 
        /// <param name="dtCalendarioPeriodo">DataTable de Períodos do Calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaChoqueDatasCalPeriodo
        (
            ACA_CalendarioAnual entity
            , DataTable dtCalendarioPeriodo
        )
        {
            DateTime DataInicioAnual = entity.cal_dataInicio;
            DateTime DataFimAnual = entity.cal_dataFim;

            if (DataInicioAnual < DataFimAnual)
            {
                if (dtCalendarioPeriodo.Rows.Count != 0)
                {
                    for (int i = 0; i < dtCalendarioPeriodo.Rows.Count; i++)
                    {
                        if (dtCalendarioPeriodo.Rows.Count.Equals(1) && dtCalendarioPeriodo.Rows[i].RowState == DataRowState.Deleted)
                            return true;

                        if (dtCalendarioPeriodo.Rows[i].RowState != DataRowState.Deleted)
                        {
                            if (Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataInicio"]) < Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataFim"]))
                            {
                                DateTime DataInicioPeriodo = Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataInicio"]);
                                DateTime DataFimPeriodo = Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataFim"]);

                                if (DataInicioAnual <= DataInicioPeriodo && DataFimAnual >= DataFimPeriodo)
                                {
                                    for (int j = i + 1; j < dtCalendarioPeriodo.Rows.Count; j++)
                                    {
                                        if (dtCalendarioPeriodo.Rows[j].RowState != DataRowState.Deleted)
                                        {
                                            DateTime I1 = Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataInicio"]);
                                            DateTime I2 = Convert.ToDateTime(dtCalendarioPeriodo.Rows[j]["cap_dataInicio"]);
                                            DateTime F1 = Convert.ToDateTime(dtCalendarioPeriodo.Rows[i]["cap_dataFim"]);
                                            DateTime F2 = Convert.ToDateTime(dtCalendarioPeriodo.Rows[j]["cap_dataFim"]);

                                            if (I1.Equals(I2) || I1.Equals(F2) || F1.Equals(I2) || F1.Equals(F2))
                                                throw new ArgumentException("Existe um choque entre as datas dos períodos do calendário.");

                                            if (I1 < I2 && I2 < F1)
                                                throw new ArgumentException("Existe um choque entre as datas dos períodos do calendário.");

                                            if (I2 < I1 && I1 < F2)
                                                throw new ArgumentException("Existe um choque entre as datas dos períodos do calendário.");
                                        }
                                    }
                                }
                                else
                                    throw new ArgumentException("Data do período do calendário deve estar no intervalo das datas de início e fim do calendário escolar.");
                            }
                            else
                                throw new ArgumentException("Data de início do período deve ser anterior a data final do período.");
                        }
                    }
                }
                return true;
            }

            throw new ArgumentException("Data de início do período letivo deve ser anterior a data de fim do período letivo no cadastro de calendário escolar.");
        }

        /// <summary>
        /// Inclui ou altera o calendario anual
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoNivelEnsino</param>
        /// <param name="dtCalendarioPeriodo">DataTable de Períodos do Calendário</param>
        /// <param name="ltCalendarioCurso">Lista de Cursos do Calendário</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            ACA_CalendarioAnual entity
            , DataTable dtCalendarioPeriodo
            , List<ACA_CalendarioCurso> ltCalendarioCurso
            , Guid ent_id
            , TalkDBTransaction bancoGestao = null
            , TalkDBTransaction bancoCore = null
        )
        {
            ACA_CalendarioAnualDAO calDao = new ACA_CalendarioAnualDAO();
            if (bancoGestao == null)
                calDao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                calDao._Banco = bancoGestao;

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            if (bancoCore == null)
                entDao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                entDao._Banco = bancoCore;            

            try
            {
                if (ltCalendarioCurso.Count == 0)
                    throw new ArgumentException("É necessário informar pelo menos um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + " para o calendário escolar.");

                if (entity.Validate())
                {
                    if (VerificaCalendarioExistente(entity))
                        throw new DuplicateNameException("Já existe um calendário escolar cadastrado com este nome.");

                    // Exclui cache de períodos guardado para o calendário.
                    if ((!entity.IsNew) && (HttpContext.Current != null))
                    {
                        string chave = "TipoPeriodoCalendarioBy_cal_id_" + entity.cal_id;

                        HttpContext.Current.Cache.Remove(chave);
                    }

                    #region Período

                    if (VerificaChoqueDatasCalPeriodo(entity, dtCalendarioPeriodo))
                    {
                        ACA_CalendarioPeriodoDAO CAlendarioPeriodoDAO = new ACA_CalendarioPeriodoDAO();
                        ACA_CalendarioPeriodo entityCalPeriodo = new ACA_CalendarioPeriodo {cal_id = entity.cal_id};
                        calDao.Salvar(entity);
                        DataTable dtCalendarioPeriodoBanco = ACA_CalendarioPeriodoBO.Seleciona_cal_id(entity.cal_id, false, 0, 0);

                        //Percorre o dtPeriodoCalendario adicionando e alterando os períodos
                        foreach (DataRow row in dtCalendarioPeriodo.Rows)
                        {
                            int cap_id = Convert.ToInt32(row["cap_id"].ToString());

                            entityCalPeriodo.cal_id = entity.cal_id;
                            entityCalPeriodo.cap_id = cap_id;
                            entityCalPeriodo.cap_descricao = row["cap_descricao"].ToString();
                            entityCalPeriodo.tpc_id = Convert.ToInt32(row["tpc_id"].ToString());
                            entityCalPeriodo.cap_dataInicio = Convert.ToDateTime(row["cap_dataInicio"].ToString());
                            entityCalPeriodo.cap_dataFim = Convert.ToDateTime(row["cap_dataFim"].ToString());
                            entityCalPeriodo.cap_situacao = 1;

                            entityCalPeriodo.IsNew = (cap_id == 0);
                            ACA_CalendarioPeriodoBO.Save(entityCalPeriodo, calDao._Banco);

                        }

                        foreach (DataRow r in dtCalendarioPeriodoBanco.Rows)
                        {
                            bool exists = false;
                            int cap_id_banco = Convert.ToInt32(r["cap_id"].ToString());

                            //Percorre o dtPeriodoCalendario verificando se o cap_id_banco existe no DataTable
                            foreach (DataRow row in dtCalendarioPeriodo.Rows)
                            {
                                int cap_id = Convert.ToInt32(row["cap_id"].ToString());

                                if (cap_id_banco == cap_id)
                                    exists = true;
                            }
                            //Se não existe deleta lógicamente o campo
                            if (!exists)
                            {
                                entityCalPeriodo.cap_id = Convert.ToInt32(r["cap_id", DataRowVersion.Original].ToString());
                                CAlendarioPeriodoDAO._Banco = calDao._Banco;
                                CAlendarioPeriodoDAO.Delete(entityCalPeriodo);
                            }
                        }
                    }

                    #endregion

                    #region Curso

                    //Deleta as associações do curso com o calendário
                    if (!entity.IsNew)
                        ACA_CalendarioCursoBO.DeletarPorCalendario(entity.cal_id, entity.ent_id, ltCalendarioCurso, calDao._Banco);

                    // Inclui as associações de curso com o calendário.
                    foreach (ACA_CalendarioCurso entityCalendarioCurso in ltCalendarioCurso)
                    {
                        // Exclui cache de calendário guardado para o curso.
                        if (HttpContext.Current != null)
                        {
                            string chave = string.Format("CalendarioAnual_{0}", entityCalendarioCurso.cur_id);
                            HttpContext.Current.Cache.Remove(chave);
                        }

                        entityCalendarioCurso.cal_id = entity.cal_id;
                        ACA_CalendarioCursoBO.Save(entityCalendarioCurso, calDao._Banco);
                    }

                    #endregion
                }
                else
                    throw new ValidationException(entity.PropertiesErrorList[0].Message);

                //Incrementa um na integridade da entidade
                if (entity.IsNew)
                    entDao.Update_IncrementaIntegridade(entity.ent_id);

                GestaoEscolarUtilBO.LimpaCache(string.Format(ACA_TipoPeriodoCalendarioBO.chaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente + "_{0}", entity.cal_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaDisciplinaPorTurmaDocente);
                CacheManager.Factory.RemoveByPattern(ModelCache.ALUNOS_ATIVOS_COC_DISCIPLINA_PATTERN_KEY);
                CacheManager.Factory.RemoveByPattern(ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY);
                LimpaCache(entity);

                return true;
            }
            catch (Exception err)
            {
                calDao._Banco.Close(err);
                entDao._Banco.Close(err); 

                throw;
            }
            finally
            {
                if (calDao._Banco.ConnectionIsOpen && bancoGestao == null)
                    calDao._Banco.Close();

                if (entDao._Banco.ConnectionIsOpen && bancoCore == null)
                    entDao._Banco.Close();
            }
        }

        /// <summary>
        /// Deleta logicamente o calendário escolar
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioAnual</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_CalendarioAnual entity
        )
        {
            ACA_CalendarioAnualDAO calDao = new ACA_CalendarioAnualDAO();
            calDao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Verifica pela FK se existe algum registro em outra tabela usando o calendário.
                if (GestaoEscolarUtilBO.VerificarIntegridade
                (
                    "cal_id"
                    , entity.cal_id.ToString()
                    , "ACA_CalendarioAnual,ACA_CalendarioCurso,ACA_CalendarioEscola,ACA_CalendarioPeriodo,REL_AlunosSituacaoFechamento,MTR_MatriculasBoletimDisciplina"
                    , calDao._Banco
                ))
                {
                    throw new ValidationException("Não é possível excluir o calendário pois possui outros registros ligados a ele.");
                }

                //Decrementa um na integridade da entidade
                entDao.Update_DecrementaIntegridade(entity.ent_id);

                //Deleta logicamente o calendário escolar
                calDao.Delete(entity);

                LimpaCache(entity);

                return true;
            }
            catch (Exception err)
            {
                calDao._Banco.Close(err);
                entDao._Banco.Close(err);

                throw;
            }
            finally
            {
                calDao._Banco.Close();
                entDao._Banco.Close();
            }
        }
    }
}

