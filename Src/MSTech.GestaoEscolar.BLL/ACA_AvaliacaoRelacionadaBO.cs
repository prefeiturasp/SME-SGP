using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region
    /// <summary>
    /// Estrutura utilizada para armazenar as avaliações relacionadas. 
    /// </summary>
    public struct sAvaliacaoRelacionada
    {
        public int ava_idRelacionada { get; set; }
        public int tpc_idRelacionado { get; set; }
    }
    #endregion

    public class ACA_AvaliacaoRelacionadaBO : BusinessBase<ACA_AvaliacaoRelacionadaDAO, ACA_AvaliacaoRelacionada>    
    {
        /// <summary>
        /// Retorna uma string com os ava_id (separados por ",") das avaliações 
        /// relacionadas à avaliação passada por parâmetro.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns></returns>
        public static string RetornaRelacionadasPor_Avaliacao
        (
             int fav_id
            , int ava_id
            , int appMinutosCacheMedio
        )
        {
            string ret = "";
            ACA_AvaliacaoRelacionadaDAO dao = new ACA_AvaliacaoRelacionadaDAO();
            List<sAvaliacaoRelacionada> lstAvaliacaoRelacionada = null;

            Func<List<sAvaliacaoRelacionada>> retorno = delegate()
            {
                using (DataTable dt = dao.SelectBy_Avaliacao(fav_id, ava_id))
                {
                    return dt.AsEnumerable().Select(m => new sAvaliacaoRelacionada()
                    {
                        ava_idRelacionada = m.Field<int>("ava_idRelacionada"),
                        tpc_idRelacionado = m.Field<int>("tpc_idRelacionado")
                    }).ToList();
                }
            };


            if (appMinutosCacheMedio > 0)
            {
                string chave = String.Format(RetornaChaveCache_RetornaRelacionadasPor_Avaliacao()
                                             , fav_id, ava_id);

                lstAvaliacaoRelacionada = CacheManager.Factory.Get
                                          (
                                              chave,
                                              retorno,
                                              appMinutosCacheMedio
                                          );
            }
            else
            {
                lstAvaliacaoRelacionada = retorno();
            }

            if (lstAvaliacaoRelacionada != null)
            {
                foreach (sAvaliacaoRelacionada lstItem in lstAvaliacaoRelacionada)
                {
                    if (!string.IsNullOrEmpty(ret))
                        ret += ",";

                    ret += lstItem.ava_idRelacionada.ToString();
                }
            }

            return ret;
        }

        /// <summary>
        /// Retorna uma string com os tpc_id (separados por ",") das avaliações 
        /// relacionadas à avaliação passada por parâmetro.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns></returns>
        public static string RetornaPeriodoCalendarioRelacionadosPorAvaliacao(int fav_id, int ava_id)
        {
            List<sAvaliacaoRelacionada> lstAvaliacaoRelacionada = null;
            DataTable dt = null;
            if (HttpContext.Current != null)
            {
                string chave = String.Format(RetornaChaveCache_RetornaPeriodoCalendarioRelacionadosPorAvaliacao()
                                             , fav_id, ava_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dt = new ACA_AvaliacaoRelacionadaDAO().SelectBy_Avaliacao(fav_id, ava_id);

                    // converte um dataTable em uma lista
                    lstAvaliacaoRelacionada = dt.AsEnumerable().Select(m => new sAvaliacaoRelacionada()
                    {
                        ava_idRelacionada = m.Field<int>("ava_idRelacionada"),
                        tpc_idRelacionado = m.Field<int>("tpc_idRelacionado")
                    }).ToList();

                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, lstAvaliacaoRelacionada, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    lstAvaliacaoRelacionada = (List<sAvaliacaoRelacionada>)cache;
                }
            }
            return string.Join(",", lstAvaliacaoRelacionada.Select(lst => lst.tpc_idRelacionado.ToString()).ToArray());
        }

        /// <summary>
        /// Retorna um datatable contendo todas as avaliações relacionadas
        /// que não foram excluídas logicamente, filtradas por 
        ///	fav_id_ava_id      
        /// </summary>
        /// <param name="fav_id">ID de Formato Avaliacao</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_fav_id
        (
             int fav_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
           if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_AvaliacaoRelacionadaDAO dao = new ACA_AvaliacaoRelacionadaDAO();
            return dao.SelectBy_fav_id(fav_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Cria List de Avaliacao Relacionada, cada entidade recebe um registro do DataTable.
        /// </summary>
        /// <returns>List Entidade Avaliacao Relacioncada</returns>
        public static List<ACA_AvaliacaoRelacionada> CriaList_Entities_AvaliacaoRelacionada(DataTable dtAvaliacaoRelacionada)
        {
            //cria List
            List<ACA_AvaliacaoRelacionada> lt = new List<ACA_AvaliacaoRelacionada>();
            for (int i = 0; i < dtAvaliacaoRelacionada.Rows.Count; i++)
            {
                //cria entidade
                ACA_AvaliacaoRelacionada entityAvaliacaoRelacionada = new ACA_AvaliacaoRelacionada();
                //verifica se registro do DataTable é um novo registro
                if (dtAvaliacaoRelacionada.Rows[i].RowState == DataRowState.Added)
                {
                    entityAvaliacaoRelacionada.fav_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["fav_id"]);
                    entityAvaliacaoRelacionada.ava_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_id"]);
                    entityAvaliacaoRelacionada.avr_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["avr_id"]);
                    entityAvaliacaoRelacionada.ava_idRelacionada = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_idRelacionada"]);
                    entityAvaliacaoRelacionada.avr_substituiNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_substituiNota"]);
                    entityAvaliacaoRelacionada.avr_mantemMaiorNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_mantemMaiorNota"]);
                    entityAvaliacaoRelacionada.avr_obrigatorioNotaMinima = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_obrigatorioNotaMinima"]);
                    entityAvaliacaoRelacionada.avr_situacao = Convert.ToByte(1);
                    entityAvaliacaoRelacionada.IsNew = true;
                    //adiciona entidade na List
                    lt.Add(entityAvaliacaoRelacionada);
                }
                    //verifica se registro do Datable foi deletado.
                else if (dtAvaliacaoRelacionada.Rows[i].RowState == DataRowState.Deleted)
                {
                    //instancia valores para entidade como um registro deletado logicamente.
                    entityAvaliacaoRelacionada.fav_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["fav_id", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.ava_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_id", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.avr_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["avr_id", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.ava_idRelacionada = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_idRelacionada", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.avr_substituiNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_substituiNota", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.avr_mantemMaiorNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_mantemMaiorNota", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.avr_obrigatorioNotaMinima = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_obrigatorioNotaMinima", DataRowVersion.Original]);
                    entityAvaliacaoRelacionada.avr_situacao = Convert.ToByte(3);
                    entityAvaliacaoRelacionada.IsNew = false;
                    //adiciona entidade na List
                    lt.Add(entityAvaliacaoRelacionada);
                }
                    //em ultimo caso registro é um registro já existente e não foi modificado.
                else
                {
                    //instancia valores para entidade como um registro já existente sem modificação. Atualiza apenas data de alteração
                    //para este registro.
                    entityAvaliacaoRelacionada.fav_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["fav_id"]);
                    entityAvaliacaoRelacionada.ava_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_id"]);
                    entityAvaliacaoRelacionada.avr_id = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["avr_id"]);
                    entityAvaliacaoRelacionada.ava_idRelacionada = Convert.ToInt32(dtAvaliacaoRelacionada.Rows[i]["ava_idRelacionada"]);
                    entityAvaliacaoRelacionada.avr_substituiNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_substituiNota"]);
                    entityAvaliacaoRelacionada.avr_mantemMaiorNota = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_mantemMaiorNota"]);
                    entityAvaliacaoRelacionada.avr_obrigatorioNotaMinima = Convert.ToBoolean(dtAvaliacaoRelacionada.Rows[i]["avr_obrigatorioNotaMinima"]);
                    entityAvaliacaoRelacionada.avr_situacao = Convert.ToByte(1);
                    entityAvaliacaoRelacionada.IsNew = false;
                    //adiciona entidade na List
                    lt.Add(entityAvaliacaoRelacionada);
                }
            }
            //retorna List
            return lt;
        }
        
        /// <summary>
        /// Retorno booleano na qual verifica se já existe a avaliação relacionada para a avaliação 
        /// cadastrado no banco com situação diferente de Excluido.
        /// </summary>
        /// <param name="entity1">Entidade de avaliação relacionada</param>
        /// <param name="entity2"></param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaMesmaAvaliacaoRelacionada(ACA_AvaliacaoRelacionada entity1, ACA_AvaliacaoRelacionada entity2)
        {
            if (entity1.avr_situacao != 3 && entity2.avr_situacao != 3)
            {
                if (entity1.ava_idRelacionada == entity2.ava_idRelacionada)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_RetornaRelacionadasPor_Avaliacao()
        {
            return "Cache_RetornaRelacionadasPor_Avaliacao_fav_id_{0}_ava_id_{1}";
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_RetornaPeriodoCalendarioRelacionadosPorAvaliacao()
        {
            return "Cache_RetornaPeriodoCalendarioRelacionadosPorAvaliacao_fav_id_{0}_ava_id_{1}";
        }

    }
}
