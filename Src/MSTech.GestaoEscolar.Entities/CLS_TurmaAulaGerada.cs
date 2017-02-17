/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using System;
	using System.ComponentModel;
	using MSTech.GestaoEscolar.Entities.Abstracts;
	using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_TurmaAulaGerada : Abstract_CLS_TurmaAulaGerada
	{
		[MSNotNullOrEmpty("Turma disciplina é obrigatório.")]
		[DataObjectField(true, false, false)]
		public override long tud_id { get; set; }

		[DataObjectField(true, true, false)]
		public override int tag_id { get; set; }

		/// <summary>
		/// Propriedade tag_diaSemana.
		/// </summary>
		[MSNotNullOrEmpty("Dia da semana é obrigatório.")]
		public override byte tag_diaSemana { get; set; }

		[MSDefaultValue(1)]
		public override short tdt_posicao { get; set; }

		[MSDefaultValue(1)]
		public override byte tag_situacao { get; set; }

		/// <summary>
		/// Propriedade tag_dataCriacao.
		/// </summary>
		public override DateTime tag_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tag_dataAlteracao.
		/// </summary>
		public override DateTime tag_dataAlteracao { get; set; }

		// Variáveis não presentes no banco de dados
		// utilizadas na agenda
		public int uni_id { get; set; }
		public int esc_id { get; set; }
		public int cal_id { get; set; }
		public long tur_id { get; set; }
		public byte tud_tipo { get; set; }
		public int tud_cargaHorariaSemanal { get; set; }
        public bool fav_fechamentoAutomatico { get; set; }
	}
}