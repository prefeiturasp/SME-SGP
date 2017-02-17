namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Data;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CFG_CorRelatorioDAO : Abstract_CFG_CorRelatorioDAO
    {
        #region Consultas

        /// <summary>
        /// Retorna um datatable com a lista de cores para o relatório
        /// </summary>
        /// <param name="rlt_id">rlt_id id do relatório</param>
        /// <returns>
        /// DataTable com as cores por rlt_id
        /// </returns>
        public List<CFG_CorRelatorio> SelecionaCoresRelatorio(int rlt_id)
        {

            List<CFG_CorRelatorio> lt = new List<CFG_CorRelatorio>();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_CorRelatorioSelecionaPorRelatorio", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                foreach (DataRow dr in qs.Return.Rows)
                {
                    CFG_CorRelatorio entity = new CFG_CorRelatorio();
                    lt.Add(this.DataRowToEntity(dr, entity));
                }
                return lt;

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
        /// Retorna todas as cores não excluídos logicamente
        /// </summary>              
        /// <param name="rlt_id">Id do relatorio</param>        
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
            int rlt_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CorRelatorio_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS                

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                #endregion
                                   
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
        /// Retorna todas as cores não excluídos logicamente
        /// </summary>              
        /// <param name="rlt_id">Id do relatorio</param>        
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public bool ValidaCor
        (
            CFG_CorRelatorio entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_CorRelatorio_ValidaCor", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Size = 4;
                Param.Value = entity.rlt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cor_id";
                Param.Size = 4;
                Param.Value = entity.cor_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cor_corPaleta";
                Param.Size = 10;
                Param.Value = entity.cor_corPaleta;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                //totalRecords = qs.Return.Rows.Count;

                return qs.Return.Rows.Count > 0;
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

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_CorRelatorio entity)
        {
            base.ParamInserir(qs, entity);
            qs.Parameters["@cor_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cor_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_CorRelatorio entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cor_dataCriacao");
            qs.Parameters["@cor_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(CFG_CorRelatorio entity)
        {
            __STP_UPDATE = "NEW_CFG_CorRelatorio_UPDATE_AlteraTipo";
            return base.Alterar(entity);
        } 

        #endregion
    }
}