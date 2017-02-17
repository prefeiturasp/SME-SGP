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
    public abstract class AbstractACA_CurriculoControleSemestralVigencia : Abstract_Entity
    {
		
		/// <summary>
		/// ID Curso.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID Curriculo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// ID CurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// ID CurriculoPeriodoVigencia.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int vig_id { get; set; }

		/// <summary>
		/// Ano de inicio da Vicência.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int vig_anoInicio { get; set; }

		/// <summary>
		/// Ano de fim da vigência.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual int vig_anoFim { get; set; }

		/// <summary>
		/// Situação do registro: 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short vig_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime vig_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime vig_dataAlteracao { get; set; }

    }
}