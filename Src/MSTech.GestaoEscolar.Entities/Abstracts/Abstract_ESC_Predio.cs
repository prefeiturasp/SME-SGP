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
    public abstract class Abstract_ESC_Predio : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int prd_id { get; set; }
		[MSValidRange(1000)]
		public virtual string prd_descricao { get; set; }
		public virtual bool prd_adaptado_especial { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte prd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime prd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime prd_dataAlteracao { get; set; }

    }
}