/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class LOG_AlertaPreenchimentoFrequencia : Abstract_LOG_AlertaPreenchimentoFrequencia
	{
        public static DataTable TipoTabela_LOG_AlertaPreenchimentoFrequencia()
        {
            DataTable dtLog = new DataTable();
            dtLog.Columns.Add("usu_id", typeof(Guid));
            dtLog.Columns.Add("lpf_dataEnvio", typeof(DateTime));
            return dtLog;
        }
    }
}