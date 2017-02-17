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
    public abstract class Abstract_ACA_EscalaAvaliacaoNumerica : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esa_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual decimal ean_menorValor { get; set; }
		[MSNotNullOrEmpty()]
		public virtual decimal ean_maiorValor { get; set; }
		[MSNotNullOrEmpty()]
		public virtual decimal ean_variacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte ean_situacao { get; set; }

    }
}