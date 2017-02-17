/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class TUR_TurmaDisciplinaAulaPrevista : Abstract_TUR_TurmaDisciplinaAulaPrevista
	{
        /// <summary>
        /// Campo Id da tabela TUR_TurmaDisciplina.
        /// </summary>
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }

        /// <summary>
        /// Campo Id da tabela ACA_TipoPeriodoCalendario.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de período do calendário é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tpc_id { get; set; }

        /// <summary>
        /// Quantidade de aulas previstas.
        /// </summary>
        [MSNotNullOrEmpty("Quantidade de aulas previstas é obrigatório e deve ser maior que 0 (zero).")]
        public override int tap_aulasPrevitas { get; set; }

        /// <summary>
        /// Indica se os registros de frequência dos alunos foram recalculados utilizando aulas previstas.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool tap_registrosCorrigidos { get; set; }

        [MSDefaultValue(1)]
        public override short tap_situacao { get; set; }
        public override DateTime tap_dataCriacao { get; set; }
        public override DateTime tap_dataAlteracao { get; set; }

        public byte tud_tipo { get; set; }
	}
}