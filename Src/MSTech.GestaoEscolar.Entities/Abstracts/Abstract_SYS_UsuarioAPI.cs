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
    public abstract class Abstract_SYS_UsuarioAPI : Abstract_Entity
    {
		
		/// <summary>
		/// ID do usuário API.
		/// </summary>
		[MSNotNullOrEmpty("[uap_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int uap_id { get; set; }

		/// <summary>
		/// Usuário da API.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty("[uap_usuario] é obrigatório.")]
		public virtual string uap_usuario { get; set; }

		/// <summary>
		/// Senha do usuário da API.
		/// </summary>
		[MSValidRange(256)]
		[MSNotNullOrEmpty("[uap_senha] é obrigatório.")]
		public virtual string uap_senha { get; set; }

		/// <summary>
		/// Situação do registro (1 - Ativo, 3 - Excluído).
		/// </summary>
		[MSNotNullOrEmpty("[uap_situacao] é obrigatório.")]
		public virtual byte uap_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[uap_dataCriacao] é obrigatório.")]
		public virtual DateTime uap_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[uap_dataAlteracao] é obrigatório.")]
		public virtual DateTime uap_dataAlteracao { get; set; }

    }
}