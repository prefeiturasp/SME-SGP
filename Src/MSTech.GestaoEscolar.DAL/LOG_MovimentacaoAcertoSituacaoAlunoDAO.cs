/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL.Abstracts;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class LOG_MovimentacaoAcertoSituacaoAlunoDAO : AbstractLOG_MovimentacaoAcertoSituacaoAlunoDAO
	{
        #region Métodos Sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, LOG_MovimentacaoAcertoSituacaoAluno entity)
        {
            if (entity != null & qs != null)
            {
                entity.mta_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.mta_id > 0);
            }

            return false;
        }

        #endregion Métodos Sobrescritos
	}
}