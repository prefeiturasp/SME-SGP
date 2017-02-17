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
    public abstract class Abstract_ESC_EscolaVinculada : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esv_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int esc_idVinculada { get; set; }
		[MSValidRange(1000)]
		public virtual string esv_observacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esv_vigenciaInicio { get; set; }
		public virtual DateTime esv_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte esv_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esv_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime esv_dataAlteracao { get; set; }

    }
}