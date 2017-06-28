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
    public abstract class Abstract_CLS_RelatorioPreenchimentoAcoesRealizadas : Abstract_Entity
    {
		
		/// <summary>
		/// ID da ação realizada.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long rpa_id { get; set; }

		/// <summary>
		/// ID da tabela CLS_RelatorioPreenchimento, referente ao preenchimento relacionado.
		/// </summary>
		[MSNotNullOrEmpty("[reap_id] é obrigatório.")]
		public virtual long reap_id { get; set; }

		/// <summary>
		/// Data de realização da ação.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_data] é obrigatório.")]
		public virtual DateTime rpa_data { get; set; }

		/// <summary>
		/// Flag que indica se a ação realizada aparecerá na impressão.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_impressao] é obrigatório.")]
		public virtual bool rpa_impressao { get; set; }

		/// <summary>
		/// Texto sobre a ação realizada.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_acao] é obrigatório.")]
		public virtual string rpa_acao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty("[rpa_situacao] é obrigatório.")]
		public virtual byte rpa_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_dataCriacao] é obrigatório.")]
		public virtual DateTime rpa_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rpa_dataAlteracao] é obrigatório.")]
		public virtual DateTime rpa_dataAlteracao { get; set; }

    }
}