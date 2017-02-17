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
    public abstract class Abstract_TUR_TurmaAberturaAnosAnteriores : Abstract_Entity
    {
		
		/// <summary>
		/// Id da tabela.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long tab_id { get; set; }

        /// <summary>
        /// Ano.
        /// </summary>
        [MSValidRange(4)]
        [MSNotNullOrEmpty]
		public virtual int tab_ano { get; set; }

		/// <summary>
		/// Id da unidade superior (DRE).
		/// </summary>
		public virtual Guid uad_idSuperior { get; set; }

		/// <summary>
		/// Id da escola.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade.
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Data de início.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tab_dataInicio { get; set; }

		/// <summary>
		/// Data de fim.
		/// </summary>
		public virtual DateTime tab_dataFim { get; set; }

		/// <summary>
		/// Status (1 - Aguardando execução, 2 - Aberto,  3 - Encerrado).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tab_status { get; set; }

		/// <summary>
		/// Situação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tab_situacao { get; set; }

		/// <summary>
		/// Data de criação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tab_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tab_dataAlteracao { get; set; }

    }
}