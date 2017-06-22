/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_QuestionarioRespostaPreenchimentoDAO : Abstract_CLS_QuestionarioRespostaPreenchimentoDAO
	{
        /// <summary>
        /// Exclui os conteudos respondidos por reap_id
        /// </summary>
        /// <param name="reap_id"></param>
        /// <returns></returns>
        public bool ExcluirPorReapId(long reap_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_QuestionarioRespostaPreenchimento_ExcluiPorReapId", _Banco);

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