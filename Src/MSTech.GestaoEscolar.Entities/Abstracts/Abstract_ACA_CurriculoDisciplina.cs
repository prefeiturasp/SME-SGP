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
    public abstract class Abstract_ACA_CurriculoDisciplina : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crp_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int dis_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte crd_tipo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte crd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime crd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime crd_dataAlteracao { get; set; }

    }
}