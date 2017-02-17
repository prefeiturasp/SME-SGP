/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Data;
    using System.Collections.Generic;
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_TurmaAulaRecursoRegenciaDAO : AbstractCLS_TurmaAulaRecursoRegenciaDAO
    {
        #region Metodos

        /// <summary>
        /// Excluir recursos.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <param name="rsa_id"></param>
        /// <returns></returns>
        public bool DeleteBy_rsa_id(long tud_id, int tau_id, int rsa_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaRecursoRegencia_DELETE", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rsa_id";
                Param.Size = 4;
                if (rsa_id > 0)
                    Param.Value = rsa_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return true;

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

        /// <summary>
        /// Atualiza recursos.
        /// </summary>
        /// <param name="entityAltera"></param>
        /// <returns></returns>
        public bool UpdateBy_rsa_id(CLS_TurmaAulaRecursoRegencia entity)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaRecursoRegencia_UPDATE", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = entity.tud_idFilho;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rsa_id";
                Param.Size = 4;
                if (entity.rsa_id > 0)
                    Param.Value = entity.rsa_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@trr_observacao";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(entity.trr_observacao))
                    Param.Value = entity.trr_observacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@trr_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.trr_dataAlteracao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return true;

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

        /// <summary>
        /// Seleciona recurso da aula componente da regência, por disciplina e aula.
        /// </summary>
        /// <param name="tud_id">Id da disciplina</param>
        /// <param name="tau_id">Id da aula</param>
        /// <returns>List<CLS_TurmaAulaRecursoRegencia></returns>
        public List<CLS_TurmaAulaRecursoRegencia> SelectBy_tud_id_tau_id
        (
            long tud_id
            , int tau_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaRecursoRegencia_By_tud_id_tau_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                List<CLS_TurmaAulaRecursoRegencia> listaRecurso = new List<CLS_TurmaAulaRecursoRegencia>();

                listaRecurso = (from DataRow dr in qs.Return.Rows
                                select (CLS_TurmaAulaRecursoRegencia)DataRowToEntity(dr, new CLS_TurmaAulaRecursoRegencia())).ToList();

                return listaRecurso;
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
        /// Deleta os recursos por aula regencia
        /// </summary>
        /// <param name="tud_id">Disciplina pai</param>
        /// <param name="tau_id">Aula</param>
        /// <param name="tud_idFilho">Disciplina filho</param>
        /// <returns>bool</returns>
        public bool DeletePorAulaRegencia(long tud_id, int tau_id, long tud_idFilho)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaRecursoRegencia_DELETE_Por_AulaRegencia", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@tau_id";
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tud_idFilho";
                Param.Value = tud_idFilho;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return true;
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

        /// <summary>
        /// Seleciona os recursos por componente da regência.
        /// </summary>
        /// <param name="tud_id">Id da disciplina regente</param>
        /// <param name="tau_id">Id da aula</param>
        /// <param name="tud_idFilho">Id da disciplina componente</param>
        /// <returns>List<CLS_TurmaAulaRecursoRegencia></returns>
        public List<CLS_TurmaAulaRecursoRegencia> SelectBy_tud_id_tau_id_tud_idFilho(long tud_id, int tau_id, long tud_idFilho)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaRecursoRegencia_By_tud_id_tau_id_tud_idFilho", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = tud_idFilho;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                List<CLS_TurmaAulaRecursoRegencia> listaRecurso = new List<CLS_TurmaAulaRecursoRegencia>();

                listaRecurso = (from DataRow dr in qs.Return.Rows
                                select (CLS_TurmaAulaRecursoRegencia)DataRowToEntity(dr, new CLS_TurmaAulaRecursoRegencia())).ToList();

                return listaRecurso;
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
        /// Altera/Inclui os recursos de aula regente da tabela
        /// </summary>
        /// <param name="dtTurmaAulaRecursoRegencia"></param>
        /// <returns></returns>
        public bool SalvaRecursosAulaRegencia(DataTable dtTurmaAulaRecursoRegencia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaRecurso_SalvaRecursosAulaRegencia", _Banco);

            try
            {
                #region Parametro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaRecursoAulaRegencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaRecursoRegencia";
                sqlParam.Value = dtTurmaAulaRecursoRegencia;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion
    }
}