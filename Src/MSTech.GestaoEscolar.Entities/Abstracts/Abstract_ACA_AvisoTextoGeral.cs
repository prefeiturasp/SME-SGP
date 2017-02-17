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
    public abstract class AbstractACA_AvisoTextoGeral : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade atg_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long atg_id { get; set; }

		/// <summary>
		/// Propriedade esc_id.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Propriedade uni_id.
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Propriedade cur_id.
		/// </summary>
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Propriedade crr_id.
		/// </summary>
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Propriedade atg_titulo.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string atg_titulo { get; set; }

		/// <summary>
		/// Propriedade atg_descricao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string atg_descricao { get; set; }

		/// <summary>
		/// Propriedade atg_timbreCabecalho.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool atg_timbreCabecalho { get; set; }

		/// <summary>
		/// Propriedade atg_anotacaoAula.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool atg_anotacaoAula { get; set; }

		/// <summary>
		/// Propriedade atg_tipo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte atg_tipo { get; set; }

		/// <summary>
		/// Propriedade atg_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte atg_situacao { get; set; }

		/// <summary>
		/// Propriedade atg_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atg_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade atg_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atg_dataAlteracao { get; set; }

    }
}