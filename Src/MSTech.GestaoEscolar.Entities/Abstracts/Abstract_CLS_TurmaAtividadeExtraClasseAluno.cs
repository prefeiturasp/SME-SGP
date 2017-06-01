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
    public abstract class Abstract_CLS_TurmaAtividadeExtraClasseAluno : Abstract_Entity
    {
		
		/// <summary>
		/// Id da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tud_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da atividade extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[tae_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tae_id { get; set; }

		/// <summary>
		/// Id do aluno.
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Id da matricula turma do aluno.
		/// </summary>
		[MSNotNullOrEmpty("[mtu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// Id da matricula turma disciplina do aluno.
		/// </summary>
		[MSNotNullOrEmpty("[mtd_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// Avaliação do aluno.
		/// </summary>
		[MSValidRange(20)]
		public virtual string aea_avaliacao { get; set; }

		/// <summary>
		/// Avaliação relatório do aluno.
		/// </summary>
		public virtual string aea_relatorio { get; set; }

		/// <summary>
		/// Indica se a atividade foi entregue pelo aluno.
		/// </summary>
		[MSNotNullOrEmpty("[aea_entregue] é obrigatório.")]
		public virtual bool aea_entregue { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aea_situacao] é obrigatório.")]
		public virtual byte aea_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aea_dataCriacao] é obrigatório.")]
		public virtual DateTime aea_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aea_dataAlteracao] é obrigatório.")]
		public virtual DateTime aea_dataAlteracao { get; set; }

    }
}