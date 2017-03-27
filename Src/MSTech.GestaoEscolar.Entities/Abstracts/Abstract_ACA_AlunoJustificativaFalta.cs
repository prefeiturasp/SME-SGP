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
    public abstract class Abstract_ACA_AlunoJustificativaFalta : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int afj_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tjf_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime afj_dataInicio { get; set; }
		public virtual DateTime afj_dataFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte afj_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime afj_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime afj_dataAlteracao { get; set; }
        public virtual Guid pro_id { get; set; }

    }
}