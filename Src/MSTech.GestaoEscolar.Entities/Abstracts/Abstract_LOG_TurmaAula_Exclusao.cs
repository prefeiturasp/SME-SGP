/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace GestaoEscolar.Entities.Abstracts
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
    public abstract class Abstract_LOG_TurmaAula_Exclusao : Abstract_Entity
    {
		
		/// <summary>
		/// Identificador da tabela LOG_TurmaAula_Exclusao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid lte_id { get; set; }

		/// <summary>
		/// Id da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da aula.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Id do tipo de justificativa para exclusão de aulas.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tje_id { get; set; }

		/// <summary>
		/// Observação para a exclusão da aula.
		/// </summary>
		public virtual string lte_observacao { get; set; }

		/// <summary>
		/// Usuário que gerou a alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data da criação do registro de log.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime lte_data { get; set; }

    }
}