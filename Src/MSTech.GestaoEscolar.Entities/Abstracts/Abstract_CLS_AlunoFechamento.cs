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
	/// Description: Dados pré-calculados do fechamento..
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_AlunoFechamento : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela TUR_TurmaDisciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoPeriodoCalendario..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurmaDisciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurmaDisciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// ID da tabela MTR_MatriculaTurmaDisciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }

		/// <summary>
		/// Número de faltas..
		/// </summary>
		public virtual int caf_qtFaltas { get; set; }

		/// <summary>
		/// Número de aulas..
		/// </summary>
		public virtual int caf_qtAulas { get; set; }

		/// <summary>
		/// Número de faltas de reposição..
		/// </summary>
		public virtual int caf_qtFaltasReposicao { get; set; }

		/// <summary>
		/// Número de aulas de reposição..
		/// </summary>
		public virtual int caf_qtAulasReposicao { get; set; }

		/// <summary>
		/// Ausências compensadas do aluno..
		/// </summary>
		public virtual int caf_qtAusenciasCompensadas { get; set; }

		/// <summary>
		/// Frequência..
		/// </summary>
		public virtual decimal caf_frequencia { get; set; }

		/// <summary>
		/// Frequência final ajustada do aluno..
		/// </summary>
		public virtual decimal caf_frequenciaFinalAjustada { get; set; }

		/// <summary>
		/// Nota..
		/// </summary>
		[MSValidRange(20)]
		public virtual string caf_avaliacao { get; set; }

		/// <summary>
		/// Indica se os dados foram efetivados no fechamento..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool caf_efetivado { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caf_dataAlteracao { get; set; }

        /// <summary>
		/// Número de faltas externas.
		/// </summary>
		public virtual int caf_qtFaltasExterna { get; set; }

        /// <summary>
        /// Número de aulas externas.
        /// </summary>
        public virtual int caf_qtAulasExterna { get; set; }
    }
}