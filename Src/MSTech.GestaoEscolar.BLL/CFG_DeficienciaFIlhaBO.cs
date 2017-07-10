/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System.Collections.Generic;
    using Data.Common;
    using System;
    using System.Linq;

    /// <summary>
    /// Description: CFG_DeficienciaFIlha Business Object. 
    /// </summary>
    public class CFG_DeficienciaFIlhaBO : BusinessBase<CFG_DeficienciaFIlhaDAO, CFG_DeficienciaFIlha>
	{

        public static bool Salvar(Guid tde_id, List<CFG_DeficienciaFIlha> lstFilha, TalkDBTransaction banco = null)
        {
            CFG_DeficienciaFIlhaDAO dao = new CFG_DeficienciaFIlhaDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                //Carrega os detalhes ligados a deficiencia
                List<CFG_DeficienciaFIlha> lstFilhaBanco = CFG_DeficienciaFIlhaBO.SelectFilhaBy_Deficiencia(tde_id, dao._Banco);

                //Salva defeiciencias filhas novas. A alteração não é feita, pois na tabela tem apenas as duas chaves então não tem o que alterar.
                foreach (CFG_DeficienciaFIlha def in lstFilha.Where(q => q.IsNew ==true))
                {
                      if (!CFG_DeficienciaFIlhaBO.Save(def, dao._Banco))
                        return false;
                }

                //Remove logicamente no banco detalhes que foram removidas da deficiencia
                foreach (CFG_DeficienciaFIlha defB in lstFilhaBanco)
                    if (!lstFilha.Any(q => q.tde_idFilha == defB.tde_idFilha))
                    {
                        CFG_DeficienciaFIlhaBO.Delete(defB, dao._Banco);
                    }

                return true;

            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        public static List<CFG_DeficienciaFIlha> SelectFilhaBy_Deficiencia(Guid tde_id, TalkDBTransaction banco = null)
        {
            CFG_DeficienciaFIlhaDAO dao = new CFG_DeficienciaFIlhaDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectFilhaBy_Deficiencia(tde_id);
        }

    }
}