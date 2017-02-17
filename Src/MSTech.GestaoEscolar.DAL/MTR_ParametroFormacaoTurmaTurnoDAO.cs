/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// Classe MTR_ParametroFormacaoTurmaTurnoDAO
	/// </summary>
	public class MTR_ParametroFormacaoTurmaTurnoDAO : Abstract_MTR_ParametroFormacaoTurmaTurnoDAO
	{
        #region Métodos

        /// <summary>
        /// Seleciona todos os cursos e grupamentos de ensino de
        /// acordo com os registro da tabela MTR_ParametroFormacaoTurma.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do parâmetro período.</param>
        /// <param name="mostrarportipoturno"></param>
        /// <returns>Lista de estruturas de parâmetro de formação de turmas.</returns>
        public DataTable SelectBy_ProcessoParametro(int pfi_id, int pft_id, bool mostrarportipoturno)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ParametroFormacaoTurmaTurno_SelectBy_ProcessoParametro", _Banco);
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostrarportipoturno";
                Param.Size = 1;
                Param.Value = mostrarportipoturno;
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