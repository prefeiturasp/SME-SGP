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
    public abstract class Abstract_MTR_TipoMovimentacao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tmo_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tmo_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tmo_tipoMovimento { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool tmo_todosMomentos { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tmo_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmo_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmo_dataAlteracao { get; set; }
		public virtual int tmv_idEntrada { get; set; }
		public virtual int tmv_idSaida { get; set; }

    }
}