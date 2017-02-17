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
    public abstract class Abstract_ESC_EscolaDiretor : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esd_id { get; set; }
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Int64 col_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esd_vigenciaInicio { get; set; }
		public virtual DateTime esd_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool esd_geralEscola { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte esd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esd_dataAlteracao { get; set; }

    }
}