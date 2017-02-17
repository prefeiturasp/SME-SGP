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
    public abstract class AbstractCLS_TurmaAulaRegencia : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade tau_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Propriedade tud_idFilho.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_idFilho { get; set; }

		/// <summary>
		/// Propriedade tuf_data.
		/// </summary>
		public virtual DateTime tuf_data { get; set; }

		/// <summary>
		/// Propriedade tuf_numeroAulas.
		/// </summary>
		public virtual int tuf_numeroAulas { get; set; }

		/// <summary>
		/// Propriedade tuf_planoAula.
		/// </summary>
		public virtual string tuf_planoAula { get; set; }

		/// <summary>
		/// Propriedade tuf_diarioClasse.
		/// </summary>
		public virtual string tuf_diarioClasse { get; set; }

		/// <summary>
		/// Propriedade tuf_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tuf_situacao { get; set; }

		/// <summary>
		/// Propriedade tuf_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tuf_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tuf_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tuf_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade tuf_conteudo.
		/// </summary>
		public virtual string tuf_conteudo { get; set; }

		/// <summary>
		/// Propriedade tuf_atividadeCasa.
		/// </summary>
		public virtual string tuf_atividadeCasa { get; set; }

		/// <summary>
		/// Propriedade pro_id.
		/// </summary>
		public virtual Guid pro_id { get; set; }

        /// <summary>
        /// Propriedade tuf_sintese.
        /// </summary>
        public virtual string tuf_sintese { get; set; }

        /// <summary>
        /// Propriedade tuf_checadoAtividadeCasa.
        /// </summary>
        public virtual bool tuf_checadoAtividadeCasa { get; set; }

    }
}