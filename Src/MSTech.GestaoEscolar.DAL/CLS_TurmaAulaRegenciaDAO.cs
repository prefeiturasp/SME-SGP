/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_TurmaAulaRegenciaDAO : AbstractCLS_TurmaAulaRegenciaDAO
    {
        #region Metodos

        /// <summary>
        /// Seleciona as turmas aula de regencia por disciplina e turma aula
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <returns></returns>
        public DataTable SelecionaPorDisciplinaTurmaAula
        (
            long tud_id
            , int tau_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaRegencia_SelectBy_DisciplinaTurmaAula", _Banco);
            try
            {
                #region PARAMETROS

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

                #endregion PARAMETROS

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

        #endregion Metodos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAulaRegencia entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tuf_data"].DbType = DbType.DateTime;
            qs.Parameters["@tuf_planoAula"].DbType = DbType.String;
            qs.Parameters["@tuf_diarioClasse"].DbType = DbType.String;
            qs.Parameters["@tuf_conteudo"].DbType = DbType.String;

            qs.Parameters["@tuf_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tuf_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAulaRegencia entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@tuf_data"].DbType = DbType.DateTime;
            qs.Parameters["@tuf_planoAula"].DbType = DbType.String;
            qs.Parameters["@tuf_diarioClasse"].DbType = DbType.String;
            qs.Parameters["@tuf_conteudo"].DbType = DbType.String;

            qs.Parameters.RemoveAt("@tuf_dataCriacao");
            qs.Parameters["@tuf_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação.
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAulaRegencia</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_TurmaAulaRegencia entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAulaRegencia_UPDATE";
            return base.Alterar(entity);
        }
	}
}