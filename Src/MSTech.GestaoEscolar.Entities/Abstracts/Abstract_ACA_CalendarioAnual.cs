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
    public abstract class Abstract_ACA_CalendarioAnual : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int cal_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		public virtual bool cal_padrao { get; set; }
		[MSValidRange(4)]
		[MSNotNullOrEmpty()]
		public virtual int cal_ano { get; set; }
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string cal_descricao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cal_dataInicio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cal_dataFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cal_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cal_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cal_dataAlteracao { get; set; }
        [MSNotNullOrEmpty()]
        [MSDefaultValue(false)]
        public virtual bool cal_permiteLancamentoRecesso { get; set; }
    }
}