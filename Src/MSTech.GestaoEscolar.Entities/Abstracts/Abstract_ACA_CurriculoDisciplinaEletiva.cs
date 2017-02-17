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
    public abstract class Abstract_ACA_CurriculoDisciplinaEletiva : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int cur_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int crr_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int crp_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int dis_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int cde_id { get; set; }
        [MSValidRange(200)]
        public virtual string cde_nome { get; set; }

    }
}