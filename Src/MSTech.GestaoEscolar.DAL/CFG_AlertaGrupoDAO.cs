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
        public DataTable SelecionarGruposPorAlerta(short cfa_id, int sis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_AlertaGrupo_SelecionarGruposPorAlerta", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@cfa_id";
                Param.Size = 2;
                Param.Value = cfa_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@sis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = sis_id;
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
        public bool ExcluirPorAlerta(short cfa_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CFG_AlertaGrupo_ExcluirPorAlerta", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@cfa_id";
                Param.Size = 2;
                Param.Value = cfa_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return true;
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