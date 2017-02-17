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
	/// Ciclos de Aprendizagem
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ACA_TipoCiclo : Abstract_Entity
    {

        /// <summary>
        /// Id do tipo de ciclo
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, true, false)]
        public virtual int tci_id { get; set; }

        /// <summary>
        /// Nome  do tipo de ciclo
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty()]
        public virtual string tci_nome { get; set; }

        /// <summary>
        /// Situação do tipo de ciclo (1-Ativo/3-Excluído)
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte tci_situacao { get; set; }

        /// <summary>
        /// Data de criação do tipo de ciclo
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime tci_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do tipo de ciclo
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime tci_dataAlteracao { get; set; }

        /// <summary>
        /// Exibir compromisso do aluno no boletim.
        /// </summary>
        public virtual bool tci_exibirBoletim { get; set; }

        /// <summary>
        /// Indica a ordem para exibir o tipo do ciclo.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int tci_ordem { get; set; }

        /// <summary>
        /// Propriedade tci_layout.
        /// </summary>
        [MSValidRange(50)]
        public virtual string tci_layout { get; set; }

    }
}