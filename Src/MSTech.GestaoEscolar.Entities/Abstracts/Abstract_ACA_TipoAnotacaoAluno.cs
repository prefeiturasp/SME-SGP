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
    public abstract class Abstract_ACA_TipoAnotacaoAluno : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade id da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
		[MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
		public virtual int tia_id { get; set; }

		/// <summary>
        /// Propriedade código da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
		[MSValidRange(50)]
        [MSNotNullOrEmpty]
		public virtual string tia_codigo { get; set; }

		/// <summary>
        /// Propriedade nome da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
		[MSValidRange(50)]
        [MSNotNullOrEmpty]
		public virtual string tia_nome { get; set; }

		/// <summary>
        /// Propriedade situacao da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
        [MSNotNullOrEmpty]
		public virtual short tia_situacao { get; set; }

		/// <summary>
        /// Propriedade data de criacao da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tia_dataCriacao { get; set; }

		/// <summary>
        /// Propriedade data de alteracao da tabela ACA_TipoAnotacaoAluno.
		/// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tia_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade id da entidade.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid ent_id { get; set; }

    }
}