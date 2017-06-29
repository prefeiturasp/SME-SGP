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
    using System;

    /// <summary>
    /// Description: CFG_Alerta Business Object. 
    /// </summary>
    public class CFG_AlertaBO : BusinessBase<CFG_AlertaDAO, CFG_Alerta>
    {
        /// <summary>
        /// Retorna os alertas não excluídos.
        /// </summary>      
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarAlertas()
        {
            totalRecords = 0;
            return new CFG_AlertaDAO().SelecionarAlertas(out totalRecords);
        }

        public static bool Salvar(CFG_Alerta alerta, List<CFG_AlertaGrupo> lstGrupos)
        {
            CFG_AlertaDAO dao = new CFG_AlertaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                bool retorno = true;

                retorno &= Save(alerta, dao._Banco);
                retorno &= new CFG_AlertaGrupoDAO { _Banco = dao._Banco }.ExcluirPorAlerta(alerta.cfa_id);
                lstGrupos.ForEach
                (
                    p =>
                    {
                        retorno &= CFG_AlertaGrupoBO.Save(p, dao._Banco);
                    }
                );

                return retorno;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }
    }
}