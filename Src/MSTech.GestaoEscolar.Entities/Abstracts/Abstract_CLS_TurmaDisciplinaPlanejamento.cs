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
    public abstract class Abstract_CLS_TurmaDisciplinaPlanejamento : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 tud_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tdp_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string tdp_planejamento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string tdp_diagnostico { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string tdp_avaliacaoTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string tdp_recursos { get; set; }


        /// <summary>
        ///
        /// </summary>
        public virtual string tdp_intervencoesPedagogicas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string tdp_registroIntervencoes { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
        public virtual int crp_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte tdt_posicao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tdp_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tdp_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tdp_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade pro_id.
        /// </summary>
        public virtual Guid pro_id { get; set; }

    }
}