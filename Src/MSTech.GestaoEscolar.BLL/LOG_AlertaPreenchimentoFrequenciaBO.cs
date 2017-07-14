/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;
    using System;

    #region Estruturas

    /// <summary>
    /// Estrutura que guarda os usuários para envio do alerta.
    /// </summary>
    [Serializable]
    public struct sAlertaPreenchimentoFrequencia
    {
        public Guid usu_id { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: LOG_AlertaPreenchimentoFrequencia Business Object. 
    /// </summary>
    public class LOG_AlertaPreenchimentoFrequenciaBO : BusinessBase<LOG_AlertaPreenchimentoFrequenciaDAO, LOG_AlertaPreenchimentoFrequencia>
	{
        /// <summary>
        /// Salva o log de envio da notificação em lote.
        /// </summary>
        /// <param name="lstLog"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<LOG_AlertaPreenchimentoFrequencia> lstLog)
        {
            DataTable dtLog = LOG_AlertaPreenchimentoFrequencia.TipoTabela_LOG_AlertaPreenchimentoFrequencia();
            lstLog.ForEach(p =>
            {
                dtLog.Rows.Add(LogToDataRow(p, dtLog.NewRow()));
            });
            new LOG_AlertaPreenchimentoFrequenciaDAO().SalvarEmLote(dtLog);
            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static DataRow LogToDataRow(LOG_AlertaPreenchimentoFrequencia entity, DataRow dr)
        {
            dr["usu_id"] = entity.usu_id;
            dr["lpf_dataEnvio"] = entity.lpf_dataEnvio;
            return dr;
        }
    }
}