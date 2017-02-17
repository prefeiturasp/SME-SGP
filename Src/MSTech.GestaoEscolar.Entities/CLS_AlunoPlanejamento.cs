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
	public class CLS_AlunoPlanejamento : Abstract_CLS_AlunoPlanejamento
    {
        /// <summary>
        /// ID do aluno.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public virtual long alu_id { get; set; }

        /// <summary>
        /// ID da turma disciplina.
        /// </summary>
        [MSNotNullOrEmpty("Turma disciplina é obrigatória.")]
        [DataObjectField(true, false, false)]
        public virtual long tud_id { get; set; }

        /// <summary>
        /// Id do planejamento do aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int apl_id { get; set; }

        /// <summary>
        /// Planejamento do aluno.
        /// </summary>
        public virtual string apl_planejamento { get; set; }

        /// <summary>
        /// Situação do registro.
        /// </summary>
        [MSDefaultValue(1)]
        public virtual short apl_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public virtual DateTime apl_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public virtual DateTime apl_dataAlteracao { get; set; }
	}
}