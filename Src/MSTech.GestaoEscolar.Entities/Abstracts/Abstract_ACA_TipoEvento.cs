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
    public abstract class Abstract_ACA_TipoEvento : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int tev_id { get; set; }
        [MSValidRange(200)]
        [MSNotNullOrEmpty()]
        public virtual string tev_nome { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte tev_situacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime tev_dataCriacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime tev_dataAlteracao { get; set; }
        public virtual bool tev_periodoCalendario { get; set; }
        public virtual byte tev_liberacao { get; set; }

    }
}