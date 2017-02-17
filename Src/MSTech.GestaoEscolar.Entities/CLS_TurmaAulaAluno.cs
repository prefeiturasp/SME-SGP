/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    using System.Data;

    /// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class CLS_TurmaAulaAluno : AbstractCLS_TurmaAulaAluno
	{
        [MSDefaultValue(1)]
        public override byte taa_situacao { get; set; }
        public override DateTime taa_dataCriacao { get; set; }
        public override DateTime taa_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade auxiliar para a sincronização em lote das aulas do diário de classe.
        /// </summary>
        public long idAula { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_TurmaAulaAluno.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_TurmaAulaAluno.</returns>
        public static DataTable TipoTabela_TurmaAulaAluno()
        {
            DataTable dtTurmaAulaAluno = new DataTable();
            dtTurmaAulaAluno.Columns.Add("idAula", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaAluno.Columns.Add("alu_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("mtu_id", typeof(Int32));
            dtTurmaAulaAluno.Columns.Add("mtd_id", typeof(Int32));
            dtTurmaAulaAluno.Columns.Add("taa_frequencia", typeof(Int32));
            dtTurmaAulaAluno.Columns.Add("taa_anotacao", typeof(String));
            dtTurmaAulaAluno.Columns.Add("taa_situacao", typeof(Int16));
            dtTurmaAulaAluno.Columns.Add("taa_frequenciaBitMap", typeof(String));
            dtTurmaAulaAluno.Columns.Add("taa_dataAlteracao", typeof(DateTime));
            dtTurmaAulaAluno.Columns.Add("usu_idDocenteAlteracao", typeof(Guid));

            return dtTurmaAulaAluno;
        }
	}
}