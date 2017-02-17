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
	public class CLS_TurmaAulaDisciplinaRelacionada : AbstractCLS_TurmaAulaDisciplinaRelacionada
	{
        /// <summary>
        /// Propriedade auxiliar para a sincronização em lote das aulas do diário de classe.
        /// </summary>
        public long idAula { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_TurmaAulaDisciplinaRelacionada.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_TurmaAulaDisciplinaRelacionada.</returns>
        public static DataTable TipoTabela_TurmaAulaDisciplinaRelacionada()
        {
            DataTable dtTurmaAulaDisciplinaRelacionada = new DataTable();
            dtTurmaAulaDisciplinaRelacionada.Columns.Add("idAula", typeof(Int64));
            dtTurmaAulaDisciplinaRelacionada.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaDisciplinaRelacionada.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaDisciplinaRelacionada.Columns.Add("tdr_id", typeof(Int64));
            dtTurmaAulaDisciplinaRelacionada.Columns.Add("tud_idRelacionada", typeof(Int64));

            return dtTurmaAulaDisciplinaRelacionada;
        }

        /// <summary>
        /// O método que converte o registro da CLS_TurmaAulaDisciplinaRelacionada em um DataRow.
        /// </summary>
        /// <param name="alunoAvaliacaoTurmaDisciplinaMedia">Registro da CLS_TurmaAulaDisciplinaRelacionada.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        public static DataRow TurmaAulaDisciplinaRelacionadaToDataRow(CLS_TurmaAulaDisciplinaRelacionada turmaAulaDisciplinaRelacionada, DataRow dr)
        {
            dr["idAula"] = turmaAulaDisciplinaRelacionada.idAula;
            dr["tud_id"] = turmaAulaDisciplinaRelacionada.tud_id;
            dr["tau_id"] = turmaAulaDisciplinaRelacionada.tau_id;
            dr["tdr_id"] = turmaAulaDisciplinaRelacionada.tdr_id;
            dr["tud_idRelacionada"] = turmaAulaDisciplinaRelacionada.tud_idRelacionada;

            return dr;
        }	
	}
}