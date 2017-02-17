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
    public abstract class Abstract_MTR_MomentoCalendarioPeriodo : Abstract_Entity
    {
		[MSNotNullOrEmpty("Ano é obrigatório")]
		[DataObjectField(true, false, false)]
		public virtual int mom_ano { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mom_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cap_id { get; set; }
		[MSNotNullOrEmpty("Data inicial é obrigatória")]
		public virtual DateTime mcp_inicio { get; set; }
		[MSNotNullOrEmpty("Data final é obrigatória")]
		public virtual DateTime mcp_fim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte mcp_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mcp_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mcp_dataAlteracao { get; set; }

    }
}