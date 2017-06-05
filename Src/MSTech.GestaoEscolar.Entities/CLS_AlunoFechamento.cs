/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.Data;
		
	/// <summary>
	/// Description: Dados pré-calculados do fechamento..
	/// </summary>
	public class CLS_AlunoFechamento : Abstract_CLS_AlunoFechamento
	{
        public int tpc_ordem { get; set; }

        public static DataTable TipoTabela_AlunoFechamento()
        {
            DataTable dtAlunoFechamento = new DataTable();
            dtAlunoFechamento.Columns.Add("tud_id", typeof(Int64));
            dtAlunoFechamento.Columns.Add("tpc_id", typeof(Int32));
            dtAlunoFechamento.Columns.Add("alu_id", typeof(Int64));
            dtAlunoFechamento.Columns.Add("mtu_id", typeof(Int32));
            dtAlunoFechamento.Columns.Add("mtd_id", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtFaltas", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtAulas", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtFaltasReposicao", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtAulasReposicao", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtAusenciasCompensadas", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_frequencia", typeof(Decimal));
            dtAlunoFechamento.Columns.Add("caf_frequenciaFinalAjustada", typeof(Decimal));
            dtAlunoFechamento.Columns.Add("caf_avaliacao", typeof(String));
            dtAlunoFechamento.Columns.Add("caf_efetivado", typeof(Boolean));
            dtAlunoFechamento.Columns.Add("caf_dataAlteracao", typeof(DateTime));
            dtAlunoFechamento.Columns.Add("caf_qtFaltasExterna", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtAulasExterna", typeof(Int32));
            dtAlunoFechamento.Columns.Add("caf_qtAtividadeExtraclasse", typeof(Int32));

            return dtAlunoFechamento;
        }
	}
}