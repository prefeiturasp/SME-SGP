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
    public abstract class Abstract_CLS_AlunoAvaliacaoTurmaObservacao : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tur_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tur_id { get; set; }

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
		/// Propriedade ato_qualidade.
		/// </summary>
		public virtual string ato_qualidade { get; set; }

		/// <summary>
		/// Propriedade ato_desempenhoAprendizado.
		/// </summary>
		public virtual string ato_desempenhoAprendizado { get; set; }

		/// <summary>
		/// Propriedade ato_recomendacaoAluno.
		/// </summary>
		public virtual string ato_recomendacaoAluno { get; set; }

		/// <summary>
		/// Propriedade ato_recomendacaoResponsavel.
		/// </summary>
		public virtual string ato_recomendacaoResponsavel { get; set; }

        /// <summary>
        /// Propriedade ato_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte ato_situacao { get; set; }

        /// <summary>
        /// Propriedade ato_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime ato_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade ato_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime ato_dataAlteracao { get; set; }

        public virtual Guid usu_idAlteracao { get; set; }
    }
}