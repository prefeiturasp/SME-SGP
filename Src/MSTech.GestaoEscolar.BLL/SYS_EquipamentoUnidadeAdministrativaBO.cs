using System;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// SYS_EquipamentoUnidadeAdministrativa Business Object 
    /// </summary>
    public class SYS_EquipamentoUnidadeAdministrativaBO : BusinessBase<SYS_EquipamentoUnidadeAdministrativaDAO, SYS_EquipamentoUnidadeAdministrativa>
    {
        #region DML

        public new static bool Save(SYS_EquipamentoUnidadeAdministrativa entity)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.eua_dataCriacao = DateTime.Now;
                }

                entity.eua_dataAlteracao = DateTime.Now;

                SYS_EquipamentoUnidadeAdministrativaDAO dao = new SYS_EquipamentoUnidadeAdministrativaDAO();
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        public new static bool Save(SYS_EquipamentoUnidadeAdministrativa entity, TalkDBTransaction banco)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.eua_dataCriacao = DateTime.Now;
                }

                entity.eua_dataAlteracao = DateTime.Now;

                SYS_EquipamentoUnidadeAdministrativaDAO dao = new SYS_EquipamentoUnidadeAdministrativaDAO { _Banco = banco };
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
