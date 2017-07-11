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
    public abstract class Abstract_LOG_AlertaFimFechamento : Abstract_Entity
    {
		
		/// <summary>
		/// ID do usuário.
		/// </summary>
		[MSNotNullOrEmpty("[usu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// ID do evento de fechamento.
		/// </summary>
		[MSNotNullOrEmpty("[evt_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long evt_id { get; set; }

		/// <summary>
		/// Data de envio da notificação.
		/// </summary>
		[MSNotNullOrEmpty("[lff_dataEnvio] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual DateTime lff_dataEnvio { get; set; }

    }
}