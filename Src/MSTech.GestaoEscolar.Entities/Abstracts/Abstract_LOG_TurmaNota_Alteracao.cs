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
    public abstract class Abstract_LOG_TurmaNota_Alteracao : Abstract_Entity
    {
		
		/// <summary>
		/// Identificador da tabela LOG_TurmaNota_Alteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid ltn_id { get; set; }

		/// <summary>
		/// Id da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da atividade.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tnt_id { get; set; }

		/// <summary>
		/// Tipo de alteração realizada (1- Alteração de atividade / 2 - Lançamento de notas / 3 - Exclusão de atividade).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ltn_tipo { get; set; }

		/// <summary>
		/// Origem da inclusão/alteração (1-Web - diário de classe / 2 - Web - Listão / 3 - Sincronização).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ltn_origem { get; set; }

		/// <summary>
		/// Usuário que gerou a alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data da criação do registro de log.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ltn_data { get; set; }

    }
}