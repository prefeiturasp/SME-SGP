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
    public abstract class Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaObservacao : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Propriedade mtu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// Propriedade mtd_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// Propriedade fav_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int fav_id { get; set; }

		/// <summary>
		/// Propriedade ava_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ava_id { get; set; }

		/// <summary>
		/// Propriedade ado_qualidade.
		/// </summary>
		public virtual string ado_qualidade { get; set; }

		/// <summary>
		/// Propriedade ado_desempenhoAprendizado.
		/// </summary>
		public virtual string ado_desempenhoAprendizado { get; set; }

		/// <summary>
		/// Propriedade ado_recomendacaoAluno.
		/// </summary>
		public virtual string ado_recomendacaoAluno { get; set; }

		/// <summary>
		/// Propriedade ado_recomendacaoResponsavel.
		/// </summary>
		public virtual string ado_recomendacaoResponsavel { get; set; }

        /// <summary>
        /// Propriedade ado_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte ado_situacao { get; set; }

        /// <summary>
        /// Propriedade ado_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime ado_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade ado_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime ado_dataAlteracao { get; set; }
    }
}