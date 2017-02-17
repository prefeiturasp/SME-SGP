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
    public abstract class Abstract_MTR_DocumentoMatricula : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int dmt_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid tdo_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte dmt_obrigatoriedade { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte dmt_apresentacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime dmt_vigenciaInicio { get; set; }
		public virtual DateTime dmt_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte dmt_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime dmt_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime dmt_dataAlteracao { get; set; }

    }
}