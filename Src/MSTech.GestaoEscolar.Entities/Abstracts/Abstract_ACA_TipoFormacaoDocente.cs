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
    public abstract class Abstract_ACA_TipoFormacaoDocente : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tfd_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tfd_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tfd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tfd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tfd_dataAlteracao { get; set; }

    }
}