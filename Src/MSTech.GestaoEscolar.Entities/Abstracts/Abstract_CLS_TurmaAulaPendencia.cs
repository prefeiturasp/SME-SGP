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
    public abstract class Abstract_CLS_TurmaAulaPendencia : Abstract_Entity
    {
		
		/// <summary>
		/// ID TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tud_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID TurmaAula.
		/// </summary>
		[MSNotNullOrEmpty("[tau_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tau_id { get; set; }

		/// <summary>
		/// Indica se a aula está sem plano de aula.
		/// </summary>
		[MSNotNullOrEmpty("[apn_semPlanoAula] é obrigatório.")]
		public virtual bool apn_semPlanoAula { get; set; }

        /// <summary>
		/// Indica se a aula não possui objeto de conhecimento no plano de aula.
		/// </summary>
		[MSNotNullOrEmpty("[apn_semObjetoConhecimento] é obrigatório.")]
        public virtual bool apn_semObjetoConhecimento { get; set; }

        /// <summary>
        /// Data do processamento do serviço.
        /// </summary>
        [MSNotNullOrEmpty("[apn_data] é obrigatório.")]
		public virtual DateTime apn_data { get; set; }

    }
}