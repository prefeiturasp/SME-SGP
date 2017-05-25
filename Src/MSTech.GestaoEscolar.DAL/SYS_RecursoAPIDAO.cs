/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;    /// <summary>
                          /// Description: .
                          /// </summary>
    public class SYS_RecursoAPIDAO : Abstract_SYS_RecursoAPIDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_RecursoAPI entity)
        {
            entity.rap_dataCriacao = entity.rap_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, SYS_RecursoAPI entity)
        {
            entity.rap_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@rap_dataCriacao");
        }

        protected override bool Alterar(SYS_RecursoAPI entity)
        {
            __STP_UPDATE = "NEW_SYS_RecursoAPI_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, SYS_RecursoAPI entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rap_id";
                Param.Size = 4;
                Param.Value = entity.rap_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@rap_situacao";
                Param.Size = 1;
                Param.Value = 3;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@rap_dataAlteracao";
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        public override bool Delete(SYS_RecursoAPI entity)
        {
            __STP_DELETE = "NEW_SYS_RecursoAPI_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, SYS_RecursoAPI entity)
        {
            if (qs != null && entity != null)
            {
                return true;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}