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
    public abstract class Abstract_RHU_Colaborador : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 col_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Guid pes_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime col_dataAdmissao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime col_dataDemissao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte col_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime col_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime col_dataAlteracao { get; set; }

		/// <summary>
		/// Indica se o colaborador é controlado via integração
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool col_controladoIntegracao { get; set; }

    }
}