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
    public abstract class Abstract_RHU_TipoVinculo : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tvi_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tvi_nome { get; set; }
		public virtual string tvi_descricao { get; set; }
		public virtual int tvi_horasSemanais { get; set; }
		public virtual int tvi_minutosAlmoco { get; set; }
        public virtual TimeSpan tvi_horarioMinEntrada { get; set; }
        public virtual TimeSpan tvi_horarioMaxSaida { get; set; }
		[MSValidRange(20)]
		public virtual string tvi_codIntegracao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tvi_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tvi_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tvi_dataAlteracao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }

    }
}