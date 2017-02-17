/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ACA_AlunoCurriculoAvaliacao : Abstract_ACA_AlunoCurriculoAvaliacao
    {
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }
        
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int alc_id { get; set; }

        [DataObjectField(true, false, false)]
        public override int ala_id { get; set; }

        [MSNotNullOrEmpty("Turma é obrigatório.")]
        public override Int64 tur_id { get; set; }

        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int cur_id { get; set; }

        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int crr_id { get; set; }

        [MSNotNullOrEmpty("Período do curso é obrigatório.")]
        public override int crp_id { get; set; }

        [MSNotNullOrEmpty("Avaliação é obrigatório.")]
        public override int tca_id { get; set; }

        /// <summary>
        /// Indica se o aluno foi/será avaliado.
        /// </summary>
        [MSDefaultValue(1)]
        public override bool ala_avaliado { get; set; }
        
        [MSDefaultValue(1)]
        public override byte ala_situacao { get; set; }

        public override DateTime ala_dataCriacao { get; set; }
        public override DateTime ala_dataAlteracao { get; set; }
    }
}