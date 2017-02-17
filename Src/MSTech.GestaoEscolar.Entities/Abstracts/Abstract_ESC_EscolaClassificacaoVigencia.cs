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
    public abstract class AbstractESC_EscolaClassificacaoVigencia : Abstract_Entity
    {
		
		/// <summary>
		/// Id da vigência da classificação.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long ecv_id { get; set; }

		/// <summary>
		/// Data de início da vigência.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ecv_dataInicio { get; set; }

		/// <summary>
		/// Data final da vigência.
		/// </summary>
		public virtual DateTime ecv_dataFim { get; set; }

		/// <summary>
		/// Situação do registro (1- Ativo; 2- Bloqueado; 3- Excluído; 4- Inativo).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ecv_situacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ecv_dataAlteracao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ecv_dataCriacao { get; set; }

    }
}