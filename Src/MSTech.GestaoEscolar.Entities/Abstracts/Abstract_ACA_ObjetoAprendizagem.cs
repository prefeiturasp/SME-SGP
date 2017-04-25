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
    public abstract class Abstract_ACA_ObjetoAprendizagem : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade oap_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int oap_id { get; set; }

		/// <summary>
		/// Propriedade tds_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// Propriedade oap_descricao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string oap_descricao { get; set; }

        /// <summary>
        /// Propriedade cal_ano.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int cal_ano { get; set; }

        /// <summary>
        /// Propriedade id do eixo.
        /// </summary>
        public virtual int oae_id { get; set; }

        /// <summary>
        /// Propriedade oap_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
		public virtual byte oap_situacao { get; set; }

		/// <summary>
		/// Propriedade oap_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime oap_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade oap_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime oap_dataAlteracao { get; set; }

    }
}