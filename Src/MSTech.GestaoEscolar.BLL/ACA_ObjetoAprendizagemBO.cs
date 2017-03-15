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
    /// Description: ACA_ObjetoAprendizagem Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemBO : BusinessBase<ACA_ObjetoAprendizagemDAO, ACA_ObjetoAprendizagem>
	{
        public static DataTable SelectBy_TipoDisciplina(int tds_id)
        {
            totalRecords = 0;

            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, out totalRecords);
        }
    }
}