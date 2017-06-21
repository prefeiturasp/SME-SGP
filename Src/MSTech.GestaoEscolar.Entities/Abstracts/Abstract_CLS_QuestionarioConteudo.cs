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
		[MSNotNullOrEmpty("[qst_id] é obrigatório.")]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Id do conteúdo do questionário..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int qtc_id { get; set; }

		/// <summary>
		/// Texto do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_texto] é obrigatório.")]
		public virtual string qtc_texto { get; set; }

		/// <summary>
		/// Tipo do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_tipo] é obrigatório.")]
		public virtual byte qtc_tipo { get; set; }

		/// <summary>
		/// Tipo de resposta para o tipo de conteúdo Pergunta..
		/// </summary>
		public virtual byte qtc_tipoResposta { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_dataCriacao] é obrigatório.")]
		public virtual DateTime qtc_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_dataAlteracao] é obrigatório.")]
		public virtual DateTime qtc_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_situacao] é obrigatório.")]
		public virtual int qtc_situacao { get; set; }

		/// <summary>
		/// Ordem do conteúdo..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_ordem] é obrigatório.")]
		public virtual int qtc_ordem { get; set; }

    }
}