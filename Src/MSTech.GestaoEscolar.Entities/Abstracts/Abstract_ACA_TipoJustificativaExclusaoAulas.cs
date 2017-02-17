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
    public abstract class Abstract_ACA_TipoJustificativaExclusaoAulas : Abstract_Entity
    {
		
		/// <summary>
		/// ID do tipo de justificativa de exclusão de aulas.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int tje_id { get; set; }

		/// <summary>
		/// Nome do tipo de justificativa de exclusão de aulas.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string tje_nome { get; set; }

		/// <summary>
		/// Situação do registro: 1-Ativo, 3-Excluído, 4-Inativo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tje_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tje_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tje_dataAlteracao { get; set; }

		/// <summary>
		/// Código do tipo de justificativa de exclusão de aulas.
		/// </summary>
		[MSValidRange(20)]
		public virtual string tje_codigo { get; set; }

    }
}