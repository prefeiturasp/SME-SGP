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
    public abstract class Abstract_ACA_AlunoHistoricoObservacao : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Int64 alu_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int aho_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte aho_tipo { get; set; }
        public virtual int aho_numeroObs { get; set; }
        [MSNotNullOrEmpty()]
        public virtual string aho_observacao { get; set; }
        public virtual int hop_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte aho_situacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime aho_dataCriacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime aho_dataAlteracao { get; set; }

    }
}