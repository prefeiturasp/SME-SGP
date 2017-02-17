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
    public abstract class Abstract_DCL_ProtocoloHistorico : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade pro_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid pro_id { get; set; }

		/// <summary>
		/// Propriedade prh_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int prh_id { get; set; }

		/// <summary>
		/// Propriedade pro_status.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pro_status { get; set; }

		/// <summary>
		/// Propriedade pro_statusObservacao.
		/// </summary>
		public virtual string pro_statusObservacao { get; set; }

		/// <summary>
		/// Propriedade tur_id.
		/// </summary>
		public virtual long tur_id { get; set; }

		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade tau_id.
		/// </summary>
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Propriedade prh_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime prh_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade prh_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime prh_dataAlteracao { get; set; }

    }
}