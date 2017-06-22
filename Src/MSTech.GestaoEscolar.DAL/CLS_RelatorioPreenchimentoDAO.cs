/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioPreenchimentoDAO : Abstract_CLS_RelatorioPreenchimentoDAO
	{
        /// <summary>
        /// Seleciona dados de preenchimento de relaório do aluno
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
		public DataSet SelecionaPorRelatorioAlunoTurmaDisciplina(int rea_id, long alu_id, long tur_id, long tud_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioPreenchimento_SelecionaPorRelatorioAlunoTurmaDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rea_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@alu_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tur_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                if (tud_id > 0)
                {
                    Param.Value = tud_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tpc_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (tpc_id > 0)
                {
                    Param.Value = tpc_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion 

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            } 
        }

    }
}