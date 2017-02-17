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
	/// Description: Fila para o pré-processamento dos cálculos do fechamento..
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_AlunoFechamentoPendencia : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela TUR_TurmaDisciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoPeriodoCalendario..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Indica se deve processar os cálculos relacionados a frequência..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool afp_frequencia { get; set; }

		/// <summary>
		/// Indica se deve processar os cálculos relacionados a nota..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool afp_nota { get; set; }

		/// <summary>
		/// Indica se os cálculos foram processados..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short afp_processado { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime afp_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime afp_dataAlteracao { get; set; }

    }
}