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
    public abstract class AbstractMTR_ProcessoFechamentoInicioHistorico : Abstract_Entity
    {
		
		/// <summary>
		/// Id do processo de fechamento/início do ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// Id da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do histórico do processo de fechamento/início do ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int pfh_id { get; set; }

		/// <summary>
		/// Id do curso.
		/// </summary>
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Id do currículo do curso.
		/// </summary>
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Id do período do currículo do curso.
		/// </summary>
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Id do usuário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Operação realizada (1-Executar, 2-Desfazer).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short pfh_operacao { get; set; }

		/// <summary>
		/// Tipo de etapa (1-Previsão, 2-Enturmação, 3-Remanejamento, 4-Renovação, 5-Formação de turmas, 6-Sequenciamento de chamada, 7-Confirmação de fechamento do ano letivo).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short pfh_tipoEtapa { get; set; }

		/// <summary>
		/// Indica se o histórico conclui a etapa do processo de fechamento/início do ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pfh_concluiEtapa { get; set; }

		/// <summary>
		/// Data de realização da etapa do processo de fechamento/início do ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime pfh_data { get; set; }

    }
}