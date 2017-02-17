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
    public abstract class Abstract_ORC_Habilidades : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int obj_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ctd_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int hbl_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual string hbl_descricao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte hbl_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime hbl_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime hbl_dataAlteracao { get; set; }

    }
}