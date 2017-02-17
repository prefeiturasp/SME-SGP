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
    public abstract class AbstractCFG_ObservacaoBoletim : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade ent_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Propriedade obb_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int obb_id { get; set; }

		/// <summary>
		/// Propriedade obb_tipoObservacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int obb_tipoObservacao { get; set; }

		/// <summary>
		/// Propriedade obb_nome.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string obb_nome { get; set; }

		/// <summary>
		/// Propriedade obb_descricao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string obb_descricao { get; set; }

		/// <summary>
		/// Propriedade obb_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short obb_situacao { get; set; }

		/// <summary>
		/// Propriedade obb_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime obb_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade obb_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime obb_dataAlteracao { get; set; }

    }
}