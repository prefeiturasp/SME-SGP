/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
    using System.Data;

    /// <summary>
	/// 
	/// </summary>
    [Serializable]
    public class MTR_MatriculaTurma : Abstract_MTR_MatriculaTurma
	{
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }        
        public override string mtu_avaliacao { get; set; }
        [MSNotNullOrEmpty("Data de matrícula é obrigatório.")]
        public override DateTime mtu_dataMatricula { get; set; }
        [MSDefaultValue(1)]
        public override byte mtu_situacao { get; set; }
        public override DateTime mtu_dataCriacao { get; set; }
        public override DateTime mtu_dataAlteracao { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_AlunoMatriculaTurma.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_AlunoMatriculaTurma.</returns>
        public static DataTable TipoTabela_AlunoMatriculaTurma()
        {
            DataTable dtAlunoMatriculaTurma = new DataTable();
            dtAlunoMatriculaTurma.Columns.Add("alu_id", typeof(Int64));
            dtAlunoMatriculaTurma.Columns.Add("mtu_id", typeof(Int32));

            return dtAlunoMatriculaTurma;
        }
	}
}