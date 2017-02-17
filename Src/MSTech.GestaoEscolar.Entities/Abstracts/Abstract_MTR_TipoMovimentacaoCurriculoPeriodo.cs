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
    public abstract class Abstract_MTR_TipoMovimentacaoCurriculoPeriodo : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tmo_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tmp_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int crr_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int crp_id { get; set; }
        public virtual int cur_idDestino { get; set; }
        public virtual int crr_idDestino { get; set; }
        public virtual int crp_idDestino { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tmp_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmp_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tmp_dataAlteracao { get; set; }

    }
}