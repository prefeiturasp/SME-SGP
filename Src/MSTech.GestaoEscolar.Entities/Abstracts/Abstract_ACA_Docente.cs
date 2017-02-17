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
    public abstract class Abstract_ACA_Docente : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 doc_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Int64 col_id { get; set; }
		[MSValidRange(20)]
		public virtual string doc_codigoInep { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte doc_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime doc_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime doc_dataAlteracao { get; set; }

    }
}