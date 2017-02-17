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
    public abstract class Abstract_ACA_AlunoCurriculoDocumento : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int alc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int dmt_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte acd_situacao { get; set; }
		[MSValidRange(2000)]
		public virtual string acd_observacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime acd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime acd_dataAlteracao { get; set; }

    }
}