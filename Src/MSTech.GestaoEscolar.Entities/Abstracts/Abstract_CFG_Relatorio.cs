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
    public abstract class Abstract_CFG_Relatorio : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade rlt_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rlt_id { get; set; }

		/// <summary>
		/// Propriedade rlt_nome.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string rlt_nome { get; set; }

		/// <summary>
		/// Propriedade rlt_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rlt_situacao { get; set; }

		/// <summary>
		/// Propriedade rlt_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rlt_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade rlt_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rlt_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade rlt_integridade.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int rlt_integridade { get; set; }

    }
}