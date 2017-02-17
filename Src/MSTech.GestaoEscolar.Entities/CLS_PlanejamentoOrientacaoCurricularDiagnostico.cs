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
	public class CLS_PlanejamentoOrientacaoCurricularDiagnostico : Abstract_CLS_PlanejamentoOrientacaoCurricularDiagnostico
	{
        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_PlanejamentoOrientacaoCurricularDiagnostico.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_PlanejamentoOrientacaoCurricularDiagnostico.</returns>
        public static DataTable TipoTabela_PlanejamentoOrientacaoCurricularDiagnostico()
        {
            DataTable dtTurmaAulaAluno = new DataTable();
            dtTurmaAulaAluno.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("ocr_id", typeof(Int64));
            dtTurmaAulaAluno.Columns.Add("pod_alcancado", typeof(Boolean));
            dtTurmaAulaAluno.Columns.Add("tdt_posicao", typeof(Int16));

            return dtTurmaAulaAluno;
        }
	}
}