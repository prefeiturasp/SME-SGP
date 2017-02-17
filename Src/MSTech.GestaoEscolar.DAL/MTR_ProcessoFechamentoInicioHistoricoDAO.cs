/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;

    /// <summary>
    /// Description: .
    /// </summary>
    public class MTR_ProcessoFechamentoInicioHistoricoDAO : AbstractMTR_ProcessoFechamentoInicioHistoricoDAO
    {
        /// <summary>
        /// Retorna todos os históricos filtrados por processo de fechamento/início de ano letivo e por escola
        /// </summary>
        /// <param name="pfi_id">ID do processo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <returns></returns>
        public DataTable SelecionaHistoricoFechamentoAnoLetivo
        (
            int pfi_id
            , int esc_id
            , int uni_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ProcessoFechamentoInicioHistorico_SelectBy_ProcessoEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}