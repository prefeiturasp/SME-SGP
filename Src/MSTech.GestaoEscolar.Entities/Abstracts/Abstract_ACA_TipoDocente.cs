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
    public abstract class Abstract_ACA_TipoDocente : Abstract_Entity
    {
		
		/// <summary>
		/// ID tipo docente (EnumTipoDocente).
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
        public virtual byte tdc_id { get; set; }

		/// <summary>
		/// Descrição tipo docente.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string tdc_descricao { get; set; }

        /// <summary>
        /// Nome tipo docente.
        /// </summary>
        [MSValidRange(50)]
        [MSNotNullOrEmpty]
        public virtual string tdc_nome { get; set; }

		/// <summary>
		/// Posicao para tipo de docente.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte tdc_posicao { get; set; }

        /// <summary>
        /// Cor de destaque.
        /// </summary>
        [MSValidRange(10)]
        public virtual string tdc_corDestaque { get; set; }

		/// <summary>
		/// Situação do registro: 1-Ativo, 3-Excluido.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte tdc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdc_dataAlteracao { get; set; }

        /// <summary>
        /// Quantidade permitida para o tipo de docente. 
        /// Se for igual a zero, não possui limitação de quantidade.
        /// </summary>
        [MSDefaultValue(1)]
        public virtual int tdc_quantidade { get; set; }
    }
}