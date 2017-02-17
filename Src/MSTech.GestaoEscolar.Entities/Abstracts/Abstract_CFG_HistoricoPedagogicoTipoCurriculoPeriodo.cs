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
    public abstract class Abstract_CFG_HistoricoPedagogicoTipoCurriculoPeriodo : Abstract_Entity
    {
		
		/// <summary>
		/// Ano letivo de geração do histórico.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int chp_anoLetivo { get; set; }

		/// <summary>
		/// ID do tipo de currículo período.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tcp_id { get; set; }

		/// <summary>
		/// ID do formato de avaliação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int fav_id { get; set; }

    }
}