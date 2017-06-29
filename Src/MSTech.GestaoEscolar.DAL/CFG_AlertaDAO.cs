/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CFG_AlertaDAO : Abstract_CFG_AlertaDAO
	{
        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_Alerta entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@cfa_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cfa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_Alerta entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cfa_dataCriacao");
            qs.Parameters["@cfa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>s
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade CFG_Alerta</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CFG_Alerta entity)
        {
            __STP_UPDATE = "NEW_CFG_Alerta_Update";
            return base.Alterar(entity);
        }

        #endregion Métodos Sobrescritos
    }
}