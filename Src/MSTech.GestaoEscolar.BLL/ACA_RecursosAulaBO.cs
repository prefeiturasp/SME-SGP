/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{

    /// <summary>
    /// ACA_RecursosAula Business Object 
    /// </summary>
    public class ACA_RecursosAulaBO : BusinessBase<ACA_RecursosAulaDAO, ACA_RecursosAula>
    {
        #region Constantes

        public const string sChaveCache = "Cache_RecursosAula";

        #endregion

        #region Saves

        public new static bool Save(ACA_RecursosAula entity)
        {
            if (entity.IsNew)
            {
                entity.rsa_dataAlteracao = DateTime.Now;
                entity.rsa_dataCriacao = DateTime.Now;
            }
            else
            {
                entity.rsa_dataAlteracao = DateTime.Now;
            }

            //Removendo o cache
            RemoveCacheRecursosAula();

            ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
            return dao.Salvar(entity);
        }

        #endregion

        #region Metodos

        #region Cache

        /// <summary>
        /// Remove do cache da consulta de recursos da aula
        /// </summary>
        public static void RemoveCacheRecursosAula()
        {
            if (HttpContext.Current != null)
            {
                string chave = sChaveCache;
                HttpContext.Current.Cache.Remove(chave);
            }
        }

        #endregion

        /// <summary>
        /// Deleta logicamente um recurso de aula
        /// </summary>
        /// <param name="entity">Entidade ACA_RecursosAula</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_RecursosAula entity
        )
        {
            ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o recurso de aula pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("rsa_id", entity.rsa_id.ToString(), "ACA_RecursosAula", dao._Banco))
                    throw new ValidationException("Não é possível excluir o recurso de aula pois possui outros registros ligados a ele.");

                //Removendo o cache
                RemoveCacheRecursosAula();

                //Deleta logicamente o recurso de aula
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        ///Busca os recursos de aula buscando por nome
        /// </summary>
        /// <param name="rsa_nome"></param>
        /// <returns></returns>
        public static DataTable GetRecursoAulaBy_rsa_nome
           (
                string rsa_nome
           )
        {
            ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
            return dao.SelectBy_rsa_nome(rsa_nome);
        }

        /// <summary>
        ///Busca os recursos de aula não excluídos
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetRecursoAulaBy_All_Paginado(int currentPage
            , int pageSize)
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
            return dao.SelectBy_All(true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        ///Busca os recursos de aula não excluídos - não paginado
        /// </summary>
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_RecursosAula> GetRecursoAulaBy_All(int appMinutosCacheLongo = 0)
        {
            totalRecords = 0;
            ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
            List<ACA_RecursosAula> dados = null;

            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = sChaveCache;
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        dados = dao.SelectBy_All(false, 0, 0, out totalRecords).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new ACA_RecursosAula())).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<ACA_RecursosAula>)cache;
                    }
                }
            }

            // Se não carregou pelo cache, seleciona os dados do banco.
            if (dados == null)
                dados = dao.SelectBy_All(false, 0, 0, out totalRecords).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new ACA_RecursosAula())).ToList();

            return dados;
        }

        #endregion
    }
}