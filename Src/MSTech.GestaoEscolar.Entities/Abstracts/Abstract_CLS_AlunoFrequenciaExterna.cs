/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_AlunoFrequenciaExterna : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Aluno.
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurma.
		/// </summary>
		[MSNotNullOrEmpty("[mtu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty("[mtd_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoPeriodoCalendario.
		/// </summary>
		[MSNotNullOrEmpty("[tpc_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// ID da tabela CLS_AlunoFrequenciaExterna.
		/// </summary>
		[MSNotNullOrEmpty("[afx_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int afx_id { get; set; }

		/// <summary>
		/// Quantidade de aulas no período.
		/// </summary>
		public virtual int afx_qtdAulas { get; set; }

		/// <summary>
		/// Quantidade de faltas no período.
		/// </summary>
		public virtual int afx_qtdFaltas { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty("[afx_situacao] é obrigatório.")]
		public virtual byte afx_situacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[afx_dataAlteracao] é obrigatório.")]
		public virtual DateTime afx_dataAlteracao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[afx_dataCriacao] é obrigatório.")]
		public virtual DateTime afx_dataCriacao { get; set; }

    }
}