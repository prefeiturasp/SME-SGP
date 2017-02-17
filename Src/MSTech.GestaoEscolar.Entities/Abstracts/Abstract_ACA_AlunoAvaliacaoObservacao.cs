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
    public abstract class AbstractACA_AlunoAvaliacaoObservacao : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Propriedade aao_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int aao_id { get; set; }

		/// <summary>
		/// Propriedade aao_tipo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aao_tipo { get; set; }

		/// <summary>
		/// Propriedade aao_numeroObs.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int aao_numeroObs { get; set; }

		/// <summary>
		/// Propriedade aao_observacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string aao_observacao { get; set; }

		/// <summary>
		/// Propriedade aao_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aao_situacao { get; set; }

		/// <summary>
		/// Propriedade aao_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aao_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade aao_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aao_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade aao_dataInicial.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aao_dataInicial { get; set; }

		/// <summary>
		/// Propriedade aao_dataFinal.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aao_dataFinal { get; set; }

    }
}