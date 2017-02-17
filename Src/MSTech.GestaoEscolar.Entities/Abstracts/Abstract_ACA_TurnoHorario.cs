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
    public abstract class Abstract_ACA_TurnoHorario : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int trn_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int trh_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte trh_diaSemana { get; set; }
		[MSNotNullOrEmpty()]
		public virtual TimeSpan trh_horaInicio { get; set; }
		[MSNotNullOrEmpty()]
        public virtual TimeSpan trh_horaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte trh_tipo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte trh_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime trh_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime trh_dataAlteracao { get; set; }

    }
}