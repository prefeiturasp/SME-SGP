/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class CLS_RelatorioAtendimentoQuestionario : Abstract_CLS_RelatorioAtendimentoQuestionario
    {
        /// <summary>
        /// ID do registro (Relatorio Questionário).
        /// </summary>
        public override int raq_id { get; set; }

        /// <summary>
        /// ID do relatório de atendimento.
        /// </summary>
        [MSNotNullOrEmpty("ID do relatório de atendimento é obrigatório.")]
        public override int rea_id { get; set; }

        /// <summary>
        /// ID do questionário.
        /// </summary>
        [MSNotNullOrEmpty("ID do questionário é obrigatório.")]
        public override int qst_id { get; set; }

        /// <summary>
        /// Ordem do questionário no relatório de atendimento.
        /// </summary>
        [MSNotNullOrEmpty("Ordem do questionário no relatório de atendimento é obrigatório.")]
        public override int raq_ordem { get; set; }

        /// <summary>
        /// Situação do registro (1 - ativo, 3 - excluído).
        /// </summary>
        public override byte raq_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime raq_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime raq_dataAlteracao { get; set; }

        /// <summary>
        /// Variável auxiliar do tírulo do questionário.
        /// </summary>
        public string qst_titulo { get; set; }
    }
}