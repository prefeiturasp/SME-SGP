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
    public abstract class Abstract_ACA_SondagemQuestao : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Sondagem.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int snd_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_SondagemQuestao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int sdq_id { get; set; }

		/// <summary>
		/// Descrição da questão.
		/// </summary>
		[MSValidRange(250)]
		[MSNotNullOrEmpty]
		public virtual string sdq_descricao { get; set; }

		/// <summary>
		/// Ordem do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int sdq_ordem { get; set; }

		/// <summary>
		/// Informa se é uma sub-questão.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool sdq_subQuestao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte sdq_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sdq_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sdq_dataAlteracao { get; set; }

    }
}