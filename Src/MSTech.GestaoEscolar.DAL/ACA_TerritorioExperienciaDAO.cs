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
    public class ACA_TerritorioExperienciaDAO : Abstract_ACA_TerritorioExperienciaDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TerritorioExperiencia entity)
        {
            entity.ter_dataCriacao = entity.ter_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TerritorioExperiencia entity)
        {
            entity.ter_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@ter_dataCriacao");
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TerritorioExperiencia entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ter_situacao";
            Param.Size = 1;
            Param.Value = entity.ter_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ter_dataAlteracao";
            Param.Value = entity.ter_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        protected override bool Alterar(ACA_TerritorioExperiencia entity)
        {
            __STP_UPDATE = "NEW_ACA_TerritorioExperiencia_UPDATE";
            return base.Alterar(entity);
        }

        public override bool Delete(ACA_TerritorioExperiencia entity)
        {
            __STP_DELETE = "NEW_ACA_TerritorioExperiencia_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos sobrescritos
    }
}