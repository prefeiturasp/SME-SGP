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
	/// 
	/// </summary>
    [Serializable]
	public class ACA_AlunoCurriculo : AbstractACA_AlunoCurriculo
	{
        /// <summary>
        /// ID do currículo do aluno.
        /// </summary>        
        [DataObjectField(true, false, false)]
        public override int alc_id { get; set; }

        /// <summary>
        /// Id da escola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int esc_id { get; set; }

        /// <summary>
        /// Id da unidade da escola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int uni_id { get; set; }

        /// <summary>
        /// Id do curso.
        /// </summary>        
        public override int cur_id { get; set; }

        /// <summary>
        /// Id do currículo do curso.
        /// </summary>        
        public override int crr_id { get; set; }

        /// <summary>
        /// Id do período do curso.
        /// </summary>        
        public override int crp_id { get; set; }

        /// <summary>
        /// Número de matrícula do aluno.
        /// </summary>
        [MSValidRange(50, "Matrícula pode conter até 50 caracteres.")]
        public override string alc_matricula { get; set; }

        /// <summary>
        /// Código INEP do histórico do aluno.
        /// </summary>
        [MSValidRange(20, "Código INEP pode conter até 20 caracteres.")]
        public override string alc_codigoInep { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído, 4-Inativo, 5-Formado, 6-Cancelado, 7- Em matrícula, 8- Excedente, 9-Evadido, 10- Em movimentação.
        /// </summary>    
        [MSDefaultValue(1)]
        public override short alc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>        
        public override DateTime alc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>        
        public override DateTime alc_dataAlteracao { get; set; }        
	}
}