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
    public abstract class Abstract_CLS_TurmaAulaRecurso : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 tud_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tau_id { get; set; }
		[DataObjectField(true, false, true)]
		public virtual int tar_id { get; set; }
		public virtual int rsa_id { get; set; }
		public virtual string tar_observacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tar_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tar_dataAlteracao { get; set; }

    }
}