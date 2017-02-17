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
    public abstract class AbstractACA_RecomendacaoAlunoResponsavel : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade rar_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int rar_id { get; set; }

		/// <summary>
		/// Propriedade rar_descricao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string rar_descricao { get; set; }

		/// <summary>
		/// Propriedade rar_tipo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rar_tipo { get; set; }

		/// <summary>
		/// Propriedade rar_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rar_situacao { get; set; }

		/// <summary>
		/// Propriedade rar_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rar_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade rar_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rar_dataAlteracao { get; set; }

    }
}