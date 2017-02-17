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
    public abstract class AbstractACA_AlunoAnexo : Abstract_Entity
    {
		
		/// <summary>
		/// ID do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID do anexo do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int aan_id { get; set; }

		/// <summary>
		/// ID do arquivo de anexo do aluno.
		/// </summary>
		public virtual long arq_id { get; set; }

        /// <summary>
        /// Descrição do anexo do aluno.
        /// </summary>
        [MSValidRange(500)]
        [MSNotNullOrEmpty]
        public virtual string aan_descricao { get; set; }

		/// <summary>
		/// Situacao do anexo do aluno (1 - Ativo, 3 - Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte aan_situacao { get; set; }

		/// <summary>
		/// Data de criação do anexo do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aan_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do anexo do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aan_dataAlteracao { get; set; }

    }
}