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
    public abstract class Abstract_TUR_TipoAtendimentoTurma : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tat_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tat_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tat_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tat_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tat_dataAlteracao { get; set; }

    }
}