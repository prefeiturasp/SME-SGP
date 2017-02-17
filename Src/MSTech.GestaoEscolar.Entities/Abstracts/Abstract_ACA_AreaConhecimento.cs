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
    public abstract class Abstract_ACA_AreaConhecimento : Abstract_Entity
    {
		
		/// <summary>
		/// Id da área de conhecimento.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int aco_id { get; set; }

		/// <summary>
		/// Nome da área de conhecimento.
		/// </summary>
		[MSValidRange(150)]
		[MSNotNullOrEmpty]
		public virtual string aco_nome { get; set; }

		/// <summary>
		/// Tipo de base geral (1- Resolucao, 2- Decreto).
		/// </summary>
		public virtual short aco_tipoBaseGeral { get; set; }

		/// <summary>
		/// Tipo de base (1- Nacional comum, 2- Parte diversificada).
		/// </summary>
		public virtual short aco_tipoBase { get; set; }

        /// <summary>
        /// Ordem da área de conhecimento.
        /// </summary>
        public virtual int aco_ordem { get; set; }

		/// <summary>
		/// Situacao da área de conhecimento. (1 - Ativo, 3 - Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aco_situacao { get; set; }

		/// <summary>
		/// Data de alteração da área de conhecimento.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aco_dataAlteracao { get; set; }

		/// <summary>
		/// Data de cadastro da área de conhecimento.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual DateTime aco_dataCriacao { get; set; }

    }
}