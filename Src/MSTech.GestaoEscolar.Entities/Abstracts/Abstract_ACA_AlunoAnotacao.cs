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
    public abstract class Abstract_ACA_AlunoAnotacao : Abstract_Entity
    {
		
		/// <summary>
		/// ID do Aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da anotação do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ano_id { get; set; }

		/// <summary>
		/// Data da anotação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ano_dataAnotacao { get; set; }

		/// <summary>
		/// Texto da anotação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string ano_anotacao { get; set; }

        /// <summary>
        /// Usuário que criou a anotação.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual Guid usu_id { get; set; }

        /// <summary>
        /// Grupo do usuário que criou a anotação.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual Guid gru_id { get; set; }

		/// <summary>
		/// 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ano_situacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ano_dataAlteracao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ano_dataCriacao { get; set; }

    }
}