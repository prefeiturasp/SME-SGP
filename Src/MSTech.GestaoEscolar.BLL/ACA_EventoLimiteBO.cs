namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	using System.Collections.Generic;
using MSTech.Data.Common;
	
	/// <summary>
	/// Description: ACA_EventoLimite Business Object. 
	/// </summary>
	public class ACA_EventoLimiteBO : BusinessBase<ACA_EventoLimiteDAO, ACA_EventoLimite>
	{
		public static List<ACA_EventoLimite> GetSelectByCalendario(int cal_id, TalkDBTransaction banco = null)
		{
			var dao = new ACA_EventoLimiteDAO();

			if (banco != null)
				dao._Banco = banco;

			return dao.SelectByCalendario(cal_id);
		}

        public static List<ACA_EventoLimite> GetSelectByTipoEvento(int tev_id, TalkDBTransaction banco = null)
        {
            var dao = new ACA_EventoLimiteDAO();

            if (banco != null)
                dao._Banco = banco;

            return dao.SelectByTipoEvento(tev_id);
        }
    }
}