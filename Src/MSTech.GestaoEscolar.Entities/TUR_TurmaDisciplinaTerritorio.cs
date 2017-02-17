/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class TUR_TurmaDisciplinaTerritorio : Abstract_TUR_TurmaDisciplinaTerritorio
	{
        /// <summary>
		/// ID da relação entre experiência e território.
		/// </summary>
        [DataObjectField(true, true, false)]
        public override long tte_id { get; set; }

        /// <summary>
        /// ID da turma disciplina de experiência.
        /// </summary>
        [MSNotNullOrEmpty("Experiência é obrigatório.")]
        public override long tud_idExperiencia { get; set; }

        /// <summary>
        /// ID da turma disciplina do território do saber.
        /// </summary>
        [MSNotNullOrEmpty("Território do saber é obrigatório.")]
        public override long tud_idTerritorio { get; set; }

        /// <summary>
        /// Data de vigência inicial da relação entre experiência e território.
        /// </summary>
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime tte_vigenciaInicio { get; set; }

        /// <summary>
        /// Situacao do registro (1 - Ativo, 3 excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte tte_situacao { get; set; }

        /// <summary>
        /// Propriedade tte_dataCriacao.
        /// </summary>
        public override DateTime tte_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade tte_dataAlteracao.
        /// </summary>
        public override DateTime tte_dataAlteracao { get; set; }


        public virtual string tud_nomeTerritorio { get; set; }
    }
}