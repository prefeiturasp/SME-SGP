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
    public abstract class Abstract_ORC_OrientacaoCurricular : Abstract_Entity
    {
		
		/// <summary>
		/// ID da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long ocr_id { get; set; }

		/// <summary>
		/// ID do nível da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int nvl_id { get; set; }

		/// <summary>
		/// ID do tipo de disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// ID da orientação curricular superior.
		/// </summary>
		public virtual long ocr_idSuperior { get; set; }

		/// <summary>
		/// Código da orientação curricular.
		/// </summary>
		[MSValidRange(20)]
		public virtual string ocr_codigo { get; set; }

		/// <summary>
		/// Descrição da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string ocr_descricao { get; set; }

		/// <summary>
		/// Situação da orientação curricular(1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte ocr_situacao { get; set; }

		/// <summary>
		/// Data de criação da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ocr_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração da orientação curricular.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ocr_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade mat_id.
		/// </summary>
		public virtual long mat_id { get; set; }

    }
}