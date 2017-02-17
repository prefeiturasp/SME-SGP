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
    public abstract class Abstract_ACA_AvaliacaoRelacionada : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int fav_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ava_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int avr_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int ava_idRelacionada { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool avr_substituiNota { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool avr_mantemMaiorNota { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool avr_obrigatorioNotaMinima { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte avr_situacao { get; set; }

    }
}