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
    public abstract class Abstract_ACA_Evento : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 evt_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tev_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		public virtual int esc_id { get; set; }
		public virtual int uni_id { get; set; }
		public virtual bool evt_padrao { get; set; }
		public virtual int tpc_id { get; set; }
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string evt_nome { get; set; }
		public virtual string evt_descricao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime evt_dataInicio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime evt_dataFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool evt_semAtividadeDiscente { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte evt_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime evt_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime evt_dataAlteracao { get; set; }
        public virtual bool evt_limitarDocente { get; set; }
    }
}