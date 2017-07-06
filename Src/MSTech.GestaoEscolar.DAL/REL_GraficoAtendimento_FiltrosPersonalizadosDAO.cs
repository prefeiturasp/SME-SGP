/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System.Collections.Generic;
    using Data.Common;
    using System.Data;
    using System.Linq;
    using System;

    /// <summary>
    /// Description: .
    /// </summary>
    public class REL_GraficoAtendimento_FiltrosPersonalizadosDAO : Abstract_REL_GraficoAtendimento_FiltrosPersonalizadosDAO
	{
        public List<REL_GraficoAtendimento_FiltrosPersonalizados> SelectBy_gra_id(int gra_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_FiltrosPersonalizados_SelectBy_gra_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@gra_id";
                Param.Size = 4;
                Param.Value = gra_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (from DataRow dr in qs.Return.Rows select DataRowToEntity(dr, new REL_GraficoAtendimento_FiltrosPersonalizados())).ToList();
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
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(REL_GraficoAtendimento_FiltrosPersonalizados entity)
        {
            __STP_DELETE = "NEW_REL_GraficoAtendimento_FiltrosPersonalizados_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, REL_GraficoAtendimento_FiltrosPersonalizados entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@gfp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@gfp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, REL_GraficoAtendimento_FiltrosPersonalizados entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@gfp_dataCriacao");
            qs.Parameters["@gfp_dataAlteracao"].Value = DateTime.Now;
        }
        
        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(REL_GraficoAtendimento_FiltrosPersonalizados entity)
        {
            __STP_UPDATE = "NEW_REL_GraficoAtendimento_FiltrosPersonalizados_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, REL_GraficoAtendimento_FiltrosPersonalizados entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@gfp_id";
            Param.Size = 4;
            Param.Value = entity.gra_id;
            qs.Parameters.Add(Param);           
        }
    }
}