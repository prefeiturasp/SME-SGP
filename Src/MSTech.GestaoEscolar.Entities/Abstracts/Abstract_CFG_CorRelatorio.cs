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
    public abstract class Abstract_CFG_CorRelatorio : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela CFG_Relatorio.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rlt_id { get; set; }

		/// <summary>
		/// Campo Id da tabela CFG_CorRelatorio.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cor_id { get; set; }

		/// <summary>
		/// HEX da cor..
		/// </summary>
		[MSValidRange(10)]
		[MSNotNullOrEmpty]
		public virtual string cor_corPaleta { get; set; }

		/// <summary>
		/// Ordem de uilização da cor..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short cor_ordem { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short cor_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cor_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cor_dataAlteracao { get; set; }

    }
}