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
    public class LOG_AlertaFimFechamento : Abstract_LOG_AlertaFimFechamento
	{
        public static DataTable TipoTabela_LOG_AlertaFimFechamento()
        {
            DataTable dtLog = new DataTable();
            dtLog.Columns.Add("usu_id", typeof(Guid));
            dtLog.Columns.Add("evt_id", typeof(long));
            dtLog.Columns.Add("lff_dataEnvio", typeof(DateTime));
            return dtLog;
        }
    }
}