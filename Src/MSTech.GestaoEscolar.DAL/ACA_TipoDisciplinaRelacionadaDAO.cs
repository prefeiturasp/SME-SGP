/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_TipoDisciplinaRelacionadaDAO : Abstract_ACA_TipoDisciplinaRelacionadaDAO
	{
        /// <summary>
        /// Remove os tipos de disciplina relacionados com um tipo de disciplina.
        /// </summary>
        /// <param name="tds_id"></param>
		public void DeleteByTdsId(int tds_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_TipoDisciplinaRelacionada_DeleteByTdsId", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.DbType = DbType.Int32;
                sqlParam.ParameterName = "@tds_id";
                sqlParam.Size = 8;
                sqlParam.Value = tds_id;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}