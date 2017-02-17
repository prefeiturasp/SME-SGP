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
    public abstract class Abstract_ACA_ParametroAcademico : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int pac_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string pac_chave { get; set; }
		[MSValidRange(1000)]
		[MSNotNullOrEmpty()]
		public virtual string pac_valor { get; set; }
		[MSValidRange(200)]
		public virtual string pac_descricao { get; set; }
		public virtual bool pac_obrigatorio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte pac_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime pac_vigenciaInicio { get; set; }
		public virtual DateTime pac_vigenciaFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime pac_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime pac_dataAlteracao { get; set; }
        public virtual Guid ent_id { get; set; }

    }
}