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
    public abstract class Abstract_ACA_ObjetoAprendizagemCicloPeriodo : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade oap_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int oap_id { get; set; }

		/// <summary>
		/// Propriedade tci_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tci_id { get; set; }

		/// <summary>
		/// Propriedade tcp_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tcp_id { get; set; }

    }
}