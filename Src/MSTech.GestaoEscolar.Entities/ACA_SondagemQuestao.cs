/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;
    
    [Serializable]
    public class ACA_SondagemQuestao : Abstract_ACA_SondagemQuestao
	{
        /// <summary>
        /// ID da tabela ACA_SondagemQuestao.
        /// </summary>
        public override int sdq_id { get; set; }

        /// <summary>
        /// Descrição da questão.
        /// </summary>
        [MSValidRange(250, "Descrição da questão pode conter até 250 caracteres.")]
        [MSNotNullOrEmpty("Descrição da questão é obrigatória.")]
        public override string sdq_descricao { get; set; }

        /// <summary>
        /// Ordem do registro.
        /// </summary>
        [MSNotNullOrEmpty("Ordem do registro é obrigatória.")]
        public override int sdq_ordem { get; set; }

        /// <summary>
        /// Informa se é uma sub-questão.
        /// </summary>
        public override bool sdq_subQuestao { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte sdq_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime sdq_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime sdq_dataAlteracao { get; set; }
    }
}