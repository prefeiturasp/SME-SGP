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
    public abstract class Abstract_SYS_ServicosLogExecucao : Abstract_Entity
    {
		
		/// <summary>
		/// Id do log de execução do serviço.
		/// </summary>
		[MSNotNullOrEmpty("[sle_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual Guid sle_id { get; set; }

		/// <summary>
		/// Id do serviço.
		/// </summary>
		[MSNotNullOrEmpty("[ser_id] é obrigatório.")]
		public virtual short ser_id { get; set; }

		/// <summary>
		/// Data de início da execução do serviço.
		/// </summary>
		[MSNotNullOrEmpty("[sle_dataInicioExecucao] é obrigatório.")]
		public virtual DateTime sle_dataInicioExecucao { get; set; }

		/// <summary>
		/// Data de fim da execução do serviço.
		/// </summary>
		public virtual DateTime sle_dataFimExecucao { get; set; }

    }
}