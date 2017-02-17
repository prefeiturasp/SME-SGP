using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Data.Common;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// DCL_ProtocoloReprocesso Business Object 
    /// </summary>
    public class DCL_ProtocoloReprocessoBO : BusinessBase<DCL_ProtocoloReprocessoDAO, DCL_ProtocoloReprocesso>
    {
        #region DML

        public new static bool Save(DCL_ProtocoloReprocesso entity)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.prp_dataCriacao = DateTime.Now;
                }

                entity.prp_dataAlteracao = DateTime.Now;

                DCL_ProtocoloReprocessoDAO dao = new DCL_ProtocoloReprocessoDAO();
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        public new static bool Save(DCL_ProtocoloReprocesso entity, TalkDBTransaction banco)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.prp_dataCriacao = DateTime.Now;
                }

                entity.prp_dataAlteracao = DateTime.Now;

                DCL_ProtocoloReprocessoDAO dao = new DCL_ProtocoloReprocessoDAO() { _Banco = banco };
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        public static DataTable GetSelectBy_Protocolo(Guid pro_id)
        {
            try
            {
                DCL_ProtocoloReprocessoDAO dao = new DCL_ProtocoloReprocessoDAO();
                return dao.SelectBy_Protocolo(pro_id);
            }
            catch
            {
                throw;
            }
        }
    }
}
