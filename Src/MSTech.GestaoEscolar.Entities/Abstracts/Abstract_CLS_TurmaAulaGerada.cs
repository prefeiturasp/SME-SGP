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
    public abstract class Abstract_CLS_TurmaAulaGerada : Abstract_Entity
    {
		
		/// <summary>
		/// ID TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID TurmaAula.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int tag_id { get; set; }

		/// <summary>
		/// Dia da semana da aula gerada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tag_diaSemana { get; set; }

		/// <summary>
		/// Diário da aula.
		/// </summary>
		public virtual int tag_numeroAulas { get; set; }

		/// <summary>
		/// Posicao do docente.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tdt_posicao { get; set; }

		/// <summary>
		/// 1–Aula prevista, 3–Excluído, 4–Aula dada, 6–Aula cancelada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tag_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tag_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tag_dataAlteracao { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TipoPeriodoCalendario.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Propriedade tud_idRelacionada.
		/// </summary>
		public virtual long tud_idRelacionada { get; set; }

    }
}