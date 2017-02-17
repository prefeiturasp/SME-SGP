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
	public class CLS_AlunoProjeto : Abstract_CLS_AlunoProjeto
	{
        /// <summary>
        /// ID do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// ID AlunoHistoricoProjeto.
        /// </summary>
        [MSNotNullOrEmpty("Projeto do histórico do aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int ahp_id { get; set; }

        /// <summary>
        /// Propriedade apj_id.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int apj_id { get; set; }

        /// <summary>
        /// Nota do aluno no projeto.
        /// </summary>
        [MSValidRange(20, "Avaliação do aluno no projeto deve possuir no máximo 20 caracteres.")]
        public override string apj_avaliacao { get; set; }

        /// <summary>
        /// Situação do registro.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte apj_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime apj_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime apj_dataAlteracao { get; set; }
	}
}