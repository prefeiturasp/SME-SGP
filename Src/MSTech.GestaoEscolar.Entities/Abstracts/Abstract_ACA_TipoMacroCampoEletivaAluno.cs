/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ACA_TipoMacroCampoEletivaAluno : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tea_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tea_nome { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(10)]
		[MSNotNullOrEmpty()]
		public virtual string tea_sigla { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tea_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tea_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tea_dataAlteracao { get; set; }

    }
}