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
    public abstract class Abstract_ACA_TipoResponsavelAluno : Abstract_Entity
    {
        /// <summary>
        /// Propriedade tra_id.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public virtual int tra_id { get; set; }

        /// <summary>
        /// Propriedade tra_nome.
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty]
        public virtual string tra_nome { get; set; }

        /// <summary>
        /// Tipo de responsável padrão, utlizado pelo sistema Matrícula (Mae = 1,Pai = 2,Familiar = 3,Tutor = 4,Instituicao = 5,Proprio = 6,Outro = 7).
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte tra_tipoResponsavelPadrao { get; set; }

        /// <summary>
        /// Propriedade tra_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte tra_situacao { get; set; }

        /// <summary>
        /// Propriedade tra_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tra_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade tra_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tra_dataAlteracao { get; set; }

    }
}