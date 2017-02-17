/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    using System.Data;

    /// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class CLS_TurmaNotaAluno : Abstract_CLS_TurmaNotaAluno
	{ 
        [MSValidRange(20, "Avaliação pode conter até 20 caracteres.")]
        public override string tna_avaliacao { get; set; }
        [MSValidRange(1000, "Comentários pode conter até 1000 caracteres.")]
        public override string tna_comentarios { get; set; }
        [MSDefaultValue(1)]
        public override byte tna_situacao { get; set; }
        public override DateTime tna_dataCriacao { get; set; }
        public override DateTime tna_dataAlteracao { get; set; }
        [MSDefaultValue(1)]
        public override bool tna_participante { get; set; }
        [MSDefaultValue(0)]
        public override bool tna_naoCompareceu { get; set; }

        public long idAtividade { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_TurmaNotaAluno.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_TurmaNotaAluno.</returns>
        public static DataTable TipoTabela_TurmaNotaAluno()
        {
            DataTable dtTurmaNotaAluno = new DataTable();
            dtTurmaNotaAluno.Columns.Add("idAtividade", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("tud_id", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("tnt_id", typeof(Int32));
            dtTurmaNotaAluno.Columns.Add("alu_id", typeof(Int64));
            dtTurmaNotaAluno.Columns.Add("mtu_id", typeof(Int32));
            dtTurmaNotaAluno.Columns.Add("mtd_id", typeof(Int32));
            dtTurmaNotaAluno.Columns.Add("tna_avaliacao", typeof(String));
            dtTurmaNotaAluno.Columns.Add("tna_naoCompareceu", typeof(Boolean));
            dtTurmaNotaAluno.Columns.Add("tna_comentarios", typeof(String));
            dtTurmaNotaAluno.Columns.Add("tna_relatorio", typeof(String));
            dtTurmaNotaAluno.Columns.Add("tna_situacao", typeof(Int16));
            dtTurmaNotaAluno.Columns.Add("tna_participante", typeof(Boolean));
            dtTurmaNotaAluno.Columns.Add("tna_dataAlteracao", typeof(DateTime));

            return dtTurmaNotaAluno;
        }
	}
}