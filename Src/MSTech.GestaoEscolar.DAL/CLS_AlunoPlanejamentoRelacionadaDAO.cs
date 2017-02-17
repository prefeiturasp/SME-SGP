/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoPlanejamentoRelacionadaDAO : Abstract_CLS_AlunoPlanejamentoRelacionadaDAO
    {
        /// <summary>
        /// Seleciona as turmadisciplinas relacionadas ao planejamento do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="apl_id">ID do planejamento do aluno</param>
        /// <returns></returns>
        public DataTable SelecionaPlanejamentoAlunoRelacionada(long alu_id, long tud_id, int apl_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamentoRelacionada_SelectBy_alu_tud_apl_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@apl_id";
                Param.Size = 4;
                if (apl_id > 0)
                    Param.Value = apl_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Remove todas turmadisciplinas relacionadas ao planejamento do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="apl_id">ID do planejamento do aluno</param>
        public void LimparRelacionadas(long alu_id, long tud_id, int apl_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoPlanejamentoRelacionada_LimparRelacionadas", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@apl_id";
                Param.Size = 4;
                if (apl_id > 0)
                    Param.Value = apl_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
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
        /// Remove todas turmadisciplinas relacionadas ao planejamento do aluno por listas
        /// </summary>
        /// <param name="alu_ids">ID do aluno</param>
        /// <param name="tud_ids">ID da turma disciplina</param>
        /// <param name="apl_ids">ID da disciplina</param>
        public void LimparRelacionadas(String alu_ids, String tud_ids, String apl_ids)  
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoPlanejamentoRelacionada_LimparRelacionadas_PorLista", _Banco);

            try
            {
                #region PARAMETROS
                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";
                Param.Value = alu_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tud_ids";
                Param.Value = tud_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@apl_ids";
                Param.Value = apl_ids;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();
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