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
    public abstract class Abstract_CLS_TurmaNotaAluno : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 tud_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tnt_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }
		[MSValidRange(20)]
		public virtual string tna_avaliacao { get; set; }
		public virtual bool tna_naoCompareceu { get; set; }
		[MSValidRange(1000)]
		public virtual string tna_comentarios { get; set; }
		public virtual string tna_relatorio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tna_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tna_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tna_dataAlteracao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool tna_participante{ get; set; }

    }
}