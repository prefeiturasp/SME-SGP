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
    public abstract class Abstract_ACA_TipoPeriodoCalendario : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tpc_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tpc_nome { get; set; }
        public virtual string tpc_nomeAbreviado { get; set; }
        [MSNotNullOrEmpty()]
		public virtual int tpc_ordem { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool tpc_foraPeriodoLetivo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tpc_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tpc_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tpc_dataAlteracao { get; set; }

    }
}