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
    public abstract class AbstractTUR_TurmaDisciplinaRelacionada : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relacionamento entre as turmas disciplinas.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tdr_id { get; set; }

		/// <summary>
		/// ID da turma disciplina compartilhada.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID da turma disciplina relacionada.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_idRelacionada { get; set; }

		/// <summary>
		/// Inicio de vigência do relacionamento entre as turmas disciplinas.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdr_vigenciaInicio { get; set; }

		/// <summary>
		/// Fim de vigência do relacionamento entre as turmas disciplinas.
		/// </summary>
		public virtual DateTime tdr_vigenciaFim { get; set; }

		/// <summary>
		/// Situação do registro. 1-Ativo, 3-Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tdr_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdr_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdr_dataAlteracao { get; set; }

    }
}