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
    public abstract class Abstract_CFG_AlertaGrupo : Abstract_Entity
    {
		
		/// <summary>
		/// ID do alerta.
		/// </summary>
		[MSNotNullOrEmpty("[cfa_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual short cfa_id { get; set; }

		/// <summary>
		/// ID do grupo de usuário do Core.
		/// </summary>
		[MSNotNullOrEmpty("[gru_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid gru_id { get; set; }

    }
}