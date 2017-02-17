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
    public abstract class Abstract_CFG_ServidorRelatorio : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade ent_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Propriedade srr_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int srr_id { get; set; }

		/// <summary>
		/// Propriedade srr_nome.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string srr_nome { get; set; }

		/// <summary>
		/// Propriedade srr_descricao.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string srr_descricao { get; set; }

		/// <summary>
		/// Propriedade srr_remoteServer.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool srr_remoteServer { get; set; }

		/// <summary>
		/// Propriedade srr_usuario.
		/// </summary>
		[MSValidRange(512)]
		public virtual string srr_usuario { get; set; }

		/// <summary>
		/// Propriedade srr_senha.
		/// </summary>
		[MSValidRange(512)]
		public virtual string srr_senha { get; set; }

		/// <summary>
		/// Propriedade srr_dominio.
		/// </summary>
		[MSValidRange(512)]
		public virtual string srr_dominio { get; set; }

		/// <summary>
		/// Propriedade srr_diretorioRelatorios.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string srr_diretorioRelatorios { get; set; }

		/// <summary>
		/// Propriedade srr_pastaRelatorios.
		/// </summary>
		[MSValidRange(1000)]
		[MSNotNullOrEmpty]
		public virtual string srr_pastaRelatorios { get; set; }

		/// <summary>
		/// Propriedade srr_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short srr_situacao { get; set; }

		/// <summary>
		/// Propriedade srr_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime srr_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade srr_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime srr_dataAlteracao { get; set; }

    }
}