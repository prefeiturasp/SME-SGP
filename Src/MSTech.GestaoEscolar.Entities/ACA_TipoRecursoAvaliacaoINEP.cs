/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.Validation;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class ACA_TipoRecursoAvaliacaoINEP : AbstractACA_TipoRecursoAvaliacaoINEP
    {
        /// <summary>
        /// Id do tipo de recurso necessário para a realização de uma prova do Inep, esse Id é fixo, sendo utilizado no censo.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int tri_id { get; set; }

        /// <summary>
        /// Nome do tipo de recurso.
        /// </summary>
        [MSValidRange(100, "Nome do tipo de recurso pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do tipo de recurso é obrigatório.")]
        public override string tri_nome { get; set; }

        /// <summary>
        /// Situação. 1 – Ativo; 3 – Excluído..
        /// </summary>
        [MSDefaultValue(1)]
        public override short tri_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime tri_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime tri_dataAlteracao { get; set; }
    }
}