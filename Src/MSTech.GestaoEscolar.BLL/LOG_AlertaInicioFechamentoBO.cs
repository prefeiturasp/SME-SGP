/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data;

    #region Estruturas

    /// <summary>
    /// Estrutura que guarda os usuários para envio do alerta.
    /// </summary>
    [Serializable]
    public struct sAlertaInicioFechamento
    {
        public Guid usu_id { get; set; }
        public long evt_id { get; set; }
        public string evt_nome { get; set; }
        public int dias { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: LOG_AlertaInicioFechamento Business Object. 
    /// </summary>
    public class LOG_AlertaInicioFechamentoBO : BusinessBase<LOG_AlertaInicioFechamentoDAO, LOG_AlertaInicioFechamento>
    {
        /// <summary>
        /// Salva o log de envio da notificação em lote.
        /// </summary>
        /// <param name="lstLog"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<LOG_AlertaInicioFechamento> lstLog)
        {
            DataTable dtLog = LOG_AlertaInicioFechamento.TipoTabela_LOG_AlertaInicioFechamento();
            lstLog.ForEach(p =>
            {
                dtLog.Rows.Add(LogToDataRow(p, dtLog.NewRow()));
            });
            new LOG_AlertaInicioFechamentoDAO().SalvarEmLote(dtLog);
            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static DataRow LogToDataRow(LOG_AlertaInicioFechamento entity, DataRow dr)
        {
            dr["usu_id"] = entity.usu_id;
            dr["evt_id"] = entity.evt_id;
            dr["lif_dataEnvio"] = entity.lif_dataEnvio;
            return dr;
        }
    }
}