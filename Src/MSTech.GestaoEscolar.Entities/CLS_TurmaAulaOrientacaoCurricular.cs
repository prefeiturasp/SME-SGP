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
    public class CLS_TurmaAulaOrientacaoCurricular : Abstract_CLS_TurmaAulaOrientacaoCurricular
	{
        public virtual long tud_idRegencia { get; set; }

        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaOrientacaoCurricular()
        {
            DataTable dtTurmaAulaOrientacaoCurricular = new DataTable();
            DataColumn dcId = dtTurmaAulaOrientacaoCurricular.Columns.Add("idAula", typeof(Int64));
            dcId.AutoIncrement = true;
            dcId.AutoIncrementSeed = 1;
            dcId.AutoIncrementStep = 1;

            dtTurmaAulaOrientacaoCurricular.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaOrientacaoCurricular.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaOrientacaoCurricular.Columns.Add("ocr_id", typeof(Int64));
            dtTurmaAulaOrientacaoCurricular.Columns.Add("tao_pranejado", typeof(Int32));
            dtTurmaAulaOrientacaoCurricular.Columns.Add("tao_trabalhado", typeof(Int32));
            dtTurmaAulaOrientacaoCurricular.Columns.Add("tao_alcancado", typeof(Int32));

            return dtTurmaAulaOrientacaoCurricular;
        }
	}
}