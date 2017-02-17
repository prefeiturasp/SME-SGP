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
    public abstract class Abstract_ACA_TipoAreaDocumento : Abstract_Entity
    {
		
		/// <summary>
		/// Id do tipo de area.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int tad_id { get; set; }

		/// <summary>
		/// Nome do tipo de area.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string tad_nome { get; set; }

		/// <summary>
		/// Situação do tipo de area (1-Ativo/3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tad_situacao { get; set; }

		/// <summary>
		/// Data de criação do tipo de area.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tad_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do tipo de area.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tad_dataAlteracao { get; set; }

		/// <summary>
		/// Indica a ordem para exibir o tipo de area.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tad_ordem { get; set; }

		/// <summary>
		/// Permite cadastro pela escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool tad_cadastroEscola { get; set; }

    }
}