using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Data;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_EscalaAvaliacaoNumericaBO : BusinessBase<ACA_EscalaAvaliacaoNumericaDAO, ACA_EscalaAvaliacaoNumerica>
    {
        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_EscalaAvaliacaoNumerica entity)
        {
            return string.Format(ModelCache.ESCALA_AVALIACAO_NUMERICA_MODEL_KEY, entity.esa_id);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_EscalaAvaliacaoNumerica GetEntity(ACA_EscalaAvaliacaoNumerica entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_EscalaAvaliacaoNumericaDAO dao = new ACA_EscalaAvaliacaoNumericaDAO();
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
        private static void LimpaCache(ACA_EscalaAvaliacaoNumerica entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
            CacheManager.Factory.Remove(RetornaChaveCache_GetSelectBy_Escala(entity.esa_id));
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_GetSelectBy_Escala(int esa_id)
        {
            return string.Format(ModelCache.ESCALA_AVALIACAO_NUMERICA_SELECTBY_ESCALA_KEY, esa_id.ToString());
        }

        /// <summary>
        /// Retorna um list com as escalas numericas.
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <param name="appMinutosCacheCurto"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_EscalaAvaliacaoNumerica> GetSelectBy_Escala(int esa_id)
        {
            return GetSelectBy_Escala(esa_id, false, 0, 0);
        }

        /// <summary>
        /// Retorna um list com as escalas numericas.
        /// </summary>
        /// <param name="esa_id">ID da escala</param>
        /// <param name="appMinutosCacheCurto"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_EscalaAvaliacaoNumerica> GetSelectBy_Escala(int esa_id, bool paginado, int currentPage, int pageSize)
        {
            List<ACA_EscalaAvaliacaoNumerica> retorno = new List<ACA_EscalaAvaliacaoNumerica>();
            ACA_EscalaAvaliacaoNumericaDAO dao = new ACA_EscalaAvaliacaoNumericaDAO();

            string chave = RetornaChaveCache_GetSelectBy_Escala(esa_id);

            retorno = CacheManager.Factory.Get(
                        chave,
                            () =>
                            {
                                totalRecords = 0;
                                return (from DataRow escala in dao.SelectBy_esa_id(esa_id, 1, paginado, currentPage, pageSize, out totalRecords).Rows
                                        select dao.DataRowToEntity(escala, new ACA_EscalaAvaliacaoNumerica())).ToList();
                            },
                            GestaoEscolarUtilBO.MinutosCacheMedio
                       );

            return retorno;
        }

        public static bool VerificaValor (decimal valorMenor, decimal valorMaior)
        {
            if (valorMaior > valorMenor)
                return true;
            else
                return false;
        }
    }
}
