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
    public abstract class Abstract_MTR_ParametroFormacaoTurmaCapacidadeDeficiente : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pft_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pfc_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int pfc_qtdDeficiente { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int pfc_capacidadeComDeficiente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte pfc_situacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime pfc_dataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime pfc_dataAlteracao { get; set; }
    }
}