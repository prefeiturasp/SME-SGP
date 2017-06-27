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
		/// ID do grafico.
		/// </summary>
		[MSNotNullOrEmpty("[gra_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int gra_id { get; set; }

		/// <summary>
		/// ID da resposta do questionario em CLS_QuestionarioResposta.
		/// </summary>
		[MSNotNullOrEmpty("[qtr_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int qtr_id { get; set; }

    }
}