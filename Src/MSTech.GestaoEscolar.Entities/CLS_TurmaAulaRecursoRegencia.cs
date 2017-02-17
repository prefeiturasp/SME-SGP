/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.Data;
    using MSTech.GestaoEscolar.Entities.Abstracts;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_TurmaAulaRecursoRegencia : AbstractCLS_TurmaAulaRecursoRegencia
	{
        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaRecursoRegencia()
        {
            DataTable dtTurmaAulaRecursoRegencia = new DataTable();
            dtTurmaAulaRecursoRegencia.Columns.Add("idAula", typeof(Int64));
            dtTurmaAulaRecursoRegencia.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaRecursoRegencia.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaRecursoRegencia.Columns.Add("tud_idFilho", typeof(Int64));
            dtTurmaAulaRecursoRegencia.Columns.Add("rsa_id", typeof(Int32));
            dtTurmaAulaRecursoRegencia.Columns.Add("trr_observacao", typeof(String));
            dtTurmaAulaRecursoRegencia.Columns.Add("trr_dataAlteracao", typeof(DateTime));
            return dtTurmaAulaRecursoRegencia;
        }
	}
}