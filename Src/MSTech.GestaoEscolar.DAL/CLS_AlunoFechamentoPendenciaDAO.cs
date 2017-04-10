/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
	
	/// <summary>
	/// Description: Fila para o pré-processamento dos cálculos do fechamento..
	/// </summary>
	public class CLS_AlunoFechamentoPendenciaDAO : Abstract_CLS_AlunoFechamentoPendenciaDAO
	{
        /// <summary>
        /// Retorna as quantidades da fila de acordo com a situação.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaFila_PorSituacao()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SelectFila", _Banco);
            
            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        ///	Retorna os logs da execução de fechamento de acordo com os filtros.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaExecucoesFila(int top, bool somenteCompleta)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_LOG_FilaFechamento_SelecionaExecucoes", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@top";
            Param.Size = 4;
            Param.Value = top;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@somenteCompleta";
            Param.Size = 1;
            Param.Value = somenteCompleta;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag frequência marcada (caso o item já exista, apenas atualiza).
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarFilaFrequencia(long tud_id, int tpc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SalvarFilaFrequencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag frequência externa marcada (caso o item já exista, apenas atualiza).
        /// </summary>
        /// <param name="listFila">Lista com tpc_ids e tud_ids.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarFilaFrequenciaExterna(DataTable dtAlunoFechamentoPendencia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SalvarFila", _Banco);

            #region PARAMETROS

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@tbDadosAlunoFechamentoPendencia";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoFechamentoPendencia";
            sqlParam.Value = dtAlunoFechamentoPendencia;
            qs.Parameters.Add(sqlParam);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag nota marcada.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarFilaNota(long tud_id, int tpc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SalvarFilaNota", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }

        /// <summary>
        /// Salva item na fila de processamento com a processo = 2, para verificar pendencia de fechamento.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id"></param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarFilaPendencias(long tud_id, int tpc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SalvarFilaPendencias", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);
            
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@tbAlunoFechamentoPendencia";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoFechamentoPendencia";
            sqlParam.Value = null;
            qs.Parameters.Add(sqlParam);
            
            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }

        /// <summary>
        /// Salva item na fila de processamento em lote com a processo = 2, para verificar pendencia de fechamento.
        /// </summary>
        /// <param name="dtAlunoFechamentoPendencia"></param>
        /// /// <returns>True em caso de sucesso.</returns>
        public bool SalvarFilaPendencias(DataTable dtAlunoFechamentoPendencia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SalvarFilaPendencias", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@tbAlunoFechamentoPendencia";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoFechamentoPendencia";
            sqlParam.Value = dtAlunoFechamentoPendencia;
            qs.Parameters.Add(sqlParam);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }


        /// <summary>
        /// Retorna se existe registro nao processado na fila de fechamento para a turma disciplina e periodo.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tud_tipo">Tipo da disciplina na turma.</param>       
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns></returns>
        public DataTable SelecionarAguardandoProcessamento(long tur_id, long tud_id, byte tud_tipo, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_SelecionarAguardandoProcessamento", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tud_tipo";
            Param.Size = 1;
            Param.Value = tud_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        public bool Processar(long tud_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("MS_JOB_AtualizaFaltasFechamento", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tud_idFiltrar";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return > 0);
        }
        
        /// <summary>
        /// Delete as pendências de fechamento.
        /// </summary>
        /// <param name="dtAlunoFechamentoPendencia"></param>
        /// <returns></returns>
        public bool DeletarEmLote(DataTable dtAlunoFechamentoPendencia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoFechamentoPendencia_DeletaEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoFechamentoPendencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoFechamentoPendencia";
                sqlParam.Value = dtAlunoFechamentoPendencia;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Processa as pendências de fechamento dos bimestres.
        /// </summary>
        /// <param name="tud_id"></param>
        public void ProcessarPendenciasAnteriores(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_idFiltrar";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
	}
}