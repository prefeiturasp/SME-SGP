/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Description: REL_GraficoAtendimento Business Object. 
    /// </summary>
    public class REL_GraficoAtendimentoBO : BusinessBase<REL_GraficoAtendimentoDAO, REL_GraficoAtendimento>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="gra_titulo"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable PesquisaGraficoPorRelatorio(int rea_id, string gra_titulo, int currentPage, int pageSize)
        {
            return new REL_GraficoAtendimentoDAO().SelecionaGraficoPorRelatorio(true, currentPage / pageSize, pageSize,rea_id, gra_titulo, out totalRecords);
        }
    }
}