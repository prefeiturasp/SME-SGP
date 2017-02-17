using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// Classe MTR_ProcessoFechamentoInicioDAO
    /// </summary>
    public class MTR_ProcessoFechamentoInicioDAO : Abstract_MTR_ProcessoFechamentoInicioDAO
    {
        #region Métodos

        /// <summary>
        /// Seleciona os processos de fechamento/início do ano letivo por ano letivo corrente.
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Retorna DataTable contendo os processos de fechamento/início do ano letivo corrente</returns>
        public MTR_ProcessoFechamentoInicio CarregaPorAnoLetivoCorrente(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ProcessoFechamentoInicio_SelectBy_pfi_anoLetivoCorrente", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                DataTable dt = qs.Return;

                MTR_ProcessoFechamentoInicio entity = new MTR_ProcessoFechamentoInicio();
                if (dt.Rows.Count > 0)
                {
                    MTR_ProcessoFechamentoInicioDAO dao = new MTR_ProcessoFechamentoInicioDAO();
                    entity = dao.DataRowToEntity(dt.Rows[0], entity);
                }

                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos
    }
}