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
    public abstract class AbstractCFG_HistoricoConceito : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade chc_ano.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int chc_ano { get; set; }

		/// <summary>
		/// Propriedade chc_conceitos.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string chc_conceitos { get; set; }

    }
}