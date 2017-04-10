/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.Data;
		
	/// <summary>
	/// Description: Fila para o pré-processamento dos cálculos do fechamento..
	/// </summary>
    [Serializable]
	public class CLS_AlunoFechamentoPendencia : Abstract_CLS_AlunoFechamentoPendencia
	{
        public int tpc_ordem { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_AlunoFechamentoPendencia.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_AlunoFechamentoPendencia.</returns>
        public static DataTable TipoTabela_AlunoFechamentoPendencia()
        {
            DataTable dtAlunoFechamentoPendencia = new DataTable();
            dtAlunoFechamentoPendencia.Columns.Add("tud_id", typeof(Int64));
            dtAlunoFechamentoPendencia.Columns.Add("tpc_id", typeof(Int32));
            dtAlunoFechamentoPendencia.Columns.Add("afp_frequencia", typeof(Boolean));
            dtAlunoFechamentoPendencia.Columns.Add("afp_nota", typeof(Boolean));
            dtAlunoFechamentoPendencia.Columns.Add("afp_frequenciaExterna", typeof(Boolean));
            dtAlunoFechamentoPendencia.Columns.Add("afp_processado", typeof(Byte));
            dtAlunoFechamentoPendencia.Columns.Add("afp_processoServico", typeof(Guid));            
            return dtAlunoFechamentoPendencia;
        }
	}
}