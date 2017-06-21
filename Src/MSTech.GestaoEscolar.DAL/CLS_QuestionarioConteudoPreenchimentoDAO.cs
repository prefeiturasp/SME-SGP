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
                   int qtc_id
                )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioConteudoPreenchimento_SelecionaConteudoPreenchido", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtc_id";
                if (qtc_id > 0)
                    Param.Value = qtc_id;
                else
                    Param.Value = DBNull.Value;
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
    }
}