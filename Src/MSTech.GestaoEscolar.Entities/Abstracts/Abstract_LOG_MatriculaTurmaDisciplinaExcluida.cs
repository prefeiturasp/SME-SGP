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
    public abstract class AbstractLOG_MatriculaTurmaDisciplinaExcluida : Abstract_Entity
    {
		
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
		/// Propriedade lme_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int lme_id { get; set; }

		/// <summary>
		/// Propriedade mtd_idExcluido.
		/// </summary>
		public virtual int mtd_idExcluido { get; set; }

		/// <summary>
		/// Propriedade mtd_situacaoAnterior.
		/// </summary>
		public virtual short mtd_situacaoAnterior { get; set; }

		/// <summary>
		/// Propriedade usu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Propriedade lme_data.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime lme_data { get; set; }

		/// <summary>
		/// Propriedade lme_motivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string lme_motivo { get; set; }

    }
}