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
		/// Tipo da resposta. (1 - Múltipla seleção; 2 - Seleção única; 3 - Texto aberto).
		/// </summary>
		[MSNotNullOrEmpty("[qtr_tipo] é obrigatório.")]
		public virtual byte qtr_tipo { get; set; }

		/// <summary>
		/// Texto da resposta..
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty("[qtr_texto] é obrigatório.")]
		public virtual string qtr_texto { get; set; }

		/// <summary>
		/// Flag permite adicionar texto..
		/// </summary>
		[MSNotNullOrEmpty("[qtr_permiteAdicionarTexto] é obrigatório.")]
		public virtual bool qtr_permiteAdicionarTexto { get; set; }

    }
}