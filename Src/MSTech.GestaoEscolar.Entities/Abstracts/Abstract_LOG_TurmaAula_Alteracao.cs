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
    public abstract class Abstract_LOG_TurmaAula_Alteracao : Abstract_Entity
    {
		
		/// <summary>
		/// Identificador da tabela LOG_TurmaAula_Alteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid lta_id { get; set; }

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
		/// Tipo de alteração realizada (1 - Alteração da aula / 2 - Alteração no plano de aula / 3 - Alteração na frequência / 4 - Anotação do aluno / 5 - Exclusão de aula).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short lta_tipo { get; set; }

		/// <summary>
		/// Origem da inclusão/alteração (1 - Web - diário de classe / 2 - Web - Listão / 3 - Sincronização / 4 - Web - Agenda).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short lta_origem { get; set; }

		/// <summary>
		/// Usuário que gerou a alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data da criação do registro de log.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime lta_data { get; set; }

    }
}