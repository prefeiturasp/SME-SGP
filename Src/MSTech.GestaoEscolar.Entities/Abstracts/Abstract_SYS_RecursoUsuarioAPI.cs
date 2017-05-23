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
    public abstract class Abstract_SYS_RecursoUsuarioAPI : Abstract_Entity
    {
		
		/// <summary>
		/// ID do recurso externo (Web API).
		/// </summary>
		[MSNotNullOrEmpty("[rap_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int rap_id { get; set; }

		/// <summary>
		/// ID do usuário API.
		/// </summary>
		[MSNotNullOrEmpty("[uap_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int uap_id { get; set; }

    }
}