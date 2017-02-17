/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class TUR_TurmaDisciplinaRelacionadaDAO : AbstractTUR_TurmaDisciplinaRelacionadaDAO
	{
        /// <summary>
        /// Seleciona o historico de docencia compartilhada da disciplina.
        /// </summary>
        /// <param name="tud_id">ID disciplina compartilhada</param>
        /// <param name="totalRecords">Total de linhas</param>
        /// <returns>Datatable com dados</returns>
        public DataTable SelecionarHistoricoPorDisciplinaCompartilhada
        (
            long tud_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplinaRelacionada_SelecionarHistoricoPorDisciplinaCompartilhada", _Banco);
            try
            {

                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
        /// Valida se ja existe aula criada na data de encerramento da vigencia ou em data posterior
        /// </summary>
        /// <param name="tud_id">ID disciplina compartilhada</param>
        /// <param name="data">Data para validar</param>
        /// <returns>true -> existe aula; false -> nao existe aula</returns>
        public bool ValidarAulaDocenciaCompartilhada
        (
            long tud_id
            , DateTime data
        )
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_TUR_TurmaDisciplinaRelacionada_ValidarAulaDocenciaCompartilhada", _Banco);
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
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@data";
                Param.Size = 20;
                Param.Value = data;
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

        #region Métodos Sobrescritos

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
        {
            entity.tdr_id = Convert.ToInt64(qs.Return.Rows[0][0]);
            return (entity.tdr_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tdr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tdr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tdr_dataCriacao");
            qs.Parameters["@tdr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade TUR_TurmaDisciplinaRelacionada</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(TUR_TurmaDisciplinaRelacionada entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaDisciplinaRelacionada_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdr_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tdr_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade TUR_TurmaDisciplinaRelacionada</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(TUR_TurmaDisciplinaRelacionada entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaDisciplinaRelacionada_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
	}
}