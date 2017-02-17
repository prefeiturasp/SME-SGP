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
    public abstract class Abstract_ACA_ArquivoArea : Abstract_Entity
    {
		
		/// <summary>
		/// Id do arquivo da área.
		/// </summary>
		[MSNotNullOrEmpty("[aar_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int aar_id { get; set; }

		/// <summary>
		/// Descrição do arquivo da área.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[aar_descricao] é obrigatório.")]
		public virtual string aar_descricao { get; set; }

		/// <summary>
		/// Link do arquivo da área.
		/// </summary>
		[MSValidRange(200)]
		public virtual string aar_link { get; set; }

		/// <summary>
		/// ID do arquivo.
		/// </summary>
		public virtual long arq_id { get; set; }

		/// <summary>
		/// ID da escola..
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID da unidade da escola..
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id da unidade administrativa superior.
		/// </summary>
		public virtual Guid uad_idSuperior { get; set; }

		/// <summary>
		/// Id do tipo de área de documento.
		/// </summary>
		[MSNotNullOrEmpty("[tad_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tad_id { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aar_situacao] é obrigatório.")]
		public virtual byte aar_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aar_dataCriacao] é obrigatório.")]
		public virtual DateTime aar_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[aar_dataAlteracao] é obrigatório.")]
		public virtual DateTime aar_dataAlteracao { get; set; }

		/// <summary>
		/// Tipo de documento (1 - Arquivo; 2 - Link).
		/// </summary>
		[MSNotNullOrEmpty("[aar_tipoDocumento] é obrigatório.")]
		public virtual byte aar_tipoDocumento { get; set; }

		/// <summary>
		/// Id do tipo de nível de ensino..
		/// </summary>
		public virtual int tne_id { get; set; }

		/// <summary>
		/// Flag Plano Político Pedagógico.
		/// </summary>
		[MSNotNullOrEmpty("[aar_planoPoliticoPedagogico] é obrigatório.")]
		public virtual bool aar_planoPoliticoPedagogico { get; set; }

    }
}