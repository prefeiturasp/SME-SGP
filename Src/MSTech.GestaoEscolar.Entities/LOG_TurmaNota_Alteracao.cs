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
	public class LOG_TurmaNota_Alteracao : Abstract_LOG_TurmaNota_Alteracao
	{

        /// <summary>
        /// Id da atividade utilizado para a sincronização.
        /// </summary>
        public long idAtividade { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_LOG_TurmaNota_Alteracao.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_LOG_TurmaNota_Alteracao.</returns>
        public static DataTable TipoTabela_LOG_TurmaNota_Alteracao()
        {
            DataTable dtLOGTurmaNotaAlteracao = new DataTable();
            dtLOGTurmaNotaAlteracao.Columns.Add("idAtividade", typeof(Int64));
            dtLOGTurmaNotaAlteracao.Columns.Add("tud_id", typeof(Int64));
            dtLOGTurmaNotaAlteracao.Columns.Add("tnt_id", typeof(Int32));
            dtLOGTurmaNotaAlteracao.Columns.Add("ltn_tipo", typeof(Int16));
            dtLOGTurmaNotaAlteracao.Columns.Add("ltn_origem", typeof(Int16));
            dtLOGTurmaNotaAlteracao.Columns.Add("usu_id", typeof(Guid));
            dtLOGTurmaNotaAlteracao.Columns.Add("ltn_data", typeof(DateTime));

            return dtLOGTurmaNotaAlteracao;
        }
	}
}