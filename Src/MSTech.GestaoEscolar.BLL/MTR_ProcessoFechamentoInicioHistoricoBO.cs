/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Data;
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Description: MTR_ProcessoFechamentoInicioHistorico Business Object.
    /// </summary>
    public class MTR_ProcessoFechamentoInicioHistoricoBO : BusinessBase<MTR_ProcessoFechamentoInicioHistoricoDAO, MTR_ProcessoFechamentoInicioHistorico>
    {
        #region Métodos de seleção

        /// <summary>
        /// Seleciona todos os históricos do fechamento do ano letivo
        /// </summary>
        /// <returns>DataTable de parâmetros</returns>
        public static DataTable SelecionaHistoricoFechamentoAnoLetivo(int pfi_id, int esc_id, int uni_id)
        {
            MTR_ProcessoFechamentoInicioHistoricoDAO dao = new MTR_ProcessoFechamentoInicioHistoricoDAO();
            return dao.SelecionaHistoricoFechamentoAnoLetivo(pfi_id, esc_id, uni_id);
        }

        #endregion Métodos de seleção
    }
}