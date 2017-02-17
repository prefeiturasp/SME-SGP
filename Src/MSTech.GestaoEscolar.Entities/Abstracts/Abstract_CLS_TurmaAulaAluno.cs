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
    public abstract class AbstractCLS_TurmaAulaAluno : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade tau_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Propriedade alu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Propriedade mtu_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// Propriedade mtd_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// Propriedade taa_frequencia.
		/// </summary>
		public virtual int taa_frequencia { get; set; }

		/// <summary>
		/// Propriedade taa_anotacao.
		/// </summary>
		public virtual string taa_anotacao { get; set; }

		/// <summary>
		/// Propriedade taa_frequenciaBitMap.
		/// </summary>
		[MSValidRange(50)]
		public virtual string taa_frequenciaBitMap { get; set; }

		/// <summary>
		/// Propriedade taa_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte taa_situacao { get; set; }

		/// <summary>
		/// Propriedade taa_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime taa_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade taa_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime taa_dataAlteracao { get; set; }

        /// <summary>
        /// Id do usuário que realizou a última alteração nos dados.
        /// </summary>
        public virtual Guid usu_idDocenteAlteracao { get; set; }

    }
}