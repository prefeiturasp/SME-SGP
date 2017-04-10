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
    public abstract class Abstract_REL_TurmaDisciplinaSituacaoFechamento : Abstract_Entity
    {
		
		/// <summary>
		/// ID da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID do calendário anual.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Indica se há pendência para a disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool Pendente { get; set; }

        /// <summary>
        /// Indica se há pendência para a disciplina.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool PendentePlanejamento { get; set; }

        /// <summary>
        /// Indica se há pendência de parecer conclusivo.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool PendenteParecer { get; set; }

		/// <summary>
		/// Data de processamento da disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime DataProcessamento { get; set; }

    }
}