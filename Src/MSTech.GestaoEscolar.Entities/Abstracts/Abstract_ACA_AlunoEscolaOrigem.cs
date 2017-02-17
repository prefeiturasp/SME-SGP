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
    public abstract class Abstract_ACA_AlunoEscolaOrigem : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 eco_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int tre_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string eco_nome { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(20)]
		public virtual string eco_codigoInep { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Guid end_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(10)]
		public virtual string eco_numero { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(100)]
		public virtual string eco_complemento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte eco_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime eco_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime eco_dataAlteracao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Guid cid_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Guid unf_id { get; set; }

    }
}