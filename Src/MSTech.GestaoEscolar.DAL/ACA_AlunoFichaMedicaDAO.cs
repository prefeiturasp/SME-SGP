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
    public class ACA_AlunoFichaMedicaDAO : Abstract_ACA_AlunoFichaMedicaDAO
	{
        /// <summary>
        /// Verifica se já existe ficha médica para o aluno
        /// </summary>
        /// <param name="alu_id">ID da tabela ACA_Aluno</param>                
        /// <returns>true ou false</returns>
        public bool SelectBy_alu_id
        (
            long alu_id            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoFichaMedica_SelectBy_alu_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                if (alu_id > 0)
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
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