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
    public abstract class Abstract_CLS_QuestionarioResposta : Abstract_Entity
    {
		
		/// <summary>
		/// Id da resposta..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int qtr_id { get; set; }

		/// <summary>
		/// Id do conteúdo da resposta..
		/// </summary>
		[MSNotNullOrEmpty("[qtc_id] é obrigatório.")]
		public virtual int qtc_id { get; set; }

		/// <summary>
		/// Texto da resposta..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_texto] é obrigatório.")]
		public virtual string qtr_texto { get; set; }

		/// <summary>
		/// Flag permite adicionar texto..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_permiteAdicionarTexto] é obrigatório.")]
		public virtual bool qtr_permiteAdicionarTexto { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_dataCriacao] é obrigatório.")]
		public virtual DateTime qtr_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_dataAlteracao] é obrigatório.")]
		public virtual DateTime qtr_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_situacao] é obrigatório.")]
		public virtual int qtr_situacao { get; set; }

		/// <summary>
		/// Ordem da resposta..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_ordem] é obrigatório.")]
		public virtual int qtr_ordem { get; set; }

		/// <summary>
		/// Peso da resposta para respostas que compoem testes..
		/// </summary>
		public virtual int qtr_peso { get; set; }

    }
}