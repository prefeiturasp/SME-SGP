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
	public class CLS_ConfiguracaoAtividadeQualificadorDAO : Abstract_CLS_ConfiguracaoAtividadeQualificadorDAO
	{
        /// <summary>
        /// Seleciona a configuracao para um qualificador de atividade da turma disciplina.
        /// </summary>
        /// <param name="tudId">Id da turma disciplina</param>
        /// <param name="qatId">Id do qualificador</param>
        /// <returns></returns>
        public CLS_ConfiguracaoAtividadeQualificador GetSelectByTudQualificador(long tudId, int qatId)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_ConfiguracaoAtividadeQualificador_SelectBy_TudQualificador", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tudId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qat_id";
                Param.Size = 4;
                Param.Value = qatId;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return this.DataRowToEntity(qs.Return.Rows[0], new CLS_ConfiguracaoAtividadeQualificador());
                }
                else
                {
                    return new CLS_ConfiguracaoAtividadeQualificador();
                }
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
        /// Configura os parâmetros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_ConfiguracaoAtividadeQualificador entity)
        {
            entity.caq_quantidade = entity.caq_quantidade != null ? entity.caq_quantidade : 0;
            entity.caq_dataCriacao = DateTime.Now;
            entity.caq_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Configura os parâmetros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_ConfiguracaoAtividadeQualificador entity)
        {
            entity.caq_quantidade = entity.caq_quantidade != null ? entity.caq_quantidade : 0;
            entity.caq_dataAlteracao = DateTime.Now;

            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@caq_dataCriacao");
        }

        /// <summary>
        /// Inseri os valores da classe em um registro ja existente.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(CLS_ConfiguracaoAtividadeQualificador entity)
        {
            __STP_UPDATE = "NEW_CLS_ConfiguracaoAtividadeQualificador_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Exclui lógicamente um registro do banco.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem excluídos logicamente</param>
        /// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(CLS_ConfiguracaoAtividadeQualificador entity)
        {
            __STP_DELETE = "NEW_CLS_ConfiguracaoAtividadeQualificador_UPDATE_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Configura os parâmetros do metodo de Deletar.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_ConfiguracaoAtividadeQualificador entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@caa_id";
            Param.Size = 4;
            Param.Value = entity.caa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@qat_id";
            Param.Size = 4;
            Param.Value = entity.qat_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@caq_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@caq_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.caq_dataAlteracao;
            qs.Parameters.Add(Param);
        }
	}
}