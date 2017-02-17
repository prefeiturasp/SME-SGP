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
    [Serializable()]
    public abstract class Abstract_ESC_TipoClassificacaoEscolaCargos : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tcc_id.
		/// </summary>		
		[DataObjectField(true, true, false)]
		public virtual int tcc_id { get; set; }

		/// <summary>
		/// Id do tipo de classificação da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tce_id { get; set; }

		/// <summary>
		/// Id do cargo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crg_id { get; set; }

		/// <summary>
		/// Vigência inicial.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tcc_vigenciaInicial { get; set; }

		/// <summary>
		/// Vigência final.
		/// </summary>
		public virtual DateTime tcc_vigenciaFinal { get; set; }

		/// <summary>
		/// Situação do registro (1 – Ativo, 3 – Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tcc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tcc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tcc_dataAlteracao { get; set; }

    }
}