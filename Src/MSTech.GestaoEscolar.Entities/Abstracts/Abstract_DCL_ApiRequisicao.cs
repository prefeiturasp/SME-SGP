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
    public abstract class AbstractDCL_ApiRequisicao : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade api_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int api_id { get; set; }

		/// <summary>
		/// Propriedade api_nome.
		/// </summary>
		[MSValidRange(50)]
		public virtual string api_nome { get; set; }

		/// <summary>
		/// Propriedade api_ordem.
		/// </summary>
		public virtual int api_ordem { get; set; }

		/// <summary>
		/// Propriedade req_id.
		/// </summary>
		public virtual int req_id { get; set; }

		/// <summary>
		/// Propriedade api_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime api_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade api_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime api_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade api_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short api_situacao { get; set; }

    }
}