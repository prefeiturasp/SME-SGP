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
    public abstract class Abstract_CLS_AlunoAvaliacaoTurma : Abstract_Entity
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
		/// Propriedade aat_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int aat_id { get; set; }

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
		/// Propriedade aat_avaliacao.
		/// </summary>
		[MSValidRange(20)]
		public virtual string aat_avaliacao { get; set; }

		/// <summary>
		/// Propriedade aat_frequencia.
		/// </summary>
		public virtual decimal aat_frequencia { get; set; }

		/// <summary>
		/// Propriedade aat_comentarios.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string aat_comentarios { get; set; }

		/// <summary>
		/// Propriedade aat_relatorio.
		/// </summary>
		public virtual string aat_relatorio { get; set; }

		/// <summary>
		/// Propriedade aat_semProfessor.
		/// </summary>
		public virtual bool aat_semProfessor { get; set; }

		/// <summary>
		/// Propriedade aat_numeroFaltas.
		/// </summary>
		public virtual int aat_numeroFaltas { get; set; }

		/// <summary>
		/// Propriedade aat_numeroAulas.
		/// </summary>
		public virtual int aat_numeroAulas { get; set; }

		/// <summary>
		/// Propriedade arq_idRelatorio.
		/// </summary>
		public virtual long arq_idRelatorio { get; set; }

		/// <summary>
		/// Propriedade aat_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte aat_situacao { get; set; }

		/// <summary>
		/// Propriedade aat_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aat_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade aat_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aat_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade aat_ausenciasCompensadas.
		/// </summary>
		public virtual int aat_ausenciasCompensadas { get; set; }

		/// <summary>
		/// Propriedade aat_avaliacaoAdicional.
		/// </summary>
		[MSValidRange(20)]
		public virtual string aat_avaliacaoAdicional { get; set; }

		/// <summary>
		/// Propriedade aat_frequenciaAcumulada.
		/// </summary>
		public virtual decimal aat_frequenciaAcumulada { get; set; }

		/// <summary>
		/// Propriedade aat_faltoso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool aat_faltoso { get; set; }

		/// <summary>
		/// Propriedade aat_registroexterno.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool aat_registroexterno { get; set; }

		/// <summary>
		/// Propriedade aat_frequenciaAcumuladaCalculada.
		/// </summary>
		public virtual decimal aat_frequenciaAcumuladaCalculada { get; set; }

		/// <summary>
		/// Propriedade aat_naoAvaliado.
		/// </summary>
		public virtual bool aat_naoAvaliado { get; set; }

		/// <summary>
		/// Propriedade aat_avaliacaoPosConselho.
		/// </summary>
		[MSValidRange(20)]
		public virtual string aat_avaliacaoPosConselho { get; set; }

		/// <summary>
		/// Propriedade aat_justificativaPosConselho.
		/// </summary>
		public virtual string aat_justificativaPosConselho { get; set; }

		/// <summary>
		/// Propriedade aat_frequenciaFinalAjustada.
		/// </summary>
		public virtual decimal aat_frequenciaFinalAjustada { get; set; }

    }
}