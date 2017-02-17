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
	public class CLS_AlunoAvaliacaoTurmaRecomendacao : Abstract_CLS_AlunoAvaliacaoTurmaRecomendacao
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
        /// ID do tipo de qualidade.
        /// </summary>
        [MSNotNullOrEmpty("Recomendação ao aluno e pais-responsáveis é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int rar_id { get; set; }

        /// <summary>
        /// Propriedade auxiliar - Tipo de recomendação.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de recomendação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override short rar_tipo { get; set; }
    }
}