/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class TUR_TurmaCurriculoDAO : Abstract_TUR_TurmaCurriculoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna as entidades da TurmaCurriculo cadastradas na turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculo_SelectBy_Turma", _Banco);

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
        
        /// <summary>
        /// Seleciona a relação de turmas e cursos por escola e curso.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="apenasAtivos">Flag que indica se deve buscar apenas por relações ativas.</param>
        /// <returns></returns>
        public List<TUR_TurmaCurriculo> SelecionaPorEscolaCurso(int esc_id, int uni_id, int cur_id, int crr_id, byte tcr_situacao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculo_SelecionaPor_EscolaCurso", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tcr_situacao";
                Param.Size = 1;
                if (tcr_situacao > 0)
                    Param.Value = tcr_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>()
                                     .Select(p => DataRowToEntity(p, new TUR_TurmaCurriculo())).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// O método altera a matriz curricular das turmas passadas por parâmetro.
        /// </summary>
        /// <param name="cur_idOrigem">ID do curso de origem.</param>
        /// <param name="crr_idOrigem">ID do currículo de origem.</param>
        /// <param name="cur_idDestino">ID do curso de destino.</param>
        /// <param name="crr_idDestino">ID do currículo de destino.</param>
        /// <param name="tur_ids">IDs das turmas.</param>
        /// <returns></returns>
        public bool AlterarMatrizCurricularTurmas(int cur_idOrigem, int crr_idOrigem, int cur_idDestino, int crr_idDestino, string tur_ids)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_TUR_TurmaCurriculo_UpdateMatrizCurricular", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_idOrigem";
                Param.Size = 4;
                Param.Value = cur_idOrigem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_idOrigem";
                Param.Size = 4;
                Param.Value = crr_idOrigem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_idDestino";
                Param.Size = 4;
                Param.Value = cur_idDestino;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_idDestino";
                Param.Size = 4;
                Param.Value = crr_idDestino;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_ids";
                Param.Value = tur_ids;
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

        /// <summary>
        /// Retorna as entidades da TurmaCurriculo cadastradas nas turmas.
        /// </summary>
        /// <param name="tur_id">ID das turmas</param>
        /// <returns></returns>
        public DataTable SelecionaPorTurmas
        (
            string tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaCurriculo_SelecionaPorTurmas", _Banco);

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

        #endregion
    }
}