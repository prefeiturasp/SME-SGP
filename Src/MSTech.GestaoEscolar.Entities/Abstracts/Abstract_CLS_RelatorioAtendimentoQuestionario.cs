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
    public abstract class Abstract_CLS_RelatorioAtendimentoQuestionario : Abstract_Entity
    {
		
		/// <summary>
		/// ID do registro (Relatorio Questionário).
		/// </summary>
		[MSNotNullOrEmpty("[raq_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int raq_id { get; set; }

		/// <summary>
		/// ID do relatório de atendimento.
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// ID do questionário.
		/// </summary>
		[MSNotNullOrEmpty("[qst_id] é obrigatório.")]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Ordem do questionário no relatório de atendimento.
		/// </summary>
		[MSNotNullOrEmpty("[raq_ordem] é obrigatório.")]
		public virtual int raq_ordem { get; set; }

		/// <summary>
		/// Situação do registro (1 - ativo, 3 - excluído).
		/// </summary>
		[MSNotNullOrEmpty("[raq_situacao] é obrigatório.")]
		public virtual byte raq_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[raq_dataCriacao] é obrigatório.")]
		public virtual DateTime raq_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[raq_dataAlteracao] é obrigatório.")]
		public virtual DateTime raq_dataAlteracao { get; set; }

    }
}