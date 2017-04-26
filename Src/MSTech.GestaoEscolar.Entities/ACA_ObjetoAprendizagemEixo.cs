/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;    
    
    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class ACA_ObjetoAprendizagemEixo : Abstract_ACA_ObjetoAprendizagemEixo
	{
        /// <summary>
		/// Id do eixo.
		/// </summary>
        [DataObjectField(true, true, false)]
        public override int oae_id { get; set; }

        /// <summary>
        /// Propriedade tds_id.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de disciplina é obrigatório.")]
        public override int tds_id { get; set; }

        /// <summary>
        /// Propriedade cal_ano.
        /// </summary>
        [MSNotNullOrEmpty("Ano é obrigatório.")]
        public override int cal_ano { get; set; }

        /// <summary>
		/// Descrição do eixo.
		/// </summary>
		[MSValidRange(150, "Descrição pode conter até 500 caracteres.")]
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string oae_descricao { get; set; }

        /// <summary>
		/// Ordem do eixo.
		/// </summary>
		[MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override int oae_ordem { get; set; }

        /// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSDefaultValue(1)]
        public override byte oae_situacao { get; set; }

        /// <summary>
		/// Data de criação do registro.
		/// </summary>
        public override DateTime oae_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime oae_dataAlteracao { get; set; }

        public string oae_situacaoText { get; set; }
    }
}