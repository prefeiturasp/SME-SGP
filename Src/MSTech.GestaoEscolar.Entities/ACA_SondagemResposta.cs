/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    [Serializable]
    public class ACA_SondagemResposta : Abstract_ACA_SondagemResposta
	{
        /// <summary>
        /// ID da tabela ACA_SondagemResposta.
        /// </summary>
        public override int sdr_id { get; set; }

        /// <summary>
        /// Sigla da resposta.
        /// </summary>
        [MSValidRange(20, "Sigla da resposta pode conter até 20 caracteres.")]
        [MSNotNullOrEmpty("Sigla da resposta é obrigatória.")]
        public override string sdr_sigla { get; set; }

        /// <summary>
        /// Descrição da resposta.
        /// </summary>
        [MSValidRange(250, "Descrição da resposta pode conter até 250 caracteres.")]
        [MSNotNullOrEmpty("Descrição da resposta é obrigatória.")]
        public override string sdr_descricao { get; set; }

        /// <summary>
        /// Ordem da resposta.
        /// </summary>
        [MSNotNullOrEmpty("Ordem da resposta é obrigatória.")]
        public override int sdr_ordem { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte sdr_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime sdr_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime sdr_dataAlteracao { get; set; }
    }
}