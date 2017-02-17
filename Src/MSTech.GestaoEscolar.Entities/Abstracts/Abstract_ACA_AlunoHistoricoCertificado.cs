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
    public abstract class Abstract_ACA_AlunoHistoricoCertificado : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Propriedade alh_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int alh_id { get; set; }

		/// <summary>
		/// Propriedade ahc_livro.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string ahc_livro { get; set; }

		/// <summary>
		/// Propriedade ahc_folha.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string ahc_folha { get; set; }

		/// <summary>
		/// Propriedade ahc_numero.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string ahc_numero { get; set; }

		/// <summary>
		/// Propriedade ahc_gdae.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string ahc_gdae { get; set; }

		/// <summary>
		/// Propriedade ahc_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ahc_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade ahc_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ahc_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade ahc_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ahc_situacao { get; set; }

    }
}