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
    public abstract class Abstract_ACA_CoordenadorDisciplina : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int cdd_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Int64 doc_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tds_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cdd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cdd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cdd_dataAlteracao { get; set; }

    }
}