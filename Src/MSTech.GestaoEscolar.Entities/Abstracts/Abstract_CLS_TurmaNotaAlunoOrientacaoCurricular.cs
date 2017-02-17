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
    public abstract class AbstractCLS_TurmaNotaAlunoOrientacaoCurricular : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela TUR_TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Campo Id da tabela CLS_TurmaNotaAluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tnt_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_Aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Campo Id da tabela MTR_MatriculaTurma.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// Campo Id da tabela MTR_MatriculaTurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// ID da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long ocr_id { get; set; }

		/// <summary>
		/// ID da TurmaNotaAlunoOrientacaoCurricular.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int aoc_id { get; set; }

		/// <summary>
		/// Se a orientação curricular foi alcançada ou não.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool aoc_alcancado { get; set; }

		/// <summary>
		/// Situação do registro: 1 - Ativo; 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aoc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aoc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aoc_dataAlteracao { get; set; }

    }
}