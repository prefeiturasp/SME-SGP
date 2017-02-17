/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class TUR_TurmaCurriculoAvaliacaoDAO : Abstract_TUR_TurmaCurriculoAvaliacaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os tca_ids das turmas informadas.
        /// </summary>
        /// <param name="tur_id">IDs das turmas</param>
        /// <returns></returns>
        public DataTable SelectBy_Turmas
        (
            string tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_Turmas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tur_id";
                Param.Value = tur_id;
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
        /// Retorna as avaliações da turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        public DataTable SelectBy_Turma
        (
            long tur_id
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_Turma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// Retorna a última avaliação da turma currículo.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <returns></returns>
        public TUR_TurmaCurriculoAvaliacao SelectBy_UltimaAvaliacaoTurmaCurriculo
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_UltimaAvaliacao", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                TUR_TurmaCurriculoAvaliacao entityTurmaCurriculoAvaliacao = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new TUR_TurmaCurriculoAvaliacao())).FirstOrDefault();

                return entityTurmaCurriculoAvaliacao;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a primeira avaliação da turma currículo.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <returns></returns>
        public TUR_TurmaCurriculoAvaliacao SelectBy_PrimeiraAvaliacaoTurmaCurriculo
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_PrimeiraAvaliacao", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                TUR_TurmaCurriculoAvaliacao entityTurmaCurriculoAvaliacao = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new TUR_TurmaCurriculoAvaliacao())).FirstOrDefault();

                return entityTurmaCurriculoAvaliacao;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna o tca_id da avaliação
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do período do currículo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        public int SelectBy_NumeroAvaliacao
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_NumeroAvaliacao", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
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

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crp_id";
            Param.Size = 4;
            Param.Value = crp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tca_numeroAvaliacao";
            Param.Size = 4;
            Param.Value = tca_numeroAvaliacao;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return string.IsNullOrEmpty(qs.Return.Rows[0][0].ToString()) ? 
                    0 : 
                    Convert.ToInt32(qs.Return.Rows[0][0].ToString());
            }

            return -1;
        }

	    /// <summary>
	    /// Retorna a turma avaliação em que o aluno está ativo.
	    /// </summary>	    
	    /// <param name="alu_id"></param>
	    /// <param name="cur_id"></param>
	    /// <param name="crr_id"></param>
	    /// <param name="crp_id"></param>
	    /// <returns></returns>
	    public TUR_TurmaCurriculoAvaliacao SelectBy_AvaliacaoAtualAluno
        (
            long alu_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_AtualAluno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                TUR_TurmaCurriculoAvaliacao entityTurmaCurriculoAvaliacao = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new TUR_TurmaCurriculoAvaliacao())).FirstOrDefault();

                return entityTurmaCurriculoAvaliacao;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se existe o mesmo currículo período e número de avaliação
        /// para a turma passada por parâmetro.
        /// </summary>
        /// <param name="tur_id">Id da turma a ser verificada</param>
        /// <param name="cur_idAvaliacao">Id do curso</param>
        /// <param name="crr_idAvaliacao">Id do curriculo</param>
        /// <param name="crp_idAvaliacao">Id do período</param>
        /// <param name="tca_numeroAvaliacao">número da avaliação</param>
        public DataTable SelectBy_VerificaAvaliacaoExistenteParaTurma
        (
            long tur_id
            , int cur_idAvaliacao
            , int crr_idAvaliacao
            , int crp_idAvaliacao
            , int tca_numeroAvaliacao
        )
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculoAvaliacao_SelectBy_VerificaAvaliacaoExistenteParaTurma", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_idAvaliacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_idAvaliacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_idAvaliacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tca_numeroAvaliacao";
                Param.Size = 4;
                Param.Value = tca_numeroAvaliacao;
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