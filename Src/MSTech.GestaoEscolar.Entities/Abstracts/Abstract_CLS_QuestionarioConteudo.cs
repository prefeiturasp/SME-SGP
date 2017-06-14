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
    public abstract class Abstract_CLS_QuestionarioConteudo : Abstract_Entity
    {
		
		/// <summary>
		/// Id do questionário do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Id do conteúdo do questionário..
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int qtc_id { get; set; }

		/// <summary>
		/// Texto do conteúdo..
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty()]
		public virtual string qtc_texto { get; set; }

		/// <summary>
		/// Tipo do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte qtc_tipo { get; set; }

		/// <summary>
		/// Tipo de resposta para o tipo de conteúdo Pergunta..
		/// </summary>
		public virtual byte qtc_tipoResposta { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime qtc_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime qtc_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int qtc_situacao { get; set; }

    }
}