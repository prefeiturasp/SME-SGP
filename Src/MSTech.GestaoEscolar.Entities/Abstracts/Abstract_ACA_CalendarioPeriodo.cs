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
    public abstract class Abstract_ACA_CalendarioPeriodo : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cap_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string cap_descricao { get; set; }
		public virtual int tpc_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cap_dataInicio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cap_dataFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cap_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cap_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cap_dataAlteracao { get; set; }

    }
}