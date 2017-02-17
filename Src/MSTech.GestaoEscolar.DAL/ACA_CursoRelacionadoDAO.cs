/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_CursoRelacionadoDAO : Abstract_ACA_CursoRelacionadoDAO
	{
		/// <summary>
		/// Deleta todos os registros fisicamente da tabela que estejam relacionados com os parametros.
		/// </summary>
		/// <param name="cur_id">Id Curso</param>
		/// <param name="crr_id">Id Curriculo</param>
		/// <returns>Boleano</returns>
		public bool DeletarTodosRelacionado_Curso_Curriculo
		(
			int cur_id
			, int crr_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoRelacionadoNivelEnsino_DELETE", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return true;
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
		/// Seleciona todos os cursos que estão relacionados.
		/// </summary>
		/// <param name="cur_id">Id Curso</param>
		/// <param name="crr_id">Id Curriculo</param>
		/// <returns>List String</returns>
		public List<String> SelecionaCursosRelacionados
		(
			int cur_id
			, int crr_id
		)
		{
			List<String> list = new List<string>();
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoRelacionado_SelecionaCursosRelacionados", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();
				
				foreach (DataRow dr in qs.Return.Rows)
				{
					list.Add(dr.ItemArray[0].ToString());
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
		/// Seleciona todos os cursos que estão relacionados.
		/// </summary>
		/// <param name="cur_id">Id Curso</param>
		/// <param name="crr_id">Id Curriculo</param>
		/// <returns>List String</returns>
		public DataTable GetSelectBy_Curso
		(
			int cur_id
			, int crr_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoRelacionado_SelecionaCursosRelacionados", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
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