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
	public class ACA_TipoDocente : Abstract_ACA_TipoDocente
	{
        /// <summary>
        /// ID tipo docente (EnumTipoDocente).
        /// </summary>
        [MSNotNullOrEmpty("Tipo de docente é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override byte tdc_id { get; set; }

        /// <summary>
        /// Descrição tipo docente.
        /// </summary>
        [MSValidRange(100, "Descrição do tipo de docente deve possuir no máximo 100 caracteres.")]
        [MSNotNullOrEmpty("Descrição do tipo de docente é obrigatório.")]
        public override string tdc_descricao { get; set; }

        /// <summary>
        /// Nome tipo docente
        /// </summary>
        [MSValidRange(50, "Nome do tipo de docente deve possuir no máximo 50 caracteres.")]
        [MSNotNullOrEmpty("Nome do tipo de docente é obrigatório.")]
        public override string tdc_nome { get; set; }

        /// <summary>
        /// Posicao para tipo de docente.
        /// </summary>
        [MSNotNullOrEmpty("Posição do docente é obrigatório.")]
        public override byte tdc_posicao { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluido.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte tdc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime tdc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime tdc_dataAlteracao { get; set; }
	}
}