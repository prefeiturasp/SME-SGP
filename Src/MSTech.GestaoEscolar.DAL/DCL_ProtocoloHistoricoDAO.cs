/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class DCL_ProtocoloHistoricoDAO : Abstract_DCL_ProtocoloHistoricoDAO
    {
        #region Sobrescritos

        protected override void ParamInserir(Data.Common.QuerySelectStoredProcedure qs, Entities.DCL_ProtocoloHistorico entity)
        {
            entity.prh_dataCriacao = DateTime.Now;
            entity.prh_dataAlteracao = DateTime.Now;
            
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(Data.Common.QueryStoredProcedure qs, Entities.DCL_ProtocoloHistorico entity)
        {
            entity.prh_dataAlteracao = DateTime.Now;

            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@prh_dataCriacao");
        }

        protected override bool Alterar(Entities.DCL_ProtocoloHistorico entity)
        {
            __STP_UPDATE = "NEW_DCL_ProtocoloHistorico_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_ProtocoloHistorico entity)
        {
            if (entity != null & qs != null)
            {
                entity.prh_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.prh_id > 0;
            }

            return false;
        }

        #endregion
    }
}