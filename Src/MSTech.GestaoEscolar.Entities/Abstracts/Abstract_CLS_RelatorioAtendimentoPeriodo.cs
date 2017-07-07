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
    public abstract class Abstract_CLS_RelatorioAtendimentoPeriodo : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relatório de atendimento..
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// ID do tipo de período do calendário..
		/// </summary>
		[MSNotNullOrEmpty("[tpc_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

    }
}