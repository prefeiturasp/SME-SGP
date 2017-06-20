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
    public abstract class Abstract_CLS_AlunoDeficienciaDetalhe : Abstract_Entity
    {
		
		/// <summary>
		/// ID do aluno.
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID do tipo de deficiência.
		/// </summary>
		[MSNotNullOrEmpty("[tde_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual Guid tde_id { get; set; }

		/// <summary>
		/// ID do detalhe da deficiência.
		/// </summary>
		[MSNotNullOrEmpty("[dfd_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int dfd_id { get; set; }

    }
}