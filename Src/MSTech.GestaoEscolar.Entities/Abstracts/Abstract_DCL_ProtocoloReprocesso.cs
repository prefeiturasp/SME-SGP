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
    public abstract class Abstract_DCL_ProtocoloReprocesso : Abstract_Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid pro_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int prp_seq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual string prp_pacote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte prp_status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(200)]
        public virtual string prp_statusObervacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte prp_situacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime prp_dataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime prp_dataAlteracao { get; set; }
    }
}
