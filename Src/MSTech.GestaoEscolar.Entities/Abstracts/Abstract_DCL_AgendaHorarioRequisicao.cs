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
    public abstract class Abstract_DCL_AgendaHorarioRequisicao : Abstract_Entity
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
        /// 
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int agh_seq { get; set; }

        /// <summary>
        /// Horário início da Agenda
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual TimeSpan agh_horarioInicio { get; set; }

        /// <summary>
        /// Horário final da Agenda
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual TimeSpan agh_horarioFim { get; set; }

        /// <summary>
        /// intervalo de execução (em minutos)
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int agh_intervalo { get; set; }

        /// <summary>
        /// Situação do registro:
        ///- 1: Ativo
        ///- 3: Excluído 
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte agh_situacao { get; set; }

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime agh_dataCriacao { get; set; }

        /// <summary>
        /// Data/hora da última alteração
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime agh_dataAlteracao { get; set; }
    }
}
