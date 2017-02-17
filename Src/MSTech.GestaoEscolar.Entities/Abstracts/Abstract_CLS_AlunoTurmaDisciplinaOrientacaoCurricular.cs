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
    public abstract class Abstract_CLS_AlunoTurmaDisciplinaOrientacaoCurricular : Abstract_Entity
    {
		
		/// <summary>
		/// ID da Turma.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID do Aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da MatriculaTurma.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da MatriculaTurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// Propriedade ocr_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long ocr_id { get; set; }

		/// <summary>
		/// ID da CLS_AlunoHabilidade.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int aha_id { get; set; }

		/// <summary>
        /// ID do tipo de período do calendário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Propriedade aha_alcancada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool aha_alcancada { get; set; }

		/// <summary>
		/// Propriedade aha_efetivada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool aha_efetivada { get; set; }

		/// <summary>
		/// Propriedade aha_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte aha_situacao { get; set; }

		/// <summary>
		/// Propriedade aha_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aha_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade aha_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aha_dataAlteracao { get; set; }

    }
}