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
	public class LOG_TurmaAula_Alteracao : Abstract_LOG_TurmaAula_Alteracao
    {

        /// <summary>
        /// Propriedade auxiliar para a sincronização em lote das aulas do diário de classe.
        /// </summary>
        public long idAula { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_LOG_TurmaAula_Alteracao.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_LOG_TurmaAula_Alteracao.</returns>
        public static DataTable TipoTabela_LOG_TurmaAula_Alteracao()
        {
            DataTable dtLOGTurmaAulaAlteracao = new DataTable();
            dtLOGTurmaAulaAlteracao.Columns.Add("idAula", typeof(Int64));
            dtLOGTurmaAulaAlteracao.Columns.Add("tud_id", typeof(Int64));
            dtLOGTurmaAulaAlteracao.Columns.Add("tau_id", typeof(Int32));
            dtLOGTurmaAulaAlteracao.Columns.Add("lta_tipo", typeof(Int16));
            dtLOGTurmaAulaAlteracao.Columns.Add("lta_origem", typeof(Int16));
            dtLOGTurmaAulaAlteracao.Columns.Add("usu_id", typeof(Guid));
            dtLOGTurmaAulaAlteracao.Columns.Add("lta_data", typeof(DateTime));

            return dtLOGTurmaAulaAlteracao;
        }
	}
}