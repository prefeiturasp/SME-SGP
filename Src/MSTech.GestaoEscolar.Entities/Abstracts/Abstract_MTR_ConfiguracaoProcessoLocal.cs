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
    public abstract class Abstract_MTR_ConfiguracaoProcessoLocal : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cfg_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cpr_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cpl_id { get; set; }
		[MSValidRange(10)]
		[MSNotNullOrEmpty()]
		public virtual string cpl_numero { get; set; }
		[MSValidRange(100)]
		public virtual string cpl_complemento { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cpl_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid end_id { get; set; }

    }
}