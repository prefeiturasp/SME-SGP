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
    public abstract class Abstract_CLS_TurmaAtividadeExtraClasseRelacionada : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relacionamento entre atividades extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[taer_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid taer_id { get; set; }

		/// <summary>
		/// ID do usuário que criou as atividades extraclasse relacionadas.
		/// </summary>
		[MSNotNullOrEmpty("[usu_id] é obrigatório.")]
		public virtual Guid usu_id { get; set; }

    }
}