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
    public abstract class Abstract_SYS_EquipamentoUnidadeAdministrativa : Abstract_Entity
    {

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid equ_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid uad_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid ent_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime eua_dataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime eua_dataAlteracao { get; set; }

        /// <summary>
        /// 1 - Ativo, 3 Excluido 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte eua_situacao { get; set; }

    }
}
