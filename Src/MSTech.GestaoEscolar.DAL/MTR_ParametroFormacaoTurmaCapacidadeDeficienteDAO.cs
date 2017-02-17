/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO : Abstract_MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna um dataTable contendo os parâmetros de capacidade por deficiente filtrado por pfi_id e pft_id.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do processo de formação de turmas.</param>
        /// <returns>DataTable contendo os parâmetro de capacidade por deficiente.</returns>
        public DataTable SelectBy_ProcessoParametro(int pfi_id, int pft_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ParametroFormacaoTurmaCapacidadeDeficiente_SelectBy_ProcessoParametro", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                if (pfi_id > 0)
                    Param.Value = pfi_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pft_id";
                Param.Size = 4;
                if (pfi_id > 0)
                    Param.Value = pft_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion
    }
}