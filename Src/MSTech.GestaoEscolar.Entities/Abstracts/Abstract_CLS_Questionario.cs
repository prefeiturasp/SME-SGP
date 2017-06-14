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
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Título do questionário..
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty()]
		public virtual string qst_titulo { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime qst_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime qst_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int qst_situacao { get; set; }

    }
}