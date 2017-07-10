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
    /// Description: CLS_RelatorioAtendimentoGrupo Business Object. 
    /// </summary>
    public class CLS_RelatorioAtendimentoGrupoBO : BusinessBase<CLS_RelatorioAtendimentoGrupoDAO, CLS_RelatorioAtendimentoGrupo>
    {
        /// <summary>
        /// Carrega os grupos para o relatório de atendimento
        /// </summary>
        /// <param name="rea_id">ID do relatorio de atendimento</param>
        /// <param name="sis_id">ID do sistema</param>
        /// <returns></returns>
        public static DataTable SelectBy_rea_id(int rea_id, int sis_id)
        {
            CLS_RelatorioAtendimentoGrupoDAO dao = new CLS_RelatorioAtendimentoGrupoDAO();
            return dao.SelectBy_rea_id(rea_id, sis_id);
        }
        /// <summary>
        /// Exclui os grupos para o relatório de atendimento
        /// </summary>
        /// <param name="rea_id">ID do relatorio de atendimento</param>
        /// <returns></returns>
        public static void DeleteBy_rea_id(int rea_id, TalkDBTransaction banco)
        {
            CLS_RelatorioAtendimentoGrupoDAO dao = new CLS_RelatorioAtendimentoGrupoDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeleteBy_rea_id(rea_id);
        }
    }
}