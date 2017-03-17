/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Description: ACA_ObjetoAprendizagemTipoCiclo Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemTipoCicloBO : BusinessBase<ACA_ObjetoAprendizagemTipoCicloDAO, ACA_ObjetoAprendizagemTipoCiclo>
	{
        public static DataTable SelectBy_TipoDisciplina(int tds_id)
        {
            totalRecords = 0;

            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, out totalRecords);
        }

        public static List<int> SelectBy_ObjetoAprendizagem(int oap_id)
        {
            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            return dao.SelectBy_ObjetoAprendizagem(oap_id);
        }
    }
}