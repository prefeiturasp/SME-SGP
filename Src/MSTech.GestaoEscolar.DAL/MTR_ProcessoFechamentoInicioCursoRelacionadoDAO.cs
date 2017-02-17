/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/


namespace MSTech.GestaoEscolar.DAL
{
	using Abstracts;
    using Data.Common;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;    
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class MTR_ProcessoFechamentoInicioCursoRelacionadoDAO : Abstract_MTR_ProcessoFechamentoInicioCursoRelacionadoDAO
    {
        #region Métodos
        
        /// <summary>
        /// Seleciona os cursos equivalentes de um curso em um processo de fechamento/início.
        /// </summary>
        /// <param name="pfi_id">ID do processo de fechamento/início.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <returns></returns>
        public List<String> SelecionaCursosRelacionados(int pfi_id, int cur_id, int crr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ProcessoFechamentoInicioCursoRelacionado_SelecionaCursosRelacionados", _Banco);
            List<String> list = new List<string>();

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                list.AddRange(from DataRow dr in qs.Return.Rows select dr.ItemArray[0].ToString());

                return list;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cursos/períodos do curso relacionados
        /// com o curso e processo de fechamento/início informados
        /// </summary>
        /// <param name="pfi_id">ID do processo de fechamento/início</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <returns></returns>
        public DataTable SelecionaCursosRelacionadosPorProcesso(int pfi_id, int cur_id, int crr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ProcessoFechamentoInicioCursoRelacionado_SelecionaCursosRelacionadosPorProcesso", _Banco);            

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
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