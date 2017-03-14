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
    public abstract class Abstract_ACA_TipoCurriculoPeriodo : Abstract_Entity
    {

        /// <summary>
        /// Campo id da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public virtual int tcp_id { get; set; }

		/// <summary>
		/// Campo id da tabela ACA_TipoNivelEnsino.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tne_id { get; set; }

		/// <summary>
		/// Campo id da tabela ACA_TipoModalidadeEnsino.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tme_id { get; set; }

		/// <summary>
		/// Campo descrição da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string tcp_descricao { get; set; }

		/// <summary>
		/// Campo ordem da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tcp_ordem { get; set; }

        /// <summary>
        /// Campo que define se o tipo de periodo do curso pode possuir objeto de aprendizagem
        /// </summary>
        public virtual bool tcp_objetoAprendizagem { get; set; }

        /// <summary>
        /// Campo situação da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSNotNullOrEmpty]
		public virtual short tcp_situacao { get; set; }

		/// <summary>
		/// Campo data de criação da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tcp_dataCriacao { get; set; }

		/// <summary>
		/// Campo data de alteração da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tcp_dataAlteracao { get; set; }

    }
}