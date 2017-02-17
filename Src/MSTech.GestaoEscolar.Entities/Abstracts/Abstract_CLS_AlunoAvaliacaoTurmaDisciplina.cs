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
    public abstract class Abstract_CLS_AlunoAvaliacaoTurmaDisciplina : Abstract_Entity
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
		/// Propriedade atd_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int atd_id { get; set; }

		/// <summary>
		/// Propriedade fav_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int fav_id { get; set; }

		/// <summary>
		/// Propriedade ava_id.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int ava_id { get; set; }

		/// <summary>
		/// Propriedade atd_avaliacao.
		/// </summary>
		[MSValidRange(20)]
		public virtual string atd_avaliacao { get; set; }

		/// <summary>
		/// Propriedade atd_frequencia.
		/// </summary>
		public virtual decimal atd_frequencia { get; set; }

		/// <summary>
		/// Propriedade atd_comentarios.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string atd_comentarios { get; set; }

		/// <summary>
		/// Propriedade atd_relatorio.
		/// </summary>
		public virtual string atd_relatorio { get; set; }

		/// <summary>
		/// Propriedade atd_semProfessor.
		/// </summary>
		public virtual bool atd_semProfessor { get; set; }

		/// <summary>
		/// Propriedade atd_numeroFaltas.
		/// </summary>
		public virtual int atd_numeroFaltas { get; set; }

		/// <summary>
		/// Propriedade atd_numeroAulas.
		/// </summary>
		public virtual int atd_numeroAulas { get; set; }

		/// <summary>
		/// Propriedade arq_idRelatorio.
		/// </summary>
		public virtual long arq_idRelatorio { get; set; }

		/// <summary>
		/// Propriedade atd_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte atd_situacao { get; set; }

		/// <summary>
		/// Propriedade atd_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atd_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade atd_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime atd_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade atd_ausenciasCompensadas.
		/// </summary>
		public virtual int atd_ausenciasCompensadas { get; set; }

		/// <summary>
		/// Propriedade atd_registroexterno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool atd_registroexterno { get; set; }

		/// <summary>
		/// Propriedade atd_avaliacaoPosConselho.
		/// </summary>
		[MSValidRange(20)]
		public virtual string atd_avaliacaoPosConselho { get; set; }

		/// <summary>
		/// Propriedade atd_justificativaPosConselho.
		/// </summary>
		public virtual string atd_justificativaPosConselho { get; set; }

		/// <summary>
		/// Propriedade atd_frequenciaFinalAjustada.
		/// </summary>
		public virtual decimal atd_frequenciaFinalAjustada { get; set; }

        /// <summary>
        /// Propriedade atd_numeroFaltasReposicao.
        /// </summary>
        public virtual int atd_numeroFaltasReposicao { get; set; }

        /// <summary>
        /// Propriedade atd_numeroAulasReposicao.
        /// </summary>
        public virtual int atd_numeroAulasReposicao { get; set; }
    }
}