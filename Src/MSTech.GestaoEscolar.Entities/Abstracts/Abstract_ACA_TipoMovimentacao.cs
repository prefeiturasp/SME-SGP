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
    public abstract class Abstract_ACA_TipoMovimentacao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tmv_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tmv_nome { get; set; }
		[MSValidRange(10)]
		public virtual string tmv_codigo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tmv_motivo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tmv_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmv_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmv_dataAlteracao { get; set; }

    }
}