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
    public class ACA_ObjetoAprendizagemEixoDAO : Abstract_ACA_ObjetoAprendizagemEixoDAO
	{

        #region Métodos sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@oae_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@oae_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@oae_dataCriacao");
            qs.Parameters["@oae_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_ObjetoAprendizagemEixo</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_ObjetoAprendizagemEixo entity)
        {
            __STP_UPDATE = "NEW_ACA_ObjetoAprendizagemEixo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@oae_id";
            Param.Size = 4;
            Param.Value = entity.oae_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@oae_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@oae_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_ObjetoAprendizagemEixo</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_ObjetoAprendizagemEixo entity)
        {
            __STP_DELETE = "NEW_ACA_ObjetoAprendizagemEixo_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}