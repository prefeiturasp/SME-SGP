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
    public abstract class AbstractORC_NivelAprendizado : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade nap_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int nap_id { get; set; }

        /// <summary>
        /// Id do curso.
        /// </summary>
        public virtual int cur_id { get; set; }

        /// <summary>
        /// Id do curriculo.
        /// </summary>
        public virtual int crr_id { get; set; }

        /// <summary>
        /// Id do período do curso.
        /// </summary>
        public virtual int crp_id { get; set; }

		/// <summary>
		/// Propriedade nap_descricao.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string nap_descricao { get; set; }

		/// <summary>
		/// Propriedade nap_sigla.
		/// </summary>
		[MSValidRange(10)]
		[MSNotNullOrEmpty]
		public virtual string nap_sigla { get; set; }

		/// <summary>
		/// Propriedade nap_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short nap_situacao { get; set; }

		/// <summary>
		/// Propriedade nap_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime nap_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade nap_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime nap_dataAlteracao { get; set; }

    }
}