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
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_MotivoBaixaFrequenciaDAO : Abstract_ACA_MotivoBaixaFrequenciaDAO
    {

        #region Métodos

        /// <summary>
        /// Retorna os motivos de infrequências ativos (area e seus itens)
        /// </summary>
        /// <param name="totalRecords">Total de registros da consulta</param>
        /// <returns>Lista com os motivos</returns>
        public DataTable SelecionarAtivos
        (
            out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_MotivoBaixaFrequencia_SelecionarAtivos", _Banco);
            List<ACA_MotivoBaixaFrequencia> lstTpCiclo = new List<ACA_MotivoBaixaFrequencia>();
            try
            {
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
        /// Retorna todos os itens da área informada
        /// mbf_idPai => é o vinculo do item com a área
        /// </summary>
        /// <param name="mbf_idPai">Id do motivo de baixa frequência</param>
        /// <returns>Lista com os itens</returns>
        public DataTable SelecionaTodosItens_Por_MotivoBaixaFrequencia
        (
            int mbf_idPai
        )
        {
            return SelecionaItem_Por_MotivoBaixaFrequencia(0, mbf_idPai);
        }

        /// <summary>
        /// Retorna um item especifico do motivo de infrequência informado. 
        /// </summary>
        /// <param name="mbf_id">Id do motivo de baixa frequência</param>
        /// <param name="mbf_idPai">Id do motivo - indicando o vinculo com a area(mbf_id) </param>
        /// <returns>Lista com os itens</returns>
        public DataTable SelecionaItem_Por_MotivoBaixaFrequencia
        (
            int mbf_id,
            int mbf_idPai
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_MotivoBaixaFrequencia_SelectBy_Id", _Banco);
            try
            {
                #region
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mbf_id";
                Param.Size = 4;
                if (mbf_id > 0)
                    Param.Value = mbf_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mbf_idPai";
                Param.Size = 4;
                Param.Value = mbf_idPai;
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

        #endregion

        #region Sobrescritos
        /// <summary>
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_MotivoBaixaFrequencia entity)
        {
            entity.mbf_dataCriacao = entity.mbf_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_MotivoBaixaFrequencia entity)
        {
            entity.mbf_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@mbf_dataCriacao");
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_MotivoBaixaFrequencia</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_MotivoBaixaFrequencia entity)
        {
            __STP_UPDATE = "NEW_ACA_MotivoBaixaFrequencia_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_MotivoBaixaFrequencia entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mbf_id";
            Param.Size = 4;
            Param.Value = entity.mbf_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mbf_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mbf_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_MotivoBaixaFrequencia</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_MotivoBaixaFrequencia entity)
        {
            __STP_DELETE = "NEW_ACA_MotivoBaixaFrequencia_UPDATE_Situacao";
            return base.Delete(entity);
        }
        #endregion

	}
}