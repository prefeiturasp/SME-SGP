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
    public abstract class AbstractCFG_RelatorioDocumentoDocente : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade rdd_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int rdd_id { get; set; }

		/// <summary>
		/// Propriedade ent_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Propriedade rlt_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rlt_id { get; set; }

		/// <summary>
		/// Propriedade rdd_nomeDocumento.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string rdd_nomeDocumento { get; set; }

		/// <summary>
		/// Propriedade rdd_ordem.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int rdd_ordem { get; set; }

		/// <summary>
		/// Propriedade rdd_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rdd_situacao { get; set; }

		/// <summary>
		/// Propriedade rdd_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rdd_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade rdd_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rdd_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade vis_id.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int vis_id { get; set; }

    }
}