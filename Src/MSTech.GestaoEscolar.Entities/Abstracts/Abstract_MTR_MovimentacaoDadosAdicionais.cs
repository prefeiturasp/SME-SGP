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
    public abstract class Abstract_MTR_MovimentacaoDadosAdicionais : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mov_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mda_id { get; set; }
		public virtual Int64 eco_id { get; set; }
		public virtual Guid cid_id { get; set; }
		public virtual Guid unf_id { get; set; }
		public virtual string mda_avaliacao { get; set; }
		public virtual string mda_observacao { get; set; }
		public virtual int hop_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte mda_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mda_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mda_dataAlteracao { get; set; }

    }
}