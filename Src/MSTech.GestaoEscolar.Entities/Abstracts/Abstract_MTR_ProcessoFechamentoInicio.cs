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
    public abstract class Abstract_MTR_ProcessoFechamentoInicio : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int pfi_anoFechamento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int pfi_anoInicio { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataInicioPrevisaoSeries { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataFimPrevisaoSeries { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataInicioParametroRemanejamento { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataFimParametroRemanejamento { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataInicioEnturmacao { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataFimEnturmacao { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataInicioRemanejamento { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataFimRemanejamento { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataInicioRenovacao { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[MSNotNullOrEmpty()]
        //public virtual DateTime pfi_dataFimRenovacao { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual DateTime pfi_dataInicioFormacaoTurmas { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual DateTime pfi_dataFimFormacaoTurmas { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual DateTime pfi_dataInicioSequenciamentoChamada { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public virtual DateTime pfi_dataFimSequenciamentoChamada { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime pfi_dataReferencia { get; set; }

		/// <summary>
		/// Indica se remanejamento apenas para etapas de ensino e grupamentos não atendidos pela escola
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool pfi_remanejamentoNaoAtendido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool pfi_anoLetivoCorrente { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pfi_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfi_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pfi_dataAlteracao { get; set; }

    }
}