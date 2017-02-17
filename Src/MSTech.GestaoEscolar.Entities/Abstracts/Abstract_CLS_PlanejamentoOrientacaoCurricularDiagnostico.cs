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
    public abstract class Abstract_CLS_PlanejamentoOrientacaoCurricularDiagnostico : Abstract_Entity
    {
		
		/// <summary>
		/// ID da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID da orientacao curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long ocr_id { get; set; }

		/// <summary>
		/// Flag que indica se a orientação curricular foi alcançada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pod_alcancado { get; set; }

		/// <summary>
		/// Posição do docente associado.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tdt_posicao { get; set; }

    }
}