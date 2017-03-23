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
    using Data.Common;
    /// <summary>
    /// Description: ACA_ObjetoAprendizagemTipoCiclo Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemTipoCicloBO : BusinessBase<ACA_ObjetoAprendizagemTipoCicloDAO, ACA_ObjetoAprendizagemTipoCiclo>
	{
        public static DataTable SelectBy_TipoDisciplina(int tds_id, int cal_ano)
        {
            totalRecords = 0;

            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, cal_ano, out totalRecords);
        }

        public static Dictionary<int, bool> SelectBy_ObjetoAprendizagem(int oap_id)
        {
            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            return dao.SelectBy_ObjetoAprendizagem(oap_id);
        }

        public static void DeleteNew(int oap_id, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeleteNew(oap_id);
        }

        /// <summary>
        /// Verifica se os ciclos do objeto de aprendizagem estão em uso
        /// </summary>
        /// <param name="oap_id">ID do objeto de aprendizagem</param>
        public static Dictionary<int, string> CiclosEmUso(int oap_id, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemTipoCicloDAO dao = new ACA_ObjetoAprendizagemTipoCicloDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.CiclosEmUso(oap_id);
        }
    }
}