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
    public abstract class Abstract_ACA_EscalaAvaliacaoParecer : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int esa_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int eap_id { get; set; }
		[MSValidRange(10)]
		[MSNotNullOrEmpty()]
		public virtual string eap_valor { get; set; }
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string eap_descricao { get; set; }
		[MSValidRange(20)]
		public virtual string eap_abreviatura { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int eap_ordem { get; set; }
		public virtual decimal eap_equivalenteInicio { get; set; }
		public virtual decimal eap_equivalenteFim { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte eap_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime eap_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime eap_dataAlteracao { get; set; }

    }
}