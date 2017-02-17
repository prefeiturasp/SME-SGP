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
    public abstract class Abstract_CLS_AlunoProjeto : Abstract_Entity
    {
		
		/// <summary>
		/// ID do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID AlunoHistoricoProjeto.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ahp_id { get; set; }

		/// <summary>
		/// Propriedade apj_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int apj_id { get; set; }

		/// <summary>
		/// ID do tipo de período do calendário.
		/// </summary>
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Nota do aluno no projeto.
		/// </summary>
		[MSValidRange(20)]
		public virtual string apj_avaliacao { get; set; }

		/// <summary>
		/// Relatório do aluno no projeto.
		/// </summary>
		public virtual string apj_relatorio { get; set; }

		/// <summary>
		/// Frequência do aluno no projeto.
		/// </summary>
		public virtual decimal apj_frequencia { get; set; }

		/// <summary>
		/// Resultado do aluno no projeto.
		/// </summary>
		public virtual byte apj_resultado { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte apj_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime apj_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime apj_dataAlteracao { get; set; }

    }
}