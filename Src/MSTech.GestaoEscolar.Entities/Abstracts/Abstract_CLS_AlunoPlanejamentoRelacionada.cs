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
    public abstract class Abstract_CLS_AlunoPlanejamentoRelacionada : Abstract_Entity
    {
		
		/// <summary>
		/// ID do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID do planejamento do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int apl_id { get; set; }

		/// <summary>
		/// ID da turma disciplina relacionada.
		/// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual long tud_idRelacionado { get; set; }

    }
}