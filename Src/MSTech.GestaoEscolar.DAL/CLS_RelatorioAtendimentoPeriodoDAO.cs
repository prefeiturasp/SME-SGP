/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Data.SqlClient;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioAtendimentoPeriodoDAO : Abstract_CLS_RelatorioAtendimentoPeriodoDAO
	{
        /// <summary>
        /// Seleciona os tipos de período de calendário para preenchimento do relatório de atendimento
        /// </summary>
        /// <param name="rea_id"></param>
        /// <returns></returns>
        public List<CLS_RelatorioAtendimentoPeriodo> SelecionaPorRelatorio(int rea_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimentoPeriodo_SelecionaPorRelatorio", _Banco);

            try
            {
                Param = qs.NewParameter();
                Param.ParameterName = "@rea_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rea_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Select().Select(p => DataRowToEntity(p, new CLS_RelatorioAtendimentoPeriodo())).ToList() :
                    new List<CLS_RelatorioAtendimentoPeriodo>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Atualiza os periodos vinculados ao relatório de atendimento.
        /// </summary>
        /// <param name="dtRelatorioAtendimentoPeriodo"></param>
        /// <returns></returns>
        public bool AtualizarPeriodos(DataTable dtRelatorioAtendimentoPeriodo)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_RelatorioAtendimentoPeriodo_Atualiza", _Banco);

            try
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "TipoTabela_RelatorioAtendimentoPeriodo";
                param.Value = dtRelatorioAtendimentoPeriodo;
                param.ParameterName = "@tbRelatorioAtendimentoPeriodo";
                qs.Parameters.Add(param);

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_RelatorioAtendimentoPeriodo entity)
        {
            if (qs != null && entity != null)
            {
                return true;
            }

            return false;
        }
    }
}