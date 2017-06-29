/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioPreenchimentoAcoesRealizadasDAO : Abstract_CLS_RelatorioPreenchimentoAcoesRealizadasDAO
	{
        #region Consulta

        /// <summary>
        /// Retorna as ações realizadas cadastradas em um preenchimento de relatório.
        /// </summary>
        /// <param name="reap_id">Id do preenchimento de relatório</param>
        /// <returns></returns>
        public DataTable SelecionaPorPreenchimento(long reap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioPreenchimentoAcoesRealizadas_SelecionaPorPreenchimento", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@reap_id";
                Param.Size = 8;
                Param.Value = reap_id;
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

        #endregion Consulta

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_RelatorioPreenchimentoAcoesRealizadas entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@rpa_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@rpa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_RelatorioPreenchimentoAcoesRealizadas entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@rpa_dataCriacao");
            qs.Parameters["@rpa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>s
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade CLS_RelatorioPreenchimentoAcoesRealizadas</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_RelatorioPreenchimentoAcoesRealizadas entity)
        {
            __STP_UPDATE = "NEW_CLS_RelatorioPreenchimentoAcoesRealizadas_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_RelatorioPreenchimentoAcoesRealizadas entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@rpa_id";
            Param.Size = 8;
            Param.Value = entity.rpa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@rpa_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@rpa_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade CLS_RelatorioPreenchimentoAcoesRealizadas</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(CLS_RelatorioPreenchimentoAcoesRealizadas entity)
        {
            __STP_DELETE = "NEW_CLS_RelatorioPreenchimentoAcoesRealizadas_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}