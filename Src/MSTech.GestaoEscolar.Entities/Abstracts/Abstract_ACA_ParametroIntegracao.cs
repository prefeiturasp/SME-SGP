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
    public abstract class Abstract_ACA_ParametroIntegracao : Abstract_Entity
    {

		/// <summary>
		/// ID do parâmetro de integração.
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int pri_id { get; set; }

		/// <summary>
		/// Chave do parâmetro de integração.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string pri_chave { get; set; }

		/// <summary>
		/// Valor do parâmetro de integração.
		/// </summary>
		[MSValidRange(1000)]
		[MSNotNullOrEmpty()]
		public virtual string pri_valor { get; set; }

		/// <summary>
		/// Descrição do parâmetro de integração.
		/// </summary>
		[MSValidRange(200)]
		public virtual string pri_descricao { get; set; }

		/// <summary>
        /// Situação do parâmetro de integração: 1-Ativo, 3-Excluído, 4-Interno.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pri_situacao { get; set; }

		/// <summary>
		/// Data de criação do parâmetro de integração.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pri_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do parâmetro de integração.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pri_dataAlteracao { get; set; }

    }
}