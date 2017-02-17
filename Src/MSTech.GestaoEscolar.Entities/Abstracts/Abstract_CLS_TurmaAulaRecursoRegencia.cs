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
    public abstract class AbstractCLS_TurmaAulaRecursoRegencia : Abstract_Entity
    {

        /// <summary>
        /// Id disciplina da turma.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long tud_id { get; set; }

        /// <summary>
        /// Id da aula da turma.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int tau_id { get; set; }

        /// <summary>
        /// Id do recurso da turma.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int trr_id { get; set; }

        /// <summary>
        /// Id disciplina componente da regência.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual long tud_idFilho { get; set; }

        /// <summary>
        /// Id do recurso.
        /// </summary>
        public virtual int rsa_id { get; set; }

        /// <summary>
        /// Descrição de outros tipos de recurso.
        /// </summary>
        public virtual string trr_observacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime trr_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime trr_dataAlteracao { get; set; }

    }
}