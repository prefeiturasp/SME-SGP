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
    public abstract class Abstract_CLS_TurmaNotaOrientacaoCurricular : Abstract_Entity
    {
        /// <summary>
        /// ID da turma disciplina.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long tud_id { get; set; }

        /// <summary>
        /// ID da Turma Nota.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int tnt_id { get; set; }

        /// <summary>
        /// ID da orientação curricular.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long ocr_id { get; set; }

        /// <summary>
        /// ID da Turma Nota Orientacao Curricular.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int toc_id { get; set; }

        /// <summary>
        /// Se a orientação curricular foi alcançada ou não
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool toc_alcancado { get; set; }

        /// <summary>
        /// Situação do registro: 1 - Ativo; 3 - Excluído.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual short toc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime toc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime toc_dataAlteracao { get; set; }
    }
}