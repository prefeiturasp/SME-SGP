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
    public abstract class Abstract_ESC_UnidadeEscolaEnsino : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int uee_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tne_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tme_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uee_vigenciaInicio { get; set; }
		public virtual DateTime uee_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte uee_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uee_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime uee_dataAlteracao { get; set; }

    }
}