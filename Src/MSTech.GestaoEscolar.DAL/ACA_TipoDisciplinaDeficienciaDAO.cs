/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_TipoDisciplinaDeficienciaDAO : AbstractACA_TipoDisciplinaDeficienciaDAO
	{

        /// <summary>
        /// Exclui fisicamento os registros 
        /// </summary>
        /// <param name="tds_id">id do tipo de disciplina</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        public bool DeleteBy_TipoDisciplinaDeficiencia(int tds_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_TipoDisciplinaDeficiencia_DeleteBy_tds_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                Param.Size = 4;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return true;
                //return (qs.Return > 0);
            }
            catch
            {
                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelectBy_ACA_TipoDisciplina_ACA_TipoDisciplinaDeficiencia(int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDisciplinaDeficiencia_SelectBy_ACA_TipoDisciplina", this._Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                Param.Size = 4;
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
    }
}