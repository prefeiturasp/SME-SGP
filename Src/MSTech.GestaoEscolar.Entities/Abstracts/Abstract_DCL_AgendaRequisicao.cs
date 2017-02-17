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
    public abstract class Abstract_DCL_AgendaRequisicao : Abstract_Entity
    {
        /// <summary>
        /// Id da Entidade
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid ent_id { get; set; }

        /// <summary>
        /// Id da Requisição
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int req_id { get; set; }

        /// <summary>
        /// Periodicidade da Agenda (número em dias)
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int age_periodicidade { get; set; }

        /// <summary>
        /// Situação do registro:
        ///- 1: Ativo
        ///- 3: Excluído
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int age_situacao { get; set; }

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime age_dataCriacao { get; set; }

        /// <summary>
        /// Data/hora da última alteração
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime age_dataAlteracao { get; set; }
    }
}
