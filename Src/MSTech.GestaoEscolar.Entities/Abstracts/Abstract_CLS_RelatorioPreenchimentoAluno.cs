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
    public abstract class Abstract_CLS_RelatorioPreenchimentoAluno : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela CLS_RelatorioPreenchimento, referente ao preenchimento de um relatório..
		/// </summary>
		[MSNotNullOrEmpty("[reap_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long reap_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_Aluno..
		/// </summary>
		[MSNotNullOrEmpty("[alu_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Ano de preenchimento do relatório..
		/// </summary>
		[MSNotNullOrEmpty("[cal_ano] é obrigatório.")]
		public virtual int cal_ano { get; set; }

		/// <summary>
		/// ID do tipo ACA_TipoPeriodoCalendario..
		/// </summary>
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[pal_dataCriacao] é obrigatório.")]
		public virtual DateTime pal_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[pal_dataAlteracao] é obrigatório.")]
		public virtual DateTime pal_dataAlteracao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído)..
		/// </summary>
		[MSNotNullOrEmpty("[pal_situacao] é obrigatório.")]
		public virtual byte pal_situacao { get; set; }

    }
}