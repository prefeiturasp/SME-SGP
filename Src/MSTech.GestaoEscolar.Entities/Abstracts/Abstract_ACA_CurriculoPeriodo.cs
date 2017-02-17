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
    public abstract class Abstract_ACA_CurriculoPeriodo : Abstract_Entity
    {

		/// <summary>
		/// ID Curso
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID Curriculo
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// ID CurriculoPeriodo
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// ID Etapa MEC-INEP
		/// </summary>
		public virtual int mep_id { get; set; }

		/// <summary>
		/// Ordem dos curriculoPeriodo
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crp_ordem { get; set; }

		/// <summary>
		/// Descrição do CurriculoPeriodo
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string crp_descricao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crp_idadeIdealAnoInicio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crp_idadeIdealMesInicio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crp_idadeIdealAnoFim { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crp_idadeIdealMesFim { get; set; }

		/// <summary>
		/// 1-Tempo de aula, 2-Horas
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte crp_controleTempo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte crp_qtdeDiasSemana { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte crp_qtdeTemposDia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte crp_qtdeTemposSemana { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte crp_qtdeHorasDia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte crp_qtdeMinutosDia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte crp_qtdeEletivasAlunos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(100)]
		public virtual string crp_ciclo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool crp_turmaAvaliacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(100)]
		public virtual string crp_nomeAvaliacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(260)]
        public virtual string crp_fundoFrente { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte crp_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime crp_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
        public virtual DateTime crp_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade crp_concluiNivelEnsino.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool crp_concluiNivelEnsino { get; set; }

        /// <summary>
        /// Id do tipo de ciclo
        /// </summary>
        public virtual int tci_id { get; set; }

        /// <summary>
        /// Campo id da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        public virtual int tcp_id { get; set; }

    }
}