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
    public abstract class Abstract_ESC_EscolaOrgaoSupervisao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int eos_id { get; set; }
		[MSValidRange(200)]
		public virtual string eos_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		public virtual Guid uad_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte eos_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime eos_dataCriacao { get; set; }
		public virtual DateTime eos_dataAlteracao { get; set; }

    }
}