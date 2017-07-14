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
    public class LOG_AlertaAlunosFaltasConsecutivasDAO : Abstract_LOG_AlertaAlunosFaltasConsecutivasDAO
	{
        /// <summary>
        /// Salva o log de envio da notificação em lote.
        /// </summary>
        /// <returns></returns>
        public void SalvarEmLote(DataTable dtLog)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_LOG_AlertaAlunosFaltasConsecutivas_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbLog";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_LOG_AlertaAlunosFaltasConsecutivas";
                sqlParam.Value = dtLog;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetros

                qs.Execute();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}