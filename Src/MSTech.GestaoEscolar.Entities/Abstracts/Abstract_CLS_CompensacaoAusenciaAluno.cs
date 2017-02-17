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
    public abstract class AbstractCLS_CompensacaoAusenciaAluno : Abstract_Entity
    {
		
		/// <summary>
		/// Id da tabela TUR_TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da tabela CLS_CompensacaoAusencia.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cpa_id { get; set; }

		/// <summary>
		/// Id da tabela ACA_aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurma.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// 1-Ativo, 3-Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short caa_situacao { get; set; }

		/// <summary>
		/// Data de criação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caa_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caa_dataAlteracao { get; set; }

    }
}