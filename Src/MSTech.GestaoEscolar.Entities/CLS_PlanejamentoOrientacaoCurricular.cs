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
	public class CLS_PlanejamentoOrientacaoCurricular : Abstract_CLS_PlanejamentoOrientacaoCurricular
	{
        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_PlanejamentoOrientacaoCurricular.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_PlanejamentoOrientacaoCurricular.</returns>
        public static DataTable TipoTabela_PlanejamentoOrientacaoCurricular()
        {
            DataTable dtTurmaAulaAluno = new DataTable();
            dtTurmaAulaAluno.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("ocr_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("tpc_id", typeof(Int32));
            dtTurmaAulaAluno.Columns.Add("poc_planejado", typeof(Boolean));
            dtTurmaAulaAluno.Columns.Add("poc_trabalhado", typeof(Boolean));
            dtTurmaAulaAluno.Columns.Add("poc_alcancado", typeof(Boolean));
            dtTurmaAulaAluno.Columns.Add("tdt_posicao", typeof(Int16));

            return dtTurmaAulaAluno;
        }
	}
}