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
    public abstract class AbstractACA_TipoQualidadeAluno : Abstract_Entity
    {

        /// <summary>
        /// Propriedade tqa_id.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public virtual int tqa_id { get; set; }

        /// <summary>
        /// Propriedade tqa_descricao.
        /// </summary>
        [MSValidRange(200)]
        [MSNotNullOrEmpty]
        public virtual string tqa_descricao { get; set; }

        /// <summary>
        /// Propriedade tqa_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual short tqa_situacao { get; set; }

        /// <summary>
        /// Propriedade tqa_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tqa_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade tqa_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tqa_dataAlteracao { get; set; }

    }
}