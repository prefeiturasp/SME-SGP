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
	public class LOG_MatriculaTurmaDisciplinaExcluidaDAO : AbstractLOG_MatriculaTurmaDisciplinaExcluidaDAO
    {
        #region Métodos Sobrescritos
        
        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, LOG_MatriculaTurmaDisciplinaExcluida entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@lme_data"].Value = DateTime.Now;
        }

        #endregion
	}
}