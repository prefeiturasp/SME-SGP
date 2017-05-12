/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_ConfiguracaoServicoPendenciaDAO : Abstract_ACA_ConfiguracaoServicoPendenciaDAO
	{
        /// <summary>
        /// Retorna as configurações de serviço de pendência não excluídas logicamente, de acordo com tipo de nível de ensino,
        /// tipo de modalidade de ensino e tipo de turma.
        /// </summary>   
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tur_tipo">Enum do tipo de turma</param>
        public DataTable SelectBy_tne_id_tme_id_tur_tipo(int tne_id, int tme_id, int tur_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ConfiguracaoServicoPendencia_SelectBy_tne_id_tme_id_tur_tipo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                Param.Value = tme_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_tipo";
                Param.Size = 4;
                Param.Value = tur_tipo;
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
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@csp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@csp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@csp_dataCriacao");
            qs.Parameters["@csp_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_ConfiguracaoServicoPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_ConfiguracaoServicoPendencia entity)
        {
            __STP_UPDATE = "NEW_ACA_ConfiguracaoServicoPendencia_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@csp_id";
            Param.Size = 4;
            Param.Value = entity.csp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@csp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@csp_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_ConfiguracaoServicoPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_ConfiguracaoServicoPendencia entity)
        {
            __STP_DELETE = "NEW_ACA_ConfiguracaoServicoPendencia_UpdateSituacao";
            return base.Delete(entity);
        }
    }
}