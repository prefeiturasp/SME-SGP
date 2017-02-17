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
    public abstract class Abstract_SYS_Equipamento : Abstract_Entity
    {

        /// <summary>
        /// Id do equipamento
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid equ_id { get; set; }

        /// <summary>
        /// Id da Entidade
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual Guid ent_id { get; set; }

        /// <summary>
        /// Identificador do equipamento (K4, MAC)
        /// </summary>
        [MSValidRange(50)]
        [MSNotNullOrEmpty()]
        public virtual string equ_identificador { get; set; }

        /// <summary>
        /// Descrição do equipamento
        /// </summary>
        [MSValidRange(200)]
        public virtual string equ_descricao { get; set; }

        /// <summary>
        /// Situação do registro:
        /// - 1: Ativo
        ///- 3: Excluido
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte equ_situacao { get; set; }

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime equ_dataCriacao { get; set; }

        /// <summary>
        /// Data/hora da última alteração
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime equ_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade equ_appVersion.
        /// </summary>
        [MSValidRange(50)]
        public virtual string equ_appVersion { get; set; }

        /// <summary>
        /// Propriedade equ_soVersion.
        /// </summary>
        [MSValidRange(50)]
        public virtual string equ_soVersion { get; set; }

        /// <summary>
        /// Propriedade sis_id.
        /// </summary>
        public virtual int sis_id { get; set; }
    }
}
