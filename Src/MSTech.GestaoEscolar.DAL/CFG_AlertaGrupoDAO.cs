/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CFG_AlertaGrupoDAO : Abstract_CFG_AlertaGrupoDAO
	{
        /// <summary>
        /// Carrega os grupos para o alerta
        /// </summary>
        /// <param name="cfa_id">ID do alerta</param>
        /// <returns></returns>
        public DataTable SelecionarGruposPorAlerta(int cfa_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_AlertaGrupo_SelecionarGruposPorAlerta", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.ParameterName = "@cfa_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = cfa_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Remove os grupos ligados ao alerta.
        /// </summary>
        /// <returns></returns>
        public bool ExcluirPorAlerta(int cfa_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CFG_AlertaGrupo_ExcluirPorAlerta", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.ParameterName = "@cfa_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = cfa_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}