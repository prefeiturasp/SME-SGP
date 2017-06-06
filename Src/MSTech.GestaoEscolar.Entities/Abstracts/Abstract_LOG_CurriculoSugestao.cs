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
    public abstract class Abstract_LOG_CurriculoSugestao : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela LOG_CurriculoSugestao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int lcs_id { get; set; }

		/// <summary>
		/// ID data tabela ACA_TipoNivelEnsino.
		/// </summary>
		public virtual int tne_id { get; set; }

		/// <summary>
		/// ID data tabela ACA_TipoModalidadeEnsino.
		/// </summary>
		public virtual int tme_id { get; set; }

		/// <summary>
		/// ID do usuário que acessou o currículo.
		/// </summary>
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Data do acesso.
		/// </summary>
		public virtual DateTime lcs_data { get; set; }

        /// <summary>
		/// Ano do calendário.
		/// </summary>
		public virtual int cal_ano { get; set; }
    }
}