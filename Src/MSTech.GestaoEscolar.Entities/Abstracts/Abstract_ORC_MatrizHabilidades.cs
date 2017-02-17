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
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_ORC_MatrizHabilidades : Abstract_Entity
    {
		
		/// <summary>
		/// ID da matriz de habilidades.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long mat_id { get; set; }

        /// <summary>
        /// Campo Id da tabela SYS_Entidde do CoreSSO.
        /// </summary>
        public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Nome da matriz de habilidades.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string mat_nome { get; set; }

		/// <summary>
		/// Situação da matriz de habilidades(1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mat_situacao { get; set; }

		/// <summary>
		/// Data de criação da matriz de habilidades.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mat_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração da matriz de habilidades.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mat_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade mat_padrao.
		/// </summary>
		public virtual bool mat_padrao { get; set; }

    }
}