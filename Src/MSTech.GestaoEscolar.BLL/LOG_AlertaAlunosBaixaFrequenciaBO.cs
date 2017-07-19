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
    public struct sAlertaAlunosBaixaFrequencia
    {
        public Guid usu_id { get; set; }
        public int esc_id { get; set; }
        public string esc_nome { get; set; }
        public decimal percentualBaixaFrequencia { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: LOG_AlertaAlunosBaixaFrequencia Business Object. 
    /// </summary>
    public class LOG_AlertaAlunosBaixaFrequenciaBO : BusinessBase<LOG_AlertaAlunosBaixaFrequenciaDAO, LOG_AlertaAlunosBaixaFrequencia>
    {
        /// <summary>
        /// Salva o log de envio da notificação em lote.
        /// </summary>
        /// <param name="lstLog"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<LOG_AlertaAlunosBaixaFrequencia> lstLog)
        {
            DataTable dtLog = LOG_AlertaAlunosBaixaFrequencia.TipoTabela_LOG_AlertaAlunosBaixaFrequencia();
            lstLog.ForEach(p =>
            {
                dtLog.Rows.Add(LogToDataRow(p, dtLog.NewRow()));
            });
            new LOG_AlertaAlunosBaixaFrequenciaDAO().SalvarEmLote(dtLog);
            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static DataRow LogToDataRow(LOG_AlertaAlunosBaixaFrequencia entity, DataRow dr)
        {
            dr["usu_id"] = entity.usu_id;
            dr["esc_id"] = entity.esc_id;
            dr["lbf_dataEnvio"] = entity.lbf_dataEnvio;
            return dr;
        }
    }
}