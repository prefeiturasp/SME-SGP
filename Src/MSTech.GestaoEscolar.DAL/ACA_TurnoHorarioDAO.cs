/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_TurnoHorarioDAO : Abstract_ACA_TurnoHorarioDAO
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trn_id"></param>
        /// <returns></returns>
        public List<ACA_TurnoHorario> ListaHorariosPorTurnoId
        (
            int trn_id
        )
        {
            List<ACA_TurnoHorario> list = new List<ACA_TurnoHorario>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TurnoHorario_SelectBy_Turno", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 1;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ACA_TurnoHorario entity = new ACA_TurnoHorario();
                    list.Add(DataRowToEntity(dr, entity));
                }

                return list;
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

        /// <summary>
        /// Retorna um datatable contendo todos os Horarios
        /// que não foram excluídas logicamente, filtradas por 
        /// ID Turno, paginado.
        /// </summary>
        /// <param name="trn_id">ID de Turno</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com o turno horario</returns>
        public DataTable SelectBy_trn_id
        (
            int trn_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TurnoHorario_SelectBy_trn_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 1;
                if (trn_id > 0)
                    Param.Value = trn_id;
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

                return qs.Return;
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

        /// <summary>
        /// Retorna um datatable contendo todos os dias da semana cadastrados para o turno        
        /// </summary>
        /// <param name="trn_id">ID do Turno</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable Select_thr_diaSemana
        (
            int trn_id
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TurnoHorario_Select_trh_diaSemana_By_trn_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 1;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
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