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
    public abstract class Abstract_ACA_TipoDisciplinaRelacionada : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_TipoDisciplina da recuperação paralela.
		/// </summary>
		[MSNotNullOrEmpty("Tipo de disciplina é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoDisciplina da disciplina relacionada com a recuperação paralela.
		/// </summary>
		[MSNotNullOrEmpty("Tipo de disciplina relacionada é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tds_idRelacionada { get; set; }

    }
}