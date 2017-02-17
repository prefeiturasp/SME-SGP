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
    public abstract class Abstract_TUR_MetodoAvaliacao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int mav_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		public virtual int esc_id { get; set; }
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool mav_padrao { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string mav_nome { get; set; }
		[MSValidRange(200)]
		public virtual string mav_formula { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte mav_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mav_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mav_dataAlteracao { get; set; }

    }
}