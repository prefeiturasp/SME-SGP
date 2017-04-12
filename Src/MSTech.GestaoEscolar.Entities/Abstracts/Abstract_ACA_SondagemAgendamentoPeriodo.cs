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
    public abstract class Abstract_ACA_SondagemAgendamentoPeriodo : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Sondagem.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int snd_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_SondagemAgendamento.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int sda_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tcp_id { get; set; }

    }
}