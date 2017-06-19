using System;
using System.Data;
using System.ComponentModel;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;
using System.Linq;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    /// <summary>
    /// Dados do tipo disciplina.
    /// </summary>
    [Serializable]
    public struct sTipoDisciplina
    {
        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }
        public int dis_id { get; set; }
        public string dis_codigo { get; set; }
        public string dis_nome { get; set; }
        public int dis_cargaHorariaTeorica { get; set; }
        public int dis_cargaHorariaPratica { get; set; }
        public int dis_cargaHorariaSupervisionada { get; set; }
        public int dis_cargaHorariaExtra { get; set; }
        public int tds_id { get; set; }
        public string tds_nome { get; set; }
        public string tne_nome { get; set; }
        public string tne_tds_nome { get; set; }
        public string tds_base_nome { get; set; }
        public int tds_base { get; set; }
        public string tds_situacao { get; set; }
    }

    #endregion Estruturas

    public class ACA_TipoDisciplinaBO : BusinessBase<ACA_TipoDisciplinaDAO, ACA_TipoDisciplina>
    {
        #region Enumeradores

        public enum TipoDisciplina
        {
            Disciplina = 1
            ,
            EnriquecimentoCurricular = 2
            ,
            RegenciaClasse = 3
            ,
            TerritorioSaber = 4
            ,
            NaoCadastrada = 5
            ,
            RecuperacaoParalela = 6
            ,
            AtendimentoEducacionalEspecializado = 7
        }

        #endregion Enumeradores

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_TipoDisciplina entity)
        {
            return string.Format(ModelCache.TIPO_DISCIPLINA_MODEL_KEY, entity.tds_id);
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_TipoDisciplina entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
            GestaoEscolarUtilBO.LimpaCache("Cache_SelecionaTipoDisciplinaObjetosAprendizagem");
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_TipoDisciplina GetEntity(ACA_TipoDisciplina entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
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
        /// Gera os numeros das ordens para os tipos disciplinas se escolher o parametro academico CONTROLAR_ORDEM_DISCIPLINAS
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static void OrdenaTiposDisciplinas()
        {
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            dao.Ordena_TipoDisciplina_tds_Ordem();
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de disciplina
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Altera a ordem do tipo de disciplina
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo disciplina</param>
        /// <param name="entityDescer">Entidade do tipo disciplina</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_TipoDisciplina entityDescer
            , ACA_TipoDisciplina entitySubir
        )
        {
            LimpaCache(entityDescer);
            LimpaCache(entitySubir);

            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente
        /// Sem paginação
        /// </summary>            
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaNaoPaginado(Guid ent_id)
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            return dao.SelectBy_Pesquisa(0, 0, 0, false, controlarOrdem, false, 1, 1, out totalRecords);
        }
        
        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente
        /// Sem paginação
        /// </summary>   
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoDisciplina> SelecionaTipoDisciplina(Guid ent_id, int AppMinutosCacheLongo = 0)
        {
            List<sTipoDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_SelecionaTipoDisciplina_{0}", ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

                    lista = (from dr in new ACA_TipoDisciplinaDAO().SelectBy_Pesquisa(0, 0, 0, true, controlarOrdem, false, 1, 1, out totalRecords).AsEnumerable()
                             select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoDisciplina>)cache;
            }
            else
            {
                bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

                lista = (from dr in new ACA_TipoDisciplinaDAO().SelectBy_Pesquisa(0, 0, 0, true, controlarOrdem, false, 1, 1, out totalRecords).AsEnumerable()
                         select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();
            }

            return lista;
        }
        
        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente
        /// Sem paginação
        /// </summary>   
        /// <param name="cal_ano">Ano do objeto de aprendizagem</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uad_idSuperior">ID da unidade superior</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoDisciplina> SelecionaTipoDisciplinaObjetosAprendizagem(Guid ent_id, int cal_ano, int esc_id, Guid uad_idSuperior, int AppMinutosCacheLongo = 0)
        {
            List<sTipoDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_SelecionaTipoDisciplinaObjetosAprendizagem_{0}_{1}_{2}_{3}", ent_id, cal_ano, esc_id, uad_idSuperior);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

                    lista = (from dr in new ACA_TipoDisciplinaDAO().SelectBy_ObjetosAprendizagem(cal_ano, true, controlarOrdem, esc_id, uad_idSuperior, out totalRecords).AsEnumerable()
                             select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoDisciplina>)cache;
            }
            else
            {
                bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

                lista = (from dr in new ACA_TipoDisciplinaDAO().SelectBy_ObjetosAprendizagem(cal_ano, true, controlarOrdem, esc_id, uad_idSuperior, out totalRecords).AsEnumerable()
                         select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplina_Curso(int cur_id, Guid ent_id)
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();

            DataTable dt = dao.SelectBy_Pesquisa_TipoDisciplina_Curso(controlarOrdem, cur_id);
            DataRow dr = dt.NewRow();
            dr["tds_id"] = -1;
            dr["tds_nome"] = "-- Selecione um(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --";
            dt.Rows.InsertAt(dr, 0);

            //DataView dv = dt.DefaultView;
            //dv.Sort = "tds_nome";

            return dt;

        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaTodasEletivaAluno(Guid ent_id)
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            return dao.SelectBy_Pesquisa(0, 0, 0, false, controlarOrdem, false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente por nível de ensino
        /// Sem paginação
        /// </summary>       
        /// <param name="tne_id">ID do tipo de nível de ensino</param>  
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaPorNivelEnsino
        (
            int tne_id
            , Guid ent_id
        )
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            return dao.SelectBy_Pesquisa(0, tne_id, 0, true, controlarOrdem, false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina obrigatórias para o nível de ensino
        /// </summary>       
        /// <param name="tne_id">ID do tipo de nível de ensino</param>  
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaObrigatoriasPorNivelEnsinoEventoAno
        (
            int tne_id
            , int tme_id
            , Guid ent_id
            , long doc_id
            , string eventosAbertos
            , int cal_ano
        )
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            return new ACA_TipoDisciplinaDAO().SelecionaObrigatoriasPorNivelEnsinoEvento(tne_id, tme_id, controlarOrdem, doc_id, eventosAbertos, cal_ano);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina obrigatórias para o nível de ensino
        /// </summary>       
        /// <param name="tne_id">ID do tipo de nível de ensino</param>  
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaObrigatoriasPorNivelEnsinoEvento
        (
            int tne_id
            , int tme_id
            , Guid ent_id
            , long doc_id
            , string eventosAbertos
        )
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            return new ACA_TipoDisciplinaDAO().SelecionaObrigatoriasPorNivelEnsinoEvento(tne_id, tme_id, controlarOrdem, doc_id, eventosAbertos, 0);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente por nível de ensino
        /// Sem paginação
        /// </summary>       
        /// <param name="tne_id">ID do tipo de nível de ensino</param>  
        /// <param name="tds_base">Base da disciplina</param>  
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaPorNivelEnsinoBase
        (
            int tne_id
            , int tds_base
            , Guid ent_id
        )
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
            return dao.SelectBy_Pesquisa_SemRegencia(0, tne_id, tds_base, true, controlarOrdem, false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente por tipo de disciplina
        /// Sem paginação
        /// </summary>        
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="banco">Transação com banco</param> 
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaPorTipoDisciplina
        (
            int tds_id
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
            ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();

            if (banco != null)
                dao._Banco = banco;

            return dao.SelectBy_Pesquisa(tds_id, 0, 0, true, controlarOrdem, false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente por tipo de disciplina
        /// Sem paginação
        /// </summary>        
        /// <param name="tds_id">ID do tipo de disciplina</param> 
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaPorTipoDisciplina
        (
            int tds_id
            , Guid ent_id
        )
        {
            return SelecionaTipoDisciplinaPorTipoDisciplina(tds_id, null, ent_id);
        }
        
        /// <summary>
        /// seleciona as disciplinas conforme o cargo selecionado
        /// </summary>
        /// <param name="crg_id"></param>
        /// <returns></returns>
        public static DataTable GetSelect_Disciplina(int crg_id)
        {
            try
            {
                ACA_TipoDisciplinaDAO dao = new ACA_TipoDisciplinaDAO();
                return dao.GetSelect_Disciplina(crg_id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        public static List<sTipoDisciplina> SelecionaTipoDisciplinaPorCursoCurriculoPeriodo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int AppMinutosCacheLongo = 0
        )
        {
            List<sTipoDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_SelecionaTipoDisciplinaPorCursoCurriculoPeriodo_{0}_{1}_{2}"
                                             , cur_id
                                             , crr_id
                                             , crp_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id, false, 1, 1, out totalRecords).AsEnumerable()
                             select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoDisciplina>)cache;
            }
            else
                lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id, false, 1, 1, out totalRecords).AsEnumerable()
                         select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="cap_id"></param>
        /// <returns></returns>
        public static List<sTipoDisciplina> SelecionaTipoDisciplinaPorCursoCurriculoPeriodoEscola
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cap_id
            , int AppMinutosCacheLongo = 0
        )
        {
            List<sTipoDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_SelecionaTipoDisciplinaPorCursoCurriculoPeriodoEscola_{0}_{1}_{2}_{3}_{4}_{5}_{6}"
                                             , cur_id
                                             , crr_id
                                             , crp_id
                                             , esc_id
                                             , uni_id
                                             , cal_id
                                             , cap_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_cur_id_crr_id_crp_id_esc_id_uni_id(cur_id, crr_id, crp_id, esc_id, uni_id, cal_id, cap_id, false, 1, 1, out totalRecords).AsEnumerable()
                             select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoDisciplina>)cache;
            }
            else
                lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_cur_id_crr_id_crp_id_esc_id_uni_id(cur_id, crr_id, crp_id, esc_id, uni_id, cal_id, cap_id, false, 1, 1, out totalRecords).AsEnumerable()
                         select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// Carrega todos os tipos de disciplinas de acordo com os filtros informados
        /// exceto as disciplinas do tipo Eletiva do aluno e as disciplinas do tipo informado
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="crd_tipo">Tipo de disciplina que NÃO será carregado</param>
        /// <returns>DataTable com os dados</returns>
        public static DataTable SelecionaTipoDisciplinaPorCursoCurriculoPeriodoTipoDisciplina
        (
            int cur_id
            , int crr_id
            , int crp_id
            , byte crd_tipo
        )
        {
            ACA_TipoDisciplinaDAO dal = new ACA_TipoDisciplinaDAO();
            return dal.SelectBy_CursoCurriculoPeriodo(cur_id, crr_id, crp_id, crd_tipo);
        }

        /// <summary>
        /// Carrega os tipos de disciplinas por curso e ciclo
        /// </summary>
        /// <param name="cur_id">Campo Id da tabela ACA_Curso</param>
        /// <param name="tci_id">Id do tipo de ciclo</param>
        /// <returns>Lista com os tipos de disciplina</returns>
        public static List<ACA_TipoDisciplina> SelecionaTipoDisciplinaPorCursoTipoCiclo(int cur_id, int tci_id, int esc_id, int AppMinutosCacheLongo = 0)
        {
            List<ACA_TipoDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_SelecionaTipoDisciplinaPorCursoTipoCiclo_{0}_{1}_{2}"
                                             , cur_id
                                             , tci_id
                                             , esc_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_CursoTipoCiclo(cur_id, tci_id, esc_id).AsEnumerable()
                             select (ACA_TipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplina())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<ACA_TipoDisciplina>)cache;
            }
            else
                lista = (from dr in new ACA_CurriculoDisciplinaDAO().SelectBy_CursoTipoCiclo(cur_id, tci_id, esc_id).AsEnumerable()
                         select (ACA_TipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna os tipos de disciplina por escola.
        /// </summary>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoDisciplina> SelecionaTipoDisciplinaPorEscola(int esc_id)
        {
            List<sTipoDisciplina> lista = (from dr in new ACA_TipoDisciplinaDAO().SelectByEscola(esc_id).AsEnumerable()
                                           select (sTipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoDisciplina())).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna o tipo de disciplina que não foram excluídos logicamente
        /// filtrado por tud_id.
        /// </summary>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_TipoDisciplina SelecionaTipoDisciplinaPorTudId(long tudId)
        {
            DataTable dt = new ACA_TipoDisciplinaDAO().SelecionaByTudId(tudId);
            if (dt.Rows.Count > 0)
            {
                return (ACA_TipoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dt.Rows[0], new ACA_TipoDisciplina());
            }
            return new ACA_TipoDisciplina();
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina relacionadas pelo tipo do tipo de disciplina.
        /// </summary>    
        /// <param name="tds_id">ID do tipo de disciplina de recuperação paralela</param>
        /// <param name="tds_tipo">Tipo do tipo de disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaRelacionadaPorTipo
        (
            int tds_id
            , string tds_tipo
        )
        {
            return new ACA_TipoDisciplinaDAO().SelecionaTipoDisciplinaRelacionadaPorTipo(tds_id, tds_tipo);
        }
    }
}
