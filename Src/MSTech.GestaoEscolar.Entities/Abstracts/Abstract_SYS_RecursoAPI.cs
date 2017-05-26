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
    public abstract class Abstract_SYS_RecursoAPI : Abstract_Entity
    {
		
		/// <summary>
		/// ID do recurso externo (Web API).
		/// </summary>
		[MSNotNullOrEmpty("[rap_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int rap_id { get; set; }

		/// <summary>
		/// Descrição do recurso externo (Web API).
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[rap_descricao] é obrigatório.")]
		public virtual string rap_descricao { get; set; }

		/// <summary>
		/// URL do recurso externo (Web API).
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[rap_url] é obrigatório.")]
		public virtual string rap_url { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rap_situacao] é obrigatório.")]
		public virtual byte rap_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rap_dataCriacao] é obrigatório.")]
		public virtual DateTime rap_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rap_dataAlteracao] é obrigatório.")]
		public virtual DateTime rap_dataAlteracao { get; set; }

    }
}