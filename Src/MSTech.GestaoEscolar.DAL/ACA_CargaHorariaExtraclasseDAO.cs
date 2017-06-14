/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CargaHorariaExtraclasseDAO : Abstract_ACA_CargaHorariaExtraclasseDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            entity.che_dataCriacao = entity.che_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            entity.che_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@che_dataCriacao");
        }

        protected override bool Alterar(ACA_CargaHorariaExtraclasse entity)
        {
            __STP_UPDATE = "NEW_ACA_CargaHorariaExtraclasse_UPDATE";
            return base.Alterar(entity); 
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@che_situacao";
            Param.Size = 1;
            Param.Value = entity.che_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@che_dataAlteracao";
            Param.Value = entity.che_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ACA_CargaHorariaExtraclasse entity)
        {
            __STP_DELETE = "NEW_ACA_CargaHorariaExtraclasse_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            if (entity != null & qs != null && qs.Return.Rows.Count > 0)
            {
                entity.che_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.che_id > 0;
            }

            return false;
        }

        #endregion
    }
}