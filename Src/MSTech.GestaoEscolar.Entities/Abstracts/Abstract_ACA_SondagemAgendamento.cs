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
    public abstract class Abstract_ACA_SondagemAgendamento : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Sondagem.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int snd_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_SondagemAgendamento.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int sda_id { get; set; }

		/// <summary>
		/// Data início do agendamento.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sda_dataInicio { get; set; }

		/// <summary>
		/// Data fim do agendamento.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sda_dataFim { get; set; }

        /// <summary>
        /// ID do agendamento retificado.
        /// </summary>
        public virtual int sda_idRetificada { get; set; }

        /// <summary>
        /// ID da tabela ESC_Escola.
        /// </summary>
        public virtual int esc_id { get; set; }

        /// <summary>
        /// ID da tabela ESC_UnidadeEscola.
        /// </summary>
        public virtual int uni_id { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluido).
        /// </summary>
        [MSNotNullOrEmpty]
		public virtual byte sda_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sda_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sda_dataAlteracao { get; set; }

    }
}