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
    public abstract class Abstract_CLS_RelatorioAtendimentoGrupo : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relatório de atendimento.
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// ID do grupo de usuário.
		/// </summary>
		[MSNotNullOrEmpty("[gru_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid gru_id { get; set; }

		/// <summary>
		/// Permite consultar.
		/// </summary>
		[MSNotNullOrEmpty("[rag_permissaoConsulta] é obrigatório.")]
		public virtual bool rag_permissaoConsulta { get; set; }

		/// <summary>
		/// Permite editar.
		/// </summary>
		[MSNotNullOrEmpty("[rag_permissaoEdicao] é obrigatório.")]
		public virtual bool rag_permissaoEdicao { get; set; }

		/// <summary>
		/// Permite excluir.
		/// </summary>
		[MSNotNullOrEmpty("[rag_permissaoExclusao] é obrigatório.")]
		public virtual bool rag_permissaoExclusao { get; set; }

		/// <summary>
		/// Permite aprovar.
		/// </summary>
		[MSNotNullOrEmpty("[rag_permissaoAprovacao] é obrigatório.")]
		public virtual bool rag_permissaoAprovacao { get; set; }

    }
}