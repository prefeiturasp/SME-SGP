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
    public abstract class Abstract_ESC_UnidadeEscolaPredio : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int prd_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uep_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool uep_principal { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uep_vigenciaInicio { get; set; }
		public virtual DateTime uep_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte uep_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uep_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uep_dataAlteracao { get; set; }

    }
}