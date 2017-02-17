/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_TUR_TurmaDisciplinaMultisseriada : Abstract_Entity
    {
		
		/// <summary>
		/// ID da disciplina relacionado ao docente..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_idDocente { get; set; }

		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Propriedade mtu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// Propriedade mtd_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

    }
}