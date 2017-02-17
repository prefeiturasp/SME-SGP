/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public abstract class Abstract_ACA_TipoNivelEnsino : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int tne_id { get; set; }

        [MSValidRange(100)]
        [MSNotNullOrEmpty()]
        public virtual string tne_nome { get; set; }

        [MSNotNullOrEmpty()]
        public virtual byte tne_situacao { get; set; }

        [MSNotNullOrEmpty()]
        public virtual DateTime tne_dataCriacao { get; set; }

        [MSNotNullOrEmpty()]
        public virtual DateTime tne_dataAlteracao { get; set; }

        /// <summary>
        /// Ordem do tipo de nivel de ensino.
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int tne_ordem { get; set; }
    }
}