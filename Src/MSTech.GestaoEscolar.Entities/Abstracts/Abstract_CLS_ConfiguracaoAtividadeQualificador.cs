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
    public abstract class Abstract_CLS_ConfiguracaoAtividadeQualificador : Abstract_Entity
    {
		
		/// <summary>
		/// Id da configuração da atividade.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int caa_id { get; set; }

		/// <summary>
		/// Id do qualificador de atividade.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int qat_id { get; set; }

		/// <summary>
		/// Quantidade de atividade do qualificador.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int caq_quantidade { get; set; }

		/// <summary>
		/// Indica se o qualificador possui recuperação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool caq_possuiRecuperacao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short caq_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caq_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caq_dataAlteracao { get; set; }

    }
}