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
    public abstract class Abstract_CLS_PlanejamentoProjetoRelacionada : Abstract_Entity
    {
		
		/// <summary>
		/// ID da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID da unidade escolar.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// ID do calendario anual.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// ID do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID do planejamento do projeto.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int plp_id { get; set; }

		/// <summary>
		/// ID do tipo de disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tds_id { get; set; }

    }
}