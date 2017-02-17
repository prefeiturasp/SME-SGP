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
    public abstract class Abstract_CFG_ModuloClasse : Abstract_Entity
    {
		
		/// <summary>
		/// Campo ID da tabela SYS_Modulo do Core..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mod_id { get; set; }

		/// <summary>
		/// Campo ID da tabela CFG_ModuloClasse..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mdc_id { get; set; }

		/// <summary>
		/// Classe css que irá informar a imagem do ícone..
		/// </summary>
		[MSValidRange(50)]
		[MSNotNullOrEmpty]
		public virtual string mdc_classe { get; set; }

		/// <summary>
		/// 1-Ativo, 3-Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mdc_situacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mdc_dataAlteracao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mdc_dataCriacao { get; set; }

    }
}