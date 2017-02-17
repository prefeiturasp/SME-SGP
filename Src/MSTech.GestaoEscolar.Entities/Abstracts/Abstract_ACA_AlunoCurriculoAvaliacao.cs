/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ACA_AlunoCurriculoAvaliacao : Abstract_Entity
    {

		/// <summary>
		/// ID do aluno
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }

		/// <summary>
		/// ID do currículo do aluno
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int alc_id { get; set; }

		/// <summary>
		/// ID da avaliação do currículo do aluno
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ala_id { get; set; }

		/// <summary>
		/// ID da turma
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Int64 tur_id { get; set; }

		/// <summary>
		/// ID do curso
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID do currículo do curso
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// ID do período do curso
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// ID da avaliação no currículo da turma
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int tca_id { get; set; }

		/// <summary>
		/// ID da matrícula na turma
		/// </summary>
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da avaliação do aluno na turma
		/// </summary>
		public virtual int aat_id { get; set; }

		/// <summary>
		/// Indica se o aluno foi/será avaliado.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool ala_avaliado { get; set; }

		/// <summary>
		/// Data de início do período em que o aluno cursará a avaliação
		/// </summary>
		public virtual DateTime ala_dataInicio { get; set; }

		/// <summary>
		/// Data de fim do período em que o aluno cursará a avaliação
		/// </summary>
		public virtual DateTime ala_dataFim { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído, 4 – Inativo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte ala_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime ala_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime ala_dataAlteracao { get; set; }

    }
}