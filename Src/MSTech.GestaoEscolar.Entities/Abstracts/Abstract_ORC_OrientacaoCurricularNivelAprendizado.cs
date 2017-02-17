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
    public abstract class AbstractORC_OrientacaoCurricularNivelAprendizado : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade ocr_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long ocr_id { get; set; }

		/// <summary>
		/// Propriedade nap_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int nap_id { get; set; }

		/// <summary>
		/// Propriedade ocn_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ocn_id { get; set; }

		/// <summary>
		/// Propriedade ocn_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ocn_situacao { get; set; }

		/// <summary>
		/// Propriedade ocn_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ocn_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade ocn_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ocn_dataAlteracao { get; set; }

    }
}