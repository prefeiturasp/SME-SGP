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
    public abstract class Abstract_ESC_UnidadeEscolaContato : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uec_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid tmc_id { get; set; }
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string uec_contato { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte uec_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uec_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uec_dataAlteracao { get; set; }

    }
}