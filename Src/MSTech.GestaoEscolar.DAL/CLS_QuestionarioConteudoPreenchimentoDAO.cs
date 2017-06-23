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
    public class CLS_QuestionarioConteudoPreenchimentoDAO : Abstract_CLS_QuestionarioConteudoPreenchimentoDAO
	{
        /// <summary>
        /// Retorna se o conteúdo foi preenchido.
        /// </summary>
        /// <param name="qtc_id">Id do conteúdo.</param>
        /// <returns></returns>
        public DataTable SelecionaConteudoPreenchido
                (
                   string qtc_ids
                )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioConteudoPreenchimento_SelecionaConteudoPreenchido", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@qtc_ids";
                Param.Value = qtc_ids;
                qs.Parameters.Add(Param);
                
                #endregion

                qs.Execute();

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

        /// <summary>
        /// Exclui os conteudos respondidos por reap_id
        /// </summary>
        /// <param name="reap_id"></param>
        /// <returns></returns>
        public bool ExcluirPorReapId(long reap_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_QuestionarioConteudoPreenchimento_ExcluiPorReapId", _Banco);

            try
            {
                Param = qs.NewParameter();
                Param.ParameterName = "@reap_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = reap_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}