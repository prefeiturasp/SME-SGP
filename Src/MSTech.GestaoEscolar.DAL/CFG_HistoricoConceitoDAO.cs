/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
	using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;
    using MSTech.Data.Common;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CFG_HistoricoConceitoDAO : AbstractCFG_HistoricoConceitoDAO
	{
        #region Consultas

        /// <summary>
        /// Retorna um datatable contendo todas os conceitos do histórico
        /// </summary>    
        /// <returns>DataTable com os conceitos do Historico </returns>
        public DataTable SelecionaConceitosHistorico()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_HistoricoConceitoSelecionaConceitos", _Banco);

            try
            {
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
        #endregion
	}
}