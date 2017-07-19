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
    public class ACA_CurriculoSugestaoDAO : Abstract_ACA_CurriculoSugestaoDAO
	{
        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoSugestao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@crs_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@crs_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoSugestao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@crs_dataCriacao");
            qs.Parameters["@crs_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>s
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoSugestao</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ACA_CurriculoSugestao entity)
        {
            __STP_UPDATE = "NEW_ACA_CurriculoSugestao_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoSugestao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crs_id";
            Param.Size = 4;
            Param.Value = entity.crs_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crs_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@crs_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoCapitulo</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_CurriculoSugestao entity)
        {
            __STP_DELETE = "NEW_ACA_CurriculoSugestao_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}