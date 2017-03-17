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
    using Validation.Exceptions;
    using CoreSSO.BLL;
    using System.Linq;

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

        public static void Save(ACA_ObjetoAprendizagem entity, IEnumerable<int> listTci_ids)
        {
            if (entity.Validate())
            {
                var dao = new ACA_ObjetoAprendizagemDAO();
                dao.Salvar(entity);

                var list = listTci_ids.Select(x => new ACA_ObjetoAprendizagemTipoCiclo
                {
                    oap_id = entity.oap_id,
                    tci_id = x
                }).ToList();

                var daoTipoCiclo = new ACA_ObjetoAprendizagemTipoCicloDAO();
                daoTipoCiclo.DeleteNew(entity.oap_id);

                foreach (var item in list)
                {
                    daoTipoCiclo.Salvar(item);
                }
            }
            else
                throw new ValidationException(UtilBO.ErrosValidacao(entity));
        }
    }
}