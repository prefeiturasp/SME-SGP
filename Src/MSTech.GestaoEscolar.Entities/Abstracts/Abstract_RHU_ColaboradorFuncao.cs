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
    public abstract class Abstract_RHU_ColaboradorFuncao : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade col_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long col_id { get; set; }

		/// <summary>
		/// Propriedade fun_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int fun_id { get; set; }

		/// <summary>
		/// Propriedade cof_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cof_id { get; set; }

		/// <summary>
		/// Propriedade cof_matricula.
		/// </summary>
		[MSValidRange(30)]
		public virtual string cof_matricula { get; set; }

		/// <summary>
		/// Propriedade cof_observacao.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string cof_observacao { get; set; }

		/// <summary>
		/// Propriedade cof_vigenciaInicio.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cof_vigenciaInicio { get; set; }

		/// <summary>
		/// Propriedade cof_vigenciaFim.
		/// </summary>
		public virtual DateTime cof_vigenciaFim { get; set; }

		/// <summary>
		/// Propriedade ent_id.
		/// </summary>
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Propriedade uad_id.
		/// </summary>
		public virtual Guid uad_id { get; set; }

		/// <summary>
		/// Propriedade cof_responsavelUa.
		/// </summary>
		public virtual bool cof_responsavelUa { get; set; }

		/// <summary>
		/// Propriedade cof_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte cof_situacao { get; set; }

		/// <summary>
		/// Propriedade cof_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cof_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade cof_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cof_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade cof_controladoIntegracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool cof_controladoIntegracao { get; set; }

    }
}