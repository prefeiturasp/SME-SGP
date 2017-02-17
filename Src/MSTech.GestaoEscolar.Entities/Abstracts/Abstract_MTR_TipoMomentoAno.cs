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
    public abstract class Abstract_MTR_TipoMomentoAno : Abstract_Entity
    {
        [MSNotNullOrEmpty("Ano é obrigatório")]
		[DataObjectField(true, false, false)]
		public virtual int mom_ano { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mom_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual byte tmm_id { get; set; }
		[MSNotNullOrEmpty("Data inicial é obrigatória")]
		public virtual DateTime tma_dataInicio { get; set; }
        [MSNotNullOrEmpty("Data inicial é obrigatória")]
		public virtual DateTime tma_dataFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tma_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tma_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tma_dataAlteracao { get; set; }

    }
}