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
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_ConfiguracaoAtividadeDAO : Abstract_CLS_ConfiguracaoAtividadeDAO
	{
        /// <summary>
        /// Seleciona configuracoes de acordo com o curriculo periodo
        /// </summary>
        /// <param name="cur_id">ID curso</param>
        /// <param name="crr_id">ID curriculo</param>
        /// <param name="crp_id">ID periodo</param>
        /// <param name="anoLetivo"></param>
        /// <returns>Lista de disciplinas com configurações de atividade</returns>
        public DataTable GetSelectBy_CurriculoPeriodo
            ( int cur_id
            , int crr_id
            , int crp_id
            , int anoLetivo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_ConfiguracaoAtividade_SelectBy_CurriculoPeriodo", _Banco);

            try
            {
                #region Parâmetros

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
                Param.ParameterName = "@anoLetivo";
                Param.Size = 4;
                Param.Value = anoLetivo;
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

        /// <summary>
        /// Seleciona configuracoes de acordo com o curriculo periodo e escola
        /// </summary>
        /// <param name="cur_id">ID curso</param>
        /// <param name="crr_id">ID curriculo</param>
        /// <param name="crp_id">ID periodo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="anoLetivo">Ano letivo da configuração</param>
        /// <returns>Lista de disciplinas com configurações de atividade</returns>
        public DataTable GetSelectBy_CurriculoPeriodoEscola
            (int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
            , int anoLetivo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_ConfiguracaoAtividade_SelectBy_CurriculoPeriodoEscola", _Banco);

            try
            {
                #region Parâmetros

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
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.ParameterName = "@anoLetivo";
                Param.Size = 4;
                Param.Value = anoLetivo;
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

        /// <summary>
        /// Atualiza a situação de configuração de atividade por ano, escola, curso e período.
        /// </summary>
        /// <param name="caa_anoLetivo">Ano letivo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade de escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="caa_situacao">Situação</param>
        /// <returns></returns>
        public bool UpdateSituacaoPorEscolaCursoPeriodoAno
        (
            int caa_anoLetivo
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , byte caa_situacao
        )
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_ConfiguracaoAtividade_UpdateSituacaoPorEscolaCursoPeriodoAno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caa_anoLetivo";
                Param.Size = 4;
                Param.Value = caa_anoLetivo;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@caa_situacao";
                Param.Size = 4;
                Param.Value = caa_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@caa_dataAlteracao";
                Param.Size = 4;
                Param.Value = DateTime.Now;
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
     
	}
}