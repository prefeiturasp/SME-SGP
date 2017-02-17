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
    public abstract class Abstract_ACA_Curriculo : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }
		public virtual int crr_idBasico { get; set; }
		[MSValidRange(10)]
		public virtual string crr_codigo { get; set; }
		[MSValidRange(200)]
		public virtual string crr_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte crr_regimeMatricula { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte crr_periodosNormal { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int crr_diasLetivos { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime crr_vigenciaInicio { get; set; }
		public virtual DateTime crr_vigenciaFim { get; set; }
		public virtual int crr_qtdeAvaliacaoProgressao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte crr_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime crr_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime crr_dataAlteracao { get; set; }

    }
}