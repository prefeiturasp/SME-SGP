/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_PlanejamentoProjetoRelacionada : Abstract_CLS_PlanejamentoProjetoRelacionada
	{

        /// <summary>
        /// ID da escola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override int esc_id { get; set; }

        /// <summary>
        /// ID da unidade escolar.
        /// </summary>
        [MSNotNullOrEmpty("Unidade escolar é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override int uni_id { get; set; }

        /// <summary>
        /// ID do calendario anual.
        /// </summary>
        [MSNotNullOrEmpty("Calendario anual é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cal_id { get; set; }

        /// <summary>
        /// ID do curso.
        /// </summary>
        [MSNotNullOrEmpty("Curso é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cur_id { get; set; }

        /// <summary>
        /// ID do planejamento do projeto.
        /// </summary>
        [MSNotNullOrEmpty("Planejamento do projeto é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override int plp_id { get; set; }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de disciplina é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override int tds_id { get; set; }

	}
}