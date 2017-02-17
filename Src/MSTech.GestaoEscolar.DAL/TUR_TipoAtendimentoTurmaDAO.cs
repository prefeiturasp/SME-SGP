/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class TUR_TipoAtendimentoTurmaDAO : Abstract_TUR_TipoAtendimentoTurmaDAO
	{
        #region Métodos Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TipoAtendimentoTurma entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tat_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tat_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TipoAtendimentoTurma entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tat_dataCriacao");
            qs.Parameters["@tat_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Alterar(TUR_TipoAtendimentoTurma entity)
        {
            __STP_UPDATE = "NEW_TUR_TipoAtendimentoTurma_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TipoAtendimentoTurma entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tat_id";
            Param.Size = 4;
            Param.Value = entity.tat_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tat_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tat_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(TUR_TipoAtendimentoTurma entity)
        {
            __STP_DELETE = "NEW_TUR_TipoAtendimentoTurma_UPDATEBy_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}