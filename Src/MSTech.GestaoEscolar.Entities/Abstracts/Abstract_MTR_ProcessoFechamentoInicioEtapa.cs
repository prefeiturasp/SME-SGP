/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_MTR_ProcessoFechamentoInicioEtapa : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pfe_id { get; set; }

		/// <summary>
		/// Tipo de etapa (1 - Previsão, 2 - Enturmação, 3 - Remanejamento, 4 - Renovação ,5 - formação de turmas, 6 - Sequenciamento de chamada)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pfe_tipoEtapa { get; set; }

		/// <summary>
		/// Alcance (1 - Toda a rede, 2 - Escola, 3 - Curso, 4 - Período do curso, 5 - Calendário escolar)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pfe_alcance { get; set; }

		/// <summary>
		/// Data início da etapa
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfe_dataInicio { get; set; }

		/// <summary>
		/// Data fim da etapa
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfe_dataFim { get; set; }

		/// <summary>
		/// ID do calendário do ano de fechamento
		/// </summary>
		public virtual int cal_idFechamento { get; set; }

		/// <summary>
		/// ID do calendário do ano de início
		/// </summary>
		public virtual int cal_idInicio { get; set; }

		/// <summary>
		/// ID do curso 
		/// </summary>
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID do currículo
		/// </summary>
		public virtual int crr_id { get; set; }

		/// <summary>
		/// ID do período do curso
		/// </summary>
		public virtual int crp_id { get; set; }

		/// <summary>
		/// ID da escola
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID da unidade
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Situação (1 - Ativo, 3 - Excluído)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pfe_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfe_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfe_dataAlteracao { get; set; }

    }
}