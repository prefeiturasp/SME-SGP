/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;    /// <summary>
                          /// Description: .
                          /// </summary>
    public class ACA_CurriculoObjetivoDAO : Abstract_ACA_CurriculoObjetivoDAO
	{
        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@cro_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cro_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cro_dataCriacao");
            qs.Parameters["@cro_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoObjetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ACA_CurriculoObjetivo entity)
        {
            __STP_UPDATE = "NEW_ACA_CurriculoObjetivo_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cro_id";
            Param.Size = 4;
            Param.Value = entity.cro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cro_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cro_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoObjetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_CurriculoObjetivo entity)
        {
            __STP_DELETE = "NEW_ACA_CurriculoObjetivo_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}