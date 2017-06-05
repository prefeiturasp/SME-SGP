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
    public abstract class Abstract_ACA_Sondagem : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Sondagem.
		/// </summary>
		[MSNotNullOrEmpty("[snd_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int snd_id { get; set; }

		/// <summary>
		/// Título da sondagem.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[snd_titulo] é obrigatório.")]
		public virtual string snd_titulo { get; set; }

		/// <summary>
		/// Descrição da sondagem.
		/// </summary>
		[MSValidRange(4000)]
		public virtual string snd_descricao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 2-Bloqueado, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty("[snd_situacao] é obrigatório.")]
		public virtual byte snd_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[snd_dataCriacao] é obrigatório.")]
		public virtual DateTime snd_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[snd_dataAlteracao] é obrigatório.")]
		public virtual DateTime snd_dataAlteracao { get; set; }

		/// <summary>
		/// Enum da opção de resposta (1 = multiseleção; 2 = seleção única).
		/// </summary>
		[MSNotNullOrEmpty("[snd_opcaoResposta] é obrigatório.")]
		public virtual byte snd_opcaoResposta { get; set; }

    }
}