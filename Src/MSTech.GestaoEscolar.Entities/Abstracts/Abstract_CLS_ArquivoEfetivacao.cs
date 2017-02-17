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
    public abstract class AbstractCLS_ArquivoEfetivacao : Abstract_Entity
    {
		
		/// <summary>
		/// Id do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long aef_id { get; set; }

		/// <summary>
		/// Id da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do calendário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Id do tipo período currículo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Id da turma.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long tur_id { get; set; }

		/// <summary>
		/// Id do arquivo recebido na importação.
		/// </summary>
		public virtual long arq_id { get; set; }

		/// <summary>
		/// Tipo: 1-Importação, 2-Exportação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aef_tipo { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short aef_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aef_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime aef_dataAlteracao { get; set; }

    }
}