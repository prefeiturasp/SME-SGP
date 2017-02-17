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
	public class CLS_AlunoPlanejamentoRelacionada : Abstract_CLS_AlunoPlanejamentoRelacionada
	{
        /// <summary>
        /// ID do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// ID da turma disciplina.
        /// </summary>
        [MSNotNullOrEmpty("Turma disciplina é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }

        /// <summary>
        /// ID do planejamento do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Planejamento do aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int apl_id { get; set; }

        /// <summary>
        /// ID da turma disciplina relacionada.
        /// </summary>
        [MSNotNullOrEmpty("Turma disciplina relacionada é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override long tud_idRelacionado { get; set; }
	}
}