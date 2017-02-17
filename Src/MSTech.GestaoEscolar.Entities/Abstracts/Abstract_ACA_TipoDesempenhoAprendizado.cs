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
    public abstract class AbstractACA_TipoDesempenhoAprendizado : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tda_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int tda_id { get; set; }

		/// <summary>
		/// Propriedade cal_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Propriedade cur_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Propriedade crr_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Propriedade crp_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Propriedade tds_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// Propriedade tda_descricao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string tda_descricao { get; set; }

		/// <summary>
		/// Propriedade tda_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tda_situacao { get; set; }

		/// <summary>
		/// Propriedade tda_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tda_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tda_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tda_dataAlteracao { get; set; }

    }
}