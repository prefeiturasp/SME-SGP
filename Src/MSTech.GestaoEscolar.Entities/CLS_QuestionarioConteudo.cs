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
    public class CLS_QuestionarioConteudo : Abstract_CLS_QuestionarioConteudo
	{
        /// <summary>
		/// Id do questionário do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty()]
        public override int qst_id { get; set; }

        /// <summary>
        /// Id do conteúdo do questionário..
        /// </summary>
        public override int qtc_id { get; set; }

        /// <summary>
        /// Texto do conteúdo..
        /// </summary>
        [MSValidRange(50)]
        [MSNotNullOrEmpty("Texto do conteúdo é obrigatório.")]
        public override string qtc_texto { get; set; }

        /// <summary>
        /// Tipo do conteúdo..
        /// </summary>
        [MSNotNullOrEmpty("Tipo de conteúdo é obrigatório.")]
        public override byte qtc_tipo { get; set; }

        /// <summary>
        /// Tipo de resposta para o tipo de conteúdo Pergunta..
        /// </summary>
        public override byte qtc_tipoResposta { get; set; }

        /// <summary>
        /// Data de criação do registro..
        /// </summary>
        public override DateTime qtc_dataCriacao { get; set; }

        /// <summary>
        /// Data da última alteração do registro..
        /// </summary>
        public override DateTime qtc_dataAlteracao { get; set; }

        /// <summary>
        /// Situação do registro..
        /// </summary>
        [MSDefaultValue(1)]
        [MSNotNullOrEmpty()]
        public override int qtc_situacao { get; set; }
    }
}