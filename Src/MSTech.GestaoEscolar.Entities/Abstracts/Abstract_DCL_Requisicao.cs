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
    public abstract class Abstract_DCL_Requisicao : Abstract_Entity
    {
        /// <summary>
        /// Id da Requisição
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int req_id { get; set; }

        /// <summary>
        /// Nome da Requisição
        /// </summary>
        [MSValidRange(50)]
        [MSNotNullOrEmpty()]
        public virtual string req_nome { get; set; }

        /// <summary>
        /// Determina se a Requisição permite agendar horários
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual bool req_permiteAgenda { get; set; }
    }
}
