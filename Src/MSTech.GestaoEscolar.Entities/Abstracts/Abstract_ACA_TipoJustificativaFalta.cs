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
    public abstract class Abstract_ACA_TipoJustificativaFalta : Abstract_Entity
    {

		/// <summary>
		/// ID do tipo de justificativa de falta
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tjf_id { get; set; }

		/// <summary>
		/// Nome do tipo de justificativa de falta
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tjf_nome { get; set; }

		/// <summary>
		/// Código do tipo de justificativa de falta
		/// </summary>
		[MSValidRange(20)]
		public virtual string tjf_codigo { get; set; }

		/// <summary>
		/// Indica se o tipo de justificativa de falta abona falta
		/// </summary>
		public virtual bool tjf_abonaFalta { get; set; }

		/// <summary>
		/// Situação do registro: 1-Ativo, 3-Excluído, 4-Inativo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tjf_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tjf_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tjf_dataAlteracao { get; set; }

    }
}