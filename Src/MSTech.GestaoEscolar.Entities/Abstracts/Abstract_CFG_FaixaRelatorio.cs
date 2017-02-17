using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	/// <summary>
	/// Description: .
	/// </summary>
    [Serializable()]
    public abstract class AbstractCFG_FaixaRelatorio : Abstract_Entity
    {
		/// <summary>
		/// Propriedade far_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int far_id { get; set; }

		/// <summary>
		/// Propriedade rlt_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rlt_id { get; set; }

		/// <summary>
		/// Propriedade far_descricao.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string far_descricao { get; set; }

		/// <summary>
		/// Propriedade far_inicio.
		/// </summary>
        [MSValidRange(20)]
		public virtual string far_inicio { get; set; }

		/// <summary>
		/// Propriedade far_fim.
		/// </summary>
        [MSValidRange(20)]
		public virtual string far_fim { get; set; }

        /// <summary>
        /// Propriedade esa_id.
        /// </summary>
        public virtual int esa_id { get; set; }

        /// <summary>
        /// Propriedade eap_id.
        /// </summary>
        public virtual int eap_id { get; set; }

        /// <summary>
        /// Propriedade far_cor.
        /// </summary>
        [MSValidRange(200)]
        public virtual string far_cor { get; set; }

		/// <summary>
		/// Propriedade far_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte far_situacao { get; set; }

		/// <summary>
		/// Propriedade far_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime far_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade far_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime far_dataAlteracao { get; set; }

    }
}