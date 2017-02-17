/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Data;
    using System.Data.SqlClient;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_FechamentoJustificativaPendenciaDAO : AbstractCLS_FechamentoJustificativaPendenciaDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona as justificativas de pendência de acordo com os filtros de busca.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <param name="totalRecords">Total de registros retornados</param>
        /// <returns></returns>
        public DataTable SelectBy_Busca
        (
            int esc_id
            , int uni_id
            , int cal_id
            , long tud_id
            , int tpc_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_FechamentoJustificativaPendencia_Select_Busca", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

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
        /// Seleciona as justificativas de pendência cadastradas para a turma disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <returns></returns>
        public DataTable SelectBy_TurmaDisciplina
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_FechamentoJustificativaPendencia_SelectBy_TurmaDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
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
        /// Seleciona as justificativas de pendência cadastradas para a turma disciplina e período do calendário.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="cal_id">Id do período do calendário</param>
        /// <returns></returns>
        public CLS_FechamentoJustificativaPendencia SelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , int cal_id
            , int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_FechamentoJustificativaPendencia_SelectBy_TurmaDisciplinaPeriodo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                
                if (qs.Return.Rows.Count > 0)
                {
                    return DataRowToEntity(qs.Return.Rows[0], new CLS_FechamentoJustificativaPendencia());
                }
                return new CLS_FechamentoJustificativaPendencia();
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

        #endregion Métodos de consulta

        #region Métodos de insert/update

        /// <summary>
        /// Salva as justificativas de pendencia em lote.
        /// </summary>
        /// <param name="dtFechamentoJustificativaPendencia">Tabela com os dados das justificativas.</param>
        /// <returns></returns>
        public bool SalvarEmLote(DataTable dtFechamentoJustificativaPendencia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_FechamentoJustificativaPendencia_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@JustificativaPendencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_FechamentoJustificativaPendencia";
                sqlParam.Value = dtFechamentoJustificativaPendencia;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de insert/update

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_FechamentoJustificativaPendencia entity)
        {
            base.ParamInserir(qs, entity);
            qs.Parameters["@fjp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@fjp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_FechamentoJustificativaPendencia entity)
        {
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@fjp_dataCriacao");
            qs.Parameters.RemoveAt("@usu_id");
            qs.Parameters["@fjp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_FechamentoJustificativaPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_FechamentoJustificativaPendencia entity)
        {
            __STP_UPDATE = "NEW_CLS_FechamentoJustificativaPendencia_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_FechamentoJustificativaPendencia entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fjp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@fjp_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_idAlteracao";
            Param.Size = 16;
            Param.Value = entity.usu_idAlteracao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_FechamentoJustificativaPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(CLS_FechamentoJustificativaPendencia entity)
        {
            __STP_DELETE = "NEW_CLS_FechamentoJustificativaPendencia_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}