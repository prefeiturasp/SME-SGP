/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class TUR_TurmaRelTurmaDisciplinaDAO : Abstract_TUR_TurmaRelTurmaDisciplinaDAO
	{
        /// <summary>
        /// Retorna as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns></returns>
        public DataTable SelectBy_tur_id
        (
            long tur_id
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaRelTurmaDisciplina_SelectBy_tur_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion
            
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
        /// Retorna as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns></returns>
        public DataTable SelectBy_Turmas
        (
            string tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaRelTurmaDisciplina_SelectBy_Turmas", _Banco);
            
            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tur_id";
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a turma vinculada a disciplina. 
        /// </summary>
        /// <param name="tud_id">código da disciplina</param>
        /// <returns></returns>
        public DataTable SelecionarTurmaPorTurmaDisciplina(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaRelTurmaDisciplina_Select_Turma", _Banco);

            #region parametros
            
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            return qs.Return;
        }
	}
}