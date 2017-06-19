/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_QuestionarioRespostaPreenchimento : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela CLS_RelatorioPreenchimento, referente ao preenchimento de um relatório..
		/// </summary>
		[MSNotNullOrEmpty("[reap_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long reap_id { get; set; }

		/// <summary>
		/// ID da tabela CLS_RelatorioAtendimentoQuestionario, porque é possível adicionar várias vezes o mesmo questionário no relatório..
		/// </summary>
		[MSNotNullOrEmpty("[raq_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int raq_id { get; set; }

		/// <summary>
		/// ID da tabela CLS_QuestionarioResposta, apenas para resposta do tipo Múltipla seleção ou Seleção única..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int qtr_id { get; set; }

		/// <summary>
		/// Texto adicional inserido na resposta..
		/// </summary>
		[MSValidRange(500)]
		public virtual string qrp_textoAdicional { get; set; }

    }
}