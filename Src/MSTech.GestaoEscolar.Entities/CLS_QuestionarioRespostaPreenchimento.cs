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
	public class CLS_QuestionarioRespostaPreenchimento : Abstract_CLS_QuestionarioRespostaPreenchimento
	{
        /// <summary>
		/// Texto adicional inserido na resposta..
		/// </summary>
		[MSValidRange(500, "Texto adicional deve possuir até 500 caracteres.")]
        public override string qrp_textoAdicional { get; set; }
    }
}