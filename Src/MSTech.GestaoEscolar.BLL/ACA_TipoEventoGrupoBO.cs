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
	/// Description: ACA_TipoEventoGrupo Business Object. 
	/// </summary>
	public class ACA_TipoEventoGrupoBO : BusinessBase<ACA_TipoEventoGrupoDAO, ACA_TipoEventoGrupo>
	{
	    public static void DeleteByTipoEvento(int tev_id)
        {
            ACA_TipoEventoGrupoDAO dao = new ACA_TipoEventoGrupoDAO();
            dao.DeleteByTipoEvento(tev_id);
        }

        public static List<ACA_TipoEventoGrupo> SelectByTipoEvento(int tev_id)
        {
            ACA_TipoEventoGrupoDAO dao = new ACA_TipoEventoGrupoDAO();
            return dao.SelectByTipoEvento(tev_id);
        }
	}
}