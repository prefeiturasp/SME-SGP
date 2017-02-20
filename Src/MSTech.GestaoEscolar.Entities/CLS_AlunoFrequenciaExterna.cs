/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_AlunoFrequenciaExterna : Abstract_CLS_AlunoFrequenciaExterna
	{
        /// <summary>
		/// ID da tabela ACA_Aluno.
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// ID da tabela MTR_MatriculaTurma.
        /// </summary>
        [MSNotNullOrEmpty("[mtu_id] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }

        /// <summary>
        /// ID da tabela MTR_MatriculaTurmaDisciplina.
        /// </summary>
        [MSNotNullOrEmpty("[mtd_id] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtd_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_TipoPeriodoCalendario.
        /// </summary>
        [MSNotNullOrEmpty("[tpc_id] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tpc_id { get; set; }

        /// <summary>
        /// ID da tabela CLS_AlunoFrequenciaExterna.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int afx_id { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte afx_situacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime afx_dataAlteracao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime afx_dataCriacao { get; set; }
    }
}