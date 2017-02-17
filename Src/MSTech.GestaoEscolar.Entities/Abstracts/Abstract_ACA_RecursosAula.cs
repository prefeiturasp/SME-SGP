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
    public abstract class Abstract_ACA_RecursosAula : Abstract_Entity
    {
		[DataObjectField(true, true, false)]
		public virtual int rsa_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual string rsa_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte rsa_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime rsa_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime rsa_dataAlteracao { get; set; }

    }
}