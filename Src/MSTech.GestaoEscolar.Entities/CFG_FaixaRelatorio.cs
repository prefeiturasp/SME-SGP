/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CFG_FaixaRelatorio : AbstractCFG_FaixaRelatorio
	{
        /// <summary>
        /// Id da faixa por relatório.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int far_id { get; set; }

        /// <summary>
        /// Id do relatório.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int rlt_id { get; set; }

        /// <summary>
        /// Descrição da faixa por relatório.
        /// </summary>
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        public override string far_descricao { get; set; }

        /// <summary>
        /// Início da faixa por relatório.
        /// </summary>
        [MSValidRange(20, "Início da faixa pode conter até 20 caracteres.")]
        public override string far_inicio { get; set; }

        /// <summary>
        /// Fim da faixa por relatório.
        /// </summary>
        [MSValidRange(20, "Fim da faixa pode conter até 20 caracteres.")]
        public override string far_fim { get; set; }

        /// <summary>
        /// Id da escala de avaliação.
        /// </summary>
        public override int esa_id { get; set; }

        /// <summary>
        /// Id da escala de avaliação parecer.
        /// </summary>
        public override int eap_id { get; set; }

        /// <summary>
        /// Cor da faixa.
        /// </summary>
        [MSValidRange(200, "Cor pode conter até 200 caracteres.")]
        public override string far_cor { get; set; }

        /// <summary>
        /// Situação da faixa por relatório: 1-Ativo; 3-Excluído;
        /// </summary>
        [MSDefaultValue(1)]
        public override byte far_situacao { get; set; }

        /// <summary>
        /// Data de criação da faixa por relatório.
        /// </summary>
        public override DateTime far_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração da faixa por relatório.
        /// </summary>
        public override DateTime far_dataAlteracao { get; set; }

        /// <summary>
        /// Descrição do relatório de acordo com o campo rlt_id.
        /// </summary>
        public string NomeRelatorio { get; set; }

	}
}