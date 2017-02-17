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
    public class ACA_TipoJustificativaExclusaoAulas : Abstract_ACA_TipoJustificativaExclusaoAulas
	{
        /// <summary>
        /// ID do tipo de justificativa para exclusão de aulas
        /// </summary>        
        [DataObjectField(true, true, false)]
        public override int tje_id { get; set; }

        /// <summary>
        /// Nome do tipo de justificativa para exclusão de aulas
        /// </summary>
        [MSValidRange(100, "Tipo de justificativa para exclusão de aulas pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de justificativa para exclusão de aulas é obrigatório.")]
        public override string tje_nome { get; set; }

        /// <summary>
        /// Código do tipo de justificativa para exclusão de aulas
        /// </summary>
        [MSValidRange(20, "Código pode conter até 20 caracteres.")]
        public override string tje_codigo { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído, 4-Inativo
        /// </summary>
        [MSDefaultValue(1)]
        public override byte tje_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>        
        public override DateTime tje_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>        
        public override DateTime tje_dataAlteracao { get; set; }
    }
}