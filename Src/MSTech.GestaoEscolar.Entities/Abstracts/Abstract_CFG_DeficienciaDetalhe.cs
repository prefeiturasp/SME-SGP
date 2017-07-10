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
    public abstract class Abstract_CFG_DeficienciaDetalhe : Abstract_Entity
    {
		
		/// <summary>
		/// ID da deficiencia no Core - Pes_TipoDeficiencia.
		/// </summary>
		[MSNotNullOrEmpty("[tde_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid tde_id { get; set; }

		/// <summary>
		/// ID do detalhe, gerado automaticamente.
		/// </summary>
		[MSNotNullOrEmpty("[dfd_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int dfd_id { get; set; }

		/// <summary>
		/// Nome do detalhamento da deficiência.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty("[dfd_nome] é obrigatório.")]
		public virtual string dfd_nome { get; set; }

		/// <summary>
		/// Situação do detalhe - padrão é sem 1.
		/// </summary>
		[MSNotNullOrEmpty("[dfd_situacao] é obrigatório.")]
		public virtual byte dfd_situacao { get; set; }

		/// <summary>
		/// Data de criação .
		/// </summary>
		[MSNotNullOrEmpty("[dfd_dataCriacao] é obrigatório.")]
		public virtual DateTime dfd_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração.
		/// </summary>
		[MSNotNullOrEmpty("[dfd_dataAlteracao] é obrigatório.")]
		public virtual DateTime dfd_dataAlteracao { get; set; }

    }
}