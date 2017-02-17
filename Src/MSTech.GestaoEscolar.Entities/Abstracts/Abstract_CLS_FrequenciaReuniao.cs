/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using MSTech.Data.Common.Abstracts;
    using MSTech.Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public abstract class AbstractCLS_FrequenciaReuniao : Abstract_Entity
    {
        /// <summary>
        /// Id da Turma.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long tur_id { get; set; }

        /// <summary>
        /// ID do calendário.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int cal_id { get; set; }

        /// <summary>
        /// ID do período do calendário.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int cap_id { get; set; }

        /// <summary>
        /// ID sequêncial da reunião por período do calendário.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int frp_id { get; set; }

        /// <summary>
        /// Se já foi efetivado.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool frr_efetivado { get; set; }

        /// <summary>
        /// Data de criação.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime frr_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime frr_dataAlteracao { get; set; }
    }
}