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
    public abstract class Abstract_LOG_AvaliacaoMedia_Alteracao : Abstract_Entity
    {
		
		/// <summary>
		/// Identificador da tabela LOG_AvaliacaoMedia_Alteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid lam_id { get; set; }

		/// <summary>
		/// Id da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id do tipo período calendário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Tipo de alteração realizada (1 - Alteração da média).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short lam_tipo { get; set; }

		/// <summary>
		/// Origem da inclusão/alteração (1 - Web / 2 - Sincronização).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short lam_origem { get; set; }

		/// <summary>
		/// Usuário que gerou a alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data da criação do registro de log.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime lam_data { get; set; }

    }
}