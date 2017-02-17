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
    public abstract class AbstractACA_TipoRecursoAvaliacaoINEP : Abstract_Entity
    {
		
		/// <summary>
		/// Id do tipo de recurso necessário para a realização de uma prova do Inep, esse Id é fixo, sendo utilizado no censo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tri_id { get; set; }

		/// <summary>
		/// Nome do tipo de recurso.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string tri_nome { get; set; }

		/// <summary>
		/// Situação. 1 – Ativo; 3 – Excluído..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tri_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tri_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tri_dataAlteracao { get; set; }

    }
}