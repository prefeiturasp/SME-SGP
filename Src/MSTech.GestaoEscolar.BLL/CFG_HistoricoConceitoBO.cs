/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: CFG_HistoricoConceito Business Object. 
	/// </summary>
	public class CFG_HistoricoConceitoBO : BusinessBase<CFG_HistoricoConceitoDAO, CFG_HistoricoConceito>
	{
        /// <summary>
        /// Retorna uma lista com os conceitos do histórico
        /// Apenas para escolas dentro da rede
        /// </summary>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<CFG_HistoricoConceito> RetornaListaConceitosHistorico
        (
            TalkDBTransaction banco
        )
        {
            CFG_HistoricoConceitoDAO dao = new CFG_HistoricoConceitoDAO();

            if (banco != null)
            {
                dao._Banco = banco;
            }

            List<CFG_HistoricoConceito> lista = new List<CFG_HistoricoConceito>();

            DataTable dt = dao.SelecionaConceitosHistorico();

            foreach (DataRow dr in dt.Rows)
            {
                CFG_HistoricoConceito ent = new CFG_HistoricoConceito();
                lista.Add(dao.DataRowToEntity(dr, ent));
            }

            return lista;
        }
	}
}