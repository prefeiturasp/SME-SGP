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
    public abstract class AbstractLOG_MovimentacaoAcertoSituacaoAluno : Abstract_Entity
    {
		
		/// <summary>
		/// Id do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Id do log de acerto de situação do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mta_id { get; set; }

		/// <summary>
		/// Propriedade pfi_id.
		/// </summary>
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// Id da renovação anterior do aluno.
		/// </summary>
		public virtual int mtr_idAnterior { get; set; }

		/// <summary>
		/// Id da renovação atual do aluno.
		/// </summary>
		public virtual int mtr_idAtual { get; set; }

		/// <summary>
		/// Id da matrícula anterior do aluno.
		/// </summary>
		public virtual int mov_idAnterior { get; set; }

		/// <summary>
		/// Id da matrícula atual do aluno.
		/// </summary>
		public virtual int mov_idAtual { get; set; }

		/// <summary>
		/// Id do usuário que realizou o acerto da situação do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data de realização do acerto do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mtd_data { get; set; }

    }
}