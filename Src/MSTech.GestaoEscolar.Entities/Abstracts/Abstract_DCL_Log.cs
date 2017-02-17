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
    public abstract class AbstractDCL_Log : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade log_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid log_id { get; set; }

		/// <summary>
		/// Propriedade log_dataHora.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime log_dataHora { get; set; }

		/// <summary>
		/// Propriedade usu_id.
		/// </summary>
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Propriedade usu_login.
		/// </summary>
		[MSValidRange(100)]
		public virtual string usu_login { get; set; }

		/// <summary>
		/// Propriedade log_acao.
		/// </summary>
		[MSValidRange(50)]
		public virtual string log_acao { get; set; }

		/// <summary>
		/// Propriedade log_descricao.
		/// </summary>
		public virtual string log_descricao { get; set; }

		/// <summary>
		/// Propriedade log_macAddress.
		/// </summary>
		[MSValidRange(256)]
		public virtual string log_macAddress { get; set; }

		/// <summary>
		/// Propriedade log_ipAddress.
		/// </summary>
		[MSValidRange(100)]
		public virtual string log_ipAddress { get; set; }

		/// <summary>
		/// Propriedade log_appVersion.
		/// </summary>
		[MSValidRange(50)]
		public virtual string log_appVersion { get; set; }

		/// <summary>
		/// Propriedade log_serialNumber.
		/// </summary>
		[MSValidRange(50)]
		public virtual string log_serialNumber { get; set; }

		/// <summary>
		/// Propriedade esc_id.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Propriedade tur_id.
		/// </summary>
		public virtual long tur_id { get; set; }

		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		public virtual long tud_id { get; set; }

        /// <summary>
        /// Propriedade esc_id.
        /// </summary>
        public virtual int sis_id { get; set; }

    }
}