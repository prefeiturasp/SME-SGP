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
    public abstract class Abstract_ACA_HistoricoObservacaoPadrao : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int hop_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte hop_tipo { get; set; }
        [MSValidRange(100)]
        [MSNotNullOrEmpty()]
        public virtual string hop_nome { get; set; }
        [MSNotNullOrEmpty()]
        public virtual string hop_descricao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte hop_situacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime hop_dataCriacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime hop_dataAlteracao { get; set; }

    }
}