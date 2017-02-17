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
    public abstract class Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaMedia : Abstract_Entity
    {
		
		/// <summary>
		/// ID da disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da matrícula do aluno na turma.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da matrícula do aluno na disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

        /// <summary>
        /// ID do tipo período do calendário.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int tpc_id { get; set; }

		/// <summary>
		/// Valor da média calculada.
		/// </summary>
		[MSValidRange(20)]
		public virtual string atm_media { get; set; }

		/// <summary>
		/// Situação do registro. 1-Ativo, 3-Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short atm_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atm_dataCriacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atm_dataAlteracao { get; set; }

    }
}