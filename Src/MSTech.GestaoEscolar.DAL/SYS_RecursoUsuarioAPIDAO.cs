/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;

    /// <summary>
    /// Description: .
    /// </summary>
    public class SYS_RecursoUsuarioAPIDAO : Abstract_SYS_RecursoUsuarioAPIDAO
	{
        #region Métodos sobrescritos

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, SYS_RecursoUsuarioAPI entity)
        {
            if (qs != null && entity != null)
            {
                return true;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}