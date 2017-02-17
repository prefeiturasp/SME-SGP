/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
	
	/// <summary>
	/// Description: ACA_TipoDesempenhoAprendizado Business Object. 
	/// </summary>
	public class ACA_TipoDesempenhoAprendizadoBO : BusinessBase<ACA_TipoDesempenhoAprendizadoDAO, ACA_TipoDesempenhoAprendizado>
	{
        /// <summary>
        /// Seleciona os tipos de desempenhos que nao estejam excluidos com base nos filtros
        /// </summary>
        /// <param name="cal_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="tds_id"></param>
        /// <returns></returns>
        public static DataTable SELECT_By_Pesquisa(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id)
        {
            ACA_TipoDesempenhoAprendizadoDAO dao = new ACA_TipoDesempenhoAprendizadoDAO();
            return dao.SELECT_By_Pesquisa(cal_id, cur_id, crr_id, crp_id, tds_id);
        }	
	}
}