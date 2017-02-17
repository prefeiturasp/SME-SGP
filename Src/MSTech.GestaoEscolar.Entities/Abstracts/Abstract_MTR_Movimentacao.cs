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
    public abstract class AbstractMTR_Movimentacao : Abstract_Entity
    {
		
		/// <summary>
		/// ID do Aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da Movimentação.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mov_id { get; set; }

		/// <summary>
		/// ID do TipoMovimentacao de entrada.
		/// </summary>
		public virtual int tmv_idEntrada { get; set; }

		/// <summary>
		/// ID do TipoMovimentacao de saída.
		/// </summary>
		public virtual int tmv_idSaida { get; set; }

		/// <summary>
		/// ID do Usuario que realizou.
		/// </summary>
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// ID AlunoCurriculoAnterior.
		/// </summary>
		public virtual int alc_idAnterior { get; set; }

		/// <summary>
		/// ID AlunoCurriculo Atual.
		/// </summary>
		public virtual int alc_idAtual { get; set; }

		/// <summary>
		/// ID MatriculaTurma anterior.
		/// </summary>
		public virtual int mtu_idAnterior { get; set; }

		/// <summary>
		/// ID MatriculaTurma nova.
		/// </summary>
		public virtual int mtu_idAtual { get; set; }

		/// <summary>
		/// Data em que a movimentação foi realizada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mov_dataRealizacao { get; set; }

		/// <summary>
		/// 1 - Ativo, 3 - Excluído, 4 - Retroativa.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte mov_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mov_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mov_dataAlteracao { get; set; }

		/// <summary>
		/// ID do tipo de movimentação.
		/// </summary>
		public virtual int tmo_id { get; set; }

		/// <summary>
		/// Propriedade tmo_idImportado.
		/// </summary>
		public virtual int tmo_idImportado { get; set; }

		/// <summary>
		/// Ordem que a movimentação foi realizada por aluno.
		/// </summary>
		public virtual int mov_ordem { get; set; }

		/// <summary>
		/// Frequência do aluno.
		/// </summary>
		public virtual decimal mov_frequencia { get; set; }

    }
}