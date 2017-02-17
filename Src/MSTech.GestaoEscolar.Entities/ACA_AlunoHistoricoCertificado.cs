/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
	using MSTech.Validation;
	using System;
	using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ACA_AlunoHistoricoCertificado : Abstract_ACA_AlunoHistoricoCertificado
	{
		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public override long alu_id { get; set; }

		/// <summary>
		/// Propriedade alh_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public override int alh_id { get; set; }

		/// <summary>
		/// Propriedade ahc_livro.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public override string ahc_livro { get; set; }

		/// <summary>
		/// Propriedade ahc_folha.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public override string ahc_folha { get; set; }

		/// <summary>
		/// Propriedade ahc_numero.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public override string ahc_numero { get; set; }

		/// <summary>
		/// Propriedade ahc_gdae.
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public override string ahc_gdae { get; set; }

		/// <summary>
		/// Propriedade ahc_dataCriacao.
		/// </summary>
		public override DateTime ahc_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade ahc_dataAlteracao.
		/// </summary>
		public override DateTime ahc_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade ahc_situacao.
		/// </summary>
		[MSDefaultValue(1)]
		public override short ahc_situacao { get; set; }
	}
}