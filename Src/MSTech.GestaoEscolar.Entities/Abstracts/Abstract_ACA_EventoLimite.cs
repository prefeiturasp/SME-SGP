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
    public abstract class Abstract_ACA_EventoLimite : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade cal_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Propriedade tev_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tev_id { get; set; }

		/// <summary>
		/// Propriedade evl_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int evl_id { get; set; }

		/// <summary>
		/// Propriedade tpc_id.
		/// </summary>
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Propriedade esc_id.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Propriedade uni_id.
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Propriedade evl_dataInicio.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime evl_dataInicio { get; set; }

		/// <summary>
		/// Propriedade evl_dataFim.
		/// </summary>
		public virtual DateTime evl_dataFim { get; set; }

		/// <summary>
		/// Propriedade usu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Propriedade evl_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short evl_situacao { get; set; }

		/// <summary>
		/// Propriedade evl_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime evl_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade evl_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime evl_dataAlteracao { get; set; }

        public virtual Guid uad_id { get; set; }
    }
}