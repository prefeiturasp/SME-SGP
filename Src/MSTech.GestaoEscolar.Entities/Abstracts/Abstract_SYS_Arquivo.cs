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
    public abstract class Abstract_SYS_Arquivo : Abstract_Entity
    {

		/// <summary>
		/// Id do arquivo
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 arq_id { get; set; }

		/// <summary>
		/// Nome do arquivo
		/// </summary>
		[MSValidRange(256)]
		[MSNotNullOrEmpty()]
		public virtual string arq_nome { get; set; }

		/// <summary>
		/// Tamanho do arquivo em Kb
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Int64 arq_tamanhoKB { get; set; }

		/// <summary>
		/// TypeMime do arquivo
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string arq_typeMime { get; set; }

		/// <summary>
		/// Data - Conteúdo do Arquivo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte[] arq_data { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte arq_situacao { get; set; }

		/// <summary>
		/// Date de criação do arquivo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime arq_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do arquivo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime arq_dataAlteracao { get; set; }

    }
}