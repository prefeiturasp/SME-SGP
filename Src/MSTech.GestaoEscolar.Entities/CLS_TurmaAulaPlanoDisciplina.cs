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
	public class CLS_TurmaAulaPlanoDisciplina : Abstract_CLS_TurmaAulaPlanoDisciplina
    {

        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaPlanoDisciplina()
        {
            DataTable dtTurmaAulaPlanoDisciplina = new DataTable();
            DataColumn dcId = dtTurmaAulaPlanoDisciplina.Columns.Add("idAula", typeof(Int64));
            dcId.AutoIncrement = true;
            dcId.AutoIncrementSeed = 1;
            dcId.AutoIncrementStep = 1;

            dtTurmaAulaPlanoDisciplina.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaPlanoDisciplina.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaPlanoDisciplina.Columns.Add("tud_idPlano", typeof(Int64));
            return dtTurmaAulaPlanoDisciplina;
        }

	}
}