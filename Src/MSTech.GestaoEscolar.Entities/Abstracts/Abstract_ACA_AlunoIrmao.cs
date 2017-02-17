/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
    [Serializable()]
    public abstract class Abstract_ACA_AlunoIrmao : Abstract_Entity
    {
        [DataObjectField(true, true, false)]
		public virtual Int64 alu_id { get; set; }
		public virtual int ali_id { get; set; }
		public virtual Int64 alu_idIrmao { get; set; }
		public virtual byte ali_situacao { get; set; }

    }
}