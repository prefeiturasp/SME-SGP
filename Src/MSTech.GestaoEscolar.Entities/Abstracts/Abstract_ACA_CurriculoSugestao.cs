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
    public abstract class Abstract_ACA_CurriculoSugestao : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_CurriculoSugestao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int crs_id { get; set; }

		/// <summary>
		/// Texto da sugestão.
		/// </summary>
		[MSValidRange(400)]
		public virtual string crs_sugestao { get; set; }

		/// <summary>
		/// Tipo da sugestão: 1-Sugestão; 2-Exclusão; 3-Inclusão.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte crs_tipo { get; set; }

		/// <summary>
		/// ID do usuário que cadastrou a sugestão.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte crs_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crs_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crs_dataAlteracao { get; set; }

    }
}