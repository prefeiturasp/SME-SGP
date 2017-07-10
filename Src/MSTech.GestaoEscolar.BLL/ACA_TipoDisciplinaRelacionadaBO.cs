/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using Data.Common;
    using System.Data;
    using System;
    using Validation.Exceptions;

    /// <summary>
    /// Description: ACA_TipoDisciplinaRelacionada Business Object. 
    /// </summary>
    public class ACA_TipoDisciplinaRelacionadaBO : BusinessBase<ACA_TipoDisciplinaRelacionadaDAO, ACA_TipoDisciplinaRelacionada>
	{
		public static void Save(List<ACA_TipoDisciplinaRelacionada> lstRelacionadas)
        {
            if (lstRelacionadas.Count > 0)
            {
                ACA_TipoDisciplinaRelacionadaDAO dao = new ACA_TipoDisciplinaRelacionadaDAO();
                TalkDBTransaction banco = dao._Banco.CopyThisInstance();
                banco.Open(IsolationLevel.ReadCommitted);
                try
                {
                    dao.DeleteByTdsId(lstRelacionadas[0].tds_id);
                    lstRelacionadas.ForEach(p => { if (p.Validate()) { Save(p, banco); } else { throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(p)); } });
                }
                catch (Exception err)
                {
                    banco.Close(err);
                    throw;
                }
                finally
                {
                    banco.Close();
                }
            }
        }	
	}
}