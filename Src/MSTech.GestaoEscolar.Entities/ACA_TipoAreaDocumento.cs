/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class ACA_TipoAreaDocumento : Abstract_ACA_TipoAreaDocumento
    {
        /// <summary>
        /// Id do tipo de area.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int tad_id { get; set; }

        /// <summary>
        /// Nome do tipo de area.
        /// </summary>
        [MSValidRange(200, "Nome pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Nome do tipo de área documento é obrigatório.")]
        public override string tad_nome { get; set; }

        /// <summary>
        /// Situação do tipo de area (1-Ativo/3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte tad_situacao { get; set; }

        /// <summary>
        /// Data de criação do tipo de area.
        /// </summary>		
        public override DateTime tad_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do tipo de area.
        /// </summary>
        public override DateTime tad_dataAlteracao { get; set; }

        /// <summary>
        /// Indica a ordem para exibir o tipo de area.
        /// </summary>
        public override int tad_ordem { get; set; }

        /// <summary>
        /// Permite cadastro pela escola.
        /// </summary>        
        [MSDefaultValue(false)]
        public override bool tad_cadastroEscola { get; set; }
    }
}