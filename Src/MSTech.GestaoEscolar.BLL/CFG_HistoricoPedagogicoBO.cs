/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	
	/// <summary>
	/// Description: CFG_HistoricoPedagogico Business Object. 
	/// </summary>
	public class CFG_HistoricoPedagogicoBO : BusinessBase<CFG_HistoricoPedagogicoDAO, CFG_HistoricoPedagogico>
    {
        /// <summary>
        /// Seleciona a configuração cadastrada do último ano
        /// </summary>
        /// <returns></returns>
        public static CFG_HistoricoPedagogico SelecionaUltimoAno()
        {
            CFG_HistoricoPedagogicoDAO dao = new CFG_HistoricoPedagogicoDAO();
            return dao.SelecionaUltimoAno();
        }

        /// <summary>
        /// Seleciona a configuração cadastrada do ano informado ou a última cadastrada
        /// </summary>
        /// <param name="chp_anoLetivo">Ano letivo</param>
        /// <returns></returns>
        public static CFG_HistoricoPedagogico SelecionaByAno(int chp_anoLetivo)
        {
            CFG_HistoricoPedagogicoDAO dao = new CFG_HistoricoPedagogicoDAO();
            return dao.SelecionaByAno(chp_anoLetivo);
        }
    }
}