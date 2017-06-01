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
    public class CLS_TurmaAtividadeExtraClasseDAO : Abstract_CLS_TurmaAtividadeExtraClasseDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            entity.tae_dataCriacao = entity.tae_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            entity.tae_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tae_dataCriacao");
        }

        protected override bool Alterar(CLS_TurmaAtividadeExtraClasse entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAtividadeExtraClasse_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tae_id";
                Param.Size = 4;
                Param.Value = entity.tae_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tae_situacao";
                Param.Size = 3;
                Param.Value = entity.tae_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tae_dataAlteracao";
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        public override bool Delete(CLS_TurmaAtividadeExtraClasse entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaAtividadeExtraClasse_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            if (entity != null & qs != null)
            {
                entity.tae_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.tae_id > 0;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}