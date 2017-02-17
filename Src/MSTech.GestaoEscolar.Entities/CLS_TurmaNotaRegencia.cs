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
    [Serializable]
	public class CLS_TurmaNotaRegencia : Abstract_CLS_TurmaNotaRegencia
	{
        /// <summary>
        /// Id da atividade utilizado para a sincronização.
        /// </summary>
        public long idAtividade { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_TurmaNotaRegencia.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_TurmaNotaRegencia.</returns>
        public static DataTable TipoTabela_TurmaNotaRegencia()
        {
            DataTable dtTurmaNotaAluno = new DataTable();
            dtTurmaNotaAluno.Columns.Add("idAtividade", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("tud_id", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("tnt_id", typeof(Int32));
            dtTurmaNotaAluno.Columns.Add("tud_idAula", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("tau_idAula", typeof(Int32));

            return dtTurmaNotaAluno;
        }
	}
}