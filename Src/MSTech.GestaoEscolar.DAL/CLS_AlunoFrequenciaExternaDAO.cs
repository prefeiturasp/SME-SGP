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
    public class CLS_AlunoFrequenciaExternaDAO : Abstract_CLS_AlunoFrequenciaExternaDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoFrequenciaExterna entity)
        {
            entity.afx_dataCriacao = entity.afx_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoFrequenciaExterna entity)
        {
            entity.afx_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@afx_dataCriacao");
        }

        protected override bool Alterar(CLS_AlunoFrequenciaExterna entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoFrequenciaExterna_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoFrequenciaExterna entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@afx_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@afx_dataAlteracao";

            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CLS_AlunoFrequenciaExterna entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoFrequenciaExterna_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoFrequenciaExterna entity)
        {
            if (entity != null & qs != null)
            {
                entity.afx_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.afx_id > 0;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}