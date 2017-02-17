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
    public abstract class Abstract_ESC_UnidadeEscola : Abstract_Entity
    {

        /// <summary>
        /// Propriedade esc_id.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int esc_id { get; set; }

        /// <summary>
        /// Propriedade uni_id.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int uni_id { get; set; }

        /// <summary>
        /// Propriedade uni_codigo.
        /// </summary>
        [MSValidRange(20)]
        public virtual string uni_codigo { get; set; }

        /// <summary>
        /// Propriedade uni_descricao.
        /// </summary>
        [MSValidRange(1000)]
        public virtual string uni_descricao { get; set; }

        /// <summary>
        /// Propriedade uni_principal.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool uni_principal { get; set; }

        /// <summary>
        /// Propriedade uni_zona.
        /// </summary>
        public virtual short uni_zona { get; set; }

        /// <summary>
        /// Propriedade uni_funcionamentoInicio.
        /// </summary>
        public virtual DateTime uni_funcionamentoInicio { get; set; }

        /// <summary>
        /// Propriedade uni_funcionamentoFim.
        /// </summary>
        public virtual DateTime uni_funcionamentoFim { get; set; }

        /// <summary>
        /// Propriedade uni_cepsProximos.
        /// </summary>
        public virtual string uni_cepsProximos { get; set; }

        /// <summary>
        /// Propriedade uni_observacao.
        /// </summary>
        public virtual string uni_observacao { get; set; }

        /// <summary>
        /// Propriedade uni_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte uni_situacao { get; set; }

        /// <summary>
        /// Propriedade uni_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime uni_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade uni_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime uni_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade uni_alimentacaoEscolar.
        /// </summary>
        public virtual bool uni_alimentacaoEscolar { get; set; }

        /// <summary>
        /// Propriedade uni_propostaFormacaoAlternancia.
        /// </summary>
        public virtual bool uni_propostaFormacaoAlternancia { get; set; }

    }
}