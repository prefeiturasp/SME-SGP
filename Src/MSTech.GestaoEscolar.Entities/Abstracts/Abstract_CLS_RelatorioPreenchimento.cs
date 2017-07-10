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
    public abstract class Abstract_CLS_RelatorioPreenchimento : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela CLS_RelatorioPreenchimento, referente ao preenchimento de um relatório..
		/// </summary>
		[MSNotNullOrEmpty("[reap_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual long reap_id { get; set; }

		/// <summary>
		/// ID da tabela CLS_RelatorioAtendimento, referente ao relatório que foi preenchido..
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		public virtual int rea_id { get; set; }

    }
}