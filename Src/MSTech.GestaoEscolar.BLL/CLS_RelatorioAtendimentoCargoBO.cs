/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Data;
    using Data.Common;

    /// <summary>
    /// Description: CLS_RelatorioAtendimentoCargo Business Object. 
    /// </summary>
    public class CLS_RelatorioAtendimentoCargoBO : BusinessBase<CLS_RelatorioAtendimentoCargoDAO, CLS_RelatorioAtendimentoCargo>
    {
        /// <summary>
        /// Carrega os cargos para o relatório de atendimento
        /// </summary>
        /// <param name="rea_id">ID do relatorio de atendimento</param>
        /// <returns></returns>
        public static DataTable SelectBy_rea_id(int rea_id)
        {
            CLS_RelatorioAtendimentoCargoDAO dao = new CLS_RelatorioAtendimentoCargoDAO();
            return dao.SelectBy_rea_id(rea_id);
        }

        /// <summary>
        /// Exclui os cargos para o relatório de atendimento
        /// </summary>
        /// <param name="rea_id">ID do relatorio de atendimento</param>
        /// <returns></returns>
        public static void DeleteBy_rea_id(int rea_id, TalkDBTransaction banco)
        {
            CLS_RelatorioAtendimentoCargoDAO dao = new CLS_RelatorioAtendimentoCargoDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeleteBy_rea_id(rea_id);
        }
    }
}