/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Data;
    using System.Collections.Generic;

    #region Estruturas

    /// <summary>
    /// Estrutura que guarda os usuários para envio do alerta.
    /// </summary>
    [Serializable]
    public struct sAlertaFimFechamento
    {
        public Guid usu_id { get; set; }
        public long evt_id { get; set; }
        public string evt_nome { get; set; }
        public int dias { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: LOG_AlertaFimFechamento Business Object. 
    /// </summary>
    public class LOG_AlertaFimFechamentoBO : BusinessBase<LOG_AlertaFimFechamentoDAO, LOG_AlertaFimFechamento>
	{
        /// <summary>
        /// Salva o log de envio da notificação em lote.
        /// </summary>
        /// <param name="lstLog"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<LOG_AlertaFimFechamento> lstLog)
        {
            DataTable dtLog = LOG_AlertaFimFechamento.TipoTabela_LOG_AlertaFimFechamento();
            lstLog.ForEach(p =>
            {
                dtLog.Rows.Add(LogToDataRow(p, dtLog.NewRow()));
            });
            new LOG_AlertaFimFechamentoDAO().SalvarEmLote(dtLog);
            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static DataRow LogToDataRow(LOG_AlertaFimFechamento entity, DataRow dr)
        {
            dr["usu_id"] = entity.usu_id;
            dr["evt_id"] = entity.evt_id;
            dr["lff_dataEnvio"] = entity.lff_dataEnvio;
            return dr;
        }
    }
}