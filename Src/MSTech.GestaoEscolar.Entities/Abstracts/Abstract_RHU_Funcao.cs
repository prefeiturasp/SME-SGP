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
    public abstract class Abstract_RHU_Funcao : Abstract_Entity
    {

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int fun_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(20)]
        public virtual string fun_codigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty()]
        public virtual string fun_nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string fun_descricao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(20)]
        public virtual string fun_codIntegracao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSValidRange(100)]
        public virtual string pgs_chave { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte fun_situacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime fun_dataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime fun_dataAlteracao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual Guid ent_id { get; set; }

    }
}