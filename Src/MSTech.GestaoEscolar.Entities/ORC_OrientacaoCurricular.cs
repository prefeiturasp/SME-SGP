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
	public class ORC_OrientacaoCurricular : Abstract_ORC_OrientacaoCurricular
	{
        /// <summary>
        /// Propriedade ocr_id.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override long ocr_id { get; set; }

        /// <summary>
        /// Propriedade nvl_id.
        /// </summary>
        [MSNotNullOrEmpty("Nível é obrigatório.")]
        public override int nvl_id { get; set; }

        /// <summary>
        /// Propriedade tds_id.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de [MSG_DISCIPLINA] é obrigatório.")]
        public override int tds_id { get; set; }

        /// <summary>
        /// Propriedade ocr_codigo.
        /// </summary>
        [MSValidRange(20, "Código pode conter até 20 caracteres.")]
        public override string ocr_codigo { get; set; }

        /// <summary>
        /// Propriedade ocr_descricao.
        /// </summary>
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string ocr_descricao { get; set; }

        /// <summary>
        /// Propriedade ocr_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte ocr_situacao { get; set; }

        /// <summary>
        /// Propriedade ocr_dataCriacao.
        /// </summary>
        public override DateTime ocr_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade ocr_dataAlteracao.
        /// </summary>
        public override DateTime ocr_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade que indica se existe habilidades relacionadas
        /// </summary>
        public bool Relacionada { get; set; }
	}
}