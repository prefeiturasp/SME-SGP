using MSTech.GestaoEscolar.Entities.Abstracts;
using System;
using System.Data;

namespace MSTech.GestaoEscolar.Entities
{
		
		
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_TurmaAulaAlunoTipoAnotacao : Abstract_CLS_TurmaAulaAlunoTipoAnotacao
	{
        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaAlunoTipoAnotacao()
        {
            DataTable dtTurmaAulaAlunoTipoAnotacao = new DataTable();
            DataColumn dcId = dtTurmaAulaAlunoTipoAnotacao.Columns.Add("idAula", typeof(Int64));
            dcId.AutoIncrement = true;
            dcId.AutoIncrementSeed = 1;
            dcId.AutoIncrementStep = 1;

            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("alu_id", typeof(Int64));
            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("mtu_id", typeof(Int32));
            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("mtd_id", typeof(Int32));
            dtTurmaAulaAlunoTipoAnotacao.Columns.Add("tia_id", typeof(Int32));

            return dtTurmaAulaAlunoTipoAnotacao;
        }
	}
}