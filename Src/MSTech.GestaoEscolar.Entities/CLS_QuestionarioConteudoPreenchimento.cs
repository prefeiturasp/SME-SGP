/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;    /// <summary>
                         /// Description: .
                         /// </summary>
    [Serializable]
	public class CLS_QuestionarioConteudoPreenchimento : Abstract_CLS_QuestionarioConteudoPreenchimento
	{
        /// <summary>
		/// Texto da resposta do tipo Texto aberto..
		/// </summary>
		[MSValidRange(4000, "Resposta deve possuir até 4000 caracteres")]
        [MSNotNullOrEmpty("Resposta é obrigatório.")]
        public override string qcp_textoResposta { get; set; }
    }
}