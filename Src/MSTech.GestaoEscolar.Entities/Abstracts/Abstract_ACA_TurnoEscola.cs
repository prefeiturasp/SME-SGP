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
    public abstract class Abstract_ACA_TurnoEscola : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int trn_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tes_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tes_vigenciaInicio { get; set; }
		public virtual DateTime tes_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tes_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tes_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tes_dataAlteracao { get; set; }

    }
}