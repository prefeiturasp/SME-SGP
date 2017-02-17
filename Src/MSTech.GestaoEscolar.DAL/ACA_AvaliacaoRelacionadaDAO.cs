/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_AvaliacaoRelacionadaDAO : Abstract_ACA_AvaliacaoRelacionadaDAO
	{
        /// <summary>
        /// Retorna os ava_id das avaliações relacionadas à avaliação passada
        /// por parâmetro.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns></returns>
        public DataTable SelectBy_Avaliacao
        (
            int fav_id
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AvaliacaoRelacionada_SelectBy_Avaliacao", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as avaliações relacionadas
        /// que não foram excluídas logicamente, filtradas por 
        ///	fav_id_ava_id      
        /// </summary>
        /// <param name="fav_id">ID de Formato Avaliacao</param>
        /// <param name="ava_id">ID de Avaliacao</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_fav_id
        (
            int fav_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AvaliacaoRelacionada_SelectBy_fav_id", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                if (fav_id > 0)
                    Param.Value = fav_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
	}
}