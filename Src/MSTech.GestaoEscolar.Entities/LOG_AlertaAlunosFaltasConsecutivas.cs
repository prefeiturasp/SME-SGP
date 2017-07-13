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
    public class LOG_AlertaAlunosFaltasConsecutivas : Abstract_LOG_AlertaAlunosFaltasConsecutivas
	{
        public static DataTable TipoTabela_LOG_AlertaAlunosFaltasConsecutivas()
        {
            DataTable dtLog = new DataTable();
            dtLog.Columns.Add("usu_id", typeof(Guid));
            dtLog.Columns.Add("esc_id", typeof(int));
            dtLog.Columns.Add("lfc_dataEnvio", typeof(DateTime));
            return dtLog;
        }
    }
}