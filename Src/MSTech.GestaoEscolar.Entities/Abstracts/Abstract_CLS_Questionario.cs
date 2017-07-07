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
    public abstract class Abstract_CLS_Questionario : Abstract_Entity
    {
		
		/// <summary>
		/// Id do questionário..
		/// </summary>
		[MSNotNullOrEmpty("[qst_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int qst_id { get; set; }

		/// <summary>
		/// Título do questionário..
		/// </summary>
		[MSValidRange(500)]
		[MSNotNullOrEmpty("[qst_titulo] é obrigatório.")]
		public virtual string qst_titulo { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qst_dataCriacao] é obrigatório.")]
		public virtual DateTime qst_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qst_dataAlteracao] é obrigatório.")]
		public virtual DateTime qst_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[qst_situacao] é obrigatório.")]
		public virtual int qst_situacao { get; set; }

		/// <summary>
		/// Tipo do cálculo para questionários que compoem testes. (1- Sem cálculo; 2 - Soma).
		/// </summary>
		[MSNotNullOrEmpty("[qst_tipoCalculo] é obrigatório.")]
		public virtual byte qst_tipoCalculo { get; set; }

		/// <summary>
		/// Título do cálculo para questionários que compoem testes..
		/// </summary>
		[MSValidRange(500)]
		public virtual string qst_tituloCalculo { get; set; }

    }
}