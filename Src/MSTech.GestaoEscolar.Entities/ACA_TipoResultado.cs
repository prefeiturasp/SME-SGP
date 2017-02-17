/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ACA_TipoResultado : AbstractACA_TipoResultado
	{
        /// <summary>
        /// Propriedade tpr_id.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int tpr_id { get; set; }

        /// <summary>
        /// Propriedade tpr_resultado.
        /// </summary>
        [MSNotNullOrEmpty("Resultado é obrigatório.")]
        public override short tpr_resultado { get; set; }

        /// <summary>
        /// Propriedade tpr_nomenclatura.
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty("Nomenclatura é obrigatório.")]
        public override string tpr_nomenclatura { get; set; }

        /// <summary>
        /// Propriedade tpr_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override short tpr_situacao { get; set; }

        /// <summary>
        /// Propriedade tpr_dataCriacao.
        /// </summary>
        public override DateTime tpr_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade tpr_dataAlteracao.
        /// </summary>
        public override DateTime tpr_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade tpr_tipoLancamento.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de lançamento é obrigatório.")]
        [MSDefaultValue(1)]
        public override short tpr_tipoLancamento { get; set; }
	}
}