/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data;
    using System.Data.SqlClient;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class DCL_LogDAO : AbstractDCL_LogDAO
    {

        /// <summary>
        /// Salva os os logs vindo do diário de classe
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable dos logs.</param>
        /// <returns></returns>
        public bool SalvarLogs(DataTable dtLogs)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("DCL_Log_SalvaLogs", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbLogs";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_DiarioDeClasseLogs";
                sqlParam.Value = dtLogs;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        #region OVERRIDE

        /// <summary>
        /// sobescreve o metodo do abstract.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_Log entity)
        {
            return true;
        }
        
        #endregion

    }
}