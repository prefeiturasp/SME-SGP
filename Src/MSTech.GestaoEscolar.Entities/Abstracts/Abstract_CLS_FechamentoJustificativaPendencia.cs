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
    public abstract class AbstractCLS_FechamentoJustificativaPendencia : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade cal_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Propriedade tpc_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Propriedade fjp_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int fjp_id { get; set; }

		/// <summary>
		/// Propriedade fjp_justificativa.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string fjp_justificativa { get; set; }

		/// <summary>
		/// Propriedade usu_id.
		/// </summary>
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Propriedade usu_idAlteracao.
		/// </summary>
		public virtual Guid usu_idAlteracao { get; set; }

		/// <summary>
		/// Propriedade fjp_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte fjp_situacao { get; set; }

		/// <summary>
		/// Propriedade fjp_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime fjp_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade fjp_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime fjp_dataAlteracao { get; set; }

    }
}