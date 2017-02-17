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
    public abstract class Abstract_MTR_ParametroTipoMovimentacao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tmo_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ptm_id { get; set; }
		[MSValidRange(50)]
		[MSNotNullOrEmpty()]
		public virtual string ptm_chave { get; set; }
		[MSValidRange(150)]
		[MSNotNullOrEmpty()]
		public virtual string ptm_valor { get; set; }
		[MSValidRange(250)]
		[MSNotNullOrEmpty()]
		public virtual string ptm_descricao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte ptm_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ptm_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ptm_dataAlteracao { get; set; }

    }
}