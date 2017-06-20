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
    public abstract class Abstract_CFG_DeficienciaFIlha : Abstract_Entity
    {
		
		/// <summary>
		/// Id da deficiência múltipla, que terá deficiências dependentes.
		/// </summary>
		[MSNotNullOrEmpty("[tde_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid tde_id { get; set; }

		/// <summary>
		/// ID das deficiências filhas.
		/// </summary>
		[MSNotNullOrEmpty("[tde_idFilha] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid tde_idFilha { get; set; }

    }
}