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
    public abstract class Abstract_SYS_Servicos : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela SYS_Servicos.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual short ser_id { get; set; }

		/// <summary>
		/// Nome do serviço.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string ser_nome { get; set; }

		/// <summary>
		/// Nome do procedimento do servico.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string ser_nomeProcedimento { get; set; }

		/// <summary>
		/// Indica se o servico esta ativo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ser_ativo { get; set; }

		/// <summary>
		/// Propriedade ser_dataUltimaExecucao.
		/// </summary>
		public virtual DateTime ser_dataUltimaExecucao { get; set; }

        /// <summary>
		/// Detalhes referente ao serviço.
		/// </summary>
        public virtual string ser_descricao { get; set; }

    }
}