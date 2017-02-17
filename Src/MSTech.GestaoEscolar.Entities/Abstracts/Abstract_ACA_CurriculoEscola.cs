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
    public abstract class Abstract_ACA_CurriculoEscola : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ces_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ces_vigenciaInicio { get; set; }
		public virtual DateTime ces_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte ces_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ces_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ces_dataAlteracao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual int vis_id { get; set; }
    }
}