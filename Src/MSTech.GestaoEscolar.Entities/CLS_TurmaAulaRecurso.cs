/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class CLS_TurmaAulaRecurso : Abstract_CLS_TurmaAulaRecurso
	{
        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaRecurso()
        {
            DataTable dtTurmaAulaRecurso = new DataTable();
            dtTurmaAulaRecurso.Columns.Add("idAula", typeof(Int64));
            dtTurmaAulaRecurso.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaRecurso.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaRecurso.Columns.Add("rsa_id", typeof(Int32));
            dtTurmaAulaRecurso.Columns.Add("tar_observacao", typeof(String));
            dtTurmaAulaRecurso.Columns.Add("tar_dataAlteracao", typeof(DateTime));
            return dtTurmaAulaRecurso;
        }
	}
}