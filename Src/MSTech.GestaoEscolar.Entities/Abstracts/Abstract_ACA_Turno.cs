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
    public abstract class Abstract_ACA_Turno : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int trn_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual Guid ent_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual int ttn_id { get; set; }
        [MSValidRange(200)]
        [MSNotNullOrEmpty()]
        public virtual string trn_descricao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool trn_padrao { get; set; }
        public virtual byte trn_controleTempo { get; set; }
        public virtual TimeSpan trn_horaInicio { get; set; }
        public virtual TimeSpan trn_horaFim { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte trn_situacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime trn_dataCriacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime trn_dataAlteracao { get; set; }
    }
}