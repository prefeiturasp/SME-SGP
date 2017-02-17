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
    public abstract class AbstractCLS_CompensacaoAusencia : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade cpa_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cpa_id { get; set; }

		/// <summary>
		/// Propriedade tpc_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Propriedade cpa_quantidadeAulasCompensadas.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cpa_quantidadeAulasCompensadas { get; set; }

		/// <summary>
		/// Propriedade cpa_atividadesDesenvolvidas.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string cpa_atividadesDesenvolvidas { get; set; }

		/// <summary>
		/// Propriedade cpa_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short cpa_situacao { get; set; }

		/// <summary>
		/// Propriedade cpa_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cpa_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade cpa_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cpa_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade pro_id.
		/// </summary>
		public virtual Guid pro_id { get; set; }

    }
}