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
    public abstract class Abstract_MTR_Configuracao : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int cfg_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string cfg_nome { get; set; }
		public virtual DateTime cfg_dataBaseAluno { get; set; }
		public virtual bool cfg_consideraTurno { get; set; }
		public virtual byte cfg_entregaDoc { get; set; }
		public virtual int cfg_prazoEntregaDoc { get; set; }
		public virtual byte cfg_responsavelVaga { get; set; }
		public virtual bool cfg_todasEscolas { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cfg_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cfg_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cfg_dataAlteracao { get; set; }
		public virtual Int64 evt_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }

    }
}