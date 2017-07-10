/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class CLS_QuestionarioResposta : Abstract_CLS_QuestionarioResposta
	{
        /// <summary>
		/// Id da resposta..
		/// </summary>
        public override int qtr_id { get; set; }

        /// <summary>
        /// Id do conteúdo da resposta..
        /// </summary>
        [MSNotNullOrEmpty()]
        public override int qtc_id { get; set; }

        /// <summary>
        /// Texto da resposta..
        /// </summary>
        [MSNotNullOrEmpty("Texto da resposta é obrigatório.")]
        public override string qtr_texto { get; set; }

        /// <summary>
        /// Flag permite adicionar texto..
        /// </summary>
        [MSNotNullOrEmpty()]
        public override bool qtr_permiteAdicionarTexto { get; set; }

        /// <summary>
        /// Data de criação do registro..
        /// </summary>
        public override DateTime qtr_dataCriacao { get; set; }

        /// <summary>
        /// Data da última alteração do registro..
        /// </summary>
        public override DateTime qtr_dataAlteracao { get; set; }

        /// <summary>
        /// Situação do registro..
        /// </summary>
        /// [MSDefaultValue(1)]
        [MSNotNullOrEmpty()]
        public override int qtr_situacao { get; set; }

        /// <summary>
        /// Ordem da resposta..
        /// </summary>
        public override int qtr_ordem { get; set; }

        /// <summary>
		/// Peso da resposta para respostas que compoem testes..
		/// </summary>
        public override int qtr_peso { get; set; }

    }
}