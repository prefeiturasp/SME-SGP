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
    public abstract class Abstract_CLS_RelatorioAtendimentoCargo : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relatório de atendimento.
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// ID do cargo.
		/// </summary>
		[MSNotNullOrEmpty("[crg_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int crg_id { get; set; }

		/// <summary>
		/// Permite consultar.
		/// </summary>
		[MSNotNullOrEmpty("[rac_permissaoConsulta] é obrigatório.")]
		public virtual bool rac_permissaoConsulta { get; set; }

		/// <summary>
		/// Permite editar.
		/// </summary>
		[MSNotNullOrEmpty("[rac_permissaoEdicao] é obrigatório.")]
		public virtual bool rac_permissaoEdicao { get; set; }

		/// <summary>
		/// Permite excluir.
		/// </summary>
		[MSNotNullOrEmpty("[rac_permissaoExclusao] é obrigatório.")]
		public virtual bool rac_permissaoExclusao { get; set; }

		/// <summary>
		/// Permite aprovar.
		/// </summary>
		[MSNotNullOrEmpty("[rac_permissaoAprovacao] é obrigatório.")]
		public virtual bool rac_permissaoAprovacao { get; set; }

    }
}