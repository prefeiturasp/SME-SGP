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
    public abstract class Abstract_ACA_ObjetoAprendizagemEixo : Abstract_Entity
    {
		
		/// <summary>
		/// Id do eixo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int oae_id { get; set; }

        /// <summary>
        /// Propriedade tds_id.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int tds_id { get; set; }

        /// <summary>
        /// Propriedade cal_ano.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int cal_ano { get; set; }

        /// <summary>
        /// Descrição do eixo.
        /// </summary>
        [MSValidRange(500)]
		[MSNotNullOrEmpty]
		public virtual string oae_descricao { get; set; }

		/// <summary>
		/// Ordem do eixo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int oae_ordem { get; set; }

		/// <summary>
		/// Id do eixo pai.
		/// </summary>
		public virtual int oae_idPai { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte oae_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime oae_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime oae_dataAlteracao { get; set; }

    }
}