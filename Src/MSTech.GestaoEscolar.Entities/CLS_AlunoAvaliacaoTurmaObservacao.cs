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
	public class CLS_AlunoAvaliacaoTurmaObservacao : Abstract_CLS_AlunoAvaliacaoTurmaObservacao
	{
        /// <summary>
        /// ID da turma.
        /// </summary>
        [MSNotNullOrEmpty("Turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tur_id { get; set; }

        /// <summary>
        /// ID do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// ID da matrícula turma do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }

        /// <summary>
        /// ID do formato de avaliação.
        /// </summary>
        [MSNotNullOrEmpty("Formato de avaliação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int fav_id { get; set; }

        /// <summary>
        /// ID da avaliação.
        /// </summary>
        [MSNotNullOrEmpty("Avaliação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int ava_id { get; set; }

        /// <summary>
        /// Propriedade ato_desempenhoAprendizado.
        /// </summary>
        [MSValidRange(600, "[MSG_DESEMPENHOAPRENDIZADO] pode conter até 600 caracteres.")]
        public override string ato_desempenhoAprendizado { get; set; }

        /// <summary>
        /// Propriedade ato_recomendacaoAluno.
        /// </summary>
        [MSValidRange(600, "Recomendações ao aluno pode conter até 600 caracteres.")]
        public override string ato_recomendacaoAluno { get; set; }

        /// <summary>
        /// Propriedade ato_recomendacaoResponsavel.
        /// </summary>
        [MSValidRange(700, "Recomendações aos pais/responsáveis pode conter até 700 caracteres.")]
        public override string ato_recomendacaoResponsavel { get; set; }

        [MSDefaultValue(1)]
        public override byte ato_situacao { get; set; }
        public override DateTime ato_dataCriacao { get; set; }
        public override DateTime ato_dataAlteracao { get; set; }
	}
}