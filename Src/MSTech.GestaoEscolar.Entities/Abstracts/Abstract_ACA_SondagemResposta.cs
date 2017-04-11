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
    public abstract class Abstract_ACA_SondagemResposta : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_Sondagem.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int snd_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_SondagemResposta.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int sdr_id { get; set; }

        /// <summary>
        /// Sigla da resposta.
        /// </summary>
        [MSValidRange(20)]
        [MSNotNullOrEmpty]
        public virtual string sdr_sigla { get; set; }

        /// <summary>
        /// Descrição da resposta.
        /// </summary>
        [MSValidRange(250)]
		[MSNotNullOrEmpty]
		public virtual string sdr_descricao { get; set; }

		/// <summary>
		/// Ordem da resposta.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int sdr_ordem { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte sdr_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		public virtual DateTime sdr_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime sdr_dataAlteracao { get; set; }

    }
}