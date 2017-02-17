/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_TipoResultadoDAO : AbstractACA_TipoResultadoDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Busca os tipos de resultados com base no curso.
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <returns></returns>
        public DataTable SELECT_By_Pesquisa(
            int cur_id
            , int crr_id
            , out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoResultado_SELECT_By_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

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

                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca os tipos de resultados pelo tipo de lançamento.
        /// </summary>
        /// <param name="cur_id">Tipo de lançamento</param>
        /// <returns></returns>
        public DataTable SELECT_By_TipoLancamento(byte tpr_tipoLancamento)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoResultado_SELECT_By_TipoLancamento", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpr_tipoLancamento";
                Param.Size = 4;
                Param.Value = tpr_tipoLancamento;
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
        /// Seleciona tipos de resultado por tipo lançamento e tipo de currículo período e ano letivo.
        /// </summary>
        /// <param name="tpr_tipoLancamento">Tipo de lançamento do resultado.</param>
        /// <param name="tcp_id">ID do tipo de currículo período.</param>
        /// <param name="anoLetivo">Ano letivo.</param>
        /// <returns></returns>
        public DataTable SelecionaPorTipoLancamentoTipoCurriculoPeriodoAno(byte tpr_tipoLancamento, int tcp_id, int anoLetivo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoResultado_SelecionaPorTipoLancamentoTipoCurriculoPeriodoAno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tpr_tipoLancamento";
                Param.Size = 1;
                Param.Value = tpr_tipoLancamento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Size = 4;
                Param.Value = tcp_id;
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

        #endregion

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ACA_TipoResultado entity)
        {
            entity.tpr_dataCriacao = DateTime.Now;
            entity.tpr_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ACA_TipoResultado entity)
        {

            entity.tpr_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tpr_dataCriacao");
        }

        protected override bool Alterar(Entities.ACA_TipoResultado entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoResultado_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.ACA_TipoResultado entity)
        {
            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tpr_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tpr_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.ACA_TipoResultado entity)
        {
            __STP_DELETE = "NEW_ACA_TipoResultado_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion Sobrescritos
	}
}