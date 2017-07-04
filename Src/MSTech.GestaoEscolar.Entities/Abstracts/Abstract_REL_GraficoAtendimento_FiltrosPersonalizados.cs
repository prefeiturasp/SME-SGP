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
    public abstract class Abstract_REL_GraficoAtendimento_FiltrosPersonalizados : Abstract_Entity
    {
		
		/// <summary>
		/// ID do grafico..
		/// </summary>
		[MSNotNullOrEmpty("[gra_id] é obrigatório.")]
		public virtual int gra_id { get; set; }

		/// <summary>
		/// ID da resposta do questionario em CLS_QuestionarioResposta.
		/// </summary>
		[MSNotNullOrEmpty("[qtr_id] é obrigatório.")]
		public virtual int qtr_id { get; set; }

		/// <summary>
		/// Id do filtro personalizado..
		/// </summary>
		[MSNotNullOrEmpty("[gfp_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int gfp_id { get; set; }

		/// <summary>
		/// Situação do registro (1- Ativo; 3 - Excluído)..
		/// </summary>
		[MSNotNullOrEmpty("[gfp_situacao] é obrigatório.")]
		public virtual int gfp_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[gfp_dataCriacao] é obrigatório.")]
		public virtual DateTime gfp_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[gfp_dataAlteracao] é obrigatório.")]
		public virtual DateTime gfp_dataAlteracao { get; set; }

    }
}