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
    public abstract class Abstract_CLS_Questionario : Abstract_Entity
    {
		
		/// <summary>
		/// Id do questionário..
		/// </summary>
		[MSNotNullOrEmpty("[qst_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Título do questionário..
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty("[qst_titulo] é obrigatório.")]
		public virtual string qst_titulo { get; set; }

    }
}