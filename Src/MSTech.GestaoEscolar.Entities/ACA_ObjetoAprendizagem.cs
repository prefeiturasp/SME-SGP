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
    public class ACA_ObjetoAprendizagem : Abstract_ACA_ObjetoAprendizagem
	{
        /// <summary>
		/// Propriedade oap_id.
		/// </summary>
        [DataObjectField(true, true, false)]
        public override int oap_id { get; set; }

        /// <summary>
        /// Propriedade tds_id.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de disciplina é obrigatório.")]
        public override int tds_id { get; set; }

        /// <summary>
        /// Propriedade oap_descricao.
        /// </summary>
        [MSValidRange(150, "Descrição pode conter até 500 caracteres.")]
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string oap_descricao { get; set; }

        /// <summary>
        /// Propriedade oap_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte oap_situacao { get; set; }

        /// <summary>
        /// Propriedade oap_dataCriacao.
        /// </summary>
        public override DateTime oap_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade oap_dataAlteracao.
        /// </summary>
        public override DateTime oap_dataAlteracao { get; set; }
    }
}