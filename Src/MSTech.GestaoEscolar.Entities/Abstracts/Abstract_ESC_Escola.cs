/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ESC_Escola : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Guid uad_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(20)]
		public virtual string esc_codigo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string esc_nome { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(20)]
		public virtual string esc_codigoInep { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int esc_codigoNumeroMatricula { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Guid cid_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int tre_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime esc_funcionamentoInicio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime esc_funcionamentoFim { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(260)]
        public virtual string esc_fundoVerso { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte esc_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime esc_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime esc_dataAlteracao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool esc_controleSistema { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte esc_autorizada { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(200)]
		public virtual string esc_atoCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime esc_dataPublicacaoDiarioOficial { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual Guid uad_idSuperiorGestao { get; set; }

        /// <summary>
		/// Indica se a escola é terceirizada.
		/// </summary>
		[MSNotNullOrEmpty()]
        [MSDefaultValue(false)]
        public virtual bool esc_terceirizada { get; set; }
    }
}