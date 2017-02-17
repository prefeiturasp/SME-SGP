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
    public abstract class Abstract_ACA_MotivoBaixaFrequencia : Abstract_Entity
    {
		
		/// <summary>
		/// id do motivo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mbf_id { get; set; }

		/// <summary>
		/// id do motivo pai.
		/// </summary>
		public virtual int mbf_idPai { get; set; }

		/// <summary>
		/// Sigla do motivo.
		/// </summary>
		[MSValidRange(5)]
		public virtual string mbf_sigla { get; set; }

		/// <summary>
		/// Descrição do motivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string mbf_descricao { get; set; }

		/// <summary>
		/// 1-Area, 2-Item.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mbf_tipo { get; set; }

		/// <summary>
		/// Situação do registro: 1-Ativo, 3-Excluido.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mbf_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mbf_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mbf_dataAlteracao { get; set; }

    }
}