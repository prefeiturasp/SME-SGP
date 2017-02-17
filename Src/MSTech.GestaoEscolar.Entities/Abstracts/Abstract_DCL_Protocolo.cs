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
    public abstract class Abstract_DCL_Protocolo : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade pro_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid pro_id { get; set; }

		/// <summary>
		/// Propriedade equ_id.
		/// </summary>
		public virtual Guid equ_id { get; set; }

		/// <summary>
		/// Propriedade pro_tipo.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pro_tipo { get; set; }

		/// <summary>
		/// Propriedade pro_protocolo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long pro_protocolo { get; set; }

		/// <summary>
		/// Propriedade pro_pacote.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string pro_pacote { get; set; }

		/// <summary>
		/// Propriedade pro_status.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pro_status { get; set; }

		/// <summary>
		/// Propriedade pro_statusObservacao.
		/// </summary>
		public virtual string pro_statusObservacao { get; set; }

		/// <summary>
		/// Propriedade tur_id.
		/// </summary>
		public virtual long tur_id { get; set; }

		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade tau_id.
		/// </summary>
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Propriedade pro_qtdeAlunos.
		/// </summary>
		public virtual int pro_qtdeAlunos { get; set; }

		/// <summary>
		/// Propriedade pro_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pro_situacao { get; set; }

		/// <summary>
		/// Propriedade pro_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime pro_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade pro_dataalteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime pro_dataalteracao { get; set; }

        /// <summary>
        /// Propriedade pro_tentativa
        /// </summary>
        public virtual int pro_tentativa { get; set; }

        /// <summary>
        /// Propriedade pro_pacote.
        /// </summary>
        public virtual string pro_versaoAplicativo { get; set; }

        /// <summary>
        /// Propriedade esc_id.
        /// </summary>
        public virtual int esc_id { get; set; }

    }
}