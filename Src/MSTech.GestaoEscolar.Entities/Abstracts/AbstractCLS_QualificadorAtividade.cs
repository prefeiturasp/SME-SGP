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
    public abstract class AbstractCLS_QualificadorAtividade : Abstract_Entity
    {
        /// <summary>
        /// ID do qualificador de atividade. Tipo do qualificador de atividade: 1-Atividade diversificada, 2-Instrumento de avaliação, 3-Recuperação da atividade diversificada, 4-Recuperação do instrumento de avaliação, 5-Lição de casa.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public virtual int qat_id { get; set; }

        /// <summary>
        /// Nome do qualificador de atividade.
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty]
        public virtual string qat_nome { get; set; }

        /// <summary>
        /// Sigla do qualificador de atividade.
        /// </summary>
        [MSValidRange(30)]
        public virtual string qat_sigla { get; set; }

        /// <summary>
        /// Cor do qualificador de atividade.
        /// </summary>
        [MSValidRange(10)]
        public virtual string qat_cor { get; set; }

        /// <summary>
        /// Ordem de exibição do qualificador de atividade.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int qat_ordem { get; set; }

        /// <summary>
        /// 1 – Ativo, 3 – Excluído.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte qat_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime qat_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime qat_dataAlteracao { get; set; }

        /// <summary>
        /// .
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool qat_permiteConfigurarQuantidade { get; set; }
    }
}
