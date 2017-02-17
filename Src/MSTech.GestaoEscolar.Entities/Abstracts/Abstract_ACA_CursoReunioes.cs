/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using MSTech.Data.Common.Abstracts;
    using MSTech.Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public abstract class Abstract_ACA_CursoReunioes : Abstract_Entity
    {

        /// <summary>
        /// ID do curso
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int cur_id { get; set; }

        /// <summary>
        /// ID do currículo do curso
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int crr_id { get; set; }

        /// <summary>
        /// ID do calendário
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int cal_id { get; set; }

        /// <summary>
        /// ID de curso reunião
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int crn_id { get; set; }

        /// <summary>
        /// ID do périodo do calendário
        /// </summary>
        public virtual int cap_id { get; set; }

        /// <summary>
        /// Quantidade de reuniões por período do calendário
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int crn_qtde { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte crn_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime crn_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime crn_dataAlteracao { get; set; }

    }
}