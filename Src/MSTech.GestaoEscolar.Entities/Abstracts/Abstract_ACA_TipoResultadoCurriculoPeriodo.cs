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
    public abstract class AbstractACA_TipoResultadoCurriculoPeriodo : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tpr_id.
		/// </summary>
		[MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual int tpr_id { get; set; }

		/// <summary>
		/// Propriedade cur_id.
		/// </summary>
		[MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Propriedade crr_id.
		/// </summary>
		[MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Propriedade crp_id.
		/// </summary>
		[MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual int crp_id { get; set; }

    }
}