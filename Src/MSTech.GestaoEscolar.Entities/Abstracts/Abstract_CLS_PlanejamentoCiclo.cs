/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
    using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_PlanejamentoCiclo : Abstract_Entity
    {
		
		/// <summary>
		/// ID da esola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade escolar.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do tipo de ciclo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tci_id { get; set; }

		/// <summary>
		/// Ano letivo do plano de ciclo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int plc_anoLetivo { get; set; }

		/// <summary>
		/// Id do plano de ciclo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int plc_id { get; set; }

		/// <summary>
		/// Plano do ciclo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string plc_planoCiclo { get; set; }

		/// <summary>
		/// Id do usuario que criou o plano.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Situacao do registro: 1- Ativo; 2- Bloqueado; 3- Excluído; 4- Inativo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte plc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plc_dataAlteracao { get; set; }

    }
}