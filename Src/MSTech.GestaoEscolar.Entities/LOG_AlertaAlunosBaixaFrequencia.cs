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
    public class LOG_AlertaAlunosBaixaFrequencia : Abstract_LOG_AlertaAlunosBaixaFrequencia
	{
        public static DataTable TipoTabela_LOG_AlertaAlunosBaixaFrequencia()
        {
            DataTable dtLog = new DataTable();
            dtLog.Columns.Add("usu_id", typeof(Guid));
            dtLog.Columns.Add("esc_id", typeof(int));
            dtLog.Columns.Add("lbf_dataEnvio", typeof(DateTime));
            return dtLog;
        }
    }
}