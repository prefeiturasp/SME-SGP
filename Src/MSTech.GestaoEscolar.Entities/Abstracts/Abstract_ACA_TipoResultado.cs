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
    public abstract class AbstractACA_TipoResultado : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tpr_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int tpr_id { get; set; }

		/// <summary>
		/// Propriedade tpr_resultado.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tpr_resultado { get; set; }

		/// <summary>
		/// Propriedade tpr_nomenclatura.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string tpr_nomenclatura { get; set; }

		/// <summary>
		/// Propriedade tpr_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tpr_situacao { get; set; }

		/// <summary>
		/// Propriedade tpr_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tpr_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tpr_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tpr_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade tpr_tipoLancamento.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual short tpr_tipoLancamento { get; set; }

        /// <summary>
        /// Propriedade tds_id (tipo de disciplina).
        /// </summary>
        public virtual int tds_id { get; set; }
    }
}