/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Data;
using System;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// RHU_ColaboradorCargoDisciplina Business Object 
	/// </summary>
	public class RHU_ColaboradorCargoDisciplinaBO : BusinessBase<RHU_ColaboradorCargoDisciplinaDAO,RHU_ColaboradorCargoDisciplina>
	{
        /// <summary>
        /// Retorna todos os ColaboradorCargoDisciplina cadastrados para o colaborador.
        /// </summary>
        /// <param name="col_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Colaborador(Int64 col_id, TalkDBTransaction banco = null)
	    {
            RHU_ColaboradorCargoDisciplinaDAO dao = new RHU_ColaboradorCargoDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_Colaborador(col_id);
	    }
	}
}