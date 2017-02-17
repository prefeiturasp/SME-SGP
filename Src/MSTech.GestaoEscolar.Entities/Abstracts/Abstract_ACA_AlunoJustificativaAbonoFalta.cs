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
    public abstract class Abstract_ACA_AlunoJustificativaAbonoFalta : Abstract_Entity
    {
		
		/// <summary>
		/// Id do aluno.
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Id da disciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tud_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da tabela.
		/// </summary>
		[MSNotNullOrEmpty("[ajf_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int ajf_id { get; set; }

		/// <summary>
		/// Data incial da justtificativa de falta.
		/// </summary>
		[MSNotNullOrEmpty("[ajf_dataInicio] é obrigatório.")]
		public virtual DateTime ajf_dataInicio { get; set; }

		/// <summary>
		/// Data final da justificativa de falta.
		/// </summary>
		public virtual DateTime ajf_dataFim { get; set; }

		/// <summary>
		/// Observação.
		/// </summary>
		public virtual string ajf_observacao { get; set; }

		/// <summary>
		/// Status de processamento (1-Aguardando processamento, 2-Em processamento, 3-Processado).
		/// </summary>
		[MSNotNullOrEmpty("[ajf_status] é obrigatório.")]
		public virtual byte ajf_status { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído.
		/// </summary>
		[MSNotNullOrEmpty("[ajf_situacao] é obrigatório.")]
		public virtual byte ajf_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[ajf_dataCriacao] é obrigatório.")]
		public virtual DateTime ajf_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[ajf_dataAlteracao] é obrigatório.")]
		public virtual DateTime ajf_dataAlteracao { get; set; }

    }
}