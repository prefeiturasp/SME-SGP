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
    public abstract class Abstract_ACA_ParametroBuscaAluno : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int pba_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte pba_tipo { get; set; }
		public virtual Guid tdo_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool pba_integridade { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte pba_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime pba_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime pba_dataAlteracao { get; set; }

    }
}