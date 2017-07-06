/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System.Collections.Generic;
    using Data.Common;
    using System.Data;
    using System.Linq;
    using System;

    /// <summary>
    /// Description: .
    /// </summary>
    public class REL_GraficoAtendimento_FiltrosPersonalizadosDAO : Abstract_REL_GraficoAtendimento_FiltrosPersonalizadosDAO
	{
        public List<REL_GraficoAtendimento_FiltrosPersonalizados> SelectBy_gra_id(int gra_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_FiltrosPersonalizados_SelectBy_gra_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@gra_id";
                Param.Size = 4;
                Param.Value = gra_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (from DataRow dr in qs.Return.Rows select DataRowToEntity(dr, new REL_GraficoAtendimento_FiltrosPersonalizados())).ToList();
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