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
    public class ACA_EscalaAvaliacaoParecerBO : BusinessBase<ACA_EscalaAvaliacaoParecerDAO, ACA_EscalaAvaliacaoParecer>
    {
        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_EscalaAvaliacaoParecer GetEntity(ACA_EscalaAvaliacaoParecer entity, TalkDBTransaction banco = null)
        {
            // Chave padrão do cache - nome do método + parâmetros.
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
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
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_EscalaAvaliacaoParecer entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
            CacheManager.Factory.Remove(RetornaChaveCache_GetSelectBy_Escala(entity.esa_id));
            GestaoEscolarUtilBO.LimpaCache(ModelCache.ESCALA_AVALIACAO_RETORNA_ORDEM_PARECER_MODEL_KEY, entity.esa_id.ToString());
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_EscalaAvaliacaoParecer entity)
        {
            return string.Format(ModelCache.ESCALA_AVALIACAO_PARECER_MODEL_KEY, entity.esa_id, entity.eap_id);
        }

        private static string RetornaChaveCache_Ordem_Parecer(int esa_id, string eap_valor)
        {
            return String.Format(ModelCache.ESCALA_AVALIACAO_RETORNA_ORDEM_PARECER_MODEL_KEY, esa_id, eap_valor);
        }

        /// <summary>
        /// Retorna a ordem do parecer de acordo com seu valor na escala de avliação.
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <param name="eap_valor">Valor do parecer</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int RetornaOrdem_Parecer(int esa_id, string eap_valor, int appMinutosCacheLongo = 0)
        {
            int ordem = -1;

            Func<int> retorno = delegate()
            {
                ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
                using (DataTable dt = dao.SelectOrdem_Parecer(esa_id, eap_valor))
                {
                    if ((dt.Rows.Count > 0) && (!string.IsNullOrEmpty(dt.Rows[0]["ordem"].ToString())))
                    {
                        return Convert.ToInt32(dt.Rows[0]["ordem"]);
                    }
                }

                return -1;
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = RetornaChaveCache_Ordem_Parecer(esa_id, eap_valor);
                ordem = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
            }
            else
            {
                ordem = retorno();
            }

            return ordem;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta de ordem do parecer
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <param name="eap_valor">Valor do parecer</param>
        /// <returns></returns>
        private static string RetornaChaveCache_OrdemParecer(int esa_id, string eap_valor)
        {
            return string.Format(RetornaChaveCache_OrdemParecer_Exclusao(esa_id) + "{0}", eap_valor);
        }

        /// <summary>
        /// Retorna a chave do cache a ser excluida, a qual era utilizaca na consulta de ordem do parecer
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <returns></returns>
        private static string RetornaChaveCache_OrdemParecer_Exclusao(int esa_id)
        {
            return string.Format("Cache_EscalaAvaliacaoParecer_OrdemParecer_{0}_", esa_id);
        }

        /// <summary>
        /// Retorna o parecer da escala por escala e valor
        /// </summary>
        /// <param name="esa_id">ID da escala de avaliação</param>
        /// <param name="eap_valor">Valor do parecer da escala de avaliação</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Valor
        (
            int esa_id
            , string eap_valor
        )
        {
            ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
            return dao.SelectBy_Valor(esa_id, eap_valor);
        }

        /// <summary>
        /// Retorna um list com os pareceres da escala.
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_EscalaAvaliacaoParecer> GetSelectBy_Escala(int esa_id, TalkDBTransaction banco = null)
        {
            List<ACA_EscalaAvaliacaoParecer> retorno = new List<ACA_EscalaAvaliacaoParecer>();
            ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
            if (banco != null)
                dao._Banco = banco;

            string chave = RetornaChaveCache_GetSelectBy_Escala(esa_id);

            retorno = CacheManager.Factory.Get(
                        chave,
                            () =>
                            {
                                return (from DataRow escala in dao.SelectBy_Escala(esa_id).Rows
                                        select dao.DataRowToEntity(escala, new ACA_EscalaAvaliacaoParecer())).ToList();
                            },
                            GestaoEscolarUtilBO.MinutosCacheMedio
                        );

            return retorno;
        }

        /// <summary>
        /// retorna as escalas de avaliação parecer
        /// que não foram excluídas logicamente filtradas por formato avaliação(fav_id), usada na tela de busca dos quadros totalizadores
        /// </summary>
        /// <param name="fav_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Escala_Parecer
        (
           string esa_ids
        )
        {
            ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
            return dao.SelectBy_Escala_Parecer(esa_ids);
        }

        /// <summary>
        /// retorna as escalas de avaliação parecer
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Select_EscalaParametros
        (
        )
        {
            ACA_EscalaAvaliacaoParecerDAO dao = new ACA_EscalaAvaliacaoParecerDAO();
            return dao.Select_EscalaParametros(out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Seleciona_esa_id
        (
            int esa_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_EscalaAvaliacaoParecerDAO dal = new ACA_EscalaAvaliacaoParecerDAO();
            return dal.SelectBy_esa_id(esa_id, 1, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }


        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_GetSelectBy_Escala(int esa_id)
        {
            return string.Format(ModelCache.ESCALA_AVALIACAO_PARECER_SELECTBY_ESCALA_KEY, esa_id.ToString());
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_GetSelectListBy_Escala()
        {
            return "Cache_GetSelectListBy_Escala_esa_id_{0}";
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaEscalaAvaliacaoParecer()
        {
            return "Cache_SelecionaEscalaAvaliacaoParecer_esa_id_{0}";
        }

    }
}