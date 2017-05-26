/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System.Data;
    /// <summary>
    /// Description: .
    /// </summary>
    public class SYS_RecursoUsuarioAPIDAO : Abstract_SYS_RecursoUsuarioAPIDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona usuários para autenticação em apis
        /// </summary>
        /// <param name="rap_id"></param>
        /// <returns></returns>
        public DataTable SelecionaUsuarioPorRecurso(int rap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_RecursoUsuarioAPI_SelecionaUsuarioPorRecurso", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@rap_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rap_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

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