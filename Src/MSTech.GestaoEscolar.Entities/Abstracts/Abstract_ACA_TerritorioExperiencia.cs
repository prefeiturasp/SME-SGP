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
    public abstract class Abstract_ACA_TerritorioExperiencia : Abstract_Entity
    {
		
		/// <summary>
		/// Id da experiência do territorio.
		/// </summary>
		[MSNotNullOrEmpty("[ter_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int ter_id { get; set; }

        /// <summary>
		/// Codigo da experiência do territorio.
		/// </summary>
        public virtual int ter_codigo { get; set; }

        /// <summary>
        /// Nome da experiência do territorio.
        /// </summary>
        [MSValidRange(200)]
		[MSNotNullOrEmpty("[ter_nome] é obrigatório.")]
		public virtual string ter_nome { get; set; }

		/// <summary>
		/// Situacao do registro (1 - Ativo, 3 - Excluido).
		/// </summary>
		[MSNotNullOrEmpty("[ter_situacao] é obrigatório.")]
		public virtual byte ter_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[ter_dataCriacao] é obrigatório.")]
		public virtual DateTime ter_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[ter_dataAlteracao] é obrigatório.")]
		public virtual DateTime ter_dataAlteracao { get; set; }

    }
}